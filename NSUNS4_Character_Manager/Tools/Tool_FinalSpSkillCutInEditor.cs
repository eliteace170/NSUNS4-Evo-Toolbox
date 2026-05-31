using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_FinalSpSkillCutInEditor : Form
    {
        private const int HeaderSize = 0x14;
        private const int VictimCount = 50;
        private const int VictimSize = 0x18;
        private const int EntrySize = 0x4D8;
        private readonly FinalSpSkillCutInFileState fileState = new FinalSpSkillCutInFileState();
        private bool displayHex;

        private sealed class FinalSpSkillCutInVictim
        {
            public int PlayerSettingIdVictim;
            public string VictimFileName = "";
            public string VictimTextureName = "";
            public int Padding;
        }

        private sealed class FinalSpSkillCutInEntry
        {
            public int StoryModeId;
            public int TeamUltId;
            public int PlayerSettingId;
            public int CostumeSlot;
            public string OugiName1 = "";
            public string OugiName2 = "";
            public int Padding1;
            public readonly List<FinalSpSkillCutInVictim> Victims = new List<FinalSpSkillCutInVictim>();
            public int Padding2;
        }

        private sealed class FinalSpSkillCutInFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public string ChunkName = "";
            public string ChunkPath = "";
            public uint Version;
            public byte[] ReservedBytes = new byte[8];
            public readonly List<FinalSpSkillCutInEntry> Entries = new List<FinalSpSkillCutInEntry>();
        }

        private sealed class DuplicateVictimResult
        {
            public int MatchedEntries;
            public int AddedEntries;
            public int SkippedExisting;
            public int SkippedFull;
        }

        public Tool_FinalSpSkillCutInEditor()
        {
            InitializeComponent();
            SetNumericRanges();
            ApplyDisplayMode();
            ResetUi();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime && File.Exists(Main.finalSpSkillCutInPath))
                LoadFile(Main.finalSpSkillCutInPath);
        }

        private void SetNumericRanges()
        {
            SetSignedRange(storyModeIdValue);
            SetSignedRange(teamUltIdValue);
            SetSignedRange(playerSettingIdValue);
            SetSignedRange(costumeSlotValue);
            SetSignedRange(padding1Value);
            SetSignedRange(padding2Value);
            SetSignedRange(victimPlayerSettingIdValue);
            SetSignedRange(victimPaddingValue);
            SetSignedRange(duplicateVictimSourceIdValue);
            SetSignedRange(duplicateVictimNewIdValue);
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
            victimListBox.Items.Clear();
            victimListBox.SelectedIndex = -1;
            ClearEntryFields();
            ClearVictimFields();
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
            duplicateVictimPanel.Enabled = enabled;
        }

        private void SetEditorEnabled(bool enabled)
        {
            entryPanel.Enabled = enabled;
            victimPanel.Enabled = enabled;
            saveVictimButton.Enabled = enabled;
            addVictimButton.Enabled = enabled;
            deleteVictimButton.Enabled = enabled;
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

        private void ClearEntryFields()
        {
            storyModeIdValue.Value = 0;
            teamUltIdValue.Value = 0;
            playerSettingIdValue.Value = 0;
            costumeSlotValue.Value = 0;
            ougiName1TextBox.Text = "";
            ougiName2TextBox.Text = "";
            padding1Value.Value = 0;
            padding2Value.Value = 0;
        }

        private void ClearVictimFields()
        {
            victimPlayerSettingIdValue.Value = 0;
            victimFileNameTextBox.Text = "";
            victimTextureNameTextBox.Text = "";
            victimPaddingValue.Value = 0;
        }

        private void ApplyDisplayMode()
        {
            storyModeIdValue.Hexadecimal = displayHex;
            teamUltIdValue.Hexadecimal = displayHex;
            playerSettingIdValue.Hexadecimal = displayHex;
            costumeSlotValue.Hexadecimal = displayHex;
            padding1Value.Hexadecimal = displayHex;
            padding2Value.Hexadecimal = displayHex;
            victimPlayerSettingIdValue.Hexadecimal = displayHex;
            victimPaddingValue.Hexadecimal = displayHex;
            duplicateVictimSourceIdValue.Hexadecimal = displayHex;
            duplicateVictimNewIdValue.Hexadecimal = displayHex;
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
                XfbinBinaryChunkPage page = FindFinalSpSkillCutInChunk(backend.GetBinaryChunkPages());
                if (page == null)
                {
                    MessageBox.Show("No finalSpSkillCutIn binary chunk was found in this XFBIN.");
                    return;
                }

                uint version;
                byte[] reservedBytes;
                List<FinalSpSkillCutInEntry> entries;
                if (!TryParseChunk(page.BinaryData, out version, out reservedBytes, out entries))
                {
                    MessageBox.Show("Could not parse the finalSpSkillCutIn binary chunk.");
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

            SetFileControlsEnabled(true);
            RefreshEntryList();
        }

        private static XfbinBinaryChunkPage FindFinalSpSkillCutInChunk(List<XfbinBinaryChunkPage> pages)
        {
            foreach (XfbinBinaryChunkPage page in pages)
            {
                string combined = ((page.ChunkName ?? "") + "|" + (page.ChunkPath ?? "") + "|" + (page.BinaryFileName ?? "")).ToLowerInvariant();
                if (combined.Contains("finalspskillcutin"))
                    return page;
            }

            foreach (XfbinBinaryChunkPage page in pages)
            {
                uint version;
                byte[] reservedBytes;
                List<FinalSpSkillCutInEntry> entries;
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
                ClearEntryFields();
                ClearVictimList();
                SetEditorEnabled(false);
                return;
            }

            foreach (FinalSpSkillCutInEntry entry in fileState.Entries)
                entryListBox.Items.Add(BuildEntryLabel(entry));

            entryListBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < fileState.Entries.Count ? selectedIndex : 0;
            SetEditorEnabled(true);
        }

        private void ClearVictimList()
        {
            victimListBox.Items.Clear();
            victimListBox.SelectedIndex = -1;
            ClearVictimFields();
        }

        private string BuildEntryLabel(FinalSpSkillCutInEntry entry)
        {
            return "PlayerSettingParam ID: " + FormatSignedValue(entry.PlayerSettingId);
        }

        private string BuildVictimLabel(FinalSpSkillCutInVictim victim)
        {
            return "PlayerSettingParam ID: " + FormatSignedValue(victim.PlayerSettingIdVictim);
        }

        private string FormatSignedValue(int value)
        {
            return displayHex ? value.ToString("X") : value.ToString();
        }

        private FinalSpSkillCutInEntry GetSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
                return null;
            return fileState.Entries[index];
        }

        private FinalSpSkillCutInVictim GetSelectedVictim()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            int index = victimListBox.SelectedIndex;
            if (entry == null || index < 0 || index >= entry.Victims.Count)
                return null;
            return entry.Victims[index];
        }

        private void LoadSelectedEntryToEditor()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                ClearEntryFields();
                ClearVictimList();
                SetEditorEnabled(false);
                return;
            }

            SetEditorEnabled(true);
            storyModeIdValue.Value = entry.StoryModeId;
            teamUltIdValue.Value = entry.TeamUltId;
            playerSettingIdValue.Value = entry.PlayerSettingId;
            costumeSlotValue.Value = entry.CostumeSlot;
            ougiName1TextBox.Text = entry.OugiName1 ?? "";
            ougiName2TextBox.Text = entry.OugiName2 ?? "";
            padding1Value.Value = entry.Padding1;
            padding2Value.Value = entry.Padding2;
            RefreshVictimList();
        }

        private void RefreshVictimList()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            int selectedIndex = victimListBox.SelectedIndex;
            victimListBox.Items.Clear();

            if (entry == null || entry.Victims.Count == 0)
            {
                ClearVictimList();
                return;
            }

            for (int i = 0; i < entry.Victims.Count; i++)
                victimListBox.Items.Add(BuildVictimLabel(entry.Victims[i]));

            victimListBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < entry.Victims.Count ? selectedIndex : 0;
        }

        private void LoadSelectedVictimToEditor()
        {
            FinalSpSkillCutInVictim victim = GetSelectedVictim();
            if (victim == null)
            {
                ClearVictimFields();
                return;
            }

            victimPlayerSettingIdValue.Value = victim.PlayerSettingIdVictim;
            victimFileNameTextBox.Text = victim.VictimFileName ?? "";
            victimTextureNameTextBox.Text = victim.VictimTextureName ?? "";
            victimPaddingValue.Value = victim.Padding;
        }

        private FinalSpSkillCutInEntry BuildEntryFromEditor()
        {
            FinalSpSkillCutInEntry current = GetSelectedEntry();
            FinalSpSkillCutInEntry entry = new FinalSpSkillCutInEntry
            {
                StoryModeId = (int)storyModeIdValue.Value,
                TeamUltId = (int)teamUltIdValue.Value,
                PlayerSettingId = (int)playerSettingIdValue.Value,
                CostumeSlot = (int)costumeSlotValue.Value,
                OugiName1 = ougiName1TextBox.Text ?? "",
                OugiName2 = ougiName2TextBox.Text ?? "",
                Padding1 = (int)padding1Value.Value,
                Padding2 = (int)padding2Value.Value
            };

            if (current != null)
            {
                foreach (FinalSpSkillCutInVictim victim in current.Victims)
                    entry.Victims.Add(CloneVictim(victim));
            }

            return entry;
        }

        private FinalSpSkillCutInVictim BuildVictimFromEditor()
        {
            return new FinalSpSkillCutInVictim
            {
                PlayerSettingIdVictim = (int)victimPlayerSettingIdValue.Value,
                VictimFileName = victimFileNameTextBox.Text ?? "",
                VictimTextureName = victimTextureNameTextBox.Text ?? "",
                Padding = (int)victimPaddingValue.Value
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
            RefreshVictimList();
        }

        private void UpdateSelectedVictim()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            int index = victimListBox.SelectedIndex;
            if (entry == null || index < 0 || index >= entry.Victims.Count)
            {
                MessageBox.Show("No victim selected.");
                return;
            }

            FinalSpSkillCutInVictim updatedVictim = BuildVictimFromEditor();
            if (!IsVictimExisting(updatedVictim))
            {
                entry.Victims.RemoveAt(index);
                RefreshVictimList();
                return;
            }

            entry.Victims[index] = updatedVictim;
            victimListBox.Items[index] = BuildVictimLabel(entry.Victims[index]);
        }

        private void AddEntry()
        {
            FinalSpSkillCutInEntry entry = BuildEntryFromEditor();
            fileState.Entries.Add(entry);
            RefreshEntryList();
            entryListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DuplicateEntry()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            fileState.Entries.Add(CloneEntry(entry));
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

        private void AddVictim()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            if (entry.Victims.Count >= VictimCount)
            {
                MessageBox.Show("This entry already has 50 victims.");
                return;
            }

            FinalSpSkillCutInVictim victim = BuildVictimFromEditor();
            if (!IsVictimExisting(victim))
            {
                MessageBox.Show("Enter victim data before adding a victim.");
                return;
            }

            entry.Victims.Add(victim);
            RefreshVictimList();
            victimListBox.SelectedIndex = entry.Victims.Count - 1;
        }

        private void DeleteVictim()
        {
            FinalSpSkillCutInEntry entry = GetSelectedEntry();
            int index = victimListBox.SelectedIndex;
            if (entry == null || index < 0 || index >= entry.Victims.Count)
            {
                MessageBox.Show("No victim selected.");
                return;
            }

            entry.Victims.RemoveAt(index);
            RefreshVictimList();
            if (entry.Victims.Count > 0)
                victimListBox.SelectedIndex = Math.Min(index, entry.Victims.Count - 1);
        }

        private void DuplicateVictimAcrossEntries()
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("No file loaded...");
                return;
            }

            int sourceId = (int)duplicateVictimSourceIdValue.Value;
            int newId = (int)duplicateVictimNewIdValue.Value;
            if (sourceId == newId)
            {
                MessageBox.Show("Source and duplicate PlayerSettingParam IDs must be different.");
                return;
            }

            DuplicateVictimResult result = DuplicateVictimAcrossEntriesCore(sourceId, newId);
            RefreshEntryList();
            MessageBox.Show("Matched entries: " + result.MatchedEntries + "\nAdded victims: " + result.AddedEntries + "\nSkipped existing: " + result.SkippedExisting + "\nSkipped full: " + result.SkippedFull);
        }

        private DuplicateVictimResult DuplicateVictimAcrossEntriesCore(int sourceId, int newId)
        {
            DuplicateVictimResult result = new DuplicateVictimResult();
            foreach (FinalSpSkillCutInEntry entry in fileState.Entries)
            {
                FinalSpSkillCutInVictim sourceVictim = null;
                foreach (FinalSpSkillCutInVictim victim in entry.Victims)
                {
                    if (victim.PlayerSettingIdVictim == sourceId)
                    {
                        sourceVictim = victim;
                        break;
                    }
                }

                if (sourceVictim == null)
                    continue;

                result.MatchedEntries++;
                if (EntryHasVictimId(entry, newId))
                {
                    result.SkippedExisting++;
                    continue;
                }

                if (entry.Victims.Count >= VictimCount)
                {
                    result.SkippedFull++;
                    continue;
                }

                FinalSpSkillCutInVictim duplicate = CloneVictim(sourceVictim);
                duplicate.PlayerSettingIdVictim = newId;
                entry.Victims.Add(duplicate);
                result.AddedEntries++;
            }

            return result;
        }

        private static bool EntryHasVictimId(FinalSpSkillCutInEntry entry, int playerSettingId)
        {
            foreach (FinalSpSkillCutInVictim victim in entry.Victims)
            {
                if (victim.PlayerSettingIdVictim == playerSettingId)
                    return true;
            }

            return false;
        }

        private void SortEntries()
        {
            FinalSpSkillCutInEntry selectedEntry = GetSelectedEntry();
            fileState.Entries.Sort((left, right) => left.PlayerSettingId.CompareTo(right.PlayerSettingId));
            RefreshEntryList();

            if (selectedEntry == null)
                return;

            int index = fileState.Entries.IndexOf(selectedEntry);
            if (index >= 0)
                entryListBox.SelectedIndex = index;
        }

        private static FinalSpSkillCutInEntry CloneEntry(FinalSpSkillCutInEntry entry)
        {
            FinalSpSkillCutInEntry copy = new FinalSpSkillCutInEntry
            {
                StoryModeId = entry.StoryModeId,
                TeamUltId = entry.TeamUltId,
                PlayerSettingId = entry.PlayerSettingId,
                CostumeSlot = entry.CostumeSlot,
                OugiName1 = entry.OugiName1,
                OugiName2 = entry.OugiName2,
                Padding1 = entry.Padding1,
                Padding2 = entry.Padding2
            };

            foreach (FinalSpSkillCutInVictim victim in entry.Victims)
                copy.Victims.Add(CloneVictim(victim));
            return copy;
        }

        private static FinalSpSkillCutInVictim CloneVictim(FinalSpSkillCutInVictim victim)
        {
            return new FinalSpSkillCutInVictim
            {
                PlayerSettingIdVictim = victim.PlayerSettingIdVictim,
                VictimFileName = victim.VictimFileName,
                VictimTextureName = victim.VictimTextureName,
                Padding = victim.Padding
            };
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

        private static bool TryParseChunk(byte[] bytes, out uint version, out byte[] reservedBytes, out List<FinalSpSkillCutInEntry> entries)
        {
            version = 0;
            reservedBytes = new byte[8];
            entries = new List<FinalSpSkillCutInEntry>();

            if (bytes == null || bytes.Length < HeaderSize)
                return false;

            uint fileSize = ReadUInt32BE(bytes, 0);
            if (fileSize != bytes.Length - 4 && fileSize != bytes.Length)
                return false;

            version = BitConverter.ToUInt32(bytes, 4);
            uint count = BitConverter.ToUInt32(bytes, 8);
            Array.Copy(bytes, 12, reservedBytes, 0, 8);

            long tableEndLong = HeaderSize + ((long)count * EntrySize);
            if (count > int.MaxValue || tableEndLong > bytes.Length)
                return false;

            int tableEnd = (int)tableEndLong;
            for (int i = 0; i < count; i++)
            {
                int entryOffset = HeaderSize + (i * EntrySize);
                if (!TryReadStringPointer(bytes, entryOffset + 16, tableEnd, out string ougiName1))
                    return false;
                if (!TryReadStringPointer(bytes, entryOffset + 24, tableEnd, out string ougiName2))
                    return false;

                FinalSpSkillCutInEntry entry = new FinalSpSkillCutInEntry
                {
                    StoryModeId = BitConverter.ToInt32(bytes, entryOffset),
                    TeamUltId = BitConverter.ToInt32(bytes, entryOffset + 4),
                    PlayerSettingId = BitConverter.ToInt32(bytes, entryOffset + 8),
                    CostumeSlot = BitConverter.ToInt32(bytes, entryOffset + 12),
                    OugiName1 = ougiName1,
                    OugiName2 = ougiName2,
                    Padding1 = BitConverter.ToInt32(bytes, entryOffset + 32),
                    Padding2 = BitConverter.ToInt32(bytes, entryOffset + 1236)
                };

                int victimsOffset = entryOffset + 36;
                for (int victimIndex = 0; victimIndex < VictimCount; victimIndex++)
                {
                    int victimOffset = victimsOffset + (victimIndex * VictimSize);
                    if (!TryReadStringPointer(bytes, victimOffset + 4, tableEnd, out string victimFileName))
                        return false;
                    if (!TryReadStringPointer(bytes, victimOffset + 12, tableEnd, out string victimTextureName))
                        return false;

                    FinalSpSkillCutInVictim victim = new FinalSpSkillCutInVictim
                    {
                        PlayerSettingIdVictim = BitConverter.ToInt32(bytes, victimOffset),
                        VictimFileName = victimFileName,
                        VictimTextureName = victimTextureName,
                        Padding = BitConverter.ToInt32(bytes, victimOffset + 20)
                    };
                    if (IsVictimExisting(victim))
                        entry.Victims.Add(victim);
                }

                entries.Add(entry);
            }

            return true;
        }

        private byte[] BuildChunkData()
        {
            int tableEnd = HeaderSize + (fileState.Entries.Count * EntrySize);
            List<byte> output = new List<byte>(new byte[tableEnd]);
            Dictionary<string, int> stringOffsets = new Dictionary<string, int>();

            WriteUInt32BE(output, 0, 0);
            WriteUInt32LE(output, 4, fileState.Version);
            WriteUInt32LE(output, 8, (uint)fileState.Entries.Count);
            if (fileState.ReservedBytes != null)
            {
                for (int i = 0; i < Math.Min(8, fileState.ReservedBytes.Length); i++)
                    output[12 + i] = fileState.ReservedBytes[i];
            }

            for (int i = 0; i < fileState.Entries.Count; i++)
            {
                FinalSpSkillCutInEntry entry = fileState.Entries[i];
                int entryOffset = HeaderSize + (i * EntrySize);

                WriteInt32LE(output, entryOffset, entry.StoryModeId);
                WriteInt32LE(output, entryOffset + 4, entry.TeamUltId);
                WriteInt32LE(output, entryOffset + 8, entry.PlayerSettingId);
                WriteInt32LE(output, entryOffset + 12, entry.CostumeSlot);
                WriteStringPointer(output, entryOffset + 16, GetStringOffset(output, stringOffsets, entry.OugiName1));
                WriteStringPointer(output, entryOffset + 24, GetStringOffset(output, stringOffsets, entry.OugiName2));
                WriteInt32LE(output, entryOffset + 32, entry.Padding1);

                int victimsOffset = entryOffset + 36;
                for (int victimIndex = 0; victimIndex < VictimCount; victimIndex++)
                {
                    FinalSpSkillCutInVictim victim = victimIndex < entry.Victims.Count ? entry.Victims[victimIndex] : new FinalSpSkillCutInVictim();
                    int victimOffset = victimsOffset + (victimIndex * VictimSize);
                    WriteInt32LE(output, victimOffset, victim.PlayerSettingIdVictim);
                    WriteStringPointer(output, victimOffset + 4, GetStringOffset(output, stringOffsets, victim.VictimFileName));
                    WriteStringPointer(output, victimOffset + 12, GetStringOffset(output, stringOffsets, victim.VictimTextureName));
                    WriteInt32LE(output, victimOffset + 20, victim.Padding);
                }

                WriteInt32LE(output, entryOffset + 1236, entry.Padding2);
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

        private static bool IsVictimExisting(FinalSpSkillCutInVictim victim)
        {
            return victim.PlayerSettingIdVictim != 0 ||
                   !string.IsNullOrWhiteSpace(victim.VictimFileName) ||
                   !string.IsNullOrWhiteSpace(victim.VictimTextureName) ||
                   victim.Padding != 0;
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

        private void victimListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedVictimToEditor();
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

        private void saveVictimButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedVictim();
        }

        private void addVictimButton_Click(object sender, EventArgs e)
        {
            AddVictim();
        }

        private void deleteVictimButton_Click(object sender, EventArgs e)
        {
            DeleteVictim();
        }

        private void duplicateVictimAcrossEntriesButton_Click(object sender, EventArgs e)
        {
            DuplicateVictimAcrossEntries();
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
            {
                RefreshEntryList();
                RefreshVictimList();
            }
        }
    }
}
