using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NSUNS4_Character_Manager;

namespace NSUNS4_Character_Manager.Tools
{
    public partial class Tool_MovesetCoder : Form
    {
        public Tool_MovesetCoder()
        {
            InitializeComponent();

            // FUNCT
            for(int x = 0; x < Program.ME_LIST.Length; x++) t_function.Items.Add(x.ToString("X2") + " = " + Program.ME_LIST[x]);

            // COND
            for(int x = 0; x < Program.COND.Length; x++) t_condition.Items.Add(Program.COND[x]);
            for(int x = Program.COND.Length; x < 256; x++) t_condition.Items.Add("0x" + t_condition.Items.Count.ToString("X2").PadLeft(2, '0') + " = ???");

            // LINK_COND
            for (int x = 0; x < Program.LINK_COND.Length; x++) t_linkCondition.Items.Add(Program.LINK_COND[x]);
            for (int x = Program.LINK_COND.Length; x < 256; x++) t_linkCondition.Items.Add("0x" + (t_linkCondition.Items.Count-1).ToString("X2").PadLeft(2, '0') + " = ???");

            // DMG COND
            for (int x = 0; x < Program.DMGCOND.Length; x++) t_dmgcond.Items.Add(Program.DMGCOND[x]);
            for (int x = Program.DMGCOND.Length; x < 256; x++) t_dmgcond.Items.Add("0x" + t_dmgcond.Items.Count.ToString("X2").PadLeft(2, '0') + " = ???");

            // HIT EFF
            for (int x = 0; x < Program.HITEFF.Length; x++) t_hiteffect.Items.Add(Program.HITEFF[x]);
            for (int x = Program.HITEFF.Length; x < 256; x++) t_hiteffect.Items.Add("0x" + t_hiteffect.Items.Count.ToString("X2").PadLeft(2, '0') + " = ???");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen) CloseFile(true);
            else CloseFile();

            OpenFile();
        }
        public int index1;
        public int index2;
        public int index3;
        bool fileOpen = false;
        public string filePath = "";
        byte[] fileBytes;
        //Effect Entry
        bool effectSecFound = false;
        int effectSecLength = 0;
        int effectSecCount = 0;
        List<string> effectSecName = new List<string>();
        List<string> effectSecSkillName = new List<string>();
        List<string> effectSecSkillEntry = new List<string>();
        List<int> effectSecSkillValue = new List<int>();
        List<byte> EffectSection = new List<byte>();
        List<int> effectVisibleIndices = new List<int>();
        public string[] sectionnames;
        //Collision Entry
        public bool collisionChanged = false;
        public int collisionSecLength = 0;
        public int collisionSecCount = 0;
        public List<int> collisionSecTypeValue = new List<int>();
        public List<int> collisionSecStateValue = new List<int>();
        public List<int> collisionSecEnablerBoneValue = new List<int>();
        public List<long> collisionSecRadiusValue = new List<long>();
        public List<long> collisionSecYPosValue = new List<long>();
        public List<long> collisionSecZPosValue = new List<long>();
        public List<string> collisionSecBoneName = new List<string>();

        List<byte[]> verSection = new List<byte[]>();
        List<int> anmCount = new List<int>();
        List<List<byte[]>> anmSection = new List<List<byte[]>>();

        public List<int> verList = new List<int>();
        List<int> verLength = new List<int>();
        public List<List<byte[]>> plAnmList = new List<List<byte[]>>();
        public List<List<List<byte[]>>> movementList = new List<List<List<byte[]>>>();
        private readonly List<string> verChunkNames = new List<string>();
        private readonly List<string> verChunkPaths = new List<string>();
        private readonly List<string> removedVerChunkNames = new List<string>();
        private string effectChunkName = "";
        private string effectChunkPath = "";
        private string collisionChunkName = "";
        private string collisionChunkPath = "";

