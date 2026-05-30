using NAudio.Codecs;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NSUNS4_Character_Manager.Misc {
    public partial class Tool_nus3bankEditor_v2 : Form {
        private const string SoundClipboardFormat = "NSUNS4EvoToolbox.Nus3bankSound";
        private const string SoundClipboardMagic = "NUS3SOUND";
        private WaveOutEvent waveOut; // or WaveOutEvent()
        private WaveFileReader reader;
        public Tool_nus3bankEditor_v2() {
            InitializeComponent();
            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            toolStripComboBox1.SelectedIndex = 0;
        }
        public bool cleaning = false;
        public bool FileOpen = false;
        public string FilePath = "";
        public byte[] fileBytes = new byte[0];
        public bool XfbinHeader = false;
        public int NUS3_Position = 0;
        public int PROP_Position = 0;
        public int BINF_Position = 0;
        public int GRP_Position = 0;
        public int DTON_Position = 0;
        public int TONE_Position = 0;
        public int JUNK_Position = 0;
        public int PACK_Position = 0;
        public byte[] PROP_fileBytes = new byte[0];
        public byte[] BINF_fileBytes = new byte[0];
        public byte[] GRP_fileBytes = new byte[0];
        public byte[] DTON_fileBytes = new byte[0];
        public byte[] JUNK_fileBytes = new byte[0];

        public byte[] PlaySound_bytes = new byte[2] { 0xFF, 0xFF };
        public byte[] Randomizer_bytes = new byte[2] { 0x7F, 0x00 };
        public byte[] EmptySound_bytes = new byte[2] { 0x01, 0x00 };

        public List<int> TONE_SectionType_List = new List<int>();
        public List<byte[]> TONE_SectionTypeValues_List = new List<byte[]>();
        public List<string> TONE_SoundName_List = new List<string>();

        //PlaySound
        public List<int> TONE_SoundPos_List = new List<int>();
        public List<int> TONE_SoundSize_List = new List<int>();
        public List<float> TONE_MainVolume_List = new List<float>();
        public List<byte[]> TONE_SoundSettings_List = new List<byte[]>();

        public List<byte[]> TONE_SoundData_List = new List<byte[]>();
        //Randomizer
        public List<int> TONE_RandomizerType_List = new List<int>();
        public List<int> TONE_RandomizerLength_List = new List<int>();
        public List<int> TONE_RandomizerUnk1_List = new List<int>();
        public List<int> TONE_RandomizerSectionCount_List = new List<int>();
        public List<List<int>> TONE_RandomizerOneSection_ID_List = new List<List<int>>();
        public List<List<int>> TONE_RandomizerOneSection_unk_List = new List<List<int>>();
        public List<List<float>> TONE_RandomizerOneSection_PlayChance_List = new List<List<float>>();
        public List<List<int>> TONE_RandomizerOneSection_SoundID_List = new List<List<int>>();
        public List<float> TONE_RandomizerUnk2_List = new List<float>();
        public List<float> TONE_RandomizerUnk3_List = new List<float>();
        public List<float> TONE_RandomizerUnk4_List = new List<float>();
        public List<float> TONE_RandomizerUnk5_List = new List<float>();
        public List<float> TONE_RandomizerUnk6_List = new List<float>();

        public List<bool> TONE_OverlaySound_List = new List<bool>();

        public int IndexSelectedRow = 0;


        public int EntryCount = 0;

        public int FileID = 0;

        private void supportedFormatsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("BNSF, BNSG, WAV, RIFF, IVAG, VAG, IPCM, AAC, IDSP, IS14, IS22, IMA4, XMA, XMA2, OGG, CAF, AIFF");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFile();
        }

        public void OpenFile(string basepath = "") {
            OpenFileDialog o = new OpenFileDialog();
            {
                o.DefaultExt = ".xfbin";
                o.Filter = "XFBIN Container(*.xfbin)|*.xfbin|NUS3BANK Container(*.nus3bank)|*.nus3bank";
            }
            if (basepath != "") {
                o.FileName = basepath;
            } else {
                o.ShowDialog();
            }
            if (!(o.FileName != "") || !File.Exists(o.FileName)) {
                return;
            }
            ClearFile();
            FilePath = o.FileName;
            fileBytes = File.ReadAllBytes(FilePath);
            if (Main.b_ReadString2(fileBytes, 0, 4) == "NUCC")
                XfbinHeader = true;
            else
                XfbinHeader = false;
            NUS3_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x4E, 0x55, 0x53, 0x33 });
            PROP_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x50, 0x52, 0x4F, 0x50 }, NUS3_Position+0x50);
            BINF_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x42, 0x49, 0x4E, 0x46 }, NUS3_Position + 0x50);
            GRP_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x47, 0x52, 0x50, 0x20 }, NUS3_Position + 0x50);
            DTON_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x44, 0x54, 0x4F, 0x4E }, NUS3_Position + 0x50);
            TONE_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x54, 0x4F, 0x4E, 0x45 }, NUS3_Position + 0x50);
            JUNK_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x4A, 0x55, 0x4E, 0x4B }, NUS3_Position + 0x50);
            PACK_Position = Main.b_FindBytes(fileBytes, new byte[4] { 0x50, 0x41, 0x43, 0x4B }, NUS3_Position + 0x50);

            PROP_fileBytes = Main.b_ReadByteArray(fileBytes, PROP_Position, Main.b_ReadInt(fileBytes, PROP_Position + 0x4)+0x8);
            BINF_fileBytes = Main.b_ReadByteArray(fileBytes, BINF_Position, Main.b_ReadInt(fileBytes, BINF_Position + 0x4) + 0x8);
            GRP_fileBytes = Main.b_ReadByteArray(fileBytes, GRP_Position, Main.b_ReadInt(fileBytes, GRP_Position + 0x4) + 0x8);
            DTON_fileBytes = Main.b_ReadByteArray(fileBytes, DTON_Position, Main.b_ReadInt(fileBytes, DTON_Position + 0x4) + 0x8);
            JUNK_fileBytes = Main.b_ReadByteArray(fileBytes, JUNK_Position, Main.b_ReadInt(fileBytes, JUNK_Position + 0x4) + 0x8);

            FileID = Main.b_ReadInt(BINF_fileBytes, BINF_fileBytes.Length - 0x04);
            FileID_v.Value = FileID;
            EntryCount = Main.b_ReadInt(fileBytes, TONE_Position + 0x08);

            for (int x = 0; x<EntryCount; x++) {
                long _ptr = TONE_Position + 0x0C + (0x08 * x);
                int TONE_Size = Main.b_ReadInt(fileBytes, (int)_ptr+4);
                int newPtr = TONE_Position + 0x08 + Main.b_ReadInt(fileBytes, (int)_ptr);
                byte[] SectionType = new byte[0];
                byte[] SectionTypeValues = new byte[0];
                byte[] SoundData = new byte[0];
                SectionType = Main.b_ReadByteArray(fileBytes, newPtr + 0x04, 0x02);
                SectionTypeValues = Main.b_ReadByteArray(fileBytes, newPtr + 0x06, 0x06);
                if (BitConverter.ToString(SectionType) == BitConverter.ToString(PlaySound_bytes))
                    TONE_SectionType_List.Add(0);
                else if (BitConverter.ToString(SectionType) == BitConverter.ToString(Randomizer_bytes))
                    TONE_SectionType_List.Add(1);
                else
                    TONE_SectionType_List.Add(2);
                TONE_SectionTypeValues_List.Add(SectionTypeValues);

                string SoundName = Main.b_ReadString2(fileBytes, TONE_Position + 0x08 + Main.b_ReadInt(fileBytes, (int)_ptr) + 0x0D);
                if (SoundName == "")
                    SoundName = "Empty slot";

                TONE_SoundName_List.Add(SoundName);
                //PlaySound
                int SoundSize = 0;
                int SoundPos = 0;
                float MainVolume = 0;
                byte[] SectionSettings = new byte[0];

                //Randomizer
                int RandomizerType = 0;
                int RandomizerLength = 0;
                int RandomizerUnk1 = -1;
                int RandomizerSectionCount = 0;
                List<int> Randomizer_OneSectionID = new List<int>();
                List<int> Randomizer_OneSection_unk = new List<int>();
                List<float> Randomizer_OneSection_PlayChance = new List<float>();
                List<int> Randomizer_OneSection_SoundID = new List<int>();

                float RandomizerUnk2 = 0;
                float RandomizerUnk3 = 0;
                float RandomizerUnk4 = 0;
                float RandomizerUnk5 = 0;
                float RandomizerUnk6 = 0;
                bool OverlaySound = false;

                if (TONE_SectionType_List[x] == 0) {
                    int newPos = 0;
                    do {
                        newPos++;
                    }
                    while (Main.b_ReadInt(fileBytes, newPtr + 0x0D + newPos) != 8);
                    newPos += 0xD;
                    newPtr += newPos;
                    SoundPos = Main.b_ReadInt(fileBytes, newPtr + 0x04);
                    SoundSize = Main.b_ReadInt(fileBytes, newPtr + 0x08);
                    MainVolume = Main.b_ReadFloat(fileBytes, newPtr + 0x0C);
                    int index = newPos + 0x10;
                    SectionSettings = Main.b_ReadByteArray(fileBytes, newPtr + 0x10, TONE_Size - index - 4);
                    OverlaySound = !Convert.ToBoolean(Main.b_ReadInt(fileBytes, newPtr + 0x10 + TONE_Size - index - 4));
                    SoundData = Main.b_ReadByteArray(fileBytes, PACK_Position + 8 + SoundPos, SoundSize);

                } else if (TONE_SectionType_List[x] == 1) {
                    int newPos = 0;
                    do {
                        newPos++;
                    }
                    while (Main.b_ReadInt(fileBytes, newPtr + 0x0D + newPos) != 1);
                    newPos += 0xD;
                    RandomizerType = Main.b_ReadInt(fileBytes, newPtr + newPos);
                    RandomizerLength = Main.b_ReadInt(fileBytes, newPtr + newPos+0x04);
                    RandomizerUnk1 = Main.b_ReadInt(fileBytes, newPtr + newPos + 0x08);
                    RandomizerSectionCount = Main.b_ReadInt(fileBytes, newPtr + newPos + 0x0C);
                    for (int c = 0; c<RandomizerSectionCount; c++) {
                        int SectionID = Main.b_ReadInt(fileBytes, newPtr + newPos + 0x10 + (0x10*c));
                        int unk = Main.b_ReadInt(fileBytes, newPtr + newPos + 0x10 + (0x10 * c)+0x04);
                        float PlayChance = Main.b_ReadFloat(fileBytes, newPtr + newPos + 0x10 + (0x10 * c) + 0x08);
                        int SoundID =  Main.b_ReadInt(fileBytes, newPtr + newPos + 0x10 + (0x10 * c) + 0x0C);
                        Randomizer_OneSectionID.Add(SectionID);
                        Randomizer_OneSection_unk.Add(unk);
                        Randomizer_OneSection_PlayChance.Add(PlayChance);
                        Randomizer_OneSection_SoundID.Add(SoundID);
                    }
                    RandomizerUnk2 = Main.b_ReadFloat(fileBytes, newPtr + newPos + 0x10 + (0x10 * RandomizerSectionCount));
                    RandomizerUnk3 = Main.b_ReadFloat(fileBytes, newPtr + newPos + 0x10 + (0x10 * RandomizerSectionCount)+0x04);
                    RandomizerUnk4 = Main.b_ReadFloat(fileBytes, newPtr + newPos + 0x10 + (0x10 * RandomizerSectionCount) + 0x08);
                    RandomizerUnk5 = Main.b_ReadFloat(fileBytes, newPtr + newPos + 0x10 + (0x10 * RandomizerSectionCount) + 0x0C);
                    RandomizerUnk6 = Main.b_ReadFloat(fileBytes, newPtr + newPos + 0x10 + (0x10 * RandomizerSectionCount) + 0x10);
                    OverlaySound = !Convert.ToBoolean(Main.b_ReadInt(fileBytes, newPtr + newPos + 0x10 + (0x10 * RandomizerSectionCount) + 0x14));

                }
                TONE_SoundPos_List.Add(SoundPos);
                TONE_SoundSize_List.Add(SoundSize);
                TONE_OverlaySound_List.Add(OverlaySound);
                TONE_MainVolume_List.Add(MainVolume);
                TONE_SoundSettings_List.Add(SectionSettings);
                TONE_SoundData_List.Add(SoundData);

                TONE_RandomizerType_List.Add(RandomizerType);
                TONE_RandomizerLength_List.Add(RandomizerLength);
                TONE_RandomizerUnk1_List.Add(RandomizerUnk1);
                TONE_RandomizerSectionCount_List.Add(RandomizerSectionCount);
                TONE_RandomizerOneSection_ID_List.Add(Randomizer_OneSectionID);
                TONE_RandomizerOneSection_unk_List.Add(Randomizer_OneSection_unk);
                TONE_RandomizerOneSection_PlayChance_List.Add(Randomizer_OneSection_PlayChance);
                TONE_RandomizerOneSection_SoundID_List.Add(Randomizer_OneSection_SoundID);
                TONE_RandomizerUnk2_List.Add(RandomizerUnk2);
                TONE_RandomizerUnk3_List.Add(RandomizerUnk3);
                TONE_RandomizerUnk4_List.Add(RandomizerUnk4);
                TONE_RandomizerUnk5_List.Add(RandomizerUnk5);
                TONE_RandomizerUnk6_List.Add(RandomizerUnk6);
                dataGridView1.Rows.Add(x, SoundName);
                FileOpen = true;
            }
            ApplySoundNameColors();
        }

        public void ClearFile() {
            cleaning = true;
            FileOpen = false;
            FilePath = "";
            fileBytes = new byte[0];
            XfbinHeader = false;
            NUS3_Position = 0;
            PROP_Position = 0;
            BINF_Position = 0;
            GRP_Position = 0;
            DTON_Position = 0;
            TONE_Position = 0;
            JUNK_Position = 0;
            PACK_Position = 0;
            PROP_fileBytes = new byte[0];
            BINF_fileBytes = new byte[0];
            GRP_fileBytes = new byte[0];
            DTON_fileBytes = new byte[0];
            JUNK_fileBytes = new byte[0];
            TONE_SectionType_List = new List<int>();
            TONE_SectionTypeValues_List = new List<byte[]>();
            TONE_SoundName_List = new List<string>();
            TONE_SoundPos_List = new List<int>();
            TONE_SoundSize_List = new List<int>();
            TONE_MainVolume_List = new List<float>();
            TONE_SoundSettings_List = new List<byte[]>();
            TONE_SoundData_List = new List<byte[]>();
            TONE_RandomizerType_List = new List<int>();
            TONE_RandomizerLength_List = new List<int>();
            TONE_RandomizerUnk1_List = new List<int>();
            TONE_RandomizerSectionCount_List = new List<int>();
            TONE_RandomizerOneSection_ID_List = new List<List<int>>();
            TONE_RandomizerOneSection_unk_List = new List<List<int>>();
            TONE_RandomizerOneSection_PlayChance_List = new List<List<float>>();
            TONE_RandomizerOneSection_SoundID_List = new List<List<int>>();
            TONE_RandomizerUnk2_List = new List<float>();
            TONE_RandomizerUnk3_List = new List<float>();
            TONE_RandomizerUnk4_List = new List<float>();
            TONE_RandomizerUnk5_List = new List<float>();
            TONE_RandomizerUnk6_List = new List<float>();
            TONE_OverlaySound_List = new List<bool>();
            IndexSelectedRow = 0;
            EntryCount = 0;
            FileID = 0;
            dataGridView1.Rows.Clear();
            listBox2.Items.Clear();
            cleaning = false;
        }

        public void CloseFile() {
            ClearFile();
            FileOpen = false;
            XfbinHeader = false;
            FilePath = "";
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                DialogResult msg = MessageBox.Show("Are you sure you want to close this file?", "", MessageBoxButtons.OKCancel);
                if (msg == DialogResult.OK) {
                    CloseFile();
                }
            } else {
                MessageBox.Show("No file loaded...");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox1.SelectedIndex == 0) {
                tabControl1.TabPages.Add(tabPage1);
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);


            }
            else if(comboBox1.SelectedIndex == 1) {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Add(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
            }
            else {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Add(tabPage3);
            }
            int currentRow = GetCurrentRowIndex();
            if (currentRow != -1) {
                TONE_SectionType_List[currentRow] = comboBox1.SelectedIndex;
                ApplySoundNameColor(currentRow);
                UpdateSoundFormatDisplay(currentRow);
            }
        }

        private void Tool_nus3bankEditor_v2_Load(object sender, EventArgs e) {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            int c = listBox2.SelectedIndex;
            if (x != -1 && c != -1) {
                SoundID_v.Value = TONE_RandomizerOneSection_SoundID_List[x][c];
                PlayChance_v.Value = (decimal)TONE_RandomizerOneSection_PlayChance_List[x][c];
                unk1_r_v.Value = TONE_RandomizerOneSection_unk_List[x][c];
            }
        }

        private static string GetApplicationPath() {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static string GetTempPath() {
            return Path.Combine(GetApplicationPath(), "temp");
        }

        private static string[] GetExternalToolCandidates(string toolName, params string[] relativePaths) {
            List<string> candidates = new List<string>();
            string appPath = GetApplicationPath();
            string dependencyPath = Path.Combine(appPath, "dependencies");

            candidates.Add(Path.Combine(appPath, toolName));
            candidates.Add(Path.Combine(dependencyPath, toolName));

            foreach (string relativePath in relativePaths) {
                candidates.Add(Path.Combine(appPath, relativePath));
                candidates.Add(Path.Combine(dependencyPath, relativePath));
            }

            return candidates.ToArray();
        }

        private static string FindExternalTool(string toolName, params string[] relativePaths) {
            foreach (string candidate in GetExternalToolCandidates(toolName, relativePaths)) {
                if (File.Exists(candidate))
                    return candidate;
            }

            return "";
        }

        private static bool ConfigureExternalTool(ProcessStartInfo startInfo, string toolName, params string[] relativePaths) {
            string toolPath = FindExternalTool(toolName, relativePaths);
            if (toolPath == "") {
                MessageBox.Show(
                    toolName + " was not found.\n\nPlace it in one of these locations:\n" +
                    string.Join("\n", GetExternalToolCandidates(toolName, relativePaths)),
                    "Missing audio tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            string toolDirectory = Path.GetDirectoryName(toolPath);
            string dependencyDirectory = Path.Combine(GetApplicationPath(), "dependencies");
            string currentPath = startInfo.EnvironmentVariables["PATH"] ?? "";

            startInfo.FileName = toolPath;
            startInfo.WorkingDirectory = toolDirectory;
            startInfo.EnvironmentVariables["PATH"] = dependencyDirectory + ";" + toolDirectory + ";" + currentPath;
            return true;
        }

        private void ApplySoundNameColors() {
            for (int rowIndex = 0; rowIndex < dataGridView1.Rows.Count; rowIndex++) {
                ApplySoundNameColor(rowIndex);
            }
        }

        private void ApplySoundNameColor(int rowIndex) {
            if (rowIndex < 0 || rowIndex >= dataGridView1.Rows.Count || rowIndex >= TONE_SectionType_List.Count)
                return;

            DataGridViewCell soundNameCell = dataGridView1.Rows[rowIndex].Cells[1];
            soundNameCell.Style.BackColor = Color.White;
            soundNameCell.Style.SelectionBackColor = SystemColors.Highlight;

            if (TONE_SectionType_List[rowIndex] == 1) {
                soundNameCell.Style.BackColor = Color.Khaki;
                soundNameCell.Style.SelectionBackColor = Color.Goldenrod;
            }
            else if (TONE_SectionType_List[rowIndex] == 0) {
                bool hasSound = rowIndex < TONE_SoundData_List.Count && TONE_SoundData_List[rowIndex].Length > 4;
                bool isEmptySlot = rowIndex < TONE_SoundName_List.Count && TONE_SoundName_List[rowIndex] == "Empty slot";

                if (hasSound) {
                    soundNameCell.Style.BackColor = Color.LightGreen;
                    soundNameCell.Style.SelectionBackColor = Color.ForestGreen;
                }
                else if (!isEmptySlot) {
                    soundNameCell.Style.BackColor = Color.LightCoral;
                    soundNameCell.Style.SelectionBackColor = Color.Firebrick;
                }
            }
        }

        private string GetSoundFormat(int rowIndex) {
            if (rowIndex < 0 || rowIndex >= TONE_SectionType_List.Count)
                return "None";

            if (TONE_SectionType_List[rowIndex] == 1)
                return "Randomizer";

            if (TONE_SectionType_List[rowIndex] == 2)
                return "Empty";

            if (rowIndex >= TONE_SoundData_List.Count || TONE_SoundData_List[rowIndex].Length <= 4)
                return "No sound";

            string format = Main.b_ReadString(TONE_SoundData_List[rowIndex], 0);
            if (format.Length > 4)
                format = Main.b_ReadString(TONE_SoundData_List[rowIndex], 0, 4);

            if (format.Contains("VAG"))
                return "VAG";
            if (format.Contains("IDSP"))
                return "IDSP";
            if (format.Contains("RIFF"))
                return "WAV";

            format = format.Trim('\0', ' ');
            if (format == "")
                return "Unknown";

            return format.ToUpperInvariant();
        }

        private string GetSoundFileExtension(int rowIndex) {
            string format = GetSoundFormat(rowIndex);
            if (format == "No sound" || format == "Empty" || format == "Randomizer" || format == "Unknown" || format == "None")
                return "bin";

            return format.ToLowerInvariant();
        }

        private string GetSafeFileName(string fileName) {
            foreach (char invalidChar in Path.GetInvalidFileNameChars()) {
                fileName = fileName.Replace(invalidChar, '_');
            }

            return fileName;
        }

        private void UpdateSoundFormatDisplay(int rowIndex) {
            if (SoundFormat_v == null)
                return;

            SoundFormat_v.Text = GetSoundFormat(rowIndex);
        }

        private int GetCurrentRowIndex() {
            if (dataGridView1.CurrentCell == null)
                return -1;

            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= TONE_SoundName_List.Count)
                return -1;

            return rowIndex;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            var senderGrid = (DataGridView)sender;
            string tempPath = GetTempPath();
            if (!Directory.Exists(tempPath)) {
                Directory.CreateDirectory(tempPath);
            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) {
                if (TONE_SoundData_List[e.RowIndex].Length > 4 && TONE_SectionType_List[e.RowIndex] == 0) {
                    StopCurrentPlayback();
                    string format = Main.b_ReadString(TONE_SoundData_List[e.RowIndex], 0);
                    if (format.Length > 4) {
                        format = Main.b_ReadString(TONE_SoundData_List[e.RowIndex], 0, 4);
                    }
                    if (format.Contains("VAG"))
                        format = "VAG";
                    else if (format.Contains("IDSP"))
                        format = "IDSP";
                    else if (format.Contains("RIFF"))
                        format = "WAV";
                    string sourcePath = Path.Combine(tempPath, TONE_SoundName_List[e.RowIndex] + "." + format);
                    if (File.Exists(sourcePath))
                        File.Delete(sourcePath);
                    if (!Decode(TONE_SoundData_List[e.RowIndex], TONE_SoundName_List[e.RowIndex]))
                        return;

                    waveOut = new WaveOutEvent();
                    waveOut.PlaybackStopped += OnPlaybackStopped;
                    reader = new WaveFileReader(Path.Combine(tempPath, TONE_SoundName_List[e.RowIndex] + ".wav"));
                    waveOut.Init(CreatePlaybackProvider(reader));
                    waveOut.Volume = (float)trackBar1.Value/100;
                    waveOut.Play();
                }
                else {
                    if (TONE_SectionType_List[e.RowIndex] == 0)
                        MessageBox.Show("No Sound Data");
                    else
                        MessageBox.Show("Can't play sound in that type of section");
                }
            }
            if (dataGridView1.Rows.Count > 0)
                UpdateDataGrid();
        }
        void UpdateDataGrid() {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                UpdateSoundFormatDisplay(x);
                listBox2.Items.Clear();
                comboBox1.SelectedIndex = TONE_SectionType_List[x];
                if (TONE_SectionType_List[x] == 0) {
                    Volume_v.Value = (decimal)TONE_MainVolume_List[x];
                    Overlay_v.Checked = TONE_OverlaySound_List[x];
                } else if (TONE_SectionType_List[x] == 1) {
                    Overlay_v.Checked = TONE_OverlaySound_List[x];
                    for (int c = 0; c < TONE_RandomizerSectionCount_List[x]; c++) {
                        listBox2.Items.Add("Sound");
                        unk1_v.Value = (decimal)TONE_RandomizerUnk2_List[x];
                        unk2_v.Value = (decimal)TONE_RandomizerUnk3_List[x];
                        unk3_v.Value = (decimal)TONE_RandomizerUnk4_List[x];
                        unk4_v.Value = (decimal)TONE_RandomizerUnk5_List[x];
                        unk5_v.Value = (decimal)TONE_RandomizerUnk6_List[x];
                    }
                }
            }
        }
        private bool Decode(byte[] data, string name) {
            string tempPath = GetTempPath();
            if (!Directory.Exists(tempPath)) {
                Directory.CreateDirectory(tempPath);
            }
            string format = Main.b_ReadString(data, 0);
            if (format.Length > 4) {
                format = Main.b_ReadString(data, 0, 4);
            }
            if (format.Contains("VAG"))
                format = "VAG";
            else if (format.Contains("IDSP"))
                format = "IDSP";
            if (format != "RIFF") {
                string sourcePath = Path.Combine(tempPath, name + "." + format);
                string wavPath = Path.Combine(tempPath, name + ".wav");
                if (!File.Exists(wavPath)) {
                    File.WriteAllBytes(sourcePath, data);
                    Process p = new Process();
                    // Redirect the output stream of the child process.
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    if (!ConfigureExternalTool(p.StartInfo, "vgmstream.exe", Path.Combine("vgmstream", "vgmstream.exe")))
                        return false;
                    p.StartInfo.Arguments = "-o " + "\"" + wavPath + "\" " + "\"" + sourcePath + "\"";
                    p.Start();
                    string output = p.StandardOutput.ReadToEnd();
                    string error = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                    if (p.ExitCode != 0 || !File.Exists(wavPath)) {
                        MessageBox.Show("vgmstream.exe failed to decode this sound.\n\n" + output + error, "Decode failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            else {
                string wavPath = Path.Combine(tempPath, name + ".wav");
                if (!File.Exists(wavPath)) {
                    File.WriteAllBytes(wavPath, data);
                }

            }
            return true;
        }

        private void tabPage3_Click(object sender, EventArgs e) {

        }

        private void button5_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                TONE_SoundData_List[x] = new byte[0];
                TONE_SoundSettings_List[x] = new byte[116] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0xB4, 0xC2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB4, 0xC2, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                TONE_SoundSize_List[x] = 0;
                TONE_SoundPos_List[x] = 0;
                TONE_SectionTypeValues_List[x] = new byte[6] { 0x27, 0x84, 0x80, 0x18, 0x00, 0x00 };
                ApplySoundNameColor(x);
                UpdateSoundFormatDisplay(x);
                MessageBox.Show("Sound data was deleted.");
            }
            else {
                MessageBox.Show("Select sound.");
            }
        }

        private void button9_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                OpenFileDialog o = new OpenFileDialog();
                {
                    o.DefaultExt = "*.*";
                    o.Filter = "All formats(*.*)|*.*";
                }
                if (o.ShowDialog() != DialogResult.OK || !(o.FileName != "") || !File.Exists(o.FileName)) {
                    return;
                }
                TONE_SoundSettings_List[x] = new byte[136] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0xB4, 0xC2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB4, 0xC2, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0xBB, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0xBA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                TONE_SoundData_List[x] = File.ReadAllBytes(o.FileName);
                TONE_SectionTypeValues_List[x] = new byte[6] { 0x27, 0x84, 0x9F, 0x38, 0x00, 0x00 };
                ApplySoundNameColor(x);
                UpdateSoundFormatDisplay(x);
                MessageBox.Show("Sound successfully imported.");
            } else {
                MessageBox.Show("Select sound slot.");
            }
        }

        private int GetSelectedExtractableSoundIndex() {
            int x = GetCurrentRowIndex();
            if (x == -1) {
                MessageBox.Show("Select sound slot.");
                return -1;
            }

            if (TONE_SectionType_List[x] != 0 || TONE_SoundData_List[x].Length <= 4) {
                MessageBox.Show("Selected entry has no extractable sound data.");
                return -1;
            }

            return x;
        }

        private void buttonExtractRawSound_Click(object sender, EventArgs e) {
            int x = GetSelectedExtractableSoundIndex();
            if (x == -1)
                return;

            ExtractSelectedSoundRaw(x);
        }

        private void buttonExtractWavSound_Click(object sender, EventArgs e) {
            int x = GetSelectedExtractableSoundIndex();
            if (x == -1)
                return;

            ExtractSelectedSoundAsWav(x);
        }

        private void buttonCopySound_Click(object sender, EventArgs e) {
            int x = GetSelectedExtractableSoundIndex();
            if (x == -1)
                return;

            DataObject dataObject = new DataObject();
            dataObject.SetData(SoundClipboardFormat, false, CreateSoundClipboardPayload(x));
            Clipboard.SetDataObject(dataObject, true);
            MessageBox.Show("Sound copied to clipboard.");
        }

        private void buttonPasteSound_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x == -1) {
                MessageBox.Show("Select sound slot.");
                return;
            }

            IDataObject clipboardData = Clipboard.GetDataObject();
            if (clipboardData == null || !clipboardData.GetDataPresent(SoundClipboardFormat)) {
                MessageBox.Show("No copied NUS3BANK sound data was found on the clipboard.");
                return;
            }

            object clipboardObject = clipboardData.GetData(SoundClipboardFormat);
            byte[] payload = clipboardObject as byte[];
            if (payload == null || !TryApplySoundClipboardPayload(x, payload)) {
                MessageBox.Show("Clipboard sound data is invalid.");
                return;
            }

            dataGridView1.Rows[x].Cells[1].Value = TONE_SoundName_List[x];
            comboBox1.SelectedIndex = 0;
            ApplySoundNameColor(x);
            UpdateSoundFormatDisplay(x);
            MessageBox.Show("Sound pasted from clipboard.");
        }

        private byte[] CreateSoundClipboardPayload(int rowIndex) {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(SoundClipboardMagic);
            writer.Write(1);
            writer.Write(TONE_MainVolume_List[rowIndex]);
            writer.Write(TONE_OverlaySound_List[rowIndex]);
            WriteClipboardByteArray(writer, TONE_SectionTypeValues_List[rowIndex]);
            WriteClipboardByteArray(writer, TONE_SoundSettings_List[rowIndex]);
            WriteClipboardByteArray(writer, TONE_SoundData_List[rowIndex]);
            writer.Flush();

            return stream.ToArray();
        }

        private bool TryApplySoundClipboardPayload(int rowIndex, byte[] payload) {
            try {
                MemoryStream stream = new MemoryStream(payload);
                BinaryReader reader = new BinaryReader(stream);

                string magic = reader.ReadString();
                int version = reader.ReadInt32();
                if (magic != SoundClipboardMagic || version != 1)
                    return false;

                float volume = reader.ReadSingle();
                bool overlaySound = reader.ReadBoolean();
                byte[] sectionTypeValues = ReadClipboardByteArray(reader);
                byte[] soundSettings = ReadClipboardByteArray(reader);
                byte[] soundData = ReadClipboardByteArray(reader);
                if (soundData.Length <= 4)
                    return false;

                TONE_SectionType_List[rowIndex] = 0;
                TONE_MainVolume_List[rowIndex] = volume;
                TONE_OverlaySound_List[rowIndex] = overlaySound;
                TONE_SectionTypeValues_List[rowIndex] = sectionTypeValues;
                TONE_SoundSettings_List[rowIndex] = soundSettings;
                TONE_SoundData_List[rowIndex] = soundData;
                TONE_SoundSize_List[rowIndex] = soundData.Length;
                TONE_SoundPos_List[rowIndex] = 0;
                return true;
            }
            catch {
                return false;
            }
        }

        private void WriteClipboardByteArray(BinaryWriter writer, byte[] data) {
            writer.Write(data.Length);
            writer.Write(data);
        }

        private byte[] ReadClipboardByteArray(BinaryReader reader) {
            int length = reader.ReadInt32();
            if (length < 0)
                throw new InvalidDataException();

            byte[] data = reader.ReadBytes(length);
            if (data.Length != length)
                throw new EndOfStreamException();

            return data;
        }

        private byte[] GetDefaultImportedSoundSettings() {
            return new byte[136] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0xB4, 0xC2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB4, 0xC2, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0xBB, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0xBA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        private byte[] GetDefaultImportedSectionTypeValues() {
            return new byte[6] { 0x27, 0x84, 0x9F, 0x38, 0x00, 0x00 };
        }

        private int GetBnsfSamplesPerFrame(string codec) {
            if (codec == "IS14")
                return 640;
            if (codec == "IS22")
                return 960;

            return 0;
        }

        private List<int> GetBnsfBytesPerFrameCandidates(string codec, int channels) {
            List<int> candidates = new List<int>();

            if (codec == "IS14") {
                int[] baseCandidates = new int[3] { 60, 80, 120 };
                foreach (int candidate in baseCandidates)
                    candidates.Add(channels == 1 ? candidate : candidate * 2);
            }
            else if (codec == "IS22") {
                int[] baseCandidates = new int[3] { 80, 120, 160 };
                foreach (int candidate in baseCandidates)
                    candidates.Add(channels == 1 ? candidate : candidate * 2);

                for (int kbps = 36; kbps < 129; kbps += 4) {
                    int candidate = (int)(2.5 * kbps);
                    candidates.Add(channels == 1 ? candidate : candidate * 2);
                }
            }

            return candidates.Where(candidate => candidate > 0).Distinct().OrderBy(candidate => candidate).ToList();
        }

        private int ComputeBnsfTotalSamples(string codec, int channels, int sdatSize, int oldTotalSamples) {
            int samplesPerFrame = GetBnsfSamplesPerFrame(codec);
            if (samplesPerFrame == 0 || (channels != 1 && channels != 2) || sdatSize <= 0)
                return oldTotalSamples;

            List<int> candidates = GetBnsfBytesPerFrameCandidates(codec, channels);
            if (candidates.Count == 0)
                return oldTotalSamples;

            int bestTotalSamples = oldTotalSamples;
            int bestBadRemainder = int.MaxValue;
            int bestRemainder = int.MaxValue;
            int bestCloseness = int.MaxValue;
            int bestNegativeBpf = int.MaxValue;

            foreach (int bytesPerFrame in candidates) {
                int remainder = sdatSize % bytesPerFrame;
                int frames = sdatSize / bytesPerFrame;
                int totalSamples = frames * samplesPerFrame;
                int badRemainder = remainder == 0 ? 0 : 1;
                int closeness = oldTotalSamples > 0 ? Math.Abs(totalSamples - oldTotalSamples) : 0;
                int negativeBpf = -bytesPerFrame;

                if (badRemainder < bestBadRemainder ||
                    (badRemainder == bestBadRemainder && remainder < bestRemainder) ||
                    (badRemainder == bestBadRemainder && remainder == bestRemainder && closeness < bestCloseness) ||
                    (badRemainder == bestBadRemainder && remainder == bestRemainder && closeness == bestCloseness && negativeBpf < bestNegativeBpf)) {
                    bestTotalSamples = totalSamples;
                    bestBadRemainder = badRemainder;
                    bestRemainder = remainder;
                    bestCloseness = closeness;
                    bestNegativeBpf = negativeBpf;
                }
            }

            return bestTotalSamples;
        }

        private void ApplyImportedSoundToSlot(int rowIndex, byte[] soundData) {
            TONE_SectionType_List[rowIndex] = 0;
            TONE_SoundSettings_List[rowIndex] = GetDefaultImportedSoundSettings();
            TONE_SoundData_List[rowIndex] = soundData;
            TONE_SoundSize_List[rowIndex] = soundData.Length;
            TONE_SoundPos_List[rowIndex] = 0;
            TONE_SectionTypeValues_List[rowIndex] = GetDefaultImportedSectionTypeValues();
            ApplySoundNameColor(rowIndex);
        }

        private void ExtractSelectedSoundRaw(int rowIndex) {
            string extension = GetSoundFileExtension(rowIndex);
            SaveFileDialog s = new SaveFileDialog();
            {
                s.DefaultExt = "." + extension;
                s.Filter = GetSoundFormat(rowIndex) + " file|*." + extension + "|All files|*.*";
                s.FileName = GetSafeFileName(TONE_SoundName_List[rowIndex]) + "." + extension;
            }
            if (s.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllBytes(s.FileName, TONE_SoundData_List[rowIndex]);
            MessageBox.Show("Sound extracted to " + s.FileName + ".");
        }

        private void ExtractSelectedSoundAsWav(int rowIndex) {
            string tempPath = GetTempPath();
            string wavPath = Path.Combine(tempPath, TONE_SoundName_List[rowIndex] + ".wav");

            StopCurrentPlayback();
            if (File.Exists(wavPath))
                File.Delete(wavPath);

            if (!Decode(TONE_SoundData_List[rowIndex], TONE_SoundName_List[rowIndex]))
                return;

            SaveFileDialog s = new SaveFileDialog();
            {
                s.DefaultExt = ".wav";
                s.Filter = "WAV file|*.wav|All files|*.*";
                s.FileName = GetSafeFileName(TONE_SoundName_List[rowIndex]) + ".wav";
            }
            if (s.ShowDialog() != DialogResult.OK)
                return;

            File.Copy(wavPath, s.FileName, true);
            MessageBox.Show("WAV extracted to " + s.FileName + ".");
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {
            if (!cleaning) {
                TONE_SectionType_List.RemoveAt(IndexSelectedRow);
                TONE_SectionTypeValues_List.RemoveAt(IndexSelectedRow);
                TONE_SoundName_List.RemoveAt(IndexSelectedRow);
                TONE_SoundPos_List.RemoveAt(IndexSelectedRow);
                TONE_SoundSize_List.RemoveAt(IndexSelectedRow);
                TONE_MainVolume_List.RemoveAt(IndexSelectedRow);
                TONE_SoundSettings_List.RemoveAt(IndexSelectedRow);
                TONE_SoundData_List.RemoveAt(IndexSelectedRow);
                //Randomizer
                TONE_RandomizerType_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerLength_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerUnk1_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerSectionCount_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerOneSection_ID_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerOneSection_unk_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerOneSection_PlayChance_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerOneSection_SoundID_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerUnk2_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerUnk3_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerUnk4_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerUnk5_List.RemoveAt(IndexSelectedRow);
                TONE_RandomizerUnk6_List.RemoveAt(IndexSelectedRow);
                TONE_OverlaySound_List.RemoveAt(IndexSelectedRow);

                for (int c = IndexSelectedRow; c < dataGridView1.Rows.Count; c++) {
                    dataGridView1.Rows[c].Cells[0].Value = c;
                }
                ApplySoundNameColors();
                for (int c = 0; c < TONE_RandomizerOneSection_SoundID_List.Count; c++) {
                    for (int k = 0; k < TONE_RandomizerOneSection_SoundID_List[c].Count; k++) {
                        if (TONE_RandomizerOneSection_SoundID_List[c][k] > IndexSelectedRow) {
                            TONE_RandomizerOneSection_SoundID_List[c][k] -= 1;
                        }
                    }
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
            if (!cleaning) {
                IndexSelectedRow = GetCurrentRowIndex();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (FileOpen) {
                if (!cleaning && IndexSelectedRow != -1) {
                    TONE_SoundName_List[IndexSelectedRow] = dataGridView1.Rows[IndexSelectedRow].Cells[1].Value.ToString();
                    ApplySoundNameColor(IndexSelectedRow);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            for (int c = 0; c < dataGridView1.Rows.Count; c++) {
                if (dataGridView1.Rows[c].Cells[1].Value.ToString().Contains(textBox1.Text)) {
                    dataGridView1.Rows[c].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[c].Cells[1];
                    break;
                }

            }
            if (dataGridView1.Rows.Count > 0)
                UpdateDataGrid();
        }

        private void button1_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x!=-1) {
                if (TONE_SectionType_List[x] != 2) {
                    TONE_SectionType_List.Add(TONE_SectionType_List[x]);
                    TONE_SectionTypeValues_List.Add(TONE_SectionTypeValues_List[x]);
                    TONE_SoundName_List.Add(TONE_SoundName_List[x] + "_copy");
                    TONE_SoundPos_List.Add(TONE_SoundPos_List[x]);
                    TONE_SoundSize_List.Add(TONE_SoundSize_List[x]);
                    TONE_MainVolume_List.Add(TONE_MainVolume_List[x]);
                    TONE_SoundSettings_List.Add(TONE_SoundSettings_List[x]);
                    TONE_SoundData_List.Add(TONE_SoundData_List[x]);
                    TONE_RandomizerType_List.Add(TONE_RandomizerType_List[x]);
                    TONE_RandomizerLength_List.Add(TONE_RandomizerLength_List[x]);
                    TONE_RandomizerUnk1_List.Add(TONE_RandomizerUnk1_List[x]);
                    TONE_RandomizerSectionCount_List.Add(TONE_RandomizerSectionCount_List[x]);
                    TONE_RandomizerOneSection_ID_List.Add(TONE_RandomizerOneSection_ID_List[x]);
                    TONE_RandomizerOneSection_unk_List.Add(TONE_RandomizerOneSection_unk_List[x]);
                    TONE_RandomizerOneSection_PlayChance_List.Add(TONE_RandomizerOneSection_PlayChance_List[x]);
                    TONE_RandomizerOneSection_SoundID_List.Add(TONE_RandomizerOneSection_SoundID_List[x]);
                    TONE_RandomizerUnk2_List.Add(TONE_RandomizerUnk2_List[x]);
                    TONE_RandomizerUnk3_List.Add(TONE_RandomizerUnk3_List[x]);
                    TONE_RandomizerUnk4_List.Add(TONE_RandomizerUnk4_List[x]);
                    TONE_RandomizerUnk5_List.Add(TONE_RandomizerUnk5_List[x]);
                    TONE_RandomizerUnk6_List.Add(TONE_RandomizerUnk6_List[x]);
                    TONE_OverlaySound_List.Add(TONE_OverlaySound_List[x]);
                    dataGridView1.Rows.Add(dataGridView1.Rows.Count, TONE_SoundName_List[x] + "_copy");
                }
                else {
                    byte[] SoundData = new byte[0];
                    string SoundName = "Empty slot";
                    //PlaySound
                    int SoundSize = 0;
                    int SoundPos = 0;
                    float MainVolume = 0;
                    byte[] SectionSettings = new byte[0];

                    //Randomizer
                    int RandomizerType = 0;
                    int RandomizerLength = 0;
                    int RandomizerUnk1 = 0;
                    int RandomizerSectionCount = 0;
                    List<int> Randomizer_OneSectionID = new List<int>();
                    List<int> Randomizer_OneSection_unk = new List<int>();
                    List<float> Randomizer_OneSection_PlayChance = new List<float>();
                    List<int> Randomizer_OneSection_SoundID = new List<int>();

                    float RandomizerUnk2 = 0;
                    float RandomizerUnk3 = 0;
                    float RandomizerUnk4 = 0;
                    float RandomizerUnk5 = 0;
                    float RandomizerUnk6 = 0;
                    bool OverlaySound = false;
                    TONE_SectionType_List.Add(TONE_SectionType_List[x]);
                    TONE_SectionTypeValues_List.Add(TONE_SectionTypeValues_List[x]);
                    TONE_SoundName_List.Add(SoundName);
                    TONE_SoundPos_List.Add(SoundPos);
                    TONE_SoundSize_List.Add(SoundSize);
                    TONE_MainVolume_List.Add(MainVolume);
                    TONE_SoundSettings_List.Add(SectionSettings);
                    TONE_SoundData_List.Add(SoundData);
                    TONE_RandomizerType_List.Add(RandomizerType);
                    TONE_RandomizerLength_List.Add(RandomizerLength);
                    TONE_RandomizerUnk1_List.Add(RandomizerUnk1);
                    TONE_RandomizerSectionCount_List.Add(RandomizerSectionCount);
                    TONE_RandomizerOneSection_ID_List.Add(Randomizer_OneSectionID);
                    TONE_RandomizerOneSection_unk_List.Add(Randomizer_OneSection_unk);
                    TONE_RandomizerOneSection_PlayChance_List.Add(Randomizer_OneSection_PlayChance);
                    TONE_RandomizerOneSection_SoundID_List.Add(Randomizer_OneSection_SoundID);
                    TONE_RandomizerUnk2_List.Add(RandomizerUnk2);
                    TONE_RandomizerUnk3_List.Add(RandomizerUnk3);
                    TONE_RandomizerUnk4_List.Add(RandomizerUnk4);
                    TONE_RandomizerUnk5_List.Add(RandomizerUnk5);
                    TONE_RandomizerUnk6_List.Add(RandomizerUnk6);
                    TONE_OverlaySound_List.Add(OverlaySound);
                    dataGridView1.Rows.Add(dataGridView1.Rows.Count, SoundName);
                }
                dataGridView1.Rows[dataGridView1.Rows.Count-1].Selected = true;
                ApplySoundNameColor(dataGridView1.Rows.Count - 1);
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];

            }
        }

        private void button2_Click(object sender, EventArgs e) {

            if (comboBox1.SelectedIndex == 0) {

            }
        }

        private void FileID_v_ValueChanged(object sender, EventArgs e) {

        }

        private byte[] ConvertWavToBnsf(string importSoundPath, bool showErrors = true) {
            string tempPath = GetTempPath();
            if (!Directory.Exists(tempPath)) {
                Directory.CreateDirectory(tempPath);
            }

            string outputWavPath = Path.Combine(tempPath, "48000_output.wav");
            using (var reader = new WaveFileReader(importSoundPath)) {
                var newFormat = new WaveFormat(48000, 16, 1);
                using (var conversionStream = new WaveFormatConversionStream(newFormat, reader)) {
                    WaveFileWriter.CreateWaveFile(outputWavPath, conversionStream);
                }
            }

            byte[] importSoundFile = File.ReadAllBytes(outputWavPath);
            int posOfPcm = Main.b_FindBytes(importSoundFile, Encoding.ASCII.GetBytes("data"));
            if (posOfPcm == -1) {
                if (showErrors)
                    MessageBox.Show("Wrong format file");
                return null;
            }

            byte[] cleanedImportedSoundFile = new byte[0];
            cleanedImportedSoundFile = Main.b_AddBytes(cleanedImportedSoundFile, importSoundFile, 0, posOfPcm + 8, importSoundFile.Length - posOfPcm - 8);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            if (!ConfigureExternalTool(p.StartInfo, "encode.exe"))
                return null;

            string cleanedFilePath = Path.Combine(tempPath, "cleaned_file.wav");
            string convertedFilePath = Path.Combine(tempPath, "converted_file.bnsf");
            File.WriteAllBytes(cleanedFilePath, cleanedImportedSoundFile);
            p.StartInfo.Arguments = "0 " + "\"" + cleanedFilePath + "\" " + "\"" + convertedFilePath + "\"" + " 48000 14000";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();
            if (p.ExitCode != 0 || !File.Exists(convertedFilePath)) {
                if (showErrors)
                    MessageBox.Show("encode.exe failed to convert this WAV file.\n\n" + output + error, "Encode failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            byte[] convertedSoundFile = File.ReadAllBytes(convertedFilePath);
            byte[] bnsfHeader = new byte[48] { 0x42, 0x4E, 0x53, 0x46, 0x00, 0x00, 0x4C, 0x80, 0x49, 0x53, 0x31, 0x34, 0x73, 0x66, 0x6D, 0x74, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0xBB, 0x80, 0x00, 0x00, 0xED, 0x89, 0x00, 0x00, 0x00, 0x00, 0x00, 0x78, 0x02, 0x80, 0x73, 0x64, 0x61, 0x74, 0x00, 0x00, 0x4C, 0x58 };
            byte[] bnsfSound = new byte[0];
            bnsfSound = Main.b_AddBytes(bnsfSound, bnsfHeader);
            bnsfSound = Main.b_AddBytes(bnsfSound, convertedSoundFile);

            int importedSampleRate = Main.b_byteArrayToInt(Main.b_ReadByteArray(importSoundFile, 24, 4));
            int wavSampleCount = Main.b_byteArrayToInt(Main.b_ReadByteArray(importSoundFile, posOfPcm + 4, 4)) / importSoundFile[32];
            int bnsfTotalSamples = ComputeBnsfTotalSamples("IS14", 1, convertedSoundFile.Length, wavSampleCount);

            byte[] size1OfBnsf = BitConverter.GetBytes(convertedSoundFile.Length);
            byte[] invertedSize1OfBnsf = new byte[4] { size1OfBnsf[3], size1OfBnsf[2], size1OfBnsf[1], size1OfBnsf[0] };
            byte[] size2OfBnsf = BitConverter.GetBytes(convertedSoundFile.Length + 0x28);
            byte[] invertedSize2OfBnsf = new byte[4] { size2OfBnsf[3], size2OfBnsf[2], size2OfBnsf[1], size2OfBnsf[0] };
            byte[] bnsfSampleRate = BitConverter.GetBytes(importedSampleRate);
            byte[] invertedBnsfSampleRate = new byte[4] { bnsfSampleRate[3], bnsfSampleRate[2], bnsfSampleRate[1], bnsfSampleRate[0] };
            byte[] bnsfSoundLength = BitConverter.GetBytes(bnsfTotalSamples);
            byte[] invertedBnsfSoundLength = new byte[4] { bnsfSoundLength[3], bnsfSoundLength[2], bnsfSoundLength[1], bnsfSoundLength[0] };

            bnsfSound = Main.b_ReplaceBytes(bnsfSound, invertedSize1OfBnsf, 44);
            bnsfSound = Main.b_ReplaceBytes(bnsfSound, invertedSize2OfBnsf, 4);
            bnsfSound = Main.b_ReplaceBytes(bnsfSound, invertedBnsfSampleRate, 24);
            bnsfSound = Main.b_ReplaceBytes(bnsfSound, invertedBnsfSoundLength, 28);
            return bnsfSound;
        }

        private void ResetBatchImportProgress() {
            if (batchImportProgressBar == null)
                return;

            batchImportProgressBar.Value = 0;
            batchImportProgressBar.Visible = false;
        }

        private void StartBatchImportProgress(int maximum) {
            if (batchImportProgressBar == null)
                return;

            batchImportProgressBar.Minimum = 0;
            batchImportProgressBar.Maximum = Math.Max(1, maximum);
            batchImportProgressBar.Value = 0;
            batchImportProgressBar.Visible = true;
        }

        private void UpdateBatchImportProgress(int value) {
            if (batchImportProgressBar == null)
                return;

            batchImportProgressBar.Value = Math.Min(value, batchImportProgressBar.Maximum);
            Application.DoEvents();
        }

        private void BatchImportMatchedFolder(bool convertWavToBnsf) {
            if (!FileOpen) {
                MessageBox.Show("Open NUS3BANK file");
                return;
            }

            string selectedFolder = SelectExplorerFolder("Select folder with matching sound files");
            if (selectedFolder == "")
                return;

            string[] files = Directory.GetFiles(selectedFolder);
            if (files.Length == 0) {
                MessageBox.Show("No files found in selected folder.");
                return;
            }

            int importedCount = 0;
            int convertedCount = 0;
            int failedCount = 0;
            int matchedFileCount = 0;

            StartBatchImportProgress(files.Length);
            try {
                for (int fileIndex = 0; fileIndex < files.Length; fileIndex++) {
                    string filePath = files[fileIndex];
                    string soundName = Path.GetFileNameWithoutExtension(filePath);
                    List<int> matchingRows = new List<int>();

                    for (int rowIndex = 0; rowIndex < TONE_SoundName_List.Count; rowIndex++) {
                        if (string.Equals(TONE_SoundName_List[rowIndex], soundName, StringComparison.OrdinalIgnoreCase))
                            matchingRows.Add(rowIndex);
                    }

                    if (matchingRows.Count > 0) {
                        matchedFileCount++;
                        byte[] soundData = null;
                        bool converted = false;

                        try {
                            if (convertWavToBnsf && string.Equals(Path.GetExtension(filePath), ".wav", StringComparison.OrdinalIgnoreCase)) {
                                soundData = ConvertWavToBnsf(filePath, false);
                                converted = soundData != null;
                            }
                            else {
                                soundData = File.ReadAllBytes(filePath);
                            }
                        }
                        catch {
                            soundData = null;
                        }

                        if (soundData == null || soundData.Length <= 4) {
                            failedCount += matchingRows.Count;
                        }
                        else {
                            foreach (int rowIndex in matchingRows) {
                                ApplyImportedSoundToSlot(rowIndex, soundData);
                                importedCount++;
                                if (converted)
                                    convertedCount++;
                            }
                        }
                    }

                    UpdateBatchImportProgress(fileIndex + 1);
                }
            }
            finally {
                ResetBatchImportProgress();
            }

            int currentRow = GetCurrentRowIndex();
            if (currentRow != -1)
                UpdateSoundFormatDisplay(currentRow);

            MessageBox.Show(
                "Batch import complete.\n\nMatched files: " + matchedFileCount +
                "\nImported slots: " + importedCount +
                "\nConverted WAV files: " + convertedCount +
                "\nFailed slots: " + failedCount);
        }

        private string SelectExplorerFolder(string title) {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = title;

            if (dialog.ShowDialog() != Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok || dialog.FileName == "")
                return "";

            return dialog.FileName;
        }

        private void button10_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                OpenFileDialog o = new OpenFileDialog();
                {
                    o.DefaultExt = "*.wav";
                    o.Filter = "Waveform Audio File (*.wav)|*.wav*";
                }
                if (o.ShowDialog() != DialogResult.OK || !(o.FileName != "") || !File.Exists(o.FileName)) {
                    return;
                }

                byte[] bnsfSound = ConvertWavToBnsf(o.FileName);
                if (bnsfSound == null)
                    return;

                ApplyImportedSoundToSlot(x, bnsfSound);
                UpdateSoundFormatDisplay(x);

                MessageBox.Show("Sound successfully imported in BNSF format.");
            } else {
                MessageBox.Show("Select sound slot.");
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void Tool_nus3bankEditor_v2_FormClosed(object sender, FormClosedEventArgs e) {
            StopCurrentPlayback();
            string tempPath = GetTempPath();
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
        }

        private void StopCurrentPlayback() {
            if (waveOut != null) {
                waveOut.PlaybackStopped -= OnPlaybackStopped;
                waveOut.Stop();
            }

            DisposeCurrentPlayback();
        }

        private void DisposeCurrentPlayback() {
            WaveOutEvent output = waveOut;
            WaveFileReader activeReader = reader;

            waveOut = null;
            reader = null;

            if (output != null) {
                output.PlaybackStopped -= OnPlaybackStopped;
                output.Dispose();
            }

            if (activeReader != null)
                activeReader.Dispose();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args) {
            if (!ReferenceEquals(sender, waveOut))
                return;

            DisposeCurrentPlayback();
        }

        private IWaveProvider CreatePlaybackProvider(WaveFileReader activeReader) {
            ISampleProvider sampleProvider = activeReader.ToSampleProvider();
            float pitchFactor = GetPitchFactor();

            if (Math.Abs(pitchFactor - 1.0f) > 0.001f) {
                SmbPitchShiftingSampleProvider pitchProvider = new SmbPitchShiftingSampleProvider(sampleProvider);
                pitchProvider.PitchFactor = pitchFactor;
                sampleProvider = pitchProvider;
            }

            return new SampleToWaveProvider16(sampleProvider);
        }

        private float GetPitchFactor() {
            if (Pitch_v == null)
                return 1.0f;

            double cents = Math.Max(-30000.0, Math.Min(30000.0, (double)Pitch_v.Value));
            return (float)Math.Pow(2.0, cents / 1200.0);
        }

        private void PitchSlider_v_ValueChanged(object sender, EventArgs e) {
            if (Pitch_v != null && Pitch_v.Value != PitchSlider_v.Value)
                Pitch_v.Value = PitchSlider_v.Value;
        }

        private void Pitch_v_ValueChanged(object sender, EventArgs e) {
            if (PitchSlider_v != null && PitchSlider_v.Value != (int)Pitch_v.Value)
                PitchSlider_v.Value = (int)Pitch_v.Value;
        }

        private void dataGridView1_Click(object sender, EventArgs e) {

            if (dataGridView1.Rows.Count > 0)
                UpdateDataGrid();
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
        }

        private void button2_Click_1(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x!=-1) {
                TONE_MainVolume_List[x] = (float)Volume_v.Value;
                TONE_OverlaySound_List[x] = Overlay_v.Checked;
            }
            else {
                MessageBox.Show("Select sound section");
            }
        }

        private void button6_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                int x2 = listBox2.SelectedIndex;
                if (x2 != -1) {
                    TONE_RandomizerOneSection_ID_List[x].Add(TONE_RandomizerOneSection_ID_List[x].Max() + 1);
                    TONE_RandomizerOneSection_PlayChance_List[x].Add(TONE_RandomizerOneSection_PlayChance_List[x][x2]);
                    TONE_RandomizerOneSection_SoundID_List[x].Add(TONE_RandomizerOneSection_SoundID_List[x][x2]);
                    TONE_RandomizerOneSection_unk_List[x].Add(TONE_RandomizerOneSection_unk_List[x][x2]);
                    TONE_RandomizerSectionCount_List[x]++;
                    if (TONE_RandomizerOneSection_ID_List[x].Count > 0) {
                        if (TONE_RandomizerOneSection_ID_List[x].Count > 1) {
                            for (int c = 0; c < TONE_RandomizerOneSection_ID_List[x].Count; c++) {
                                TONE_RandomizerOneSection_ID_List[x][c] = c + 1;
                            }
                        }
                        TONE_RandomizerOneSection_ID_List[x][TONE_RandomizerOneSection_ID_List[x].Count-1] = 0;

                    }
                    listBox2.Items.Add("Sound");
                    listBox2.SelectedIndex = listBox2.Items.Count-1;

                } else {
                    MessageBox.Show("Select randomize section");
                }

            } else {
                MessageBox.Show("Select sound section");
            }
        }

        private void button8_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                int x2 = listBox2.SelectedIndex;
                if (x2 != -1) {
                    TONE_RandomizerOneSection_ID_List[x].RemoveAt(x2);
                    TONE_RandomizerOneSection_PlayChance_List[x].RemoveAt(x2);
                    TONE_RandomizerOneSection_SoundID_List[x].RemoveAt(x2);
                    TONE_RandomizerOneSection_unk_List[x].RemoveAt(x2);
                    listBox2.Items.RemoveAt(x2);
                    TONE_RandomizerSectionCount_List[x]--;
                    if (TONE_RandomizerOneSection_ID_List[x].Count > 0) {
                        if (TONE_RandomizerOneSection_ID_List[x].Count > 1) {
                            for (int c = 0; c < TONE_RandomizerOneSection_ID_List[x].Count - 1; c++) {
                                TONE_RandomizerOneSection_ID_List[x][c] = c + 1;
                            }
                        }
                        TONE_RandomizerOneSection_ID_List[x][TONE_RandomizerOneSection_ID_List[x].Count - 1] = 0;

                    }

                } else {
                    MessageBox.Show("Select randomize section");
                }

            } else {
                MessageBox.Show("Select sound section");
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {
                int x2 = listBox2.SelectedIndex;
                if (x2 != -1) {
                    TONE_RandomizerOneSection_PlayChance_List[x][x2] = (float)PlayChance_v.Value;
                    TONE_RandomizerOneSection_SoundID_List[x][x2] = (int)SoundID_v.Value;
                    TONE_RandomizerOneSection_unk_List[x][x2] = (int)unk1_r_v.Value;

                } else {
                    MessageBox.Show("Select randomize section");
                }

            } else {
                MessageBox.Show("Select sound section");
            }
        }

        private void button14_Click(object sender, EventArgs e) {
            int x = GetCurrentRowIndex();
            if (x != -1) {

                TONE_RandomizerUnk2_List[x] = (float)unk1_v.Value;
                TONE_RandomizerUnk3_List[x] = (float)unk2_v.Value;
                TONE_RandomizerUnk4_List[x] = (float)unk3_v.Value;
                TONE_RandomizerUnk5_List[x] = (float)unk4_v.Value;
                TONE_RandomizerUnk6_List[x] = (float)unk5_v.Value;
                TONE_OverlaySound_List[x] = true;

            } else {
                MessageBox.Show("Select sound section");
            }
        }

        private void exportAllSoundsToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void originalFormatToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                string selectedFolder = SelectExplorerFolder("Select export folder");
                if (selectedFolder == "")
                    return;

                for (int x = 0; x < TONE_SoundData_List.Count; x++) {

                    if (TONE_SectionType_List[x] != 2 && TONE_SectionType_List[x] != 1 && TONE_SoundData_List[x].Length > 4) {
                        string name = TONE_SoundName_List[x];
                        string format = Main.b_ReadString(TONE_SoundData_List[x], 0);
                        if (format.Length > 4)
                            format = Main.b_ReadString(TONE_SoundData_List[x], 0, 4);
                        if (format.Contains("VAG"))
                            format = "VAG";
                        else if (format.Contains("IDSP"))
                            format = "IDSP";
                        else if (format == "RIFF")
                            format = "WAV";
                        string path = "";
                        if (toolStripComboBox1.SelectedIndex == 0)
                            path = Path.Combine(selectedFolder, x.ToString() + "-" + name + "." + format);
                        else if (toolStripComboBox1.SelectedIndex == 1)
                            path = Path.Combine(selectedFolder, name + "." + format);

                        File.WriteAllBytes(path, TONE_SoundData_List[x]);
                    };
                }
                MessageBox.Show("Files saved to " + selectedFolder);
            } else {
                MessageBox.Show("Open NUS3BANK file");

            }
        }
        private void wAVFormatToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                string selectedFolder = SelectExplorerFolder("Select export folder");
                if (selectedFolder == "")
                    return;

                string tempPath = GetTempPath();
                for (int x = 0; x < TONE_SoundData_List.Count; x++) {

                    if (TONE_SectionType_List[x] != 2 && TONE_SectionType_List[x] != 1 && TONE_SoundData_List[x].Length > 4) {
                        if (!Decode(TONE_SoundData_List[x], TONE_SoundName_List[x]))
                            return;
                        string name = TONE_SoundName_List[x];
                        string exp_path = "";
                        if (toolStripComboBox1.SelectedIndex == 0)
                            exp_path = Path.Combine(selectedFolder, x.ToString() + "-" + name + ".wav");
                        else if (toolStripComboBox1.SelectedIndex == 1)
                            exp_path = Path.Combine(selectedFolder, name + ".wav");
                        File.Copy(Path.Combine(tempPath, TONE_SoundName_List[x] + ".wav"), exp_path, true);
                    };
                }
                MessageBox.Show("Files saved to " + selectedFolder);
            }
            else {
                MessageBox.Show("Open NUS3BANK file");
            }
        }

        private void batchImportingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                OpenFileDialog o = new OpenFileDialog();
                o.Multiselect = true;
                if (o.ShowDialog() != DialogResult.OK)
                    return;

                for (int x = 0; x < o.FileNames.Length; x++) {
                    TONE_SoundName_List.Add(Path.GetFileNameWithoutExtension(o.FileNames[x]));
                    TONE_SectionType_List.Add(0);
                    TONE_SectionTypeValues_List.Add(new byte[6] { 0x27, 0x84, 0x9F, 0x38, 0x00, 0x00 });
                    TONE_SoundData_List.Add(File.ReadAllBytes(o.FileNames[x]));
                    TONE_MainVolume_List.Add(0);
                    TONE_OverlaySound_List.Add(true);
                    TONE_SoundSettings_List.Add(new byte[136] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0xB4, 0xC2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB4, 0xC2, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0xBB, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x98, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    TONE_RandomizerType_List.Add(0);
                    TONE_RandomizerLength_List.Add(0);
                    TONE_RandomizerOneSection_ID_List.Add(new List<int>());
                    TONE_RandomizerOneSection_SoundID_List.Add(new List<int>());
                    TONE_RandomizerOneSection_PlayChance_List.Add(new List<float>());
                    TONE_RandomizerOneSection_unk_List.Add(new List<int>());
                    TONE_RandomizerSectionCount_List.Add(0);
                    TONE_RandomizerUnk1_List.Add(-1);
                    TONE_RandomizerUnk2_List.Add(0);
                    TONE_RandomizerUnk3_List.Add(0);
                    TONE_RandomizerUnk4_List.Add(0);
                    TONE_RandomizerUnk5_List.Add(1);
                    TONE_RandomizerUnk6_List.Add(1);
                    TONE_SoundPos_List.Add(0);
                    TONE_SoundSize_List.Add(0);
                    dataGridView1.Rows.Add(dataGridView1.Rows.Count, Path.GetFileNameWithoutExtension(o.FileNames[x]));
                    ApplySoundNameColor(dataGridView1.Rows.Count - 1);
                }
            } else {
                MessageBox.Show("Open NUS3BANK file");
            }
        }

        private void batchImportMatchedRawToolStripMenuItem_Click(object sender, EventArgs e) {
            BatchImportMatchedFolder(false);
        }

        private void batchImportMatchedConvertWavToolStripMenuItem_Click(object sender, EventArgs e) {
            BatchImportMatchedFolder(true);
        }

        private void button3_Click(object sender, EventArgs e) {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                SaveFile();
            } else {
                MessageBox.Show("No file loaded...");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                SaveFileAs();
            } else {
                MessageBox.Show("No file loaded...");
            }
        }
        public void SaveFile() {
            if (FilePath != "") {
                if (File.Exists(FilePath + ".backup")) {
                    File.Delete(FilePath + ".backup");
                }
                File.Copy(FilePath, FilePath + ".backup");
                string extension = Path.GetExtension(FilePath);
                File.WriteAllBytes(FilePath, ConvertToFile(extension));
                if (this.Visible) MessageBox.Show("File saved to " + FilePath + ".");
            } else {
                SaveFileAs();
            }
        }
        public void SaveFileAs(string basepath = "") {
            SaveFileDialog s = new SaveFileDialog();
            {
                if (XfbinHeader) {
                    s.DefaultExt = ".xfbin";
                    s.Filter = "XFBIN files|*.xfbin|NUS3BANK files|*.NUS3BANK";
                }
                else {
                    s.DefaultExt = ".NUS3BANK";
                    s.Filter = "NUS3BANK files|*.NUS3BANK";
                }
            }
            if (basepath == "") {
                if (s.ShowDialog() != DialogResult.OK)
                    return;
            }
            else
                s.FileName = basepath;
            if (!(s.FileName != "")) {
                return;
            }
            if (s.FileName == FilePath) {
                if (File.Exists(FilePath + ".backup")) {
                    File.Delete(FilePath + ".backup");
                }
                File.Copy(FilePath, FilePath + ".backup");
            } else {
                FilePath = s.FileName;
            }
            string extension = Path.GetExtension(s.FileName);
            File.WriteAllBytes(FilePath, ConvertToFile(extension));
            if (basepath == "")
                MessageBox.Show("File saved to " + FilePath + ".");
        }
        public byte[] ConvertToFile(string extension) {
            byte[] ConvertedFile = new byte[0];
            if (XfbinHeader && extension.Contains("xfbin"))
                ConvertedFile = Main.b_AddBytes(ConvertedFile, Main.b_ReadByteArray(fileBytes, 0, NUS3_Position));
            int NUS3_Length_ptr = ConvertedFile.Length;
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Encoding.ASCII.GetBytes("NUS3"));
            ConvertedFile = Main.b_AddBytes(ConvertedFile, new byte[4]);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Main.b_ReadByteArray(fileBytes, NUS3_Position+8, 0x30));
            int TONE_Header_Length_ptr = ConvertedFile.Length;
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Encoding.ASCII.GetBytes("TONE"));
            ConvertedFile = Main.b_AddBytes(ConvertedFile, new byte[4]);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Main.b_ReadByteArray(fileBytes, JUNK_Position, 0x08));
            int PACK_Header_Length_ptr = ConvertedFile.Length;
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Encoding.ASCII.GetBytes("PACK"));
            ConvertedFile = Main.b_AddBytes(ConvertedFile, new byte[4]);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, PROP_fileBytes);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, BINF_fileBytes);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, GRP_fileBytes);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, DTON_fileBytes);
            int TONE_Length_ptr = ConvertedFile.Length;
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Encoding.ASCII.GetBytes("TONE"));
            ConvertedFile = Main.b_AddBytes(ConvertedFile, new byte[4]);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, BitConverter.GetBytes(TONE_SoundName_List.Count));
            List<int> TONE_ptr = new List<int>();
            List<int> TONE_length = new List<int>();
            List<int> TONE_size = new List<int>();
            for (int x = 0; x < TONE_SoundName_List.Count; x++) {
                TONE_ptr.Add(ConvertedFile.Length);
                ConvertedFile = Main.b_AddBytes(ConvertedFile, new byte[8]);
            }
            byte[] PACK_SECTION = new byte[0];
            for (int x = 0; x < TONE_SoundName_List.Count; x++) {
                byte[] TONE_Section = new byte[0];
                TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4]);
                if (TONE_SectionType_List[x] == 0) {
                    TONE_Section = Main.b_AddBytes(TONE_Section, PlaySound_bytes);
                    TONE_Section = Main.b_AddBytes(TONE_Section, TONE_SectionTypeValues_List[x]);
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[1] { (byte)(TONE_SoundName_List[x].Length+1)});
                    TONE_Section = Main.b_AddBytes(TONE_Section, Encoding.ASCII.GetBytes(TONE_SoundName_List[x]));
                    if (TONE_Section.Length % 4 != 0) {
                        do {
                            if (TONE_Section.Length % 4 != 0)
                                TONE_Section = Main.b_AddBytes(TONE_Section, new byte[1]);
                        }
                        while (TONE_Section.Length % 4 != 0);
                    }
                    else {
                        TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4]);
                    }
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4]);
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4] { 8, 0, 0, 0 });
                    if (TONE_SoundData_List[x].Length > 4) {
                        TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(PACK_SECTION.Length));
                        TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_SoundData_List[x].Length));
                        TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_MainVolume_List[x]));
                        PACK_SECTION = Main.b_AddBytes(PACK_SECTION, TONE_SoundData_List[x]);
                    }
                    else {
                        TONE_Section = Main.b_AddBytes(TONE_Section, new byte[12]);
                    }
                    TONE_Section = Main.b_AddBytes(TONE_Section, TONE_SoundSettings_List[x]);
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(!TONE_OverlaySound_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[3]);
                }
                else if (TONE_SectionType_List[x] == 1) {
                    TONE_Section = Main.b_AddBytes(TONE_Section, Randomizer_bytes);
                    TONE_Section = Main.b_AddBytes(TONE_Section, TONE_SectionTypeValues_List[x]);
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[1] { (byte)(TONE_SoundName_List[x].Length + 1) });
                    TONE_Section = Main.b_AddBytes(TONE_Section, Encoding.ASCII.GetBytes(TONE_SoundName_List[x]));
                    do {
                        TONE_Section = Main.b_AddBytes(TONE_Section, new byte[1]);
                    }
                    while (TONE_Section.Length % 4 != 0);
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4] { 1, 0, 0, 0 });
                    int Randomizer_Length = (TONE_RandomizerSectionCount_List[x] * 0x10) + 0x08;
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4] { (byte)Randomizer_Length, 0, 0, 0 });
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerUnk1_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerSectionCount_List[x]));
                    for (int c = 0; c< TONE_RandomizerSectionCount_List[x]; c++) {
                        if (c != TONE_RandomizerSectionCount_List[x]-1)
                            TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(c+1));
                        else
                            TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4] { 0, 0, 0, 0 });
                        TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerOneSection_unk_List[x][c]));
                        TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerOneSection_PlayChance_List[x][c]));
                        TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerOneSection_SoundID_List[x][c]));
                    }
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerUnk2_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerUnk3_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerUnk4_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerUnk5_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, BitConverter.GetBytes(TONE_RandomizerUnk6_List[x]));
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[4] { 0, 0, 0, 0 });
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[3]);

                }
                else if (TONE_SectionType_List[x] == 2) {
                    TONE_Section = Main.b_AddBytes(TONE_Section, new byte[8] { 0x01, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00 });
                }
                ConvertedFile = Main.b_AddBytes(ConvertedFile, TONE_Section);
                TONE_length.Add(ConvertedFile.Length - TONE_Length_ptr - 0x08 - TONE_Section.Length);
                TONE_size.Add(TONE_Section.Length);
                ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(TONE_length[x]), TONE_ptr[x]);
                ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(TONE_size[x]), TONE_ptr[x]+4);
            }
            ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(ConvertedFile.Length- TONE_Length_ptr-0x08), TONE_Length_ptr + 4);
            int Tone_len = ConvertedFile.Length - TONE_Length_ptr - 0x08;
            ConvertedFile = Main.b_AddBytes(ConvertedFile, JUNK_fileBytes);
            ConvertedFile = Main.b_AddBytes(ConvertedFile, Encoding.ASCII.GetBytes("PACK"));
            ConvertedFile = Main.b_AddBytes(ConvertedFile, BitConverter.GetBytes(PACK_SECTION.Length));
            ConvertedFile = Main.b_AddBytes(ConvertedFile, PACK_SECTION);
            int GRP_pos = Main.b_FindBytes(ConvertedFile, new byte[4] { 0x47, 0x52, 0x50, 0x20 }, NUS3_Length_ptr + 0x50);
            ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes((int)FileID_v.Value), GRP_pos-4);
            ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(PACK_SECTION.Length), PACK_Header_Length_ptr + 4);
            ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(Tone_len), TONE_Header_Length_ptr + 4);
            int NUS3_Size = ConvertedFile.Length - NUS3_Length_ptr - 0x08;
            ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(NUS3_Size), NUS3_Length_ptr + 4);
            if (XfbinHeader && extension.Contains("xfbin")) {
                ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes((int)FileID_v.Value), NUS3_Position - 4);
                ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(ConvertedFile.Length- NUS3_Length_ptr), NUS3_Length_ptr-4,1);
                ConvertedFile = Main.b_ReplaceBytes(ConvertedFile, BitConverter.GetBytes(ConvertedFile.Length - NUS3_Length_ptr+4), NUS3_Length_ptr - 0x10,1);
                ConvertedFile = Main.b_AddBytes(ConvertedFile, new byte[0x14] { 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x79, 0x18, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 });
            }
            return ConvertedFile;
        }

        private void tabPage2_Click(object sender, EventArgs e) {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e) {
            if (waveOut != null && reader != null)
                waveOut.Volume = (float)trackBar1.Value / 100;
        }

        private void createListForSeparamToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileOpen) {
                if (FilePath.Contains("btlcmn") && FileID == 1) {
                    byte[] separamBytes = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\systemFiles\\separam.xfbin");
                    byte[] fileStart = new byte[0];
                    fileStart = Main.b_AddBytes(fileStart, separamBytes, 0, 0, 0xE2A);
                    fileStart = Main.b_AddBytes(fileStart, BitConverter.GetBytes((TONE_SoundName_List.Count * 0x20 + 6)), 1);
                    fileStart = Main.b_AddBytes(fileStart, new byte[8] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x79, 0x00, 0x00 });
                    fileStart = Main.b_AddBytes(fileStart, BitConverter.GetBytes((TONE_SoundName_List.Count * 0x20 + 2)), 1);
                    fileStart = Main.b_AddBytes(fileStart, BitConverter.GetBytes(TONE_SoundName_List.Count), 0, 0, 2);
                    for (int z = 0; z < TONE_SoundName_List.Count; z++) {
                        byte[] section = new byte[0x20];
                        string name = TONE_SoundName_List[z];
                        if (name.Length > 31)
                            name = name.Substring(0, 31);
                        section = Main.b_ReplaceBytes(section, Encoding.ASCII.GetBytes(name), 0);
                        fileStart = Main.b_AddBytes(fileStart, section);
                    }
                    fileStart = Main.b_AddBytes(fileStart, separamBytes, 0, 0x815C, 0x815C + 0x5DB2);
                    SaveFileDialog s = new SaveFileDialog();
                    {
                        s.DefaultExt = ".xfbin";
                        s.Filter = ".xfbin|.xfbin";
                    }
                    if (s.ShowDialog() != DialogResult.OK)
                        return;

                    File.WriteAllBytes(s.FileName, fileStart);
                    MessageBox.Show("File saved to " + s.FileName + ".");
                }
                else {
                    MessageBox.Show("This file isn't btlcmn");
                }
            }
        }
    }
}
