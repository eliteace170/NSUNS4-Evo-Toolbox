using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager.Tools
{
    public partial class Tool_CpuParamEditor : Form
    {
        private readonly Dictionary<CpuParamChunkKind, CpuParamChunkState> chunks = new Dictionary<CpuParamChunkKind, CpuParamChunkState>();
        private readonly List<string> characodeIds = new List<string>();
        private bool fileOpen;
        private string filePath = string.Empty;
        private string characodeReferencePath = string.Empty;
        private bool updatingUi;
        private CpuParamChunkKind currentKind = CpuParamChunkKind.Script;

        private sealed class EnumChoice
        {
            public int Value;
            public string Label = string.Empty;

            public override string ToString()
            {
                return Label;
            }
        }

        private static readonly string[] PlayerSlotLabels =
        {
            "00 - Unknown",
            "01 - Unknown",
            "02 - Skill_1",
            "03 - SkillAir_1",
            "04 - SkillReinforce_1",
            "05 - Skill_2",
            "06 - SkillAir_2",
            "07 - SkillReinforce_2",
            "08 - Skill_3",
            "09 - SkillAir_3",
            "10 - SkillReinforce_3",
            "11 - Skill_4",
            "12 - SkillAir_4",
            "13 - SkillReinforce_4",
            "14 - SpSkill_1",
            "15 - SpSkill_2",
            "16 - SpSkill_3",
            "17 - SpSkill_4",
            "19 - Unknown",
            "20 - AwakeSkill",
            "21 - AwakeSkillAir",
            "22 - AwakeSkillReinforce",
            "23 - Unknown",
            "25 - Unknown",
            "26 - Unknown",
            "27 - Unknown",
            "28 - Unknown",
            "29 - Unknown",
            "30 - SupportL",
            "31 - SupportR"
            
        };

        public Tool_CpuParamEditor()
        {
            InitializeComponent();
            InitializeEnumControls();
            InitializePlayerSlotGrid();
            ClearFileState();
            if (File.Exists(Main.chaPath))
                LoadCharacodeReferences(Main.chaPath, false);
            TryLoadConfiguredCpuParam();
        }

        private void TryLoadConfiguredCpuParam()
        {
            if (!File.Exists(Main.cpuParamPath))
                return;

            try
            {
                LoadFile(Main.cpuParamPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "Could not open the configured cpuparam.xfbin." + Environment.NewLine + Environment.NewLine + ex.Message,
                    "CPU Param Editor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                SetStatus("Configured cpuparam.xfbin could not be opened.");
            }
        }

        private void InitializeEnumControls()
        {
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.CtrlIf, "0 - Control If");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.NextProcessIf, "1 - Next Process If");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.Skip2, "2 - Skip / Deprecated");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.CtrlSwitch, "3 - Control Switch");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.NextProcessSwitch, "4 - Next Process Switch");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.Skip5, "5 - Skip / Deprecated");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.CtrlJump, "6 - Control Jump");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.Skip7, "7 - Skip / Deprecated");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.End, "8 - End");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.CtrlCommand, "9 - Control Command");
            AddChoice(scriptTypeComboBox, (int)CpuScriptType.Unknown10, "10 - Unknown");

            CopyChoices(scriptTypeComboBox, strengthTypeComboBox);

            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.SetProb, "0 - Set Probability");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.AddProb, "1 - Add Probability");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.JudgeGauge, "2 - Judge Gauge");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.JudgeSituation, "3 - Judge Situation");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.JudgeDistance, "4 - Judge Distance");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.SetParam, "5 - Set Parameter");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.JudgeAction, "6 - Judge Action");
            AddChoice(scriptCommandComboBox, (int)CpuScriptCommand.None, "65535 - None / Control Marker");

            CopyChoices(scriptCommandComboBox, strengthCommandComboBox);

            AddChoice(playerTypeComboBox, (int)CpuPlayerType.Normal, "0 - Normal");
            AddChoice(playerTypeComboBox, (int)CpuPlayerType.Dash, "1 - Dash");
            AddChoice(playerTypeComboBox, (int)CpuPlayerType.ProjectileType, "2 - Projectile Type");
            AddChoice(playerTypeComboBox, 3, "3 - Unknown");
            AddChoice(playerTypeComboBox, 4, "4 - Unknown");
            AddChoice(playerTypeComboBox, 5, "5 - Unknown");
            AddChoice(playerTypeComboBox, 6, "6 - Unknown");
            AddChoice(playerTypeComboBox, (int)CpuPlayerType.PuppetType, "7 - Puppet Type");
            AddChoice(playerTypeComboBox, 8, "8 - Unknown");
            AddChoice(playerTypeComboBox, 9, "9 - Unknown");
            AddChoice(playerTypeComboBox, 10, "10 - Unknown");
        }

        private void InitializePlayerSlotGrid()
        {
            RefreshPlayerActionChoices();
            playerActionSlotsGrid.Rows.Clear();
            for (int i = 0; i < PlayerSlotLabels.Length; i++)
                playerActionSlotsGrid.Rows.Add(PlayerSlotLabels[i], string.Empty);
        }

        private void RefreshPlayerActionChoices()
        {
            List<string> currentCellValues = new List<string>();
            foreach (DataGridViewRow row in playerActionSlotsGrid.Rows)
            {
                string value = Convert.ToString(row.Cells[playerActionNameColumn.Index].Value);
                if (!string.IsNullOrWhiteSpace(value))
                    currentCellValues.Add(value);
            }

            playerActionNameColumn.Items.Clear();
            playerActionNameColumn.Items.Add(string.Empty);
            HashSet<string> addedNames = new HashSet<string>(StringComparer.Ordinal);

            CpuActionChunkState actionChunk = GetActionChunk();
            if (actionChunk != null)
            {
                foreach (CpuActionEntry entry in actionChunk.Entries)
                {
                    string name = entry.Name ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(name) && addedNames.Add(name))
                        playerActionNameColumn.Items.Add(name);
                }
            }

            CpuPlayerChunkState playerChunk = GetPlayerChunk();
            if (playerChunk != null)
            {
                foreach (CpuPlayerEntry entry in playerChunk.Entries)
                {
                    foreach (int slotIndex in CpuParamCodec.PlayerStringSlotIndices)
                    {
                        string name = entry.ActionSlots[slotIndex] ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(name) && addedNames.Add(name))
                            playerActionNameColumn.Items.Add(name);
                    }
                }
            }

            foreach (string name in currentCellValues)
            {
                if (addedNames.Add(name))
                    playerActionNameColumn.Items.Add(name);
            }
        }

        private static void AddChoice(ComboBox comboBox, int value, string label)
        {
            comboBox.Items.Add(new EnumChoice { Value = value, Label = label });
        }

        private static void CopyChoices(ComboBox source, ComboBox target)
        {
            foreach (EnumChoice choice in source.Items)
                target.Items.Add(new EnumChoice { Value = choice.Value, Label = choice.Label });
        }

        private static void SelectChoice(ComboBox comboBox, int value)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                EnumChoice choice = comboBox.Items[i] as EnumChoice;
                if (choice != null && choice.Value == value)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            comboBox.Items.Add(new EnumChoice { Value = value, Label = value + " - Unknown" });
            comboBox.SelectedIndex = comboBox.Items.Count - 1;
        }

        private static int GetChoice(ComboBox comboBox)
        {
            EnumChoice choice = comboBox.SelectedItem as EnumChoice;
            if (choice == null)
                throw new InvalidOperationException("Select an enum value before applying the entry.");
            return choice.Value;
        }

        private void ClearFileState()
        {
            fileOpen = false;
            filePath = string.Empty;
            chunks.Clear();
            Text = "CPU Param Editor";
            RefreshAllEditors();
            SetStatus("Open cpuparam.xfbin to begin.");
        }

        private void OpenFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = ".xfbin";
                dialog.Filter = "CPU Param XFBIN (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                dialog.Title = "Open CPU Parameter XFBIN";
                if (File.Exists(Main.cpuParamPath))
                    dialog.FileName = Main.cpuParamPath;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    LoadFile(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Could not open CPU parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("Open failed.");
                }
            }
        }

        private void OpenCharacodeReferences()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "xfbin";
                dialog.Filter = "Characode XFBIN (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                dialog.Title = "Load characode.bin.xfbin";
                if (File.Exists(characodeReferencePath))
                    dialog.FileName = characodeReferencePath;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                LoadCharacodeReferences(dialog.FileName, true);
            }
        }

        private bool LoadCharacodeReferences(string sourcePath, bool showError)
        {
            try
            {
                List<string> parsedIds = null;
                string parseError = string.Empty;
                using (XfbinParserBackend backend = new XfbinParserBackend(sourcePath))
                {
                    List<XfbinBinaryChunkItem> chunksInFile = backend.GetBinaryChunks();
                    List<XfbinBinaryChunkItem> namedCandidates = chunksInFile.Where(IsCharacodeChunk).ToList();
                    IEnumerable<XfbinBinaryChunkItem> candidates = namedCandidates.Count > 0 ? namedCandidates : chunksInFile;
                    foreach (XfbinBinaryChunkItem item in candidates)
                    {
                        List<string> ids;
                        string error;
                        if (TryParseCharacodeChunk(item.BinaryData, out ids, out error))
                        {
                            parsedIds = ids;
                            break;
                        }
                        parseError = error;
                    }
                }

                if (parsedIds == null)
                    throw new InvalidDataException("No valid characode binary chunk was found. " + parseError);

                UpdatePlayerEntry(false);
                characodeIds.Clear();
                characodeIds.AddRange(parsedIds);
                characodeReferencePath = sourcePath;

                int selectedIndex = playerEntryListBox.SelectedIndex;
                RefreshPlayerEntries(selectedIndex);
                CpuPlayerChunkState playerChunk = GetPlayerChunk();
                int matchedEntries = playerChunk == null
                    ? 0
                    : playerChunk.Entries.Count(x => IsKnownCharacodeEntry(x.Characode));
                SetStatus(string.Format(
                    "Loaded {0} character IDs from {1}; resolved {2} player entries.",
                    characodeIds.Count,
                    Path.GetFileName(sourcePath),
                    matchedEntries));
                return true;
            }
            catch (Exception ex)
            {
                if (showError)
                {
                    MessageBox.Show(
                        this,
                        "Could not load characode references." + Environment.NewLine + Environment.NewLine + ex.Message,
                        "CPU Param Editor",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return false;
            }
        }

        private static bool IsCharacodeChunk(XfbinBinaryChunkItem item)
        {
            string identity = (item.ChunkName ?? string.Empty) + "|" +
                              (item.ChunkPath ?? string.Empty) + "|" +
                              (item.FileName ?? string.Empty);
            return identity.IndexOf("characode", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool TryParseCharacodeChunk(byte[] bytes, out List<string> ids, out string error)
        {
            ids = new List<string>();
            error = string.Empty;
            if (bytes == null || bytes.Length < 8)
            {
                error = "The characode chunk is smaller than its 8-byte header.";
                return false;
            }

            uint declaredSize = ReadReferenceUInt32BE(bytes, 0);
            uint count = ReadReferenceUInt32LE(bytes, 4);
            long expectedLength = 8L + ((long)count * 8L);
            if (declaredSize != bytes.Length - 4 || expectedLength != bytes.Length)
            {
                error = "The characode size or entry count is invalid.";
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                int offset = 8 + (i * 8);
                int length = 0;
                while (length < 8 && bytes[offset + length] != 0)
                    length++;
                ids.Add(Encoding.ASCII.GetString(bytes, offset, length));
            }
            return true;
        }

        private static uint ReadReferenceUInt32BE(byte[] bytes, int offset)
        {
            return ((uint)bytes[offset] << 24) |
                   ((uint)bytes[offset + 1] << 16) |
                   ((uint)bytes[offset + 2] << 8) |
                   bytes[offset + 3];
        }

        private static uint ReadReferenceUInt32LE(byte[] bytes, int offset)
        {
            return bytes[offset] |
                   ((uint)bytes[offset + 1] << 8) |
                   ((uint)bytes[offset + 2] << 16) |
                   ((uint)bytes[offset + 3] << 24);
        }

        private void LoadFile(string sourcePath)
        {
            Dictionary<CpuParamChunkKind, CpuParamChunkState> loadedChunks = new Dictionary<CpuParamChunkKind, CpuParamChunkState>();
            using (XfbinParserBackend backend = new XfbinParserBackend(sourcePath))
            {
                foreach (XfbinBinaryChunkItem item in backend.GetBinaryChunks())
                {
                    CpuParamChunkKind kind;
                    if (!CpuParamCodec.TryGetChunkKind(item.ChunkName, item.ChunkPath, item.FileName, out kind))
                        continue;
                    if (loadedChunks.ContainsKey(kind))
                        throw new InvalidOperationException("The XFBIN contains more than one " + item.ChunkName + " chunk.");

                    CpuParamChunkState state;
                    try
                    {
                        state = CpuParamCodec.Parse(kind, item.BinaryData);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("The " + item.ChunkName + " chunk does not match cpuparam.bt: " + ex.Message, ex);
                    }

                    state.OriginalChunkName = item.ChunkName ?? string.Empty;
                    state.ChunkName = item.ChunkName ?? CpuParamCodec.GetDefaultChunkName(kind);
                    state.ChunkPath = item.ChunkPath ?? CpuParamCodec.GetDefaultChunkPath(kind);
                    state.ContainerVersion = item.Version;
                    state.ContainerVersionAttribute = item.VersionAttribute;
                    loadedChunks.Add(kind, state);
                }
            }

            if (loadedChunks.Count == 0)
                throw new InvalidOperationException("No cpuparam.bt binary chunks were found. Expected cpu_script, cpu_strength, cpuActionParam, or cpuPlayerParam.");

            chunks.Clear();
            foreach (KeyValuePair<CpuParamChunkKind, CpuParamChunkState> pair in loadedChunks)
                chunks.Add(pair.Key, pair.Value);
            fileOpen = true;
            filePath = sourcePath;
            Text = "CPU Param Editor - " + Path.GetFileName(sourcePath);
            RefreshAllEditors();
            SetStatus("Loaded " + loadedChunks.Count + " CPU parameter chunk(s) from " + Path.GetFileName(sourcePath) + ".");
        }

        private void SaveFile(bool saveAs)
        {
            if (!fileOpen)
            {
                MessageBox.Show(this, "No XFBIN is open.", "CPU Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string outputPath = filePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = ".xfbin";
                    dialog.Filter = "XFBIN files (*.xfbin)|*.xfbin";
                    dialog.FileName = Path.GetFileName(filePath);
                    dialog.Title = "Save CPU Parameter XFBIN As";
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    outputPath = dialog.FileName;
                }
            }

            try
            {
                CommitActiveEntryIfSelected();

                using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
                {
                    foreach (CpuParamChunkState state in chunks.Values.OrderBy(x => x.Kind))
                    {
                        byte[] chunkBytes = CpuParamCodec.Build(state);
                        backend.UpsertChunk(
                            state.OriginalChunkName,
                            state.ChunkName,
                            CpuParamCodec.BinaryChunkType,
                            state.ChunkPath,
                            ".binary",
                            chunkBytes,
                            state.ContainerVersion,
                            state.ContainerVersionAttribute);
                    }

                    backend.RepackTo(outputPath);
                }

                if (!File.Exists(outputPath))
                    throw new IOException("xfbin_parser.exe did not create the requested output file.");

                filePath = outputPath;
                foreach (CpuParamChunkState state in chunks.Values)
                    state.OriginalChunkName = state.ChunkName;
                Text = "CPU Param Editor - " + Path.GetFileName(outputPath);
                SetStatus("Saved " + chunks.Count + " CPU parameter chunk(s) to " + Path.GetFileName(outputPath) + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Could not save CPU parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Save failed.");
            }
        }

        private void RefreshAllEditors()
        {
            updatingUi = true;
            try
            {
                RefreshScriptGroups(0);
                RefreshStrengthGroups(0);
                RefreshActionEntries(0);
                RefreshPlayerEntries(0);
            }
            finally
            {
                updatingUi = false;
            }
            UpdateEditorEnabledStates();
        }

        private void UpdateEditorEnabledStates()
        {
            scriptRootPanel.Enabled = fileOpen && chunks.ContainsKey(CpuParamChunkKind.Script);
            strengthRootPanel.Enabled = fileOpen && chunks.ContainsKey(CpuParamChunkKind.Strength);
            actionRootPanel.Enabled = fileOpen && chunks.ContainsKey(CpuParamChunkKind.Action);
            playerRootPanel.Enabled = fileOpen && chunks.ContainsKey(CpuParamChunkKind.Player);
            saveToolStripMenuItem.Enabled = fileOpen;
            saveAsToolStripMenuItem.Enabled = fileOpen;
            closeToolStripMenuItem.Enabled = fileOpen;
        }

        private CpuScriptChunkState GetScriptChunk()
        {
            CpuParamChunkState state;
            return chunks.TryGetValue(CpuParamChunkKind.Script, out state) ? state as CpuScriptChunkState : null;
        }

        private CpuStrengthChunkState GetStrengthChunk()
        {
            CpuParamChunkState state;
            return chunks.TryGetValue(CpuParamChunkKind.Strength, out state) ? state as CpuStrengthChunkState : null;
        }

        private CpuActionChunkState GetActionChunk()
        {
            CpuParamChunkState state;
            return chunks.TryGetValue(CpuParamChunkKind.Action, out state) ? state as CpuActionChunkState : null;
        }

        private CpuPlayerChunkState GetPlayerChunk()
        {
            CpuParamChunkState state;
            return chunks.TryGetValue(CpuParamChunkKind.Player, out state) ? state as CpuPlayerChunkState : null;
        }

        private void RefreshScriptGroups(int requestedIndex)
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            scriptGroupListBox.Items.Clear();
            if (chunk == null)
            {
                scriptEntryListBox.Items.Clear();
                ClearScriptEditor();
                return;
            }
            for (int i = 0; i < chunk.Groups.Count; i++)
                scriptGroupListBox.Items.Add("Group " + i + " (" + chunk.Groups[i].Instructions.Count + " instructions)");
            if (chunk.Groups.Count > 0)
            {
                int selectedIndex = Math.Max(0, Math.Min(requestedIndex, chunk.Groups.Count - 1));
                scriptGroupListBox.SelectedIndex = selectedIndex;
                RefreshScriptEntries(selectedIndex, 0);
            }
            else
                RefreshScriptEntries(-1, 0);
        }

        private void RefreshScriptEntries(int groupIndex, int requestedIndex)
        {
            scriptEntryListBox.Items.Clear();
            CpuScriptChunkState chunk = GetScriptChunk();
            if (chunk == null || groupIndex < 0 || groupIndex >= chunk.Groups.Count)
            {
                ClearScriptEditor();
                return;
            }
            List<CpuScriptInstruction> entries = chunk.Groups[groupIndex].Instructions;
            for (int i = 0; i < entries.Count; i++)
                scriptEntryListBox.Items.Add(i + ": " + DescribeInstruction(entries[i]));
            if (entries.Count > 0)
            {
                scriptEntryListBox.SelectedIndex = Math.Max(0, Math.Min(requestedIndex, entries.Count - 1));
                LoadSelectedScriptEntry();
            }
            else
                ClearScriptEditor();
        }

        private static string DescribeInstruction(CpuScriptInstruction entry)
        {
            if (entry.Type == (ushort)CpuScriptType.CtrlCommand && entry.CommandNumber == (ushort)CpuScriptCommand.SetProb)
                return "SetProb action " + entry.Arguments[0] + " = " + entry.Arguments[1];
            if (entry.Type == (ushort)CpuScriptType.CtrlCommand && entry.CommandNumber == (ushort)CpuScriptCommand.AddProb)
                return "AddProb action " + entry.Arguments[0] + " += " + entry.Arguments[1];
            if (entry.Type == (ushort)CpuScriptType.CtrlCommand && entry.CommandNumber == (ushort)CpuScriptCommand.SetParam)
                return "SetParam " + entry.Arguments[0] + " = " + entry.Arguments[1];
            if (entry.CommandNumber == (ushort)CpuScriptCommand.None)
                return "Control marker, type " + entry.Type;
            return "Type " + entry.Type + ", Command " + entry.CommandNumber;
        }

        private void LoadSelectedScriptEntry()
        {
            CpuScriptInstruction entry = GetSelectedScriptEntry();
            if (entry == null)
            {
                ClearScriptEditor();
                return;
            }
            SelectChoice(scriptTypeComboBox, entry.Type);
            SelectChoice(scriptCommandComboBox, entry.CommandNumber);
            NumericUpDown[] controls = GetScriptArgumentControls();
            for (int i = 0; i < controls.Length; i++)
                controls[i].Value = entry.Arguments[i];
            scriptEditorGroupBox.Enabled = true;
        }

        private CpuScriptInstruction GetSelectedScriptEntry()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int groupIndex = scriptGroupListBox.SelectedIndex;
            int entryIndex = scriptEntryListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || groupIndex >= chunk.Groups.Count)
                return null;
            if (entryIndex < 0 || entryIndex >= chunk.Groups[groupIndex].Instructions.Count)
                return null;
            return chunk.Groups[groupIndex].Instructions[entryIndex];
        }

        private CpuScriptInstruction BuildScriptEntryFromEditor()
        {
            CpuScriptInstruction entry = new CpuScriptInstruction
            {
                Type = checked((ushort)GetChoice(scriptTypeComboBox)),
                CommandNumber = checked((ushort)GetChoice(scriptCommandComboBox)),
                Arguments = new int[8]
            };
            NumericUpDown[] controls = GetScriptArgumentControls();
            for (int i = 0; i < controls.Length; i++)
                entry.Arguments[i] = decimal.ToInt32(controls[i].Value);
            return entry;
        }

        private NumericUpDown[] GetScriptArgumentControls()
        {
            return new[]
            {
                scriptArgument0Value, scriptArgument1Value, scriptArgument2Value, scriptArgument3Value,
                scriptArgument4Value, scriptArgument5Value, scriptArgument6Value, scriptArgument7Value
            };
        }

        private void ClearScriptEditor()
        {
            scriptEditorGroupBox.Enabled = false;
            scriptTypeComboBox.SelectedIndex = -1;
            scriptCommandComboBox.SelectedIndex = -1;
            foreach (NumericUpDown control in GetScriptArgumentControls())
                control.Value = -1;
        }

        private void AddScriptGroup()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            if (chunk == null) return;
            chunk.Groups.Add(new CpuScriptGroup());
            RefreshScriptGroups(chunk.Groups.Count - 1);
        }

        private void DuplicateScriptGroup()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int index = scriptGroupListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Groups.Count) return;
            chunk.Groups.Insert(index + 1, chunk.Groups[index].Clone());
            RefreshScriptGroups(index + 1);
        }

        private void DeleteScriptGroup()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int index = scriptGroupListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Groups.Count) return;
            if (chunk.Groups.Count == 1)
            {
                MessageBox.Show(this, "cpu_script must keep at least one group. Delete the whole chunk if it is not needed.");
                return;
            }
            chunk.Groups.RemoveAt(index);
            RefreshScriptGroups(Math.Min(index, chunk.Groups.Count - 1));
        }

        private void AddScriptEntry()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int groupIndex = scriptGroupListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || groupIndex >= chunk.Groups.Count) return;
            CpuScriptInstruction entry = new CpuScriptInstruction
            {
                Type = (ushort)CpuScriptType.CtrlCommand,
                CommandNumber = (ushort)CpuScriptCommand.None
            };
            chunk.Groups[groupIndex].Instructions.Add(entry);
            RefreshScriptGroups(groupIndex);
            scriptEntryListBox.SelectedIndex = chunk.Groups[groupIndex].Instructions.Count - 1;
        }

        private void DuplicateScriptEntry()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int groupIndex = scriptGroupListBox.SelectedIndex;
            int entryIndex = scriptEntryListBox.SelectedIndex;
            CpuScriptInstruction entry = GetSelectedScriptEntry();
            if (chunk == null || entry == null) return;
            chunk.Groups[groupIndex].Instructions.Insert(entryIndex + 1, entry.Clone());
            RefreshScriptGroups(groupIndex);
            scriptEntryListBox.SelectedIndex = entryIndex + 1;
        }

        private void DeleteScriptEntry()
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int groupIndex = scriptGroupListBox.SelectedIndex;
            int entryIndex = scriptEntryListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || entryIndex < 0) return;
            chunk.Groups[groupIndex].Instructions.RemoveAt(entryIndex);
            RefreshScriptGroups(groupIndex);
            if (chunk.Groups[groupIndex].Instructions.Count > 0)
                scriptEntryListBox.SelectedIndex = Math.Min(entryIndex, chunk.Groups[groupIndex].Instructions.Count - 1);
        }

        private void UpdateScriptEntry(bool showMessage)
        {
            CpuScriptChunkState chunk = GetScriptChunk();
            int groupIndex = scriptGroupListBox.SelectedIndex;
            int entryIndex = scriptEntryListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || entryIndex < 0 || entryIndex >= chunk.Groups[groupIndex].Instructions.Count)
            {
                if (showMessage) MessageBox.Show(this, "Select a script instruction first.");
                return;
            }
            chunk.Groups[groupIndex].Instructions[entryIndex] = BuildScriptEntryFromEditor();
            scriptEntryListBox.Items[entryIndex] = entryIndex + ": " + DescribeInstruction(chunk.Groups[groupIndex].Instructions[entryIndex]);
        }

        private void RefreshStrengthGroups(int requestedIndex)
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            strengthGroupListBox.Items.Clear();
            if (chunk == null)
            {
                strengthEntryListBox.Items.Clear();
                ClearStrengthEditor();
                return;
            }
            for (int i = 0; i < chunk.Groups.Count; i++)
                strengthGroupListBox.Items.Add("Strength level " + i + " (" + chunk.Groups[i].Entries.Count + " entries)");
            if (chunk.Groups.Count > 0)
            {
                int selectedIndex = Math.Max(0, Math.Min(requestedIndex, chunk.Groups.Count - 1));
                strengthGroupListBox.SelectedIndex = selectedIndex;
                RefreshStrengthEntries(selectedIndex, 0);
            }
            else
                RefreshStrengthEntries(-1, 0);
        }

        private void RefreshStrengthEntries(int groupIndex, int requestedIndex)
        {
            strengthEntryListBox.Items.Clear();
            CpuStrengthChunkState chunk = GetStrengthChunk();
            if (chunk == null || groupIndex < 0 || groupIndex >= chunk.Groups.Count)
            {
                ClearStrengthEditor();
                return;
            }
            List<CpuStrengthEntry> entries = chunk.Groups[groupIndex].Entries;
            for (int i = 0; i < entries.Count; i++)
                strengthEntryListBox.Items.Add(i + ": Parameter " + entries[i].ParameterNumber + " = " + entries[i].Value);
            if (entries.Count > 0)
            {
                strengthEntryListBox.SelectedIndex = Math.Max(0, Math.Min(requestedIndex, entries.Count - 1));
                LoadSelectedStrengthEntry();
            }
            else
                ClearStrengthEditor();
        }

        private CpuStrengthEntry GetSelectedStrengthEntry()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int groupIndex = strengthGroupListBox.SelectedIndex;
            int entryIndex = strengthEntryListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || groupIndex >= chunk.Groups.Count)
                return null;
            if (entryIndex < 0 || entryIndex >= chunk.Groups[groupIndex].Entries.Count)
                return null;
            return chunk.Groups[groupIndex].Entries[entryIndex];
        }

        private void LoadSelectedStrengthEntry()
        {
            CpuStrengthEntry entry = GetSelectedStrengthEntry();
            if (entry == null)
            {
                ClearStrengthEditor();
                return;
            }
            SelectChoice(strengthTypeComboBox, entry.Type);
            SelectChoice(strengthCommandComboBox, entry.CommandNumber);
            strengthParameterValue.Value = entry.ParameterNumber;
            strengthValueValue.Value = entry.Value;
            NumericUpDown[] controls = GetStrengthUnusedArgumentControls();
            for (int i = 0; i < controls.Length; i++)
                controls[i].Value = entry.UnusedArguments[i];
            strengthEditorGroupBox.Enabled = true;
        }

        private CpuStrengthEntry BuildStrengthEntryFromEditor()
        {
            CpuStrengthEntry entry = new CpuStrengthEntry
            {
                Type = checked((ushort)GetChoice(strengthTypeComboBox)),
                CommandNumber = checked((ushort)GetChoice(strengthCommandComboBox)),
                ParameterNumber = decimal.ToInt32(strengthParameterValue.Value),
                Value = decimal.ToInt32(strengthValueValue.Value),
                UnusedArguments = new int[6]
            };
            NumericUpDown[] controls = GetStrengthUnusedArgumentControls();
            for (int i = 0; i < controls.Length; i++)
                entry.UnusedArguments[i] = decimal.ToInt32(controls[i].Value);
            return entry;
        }

        private NumericUpDown[] GetStrengthUnusedArgumentControls()
        {
            return new[]
            {
                strengthUnused0Value, strengthUnused1Value, strengthUnused2Value,
                strengthUnused3Value, strengthUnused4Value, strengthUnused5Value
            };
        }

        private void ClearStrengthEditor()
        {
            strengthEditorGroupBox.Enabled = false;
            strengthTypeComboBox.SelectedIndex = -1;
            strengthCommandComboBox.SelectedIndex = -1;
            strengthParameterValue.Value = 0;
            strengthValueValue.Value = 0;
            foreach (NumericUpDown control in GetStrengthUnusedArgumentControls())
                control.Value = -1;
        }

        private void AddStrengthGroup()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            if (chunk == null) return;
            chunk.Groups.Add(new CpuStrengthGroup());
            RefreshStrengthGroups(chunk.Groups.Count - 1);
        }

        private void DuplicateStrengthGroup()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int index = strengthGroupListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Groups.Count) return;
            chunk.Groups.Insert(index + 1, chunk.Groups[index].Clone());
            RefreshStrengthGroups(index + 1);
        }

        private void DeleteStrengthGroup()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int index = strengthGroupListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Groups.Count) return;
            if (chunk.Groups.Count == 1)
            {
                MessageBox.Show(this, "cpu_strength must keep at least one group. Delete the whole chunk if it is not needed.");
                return;
            }
            chunk.Groups.RemoveAt(index);
            RefreshStrengthGroups(Math.Min(index, chunk.Groups.Count - 1));
        }

        private void AddStrengthEntry()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int groupIndex = strengthGroupListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || groupIndex >= chunk.Groups.Count) return;
            chunk.Groups[groupIndex].Entries.Add(new CpuStrengthEntry());
            RefreshStrengthGroups(groupIndex);
            strengthEntryListBox.SelectedIndex = chunk.Groups[groupIndex].Entries.Count - 1;
        }

        private void DuplicateStrengthEntry()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int groupIndex = strengthGroupListBox.SelectedIndex;
            int entryIndex = strengthEntryListBox.SelectedIndex;
            CpuStrengthEntry entry = GetSelectedStrengthEntry();
            if (chunk == null || entry == null) return;
            chunk.Groups[groupIndex].Entries.Insert(entryIndex + 1, entry.Clone());
            RefreshStrengthGroups(groupIndex);
            strengthEntryListBox.SelectedIndex = entryIndex + 1;
        }

        private void DeleteStrengthEntry()
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int groupIndex = strengthGroupListBox.SelectedIndex;
            int entryIndex = strengthEntryListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || entryIndex < 0) return;
            chunk.Groups[groupIndex].Entries.RemoveAt(entryIndex);
            RefreshStrengthGroups(groupIndex);
            if (chunk.Groups[groupIndex].Entries.Count > 0)
                strengthEntryListBox.SelectedIndex = Math.Min(entryIndex, chunk.Groups[groupIndex].Entries.Count - 1);
        }

        private void UpdateStrengthEntry(bool showMessage)
        {
            CpuStrengthChunkState chunk = GetStrengthChunk();
            int groupIndex = strengthGroupListBox.SelectedIndex;
            int entryIndex = strengthEntryListBox.SelectedIndex;
            if (chunk == null || groupIndex < 0 || entryIndex < 0 || entryIndex >= chunk.Groups[groupIndex].Entries.Count)
            {
                if (showMessage) MessageBox.Show(this, "Select a strength entry first.");
                return;
            }
            chunk.Groups[groupIndex].Entries[entryIndex] = BuildStrengthEntryFromEditor();
            CpuStrengthEntry entry = chunk.Groups[groupIndex].Entries[entryIndex];
            strengthEntryListBox.Items[entryIndex] = entryIndex + ": Parameter " + entry.ParameterNumber + " = " + entry.Value;
        }

        private void RefreshActionEntries(int requestedIndex)
        {
            actionEntryListBox.Items.Clear();
            CpuActionChunkState chunk = GetActionChunk();
            RefreshPlayerActionChoices();
            if (chunk == null)
            {
                ClearActionEditor();
                return;
            }
            for (int i = 0; i < chunk.Entries.Count; i++)
            {
                CpuActionEntry entry = chunk.Entries[i];
                actionEntryListBox.Items.Add(i + ": " + (string.IsNullOrWhiteSpace(entry.Name) ? "(unnamed)" : entry.Name));
            }
            if (chunk.Entries.Count > 0)
            {
                actionEntryListBox.SelectedIndex = Math.Max(0, Math.Min(requestedIndex, chunk.Entries.Count - 1));
                LoadSelectedActionEntry();
            }
            else
                ClearActionEditor();
        }

        private CpuActionEntry GetSelectedActionEntry()
        {
            CpuActionChunkState chunk = GetActionChunk();
            int index = actionEntryListBox.SelectedIndex;
            return chunk != null && index >= 0 && index < chunk.Entries.Count ? chunk.Entries[index] : null;
        }

        private void LoadSelectedActionEntry()
        {
            CpuActionEntry entry = GetSelectedActionEntry();
            if (entry == null)
            {
                ClearActionEditor();
                return;
            }
            actionNameTextBox.Text = entry.Name ?? string.Empty;
            NumericUpDown[] unknownControls = GetActionUnknownControls();
            NumericUpDown[] tagControls = GetActionTagControls();
            for (int i = 0; i < unknownControls.Length; i++) unknownControls[i].Value = entry.UnknownValues[i];
            for (int i = 0; i < tagControls.Length; i++) tagControls[i].Value = entry.ActionNumberTags[i];
            actionEditorGroupBox.Enabled = true;
        }

        private CpuActionEntry BuildActionEntryFromEditor()
        {
            CpuActionEntry entry = new CpuActionEntry
            {
                Name = actionNameTextBox.Text ?? string.Empty,
                UnknownValues = new int[6],
                ActionNumberTags = new int[4]
            };
            NumericUpDown[] unknownControls = GetActionUnknownControls();
            NumericUpDown[] tagControls = GetActionTagControls();
            for (int i = 0; i < unknownControls.Length; i++) entry.UnknownValues[i] = decimal.ToInt32(unknownControls[i].Value);
            for (int i = 0; i < tagControls.Length; i++) entry.ActionNumberTags[i] = decimal.ToInt32(tagControls[i].Value);
            return entry;
        }

        private NumericUpDown[] GetActionUnknownControls()
        {
            return new[] { actionUnknown0Value, actionUnknown1Value, actionUnknown2Value, actionUnknown3Value, actionUnknown4Value, actionUnknown5Value };
        }

        private NumericUpDown[] GetActionTagControls()
        {
            return new[] { actionTag0Value, actionTag1Value, actionTag2Value, actionTag3Value };
        }

        private void ClearActionEditor()
        {
            actionEditorGroupBox.Enabled = false;
            actionNameTextBox.Text = string.Empty;
            foreach (NumericUpDown control in GetActionUnknownControls()) control.Value = 0;
            foreach (NumericUpDown control in GetActionTagControls()) control.Value = 9999;
        }

        private string BuildNextActionName()
        {
            CpuActionChunkState chunk = GetActionChunk();
            HashSet<string> names = new HashSet<string>(chunk.Entries.Select(x => x.Name), StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < int.MaxValue; i++)
            {
                string candidate = "CAP_" + i.ToString("D3");
                if (!names.Contains(candidate)) return candidate;
            }
            return "CAP_NEW";
        }

        private void AddActionEntry()
        {
            CpuActionChunkState chunk = GetActionChunk();
            if (chunk == null) return;
            CpuActionEntry entry = new CpuActionEntry { Name = BuildNextActionName() };
            chunk.Entries.Add(entry);
            RefreshActionEntries(chunk.Entries.Count - 1);
        }

        private void DuplicateActionEntry()
        {
            CpuActionChunkState chunk = GetActionChunk();
            int index = actionEntryListBox.SelectedIndex;
            CpuActionEntry entry = GetSelectedActionEntry();
            if (chunk == null || entry == null) return;
            CpuActionEntry copy = entry.Clone();
            copy.Name = BuildNextActionName();
            chunk.Entries.Insert(index + 1, copy);
            RefreshActionEntries(index + 1);
        }

        private void DeleteActionEntry()
        {
            CpuActionChunkState chunk = GetActionChunk();
            int index = actionEntryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count) return;
            chunk.Entries.RemoveAt(index);
            RefreshActionEntries(Math.Min(index, chunk.Entries.Count - 1));
        }

        private void UpdateActionEntry(bool showMessage)
        {
            CpuActionChunkState chunk = GetActionChunk();
            int index = actionEntryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count)
            {
                if (showMessage) MessageBox.Show(this, "Select an action entry first.");
                return;
            }
            chunk.Entries[index] = BuildActionEntryFromEditor();
            actionEntryListBox.Items[index] = index + ": " + chunk.Entries[index].Name;
            RefreshPlayerActionChoices();
        }

        private void RefreshPlayerEntries(int requestedIndex)
        {
            playerEntryListBox.Items.Clear();
            CpuPlayerChunkState chunk = GetPlayerChunk();
            if (chunk == null)
            {
                ClearPlayerEditor();
                return;
            }
            for (int i = 0; i < chunk.Entries.Count; i++)
            {
                CpuPlayerEntry entry = chunk.Entries[i];
                playerEntryListBox.Items.Add(BuildPlayerEntryLabel(i, entry));
            }
            if (chunk.Entries.Count > 0)
            {
                playerEntryListBox.SelectedIndex = Math.Max(0, Math.Min(requestedIndex, chunk.Entries.Count - 1));
                LoadSelectedPlayerEntry();
            }
            else
                ClearPlayerEditor();
        }

        private static string GetPlayerTypeLabel(int value)
        {
            switch (value)
            {
                case (int)CpuPlayerType.Normal: return "Normal";
                case (int)CpuPlayerType.Dash: return "Dash";
                case (int)CpuPlayerType.ProjectileType: return "Projectile Type";
                case (int)CpuPlayerType.PuppetType: return "Puppet Type";
                default: return "Unknown " + value;
            }
        }

        private string BuildPlayerEntryLabel(int entryIndex, CpuPlayerEntry entry)
        {
            string characterId;
            string character = TryGetCharacodeId(entry.Characode, out characterId)
                ? "[Entry " + entry.Characode + "] " + characterId
                : "Characode Entry " + entry.Characode;
            return entryIndex + ": " + character + " | " + GetPlayerTypeLabel(entry.Type) +
                   " | Groups " + entry.ScriptGroupIndexA + "/" + entry.ScriptGroupIndexB;
        }

        private bool IsKnownCharacodeEntry(int characodeEntry)
        {
            string characterId;
            return TryGetCharacodeId(characodeEntry, out characterId);
        }

        private bool TryGetCharacodeId(int characodeEntry, out string characterId)
        {
            // cpuPlayerParam stores a one-based characode entry number. The parsed
            // characode ID list is zero-based, so entry 1 resolves to list position 0.
            if (characodeEntry > 0 && characodeEntry <= characodeIds.Count &&
                !string.IsNullOrWhiteSpace(characodeIds[characodeEntry - 1]))
            {
                characterId = characodeIds[characodeEntry - 1];
                return true;
            }

            characterId = string.Empty;
            return false;
        }

        private void RefreshSelectedCharacterId()
        {
            string characterId;
            int index = decimal.ToInt32(playerCharacodeValue.Value);
            if (TryGetCharacodeId(index, out characterId))
                playerCharacterIdTextBox.Text = characterId;
            else if (characodeIds.Count == 0)
                playerCharacterIdTextBox.Text = "Load characode reference";
            else
                playerCharacterIdTextBox.Text = "Entry not found";
        }

        private CpuPlayerEntry GetSelectedPlayerEntry()
        {
            CpuPlayerChunkState chunk = GetPlayerChunk();
            int index = playerEntryListBox.SelectedIndex;
            return chunk != null && index >= 0 && index < chunk.Entries.Count ? chunk.Entries[index] : null;
        }

        private void LoadSelectedPlayerEntry()
        {
            CpuPlayerEntry entry = GetSelectedPlayerEntry();
            if (entry == null)
            {
                ClearPlayerEditor();
                return;
            }
            playerCharacodeValue.Value = entry.Characode;
            RefreshSelectedCharacterId();
            SelectChoice(playerTypeComboBox, entry.Type);
            playerScriptGroupAValue.Value = entry.ScriptGroupIndexA;
            playerScriptGroupAFlagCheckBox.Checked = entry.ScriptGroupFlagA;
            playerScriptGroupBValue.Value = entry.ScriptGroupIndexB;
            playerScriptGroupBFlagCheckBox.Checked = entry.ScriptGroupFlagB;
            for (int i = 0; i < CpuParamCodec.PlayerStringSlotIndices.Length; i++)
                playerActionSlotsGrid.Rows[i].Cells[1].Value = entry.ActionSlots[CpuParamCodec.PlayerStringSlotIndices[i]] ?? string.Empty;
            playerEditorGroupBox.Enabled = true;
        }

        private CpuPlayerEntry BuildPlayerEntryFromEditor()
        {
            playerActionSlotsGrid.EndEdit();
            CpuPlayerEntry entry = new CpuPlayerEntry
            {
                Characode = decimal.ToInt32(playerCharacodeValue.Value),
                Type = GetChoice(playerTypeComboBox),
                ScriptGroupIndexA = decimal.ToUInt32(playerScriptGroupAValue.Value),
                ScriptGroupFlagA = playerScriptGroupAFlagCheckBox.Checked,
                ScriptGroupIndexB = decimal.ToUInt32(playerScriptGroupBValue.Value),
                ScriptGroupFlagB = playerScriptGroupBFlagCheckBox.Checked
            };
            for (int i = 0; i < CpuParamCodec.PlayerStringSlotIndices.Length; i++)
            {
                object value = playerActionSlotsGrid.Rows[i].Cells[1].Value;
                entry.ActionSlots[CpuParamCodec.PlayerStringSlotIndices[i]] = value == null ? string.Empty : Convert.ToString(value);
            }
            return entry;
        }

        private void ClearPlayerEditor()
        {
            playerEditorGroupBox.Enabled = false;
            playerCharacodeValue.Value = 0;
            playerCharacterIdTextBox.Clear();
            playerTypeComboBox.SelectedIndex = -1;
            playerScriptGroupAValue.Value = 0;
            playerScriptGroupAFlagCheckBox.Checked = false;
            playerScriptGroupBValue.Value = 0;
            playerScriptGroupBFlagCheckBox.Checked = false;
            for (int i = 0; i < playerActionSlotsGrid.Rows.Count; i++)
                playerActionSlotsGrid.Rows[i].Cells[1].Value = string.Empty;
        }

        private void AddPlayerEntry()
        {
            CpuPlayerChunkState chunk = GetPlayerChunk();
            if (chunk == null) return;
            chunk.Entries.Add(new CpuPlayerEntry());
            RefreshPlayerEntries(chunk.Entries.Count - 1);
        }

        private void DuplicatePlayerEntry()
        {
            CpuPlayerChunkState chunk = GetPlayerChunk();
            int index = playerEntryListBox.SelectedIndex;
            CpuPlayerEntry entry = GetSelectedPlayerEntry();
            if (chunk == null || entry == null) return;
            chunk.Entries.Insert(index + 1, entry.Clone());
            RefreshPlayerEntries(index + 1);
        }

        private void DeletePlayerEntry()
        {
            CpuPlayerChunkState chunk = GetPlayerChunk();
            int index = playerEntryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count) return;
            chunk.Entries.RemoveAt(index);
            RefreshPlayerEntries(Math.Min(index, chunk.Entries.Count - 1));
        }

        private void UpdatePlayerEntry(bool showMessage)
        {
            CpuPlayerChunkState chunk = GetPlayerChunk();
            int index = playerEntryListBox.SelectedIndex;
            if (chunk == null || index < 0 || index >= chunk.Entries.Count)
            {
                if (showMessage) MessageBox.Show(this, "Select a player entry first.");
                return;
            }
            chunk.Entries[index] = BuildPlayerEntryFromEditor();
            CpuPlayerEntry entry = chunk.Entries[index];
            playerEntryListBox.Items[index] = BuildPlayerEntryLabel(index, entry);
        }

        private void NavigateToPlayerAction(string actionName)
        {
            if (string.IsNullOrWhiteSpace(actionName))
                return;

            CpuActionChunkState actionChunk = GetActionChunk();
            if (actionChunk == null)
            {
                SetStatus("The CPU Action Param chunk is not loaded.");
                return;
            }

            int actionIndex = actionChunk.Entries.FindIndex(x =>
                string.Equals(x.Name, actionName, StringComparison.Ordinal));
            if (actionIndex < 0)
            {
                SetStatus("No CPU Action Param entry named " + actionName + " was found.");
                return;
            }

            UpdatePlayerEntry(false);
            chunkTabControl.SelectedTab = actionTabPage;
            actionEntryListBox.SelectedIndex = actionIndex;
            LoadSelectedActionEntry();
            SetStatus("Opened CPU Action Param entry " + actionName + ".");
        }

        private void CommitActiveEntryIfSelected()
        {
            switch (currentKind)
            {
                case CpuParamChunkKind.Script: UpdateScriptEntry(false); break;
                case CpuParamChunkKind.Strength: UpdateStrengthEntry(false); break;
                case CpuParamChunkKind.Action: UpdateActionEntry(false); break;
                case CpuParamChunkKind.Player: UpdatePlayerEntry(false); break;
            }
        }

        private void SetStatus(string message)
        {
            statusLabel.Text = message;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) { OpenFile(); }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { SaveFile(false); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) { SaveFile(true); }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) { ClearFileState(); }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }
        private void loadCharacodeReferenceToolStripMenuItem_Click(object sender, EventArgs e) { OpenCharacodeReferences(); }

        private void chunkTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updatingUi) return;
            if (chunkTabControl.SelectedTab == scriptTabPage) currentKind = CpuParamChunkKind.Script;
            else if (chunkTabControl.SelectedTab == strengthTabPage) currentKind = CpuParamChunkKind.Strength;
            else if (chunkTabControl.SelectedTab == actionTabPage) currentKind = CpuParamChunkKind.Action;
            else if (chunkTabControl.SelectedTab == playerTabPage) currentKind = CpuParamChunkKind.Player;
        }

        private void scriptGroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingUi) RefreshScriptEntries(scriptGroupListBox.SelectedIndex, 0);
        }
        private void scriptEntryListBox_SelectedIndexChanged(object sender, EventArgs e) { if (!updatingUi) LoadSelectedScriptEntry(); }
        private void scriptAddGroupButton_Click(object sender, EventArgs e) { AddScriptGroup(); }
        private void scriptDuplicateGroupButton_Click(object sender, EventArgs e) { DuplicateScriptGroup(); }
        private void scriptDeleteGroupButton_Click(object sender, EventArgs e) { DeleteScriptGroup(); }
        private void scriptAddEntryButton_Click(object sender, EventArgs e) { AddScriptEntry(); }
        private void scriptDuplicateEntryButton_Click(object sender, EventArgs e) { DuplicateScriptEntry(); }
        private void scriptDeleteEntryButton_Click(object sender, EventArgs e) { DeleteScriptEntry(); }
        private void scriptApplyEntryButton_Click(object sender, EventArgs e) { UpdateScriptEntry(true); }

        private void strengthGroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingUi) RefreshStrengthEntries(strengthGroupListBox.SelectedIndex, 0);
        }
        private void strengthEntryListBox_SelectedIndexChanged(object sender, EventArgs e) { if (!updatingUi) LoadSelectedStrengthEntry(); }
        private void strengthAddGroupButton_Click(object sender, EventArgs e) { AddStrengthGroup(); }
        private void strengthDuplicateGroupButton_Click(object sender, EventArgs e) { DuplicateStrengthGroup(); }
        private void strengthDeleteGroupButton_Click(object sender, EventArgs e) { DeleteStrengthGroup(); }
        private void strengthAddEntryButton_Click(object sender, EventArgs e) { AddStrengthEntry(); }
        private void strengthDuplicateEntryButton_Click(object sender, EventArgs e) { DuplicateStrengthEntry(); }
        private void strengthDeleteEntryButton_Click(object sender, EventArgs e) { DeleteStrengthEntry(); }
        private void strengthApplyEntryButton_Click(object sender, EventArgs e) { UpdateStrengthEntry(true); }

        private void actionEntryListBox_SelectedIndexChanged(object sender, EventArgs e) { if (!updatingUi) LoadSelectedActionEntry(); }
        private void actionAddEntryButton_Click(object sender, EventArgs e) { AddActionEntry(); }
        private void actionDuplicateEntryButton_Click(object sender, EventArgs e) { DuplicateActionEntry(); }
        private void actionDeleteEntryButton_Click(object sender, EventArgs e) { DeleteActionEntry(); }
        private void actionApplyEntryButton_Click(object sender, EventArgs e) { UpdateActionEntry(true); }

        private void playerEntryListBox_SelectedIndexChanged(object sender, EventArgs e) { if (!updatingUi) LoadSelectedPlayerEntry(); }
        private void playerCharacodeValue_ValueChanged(object sender, EventArgs e) { RefreshSelectedCharacterId(); }
        private void playerLoadCharacodeButton_Click(object sender, EventArgs e) { OpenCharacodeReferences(); }
        private void playerActionSlotsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != playerActionNameColumn.Index)
                return;

            if (!playerActionSlotsGrid.BeginEdit(true))
                return;
            ComboBox comboBox = playerActionSlotsGrid.EditingControl as ComboBox;
            if (comboBox != null)
                comboBox.DroppedDown = true;
        }

        private void playerActionSlotsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != playerEditActionColumn.Index)
                return;

            playerActionSlotsGrid.EndEdit();
            string actionName = Convert.ToString(
                playerActionSlotsGrid.Rows[e.RowIndex].Cells[playerActionNameColumn.Index].Value);

            if (string.IsNullOrWhiteSpace(actionName))
            {
                SetStatus("Select an Action Name before clicking Edit.");
                return;
            }

            NavigateToPlayerAction(actionName);
        }

        private void playerAddEntryButton_Click(object sender, EventArgs e) { AddPlayerEntry(); }
        private void playerDuplicateEntryButton_Click(object sender, EventArgs e) { DuplicatePlayerEntry(); }
        private void playerDeleteEntryButton_Click(object sender, EventArgs e) { DeletePlayerEntry(); }
        private void playerApplyEntryButton_Click(object sender, EventArgs e) { UpdatePlayerEntry(true); }
    }
}