        private static bool ChunkNameContains(XfbinBinaryChunkItem chunk, string value)
        {
            return chunk != null && !string.IsNullOrWhiteSpace(chunk.ChunkName) &&
                   chunk.ChunkName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool BufferContainsAscii(byte[] bytes, string value)
        {
            if (bytes == null || bytes.Length == 0 || string.IsNullOrEmpty(value))
                return false;
            byte[] pattern = Encoding.ASCII.GetBytes(value);
            if (pattern.Length == 0 || bytes.Length < pattern.Length)
                return false;

            for (int i = 0; i <= bytes.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (bytes[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return true;
            }

            return false;
        }

        private static List<XfbinBinaryChunkItem> GetMovesetBinaryChunks(string xfbinPath)
        {
            using (XfbinParserBackend backend = new XfbinParserBackend(xfbinPath))
            {
                return backend.GetBinaryChunks();
            }
        }

        private static int GetPrmVerChunkRank(string chunkName)
        {
            string name = (chunkName ?? string.Empty).ToLowerInvariant();
            if (name.EndsWith("prm_awa2")) return 0;
            if (name.EndsWith("prm_awa")) return 1;
            if (name.EndsWith("prm_mot")) return 2;
            if (name.EndsWith("prm_motcmn")) return 3;
            if (name.EndsWith("prm_skl")) return 4;
            if (name.EndsWith("prm_spl")) return 5;
            if (name.EndsWith("prm_upgrade01")) return 6;
            if (name.EndsWith("prm_upgrade02")) return 7;
            if (name.EndsWith("prm_upgrade03")) return 8;
            if (name.EndsWith("prm_boss")) return 9;
            if (name.EndsWith("prm_boss01")) return 10;
            if (name.EndsWith("prm_boss02")) return 11;
            if (name.EndsWith("prm_boss03")) return 12;
            if (name.EndsWith("prm_boss04")) return 13;
            return int.MaxValue;
        }

        private static string GetPrmSectionLabel(string chunkName)
        {
            string name = (chunkName ?? string.Empty).ToLowerInvariant();
            if (name.EndsWith("prm_awa2")) return "Awakening 2";
            if (name.EndsWith("prm_awa")) return "Awakening";
            if (name.EndsWith("prm_mot")) return "Base";
            if (name.EndsWith("prm_motcmn")) return "Damage animations";
            if (name.EndsWith("prm_skl")) return "Jutsu";
            if (name.EndsWith("prm_spl")) return "Ultimate Jutsu";
            if (name.EndsWith("prm_upgrade01")) return "Expansion A";
            if (name.EndsWith("prm_upgrade02")) return "Expansion B";
            if (name.EndsWith("prm_upgrade03")) return "Expansion C";
            if (name.EndsWith("prm_boss")) return "Boss";
            if (name.EndsWith("prm_boss01")) return "Boss 1";
            if (name.EndsWith("prm_boss02")) return "Boss 2";
            if (name.EndsWith("prm_boss03")) return "Boss 3";
            if (name.EndsWith("prm_boss04")) return "Boss 4";
            return string.IsNullOrWhiteSpace(chunkName) ? "Section" : chunkName;
        }

        private string FormatMovementListItem(byte[] section)
        {
            if (section == null || section.Length < 0x24)
                return "Frame: 0 Motion: 0";

            int frame = Main.b_ReadIntFromTwoBytes(section, 0x20);
            int function = Main.b_ReadIntFromTwoBytes(section, 0x22);
            string functionText = function >= 0 && function < t_function.Items.Count
                ? t_function.Items[function].ToString()
                : function.ToString();

            return "Frame: " + frame.ToString() + " Motion: " + functionText;
        }

        private static readonly string[] AllowedPrmSectionSuffixes =
        {
            "prm_awa2",
            "prm_awa",
            "prm_mot",
            "prm_motcmn",
            "prm_skl",
            "prm_spl",
            "prm_upgrade01",
            "prm_upgrade02",
            "prm_upgrade03",
            "prm_boss",
            "prm_boss01",
            "prm_boss02",
            "prm_boss03",
            "prm_boss04"
        };

        private static string GetPrmSectionSuffixFromLabel(string label)
        {
            switch (label)
            {
                case "Awakening 2": return "prm_awa2";
                case "Awakening": return "prm_awa";
                case "Base": return "prm_mot";
                case "Damage animations": return "prm_motcmn";
                case "Jutsu": return "prm_skl";
                case "Ultimate Jutsu": return "prm_spl";
                case "Expansion A": return "prm_upgrade01";
                case "Expansion B": return "prm_upgrade02";
                case "Expansion C": return "prm_upgrade03";
                case "Boss": return "prm_boss";
                case "Boss 1": return "prm_boss01";
                case "Boss 2": return "prm_boss02";
                case "Boss 3": return "prm_boss03";
                case "Boss 4": return "prm_boss04";
                default: return string.Empty;
            }
        }

        private string GetPrmSectionNamePrefix()
        {
            string firstChunkName = verChunkNames.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            if (string.IsNullOrWhiteSpace(firstChunkName))
                return string.Empty;

            foreach (string suffix in AllowedPrmSectionSuffixes)
            {
                if (firstChunkName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    return firstChunkName.Substring(0, firstChunkName.Length - suffix.Length);
            }

            return string.Empty;
        }

        private string BuildPrmChunkPath(string chunkName)
        {
            string existingPath = verChunkPaths.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            if (string.IsNullOrWhiteSpace(existingPath))
                return chunkName + ".bin";

            string normalized = existingPath.Replace('\\', '/');
            int slashIndex = normalized.LastIndexOf('/');
            string directory = slashIndex >= 0 ? normalized.Substring(0, slashIndex + 1) : string.Empty;
            string extension = Path.GetExtension(normalized);
            if (string.IsNullOrWhiteSpace(extension))
                extension = ".bin";

            return directory + chunkName + extension;
        }

        private void RefreshPrmSectionAddOptions()
        {
            if (prmSectionAddComboBox == null)
                return;

            string selected = prmSectionAddComboBox.SelectedItem as string;
            prmSectionAddComboBox.Items.Clear();

            foreach (string suffix in AllowedPrmSectionSuffixes)
            {
                string label = GetPrmSectionLabel(suffix);
                if (!sectionnames.Contains(label))
                    prmSectionAddComboBox.Items.Add(label);
            }

            if (!string.IsNullOrWhiteSpace(selected) && prmSectionAddComboBox.Items.Contains(selected))
                prmSectionAddComboBox.SelectedItem = selected;
            else if (prmSectionAddComboBox.Items.Count > 0)
                prmSectionAddComboBox.SelectedIndex = 0;
        }

        private void AddPrmSection()
        {
            if (!fileOpen)
            {
                MessageBox.Show("Open prm file");
                return;
            }

            string missingLabel = prmSectionAddComboBox.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(missingLabel))
            {
                MessageBox.Show("No additional PRM sections are available to add.");
                return;
            }

            string suffix = GetPrmSectionSuffixFromLabel(missingLabel);
            string prefix = GetPrmSectionNamePrefix();
            if (string.IsNullOrWhiteSpace(suffix) || string.IsNullOrWhiteSpace(prefix))
            {
                MessageBox.Show("Could not determine the new section name from the current file.");
                return;
            }

            string newChunkName = prefix + suffix;
            string newChunkPath = BuildPrmChunkPath(newChunkName);
            int insertIndex = Array.IndexOf(AllowedPrmSectionSuffixes, suffix);
            if (insertIndex < 0 || insertIndex > plAnmList.Count)
                insertIndex = plAnmList.Count;

            List<string> labels = sectionnames.ToList();
            labels.Insert(insertIndex, missingLabel);
            sectionnames = labels.ToArray();
            verChunkNames.Insert(insertIndex, newChunkName);
            verChunkPaths.Insert(insertIndex, newChunkPath);
            verSection.Insert(insertIndex, new byte[0x44]);
            plAnmList.Insert(insertIndex, new List<byte[]>());
            movementList.Insert(insertIndex, new List<List<byte[]>>());
            listBox1.Items.Insert(insertIndex, missingLabel);
            listBox1.SelectedIndex = insertIndex;
            RefreshPrmSectionAddOptions();

            if (autosave.Checked)
                AutoSave();
        }

        private void DeletePrmSection()
        {
            if (!fileOpen)
            {
                MessageBox.Show("Open prm file");
                return;
            }

            int sectionIndex = listBox1.SelectedIndex;
            if (sectionIndex < 0 || sectionIndex >= sectionnames.Length)
                return;

            string label = sectionnames[sectionIndex];
            if (string.IsNullOrWhiteSpace(GetPrmSectionSuffixFromLabel(label)))
                return;

            if (sectionIndex < verChunkNames.Count && !string.IsNullOrWhiteSpace(verChunkNames[sectionIndex]))
                removedVerChunkNames.Add(verChunkNames[sectionIndex]);

            listBox1.Items.RemoveAt(sectionIndex);
            sectionnames = sectionnames.Where((x, i) => i != sectionIndex).ToArray();
            if (sectionIndex < verChunkNames.Count) verChunkNames.RemoveAt(sectionIndex);
            if (sectionIndex < verChunkPaths.Count) verChunkPaths.RemoveAt(sectionIndex);
            if (sectionIndex < verSection.Count) verSection.RemoveAt(sectionIndex);
            if (sectionIndex < plAnmList.Count) plAnmList.RemoveAt(sectionIndex);
            if (sectionIndex < movementList.Count) movementList.RemoveAt(sectionIndex);
            RefreshPrmSectionAddOptions();

            anm_list.Items.Clear();
            mov_list.Items.Clear();
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = Math.Max(0, sectionIndex - 1);

            if (autosave.Checked)
                AutoSave();
        }

        private void LoadEffectAndCollisionChunks(List<XfbinBinaryChunkItem> binaryChunks)
        {
            effectSecFound = false;
            effectSecLength = 0;
            effectSecCount = 0;
            effectSecName.Clear();
            effectSecSkillName.Clear();
            effectSecSkillEntry.Clear();
            effectSecSkillValue.Clear();
            EffectSection.Clear();
            effectVisibleIndices.Clear();
            collisionSecLength = 0;
            collisionSecCount = 0;
            collisionSecTypeValue.Clear();
            collisionSecStateValue.Clear();
            collisionSecEnablerBoneValue.Clear();
            collisionSecRadiusValue.Clear();
            collisionSecYPosValue.Clear();
            collisionSecZPosValue.Clear();
            collisionSecBoneName.Clear();
            effectChunkName = "";
            effectChunkPath = "";
            collisionChunkName = "";
            collisionChunkPath = "";
            listBox2.Items.Clear();

            XfbinBinaryChunkItem effectChunk = binaryChunks.FirstOrDefault(x => ChunkNameContains(x, "prm_sklslot"));
            if (effectChunk != null)
            {
                effectChunkName = effectChunk.ChunkName ?? "";
                effectChunkPath = effectChunk.ChunkPath ?? "";
                byte[] effectBytes = effectChunk.BinaryData ?? new byte[0];
                effectSecFound = true;
                if (effectBytes.Length >= 4)
                {
                    effectSecLength = Main.b_ReadIntRev(effectBytes, 0);
                    effectSecCount = effectSecLength / 0x81;
                }
                else
                {
                    effectSecLength = 0;
                    effectSecCount = 0;
                }
                for (int z = 0; z < effectSecCount; z++)
                {
                    int skillIndex = 4 + (z * 0x81);
                    int skillNameIndex = skillIndex + 0x40;
                    int skillEntryIndex = skillIndex + 0x60;
                    int skillValueIndex = skillIndex + 0x80;
                    if (skillValueIndex >= effectBytes.Length)
                        break;

                    string skill = Main.b_ReadString2(effectBytes, skillIndex, 0x20);
                    string skillName = Main.b_ReadString2(effectBytes, skillNameIndex, 0x20);
                    string skillEntry = Main.b_ReadString2(effectBytes, skillEntryIndex, 0x20);
                    int skillValue = effectBytes[skillValueIndex];
                    effectSecName.Add(skill);
                    effectSecSkillName.Add(skillName);
                    effectSecSkillEntry.Add(skillEntry);
                    effectSecSkillValue.Add(skillValue);
                    if (IsEffectTerminator(effectSecName.Count - 1))
                        break;

                    effectVisibleIndices.Add(effectSecName.Count - 1);
                    listBox2.Items.Add(skill);
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                MessageBox.Show("Effect section couldn't be found. If you used character prm, send this file in modding group and make sure it's original file from game.\nEffects will not be affected!");
            }

            editCollisionOfPlayerToolStripMenuItem.Enabled = false;
            XfbinBinaryChunkItem collisionChunk = binaryChunks.FirstOrDefault(x => ChunkNameContains(x, "prm_hit"));
            if (collisionChunk != null)
            {
                collisionChunkName = collisionChunk.ChunkName ?? "";
                collisionChunkPath = collisionChunk.ChunkPath ?? "";
                byte[] collisionBytes = collisionChunk.BinaryData ?? new byte[0];
                if (collisionBytes.Length >= 8)
                {
                    collisionSecLength = Main.b_ReadIntRev(collisionBytes, 0);
                    collisionSecCount = Main.b_ReadInt(collisionBytes, 4);
                }
                else
                {
                    collisionSecLength = 0;
                    collisionSecCount = 0;
                }
                for (int z = 0; z < collisionSecCount; z++)
                {
                    int collisionTypeIndex = 8 + (z * 0x5C);
                    int collisionStateIndex = collisionTypeIndex + 0x4;
                    int collisionLoadBoneIndex = collisionTypeIndex + 0x8;
                    int collisionHitboxIndex = collisionTypeIndex + 0x10;
                    int collisionRadiusValueIndex = collisionTypeIndex + 0x50;
                    int collisionYPosValueIndex = collisionTypeIndex + 0x54;
                    int collisionZPosValueIndex = collisionTypeIndex + 0x58;
                    if (collisionZPosValueIndex + 3 >= collisionBytes.Length)
                        break;

                    int collisionType = collisionBytes[collisionTypeIndex];
                    int collisionState = collisionBytes[collisionStateIndex];
                    int collisionLoadBone = collisionBytes[collisionLoadBoneIndex];
                    long collisionRadiusValue = collisionBytes[collisionRadiusValueIndex] + collisionBytes[collisionRadiusValueIndex + 1] * 0x100 + collisionBytes[collisionRadiusValueIndex + 2] * 0x10000 + collisionBytes[collisionRadiusValueIndex + 3] * 0x1000000;
                    long collisionYPosValue = collisionBytes[collisionYPosValueIndex] + collisionBytes[collisionYPosValueIndex + 1] * 0x100 + collisionBytes[collisionYPosValueIndex + 2] * 0x10000 + collisionBytes[collisionYPosValueIndex + 3] * 0x1000000;
                    long collisionZPosValue = collisionBytes[collisionZPosValueIndex] + collisionBytes[collisionZPosValueIndex + 1] * 0x100 + collisionBytes[collisionZPosValueIndex + 2] * 0x10000 + collisionBytes[collisionZPosValueIndex + 3] * 0x1000000;
                    string collisionHitbox = Main.b_ReadString2(collisionBytes, collisionHitboxIndex, 0x20);

                    collisionSecTypeValue.Add(collisionType);
                    collisionSecStateValue.Add(collisionState);
                    collisionSecEnablerBoneValue.Add(collisionLoadBone);
                    collisionSecRadiusValue.Add(collisionRadiusValue);
                    collisionSecYPosValue.Add(collisionYPosValue);
                    collisionSecZPosValue.Add(collisionZPosValue);
                    collisionSecBoneName.Add(collisionHitbox);
                }
                editCollisionOfPlayerToolStripMenuItem.Enabled = true;
            }
        }

        private int GetEffectActualIndexFromVisibleIndex(int visibleIndex)
        {
            if (visibleIndex < 0 || visibleIndex >= effectVisibleIndices.Count)
                return -1;
            return effectVisibleIndices[visibleIndex];
        }

        private bool IsEffectTerminator(int index)
        {
            if (index < 0 || index >= effectSecName.Count)
                return false;

            return IsEndMarker(effectSecName[index]) ||
                   (index < effectSecSkillName.Count && IsEndMarker(effectSecSkillName[index])) ||
                   (index < effectSecSkillEntry.Count && IsEndMarker(effectSecSkillEntry[index]));
        }

        private static bool IsEndMarker(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string normalized = value.Replace("\0", string.Empty).Trim();
            return normalized.Equals("END", StringComparison.OrdinalIgnoreCase) ||
                   normalized.StartsWith("END", StringComparison.OrdinalIgnoreCase);
        }

        public void OpenFile(string loadPath = "")
        {
            editCollisionOfPlayerToolStripMenuItem.Enabled = false;
            if (loadPath == "")
            {
                OpenFileDialog o = new OpenFileDialog();
                o.ShowDialog();
                if (o.FileName == "" || File.Exists(o.FileName) == false) return;

                filePath = o.FileName;
            }
            else
                filePath = loadPath;

            listBox1.Items.Clear();
            anm_list.Items.Clear();
            mov_list.Items.Clear();
            verSection.Clear();
            anmCount.Clear();
            anmSection.Clear();
            verList.Clear();
            verLength.Clear();
            plAnmList.Clear();
            movementList.Clear();

            fileBytes = File.ReadAllBytes(filePath);
            List<XfbinBinaryChunkItem> binaryChunks = GetMovesetBinaryChunks(filePath);
            verChunkNames.Clear();
            verChunkPaths.Clear();
            removedVerChunkNames.Clear();

            // Find all ver sections
            int actualver = 0;
            while(actualver != -1)
            {
                actualver = XfbinParser.FindString(fileBytes, "ver0.000", actualver + 1);
                //MessageBox.Show(actualver.ToString("X2"));
                if (actualver != -1)
                {
                    verList.Add(actualver);
                    verLength.Add(Main.b_ReadIntRev(fileBytes, actualver - 4));
                }
            }

            List<XfbinBinaryChunkItem> verChunks = binaryChunks
                .Where(x => BufferContainsAscii(x.BinaryData, "ver0.000"))
                .Where(x => GetPrmVerChunkRank(x.ChunkName) != int.MaxValue)
                .OrderBy(x => GetPrmVerChunkRank(x.ChunkName))
                .ThenBy(x => x.ChunkName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToList();

            verSection.Clear();
            List<string> detectedSectionLabels = new List<string>();
            foreach (XfbinBinaryChunkItem chunk in verChunks)
            {
                verSection.Add(chunk.BinaryData);
                verChunkNames.Add(chunk.ChunkName ?? "");
                verChunkPaths.Add(chunk.ChunkPath ?? "");
                detectedSectionLabels.Add(GetPrmSectionLabel(chunk.ChunkName));
            }

            if (verSection.Count == 0)
            {
                for (int i = 0; i < verList.Count; i++)
                {
                    if (verList[i] < 0 || verLength[i] <= 0 || verList[i] + verLength[i] > fileBytes.Length)
                        continue;

                    byte[] section = new byte[verLength[i]];
                    Array.Copy(fileBytes, verList[i], section, 0, verLength[i]);
                    verSection.Add(section);
                    verChunkNames.Add(string.Empty);
                    verChunkPaths.Add(string.Empty);
                    detectedSectionLabels.Add("Section " + i.ToString());
                }
            }

            LoadEffectAndCollisionChunks(binaryChunks);
            sectionnames = detectedSectionLabels.ToArray();
            RefreshPrmSectionAddOptions();
            for (int a = 0; a < verSection.Count; a++)
            {
                string sectionLabel = a < sectionnames.Length ? sectionnames[a] : ("Section " + a.ToString());
                listBox1.Items.Add(sectionLabel);

                byte[] actualSection = verSection[a];
                if (actualSection.Length < 0x44)
                    continue;

                int anmSectionCount = Main.b_ReadInt(actualSection, 0x34);
                int start = 0x44;
                int index = 0x44;

                plAnmList.Add(new List<byte[]>());
                movementList.Add(new List<List<byte[]>>()); //

                //anmSection.Add(new List<byte[]>());
                //anmCount.Add(actualSection[0x30]);

                for (int x = 0; x < anmSectionCount; x++)
                {
                    if (start + 0xD4 > actualSection.Length)
                        break;

                    // Add this pl_anm's header to plAnmList
                    List<byte> planmheader = new List<byte>();
                    for (int y = 0; y < 0xD4; y++)
                    {
                        planmheader.Add(actualSection[start + y]);
                    }
                    //MessageBox.Show(Main.b_ReadString(planmheader.ToArray(), 0));
                    plAnmList[a].Add(planmheader.ToArray());
                    movementList[a].Add(new List<byte[]>());

                    index = start + 0x50;
                    byte m_movcount = actualSection[index];
                    //MessageBox.Show("ANM " + x.ToString() + " has " + m_movcount.ToString() + " sections");

                    index = start + 0xD4;

                    // Add each movement section of this pl_anm to the master list
                    for (int y = 0; y < m_movcount; y++)
                    {
                        List<byte> movementsection = new List<byte>();

                        // Default movement section length is 0x40
                        int sectionLength = 0x40;

                        if (index + 0x24 > actualSection.Length)
                            break;

                        int function = actualSection[index + 0x22] * 0x1 + actualSection[index + 0x23] * 0x100;
                        
                        switch(function)
                        {
                            case 0x83:
                                if (index + 0x40 < actualSection.Length)
                                {
                                    string str = Main.b_ReadString(actualSection, index + 0x40);
                                    if (str == "SPSKILL_END") sectionLength = 0xA0;
                                }
                                break;
                            case 0x5E:
                            case 0x8A:
                            case 0xC1:
                            case 0xC3:
                            case 0xC6:
                            case 0xC8:
                            case 0xCA:
                            case 0xD1:
                            case 0xD3:
                            case 0xD5:
                            case 0xD7:
                            case 0xD9:
                                sectionLength = 0xA0;
                                break;
                            case 0xA0:
                            case 0xA1:
                            case 0xA2:
                            case 0xA3:
                            case 0xA4:
                            case 0xA5:
                            case 0xA6:
                                if (index + 0x40 < actualSection.Length)
                                {
                                    string str = Main.b_ReadString(actualSection, index + 0x40);
                                    if (str.Length >= 7 && str.Substring(0, 7) == "SKL_ATK") sectionLength = 0xA0;
                                }
                                break;
                        }

                        // If there's a D (from DAMAGE_ID) in section + 0x40, length is 0xA0
                        if (index + 0x40 < actualSection.Length)
                        {
                            string str = Main.b_ReadString(actualSection, index + 0x40);
                            if(str.Length > 3 && (str.Substring(0, 3) == "DMG" || str.Substring(0, 3) == "DAM"))
                            {
                                sectionLength = 0xA0;
                            }

                            /*char act1 = (char)actualSection[index + 0x40];
                            if (!Char.IsDigit(act1) && Char.IsUpper(act1))
                            {
                                byte byte1 = actualSection[index + 0x40 + 0x2A]; // 20 
                                byte byte2 = actualSection[index + 0x40 + 0x2C]; // 22

                                if(byte1 == 0x0 && byte2 == 0x0)
                                {

                                }
                            }*/
                        }

                        // If the first letter of the hitbox is caps, then it's a special 0x60 section
                        // char act = (char)actualSection[index];
                        // if (actualSection[index] != 0x0 && Char.IsUpper(act) && !Char.IsDigit(act)) sectionLength = 0x40;

                        //MessageBox.Show("Movement " + y.ToString() + " of ANM " + x.ToString() + " is " + sectionLength.ToString("X2") + " bytes long");
                        if (index + sectionLength > actualSection.Length)
                            break;

                        for (int z = 0; z < sectionLength; z++) movementsection.Add(actualSection[z + index]);
                        index = index + sectionLength;

                        // Add to master list
                        movementList[a][x].Add(movementsection.ToArray());
                    }

                    start = index;
                }
            }

            fileOpen = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int actualIndex = listBox1.SelectedIndex;

            if (fileOpen == false || actualIndex == -1) return;

            ReadAnmList(actualIndex);
        }

        public void ReadAnmList(int sectionindex)
        {
            anm_list.Items.Clear();
            int anmSectionCount = plAnmList[sectionindex].Count;

            for (int x = 0; x < anmSectionCount; x++)
            {
                int index = 0x00;

                string m_planm = Main.b_ReadString(plAnmList[sectionindex][x], index);
                if (m_planm == "") m_planm = "[EMPTY]";

                anm_list.Items.Add(m_planm);
            }
        }

        private void anm_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            int actualIndex = anm_list.SelectedIndex;
            int currentVer = listBox1.SelectedIndex;

            if (fileOpen == false || actualIndex == -1) return;

            t_planm.Text = "";
            t_anm.Text = "";
            t_loadsection.Value = 0;
            t_1cmn.Checked = false;
            t_flag1.Checked = false;
            t_flag2.Checked = false;
            t_flag3.Checked = false;
            t_flag4.Checked = false;
            t_prevanm1.Text = "";
            t_prevanm2.Text = "";
            t_prevanm3.Text = "";
            t_distance.Text = "";
            t_direction.Value = 0;
            t_condition.SelectedIndex = -1;
            t_linkCondition.SelectedIndex = -1;
            t_length.Value = 0;
            t_btnpress.Value = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            t_hitboxid.Text = "";
            t_dmgid.Text = "";
            t_hiteffect.SelectedIndex = -1;
            t_dmgcond.SelectedIndex = -1;
            t_function.SelectedIndex = -1;
            t_timing1.Value = 0;
            t_param1_1.Value = 0;
            t_param2_1.Value = 0;
            t_param3_1.Value = 0;
            t_dmgamount.Value = 0;
            t_dmgguardamount.Value = 0;
            t_pushamount.Value = 0;
            t_vertpushamount.Value = 0;
            t_hits.Value = 0;
            t_freeze.Value = 0;
            t_unknown.Value = 0;

            ReadPlAnm(currentVer, actualIndex);
        }

        List<byte> attackData = new List<byte>();
        List<byte[]> movSection = new List<byte[]>();
        public void ReadPlAnm(int sectionindex, int planm)
        {
            byte[] actualSection = plAnmList[sectionindex][planm];

            // Clear old animations
            attackData.Clear();
            mov_list.Items.Clear();

            int index = 0;
            int start = 0;

            index = start + 0x00;
            string m_planm = Main.b_ReadString(actualSection, index);
            if (m_planm == "") m_planm = "[EMPTY]";
            t_planm.Text = m_planm;

            index = start + 0x20;
            string m_anim = Main.b_ReadString(actualSection, index);
            t_anm.Text = m_anim;

            index = start + 0x50;
            byte m_movcount = actualSection[index];

            index = start + 0x54;
            int frameSkip = actualSection[index] * 0x1 + actualSection[index + 0x1] * 0x100;
            numericUpDown1.Value = frameSkip;

            index = start + 0x56;
            byte sectionload = actualSection[index];
            t_loadsection.Value = sectionload;

            index = start + 0x58;
            byte cubeman = actualSection[index];
            t_1cmn.Checked = (cubeman == 1);

            index = start + 0x5A;
            bool flag1 = (actualSection[index] == 2);
            t_flag1.Checked = flag1;

            index = start + 0x5C;
            bool flag2 = (actualSection[index] == 1);
            t_flag2.Checked = flag2;

            index = start + 0x5E;
            bool flag3 = (actualSection[index] == 1);
            t_flag3.Checked = flag3;

            index = start + 0x60;
            bool flag4 = (actualSection[index] == 1);
            t_flag4.Checked = flag4;

            index = start + 0x62;
            // distance

            index = start + 0x68;
            byte direction = actualSection[index];
            t_direction.Value = direction;

            index = start + 0x6A;
            int linkCondition1 = actualSection[index];
            int linkCondition2 = actualSection[index+1];
            if (linkCondition1 == 0xFF && linkCondition2 == 0xFF)
                t_linkCondition.SelectedIndex = 0;
            else
                t_linkCondition.SelectedIndex = linkCondition1 + 1;
            //MessageBox.Show(index.ToString("X2"));

            index = start + 0x6C;
            byte condition = actualSection[index];
            t_condition.SelectedIndex = condition;
            //MessageBox.Show(index.ToString("X2"));

            

            index = start + 0x6E;
            int atttime = actualSection[index] * 0x1 + actualSection[index + 0x1] * 0x100;
            t_length.Value = atttime;

            index = start + 0x70;
            int btnpress = actualSection[index] * 0x1 + actualSection[index + 0x1] * 0x100;
            t_btnpress.Value = btnpress;

            index = start + 0x72;
            int cond2 = actualSection[index] * 0x1 + actualSection[index + 0x1] * 0x100;
            numericUpDown2.Value = cond2;

            index = start + 0x74;
            string planm1 = Main.b_ReadString(actualSection, index);
            t_prevanm1.Text = planm1;

            index = start + 0x94;
            string planm2 = Main.b_ReadString(actualSection, index);
            t_prevanm2.Text = planm2;

            index = start + 0xB4;
            string planm3 = Main.b_ReadString(actualSection, index);
            t_prevanm3.Text = planm3;

            ReloadMovementList(sectionindex, planm);
        }

        private void ReloadMovementList(int sectionindex, int planm)
        {
            mov_list.Items.Clear();
            if (sectionindex < 0 || planm < 0)
                return;
            if (sectionindex >= movementList.Count || planm >= movementList[sectionindex].Count)
                return;

            for (int x = 0; x < movementList[sectionindex][planm].Count; x++)
                mov_list.Items.Add(FormatMovementListItem(movementList[sectionindex][planm][x]));
        }

        private void mov_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            int actualindex = mov_list.SelectedIndex;
            int actualver = listBox1.SelectedIndex;
            int actualanm = anm_list.SelectedIndex;

            if (fileOpen == false || actualindex == -1) return;

            t_hitboxid.Text = "";
            t_dmgid.Text = "";
            t_hiteffect.SelectedIndex = -1;
            t_dmgcond.SelectedIndex = -1;
            t_function.SelectedIndex = -1;
            t_timing1.Value = 0;
            t_param1_1.Value = 0;
            t_param2_1.Value = 0;
            t_param3_1.Value = 0;
            t_param3.Value = 0;
            t_dmgamount.Value = 0;
            t_dmgguardamount.Value = 0;
            t_pushamount.Value = 0;
            t_vertpushamount.Value = 0;
            t_hits.Value = 0;
            t_freeze.Value = 0;
            t_unknown.Value = 0;

            ReadMovementSection(movementList[actualver][actualanm][actualindex]);
        }
        
        public void ReadMovementSection(byte[] section)
        {
            int len = section.Length;
            int index = 0;

            string hit = Main.b_ReadString(section, index);
            t_hitboxid.Text = hit;

            index = 0x20;
            byte[] timing1 = Main.b_ReadByteArray(section, index, 2);
            byte[] timingArray = new byte[4]
            {
                timing1[0],
                timing1[1],
                0,
                0
            };
            t_timing1.Value = Main.b_byteArrayToInt(timingArray);
            index = 0x22;
            int function = section[index] * 0x1 + section[index + 1] * 0x100;
            if (function > t_function.Items.Count) function = 0;
            t_function.SelectedIndex = function;

            index = 0x24;
            byte[] param1_1 = Main.b_ReadByteArray(section, index, 2);
            byte[] param1_1Array = new byte[4]
            {
                param1_1[0],
                param1_1[1],
                0,
                0
            };
            t_param1_1.Value = (short)Main.b_byteArrayToInt(param1_1Array);

            index = 0x26;
            byte[] param2_1 = Main.b_ReadByteArray(section, index, 2);
            byte[] param2_1Array = new byte[4]
            {
                param2_1[0],
                param2_1[1],
                0,
                0
            };
            t_param2_1.Value = (short)Main.b_byteArrayToInt(param2_1Array) ;
            byte[] param3_1 = Main.b_ReadByteArray(section, 0x28, 2);
            byte[] param3_1Array = new byte[4]
            {
                param3_1[0],
                param3_1[1],
                0,
                0
            };
            t_param3_1.Value = (short)Main.b_byteArrayToInt(param3_1Array);

            index = 0x2C;
            float param3 = Main.b_ReadFloat(section, index);
            t_param3.Value = (decimal)param3;

            if (len == 0xA0)
            {
                index = 0x40;
                string damageid = Main.b_ReadString(section, index);
                t_dmgid.Text = damageid;

                index = 0x82;
                int selectedhit = Main.b_ReadIntFromTwoBytes(section,index);
                if (selectedhit == 65535) selectedhit = -1;
                t_hiteffect.SelectedIndex = selectedhit;

                index = 0x86;
                int selectedmg = section[index];
                if (selectedmg == 65535) selectedmg = -1;
                t_dmgcond.SelectedIndex = selectedmg;

                index = 0x88;
                t_dmgamount.Value = (decimal)Main.b_ReadFloat(section, index);
                t_dmgguardamount.Value = (decimal)Main.b_ReadFloat(section, 0x8C);
                index = 0x90;
                t_pushamount.Value = (decimal)Main.b_ReadFloat(section, index);

                index = 0x94;
                t_vertpushamount.Value = (decimal)Main.b_ReadFloat(section, index);

                index = 0x98;
                t_hits.Value = section[index];
                t_unknown.Value = section[0x9A];
                t_freeze.Value = section[0x9C];
            }
        }

        private void t_secadd_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1) return;

                byte[] newsec = new byte[0x40];
                movementList[actualver][actualanm].Add(newsec);

                // Replace movement count
                plAnmList[actualver][actualanm][0x50] = (byte)movementList[actualver][actualanm].Count;

                mov_list.Items.Add(FormatMovementListItem(newsec));
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void t_secdup_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;
                int actualsec = mov_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1 || actualsec == -1) return;

                byte[] newsec = movementList[actualver][actualanm][actualsec];
                movementList[actualver][actualanm].Add(newsec.ToList().ToArray());

                // Replace movement count
                plAnmList[actualver][actualanm][0x50] = (byte)movementList[actualver][actualanm].Count;

                mov_list.Items.Add(FormatMovementListItem(newsec));
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void t_secdel_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;
                int actualsec = mov_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1 || actualsec == -1) return;

