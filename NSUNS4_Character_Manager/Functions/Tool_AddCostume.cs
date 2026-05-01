using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using NSUNS4_Character_Manager.Tools;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_AddCostume : Form
    {
        Main mf;

        public Tool_AddCostume(Main m)
        {
            mf = m;
            InitializeComponent();
        }
        public int iconID = 0;
        public int iconID_cost = 0;

        private static bool HasNumericSuffix(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 2) return false;
            return int.TryParse(value.Substring(value.Length - 2, 2), NumberStyles.None, CultureInfo.InvariantCulture, out _);
        }

        private static string GetCharacterPrefix(string value)
        {
            if (HasNumericSuffix(value))
            {
                return value.Substring(0, value.Length - 2);
            }

            return value;
        }

        private static string ReadUnlockMessage(byte[] entry)
        {
            byte[] msg = Main.b_ReadByteArray(entry, 8, 16);
            int terminator = Array.IndexOf(msg, (byte)0);
            if (terminator >= 0)
            {
                Array.Resize(ref msg, terminator);
            }

            return Encoding.ASCII.GetString(msg).Trim();
        }

        private static byte[] BuildUnlockEntry(int presetId, byte dlcId, int versionValue, bool disableNotification, string messageId)
        {
            byte[] entry = new byte[0x18];
            entry[0] = dlcId;
            entry[1] = 0x19;

            byte[] presetBytes = BitConverter.GetBytes((short)presetId);
            entry[2] = presetBytes[0];
            entry[3] = presetBytes[1];

            byte[] versionBytes = disableNotification ? BitConverter.GetBytes(-1) : BitConverter.GetBytes(versionValue);
            entry[4] = versionBytes[0];
            entry[5] = versionBytes[1];
            entry[6] = versionBytes[2];
            entry[7] = versionBytes[3];

            byte[] messageBytes = Encoding.ASCII.GetBytes(messageId ?? "");
            for (int i = 0; i < messageBytes.Length && i < 16; i++)
            {
                entry[8 + i] = messageBytes[i];
            }

            return entry;
        }

        private static int GetCharacterSlotNumber(string value, string prefix)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(prefix)) return int.MaxValue;
            if (value.Length != prefix.Length + 2 || !value.StartsWith(prefix, StringComparison.Ordinal)) return int.MaxValue;

            if (int.TryParse(value.Substring(value.Length - 2, 2), NumberStyles.None, CultureInfo.InvariantCulture, out int slot))
            {
                return slot;
            }

            return int.MaxValue;
        }

        private static string GetFirstNonEmptyValue(IList<string> values)
        {
            if (values == null) return "";

            for (int i = 0; i < values.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(values[i]))
                {
                    return values[i];
                }
            }

            return "";
        }

        public int AddCostume()
        {
            string character = w_base.Text;
            string model = w_model.Text;
            string awamodel = awaModel.Text;
            string characterPrefix = GetCharacterPrefix(character);
            string customDisplayName = unlockMessage.Text.Trim();

            // Open DPP
            Tool_DuelPlayerParamEditor dpp = new Tool_DuelPlayerParamEditor();
            dpp.OpenFile(Main.dppPath);

            // Find DPP index
            int dppIndex = -1;
            for (int x = 0; x < dpp.EntryCount; x++)
            {
                string thischaracter = dpp.CharaList[x];
                if (thischaracter == character)
                {
                    dppIndex = x;
                    x = dpp.EntryCount;
                }
            }
            if (dppIndex == -1)
            {
                if(this.Visible) MessageBox.Show("Base character not found in duelPlayerParam.");
                return 1;
            }

            if (string.IsNullOrWhiteSpace(awamodel))
            {
                awamodel = GetFirstNonEmptyValue(dpp.AwkCostumeList[dppIndex]);
            }

            // Find null costume and add ours
            int dpp_costId = -1;
            dpp.listBox1.SelectedIndex = dppIndex;
            for (int x = 0; x < 20; x++)
            {
                if (dpp.CostumeList[dppIndex][x] == "")
                {
                    dpp.CostumeList[dppIndex][x] = model;
                    dpp.AwkCostumeList[dppIndex][x] = awamodel;
                    dpp_costId = x;
                    x = 20;
                }
            }
            if (dpp_costId == -1)
            {
                if(this.Visible) MessageBox.Show("This character's costumes are full.");
                return 2;
            }

            // Open PSP
            Tool_PlayerSettingParamEditor psp = new Tool_PlayerSettingParamEditor();
            psp.OpenFile(Main.pspPath);

            // Find PSP entry
            int pspIndex = -1;
            for (int x = 0; x < psp.EntryCount; x++)
            {
                string thischaracter = psp.CharacterList[x];
                if (string.Equals(thischaracter, character, StringComparison.Ordinal))
                {
                    pspIndex = x;
                    break;
                }
            }
            if (pspIndex == -1)
            {
                for (int x = 0; x < psp.EntryCount; x++)
                {
                    string thischaracter = psp.CharacterList[x];
                    if (thischaracter.Length >= character.Length && thischaracter.StartsWith(character, StringComparison.Ordinal))
                    {
                        pspIndex = x;
                        break;
                    }
                }
            }
            if (pspIndex == -1)
            {
                if(this.Visible) MessageBox.Show("Base character not found in playerSettingParam.");
                return 3;
            }

            int basePresetId = Main.b_byteArrayToInt(psp.PresetList[pspIndex]);
            int defaultNameIndex = -1;
            int defaultNameSlot = int.MaxValue;
            for (int x = 0; x < psp.EntryCount; x++)
            {
                int slotNumber = GetCharacterSlotNumber(psp.CharacterList[x], characterPrefix);
                if (slotNumber < defaultNameSlot)
                {
                    defaultNameSlot = slotNumber;
                    defaultNameIndex = x;
                }
            }
            if (defaultNameIndex == -1)
            {
                defaultNameIndex = pspIndex;
            }

            string resolvedDisplayName = customDisplayName;
            if (string.IsNullOrWhiteSpace(resolvedDisplayName))
            {
                resolvedDisplayName = psp.c_cha_b_List[defaultNameIndex];
            }
            if (string.IsNullOrWhiteSpace(resolvedDisplayName))
            {
                resolvedDisplayName = character;
            }

            // Create psp entry for our costume
            psp.ListBox1.SelectedIndex = pspIndex;
            psp.AddID();
            pspIndex = psp.ListBox1.Items.Count - 1;

            // Keep new presets above the current max preset id instead of backfilling gaps.
            int newPresetId = 0;
            for (int x = 0; x < psp.EntryCount - 1; x++)
            {
                int presetId = Main.b_byteArrayToInt(psp.PresetList[x]);
                if (presetId > newPresetId) newPresetId = presetId;
            }
            newPresetId++;
            while (psp.PresetList.Take(psp.EntryCount - 1).Any(preset => Main.b_byteArrayToInt(preset) == newPresetId))
            {
                newPresetId++;
            }
            psp.PresetList[pspIndex] = BitConverter.GetBytes(newPresetId);

            // Set a new name (this will find an unused number, like 3obt00, 3obt01, 3obt02, until a number is not used)
            int maxNum = 0;
            for (int x = 0; x < psp.EntryCount; x++)
            {
                string pspCharacter = psp.CharacterList[x];
                if (pspCharacter.Length == characterPrefix.Length + 2 && pspCharacter.StartsWith(characterPrefix, StringComparison.Ordinal))
                {
                    string suffix = pspCharacter.Substring(pspCharacter.Length - 2, 2);
                    if (int.TryParse(suffix, NumberStyles.None, CultureInfo.InvariantCulture, out int actualNum) && actualNum > maxNum)
                    {
                        maxNum = actualNum;
                    }
                }
            }
            string characterPspName = characterPrefix;
            maxNum = maxNum + 1;

            characterPspName += maxNum.ToString("D2");

            psp.CharacterList[pspIndex] = characterPspName;
            psp.OptValueA[pspIndex] = dpp_costId;
            psp.OptValueE[pspIndex] = basePresetId;
            psp.c_cha_b_List[pspIndex] = resolvedDisplayName;

            // Open unlock evo item tool
            Tool_EvoUnlockItemParamEditor unlock = new Tool_EvoUnlockItemParamEditor();
            unlock.OpenFile(Main.unlockEvoItemParamPath);
            if (!unlock.FileOpen || string.IsNullOrWhiteSpace(unlock.FilePath) || !File.Exists(unlock.FilePath))
            {
                if (this.Visible) MessageBox.Show("EvoUnlockItemParam file not found.");
                return 6;
            }

            byte unlockDlcId = 0;
            int unlockVersionValue = 0;
            bool unlockBaseEntryFound = false;
            for (int x = 0; x < unlock.EntryList.Count; x++)
            {
                byte[] entry = unlock.EntryList[x];
                if (entry.Length >= 0x18 &&
                    entry[1] == 0x19 &&
                    Main.b_byteArrayToIntTwoBytes(new byte[] { entry[2], entry[3] }) == basePresetId)
                {
                    unlockDlcId = entry[0];
                    unlockVersionValue = BitConverter.ToInt32(entry, 4);
                    unlockBaseEntryFound = true;
                    break;
                }
            }

            unlock.EntryList.Add(BuildUnlockEntry(
                newPresetId,
                unlockBaseEntryFound ? unlockDlcId : (byte)0,
                unlockBaseEntryFound ? unlockVersionValue : 0,
                unlockNotification.Checked,
                resolvedDisplayName));
            unlock.EntryCount = unlock.EntryList.Count;

            // Open roster tool
            Tool_RosterEditor csp = new Tool_RosterEditor();
            csp.OpenFile(Main.cspPath);

            // Find roster ID
            int rosterId = -1;
            for (int x = 0; x < csp.EntryCount; x++)
            {
                string rosterCharacter = csp.CharacterList[x];
                if (string.Equals(rosterCharacter, character, StringComparison.Ordinal))
                {
                    rosterId = x;
                    break;
                }
            }
            if (rosterId == -1)
            {
                for (int x = 0; x < csp.EntryCount; x++)
                {
                    string rosterCharacter = csp.CharacterList[x];
                    if (rosterCharacter.Length >= character.Length && rosterCharacter.StartsWith(character, StringComparison.Ordinal))
                    {
                        rosterId = x;
                        break;
                    }
                }
            }
            if (rosterId == -1)
            {
                if (this.Visible) MessageBox.Show("Base character not found in characterSelectParam.");
                return 4;
            }

            // Copy roster entry
            csp.ListBox1.SelectedIndex = rosterId;
            csp.AddEntry();
            rosterId = csp.EntryCount - 1;

            // Set roster name as psp name
            csp.CharacterList[rosterId] = characterPspName;

            // Set as last costume
            int costumecount = 0;
            int thispage = csp.PageList[rosterId];
            int thispos = csp.PositionList[rosterId];
            for (int x = 0; x < csp.EntryCount; x++)
            {
                if (csp.PageList[x] == thispage && csp.PositionList[x] == thispos)
                {
                    int thiscos = csp.CostumeList[x];
                    if (thiscos > costumecount) costumecount = thiscos;
                }
                /*if (csp.CharacterList[x].Substring(0, character.Length) == character)
                {
                    int thiscost = csp.CostumeList[x];
                    if (thiscost > costumecount) costumecount = thiscost;
                }*/
            }
            costumecount = costumecount + 1;
            csp.CostumeList[rosterId] = costumecount;

            // Open characode tool
            Tool_CharacodeEditor cha = new Tool_CharacodeEditor();
            cha.OpenFile(Main.chaPath);
            int cha_count = cha.CharacterList.Count;
            for (int i = 0; i< cha_count;i++)
            {
                if (character == cha.CharacterList[i])
                {
                    iconID = i+1;
                    //MessageBox.Show(iconID.ToString("X2"));
                    break;
                }
            }
            if (iconID == 0)
            {
                if (this.Visible) MessageBox.Show("Base character not found in characode.");
                return 5;
            }
            // Open player_icon tool
            Tools.Tool_IconEditor icon = new Tools.Tool_IconEditor();
            icon.OpenFile(Main.iconPath);
            icon.AddID_costume(iconID, dpp_costId);

            // Save files
            dpp.SaveFile();
            psp.SaveFile();
            if (File.Exists(unlock.FilePath + ".backup"))
            {
                File.Delete(unlock.FilePath + ".backup");
            }
            File.Copy(unlock.FilePath, unlock.FilePath + ".backup");
            File.WriteAllBytes(unlock.FilePath, unlock.ConvertToFile());
            csp.SaveFile();
            icon.SaveFile();
            if (this.Visible) MessageBox.Show("Added costume of base character " + character + " with model " + model +
                " in roster page " + csp.PageList[rosterId].ToString("X2") + " and position " +
                csp.PositionList[rosterId].ToString("X2") + ", as the costume id " + csp.CostumeList[rosterId].ToString("X2"));

            return 0;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            AddCostume();
        }

        private void Tool_AddCostume_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void unlockNotification_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
