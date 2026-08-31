using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager.Tools
{
    public partial class Tool_PrivateCameraEditor : Form
    {
        private const string TargetChunkName = "privateCamera";
        private readonly List<PrivateCameraEntry> entries = new List<PrivateCameraEntry>();
        private PrivateCameraEntry copiedEntry;
        private string filePath;
        private bool updating;
        private bool dirty;
        private PrivateCameraEntry currentEntry;
        private readonly CameraField[] cameraFields;
        private sealed class CameraField
        {
            public string Label;
            public TextBox Control;
            public Func<PrivateCameraEntry, float> Read;
            public Action<PrivateCameraEntry, float> Write;
        }

        private bool HasPendingEntryChanges
        {
            get { return currentEntry != null && cameraFields.Any(field => field.Control.Text != FormatValue(field.Read(currentEntry))); }
        }

        private static string FormatValue(float value) { return value.ToString("R", CultureInfo.InvariantCulture); }
        public Tool_PrivateCameraEditor()
        {
            InitializeComponent();
            cameraFields = new CameraField[]
            {
                new CameraField { Label = "Camera distance", Control = cameraDistanceTextBox, Read = entry => entry.CameraDistance, Write = (entry, value) => entry.CameraDistance = value },
                new CameraField { Label = "Camera speed", Control = cameraSpeedTextBox, Read = entry => entry.CameraSpeed, Write = (entry, value) => entry.CameraSpeed = value },
                new CameraField { Label = "Camera movement", Control = cameraMovementTextBox, Read = entry => entry.CameraMovement, Write = (entry, value) => entry.CameraMovement = value },
                new CameraField { Label = "Unknown 1", Control = unk1TextBox, Read = entry => entry.Unk1, Write = (entry, value) => entry.Unk1 = value },
                new CameraField { Label = "Camera height", Control = cameraHeightTextBox, Read = entry => entry.CameraHeight, Write = (entry, value) => entry.CameraHeight = value },
                new CameraField { Label = "Camera angle", Control = cameraAngleTextBox, Read = entry => entry.CameraAngle, Write = (entry, value) => entry.CameraAngle = value },
                new CameraField { Label = "Camera height 2", Control = cameraHeight2TextBox, Read = entry => entry.CameraHeight2, Write = (entry, value) => entry.CameraHeight2 = value },
                new CameraField { Label = "FOV", Control = fovTextBox, Read = entry => entry.FOV, Write = (entry, value) => entry.FOV = value },
                new CameraField { Label = "Unknown 2", Control = unk2TextBox, Read = entry => entry.Unk2, Write = (entry, value) => entry.Unk2 = value },
                new CameraField { Label = "Camera distance 2", Control = cameraDistance2TextBox, Read = entry => entry.CameraDistance2, Write = (entry, value) => entry.CameraDistance2 = value },
                new CameraField { Label = "FOV 2", Control = fov2TextBox, Read = entry => entry.FOV2, Write = (entry, value) => entry.FOV2 = value },
            };

            RefreshEntries();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime &&
                File.Exists(Main.privateCameraPath))
                RunAction(() => LoadFile(Main.privateCameraPath));
        }

        private void RunAction(Action action)
        {
            try { action(); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Private Camera", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool ConfirmDiscard()
        {
            if (!ConfirmDiscardEntry()) return false;
            if (!dirty) return true;
            DialogResult result = MessageBox.Show(this, "Save changes before continuing?", "Private Camera", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return false;
            if (result == DialogResult.No) return true;
            ShowEntry(); // Discard explicitly approved drafts without committing them.
            return Save(false);
        }

        private bool ConfirmDiscardEntry()
        {
            return !HasPendingEntryChanges || MessageBox.Show(this,
                "Discard the pending entry edits? Choose No to keep editing, then use Save Entry to apply them.",
                "Unsaved entry edits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            RunAction(() =>
            {
                if (!ConfirmDiscard()) return;
                using (var dialog = new OpenFileDialog { Filter = "XFBIN files|*.xfbin", FileName = "privateCamera.bin.xfbin" })
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    LoadFile(dialog.FileName);
                }
            });
        }

        private void LoadFile(string path)
        {
            List<PrivateCameraEntry> loaded;
            using (var backend = new XfbinParserBackend(path))
                loaded = PrivateCameraCodec.Read(GetTargetChunk(backend).BinaryData);
            entries.Clear(); entries.AddRange(loaded);
            filePath = path; dirty = false;
            RefreshEntries();
        }

        private static XfbinBinaryChunkItem GetTargetChunk(XfbinParserBackend backend)
        {
            var binaryChunks = backend.GetBinaryChunks();
            if (binaryChunks.Count != 1 || !string.Equals(binaryChunks[0].ChunkName, TargetChunkName, StringComparison.Ordinal))
                throw new InvalidDataException("Expected exactly one binary chunk named privateCamera.");
            return binaryChunks[0];
        }
        private void SaveMenuItem_Click(object sender, EventArgs e) { RunAction(() => Save(false)); }
        private void SaveAsMenuItem_Click(object sender, EventArgs e) { RunAction(() => Save(true)); }

        private bool Save(bool saveAs)
        {
            if (filePath == null) return false;
            if (HasPendingEntryChanges)
                throw new InvalidOperationException("Press Save Entry to apply the pending entry edits before saving the XFBIN.");
            string output = filePath;
            if (saveAs)
            {
                using (var dialog = new SaveFileDialog { Filter = "XFBIN files|*.xfbin", DefaultExt = "xfbin", FileName = System.IO.Path.GetFileName(filePath) })
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return false;
                    output = dialog.FileName;
                }
            }
            string temporary = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(output), ".private-camera-" + Guid.NewGuid().ToString("N") + ".xfbin");
            byte[] payload = PrivateCameraCodec.Write(entries);
            try
            {
                // Always work on a fresh unpack; failed saves do not mutate the loaded source.
                using (var backend = new XfbinParserBackend(filePath))
                {
                    var chunk = GetTargetChunk(backend);
                    backend.UpsertChunk(TargetChunkName, TargetChunkName, "nuccChunkBinary", chunk.ChunkPath, ".binary",
                        payload, chunk.Version, chunk.VersionAttribute);
                    backend.RepackTo(temporary);
                }
                using (var verify = new XfbinParserBackend(temporary))
                {
                    var saved = GetTargetChunk(verify);
                    if (!saved.BinaryData.SequenceEqual(payload))
                        throw new InvalidDataException("Repacked privateCamera validation failed.");
                }
                if (File.Exists(output)) File.Replace(temporary, output, null);
                else File.Move(temporary, output);
                filePath = output;
                dirty = false; UpdateStatus();
                return true;
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }

        private void RefreshEntries(PrivateCameraEntry selected = null)
        {
            updating = true;
            for (int i = 0; i < entries.Count; i++) entries[i].CharacodeIndex = i + 1;
            entryListBox.Items.Clear();
            entryListBox.Items.AddRange(entries.Cast<object>().ToArray());
            if (selected != null && entryListBox.Items.Contains(selected)) entryListBox.SelectedItem = selected;
            else if (entryListBox.Items.Count > 0) entryListBox.SelectedIndex = 0;
            updating = false;
            ShowEntry();
        }

        private void EntryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating) return;
            if (!ConfirmDiscardEntry())
            {
                updating = true;
                entryListBox.SelectedItem = currentEntry;
                updating = false;
                return;
            }
            ShowEntry();
        }

        private void ShowEntry()
        {
            updating = true;
            currentEntry = entryListBox.SelectedItem as PrivateCameraEntry;
            foreach (var field in cameraFields)
                field.Control.Text = currentEntry == null ? "" : FormatValue(field.Read(currentEntry));
            updating = false;
            UpdateStatus();
        }

        private void CameraValue_TextChanged(object sender, EventArgs e)
        {
            if (!updating) UpdateStatus();
        }

        private void SaveEntryButton_Click(object sender, EventArgs e) { RunAction(SaveEntry); }

        private void SaveEntry()
        {
            if (!HasPendingEntryChanges) return;
            // Validate a copy before changing the committed entry or its label.
            var savedEntry = currentEntry.Clone();
            foreach (var field in cameraFields)
            {
                // Do not rewrite unchanged floats: preserve NaN payloads and signed zero bits.
                if (field.Control.Text == FormatValue(field.Read(currentEntry))) continue;
                float value;
                if (!float.TryParse(field.Control.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) ||
                    float.IsNaN(value) || float.IsInfinity(value))
                    throw new ArgumentException(field.Label + " must be a finite number (use a dot for decimals).");
                field.Write(savedEntry, value);
            }
            int index = entries.IndexOf(currentEntry);
            entries[index] = savedEntry;
            dirty = true;
            RefreshEntries(savedEntry);
        }

        private void AddEntryButton_Click(object sender, EventArgs e) { InsertEntry(new PrivateCameraEntry()); }
        private void PasteEntryButton_Click(object sender, EventArgs e) { if (copiedEntry != null) InsertEntry(copiedEntry.Clone()); }
        private void DuplicateEntryButton_Click(object sender, EventArgs e) { if (currentEntry != null) InsertEntry(currentEntry.Clone()); }
        private void CopyEntryButton_Click(object sender, EventArgs e) { if (currentEntry != null) copiedEntry = currentEntry.Clone(); UpdateStatus(); }
        private void InsertEntry(PrivateCameraEntry entry)
        {
            if (filePath == null || !ConfirmDiscardEntry()) return;
            entries.Add(entry); // Append so existing positional character indexes do not shift.
            dirty = true;
            RefreshEntries(entry);
        }

        private void DeleteEntryButton_Click(object sender, EventArgs e)
        {
            if (currentEntry == null || !ConfirmDiscardEntry()) return;
            int index = entryListBox.SelectedIndex;
            entries.Remove(currentEntry); dirty = true;
            var next = entryListBox.Items.Count > 1 ?
                entryListBox.Items[index + 1 < entryListBox.Items.Count ? index + 1 : index - 1] as PrivateCameraEntry : null;
            RefreshEntries(next);
        }

        private void UpdateStatus()
        {
            Text = "Private Camera" + (dirty ? " *" : "") + (HasPendingEntryChanges ? " (entry edits pending)" : "") +
                (filePath == null ? "" : " - " + System.IO.Path.GetFileName(filePath));
            saveMenuItem.Enabled = saveAsMenuItem.Enabled = addEntryButton.Enabled = filePath != null;
            foreach (var field in cameraFields) field.Control.Enabled = currentEntry != null;
            copyEntryButton.Enabled = duplicateEntryButton.Enabled = deleteEntryButton.Enabled = currentEntry != null;
            saveEntryButton.Enabled = HasPendingEntryChanges;
            pasteEntryButton.Enabled = filePath != null && copiedEntry != null;
        }

        private void Tool_PrivateCameraEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { e.Cancel = !ConfirmDiscard(); }
            catch (Exception ex) { e.Cancel = true; MessageBox.Show(this, ex.Message, "Could not save"); }
        }
    }
}
