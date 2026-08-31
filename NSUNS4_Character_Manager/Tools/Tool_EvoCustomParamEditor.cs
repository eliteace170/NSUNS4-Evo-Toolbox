using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager.Tools
{
    public partial class Tool_EvoCustomParamEditor : Form
    {
        private const string TargetChunkName = "EvoCustomParam";
        private readonly List<EvoCustomParamEntry> entries = new List<EvoCustomParamEntry>();
        private EvoCustomParamEntry copiedEntry;
        private string filePath;
        private bool updating;
        private bool dirty;
        private EvoCustomParamEntry currentEntry;
        private uint draftCostumeFlags;
        private string nameFilter = string.Empty;
        private bool HasPendingEntryChanges
        {
            get { return currentEntry != null && (characterCodeTextBox.Text != currentEntry.CharacterCode || draftCostumeFlags != currentEntry.CostumeFlags); }
        }

        public Tool_EvoCustomParamEditor()
        {
            InitializeComponent();
            RefreshEntries();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime &&
                File.Exists(Main.evoCustomParamPath))
                RunAction(() => LoadFile(Main.evoCustomParamPath));
        }

        private void RunAction(Action action)
        {
            try { action(); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Evo Custom Param", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool ConfirmDiscard()
        {
            if (!ConfirmDiscardEntry()) return false;
            if (!dirty) return true;
            DialogResult result = MessageBox.Show(this, "Save changes before continuing?", "Evo Custom Param", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
                using (var dialog = new OpenFileDialog { Filter = "XFBIN files|*.xfbin", FileName = "EvoCustomParam.xfbin" })
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    LoadFile(dialog.FileName);
                }
            });
        }

        private void LoadFile(string path)
        {
            List<EvoCustomParamEntry> loaded;
            using (var backend = new XfbinParserBackend(path))
                loaded = EvoCustomParamCodec.Read(GetTargetChunk(backend).BinaryData);
            entries.Clear(); entries.AddRange(loaded);
            filePath = path; dirty = false;
            ClearSearch();
            RefreshEntries();
        }

        private static XfbinBinaryChunkItem GetTargetChunk(XfbinParserBackend backend)
        {
            var binaryChunks = backend.GetBinaryChunks();
            if (binaryChunks.Count != 1 || !string.Equals(binaryChunks[0].ChunkName, TargetChunkName, StringComparison.Ordinal))
                throw new InvalidDataException("Expected exactly one binary chunk named EvoCustomParam.");
            return binaryChunks[0];
        }
        private void SaveMenuItem_Click(object sender, EventArgs e) { RunAction(() => Save(false)); }
        private void SaveAsMenuItem_Click(object sender, EventArgs e) { RunAction(() => Save(true)); }

        private void SortByNameButton_Click(object sender, EventArgs e)
        {
            if (filePath == null || HasPendingEntryChanges) return;
            // Stable sorting preserves the relative order of entries with matching names.
            var sorted = entries.OrderBy(entry => entry.CharacterCode, StringComparer.OrdinalIgnoreCase).ToList();
            if (entries.SequenceEqual(sorted)) return;
            var selected = currentEntry;
            entries.Clear();
            entries.AddRange(sorted);
            dirty = true;
            RefreshEntries(selected);
        }

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
            string temporary = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(output), ".evo-custom-" + Guid.NewGuid().ToString("N") + ".xfbin");
            byte[] payload = EvoCustomParamCodec.Write(entries);
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
                        throw new InvalidDataException("Repacked EvoCustomParam validation failed.");
                }
                if (File.Exists(output)) File.Replace(temporary, output, null);
                else File.Move(temporary, output);
                filePath = output;
                dirty = false; UpdateStatus();
                return true;
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }

        private void ClearSearch()
        {
            updating = true;
            nameFilter = string.Empty;
            searchTextBox.Clear();
            updating = false;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (updating) return;
            if (HasPendingEntryChanges)
            {
                updating = true;
                searchTextBox.Text = nameFilter;
                updating = false;
                return;
            }
            nameFilter = searchTextBox.Text;
            RefreshEntries(currentEntry);
        }

        private void RefreshEntries(EvoCustomParamEntry selected = null)
        {
            updating = true;
            entryListBox.Items.Clear();
            entryListBox.Items.AddRange(entries.Where(entry =>
                entry.CharacterCode.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0).Cast<object>().ToArray());
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
            currentEntry = entryListBox.SelectedItem as EvoCustomParamEntry;
            characterCodeTextBox.Text = currentEntry == null ? "" : currentEntry.CharacterCode;
            draftCostumeFlags = currentEntry == null ? 0 : currentEntry.CostumeFlags;
            for (int i = 0; i < costumeFlagsCheckedListBox.Items.Count; i++)
                costumeFlagsCheckedListBox.SetItemChecked(i, (draftCostumeFlags & (1u << i)) != 0);
            updating = false;
            UpdateStatus();
        }

        private void CharacterCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!updating) UpdateStatus();
        }

        private void CostumeFlagsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (updating || currentEntry == null) return;
            uint bit = 1u << e.Index;
            draftCostumeFlags = e.NewValue == CheckState.Checked ? draftCostumeFlags | bit : draftCostumeFlags & ~bit;
            UpdateStatus();
        }

        private void SaveEntryButton_Click(object sender, EventArgs e) { RunAction(SaveEntry); }

        private void SaveEntry()
        {
            if (!HasPendingEntryChanges) return;
            // Validate a copy before changing the committed entry or its label.
            var savedEntry = currentEntry.Clone();
            savedEntry.CharacterCode = characterCodeTextBox.Text;
            savedEntry.CostumeFlags = draftCostumeFlags;
            int index = entries.IndexOf(currentEntry);
            entries[index] = savedEntry;
            dirty = true;
            RefreshEntries(savedEntry);
        }

        private void AddEntryButton_Click(object sender, EventArgs e) { InsertEntry(new EvoCustomParamEntry()); }
        private void PasteEntryButton_Click(object sender, EventArgs e) { if (copiedEntry != null) InsertEntry(copiedEntry.Clone()); }
        private void DuplicateEntryButton_Click(object sender, EventArgs e) { if (currentEntry != null) InsertEntry(currentEntry.Clone()); }
        private void CopyEntryButton_Click(object sender, EventArgs e) { if (currentEntry != null) copiedEntry = currentEntry.Clone(); UpdateStatus(); }
        private void InsertEntry(EvoCustomParamEntry entry)
        {
            if (filePath == null || !ConfirmDiscardEntry()) return;
            int index = currentEntry == null ? entries.Count : entries.IndexOf(currentEntry) + 1;
            entries.Insert(index, entry); dirty = true;
            ClearSearch(); // Keep a new or pasted entry visible even if it does not match the filter.
            RefreshEntries(entry);
        }

        private void DeleteEntryButton_Click(object sender, EventArgs e)
        {
            if (currentEntry == null || !ConfirmDiscardEntry()) return;
            int index = entryListBox.SelectedIndex;
            entries.Remove(currentEntry); dirty = true;
            var next = entryListBox.Items.Count > 1 ?
                entryListBox.Items[index + 1 < entryListBox.Items.Count ? index + 1 : index - 1] as EvoCustomParamEntry : null;
            RefreshEntries(next);
        }

        private void UpdateStatus()
        {
            Text = "Evo Custom Param" + (dirty ? " *" : "") + (HasPendingEntryChanges ? " (entry edits pending)" : "") +
                (filePath == null ? "" : " - " + System.IO.Path.GetFileName(filePath));
            saveMenuItem.Enabled = saveAsMenuItem.Enabled = addEntryButton.Enabled = filePath != null;
            characterCodeTextBox.Enabled = costumeFlagsCheckedListBox.Enabled = copyEntryButton.Enabled = duplicateEntryButton.Enabled = deleteEntryButton.Enabled = currentEntry != null;
            saveEntryButton.Enabled = HasPendingEntryChanges;
            searchTextBox.Enabled = filePath != null && !HasPendingEntryChanges;
            sortByNameButton.Enabled = filePath != null && entries.Count > 1 && !HasPendingEntryChanges;
            pasteEntryButton.Enabled = filePath != null && copiedEntry != null;
        }

        private void Tool_EvoCustomParamEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { e.Cancel = !ConfirmDiscard(); }
            catch (Exception ex) { e.Cancel = true; MessageBox.Show(this, ex.Message, "Could not save"); }
        }
    }
}