                if (actualsec == mov_list.Items.Count - 1) mov_list.SelectedIndex--;
                int newselection = mov_list.SelectedIndex;

                movementList[actualver][actualanm].RemoveAt(actualsec);
                mov_list.Items.RemoveAt(actualsec);
                mov_list.SelectedIndex = newselection;

                // Replace movement count
                plAnmList[actualver][actualanm][0x50] = (byte)movementList[actualver][actualanm].Count;
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void t_saveanm_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int ver = listBox1.SelectedIndex;
                int anm = anm_list.SelectedIndex;
                if (fileOpen == false || ver == -1 || anm == -1) return;

                // Replace pl_anm
                plAnmList[ver][anm] = Main.b_ReplaceBytes(plAnmList[ver][anm], new byte[0x20], 0);
                plAnmList[ver][anm] = Main.b_ReplaceString(plAnmList[ver][anm], t_planm.Text, 0);

                // Replace animation
                plAnmList[ver][anm] = Main.b_ReplaceBytes(plAnmList[ver][anm], new byte[0x20], 0x20);
                plAnmList[ver][anm] = Main.b_ReplaceString(plAnmList[ver][anm], t_anm.Text, 0x20);

                // Replace movement count
                plAnmList[ver][anm][0x50] = (byte)movementList[ver][anm].Count;

