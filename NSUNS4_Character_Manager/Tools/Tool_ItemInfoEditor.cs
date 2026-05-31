using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_ItemInfoEditor : Form
    {
        private const int HeaderSize = 0x14;
        private const int EntrySize = 0x78;
        private readonly ItemInfoFileState fileState = new ItemInfoFileState();
        private bool displayHex;

        private sealed class ItemInfoEntry
        {
            public string BattleItemName = "";
            public int Unknown1;
            public int ItemPadding;
            public string BattleIconName1 = "";
            public string BattleIconName2 = "";
            public string BattleIconName3 = "";
            public int Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;
            public string ItemNameComment = "";
            public int Unknown9;
            public int Unknown10;
            public int Unknown11;
            public int Unknown12;
            public string ConditionPrmName = "";
            public int Unknown13;
            public int Unknown14;
            public int Unknown15;
            public int Unknown16;
            public int Unknown17;
            public int Unknown18;
        }

        private sealed class ItemInfoFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public string ChunkName = "";
            public string ChunkPath = "";
            public uint TypeTypeVersion;
            public int Padding;
            public int Padding2;
            public readonly List<ItemInfoEntry> Entries = new List<ItemInfoEntry>();
        }

        public Tool_ItemInfoEditor()
        {
            InitializeComponent();
            SetNumericRanges();
            ApplyDisplayMode();
            ResetUi();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime && File.Exists(Main.itemInfoPath))
                LoadFile(Main.itemInfoPath);
        }

        private void SetNumericRanges()
        {
            SetSignedRange(unknown1Value);
            SetSignedRange(itemPaddingValue);
            SetSignedRange(unknown2Value);
            SetSignedRange(unknown3Value);
            SetSignedRange(unknown4Value);
            SetSignedRange(unknown5Value);
            SetSignedRange(unknown6Value);
            SetSignedRange(unknown7Value);
            SetSignedRange(unknown9Value);
            SetSignedRange(unknown10Value);
            SetSignedRange(unknown11Value);
            SetSignedRange(unknown12Value);
            SetSignedRange(unknown13Value);
            SetSignedRange(unknown14Value);
            SetSignedRange(unknown15Value);
            SetSignedRange(unknown16Value);
            SetSignedRange(unknown17Value);
            SetSignedRange(unknown18Value);
        }

        private static void SetSignedRange(NumericUpDown numeric)
        {
            numeric.Minimum = int.MinValue;
            numeric.Maximum = int.MaxValue;
        }

        private void ResetUi()
        {
            entryListBox.Items.Clear();
            entryListBox.Items.Add("No entries found...");
            entryListBox.SelectedIndex = -1;

            ClearEditorFields();
            SetEditorEnabled(false);
            SetFileControlsEnabled(false);
        }

        private void SetFileControlsEnabled(bool enabled)
        {
            addEntryButton.Enabled = enabled;
            duplicateEntryButton.Enabled = enabled;
            deleteEntryButton.Enabled = enabled;
            saveSelectedButton.Enabled = enabled;
            sortButton.Enabled = enabled;
            displayModeButton.Enabled = enabled;
        }

        private void ClearFileState()
        {
            fileState.FileOpen = false;
            fileState.FilePath = "";
            fileState.ChunkName = "";
            fileState.ChunkPath = "";
            fileState.TypeTypeVersion = 0;
            fileState.Padding = 0;
            fileState.Padding2 = 0;
            fileState.Entries.Clear();
            ResetUi();
        }

        private void ClearEditorFields()
        {
            battleItemNameTextBox.Text = "";
            unknown1Value.Value = 0;
            itemPaddingValue.Value = 0;
            battleIconName1TextBox.Text = "";
            battleIconName2TextBox.Text = "";
            battleIconName3TextBox.Text = "";
            unknown2Value.Value = 0;
            unknown3Value.Value = 0;
            unknown4Value.Value = 0;
            unknown5Value.Value = 0;
            unknown6Value.Value = 0;
            unknown7Value.Value = 0;
            itemNameCommentTextBox.Text = "";
            unknown9Value.Value = 0;
            unknown10Value.Value = 0;
            unknown11Value.Value = 0;
            unknown12Value.Value = 0;
            conditionPrmNameTextBox.Text = "";
            unknown13Value.Value = 0;
            unknown14Value.Value = 0;
            unknown15Value.Value = 0;
            unknown16Value.Value = 0;
            unknown17Value.Value = 0;
            unknown18Value.Value = 0;
        }

        private void SetEditorEnabled(bool enabled)
        {
            editorPanel.Enabled = enabled;
        }

        private void ApplyDisplayMode()
        {
            unknown1Value.Hexadecimal = displayHex;
            itemPaddingValue.Hexadecimal = displayHex;
            unknown2Value.Hexadecimal = displayHex;
            unknown3Value.Hexadecimal = displayHex;
            unknown4Value.Hexadecimal = displayHex;
            unknown5Value.Hexadecimal = displayHex;
            unknown6Value.Hexadecimal = displayHex;
            unknown7Value.Hexadecimal = displayHex;
            unknown9Value.Hexadecimal = displayHex;
            unknown10Value.Hexadecimal = displayHex;
            unknown11Value.Hexadecimal = displayHex;
            unknown12Value.Hexadecimal = displayHex;
            unknown13Value.Hexadecimal = displayHex;
            unknown14Value.Hexadecimal = displayHex;
            unknown15Value.Hexadecimal = displayHex;
            unknown16Value.Hexadecimal = displayHex;
            unknown17Value.Hexadecimal = displayHex;
            unknown18Value.Hexadecimal = displayHex;
            displayModeButton.Text = displayHex ? "Hex" : "Dec";
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
                XfbinBinaryChunkPage page = FindItemInfoChunk(backend.GetBinaryChunkPages());
                if (page == null)
                {
                    MessageBox.Show("No itemInfo binary chunk was found in this XFBIN.");
                    return;
                }

                uint typeTypeVersion;
                int padding;
                int padding2;
                List<ItemInfoEntry> entries;
                if (!TryParseChunk(page.BinaryData, out typeTypeVersion, out padding, out padding2, out entries))
                {
                    MessageBox.Show("Could not parse the itemInfo binary chunk.");
                    return;
                }

                fileState.FileOpen = true;
                fileState.FilePath = filePath;
                fileState.ChunkName = page.ChunkName ?? "";
                fileState.ChunkPath = page.ChunkPath ?? "";
                fileState.TypeTypeVersion = typeTypeVersion;
                fileState.Padding = padding;
                fileState.Padding2 = padding2;
                fileState.Entries.Clear();
                fileState.Entries.AddRange(entries);
            }

            SetFileControlsEnabled(true);
            RefreshEntryList();
        }

        private static XfbinBinaryChunkPage FindItemInfoChunk(List<XfbinBinaryChunkPage> pages)
        {
            foreach (XfbinBinaryChunkPage page in pages)
            {
                string combined = ((page.ChunkName ?? "") + "|" + (page.ChunkPath ?? "") + "|" + (page.BinaryFileName ?? "")).ToLowerInvariant();
                if (combined.Contains("iteminfo"))
                    return page;
            }

            foreach (XfbinBinaryChunkPage page in pages)
            {
                uint version;
                int padding;
                int padding2;
                List<ItemInfoEntry> entries;
                if (TryParseChunk(page.BinaryData, out version, out padding, out padding2, out entries))
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

            foreach (ItemInfoEntry entry in fileState.Entries)
                entryListBox.Items.Add(BuildEntryLabel(entry));

            entryListBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < fileState.Entries.Count ? selectedIndex : 0;
            SetEditorEnabled(true);
        }

        private string BuildEntryLabel(ItemInfoEntry entry)
        {
            string battleName = string.IsNullOrWhiteSpace(entry.BattleItemName) ? "(no battle item)" : entry.BattleItemName;
            string itemName = string.IsNullOrWhiteSpace(entry.ItemNameComment) ? "(no item name)" : entry.ItemNameComment;
            string condition = string.IsNullOrWhiteSpace(entry.ConditionPrmName) ? "(no condition)" : entry.ConditionPrmName;
            return battleName + " | " + itemName + " | " + condition;
        }

        private ItemInfoEntry GetSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
                return null;
            return fileState.Entries[index];
        }

        private void LoadSelectedEntryToEditor()
        {
            ItemInfoEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                ClearEditorFields();
                SetEditorEnabled(false);
                return;
            }

            SetEditorEnabled(true);
            battleItemNameTextBox.Text = entry.BattleItemName ?? "";
            unknown1Value.Value = entry.Unknown1;
            itemPaddingValue.Value = entry.ItemPadding;
            battleIconName1TextBox.Text = entry.BattleIconName1 ?? "";
            battleIconName2TextBox.Text = entry.BattleIconName2 ?? "";
            battleIconName3TextBox.Text = entry.BattleIconName3 ?? "";
            unknown2Value.Value = entry.Unknown2;
            unknown3Value.Value = entry.Unknown3;
            unknown4Value.Value = entry.Unknown4;
            unknown5Value.Value = entry.Unknown5;
            unknown6Value.Value = entry.Unknown6;
            unknown7Value.Value = entry.Unknown7;
            itemNameCommentTextBox.Text = entry.ItemNameComment ?? "";
            unknown9Value.Value = entry.Unknown9;
            unknown10Value.Value = entry.Unknown10;
            unknown11Value.Value = entry.Unknown11;
            unknown12Value.Value = entry.Unknown12;
            conditionPrmNameTextBox.Text = entry.ConditionPrmName ?? "";
            unknown13Value.Value = entry.Unknown13;
            unknown14Value.Value = entry.Unknown14;
            unknown15Value.Value = entry.Unknown15;
            unknown16Value.Value = entry.Unknown16;
            unknown17Value.Value = entry.Unknown17;
            unknown18Value.Value = entry.Unknown18;
        }

        private ItemInfoEntry BuildEntryFromEditor()
        {
            return new ItemInfoEntry
            {
                BattleItemName = battleItemNameTextBox.Text ?? "",
                Unknown1 = (int)unknown1Value.Value,
                ItemPadding = (int)itemPaddingValue.Value,
                BattleIconName1 = battleIconName1TextBox.Text ?? "",
                BattleIconName2 = battleIconName2TextBox.Text ?? "",
                BattleIconName3 = battleIconName3TextBox.Text ?? "",
                Unknown2 = (int)unknown2Value.Value,
                Unknown3 = (int)unknown3Value.Value,
                Unknown4 = (int)unknown4Value.Value,
                Unknown5 = (int)unknown5Value.Value,
                Unknown6 = (int)unknown6Value.Value,
                Unknown7 = (int)unknown7Value.Value,
                ItemNameComment = itemNameCommentTextBox.Text ?? "",
                Unknown9 = (int)unknown9Value.Value,
                Unknown10 = (int)unknown10Value.Value,
                Unknown11 = (int)unknown11Value.Value,
                Unknown12 = (int)unknown12Value.Value,
                ConditionPrmName = conditionPrmNameTextBox.Text ?? "",
                Unknown13 = (int)unknown13Value.Value,
                Unknown14 = (int)unknown14Value.Value,
                Unknown15 = (int)unknown15Value.Value,
                Unknown16 = (int)unknown16Value.Value,
                Unknown17 = (int)unknown17Value.Value,
                Unknown18 = (int)unknown18Value.Value
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
            fileState.Entries.Add(BuildEntryFromEditor());
            RefreshEntryList();
            entryListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DuplicateEntry()
        {
            ItemInfoEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            fileState.Entries.Add(CloneEntry(entry));
            RefreshEntryList();
            entryListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private static ItemInfoEntry CloneEntry(ItemInfoEntry entry)
        {
            return new ItemInfoEntry
            {
                BattleItemName = entry.BattleItemName,
                Unknown1 = entry.Unknown1,
                ItemPadding = entry.ItemPadding,
                BattleIconName1 = entry.BattleIconName1,
                BattleIconName2 = entry.BattleIconName2,
                BattleIconName3 = entry.BattleIconName3,
                Unknown2 = entry.Unknown2,
                Unknown3 = entry.Unknown3,
                Unknown4 = entry.Unknown4,
                Unknown5 = entry.Unknown5,
                Unknown6 = entry.Unknown6,
                Unknown7 = entry.Unknown7,
                ItemNameComment = entry.ItemNameComment,
                Unknown9 = entry.Unknown9,
                Unknown10 = entry.Unknown10,
                Unknown11 = entry.Unknown11,
                Unknown12 = entry.Unknown12,
                ConditionPrmName = entry.ConditionPrmName,
                Unknown13 = entry.Unknown13,
                Unknown14 = entry.Unknown14,
                Unknown15 = entry.Unknown15,
                Unknown16 = entry.Unknown16,
                Unknown17 = entry.Unknown17,
                Unknown18 = entry.Unknown18
            };
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
            ItemInfoEntry selectedEntry = GetSelectedEntry();
            fileState.Entries.Sort((left, right) => string.Compare(left.BattleItemName, right.BattleItemName, StringComparison.OrdinalIgnoreCase));
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

        private static bool TryParseChunk(byte[] bytes, out uint typeTypeVersion, out int padding, out int padding2, out List<ItemInfoEntry> entries)
        {
            typeTypeVersion = 0;
            padding = 0;
            padding2 = 0;
            entries = new List<ItemInfoEntry>();

            if (bytes == null || bytes.Length < HeaderSize)
                return false;

            uint fileSize = ReadUInt32BE(bytes, 0);
            if (fileSize != bytes.Length - 4 && fileSize != bytes.Length)
                return false;

            typeTypeVersion = BitConverter.ToUInt32(bytes, 4);
            uint count = BitConverter.ToUInt32(bytes, 8);
            padding = BitConverter.ToInt32(bytes, 12);
            padding2 = BitConverter.ToInt32(bytes, 16);

            long tableEndLong = HeaderSize + ((long)count * EntrySize);
            if (count > int.MaxValue || tableEndLong > bytes.Length)
                return false;

            int tableEnd = (int)tableEndLong;
            for (int i = 0; i < count; i++)
            {
                int entryOffset = HeaderSize + (i * EntrySize);
                if (!TryReadStringPointer(bytes, entryOffset, tableEnd, out string battleItemName))
                    return false;
                if (!TryReadStringPointer(bytes, entryOffset + 16, tableEnd, out string battleIcon1))
                    return false;
                if (!TryReadStringPointer(bytes, entryOffset + 24, tableEnd, out string battleIcon2))
                    return false;
                if (!TryReadStringPointer(bytes, entryOffset + 32, tableEnd, out string battleIcon3))
                    return false;
                if (!TryReadStringPointer(bytes, entryOffset + 64, tableEnd, out string itemNameComment))
                    return false;
                if (!TryReadStringPointer(bytes, entryOffset + 88, tableEnd, out string conditionPrmName))
                    return false;

                entries.Add(new ItemInfoEntry
                {
                    BattleItemName = battleItemName,
                    Unknown1 = BitConverter.ToInt32(bytes, entryOffset + 8),
                    ItemPadding = BitConverter.ToInt32(bytes, entryOffset + 12),
                    BattleIconName1 = battleIcon1,
                    BattleIconName2 = battleIcon2,
                    BattleIconName3 = battleIcon3,
                    Unknown2 = BitConverter.ToInt32(bytes, entryOffset + 40),
                    Unknown3 = BitConverter.ToInt32(bytes, entryOffset + 44),
                    Unknown4 = BitConverter.ToInt32(bytes, entryOffset + 48),
                    Unknown5 = BitConverter.ToInt32(bytes, entryOffset + 52),
                    Unknown6 = BitConverter.ToInt32(bytes, entryOffset + 56),
                    Unknown7 = BitConverter.ToInt32(bytes, entryOffset + 60),
                    ItemNameComment = itemNameComment,
                    Unknown9 = BitConverter.ToInt32(bytes, entryOffset + 72),
                    Unknown10 = BitConverter.ToInt32(bytes, entryOffset + 76),
                    Unknown11 = BitConverter.ToInt32(bytes, entryOffset + 80),
                    Unknown12 = BitConverter.ToInt32(bytes, entryOffset + 84),
                    ConditionPrmName = conditionPrmName,
                    Unknown13 = BitConverter.ToInt32(bytes, entryOffset + 96),
                    Unknown14 = BitConverter.ToInt32(bytes, entryOffset + 100),
                    Unknown15 = BitConverter.ToInt32(bytes, entryOffset + 104),
                    Unknown16 = BitConverter.ToInt32(bytes, entryOffset + 108),
                    Unknown17 = BitConverter.ToInt32(bytes, entryOffset + 112),
                    Unknown18 = BitConverter.ToInt32(bytes, entryOffset + 116)
                });
            }

            return true;
        }

        private byte[] BuildChunkData()
        {
            int tableEnd = HeaderSize + (fileState.Entries.Count * EntrySize);
            List<byte> output = new List<byte>(new byte[tableEnd]);
            Dictionary<string, int> stringOffsets = new Dictionary<string, int>();

            WriteUInt32BE(output, 0, 0);
            WriteUInt32LE(output, 4, fileState.TypeTypeVersion);
            WriteUInt32LE(output, 8, (uint)fileState.Entries.Count);
            WriteInt32LE(output, 12, fileState.Padding);
            WriteInt32LE(output, 16, fileState.Padding2);

            for (int i = 0; i < fileState.Entries.Count; i++)
            {
                ItemInfoEntry entry = fileState.Entries[i];
                int entryOffset = HeaderSize + (i * EntrySize);

                WriteStringPointer(output, entryOffset, GetStringOffset(output, stringOffsets, entry.BattleItemName));
                WriteInt32LE(output, entryOffset + 8, entry.Unknown1);
                WriteInt32LE(output, entryOffset + 12, entry.ItemPadding);
                WriteStringPointer(output, entryOffset + 16, GetStringOffset(output, stringOffsets, entry.BattleIconName1));
                WriteStringPointer(output, entryOffset + 24, GetStringOffset(output, stringOffsets, entry.BattleIconName2));
                WriteStringPointer(output, entryOffset + 32, GetStringOffset(output, stringOffsets, entry.BattleIconName3));
                WriteInt32LE(output, entryOffset + 40, entry.Unknown2);
                WriteInt32LE(output, entryOffset + 44, entry.Unknown3);
                WriteInt32LE(output, entryOffset + 48, entry.Unknown4);
                WriteInt32LE(output, entryOffset + 52, entry.Unknown5);
                WriteInt32LE(output, entryOffset + 56, entry.Unknown6);
                WriteInt32LE(output, entryOffset + 60, entry.Unknown7);
                WriteStringPointer(output, entryOffset + 64, GetStringOffset(output, stringOffsets, entry.ItemNameComment));
                WriteInt32LE(output, entryOffset + 72, entry.Unknown9);
                WriteInt32LE(output, entryOffset + 76, entry.Unknown10);
                WriteInt32LE(output, entryOffset + 80, entry.Unknown11);
                WriteInt32LE(output, entryOffset + 84, entry.Unknown12);
                WriteStringPointer(output, entryOffset + 88, GetStringOffset(output, stringOffsets, entry.ConditionPrmName));
                WriteInt32LE(output, entryOffset + 96, entry.Unknown13);
                WriteInt32LE(output, entryOffset + 100, entry.Unknown14);
                WriteInt32LE(output, entryOffset + 104, entry.Unknown15);
                WriteInt32LE(output, entryOffset + 108, entry.Unknown16);
                WriteInt32LE(output, entryOffset + 112, entry.Unknown17);
                WriteInt32LE(output, entryOffset + 116, entry.Unknown18);
            }

            WriteUInt32BE(output, 0, (uint)(output.Count - 4));
            return output.ToArray();
        }

        private static bool TryReadStringPointer(byte[] bytes, int pointerOffset, int minStringOffset, out string value)
        {
            value = "";
            ulong relative = BitConverter.ToUInt64(bytes, pointerOffset);
            if (relative == 0)
                return true;

            ulong target = (ulong)pointerOffset + relative;
            if (target > int.MaxValue)
                return false;

            int stringOffset = (int)target;
            if (!IsValidStringOffset(bytes, stringOffset, minStringOffset))
                return false;

            value = ReadNullTerminatedString(bytes, stringOffset);
            return true;
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

        private static int GetStringOffset(List<byte> output, Dictionary<string, int> stringOffsets, string value)
        {
            value = value ?? "";
            if (value.Length == 0)
                return -1;

            int offset;
            if (stringOffsets.TryGetValue(value, out offset))
                return offset;

            offset = output.Count;
            byte[] stringBytes = Encoding.UTF8.GetBytes(value);
            output.AddRange(stringBytes);
            output.Add(0);
            stringOffsets.Add(value, offset);
            return offset;
        }

        private static uint ReadUInt32BE(byte[] bytes, int offset)
        {
            return (uint)((bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3]);
        }

        private static void WriteUInt32BE(List<byte> bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)value;
        }

        private static void WriteUInt32LE(List<byte> bytes, int offset, uint value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++)
                bytes[offset + i] = valueBytes[i];
        }

        private static void WriteInt32LE(List<byte> bytes, int offset, int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++)
                bytes[offset + i] = valueBytes[i];
        }

        private static void WriteStringPointer(List<byte> bytes, int pointerOffset, int stringOffset)
        {
            if (stringOffset < 0)
            {
                for (int i = 0; i < 8; i++)
                    bytes[pointerOffset + i] = 0;
                return;
            }

            ulong relative = (ulong)(stringOffset - pointerOffset);
            byte[] valueBytes = BitConverter.GetBytes(relative);
            for (int i = 0; i < 8; i++)
                bytes[pointerOffset + i] = valueBytes[i];
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
        }
    }
}
