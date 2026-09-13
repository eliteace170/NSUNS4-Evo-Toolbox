using NSUNS4_Character_Manager.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
	public partial class Tool_DuelPlayerParamEditor : Form
	{
		private enum DuelCopySettingsMode
		{
			ItemsOnly,
			ConditionsOnly,
			AllSettings,
			ConditionsAndAllSettings,
			Everything
		}

		public class DuelPlayerParamEntry
		{
			public string BinPath = "";
			public string BinName = "";
			public byte[] Data = new byte[760];
			public string CharacterId = "";
			public string MotionCode = "";
			public string[] BaseCostumes = new string[20];
			public string[] AwakeCostumes = new string[20];
			public string DefaultAssist1 = "";
			public string DefaultAssist2 = "";
			public string AwakeAction = "";
			public string[] Items = new string[4];
			public short[] ItemCounts = new short[4];
			public string Partner = "";
			public byte[] SettingList = new byte[36];
			public byte[] AwaSettingList = new byte[84];
			public byte[] Setting2List = new byte[16];
			public long EvoDup = 0;
			public int AwaBodyPriority = 0;
			public int DefaultAwaSkillIndex = -1;
			public int ConditionFlag = 0;
			public int EnableAwaSkill = 0;
			public int CameraDistance = 0;
			public int CameraUnknown1 = 0;
			public int VictoryAngle = 0;
			public int CameraUnknown2 = 0;
			public int CameraUnknown3 = 0;
			public int CameraUnknown4 = 0;

			public DuelPlayerParamEntry Clone()
			{
				return new DuelPlayerParamEntry
				{
					BinPath = BinPath,
					BinName = BinName,
					Data = (byte[])Data.Clone(),
					CharacterId = CharacterId,
					MotionCode = MotionCode,
					BaseCostumes = (string[])BaseCostumes.Clone(),
					AwakeCostumes = (string[])AwakeCostumes.Clone(),
					DefaultAssist1 = DefaultAssist1,
					DefaultAssist2 = DefaultAssist2,
					AwakeAction = AwakeAction,
					Items = (string[])Items.Clone(),
					ItemCounts = (short[])ItemCounts.Clone(),
					Partner = Partner,
					SettingList = (byte[])SettingList.Clone(),
					AwaSettingList = (byte[])AwaSettingList.Clone(),
					Setting2List = (byte[])Setting2List.Clone(),
					EvoDup = EvoDup,
					AwaBodyPriority = AwaBodyPriority,
					DefaultAwaSkillIndex = DefaultAwaSkillIndex,
					ConditionFlag = ConditionFlag,
					EnableAwaSkill = EnableAwaSkill,
					CameraDistance = CameraDistance,
					CameraUnknown1 = CameraUnknown1,
					VictoryAngle = VictoryAngle,
					CameraUnknown2 = CameraUnknown2,
					CameraUnknown3 = CameraUnknown3,
					CameraUnknown4 = CameraUnknown4
				};
			}
		}

		public bool FileOpen = false;
		public string FilePath = "";
		public int EntryCount = 0;
		public List<DuelPlayerParamEntry> Entries = new List<DuelPlayerParamEntry>();
		public List<string> BinPath = new List<string>();
		public List<string> BinName = new List<string>();
		public List<byte[]> Data = new List<byte[]>();
		public List<string> CharaList = new List<string>();
		public List<string[]> CostumeList = new List<string[]>();
		public List<string[]> AwkCostumeList = new List<string[]>();
		public List<string> DefaultAssist1 = new List<string>();
		public List<string> DefaultAssist2 = new List<string>();
		public List<string> AwkAction = new List<string>();
		public List<string[]> ItemList = new List<string[]>();
		public List<short[]> ItemCount = new List<short[]>();
        public List<string> Partner = new List<string>();
		public List<byte[]> SettingList = new List<byte[]>();
		public List<byte[]> AwaSettingList = new List<byte[]>();
		public List<byte[]> Setting2List = new List<byte[]>();
		public List<long> EvoDupList = new List<long>();
		public List<int> AwaBodyPriorityList = new List<int>();
		public List<int> DefaultAwaSkillIndexList = new List<int>();
		public List<int> ConditionFlagList = new List<int>();
		public List<int> EnableAwaSkillList = new List<int>();
		public List<int> CameraDistanceList = new List<int>();
		public List<int> CameraUnknown1List = new List<int>();
		public List<int> VictoryAngleList = new List<int>();
		public List<int> CameraUnknown2List = new List<int>();
		public List<int> CameraUnknown3List = new List<int>();
		public List<int> CameraUnknown4List = new List<int>();
		public List<int> VictoryPosList => CameraUnknown1List;
		public List<int> VictoryUnknownList => CameraDistanceList;
		private bool syncingConditionControls = false;

		public Tool_DuelPlayerParamEditor()
		{
			InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
			InitializeConditionFlagList();
            InitializeFieldContext();
			ResetInlineSettings();
		}

		private static int ReadUInt16(byte[] data, int offset)
		{
			return BitConverter.ToInt16(data, offset); // S4 reads these values with MOVSX.
		}

		private static int SwapInt32Endian(int value)
		{
			uint raw = unchecked((uint)value);
			uint swapped = (raw >> 24)
				| ((raw >> 8) & 0x0000FF00u)
				| ((raw << 8) & 0x00FF0000u)
				| (raw << 24);
			return unchecked((int)swapped);
		}

		private static int GetConditionDisplayValue(int storedConditionFlag)
		{
			return SwapInt32Endian(storedConditionFlag);
		}

		private static int GetStoredConditionValue(int displayedConditionFlag)
		{
			return SwapInt32Endian(displayedConditionFlag);
		}

		private static string EncodeText(string value)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(value ?? ""));
		}

		private static string EncodeBytes(byte[] value)
		{
			return Convert.ToBase64String(value ?? new byte[0]);
		}

		private static string DecodeText(string value)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(value ?? ""));
		}

		private static byte[] DecodeBytes(string value)
		{
			return Convert.FromBase64String(value ?? "");
		}

		private static long CombineEvoDup(int lowPart, int highPart)
		{
			return ((long)(uint)highPart << 32) | (uint)lowPart;
		}

		private static int GetEvoDupLow(long evoDup)
		{
			return unchecked((int)(uint)(evoDup & 0xFFFFFFFFL));
		}

		private static int GetEvoDupHigh(long evoDup)
		{
			return unchecked((int)(uint)((evoDup >> 32) & 0xFFFFFFFFL));
		}

		private void SetNumericValue(NumericUpDown control, decimal value)
		{
			if (control == null)
			{
				return;
			}

			if (value < control.Minimum)
			{
				control.Value = control.Minimum;
			}
			else if (value > control.Maximum)
			{
				control.Value = control.Maximum;
			}
			else
			{
				control.Value = value;
			}
		}

		private string GetEntryPrefixFromBinName(string binName)
		{
			if (string.IsNullOrWhiteSpace(binName))
			{
				return "";
			}

			return binName.EndsWith("prm_bas", StringComparison.OrdinalIgnoreCase) && binName.Length > 7
				? binName.Substring(0, binName.Length - 7)
				: binName;
		}

		private void ClearEntryCollections()
		{
			EntryCount = 0;
			Entries.Clear();
			BinPath.Clear();
			BinName.Clear();
			Data.Clear();
			CharaList.Clear();
			CostumeList.Clear();
			AwkCostumeList.Clear();
			DefaultAssist1.Clear();
			DefaultAssist2.Clear();
			AwkAction.Clear();
			ItemList.Clear();
			ItemCount.Clear();
			Partner.Clear();
			SettingList.Clear();
			AwaSettingList.Clear();
			Setting2List.Clear();
			EvoDupList.Clear();
			AwaBodyPriorityList.Clear();
			DefaultAwaSkillIndexList.Clear();
			ConditionFlagList.Clear();
			EnableAwaSkillList.Clear();
			CameraDistanceList.Clear();
			CameraUnknown1List.Clear();
			VictoryAngleList.Clear();
			CameraUnknown2List.Clear();
			CameraUnknown3List.Clear();
			CameraUnknown4List.Clear();
			listBox1.ClearSelected();
			listBox1.Items.Clear();
		}

		private void RebuildEntriesFromLegacyLists()
		{
			Entries.Clear();
			for (int i = 0; i < EntryCount; i++)
			{
				Entries.Add(new DuelPlayerParamEntry
				{
					BinPath = BinPath[i],
					BinName = BinName[i],
					Data = (byte[])Data[i].Clone(),
					CharacterId = GetEntryPrefixFromBinName(BinName[i]),
					MotionCode = CharaList[i],
					BaseCostumes = (string[])CostumeList[i].Clone(),
					AwakeCostumes = (string[])AwkCostumeList[i].Clone(),
					DefaultAssist1 = DefaultAssist1[i],
					DefaultAssist2 = DefaultAssist2[i],
					AwakeAction = AwkAction[i],
					Items = (string[])ItemList[i].Clone(),
					ItemCounts = (short[])ItemCount[i].Clone(),
					Partner = Partner[i],
					SettingList = (byte[])SettingList[i].Clone(),
					AwaSettingList = (byte[])AwaSettingList[i].Clone(),
					Setting2List = (byte[])Setting2List[i].Clone(),
					EvoDup = EvoDupList[i],
					AwaBodyPriority = AwaBodyPriorityList[i],
					DefaultAwaSkillIndex = DefaultAwaSkillIndexList[i],
					ConditionFlag = ConditionFlagList[i],
					EnableAwaSkill = EnableAwaSkillList[i],
					CameraDistance = CameraDistanceList[i],
					CameraUnknown1 = CameraUnknown1List[i],
					VictoryAngle = VictoryAngleList[i],
					CameraUnknown2 = CameraUnknown2List[i],
					CameraUnknown3 = CameraUnknown3List[i],
					CameraUnknown4 = CameraUnknown4List[i]
				});
			}
		}

		private void RefreshLegacyListsFromEntries()
		{
			EntryCount = Entries.Count;
			BinPath = Entries.Select(e => e.BinPath).ToList();
			BinName = Entries.Select(e => e.BinName).ToList();
			Data = Entries.Select(e => (byte[])e.Data.Clone()).ToList();
			CharaList = Entries.Select(e => e.MotionCode).ToList();
			CostumeList = Entries.Select(e => (string[])e.BaseCostumes.Clone()).ToList();
			AwkCostumeList = Entries.Select(e => (string[])e.AwakeCostumes.Clone()).ToList();
			DefaultAssist1 = Entries.Select(e => e.DefaultAssist1).ToList();
			DefaultAssist2 = Entries.Select(e => e.DefaultAssist2).ToList();
			AwkAction = Entries.Select(e => e.AwakeAction).ToList();
			ItemList = Entries.Select(e => (string[])e.Items.Clone()).ToList();
			ItemCount = Entries.Select(e => (short[])e.ItemCounts.Clone()).ToList();
			Partner = Entries.Select(e => e.Partner).ToList();
			SettingList = Entries.Select(e => (byte[])e.SettingList.Clone()).ToList();
			AwaSettingList = Entries.Select(e => (byte[])e.AwaSettingList.Clone()).ToList();
			Setting2List = Entries.Select(e => (byte[])e.Setting2List.Clone()).ToList();
			EvoDupList = Entries.Select(e => e.EvoDup).ToList();
			AwaBodyPriorityList = Entries.Select(e => e.AwaBodyPriority).ToList();
			DefaultAwaSkillIndexList = Entries.Select(e => e.DefaultAwaSkillIndex).ToList();
			ConditionFlagList = Entries.Select(e => e.ConditionFlag).ToList();
			EnableAwaSkillList = Entries.Select(e => e.EnableAwaSkill).ToList();
			CameraDistanceList = Entries.Select(e => e.CameraDistance).ToList();
			CameraUnknown1List = Entries.Select(e => e.CameraUnknown1).ToList();
			VictoryAngleList = Entries.Select(e => e.VictoryAngle).ToList();
			CameraUnknown2List = Entries.Select(e => e.CameraUnknown2).ToList();
			CameraUnknown3List = Entries.Select(e => e.CameraUnknown3).ToList();
			CameraUnknown4List = Entries.Select(e => e.CameraUnknown4).ToList();
		}

		private void RefreshEntryListBox()
		{
			int selectedIndex = listBox1.SelectedIndex;
			listBox1.Items.Clear();
			foreach (DuelPlayerParamEntry entry in Entries)
			{
				listBox1.Items.Add(entry.BinName);
			}
			if (selectedIndex >= 0 && selectedIndex < listBox1.Items.Count)
			{
				listBox1.SelectedIndex = selectedIndex;
			}
		}

		private DuelPlayerParamEntry CreateDefaultEntry(string prefix)
		{
			if (string.IsNullOrWhiteSpace(prefix))
			{
				prefix = Entries.Count.ToString("X2") + "cd";
			}

			return new DuelPlayerParamEntry
			{
				BinPath = "Z:/param/player/Converter/bin/" + prefix + "prm_bas.bin",
				BinName = prefix + "prm_bas",
				CharacterId = prefix,
				MotionCode = prefix,
				Partner = "",
				DefaultAssist1 = "",
				DefaultAssist2 = "",
				AwakeAction = "",
				Items = new string[4],
				ItemCounts = new short[4],
				SettingList = new byte[36],
				Setting2List = new byte[16],
				AwaSettingList = new byte[84],
				EvoDup = 0,
				AwaBodyPriority = 0,
				DefaultAwaSkillIndex = -1,
				EnableAwaSkill = 0,
				CameraDistance = 45,
				CameraUnknown1 = 40,
				VictoryAngle = 150,
				CameraUnknown2 = 0,
				CameraUnknown3 = 0,
				CameraUnknown4 = 0
			};
		}

		private DuelPlayerParamEntry CreateEntryFromCurrentForm()
		{
			DuelPlayerParamEntry entry = listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < Entries.Count
				? Entries[listBox1.SelectedIndex].Clone()
				: CreateDefaultEntry(w_charaprmbas.Text);

			entry.BinPath = "Z:/param/player/Converter/bin/" + w_charaprmbas.Text + "prm_bas.bin";
			entry.BinName = w_charaprmbas.Text + "prm_bas";
			entry.CharacterId = w_charaprmbas.Text;
			entry.MotionCode = w_characodeid.Text;
            costumeGrid.EndEdit();
            for (int slot = 0; slot < 20; slot++)
            {
                entry.BaseCostumes[slot] = Convert.ToString(costumeGrid.Rows[slot].Cells[1].Value);
                entry.AwakeCostumes[slot] = Convert.ToString(costumeGrid.Rows[slot].Cells[2].Value);
            }
			entry.DefaultAssist1 = w_defaultassist1.Text;
			entry.DefaultAssist2 = w_defaultassist2.Text;
			entry.AwakeAction = w_awkaction.Text;
			entry.Items = new string[4] { w_item1.Text, w_item2.Text, w_item3.Text, w_item4.Text };
			entry.ItemCounts = new short[4] { (short)w_itemc1.Value, (short)w_itemc2.Value, (short)w_itemc3.Value, (short)w_itemc4.Value };
			entry.Partner = w_partner.Text;
			int displayedConditionFlag = unchecked((int)decimal.ToUInt32(v_enableAwaSkill.Value));
			int storedConditionFlag = GetStoredConditionValue(displayedConditionFlag);
			entry.ConditionFlag = storedConditionFlag;
			entry.EnableAwaSkill = storedConditionFlag & 0xFF;
			ApplyInlineSettingsToEntry(entry);
            if (loadedExtraSettings != null) DecodeExtraSettings(entry, loadedExtraSettings);
            ApplyAdditionalFields(entry);
			return entry;
		}

		private void ApplyEntryToForm(DuelPlayerParamEntry entry)
		{
			w_charaprmbas.Text = !string.IsNullOrWhiteSpace(entry.CharacterId) ? entry.CharacterId : GetEntryPrefixFromBinName(entry.BinName);
			w_characodeid.Text = entry.MotionCode;
            LoadCostumes(entry);
			w_defaultassist1.Text = entry.DefaultAssist1;
			w_defaultassist2.Text = entry.DefaultAssist2;
			w_awkaction.Text = entry.AwakeAction;
			w_item1.Text = entry.Items[0];
			SetNumericValue(w_itemc1, entry.ItemCounts[0]);
			w_item2.Text = entry.Items[1];
			SetNumericValue(w_itemc2, entry.ItemCounts[1]);
			w_item3.Text = entry.Items[2];
			SetNumericValue(w_itemc3, entry.ItemCounts[2]);
			w_item4.Text = entry.Items[3];
			SetNumericValue(w_itemc4, entry.ItemCounts[3]);
			w_partner.Text = entry.Partner;
			int displayedConditionFlag = GetConditionDisplayValue(entry.ConditionFlag);
			SetNumericValue(v_enableAwaSkill, (decimal)(uint)displayedConditionFlag);
			UpdateConditionFlagsFromValue(displayedConditionFlag);
			LoadInlineSettingsFromEntry(entry);
		}

        private static string ReadCostumeCode(byte[] data, int offset)
        {
            // A full eight-byte code has no terminator; never read into the next slot.
            int length = 0;
            while (length < 8 && data[offset + length] != 0) length++;
            return Encoding.ASCII.GetString(data, offset, length);
        }

        private void LoadCostumes(DuelPlayerParamEntry entry)
        {
            // Rows are record data; the grid and all columns are defined in Designer.cs.
            costumeGrid.Rows.Clear();
            for (int slot = 0; slot < 20; slot++)
                costumeGrid.Rows.Add(slot, entry.BaseCostumes[slot] ?? "", entry.AwakeCostumes[slot] ?? "");
        }

        // S4 record context. Keep legacy member names for clipboard/caller compatibility.
        private int hoveredConditionFlag = -1;
        private string loadedExtraSettings;
        private readonly Dictionary<NumericUpDown, decimal> loadedFloatValues = new Dictionary<NumericUpDown, decimal>();

        // Numeric values are file values; names describe verified branches, not original source enums.
        private enum AwakeRiskMode { Mode0 = 0, Reaction = 1, ReactionSkills = 2, Life = 3, LifeSkills = 4, Skills = 5 }
        private enum DefaultSupportClass { Unspecified = -1, Attack = 0, Defense = 1, Balance = 2 }
        private enum AwakeningPriority { Default = -1, Priority301 = 301, Priority390 = 390 }
        private enum AwakeningSkill { Default = -1, Slot0 = 0, Slot1 = 1, Slot2 = 2, Slot3 = 3, Slot4 = 4, Slot5 = 5 }
        private enum DashPriorityMode { Disabled = 0, Enabled = 1 }
        private enum JobType { Default = -1, Type0 = 0, Type1 = 1 }
        private enum AnimationStyle { Default = -1, Normal = 0, Female = 1 }
        private sealed class RiskChoice
        {
            public int Value;
            public string Label;
            public override string ToString() { return Value + " - " + Label; }
        }

        private static string EnumChoiceLabel(Type type, object choice)
        {
            int value = Convert.ToInt32(choice);
            if (type == typeof(DefaultSupportClass))
                return value == -1 ? "Unspecified" : value == 0 ? "Attack" : value == 1 ? "Defense (legacy)" : "Balance (legacy)";
            if (type == typeof(JobType))
                return value == -1 ? "Default (uses type 0)" : value == 0 ? "Type 0 (excludes command 039)" : "Type 1 (excludes command 018)";
            if (type == typeof(AnimationStyle))
                return value == -1 ? "Default (unchanged)" : value == 0 ? "Normal (unchanged)" : "Female animation conversion";
            if (type == typeof(AwakeningPriority))
                return value == -1 ? "Default (no override)" : "Priority " + value;
            if (type == typeof(AwakeningSkill))
                return value == -1 ? "Default" : "Skill slot " + value;
            if (type == typeof(DashPriorityMode))
                return value == 0 ? "Disabled (350)" : "Enabled (390)";
            if (type == typeof(AwakeRiskMode))
                return new[] { "Chakra recovery after awakening", "Exit reaction", "Exit reaction + action gauges", "Life recovery", "Life recovery + action gauges", "Action gauges" }[value];
            return choice.ToString();
        }

        private static void SelectEnumValue(ComboBox combo, Type type, int value)
        {
            combo.Items.Clear();
            combo.Tag = type;
            foreach (object choice in Enum.GetValues(type).Cast<object>().OrderBy(c => Convert.ToInt32(c)))
                combo.Items.Add(new RiskChoice { Value = Convert.ToInt32(choice), Label = EnumChoiceLabel(type, choice) });
            if (!combo.Items.Cast<RiskChoice>().Any(c => c.Value == value))
                combo.Items.Add(new RiskChoice { Value = value, Label = type == typeof(DashPriorityMode) && value != 0 ? "Enabled (nonzero; preserved)" : "Unlisted value (preserved)" });
            combo.SelectedItem = combo.Items.Cast<RiskChoice>().First(c => c.Value == value);
        }

        private static int ReadEnumValue(ComboBox combo)
        {
            return ((RiskChoice)combo.SelectedItem).Value;
        }

        private void SelectAwakeRisk(int value)
        {
            SelectEnumValue(awakeRisk, typeof(AwakeRiskMode), value);
        }

        private void InitializeFieldContext()
        {
            checkedListConditionFlags.MouseMove += (s, e) =>
            {
                int index = checkedListConditionFlags.IndexFromPoint(e.Location);
                if (index == hoveredConditionFlag) return;
                hoveredConditionFlag = index;
                fieldTips.Hide(checkedListConditionFlags);
                if (index >= 0 && index < ConditionFlagDetails.Length)
                    fieldTips.Show(WrapFieldHelp(ConditionFlagDetails[index]),
                        checkedListConditionFlags, e.X + 18, e.Y + 20, 30000);
            };
            checkedListConditionFlags.MouseLeave += (s, e) =>
            {
                hoveredConditionFlag = -1;
                fieldTips.Hide(checkedListConditionFlags);
            };
            checkedListConditionFlags.SelectedIndexChanged += (s, e) =>
            {
                // Keyboard users receive the same per-row context. Mouse hover does not toggle bits.
                int index = checkedListConditionFlags.SelectedIndex;
                if (index >= 0 && index < ConditionFlagDetails.Length)
                    checkedListConditionFlags.AccessibleDescription = ConditionFlagDetails[index];
            };
            SelectAwakeRisk(1);
            InitializeAdditionalFields();
            LoadCostumes(CreateDefaultEntry("1new"));
            w_item1.MaxLength = 29;
            w_item2.MaxLength = 29;
            w_item3.MaxLength = 29;
            w_item4.MaxLength = 29;
        }

        // Bind the Designer-owned tab controls to record fields.
        // All scalar fields are Designer-owned controls, displayed in file order.
        private readonly Dictionary<int, Control> additionalFields = new Dictionary<int, Control>();

        // Bind Designer-owned controls to file fields; layout belongs in Designer.cs.
        private void InitializeAdditionalFields()
        {
            additionalFields.Add(0x15C, extraUnknown15C);
            additionalFields.Add(0x168, extraJobType);
            additionalFields.Add(0x16C, extraUnknown16C);
            additionalFields.Add(0x170, extraUnknown170);
            additionalFields.Add(0x174, extraUnknown174);
            additionalFields.Add(0x178, extraUnknown178);
            additionalFields.Add(0x17C, extraFemaleAnims);
            additionalFields.Add(0x180, extraUnknown180);
            additionalFields.Add(0x184, extraUnknown184);
            additionalFields.Add(0x188, extraUnknown188);
            additionalFields.Add(0x18C, extraUnknown18C);
            additionalFields.Add(0x190, extraUnknown190);
            additionalFields.Add(0x194, extraUnknown194);
            additionalFields.Add(0x198, extraUnknown198);
            additionalFields.Add(0x19C, extraUnknown19C);
            additionalFields.Add(0x1A0, extraAdventureSupport);
            additionalFields.Add(0x1CC, extraUnknown1CC);
            additionalFields.Add(0x1FE, extraDashTrackTime);
            additionalFields.Add(0x200, extraDashTurnRate);
            additionalFields.Add(0x292, extraAwakeDashTrackTime);
            additionalFields.Add(0x294, extraAwakeDashTurnRate);
            additionalFields.Add(0x298, extraUnknown298);
            additionalFields.Add(0x29C, extraUnknown29C);
            additionalFields.Add(0x2A0, extraUnknown2A0);
            additionalFields.Add(0x2A4, extraUnknown2A4);
            additionalFields.Add(0x2A8, extraUnknown2A8);
            additionalFields.Add(0x2AC, extraUnknown2AC);
            additionalFields.Add(0x2B0, extraUnknown2B0);
            additionalFields.Add(0x2B4, extraUnknown2B4);
            additionalFields.Add(0x2B8, extraUnknown2B8);
            additionalFields.Add(0x2BC, extraUnknown2BC);
            additionalFields.Add(0x2CC, extraUnknown2CC);
            extraJobType.Tag = typeof(JobType);
            extraFemaleAnims.Tag = typeof(AnimationStyle);
            extraAdventureSupport.Tag = typeof(DefaultSupportClass);
        }

        private void LoadAdditionalFields(DuelPlayerParamEntry entry)
        {
            foreach (var item in additionalFields)
            {
                ExtraField field = ExtraFields.First(f => f.Offset == item.Key);
                string raw = ReadExtraField(entry, field);
                var combo = item.Value as ComboBox;
                if (combo != null)
                {
                    SelectEnumValue(combo, (Type)combo.Tag, int.Parse(raw, CultureInfo.InvariantCulture));
                }
                else
                {
                    var number = (NumericUpDown)item.Value;
                    decimal value;
                    if (!decimal.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) value = 0;
                    SetNumericValue(number, value);
                    // Preserve the original float bits, including values outside decimal range.
                    number.Tag = number.Value;
                }
            }
        }

        private void ApplyAdditionalFields(DuelPlayerParamEntry entry)
        {
            foreach (var item in additionalFields)
            {
                ExtraField field = ExtraFields.First(f => f.Offset == item.Key);
                var combo = item.Value as ComboBox;
                if (combo != null)
                {
                    var choice = combo.SelectedItem as RiskChoice;
                    if (choice != null) WriteExtraField(entry, field, choice.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    var number = (NumericUpDown)item.Value;
                    if (!(number.Tag is decimal) || number.Value != (decimal)number.Tag)
                        WriteExtraField(entry, field, number.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        private static string WrapFieldHelp(string value)
        {
            var output = new StringBuilder();
            foreach (string line in value.Replace("\r", "").Split('\n'))
            {
                int width = 0;
                foreach (string word in line.Split(' '))
                {
                    if (width != 0 && width + word.Length + 1 > 88) { output.AppendLine(); width = 0; }
                    if (width != 0) { output.Append(' '); width++; }
                    output.Append(word); width += word.Length;
                }
                output.AppendLine();
            }
            return output.ToString().TrimEnd();
        }

        private void LoadFloatSetting(NumericUpDown control, byte[] data, int offset)
        {
            float value = BitConverter.ToSingle(data, offset);
            decimal display = 0;
            if (!float.IsNaN(value) && !float.IsInfinity(value))
            {
                try { display = (decimal)value; }
                catch (OverflowException) { display = value < 0 ? control.Minimum : control.Maximum; }
            }
            SetNumericValue(control, display);
            loadedFloatValues[control] = control.Value;
        }

        private void SaveFloatSetting(NumericUpDown control, byte[] data, int offset)
        {
            decimal loaded;
            if (!loadedFloatValues.TryGetValue(control, out loaded) || control.Value != loaded)
                Buffer.BlockCopy(BitConverter.GetBytes((float)control.Value), 0, data, offset, 4);
        }

        private static string EncodeItemCounts(short[] counts)
        {
            var bytes = new byte[8];
            for (int i = 0; i < 4; i++) Buffer.BlockCopy(BitConverter.GetBytes(counts[i]), 0, bytes, i * 2, 2);
            return EncodeBytes(bytes);
        }

        private static short[] DecodeItemCounts(string text)
        {
            byte[] bytes = DecodeBytes(text);
            if (bytes.Length != 4 && bytes.Length != 8) throw new FormatException("Invalid item-count clipboard data.");
            var counts = new short[4];
            for (int i = 0; i < 4; i++) counts[i] = bytes.Length == 4 ? (short)bytes[i] : BitConverter.ToInt16(bytes, i * 2);
            return counts;
        }

        private enum FieldKind { Short, Int, Float }
        private sealed class ExtraField
        {
            public readonly int Offset;
            public readonly FieldKind Kind;
            public readonly string Name, Description;
            public ExtraField(int offset, FieldKind kind, string name, string description)
            { Offset = offset; Kind = kind; Name = name; Description = description; }
        }

        private static readonly ExtraField[] ExtraFields = new ExtraField[]
        {
            new ExtraField(0x15C, FieldKind.Int, "Unknown15C", "All supplied records contain zero. No confirmed stock-game consumer; preserve these four bytes. It is addressable arithmetically as flag word 3 for indexes 96..127, but no such caller was recovered. Direct +0x15C hits in player movement refer to player-object state, not automatically to the parameter record."),
            new ExtraField(0x168, FieldKind.Int, "JobType", "Indexed parameter 2. Negative values normalize to 0. Pause-menu filtering: type 0 excludes sys_free_command_039; type 1 excludes sys_free_command_018. Broader meaning of the category is unresolved. Fresh direct/wrapper audits still find a command-list role rather than a combat-state switch. Only 3mfn has value 1 in the supplied asset. This correlation alone does not prove a general Samurai class or enable samurai abilities."),
            new ExtraField(0x16C, FieldKind.Int, "Unknown16C", "Indexed integer slot 3. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x170, FieldKind.Int, "Unknown170", "Indexed integer slot 4. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x174, FieldKind.Int, "Unknown174", "Indexed integer slot 5. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x178, FieldKind.Int, "Unknown178", "Indexed integer slot 6. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x17C, FieldKind.Int, "FemaleAnims", "Exactly 1 invokes ConvertWomanAnm for selected damage and support-rescue animation IDs; other values leave those IDs unchanged."),
            new ExtraField(0x180, FieldKind.Int, "Unknown180", "Indexed integer slot 8. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x184, FieldKind.Int, "Unknown184", "Indexed integer slot 9. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x188, FieldKind.Int, "Unknown188", "Indexed integer slot 10. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x18C, FieldKind.Int, "Unknown18C", "Indexed integer slot 11. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x190, FieldKind.Int, "Unknown190", "Indexed integer slot 12. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x194, FieldKind.Int, "Unknown194", "Indexed integer slot 13. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x198, FieldKind.Int, "Unknown198", "Indexed integer slot 14. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x19C, FieldKind.Int, "Unknown19C", "Indexed integer slot 15. All supplied records contain -1. A fresh audit of both the global accessor and the player/pair-awakening wrapper finds constant consumers only for indexes 0, 1, 2 and 7; no semantic reader for this slot was established. Preserve the slot; lack of a traced consumer is not proof of unused storage."),
            new ExtraField(0x1A0, FieldKind.Int, "AdventureSupport", "Default support class copied during adventure battle setup into +0x0C of a 0x3C-byte team-member record. S4 IsAttackTypeSupport tests that member class for 0. S3 establishes the legacy class names: 0 Attack, 1 Defense, 2 Balance. S4 effects for classes 1/2 are not established by this trace; this field does not select its separate Strike Back/Dash Cut/etc. support actions."),
            new ExtraField(0x1CC, FieldKind.Float, "LegacyGuardRate", "Legacy guard-rate slot: S3 GetGrdPer reads its corresponding +0x12C float, and Connections has a +0x1CC float getter. Neither getter has a recovered code caller in these databases; no S4 semantic reader was found. Final guard behavior is unverified. The former AirDashSpeed name is unsupported. A fresh pointer-reference check also recovered no hidden absolute function pointer to either legacy getter; this does not rule out indirect/generated access."),
            new ExtraField(0x1FE, FieldKind.Short, "DashTrackTime", "Chakra-dash target-tracking duration, move parameter 4. During continued dash motion, the game rotates toward the target only while ActCnt / (nuccAnmTimePerSec / FPS) is less than this value. This limits homing, not total dash duration (move parameter 3). The loader converts file frames by currentFPS/30. Once the action counter is nonnegative, zero or negative values skip that steering window."),
            new ExtraField(0x200, FieldKind.Float, "DashTurnRate", "Chakra-dash turn-strength coefficient, move parameter 5. The dash passes field * 72 degrees to CalcDircInterp; team-secret-technique preparation passes field * 90 degrees. The helper multiplies by actor GetSpeedRate, applies 1/4, 1/3 or 1/2 damping near the target, quantizes to 1/65536 of a turn, then converts to radians. Nearly aligned directions snap to the target. This is not a literal degrees-per-second field."),
            new ExtraField(0x292, FieldKind.Short, "AwakeDashTrackTime", "True-awakening counterpart: selected when awakening is active and AwakeType != 1. Chakra-dash target-tracking duration, move parameter 4. During continued dash motion, the game rotates toward the target only while ActCnt / (nuccAnmTimePerSec / FPS) is less than this value. This limits homing, not total dash duration (move parameter 3). The loader converts file frames by currentFPS/30. Once the action counter is nonnegative, zero or negative values skip that steering window."),
            new ExtraField(0x294, FieldKind.Float, "AwakeDashTurnRate", "True-awakening counterpart: selected when awakening is active and AwakeType != 1. Chakra-dash turn-strength coefficient, move parameter 5. The dash passes field * 72 degrees to CalcDircInterp; team-secret-technique preparation passes field * 90 degrees. The helper multiplies by actor GetSpeedRate, applies 1/4, 1/3 or 1/2 damping near the target, quantizes to 1/65536 of a turn, then converts to radians. Nearly aligned directions snap to the target. This is not a literal degrees-per-second field."),
            new ExtraField(0x298, FieldKind.Float, "Unknown298", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x29C, FieldKind.Float, "Unknown29C", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2A0, FieldKind.Float, "Unknown2A0", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2A4, FieldKind.Float, "Unknown2A4", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2A8, FieldKind.Float, "Unknown2A8", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2AC, FieldKind.Float, "Unknown2AC", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2B0, FieldKind.Float, "Unknown2B0", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2B4, FieldKind.Float, "Unknown2B4", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2B8, FieldKind.Float, "Unknown2B8", "Float interpretation retained from existing type annotation and plausible data. All records share the reported value. No verified gameplay consumer; do not infer a meaning from the value alone. The similarly positioned S3 region is an array of signed skill-priority shorts (+0x370 onward), not matching floats. S4 obtains those priorities from skillCustomizeParam/spSkillCustomizeParam, so the S3 names cannot be transferred to this slot."),
            new ExtraField(0x2BC, FieldKind.Float, "Unknown2BC", "Unresolved four-byte slot. Existing S4 float interpretation is provisional. Connections has an uncalled integer getter at this offset; zero data cannot settle its type. Preserve raw bits; the template also exposes an integer/hex view. S3 has two distinct integer awakening-start fields (+0x39C flag and +0x3A0 action). Neighboring placement alone cannot establish which, if either, corresponds to S4 +0x2BC."),
            new ExtraField(0x2CC, FieldKind.Float, "Unknown2CC", "Float values approximately 0.1 (263 records) or 0.03 (6 records). No verified consumer; not the confirmed maximum-chakra recovery field, which is +0x2D0. A fresh Connections offset scan likewise did not establish a parameter-record reader. Do not alias it to maximum-chakra recovery or special-support gauge recovery merely because their values are similar."),
        };

        private static byte[] FieldBuffer(DuelPlayerParamEntry entry, int offset, out int index)
        {
            index = offset;
            if (offset >= 0x284 && offset < 0x2D8) { index -= 0x284; return entry.AwaSettingList; }
            if (offset >= 0x1F4 && offset < 0x204) { index -= 0x1F4; return entry.Setting2List; }
            if (offset >= 0x1C0 && offset < 0x1E4) { index -= 0x1C0; return entry.SettingList; }
            return entry.Data;
        }

        private static string EncodeExtraSettings(DuelPlayerParamEntry entry)
        {
            var bytes = new byte[ExtraFields.Length * 8];
            for (int i = 0; i < ExtraFields.Length; i++)
            {
                ExtraField field = ExtraFields[i];
                ushort size = (ushort)(field.Kind == FieldKind.Short ? 2 : 4);
                Buffer.BlockCopy(BitConverter.GetBytes((ushort)field.Offset), 0, bytes, i * 8, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(size), 0, bytes, i * 8 + 2, 2);
                int index;
                byte[] source = FieldBuffer(entry, field.Offset, out index);
                Buffer.BlockCopy(source, index, bytes, i * 8 + 4, size);
            }
            return EncodeBytes(bytes);
        }

        private static void DecodeExtraSettings(DuelPlayerParamEntry entry, string text)
        {
            byte[] bytes = DecodeBytes(text);
            if (bytes.Length % 8 != 0) throw new FormatException("Invalid extra-field clipboard data.");
            for (int i = 0; i < bytes.Length; i += 8)
            {
                int offset = BitConverter.ToUInt16(bytes, i);
                int size = BitConverter.ToUInt16(bytes, i + 2);
                ExtraField field = ExtraFields.FirstOrDefault(f => f.Offset == offset);
                if (field == null || size != (field.Kind == FieldKind.Short ? 2 : 4))
                    throw new FormatException("Unknown extra field in clipboard data.");
                int index;
                byte[] target = FieldBuffer(entry, offset, out index);
                Buffer.BlockCopy(bytes, i + 4, target, index, size);
            }
        }

        private static string ReadExtraField(DuelPlayerParamEntry entry, ExtraField field)
        {
            int index;
            byte[] bytes = FieldBuffer(entry, field.Offset, out index);
            if (field.Kind == FieldKind.Float) return BitConverter.ToSingle(bytes, index).ToString("R", CultureInfo.InvariantCulture);
            if (field.Kind == FieldKind.Short) return BitConverter.ToInt16(bytes, index).ToString(CultureInfo.InvariantCulture);
            return BitConverter.ToInt32(bytes, index).ToString(CultureInfo.InvariantCulture);
        }

        private static void WriteExtraField(DuelPlayerParamEntry entry, ExtraField field, string value)
        {
            byte[] replacement;
            if (field.Kind == FieldKind.Float)
            {
                float number = float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
                if (float.IsNaN(number) || float.IsInfinity(number)) throw new FormatException("Enter a finite float.");
                replacement = BitConverter.GetBytes(number);
            }
            else if (field.Kind == FieldKind.Short) replacement = BitConverter.GetBytes(short.Parse(value, CultureInfo.InvariantCulture));
            else replacement = BitConverter.GetBytes(int.Parse(value, CultureInfo.InvariantCulture));
            int index;
            byte[] bytes = FieldBuffer(entry, field.Offset, out index);
            Buffer.BlockCopy(replacement, 0, bytes, index, replacement.Length);
        }

        // Verified against NSUNS4_107, NSUNS3HD_NX (ns3), and NSUNSC.exe.
        // Offsets are relative to MotionCode; add four for the binary size word.
        // A counterpart documents another build, never an automatic S4 semantic claim.
        // Runtime flags are LE at +0x150 from MotionCode. Checkbox index == game bit.
        // ConditionFlag retains its legacy swapped storage; do not remove the endian
        // conversions or change EnableAwaSkill/EvoDup serialization when editing labels.
        [Flags]
        private enum PlayerFlags : uint
        {
            None = 0,
            ThrowFall = 0x00000001u,
            KeepDouble = 0x00000002u,
            RenderToggle = 0x00000004u,
            Puppet = 0x00000008u,
            PuppetOwner = 0x00000010u,
            AnimAttachment = 0x00000020u,
            FinishRunStop = 0x00000040u,
            FinishAwakeRunStop = 0x00000080u,
            DefaultSubstitution = 0x00000100u,
            Unknown09 = 0x00000200u,
            BaseGlare = 0x00000400u,
            AwakeGlare = 0x00000800u,
            WarpDash = 0x00001000u,
            DodgeDraw = 0x00002000u,
            Unknown14 = 0x00004000u,
            AwakeEvents = 0x00008000u,
            HugeAwake = 0x00010000u,
            HugeCamera = 0x00020000u,
            HugeFootSound = 0x00040000u,
            AwakeHitmark = 0x00080000u,
            NoDamageTurn = 0x00100000u,
            DirectionDamage = 0x00200000u,
            Unknown22 = 0x00400000u,
            EndAwakeOnDefeat = 0x00800000u,
            AwakeSkill = 0x01000000u,
            Unknown25 = 0x02000000u,
            Unknown26 = 0x04000000u,
            Unknown27 = 0x08000000u,
            Unknown28 = 0x10000000u,
            Unknown29 = 0x20000000u,
            Unknown30 = 0x40000000u,
            Unknown31 = 0x80000000u,
        }

        private string[] GetConditionFlagNames()
        {
            return new string[]
            {
                "Force Air State For Throw Success",
                "Keep double state after throw",
                "Render toggle (partial)",
                "Puppet",
                "Puppet controller",
                "Attachment by animation source",
                "Finish run-stop animation",
                "Finish awakened run-stop",
                "Default substitution selection",
                "Deprecated",
                "Keep base glare",
                "Keep awakened glare",
                "Awakened warp dash",
                "Keep dodge drawing (predicate)",
                "Deprecated",
                "Separate awakening events",
                "Awakening super armor / huge classification",
                "Huge awakening camera",
                "Huge awakening foot sound",
                "Awakened hitmark decoration",
                "No awakened damage turn",
                "Awakened directional-damage exception",
                "Deprecated",
                "End awakening on defeat",
                "Awakening skill",
                "Unused 25",
                "Unused 26",
                "Unused 27",
                "Unused 28",
                "Unused 29",
                "Unused 30",
                "Unused 31",
            };
        }

        // Behavior traced from S4 callers; partial/unresolved meanings are explicit.
        private static readonly string[] ConditionFlagDetails = new string[]
        {
            "For THROW_SUCCESS_ATTACKER, resets velocity/gravity and movement state during cleanup; on animation end sets zero gravity and requests FALL rather than NUT. Seen in 2nrt/2nrv. Specific throw handling, not a general air-throw enable.",
            "Skips ResetDoubleState when ending the direction-attack/throw action. Seen in 2ten/8ten. Exact visual role of double state requires further tracing.",
            "Clears a subdraw/clump byte during creation, costume break, awakening model changes and several character-specific paths. The cleared field is not named in this database; do not label it as a specific visual feature yet.",
            "Returned by IsPuppetCharacter.",
            "IsPuppetController requires this flag and not currently awakened. IsPuppetOwner checks the flag without that awakening restriction.",
            "During animation changes, tests whether the animation chunk name contains the motion characode. Disables attachment slot 1 if it does, enables slot 1 otherwise.",
            "Suppresses the early transition to neutral when forward speed falls below 1; run-stop still exits on animation end. This bit applies in normal and awakened states.",
            "Same suppression as bit 6, but only while awakened. Animation end still permits the neutral transition.",
            "Forces a selected dodge/substitution value to 0 during player creation and dodge-skill selection checks. Observed on Rock Lee/Mifune-related motion codes. This does not disable substitution itself.",
            "Effect is not yet known.",
            "Prevents this helper from forcing the model glare coefficient to zero while not awakened or while using instant awakening. It preserves glare; it does not set a glow intensity.",
            "Prevents the same zeroing while truly awakened. Player state UNKNOWN_17A8 == 2 bypasses the helper independently.",
            "IsEnableChakraDashWarp requires both awakening and this bit. Also affects post-dodge positioning while in the warp chakra-dash action.",
            "IsDisableDrawAtDodge returns false only when awakened AND this bit is set; otherwise true. The enum name suggests the opposite polarity, so preserve this exact predicate. Likely prevents the usual dodge hide while awakened; final rendering should be tested in-game.",
            "Effect is not yet known.",
            "For motion modes 1 or 9, skips normal prm_mot/prm_skl/prm_spl event setup. The prm_awa setup is separately executed for those modes. This is more precise than ENABLE AWAKENING MOVESET; it does not independently create a moveset.",
            "Used by IsAwakeWithSuperArmorCharacter and IsHugeAwakeCharacter when player AwakeType is not 1. IsHugeAwakeNow additionally requires the awakening condition group. It is a classification flag, not a model-size setting.",
            "IsHugeAwakeNowForCamera requires this bit, an active awakening condition group and AwakeType != 1. This is not a generic enable-private-camera switch.",
            "When awakened and queried for sound type 6, selects column 7 of the surface sound table instead of column 2. It changes a particular foot/landing sound lookup, not every movement sound.",
            "Attacker must be awakened; the flag gates the skill-slot hitmark/damage-decoration path. It does not enable every hit effect universally.",
            "While awakened, blocks the normal damage-facing turn even if IsEnableTurnDamageNormal permits it. Applies within additional damage-attribute checks.",
            "IsAwakeChangeNotDirectionDamage returns this bit while truly awakened. Wall-battle states independently force that predicate true; instant awakening returns false. Also used in character-specific damage action selection.",
            "Effect is not yet known.",
            "On defeat, clears awakening conditions, ends awakening and its gauge, and triggers transition effects. DMG_HELL_ACTION is excluded. Battle-end code also uses it for a color-conversion effect.",
            "IsPlayerFlagParamAwakeSkill uses this bit; Orochimaru (2orc) with current skill number 1 is an explicit exception. Also used in current-skill attack-priority selection. The awakening skill number comes from a separate parameter at +0x164.",
            "Effect is not yet known.",
            "Effect is not yet known.",
            "Effect is not yet known.",
            "Effect is not yet known.",
            "Effect is not yet known.",
            "Effect is not yet known.",
            "Effect is not yet known.",
        };




		private void InitializeConditionFlagList()
		{
            // Keep the existing checkbox order, raw-value conversion and custom setEvo1 field.
			checkedListConditionFlags.Items.Clear();
			foreach (string flagName in GetConditionFlagNames())
			{
				checkedListConditionFlags.Items.Add(flagName);
			}
		}

		private void LoadInlineSettingsFromEntry(DuelPlayerParamEntry entry)
		{
			byte[] setting1 = entry.SettingList ?? new byte[36];
			byte[] setting2 = entry.Setting2List ?? new byte[16];
			byte[] awake = entry.AwaSettingList ?? new byte[84];
            loadedExtraSettings = EncodeExtraSettings(entry);
            LoadAdditionalFields(entry);

			LoadFloatSetting(setBaseMovement, setting1, 0);
			LoadFloatSetting(setBaseChakraDash, setting1, 4);
			LoadFloatSetting(setGuardPressure, setting1, 8);
			LoadFloatSetting(setAttack, setting1, 16);
			LoadFloatSetting(setDefense, setting1, 20);
			LoadFloatSetting(setAssistDamage, setting1, 24);
			LoadFloatSetting(setItemBuffDuration, setting1, 28);
			LoadFloatSetting(setChakraCharge, setting1, 32);

			LoadFloatSetting(setAwakeHpRequirement, setting2, 0);
			SetNumericValue(setBaseNinjaDash, BitConverter.ToInt16(setting2, 4));
			SetNumericValue(setBaseAirDashDuration, BitConverter.ToInt16(setting2, 6));
			SetNumericValue(setBaseGroundedChakraDashDuration, BitConverter.ToInt16(setting2, 8));

			LoadFloatSetting(setAwakeMovement, awake, 0);
			LoadFloatSetting(setAwakeChakraDash, awake, 4);
			SetNumericValue(setAwakeNinjaDash, BitConverter.ToInt16(awake, 8));
			SetNumericValue(setAwakeAirDashDuration, BitConverter.ToInt16(awake, 10));
			SetNumericValue(setAwakeGroundedChakraDashDuration, BitConverter.ToInt16(awake, 12));
			SelectEnumValue(setEnableDashPriority, typeof(DashPriorityMode), BitConverter.ToInt32(awake, 60));
			SelectAwakeRisk(BitConverter.ToInt32(awake, 64));
			LoadFloatSetting(setChakraCostAwakening, awake, 68);
			LoadFloatSetting(setChakraBlockRecovery, awake, 76);
			LoadFloatSetting(setAwakeningActionCharge, awake, 80);

			SetNumericValue(setEvo1, entry.EvoDup);
			SelectEnumValue(setAwaBodyPriority, typeof(AwakeningPriority), entry.AwaBodyPriority);
			SelectEnumValue(setDefaultAwaSkillIndex, typeof(AwakeningSkill), entry.DefaultAwaSkillIndex);
			SetNumericValue(setCameraDistance, entry.CameraDistance);
			SetNumericValue(setCameraUnknown1, entry.CameraUnknown1);
			SetNumericValue(setVictoryCameraAngle, entry.VictoryAngle);
			SetNumericValue(setCameraUnknown2, entry.CameraUnknown2);
			SetNumericValue(setCameraUnknown3, entry.CameraUnknown3);
			SetNumericValue(setCameraUnknown4, entry.CameraUnknown4);
		}

		private void ApplyInlineSettingsToEntry(DuelPlayerParamEntry entry)
		{
			byte[] setting1 = entry.SettingList != null && entry.SettingList.Length == 36 ? (byte[])entry.SettingList.Clone() : new byte[36];
			byte[] setting2 = entry.Setting2List != null && entry.Setting2List.Length == 16 ? (byte[])entry.Setting2List.Clone() : new byte[16];
			byte[] awake = entry.AwaSettingList != null && entry.AwaSettingList.Length == 84 ? (byte[])entry.AwaSettingList.Clone() : new byte[84];

			SaveFloatSetting(setBaseMovement, setting1, 0);
			SaveFloatSetting(setBaseChakraDash, setting1, 4);
			SaveFloatSetting(setGuardPressure, setting1, 8);
			SaveFloatSetting(setAttack, setting1, 16);
			SaveFloatSetting(setDefense, setting1, 20);
			SaveFloatSetting(setAssistDamage, setting1, 24);
			SaveFloatSetting(setItemBuffDuration, setting1, 28);
			SaveFloatSetting(setChakraCharge, setting1, 32);

			SaveFloatSetting(setAwakeHpRequirement, setting2, 0);
			Buffer.BlockCopy(BitConverter.GetBytes((short)setBaseNinjaDash.Value), 0, setting2, 4, 2);
			Buffer.BlockCopy(BitConverter.GetBytes((short)setBaseAirDashDuration.Value), 0, setting2, 6, 2);
			Buffer.BlockCopy(BitConverter.GetBytes((short)setBaseGroundedChakraDashDuration.Value), 0, setting2, 8, 2);

			SaveFloatSetting(setAwakeMovement, awake, 0);
			SaveFloatSetting(setAwakeChakraDash, awake, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((short)setAwakeNinjaDash.Value), 0, awake, 8, 2);
			Buffer.BlockCopy(BitConverter.GetBytes((short)setAwakeAirDashDuration.Value), 0, awake, 10, 2);
			Buffer.BlockCopy(BitConverter.GetBytes((short)setAwakeGroundedChakraDashDuration.Value), 0, awake, 12, 2);
			// Write the selected raw enum value, preserving noncanonical nonzero values.
            Buffer.BlockCopy(BitConverter.GetBytes(ReadEnumValue(setEnableDashPriority)), 0, awake, 60, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(((RiskChoice)awakeRisk.SelectedItem).Value), 0, awake, 64, 4);
			SaveFloatSetting(setChakraCostAwakening, awake, 68);
			SaveFloatSetting(setChakraBlockRecovery, awake, 76);
			SaveFloatSetting(setAwakeningActionCharge, awake, 80);

			entry.SettingList = setting1;
			entry.Setting2List = setting2;
			entry.AwaSettingList = awake;
			entry.EvoDup = (long)setEvo1.Value;
			entry.AwaBodyPriority = ReadEnumValue(setAwaBodyPriority);
			entry.DefaultAwaSkillIndex = ReadEnumValue(setDefaultAwaSkillIndex);
			entry.CameraDistance = (int)setCameraDistance.Value;
			entry.CameraUnknown1 = (int)setCameraUnknown1.Value;
			entry.VictoryAngle = (int)setVictoryCameraAngle.Value;
			entry.CameraUnknown2 = (int)setCameraUnknown2.Value;
			entry.CameraUnknown3 = (int)setCameraUnknown3.Value;
			entry.CameraUnknown4 = (int)setCameraUnknown4.Value;
		}

		private void ResetInlineSettings()
		{
			LoadInlineSettingsFromEntry(CreateDefaultEntry("1new"));
            LoadCostumes(CreateDefaultEntry("1new"));
		}

		private void SortEntries()
		{
			if (Entries.Count == 0)
			{
				return;
			}

			DuelPlayerParamEntry selectedEntry = listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < Entries.Count
				? Entries[listBox1.SelectedIndex]
				: null;

			Entries = Entries
				.OrderBy(entry => entry.BinName, StringComparer.OrdinalIgnoreCase)
				.ThenBy(entry => entry.MotionCode, StringComparer.OrdinalIgnoreCase)
				.ToList();

			RefreshLegacyListsFromEntries();
			RefreshEntryListBox();

			if (selectedEntry != null)
			{
				int newIndex = Entries.IndexOf(selectedEntry);
				if (newIndex >= 0)
				{
					listBox1.SelectedIndex = newIndex;
				}
			}
		}

        private DuelCopySettingsMode? ShowCopySettingsDialog()
        {
            using (var dialog = new Tool_DuelPlayerParamEditor_CopySettings())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedModeIndex < 0)
                    return null;
                return (DuelCopySettingsMode)dialog.SelectedModeIndex;
            }
        }


		private string BuildCopySettingsPayload(DuelPlayerParamEntry entry, DuelCopySettingsMode mode)
		{
			List<string> lines = new List<string>
			{
				"NSUNS4_EVO_DUEL_SETTINGS",
				"mode=" + mode.ToString()
			};

			if (mode == DuelCopySettingsMode.ItemsOnly || mode == DuelCopySettingsMode.Everything)
			{
				lines.Add("item0=" + EncodeText(entry.Items[0]));
				lines.Add("item1=" + EncodeText(entry.Items[1]));
				lines.Add("item2=" + EncodeText(entry.Items[2]));
				lines.Add("item3=" + EncodeText(entry.Items[3]));
				lines.Add("itemCounts=" + EncodeItemCounts(entry.ItemCounts));
			}

			if (mode == DuelCopySettingsMode.ConditionsOnly ||
				mode == DuelCopySettingsMode.ConditionsAndAllSettings ||
				mode == DuelCopySettingsMode.Everything)
			{
				lines.Add("conditionFlag=" + unchecked((uint)entry.ConditionFlag).ToString());
				lines.Add("enableAwaSkill=" + entry.EnableAwaSkill.ToString());
			}

			if (mode == DuelCopySettingsMode.AllSettings ||
				mode == DuelCopySettingsMode.ConditionsAndAllSettings ||
				mode == DuelCopySettingsMode.Everything)
			{
				lines.Add("settingList=" + EncodeBytes(entry.SettingList));
				lines.Add("setting2List=" + EncodeBytes(entry.Setting2List));
				lines.Add("awaSettingList=" + EncodeBytes(entry.AwaSettingList));
                lines.Add("extraSettings=" + EncodeExtraSettings(entry));
				lines.Add("evoDup=" + entry.EvoDup.ToString());
				lines.Add("awaBodyPriority=" + entry.AwaBodyPriority.ToString());
				lines.Add("defaultAwaSkillIndex=" + entry.DefaultAwaSkillIndex.ToString());
				lines.Add("cameraDistance=" + entry.CameraDistance.ToString());
				lines.Add("cameraUnknown1=" + entry.CameraUnknown1.ToString());
				lines.Add("victoryAngle=" + entry.VictoryAngle.ToString());
				lines.Add("cameraUnknown2=" + entry.CameraUnknown2.ToString());
				lines.Add("cameraUnknown3=" + entry.CameraUnknown3.ToString());
				lines.Add("cameraUnknown4=" + entry.CameraUnknown4.ToString());
			}

			if (mode == DuelCopySettingsMode.Everything)
			{
				lines.Add("defaultAssist1=" + EncodeText(entry.DefaultAssist1));
				lines.Add("defaultAssist2=" + EncodeText(entry.DefaultAssist2));
				lines.Add("awakeAction=" + EncodeText(entry.AwakeAction));
				lines.Add("partner=" + EncodeText(entry.Partner));
			}

			return string.Join(Environment.NewLine, lines);
		}

		private void CopySelectedSettings()
		{
			int index = listBox1.SelectedIndex;
			if (index < 0 || index >= Entries.Count)
			{
				MessageBox.Show("No entry selected...");
				return;
			}

			DuelCopySettingsMode? mode = ShowCopySettingsDialog();
			if (!mode.HasValue)
			{
				return;
			}

			Clipboard.SetText(BuildCopySettingsPayload(Entries[index], mode.Value));
			MessageBox.Show("Selected settings copied to clipboard.");
		}

		private Dictionary<string, string> ParseCopySettingsPayload(string payload)
		{
			if (string.IsNullOrWhiteSpace(payload))
			{
				return null;
			}

			string[] lines = payload.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			if (lines.Length == 0 || lines[0] != "NSUNS4_EVO_DUEL_SETTINGS")
			{
				return null;
			}

			Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			for (int i = 1; i < lines.Length; i++)
			{
				int separator = lines[i].IndexOf('=');
				if (separator <= 0)
				{
					continue;
				}

				string key = lines[i].Substring(0, separator);
				string value = lines[i].Substring(separator + 1);
				values[key] = value;
			}

			return values;
		}

		private void PasteSelectedSettings()
		{
			int index = listBox1.SelectedIndex;
			if (index < 0 || index >= Entries.Count)
			{
				MessageBox.Show("No entry selected...");
				return;
			}

			if (!Clipboard.ContainsText())
			{
				MessageBox.Show("Clipboard does not contain copied Duel settings.");
				return;
			}

			Dictionary<string, string> payload = ParseCopySettingsPayload(Clipboard.GetText());
			if (payload == null)
			{
				MessageBox.Show("Clipboard does not contain a valid Duel settings payload.");
				return;
			}

			DuelPlayerParamEntry entry = Entries[index].Clone();

			if (payload.ContainsKey("item0")) entry.Items[0] = DecodeText(payload["item0"]);
			if (payload.ContainsKey("item1")) entry.Items[1] = DecodeText(payload["item1"]);
			if (payload.ContainsKey("item2")) entry.Items[2] = DecodeText(payload["item2"]);
			if (payload.ContainsKey("item3")) entry.Items[3] = DecodeText(payload["item3"]);
			if (payload.ContainsKey("itemCounts")) entry.ItemCounts = DecodeItemCounts(payload["itemCounts"]);

			if (payload.ContainsKey("conditionFlag")) entry.ConditionFlag = unchecked((int)uint.Parse(payload["conditionFlag"]));
			if (payload.ContainsKey("enableAwaSkill")) entry.EnableAwaSkill = int.Parse(payload["enableAwaSkill"]);

			if (payload.ContainsKey("settingList")) entry.SettingList = DecodeBytes(payload["settingList"]);
			if (payload.ContainsKey("setting2List")) entry.Setting2List = DecodeBytes(payload["setting2List"]);
			if (payload.ContainsKey("awaSettingList")) entry.AwaSettingList = DecodeBytes(payload["awaSettingList"]);
            if (payload.ContainsKey("extraSettings")) DecodeExtraSettings(entry, payload["extraSettings"]);
			if (payload.ContainsKey("evoDup")) entry.EvoDup = long.Parse(payload["evoDup"]);
			if (payload.ContainsKey("awaBodyPriority")) entry.AwaBodyPriority = int.Parse(payload["awaBodyPriority"]);
			if (payload.ContainsKey("defaultAwaSkillIndex")) entry.DefaultAwaSkillIndex = int.Parse(payload["defaultAwaSkillIndex"]);
			if (payload.ContainsKey("cameraDistance")) entry.CameraDistance = int.Parse(payload["cameraDistance"]);
			if (payload.ContainsKey("cameraUnknown1")) entry.CameraUnknown1 = int.Parse(payload["cameraUnknown1"]);
			if (payload.ContainsKey("victoryAngle")) entry.VictoryAngle = int.Parse(payload["victoryAngle"]);
			if (payload.ContainsKey("cameraUnknown2")) entry.CameraUnknown2 = int.Parse(payload["cameraUnknown2"]);
			if (payload.ContainsKey("cameraUnknown3")) entry.CameraUnknown3 = int.Parse(payload["cameraUnknown3"]);
			if (payload.ContainsKey("cameraUnknown4")) entry.CameraUnknown4 = int.Parse(payload["cameraUnknown4"]);

			if (payload.ContainsKey("defaultAssist1")) entry.DefaultAssist1 = DecodeText(payload["defaultAssist1"]);
			if (payload.ContainsKey("defaultAssist2")) entry.DefaultAssist2 = DecodeText(payload["defaultAssist2"]);
			if (payload.ContainsKey("awakeAction")) entry.AwakeAction = DecodeText(payload["awakeAction"]);
			if (payload.ContainsKey("partner")) entry.Partner = DecodeText(payload["partner"]);

			ApplyEntryToForm(entry);
			MessageBox.Show("Settings pasted into the form. Press Save selected entry to apply them.");
		}

		private void UpdateConditionFlagsFromValue(int condition)
		{
			syncingConditionControls = true;
			for (int i = 0; i < checkedListConditionFlags.Items.Count; i++)
			{
				bool isChecked = ((uint)condition & (1u << i)) != 0;
				checkedListConditionFlags.SetItemChecked(i, isChecked);
			}
			syncingConditionControls = false;
		}

		private int BuildConditionValueFromFlags()
		{
			uint value = 0;
			for (int i = 0; i < checkedListConditionFlags.Items.Count; i++)
			{
				if (checkedListConditionFlags.GetItemChecked(i))
				{
					value |= (1u << i);
				}
			}
			return unchecked((int)value);
		}

		public void UpdateCostumeEntry(int index, string[] costumes, bool awakening)
		{
			if (index < 0 || index >= Entries.Count)
			{
				return;
			}

			if (awakening)
			{
				Entries[index].AwakeCostumes = (string[])costumes.Clone();
			}
			else
			{
				Entries[index].BaseCostumes = (string[])costumes.Clone();
			}

			RefreshLegacyListsFromEntries();
		}

		public void UpdateSettingsEntry(int index, byte[] setting1, byte[] setting2, byte[] awakeSetting, int awaBodyPriority, int defaultAwaSkillIndex, int cameraDistance, int cameraUnknown1, int victoryCameraAngle, int cameraUnknown2, int cameraUnknown3, int cameraUnknown4)
		{
			if (index < 0 || index >= Entries.Count)
			{
				return;
			}

			Entries[index].SettingList = (byte[])setting1.Clone();
			Entries[index].Setting2List = (byte[])setting2.Clone();
			Entries[index].AwaSettingList = (byte[])awakeSetting.Clone();
			Entries[index].AwaBodyPriority = awaBodyPriority;
			Entries[index].DefaultAwaSkillIndex = defaultAwaSkillIndex;
			Entries[index].CameraDistance = cameraDistance;
			Entries[index].CameraUnknown1 = cameraUnknown1;
			Entries[index].VictoryAngle = victoryCameraAngle;
			Entries[index].CameraUnknown2 = cameraUnknown2;
			Entries[index].CameraUnknown3 = cameraUnknown3;
			Entries[index].CameraUnknown4 = cameraUnknown4;
			RefreshLegacyListsFromEntries();
		}

		public void NewFile()
		{
			FileOpen = true;
			FilePath = "";
			ClearEntryCollections();
			EntryCount = 1;
			BinPath.Add("Z:/param/player/Converter/bin/1newprm_bas.bin");
			BinName.Add("1newprm_bas");
			Data.Add(new byte[760]
			{
				50,
				110,
				114,
				116,
				0,
				0,
				0,
				0,
				50,
				110,
				114,
				116,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				100,
				110,
				114,
				107,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				100,
				110,
				114,
				100,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				50,
				110,
				114,
				113,
				0,
				0,
				0,
				0,
				50,
				110,
				114,
				113,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				50,
				110,
				114,
				113,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				50,
				110,
				114,
				113,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				168,
				192,
				1,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				45,
				1,
				0,
				0,
				0,
				0,
				0,
				0,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				255,
				0,
				0,
				0,
				0,
				50,
				115,
				107,
				114,
				0,
				0,
				0,
				0,
				50,
				107,
				107,
				115,
				0,
				0,
				0,
				0,
				160,
				0,
				148,
				0,
				148,
				0,
				40,
				0,
				45,
				0,
				110,
				0,
				0,
				0,
				0,
				66,
				0,
				0,
				200,
				66,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				65,
				87,
				65,
				75,
				69,
				95,
				50,
				78,
				82,
				71,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				32,
				66,
				70,
				0,
				14,
				0,
				25,
				0,
				15,
				0,
				0,
				0,
				0,
				63,
				66,
				65,
				84,
				84,
				76,
				69,
				95,
				73,
				84,
				69,
				77,
				49,
				53,
				48,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				2,
				0,
				66,
				65,
				84,
				84,
				76,
				69,
				95,
				73,
				84,
				69,
				77,
				57,
				48,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				2,
				0,
				66,
				65,
				84,
				84,
				76,
				69,
				95,
				73,
				84,
				69,
				77,
				57,
				57,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				2,
				0,
				66,
				65,
				84,
				84,
				76,
				69,
				95,
				73,
				84,
				69,
				77,
				49,
				52,
				52,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				2,
				0,
				0,
				0,
				0,
				66,
				0,
				0,
				200,
				66,
				70,
				0,
				14,
				0,
				25,
				0,
				15,
				0,
				0,
				0,
				0,
				63,
				0,
				0,
				0,
				63,
				0,
				0,
				64,
				63,
				102,
				102,
				230,
				63,
				0,
				0,
				160,
				64,
				0,
				0,
				0,
				64,
				0,
				0,
				128,
				63,
				0,
				0,
				128,
				63,
				0,
				0,
				112,
				65,
				0,
				0,
				0,
				64,
				0,
				0,
				0,
				0,
				1,
				0,
				0,
				0,
				1,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				205,
				204,
				204,
				61,
				205,
				204,
				204,
				61,
				154,
				153,
				153,
				62,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			});
			CharaList.Add("1new");
			string[] costumes = new string[20];
			for (int x4 = 0; x4 < 20; x4++)
			{
				costumes[x4] = "";
			}
			CostumeList.Add(costumes);
			string[] awkcostumes = new string[20];
			for (int x3 = 0; x3 < 20; x3++)
			{
				awkcostumes[x3] = "";
			}
			AwkCostumeList.Add(awkcostumes);
			DefaultAssist1.Add("");
			DefaultAssist2.Add("");
			AwkAction.Add("");
			string[] items = new string[4];
			for (int x2 = 0; x2 < 4; x2++)
			{
				items[x2] = "";
			}
			ItemList.Add(items);
			short[] itemc = new short[4];
			for (int x = 0; x < 4; x++)
			{
				itemc[x] = 0;
			}
			ItemCount.Add(itemc);
			Partner.Add("");

			byte[] defaultData = Data[0];
			SettingList.Add(Main.b_ReadByteArray(defaultData, 448, 36));
			Setting2List.Add(Main.b_ReadByteArray(defaultData, 500, 16));
			EvoDupList.Add(CombineEvoDup(Main.b_ReadInt(defaultData, 0x154), Main.b_ReadInt(defaultData, 0x158)));
			AwaBodyPriorityList.Add(Main.b_ReadInt(defaultData, 0x160));
			DefaultAwaSkillIndexList.Add(Main.b_ReadInt(defaultData, 0x164));
			ConditionFlagList.Add(Main.b_ReadIntRev(defaultData, 0x150));
			EnableAwaSkillList.Add(defaultData[0x153]);
			CameraDistanceList.Add(ReadUInt16(defaultData, 0x1B4));
			CameraUnknown1List.Add(ReadUInt16(defaultData, 0x1B6));
			VictoryAngleList.Add(ReadUInt16(defaultData, 0x1B8));
			CameraUnknown2List.Add(ReadUInt16(defaultData, 0x1BA));
			CameraUnknown3List.Add(ReadUInt16(defaultData, 0x1BC));
			CameraUnknown4List.Add(ReadUInt16(defaultData, 0x1BE));
			AwaSettingList.Add(Main.b_ReadByteArray(defaultData, 644, 84));
			RebuildEntriesFromLegacyLists();
			RefreshEntryListBox();
			if (listBox1.Items.Count > 0)
			{
				listBox1.SelectedIndex = 0;
			}
		}

		public void OpenFile(string basepath = "")
		{
			OpenFileDialog o = new OpenFileDialog();
			{
				o.DefaultExt = ".xfbin";
				o.Filter = "*.xfbin|*.xfbin";
			}

			if (basepath == "")
            {
                o.ShowDialog();
            }
            else
            {
                o.FileName = basepath;
            }

			if (!(o.FileName != "") || !File.Exists(o.FileName))
			{
				return;
			}
			FileOpen = true;

			ClearEntryCollections();
			FilePath = o.FileName;
			byte[] FileBytes = File.ReadAllBytes(FilePath);
			EntryCount = Main.b_byteArrayToIntRev(Main.b_ReadByteArray(FileBytes, 36, 4)) - 1;
            //if (this.Visible) MessageBox.Show("This file contains " + EntryCount.ToString("X2") + " entries.");
			int Index3 = 128;
			for (int x3 = 0; x3 < EntryCount; x3++)
			{
				string path = Main.b_ReadString(FileBytes, Index3);
				BinPath.Add(path);
				Index3 = Index3 + path.Length + 1;
			}
			Index3++;
			for (int x2 = 0; x2 < EntryCount + 2; x2++)
			{
				string name = Main.b_ReadString(FileBytes, Index3);
				BinName.Add(name);
				Index3 = Index3 + name.Length + 1;
			}
			BinName.RemoveAt(1);
			BinName.RemoveAt(1);
			int StartOfFile = 68 + Main.b_byteArrayToIntRev(Main.b_ReadByteArray(FileBytes, 16, 4));
			for (int x = 0; x < EntryCount; x++)
			{
				List<byte> data = new List<byte>();
				for (int y = 0; y < 760; y++)
				{
					data.Add(FileBytes[StartOfFile + 760 * x + 48 * x + y]);
				}
				Data.Add(data.ToArray());
				int _ptr = StartOfFile + 760 * x + 48 * x;
				string characodeid = Main.b_ReadString(FileBytes, _ptr);
				string[] costumeid = new string[20];
				for (int c2 = 0; c2 < 20; c2++)
				{
					costumeid[c2] = "";
					string cid = ReadCostumeCode(FileBytes, _ptr + 8 + 8 * c2);
					if (cid != "")
					{
						costumeid[c2] = cid;
					}
				}
				string[] awkcostumeid = new string[20];
				for (int c = 0; c < 20; c++)
				{
					awkcostumeid[c] = "";
					string awkcid = ReadCostumeCode(FileBytes, _ptr + 168 + 8 * c);
					if (awkcid != "")
					{
						awkcostumeid[c] = awkcid;
					}
				}
				string defAssist3 = Main.b_ReadString(FileBytes, _ptr + 420);
				string defAssist2 = Main.b_ReadString(FileBytes, _ptr + 428);
				string awkaction = Main.b_ReadString(FileBytes, _ptr + 484);
				string[] itemlist = new string[4];
				short[] itemcount = new short[4];
				for (int i = 0; i < 4; i++)
				{
					itemlist[i] = "";
					itemcount[i] = 0;
					string item = Main.b_ReadString(FileBytes, _ptr + 516 + 32 * i);
					short count = BitConverter.ToInt16(FileBytes, _ptr + 546 + 32 * i); itemcount[i] = count;
					if (item != "")
					{
						itemlist[i] = item;
						itemcount[i] = count;
					}
				}
				SettingList.Add(Main.b_ReadByteArray(FileBytes, _ptr + 448, 36));
				Setting2List.Add(Main.b_ReadByteArray(FileBytes, _ptr + 500, 16));
				EvoDupList.Add(CombineEvoDup(Main.b_ReadInt(FileBytes, _ptr + 0x154), Main.b_ReadInt(FileBytes, _ptr + 0x158)));
				AwaBodyPriorityList.Add(Main.b_ReadInt(FileBytes, _ptr + 0x160));
				DefaultAwaSkillIndexList.Add(Main.b_ReadInt(FileBytes, _ptr + 0x164));
				ConditionFlagList.Add(Main.b_ReadIntRev(FileBytes, _ptr + 0x150));
				EnableAwaSkillList.Add(FileBytes[_ptr + 0x153]);
				CameraDistanceList.Add(ReadUInt16(FileBytes, _ptr + 0x1B4));
				CameraUnknown1List.Add(ReadUInt16(FileBytes, _ptr + 0x1B6));
				VictoryAngleList.Add(ReadUInt16(FileBytes, _ptr + 0x1B8));
				CameraUnknown2List.Add(ReadUInt16(FileBytes, _ptr + 0x1BA));
				CameraUnknown3List.Add(ReadUInt16(FileBytes, _ptr + 0x1BC));
				CameraUnknown4List.Add(ReadUInt16(FileBytes, _ptr + 0x1BE));

				AwaSettingList.Add(Main.b_ReadByteArray(FileBytes, _ptr + 644, 84));
				string partner = Main.b_ReadString(FileBytes, _ptr + 328);
                CharaList.Add(characodeid);
				CostumeList.Add(costumeid);
				AwkCostumeList.Add(awkcostumeid);
				DefaultAssist1.Add(defAssist3);
				DefaultAssist2.Add(defAssist2);
				AwkAction.Add(awkaction);
				ItemList.Add(itemlist);
				ItemCount.Add(itemcount);
                Partner.Add(partner);
			}
			Index3++;
			RebuildEntriesFromLegacyLists();
			RefreshEntryListBox();
		}

		public void SaveFile()
		{
			if (FilePath != "")
			{
				if (File.Exists(FilePath + ".backup"))
				{
					File.Delete(FilePath + ".backup");
				}
				File.Copy(FilePath, FilePath + ".backup");
				File.WriteAllBytes(FilePath, ConvertToFile());
                if (this.Visible) MessageBox.Show("File saved to " + FilePath + ".");
			}
			else
			{
				SaveFileAs();
			}
		}

		public void SaveFileAs(string basepath = "")
		{
			SaveFileDialog s = new SaveFileDialog();
			{
				s.DefaultExt = ".xfbin";
				s.Filter = "*.xfbin|*.xfbin";
			}
			if (basepath != "")
				s.FileName = basepath;
			else
				s.ShowDialog();
			if (!(s.FileName != ""))
			{
				return;
			}
			if (s.FileName == FilePath)
			{
				if (File.Exists(FilePath + ".backup"))
				{
					File.Delete(FilePath + ".backup");
				}
				File.Copy(FilePath, FilePath + ".backup");
			}
			else
			{
				FilePath = s.FileName;
			}
			RefreshLegacyListsFromEntries();
			File.WriteAllBytes(FilePath, ConvertToFile());
			if (basepath == "")
				MessageBox.Show("File saved to " + FilePath + ".");
		}

		public void CloseFile()
		{
			NewFile();
			FileOpen = false;
			FilePath = "";
		}

		public void AddEntry()
		{
			Entries.Add(CreateEntryFromCurrentForm());
			RefreshLegacyListsFromEntries();
			RefreshEntryListBox();
			listBox1.SelectedIndex = Entries.Count - 1;
		}

		public void RemoveEntry()
		{
			if (Entries.Count > 1)
			{
				int x = listBox1.SelectedIndex;
				if (x != -1)
				{
					Entries.RemoveAt(x);
					RefreshLegacyListsFromEntries();
					RefreshEntryListBox();
					if (Entries.Count > 0)
					{
						listBox1.SelectedIndex = Math.Max(0, x - 1);
					}
				}
				else
				{
					MessageBox.Show("No entry selected...");
				}
			}
			else
			{
				MessageBox.Show("You can't remove the last entry of this file.");
			}
		}

		public void EditEntry()
		{
			int x = listBox1.SelectedIndex;
			if (x != -1)
			{
				Entries[x] = CreateEntryFromCurrentForm();
				RefreshLegacyListsFromEntries();
                listBox1.Items[x] = Entries[x].BinName;
			}
			else
			{
				MessageBox.Show("No entry selected...");
			}
		}

		public byte[] ConvertToFile()
		{
            // Build the header
			int totalLength4 = 0;

            byte[] fileBytes36 = new byte[0];
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[127]
			{
				78,
				85,
				67,
				67,
				0,
				0,
				0,
				121,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				73,
				216,
				0,
				0,
				0,
				3,
				0,
				121,
				20,
				2,
				0,
				0,
				0,
				4,
				0,
				0,
				0,
				59,
				0,
				0,
				0,
				219,
				0,
				0,
				39,
				47,
				0,
				0,
				0,
				221,
				0,
				0,
				10,
				71,
				0,
				0,
				0,
				221,
				0,
				0,
				10,
				92,
				0,
				0,
				3,
				104,
				0,
				0,
				0,
				0,
				110,
				117,
				99,
				99,
				67,
				104,
				117,
				110,
				107,
				78,
				117,
				108,
				108,
				0,
				110,
				117,
				99,
				99,
				67,
				104,
				117,
				110,
				107,
				66,
				105,
				110,
				97,
				114,
				121,
				0,
				110,
				117,
				99,
				99,
				67,
				104,
				117,
				110,
				107,
				80,
				97,
				103,
				101,
				0,
				110,
				117,
				99,
				99,
				67,
				104,
				117,
				110,
				107,
				73,
				110,
				100,
				101,
				120,
				0
			});

            int PtrNucc = fileBytes36.Length;
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);

            for (int x6 = 0; x6 < EntryCount; x6++)
			{
				fileBytes36 = Main.b_AddString(fileBytes36, BinPath[x6]);
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);
			}

            int PtrPath = fileBytes36.Length;
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);

            for (int x5 = 0; x5 < 1; x5++)
			{
				fileBytes36 = Main.b_AddString(fileBytes36, BinName[x5]);
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);
			}

            fileBytes36 = Main.b_AddString(fileBytes36, "Page0");
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);
			fileBytes36 = Main.b_AddString(fileBytes36, "index");
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);

            for (int x4 = 1; x4 < EntryCount; x4++)
			{
				fileBytes36 = Main.b_AddString(fileBytes36, BinName[x4]);
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);
			}

            int PtrName = fileBytes36.Length;
			totalLength4 = PtrName;
			int AddedBytes = 0;

            while (fileBytes36.Length % 4 != 0)
			{
				AddedBytes++;
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[1]);
			}

            // Build bin1
            totalLength4 = fileBytes36.Length;
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[48]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				0,
				0,
				0,
				1,
				0,
				0,
				0,
				1,
				0,
				0,
				0,
				2,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				2,
				0,
				0,
				0,
				3,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				3
			});

            for (int x3 = 1; x3 < EntryCount; x3++)
			{
				int actualEntry = x3 - 1;
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[4]
				{
					0,
					0,
					0,
					1
				});
				byte[] xbyte = BitConverter.GetBytes(2 + actualEntry);
				byte[] ybyte = BitConverter.GetBytes(4 + actualEntry);
				fileBytes36 = Main.b_AddBytes(fileBytes36, xbyte, 1);
				fileBytes36 = Main.b_AddBytes(fileBytes36, ybyte, 1);
			}

			int PtrSection = fileBytes36.Length;
			fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[16]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				0,
				0,
				0,
				2,
				0,
				0,
				0,
				3
			});
			for (int x2 = 1; x2 < EntryCount; x2++)
			{
				int actualEntry2 = x2 - 1;
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[4]);
				byte[] xbyte2 = BitConverter.GetBytes(4 + actualEntry2);
				fileBytes36 = Main.b_AddBytes(fileBytes36, xbyte2, 1);
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[4]
				{
					0,
					0,
					0,
					2
				});
				fileBytes36 = Main.b_AddBytes(fileBytes36, new byte[4]
				{
					0,
					0,
					0,
					3
				});
			}

			totalLength4 = fileBytes36.Length;

			int PathLength = PtrPath - 127;
			int NameLength = PtrName - PtrPath;
			int Section1Length = PtrSection - PtrName - AddedBytes;
			int FullLength = totalLength4 - 68 + 40;
			int ReplaceIndex8 = 16;
			byte[] buffer8 = BitConverter.GetBytes(FullLength);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 36;
			buffer8 = BitConverter.GetBytes(EntryCount + 1);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 40;
			buffer8 = BitConverter.GetBytes(PathLength);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 44;
			buffer8 = BitConverter.GetBytes(EntryCount + 3);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 48;
			buffer8 = BitConverter.GetBytes(NameLength);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 52;
			buffer8 = BitConverter.GetBytes(EntryCount + 3);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 56;
			buffer8 = BitConverter.GetBytes(Section1Length);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			ReplaceIndex8 = 60;
			buffer8 = BitConverter.GetBytes(EntryCount * 4);
			fileBytes36 = Main.b_ReplaceBytes(fileBytes36, buffer8, ReplaceIndex8, 1);
			for (int x = 0; x < EntryCount; x++)
			{
				fileBytes36 = ((x != 0) ? Main.b_AddBytes(fileBytes36, new byte[48]
				{
					0,
					0,
					0,
					8,
					0,
					0,
					0,
					2,
					0,
					99,
					0,
					0,
					0,
					0,
					0,
					4,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					99,
					0,
					0,
					0,
					0,
					2,
					252,
					0,
					0,
					0,
					1,
					0,
					99,
					0,
					0,
					0,
					0,
					2,
					248
				}) : Main.b_AddBytes(fileBytes36, new byte[40]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					121,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					99,
					0,
					0,
					0,
					0,
					2,
					252,
					0,
					0,
					0,
					1,
					0,
					99,
					0,
					0,
					0,
					0,
					2,
					248
				}));
				fileBytes36 = Main.b_AddBytes(fileBytes36, Data[x].ToArray());
				int _ptr = 68 + FullLength + 48 * x + 760 * x;
				fileBytes36 = Main.b_ReplaceString(fileBytes36, CharaList[x], _ptr, 8);
				for (int i = 0; i < 20; i++)
				{
					fileBytes36 = Main.b_ReplaceString(fileBytes36, CostumeList[x][i], _ptr + 8 + 8 * i, 8);
					fileBytes36 = Main.b_ReplaceString(fileBytes36, AwkCostumeList[x][i], _ptr + 168 + 8 * i, 8);
				}

				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(ConditionFlagList[x]), _ptr + 0x150, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(GetEvoDupLow(EvoDupList[x])), _ptr + 0x154);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(GetEvoDupHigh(EvoDupList[x])), _ptr + 0x158);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(AwaBodyPriorityList[x]), _ptr + 0x160);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(DefaultAwaSkillIndexList[x]), _ptr + 0x164);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraDistanceList[x]), _ptr + 0x1B4);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown1List[x]), _ptr + 0x1B6);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)VictoryAngleList[x]), _ptr + 0x1B8);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown2List[x]), _ptr + 0x1BA);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown3List[x]), _ptr + 0x1BC);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown4List[x]), _ptr + 0x1BE);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, DefaultAssist1[x], _ptr + 420, 8);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, DefaultAssist2[x], _ptr + 428, 8);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, AwkAction[x], _ptr + 484, 16);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, SettingList[x], _ptr + 448);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, Setting2List[x], _ptr + 500);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, AwaSettingList[x], _ptr + 644);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][0], _ptr + 516, 30);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(ItemCount[x][0]), _ptr + 546);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][1], _ptr + 548, 30);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(ItemCount[x][1]), _ptr + 578);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][2], _ptr + 580, 30);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(ItemCount[x][2]), _ptr + 610);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][3], _ptr + 612, 30);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(ItemCount[x][3]), _ptr + 642);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, Partner[x], _ptr + 328, 8);
            }
			return Main.b_AddBytes(fileBytes36, new byte[20]
			{
				0,
				0,
				0,
				8,
				0,
				0,
				0,
				2,
				0,
				99,
				0,
				0,
				0,
				0,
				0,
				4,
				0,
				0,
				0,
				0
			});
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				DialogResult msg = MessageBox.Show("Are you sure you want to create a new file?", "", MessageBoxButtons.OKCancel);
				if (msg == DialogResult.OK)
				{
					NewFile();
				}
			}
			else
			{
				NewFile();
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				DialogResult msg = MessageBox.Show("Are you sure you want to open a new file?", "", MessageBoxButtons.OKCancel);
				if (msg == DialogResult.OK)
				{
					OpenFile();
				}
			}
			else
			{
				OpenFile();
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				SaveFile();
			}
			else
			{
				MessageBox.Show("No file loaded...");
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				SaveFileAs();
			}
			else
			{
				MessageBox.Show("No file loaded...");
			}
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				DialogResult msg = MessageBox.Show("Are you sure you want to discard this file?", "", MessageBoxButtons.OKCancel);
				if (msg == DialogResult.OK)
				{
					CloseFile();
				}
			}
			else
			{
				MessageBox.Show("No file loaded...");
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				AddEntry();
			}
			else
			{
				MessageBox.Show("No file loaded...");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				EditEntry();
			}
			else
			{
				MessageBox.Show("No file loaded...");
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (FileOpen)
			{
				RemoveEntry();
			}
			else
			{
				MessageBox.Show("No file loaded...");
			}
		}




		private void sortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortEntries();
		}

		private void copySettingsButton_Click(object sender, EventArgs e)
		{
			CopySelectedSettings();
		}

		private void pasteSettingsButton_Click(object sender, EventArgs e)
		{
			PasteSelectedSettings();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int x = listBox1.SelectedIndex;
			if (x != -1)
			{
				ApplyEntryToForm(Entries[x]);
			}
			else
			{
                w_charaprmbas.Text = "";
				w_characodeid.Text = "";
				w_defaultassist1.Text = "";
				w_defaultassist2.Text = "";
				w_awkaction.Text = "";
				w_item1.Text = "";
				w_item2.Text = "";
				w_item3.Text = "";
				w_item4.Text = "";
				w_itemc1.Value = 0;
				w_itemc2.Value = 0;
				w_itemc3.Value = 0;
				w_itemc4.Value = 0;
				w_partner.Text = "";
				UpdateConditionFlagsFromValue(0);
				v_enableAwaSkill.Value = 0;
				ResetInlineSettings();
			}
		}

		private void v_enableAwaSkill_ValueChanged(object sender, EventArgs e)
		{
			if (syncingConditionControls)
			{
				return;
			}

			uint rawValue = decimal.ToUInt32(v_enableAwaSkill.Value);
			UpdateConditionFlagsFromValue(unchecked((int)rawValue));
		}

		private void checkedListConditionFlags_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (syncingConditionControls)
			{
				return;
			}

			BeginInvoke((MethodInvoker)delegate
			{
				syncingConditionControls = true;
				v_enableAwaSkill.Value = (decimal)(uint)BuildConditionValueFromFlags();
				syncingConditionControls = false;
			});
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        private void Tool_DuelPlayerParamEditor_Load(object sender, EventArgs e)
        {
			if (File.Exists(Main.dppPath)) {
				OpenFile(Main.dppPath);
			}
        }

        private void Search_Click(object sender, EventArgs e)
        {
			if (FileOpen)
			{
				if (Search_TB.Text != "")
				{
					List<string> names = Entries.Select(entry => entry.BinName).ToList();
					if (Main.SearchStringIndex(names, Search_TB.Text, names.Count, listBox1.SelectedIndex) != -1)
					{
						listBox1.SelectedIndex = Main.SearchStringIndex(names, Search_TB.Text, names.Count, listBox1.SelectedIndex);
					}
					else
					{
						if (Main.SearchStringIndex(names, Search_TB.Text, names.Count, 0) != -1)
						{
							listBox1.SelectedIndex = Main.SearchStringIndex(names, Search_TB.Text, names.Count, -1);
						}
						else
						{
							MessageBox.Show("Section with that name doesn't exist in file");
						}
					}
				}
				else
				{
					MessageBox.Show("Write name of section in textbox");
				}
			}
			else
			{
				MessageBox.Show("Open file before trying to search section");
			}
		}

        private void Search_TB_TextChanged(object sender, EventArgs e)
        {

        }
        private void itemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Tool_DSP_ItemList s = new Tool_DSP_ItemList();
			s.Show();
		}

        private void groupConditionFlags_Enter(object sender, EventArgs e)
        {

        }

        private void checkedListConditionFlags_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Tool_DuelPlayerParamEditor_Load_1(object sender, EventArgs e)
        {
			if (File.Exists(Main.dppPath))
			{
				OpenFile(Main.dppPath);
			}
        }




        private void w_itemc3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void identityTab_Click(object sender, EventArgs e)
        {

        }
    }
}

