using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_MusicParamEditor : Form
    {
        private const int HeaderSize = 0x14;
        private const int EntrySize = 0x30;
        private readonly MusicParamFileState fileState = new MusicParamFileState();

        private sealed class MusicParamEntry
        {
            public string MusicString = "";
            public string MusicID = "";
            public uint Index;
            public uint Cost;
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
        }

        private sealed class MusicParamChunkState
        {
            public string ChunkName = "";
            public string ChunkPath = "";
            public uint Version;
            public byte[] ReservedBytes = new byte[8];
            public readonly List<MusicParamEntry> Entries = new List<MusicParamEntry>();
        }

        private sealed class MusicParamFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public readonly List<MusicParamChunkState> Chunks = new List<MusicParamChunkState>();
        }

        public Tool_MusicParamEditor()
        {
            InitializeComponent();
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
            moveUpButton.Enabled = false;
            moveDownButton.Enabled = false;
            saveSelectedButton.Enabled = false;
        }

        private void ClearFileState()
        {
            fileState.FileOpen = false;
            fileState.FilePath = "";
            fileState.Chunks.Clear();
            ResetUi();
        }

        private void ClearEditorFields()
        {
            musicStringTextBox.Text = "";
            musicIdTextBox.Text = "";
            indexValue.Value = 0;
            costValue.Value = 0;
            unknown1Value.Value = 0;
            unknown2Value.Value = 0;
            unknown3Value.Value = 0;
            unknown4Value.Value = 0;
            unknown5Value.Value = 0;
            unknown6Value.Value = 0;
        }

        private void SetEditorEnabled(bool enabled)
        {
            editorPanel.Enabled = enabled;
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
            List<MusicParamChunkState> chunks = new List<MusicParamChunkState>();
            using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
            {
                List<XfbinBinaryChunkPage> pages = backend.GetBinaryChunkPages();
                IEnumerable<XfbinBinaryChunkPage> orderedPages = pages
                    .OrderByDescending(IsNamedMusicParamChunk)
                    .ThenBy(x => x.Index);

                foreach (XfbinBinaryChunkPage page in orderedPages)
                {
                    uint version;
                    byte[] reservedBytes;
                    List<MusicParamEntry> entries;
                    if (!TryParseMusicParamChunk(page.BinaryData, out version, out reservedBytes, out entries))
                        continue;

                    chunks.Add(new MusicParamChunkState
                    {
                        ChunkName = page.ChunkName ?? "",
                        ChunkPath = page.ChunkPath ?? "",
                        Version = version,
                        ReservedBytes = reservedBytes
                    });
                    chunks[chunks.Count - 1].Entries.AddRange(entries);
                }
            }

            if (chunks.Count == 0)
            {
                MessageBox.Show("No musicParam-style binary chunk was found in this XFBIN.");
                return;
            }

            fileState.FileOpen = true;
            fileState.FilePath = filePath;
            fileState.Chunks.Clear();
            fileState.Chunks.AddRange(chunks);
            bool hasChunks = fileState.Chunks.Count > 0;
            addEntryButton.Enabled = hasChunks;
            duplicateEntryButton.Enabled = hasChunks;
            deleteEntryButton.Enabled = hasChunks;
            moveUpButton.Enabled = hasChunks;
            moveDownButton.Enabled = hasChunks;
            saveSelectedButton.Enabled = hasChunks;
            RefreshEntryList();
        }

        private void RefreshEntryList()
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            entryListBox.Items.Clear();

            if (chunk == null || chunk.Entries.Count == 0)
            {
                entryListBox.Items.Add("No entries found...");
                entryListBox.SelectedIndex = -1;
                ClearEditorFields();
                SetEditorEnabled(false);
                return;
            }

            foreach (MusicParamEntry entry in chunk.Entries)
                entryListBox.Items.Add(BuildEntryLabel(entry));

            entryListBox.SelectedIndex = 0;
            SetEditorEnabled(true);
        }

        private static string BuildEntryLabel(MusicParamEntry entry)
        {
            string musicId = string.IsNullOrWhiteSpace(entry.MusicID) ? "(no id)" : entry.MusicID;
            string musicString = string.IsNullOrWhiteSpace(entry.MusicString) ? "(no string)" : entry.MusicString;
            return entry.Index.ToString() + " | " + musicId + " | " + musicString;
        }

        private MusicParamChunkState GetSelectedChunk()
        {
            if (fileState.Chunks.Count == 0)
                return null;
            return fileState.Chunks[0];
        }

        private MusicParamEntry GetSelectedEntry()
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            int index = entryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count)
                return null;
            return chunk.Entries[index];
        }

        private void LoadSelectedEntryToEditor()
        {
            MusicParamEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                ClearEditorFields();
                SetEditorEnabled(false);
                return;
            }

            SetEditorEnabled(true);
            musicStringTextBox.Text = entry.MusicString ?? "";
            musicIdTextBox.Text = entry.MusicID ?? "";
            indexValue.Value = entry.Index;
            costValue.Value = entry.Cost;
            unknown1Value.Value = entry.Unknown1;
            unknown2Value.Value = entry.Unknown2;
            unknown3Value.Value = entry.Unknown3;
            unknown4Value.Value = entry.Unknown4;
            unknown5Value.Value = entry.Unknown5;
            unknown6Value.Value = entry.Unknown6;
        }

        private MusicParamEntry BuildEntryFromEditor()
        {
            return new MusicParamEntry
            {
                MusicString = musicStringTextBox.Text ?? "",
                MusicID = musicIdTextBox.Text ?? "",
                Index = (uint)indexValue.Value,
                Cost = (uint)costValue.Value,
                Unknown1 = (uint)unknown1Value.Value,
                Unknown2 = (uint)unknown2Value.Value,
                Unknown3 = (uint)unknown3Value.Value,
                Unknown4 = (uint)unknown4Value.Value,
                Unknown5 = (uint)unknown5Value.Value,
                Unknown6 = (uint)unknown6Value.Value
            };
        }

        private void UpdateSelectedEntry()
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            int index = entryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            chunk.Entries[index] = BuildEntryFromEditor();
            entryListBox.Items[index] = BuildEntryLabel(chunk.Entries[index]);
        }

        private void AddEntry()
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            if (chunk == null)
            {
                MessageBox.Show("No musicParam chunk loaded.");
                return;
            }

            MusicParamEntry entry = BuildEntryFromEditor();
            chunk.Entries.Add(entry);
            RefreshEntryList();
            entryListBox.SelectedIndex = chunk.Entries.Count - 1;
        }

        private void DuplicateEntry()
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            MusicParamEntry entry = GetSelectedEntry();
            if (chunk == null || entry == null)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            MusicParamEntry copy = new MusicParamEntry
            {
                MusicString = entry.MusicString,
                MusicID = entry.MusicID,
                Index = entry.Index,
                Cost = entry.Cost,
                Unknown1 = entry.Unknown1,
                Unknown2 = entry.Unknown2,
                Unknown3 = entry.Unknown3,
                Unknown4 = entry.Unknown4,
                Unknown5 = entry.Unknown5,
                Unknown6 = entry.Unknown6
            };

            chunk.Entries.Add(copy);
            RefreshEntryList();
            entryListBox.SelectedIndex = chunk.Entries.Count - 1;
        }

        private void DeleteEntry()
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            int index = entryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            chunk.Entries.RemoveAt(index);
            RefreshEntryList();
            if (chunk.Entries.Count > 0)
                entryListBox.SelectedIndex = Math.Min(index, chunk.Entries.Count - 1);
        }

        private void MoveEntry(int direction)
        {
            MusicParamChunkState chunk = GetSelectedChunk();
            int index = entryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            int newIndex = index + direction;
            if (newIndex < 0 || newIndex >= chunk.Entries.Count)
                return;

            MusicParamEntry entry = chunk.Entries[index];
            chunk.Entries.RemoveAt(index);
            chunk.Entries.Insert(newIndex, entry);
            RefreshEntryList();
            entryListBox.SelectedIndex = newIndex;
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
                foreach (MusicParamChunkState chunk in fileState.Chunks)
                    backend.UpsertBinaryChunk(chunk.ChunkName, chunk.ChunkName, chunk.ChunkPath, BuildChunkData(chunk));

                backend.RepackTo(outputPath);
            }

            if (!File.Exists(outputPath))
            {
                MessageBox.Show("XFBIN write failed.");
                return;
            }

            fileState.FilePath = outputPath;
        }

        private static bool TryParseMusicParamChunk(byte[] bytes, out uint version, out byte[] reservedBytes, out List<MusicParamEntry> entries)
        {
            version = 0;
            reservedBytes = new byte[8];
            entries = new List<MusicParamEntry>();

            if (bytes == null || bytes.Length < HeaderSize)
                return false;

            uint fileSize = ReadUInt32BE(bytes, 0);
            if (fileSize != bytes.Length && fileSize != bytes.Length - 4)
                return false;

            version = BitConverter.ToUInt32(bytes, 4);
            uint count = BitConverter.ToUInt32(bytes, 8);
            Array.Copy(bytes, 12, reservedBytes, 0, 8);

            long tableEnd = HeaderSize + ((long)count * EntrySize);
            if (tableEnd > bytes.Length)
                return false;

            for (int i = 0; i < count; i++)
            {
                int entryOffset = HeaderSize + (i * EntrySize);
                ulong musicStringRelative = BitConverter.ToUInt64(bytes, entryOffset);
                ulong musicIdRelative = BitConverter.ToUInt64(bytes, entryOffset + 8);

                long musicStringOffsetLong = entryOffset + (long)musicStringRelative;
                long musicIdOffsetLong = entryOffset + 8L + (long)musicIdRelative;
                if (musicStringOffsetLong > int.MaxValue || musicIdOffsetLong > int.MaxValue)
                    return false;

                int musicStringOffset = (int)musicStringOffsetLong;
                int musicIdOffset = (int)musicIdOffsetLong;
                if (!IsValidStringOffset(bytes, musicStringOffset) || !IsValidStringOffset(bytes, musicIdOffset))
                    return false;

                entries.Add(new MusicParamEntry
                {
                    MusicString = ReadNullTerminatedString(bytes, musicStringOffset),
                    MusicID = ReadNullTerminatedString(bytes, musicIdOffset),
                    Index = BitConverter.ToUInt32(bytes, entryOffset + 16),
                    Cost = BitConverter.ToUInt32(bytes, entryOffset + 20),
                    Unknown1 = BitConverter.ToUInt32(bytes, entryOffset + 24),
                    Unknown2 = BitConverter.ToUInt32(bytes, entryOffset + 28),
                    Unknown3 = BitConverter.ToUInt32(bytes, entryOffset + 32),
                    Unknown4 = BitConverter.ToUInt32(bytes, entryOffset + 36),
                    Unknown5 = BitConverter.ToUInt32(bytes, entryOffset + 40),
                    Unknown6 = BitConverter.ToUInt32(bytes, entryOffset + 44)
                });
            }

            return true;
        }

        private static byte[] BuildChunkData(MusicParamChunkState chunk)
        {
            List<byte[]> stringBytes = new List<byte[]>();
            int stringDataOffset = HeaderSize + (chunk.Entries.Count * EntrySize);
            int totalStringBytes = 0;

            foreach (MusicParamEntry entry in chunk.Entries)
            {
                byte[] musicStringBytes = BuildNullTerminatedString(entry.MusicString);
                byte[] musicIdBytes = BuildNullTerminatedString(entry.MusicID);
                stringBytes.Add(musicStringBytes);
                stringBytes.Add(musicIdBytes);
                totalStringBytes += musicStringBytes.Length + musicIdBytes.Length;
            }

            byte[] output = new byte[stringDataOffset + totalStringBytes];
            // The template-backed sample stores FileSize without the leading size field itself.
            WriteUInt32BE(output, 0, (uint)(output.Length - 4));
            Array.Copy(BitConverter.GetBytes(chunk.Version), 0, output, 4, 4);
            Array.Copy(BitConverter.GetBytes((uint)chunk.Entries.Count), 0, output, 8, 4);
            Array.Clear(output, 12, 8);
            if (chunk.ReservedBytes != null)
                Array.Copy(chunk.ReservedBytes, 0, output, 12, Math.Min(8, chunk.ReservedBytes.Length));

            int nextStringOffset = stringDataOffset;
            for (int i = 0; i < chunk.Entries.Count; i++)
            {
                MusicParamEntry entry = chunk.Entries[i];
                int entryOffset = HeaderSize + (i * EntrySize);

                byte[] musicStringBytes = stringBytes[i * 2];
                byte[] musicIdBytes = stringBytes[(i * 2) + 1];

                WriteUInt64LE(output, entryOffset, (ulong)(nextStringOffset - entryOffset));
                Array.Copy(musicStringBytes, 0, output, nextStringOffset, musicStringBytes.Length);
                nextStringOffset += musicStringBytes.Length;

                WriteUInt64LE(output, entryOffset + 8, (ulong)(nextStringOffset - (entryOffset + 8)));
                Array.Copy(musicIdBytes, 0, output, nextStringOffset, musicIdBytes.Length);
                nextStringOffset += musicIdBytes.Length;

                Array.Copy(BitConverter.GetBytes(entry.Index), 0, output, entryOffset + 16, 4);
                Array.Copy(BitConverter.GetBytes(entry.Cost), 0, output, entryOffset + 20, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unknown1), 0, output, entryOffset + 24, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unknown2), 0, output, entryOffset + 28, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unknown3), 0, output, entryOffset + 32, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unknown4), 0, output, entryOffset + 36, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unknown5), 0, output, entryOffset + 40, 4);
                Array.Copy(BitConverter.GetBytes(entry.Unknown6), 0, output, entryOffset + 44, 4);
            }

            return output;
        }

        private static bool IsNamedMusicParamChunk(XfbinBinaryChunkPage page)
        {
            string combined = ((page.ChunkName ?? "") + "|" + (page.ChunkPath ?? "") + "|" + (page.BinaryFileName ?? "")).ToLowerInvariant();
            return combined.Contains("musicparam");
        }

        private static bool IsValidStringOffset(byte[] bytes, int offset)
        {
            if (offset < HeaderSize || offset >= bytes.Length)
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
            output[output.Length - 1] = 0;
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

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveEntry(-1);
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveEntry(1);
        }
    }
}