                byte[] frameSkipBytes = BitConverter.GetBytes((int)numericUpDown1.Value);
                plAnmList[ver][anm][0x54] = frameSkipBytes[0];
                plAnmList[ver][anm][0x55] = frameSkipBytes[1];

                // Replace loading section
                plAnmList[ver][anm][0x56] = (byte)t_loadsection.Value;

                // Replace cubeman
                plAnmList[ver][anm][0x58] = Convert.ToByte(t_1cmn.Checked);

                // Replace flags
                byte flg1 = 2;

                if (t_flag1.Checked == false) flg1 = 0;
                plAnmList[ver][anm][0x5A] = flg1;
                plAnmList[ver][anm][0x5C] = Convert.ToByte(t_flag2.Checked);
                plAnmList[ver][anm][0x5E] = Convert.ToByte(t_flag3.Checked);
                plAnmList[ver][anm][0x60] = Convert.ToByte(t_flag4.Checked);

                // Replace distance
                // code here

                // Replace direction
                plAnmList[ver][anm][0x68] = (byte)t_direction.Value;
                if (t_linkCondition.SelectedIndex - 1 != -1)
                {
                    plAnmList[ver][anm][0x6A] = Convert.ToByte(t_linkCondition.SelectedIndex - 1);
                    plAnmList[ver][anm][0x6B] = 0x00;
                }
                else
                {
                    plAnmList[ver][anm][0x6A] = 0xFF;
                    plAnmList[ver][anm][0x6B] = 0xFF;
                }
                // Replace conditions and timing
                plAnmList[ver][anm][0x6C] = (byte)t_condition.SelectedIndex;

                byte[] lengthbytes = BitConverter.GetBytes((int)t_length.Value);
                plAnmList[ver][anm][0x6E] = lengthbytes[0];
                plAnmList[ver][anm][0x6F] = lengthbytes[1];

                byte[] btnbytes = BitConverter.GetBytes((int)t_btnpress.Value);
                plAnmList[ver][anm][0x70] = btnbytes[0];
                plAnmList[ver][anm][0x71] = btnbytes[1];

                byte[] cond2Bytes = BitConverter.GetBytes((int)numericUpDown2.Value);
                plAnmList[ver][anm][0x72] = cond2Bytes[0];
                plAnmList[ver][anm][0x73] = cond2Bytes[1];

                // Replace prev pl_anms
                plAnmList[ver][anm] = Main.b_ReplaceBytes(plAnmList[ver][anm], new byte[0x20], 0x74);
                plAnmList[ver][anm] = Main.b_ReplaceString(plAnmList[ver][anm], t_prevanm1.Text, 0x74);

                plAnmList[ver][anm] = Main.b_ReplaceBytes(plAnmList[ver][anm], new byte[0x20], 0x94);
                plAnmList[ver][anm] = Main.b_ReplaceString(plAnmList[ver][anm], t_prevanm2.Text, 0x94);

                plAnmList[ver][anm] = Main.b_ReplaceBytes(plAnmList[ver][anm], new byte[0x20], 0xB4);
                plAnmList[ver][anm] = Main.b_ReplaceString(plAnmList[ver][anm], t_prevanm3.Text, 0xB4);

                anm_list.Items[anm] = t_planm.Text;
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void t_savesec_Click(object sender, EventArgs e)
        {
            if(fileOpen)
            {
                int ind = mov_list.SelectedIndex;
                int ver = listBox1.SelectedIndex;
                int anm = anm_list.SelectedIndex;
                if (fileOpen == false || ind == -1 || ver == -1 || anm == -1) return;

                // Replace hitbox id
                movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], new byte[0x20], 0);
                movementList[ver][anm][ind] = Main.b_ReplaceString(movementList[ver][anm][ind], t_hitboxid.Text, 0);

                // Replace timing
                byte[] byteTiming = BitConverter.GetBytes((int)t_timing1.Value);
                movementList[ver][anm][ind][0x20] = byteTiming[0];
                movementList[ver][anm][ind][0x21] = byteTiming[1];

                // Replace function
                byte[] funct_bytes = BitConverter.GetBytes(t_function.SelectedIndex);
                movementList[ver][anm][ind][0x22] = funct_bytes[0];
                movementList[ver][anm][ind][0x23] = funct_bytes[1];

                // Replace parameters
                byte[] byteParam1 = BitConverter.GetBytes((short)t_param1_1.Value);
                byte[] byteParam2 = BitConverter.GetBytes((short)t_param2_1.Value);
                byte[] byteParam3 = BitConverter.GetBytes((short)t_param3_1.Value);
                movementList[ver][anm][ind][0x24] = byteParam1[0];
                movementList[ver][anm][ind][0x25] = byteParam1[1];
                movementList[ver][anm][ind][0x26] = byteParam2[0];
                movementList[ver][anm][ind][0x27] = byteParam2[1];
                movementList[ver][anm][ind][0x28] = byteParam3[0];
                movementList[ver][anm][ind][0x29] = byteParam3[1];

