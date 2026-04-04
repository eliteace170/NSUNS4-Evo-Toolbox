using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_CostumeBreakParamEditor : Form
    {
        private const int HeaderSize = 0x14;
        private const int EntrySize = 0x20;
        private readonly CostumeBreakFileState fileState = new CostumeBreakFileState();
        private bool displayHex;

        private sealed class CostumeBreakEntry
        {
            public int CharaCode;
            public uint CostumeIndex;
            public string Path = "";
            public bool AwakeningModelFlag;
            public bool ClonesFlag;
            public uint ClonesCount;
            public uint Unk;
        }

        private sealed class CostumeBreakFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public string ChunkName = "";
            public string ChunkPath = "";
            public uint Version;
            public byte[] ReservedBytes = new byte[8];
            public readonly List<CostumeBreakEntry> Entries = new List<CostumeBreakEntry>();
        }

        public Tool_CostumeBreakParamEditor()
        {
            InitializeComponent();
            clonesCountValue.Maximum = uint.MaxValue;
            unkValue.Maximum = uint.MaxValue;
            ApplyDisplayMode();
            ResetUi();
        }

        private void ResetUi()
        {
            entryListBox.Items.Clear();
            entryListBox.Items.Add("No entries found...");
            entryListBox.SelectedIndex = -1;

            ClearEditorFields();
            SetEditorEnabled(false);
            addEntryButton.Enabled = false;
            duplicateEntryButton.Enabled = false;
            deleteEntryButton.Enabled = false;
            saveSelectedButton.Enabled = false;
            sortButton.Enabled = false;
            displayModeButton.Enabled = false;
        }

        private void ClearFileState()
        {
            fileState.FileOpen = false;
            fileState.FilePath = "";
            fileState.ChunkName = "";
            fileState.ChunkPath = "";
            fileState.Version = 0;
            fileState.ReservedBytes = new byte[8];
            fileState.Entries.Clear();
            ResetUi();
        }

        private void ClearEditorFields()
        {
            charaCodeValue.Value = 0;
            costumeIndexValue.Value = 0;
            pathTextBox.Text = "";
            awakeningModelFlagCheckBox.Checked = false;
            clonesFlagCheckBox.Checked = false;
            clonesCountValue.Value = 0;
            unkValue.Value = 0;
        }

        private void SetEditorEnabled(bool enabled)
        {
            editorPanel.Enabled = enabled;
            UpdateClonesCountEnabled();
        }

        private void ApplyDisplayMode()
        {
            charaCodeValue.Hexadecimal = displayHex;
            costumeIndexValue.Hexadecimal = displayHex;
            clonesCountValue.Hexadecimal = displayHex;
            unkValue.Hexadecimal = displayHex;
            displayModeButton.Text = displayHex ? "Hex" : "Dec";
        }

        private void UpdateClonesCountEnabled()
        {
            clonesCountValue.Enabled = editorPanel.Enabled && clonesFlagCheckBox.Checked;
        }

        private void OpenFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = ".xfbin";
                dialog.Filter = "XFBIN Files (*.xfbin)|*.xfbin";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                LoadFile(dialog.FileName);
            }
        }

        private void LoadFile(string filePath)
        {
            using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
            {
                XfbinBinaryChunkPage page = FindCostumeBreakChunk(backend.GetBinaryChunkPages());
                if (page == null)
                {
                    MessageBox.Show("No costumeBreakParam binary chunk was found in this XFBIN.");
                    return;
                }

                uint version;
                byte[] reservedBytes;
                List<CostumeBreakEntry> entries;
                if (!TryParseChunk(page.BinaryData, out version, out reservedBytes, out entries))
                {
                    MessageBox.Show("Could not parse the costumeBreakParam binary chunk.");
                    return;
                }

                fileState.FileOpen = true;
                fileState.FilePath = filePath;
                fileState.ChunkName = page.ChunkName ?? "";
                fileState.ChunkPath = page.ChunkPath ?? "";
                fileState.Version = version;
                fileState.ReservedBytes = reservedBytes;
                fileState.Entries.Clear();
                fileState.Entries.AddRange(entries);
            }

            bool hasEntries = fileState.FileOpen;
            addEntryButton.Enabled = hasEntries;
            duplicateEntryButton.Enabled = hasEntries;
            deleteEntryButton.Enabled = hasEntries;
            saveSelectedButton.Enabled = hasEntries;
            sortButton.Enabled = hasEntries;
            displayModeButton.Enabled = hasEntries;
            RefreshEntryList();
        }

        private static XfbinBinaryChunkPage FindCostumeBreakChunk(List<XfbinBinaryChunkPage> pages)
        {
            foreach (XfbinBinaryChunkPage page in pages)
            {
                string combined = ((page.ChunkName ?? "") + "|" + (page.ChunkPath ?? "") + "|" + (page.BinaryFileName ?? "")).ToLowerInvariant();
                if (combined.Contains("costumebreakparam"))
                    return page;
            }

            foreach (XfbinBinaryChunkPage page in pages)
            {
                uint version;
                byte[] reservedBytes;
                List<CostumeBreakEntry> entries;
                if (TryParseChunk(page.BinaryData, out version, out reservedBytes, out entries))
                    return page;
            }

            return null;
        }

        private void RefreshEntryList()
        {
            int selectedIndex = entryListBox.SelectedIndex;
            entryListBox.Items.Clear();

            if (fileState.Entries.Count == 0)
            {
                entryListBox.Items.Add("No entries found...");
                entryListBox.SelectedIndex = -1;
                ClearEditorFields();
                SetEditorEnabled(false);
                return;
            }

            foreach (CostumeBreakEntry entry in fileState.Entries)
                entryListBox.Items.Add(BuildEntryLabel(entry));

            entryListBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < fileState.Entries.Count ? selectedIndex : 0;
            SetEditorEnabled(true);
        }

        private string BuildEntryLabel(CostumeBreakEntry entry)
        {
            string path = string.IsNullOrWhiteSpace(entry.Path) ? "(no path)" : entry.Path;
            return "Characode: " + FormatSignedValue(entry.CharaCode) + " | Costume: " + FormatUnsignedValue(entry.CostumeIndex) + " | " + path;
        }

        private string FormatSignedValue(int value)
        {
            if (!displayHex)
                return value.ToString();

            return value.ToString("X");
        }

        private string FormatUnsignedValue(uint value)
        {
            if (!displayHex)
                return value.ToString();

            return value.ToString("X");
        }

        private CostumeBreakEntry GetSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
                return null;
            return fileState.Entries[index];
        }

        private void LoadSelectedEntryToEditor()
        {
            CostumeBreakEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                ClearEditorFields();
                SetEditorEnabled(false);
                return;
            }

            SetEditorEnabled(true);
            charaCodeValue.Value = entry.CharaCode;
            costumeIndexValue.Value = entry.CostumeIndex;
            pathTextBox.Text = entry.Path ?? "";
            awakeningModelFlagCheckBox.Checked = entry.AwakeningModelFlag;
            clonesFlagCheckBox.Checked = entry.ClonesFlag;
            clonesCountValue.Value = entry.ClonesCount;
            unkValue.Value = entry.Unk;
            UpdateClonesCountEnabled();
        }

        private CostumeBreakEntry BuildEntryFromEditor()
        {
            return new CostumeBreakEntry
            {
                CharaCode = (int)charaCodeValue.Value,
                CostumeIndex = (uint)costumeIndexValue.Value,
                Path = pathTextBox.Text ?? "",
                AwakeningModelFlag = awakeningModelFlagCheckBox.Checked,
                ClonesFlag = clonesFlagCheckBox.Checked,
                ClonesCount = (uint)clonesCountValue.Value,
                Unk = (uint)unkValue.Value
            };
        }

        private void UpdateSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            fileState.Entries[index] = BuildEntryFromEditor();
            entryListBox.Items[index] = BuildEntryLabel(fileState.Entries[index]);
        }

        private void AddEntry()
        {
            CostumeBreakEntry entry = BuildEntryFromEditor();
            fileState.Entries.Add(entry);
            RefreshEntryList();
            entryListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DuplicateEntry()
        {
            CostumeBreakEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            CostumeBreakEntry copy = new CostumeBreakEntry
            {
                CharaCode = entry.CharaCode,
                CostumeIndex = entry.CostumeIndex,
                Path = entry.Path,
                AwakeningModelFlag = entry.AwakeningModelFlag,
                ClonesFlag = entry.ClonesFlag,
                ClonesCount = entry.ClonesCount,
                Unk = entry.Unk
            };

            fileState.Entries.Add(copy);
            RefreshEntryList();
            entryListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DeleteEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            fileState.Entries.RemoveAt(index);
            RefreshEntryList();
            if (fileState.Entries.Count > 0)
                entryListBox.SelectedIndex = Math.Min(index, fileState.Entries.Count - 1);
        }

        private void SortEntries()
        {
            if (fileState.Entries.Count == 0)
                return;

            CostumeBreakEntry selectedEntry = GetSelectedEntry();
            fileState.Entries.Sort((left, right) =>
            {
                int charaCompare = left.CharaCode.CompareTo(right.CharaCode);
                if (charaCompare != 0)
                    return charaCompare;

                return left.CostumeIndex.CompareTo(right.CostumeIndex);
            });

            RefreshEntryList();

            if (selectedEntry == null)
                return;

            int index = fileState.Entries.IndexOf(selectedEntry);
            if (index >= 0)
                entryListBox.SelectedIndex = index;
        }

        private void SaveFile(bool saveAs)
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("No file loaded...");
                return;
            }

            string outputPath = fileState.FilePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = ".xfbin";
                    dialog.Filter = "XFBIN Files (*.xfbin)|*.xfbin";
                    dialog.FileName = Path.GetFileName(fileState.FilePath);
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    outputPath = dialog.FileName;
                }
            }

            using (XfbinParserBackend backend = new XfbinParserBackend(fileState.FilePath))
            {
                backend.UpsertBinaryChunk(fileState.ChunkName, fileState.ChunkName, fileState.ChunkPath, BuildChunkData());
                backend.RepackTo(outputPath);
            }

            if (!File.Exists(outputPath))
            {
                MessageBox.Show("XFBIN write failed.");
                return;
            }

            fileState.FilePath = outputPath;
        }

        private static bool TryParseChunk(byte[] bytes, out uint version, out byte[] reservedBytes, out List<CostumeBreakEntry> entries)
        {
            version = 0;
            reservedBytes = new byte[8];
            entries = new List<CostumeBreakEntry>();

            if (bytes == null || bytes.Length < HeaderSize)
                return false;

            uint fileSize = ReadUInt32BE(bytes, 0);
            if (fileSize != bytes.Length - 4 && fileSize != bytes.Length)
                return false;

            version = (uint)BitConverter.ToInt32(bytes, 4);
            uint count = BitConverter.ToUInt32(bytes, 8);
            Array.Copy(bytes, 12, reservedBytes, 0, 8);

            int tableEnd = HeaderSize + ((int)count * EntrySize);
            if (tableEnd > bytes.Length)
                return false;

            for (int i = 0; i < count; i++)
            {
                int entryOffset = HeaderSize + (i * EntrySize);
                ulong pathRelative = BitConverter.ToUInt64(bytes, entryOffset + 8);
                long pathOffsetLong = entryOffset + 8L + (long)pathRelative;
                if (pathOffsetLong > int.MaxValue)
                    return false;

                int pathOffset = (int)pathOffsetLong;
                if (!IsValidStringOffset(bytes, pathOffset, tableEnd))
                    return false;

                entries.Add(new CostumeBreakEntry
                {
                    CharaCode = BitConverter.ToInt32(bytes, entryOffset),
                    CostumeIndex = BitConverter.ToUInt32(bytes, entryOffset + 4),
                    Path = ReadNullTerminatedString(bytes, pathOffset),
                    AwakeningModelFlag = BitConverter.ToUInt32(bytes, entryOffset + 16) != 0,
                    ClonesFlag = BitConverter.ToUInt32(bytes, entryOffset + 20) != 0,
                    ClonesCount = BitConverter.ToUInt32(bytes, entryOffset + 24),
                    Unk = BitConverter.ToUInt32(bytes, entryOffset + 28)
                });
            }

            return true;
        }

        private byte[] BuildChunkData()
        {
            List<byte[]> pathBytes = new List<byte[]>();
            int stringDataOffset = HeaderSize + (fileState.Entries.Count * EntrySize);
            int totalStringBytes = 0;

            foreach (CostumeBreakEntry entry in fileState.Entries)
            {
                byte[] bytes = BuildNullTerminatedString(entry.Path);
                pathBytes.Add(bytes);
                totalStringBytes += bytes.Length;
            }

            byte[] output = new byte[stringDataOffset + totalStringBytes];
            WriteUInt32BE(output, 0, (uint)(output.Length - 4));
            Array.Copy(BitConverter.GetBytes((int)fileState.Version), 0, output, 4, 4);
            Array.Copy(BitConverter.GetBytes((uint)fileState.Entries.Count), 0, output, 8, 4);
            Array.Clear(output, 12, 8);
            if (fileState.ReservedBytes != null)
                Array.Copy(fileState.ReservedBytes, 0, output, 12, Math.Min(8, fileState.ReservedBytes.Length));

            int nextStringOffset = stringDataOffset;
            for (int i = 0; i < fileState.Entries.Count; i++)
            {
                CostumeBreakEntry entry = fileState.Entries[i];
                int entryOffset = HeaderSize + (i * EntrySize);

                Array.Copy(BitConverter.GetBytes(entry.CharaCode), 0, output, entryOffset, 4);
                Array.Copy(BitConverter.GetBytes(entry.CostumeIndex), 0, output, entryOffset + 4, 4);
                WriteUInt64LE(output, entryOffset + 8, (ulong)(nextStringOffset - (entryOffset + 8)));
                Array.Copy(BitConverter.GetBytes(entry.AwakeningModelFlag ? 1U : 0U), 0, output, entryOffset + 16, 4);
                Array.Copy(BitConverter.GetBytes(entry.ClonesFlag ? 1U : 0U), 0, output, entryOffset + 20, 4);
                Array.Copy(BitConverter.GetBytes(entry.ClonesCount), 0, output, entryOffset + 24, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unk), 0, output, entryOffset + 28, 4);

                byte[] currentPathBytes = pathBytes[i];
                Array.Copy(currentPathBytes, 0, output, nextStringOffset, currentPathBytes.Length);
                nextStringOffset += currentPathBytes.Length;
            }

            return output;
        }

        private static bool IsValidStringOffset(byte[] bytes, int offset, int minOffset)
        {
            if (offset < minOffset || offset >= bytes.Length)
                return false;

            for (int i = offset; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                    return true;
            }

            return false;
        }

        private static string ReadNullTerminatedString(byte[] bytes, int offset)
        {
            int length = 0;
            while (offset + length < bytes.Length && bytes[offset + length] != 0)
                length++;
            return Encoding.UTF8.GetString(bytes, offset, length);
        }

        private static byte[] BuildNullTerminatedString(string value)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(value ?? "");
            byte[] output = new byte[stringBytes.Length + 1];
            Array.Copy(stringBytes, output, stringBytes.Length);
            return output;
        }

        private static uint ReadUInt32BE(byte[] bytes, int offset)
        {
            return (uint)((bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3]);
        }

        private static void WriteUInt32BE(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)value;
        }

        private static void WriteUInt64LE(byte[] bytes, int offset, ulong value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, bytes, offset, 8);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearFileState();
        }

        private void entryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedEntryToEditor();
        }

        private void addEntryButton_Click(object sender, EventArgs e)
        {
            AddEntry();
        }

        private void duplicateEntryButton_Click(object sender, EventArgs e)
        {
            DuplicateEntry();
        }

        private void deleteEntryButton_Click(object sender, EventArgs e)
        {
            DeleteEntry();
        }

        private void saveSelectedButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedEntry();
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            SortEntries();
        }

        private void displayModeButton_Click(object sender, EventArgs e)
        {
            displayHex = !displayHex;
            ApplyDisplayMode();

            if (fileState.FileOpen)
                RefreshEntryList();
        }

        private void clonesFlagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateClonesCountEnabled();
        }
    }
}
