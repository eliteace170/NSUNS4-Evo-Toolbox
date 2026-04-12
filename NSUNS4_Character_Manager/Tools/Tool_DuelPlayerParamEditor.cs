using NSUNS4_Character_Manager.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
			public byte[] ItemCounts = new byte[4];
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
					ItemCounts = (byte[])ItemCounts.Clone(),
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
		public List<byte[]> ItemCount = new List<byte[]>();
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
			InitializeConditionFlagList();
			ResetInlineSettings();
		}

		private static int ReadUInt16(byte[] data, int offset)
		{
			return BitConverter.ToUInt16(data, offset);
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
					ItemCounts = (byte[])ItemCount[i].Clone(),
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
			ItemCount = Entries.Select(e => (byte[])e.ItemCounts.Clone()).ToList();
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
				ItemCounts = new byte[4],
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
			entry.DefaultAssist1 = w_defaultassist1.Text;
			entry.DefaultAssist2 = w_defaultassist2.Text;
			entry.AwakeAction = w_awkaction.Text;
			entry.Items = new string[4] { w_item1.Text, w_item2.Text, w_item3.Text, w_item4.Text };
			entry.ItemCounts = new byte[4] { (byte)w_itemc1.Value, (byte)w_itemc2.Value, (byte)w_itemc3.Value, (byte)w_itemc4.Value };
			entry.Partner = w_partner.Text;
			entry.ConditionFlag = (int)v_enableAwaSkill.Value;
			entry.EnableAwaSkill = (int)v_enableAwaSkill.Value;
			ApplyInlineSettingsToEntry(entry);
			return entry;
		}

		private void ApplyEntryToForm(DuelPlayerParamEntry entry)
		{
			w_charaprmbas.Text = !string.IsNullOrWhiteSpace(entry.CharacterId) ? entry.CharacterId : GetEntryPrefixFromBinName(entry.BinName);
			w_characodeid.Text = entry.MotionCode;
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
			SetNumericValue(v_enableAwaSkill, (decimal)(uint)entry.ConditionFlag);
			UpdateConditionFlagsFromValue(entry.ConditionFlag);
			LoadInlineSettingsFromEntry(entry);
		}

		private string[] GetConditionFlagNames()
		{
			return new string[]
			{
                "ENABLE AWAKENING JUTSU",
				"unknow 1",
				"unknow 2",
				"unknow 3",
				"unknow 4",
				"unknow 5",
				"unknow 6",
				"unknow 7",
				"ENABLE GIANT AWAKENING COND",
				"ENABLE PRIVATE CAMERA",
				"ENABLE GIANT AWAKENING LAND SOUND",
				"ENABLE AWAKENING HITMARK",
                "unknow 8",
				"unknow 9",
				"unknow 10",
				"unknow 11",
				"unknow 12",
				"unknow 13",
				"ENABLE BASE GLOW",
				"ENABLE AWAKE GLOW",
				"ENABLE TELEPORT DASH",
				"unknow 14",
				"unknow 15",
				"ENABLE AWAKENING MOVESET",
				"unknow 16",
				"unknow 17",
				"unknow 18",
				"ENABLE PUPPET COND",
				"ENABLE PUPPET USER COND",
				"unknow 19",
				"unknow 20",
				"unknow 21"
            };
		}

		private void InitializeConditionFlagList()
		{
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

			SetNumericValue(setBaseMovement, (decimal)BitConverter.ToSingle(setting1, 0));
			SetNumericValue(setBaseChakraDash, (decimal)BitConverter.ToSingle(setting1, 4));
			SetNumericValue(setGuardPressure, (decimal)BitConverter.ToSingle(setting1, 8));
			SetNumericValue(setAttack, (decimal)BitConverter.ToSingle(setting1, 16));
			SetNumericValue(setDefense, (decimal)BitConverter.ToSingle(setting1, 20));
			SetNumericValue(setAssistDamage, (decimal)BitConverter.ToSingle(setting1, 24));
			SetNumericValue(setItemBuffDuration, (decimal)BitConverter.ToSingle(setting1, 28));
			SetNumericValue(setChakraCharge, (decimal)BitConverter.ToSingle(setting1, 32));

			SetNumericValue(setAwakeHpRequirement, (decimal)BitConverter.ToSingle(setting2, 0));
			SetNumericValue(setBaseNinjaDash, setting2[4]);
			SetNumericValue(setBaseAirDashDuration, setting2[6]);
			SetNumericValue(setBaseGroundedChakraDashDuration, setting2[8]);

			SetNumericValue(setAwakeMovement, (decimal)BitConverter.ToSingle(awake, 0));
			SetNumericValue(setAwakeChakraDash, (decimal)BitConverter.ToSingle(awake, 4));
			SetNumericValue(setAwakeNinjaDash, awake[8]);
			SetNumericValue(setAwakeAirDashDuration, awake[10]);
			SetNumericValue(setAwakeGroundedChakraDashDuration, awake[12]);
			setEnableDashPriority.Checked = awake[60] != 0;
			setAwakeningDebuff.Checked = awake[64] != 0;
			SetNumericValue(setChakraCostAwakening, (decimal)BitConverter.ToSingle(awake, 68));
			SetNumericValue(setChakraBlockRecovery, (decimal)BitConverter.ToSingle(awake, 76));
			SetNumericValue(setAwakeningActionCharge, (decimal)BitConverter.ToSingle(awake, 80));

			SetNumericValue(setEvo1, entry.EvoDup);
			SetNumericValue(setAwaBodyPriority, entry.AwaBodyPriority);
			SetNumericValue(setDefaultAwaSkillIndex, entry.DefaultAwaSkillIndex);
			SetNumericValue(setCameraDistance, entry.CameraDistance);
			SetNumericValue(setCameraUnknown1, entry.CameraUnknown1);
			SetNumericValue(setVictoryCameraAngle, entry.VictoryAngle);
			SetNumericValue(setCameraUnknown2, entry.CameraUnknown2);
			SetNumericValue(setCameraUnknown3, entry.CameraUnknown3);
			SetNumericValue(setCameraUnknown4, entry.CameraUnknown4);
		}

		private void ApplyInlineSettingsToEntry(DuelPlayerParamEntry entry)
		{
			byte[] setting1 = new byte[36];
			byte[] setting2 = entry.Setting2List != null && entry.Setting2List.Length == 16 ? (byte[])entry.Setting2List.Clone() : new byte[16];
			byte[] awake = entry.AwaSettingList != null && entry.AwaSettingList.Length == 84 ? (byte[])entry.AwaSettingList.Clone() : new byte[84];

			Buffer.BlockCopy(BitConverter.GetBytes((float)setBaseMovement.Value), 0, setting1, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setBaseChakraDash.Value), 0, setting1, 4, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setGuardPressure.Value), 0, setting1, 8, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setAttack.Value), 0, setting1, 16, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setDefense.Value), 0, setting1, 20, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setAssistDamage.Value), 0, setting1, 24, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setItemBuffDuration.Value), 0, setting1, 28, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setChakraCharge.Value), 0, setting1, 32, 4);

			Buffer.BlockCopy(BitConverter.GetBytes((float)setAwakeHpRequirement.Value), 0, setting2, 0, 4);
			setting2[4] = (byte)setBaseNinjaDash.Value;
			setting2[6] = (byte)setBaseAirDashDuration.Value;
			setting2[8] = (byte)setBaseGroundedChakraDashDuration.Value;

			Buffer.BlockCopy(BitConverter.GetBytes((float)setAwakeMovement.Value), 0, awake, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setAwakeChakraDash.Value), 0, awake, 4, 4);
			awake[8] = (byte)setAwakeNinjaDash.Value;
			awake[10] = (byte)setAwakeAirDashDuration.Value;
			awake[12] = (byte)setAwakeGroundedChakraDashDuration.Value;
			awake[60] = (byte)(setEnableDashPriority.Checked ? 1 : 0);
			awake[64] = (byte)(setAwakeningDebuff.Checked ? 1 : 0);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setChakraCostAwakening.Value), 0, awake, 68, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setChakraBlockRecovery.Value), 0, awake, 76, 4);
			Buffer.BlockCopy(BitConverter.GetBytes((float)setAwakeningActionCharge.Value), 0, awake, 80, 4);

			entry.SettingList = setting1;
			entry.Setting2List = setting2;
			entry.AwaSettingList = awake;
			entry.EvoDup = (long)setEvo1.Value;
			entry.AwaBodyPriority = (int)setAwaBodyPriority.Value;
			entry.DefaultAwaSkillIndex = (int)setDefaultAwaSkillIndex.Value;
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
			using (Form dialog = new Form())
			using (Label promptLabel = new Label())
			using (ListBox optionsList = new ListBox())
			using (Button okButton = new Button())
			using (Button cancelButton = new Button())
			{
				dialog.Text = "Copy settings";
				dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
				dialog.StartPosition = FormStartPosition.CenterParent;
				dialog.ClientSize = new Size(320, 245);
				dialog.MaximizeBox = false;
				dialog.MinimizeBox = false;
				dialog.ShowInTaskbar = false;

				promptLabel.AutoSize = false;
				promptLabel.Location = new Point(12, 12);
				promptLabel.Size = new Size(296, 36);
				promptLabel.Text = "Choose which settings to copy to the clipboard.";

				optionsList.Location = new Point(12, 52);
				optionsList.Size = new Size(296, 123);
				optionsList.Items.AddRange(new object[]
				{
					"1. Items only",
					"2. Conditions list",
					"3. Battle settings",
                    "4. Conditions + Battle settings",
					"5. Everything"
				});
				optionsList.SelectedIndex = 0;

				okButton.Text = "Copy";
				okButton.Location = new Point(152, 190);
				okButton.Size = new Size(75, 25);
				okButton.DialogResult = DialogResult.OK;

				cancelButton.Text = "Cancel";
				cancelButton.Location = new Point(233, 190);
				cancelButton.Size = new Size(75, 25);
				cancelButton.DialogResult = DialogResult.Cancel;

				dialog.AcceptButton = okButton;
				dialog.CancelButton = cancelButton;
				dialog.Controls.Add(promptLabel);
				dialog.Controls.Add(optionsList);
				dialog.Controls.Add(okButton);
				dialog.Controls.Add(cancelButton);

				if (dialog.ShowDialog(this) != DialogResult.OK || optionsList.SelectedIndex < 0)
				{
					return null;
				}

				return (DuelCopySettingsMode)optionsList.SelectedIndex;
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
				lines.Add("itemCounts=" + EncodeBytes(entry.ItemCounts));
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
			if (payload.ContainsKey("itemCounts")) entry.ItemCounts = DecodeBytes(payload["itemCounts"]);

			if (payload.ContainsKey("conditionFlag")) entry.ConditionFlag = unchecked((int)uint.Parse(payload["conditionFlag"]));
			if (payload.ContainsKey("enableAwaSkill")) entry.EnableAwaSkill = int.Parse(payload["enableAwaSkill"]);

			if (payload.ContainsKey("settingList")) entry.SettingList = DecodeBytes(payload["settingList"]);
			if (payload.ContainsKey("setting2List")) entry.Setting2List = DecodeBytes(payload["setting2List"]);
			if (payload.ContainsKey("awaSettingList")) entry.AwaSettingList = DecodeBytes(payload["awaSettingList"]);
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
			EntryCount = 0;
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
			AwaSettingList.Clear();
			listBox1.ClearSelected();
			listBox1.Items.Clear();
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
			byte[] itemc = new byte[4];
			for (int x = 0; x < 4; x++)
			{
				itemc[x] = 0;
			}
			ItemCount.Add(itemc);
            Partner.Add("");
			ConditionFlagList.Add(0);
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

			listBox1.Items.Clear();
			EntryCount = 0;
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
			Setting2List.Clear();
			EnableAwaSkillList.Clear();
			VictoryAngleList.Clear();
			VictoryPosList.Clear();
			VictoryUnknownList.Clear();
			AwaSettingList.Clear();
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
					string cid = Main.b_ReadString(FileBytes, _ptr + 8 + 8 * c2);
					if (cid != "")
					{
						costumeid[c2] = cid;
					}
				}
				string[] awkcostumeid = new string[20];
				for (int c = 0; c < 20; c++)
				{
					awkcostumeid[c] = "";
					string awkcid = Main.b_ReadString(FileBytes, _ptr + 168 + 8 * c);
					if (awkcid != "")
					{
						awkcostumeid[c] = awkcid;
					}
				}
				string defAssist3 = Main.b_ReadString(FileBytes, _ptr + 420);
				string defAssist2 = Main.b_ReadString(FileBytes, _ptr + 428);
				string awkaction = Main.b_ReadString(FileBytes, _ptr + 484);
				string[] itemlist = new string[4];
				byte[] itemcount = new byte[4];
				for (int i = 0; i < 4; i++)
				{
					itemlist[i] = "";
					itemcount[i] = 0;
					string item = Main.b_ReadString(FileBytes, _ptr + 516 + 32 * i);
					byte count = FileBytes[_ptr + 546 + 32 * i];
					if (item != "")
					{
						itemlist[i] = item;
						itemcount[i] = count;
					}
				}
				SettingList.Add(Main.b_ReadByteArray(FileBytes, _ptr + 448, 36));
				Setting2List.Add(Main.b_ReadByteArray(FileBytes, _ptr + 500, 16));
				EvoDupList.Add(CombineEvoDup(Main.b_ReadInt(FileBytes, _ptr + 0x154), Main.b_ReadInt(FileBytes, _ptr + 0x158)));
				AwaBodyPriorityList.Add(Main.b_ReadIntRev(FileBytes, _ptr + 0x160));
				DefaultAwaSkillIndexList.Add(Main.b_ReadIntRev(FileBytes, _ptr + 0x164));
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
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(GetEvoDupLow(EvoDupList[x])), _ptr + 0x154, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(GetEvoDupHigh(EvoDupList[x])), _ptr + 0x158, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(AwaBodyPriorityList[x]), _ptr + 0x160, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes(DefaultAwaSkillIndexList[x]), _ptr + 0x164, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraDistanceList[x]), _ptr + 0x1B4, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown1List[x]), _ptr + 0x1B6, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)VictoryAngleList[x]), _ptr + 0x1B8, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown2List[x]), _ptr + 0x1BA, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown3List[x]), _ptr + 0x1BC, 1);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, BitConverter.GetBytes((short)CameraUnknown4List[x]), _ptr + 0x1BE, 1);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, DefaultAssist1[x], _ptr + 420, 8);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, DefaultAssist2[x], _ptr + 428, 8);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, AwkAction[x], _ptr + 484, 16);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, SettingList[x], _ptr + 448);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, Setting2List[x], _ptr + 500);
				fileBytes36 = Main.b_ReplaceBytes(fileBytes36, AwaSettingList[x], _ptr + 644);
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][0], _ptr + 516, 16);
				fileBytes36[_ptr + 546] = ItemCount[x][0];
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][1], _ptr + 548, 16);
				fileBytes36[_ptr + 578] = ItemCount[x][1];
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][2], _ptr + 580, 16);
				fileBytes36[_ptr + 610] = ItemCount[x][2];
				fileBytes36 = Main.b_ReplaceString(fileBytes36, ItemList[x][3], _ptr + 612, 16);
				fileBytes36[_ptr + 642] = ItemCount[x][3];
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
					CloseFile();
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

		private void b_costumeids_Click(object sender, EventArgs e)
		{
			int x = listBox1.SelectedIndex;
			if (x != -1)
			{
				Tool_DuelPlayerParamEditor_Costumes t = new Tool_DuelPlayerParamEditor_Costumes(CostumeList[x].ToArray(), AwkCostumeList[x].ToArray(), this, x);
				t.ShowDialog();
			}
			else
			{
				MessageBox.Show("No entry selected...");
			}
		}

		private void b_awkcostumeids_Click(object sender, EventArgs e)
		{
			b_costumeids_Click(sender, e);
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

        private void settingsTitleLabel_Click(object sender, EventArgs e)
        {

        }

        private void w_itemc3_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}