                // Param 3
                byte[] floatparam3 = BitConverter.GetBytes(Convert.ToSingle(t_param3.Value));
                movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], floatparam3, 0x2C);

                int function = t_function.SelectedIndex;
                int newLength = 0x40;

                switch (function)
                {
                    case 0x83:
                        if (t_dmgid.Text == "SPSKILL_END") newLength = 0xA0;
                        break;
                    case 0x5E:
                    case 0x8A:
                    case 0xC1:
                    case 0xC3:
                    case 0xC6:
                    case 0xC8:
                    case 0xCA:
                    case 0xD1:
                    case 0xD3:
                    case 0xD5:
                    case 0xD7:
                    case 0xD9:
                        newLength = 0xA0;
                        break;
                    case 0xA0:
                    case 0xA1:
                    case 0xA2:
                    case 0xA3:
                    case 0xA4:
                    case 0xA5:
                    case 0xA6:
                        if (t_dmgid.Text.Length > 7 && t_dmgid.Text.Substring(0, 7) == "SKL_ATK") newLength = 0xA0;
                        break;
                }

                // Transform to 0x40 section if there's no damage id
                if (t_dmgid.Text == "" && newLength == 0x40)
                {
                    if (movementList[ver][anm][ind].Length > 0x40)
                    {
                        int addbytes = 0xA0 - movementList[ver][anm][ind].Length;
                        byte[] newMovement = new byte[0x40];

                        for (int x = 0; x < 0x40; x++)
                        {
                            newMovement[x] = movementList[ver][anm][ind][x];
                        }

                        movementList[ver][anm][ind] = newMovement;
                    }
                }
                else
                {
                    // Otherwise, transform to 0xA0 section and add data
                    if (movementList[ver][anm][ind].Length < 0xA0)
                    {
                        int addbytes = 0xA0 - movementList[ver][anm][ind].Length;
                        movementList[ver][anm][ind] = Main.b_AddBytes(movementList[ver][anm][ind], new byte[addbytes]);
                    }

                    // Replace damage_id
                    movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], new byte[0x20], 0x40);
                    movementList[ver][anm][ind] = Main.b_ReplaceString(movementList[ver][anm][ind], t_dmgid.Text, 0x40);

                    // Replace hit effect and sound
                    movementList[ver][anm][ind][0x82] = (byte)t_hiteffect.SelectedIndex;

                    // Replace damage condition
                    movementList[ver][anm][ind][0x86] = (byte)t_dmgcond.SelectedIndex;

                    // Replace damage amount
                    byte[] dmgamount = BitConverter.GetBytes(Convert.ToSingle(t_dmgamount.Value));
                    movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], dmgamount, 0x88);

                    // Replace damage guard amount
                    byte[] dmgguardamount = BitConverter.GetBytes(Convert.ToSingle(t_dmgguardamount.Value));
                    movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], dmgguardamount, 0x8C);

                    // Replace push away amount
                    byte[] pushaway = BitConverter.GetBytes(Convert.ToSingle(t_pushamount.Value));
                    movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], pushaway, 0x90);

                    // Replace rise push away amount
                    byte[] rise = BitConverter.GetBytes(Convert.ToSingle(t_vertpushamount.Value));
                    movementList[ver][anm][ind] = Main.b_ReplaceBytes(movementList[ver][anm][ind], rise, 0x94);

                    // Replace hit count
                    movementList[ver][anm][ind][0x98] = (byte)t_hits.Value;
                    movementList[ver][anm][ind][0x9C] = (byte)t_freeze.Value;
                    movementList[ver][anm][ind][0x9a] = (byte)t_unknown.Value;
                }
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        public byte[] GenerateFile()
        {
            byte[] newBytes = new byte[0];
            byte[] test = new byte[0];
            newBytes = Main.b_AddBytes(newBytes, fileBytes, 0, 0, verList[0]);

            int verCount = plAnmList.Count;

            for (int x = 0; x < verCount; x++)
            {
                int countLength = newBytes.Length;

                // Add header of ver
                byte[] header = new byte[0x40];
                header[0x00] = 0x76;
                header[0x01] = 0x65;
                header[0x02] = 0x72;
                header[0x03] = 0x30;
                header[0x04] = 0x2E;
                header[0x05] = 0x30;
                header[0x06] = 0x30;
                header[0x07] = 0x30;
                header[0x30] = (byte)plAnmList[x].Count;
                newBytes = Main.b_AddBytes(newBytes, header);

                // Add each pl_anm
                for (int y = 0; y < plAnmList[x].Count; y++)
                {
                    newBytes = Main.b_AddBytes(newBytes, plAnmList[x][y]);

                    for (int z = 0; z < movementList[x][y].Count; z++)
                    {
                        newBytes = Main.b_AddBytes(newBytes, movementList[x][y][z]);
                    }
                }

                int totalLen = newBytes.Length - countLength;
                newBytes = Main.b_ReplaceBytes(newBytes, BitConverter.GetBytes(totalLen), countLength - 0x04, 1);
                newBytes = Main.b_ReplaceBytes(newBytes, BitConverter.GetBytes(totalLen + 4), countLength - 0x10, 1);

                int start = verList[x];
                int end = start + verLength[x];
                int next = fileBytes.Length;

                if (x < verCount - 1) next = verList[x + 1];

                int totalBytesToAdd = next - end;
                byte[] toaddempty = new byte[totalBytesToAdd];

                int actual = newBytes.Length;
                newBytes = Main.b_AddBytes(newBytes, toaddempty);

                for (int y = 0; y < totalBytesToAdd; y++)
                {
                    newBytes[actual + y] = fileBytes[end + y];
                }
            }
            if (effectSecFound)
            {
                int FirstSize = 0;
                byte[] EffectSections = new byte[0];
                for (int z=0;z<effectSecCount;z++)
                {
                    byte[] NewEffectSection = new byte[0x81];
                    byte[] EffectSectionName = new byte[0];
                    EffectSectionName = Main.b_AddBytes(EffectSectionName, Encoding.ASCII.GetBytes(effectSecName[z]));
                    NewEffectSection = Main.b_ReplaceBytes(NewEffectSection, EffectSectionName, 0);

                    byte[] EffectSectionSkillName = new byte[0];
                    EffectSectionSkillName = Main.b_AddBytes(EffectSectionSkillName, Encoding.ASCII.GetBytes(effectSecSkillName[z]));
                    NewEffectSection = Main.b_ReplaceBytes(NewEffectSection, EffectSectionSkillName, 0x40);

                    byte[] EffectSectionSkillEntry = new byte[0];
                    EffectSectionSkillEntry = Main.b_AddBytes(EffectSectionSkillEntry, Encoding.ASCII.GetBytes(effectSecSkillEntry[z]));
                    NewEffectSection = Main.b_ReplaceBytes(NewEffectSection, EffectSectionSkillEntry, 0x60);

                    byte[] EffectSectionSkillValue = new byte[1]
                    {
                        (byte)effectSecSkillValue[z]
                    };

                    NewEffectSection = Main.b_ReplaceBytes(NewEffectSection, EffectSectionSkillValue, 0x80);
                    EffectSections = Main.b_AddBytes(EffectSections, NewEffectSection);
                    FirstSize = FirstSize + 0x81;
                }
                int newEffectPos = -1;
                int motDMG = XfbinParser.FindString(newBytes, "prm_motcmn", 0);
                int awaSec = XfbinParser.FindString(newBytes, "prm_awa", 0);
                if (motDMG == -1 && awaSec!=-1)
                {
                    //Effect data
                    byte[] EffectEntry = new byte[8]
                    {
                        0x00,
                        0x00,
                        0x00,
                        0x07,
                        0x00,
                        0x63,
                        0x00,
                        0x00
                    };
                    //MessageBox.Show(EffectPos.ToString("X2"));
                    newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x07,
                            0x00,
                            0x63
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x07,
                            0x00,
                            0x79
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x07,
                            0x00,
                            0x7A
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                }
                else if (motDMG != -1 && awaSec != -1)
                {
                    //Effect data
                    byte[] EffectEntry = new byte[8]
                    {
                        0x00,
                        0x00,
                        0x00,
                        0x08,
                        0x00,
                        0x63,
                        0x00,
                        0x00
                    };
                    //MessageBox.Show(EffectPos.ToString("X2"));
                    newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                        0x00,
                        0x00,
                        0x00,
                        0x08,
                        0x00,
                        0x63
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                        0x00,
                        0x00,
                        0x00,
                        0x08,
                        0x00,
                        0x79
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                        0x00,
                        0x00,
                        0x00,
                        0x08,
                        0x00,
                        0x7A
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                }
                else if (motDMG != -1 && awaSec == -1)
                {
                    //Effect data
                    byte[] EffectEntry = new byte[8]
                    {
                        0x00,
                        0x00,
                        0x00,
                        0x07,
                        0x00,
                        0x63,
                        0x00,
                        0x00
                    };
                    //MessageBox.Show(EffectPos.ToString("X2"));
                    newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x07,
                            0x00,
                            0x63
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x07,
                            0x00,
                            0x79
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x07,
                            0x00,
                            0x7A
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                }
                else if (motDMG == -1 && awaSec == -1)
                {
                    //Effect data
                    byte[] EffectEntry = new byte[8]
                    {
                        0x00,
                        0x00,
                        0x00,
                        0x06,
                        0x00,
                        0x63,
                        0x00,
                        0x00
                    };
                    //MessageBox.Show(EffectPos.ToString("X2"));
                    newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x06,
                            0x00,
                            0x63
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x06,
                            0x00,
                            0x79
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                    if (newEffectPos == -1)
                    {
                        EffectEntry = new byte[6]
                        {
                            0x00,
                            0x00,
                            0x00,
                            0x06,
                            0x00,
                            0x7A
                        };
                        newEffectPos = XfbinParser.FindBytes(newBytes, EffectEntry, 0);
                    }
                }
                int newLastEffectPos = newEffectPos + 12;
                int newEffectSecLength = Main.b_ReadIntRev(newBytes, newLastEffectPos - 4);
                List<byte> ChangedEffectSection = new List<byte>();
                for (int j=0; j< newBytes.Length;j++)
                {
                    ChangedEffectSection.Add(newBytes[j]);
                }
                for (int j= newLastEffectPos; j< newLastEffectPos+ newEffectSecLength;j++)
                {
                    ChangedEffectSection.RemoveAt(newLastEffectPos);
                }
                for (int j=0; j<EffectSections.Length;j++)
                {
                    ChangedEffectSection.Insert(newLastEffectPos+j, EffectSections[j]);
                }
                test = new byte[ChangedEffectSection.Count];
                for (int j=0; j< ChangedEffectSection.Count;j++)
                {
                    test[j] = ChangedEffectSection[j];
                }
                byte[] sizeBytes3 = BitConverter.GetBytes(FirstSize);
                byte[] sizeBytes4 = BitConverter.GetBytes(FirstSize+4);
                test = Main.b_ReplaceBytes(test, sizeBytes3, newLastEffectPos - 4, 1);
                test = Main.b_ReplaceBytes(test, sizeBytes4, newLastEffectPos - 16, 1);

                newBytes = test;

                // COLLISION

                if (collisionChanged)
                {
                    test = new byte[0];
                    FirstSize = 0;
                    byte[] CollisionSections = new byte[0];
                    for (int z = 0; z < collisionSecCount; z++)
                    {
                        byte[] NewCollisionSection = new byte[0x5C];

                        byte[] CollisionType = new byte[1]
                        {
                            (byte)collisionSecTypeValue[z]
                        };
                        NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionType, 0x00);

                        byte[] CollisionState = new byte[1]
                        {
                            (byte)collisionSecStateValue[z]
                        };
                        NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionState, 0x04);

                        byte[] CollisionBoneEnabler = new byte[1]
                        {
                            (byte)collisionSecEnablerBoneValue[z]
                        };
                        NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionBoneEnabler, 0x08);

                        byte[] CollisionOrder = new byte[1]
                        {
                            (byte)(z+1)
                        };
                        NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionOrder, 0x0C);

                        if (collisionSecEnablerBoneValue[z] == 0)
                        {
                            byte[] CollisionSectionHitbox = new byte[0];
                            CollisionSectionHitbox = Main.b_AddBytes(CollisionSectionHitbox, Encoding.ASCII.GetBytes(collisionSecBoneName[z]));
                            NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionSectionHitbox, 0x10);
                        }
                        byte[] CollisionRadius = BitConverter.GetBytes(collisionSecRadiusValue[z]);
                        for (int k=0; k<4; k++)
                        {
                            byte[] CollisionRadiusByte = new byte[1]
                            {
                                (byte)CollisionRadius[k]
                            };
                            NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionRadiusByte, 0x50+k);
                        }
                        byte[] CollisionYPos = BitConverter.GetBytes(collisionSecYPosValue[z]);
                        for (int k = 0; k < 4; k++)
                        {
                            byte[] CollisionYPosByte = new byte[1]
                            {
                                (byte)CollisionYPos[k]
                            };
                            NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionYPosByte, 0x54 + k);
                        }
                        byte[] CollisionZPos = BitConverter.GetBytes(collisionSecZPosValue[z]);
                        for (int k = 0; k < 4; k++)
                        {
                            byte[] CollisionYPosByte = new byte[1]
                            {
                                (byte)CollisionZPos[k]
                            };
                            NewCollisionSection = Main.b_ReplaceBytes(NewCollisionSection, CollisionYPosByte, 0x58 + k);
                        }
                        

                        CollisionSections = Main.b_AddBytes(CollisionSections, NewCollisionSection);
                        FirstSize = FirstSize + 0x5C;
                    }
                    //Collision data
                    byte[] CollisionEntry = new byte[8]
                    {
                        0x00,
                        0x00,
                        0x00,
                        0x03,
                        0x00,
                        0x63,
                        0x00,
                        0x00
                    };
                    int newCollisionPos = XfbinParser.FindBytes(newBytes, CollisionEntry, 0);
                    if (newCollisionPos == -1)
                    {
                        CollisionEntry = new byte[6]
                        {
                        0x00,
                        0x00,
                        0x00,
                        0x03,
                        0x00,
                        0x63
                        };
                        newCollisionPos = XfbinParser.FindBytes(newBytes, CollisionEntry, 0);
                    }
                    if (newCollisionPos == -1)
                    {
                        CollisionEntry = new byte[6]
                        {
                        0x00,
                        0x00,
                        0x00,
                        0x03,
                        0x00,
                        0x79
                        };
                        newCollisionPos = XfbinParser.FindBytes(newBytes, CollisionEntry, 0);
                    }
                    if (newCollisionPos == -1)
                    {
                        CollisionEntry = new byte[6]
                        {
                        0x00,
                        0x00,
                        0x00,
                        0x03,
                        0x00,
                        0x7A
                        };
                        newCollisionPos = XfbinParser.FindBytes(newBytes, CollisionEntry, 0);
                    }
                    int newLastCollisionPos = newCollisionPos + 16;
                    int newCollisionSecLength = Main.b_ReadIntRev(newBytes, newLastCollisionPos - 8)-4;
                    List<byte> ChangedCollisionSection = new List<byte>();
                    for (int j = 0; j < newBytes.Length; j++)
                    {
                        ChangedCollisionSection.Add(newBytes[j]);
                    }
                    for (int j = newLastCollisionPos; j < newLastCollisionPos + newCollisionSecLength; j++)
                    {
                        ChangedCollisionSection.RemoveAt(newLastCollisionPos);
                    }
                    for (int j = 0; j < CollisionSections.Length; j++)
                    {
                        ChangedCollisionSection.Insert(newLastCollisionPos + j, CollisionSections[j]);
                    }
                    test = new byte[ChangedCollisionSection.Count];
                    for (int j = 0; j < ChangedCollisionSection.Count; j++)
                    {
                        test[j] = ChangedCollisionSection[j];
                    }
                    sizeBytes3 = BitConverter.GetBytes(FirstSize + 4);
                    sizeBytes4 = BitConverter.GetBytes(FirstSize + 8);
                    byte[] sizeBytes5 = BitConverter.GetBytes(collisionSecCount);
                    byte[] reversedCount = new byte[4]
                    {
                        sizeBytes5[3],
                        sizeBytes5[2],
                        sizeBytes5[1],
                        sizeBytes5[0]
                    };
                    test = Main.b_ReplaceBytes(test, reversedCount, newLastCollisionPos - 4, 1);
                    test = Main.b_ReplaceBytes(test, sizeBytes3, newLastCollisionPos - 8, 1);
                    test = Main.b_ReplaceBytes(test, sizeBytes4, newLastCollisionPos - 20, 1);

                    newBytes = test;
                }
            }
            return newBytes;
        }

        private byte[] BuildVerChunkData(int index)
        {
            byte[] output = new byte[0];
            byte[] header = new byte[0x44];
            header[0x04] = 0x76;
            header[0x05] = 0x65;
            header[0x06] = 0x72;
            header[0x07] = 0x30;
            header[0x08] = 0x2E;
            header[0x09] = 0x30;
            header[0x0A] = 0x30;
            header[0x0B] = 0x30;
            header = Main.b_ReplaceBytes(header, BitConverter.GetBytes(plAnmList[index].Count), 0x34);
            output = Main.b_AddBytes(output, header);

            for (int y = 0; y < plAnmList[index].Count; y++)
            {
                output = Main.b_AddBytes(output, plAnmList[index][y]);
                for (int z = 0; z < movementList[index][y].Count; z++)
                    output = Main.b_AddBytes(output, movementList[index][y][z]);
            }

            int dataSize = output.Length - 4;
            byte[] sizeBytes = BitConverter.GetBytes(dataSize);
            output[0] = sizeBytes[3];
            output[1] = sizeBytes[2];
            output[2] = sizeBytes[1];
            output[3] = sizeBytes[0];

            return output;
        }

        private byte[] BuildEffectChunkData()
        {
            byte[] effectSections = new byte[4];
            for (int z = 0; z < effectSecCount; z++)
            {
                byte[] newEffectSection = new byte[0x81];
                byte[] effectSectionName = new byte[0];
                effectSectionName = Main.b_AddBytes(effectSectionName, Encoding.ASCII.GetBytes(effectSecName[z]));
                newEffectSection = Main.b_ReplaceBytes(newEffectSection, effectSectionName, 0);

                byte[] effectSectionSkillName = new byte[0];
                effectSectionSkillName = Main.b_AddBytes(effectSectionSkillName, Encoding.ASCII.GetBytes(effectSecSkillName[z]));
                newEffectSection = Main.b_ReplaceBytes(newEffectSection, effectSectionSkillName, 0x40);

                byte[] effectSectionSkillEntry = new byte[0];
                effectSectionSkillEntry = Main.b_AddBytes(effectSectionSkillEntry, Encoding.ASCII.GetBytes(effectSecSkillEntry[z]));
                newEffectSection = Main.b_ReplaceBytes(newEffectSection, effectSectionSkillEntry, 0x60);

                byte[] effectSectionSkillValue = new byte[1] { (byte)effectSecSkillValue[z] };
                newEffectSection = Main.b_ReplaceBytes(newEffectSection, effectSectionSkillValue, 0x80);
                effectSections = Main.b_AddBytes(effectSections, newEffectSection);
            }

            int dataSize = effectSections.Length - 4;
            byte[] sizeBytes = BitConverter.GetBytes(dataSize);
            effectSections[0] = sizeBytes[3];
            effectSections[1] = sizeBytes[2];
            effectSections[2] = sizeBytes[1];
            effectSections[3] = sizeBytes[0];

            return effectSections;
        }

        private byte[] BuildCollisionChunkData()
        {
            byte[] collisionSections = new byte[8];
            collisionSections = Main.b_ReplaceBytes(collisionSections, BitConverter.GetBytes(collisionSecCount), 4);
            for (int z = 0; z < collisionSecCount; z++)
            {
                byte[] newCollisionSection = new byte[0x5C];

                newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, BitConverter.GetBytes(collisionSecTypeValue[z]), 0x00);
                newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, BitConverter.GetBytes(collisionSecStateValue[z]), 0x04);
                newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, BitConverter.GetBytes(collisionSecEnablerBoneValue[z]), 0x08);
                newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, BitConverter.GetBytes(z + 1), 0x0C);

                if (collisionSecEnablerBoneValue[z] == 0)
                {
                    byte[] collisionSectionHitbox = new byte[0];
                    collisionSectionHitbox = Main.b_AddBytes(collisionSectionHitbox, Encoding.ASCII.GetBytes(collisionSecBoneName[z]));
                    newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, collisionSectionHitbox, 0x10);
                }

                byte[] collisionRadius = BitConverter.GetBytes(collisionSecRadiusValue[z]);
                byte[] collisionYPos = BitConverter.GetBytes(collisionSecYPosValue[z]);
                byte[] collisionZPos = BitConverter.GetBytes(collisionSecZPosValue[z]);
                for (int k = 0; k < 4; k++)
                {
                    newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, new byte[1] { collisionRadius[k] }, 0x50 + k);
                    newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, new byte[1] { collisionYPos[k] }, 0x54 + k);
                    newCollisionSection = Main.b_ReplaceBytes(newCollisionSection, new byte[1] { collisionZPos[k] }, 0x58 + k);
                }

                collisionSections = Main.b_AddBytes(collisionSections, newCollisionSection);
            }

            int dataSize = collisionSections.Length - 4;
            byte[] sizeBytes = BitConverter.GetBytes(dataSize);
            collisionSections[0] = sizeBytes[3];
            collisionSections[1] = sizeBytes[2];
            collisionSections[2] = sizeBytes[1];
            collisionSections[3] = sizeBytes[0];

            return collisionSections;
        }

        private void SaveWithBackend(string outputPath)
        {
            string sourcePath = filePath;
            using (XfbinParserBackend backend = new XfbinParserBackend(sourcePath))
            {
                for (int i = 0; i < verChunkNames.Count && i < plAnmList.Count && i < verChunkPaths.Count; i++)
                {
                    backend.UpsertBinaryChunk(verChunkNames[i], verChunkNames[i], verChunkPaths[i], BuildVerChunkData(i));
                }

                foreach (string removedChunkName in removedVerChunkNames.Distinct(StringComparer.OrdinalIgnoreCase).ToList())
                    backend.DeleteBinaryChunk(removedChunkName);

                if (!string.IsNullOrWhiteSpace(effectChunkName) && !string.IsNullOrWhiteSpace(effectChunkPath) && effectSecFound)
                    backend.UpsertBinaryChunk(effectChunkName, effectChunkName, effectChunkPath, BuildEffectChunkData());

                if (!string.IsNullOrWhiteSpace(collisionChunkName) && !string.IsNullOrWhiteSpace(collisionChunkPath) && collisionSecCount > 0)
                    backend.UpsertBinaryChunk(collisionChunkName, collisionChunkName, collisionChunkPath, BuildCollisionChunkData());

                backend.RepackTo(outputPath);
            }

            filePath = outputPath;
            fileBytes = File.ReadAllBytes(outputPath);
            removedVerChunkNames.Clear();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                if (File.Exists(filePath + ".backup")) File.Delete(filePath + ".backup");
                File.Copy(filePath, filePath + ".backup");
                SaveWithBackend(filePath);
                MessageBox.Show("File saved to " + filePath);
            }
            else
            {
                MessageBox.Show("You need to open file first!");
            }
        }

        private void AutoSave()
        {
            if (File.Exists(filePath + ".backup")) File.Delete(filePath + ".backup");
            File.Copy(filePath, filePath + ".backup");
            SaveWithBackend(filePath);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        public void SaveFileAs(string basepath = "") {
            if (fileOpen) {
                SaveFileDialog s = new SaveFileDialog();
                {
                    s.DefaultExt = ".xfbin";
                    s.Filter = "*.xfbin|*.xfbin";
                }
                if (basepath != "")
                    s.FileName = basepath;
                else
                    s.ShowDialog();

                if (s.FileName != "") {
                    SaveWithBackend(s.FileName);
                    if (basepath == "")
                        MessageBox.Show("File saved to " + filePath);
                }
            } else {
                MessageBox.Show("You need to open file first!");
            }
        }

        private void t_add_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;

                if (fileOpen == false || actualver == -1) return;

                byte[] newsec = new byte[0xD4];
                plAnmList[actualver].Add(newsec);
                movementList[actualver].Add(new List<byte[]>());
                anm_list.Items.Add("[EMPTY]");
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void t_dup_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1) return;

                //byte[] newsec = plAnmList[actualver][actualanm];
                //List<byte[]> newsec_2 = movementList[actualver][actualanm];

                //plAnmList[actualver].Add(newsec);
                //movementList[actualver].Add(movementList[actualver][actualanm].ToList());


                int length = plAnmList[actualver][actualanm].Length;
                byte[] newsec = new byte[length];
                plAnmList[actualver][actualanm].CopyTo(newsec, 0);
                List<byte[]> newsec_3 = new List<byte[]>();
                int count = movementList[actualver][actualanm].Count;
                for (int i = 0; i < count; i++)
                {
                    length = movementList[actualver][actualanm][i].Length;
                    byte[] newsec_2 = new byte[length];
                    movementList[actualver][actualanm][i].CopyTo(newsec_2, 0);
                    newsec_3.Add(newsec_2);
                }
                plAnmList[actualver].Insert(actualanm, newsec);
                movementList[actualver].Insert(actualanm, newsec_3);
                //movementList[actualver].Insert(actualanm, movementList[actualver][actualanm]);


                anm_list.Items.Insert(actualanm, Main.b_ReadString(newsec, 0));
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void t_del_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;

                if (fileOpen != true || listBox1.SelectedIndex != -1 || anm_list.SelectedIndex != -1)
                {
                    anm_list.SelectedIndex--;
                    anm_list.Items.RemoveAt(actualanm);
                    plAnmList[actualver].RemoveAt(actualanm);
                    movementList[actualver].RemoveAt(actualanm);
                }
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        void CloseFile(bool message = false, bool closePath = true)
        {
            if(message)
            {
                DialogResult r = MessageBox.Show("Are you sure you want to close this file?", "", MessageBoxButtons.YesNo);

                if (r == DialogResult.No) return;
            }
            editCollisionOfPlayerToolStripMenuItem.Enabled = false;
            if (closePath)
                filePath = "";
            fileOpen = false;
            effectSecFound = false;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            anm_list.Items.Clear();
            mov_list.Items.Clear();

            fileBytes = new byte[0];

            verSection.Clear();
            anmCount.Clear();
            anmSection.Clear();
            verList.Clear();
            verLength.Clear();
            verChunkNames.Clear();
            verChunkPaths.Clear();
            removedVerChunkNames.Clear();
            plAnmList.Clear();
            movementList.Clear();
            if (prmSectionAddComboBox != null)
                prmSectionAddComboBox.Items.Clear();
            effectSecName.Clear();
            effectSecSkillName.Clear();
            effectSecSkillEntry.Clear();
            effectSecSkillValue.Clear();
            effectChunkName = "";
            effectChunkPath = "";
            collisionChunkName = "";
            collisionChunkPath = "";
            collisionChanged = false;
            collisionSecTypeValue.Clear();
            collisionSecStateValue.Clear();
            collisionSecEnablerBoneValue.Clear();
            collisionSecRadiusValue.Clear();
            collisionSecYPosValue.Clear();
            collisionSecZPosValue.Clear();
            collisionSecBoneName.Clear();

            t_planm.Text = "";
            t_anm.Text = "";
            t_loadsection.Value = 0;
            t_1cmn.Checked = false;
            t_flag1.Checked = false;
            t_flag2.Checked = false;
            t_flag3.Checked = false;
            t_flag4.Checked = false;
            t_prevanm1.Text = "";
            t_prevanm2.Text = "";
            t_prevanm3.Text = "";
            t_distance.Text = "";
            t_direction.Value = 0;
            t_condition.SelectedIndex = -1;
            t_linkCondition.SelectedIndex = -1;
            t_length.Value = 0;
            t_btnpress.Value = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            t_hitboxid.Text = "";
            t_dmgid.Text = "";
            t_hiteffect.SelectedIndex = -1;
            t_dmgcond.SelectedIndex = -1;
            t_function.SelectedIndex = -1;
            t_timing1.Value = 0;
            t_param1_1.Value = 0;
            t_param2_1.Value = 0;
            t_param3_1.Value = 0;
            t_dmgamount.Value = 0;
            t_dmgguardamount.Value = 0;
            t_pushamount.Value = 0;
            t_vertpushamount.Value = 0;
            t_hits.Value = 0;
            t_freeze.Value = 0;
            t_unknown.Value = 0;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fileOpen) CloseFile(true);
        }

        void SwitchPlAnmAndMove(int actualver, int actualanm, int switchnum)
        {
            // Switch pl anm
            byte[] thisplanm = plAnmList[actualver][actualanm];
            plAnmList[actualver][actualanm] = plAnmList[actualver][switchnum];
            plAnmList[actualver][switchnum] = thisplanm;

            string planmname = anm_list.Items[actualanm].ToString();
            anm_list.Items[actualanm] = anm_list.Items[switchnum].ToString();
            anm_list.Items[switchnum] = planmname;

            // Switch movementList
            List<byte[]> anmmov = movementList[actualver][actualanm];
            movementList[actualver][actualanm] = movementList[actualver][switchnum];
            movementList[actualver][switchnum] = anmmov;

            anm_list.SelectedIndex = switchnum;
        }

        private void b_moveuppl_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1) return;

                if (actualanm != 0)
                {
                    int switchnum = actualanm - 1;

                    SwitchPlAnmAndMove(actualver, actualanm, switchnum);
                }
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }    
            else
            {
                MessageBox.Show("Open prm file");
            }    
        }

        private void b_movedownpl_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1) return;

                if (actualanm != anm_list.Items.Count - 1)
                {
                    int switchnum = actualanm + 1;

                    SwitchPlAnmAndMove(actualver, actualanm, switchnum);
                }
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        void SwitchMove(int actualver, int actualanm, int actualmov, int switchmov)
        {
            // Switch movementList
            byte[] mov = movementList[actualver][actualanm][actualmov];
            movementList[actualver][actualanm][actualmov] = movementList[actualver][actualanm][switchmov];
            movementList[actualver][actualanm][switchmov] = mov;

            mov_list.SelectedIndex = switchmov;
        }

        private void b_moveupmov_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;
                int actualmov = mov_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1) return;

                if (actualmov != 0)
                {
                    int switchnum = actualmov - 1;

                    SwitchMove(actualver, actualanm, actualmov, switchnum);
                }
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void b_movedownmov_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                int actualver = listBox1.SelectedIndex;
                int actualanm = anm_list.SelectedIndex;
                int actualmov = mov_list.SelectedIndex;

                if (fileOpen == false || actualver == -1 || actualanm == -1) return;

                if (actualmov < mov_list.Items.Count - 1)
                {
                    int switchnum = actualmov + 1;

                    SwitchMove(actualver, actualanm, actualmov, switchnum);
                }
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
            else
            {
                MessageBox.Show("Open prm file");
            }
        }

        private void movSortButton_Click(object sender, EventArgs e)
        {
            if (!fileOpen)
            {
                MessageBox.Show("Open prm file");
                return;
            }

            int actualver = listBox1.SelectedIndex;
            int actualanm = anm_list.SelectedIndex;
            if (actualver == -1 || actualanm == -1)
                return;

            DialogResult result = MessageBox.Show(
                "When sorting frame timings it can lead to unintended issues. Are you sure?",
                "Sort movement entries",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            List<byte[]> sorted = movementList[actualver][actualanm]
                .OrderBy(x => Main.b_ReadIntFromTwoBytes(x, 0x20))
                .ToList();

            movementList[actualver][actualanm] = sorted;
            ReloadMovementList(actualver, actualanm);
            if (mov_list.Items.Count > 0)
                mov_list.SelectedIndex = 0;

            if (autosave.Checked)
                AutoSave();
        }

        private void setCubemanToEveryANMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ver = listBox1.SelectedIndex;
            if (ver == -1) return;

            for(int x = 0; x < plAnmList[ver].Count; x++)
            {
                plAnmList[ver][x][0x58] = 1;
            }

            if (plAnmList[ver].Count > 0) t_1cmn.Checked = true;
        }

        private void setCubemanOffInAllPlanmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ver = listBox1.SelectedIndex;
            if (ver == -1) return;

            for (int x = 0; x < plAnmList[ver].Count; x++)
            {
                plAnmList[ver][x][0x58] = 0;
            }

            if(plAnmList[ver].Count > 0) t_1cmn.Checked = false;
        }

        private void t_hiteffect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void prmSectionAddButton_Click(object sender, EventArgs e)
        {
            AddPrmSection();
        }

        private void prmSectionDeleteButton_Click(object sender, EventArgs e)
        {
            DeletePrmSection();
        }

        private void t_function_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void DamageFunctionEnabler()
        {
            if (t_dmgid.Text != "")
            {
                t_dmgamount.Enabled = true;
                t_pushamount.Enabled = true;
                t_vertpushamount.Enabled = true;
                t_hits.Enabled = true;
                t_dmgguardamount.Enabled = true;
                t_freeze.Enabled = true;
                t_unknown.Enabled = true;
                t_dmgcond.Enabled = true;
                t_hiteffect.Enabled = true;
            }
            else
            {
                t_dmgamount.Enabled = false;
                t_pushamount.Enabled = false;
                t_vertpushamount.Enabled = false;
                t_hits.Enabled = false;
                t_dmgguardamount.Enabled = false;
                t_freeze.Enabled = false;
                t_unknown.Enabled = false;
                t_dmgcond.Enabled = false;
                t_hiteffect.Enabled = false;
            }
        }
        private void t_dmgid_TextChanged(object sender, EventArgs e)
        {
            DamageFunctionEnabler();
        }

        private void Tool_MovesetCoder_Load(object sender, EventArgs e)
        {
            DamageFunctionEnabler();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void t_param1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (fileOpen)
            //{
                
            //    //int actualver = listBox1.SelectedIndex;
            //    //int actualanm = anm_list.SelectedIndex;

            //    //if (fileOpen == false || actualver == -1 || actualanm == -1) return;

            //    //int length = plAnmList[actualver][actualanm].Length;

            //    //plAnmList[actualver][actualanm].CopyTo(newsec, 0);
            //    //List<byte[]> newsec_3 = new List<byte[]>();
            //    //int count = movementList[actualver][actualanm].Count;
            //    //for (int i = 0; i < count; i++)
            //    //{
            //    //    length = movementList[actualver][actualanm][i].Length;
            //    //    byte[] newsec_2 = new byte[length];
            //    //    movementList[actualver][actualanm][i].CopyTo(newsec_2, 0);
            //    //    newsec_3.Add(newsec_2);
            //    //}
            //    //plAnmList[actualver].Insert(actualanm, newsec);
            //    //movementList[actualver].Insert(actualanm, newsec_3);
            //    ////movementList[actualver].Insert(actualanm, movementList[actualver][actualanm]);


            //    //anm_list.Items.Insert(actualanm, Main.b_ReadString(newsec, 0));
            //}
            //else
            //{
            //    MessageBox.Show("Open prm file");
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void t_dmgamount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void t_pushamount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            if (effectSecFound)
            {
                int insertIndex = effectSecName.Count;
                if (effectSecName.Count > 0 && IsEffectTerminator(effectSecName.Count - 1))
                    insertIndex = effectSecName.Count - 1;

                effectSecName.Insert(insertIndex, t_skillSectionName.Text);
                effectSecSkillName.Insert(insertIndex, t_skillFileName.Text);
                effectSecSkillEntry.Insert(insertIndex, t_skillEntry.Text);
                effectSecSkillValue.Insert(insertIndex, (int)t_skillValue.Value);
                for (int i = 0; i < effectVisibleIndices.Count; i++)
                {
                    if (effectVisibleIndices[i] >= insertIndex)
                        effectVisibleIndices[i]++;
                }
                int visibleInsertIndex = effectVisibleIndices.Count;
                if (insertIndex < effectSecName.Count - 1 && effectSecName.Count > 0 && IsEffectTerminator(effectSecName.Count - 1))
                    visibleInsertIndex = effectVisibleIndices.Count;
                else
                    visibleInsertIndex = effectVisibleIndices.Count;
                effectVisibleIndices.Add(insertIndex);
                listBox2.Items.Insert(listBox2.Items.Count, t_skillSectionName.Text);
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                effectSecCount++;
                if (autosave.Checked == true)
                {
                    AutoSave();
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (effectSecFound)
            {
                if (listBox2.SelectedIndex != -1)
                {
                    int actualIndex = GetEffectActualIndexFromVisibleIndex(listBox2.SelectedIndex);
                    if (actualIndex == -1)
                        return;

                    effectSecName[actualIndex] = t_skillSectionName.Text;
                    effectSecSkillName[actualIndex] = t_skillFileName.Text;
                    effectSecSkillEntry[actualIndex] = t_skillEntry.Text;
                    effectSecSkillValue[actualIndex] = (int)t_skillValue.Value;
                    listBox2.Items[listBox2.SelectedIndex] = t_skillSectionName.Text;
                    if (autosave.Checked == true)
                    {
                        AutoSave();
                    }
                }
                else
                {
                    MessageBox.Show("Select effect entry");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (effectSecFound)
            {
                if (listBox2.SelectedIndex != -1)
                {
                    int visibleIndex = listBox2.SelectedIndex;
                    int actualIndex = GetEffectActualIndexFromVisibleIndex(visibleIndex);
                    if (actualIndex == -1)
                        return;

                    effectSecName.RemoveAt(actualIndex);
                    effectSecSkillName.RemoveAt(actualIndex);
                    effectSecSkillEntry.RemoveAt(actualIndex);
                    effectSecSkillValue.RemoveAt(actualIndex);
                    effectVisibleIndices.RemoveAt(visibleIndex);
                    for (int i = 0; i < effectVisibleIndices.Count; i++)
                    {
                        if (effectVisibleIndices[i] > actualIndex)
                            effectVisibleIndices[i]--;
                    }
                    listBox2.Items.RemoveAt(visibleIndex);
                    listBox2.SelectedIndex = visibleIndex - 1;
                    effectSecCount--;
                    if (autosave.Checked == true)
                    {
                        AutoSave();
                    }
                }
                else
                {
                    MessageBox.Show("Select effect entry");
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                int actualIndex = GetEffectActualIndexFromVisibleIndex(listBox2.SelectedIndex);
                if (actualIndex == -1)
                    return;

                t_skillSectionName.Text = effectSecName[actualIndex];
                t_skillFileName.Text = effectSecSkillName[actualIndex];
                t_skillEntry.Text = effectSecSkillEntry[actualIndex];
                t_skillValue.Value = effectSecSkillValue[actualIndex];
            }
        }

        private void autosave_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cb_hexValue_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_hexValue.Checked == true)
            {
                t_timing1.Hexadecimal = true;
                t_param1_1.Hexadecimal = true;
                t_param2_1.Hexadecimal = true;
                t_param3_1.Hexadecimal = true;
                t_freeze.Hexadecimal = true;
                t_unknown.Hexadecimal = true;
                t_hits.Hexadecimal = true;
                t_loadsection.Hexadecimal = true;
                t_length.Hexadecimal = true;
                t_btnpress.Hexadecimal = true;
                numericUpDown1.Hexadecimal = true;
                numericUpDown2.Hexadecimal = true;
            }
            else
            {
                t_timing1.Hexadecimal = false;
                t_param1_1.Hexadecimal = false;
                t_param2_1.Hexadecimal = false;
                t_param3_1.Hexadecimal = false;
                t_freeze.Hexadecimal = false;
                t_unknown.Hexadecimal = false;
                t_hits.Hexadecimal = false;
                t_loadsection.Hexadecimal = false;
                t_length.Hexadecimal = false;
                t_btnpress.Hexadecimal = false;
                numericUpDown1.Hexadecimal = false;
                numericUpDown2.Hexadecimal = false;
            }
        }

        private void editCollisionOfPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                OpenPositionEditor();
            }
            else
            {
                MessageBox.Show("No file loaded...");
            }
        }
        public void OpenPositionEditor()
        {

            Tool_playerCollisionEditor t = new Tool_playerCollisionEditor(this, collisionSecTypeValue, collisionSecStateValue, collisionSecEnablerBoneValue, collisionSecRadiusValue, collisionSecYPosValue, collisionSecZPosValue, collisionSecBoneName, collisionSecCount);
            t.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            index1 = listBox1.SelectedIndex;
            index2 = anm_list.SelectedIndex;
            index3 = mov_list.SelectedIndex;
            if (fileOpen) CloseFile(false,false);
            else CloseFile();

            OpenFile(filePath);
            listBox1.SelectedIndex = index1;
            anm_list.SelectedIndex = index2;
            mov_list.SelectedIndex = index3;
        }

        private void pRMPortToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tool_PortPRM t = new Tool_PortPRM(this, fileOpen, sectionnames);
            t.Show();
        }

        private void listOfAllSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PRMEditorInfo t = new PRMEditorInfo();
            t.Show();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This tool will help you to edit moveset of any character. Find prm file of your character in data/spc folder so you can start edit it.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (anm_list.SelectedIndex != -1)
                {
                    byte[] CopiedCode = plAnmList[listBox1.SelectedIndex][anm_list.SelectedIndex];
                    for (int i=0; i<movementList[listBox1.SelectedIndex][anm_list.SelectedIndex].Count; i++)
                    {
                        CopiedCode = Main.b_AddBytes(CopiedCode, movementList[listBox1.SelectedIndex][anm_list.SelectedIndex][i]);
                    }
                    string convertedCode = "";
                    for (int i=0; i<CopiedCode.Length; i++)
                    {
                        convertedCode = convertedCode + CopiedCode[i].ToString("X2");
                    }
                    Clipboard.SetText(convertedCode);
                }
                else
                {
                    MessageBox.Show("Select PL_ANM section which you want to copy to buffer.");
                }
            }
            else
            {
                MessageBox.Show("Select Ver section");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (Clipboard.GetText()!="")
                {
                    byte[] actualSection = Main.b_StringToBytes(Clipboard.GetText());
                    if (actualSection.Length >= 0xD4)
                    {
                        int secCount = Main.b_ReadIntFromTwoBytes(actualSection, 0x50);
                        if (actualSection.Length >= 0xD4 + (secCount * 0x40))
                        {

                            string PL_ANM_Name = Main.b_ReadString2(actualSection, 0);
                            if (PL_ANM_Name == "")
                                PL_ANM_Name = "[EMPTY]";
                            anm_list.Items.Add(PL_ANM_Name);
                            int a = listBox1.SelectedIndex;
                            List<byte> planmheader = new List<byte>();
                            for (int y = 0; y < 0xD4; y++)
                            {
                                planmheader.Add(actualSection[y]);
                            }
                            //MessageBox.Show(Main.b_ReadString(planmheader.ToArray(), 0));
                            plAnmList[a].Add(planmheader.ToArray());
                            movementList[a].Add(new List<byte[]>());

                            int index = 0x50;
                            byte m_movcount = actualSection[index];
                            index = 0xD4;
                            for (int y = 0; y < m_movcount; y++)
                            {
                                List<byte> movementsection = new List<byte>();

                                // Default movement section length is 0x40
                                int sectionLength = 0x40;

                                int function = actualSection[index + 0x22] * 0x1 + actualSection[index + 0x23] * 0x100;

                                switch (function)
                                {
                                    case 0x83:
                                        if (index + 0x40 < actualSection.Length)
                                        {
                                            string str = Main.b_ReadString(actualSection, index + 0x40);
                                            if (str == "SPSKILL_END") sectionLength = 0xA0;
                                        }
                                        break;
                                    case 0x8A:
                                    case 0xC1:
                                    case 0xC3:
                                    case 0xC6:
                                    case 0xC8:
                                    case 0xCA:
                                    case 0xD1:
                                    case 0xD3:
                                    case 0xD5:
                                    case 0xD7:
                                    case 0xD9:
                                        sectionLength = 0xA0;
                                        break;
                                    case 0xA0:
                                    case 0xA1:
                                    case 0xA2:
                                    case 0xA3:
                                    case 0xA4:
                                    case 0xA5:
                                    case 0xA6:
                                        if (index + 0x40 < actualSection.Length)
                                        {
                                            string str = Main.b_ReadString(actualSection, index + 0x40);
                                            if (str.Length >= 7 && str.Substring(0, 7) == "SKL_ATK") sectionLength = 0xA0;
                                        }
                                        break;
                                }
                                if (index + 0x40 < actualSection.Length)
                                {
                                    string str = Main.b_ReadString(actualSection, index + 0x40);
                                    if (str.Length > 3 && (str.Substring(0, 3) == "DMG" || str.Substring(0, 3) == "DAM"))
                                    {
                                        sectionLength = 0xA0;
                                    }
                                }

                                for (int z = 0; z < sectionLength; z++) movementsection.Add(actualSection[z + index]);
                                index = index + sectionLength;

                                // Add to master list
                                movementList[a][movementList[a].Count - 1].Add(movementsection.ToArray());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wrong format of PL_ANM section");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong format of PL_ANM section");
                    }
                }
                else
                {
                    MessageBox.Show("Buffer is empty");
                }
                
            }
            else
            {
                MessageBox.Show("Select Ver section before pasting code!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (anm_list.SelectedIndex != -1)
                {
                    if (mov_list.SelectedIndex != -1)
                    {
                        byte[] CopiedCode = movementList[listBox1.SelectedIndex][anm_list.SelectedIndex][mov_list.SelectedIndex];
                        string convertedCode = "";
                        for (int i = 0; i < CopiedCode.Length; i++)
                        {
                            convertedCode = convertedCode + CopiedCode[i].ToString("X2");
                        }
                        Clipboard.SetText(convertedCode);
                    }
                    else
                    {
                        MessageBox.Show("Select movement section which you want to copy to buffer.");
                    }
                }
                else
                {
                    MessageBox.Show("Select PL_ANM section which you want to copy to buffer.");
                }
            }
            else
            {
                MessageBox.Show("Select Ver section");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (anm_list.SelectedIndex != -1)
                {
                    if (mov_list.SelectedIndex != -1)
                    {
                        if (Clipboard.GetText() != "")
                        {
                            byte[] newsec = Main.b_StringToBytes(Clipboard.GetText());
                            if (newsec.Length >= 0x40 && newsec.Length <= 0xA0)
                            {
                                movementList[listBox1.SelectedIndex][anm_list.SelectedIndex][mov_list.SelectedIndex] = newsec;
                                int index = mov_list.SelectedIndex;
                                mov_list.SelectedIndex = -1;
                                mov_list.SelectedIndex = index;


                            }
                            else
                            {
                                MessageBox.Show("Wrong format of section");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Buffer is empty");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select movement section");
                    }
                }
                else
                {
                    MessageBox.Show("Select PL_ANM section.");
                }
            }
            else
            {
                MessageBox.Show("Select VER section.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (anm_list.SelectedIndex != -1)
                {
                    if (Clipboard.GetText() != "")
                    {
                        byte[] newsec = Main.b_StringToBytes(Clipboard.GetText());
                        if (newsec.Length >= 0x40 && newsec.Length <= 0xA0)
                        {
                            movementList[listBox1.SelectedIndex][anm_list.SelectedIndex].Add(newsec);

                            // Replace movement count
                            plAnmList[listBox1.SelectedIndex][anm_list.SelectedIndex][0x50] = (byte)movementList[listBox1.SelectedIndex][anm_list.SelectedIndex].Count;

                            mov_list.Items.Add(FormatMovementListItem(newsec));
                            mov_list.SelectedIndex = -1;
                            mov_list.SelectedIndex = mov_list.Items.Count-1;
                        }
                        else
                        {
                            MessageBox.Show("Wrong format of section");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Buffer is empty");
                    }
                }
                else
                {
                    MessageBox.Show("Select PL_ANM section.");
                }
            }
            else
            {
                MessageBox.Show("Select VER section.");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (Clipboard.GetText() != "")
                {
                    byte[] actualSection = Main.b_StringToBytes(Clipboard.GetText());
                    if (actualSection.Length > 0xD4)
                    {
                        int secCount = Main.b_ReadIntFromTwoBytes(actualSection,0x50);
                        if (actualSection.Length >= 0xD4 + (secCount * 0x40))
                        {

                            string PL_ANM_Name = Main.b_ReadString2(actualSection, 0);
                            if (PL_ANM_Name == "")
                                PL_ANM_Name = "[EMPTY]";
                            anm_list.Items.Add(PL_ANM_Name);
                            int a = listBox1.SelectedIndex;
                            List<byte> planmheader = new List<byte>();
                            for (int y = 0; y < 0xD4; y++)
                            {
                                planmheader.Add(actualSection[y]);
                            }
                            //MessageBox.Show(Main.b_ReadString(planmheader.ToArray(), 0));
                            plAnmList[a].Add(planmheader.ToArray());
                            movementList[a].Add(new List<byte[]>());

                            int index = 0x50;
                            byte m_movcount = actualSection[index];
                            index = 0xD4;
                            for (int y = 0; y < m_movcount; y++)
                            {
                                List<byte> movementsection = new List<byte>();

                                // Default movement section length is 0x40
                                int sectionLength = 0x40;

                                int function = actualSection[index + 0x22] * 0x1 + actualSection[index + 0x23] * 0x100;

                                switch (function)
                                {
                                    case 0x83:
                                        if (index + 0x40 < actualSection.Length)
                                        {
                                            string str = Main.b_ReadString(actualSection, index + 0x40);
                                            if (str == "SPSKILL_END") sectionLength = 0xA0;
                                        }
                                        break;
                                    case 0x8A:
                                    case 0xC1:
                                    case 0xC3:
                                    case 0xC6:
                                    case 0xC8:
                                    case 0xCA:
                                    case 0xD1:
                                    case 0xD3:
                                    case 0xD5:
                                    case 0xD7:
                                    case 0xD9:
                                        sectionLength = 0xA0;
                                        break;
                                    case 0xA0:
                                    case 0xA1:
                                    case 0xA2:
                                    case 0xA3:
                                    case 0xA4:
                                    case 0xA5:
                                    case 0xA6:
                                        if (index + 0x40 < actualSection.Length)
                                        {
                                            string str = Main.b_ReadString(actualSection, index + 0x40);
                                            if (str.Length >= 7 && str.Substring(0, 7) == "SKL_ATK") sectionLength = 0xA0;
                                        }
                                        break;
                                }
                                if (index + 0x40 < actualSection.Length)
                                {
                                    string str = Main.b_ReadString(actualSection, index + 0x40);
                                    if (str.Length > 3 && (str.Substring(0, 3) == "DMG" || str.Substring(0, 3) == "DAM"))
                                    {
                                        sectionLength = 0xA0;
                                    }
                                }

                                for (int z = 0; z < sectionLength; z++) movementsection.Add(actualSection[z + index]);
                                index = index + sectionLength;

                                // Add to master list
                                movementList[a][movementList[a].Count - 1].Add(movementsection.ToArray());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wrong format of PL_ANM section");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong format of PL_ANM section");
                    }
                }
                else
                {
                    MessageBox.Show("Buffer is empty");
                }

            }
            else
            {
                MessageBox.Show("Select Ver section before pasting code!");
            }
        }
    }
}
