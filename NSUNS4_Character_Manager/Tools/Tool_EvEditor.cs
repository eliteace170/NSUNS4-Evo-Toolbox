using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_EvEditor : Form
    {
        private const int FileSectionHeaderSize = 0x20;
        private const int FileSectionSizeOffsetA = 0x0C;
        private const int FileSectionSizeOffsetB = 0x18;
        private const int DataHeaderSize = 0x0A;
        private const int DataHeaderCountOffset = 0x08;
        private const int DataHeaderFileSizeOffset = 0x04;
        private const int EvEntrySize = 0xCA;
        private const int EvSplEntrySize = 0x40;
        private const int StringLen = 32;
        private const int TailSize = 20;
        private const int BattleSectionPreludeSize = 0x2A;
        private const string BattleClipboardPrefix = "NSUNS4_EV_BATTLE_V1";
        private const string UltimateClipboardPrefix = "NSUNS4_EV_ULTIMATE_V1";

        private readonly BattleFileState battleState = new BattleFileState();
        private readonly UltimateFileState ultimateState = new UltimateFileState();
        private bool suppressBattleSelectionEvents;
        private bool suppressUltimateSelectionEvents;
        private sealed class EventTypeOption
        {
            public float Value;
            public string Name;
            public override string ToString() { return Name; }
        }

        private readonly EventTypeOption[] eventTypeOptions =
        {
            new EventTypeOption { Value = 0.0f, Name = "SePlay" },
            new EventTypeOption { Value = 1.0f, Name = "SeStop" },
            new EventTypeOption { Value = 2.0f, Name = "BgmPlay" },
            new EventTypeOption { Value = 3.0f, Name = "BgmStop" },
            new EventTypeOption { Value = 4.0f, Name = "VoicePlay" },
            new EventTypeOption { Value = 5.0f, Name = "VoiceStop" },
            new EventTypeOption { Value = 6.0f, Name = "Deprecated1" },
            new EventTypeOption { Value = 7.0f, Name = "Deprecated2" },
            new EventTypeOption { Value = 8.0f, Name = "BgmStopFade" }
        };

        private sealed class BattleEntry
        {
            public int OriginalOffset;
            public string SoundPL = "";
            public short Pan;
            public float Volume;
            public short Pitch;
            public float EventType;
            public short SoundDelay;
            public float HighPassFilter;
            public float LowPassFilter;
            public string SoundXfbin = "";
            public string AnimationName = "";
            public string Hitbox = "";
            public float LocationX;
            public float LocationY;
            public float LocationZ;
            public short SoundDelayReset;
            public short Unknown;
            public short SoundDelayResetAnm;
            public bool LoopFlag;
            public string PL_ANM_String = "";
        }

        private sealed class UltimateEntry
        {
            public int OriginalOffset;
            public string SoundPL = "";
            public short Pan;
            public float Volume;
            public short Pitch;
            public float EventType;
            public short SoundDelay;
            public float HighPassFilter;
            public float LowPassFilter;
            public short Index;
            public short SoundCutframe;
            public short Unknown;
            public float FadeParam;
        }

        private sealed class UltimateChunk
        {
            public string Name = "";
            public int FileSectionOffset;
            public int OriginalEntryCount = -1;
            public byte[] SectionTemplate = new byte[0];
            public byte[] GapBytes = new byte[0];
            public readonly List<UltimateEntry> Entries = new List<UltimateEntry>();
        }

        private sealed class BattleFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public byte[] FileBytes = new byte[0];
            public int FileSectionOffset;
            public readonly List<BattleEntry> Entries = new List<BattleEntry>();
        }

        private sealed class UltimateFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public byte[] FileBytes = new byte[0];
            public readonly List<UltimateChunk> Chunks = new List<UltimateChunk>();
        }

        public Tool_EvEditor()
        {
            InitializeComponent();
            battleEventTypeValue.Items.AddRange(eventTypeOptions);
            ultimateEventTypeValue.Items.AddRange(eventTypeOptions);
            if (battleEventTypeValue.Items.Count > 0)
            {
                battleEventTypeValue.SelectedIndex = 0;
                ultimateEventTypeValue.SelectedIndex = 0;
            }
        }

        private void Tool_EvEditor_Load(object sender, EventArgs e)
        {
            ResetBattleUi();
            ResetUltimateUi();
        }

        private static int ReadInt32BE(byte[] bytes, int offset)
        {
            return (bytes[offset + 3]) + (bytes[offset + 2] * 256) + (bytes[offset + 1] * 65536) + (bytes[offset] * 16777216);
        }

        private static void WriteInt32BE(byte[] target, int offset, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Array.Copy(bytes, 0, target, offset, 4);
        }

        private static string ReadFixedString(byte[] bytes, int offset, int length)
        {
            string value = Encoding.ASCII.GetString(bytes, offset, length);
            int nullIndex = value.IndexOf('\0');
            return nullIndex >= 0 ? value.Substring(0, nullIndex) : value;
        }

        private static void WriteFixedString(byte[] target, int offset, int length, string value)
        {
            Array.Clear(target, offset, length);
            if (string.IsNullOrEmpty(value))
                return;
            byte[] source = Encoding.ASCII.GetBytes(value);
            Array.Copy(source, 0, target, offset, Math.Min(length, source.Length));
        }

        private static void WriteInt16(byte[] target, int offset, short value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, target, offset, 2);
        }

        private static void WriteSingle(byte[] target, int offset, float value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, target, offset, 4);
        }

        private bool IsBattleTabSelected() { return mainTabControl.SelectedTab == battleTabPage; }

        private void ResetBattleUi()
        {
            suppressBattleSelectionEvents = true;
            battleListBox.Items.Clear();
            battleListBox.Items.Add("No file loaded...");
            battleListBox.SelectedIndex = -1;
            suppressBattleSelectionEvents = false;
            ClearBattleEditorFields();
        }

        private void ResetUltimateUi()
        {
            suppressUltimateSelectionEvents = true;
            ultimateChunkListBox.Items.Clear();
            ultimateChunkListBox.Items.Add("No file loaded...");
            ultimateChunkListBox.SelectedIndex = -1;
            ultimateEntryListBox.Items.Clear();
            ultimateEntryListBox.Items.Add("No file loaded...");
            ultimateEntryListBox.SelectedIndex = -1;
            suppressUltimateSelectionEvents = false;
            ClearUltimateEditorFields();
        }

        private void ClearBattleState()
        {
            battleState.FileOpen = false;
            battleState.FilePath = "";
            battleState.FileBytes = new byte[0];
            battleState.FileSectionOffset = 0;
            battleState.Entries.Clear();
            ResetBattleUi();
        }

        private void ClearUltimateState()
        {
            ultimateState.FileOpen = false;
            ultimateState.FilePath = "";
            ultimateState.FileBytes = new byte[0];
            ultimateState.Chunks.Clear();
            ResetUltimateUi();
        }

        private void OpenActiveFile() { if (IsBattleTabSelected()) OpenBattleFile(); else OpenUltimateFile(); }
        private void SaveActiveFile() { if (IsBattleTabSelected()) SaveBattleFile(false); else SaveUltimateFile(false); }
        private void SaveActiveFileAs() { if (IsBattleTabSelected()) SaveBattleFile(true); else SaveUltimateFile(true); }

        private void CloseActiveFile()
        {
            if (IsBattleTabSelected())
            {
                if (!battleState.FileOpen) { MessageBox.Show("No Battle file loaded..."); return; }
                ClearBattleState();
            }
            else
            {
                if (!ultimateState.FileOpen) { MessageBox.Show("No Ultimate file loaded..."); return; }
                ClearUltimateState();
            }
        }

        private void OpenBattleFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = ".xfbin";
                dialog.Filter = "Battle EV (*.xfbin)|*.xfbin";
                if (dialog.ShowDialog() != DialogResult.OK) return;
                byte[] bytes = File.ReadAllBytes(dialog.FileName);
                int fileSection = XfbinParser.GetFileSectionIndex(bytes);
                int dataStart = fileSection + FileSectionHeaderSize;
                int count = BitConverter.ToInt16(bytes, dataStart + DataHeaderCountOffset);
                int fileSize = ReadInt32BE(bytes, dataStart + DataHeaderFileSizeOffset);
                if (DetectEvSplMode(dialog.FileName, count, fileSize))
                {
                    MessageBox.Show("That file looks like an Ultimate ev_spl file. Open it from the Ultimate tab.");
                    return;
                }
                ClearBattleState();
                battleState.FileOpen = true;
                battleState.FilePath = dialog.FileName;
                battleState.FileBytes = bytes;
                battleState.FileSectionOffset = fileSection;
                OpenBattleEntries(bytes, fileSection, count, battleState.Entries);
                RefreshBattleList();
            }
        }

        private void OpenUltimateFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = ".xfbin";
                dialog.Filter = "Ultimate EV SPL (*.xfbin)|*.xfbin";
                if (dialog.ShowDialog() != DialogResult.OK) return;
                byte[] bytes = File.ReadAllBytes(dialog.FileName);
                int fileSection = XfbinParser.GetFileSectionIndex(bytes);
                int dataStart = fileSection + FileSectionHeaderSize;
                int count = BitConverter.ToInt16(bytes, dataStart + DataHeaderCountOffset);
                int fileSize = ReadInt32BE(bytes, dataStart + DataHeaderFileSizeOffset);
                if (!DetectEvSplMode(dialog.FileName, count, fileSize))
                {
                    MessageBox.Show("That file looks like a Battle ev file. Open it from the Battle tab.");
                    return;
                }
                ClearUltimateState();
                ultimateState.FileOpen = true;
                ultimateState.FilePath = dialog.FileName;
                ultimateState.FileBytes = bytes;
                OpenUltimateChunks(bytes, fileSection, ultimateState.Chunks);
                RefreshUltimateChunkList();
            }
        }
        private static bool DetectEvSplMode(string fileName, int count, int fileSize)
        {
            string lower = (fileName ?? "").ToLowerInvariant();
            if (lower.Contains("_ev_spl")) return true;
            if (lower.EndsWith("_ev.xfbin")) return false;
            return count > 0 && fileSize == 2 + (count * EvSplEntrySize);
        }

        private static void OpenBattleEntries(byte[] bytes, int fileSection, int count, List<BattleEntry> target)
        {
            int dataStart = fileSection + FileSectionHeaderSize;
            int maxEntries = Math.Max(0, (bytes.Length - (dataStart + DataHeaderSize) - TailSize) / EvEntrySize);
            if (count < 0) count = 0;
            if (count > maxEntries) count = maxEntries;
            for (int i = 0; i < count; i++)
            {
                int ptr = dataStart + DataHeaderSize + (i * EvEntrySize);
                if (ptr + EvEntrySize > bytes.Length) break;
                target.Add(new BattleEntry
                {
                    OriginalOffset = ptr,
                    SoundPL = ReadFixedString(bytes, ptr, StringLen),
                    Pan = BitConverter.ToInt16(bytes, ptr + 32),
                    Volume = BitConverter.ToSingle(bytes, ptr + 34),
                    Pitch = BitConverter.ToInt16(bytes, ptr + 38),
                    EventType = BitConverter.ToSingle(bytes, ptr + 40),
                    SoundDelay = BitConverter.ToInt16(bytes, ptr + 44),
                    HighPassFilter = BitConverter.ToSingle(bytes, ptr + 46),
                    LowPassFilter = BitConverter.ToSingle(bytes, ptr + 50),
                    SoundXfbin = ReadFixedString(bytes, ptr + 54, StringLen),
                    AnimationName = ReadFixedString(bytes, ptr + 86, StringLen),
                    Hitbox = ReadFixedString(bytes, ptr + 118, StringLen),
                    LocationX = BitConverter.ToSingle(bytes, ptr + 150),
                    LocationY = BitConverter.ToSingle(bytes, ptr + 154),
                    LocationZ = BitConverter.ToSingle(bytes, ptr + 158),
                    SoundDelayReset = BitConverter.ToInt16(bytes, ptr + 162),
                    Unknown = BitConverter.ToInt16(bytes, ptr + 164),
                    SoundDelayResetAnm = BitConverter.ToInt16(bytes, ptr + 166),
                    LoopFlag = BitConverter.ToInt16(bytes, ptr + 168) != 0,
                    PL_ANM_String = ReadFixedString(bytes, ptr + 170, StringLen)
                });
            }
        }

        private static void OpenUltimateChunks(byte[] bytes, int firstFileSection, List<UltimateChunk> target)
        {
            List<string> pathList = XfbinParser.GetPathList(bytes);
            List<int> chunkOffsets = GetUltimateChunkOffsets(bytes, firstFileSection);

            int totalChunks = Math.Max(pathList.Count, chunkOffsets.Count);
            for (int chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++)
            {
                string chunkName = chunkIndex < pathList.Count ? Path.GetFileNameWithoutExtension(pathList[chunkIndex]) : "payload_" + (chunkIndex + 1);
                int chunkOffset = chunkIndex < chunkOffsets.Count ? chunkOffsets[chunkIndex] : 0;
                short count = chunkOffset > 0 ? BitConverter.ToInt16(bytes, chunkOffset + 0x28) : (short)0;
                int entryStart = chunkOffset > 0 ? chunkOffset + FileSectionHeaderSize + DataHeaderSize : 0;
                UltimateChunk chunk = new UltimateChunk { Name = chunkName, FileSectionOffset = chunkOffset, OriginalEntryCount = count };
                if (chunkOffset > 0 && chunkOffset + FileSectionHeaderSize + DataHeaderSize <= bytes.Length)
                {
                    chunk.SectionTemplate = new byte[FileSectionHeaderSize + DataHeaderSize];
                    Array.Copy(bytes, chunkOffset, chunk.SectionTemplate, 0, chunk.SectionTemplate.Length);
                    int sizeB = ReadInt32BE(bytes, chunkOffset + FileSectionSizeOffsetB);
                    int sectionEnd = chunkOffset + FileSectionHeaderSize + sizeB;
                    int nextOffset = chunkIndex + 1 < chunkOffsets.Count ? chunkOffsets[chunkIndex + 1] : sectionEnd;
                    int gapLength = Math.Max(0, nextOffset - sectionEnd);
                    if (gapLength > 0 && sectionEnd + gapLength <= bytes.Length)
                    {
                        chunk.GapBytes = new byte[gapLength];
                        Array.Copy(bytes, sectionEnd, chunk.GapBytes, 0, gapLength);
                    }
                }
                for (int entryIndex = 0; entryIndex < count; entryIndex++)
                {
                    int ptr = entryStart + (entryIndex * EvSplEntrySize);
                    if (ptr + EvSplEntrySize > bytes.Length) break;
                    chunk.Entries.Add(new UltimateEntry
                    {
                        OriginalOffset = ptr,
                        SoundPL = ReadFixedString(bytes, ptr, StringLen),
                        Pan = BitConverter.ToInt16(bytes, ptr + 32),
                        Volume = BitConverter.ToSingle(bytes, ptr + 34),
                        Pitch = BitConverter.ToInt16(bytes, ptr + 38),
                        EventType = BitConverter.ToSingle(bytes, ptr + 40),
                        SoundDelay = BitConverter.ToInt16(bytes, ptr + 44),
                        HighPassFilter = BitConverter.ToSingle(bytes, ptr + 46),
                        LowPassFilter = BitConverter.ToSingle(bytes, ptr + 50),
                        Index = BitConverter.ToInt16(bytes, ptr + 54),
                        SoundCutframe = BitConverter.ToInt16(bytes, ptr + 56),
                        Unknown = BitConverter.ToInt16(bytes, ptr + 58),
                        FadeParam = BitConverter.ToSingle(bytes, ptr + 60)
                    });
                }
                target.Add(chunk);
            }
        }

        private void RefreshBattleList()
        {
            int previousTopIndex = battleListBox.Items.Count > 0 ? battleListBox.TopIndex : 0;
            int previousSelectedIndex = battleListBox.SelectedIndex;
            suppressBattleSelectionEvents = true;
            battleListBox.Items.Clear();
            foreach (BattleEntry entry in battleState.Entries) battleListBox.Items.Add(BuildBattleListLabel(entry));
            if (battleState.Entries.Count == 0)
            {
                battleListBox.Items.Add("No entries found...");
                battleListBox.SelectedIndex = -1;
                ClearBattleEditorFields();
            }
            else
            {
                int selectedIndex = previousSelectedIndex;
                if (selectedIndex < 0 || selectedIndex >= battleState.Entries.Count) selectedIndex = 0;
                battleListBox.SelectedIndex = selectedIndex;
                battleListBox.TopIndex = Math.Min(previousTopIndex, battleListBox.Items.Count - 1);
            }
            suppressBattleSelectionEvents = false;
            if (battleState.Entries.Count > 0 && battleListBox.SelectedIndex >= 0) LoadBattleEntryToEditor(battleState.Entries[battleListBox.SelectedIndex]);
        }

        private void RefreshUltimateChunkList()
        {
            suppressUltimateSelectionEvents = true;
            ultimateChunkListBox.Items.Clear();
            foreach (UltimateChunk chunk in ultimateState.Chunks) ultimateChunkListBox.Items.Add(chunk.Name);
            if (ultimateState.Chunks.Count == 0)
            {
                ultimateChunkListBox.Items.Add("No chunks found...");
                ultimateEntryListBox.Items.Clear();
                ultimateEntryListBox.Items.Add("No entries found...");
                ultimateChunkListBox.SelectedIndex = -1;
                ultimateEntryListBox.SelectedIndex = -1;
                ClearUltimateEditorFields();
                suppressUltimateSelectionEvents = false;
                return;
            }
            ultimateChunkListBox.SelectedIndex = 0;
            suppressUltimateSelectionEvents = false;
            RefreshUltimateEntryList();
        }

        private void RefreshUltimateEntryList()
        {
            int previousTopIndex = ultimateEntryListBox.Items.Count > 0 ? ultimateEntryListBox.TopIndex : 0;
            int previousSelectedIndex = ultimateEntryListBox.SelectedIndex;
            suppressUltimateSelectionEvents = true;
            ultimateEntryListBox.Items.Clear();
            UltimateChunk chunk = GetSelectedUltimateChunk();
            if (chunk == null)
            {
                ultimateEntryListBox.Items.Add("No entries found...");
                ultimateEntryListBox.SelectedIndex = -1;
                ClearUltimateEditorFields();
                suppressUltimateSelectionEvents = false;
                return;
            }
            foreach (UltimateEntry entry in chunk.Entries) ultimateEntryListBox.Items.Add(BuildUltimateListLabel(entry));
            if (chunk.Entries.Count == 0)
            {
                ultimateEntryListBox.Items.Add("No entries found...");
                ultimateEntryListBox.SelectedIndex = -1;
                ClearUltimateEditorFields();
            }
            else
            {
                int selectedIndex = previousSelectedIndex;
                if (selectedIndex < 0 || selectedIndex >= chunk.Entries.Count) selectedIndex = 0;
                ultimateEntryListBox.SelectedIndex = selectedIndex;
                ultimateEntryListBox.TopIndex = Math.Min(previousTopIndex, ultimateEntryListBox.Items.Count - 1);
            }
            suppressUltimateSelectionEvents = false;
            if (chunk.Entries.Count > 0 && ultimateEntryListBox.SelectedIndex >= 0) LoadUltimateEntryToEditor(chunk.Entries[ultimateEntryListBox.SelectedIndex]);
        }

        private static string BuildBattleListLabel(BattleEntry entry)
        {
            string plAnm = string.IsNullOrWhiteSpace(entry.PL_ANM_String) ? "(none)" : entry.PL_ANM_String;
            string animationName = string.IsNullOrWhiteSpace(entry.AnimationName) ? "(none)" : entry.AnimationName;
            return plAnm + " | " + animationName;
        }

        private static string BuildUltimateListLabel(UltimateEntry entry)
        {
            string soundName = string.IsNullOrWhiteSpace(entry.SoundPL) ? "(none)" : entry.SoundPL;
            return "Frame: " + entry.SoundDelay + " | " + soundName;
        }

        private void ClearBattleEditorFields()
        {
            battleSoundPlText.Text = "";
            battlePanValue.Value = 0;
            battleVolumeValue.Value = 0;
            battlePitchValue.Value = 0;
            battleEventTypeValue.SelectedIndex = battleEventTypeValue.Items.Count > 0 ? 0 : -1;
            battleSoundDelayValue.Value = 0;
            battleHighPassValue.Value = 0;
            battleLowPassValue.Value = 0;
            battleSoundXfbinText.Text = "";
            battleAnimationNameText.Text = "";
            battleHitboxText.Text = "";
            battleLocXValue.Value = 0;
            battleLocYValue.Value = 0;
            battleLocZValue.Value = 0;
            battleSoundDelayResetValue.Value = 0;
            battleUnknownValue.Value = 0;
            battleSoundDelayResetAnmValue.Value = 0;
            battleLoopFlagValue.Checked = false;
            battlePlAnmText.Text = "";
        }

        private void ClearUltimateEditorFields()
        {
            ultimateSoundPlText.Text = "";
            ultimatePanValue.Value = 0;
            ultimateVolumeValue.Value = 0;
            ultimatePitchValue.Value = 0;
            ultimateEventTypeValue.SelectedIndex = ultimateEventTypeValue.Items.Count > 0 ? 0 : -1;
            ultimateSoundDelayValue.Value = 0;
            ultimateHighPassValue.Value = 0;
            ultimateLowPassValue.Value = 0;
            ultimateIndexValue.Value = 0;
            ultimateSoundCutframeValue.Value = 0;
            ultimateUnknownValue.Value = 0;
            ultimateFadeParamValue.Value = 0;
        }
        private void LoadBattleEntryToEditor(BattleEntry entry)
        {
            battleSoundPlText.Text = entry.SoundPL;
            battlePanValue.Value = ClampToRange(battlePanValue, entry.Pan);
            battleVolumeValue.Value = ClampToRange(battleVolumeValue, (decimal)entry.Volume);
            battlePitchValue.Value = ClampToRange(battlePitchValue, entry.Pitch);
            SetEventType(battleEventTypeValue, entry.EventType);
            battleSoundDelayValue.Value = ClampToRange(battleSoundDelayValue, entry.SoundDelay);
            battleHighPassValue.Value = ClampToRange(battleHighPassValue, (decimal)entry.HighPassFilter);
            battleLowPassValue.Value = ClampToRange(battleLowPassValue, (decimal)entry.LowPassFilter);
            battleSoundXfbinText.Text = entry.SoundXfbin;
            battleAnimationNameText.Text = entry.AnimationName;
            battleHitboxText.Text = entry.Hitbox;
            battleLocXValue.Value = ClampToRange(battleLocXValue, (decimal)entry.LocationX);
            battleLocYValue.Value = ClampToRange(battleLocYValue, (decimal)entry.LocationY);
            battleLocZValue.Value = ClampToRange(battleLocZValue, (decimal)entry.LocationZ);
            battleSoundDelayResetValue.Value = ClampToRange(battleSoundDelayResetValue, entry.SoundDelayReset);
            battleUnknownValue.Value = ClampToRange(battleUnknownValue, entry.Unknown);
            battleSoundDelayResetAnmValue.Value = ClampToRange(battleSoundDelayResetAnmValue, entry.SoundDelayResetAnm);
            battleLoopFlagValue.Checked = entry.LoopFlag;
            battlePlAnmText.Text = entry.PL_ANM_String;
        }

        private void LoadUltimateEntryToEditor(UltimateEntry entry)
        {
            ultimateSoundPlText.Text = entry.SoundPL;
            ultimatePanValue.Value = ClampToRange(ultimatePanValue, entry.Pan);
            ultimateVolumeValue.Value = ClampToRange(ultimateVolumeValue, (decimal)entry.Volume);
            ultimatePitchValue.Value = ClampToRange(ultimatePitchValue, entry.Pitch);
            SetEventType(ultimateEventTypeValue, entry.EventType);
            ultimateSoundDelayValue.Value = ClampToRange(ultimateSoundDelayValue, entry.SoundDelay);
            ultimateHighPassValue.Value = ClampToRange(ultimateHighPassValue, (decimal)entry.HighPassFilter);
            ultimateLowPassValue.Value = ClampToRange(ultimateLowPassValue, (decimal)entry.LowPassFilter);
            ultimateIndexValue.Value = ClampToRange(ultimateIndexValue, entry.Index);
            ultimateSoundCutframeValue.Value = ClampToRange(ultimateSoundCutframeValue, entry.SoundCutframe);
            ultimateUnknownValue.Value = ClampToRange(ultimateUnknownValue, entry.Unknown);
            ultimateFadeParamValue.Value = ClampToRange(ultimateFadeParamValue, (decimal)entry.FadeParam);
        }

        private static decimal ClampToRange(NumericUpDown control, decimal value)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }

        private void SetEventType(ComboBox comboBox, float value)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                EventTypeOption option = comboBox.Items[i] as EventTypeOption;
                if (option != null && option.Value == value)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
            comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1;
        }

        private float GetEventType(ComboBox comboBox)
        {
            EventTypeOption option = comboBox.SelectedItem as EventTypeOption;
            return option != null ? option.Value : 0.0f;
        }

        private BattleEntry BuildBattleEntryFromInputs(BattleEntry existing)
        {
            return new BattleEntry
            {
                OriginalOffset = existing != null ? existing.OriginalOffset : 0,
                SoundPL = battleSoundPlText.Text.Trim(),
                Pan = (short)battlePanValue.Value,
                Volume = (float)battleVolumeValue.Value,
                Pitch = (short)battlePitchValue.Value,
                EventType = GetEventType(battleEventTypeValue),
                SoundDelay = (short)battleSoundDelayValue.Value,
                HighPassFilter = (float)battleHighPassValue.Value,
                LowPassFilter = (float)battleLowPassValue.Value,
                SoundXfbin = battleSoundXfbinText.Text.Trim(),
                AnimationName = battleAnimationNameText.Text.Trim(),
                Hitbox = battleHitboxText.Text.Trim(),
                LocationX = (float)battleLocXValue.Value,
                LocationY = (float)battleLocYValue.Value,
                LocationZ = (float)battleLocZValue.Value,
                SoundDelayReset = (short)battleSoundDelayResetValue.Value,
                Unknown = (short)battleUnknownValue.Value,
                SoundDelayResetAnm = (short)battleSoundDelayResetAnmValue.Value,
                LoopFlag = battleLoopFlagValue.Checked,
                PL_ANM_String = battlePlAnmText.Text.Trim()
            };
        }

        private UltimateEntry BuildUltimateEntryFromInputs(UltimateEntry existing)
        {
            return new UltimateEntry
            {
                OriginalOffset = existing != null ? existing.OriginalOffset : 0,
                SoundPL = ultimateSoundPlText.Text.Trim(),
                Pan = (short)ultimatePanValue.Value,
                Volume = (float)ultimateVolumeValue.Value,
                Pitch = (short)ultimatePitchValue.Value,
                EventType = GetEventType(ultimateEventTypeValue),
                SoundDelay = (short)ultimateSoundDelayValue.Value,
                HighPassFilter = (float)ultimateHighPassValue.Value,
                LowPassFilter = (float)ultimateLowPassValue.Value,
                Index = (short)ultimateIndexValue.Value,
                SoundCutframe = (short)ultimateSoundCutframeValue.Value,
                Unknown = (short)ultimateUnknownValue.Value,
                FadeParam = (float)ultimateFadeParamValue.Value
            };
        }

        private void UpdateBattleSelected()
        {
            int index = battleListBox.SelectedIndex;
            if (!battleState.FileOpen || index < 0 || index >= battleState.Entries.Count) { MessageBox.Show("No Battle entry selected..."); return; }
            battleState.Entries[index] = BuildBattleEntryFromInputs(battleState.Entries[index]);
            battleListBox.Items[index] = BuildBattleListLabel(battleState.Entries[index]);
        }

        private void UpdateUltimateSelected()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            int entryIndex = ultimateEntryListBox.SelectedIndex;
            if (!ultimateState.FileOpen || chunk == null || entryIndex < 0 || entryIndex >= chunk.Entries.Count) { MessageBox.Show("No Ultimate entry selected..."); return; }
            chunk.Entries[entryIndex] = BuildUltimateEntryFromInputs(chunk.Entries[entryIndex]);
            ultimateEntryListBox.Items[entryIndex] = BuildUltimateListLabel(chunk.Entries[entryIndex]);
        }

        private void AddBattleEntry()
        {
            if (!battleState.FileOpen) { MessageBox.Show("No Battle file loaded..."); return; }
            BattleEntry entry = BuildBattleEntryFromInputs(null);
            battleState.Entries.Add(entry);
            RefreshBattleList();
            battleListBox.SelectedIndex = battleState.Entries.Count - 1;
        }

        private void DuplicateBattleEntry()
        {
            int index = battleListBox.SelectedIndex;
            if (!battleState.FileOpen || index < 0 || index >= battleState.Entries.Count) { MessageBox.Show("No Battle entry selected..."); return; }
            BattleEntry source = battleState.Entries[index];
            BattleEntry copy = new BattleEntry
            {
                SoundPL = source.SoundPL,
                Pan = source.Pan,
                Volume = source.Volume,
                Pitch = source.Pitch,
                EventType = source.EventType,
                SoundDelay = source.SoundDelay,
                HighPassFilter = source.HighPassFilter,
                LowPassFilter = source.LowPassFilter,
                SoundXfbin = source.SoundXfbin,
                AnimationName = source.AnimationName,
                Hitbox = source.Hitbox,
                LocationX = source.LocationX,
                LocationY = source.LocationY,
                LocationZ = source.LocationZ,
                SoundDelayReset = source.SoundDelayReset,
                Unknown = source.Unknown,
                SoundDelayResetAnm = source.SoundDelayResetAnm,
                LoopFlag = source.LoopFlag,
                PL_ANM_String = source.PL_ANM_String
            };
            battleState.Entries.Insert(index + 1, copy);
            RefreshBattleList();
            battleListBox.SelectedIndex = index + 1;
        }

        private void DeleteBattleEntry()
        {
            int index = battleListBox.SelectedIndex;
            if (!battleState.FileOpen || index < 0 || index >= battleState.Entries.Count) { MessageBox.Show("No Battle entry selected..."); return; }
            battleState.Entries.RemoveAt(index);
            RefreshBattleList();
            if (battleState.Entries.Count > 0) battleListBox.SelectedIndex = Math.Min(index, battleState.Entries.Count - 1);
        }

        private void SortBattleEntries()
        {
            if (!battleState.FileOpen || battleState.Entries.Count == 0) return;
            battleState.Entries.Sort((left, right) => string.Compare(left.PL_ANM_String, right.PL_ANM_String, StringComparison.OrdinalIgnoreCase));
            RefreshBattleList();
        }

        private void CopyBattleEntryToClipboard()
        {
            int index = battleListBox.SelectedIndex;
            if (!battleState.FileOpen || index < 0 || index >= battleState.Entries.Count) { MessageBox.Show("No Battle entry selected..."); return; }
            BattleEntry entry = battleState.Entries[index];
            Clipboard.SetText(string.Join("\n", new string[]
            {
                BattleClipboardPrefix,
                entry.SoundPL ?? "",
                entry.Pan.ToString(),
                entry.Volume.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.Pitch.ToString(),
                entry.EventType.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.SoundDelay.ToString(),
                entry.HighPassFilter.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.LowPassFilter.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.SoundXfbin ?? "",
                entry.AnimationName ?? "",
                entry.Hitbox ?? "",
                entry.LocationX.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.LocationY.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.LocationZ.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.SoundDelayReset.ToString(),
                entry.Unknown.ToString(),
                entry.SoundDelayResetAnm.ToString(),
                entry.LoopFlag ? "1" : "0",
                entry.PL_ANM_String ?? ""
            }));
        }

        private void PasteBattleEntryFromClipboard()
        {
            if (!battleState.FileOpen) { MessageBox.Show("No Battle file loaded..."); return; }
            if (!Clipboard.ContainsText()) { MessageBox.Show("Clipboard does not contain Battle entry data."); return; }
            string[] lines = Clipboard.GetText().Replace("\r\n", "\n").Split('\n');
            if (lines.Length < 20 || lines[0] != BattleClipboardPrefix) { MessageBox.Show("Clipboard does not contain Battle entry data."); return; }
            BattleEntry entry = new BattleEntry
            {
                SoundPL = lines[1],
                Pan = ParseInt16(lines[2]),
                Volume = ParseSingle(lines[3]),
                Pitch = ParseInt16(lines[4]),
                EventType = ParseSingle(lines[5]),
                SoundDelay = ParseInt16(lines[6]),
                HighPassFilter = ParseSingle(lines[7]),
                LowPassFilter = ParseSingle(lines[8]),
                SoundXfbin = lines[9],
                AnimationName = lines[10],
                Hitbox = lines[11],
                LocationX = ParseSingle(lines[12]),
                LocationY = ParseSingle(lines[13]),
                LocationZ = ParseSingle(lines[14]),
                SoundDelayReset = ParseInt16(lines[15]),
                Unknown = ParseInt16(lines[16]),
                SoundDelayResetAnm = ParseInt16(lines[17]),
                LoopFlag = lines[18] == "1",
                PL_ANM_String = lines[19]
            };
            battleState.Entries.Add(entry);
            RefreshBattleList();
            battleListBox.SelectedIndex = battleState.Entries.Count - 1;
        }

        private void SortUltimateEntries()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            if (!ultimateState.FileOpen || chunk == null || chunk.Entries.Count == 0) return;
            chunk.Entries.Sort((left, right) => left.SoundDelay.CompareTo(right.SoundDelay));
            ReindexUltimateChunk(chunk);
            RefreshUltimateEntryList();
        }

        private void ReindexUltimateChunk(UltimateChunk chunk)
        {
            for (short i = 0; i < chunk.Entries.Count; i++) chunk.Entries[i].Index = i;
        }

        private void CopyUltimateEntryToClipboard()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            int entryIndex = ultimateEntryListBox.SelectedIndex;
            if (!ultimateState.FileOpen || chunk == null || entryIndex < 0 || entryIndex >= chunk.Entries.Count) { MessageBox.Show("No Ultimate entry selected..."); return; }
            UltimateEntry entry = chunk.Entries[entryIndex];
            Clipboard.SetText(string.Join("\n", new string[]
            {
                UltimateClipboardPrefix,
                entry.SoundPL ?? "",
                entry.Pan.ToString(),
                entry.Volume.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.Pitch.ToString(),
                entry.EventType.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.SoundDelay.ToString(),
                entry.HighPassFilter.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.LowPassFilter.ToString(System.Globalization.CultureInfo.InvariantCulture),
                entry.Index.ToString(),
                entry.SoundCutframe.ToString(),
                entry.Unknown.ToString(),
                entry.FadeParam.ToString(System.Globalization.CultureInfo.InvariantCulture)
            }));
        }

        private void PasteUltimateEntryFromClipboard()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            if (!ultimateState.FileOpen || chunk == null) { MessageBox.Show("No Ultimate chunk selected..."); return; }
            if (!Clipboard.ContainsText()) { MessageBox.Show("Clipboard does not contain Ultimate entry data."); return; }
            string[] lines = Clipboard.GetText().Replace("\r\n", "\n").Split('\n');
            if (lines.Length < 13 || lines[0] != UltimateClipboardPrefix) { MessageBox.Show("Clipboard does not contain Ultimate entry data."); return; }
            UltimateEntry entry = new UltimateEntry
            {
                SoundPL = lines[1],
                Pan = ParseInt16(lines[2]),
                Volume = ParseSingle(lines[3]),
                Pitch = ParseInt16(lines[4]),
                EventType = ParseSingle(lines[5]),
                SoundDelay = ParseInt16(lines[6]),
                HighPassFilter = ParseSingle(lines[7]),
                LowPassFilter = ParseSingle(lines[8]),
                Index = ParseInt16(lines[9]),
                SoundCutframe = ParseInt16(lines[10]),
                Unknown = ParseInt16(lines[11]),
                FadeParam = ParseSingle(lines[12])
            };
            chunk.Entries.Add(entry);
            ReindexUltimateChunk(chunk);
            RefreshUltimateEntryList();
            ultimateEntryListBox.SelectedIndex = chunk.Entries.Count - 1;
        }

        private void AddUltimateEntry()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            if (!ultimateState.FileOpen || chunk == null) { MessageBox.Show("No Ultimate chunk selected..."); return; }
            UltimateEntry entry = new UltimateEntry { SoundPL = GetUltimateDefaultSoundPl(chunk.Name), EventType = 0.0f };
            chunk.Entries.Add(entry);
            ReindexUltimateChunk(chunk);
            RefreshUltimateEntryList();
            ultimateEntryListBox.SelectedIndex = chunk.Entries.Count - 1;
        }

        private string GetUltimateCharacterCode()
        {
            string fileName = Path.GetFileNameWithoutExtension(ultimateState.FilePath ?? "");
            if (string.IsNullOrWhiteSpace(fileName)) return "";
            int suffixIndex = fileName.IndexOf("_ev_spl", StringComparison.OrdinalIgnoreCase);
            if (suffixIndex > 0) return fileName.Substring(0, suffixIndex);
            suffixIndex = fileName.IndexOf("_ev", StringComparison.OrdinalIgnoreCase);
            if (suffixIndex > 0) return fileName.Substring(0, suffixIndex);
            return fileName;
        }

        private string GetUltimateDefaultSoundPl(string chunkName)
        {
            string prefix = GetUltimateCharacterCode();
            if (string.IsNullOrWhiteSpace(prefix)) return chunkName ?? "";
            if (string.IsNullOrWhiteSpace(chunkName)) return prefix;
            return prefix + chunkName;
        }
        private void DuplicateUltimateEntry()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            int entryIndex = ultimateEntryListBox.SelectedIndex;
            if (!ultimateState.FileOpen || chunk == null || entryIndex < 0 || entryIndex >= chunk.Entries.Count) { MessageBox.Show("No Ultimate entry selected..."); return; }
            UltimateEntry source = chunk.Entries[entryIndex];
            UltimateEntry copy = new UltimateEntry
            {
                SoundPL = source.SoundPL,
                Pan = source.Pan,
                Volume = source.Volume,
                Pitch = source.Pitch,
                EventType = source.EventType,
                SoundDelay = source.SoundDelay,
                HighPassFilter = source.HighPassFilter,
                LowPassFilter = source.LowPassFilter,
                Index = source.Index,
                SoundCutframe = source.SoundCutframe,
                Unknown = source.Unknown,
                FadeParam = source.FadeParam
            };
            chunk.Entries.Insert(entryIndex + 1, copy);
            ReindexUltimateChunk(chunk);
            RefreshUltimateEntryList();
            ultimateEntryListBox.SelectedIndex = entryIndex + 1;
        }

        private void DeleteUltimateEntry()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            int entryIndex = ultimateEntryListBox.SelectedIndex;
            if (!ultimateState.FileOpen || chunk == null || entryIndex < 0 || entryIndex >= chunk.Entries.Count) { MessageBox.Show("No Ultimate entry selected..."); return; }
            chunk.Entries.RemoveAt(entryIndex);
            ReindexUltimateChunk(chunk);
            RefreshUltimateEntryList();
            if (chunk.Entries.Count > 0) ultimateEntryListBox.SelectedIndex = Math.Min(entryIndex, chunk.Entries.Count - 1);
        }

        private bool BattleEntryContainsSearch(BattleEntry entry, string search)
        {
            return ContainsText(entry.SoundPL, search) || ContainsText(entry.SoundXfbin, search) || ContainsText(entry.AnimationName, search) || ContainsText(entry.Hitbox, search) || ContainsText(entry.PL_ANM_String, search);
        }

        private bool UltimateEntryContainsSearch(UltimateChunk chunk, UltimateEntry entry, string search)
        {
            return ContainsText(chunk.Name, search) || ContainsText(entry.SoundPL, search);
        }

        private static bool ContainsText(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void SearchBattleNext()
        {
            if (!battleState.FileOpen || battleState.Entries.Count == 0) { MessageBox.Show("No Battle file loaded..."); return; }
            string search = battleSearchText.Text.Trim();
            if (search == "") { MessageBox.Show("Write search text first."); return; }
            int startIndex = battleListBox.SelectedIndex;
            if (startIndex < 0) startIndex = -1;
            for (int i = startIndex + 1; i < battleState.Entries.Count; i++) if (BattleEntryContainsSearch(battleState.Entries[i], search)) { battleListBox.SelectedIndex = i; return; }
            for (int i = 0; i <= startIndex && i < battleState.Entries.Count; i++) if (BattleEntryContainsSearch(battleState.Entries[i], search)) { battleListBox.SelectedIndex = i; return; }
            MessageBox.Show("No entry contains that string.");
        }

        private void SearchUltimateNext()
        {
            UltimateChunk chunk = GetSelectedUltimateChunk();
            if (!ultimateState.FileOpen || chunk == null || chunk.Entries.Count == 0) { MessageBox.Show("No Ultimate file loaded..."); return; }
            string search = ultimateSearchText.Text.Trim();
            if (search == "") { MessageBox.Show("Write search text first."); return; }
            int startIndex = ultimateEntryListBox.SelectedIndex;
            if (startIndex < 0) startIndex = -1;
            for (int i = startIndex + 1; i < chunk.Entries.Count; i++) if (UltimateEntryContainsSearch(chunk, chunk.Entries[i], search)) { ultimateEntryListBox.SelectedIndex = i; return; }
            for (int i = 0; i <= startIndex && i < chunk.Entries.Count; i++) if (UltimateEntryContainsSearch(chunk, chunk.Entries[i], search)) { ultimateEntryListBox.SelectedIndex = i; return; }
            MessageBox.Show("No entry contains that string.");
        }

        private UltimateChunk GetSelectedUltimateChunk()
        {
            int chunkIndex = ultimateChunkListBox.SelectedIndex;
            if (chunkIndex < 0 || chunkIndex >= ultimateState.Chunks.Count) return null;
            return ultimateState.Chunks[chunkIndex];
        }

        private void SaveBattleFile(bool saveAs)
        {
            if (!battleState.FileOpen) { MessageBox.Show("No Battle file loaded..."); return; }
            string outputPath = battleState.FilePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = ".xfbin";
                    dialog.Filter = "Battle EV (*.xfbin)|*.xfbin";
                    dialog.FileName = Path.GetFileName(battleState.FilePath);
                    if (dialog.ShowDialog() != DialogResult.OK) return;
                    outputPath = dialog.FileName;
                }
            }
            byte[] output = ConvertBattleToFile();
            File.WriteAllBytes(outputPath, output);
            battleState.FilePath = outputPath;
            battleState.FileBytes = output;
            battleState.FileSectionOffset = XfbinParser.GetFileSectionIndex(output);
        }

        private void SaveUltimateFile(bool saveAs)
        {
            if (!ultimateState.FileOpen) { MessageBox.Show("No Ultimate file loaded..."); return; }
            string outputPath = ultimateState.FilePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = ".xfbin";
                    dialog.Filter = "Ultimate EV SPL (*.xfbin)|*.xfbin";
                    dialog.FileName = Path.GetFileName(ultimateState.FilePath);
                    if (dialog.ShowDialog() != DialogResult.OK) return;
                    outputPath = dialog.FileName;
                }
            }
            foreach (UltimateChunk chunk in ultimateState.Chunks)
            {
                if (chunk.FileSectionOffset <= 0 || chunk.OriginalEntryCount != chunk.Entries.Count)
                {
                    MessageBox.Show("Ultimate save currently supports editing existing binary structures only. Added or removed structures or entry count changes cannot be written yet.");
                    return;
                }
            }
            byte[] output = ConvertUltimateToFile();
            File.WriteAllBytes(outputPath, output);
            ultimateState.FilePath = outputPath;
            ultimateState.FileBytes = output;
        }

        private byte[] ConvertBattleToFile()
        {
            if (battleState.FileBytes == null || battleState.FileBytes.Length == 0) return new byte[0];
            int originalSectionOffset = battleState.FileSectionOffset;
            int prefixLength = originalSectionOffset + BattleSectionPreludeSize;
            byte[] prefix = new byte[prefixLength];
            Array.Copy(battleState.FileBytes, 0, prefix, 0, prefix.Length);
            int originalEntryStart = originalSectionOffset + BattleSectionPreludeSize;
            int originalCount = BitConverter.ToInt16(battleState.FileBytes, originalSectionOffset + 0x28);
            int originalSuffixStart = originalEntryStart + (Math.Max(0, originalCount) * EvEntrySize);
            if (originalSuffixStart > battleState.FileBytes.Length) originalSuffixStart = battleState.FileBytes.Length;
            byte[] suffix = new byte[battleState.FileBytes.Length - originalSuffixStart];
            Array.Copy(battleState.FileBytes, originalSuffixStart, suffix, 0, suffix.Length);
            int entryBytesLength = battleState.Entries.Count * EvEntrySize;
            int newDataSize = 2 + entryBytesLength;
            int newSizeB = newDataSize + 4;
            WriteInt32BE(prefix, originalSectionOffset + FileSectionSizeOffsetB, newSizeB);
            WriteInt32BE(prefix, originalSectionOffset + 0x24, newDataSize);
            WriteInt16(prefix, originalSectionOffset + 0x28, (short)battleState.Entries.Count);
            byte[] body = new byte[entryBytesLength];
            for (int i = 0; i < battleState.Entries.Count; i++)
            {
                int ptr = i * EvEntrySize;
                WriteBattleEntry(body, ptr, battleState.Entries[i]);
                battleState.Entries[i].OriginalOffset = originalEntryStart + (i * EvEntrySize);
            }
            byte[] result = new byte[prefix.Length + body.Length + suffix.Length];
            Array.Copy(prefix, 0, result, 0, prefix.Length);
            Array.Copy(body, 0, result, prefix.Length, body.Length);
            Array.Copy(suffix, 0, result, prefix.Length + body.Length, suffix.Length);
            return result;
        }

        private byte[] ConvertUltimateToFile()
        {
            byte[] output = new byte[ultimateState.FileBytes.Length];
            Array.Copy(ultimateState.FileBytes, 0, output, 0, output.Length);
            foreach (UltimateChunk chunk in ultimateState.Chunks)
            {
                chunk.Entries.Sort((left, right) => left.SoundDelay.CompareTo(right.SoundDelay));
                ReindexUltimateChunk(chunk);
                for (int i = 0; i < chunk.Entries.Count; i++) WriteUltimateEntry(output, chunk.Entries[i].OriginalOffset, chunk.Entries[i]);
            }
            return output;
        }

        private static List<int> GetUltimateChunkOffsets(byte[] bytes, int firstFileSection)
        {
            List<int> chunkOffsets = new List<int>();
            for (int offset = Math.Max(0, firstFileSection); offset <= bytes.Length - (FileSectionHeaderSize + DataHeaderSize); offset++)
            {
                int dataSize = ReadInt32BE(bytes, offset + 0x24);
                int sizeB = ReadInt32BE(bytes, offset + FileSectionSizeOffsetB);
                short count = BitConverter.ToInt16(bytes, offset + 0x28);
                if (count <= 0) continue;
                if (dataSize != 2 + (count * EvSplEntrySize)) continue;
                if (sizeB != dataSize + 4) continue;
                chunkOffsets.Add(offset);
            }
            return chunkOffsets;
        }

        private static void WriteBattleEntry(byte[] target, int offset, BattleEntry entry)
        {
            WriteFixedString(target, offset, StringLen, entry.SoundPL);
            WriteInt16(target, offset + 32, entry.Pan);
            WriteSingle(target, offset + 34, entry.Volume);
            WriteInt16(target, offset + 38, entry.Pitch);
            WriteSingle(target, offset + 40, entry.EventType);
            WriteInt16(target, offset + 44, entry.SoundDelay);
            WriteSingle(target, offset + 46, entry.HighPassFilter);
            WriteSingle(target, offset + 50, entry.LowPassFilter);
            WriteFixedString(target, offset + 54, StringLen, entry.SoundXfbin);
            WriteFixedString(target, offset + 86, StringLen, entry.AnimationName);
            WriteFixedString(target, offset + 118, StringLen, entry.Hitbox);
            WriteSingle(target, offset + 150, entry.LocationX);
            WriteSingle(target, offset + 154, entry.LocationY);
            WriteSingle(target, offset + 158, entry.LocationZ);
            WriteInt16(target, offset + 162, entry.SoundDelayReset);
            WriteInt16(target, offset + 164, entry.Unknown);
            WriteInt16(target, offset + 166, entry.SoundDelayResetAnm);
            WriteInt16(target, offset + 168, (short)(entry.LoopFlag ? 1 : 0));
            WriteFixedString(target, offset + 170, StringLen, entry.PL_ANM_String);
        }

        private static void WriteUltimateEntry(byte[] target, int offset, UltimateEntry entry)
        {
            WriteFixedString(target, offset, StringLen, entry.SoundPL);
            WriteInt16(target, offset + 32, entry.Pan);
            WriteSingle(target, offset + 34, entry.Volume);
            WriteInt16(target, offset + 38, entry.Pitch);
            WriteSingle(target, offset + 40, entry.EventType);
            WriteInt16(target, offset + 44, entry.SoundDelay);
            WriteSingle(target, offset + 46, entry.HighPassFilter);
            WriteSingle(target, offset + 50, entry.LowPassFilter);
            WriteInt16(target, offset + 54, entry.Index);
            WriteInt16(target, offset + 56, entry.SoundCutframe);
            WriteInt16(target, offset + 58, entry.Unknown);
            WriteSingle(target, offset + 60, entry.FadeParam);
        }

        private static short ParseInt16(string value)
        {
            short result;
            return short.TryParse(value, out result) ? result : (short)0;
        }

        private static float ParseSingle(string value)
        {
            float result;
            return float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result) ? result : 0.0f;
        }

        private void battleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressBattleSelectionEvents) return;
            int index = battleListBox.SelectedIndex;
            if (index >= 0 && index < battleState.Entries.Count) LoadBattleEntryToEditor(battleState.Entries[index]);
        }

        private void ultimateChunkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressUltimateSelectionEvents) return;
            RefreshUltimateEntryList();
        }

        private void ultimateEntryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressUltimateSelectionEvents) return;
            UltimateChunk chunk = GetSelectedUltimateChunk();
            int index = ultimateEntryListBox.SelectedIndex;
            if (chunk != null && index >= 0 && index < chunk.Entries.Count) LoadUltimateEntryToEditor(chunk.Entries[index]);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) { OpenActiveFile(); }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { SaveActiveFile(); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) { SaveActiveFileAs(); }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) { CloseActiveFile(); }
        private void sortToolStripMenuItem_Click(object sender, EventArgs e) { if (IsBattleTabSelected()) SortBattleEntries(); else SortUltimateEntries(); }
        private void battleAddButton_Click(object sender, EventArgs e) { AddBattleEntry(); }
        private void battleDuplicateButton_Click(object sender, EventArgs e) { DuplicateBattleEntry(); }
        private void battleDeleteButton_Click(object sender, EventArgs e) { DeleteBattleEntry(); }
        private void battleSaveSelectedButton_Click(object sender, EventArgs e) { UpdateBattleSelected(); }
        private void battleSearchButton_Click(object sender, EventArgs e) { SearchBattleNext(); }
        private void battleCopyButton_Click(object sender, EventArgs e) { CopyBattleEntryToClipboard(); }
        private void battlePasteButton_Click(object sender, EventArgs e) { PasteBattleEntryFromClipboard(); }
        private void ultimateAddEntryButton_Click(object sender, EventArgs e) { AddUltimateEntry(); }
        private void ultimateDuplicateEntryButton_Click(object sender, EventArgs e) { DuplicateUltimateEntry(); }
        private void ultimateDeleteEntryButton_Click(object sender, EventArgs e) { DeleteUltimateEntry(); }
        private void ultimateSaveSelectedButton_Click(object sender, EventArgs e) { UpdateUltimateSelected(); }
        private void ultimateSearchButton_Click(object sender, EventArgs e) { SearchUltimateNext(); }
        private void ultimateCopyButton_Click(object sender, EventArgs e) { CopyUltimateEntryToClipboard(); }
        private void ultimatePasteButton_Click(object sender, EventArgs e) { PasteUltimateEntryFromClipboard(); }
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ultimate save currently supports editing existing binary structures only. Added or removed structures or entry count changes cannot be written yet. Use the Xfbin Parser to add a new entry");
        }

        private void ultimateEditPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void battleLabelSoundPl_Click(object sender, EventArgs e)
        {

        }

        private void ultimateLabelFadeParam_Click(object sender, EventArgs e)
        {

        }

        private void battleLabelHitbox_Click(object sender, EventArgs e)
        {

        }

        private void battleHitboxText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

