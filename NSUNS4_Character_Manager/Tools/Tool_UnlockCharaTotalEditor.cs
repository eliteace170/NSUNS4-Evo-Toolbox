using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public class Tool_UnlockCharaTotalEditor : Form
    {
        private const int FileSectionSizeOffsetA = -12;
        private const int FileSizeOffset = 0;
        private const int VersionOffset = 4;
        private const int CountOffset = 8;
        private const int EntryOffset = 20;
        private const int EntrySize = 12;
        private const int FooterSize = 20;
        private const int DefaultVersion = 0;
        private const int DefaultRyo = 1;
        private const int DefaultCondition = 1;

        private static readonly byte[] DefaultXfbinTemplate = new byte[]
        {
            78, 85, 67, 67, 0, 0, 0, 121, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 228, 0, 0, 0, 3, 0, 121, 0, 0, 0, 0, 0, 4,
            0, 0, 0, 59, 0, 0, 0, 2, 0, 0, 0, 33, 0, 0, 0, 4,
            0, 0, 0, 30, 0, 0, 0, 4, 0, 0, 0, 48, 0, 0, 0, 4,
            0, 0, 0, 0, 110, 117, 99, 99, 67, 104, 117, 110, 107, 78, 117, 108,
            108, 0, 110, 117, 99, 99, 67, 104, 117, 110, 107, 66, 105, 110, 97, 114,
            121, 0, 110, 117, 99, 99, 67, 104, 117, 110, 107, 80, 97, 103, 101, 0,
            110, 117, 99, 99, 67, 104, 117, 110, 107, 73, 110, 100, 101, 120, 0, 0,
            98, 105, 110, 95, 108, 101, 47, 120, 54, 52, 47, 117, 110, 108, 111, 99,
            107, 67, 104, 97, 114, 97, 84, 111, 116, 97, 108, 46, 98, 105, 110, 0,
            0, 117, 110, 108, 111, 99, 107, 67, 104, 97, 114, 97, 84, 111, 116, 97,
            108, 0, 80, 97, 103, 101, 48, 0, 105, 110, 100, 101, 120, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0,
            0, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0,
            3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0,
            3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 121, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 121, 0, 0, 0, 0, 5, 252, 0, 0, 0,
            1, 0, 121, 0, 0, 0, 0, 5, 248, 233, 3, 0, 0, 0, 0, 0,
            0, 8, 0, 0, 0, 0, 0, 0, 0
        };

        public bool FileOpen = false;
        public string FilePath = "";
        public byte[] FileBytes = new byte[0];

        private readonly List<UnlockEntry> entries = new List<UnlockEntry>();
        private readonly List<ConditionOption> conditionOptions = new List<ConditionOption>();

        private IContainer components = null;
        private ListBox entryListBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem sortToolStripMenuItem;
        private ToolStripMenuItem sortPresetToolStripMenuItem;
        private ToolStripMenuItem displayToolStripMenuItem;
        private ToolStripMenuItem displayHexToolStripMenuItem;
        private ToolStripMenuItem displayDecToolStripMenuItem;
        private Label presetLabel;
        private Label ryoLabel;
        private Label conditionLabel;
        private NumericUpDown presetInput;
        private NumericUpDown ryoInput;
        private ComboBox conditionInput;
        private Button addButton;
        private Button updateButton;
        private Button removeButton;
        private bool displayAsHex = true;

        private sealed class UnlockEntry
        {
            public int PlayerSettingPreset;
            public int Ryo;
            public int UnlockCondition;
        }

        private sealed class ConditionOption
        {
            public int Value;
            public string Name;

            public override string ToString()
            {
                return string.Format("0x{0:X2} - {1}", Value, Name);
            }
        }

        public Tool_UnlockCharaTotalEditor()
        {
            InitializeComponent();
            BuildConditionOptions();
            BindConditions(conditionInput);
            ryoInput.Value = DefaultRyo;
            ApplyDisplayMode(true);
        }

        public void NewFile()
        {
            FileBytes = (byte[])DefaultXfbinTemplate.Clone();
            FilePath = "";
            FileOpen = true;
            entries.Clear();
            RefreshEntryList();
        }

        public void OpenFile(string path = "")
        {
            if (FileOpen)
            {
                CloseFile();
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xfbin";
            dialog.Filter = "*.xfbin|*.xfbin";

            if (path != "")
                dialog.FileName = path;
            else
                dialog.ShowDialog();

            if (!(dialog.FileName != "") || !File.Exists(dialog.FileName))
            {
                return;
            }

            byte[] bytes = File.ReadAllBytes(dialog.FileName);
            int fileSectionIndex = XfbinParser.GetFileSectionIndex(bytes);
            if (fileSectionIndex < 0 || fileSectionIndex + EntryOffset > bytes.Length)
            {
                MessageBox.Show("Please select a valid unlockCharaTotal file.");
                return;
            }

            FileOpen = true;
            FilePath = dialog.FileName;
            FileBytes = bytes;
            ReadEntries();
            RefreshEntryList();
        }

        public void AddImportEntry(int presetId, int unlockCondition)
        {
            EnsureOpenForProgrammaticEdit();
            entries.Add(new UnlockEntry
            {
                PlayerSettingPreset = presetId,
                Ryo = DefaultRyo,
                UnlockCondition = unlockCondition
            });
        }

        public byte[] ConvertToFile()
        {
            EnsureOpenForProgrammaticEdit();

            int fileSectionIndex = XfbinParser.GetFileSectionIndex(FileBytes);
            if (fileSectionIndex < 0)
            {
                fileSectionIndex = 292;
            }

            byte[] output = new byte[fileSectionIndex + EntryOffset + (entries.Count * EntrySize) + FooterSize];
            Array.Copy(FileBytes, 0, output, 0, Math.Min(fileSectionIndex + EntryOffset, FileBytes.Length));

            WriteIntBigEndian(output, fileSectionIndex + FileSectionSizeOffsetA, (entries.Count * EntrySize) + EntryOffset);
            WriteIntBigEndian(output, fileSectionIndex + FileSizeOffset, (entries.Count * EntrySize) + 16);
            WriteIntLittleEndian(output, fileSectionIndex + VersionOffset, ReadVersion());
            WriteIntLittleEndian(output, fileSectionIndex + CountOffset, entries.Count);
            WriteLongLittleEndian(output, fileSectionIndex + 12, 0L);

            for (int i = 0; i < entries.Count; i++)
            {
                int entryOffset = fileSectionIndex + EntryOffset + (i * EntrySize);
                WriteEntry(output, entryOffset, entries[i]);
            }

            byte[] footer = GetFooterBytes();
            Array.Copy(footer, 0, output, output.Length - FooterSize, FooterSize);
            return output;
        }

        public void SaveFile()
        {
            if (FilePath != "")
            {
                if (File.Exists(FilePath + ".backup"))
                    File.Delete(FilePath + ".backup");

                if (File.Exists(FilePath))
                    File.Copy(FilePath, FilePath + ".backup");

                File.WriteAllBytes(FilePath, ConvertToFile());
                MessageBox.Show("File saved to " + FilePath + ".");
            }
            else
            {
                SaveFileAs();
            }
        }

        public void SaveFileAs(string path = "")
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xfbin";
            dialog.Filter = "*.xfbin|*.xfbin";

            if (path != "")
                dialog.FileName = path;
            else
                dialog.ShowDialog();

            if (!(dialog.FileName != ""))
            {
                return;
            }

            FilePath = dialog.FileName;
            File.WriteAllBytes(FilePath, ConvertToFile());

            if (path == "")
            {
                MessageBox.Show("File saved to " + FilePath + ".");
            }
        }

        public void CloseFile()
        {
            entries.Clear();
            entryListBox.Items.Clear();
            entryListBox.Items.Add("No file loaded...");
            FileBytes = new byte[0];
            FilePath = "";
            FileOpen = false;
        }

        private void EnsureOpenForProgrammaticEdit()
        {
            if (!FileOpen)
            {
                NewFile();
            }
        }

        private void ReadEntries()
        {
            entries.Clear();

            int fileSectionIndex = XfbinParser.GetFileSectionIndex(FileBytes);
            int count = ReadIntLittleEndian(FileBytes, fileSectionIndex + CountOffset);
            int maxCount = (FileBytes.Length - FooterSize - (fileSectionIndex + EntryOffset)) / EntrySize;
            if (count < 0) count = 0;
            if (count > maxCount) count = maxCount;

            for (int i = 0; i < count; i++)
            {
                int entryOffset = fileSectionIndex + EntryOffset + (i * EntrySize);
                entries.Add(new UnlockEntry
                {
                    PlayerSettingPreset = ReadIntLittleEndian(FileBytes, entryOffset),
                    Ryo = ReadIntLittleEndian(FileBytes, entryOffset + 4),
                    UnlockCondition = ReadIntLittleEndian(FileBytes, entryOffset + 8)
                });
            }
        }

        private void RefreshEntryList(int selectedIndex = -1)
        {
            entryListBox.Items.Clear();

            if (entries.Count == 0)
            {
                if (!FileOpen)
                    entryListBox.Items.Add("No file loaded...");
                else
                    entryListBox.Items.Add("No entries found...");

                entryListBox.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                entryListBox.Items.Add(BuildEntryLabel(entries[i]));
            }

            if (selectedIndex < 0 || selectedIndex >= entries.Count)
                selectedIndex = 0;

            entryListBox.SelectedIndex = selectedIndex;
        }

        private void RefreshEntryListPreserveSelection()
        {
            int selectedIndex = entryListBox.SelectedIndex;
            RefreshEntryList(selectedIndex);
        }

        private string BuildEntryLabel(UnlockEntry entry)
        {
            return string.Format(
                "Preset: {0} | Ryo: {1} | Condition: {2}",
                FormatInt(entry.PlayerSettingPreset),
                FormatInt(entry.Ryo),
                GetConditionLabel(entry.UnlockCondition));
        }

        private string FormatInt(int value)
        {
            return displayAsHex ? string.Format("0x{0:X8}", value) : value.ToString();
        }

        private string GetConditionLabel(int conditionValue)
        {
            for (int i = 0; i < conditionOptions.Count; i++)
            {
                if (conditionOptions[i].Value == conditionValue)
                    return conditionOptions[i].ToString();
            }

            return string.Format("0x{0:X2} - Unknown", conditionValue);
        }

        private void BuildConditionOptions()
        {
            conditionOptions.Clear();
            AddCondition(0x00, "No_Condition");
            AddCondition(0x01, "Default_Unlocked_state");
            AddCondition(0x02, "Default_Locked_state");
            AddCondition(0x03, "EPISODE_00_00");
            AddCondition(0x04, "EPISODE_01_00");
            AddCondition(0x05, "EPISODE_01_01");
            AddCondition(0x06, "EPISODE_01_02");
            AddCondition(0x07, "EPISODE_01_04");
            AddCondition(0x08, "EPISODE_01_05");
            AddCondition(0x09, "EPISODE_01_06");
            AddCondition(0x0A, "EPISODE_01_07");
            AddCondition(0x0B, "EPISODE_02_00");
            AddCondition(0x0C, "EPISODE_02_01");
            AddCondition(0x0D, "EPISODE_02_02");
            AddCondition(0x0E, "EPISODE_02_03");
            AddCondition(0x0F, "EPISODE_03_00");
            AddCondition(0x10, "EPISODE_03_01");
            AddCondition(0x11, "EPISODE_03_02");
            AddCondition(0x12, "EPISODE_03_03");
            AddCondition(0x13, "EPISODE_03_04");
            AddCondition(0x14, "EPISODE_03_05");
            AddCondition(0x15, "EPISODE_04_00");
            AddCondition(0x16, "EPISODE_04_01");
            AddCondition(0x17, "EPISODE_04_02");
            AddCondition(0x18, "EPISODE_05_00");
            AddCondition(0x19, "EPISODE_05_01");
            AddCondition(0x1A, "EPISODE_05_02");
            AddCondition(0x1B, "EPISODE_06_00");
            AddCondition(0x1C, "EPISODE_06_02");
            AddCondition(0x1D, "EPISODE_06_03");
            AddCondition(0x1E, "EPISODE_07_00");
            AddCondition(0x1F, "EPISODE_07_01");
            AddCondition(0x20, "EPISODE_07_02");
            AddCondition(0x21, "R2B_BATTLE_01");
            AddCondition(0x22, "R2B_BATTLE_03");
            AddCondition(0x23, "R2B_BATTLE_04");
            AddCondition(0x24, "R2B_BATTLE_07");
            AddCondition(0x25, "R2B_BATTLE_08");
            AddCondition(0x26, "BOSSBATTLE14_PHASE1_OR_PHASE2");
        }

        private void AddCondition(int value, string name)
        {
            ConditionOption option = new ConditionOption();
            option.Value = value;
            option.Name = name;
            conditionOptions.Add(option);
        }

        private void BindConditions(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            for (int i = 0; i < conditionOptions.Count; i++)
            {
                comboBox.Items.Add(conditionOptions[i]);
            }
            comboBox.SelectedIndex = 1;
        }

        private int GetSelectedCondition(ComboBox comboBox)
        {
            ConditionOption option = comboBox.SelectedItem as ConditionOption;
            return option != null ? option.Value : DefaultCondition;
        }

        private void SetSelectedCondition(ComboBox comboBox, int conditionValue)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                ConditionOption option = comboBox.Items[i] as ConditionOption;
                if (option != null && option.Value == conditionValue)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            comboBox.SelectedIndex = 0;
        }

        private int ReadVersion()
        {
            if (FileBytes.Length == 0)
                return DefaultVersion;

            int fileSectionIndex = XfbinParser.GetFileSectionIndex(FileBytes);
            if (fileSectionIndex < 0 || fileSectionIndex + VersionOffset + 4 > FileBytes.Length)
                return DefaultVersion;

            return ReadIntLittleEndian(FileBytes, fileSectionIndex + VersionOffset);
        }

        private byte[] GetFooterBytes()
        {
            if (FileBytes.Length >= FooterSize)
            {
                byte[] footer = new byte[FooterSize];
                Array.Copy(FileBytes, FileBytes.Length - FooterSize, footer, 0, FooterSize);
                return footer;
            }

            byte[] templateFooter = new byte[FooterSize];
            Array.Copy(DefaultXfbinTemplate, DefaultXfbinTemplate.Length - FooterSize, templateFooter, 0, FooterSize);
            return templateFooter;
        }

        private static int ReadIntLittleEndian(byte[] bytes, int offset)
        {
            return BitConverter.ToInt32(bytes, offset);
        }

        private static void WriteIntLittleEndian(byte[] bytes, int offset, int value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, bytes, offset, 4);
        }

        private static void WriteLongLittleEndian(byte[] bytes, int offset, long value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, bytes, offset, 8);
        }

        private static void WriteIntBigEndian(byte[] bytes, int offset, int value)
        {
            byte[] raw = BitConverter.GetBytes(value);
            Array.Reverse(raw);
            Array.Copy(raw, 0, bytes, offset, 4);
        }

        private static void WriteEntry(byte[] bytes, int offset, UnlockEntry entry)
        {
            WriteIntLittleEndian(bytes, offset, entry.PlayerSettingPreset);
            WriteIntLittleEndian(bytes, offset + 4, entry.Ryo);
            WriteIntLittleEndian(bytes, offset + 8, entry.UnlockCondition);
        }

        private void AddEntryFromInputs()
        {
            entries.Add(new UnlockEntry
            {
                PlayerSettingPreset = (int)presetInput.Value,
                Ryo = (int)ryoInput.Value,
                UnlockCondition = GetSelectedCondition(conditionInput)
            });

            RefreshEntryList(entries.Count - 1);
        }

        private void UpdateSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= entries.Count)
            {
                MessageBox.Show("No section selected.");
                return;
            }

            entries[index].PlayerSettingPreset = (int)presetInput.Value;
            entries[index].Ryo = (int)ryoInput.Value;
            entries[index].UnlockCondition = GetSelectedCondition(conditionInput);
            RefreshEntryList(index);
        }

        private void RemoveSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= entries.Count)
            {
                MessageBox.Show("No section selected.");
                return;
            }

            entries.RemoveAt(index);
            RefreshEntryList(Math.Min(index, entries.Count - 1));
        }

        private void LoadSelectedEntry()
        {
            int index = entryListBox.SelectedIndex;
            if (index < 0 || index >= entries.Count)
            {
                return;
            }

            UnlockEntry entry = entries[index];
            presetInput.Value = entry.PlayerSettingPreset;
            ryoInput.Value = entry.Ryo;
            SetSelectedCondition(conditionInput, entry.UnlockCondition);
        }

        private void ApplySortByPreset()
        {
            if (!FileOpen || entries.Count == 0)
            {
                return;
            }

            UnlockEntry selectedEntry = null;
            int selectedIndex = entryListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < entries.Count)
            {
                selectedEntry = entries[selectedIndex];
            }

            entries.Sort((a, b) => a.PlayerSettingPreset.CompareTo(b.PlayerSettingPreset));
            RefreshEntryList();

            if (selectedEntry != null)
            {
                int newIndex = entries.IndexOf(selectedEntry);
                if (newIndex >= 0)
                {
                    entryListBox.SelectedIndex = newIndex;
                }
            }

            UpdateSortMenuStates(sortPresetToolStripMenuItem);
        }

        private void ApplyDisplayMode(bool hexMode)
        {
            displayAsHex = hexMode;
            presetInput.Hexadecimal = hexMode;
            ryoInput.Hexadecimal = hexMode;
            UpdateDisplayMenuStates(hexMode ? displayHexToolStripMenuItem : displayDecToolStripMenuItem);
            RefreshEntryListPreserveSelection();
        }

        private void UpdateSortMenuStates(ToolStripMenuItem selectedItem)
        {
            sortPresetToolStripMenuItem.Checked = false;
            if (selectedItem != null)
            {
                selectedItem.Checked = true;
            }
        }

        private void UpdateDisplayMenuStates(ToolStripMenuItem selectedItem)
        {
            displayHexToolStripMenuItem.Checked = false;
            displayDecToolStripMenuItem.Checked = false;
            if (selectedItem != null)
            {
                selectedItem.Checked = true;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen) SaveFile(); else MessageBox.Show("No file loaded...");
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen) SaveFileAs(); else MessageBox.Show("No file loaded...");
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void sortPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplySortByPreset();
        }

        private void displayHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyDisplayMode(true);
        }

        private void displayDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyDisplayMode(false);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (FileOpen) AddEntryFromInputs(); else MessageBox.Show("No file loaded...");
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (FileOpen) UpdateSelectedEntry(); else MessageBox.Show("No file loaded...");
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (FileOpen) RemoveSelectedEntry(); else MessageBox.Show("No file loaded...");
        }

        private void entryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedEntry();
        }

        private void Tool_UnlockCharaTotalEditor_Load(object sender, EventArgs e)
        {
            if (File.Exists(Main.unlPath))
            {
                OpenFile(Main.unlPath);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tool_UnlockCharaTotalEditor));
            this.entryListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayDecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presetLabel = new System.Windows.Forms.Label();
            this.ryoLabel = new System.Windows.Forms.Label();
            this.conditionLabel = new System.Windows.Forms.Label();
            this.presetInput = new System.Windows.Forms.NumericUpDown();
            this.ryoInput = new System.Windows.Forms.NumericUpDown();
            this.conditionInput = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.presetInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ryoInput)).BeginInit();
            this.SuspendLayout();
            // 
            // entryListBox
            // 
            this.entryListBox.FormattingEnabled = true;
            this.entryListBox.Items.AddRange(new object[] {
            "No file loaded..."});
            this.entryListBox.Location = new System.Drawing.Point(12, 34);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(572, 342);
            this.entryListBox.TabIndex = 3;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.entryListBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.displayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(907, 24);
            this.menuStrip1.TabIndex = 4;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortPresetToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // sortPresetToolStripMenuItem
            // 
            this.sortPresetToolStripMenuItem.Name = "sortPresetToolStripMenuItem";
            this.sortPresetToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sortPresetToolStripMenuItem.Text = "PlayerSettingPreset";
            this.sortPresetToolStripMenuItem.Click += new System.EventHandler(this.sortPresetToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayHexToolStripMenuItem,
            this.displayDecToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // displayHexToolStripMenuItem
            // 
            this.displayHexToolStripMenuItem.Name = "displayHexToolStripMenuItem";
            this.displayHexToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.displayHexToolStripMenuItem.Text = "Hex";
            this.displayHexToolStripMenuItem.Click += new System.EventHandler(this.displayHexToolStripMenuItem_Click);
            // 
            // displayDecToolStripMenuItem
            // 
            this.displayDecToolStripMenuItem.Name = "displayDecToolStripMenuItem";
            this.displayDecToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.displayDecToolStripMenuItem.Text = "Dec";
            this.displayDecToolStripMenuItem.Click += new System.EventHandler(this.displayDecToolStripMenuItem_Click);
            // 
            // presetLabel
            // 
            this.presetLabel.AutoSize = true;
            this.presetLabel.Location = new System.Drawing.Point(590, 34);
            this.presetLabel.Name = "presetLabel";
            this.presetLabel.Size = new System.Drawing.Size(108, 15);
            this.presetLabel.TabIndex = 0;
            this.presetLabel.Text = "PlayerSettingPreset";
            // 
            // ryoLabel
            // 
            this.ryoLabel.AutoSize = true;
            this.ryoLabel.Location = new System.Drawing.Point(590, 79);
            this.ryoLabel.Name = "ryoLabel";
            this.ryoLabel.Size = new System.Drawing.Size(27, 15);
            this.ryoLabel.TabIndex = 1;
            this.ryoLabel.Text = "Ryo";
            // 
            // conditionLabel
            // 
            this.conditionLabel.AutoSize = true;
            this.conditionLabel.Location = new System.Drawing.Point(590, 124);
            this.conditionLabel.Name = "conditionLabel";
            this.conditionLabel.Size = new System.Drawing.Size(97, 15);
            this.conditionLabel.TabIndex = 2;
            this.conditionLabel.Text = "UnlockCondition";
            // 
            // presetInput
            // 
            this.presetInput.Hexadecimal = true;
            this.presetInput.Location = new System.Drawing.Point(593, 52);
            this.presetInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.presetInput.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.presetInput.Name = "presetInput";
            this.presetInput.Size = new System.Drawing.Size(120, 23);
            this.presetInput.TabIndex = 5;
            // 
            // ryoInput
            // 
            this.ryoInput.Location = new System.Drawing.Point(593, 97);
            this.ryoInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ryoInput.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ryoInput.Name = "ryoInput";
            this.ryoInput.Size = new System.Drawing.Size(120, 23);
            this.ryoInput.TabIndex = 6;
            // 
            // conditionInput
            // 
            this.conditionInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionInput.Location = new System.Drawing.Point(593, 142);
            this.conditionInput.Name = "conditionInput";
            this.conditionInput.Size = new System.Drawing.Size(308, 21);
            this.conditionInput.TabIndex = 7;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(593, 178);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(308, 28);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Add Entry";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(593, 212);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(308, 28);
            this.updateButton.TabIndex = 9;
            this.updateButton.Text = "Save Entry";
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(593, 246);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(308, 28);
            this.removeButton.TabIndex = 10;
            this.removeButton.Text = "Remove Selected";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // Tool_UnlockCharaTotalEditor
            // 
            this.ClientSize = new System.Drawing.Size(907, 384);
            this.Controls.Add(this.presetLabel);
            this.Controls.Add(this.ryoLabel);
            this.Controls.Add(this.conditionLabel);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.presetInput);
            this.Controls.Add(this.ryoInput);
            this.Controls.Add(this.conditionInput);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.removeButton);
            this.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Tool_UnlockCharaTotalEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unlock Chara Total Editor";
            this.Load += new System.EventHandler(this.Tool_UnlockCharaTotalEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.presetInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ryoInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
