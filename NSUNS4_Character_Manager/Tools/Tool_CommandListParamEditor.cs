using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_CommandListParamEditor : Form
    {
        private const int CommandHeaderSize = 0x14;
        private const int CommandEntrySize = 0x34;
        private const int WmSetRedraw = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr windowHandle, int message, IntPtr wordParameter, IntPtr longParameter);

        private sealed class CommandListParamDocument
        {
            public uint Version;
            public long Padding;
            public readonly List<CommandListParamEntry> Entries = new List<CommandListParamEntry>();
        }

        private sealed class CommandListParamEntry
        {
            public uint CommandLink;
            public string CommandLinkName = string.Empty;
            public int CommandListIndex;
            public uint CharacterName;
            public string CharacterNameText = string.Empty;
            public uint Characode;
            public string CharacodeName = string.Empty;
            public int CostumeIndex;
            public uint AttackName;
            public string AttackNameText = string.Empty;
            public uint ButtonPress;
            public string ButtonPressText = string.Empty;
            public int CommandType1;
            public int CommandTypeSkill;
            public int CommandTypeAwake;
            public int CommandTypeTeam;
            public int CommandType5;
            public int CommandType6;

            public CommandListParamEntry Clone()
            {
                return (CommandListParamEntry)MemberwiseClone();
            }
        }

        private sealed class ChunkState
        {
            public string ChunkName = string.Empty;
            public string ChunkPath = string.Empty;
            public int Version;
            public int VersionAttribute;
            public CommandListParamDocument Document = new CommandListParamDocument();
        }

        private sealed class CharacterClonePlan
        {
            public string SourceCharacode = string.Empty;
            public string TargetCharacode = string.Empty;
            public int MatchedEntries;
            public int SkippedExistingEntries;
            public readonly List<CommandListParamEntry> Entries = new List<CommandListParamEntry>();
        }

        private sealed class CostumeEntryPlan
        {
            public string Characode = string.Empty;
            public int TotalCostumes;
            public int SkippedExistingEntries;
            public readonly List<CommandListParamEntry> Entries = new List<CommandListParamEntry>();
        }

        private sealed class MessageHashMatch
        {
            public uint Hash;
            public string Text = string.Empty;

            public override string ToString()
            {
                return FormatHashBytes(Hash) + " | " + Text;
            }
        }

        private sealed class ButtonFormulaOption
        {
            public string DisplayName = string.Empty;
            public string FormulaPart = string.Empty;

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private ChunkState fileState;
        private readonly List<string> characodeIds = new List<string>();
        private readonly Dictionary<uint, string> characodeNamesByHash = new Dictionary<uint, string>();
        private readonly Dictionary<uint, string> messageTextByHash = new Dictionary<uint, string>();
        private readonly Dictionary<uint, string> generatedCommandNamesByHash = new Dictionary<uint, string>();
        private readonly List<int> visibleEntryIndices = new List<int>();
        private readonly List<ButtonFormulaOption> buttonFormulaSequence = new List<ButtonFormulaOption>();
        private static readonly uint[] GameCrcTable = BuildGameCrcTable();
        private static readonly Dictionary<string, string> ButtonFormulaNames =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "btn_attack", "Attack" },
                { "btn_shuriken", "Shuriken" },
                { "btn_chakra", "Chakra" },
                { "btn_chakradash", "Chakra Dash" },
                { "btn_jump", "Jump" },
                { "btn_guard", "Guard" },
                { "btn_kawarimi", "Substitution" },
                { "btn_supportl", "Support Left" },
                { "btn_supportr", "Support Right" },
                { "stick_lu", "Up Stick" },
                { "stick_ld", "Down Stick" },
                { "stick_ll", "Left Stick" },
                { "stick_lr", "Right Stick" },
                { "(<icon stick_ll /> or <icon stick_lr />)", "Left or Right Stick" },
                { "stick_l", "Left Stick Input" },
                { "stick_r", "Right Stick Input" },
                { "key_l", "Left Trigger" },
                { "key_r", "Right Trigger" },
                { "<color red>Guard</color> + ", "Grab" },
                { "Tilting <icon stick_l />, ", "Tilt" }
            };
        private static readonly string[] KnownCommandStems =
        {
            "12kks_02_command_", "12kks_command_", "2tyo_x_command_", "3kkd_command_",
            "3ngt01_command_", "5hnb_01_command_", "5hnb_command_", "8hnt_command_",
            "b3nx_command_", "b8it_command_", "bbrb_01_command_", "bbrb_command_",
            "bnry_command_", "BOSS_BATTLE_01_command_", "BOSS_BATTLE_02_command_",
            "BOSS_BATTLE_03_command_", "BOSS_BATTLE_04_command_", "BOSS_BATTLE_05_command_",
            "BOSS_BATTLE_06_command_", "BOSS_BATTLE_07_command_", "BOSS_BATTLE_08_command_",
            "BOSS_BATTLE_09_command_", "BOSS_BATTLE_10_command_", "BOSS_BATTLE_11_command_",
            "BOSS_BATTLE_12_command_", "BOSS_BATTLE_13_command_", "BOSS_BATTLE_14_command_",
            "BOSS_BATTLE_15_command_", "Boss01_phase01_01_command_", "Boss01_phase01_command_",
            "Boss01_phase04_command_", "Boss06_phase01_command_", "Boss06_phase02_command_",
            "Boss06_phase03_command_", "Boss06_phase04_command_", "Boss11_phase01_command_",
            "Boss11_phase02_1_command_", "Boss11_phase02_2_command_", "Boss11_phase02_3_command_",
            "Boss11_phase02_4_command_", "Boss11_phase02_5_command_", "Boss23_phase01_command_",
            "Boss23_phase02_command_", "Boss24_phase01_command_", "Boss25_phase01_command_",
            "Boss25_phase02_command_", "Boss25_phase03_command_", "Boss26_phase01_command_",
            "Boss26_phase02_command_", "Boss27_phase01_command_", "Boss28_phase01_command_",
            "Boss28_phase02_command_", "kpea_command_", "kpeacommand_", "sys_jf_command_","sys_free_command_",
            "sys_forces_command_","sys_boss_01_command_","sys_boss_02_command_","sys_boss_03_command_","sys_boss_04_command_","sys_boss_05_command_","sys_boss_06_command_"
        };
        private static readonly string[] CharacodeCommandStemFormats =
        {
            "{0}_command_",
            "{0}_adv_command_",
            "{0}_adv_x_command_",
            "{0}_x_command_"
        };
        private static readonly string[] CharacodeCostumeStemFormats =
        {
            "{0}_{1:D2}_command_",
            "{0}_x_{1:D2}_command_"
        };
        private const int MaximumCostumeNumber = 20;
        private string sourceFilePath = string.Empty;
        private string characodeReferencePath = string.Empty;
        private string messageInfoReferencePath = string.Empty;
        private string activeSearchText = string.Empty;
        private int loadedEntryIndex = -1;
        private int selectedButtonFormulaNode = -1;
        private bool changingSelection;
        private bool windowRedrawPaused;

        public Tool_CommandListParamEditor()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            PopulateButtonFormulaOptions();
            ConfigureNumericControls();
            PopulateCommandTypeChoices();
            ResetEditor();
            if (File.Exists(Main.chaPath))
                LoadCharacodeReferences(Main.chaPath, false);
            TryLoadConfiguredMessageInfoReferences();
            if (File.Exists(Main.commandListParamPath))
                LoadFile(Main.commandListParamPath);
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            entryListBox.BeginUpdate();
            SendMessage(Handle, WmSetRedraw, IntPtr.Zero, IntPtr.Zero);
            windowRedrawPaused = true;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            entryListBox.EndUpdate();
            if (windowRedrawPaused)
            {
                SendMessage(Handle, WmSetRedraw, new IntPtr(1), IntPtr.Zero);
                windowRedrawPaused = false;
                Invalidate(true);
                Update();
            }
            base.OnResizeEnd(e);
        }

        private void ConfigureNumericControls()
        {
            SetSignedRange(commandListIndexNumericUpDown);
            SetSignedRange(costumeIndexNumericUpDown);
            SetSignedRange(commandType6NumericUpDown);
        }

        private static void SetSignedRange(NumericUpDown control)
        {
            control.Minimum = int.MinValue;
            control.Maximum = int.MaxValue;
        }

        private void PopulateCommandTypeChoices()
        {
            commandType1ComboBox.Items.AddRange(new object[]
            {
                "0 - Default", "1", "2", "3", "100", "200", "300", "1000"
            });
            commandTypeSkillComboBox.Items.AddRange(new object[]
            {
                "0 - Always enabled", "1 - Skill", "2 - Secret technique", "3 - Reinforced skill",
                "4 - Team skill", "5 - Non-single team", "6 - Single team", "7 - Other/always enabled", "10"
            });
            commandTypeAwakeComboBox.Items.AddRange(new object[]
            {
                "0 - Normal/instant awakening", "1 - True awakening", "2 - Always enabled",
                "3 - Disabled", "4 - Other/always enabled"
            });
            commandTypeTeamComboBox.Items.AddRange(new object[]
            {
                "0 - Always enabled", "1 - Single team", "2 - Non-single team", "3 - Other/always enabled"
            });
            commandType5ComboBox.Items.Add("-1 - Always enabled");
            for (int value = 0; value <= 20; value++)
                commandType5ComboBox.Items.Add(value.ToString());
            commandType5ComboBox.Items.Add("2000 - Always enabled");
        }

        private void ResetEditor()
        {
            changingSelection = true;
            try
            {
                fileState = null;
                sourceFilePath = string.Empty;
                loadedEntryIndex = -1;
                visibleEntryIndices.Clear();
                activeSearchText = string.Empty;
                searchTextBox.Clear();
                entryListBox.Items.Clear();
                ClearEntryEditor();
            }
            finally
            {
                changingSelection = false;
            }

            SetLoadedState(false);
            statusLabel.Text = "Open commandListParam.bin.xfbin to begin.";
            Text = "Command List Param Editor";
        }

        private void SetLoadedState(bool loaded)
        {
            saveToolStripMenuItem.Enabled = loaded;
            saveAsToolStripMenuItem.Enabled = loaded;
            entriesGroupBox.Enabled = loaded;
            cloneToolStripMenuItem.Enabled = loaded && characodeIds.Count > 0;
            costumesToolStripMenuItem.Enabled = loaded && characodeIds.Count > 0;
        }

        private void ClearEntryEditor()
        {
            entryEditorPanel.Text = "Selected entry (0x34 bytes)";
            characodeLabel.Text = "Characode";
            commandLinkLabel.Text = "Command Link";
            commandLinkTextBox.Text = string.Empty;
            commandListIndexNumericUpDown.Value = 0;
            characterNameTextBox.Text = string.Empty;
            characodeTextBox.Text = string.Empty;
            costumeIndexNumericUpDown.Value = 0;
            attackNameHashTextBox.Text = string.Empty;
            attackNameTextBox.Text = string.Empty;
            buttonPressHashTextBox.Text = string.Empty;
            buttonPressTextBox.Text = string.Empty;
            ClearButtonFormulaSearch();
            SetComboValue(commandType1ComboBox, 0);
            SetComboValue(commandTypeSkillComboBox, 0);
            SetComboValue(commandTypeAwakeComboBox, 0);
            SetComboValue(commandTypeTeamComboBox, 0);
            SetComboValue(commandType5ComboBox, -1);
            commandType6NumericUpDown.Value = -1;
            entryEditorPanel.Enabled = false;
        }

        private void OpenFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "xfbin";
                dialog.Filter = "XFBIN files (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                dialog.Title = "Open commandListParam XFBIN";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                LoadFile(dialog.FileName);
            }
        }

        private void LoadFile(string filePath)
        {
            try
            {
                ChunkState parsedState = null;
                string parseError = string.Empty;

                using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
                {
                    List<XfbinBinaryChunkItem> binaryChunks = backend.GetBinaryChunks();
                    List<XfbinBinaryChunkItem> namedCandidates = binaryChunks.Where(x => IsNamedChunk(x, "commandListParam")).ToList();
                    IEnumerable<XfbinBinaryChunkItem> candidates = namedCandidates.Count > 0 ? namedCandidates : binaryChunks;

                    foreach (XfbinBinaryChunkItem item in candidates)
                    {
                        CommandListParamDocument document;
                        string error;
                        if (!TryParseCommandListChunk(item.BinaryData, out document, out error))
                        {
                            if (namedCandidates.Count > 0)
                                parseError = error;
                            continue;
                        }

                        parsedState = new ChunkState
                        {
                            ChunkName = item.ChunkName ?? string.Empty,
                            ChunkPath = item.ChunkPath ?? string.Empty,
                            Version = item.Version,
                            VersionAttribute = item.VersionAttribute,
                            Document = document
                        };
                        break;
                    }
                }

                if (parsedState == null)
                {
                    string detail = string.IsNullOrWhiteSpace(parseError) ? string.Empty : Environment.NewLine + Environment.NewLine + parseError;
                    MessageBox.Show(this, "No valid commandListParam binary chunk was found." + detail,
                        "Command List Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                fileState = parsedState;
                sourceFilePath = filePath;
                TryAutoLoadCharacodeReferences(filePath);
                TryAutoLoadMessageInfoReferences(filePath);
                ResolveCharacodeNames();
                ResolveMessageTexts();
                RefreshEntryList(0);
                SetLoadedState(true);
                statusLabel.Text = string.Format("Loaded {0} entries from {1}. {2}",
                    fileState.Document.Entries.Count,
                    Path.GetFileName(filePath),
                    characodeNamesByHash.Count > 0 || messageTextByHash.Count > 0
                        ? string.Format("{0} characode hashes and {1} message hashes loaded; internal command patterns active.",
                            characodeNamesByHash.Count, messageTextByHash.Count)
                        : "Load reference files from the References menu.");
                Text = "Command List Param Editor - " + Path.GetFileName(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not open the XFBIN." + Environment.NewLine + Environment.NewLine + ex.Message,
                    "Command List Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsNamedChunk(XfbinBinaryChunkItem item, string name)
        {
            string identity = (item.ChunkName ?? string.Empty) + "|" +
                              (item.ChunkPath ?? string.Empty) + "|" +
                              (item.FileName ?? string.Empty);
            return identity.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool TryParseCommandListChunk(byte[] bytes, out CommandListParamDocument document, out string error)
        {
            document = null;
            error = string.Empty;
            if (bytes == null || bytes.Length < CommandHeaderSize)
            {
                error = "The chunk is smaller than the 0x14-byte header.";
                return false;
            }

            uint count = ReadUInt32LE(bytes, 8);
            if (ReadUInt32BE(bytes, 0) != bytes.Length - 4 ||
                CommandHeaderSize + ((long)count * CommandEntrySize) != bytes.Length)
            {
                error = "The size or 0x34-byte entry table is invalid.";
                return false;
            }

            document = new CommandListParamDocument
            {
                Version = ReadUInt32LE(bytes, 4),
                Padding = ReadInt64LE(bytes, 12)
            };
            for (int i = 0; i < count; i++)
            {
                int offset = CommandHeaderSize + (i * CommandEntrySize);
                document.Entries.Add(new CommandListParamEntry
                {
                    CommandLink = ReadUInt32LE(bytes, offset),
                    CommandListIndex = ReadInt32LE(bytes, offset + 0x04),
                    CharacterName = ReadUInt32LE(bytes, offset + 0x08),
                    Characode = ReadUInt32LE(bytes, offset + 0x0C),
                    CostumeIndex = ReadInt32LE(bytes, offset + 0x10),
                    AttackName = ReadUInt32LE(bytes, offset + 0x14),
                    ButtonPress = ReadUInt32LE(bytes, offset + 0x18),
                    CommandType1 = ReadInt32LE(bytes, offset + 0x1C),
                    CommandTypeSkill = ReadInt32LE(bytes, offset + 0x20),
                    CommandTypeAwake = ReadInt32LE(bytes, offset + 0x24),
                    CommandTypeTeam = ReadInt32LE(bytes, offset + 0x28),
                    CommandType5 = ReadInt32LE(bytes, offset + 0x2C),
                    CommandType6 = ReadInt32LE(bytes, offset + 0x30)
                });
            }
            return true;
        }

        private static byte[] BuildCommandListChunk(CommandListParamDocument document)
        {
            int length = checked(CommandHeaderSize + (document.Entries.Count * CommandEntrySize));
            byte[] bytes = new byte[length];
            WriteUInt32BE(bytes, 0, checked((uint)(length - 4)));
            WriteUInt32LE(bytes, 4, document.Version);
            WriteUInt32LE(bytes, 8, checked((uint)document.Entries.Count));
            WriteInt64LE(bytes, 12, document.Padding);

            for (int i = 0; i < document.Entries.Count; i++)
            {
                int offset = CommandHeaderSize + (i * CommandEntrySize);
                CommandListParamEntry entry = document.Entries[i];
                WriteUInt32LE(bytes, offset, entry.CommandLink);
                WriteInt32LE(bytes, offset + 0x04, entry.CommandListIndex);
                WriteUInt32LE(bytes, offset + 0x08, entry.CharacterName);
                WriteUInt32LE(bytes, offset + 0x0C, entry.Characode);
                WriteInt32LE(bytes, offset + 0x10, entry.CostumeIndex);
                WriteUInt32LE(bytes, offset + 0x14, entry.AttackName);
                WriteUInt32LE(bytes, offset + 0x18, entry.ButtonPress);
                WriteInt32LE(bytes, offset + 0x1C, entry.CommandType1);
                WriteInt32LE(bytes, offset + 0x20, entry.CommandTypeSkill);
                WriteInt32LE(bytes, offset + 0x24, entry.CommandTypeAwake);
                WriteInt32LE(bytes, offset + 0x28, entry.CommandTypeTeam);
                WriteInt32LE(bytes, offset + 0x2C, entry.CommandType5);
                WriteInt32LE(bytes, offset + 0x30, entry.CommandType6);
            }
            return bytes;
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

        private void TryAutoLoadCharacodeReferences(string commandListPath)
        {
            if (characodeNamesByHash.Count > 0)
                return;

            string sourceDirectory = Path.GetDirectoryName(commandListPath) ?? string.Empty;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            string[] candidates =
            {
                Main.chaPath,
                Path.Combine(sourceDirectory, "characode.bin.xfbin"),
                Path.Combine(baseDirectory, "xfbinFiles", "characode.bin.xfbin"),
                Path.Combine(Environment.CurrentDirectory ?? string.Empty, "xfbinFiles", "characode.bin.xfbin")
            };

            foreach (string candidate in candidates.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (File.Exists(candidate) && LoadCharacodeReferences(candidate, false))
                    return;
            }
        }

        private bool LoadCharacodeReferences(string filePath, bool showError)
        {
            try
            {
                List<string> ids = null;
                string parseError = string.Empty;

                using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
                {
                    List<XfbinBinaryChunkItem> chunksInFile = backend.GetBinaryChunks();
                    List<XfbinBinaryChunkItem> namedCandidates = chunksInFile.Where(x => IsNamedChunk(x, "characode")).ToList();
                    IEnumerable<XfbinBinaryChunkItem> candidates = namedCandidates.Count > 0 ? namedCandidates : chunksInFile;

                    foreach (XfbinBinaryChunkItem item in candidates)
                    {
                        List<string> parsedIds;
                        string error;
                        if (TryParseCharacodeChunk(item.BinaryData, out parsedIds, out error))
                        {
                            ids = parsedIds;
                            break;
                        }
                        parseError = error;
                    }
                }

                if (ids == null)
                    throw new InvalidDataException("No valid characode binary chunk was found. " + parseError);

                characodeIds.Clear();
                characodeIds.AddRange(ids);
                characodeNamesByHash.Clear();
                foreach (string id in ids)
                {
                    byte[] hashBytes = Main.crc32(id);
                    uint hashValue = BitConverter.ToUInt32(hashBytes, 0);
                    if (!characodeNamesByHash.ContainsKey(hashValue))
                        characodeNamesByHash.Add(hashValue, id);
                }

                characodeReferencePath = filePath;
                ResolveCharacodeNames();
                int selectedIndex = loadedEntryIndex;
                if (fileState != null)
                    RefreshEntryList(selectedIndex);
                SetLoadedState(fileState != null);

                int matchedEntries = fileState == null ? 0 : fileState.Document.Entries
                    .Count(x => !string.IsNullOrWhiteSpace(x.CharacodeName));
                int matchedCommands = fileState == null ? 0 : fileState.Document.Entries
                    .Count(x => !string.IsNullOrWhiteSpace(x.CommandLinkName));
                statusLabel.Text = string.Format("Hashed {0} characodes from {1}; matched {2} characodes and {3} command links.",
                    ids.Count, Path.GetFileName(filePath), matchedEntries, matchedCommands);
                return true;
            }
            catch (Exception ex)
            {
                if (showError)
                {
                    MessageBox.Show(this, "Could not load characode references." + Environment.NewLine + Environment.NewLine + ex.Message,
                        "Command List Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
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

            uint declaredSize = ReadUInt32BE(bytes, 0);
            uint count = ReadUInt32LE(bytes, 4);
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
                string id = Encoding.ASCII.GetString(bytes, offset, length);
                if (!string.IsNullOrWhiteSpace(id))
                    ids.Add(id);
            }
            return true;
        }

        private static uint ReadUInt32BE(byte[] bytes, int offset)
        {
            return ((uint)bytes[offset] << 24) |
                   ((uint)bytes[offset + 1] << 16) |
                   ((uint)bytes[offset + 2] << 8) |
                   bytes[offset + 3];
        }

        private static uint ReadUInt32LE(byte[] bytes, int offset)
        {
            return bytes[offset] |
                   ((uint)bytes[offset + 1] << 8) |
                   ((uint)bytes[offset + 2] << 16) |
                   ((uint)bytes[offset + 3] << 24);
        }

        private static int ReadInt32LE(byte[] bytes, int offset)
        {
            return unchecked((int)ReadUInt32LE(bytes, offset));
        }

        private static long ReadInt64LE(byte[] bytes, int offset)
        {
            return unchecked((long)(ReadUInt32LE(bytes, offset) | ((ulong)ReadUInt32LE(bytes, offset + 4) << 32)));
        }

        private static void WriteUInt32BE(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)value;
        }

        private static void WriteUInt32LE(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)value;
            bytes[offset + 1] = (byte)(value >> 8);
            bytes[offset + 2] = (byte)(value >> 16);
            bytes[offset + 3] = (byte)(value >> 24);
        }

        private static void WriteInt32LE(byte[] bytes, int offset, int value)
        {
            WriteUInt32LE(bytes, offset, unchecked((uint)value));
        }

        private static void WriteInt64LE(byte[] bytes, int offset, long value)
        {
            ulong unsignedValue = unchecked((ulong)value);
            WriteUInt32LE(bytes, offset, (uint)unsignedValue);
            WriteUInt32LE(bytes, offset + 4, (uint)(unsignedValue >> 32));
        }

        private void OpenMessageInfoReferences()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "xfbin";
                dialog.Filter = "MessageInfo XFBIN (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                dialog.Title = "Load messageInfo.bin.xfbin";
                if (File.Exists(messageInfoReferencePath))
                    dialog.FileName = messageInfoReferencePath;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                LoadMessageInfoReferences(dialog.FileName, true);
            }
        }

        private void TryLoadConfiguredMessageInfoReferences()
        {
            if (messageTextByHash.Count > 0 || string.IsNullOrWhiteSpace(Main.messageInfoPath))
                return;

            string configuredPath = Main.messageInfoPath;
            string[] candidates =
            {
                configuredPath,
                Path.Combine(configuredPath, "messageInfo.bin.xfbin"),
                Path.Combine(configuredPath, "WIN64", "eng", "messageInfo.bin.xfbin")
            };
            foreach (string candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (File.Exists(candidate) && LoadMessageInfoReferences(candidate, false))
                    return;
            }
        }

        private void TryAutoLoadMessageInfoReferences(string commandListPath)
        {
            if (messageTextByHash.Count > 0)
                return;

            TryLoadConfiguredMessageInfoReferences();
            if (messageTextByHash.Count > 0)
                return;

            string sourceDirectory = Path.GetDirectoryName(commandListPath) ?? string.Empty;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            string[] candidates =
            {
                Path.Combine(sourceDirectory, "messageInfo.bin.xfbin"),
                Path.Combine(baseDirectory, "xfbinFiles", "messageInfo.bin.xfbin"),
                Path.Combine(Environment.CurrentDirectory ?? string.Empty, "xfbinFiles", "messageInfo.bin.xfbin")
            };
            foreach (string candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (File.Exists(candidate) && LoadMessageInfoReferences(candidate, false))
                    return;
            }
        }

        private bool LoadMessageInfoReferences(string filePath, bool showError)
        {
            try
            {
                Dictionary<uint, string> parsedMessages = null;
                string parseError = string.Empty;
                using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
                {
                    List<XfbinBinaryChunkItem> chunksInFile = backend.GetBinaryChunks();
                    List<XfbinBinaryChunkItem> namedCandidates = chunksInFile.Where(x => IsNamedChunk(x, "messageInfo")).ToList();
                    IEnumerable<XfbinBinaryChunkItem> candidates = namedCandidates.Count > 0 ? namedCandidates : chunksInFile;
                    foreach (XfbinBinaryChunkItem item in candidates)
                    {
                        Dictionary<uint, string> messages;
                        string error;
                        if (TryParseMessageInfoChunk(item.BinaryData, out messages, out error))
                        {
                            parsedMessages = messages;
                            break;
                        }
                        parseError = error;
                    }
                }

                if (parsedMessages == null)
                    throw new InvalidDataException("No valid messageInfo binary chunk was found. " + parseError);

                messageTextByHash.Clear();
                foreach (KeyValuePair<uint, string> message in parsedMessages)
                    messageTextByHash[message.Key] = message.Value;
                messageInfoReferencePath = filePath;
                ResolveMessageTexts();
                ClearButtonFormulaSearch();

                int selectedIndex = loadedEntryIndex;
                if (fileState != null)
                    RefreshEntryList(selectedIndex);

                int matchedFields = fileState == null ? 0 : fileState.Document.Entries.Sum(x =>
                    (string.IsNullOrWhiteSpace(x.CharacterNameText) ? 0 : 1) +
                    (string.IsNullOrWhiteSpace(x.AttackNameText) ? 0 : 1) +
                    (string.IsNullOrWhiteSpace(x.ButtonPressText) ? 0 : 1));
                statusLabel.Text = string.Format("Loaded {0} message hashes from {1}; matched {2} fields.",
                    messageTextByHash.Count, Path.GetFileName(filePath), matchedFields);
                return true;
            }
            catch (Exception ex)
            {
                if (showError)
                {
                    MessageBox.Show(this, "Could not load messageInfo references." + Environment.NewLine + Environment.NewLine + ex.Message,
                        "Command List Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }

        private static bool TryParseMessageInfoChunk(byte[] bytes, out Dictionary<uint, string> messages, out string error)
        {
            messages = new Dictionary<uint, string>();
            error = string.Empty;
            if (bytes == null || bytes.Length < 20)
            {
                error = "The messageInfo chunk is smaller than its 0x14-byte header.";
                return false;
            }

            uint declaredSize = ReadUInt32BE(bytes, 0);
            uint count = ReadUInt32LE(bytes, 8);
            long tableEnd = 20L + ((long)count * 40L);
            if (declaredSize != bytes.Length - 4 || tableEnd > bytes.Length)
            {
                error = "The messageInfo size or entry table is invalid.";
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                int entryOffset = 20 + (i * 40);
                uint hashValue = ReadUInt32LE(bytes, entryOffset);
                uint relativeMainTextOffset = ReadUInt32LE(bytes, entryOffset + 16);
                if (hashValue == 0 || relativeMainTextOffset == 0)
                    continue;

                long textOffset = entryOffset + 16L + relativeMainTextOffset;
                if (textOffset >= bytes.Length)
                    continue;

                string text = ReadUtf8String(bytes, (int)textOffset);
                text = text.Replace('\r', ' ').Replace('\n', ' ').Trim();
                if (!string.IsNullOrWhiteSpace(text) && !messages.ContainsKey(hashValue))
                    messages.Add(hashValue, text);
            }
            return true;
        }

        private static string ReadUtf8String(byte[] bytes, int offset)
        {
            int end = offset;
            while (end < bytes.Length && bytes[end] != 0)
                end++;
            return Encoding.UTF8.GetString(bytes, offset, end - offset);
        }

        private void ResolveCharacodeNames()
        {
            if (fileState == null)
                return;
            foreach (CommandListParamEntry entry in fileState.Document.Entries)
            {
                string name;
                entry.CharacodeName = entry.Characode != 0 && characodeNamesByHash.TryGetValue(entry.Characode, out name)
                    ? name
                    : string.Empty;
            }
            ResolveCommandLinkNames();
        }

        private void ResolveMessageTexts()
        {
            if (fileState == null)
                return;
            foreach (CommandListParamEntry entry in fileState.Document.Entries)
            {
                string text;
                entry.CharacterNameText = entry.CharacterName != 0 && messageTextByHash.TryGetValue(entry.CharacterName, out text)
                    ? text
                    : string.Empty;
                entry.AttackNameText = entry.AttackName != 0 && messageTextByHash.TryGetValue(entry.AttackName, out text)
                    ? text
                    : string.Empty;
                entry.ButtonPressText = entry.ButtonPress != 0 && messageTextByHash.TryGetValue(entry.ButtonPress, out text)
                    ? text
                    : string.Empty;
            }
        }

        private void ResolveCommandLinkNames()
        {
            if (fileState == null)
                return;

            List<CommandListParamEntry> entries = fileState.Document.Entries;
            foreach (CommandListParamEntry entry in entries)
                entry.CommandLinkName = string.Empty;

            HashSet<uint> targetHashes = new HashSet<uint>(entries.Where(x => x.CommandLink != 0).Select(x => x.CommandLink));
            Dictionary<uint, string> matchedNames = new Dictionary<uint, string>();

            foreach (KeyValuePair<uint, string> commandName in generatedCommandNamesByHash)
            {
                if (targetHashes.Contains(commandName.Key))
                    matchedNames[commandName.Key] = commandName.Value;
            }

            IEnumerable<IGrouping<int, CommandListParamEntry>> commandGroups = entries
                .Where(x => x.CommandLink != 0 && !matchedNames.ContainsKey(x.CommandLink) &&
                    x.CommandListIndex >= 0 && x.CommandListIndex <= 999)
                .GroupBy(x => x.CommandListIndex);
            foreach (IGrouping<int, CommandListParamEntry> commandGroup in commandGroups)
            {
                HashSet<uint> groupHashes = new HashSet<uint>(commandGroup.Select(x => x.CommandLink));
                int[] commandNumbers = commandGroup.Key == 0
                    ? new[] { 0, 1 }
                    : new[] { commandGroup.Key, commandGroup.Key + 1 };
                foreach (int commandNumber in commandNumbers.Where(x => x >= 0 && x <= 999).Distinct())
                {
                    foreach (string commandStem in KnownCommandStems)
                        TryMatchGeneratedCommand(commandStem + commandNumber.ToString("D3", CultureInfo.InvariantCulture),
                            groupHashes, matchedNames);

                    foreach (string characodeId in characodeIds)
                    {
                        foreach (string stemFormat in CharacodeCommandStemFormats)
                        {
                            string commandStem = string.Format(CultureInfo.InvariantCulture, stemFormat, characodeId);
                            TryMatchGeneratedCommand(commandStem + commandNumber.ToString("D3", CultureInfo.InvariantCulture),
                                groupHashes, matchedNames);
                        }
                        for (int costumeNumber = 1; costumeNumber <= MaximumCostumeNumber; costumeNumber++)
                        {
                            foreach (string stemFormat in CharacodeCostumeStemFormats)
                            {
                                string commandStem = string.Format(CultureInfo.InvariantCulture, stemFormat,
                                    characodeId, costumeNumber);
                                TryMatchGeneratedCommand(commandStem + commandNumber.ToString("D3", CultureInfo.InvariantCulture),
                                    groupHashes, matchedNames);
                            }
                        }
                    }
                }
            }

            foreach (CommandListParamEntry entry in entries)
            {
                string commandName;
                if (matchedNames.TryGetValue(entry.CommandLink, out commandName))
                    entry.CommandLinkName = commandName;
            }
        }

        private static void TryMatchGeneratedCommand(string commandName, HashSet<uint> targetHashes,
            Dictionary<uint, string> matchedNames)
        {
            uint hashValue = ComputeGameCrc32(commandName);
            if (targetHashes.Contains(hashValue) && !matchedNames.ContainsKey(hashValue))
                matchedNames.Add(hashValue, commandName);
        }

        private static uint[] BuildGameCrcTable()
        {
            uint[] table = new uint[256];
            for (uint i = 0; i < table.Length; i++)
            {
                uint value = i << 24;
                for (int bit = 0; bit < 8; bit++)
                    value = (value & 0x80000000U) != 0 ? (value << 1) ^ 0x04C11DB7U : value << 1;
                table[i] = value;
            }
            return table;
        }

        private static uint ComputeGameCrc32(string value)
        {
            uint crc = 0xFFFFFFFFU;
            foreach (byte character in Encoding.ASCII.GetBytes(value))
            {
                int lookupIndex = (int)(((crc >> 24) ^ character) & 0xFFU);
                crc = (crc << 8) ^ GameCrcTable[lookupIndex];
            }
            return ~crc;
        }

        private void RefreshEntryList(int selectedEntryIndex)
        {
            changingSelection = true;
            entryListBox.BeginUpdate();
            try
            {
                entryListBox.Items.Clear();
                visibleEntryIndices.Clear();
                loadedEntryIndex = -1;

                if (fileState != null)
                {
                    for (int i = 0; i < fileState.Document.Entries.Count; i++)
                    {
                        CommandListParamEntry entry = fileState.Document.Entries[i];
                        if (!EntryMatchesSearch(entry, i, activeSearchText))
                            continue;

                        visibleEntryIndices.Add(i);
                        entryListBox.Items.Add(BuildEntryLabel(entry, i));
                    }

                    if (visibleEntryIndices.Count > 0)
                    {
                        int selectedListIndex = visibleEntryIndices.IndexOf(selectedEntryIndex);
                        entryListBox.SelectedIndex = selectedListIndex >= 0 ? selectedListIndex : 0;
                    }
                }
            }
            finally
            {
                entryListBox.EndUpdate();
                changingSelection = false;
            }

            LoadEntryToEditor(GetEntryIndex(entryListBox.SelectedIndex));
        }

        private int GetEntryIndex(int listIndex)
        {
            return listIndex >= 0 && listIndex < visibleEntryIndices.Count
                ? visibleEntryIndices[listIndex]
                : -1;
        }

        private bool EntryMatchesSearch(CommandListParamEntry entry, int index, string searchText)
        {
            string query = (searchText ?? string.Empty).Trim();
            if (query.Length == 0)
                return true;

            return BuildEntryLabel(entry, index).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                ContainsText(entry.ButtonPressText, query) ||
                HashMatchesSearch(entry.CommandLink, query) ||
                HashMatchesSearch(entry.CharacterName, query) ||
                HashMatchesSearch(entry.Characode, query) ||
                HashMatchesSearch(entry.AttackName, query) ||
                HashMatchesSearch(entry.ButtonPress, query);
        }

        private static bool ContainsText(string value, string query)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool HashMatchesSearch(uint value, string query)
        {
            return value.ToString("X8", CultureInfo.InvariantCulture)
                .IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string BuildEntryLabel(CommandListParamEntry entry, int index)
        {
            return string.Format("[{0:D4}] {1} | Chara {2} | Character {3} | Attack {4} | Costume {5}",
                index,
                GetCommandLinkLabel(entry),
                GetCharacodeLabel(entry),
                GetMessageFieldLabel(entry.CharacterName, entry.CharacterNameText),
                GetMessageFieldLabel(entry.AttackName, entry.AttackNameText),
                entry.CostumeIndex);
        }

        private static string GetMessageFieldLabel(uint hashValue, string resolvedText)
        {
            return !string.IsNullOrWhiteSpace(resolvedText)
                ? resolvedText
                : hashValue.ToString("X8", CultureInfo.InvariantCulture);
        }

        private static string GetCommandLinkLabel(CommandListParamEntry entry)
        {
            return !string.IsNullOrWhiteSpace(entry.CommandLinkName)
                ? entry.CommandLinkName
                : entry.CommandLink.ToString("X8", CultureInfo.InvariantCulture);
        }

        private static string GetCharacodeLabel(CommandListParamEntry entry)
        {
            return !string.IsNullOrWhiteSpace(entry.CharacodeName)
                ? entry.CharacodeName
                : entry.Characode.ToString("X8", CultureInfo.InvariantCulture);
        }

        private void LoadEntryToEditor(int index)
        {
            loadedEntryIndex = index;
            if (fileState == null || index < 0 || index >= fileState.Document.Entries.Count)
            {
                ClearEntryEditor();
                return;
            }

            CommandListParamEntry entry = fileState.Document.Entries[index];
            string characodeName = GetCharacodeLabel(entry);
            string commandLinkName = GetCommandLinkLabel(entry);
            entryEditorPanel.Text = "Selected entry - " + commandLinkName;
            characodeLabel.Text = "Characode: " + characodeName;
            commandLinkTextBox.Text = commandLinkName;
            commandListIndexNumericUpDown.Value = entry.CommandListIndex;
            characterNameTextBox.Text = GetMessageFieldLabel(entry.CharacterName, entry.CharacterNameText);
            characodeTextBox.Text = characodeName;
            costumeIndexNumericUpDown.Value = entry.CostumeIndex;
            attackNameHashTextBox.Text = FormatHashBytes(entry.AttackName);
            attackNameTextBox.Text = entry.AttackNameText;
            buttonPressHashTextBox.Text = FormatHashBytes(entry.ButtonPress);
            buttonPressTextBox.Text = entry.ButtonPressText;
            ClearButtonFormulaSearch();
            SetComboValue(commandType1ComboBox, entry.CommandType1);
            SetComboValue(commandTypeSkillComboBox, entry.CommandTypeSkill);
            SetComboValue(commandTypeAwakeComboBox, entry.CommandTypeAwake);
            SetComboValue(commandTypeTeamComboBox, entry.CommandTypeTeam);
            SetComboValue(commandType5ComboBox, entry.CommandType5);
            commandType6NumericUpDown.Value = entry.CommandType6;
            entryEditorPanel.Enabled = true;
        }

        private bool SaveCurrentEntry()
        {
            int type1;
            int typeSkill;
            int typeAwake;
            int typeTeam;
            int type5;
            CommandListParamEntry currentEntry = fileState.Document.Entries[loadedEntryIndex];
            uint commandLinkHash;
            string commandLinkName;
            uint characodeHash;
            string characodeName;
            uint characterNameHash;
            string characterNameText;
            uint attackNameHash;
            string attackNameText;
            uint buttonPressHash;
            string buttonPressText;

            ConvertTextToHash(commandLinkTextBox.Text, currentEntry.CommandLink, currentEntry.CommandLinkName,
                out commandLinkHash, out commandLinkName);
            ConvertTextToHash(characodeTextBox.Text, currentEntry.Characode, currentEntry.CharacodeName,
                out characodeHash, out characodeName);
            ConvertTextToHash(characterNameTextBox.Text, currentEntry.CharacterName, currentEntry.CharacterNameText,
                out characterNameHash, out characterNameText);

            if (!TryReadComboValue(commandType1ComboBox, out type1) ||
                !TryReadComboValue(commandTypeSkillComboBox, out typeSkill) ||
                !TryReadComboValue(commandTypeAwakeComboBox, out typeAwake) ||
                !TryReadComboValue(commandTypeTeamComboBox, out typeTeam) ||
                !TryReadComboValue(commandType5ComboBox, out type5) ||
                !TryReadRawHashBytes(attackNameHashTextBox.Text, out attackNameHash) ||
                !TryReadRawHashBytes(buttonPressHashTextBox.Text, out buttonPressHash))
            {
                MessageBox.Show(this, "Command types must be signed integers. Attack Name and Button Press must contain four raw bytes as eight hex digits.",
                    "Invalid entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            attackNameText = ResolveMessageText(attackNameHash, currentEntry.AttackName, currentEntry.AttackNameText);
            buttonPressText = ResolveMessageText(buttonPressHash, currentEntry.ButtonPress, currentEntry.ButtonPressText);

            fileState.Document.Entries[loadedEntryIndex] = new CommandListParamEntry
            {
                CommandLink = commandLinkHash,
                CommandLinkName = commandLinkName,
                CommandListIndex = (int)commandListIndexNumericUpDown.Value,
                CharacterName = characterNameHash,
                CharacterNameText = characterNameText,
                Characode = characodeHash,
                CharacodeName = characodeName,
                CostumeIndex = (int)costumeIndexNumericUpDown.Value,
                AttackName = attackNameHash,
                AttackNameText = attackNameText,
                ButtonPress = buttonPressHash,
                ButtonPressText = buttonPressText,
                CommandType1 = type1,
                CommandTypeSkill = typeSkill,
                CommandTypeAwake = typeAwake,
                CommandTypeTeam = typeTeam,
                CommandType5 = type5,
                CommandType6 = (int)commandType6NumericUpDown.Value
            };

            RefreshEntryList(loadedEntryIndex);
            return true;
        }

        private static string FormatHashBytes(uint hashValue)
        {
            return ReverseByteOrder(hashValue).ToString("X8", CultureInfo.InvariantCulture);
        }

        private static bool TryReadRawHashBytes(string input, out uint hashValue)
        {
            string text = (input ?? string.Empty).Trim();
            if (text.Length == 0)
            {
                hashValue = 0;
                return true;
            }

            uint byteValue;
            if (text.Length != 8 ||
                !uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byteValue))
            {
                hashValue = 0;
                return false;
            }

            hashValue = ReverseByteOrder(byteValue);
            return true;
        }

        private static uint ReverseByteOrder(uint value)
        {
            return (value >> 24) |
                   ((value >> 8) & 0x0000FF00U) |
                   ((value << 8) & 0x00FF0000U) |
                   (value << 24);
        }

        private void ClearButtonFormulaSearch()
        {
            buttonFormulaSequence.Clear();
            selectedButtonFormulaNode = -1;
            RefreshButtonFormulaNodes();
            buttonFormulaResultsComboBox.Items.Clear();
            buttonFormulaResultsComboBox.Enabled = false;
        }

        private void PopulateButtonFormulaOptions()
        {
            List<ButtonFormulaOption> options = ButtonFormulaNames
                .Select(input => new ButtonFormulaOption
                {
                    FormulaPart = input.Key,
                    DisplayName = input.Value
                })
                .OrderBy(x => x.DisplayName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            buttonFormulaInputComboBox.Items.Clear();
            foreach (ButtonFormulaOption option in options)
                buttonFormulaInputComboBox.Items.Add(option);

            buttonFormulaInputComboBox.SelectedIndex = 0;
        }

        private void AddButtonFormulaInput()
        {
            ButtonFormulaOption option = buttonFormulaInputComboBox.SelectedItem as ButtonFormulaOption;
            if (option == null)
                return;

            buttonFormulaSequence.Add(option);
            selectedButtonFormulaNode = buttonFormulaSequence.Count - 1;
            RefreshButtonFormulaNodes();
            buttonFormulaResultsComboBox.Items.Clear();
            buttonFormulaResultsComboBox.Enabled = false;
        }

        private void RemoveButtonFormulaInput()
        {
            if (buttonFormulaSequence.Count == 0)
                return;

            int index = selectedButtonFormulaNode >= 0
                ? selectedButtonFormulaNode
                : buttonFormulaSequence.Count - 1;
            buttonFormulaSequence.RemoveAt(index);
            selectedButtonFormulaNode = Math.Min(index, buttonFormulaSequence.Count - 1);
            RefreshButtonFormulaNodes();
            buttonFormulaResultsComboBox.Items.Clear();
            buttonFormulaResultsComboBox.Enabled = false;
        }

        private void RefreshButtonFormulaNodes()
        {
            buttonFormulaFlowLayoutPanel.SuspendLayout();
            while (buttonFormulaFlowLayoutPanel.Controls.Count > 0)
            {
                Control control = buttonFormulaFlowLayoutPanel.Controls[0];
                buttonFormulaFlowLayoutPanel.Controls.RemoveAt(0);
                control.Dispose();
            }
            for (int i = 0; i < buttonFormulaSequence.Count; i++)
            {
                if (i > 0)
                {
                    buttonFormulaFlowLayoutPanel.Controls.Add(new Label
                    {
                        AutoSize = true,
                        Margin = new Padding(1, 8, 1, 0),
                        Text = "→"
                    });
                }

                ButtonFormulaOption option = buttonFormulaSequence[i];
                Button node = new Button
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = i == selectedButtonFormulaNode
                        ? System.Drawing.SystemColors.Highlight
                        : System.Drawing.SystemColors.Control,
                    ForeColor = i == selectedButtonFormulaNode
                        ? System.Drawing.SystemColors.HighlightText
                        : System.Drawing.SystemColors.ControlText,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(2),
                    Padding = new Padding(6, 2, 6, 2),
                    Tag = i,
                    Text = option.DisplayName
                };
                node.Click += buttonFormulaNode_Click;
                buttonFormulaFlowLayoutPanel.Controls.Add(node);
            }
            buttonFormulaFlowLayoutPanel.ResumeLayout();
        }

        private void buttonFormulaNode_Click(object sender, EventArgs e)
        {
            selectedButtonFormulaNode = (int)((Button)sender).Tag;
            RefreshButtonFormulaNodes();
        }

        private void SearchButtonFormula()
        {
            if (messageTextByHash.Count == 0)
            {
                MessageBox.Show(this, "Load messageInfo.bin.xfbin before searching button formulas.",
                    "Button Formula Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (buttonFormulaSequence.Count == 0)
            {
                MessageBox.Show(this, "Add at least one input node before searching.", "Button Formula Search",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string formula = string.Concat(buttonFormulaSequence.Select(x => BuildFormulaPart(x.FormulaPart)));
            List<MessageHashMatch> matches = messageTextByHash
                .Where(x => MatchesButtonFormula(x.Value, formula))
                .OrderBy(x => x.Key)
                .Select(x => new MessageHashMatch { Hash = x.Key, Text = x.Value })
                .ToList();

            buttonFormulaResultsComboBox.Items.Clear();
            foreach (MessageHashMatch match in matches)
                buttonFormulaResultsComboBox.Items.Add(match);
            buttonFormulaResultsComboBox.Enabled = matches.Count > 0;

            if (matches.Count == 0)
            {
                statusLabel.Text = "No messageInfo hash matches the button formula.";
                MessageBox.Show(this, "No messageInfo entry contains that icon sequence.",
                    "Button Formula Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            buttonFormulaResultsComboBox.SelectedIndex = 0;
            statusLabel.Text = matches.Count == 1
                ? "Found and copied one Button Press hash. Click Save to apply it to the entry."
                : string.Format("Found {0} Button Press hashes. Select the required match, then click Save.", matches.Count);
        }

        private static string BuildFormulaPart(string formulaPart)
        {
            return formulaPart.IndexOf('<') >= 0
                ? formulaPart
                : "<icon " + formulaPart + " />";
        }

        private static bool MatchesButtonFormula(string message, string formula)
        {
            if (!message.StartsWith(formula, StringComparison.OrdinalIgnoreCase))
                return false;

            string remainder = message.Substring(formula.Length).TrimStart();
            return remainder.IndexOf("<icon", StringComparison.OrdinalIgnoreCase) < 0;
        }

        private void ApplySelectedButtonFormulaMatch()
        {
            MessageHashMatch match = buttonFormulaResultsComboBox.SelectedItem as MessageHashMatch;
            if (match == null)
                return;

            buttonPressHashTextBox.Text = FormatHashBytes(match.Hash);
            buttonPressTextBox.Text = match.Text;
        }

        private string ResolveMessageText(uint hashValue, uint currentHash, string currentText)
        {
            if (hashValue == 0)
                return string.Empty;

            string resolvedText;
            if (messageTextByHash.TryGetValue(hashValue, out resolvedText))
                return resolvedText;
            return hashValue == currentHash ? currentText ?? string.Empty : string.Empty;
        }

        private static void ConvertTextToHash(string input, uint currentHash, string currentResolvedName,
            out uint hashValue, out string resolvedName)
        {
            string text = (input ?? string.Empty).Trim();
            string currentDisplay = !string.IsNullOrWhiteSpace(currentResolvedName)
                ? currentResolvedName
                : currentHash.ToString("X8", CultureInfo.InvariantCulture);
            if (string.Equals(text, currentDisplay, StringComparison.Ordinal))
            {
                hashValue = currentHash;
                resolvedName = currentResolvedName;
                return;
            }

            if (text.Length == 0)
            {
                hashValue = 0;
                resolvedName = string.Empty;
                return;
            }

            string hexToken = text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                ? text.Substring(2)
                : text;
            if (hexToken.Length == 8)
            {
                uint rawHash;
                if (uint.TryParse(hexToken, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out rawHash))
                {
                    hashValue = rawHash;
                    resolvedName = string.Empty;
                    return;
                }
            }

            hashValue = ComputeGameCrc32(text);
            resolvedName = text;
        }

        private static bool TryReadComboValue(ComboBox comboBox, out int value)
        {
            string text = (comboBox.Text ?? string.Empty).Trim();
            int separator = text.IndexOf(' ');
            string token = separator >= 0 ? text.Substring(0, separator) : text;
            return int.TryParse(token, out value);
        }

        private static void SetComboValue(ComboBox comboBox, int value)
        {
            string prefix = value.ToString();
            foreach (object item in comboBox.Items)
            {
                string text = item == null ? string.Empty : item.ToString();
                if (text == prefix || text.StartsWith(prefix + " ", StringComparison.Ordinal))
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
            comboBox.SelectedIndex = -1;
            comboBox.Text = prefix;
        }

        private void AddEntry()
        {
            activeSearchText = string.Empty;
            searchTextBox.Clear();
            fileState.Document.Entries.Add(new CommandListParamEntry
            {
                CommandListIndex = fileState.Document.Entries.Count,
                CommandType5 = -1,
                CommandType6 = -1
            });
            RefreshEntryList(fileState.Document.Entries.Count - 1);
            statusLabel.Text = "Entry added. Save the XFBIN to persist the change.";
        }

        private void DuplicateEntry()
        {
            if (loadedEntryIndex < 0)
                return;

            fileState.Document.Entries.Add(fileState.Document.Entries[loadedEntryIndex].Clone());
            RefreshEntryList(fileState.Document.Entries.Count - 1);
            statusLabel.Text = "Entry duplicated. Save the XFBIN to persist the change.";
        }

        private void DeleteEntry()
        {
            if (loadedEntryIndex < 0)
                return;

            int index = loadedEntryIndex;
            fileState.Document.Entries.RemoveAt(index);
            RefreshEntryList(Math.Min(index, fileState.Document.Entries.Count - 1));
            statusLabel.Text = "Entry deleted. Save the XFBIN to persist the change.";
        }

        private void CloneCharacterEntries(string sourceCharacode, string targetCharacode)
        {
            try
            {
                CharacterClonePlan plan = CreateCharacterClonePlan(
                    sourceCharacode,
                    targetCharacode);

                if (plan.Entries.Count == 0)
                {
                    MessageBox.Show(this,
                        string.Format("No entries were added. All {0} matching target Command Links already exist.", plan.MatchedEntries),
                        "Clone Character Entries", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string prompt = string.Format(
                    "Append {0} cloned entries for {1} -> {2}?\r\n\r\nThis includes resolved costume variants and preserves their existing order.",
                    plan.Entries.Count, plan.SourceCharacode, plan.TargetCharacode);
                if (plan.SkippedExistingEntries > 0)
                    prompt += string.Format("\r\n\r\n{0} target Command Links already exist and will be skipped.", plan.SkippedExistingEntries);
                if (MessageBox.Show(this, prompt, "Clone Character Entries", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                int firstNewIndex = fileState.Document.Entries.Count;
                foreach (CommandListParamEntry entry in plan.Entries)
                    generatedCommandNamesByHash[entry.CommandLink] = entry.CommandLinkName;
                fileState.Document.Entries.AddRange(plan.Entries);
                activeSearchText = string.Empty;
                searchTextBox.Clear();
                RefreshEntryList(firstNewIndex);
                statusLabel.Text = string.Format(
                    "Appended {0} entries for {1}. Use File > Save to write the XFBIN.",
                    plan.Entries.Count, plan.TargetCharacode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Clone Character Entries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private CharacterClonePlan CreateCharacterClonePlan(string sourceInput, string targetInput)
        {
            string source = FindCanonicalCharacode(sourceInput);
            string target = FindCanonicalCharacode(targetInput);
            if (string.Equals(source, target, StringComparison.Ordinal))
                throw new InvalidOperationException("Source and clone Characodes must be different.");

            ResolveCommandLinkNames();
            uint sourceCharacodeHash = ComputeGameCrc32(source);
            uint targetCharacodeHash = ComputeGameCrc32(target);
            HashSet<uint> knownCommandLinks = new HashSet<uint>(fileState.Document.Entries.Select(x => x.CommandLink));
            CharacterClonePlan plan = new CharacterClonePlan
            {
                SourceCharacode = source,
                TargetCharacode = target
            };

            foreach (CommandListParamEntry sourceEntry in fileState.Document.Entries)
            {
                string targetCommandName;
                if (!TryBuildCloneCommandName(sourceEntry.CommandLinkName, source, target, out targetCommandName))
                    continue;

                plan.MatchedEntries++;
                uint targetCommandHash = ComputeGameCrc32(targetCommandName);
                if (!knownCommandLinks.Add(targetCommandHash))
                {
                    plan.SkippedExistingEntries++;
                    continue;
                }

                CommandListParamEntry clone = sourceEntry.Clone();
                clone.CommandLink = targetCommandHash;
                clone.CommandLinkName = targetCommandName;
                if (clone.Characode == sourceCharacodeHash)
                {
                    clone.Characode = targetCharacodeHash;
                    clone.CharacodeName = target;
                }
                plan.Entries.Add(clone);
            }

            if (plan.MatchedEntries == 0)
                throw new InvalidOperationException("No resolved Command Links were found for " + source + ".");
            return plan;
        }

        private string FindCanonicalCharacode(string input)
        {
            string value = (input ?? string.Empty).Trim();
            string canonical = characodeIds.FirstOrDefault(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
            if (canonical == null)
                throw new InvalidOperationException("Characode '" + value + "' was not found in the loaded characode file.");
            return canonical;
        }

        private static bool TryBuildCloneCommandName(string commandName, string sourceCharacode,
            string targetCharacode, out string targetCommandName)
        {
            targetCommandName = string.Empty;
            if (string.IsNullOrWhiteSpace(commandName))
                return false;

            int commandMarker = commandName.LastIndexOf("_command_", StringComparison.Ordinal);
            if (commandMarker <= 0)
                return false;

            string commandPrefix = commandName.Substring(0, commandMarker);
            if (!string.Equals(commandPrefix, sourceCharacode, StringComparison.Ordinal) &&
                !commandPrefix.StartsWith(sourceCharacode + "_", StringComparison.Ordinal))
                return false;

            targetCommandName = targetCharacode + commandName.Substring(sourceCharacode.Length);
            return true;
        }

        private void AddCostumeEntries(string characodeInput, int totalCostumes)
        {
            try
            {
                CostumeEntryPlan plan = CreateCostumeEntryPlan(characodeInput, totalCostumes);
                if (plan.Entries.Count == 0)
                {
                    MessageBox.Show(this,
                        string.Format("No entries were added. All Command Links through costume {0} already exist.", plan.TotalCostumes - 1),
                        "Add Costume Entries", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string prompt = string.Format(
                    "Append {0} costume entries for {1}?\r\n\r\nThis creates enough variants for {2} total costumes, including the base costume.",
                    plan.Entries.Count, plan.Characode, plan.TotalCostumes);
                if (plan.SkippedExistingEntries > 0)
                    prompt += string.Format("\r\n\r\n{0} Command Links already exist and will be skipped.", plan.SkippedExistingEntries);
                if (MessageBox.Show(this, prompt, "Add Costume Entries", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                int firstNewIndex = fileState.Document.Entries.Count;
                foreach (CommandListParamEntry entry in plan.Entries)
                    generatedCommandNamesByHash[entry.CommandLink] = entry.CommandLinkName;
                fileState.Document.Entries.AddRange(plan.Entries);
                activeSearchText = string.Empty;
                searchTextBox.Clear();
                RefreshEntryList(firstNewIndex);
                statusLabel.Text = string.Format(
                    "Appended {0} costume entries for {1}. Use File > Save to write the XFBIN.",
                    plan.Entries.Count, plan.Characode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Add Costume Entries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private CostumeEntryPlan CreateCostumeEntryPlan(string characodeInput, int totalCostumes)
        {
            if (totalCostumes < 1 || totalCostumes > MaximumCostumeNumber)
                throw new InvalidOperationException("Total costumes must be between 1 and 20.");

            string characode = FindCanonicalCharacode(characodeInput);
            ResolveCommandLinkNames();
            List<CommandListParamEntry> baseEntries = fileState.Document.Entries
                .Where(x => IsCostumeTemplateCommandName(x.CommandLinkName, characode))
                .ToList();
            if (baseEntries.Count == 0)
                throw new InvalidOperationException("No resolved base Command Links were found for " + characode + ".");

            HashSet<uint> knownCommandLinks = new HashSet<uint>(fileState.Document.Entries.Select(x => x.CommandLink));
            CostumeEntryPlan plan = new CostumeEntryPlan
            {
                Characode = characode,
                TotalCostumes = totalCostumes
            };

            for (int costumeIndex = 1; costumeIndex < totalCostumes; costumeIndex++)
            {
                foreach (CommandListParamEntry baseEntry in baseEntries)
                {
                    string costumeCommandName = BuildCostumeCommandName(baseEntry.CommandLinkName, characode, costumeIndex);
                    uint costumeCommandHash = ComputeGameCrc32(costumeCommandName);
                    if (!knownCommandLinks.Add(costumeCommandHash))
                    {
                        plan.SkippedExistingEntries++;
                        continue;
                    }

                    CommandListParamEntry costumeEntry = baseEntry.Clone();
                    costumeEntry.CommandLink = costumeCommandHash;
                    costumeEntry.CommandLinkName = costumeCommandName;
                    costumeEntry.CostumeIndex = costumeIndex;
                    plan.Entries.Add(costumeEntry);
                }
            }
            return plan;
        }

        private static bool IsCostumeTemplateCommandName(string commandName, string characode)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                return false;
            int marker = commandName.LastIndexOf("_command_", StringComparison.Ordinal);
            if (marker <= 0)
                return false;
            string prefix = commandName.Substring(0, marker);
            return string.Equals(prefix, characode, StringComparison.Ordinal) ||
                string.Equals(prefix, characode + "_x", StringComparison.Ordinal);
        }

        private static string BuildCostumeCommandName(string baseCommandName, string characode, int costumeIndex)
        {
            int marker = baseCommandName.LastIndexOf("_command_", StringComparison.Ordinal);
            string prefix = baseCommandName.Substring(0, marker);
            string costumePrefix = string.Equals(prefix, characode + "_x", StringComparison.Ordinal)
                ? string.Format(CultureInfo.InvariantCulture, "{0}_x_{1:D2}", characode, costumeIndex)
                : string.Format(CultureInfo.InvariantCulture, "{0}_{1:D2}", characode, costumeIndex);
            return costumePrefix + baseCommandName.Substring(marker);
        }

        private void SaveFile(bool saveAs)
        {
            string outputPath = sourceFilePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = "xfbin";
                    dialog.Filter = "XFBIN files (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                    dialog.FileName = Path.GetFileName(sourceFilePath);
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;
                    outputPath = dialog.FileName;
                }
            }

            try
            {
                using (XfbinParserBackend backend = new XfbinParserBackend(sourceFilePath))
                {
                    backend.UpsertChunk(fileState.ChunkName, fileState.ChunkName, "nuccChunkBinary", fileState.ChunkPath,
                        ".binary", BuildCommandListChunk(fileState.Document), fileState.Version, fileState.VersionAttribute);
                    backend.RepackTo(outputPath);
                }

                if (!File.Exists(outputPath))
                    throw new IOException("The parser did not create the output XFBIN.");

                sourceFilePath = outputPath;
                statusLabel.Text = "Saved " + Path.GetFileName(outputPath) + ".";
                Text = "Command List Param Editor - " + Path.GetFileName(outputPath);
                MessageBox.Show(this,
                    "File saved successfully." + Environment.NewLine + Environment.NewLine + Path.GetFullPath(outputPath),
                    "Command List Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not save the XFBIN." + Environment.NewLine + Environment.NewLine + ex.Message,
                    "Command List Param Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void entryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changingSelection)
                return;

            LoadEntryToEditor(GetEntryIndex(entryListBox.SelectedIndex));
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            activeSearchText = searchTextBox.Text;
            RefreshEntryList(loadedEntryIndex);
            statusLabel.Text = string.Format("Showing {0} of {1} entries.",
                visibleEntryIndices.Count, fileState.Document.Entries.Count);
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
            Close();
        }

        private void loadCharacodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCharacodeReferences();
        }

        private void loadMessageInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMessageInfoReferences();
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

        private void saveEntryButton_Click(object sender, EventArgs e)
        {
            if (SaveCurrentEntry())
                statusLabel.Text = "Entry saved in editor state. Use File > Save to write the XFBIN.";
        }

        private void buttonFormulaSearchButton_Click(object sender, EventArgs e)
        {
            SearchButtonFormula();
        }

        private void buttonFormulaAddButton_Click(object sender, EventArgs e)
        {
            AddButtonFormulaInput();
        }

        private void buttonFormulaRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveButtonFormulaInput();
        }

        private void buttonFormulaClearButton_Click(object sender, EventArgs e)
        {
            ClearButtonFormulaSearch();
        }

        private void buttonFormulaResultsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySelectedButtonFormulaMatch();
        }

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Tool_CommandListCloneDialog dialog = new Tool_CommandListCloneDialog(characodeIds))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    CloneCharacterEntries(dialog.SourceCharacode, dialog.TargetCharacode);
            }
        }

        private void costumesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Tool_CommandListCostumeDialog dialog = new Tool_CommandListCostumeDialog(characodeIds))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    AddCostumeEntries(dialog.Characode, dialog.TotalCostumes);
            }
        }

    }
}
