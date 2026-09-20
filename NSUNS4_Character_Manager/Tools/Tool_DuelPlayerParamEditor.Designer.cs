using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
	public partial class Tool_DuelPlayerParamEditor
	{
		private IContainer components = null;
		public ListBox listBox1;
		private Button button1;
		private Button button2;
		private Button button3;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem newToolStripMenuItem;
		private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripMenuItem saveToolStripMenuItem;
		private ToolStripMenuItem saveAsToolStripMenuItem;
		private ToolStripMenuItem closeToolStripMenuItem;
		private ToolStripMenuItem sortToolStripMenuItem;
		private Label label1;
		private TextBox w_characodeid;

		private TextBox w_awkaction;
		private Label label2;
		private TextBox w_defaultassist1;
		private Label label3;
		private TextBox w_defaultassist2;
		private Label label4;
		private TextBox w_item1;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private NumericUpDown w_itemc1;
		private NumericUpDown w_itemc2;
		private TextBox w_item2;
		private NumericUpDown w_itemc3;
		private TextBox w_item3;
		private NumericUpDown w_itemc4;
		private TextBox w_charaprmbas;
		private Label label9;
		private TextBox w_partner;
		private Label label10;
		private TextBox Search_TB;
		private Button Search;
		private Button copySettingsButton;
		private Button pasteSettingsButton;
		private ToolStripMenuItem itemListToolStripMenuItem;
		private Label label11;
		private NumericUpDown v_enableAwaSkill;


		private TextBox w_item4;

		private NumericUpDown setBaseMovement;
		private NumericUpDown setAwakeMovement;

		private NumericUpDown setBaseChakraDash;
		private NumericUpDown setAwakeChakraDash;
		private NumericUpDown setGuardPressure;
		private NumericUpDown setAttack;
		private NumericUpDown setDefense;
		private NumericUpDown setAssistDamage;
		private NumericUpDown setAwakeningActionCharge;
		private NumericUpDown setChakraCharge;
		private NumericUpDown setBaseNinjaDash;
		private NumericUpDown setAwakeNinjaDash;
		private NumericUpDown setBaseAirDashDuration;
		private NumericUpDown setAwakeAirDashDuration;
		private NumericUpDown setBaseGroundedChakraDashDuration;
		private NumericUpDown setAwakeGroundedChakraDashDuration;
		private NumericUpDown setItemBuffDuration;
		private NumericUpDown setAwakeHpRequirement;
		private NumericUpDown setChakraCostAwakening;
		private NumericUpDown setChakraBlockRecovery;
		private ComboBox setAwaBodyPriority;
		private ComboBox setDefaultAwaSkillIndex;
		private NumericUpDown setCameraDistance;
		private NumericUpDown setCameraUnknown1;
		private NumericUpDown setVictoryCameraAngle;
		private NumericUpDown setCameraUnknown2;
		private NumericUpDown setCameraUnknown3;
		private NumericUpDown setCameraUnknown4;
		private ComboBox setEnableDashPriority;
        private Label setEnableDashPriorityLabel;

		private Label setBaseMovementLabel;
		private Label setBaseChakraDashLabel;
		private Label setGuardPressureLabel;
		private Label setAttackLabel;
		private Label setDefenseLabel;
		private Label setAssistDamageLabel;
		private Label setAwakeningActionChargeLabel;
		private Label setChakraChargeLabel;
		private Label setBaseNinjaDashLabel;
		private Label setBaseAirDashDurationLabel;
		private Label setBaseGroundedChakraDashDurationLabel;
		private Label setItemBuffDurationLabel;
		private Label setAwakeHpRequirementLabel;
		private Label setChakraCostAwakeningLabel;
		private Label setChakraBlockRecoveryLabel;

		private Label setAwaBodyPriorityLabel;
		private Label setDefaultAwaSkillIndexLabel;
		private Label setCameraDistanceLabel;
		private Label setCameraUnknown1Label;
		private Label setVictoryCameraAngleLabel;
		private Label setCameraUnknown2Label;
		private Label setCameraUnknown3Label;
		private Label setCameraUnknown4Label;

        private ComboBox awakeRisk;
        private Label awakeRiskLabel;

        private ComboBox extraJobType;
        private Label extraJobTypeLabel;
        private ComboBox extraFemaleAnims;
        private Label extraFemaleAnimsLabel;
        private ComboBox extraAdventureSupport;
        private Label extraAdventureSupportLabel;
        private NumericUpDown extraDashTrackTime;
        private Label extraDashTrackTimeLabel;
        private NumericUpDown extraAwakeDashTrackTime;
        private NumericUpDown extraDashTurnRate;
        private Label extraDashTurnRateLabel;
        private NumericUpDown extraAwakeDashTurnRate;
        private ToolTip fieldTips;

        private NumericUpDown extraUnknown15C;
        private Label extraUnknown15CLabel;
        private NumericUpDown extraUnknown16C;
        private Label extraUnknown16CLabel;
        private NumericUpDown extraUnknown170;
        private Label extraUnknown170Label;
        private NumericUpDown extraUnknown174;
        private Label extraUnknown174Label;
        private NumericUpDown extraUnknown178;
        private Label extraUnknown178Label;
        private NumericUpDown extraUnknown180;
        private Label extraUnknown180Label;
        private NumericUpDown extraUnknown184;
        private Label extraUnknown184Label;
        private NumericUpDown extraUnknown188;
        private Label extraUnknown188Label;
        private NumericUpDown extraUnknown18C;
        private Label extraUnknown18CLabel;
        private NumericUpDown extraUnknown190;
        private Label extraUnknown190Label;
        private NumericUpDown extraUnknown194;
        private Label extraUnknown194Label;
        private NumericUpDown extraUnknown198;
        private Label extraUnknown198Label;
        private NumericUpDown extraUnknown19C;
        private Label extraUnknown19CLabel;
        private NumericUpDown extraUnknown1CC;
        private Label extraUnknown1CCLabel;
        private Label extraAwakeDashTrackTimeLabel;
        private Label extraAwakeDashTurnRateLabel;
        private NumericUpDown extraUnknown298;
        private Label extraUnknown298Label;
        private NumericUpDown extraUnknown29C;
        private Label extraUnknown29CLabel;
        private NumericUpDown extraUnknown2A0;
        private Label extraUnknown2A0Label;
        private NumericUpDown extraUnknown2A4;
        private Label extraUnknown2A4Label;
        private NumericUpDown extraUnknown2A8;
        private Label extraUnknown2A8Label;
        private NumericUpDown extraUnknown2AC;
        private Label extraUnknown2ACLabel;
        private NumericUpDown extraUnknown2B0;
        private Label extraUnknown2B0Label;
        private NumericUpDown extraUnknown2B4;
        private Label extraUnknown2B4Label;
        private NumericUpDown extraUnknown2B8;
        private Label extraUnknown2B8Label;
        private NumericUpDown extraUnknown2BC;
        private Label extraUnknown2BCLabel;
        private NumericUpDown extraUnknown2CC;
        private Label extraUnknown2CCLabel;
        private Label setAwakeMovementLabel;
        private Label setAwakeChakraDashLabel;
        private Label setAwakeNinjaDashLabel;
        private Label setAwakeAirDashDurationLabel;
        private Label setAwakeGroundedChakraDashDurationLabel;
        private TabControl editorTabs;
        private TabPage identityTab;
        private TabPage flagsTab;
        private TabPage bodyTab;
        private TabPage baseTab;
        private TabPage awakeTab;
        private GroupBox costumesGroup;
        private DataGridView costumeGrid;
        private DataGridViewTextBoxColumn costumeSlotColumn;
        private DataGridViewTextBoxColumn baseCostumeColumn;
        private DataGridViewTextBoxColumn awakeCostumeColumn;
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tool_DuelPlayerParamEditor));
            this.editorTabs = new System.Windows.Forms.TabControl();
            this.identityTab = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.w_charaprmbas = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.w_characodeid = new System.Windows.Forms.TextBox();
            this.costumesGroup = new System.Windows.Forms.GroupBox();
            this.costumeGrid = new System.Windows.Forms.DataGridView();
            this.costumeSlotColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baseCostumeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.awakeCostumeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label10 = new System.Windows.Forms.Label();
            this.w_partner = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.w_defaultassist1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.w_defaultassist2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.w_item1 = new System.Windows.Forms.TextBox();
            this.w_itemc1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.w_item2 = new System.Windows.Forms.TextBox();
            this.w_itemc2 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.w_item3 = new System.Windows.Forms.TextBox();
            this.w_itemc3 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.w_item4 = new System.Windows.Forms.TextBox();
            this.w_itemc4 = new System.Windows.Forms.NumericUpDown();
            this.setEvo1Label = new System.Windows.Forms.Label();
            this.setEvo1 = new System.Windows.Forms.NumericUpDown();
            this.flagsTab = new System.Windows.Forms.TabPage();
            this.groupConditionFlags = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.v_enableAwaSkill = new System.Windows.Forms.NumericUpDown();
            this.checkedListConditionFlags = new System.Windows.Forms.CheckedListBox();
            this.extraUnknown15CLabel = new System.Windows.Forms.Label();
            this.extraUnknown15C = new System.Windows.Forms.NumericUpDown();
            this.setAwaBodyPriorityLabel = new System.Windows.Forms.Label();
            this.setAwaBodyPriority = new System.Windows.Forms.ComboBox();
            this.setDefaultAwaSkillIndexLabel = new System.Windows.Forms.Label();
            this.setDefaultAwaSkillIndex = new System.Windows.Forms.ComboBox();
            this.extraJobTypeLabel = new System.Windows.Forms.Label();
            this.extraJobType = new System.Windows.Forms.ComboBox();
            this.extraUnknown16CLabel = new System.Windows.Forms.Label();
            this.extraUnknown16C = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown170Label = new System.Windows.Forms.Label();
            this.extraUnknown170 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown174Label = new System.Windows.Forms.Label();
            this.extraUnknown174 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown178Label = new System.Windows.Forms.Label();
            this.extraUnknown178 = new System.Windows.Forms.NumericUpDown();
            this.extraFemaleAnimsLabel = new System.Windows.Forms.Label();
            this.extraFemaleAnims = new System.Windows.Forms.ComboBox();
            this.extraUnknown180Label = new System.Windows.Forms.Label();
            this.extraUnknown180 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown184Label = new System.Windows.Forms.Label();
            this.extraUnknown184 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown188Label = new System.Windows.Forms.Label();
            this.extraUnknown188 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown18CLabel = new System.Windows.Forms.Label();
            this.extraUnknown18C = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown190Label = new System.Windows.Forms.Label();
            this.extraUnknown190 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown194Label = new System.Windows.Forms.Label();
            this.extraUnknown194 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown198Label = new System.Windows.Forms.Label();
            this.extraUnknown198 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown19CLabel = new System.Windows.Forms.Label();
            this.extraUnknown19C = new System.Windows.Forms.NumericUpDown();
            this.extraAdventureSupportLabel = new System.Windows.Forms.Label();
            this.extraAdventureSupport = new System.Windows.Forms.ComboBox();
            this.bodyTab = new System.Windows.Forms.TabPage();
            this.setCameraDistanceLabel = new System.Windows.Forms.Label();
            this.setCameraDistance = new System.Windows.Forms.NumericUpDown();
            this.setCameraUnknown1Label = new System.Windows.Forms.Label();
            this.setCameraUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.setVictoryCameraAngleLabel = new System.Windows.Forms.Label();
            this.setVictoryCameraAngle = new System.Windows.Forms.NumericUpDown();
            this.setCameraUnknown2Label = new System.Windows.Forms.Label();
            this.setCameraUnknown2 = new System.Windows.Forms.NumericUpDown();
            this.setCameraUnknown3Label = new System.Windows.Forms.Label();
            this.setCameraUnknown3 = new System.Windows.Forms.NumericUpDown();
            this.setCameraUnknown4Label = new System.Windows.Forms.Label();
            this.setCameraUnknown4 = new System.Windows.Forms.NumericUpDown();
            this.baseTab = new System.Windows.Forms.TabPage();
            this.setBaseMovementLabel = new System.Windows.Forms.Label();
            this.setBaseMovement = new System.Windows.Forms.NumericUpDown();
            this.setBaseChakraDashLabel = new System.Windows.Forms.Label();
            this.setBaseChakraDash = new System.Windows.Forms.NumericUpDown();
            this.setGuardPressureLabel = new System.Windows.Forms.Label();
            this.setGuardPressure = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown1CCLabel = new System.Windows.Forms.Label();
            this.extraUnknown1CC = new System.Windows.Forms.NumericUpDown();
            this.setAttackLabel = new System.Windows.Forms.Label();
            this.setAttack = new System.Windows.Forms.NumericUpDown();
            this.setDefenseLabel = new System.Windows.Forms.Label();
            this.setDefense = new System.Windows.Forms.NumericUpDown();
            this.setAssistDamageLabel = new System.Windows.Forms.Label();
            this.setAssistDamage = new System.Windows.Forms.NumericUpDown();
            this.setItemBuffDurationLabel = new System.Windows.Forms.Label();
            this.setItemBuffDuration = new System.Windows.Forms.NumericUpDown();
            this.setChakraChargeLabel = new System.Windows.Forms.Label();
            this.setChakraCharge = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.w_awkaction = new System.Windows.Forms.TextBox();
            this.setAwakeHpRequirementLabel = new System.Windows.Forms.Label();
            this.setAwakeHpRequirement = new System.Windows.Forms.NumericUpDown();
            this.setBaseNinjaDashLabel = new System.Windows.Forms.Label();
            this.setBaseNinjaDash = new System.Windows.Forms.NumericUpDown();
            this.setBaseAirDashDurationLabel = new System.Windows.Forms.Label();
            this.setBaseAirDashDuration = new System.Windows.Forms.NumericUpDown();
            this.setBaseGroundedChakraDashDurationLabel = new System.Windows.Forms.Label();
            this.setBaseGroundedChakraDashDuration = new System.Windows.Forms.NumericUpDown();
            this.extraDashTrackTimeLabel = new System.Windows.Forms.Label();
            this.extraDashTrackTime = new System.Windows.Forms.NumericUpDown();
            this.extraDashTurnRateLabel = new System.Windows.Forms.Label();
            this.extraDashTurnRate = new System.Windows.Forms.NumericUpDown();
            this.awakeTab = new System.Windows.Forms.TabPage();
            this.setAwakeMovementLabel = new System.Windows.Forms.Label();
            this.setAwakeMovement = new System.Windows.Forms.NumericUpDown();
            this.setAwakeChakraDashLabel = new System.Windows.Forms.Label();
            this.setAwakeChakraDash = new System.Windows.Forms.NumericUpDown();
            this.setAwakeNinjaDashLabel = new System.Windows.Forms.Label();
            this.setAwakeNinjaDash = new System.Windows.Forms.NumericUpDown();
            this.setAwakeAirDashDurationLabel = new System.Windows.Forms.Label();
            this.setAwakeAirDashDuration = new System.Windows.Forms.NumericUpDown();
            this.setAwakeGroundedChakraDashDurationLabel = new System.Windows.Forms.Label();
            this.setAwakeGroundedChakraDashDuration = new System.Windows.Forms.NumericUpDown();
            this.extraAwakeDashTrackTimeLabel = new System.Windows.Forms.Label();
            this.extraAwakeDashTrackTime = new System.Windows.Forms.NumericUpDown();
            this.extraAwakeDashTurnRateLabel = new System.Windows.Forms.Label();
            this.extraAwakeDashTurnRate = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown298Label = new System.Windows.Forms.Label();
            this.extraUnknown298 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown29CLabel = new System.Windows.Forms.Label();
            this.extraUnknown29C = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2A0Label = new System.Windows.Forms.Label();
            this.extraUnknown2A0 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2A4Label = new System.Windows.Forms.Label();
            this.extraUnknown2A4 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2A8Label = new System.Windows.Forms.Label();
            this.extraUnknown2A8 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2ACLabel = new System.Windows.Forms.Label();
            this.extraUnknown2AC = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2B0Label = new System.Windows.Forms.Label();
            this.extraUnknown2B0 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2B4Label = new System.Windows.Forms.Label();
            this.extraUnknown2B4 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2B8Label = new System.Windows.Forms.Label();
            this.extraUnknown2B8 = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2BCLabel = new System.Windows.Forms.Label();
            this.extraUnknown2BC = new System.Windows.Forms.NumericUpDown();
            this.setEnableDashPriorityLabel = new System.Windows.Forms.Label();
            this.setEnableDashPriority = new System.Windows.Forms.ComboBox();
            this.awakeRiskLabel = new System.Windows.Forms.Label();
            this.awakeRisk = new System.Windows.Forms.ComboBox();
            this.setChakraCostAwakeningLabel = new System.Windows.Forms.Label();
            this.setChakraCostAwakening = new System.Windows.Forms.NumericUpDown();
            this.extraUnknown2CCLabel = new System.Windows.Forms.Label();
            this.extraUnknown2CC = new System.Windows.Forms.NumericUpDown();
            this.setChakraBlockRecoveryLabel = new System.Windows.Forms.Label();
            this.setChakraBlockRecovery = new System.Windows.Forms.NumericUpDown();
            this.setAwakeningActionChargeLabel = new System.Windows.Forms.Label();
            this.setAwakeningActionCharge = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.Search_TB = new System.Windows.Forms.TextBox();
            this.Search = new System.Windows.Forms.Button();
            this.copySettingsButton = new System.Windows.Forms.Button();
            this.pasteSettingsButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.fieldTips = new System.Windows.Forms.ToolTip(this.components);
            this.editorTabs.SuspendLayout();
            this.identityTab.SuspendLayout();
            this.costumesGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.costumeGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setEvo1)).BeginInit();
            this.flagsTab.SuspendLayout();
            this.groupConditionFlags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.v_enableAwaSkill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown15C)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown16C)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown170)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown174)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown178)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown180)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown184)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown188)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown18C)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown190)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown194)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown198)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown19C)).BeginInit();
            this.bodyTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setVictoryCameraAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown4)).BeginInit();
            this.baseTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseMovement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseChakraDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setGuardPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown1CC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAttack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setDefense)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAssistDamage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setItemBuffDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeHpRequirement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseNinjaDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseAirDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseGroundedChakraDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraDashTrackTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraDashTurnRate)).BeginInit();
            this.awakeTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeMovement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeChakraDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeNinjaDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeAirDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeGroundedChakraDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraAwakeDashTrackTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraAwakeDashTurnRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown298)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown29C)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2A0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2A4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2A8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2AC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2B0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2B4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2B8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2BC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCostAwakening)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2CC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraBlockRecovery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeningActionCharge)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editorTabs
            // 
            this.editorTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorTabs.Controls.Add(this.identityTab);
            this.editorTabs.Controls.Add(this.flagsTab);
            this.editorTabs.Controls.Add(this.bodyTab);
            this.editorTabs.Controls.Add(this.baseTab);
            this.editorTabs.Controls.Add(this.awakeTab);
            this.editorTabs.Location = new System.Drawing.Point(228, 32);
            this.editorTabs.Name = "editorTabs";
            this.editorTabs.SelectedIndex = 0;
            this.editorTabs.Size = new System.Drawing.Size(884, 588);
            this.editorTabs.TabIndex = 3;
            // 
            // identityTab
            // 
            this.identityTab.Controls.Add(this.label9);
            this.identityTab.Controls.Add(this.w_charaprmbas);
            this.identityTab.Controls.Add(this.label1);
            this.identityTab.Controls.Add(this.w_characodeid);
            this.identityTab.Controls.Add(this.costumesGroup);
            this.identityTab.Controls.Add(this.label10);
            this.identityTab.Controls.Add(this.w_partner);
            this.identityTab.Controls.Add(this.label3);
            this.identityTab.Controls.Add(this.w_defaultassist1);
            this.identityTab.Controls.Add(this.label4);
            this.identityTab.Controls.Add(this.w_defaultassist2);
            this.identityTab.Controls.Add(this.label5);
            this.identityTab.Controls.Add(this.w_item1);
            this.identityTab.Controls.Add(this.w_itemc1);
            this.identityTab.Controls.Add(this.label6);
            this.identityTab.Controls.Add(this.w_item2);
            this.identityTab.Controls.Add(this.w_itemc2);
            this.identityTab.Controls.Add(this.label8);
            this.identityTab.Controls.Add(this.w_item3);
            this.identityTab.Controls.Add(this.w_itemc3);
            this.identityTab.Controls.Add(this.label7);
            this.identityTab.Controls.Add(this.w_item4);
            this.identityTab.Controls.Add(this.w_itemc4);
            this.identityTab.Controls.Add(this.setEvo1Label);
            this.identityTab.Controls.Add(this.setEvo1);
            this.identityTab.Location = new System.Drawing.Point(4, 22);
            this.identityTab.Name = "identityTab";
            this.identityTab.Size = new System.Drawing.Size(876, 562);
            this.identityTab.TabIndex = 0;
            this.identityTab.Text = "Character / costumes";
            this.identityTab.UseVisualStyleBackColor = true;
            this.identityTab.Click += new System.EventHandler(this.identityTab_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Character code";
            // 
            // w_charaprmbas
            // 
            this.w_charaprmbas.Location = new System.Drawing.Point(126, 12);
            this.w_charaprmbas.MaxLength = 8;
            this.w_charaprmbas.Name = "w_charaprmbas";
            this.w_charaprmbas.Size = new System.Drawing.Size(286, 20);
            this.w_charaprmbas.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Motion code";
            // 
            // w_characodeid
            // 
            this.w_characodeid.Location = new System.Drawing.Point(126, 38);
            this.w_characodeid.MaxLength = 8;
            this.w_characodeid.Name = "w_characodeid";
            this.w_characodeid.Size = new System.Drawing.Size(286, 20);
            this.w_characodeid.TabIndex = 3;
            this.fieldTips.SetToolTip(this.w_characodeid, "Eight-byte motion characode buffer. Used by motion-code lookup; do not treat it a" +
        "s an arbitrary variable-length string.");
            // 
            // costumesGroup
            // 
            this.costumesGroup.Controls.Add(this.costumeGrid);
            this.costumesGroup.Location = new System.Drawing.Point(432, 8);
            this.costumesGroup.Name = "costumesGroup";
            this.costumesGroup.Size = new System.Drawing.Size(432, 534);
            this.costumesGroup.TabIndex = 24;
            this.costumesGroup.TabStop = false;
            this.costumesGroup.Text = "Base costumes / awake costumes";
            // 
            // costumeGrid
            // 
            this.costumeGrid.AccessibleName = "Base and awake costume slots";
            this.costumeGrid.AllowUserToAddRows = false;
            this.costumeGrid.AllowUserToDeleteRows = false;
            this.costumeGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.costumeGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.costumeGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.costumeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.costumeSlotColumn,
            this.baseCostumeColumn,
            this.awakeCostumeColumn});
            this.costumeGrid.Location = new System.Drawing.Point(8, 22);
            this.costumeGrid.MultiSelect = false;
            this.costumeGrid.AllowUserToResizeColumns = false;
            this.costumeGrid.AllowUserToResizeRows = false;
            this.costumeGrid.AllowUserToOrderColumns = false;
            this.costumeGrid.Name = "costumeGrid";
            this.costumeGrid.RowHeadersVisible = false;
            this.costumeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.costumeGrid.Size = new System.Drawing.Size(416, 506);
            this.costumeGrid.TabIndex = 0;
            // 
            // costumeSlotColumn
            // 
            this.costumeSlotColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.costumeSlotColumn.HeaderText = "Slot";
            this.costumeSlotColumn.Name = "costumeSlotColumn";
            this.costumeSlotColumn.ReadOnly = true;
            this.costumeSlotColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.costumeSlotColumn.Width = 48;
            // 
            // baseCostumeColumn
            // 
            this.baseCostumeColumn.HeaderText = "base_costumes";
            this.baseCostumeColumn.MaxInputLength = 8;
            this.baseCostumeColumn.Name = "baseCostumeColumn";
            this.baseCostumeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // awakeCostumeColumn
            // 
            this.awakeCostumeColumn.HeaderText = "awake_costumes";
            this.awakeCostumeColumn.MaxInputLength = 8;
            this.awakeCostumeColumn.Name = "awakeCostumeColumn";
            this.awakeCostumeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Partner:";
            // 
            // w_partner
            // 
            this.w_partner.Location = new System.Drawing.Point(126, 83);
            this.w_partner.Name = "w_partner";
            this.w_partner.Size = new System.Drawing.Size(286, 20);
            this.w_partner.TabIndex = 5;
            this.fieldTips.SetToolTip(this.w_partner, "Partner characode string, resolved through the player parameter manager; not an i" +
        "nteger pointer on disk.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Support 1:";
            // 
            // w_defaultassist1
            // 
            this.w_defaultassist1.Location = new System.Drawing.Point(126, 115);
            this.w_defaultassist1.Name = "w_defaultassist1";
            this.w_defaultassist1.Size = new System.Drawing.Size(286, 20);
            this.w_defaultassist1.TabIndex = 7;
            this.fieldTips.SetToolTip(this.w_defaultassist1, "First default support characode. Used by character-select default support panels." +
        "");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Support 2:";
            // 
            // w_defaultassist2
            // 
            this.w_defaultassist2.Location = new System.Drawing.Point(126, 141);
            this.w_defaultassist2.Name = "w_defaultassist2";
            this.w_defaultassist2.Size = new System.Drawing.Size(286, 20);
            this.w_defaultassist2.TabIndex = 9;
            this.fieldTips.SetToolTip(this.w_defaultassist2, "Second default support characode. Empty strings occur.");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Item 1";
            // 
            // w_item1
            // 
            this.w_item1.Location = new System.Drawing.Point(74, 214);
            this.w_item1.Name = "w_item1";
            this.w_item1.Size = new System.Drawing.Size(260, 20);
            this.w_item1.TabIndex = 11;
            // 
            // w_itemc1
            // 
            this.w_itemc1.Location = new System.Drawing.Point(340, 214);
            this.w_itemc1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.w_itemc1.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.w_itemc1.Name = "w_itemc1";
            this.w_itemc1.Size = new System.Drawing.Size(72, 20);
            this.w_itemc1.TabIndex = 12;
            this.fieldTips.SetToolTip(this.w_itemc1, "Signed 16-bit item count.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Item 2";
            // 
            // w_item2
            // 
            this.w_item2.Location = new System.Drawing.Point(74, 242);
            this.w_item2.Name = "w_item2";
            this.w_item2.Size = new System.Drawing.Size(260, 20);
            this.w_item2.TabIndex = 14;
            // 
            // w_itemc2
            // 
            this.w_itemc2.Location = new System.Drawing.Point(340, 242);
            this.w_itemc2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.w_itemc2.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.w_itemc2.Name = "w_itemc2";
            this.w_itemc2.Size = new System.Drawing.Size(72, 20);
            this.w_itemc2.TabIndex = 15;
            this.fieldTips.SetToolTip(this.w_itemc2, "Signed 16-bit item count.");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 273);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Item 3";
            // 
            // w_item3
            // 
            this.w_item3.Location = new System.Drawing.Point(74, 270);
            this.w_item3.Name = "w_item3";
            this.w_item3.Size = new System.Drawing.Size(260, 20);
            this.w_item3.TabIndex = 17;
            // 
            // w_itemc3
            // 
            this.w_itemc3.Location = new System.Drawing.Point(340, 270);
            this.w_itemc3.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.w_itemc3.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.w_itemc3.Name = "w_itemc3";
            this.w_itemc3.Size = new System.Drawing.Size(72, 20);
            this.w_itemc3.TabIndex = 18;
            this.fieldTips.SetToolTip(this.w_itemc3, "Signed 16-bit item count.");
            this.w_itemc3.ValueChanged += new System.EventHandler(this.w_itemc3_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 301);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Item 4";
            // 
            // w_item4
            // 
            this.w_item4.Location = new System.Drawing.Point(74, 298);
            this.w_item4.Name = "w_item4";
            this.w_item4.Size = new System.Drawing.Size(260, 20);
            this.w_item4.TabIndex = 20;
            // 
            // w_itemc4
            // 
            this.w_itemc4.Location = new System.Drawing.Point(340, 298);
            this.w_itemc4.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.w_itemc4.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.w_itemc4.Name = "w_itemc4";
            this.w_itemc4.Size = new System.Drawing.Size(72, 20);
            this.w_itemc4.TabIndex = 21;
            this.fieldTips.SetToolTip(this.w_itemc4, "Signed 16-bit item count.");
            // 
            // setEvo1Label
            // 
            this.setEvo1Label.Location = new System.Drawing.Point(8, 177);
            this.setEvo1Label.Name = "setEvo1Label";
            this.setEvo1Label.Size = new System.Drawing.Size(84, 20);
            this.setEvo1Label.TabIndex = 22;
            this.setEvo1Label.Text = "Evo Duplicate";
            // 
            // setEvo1
            // 
            this.setEvo1.Hexadecimal = true;
            this.setEvo1.Location = new System.Drawing.Point(126, 175);
            this.setEvo1.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.setEvo1.Minimum = new decimal(new int[] {
            0,
            -2147483648,
            0,
            -2147483648});
            this.setEvo1.Name = "setEvo1";
            this.setEvo1.Size = new System.Drawing.Size(286, 20);
            this.setEvo1.TabIndex = 23;
            // 
            // flagsTab
            // 
            this.flagsTab.AutoScroll = true;
            this.flagsTab.Controls.Add(this.groupConditionFlags);
            this.flagsTab.Controls.Add(this.extraUnknown15CLabel);
            this.flagsTab.Controls.Add(this.extraUnknown15C);
            this.flagsTab.Controls.Add(this.setAwaBodyPriorityLabel);
            this.flagsTab.Controls.Add(this.setAwaBodyPriority);
            this.flagsTab.Controls.Add(this.setDefaultAwaSkillIndexLabel);
            this.flagsTab.Controls.Add(this.setDefaultAwaSkillIndex);
            this.flagsTab.Controls.Add(this.extraJobTypeLabel);
            this.flagsTab.Controls.Add(this.extraJobType);
            this.flagsTab.Controls.Add(this.extraUnknown16CLabel);
            this.flagsTab.Controls.Add(this.extraUnknown16C);
            this.flagsTab.Controls.Add(this.extraUnknown170Label);
            this.flagsTab.Controls.Add(this.extraUnknown170);
            this.flagsTab.Controls.Add(this.extraUnknown174Label);
            this.flagsTab.Controls.Add(this.extraUnknown174);
            this.flagsTab.Controls.Add(this.extraUnknown178Label);
            this.flagsTab.Controls.Add(this.extraUnknown178);
            this.flagsTab.Controls.Add(this.extraFemaleAnimsLabel);
            this.flagsTab.Controls.Add(this.extraFemaleAnims);
            this.flagsTab.Controls.Add(this.extraUnknown180Label);
            this.flagsTab.Controls.Add(this.extraUnknown180);
            this.flagsTab.Controls.Add(this.extraUnknown184Label);
            this.flagsTab.Controls.Add(this.extraUnknown184);
            this.flagsTab.Controls.Add(this.extraUnknown188Label);
            this.flagsTab.Controls.Add(this.extraUnknown188);
            this.flagsTab.Controls.Add(this.extraUnknown18CLabel);
            this.flagsTab.Controls.Add(this.extraUnknown18C);
            this.flagsTab.Controls.Add(this.extraUnknown190Label);
            this.flagsTab.Controls.Add(this.extraUnknown190);
            this.flagsTab.Controls.Add(this.extraUnknown194Label);
            this.flagsTab.Controls.Add(this.extraUnknown194);
            this.flagsTab.Controls.Add(this.extraUnknown198Label);
            this.flagsTab.Controls.Add(this.extraUnknown198);
            this.flagsTab.Controls.Add(this.extraUnknown19CLabel);
            this.flagsTab.Controls.Add(this.extraUnknown19C);
            this.flagsTab.Controls.Add(this.extraAdventureSupportLabel);
            this.flagsTab.Controls.Add(this.extraAdventureSupport);
            this.flagsTab.Location = new System.Drawing.Point(4, 22);
            this.flagsTab.Name = "flagsTab";
            this.flagsTab.Size = new System.Drawing.Size(876, 562);
            this.flagsTab.TabIndex = 0;
            this.flagsTab.Text = "Flag settings";
            this.flagsTab.UseVisualStyleBackColor = true;
            // 
            // groupConditionFlags
            // 
            this.groupConditionFlags.Controls.Add(this.label11);
            this.groupConditionFlags.Controls.Add(this.v_enableAwaSkill);
            this.groupConditionFlags.Controls.Add(this.checkedListConditionFlags);
            this.groupConditionFlags.Location = new System.Drawing.Point(8, 8);
            this.groupConditionFlags.Name = "groupConditionFlags";
            this.groupConditionFlags.Size = new System.Drawing.Size(362, 548);
            this.groupConditionFlags.TabIndex = 0;
            this.groupConditionFlags.TabStop = false;
            this.groupConditionFlags.Text = "Conditions (0x150)";
            this.groupConditionFlags.Enter += new System.EventHandler(this.groupConditionFlags_Enter);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Condition flags:";
            // 
            // v_enableAwaSkill
            // 
            this.v_enableAwaSkill.Hexadecimal = true;
            this.v_enableAwaSkill.Location = new System.Drawing.Point(120, 22);
            this.v_enableAwaSkill.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.v_enableAwaSkill.Name = "v_enableAwaSkill";
            this.v_enableAwaSkill.Size = new System.Drawing.Size(230, 20);
            this.v_enableAwaSkill.TabIndex = 0;
            this.fieldTips.SetToolTip(this.v_enableAwaSkill, "Combines the enabled condition flags into one value.");
            this.v_enableAwaSkill.ValueChanged += new System.EventHandler(this.v_enableAwaSkill_ValueChanged);
            // 
            // checkedListConditionFlags
            // 
            this.checkedListConditionFlags.CheckOnClick = true;
            this.checkedListConditionFlags.Location = new System.Drawing.Point(8, 52);
            this.checkedListConditionFlags.Name = "checkedListConditionFlags";
            this.checkedListConditionFlags.Size = new System.Drawing.Size(344, 484);
            this.checkedListConditionFlags.TabIndex = 2;
            this.checkedListConditionFlags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListConditionFlags_ItemCheck);
            this.checkedListConditionFlags.SelectedIndexChanged += new System.EventHandler(this.checkedListConditionFlags_SelectedIndexChanged);
            // 
            // extraUnknown15CLabel
            // 
            this.extraUnknown15CLabel.Location = new System.Drawing.Point(382, 13);
            this.extraUnknown15CLabel.Name = "extraUnknown15CLabel";
            this.extraUnknown15CLabel.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown15CLabel.TabIndex = 0;
            this.extraUnknown15CLabel.Text = "Unknown 15C";
            this.fieldTips.SetToolTip(this.extraUnknown15CLabel, resources.GetString("extraUnknown15CLabel.ToolTip"));
            // 
            // extraUnknown15C
            // 
            this.extraUnknown15C.Location = new System.Drawing.Point(606, 10);
            this.extraUnknown15C.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown15C.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown15C.Name = "extraUnknown15C";
            this.extraUnknown15C.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown15C.TabIndex = 1;
            this.fieldTips.SetToolTip(this.extraUnknown15C, resources.GetString("extraUnknown15C.ToolTip"));
            // 
            // setAwaBodyPriorityLabel
            // 
            this.setAwaBodyPriorityLabel.Location = new System.Drawing.Point(382, 42);
            this.setAwaBodyPriorityLabel.Name = "setAwaBodyPriorityLabel";
            this.setAwaBodyPriorityLabel.Size = new System.Drawing.Size(216, 20);
            this.setAwaBodyPriorityLabel.TabIndex = 2;
            this.setAwaBodyPriorityLabel.Text = "Awakening Armor";
            // 
            // setAwaBodyPriority
            // 
            this.setAwaBodyPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setAwaBodyPriority.DropDownWidth = 380;
            this.setAwaBodyPriority.Location = new System.Drawing.Point(606, 39);
            this.setAwaBodyPriority.Name = "setAwaBodyPriority";
            this.setAwaBodyPriority.Size = new System.Drawing.Size(256, 21);
            this.setAwaBodyPriority.TabIndex = 3;
            this.fieldTips.SetToolTip(this.setAwaBodyPriority, "Positive values replace awakening attack/body priority on awakening entry; ordina" +
        "ry priority is restored on exit.");
            // 
            // setDefaultAwaSkillIndexLabel
            // 
            this.setDefaultAwaSkillIndexLabel.Location = new System.Drawing.Point(382, 71);
            this.setDefaultAwaSkillIndexLabel.Name = "setDefaultAwaSkillIndexLabel";
            this.setDefaultAwaSkillIndexLabel.Size = new System.Drawing.Size(216, 20);
            this.setDefaultAwaSkillIndexLabel.TabIndex = 4;
            this.setDefaultAwaSkillIndexLabel.Text = "Awakening skill index";
            // 
            // setDefaultAwaSkillIndex
            // 
            this.setDefaultAwaSkillIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setDefaultAwaSkillIndex.DropDownWidth = 380;
            this.setDefaultAwaSkillIndex.Location = new System.Drawing.Point(606, 68);
            this.setDefaultAwaSkillIndex.Name = "setDefaultAwaSkillIndex";
            this.setDefaultAwaSkillIndex.Size = new System.Drawing.Size(256, 21);
            this.setDefaultAwaSkillIndex.TabIndex = 5;
            this.fieldTips.SetToolTip(this.setDefaultAwaSkillIndex, "Default awakening skill number; separate from flag bit 24. The helper has charact" +
        "er/state-dependent paths.");
            // 
            // extraJobTypeLabel
            // 
            this.extraJobTypeLabel.Location = new System.Drawing.Point(382, 100);
            this.extraJobTypeLabel.Name = "extraJobTypeLabel";
            this.extraJobTypeLabel.Size = new System.Drawing.Size(216, 20);
            this.extraJobTypeLabel.TabIndex = 6;
            this.extraJobTypeLabel.Text = "Command menu type";
            this.fieldTips.SetToolTip(this.extraJobTypeLabel, resources.GetString("extraJobTypeLabel.ToolTip"));
            // 
            // extraJobType
            // 
            this.extraJobType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extraJobType.DropDownWidth = 380;
            this.extraJobType.Location = new System.Drawing.Point(606, 97);
            this.extraJobType.Name = "extraJobType";
            this.extraJobType.Size = new System.Drawing.Size(256, 21);
            this.extraJobType.TabIndex = 7;
            this.fieldTips.SetToolTip(this.extraJobType, resources.GetString("extraJobType.ToolTip"));
            // 
            // extraUnknown16CLabel
            // 
            this.extraUnknown16CLabel.Location = new System.Drawing.Point(382, 129);
            this.extraUnknown16CLabel.Name = "extraUnknown16CLabel";
            this.extraUnknown16CLabel.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown16CLabel.TabIndex = 8;
            this.extraUnknown16CLabel.Text = "Unknown 16C";
            this.fieldTips.SetToolTip(this.extraUnknown16CLabel, resources.GetString("extraUnknown16CLabel.ToolTip"));
            // 
            // extraUnknown16C
            // 
            this.extraUnknown16C.Location = new System.Drawing.Point(606, 126);
            this.extraUnknown16C.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown16C.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown16C.Name = "extraUnknown16C";
            this.extraUnknown16C.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown16C.TabIndex = 9;
            this.fieldTips.SetToolTip(this.extraUnknown16C, resources.GetString("extraUnknown16C.ToolTip"));
            // 
            // extraUnknown170Label
            // 
            this.extraUnknown170Label.Location = new System.Drawing.Point(382, 158);
            this.extraUnknown170Label.Name = "extraUnknown170Label";
            this.extraUnknown170Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown170Label.TabIndex = 10;
            this.extraUnknown170Label.Text = "Unknown 170";
            this.fieldTips.SetToolTip(this.extraUnknown170Label, resources.GetString("extraUnknown170Label.ToolTip"));
            // 
            // extraUnknown170
            // 
            this.extraUnknown170.Location = new System.Drawing.Point(606, 155);
            this.extraUnknown170.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown170.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown170.Name = "extraUnknown170";
            this.extraUnknown170.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown170.TabIndex = 11;
            this.fieldTips.SetToolTip(this.extraUnknown170, resources.GetString("extraUnknown170.ToolTip"));
            // 
            // extraUnknown174Label
            // 
            this.extraUnknown174Label.Location = new System.Drawing.Point(382, 187);
            this.extraUnknown174Label.Name = "extraUnknown174Label";
            this.extraUnknown174Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown174Label.TabIndex = 12;
            this.extraUnknown174Label.Text = "Unknown 174";
            this.fieldTips.SetToolTip(this.extraUnknown174Label, resources.GetString("extraUnknown174Label.ToolTip"));
            // 
            // extraUnknown174
            // 
            this.extraUnknown174.Location = new System.Drawing.Point(606, 184);
            this.extraUnknown174.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown174.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown174.Name = "extraUnknown174";
            this.extraUnknown174.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown174.TabIndex = 13;
            this.fieldTips.SetToolTip(this.extraUnknown174, resources.GetString("extraUnknown174.ToolTip"));
            // 
            // extraUnknown178Label
            // 
            this.extraUnknown178Label.Location = new System.Drawing.Point(382, 216);
            this.extraUnknown178Label.Name = "extraUnknown178Label";
            this.extraUnknown178Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown178Label.TabIndex = 14;
            this.extraUnknown178Label.Text = "Unknown 178";
            this.fieldTips.SetToolTip(this.extraUnknown178Label, resources.GetString("extraUnknown178Label.ToolTip"));
            // 
            // extraUnknown178
            // 
            this.extraUnknown178.Location = new System.Drawing.Point(606, 213);
            this.extraUnknown178.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown178.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown178.Name = "extraUnknown178";
            this.extraUnknown178.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown178.TabIndex = 15;
            this.fieldTips.SetToolTip(this.extraUnknown178, resources.GetString("extraUnknown178.ToolTip"));
            // 
            // extraFemaleAnimsLabel
            // 
            this.extraFemaleAnimsLabel.Location = new System.Drawing.Point(382, 245);
            this.extraFemaleAnimsLabel.Name = "extraFemaleAnimsLabel";
            this.extraFemaleAnimsLabel.Size = new System.Drawing.Size(216, 20);
            this.extraFemaleAnimsLabel.TabIndex = 16;
            this.extraFemaleAnimsLabel.Text = "Damage animation style";
            this.fieldTips.SetToolTip(this.extraFemaleAnimsLabel, "Exactly 1 invokes ConvertWomanAnm for selected damage and support-rescue animatio" +
        "n IDs; other values leave those IDs unchanged.");
            // 
            // extraFemaleAnims
            // 
            this.extraFemaleAnims.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extraFemaleAnims.DropDownWidth = 380;
            this.extraFemaleAnims.Location = new System.Drawing.Point(606, 242);
            this.extraFemaleAnims.Name = "extraFemaleAnims";
            this.extraFemaleAnims.Size = new System.Drawing.Size(256, 21);
            this.extraFemaleAnims.TabIndex = 17;
            this.fieldTips.SetToolTip(this.extraFemaleAnims, "Exactly 1 invokes ConvertWomanAnm for selected damage and support-rescue animatio" +
        "n IDs; other values leave those IDs unchanged.");
            // 
            // extraUnknown180Label
            // 
            this.extraUnknown180Label.Location = new System.Drawing.Point(382, 274);
            this.extraUnknown180Label.Name = "extraUnknown180Label";
            this.extraUnknown180Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown180Label.TabIndex = 18;
            this.extraUnknown180Label.Text = "Unknown 180";
            this.fieldTips.SetToolTip(this.extraUnknown180Label, resources.GetString("extraUnknown180Label.ToolTip"));
            // 
            // extraUnknown180
            // 
            this.extraUnknown180.Location = new System.Drawing.Point(606, 271);
            this.extraUnknown180.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown180.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown180.Name = "extraUnknown180";
            this.extraUnknown180.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown180.TabIndex = 19;
            this.fieldTips.SetToolTip(this.extraUnknown180, resources.GetString("extraUnknown180.ToolTip"));
            // 
            // extraUnknown184Label
            // 
            this.extraUnknown184Label.Location = new System.Drawing.Point(382, 303);
            this.extraUnknown184Label.Name = "extraUnknown184Label";
            this.extraUnknown184Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown184Label.TabIndex = 20;
            this.extraUnknown184Label.Text = "Unknown 184";
            this.fieldTips.SetToolTip(this.extraUnknown184Label, resources.GetString("extraUnknown184Label.ToolTip"));
            // 
            // extraUnknown184
            // 
            this.extraUnknown184.Location = new System.Drawing.Point(606, 300);
            this.extraUnknown184.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown184.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown184.Name = "extraUnknown184";
            this.extraUnknown184.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown184.TabIndex = 21;
            this.fieldTips.SetToolTip(this.extraUnknown184, resources.GetString("extraUnknown184.ToolTip"));
            // 
            // extraUnknown188Label
            // 
            this.extraUnknown188Label.Location = new System.Drawing.Point(382, 332);
            this.extraUnknown188Label.Name = "extraUnknown188Label";
            this.extraUnknown188Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown188Label.TabIndex = 22;
            this.extraUnknown188Label.Text = "Unknown 188";
            this.fieldTips.SetToolTip(this.extraUnknown188Label, resources.GetString("extraUnknown188Label.ToolTip"));
            // 
            // extraUnknown188
            // 
            this.extraUnknown188.Location = new System.Drawing.Point(606, 329);
            this.extraUnknown188.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown188.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown188.Name = "extraUnknown188";
            this.extraUnknown188.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown188.TabIndex = 23;
            this.fieldTips.SetToolTip(this.extraUnknown188, resources.GetString("extraUnknown188.ToolTip"));
            // 
            // extraUnknown18CLabel
            // 
            this.extraUnknown18CLabel.Location = new System.Drawing.Point(382, 361);
            this.extraUnknown18CLabel.Name = "extraUnknown18CLabel";
            this.extraUnknown18CLabel.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown18CLabel.TabIndex = 24;
            this.extraUnknown18CLabel.Text = "Unknown 18C";
            this.fieldTips.SetToolTip(this.extraUnknown18CLabel, resources.GetString("extraUnknown18CLabel.ToolTip"));
            // 
            // extraUnknown18C
            // 
            this.extraUnknown18C.Location = new System.Drawing.Point(606, 358);
            this.extraUnknown18C.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown18C.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown18C.Name = "extraUnknown18C";
            this.extraUnknown18C.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown18C.TabIndex = 25;
            this.fieldTips.SetToolTip(this.extraUnknown18C, resources.GetString("extraUnknown18C.ToolTip"));
            // 
            // extraUnknown190Label
            // 
            this.extraUnknown190Label.Location = new System.Drawing.Point(382, 390);
            this.extraUnknown190Label.Name = "extraUnknown190Label";
            this.extraUnknown190Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown190Label.TabIndex = 26;
            this.extraUnknown190Label.Text = "Unknown 190";
            this.fieldTips.SetToolTip(this.extraUnknown190Label, resources.GetString("extraUnknown190Label.ToolTip"));
            // 
            // extraUnknown190
            // 
            this.extraUnknown190.Location = new System.Drawing.Point(606, 387);
            this.extraUnknown190.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown190.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown190.Name = "extraUnknown190";
            this.extraUnknown190.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown190.TabIndex = 27;
            this.fieldTips.SetToolTip(this.extraUnknown190, resources.GetString("extraUnknown190.ToolTip"));
            // 
            // extraUnknown194Label
            // 
            this.extraUnknown194Label.Location = new System.Drawing.Point(382, 419);
            this.extraUnknown194Label.Name = "extraUnknown194Label";
            this.extraUnknown194Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown194Label.TabIndex = 28;
            this.extraUnknown194Label.Text = "Unknown 194";
            this.fieldTips.SetToolTip(this.extraUnknown194Label, resources.GetString("extraUnknown194Label.ToolTip"));
            // 
            // extraUnknown194
            // 
            this.extraUnknown194.Location = new System.Drawing.Point(606, 416);
            this.extraUnknown194.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown194.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown194.Name = "extraUnknown194";
            this.extraUnknown194.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown194.TabIndex = 29;
            this.fieldTips.SetToolTip(this.extraUnknown194, resources.GetString("extraUnknown194.ToolTip"));
            // 
            // extraUnknown198Label
            // 
            this.extraUnknown198Label.Location = new System.Drawing.Point(382, 448);
            this.extraUnknown198Label.Name = "extraUnknown198Label";
            this.extraUnknown198Label.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown198Label.TabIndex = 30;
            this.extraUnknown198Label.Text = "Unknown 198";
            this.fieldTips.SetToolTip(this.extraUnknown198Label, resources.GetString("extraUnknown198Label.ToolTip"));
            // 
            // extraUnknown198
            // 
            this.extraUnknown198.Location = new System.Drawing.Point(606, 445);
            this.extraUnknown198.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown198.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown198.Name = "extraUnknown198";
            this.extraUnknown198.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown198.TabIndex = 31;
            this.fieldTips.SetToolTip(this.extraUnknown198, resources.GetString("extraUnknown198.ToolTip"));
            // 
            // extraUnknown19CLabel
            // 
            this.extraUnknown19CLabel.Location = new System.Drawing.Point(382, 477);
            this.extraUnknown19CLabel.Name = "extraUnknown19CLabel";
            this.extraUnknown19CLabel.Size = new System.Drawing.Size(216, 20);
            this.extraUnknown19CLabel.TabIndex = 32;
            this.extraUnknown19CLabel.Text = "Unknown 19C";
            this.fieldTips.SetToolTip(this.extraUnknown19CLabel, resources.GetString("extraUnknown19CLabel.ToolTip"));
            // 
            // extraUnknown19C
            // 
            this.extraUnknown19C.Location = new System.Drawing.Point(606, 474);
            this.extraUnknown19C.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.extraUnknown19C.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.extraUnknown19C.Name = "extraUnknown19C";
            this.extraUnknown19C.Size = new System.Drawing.Size(256, 20);
            this.extraUnknown19C.TabIndex = 33;
            this.fieldTips.SetToolTip(this.extraUnknown19C, resources.GetString("extraUnknown19C.ToolTip"));
            // 
            // extraAdventureSupportLabel
            // 
            this.extraAdventureSupportLabel.Location = new System.Drawing.Point(382, 506);
            this.extraAdventureSupportLabel.Name = "extraAdventureSupportLabel";
            this.extraAdventureSupportLabel.Size = new System.Drawing.Size(216, 20);
            this.extraAdventureSupportLabel.TabIndex = 34;
            this.extraAdventureSupportLabel.Text = "Adventure default support type";
            this.fieldTips.SetToolTip(this.extraAdventureSupportLabel, resources.GetString("extraAdventureSupportLabel.ToolTip"));
            // 
            // extraAdventureSupport
            // 
            this.extraAdventureSupport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extraAdventureSupport.DropDownWidth = 380;
            this.extraAdventureSupport.Location = new System.Drawing.Point(606, 503);
            this.extraAdventureSupport.Name = "extraAdventureSupport";
            this.extraAdventureSupport.Size = new System.Drawing.Size(256, 21);
            this.extraAdventureSupport.TabIndex = 35;
            this.fieldTips.SetToolTip(this.extraAdventureSupport, resources.GetString("extraAdventureSupport.ToolTip"));
            // 
            // bodyTab
            // 
            this.bodyTab.AutoScroll = true;
            this.bodyTab.Controls.Add(this.setCameraDistanceLabel);
            this.bodyTab.Controls.Add(this.setCameraDistance);
            this.bodyTab.Controls.Add(this.setCameraUnknown1Label);
            this.bodyTab.Controls.Add(this.setCameraUnknown1);
            this.bodyTab.Controls.Add(this.setVictoryCameraAngleLabel);
            this.bodyTab.Controls.Add(this.setVictoryCameraAngle);
            this.bodyTab.Controls.Add(this.setCameraUnknown2Label);
            this.bodyTab.Controls.Add(this.setCameraUnknown2);
            this.bodyTab.Controls.Add(this.setCameraUnknown3Label);
            this.bodyTab.Controls.Add(this.setCameraUnknown3);
            this.bodyTab.Controls.Add(this.setCameraUnknown4Label);
            this.bodyTab.Controls.Add(this.setCameraUnknown4);
            this.bodyTab.Location = new System.Drawing.Point(4, 22);
            this.bodyTab.Name = "bodyTab";
            this.bodyTab.Size = new System.Drawing.Size(876, 562);
            this.bodyTab.TabIndex = 0;
            this.bodyTab.Text = "Body settings";
            this.bodyTab.UseVisualStyleBackColor = true;
            // 
            // setCameraDistanceLabel
            // 
            this.setCameraDistanceLabel.Location = new System.Drawing.Point(12, 19);
            this.setCameraDistanceLabel.Name = "setCameraDistanceLabel";
            this.setCameraDistanceLabel.Size = new System.Drawing.Size(230, 20);
            this.setCameraDistanceLabel.TabIndex = 0;
            this.setCameraDistanceLabel.Text = "Default Camera Height ";
            // 
            // setCameraDistance
            // 
            this.setCameraDistance.Location = new System.Drawing.Point(250, 16);
            this.setCameraDistance.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setCameraDistance.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setCameraDistance.Name = "setCameraDistance";
            this.setCameraDistance.Size = new System.Drawing.Size(164, 20);
            this.setCameraDistance.TabIndex = 1;
            this.fieldTips.SetToolTip(this.setCameraDistance, "Signed 16-bit height converted to float by GetHeight. Used in collision/camera/ef" +
        "fect positioning; not a dedicated victory-camera distance.");
            // 
            // setCameraUnknown1Label
            // 
            this.setCameraUnknown1Label.Location = new System.Drawing.Point(12, 51);
            this.setCameraUnknown1Label.Name = "setCameraUnknown1Label";
            this.setCameraUnknown1Label.Size = new System.Drawing.Size(230, 20);
            this.setCameraUnknown1Label.TabIndex = 2;
            this.setCameraUnknown1Label.Text = "Unknown 1B6";
            // 
            // setCameraUnknown1
            // 
            this.setCameraUnknown1.Location = new System.Drawing.Point(250, 48);
            this.setCameraUnknown1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setCameraUnknown1.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setCameraUnknown1.Name = "setCameraUnknown1";
            this.setCameraUnknown1.Size = new System.Drawing.Size(164, 20);
            this.setCameraUnknown1.TabIndex = 3;
            this.fieldTips.SetToolTip(this.setCameraUnknown1, resources.GetString("setCameraUnknown1.ToolTip"));
            // 
            // setVictoryCameraAngleLabel
            // 
            this.setVictoryCameraAngleLabel.Location = new System.Drawing.Point(12, 83);
            this.setVictoryCameraAngleLabel.Name = "setVictoryCameraAngleLabel";
            this.setVictoryCameraAngleLabel.Size = new System.Drawing.Size(230, 20);
            this.setVictoryCameraAngleLabel.TabIndex = 4;
            this.setVictoryCameraAngleLabel.Text = "Victory Camera height";
            // 
            // setVictoryCameraAngle
            // 
            this.setVictoryCameraAngle.Location = new System.Drawing.Point(250, 80);
            this.setVictoryCameraAngle.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setVictoryCameraAngle.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setVictoryCameraAngle.Name = "setVictoryCameraAngle";
            this.setVictoryCameraAngle.Size = new System.Drawing.Size(164, 20);
            this.setVictoryCameraAngle.TabIndex = 5;
            this.fieldTips.SetToolTip(this.setVictoryCameraAngle, "Signed 16-bit value converted to float, cached by the end-demo controller, then u" +
        "sed as a vertical target offset and in camera-distance calculations. It is a fra" +
        "ming height, not an angle.");
            // 
            // setCameraUnknown2Label
            // 
            this.setCameraUnknown2Label.Location = new System.Drawing.Point(444, 19);
            this.setCameraUnknown2Label.Name = "setCameraUnknown2Label";
            this.setCameraUnknown2Label.Size = new System.Drawing.Size(230, 20);
            this.setCameraUnknown2Label.TabIndex = 6;
            this.setCameraUnknown2Label.Text = "Body radius";
            // 
            // setCameraUnknown2
            // 
            this.setCameraUnknown2.Location = new System.Drawing.Point(682, 16);
            this.setCameraUnknown2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setCameraUnknown2.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setCameraUnknown2.Name = "setCameraUnknown2";
            this.setCameraUnknown2.Size = new System.Drawing.Size(164, 20);
            this.setCameraUnknown2.TabIndex = 7;
            this.fieldTips.SetToolTip(this.setCameraUnknown2, "Radius passed to the body collision sphere and used by smash-hit updates.");
            // 
            // setCameraUnknown3Label
            // 
            this.setCameraUnknown3Label.Location = new System.Drawing.Point(444, 51);
            this.setCameraUnknown3Label.Name = "setCameraUnknown3Label";
            this.setCameraUnknown3Label.Size = new System.Drawing.Size(230, 20);
            this.setCameraUnknown3Label.TabIndex = 8;
            this.setCameraUnknown3Label.Text = "Damage / atemi radius";
            // 
            // setCameraUnknown3
            // 
            this.setCameraUnknown3.Location = new System.Drawing.Point(682, 48);
            this.setCameraUnknown3.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setCameraUnknown3.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setCameraUnknown3.Name = "setCameraUnknown3";
            this.setCameraUnknown3.Size = new System.Drawing.Size(164, 20);
            this.setCameraUnknown3.TabIndex = 9;
            this.fieldTips.SetToolTip(this.setCameraUnknown3, "Radius used for both the damage and atemi collision-sphere groups (three spheres " +
        "in each reset path).");
            // 
            // setCameraUnknown4Label
            // 
            this.setCameraUnknown4Label.Location = new System.Drawing.Point(444, 83);
            this.setCameraUnknown4Label.Name = "setCameraUnknown4Label";
            this.setCameraUnknown4Label.Size = new System.Drawing.Size(230, 20);
            this.setCameraUnknown4Label.TabIndex = 10;
            this.setCameraUnknown4Label.Text = "Parameter version";
            // 
            // setCameraUnknown4
            // 
            this.setCameraUnknown4.Enabled = false;
            this.setCameraUnknown4.Location = new System.Drawing.Point(682, 80);
            this.setCameraUnknown4.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setCameraUnknown4.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setCameraUnknown4.Name = "setCameraUnknown4";
            this.setCameraUnknown4.Size = new System.Drawing.Size(164, 20);
            this.setCameraUnknown4.TabIndex = 11;
            this.fieldTips.SetToolTip(this.setCameraUnknown4, resources.GetString("setCameraUnknown4.ToolTip"));
            // 
            // baseTab
            // 
            this.baseTab.AutoScroll = true;
            this.baseTab.Controls.Add(this.setBaseMovementLabel);
            this.baseTab.Controls.Add(this.setBaseMovement);
            this.baseTab.Controls.Add(this.setBaseChakraDashLabel);
            this.baseTab.Controls.Add(this.setBaseChakraDash);
            this.baseTab.Controls.Add(this.setGuardPressureLabel);
            this.baseTab.Controls.Add(this.setGuardPressure);
            this.baseTab.Controls.Add(this.extraUnknown1CCLabel);
            this.baseTab.Controls.Add(this.extraUnknown1CC);
            this.baseTab.Controls.Add(this.setAttackLabel);
            this.baseTab.Controls.Add(this.setAttack);
            this.baseTab.Controls.Add(this.setDefenseLabel);
            this.baseTab.Controls.Add(this.setDefense);
            this.baseTab.Controls.Add(this.setAssistDamageLabel);
            this.baseTab.Controls.Add(this.setAssistDamage);
            this.baseTab.Controls.Add(this.setItemBuffDurationLabel);
            this.baseTab.Controls.Add(this.setItemBuffDuration);
            this.baseTab.Controls.Add(this.setChakraChargeLabel);
            this.baseTab.Controls.Add(this.setChakraCharge);
            this.baseTab.Controls.Add(this.label2);
            this.baseTab.Controls.Add(this.w_awkaction);
            this.baseTab.Controls.Add(this.setAwakeHpRequirementLabel);
            this.baseTab.Controls.Add(this.setAwakeHpRequirement);
            this.baseTab.Controls.Add(this.setBaseNinjaDashLabel);
            this.baseTab.Controls.Add(this.setBaseNinjaDash);
            this.baseTab.Controls.Add(this.setBaseAirDashDurationLabel);
            this.baseTab.Controls.Add(this.setBaseAirDashDuration);
            this.baseTab.Controls.Add(this.setBaseGroundedChakraDashDurationLabel);
            this.baseTab.Controls.Add(this.setBaseGroundedChakraDashDuration);
            this.baseTab.Controls.Add(this.extraDashTrackTimeLabel);
            this.baseTab.Controls.Add(this.extraDashTrackTime);
            this.baseTab.Controls.Add(this.extraDashTurnRateLabel);
            this.baseTab.Controls.Add(this.extraDashTurnRate);
            this.baseTab.Location = new System.Drawing.Point(4, 22);
            this.baseTab.Name = "baseTab";
            this.baseTab.Size = new System.Drawing.Size(876, 562);
            this.baseTab.TabIndex = 0;
            this.baseTab.Text = "Base settings";
            this.baseTab.UseVisualStyleBackColor = true;
            // 
            // setBaseMovementLabel
            // 
            this.setBaseMovementLabel.Location = new System.Drawing.Point(12, 19);
            this.setBaseMovementLabel.Name = "setBaseMovementLabel";
            this.setBaseMovementLabel.Size = new System.Drawing.Size(230, 20);
            this.setBaseMovementLabel.TabIndex = 0;
            this.setBaseMovementLabel.Text = "Run speed";
            // 
            // setBaseMovement
            // 
            this.setBaseMovement.DecimalPlaces = 5;
            this.setBaseMovement.Location = new System.Drawing.Point(250, 16);
            this.setBaseMovement.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setBaseMovement.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setBaseMovement.Name = "setBaseMovement";
            this.setBaseMovement.Size = new System.Drawing.Size(164, 20);
            this.setBaseMovement.TabIndex = 1;
            this.fieldTips.SetToolTip(this.setBaseMovement, "Base forward speed. Loader multiplies file value by defaultFPS/currentFPS. Used o" +
        "utside true awakening, including instant awakening.");
            // 
            // setBaseChakraDashLabel
            // 
            this.setBaseChakraDashLabel.Location = new System.Drawing.Point(12, 49);
            this.setBaseChakraDashLabel.Name = "setBaseChakraDashLabel";
            this.setBaseChakraDashLabel.Size = new System.Drawing.Size(230, 20);
            this.setBaseChakraDashLabel.TabIndex = 2;
            this.setBaseChakraDashLabel.Text = "Chakra dash speed";
            // 
            // setBaseChakraDash
            // 
            this.setBaseChakraDash.DecimalPlaces = 5;
            this.setBaseChakraDash.Location = new System.Drawing.Point(250, 46);
            this.setBaseChakraDash.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setBaseChakraDash.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setBaseChakraDash.Name = "setBaseChakraDash";
            this.setBaseChakraDash.Size = new System.Drawing.Size(164, 20);
            this.setBaseChakraDash.TabIndex = 3;
            this.fieldTips.SetToolTip(this.setBaseChakraDash, "Move parameter 2, base state; loader applies defaultFPS/currentFPS. Runtime appli" +
        "es condition/charge modifiers and possible overrides.");
            // 
            // setGuardPressureLabel
            // 
            this.setGuardPressureLabel.Location = new System.Drawing.Point(12, 79);
            this.setGuardPressureLabel.Name = "setGuardPressureLabel";
            this.setGuardPressureLabel.Size = new System.Drawing.Size(230, 20);
            this.setGuardPressureLabel.TabIndex = 4;
            this.setGuardPressureLabel.Text = "Guard damage multiplier";
            // 
            // setGuardPressure
            // 
            this.setGuardPressure.DecimalPlaces = 5;
            this.setGuardPressure.Location = new System.Drawing.Point(250, 76);
            this.setGuardPressure.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setGuardPressure.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setGuardPressure.Name = "setGuardPressure";
            this.setGuardPressure.Size = new System.Drawing.Size(164, 20);
            this.setGuardPressure.TabIndex = 5;
            this.fieldTips.SetToolTip(this.setGuardPressure, "Multiplier in GetGuardScrapeRate, combined with the applicable condition percenta" +
        "ge.");
            // 
            // extraUnknown1CCLabel
            // 
            this.extraUnknown1CCLabel.Location = new System.Drawing.Point(12, 109);
            this.extraUnknown1CCLabel.Name = "extraUnknown1CCLabel";
            this.extraUnknown1CCLabel.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown1CCLabel.TabIndex = 6;
            this.extraUnknown1CCLabel.Text = "Guard rate (legacy; S4 unverified)";
            this.fieldTips.SetToolTip(this.extraUnknown1CCLabel, resources.GetString("extraUnknown1CCLabel.ToolTip"));
            // 
            // extraUnknown1CC
            // 
            this.extraUnknown1CC.DecimalPlaces = 6;
            this.extraUnknown1CC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown1CC.Location = new System.Drawing.Point(250, 106);
            this.extraUnknown1CC.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown1CC.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown1CC.Name = "extraUnknown1CC";
            this.extraUnknown1CC.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown1CC.TabIndex = 7;
            this.fieldTips.SetToolTip(this.extraUnknown1CC, resources.GetString("extraUnknown1CC.ToolTip"));
            // 
            // setAttackLabel
            // 
            this.setAttackLabel.Location = new System.Drawing.Point(12, 139);
            this.setAttackLabel.Name = "setAttackLabel";
            this.setAttackLabel.Size = new System.Drawing.Size(230, 20);
            this.setAttackLabel.TabIndex = 8;
            this.setAttackLabel.Text = "Attack";
            // 
            // setAttack
            // 
            this.setAttack.DecimalPlaces = 5;
            this.setAttack.Location = new System.Drawing.Point(250, 136);
            this.setAttack.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setAttack.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setAttack.Name = "setAttack";
            this.setAttack.Size = new System.Drawing.Size(164, 20);
            this.setAttack.TabIndex = 9;
            this.fieldTips.SetToolTip(this.setAttack, "Base multiplier used by condition-derived attack calculations; a caller argument " +
        "can bypass the base multiplier.");
            // 
            // setDefenseLabel
            // 
            this.setDefenseLabel.Location = new System.Drawing.Point(12, 169);
            this.setDefenseLabel.Name = "setDefenseLabel";
            this.setDefenseLabel.Size = new System.Drawing.Size(230, 20);
            this.setDefenseLabel.TabIndex = 10;
            this.setDefenseLabel.Text = "Defense";
            // 
            // setDefense
            // 
            this.setDefense.DecimalPlaces = 5;
            this.setDefense.Location = new System.Drawing.Point(250, 166);
            this.setDefense.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setDefense.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setDefense.Name = "setDefense";
            this.setDefense.Size = new System.Drawing.Size(164, 20);
            this.setDefense.TabIndex = 11;
            this.fieldTips.SetToolTip(this.setDefense, "Returned as the base defense-rate value by a basic-parameter getter. The final da" +
        "mage formula has additional stages.");
            // 
            // setAssistDamageLabel
            // 
            this.setAssistDamageLabel.Location = new System.Drawing.Point(12, 199);
            this.setAssistDamageLabel.Name = "setAssistDamageLabel";
            this.setAssistDamageLabel.Size = new System.Drawing.Size(230, 20);
            this.setAssistDamageLabel.TabIndex = 12;
            this.setAssistDamageLabel.Text = "Support damage";
            // 
            // setAssistDamage
            // 
            this.setAssistDamage.DecimalPlaces = 5;
            this.setAssistDamage.Location = new System.Drawing.Point(250, 196);
            this.setAssistDamage.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setAssistDamage.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setAssistDamage.Name = "setAssistDamage";
            this.setAssistDamage.Size = new System.Drawing.Size(164, 20);
            this.setAssistDamage.TabIndex = 13;
            this.fieldTips.SetToolTip(this.setAssistDamage, "Support attack multiplier; getter defaults to 1.0 if no parameter record exists.");
            // 
            // setItemBuffDurationLabel
            // 
            this.setItemBuffDurationLabel.Location = new System.Drawing.Point(12, 229);
            this.setItemBuffDurationLabel.Name = "setItemBuffDurationLabel";
            this.setItemBuffDurationLabel.Size = new System.Drawing.Size(230, 20);
            this.setItemBuffDurationLabel.TabIndex = 14;
            this.setItemBuffDurationLabel.Text = "Condition duration";
            // 
            // setItemBuffDuration
            // 
            this.setItemBuffDuration.DecimalPlaces = 5;
            this.setItemBuffDuration.Location = new System.Drawing.Point(250, 226);
            this.setItemBuffDuration.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setItemBuffDuration.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setItemBuffDuration.Name = "setItemBuffDuration";
            this.setItemBuffDuration.Size = new System.Drawing.Size(164, 20);
            this.setItemBuffDuration.TabIndex = 15;
            this.fieldTips.SetToolTip(this.setItemBuffDuration, resources.GetString("setItemBuffDuration.ToolTip"));
            // 
            // setChakraChargeLabel
            // 
            this.setChakraChargeLabel.Location = new System.Drawing.Point(444, 19);
            this.setChakraChargeLabel.Name = "setChakraChargeLabel";
            this.setChakraChargeLabel.Size = new System.Drawing.Size(230, 20);
            this.setChakraChargeLabel.TabIndex = 16;
            this.setChakraChargeLabel.Text = "Chakra charge rate";
            // 
            // setChakraCharge
            // 
            this.setChakraCharge.DecimalPlaces = 5;
            this.setChakraCharge.Location = new System.Drawing.Point(682, 16);
            this.setChakraCharge.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setChakraCharge.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setChakraCharge.Name = "setChakraCharge";
            this.setChakraCharge.Size = new System.Drawing.Size(164, 20);
            this.setChakraCharge.TabIndex = 17;
            this.fieldTips.SetToolTip(this.setChakraCharge, "Scales chakra-charge recovery along with a global charge setting, player speed ra" +
        "te and FPS conversion.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(444, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Awakening condition";
            // 
            // w_awkaction
            // 
            this.w_awkaction.Location = new System.Drawing.Point(682, 46);
            this.w_awkaction.Name = "w_awkaction";
            this.w_awkaction.Size = new System.Drawing.Size(164, 20);
            this.w_awkaction.TabIndex = 19;
            this.fieldTips.SetToolTip(this.w_awkaction, "16-byte condition-name buffer resolved by GetConditionId. Special character/costu" +
        "me and alternate-awakening paths can override it.");
            // 
            // setAwakeHpRequirementLabel
            // 
            this.setAwakeHpRequirementLabel.Location = new System.Drawing.Point(444, 79);
            this.setAwakeHpRequirementLabel.Name = "setAwakeHpRequirementLabel";
            this.setAwakeHpRequirementLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeHpRequirementLabel.TabIndex = 20;
            this.setAwakeHpRequirementLabel.Text = "Awakening HP threshold";
            // 
            // setAwakeHpRequirement
            // 
            this.setAwakeHpRequirement.DecimalPlaces = 5;
            this.setAwakeHpRequirement.Location = new System.Drawing.Point(682, 76);
            this.setAwakeHpRequirement.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setAwakeHpRequirement.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setAwakeHpRequirement.Name = "setAwakeHpRequirement";
            this.setAwakeHpRequirement.Size = new System.Drawing.Size(164, 20);
            this.setAwakeHpRequirement.TabIndex = 21;
            this.fieldTips.SetToolTip(this.setAwakeHpRequirement, "Health threshold for starting awakening: IsCanStartAwakeMode requires GetLife() <" +
        "= this value after the other eligibility check.");
            // 
            // setBaseNinjaDashLabel
            // 
            this.setBaseNinjaDashLabel.Location = new System.Drawing.Point(444, 109);
            this.setBaseNinjaDashLabel.Name = "setBaseNinjaDashLabel";
            this.setBaseNinjaDashLabel.Size = new System.Drawing.Size(230, 20);
            this.setBaseNinjaDashLabel.TabIndex = 22;
            this.setBaseNinjaDashLabel.Text = "Ninja dash speed";
            // 
            // setBaseNinjaDash
            // 
            this.setBaseNinjaDash.DecimalPlaces = 5;
            this.setBaseNinjaDash.Location = new System.Drawing.Point(682, 106);
            this.setBaseNinjaDash.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setBaseNinjaDash.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setBaseNinjaDash.Name = "setBaseNinjaDash";
            this.setBaseNinjaDash.Size = new System.Drawing.Size(164, 20);
            this.setBaseNinjaDash.TabIndex = 23;
            this.fieldTips.SetToolTip(this.setBaseNinjaDash, "Ninja-dash speed; loader applies defaultFPS/currentFPS.");
            // 
            // setBaseAirDashDurationLabel
            // 
            this.setBaseAirDashDurationLabel.Location = new System.Drawing.Point(444, 139);
            this.setBaseAirDashDurationLabel.Name = "setBaseAirDashDurationLabel";
            this.setBaseAirDashDurationLabel.Size = new System.Drawing.Size(230, 20);
            this.setBaseAirDashDurationLabel.TabIndex = 24;
            this.setBaseAirDashDurationLabel.Text = "Ninja dash duration";
            // 
            // setBaseAirDashDuration
            // 
            this.setBaseAirDashDuration.DecimalPlaces = 5;
            this.setBaseAirDashDuration.Location = new System.Drawing.Point(682, 136);
            this.setBaseAirDashDuration.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setBaseAirDashDuration.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setBaseAirDashDuration.Name = "setBaseAirDashDuration";
            this.setBaseAirDashDuration.Size = new System.Drawing.Size(164, 20);
            this.setBaseAirDashDuration.TabIndex = 25;
            this.fieldTips.SetToolTip(this.setBaseAirDashDuration, "Ninja-dash duration also read by another dash-action exit; loader scales by curre" +
        "ntFPS/30.");
            // 
            // setBaseGroundedChakraDashDurationLabel
            // 
            this.setBaseGroundedChakraDashDurationLabel.Location = new System.Drawing.Point(444, 169);
            this.setBaseGroundedChakraDashDurationLabel.Name = "setBaseGroundedChakraDashDurationLabel";
            this.setBaseGroundedChakraDashDurationLabel.Size = new System.Drawing.Size(230, 20);
            this.setBaseGroundedChakraDashDurationLabel.TabIndex = 26;
            this.setBaseGroundedChakraDashDurationLabel.Text = "Chakra dash duration";
            // 
            // setBaseGroundedChakraDashDuration
            // 
            this.setBaseGroundedChakraDashDuration.DecimalPlaces = 5;
            this.setBaseGroundedChakraDashDuration.Location = new System.Drawing.Point(682, 166);
            this.setBaseGroundedChakraDashDuration.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setBaseGroundedChakraDashDuration.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setBaseGroundedChakraDashDuration.Name = "setBaseGroundedChakraDashDuration";
            this.setBaseGroundedChakraDashDuration.Size = new System.Drawing.Size(164, 20);
            this.setBaseGroundedChakraDashDuration.TabIndex = 27;
            this.fieldTips.SetToolTip(this.setBaseGroundedChakraDashDuration, "Chakra-dash duration, subject to charged-dash extension and a runtime override; l" +
        "oader scales by currentFPS/30.");
            // 
            // extraDashTrackTimeLabel
            // 
            this.extraDashTrackTimeLabel.Location = new System.Drawing.Point(444, 199);
            this.extraDashTrackTimeLabel.Name = "extraDashTrackTimeLabel";
            this.extraDashTrackTimeLabel.Size = new System.Drawing.Size(230, 20);
            this.extraDashTrackTimeLabel.TabIndex = 28;
            this.extraDashTrackTimeLabel.Text = "Chakra-dash tracking duration";
            this.fieldTips.SetToolTip(this.extraDashTrackTimeLabel, resources.GetString("extraDashTrackTimeLabel.ToolTip"));
            // 
            // extraDashTrackTime
            // 
            this.extraDashTrackTime.Location = new System.Drawing.Point(682, 196);
            this.extraDashTrackTime.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.extraDashTrackTime.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.extraDashTrackTime.Name = "extraDashTrackTime";
            this.extraDashTrackTime.Size = new System.Drawing.Size(164, 20);
            this.extraDashTrackTime.TabIndex = 29;
            this.fieldTips.SetToolTip(this.extraDashTrackTime, resources.GetString("extraDashTrackTime.ToolTip"));
            // 
            // extraDashTurnRateLabel
            // 
            this.extraDashTurnRateLabel.Location = new System.Drawing.Point(444, 229);
            this.extraDashTurnRateLabel.Name = "extraDashTurnRateLabel";
            this.extraDashTurnRateLabel.Size = new System.Drawing.Size(230, 20);
            this.extraDashTurnRateLabel.TabIndex = 30;
            this.extraDashTurnRateLabel.Text = "Chakra-dash turn strength";
            this.fieldTips.SetToolTip(this.extraDashTurnRateLabel, resources.GetString("extraDashTurnRateLabel.ToolTip"));
            // 
            // extraDashTurnRate
            // 
            this.extraDashTurnRate.DecimalPlaces = 6;
            this.extraDashTurnRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraDashTurnRate.Location = new System.Drawing.Point(682, 226);
            this.extraDashTurnRate.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraDashTurnRate.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraDashTurnRate.Name = "extraDashTurnRate";
            this.extraDashTurnRate.Size = new System.Drawing.Size(164, 20);
            this.extraDashTurnRate.TabIndex = 31;
            this.fieldTips.SetToolTip(this.extraDashTurnRate, resources.GetString("extraDashTurnRate.ToolTip"));
            // 
            // awakeTab
            // 
            this.awakeTab.AutoScroll = true;
            this.awakeTab.Controls.Add(this.setAwakeMovementLabel);
            this.awakeTab.Controls.Add(this.setAwakeMovement);
            this.awakeTab.Controls.Add(this.setAwakeChakraDashLabel);
            this.awakeTab.Controls.Add(this.setAwakeChakraDash);
            this.awakeTab.Controls.Add(this.setAwakeNinjaDashLabel);
            this.awakeTab.Controls.Add(this.setAwakeNinjaDash);
            this.awakeTab.Controls.Add(this.setAwakeAirDashDurationLabel);
            this.awakeTab.Controls.Add(this.setAwakeAirDashDuration);
            this.awakeTab.Controls.Add(this.setAwakeGroundedChakraDashDurationLabel);
            this.awakeTab.Controls.Add(this.setAwakeGroundedChakraDashDuration);
            this.awakeTab.Controls.Add(this.extraAwakeDashTrackTimeLabel);
            this.awakeTab.Controls.Add(this.extraAwakeDashTrackTime);
            this.awakeTab.Controls.Add(this.extraAwakeDashTurnRateLabel);
            this.awakeTab.Controls.Add(this.extraAwakeDashTurnRate);
            this.awakeTab.Controls.Add(this.extraUnknown298Label);
            this.awakeTab.Controls.Add(this.extraUnknown298);
            this.awakeTab.Controls.Add(this.extraUnknown29CLabel);
            this.awakeTab.Controls.Add(this.extraUnknown29C);
            this.awakeTab.Controls.Add(this.extraUnknown2A0Label);
            this.awakeTab.Controls.Add(this.extraUnknown2A0);
            this.awakeTab.Controls.Add(this.extraUnknown2A4Label);
            this.awakeTab.Controls.Add(this.extraUnknown2A4);
            this.awakeTab.Controls.Add(this.extraUnknown2A8Label);
            this.awakeTab.Controls.Add(this.extraUnknown2A8);
            this.awakeTab.Controls.Add(this.extraUnknown2ACLabel);
            this.awakeTab.Controls.Add(this.extraUnknown2AC);
            this.awakeTab.Controls.Add(this.extraUnknown2B0Label);
            this.awakeTab.Controls.Add(this.extraUnknown2B0);
            this.awakeTab.Controls.Add(this.extraUnknown2B4Label);
            this.awakeTab.Controls.Add(this.extraUnknown2B4);
            this.awakeTab.Controls.Add(this.extraUnknown2B8Label);
            this.awakeTab.Controls.Add(this.extraUnknown2B8);
            this.awakeTab.Controls.Add(this.extraUnknown2BCLabel);
            this.awakeTab.Controls.Add(this.extraUnknown2BC);
            this.awakeTab.Controls.Add(this.setEnableDashPriorityLabel);
            this.awakeTab.Controls.Add(this.setEnableDashPriority);
            this.awakeTab.Controls.Add(this.awakeRiskLabel);
            this.awakeTab.Controls.Add(this.awakeRisk);
            this.awakeTab.Controls.Add(this.setChakraCostAwakeningLabel);
            this.awakeTab.Controls.Add(this.setChakraCostAwakening);
            this.awakeTab.Controls.Add(this.extraUnknown2CCLabel);
            this.awakeTab.Controls.Add(this.extraUnknown2CC);
            this.awakeTab.Controls.Add(this.setChakraBlockRecoveryLabel);
            this.awakeTab.Controls.Add(this.setChakraBlockRecovery);
            this.awakeTab.Controls.Add(this.setAwakeningActionChargeLabel);
            this.awakeTab.Controls.Add(this.setAwakeningActionCharge);
            this.awakeTab.Location = new System.Drawing.Point(4, 22);
            this.awakeTab.Name = "awakeTab";
            this.awakeTab.Size = new System.Drawing.Size(876, 562);
            this.awakeTab.TabIndex = 0;
            this.awakeTab.Text = "Awake settings";
            this.awakeTab.UseVisualStyleBackColor = true;
            // 
            // setAwakeMovementLabel
            // 
            this.setAwakeMovementLabel.Location = new System.Drawing.Point(12, 19);
            this.setAwakeMovementLabel.Name = "setAwakeMovementLabel";
            this.setAwakeMovementLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeMovementLabel.TabIndex = 0;
            this.setAwakeMovementLabel.Text = "Awakened run speed";
            // 
            // setAwakeMovement
            // 
            this.setAwakeMovement.DecimalPlaces = 5;
            this.setAwakeMovement.Location = new System.Drawing.Point(250, 16);
            this.setAwakeMovement.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setAwakeMovement.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setAwakeMovement.Name = "setAwakeMovement";
            this.setAwakeMovement.Size = new System.Drawing.Size(164, 20);
            this.setAwakeMovement.TabIndex = 1;
            this.fieldTips.SetToolTip(this.setAwakeMovement, "Forward speed while truly awakened. The loader adjusts the speed for the current " +
        "frame rate.");
            // 
            // setAwakeChakraDashLabel
            // 
            this.setAwakeChakraDashLabel.Location = new System.Drawing.Point(12, 49);
            this.setAwakeChakraDashLabel.Name = "setAwakeChakraDashLabel";
            this.setAwakeChakraDashLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeChakraDashLabel.TabIndex = 2;
            this.setAwakeChakraDashLabel.Text = "Awakened chakra dash speed";
            // 
            // setAwakeChakraDash
            // 
            this.setAwakeChakraDash.DecimalPlaces = 5;
            this.setAwakeChakraDash.Location = new System.Drawing.Point(250, 46);
            this.setAwakeChakraDash.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setAwakeChakraDash.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setAwakeChakraDash.Name = "setAwakeChakraDash";
            this.setAwakeChakraDash.Size = new System.Drawing.Size(164, 20);
            this.setAwakeChakraDash.TabIndex = 3;
            this.fieldTips.SetToolTip(this.setAwakeChakraDash, "Chakra-dash speed while truly awakened. Condition and charged-dash modifiers can " +
        "change the final speed.");
            // 
            // setAwakeNinjaDashLabel
            // 
            this.setAwakeNinjaDashLabel.Location = new System.Drawing.Point(12, 79);
            this.setAwakeNinjaDashLabel.Name = "setAwakeNinjaDashLabel";
            this.setAwakeNinjaDashLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeNinjaDashLabel.TabIndex = 4;
            this.setAwakeNinjaDashLabel.Text = "Awakened ninja dash speed";
            // 
            // setAwakeNinjaDash
            // 
            this.setAwakeNinjaDash.DecimalPlaces = 5;
            this.setAwakeNinjaDash.Location = new System.Drawing.Point(250, 76);
            this.setAwakeNinjaDash.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setAwakeNinjaDash.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setAwakeNinjaDash.Name = "setAwakeNinjaDash";
            this.setAwakeNinjaDash.Size = new System.Drawing.Size(164, 20);
            this.setAwakeNinjaDash.TabIndex = 5;
            this.fieldTips.SetToolTip(this.setAwakeNinjaDash, "Ninja-dash speed while truly awakened. The loader adjusts the speed for the curre" +
        "nt frame rate.");
            // 
            // setAwakeAirDashDurationLabel
            // 
            this.setAwakeAirDashDurationLabel.Location = new System.Drawing.Point(12, 109);
            this.setAwakeAirDashDurationLabel.Name = "setAwakeAirDashDurationLabel";
            this.setAwakeAirDashDurationLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeAirDashDurationLabel.TabIndex = 6;
            this.setAwakeAirDashDurationLabel.Text = "Awakened ninja dash duration";
            // 
            // setAwakeAirDashDuration
            // 
            this.setAwakeAirDashDuration.DecimalPlaces = 5;
            this.setAwakeAirDashDuration.Location = new System.Drawing.Point(250, 106);
            this.setAwakeAirDashDuration.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setAwakeAirDashDuration.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setAwakeAirDashDuration.Name = "setAwakeAirDashDuration";
            this.setAwakeAirDashDuration.Size = new System.Drawing.Size(164, 20);
            this.setAwakeAirDashDuration.TabIndex = 7;
            this.fieldTips.SetToolTip(this.setAwakeAirDashDuration, "Ninja-dash duration while truly awakened. The loader adjusts the duration for the" +
        " current frame rate.");
            // 
            // setAwakeGroundedChakraDashDurationLabel
            // 
            this.setAwakeGroundedChakraDashDurationLabel.Location = new System.Drawing.Point(12, 139);
            this.setAwakeGroundedChakraDashDurationLabel.Name = "setAwakeGroundedChakraDashDurationLabel";
            this.setAwakeGroundedChakraDashDurationLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeGroundedChakraDashDurationLabel.TabIndex = 8;
            this.setAwakeGroundedChakraDashDurationLabel.Text = "Awakened chakra dash duration";
            // 
            // setAwakeGroundedChakraDashDuration
            // 
            this.setAwakeGroundedChakraDashDuration.DecimalPlaces = 5;
            this.setAwakeGroundedChakraDashDuration.Location = new System.Drawing.Point(250, 136);
            this.setAwakeGroundedChakraDashDuration.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.setAwakeGroundedChakraDashDuration.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.setAwakeGroundedChakraDashDuration.Name = "setAwakeGroundedChakraDashDuration";
            this.setAwakeGroundedChakraDashDuration.Size = new System.Drawing.Size(164, 20);
            this.setAwakeGroundedChakraDashDuration.TabIndex = 9;
            this.fieldTips.SetToolTip(this.setAwakeGroundedChakraDashDuration, "Chakra-dash duration while truly awakened. Charged dashes and runtime overrides c" +
        "an extend it.");
            // 
            // extraAwakeDashTrackTimeLabel
            // 
            this.extraAwakeDashTrackTimeLabel.Location = new System.Drawing.Point(12, 169);
            this.extraAwakeDashTrackTimeLabel.Name = "extraAwakeDashTrackTimeLabel";
            this.extraAwakeDashTrackTimeLabel.Size = new System.Drawing.Size(230, 20);
            this.extraAwakeDashTrackTimeLabel.TabIndex = 10;
            this.extraAwakeDashTrackTimeLabel.Text = "Awakened dash tracking duration";
            this.fieldTips.SetToolTip(this.extraAwakeDashTrackTimeLabel, resources.GetString("extraAwakeDashTrackTimeLabel.ToolTip"));
            // 
            // extraAwakeDashTrackTime
            // 
            this.extraAwakeDashTrackTime.Location = new System.Drawing.Point(250, 166);
            this.extraAwakeDashTrackTime.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.extraAwakeDashTrackTime.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.extraAwakeDashTrackTime.Name = "extraAwakeDashTrackTime";
            this.extraAwakeDashTrackTime.Size = new System.Drawing.Size(164, 20);
            this.extraAwakeDashTrackTime.TabIndex = 11;
            this.fieldTips.SetToolTip(this.extraAwakeDashTrackTime, resources.GetString("extraAwakeDashTrackTime.ToolTip"));
            // 
            // extraAwakeDashTurnRateLabel
            // 
            this.extraAwakeDashTurnRateLabel.Location = new System.Drawing.Point(12, 199);
            this.extraAwakeDashTurnRateLabel.Name = "extraAwakeDashTurnRateLabel";
            this.extraAwakeDashTurnRateLabel.Size = new System.Drawing.Size(230, 20);
            this.extraAwakeDashTurnRateLabel.TabIndex = 12;
            this.extraAwakeDashTurnRateLabel.Text = "Awakened dash turn strength";
            this.fieldTips.SetToolTip(this.extraAwakeDashTurnRateLabel, resources.GetString("extraAwakeDashTurnRateLabel.ToolTip"));
            // 
            // extraAwakeDashTurnRate
            // 
            this.extraAwakeDashTurnRate.DecimalPlaces = 6;
            this.extraAwakeDashTurnRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraAwakeDashTurnRate.Location = new System.Drawing.Point(250, 196);
            this.extraAwakeDashTurnRate.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraAwakeDashTurnRate.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraAwakeDashTurnRate.Name = "extraAwakeDashTurnRate";
            this.extraAwakeDashTurnRate.Size = new System.Drawing.Size(164, 20);
            this.extraAwakeDashTurnRate.TabIndex = 13;
            this.fieldTips.SetToolTip(this.extraAwakeDashTurnRate, resources.GetString("extraAwakeDashTurnRate.ToolTip"));
            // 
            // extraUnknown298Label
            // 
            this.extraUnknown298Label.Location = new System.Drawing.Point(12, 229);
            this.extraUnknown298Label.Name = "extraUnknown298Label";
            this.extraUnknown298Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown298Label.TabIndex = 14;
            this.extraUnknown298Label.Text = "Unknown 298";
            this.fieldTips.SetToolTip(this.extraUnknown298Label, resources.GetString("extraUnknown298Label.ToolTip"));
            // 
            // extraUnknown298
            // 
            this.extraUnknown298.DecimalPlaces = 6;
            this.extraUnknown298.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown298.Location = new System.Drawing.Point(250, 226);
            this.extraUnknown298.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown298.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown298.Name = "extraUnknown298";
            this.extraUnknown298.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown298.TabIndex = 15;
            this.fieldTips.SetToolTip(this.extraUnknown298, resources.GetString("extraUnknown298.ToolTip"));
            // 
            // extraUnknown29CLabel
            // 
            this.extraUnknown29CLabel.Location = new System.Drawing.Point(12, 259);
            this.extraUnknown29CLabel.Name = "extraUnknown29CLabel";
            this.extraUnknown29CLabel.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown29CLabel.TabIndex = 16;
            this.extraUnknown29CLabel.Text = "Unknown 29C";
            this.fieldTips.SetToolTip(this.extraUnknown29CLabel, resources.GetString("extraUnknown29CLabel.ToolTip"));
            // 
            // extraUnknown29C
            // 
            this.extraUnknown29C.DecimalPlaces = 6;
            this.extraUnknown29C.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown29C.Location = new System.Drawing.Point(250, 256);
            this.extraUnknown29C.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown29C.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown29C.Name = "extraUnknown29C";
            this.extraUnknown29C.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown29C.TabIndex = 17;
            this.fieldTips.SetToolTip(this.extraUnknown29C, resources.GetString("extraUnknown29C.ToolTip"));
            // 
            // extraUnknown2A0Label
            // 
            this.extraUnknown2A0Label.Location = new System.Drawing.Point(12, 289);
            this.extraUnknown2A0Label.Name = "extraUnknown2A0Label";
            this.extraUnknown2A0Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2A0Label.TabIndex = 18;
            this.extraUnknown2A0Label.Text = "Unknown 2A0";
            this.fieldTips.SetToolTip(this.extraUnknown2A0Label, resources.GetString("extraUnknown2A0Label.ToolTip"));
            // 
            // extraUnknown2A0
            // 
            this.extraUnknown2A0.DecimalPlaces = 6;
            this.extraUnknown2A0.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2A0.Location = new System.Drawing.Point(250, 286);
            this.extraUnknown2A0.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2A0.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2A0.Name = "extraUnknown2A0";
            this.extraUnknown2A0.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2A0.TabIndex = 19;
            this.fieldTips.SetToolTip(this.extraUnknown2A0, resources.GetString("extraUnknown2A0.ToolTip"));
            // 
            // extraUnknown2A4Label
            // 
            this.extraUnknown2A4Label.Location = new System.Drawing.Point(12, 319);
            this.extraUnknown2A4Label.Name = "extraUnknown2A4Label";
            this.extraUnknown2A4Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2A4Label.TabIndex = 20;
            this.extraUnknown2A4Label.Text = "Unknown 2A4";
            this.fieldTips.SetToolTip(this.extraUnknown2A4Label, resources.GetString("extraUnknown2A4Label.ToolTip"));
            // 
            // extraUnknown2A4
            // 
            this.extraUnknown2A4.DecimalPlaces = 6;
            this.extraUnknown2A4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2A4.Location = new System.Drawing.Point(250, 316);
            this.extraUnknown2A4.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2A4.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2A4.Name = "extraUnknown2A4";
            this.extraUnknown2A4.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2A4.TabIndex = 21;
            this.fieldTips.SetToolTip(this.extraUnknown2A4, resources.GetString("extraUnknown2A4.ToolTip"));
            // 
            // extraUnknown2A8Label
            // 
            this.extraUnknown2A8Label.Location = new System.Drawing.Point(12, 349);
            this.extraUnknown2A8Label.Name = "extraUnknown2A8Label";
            this.extraUnknown2A8Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2A8Label.TabIndex = 22;
            this.extraUnknown2A8Label.Text = "Unknown 2A8";
            this.fieldTips.SetToolTip(this.extraUnknown2A8Label, resources.GetString("extraUnknown2A8Label.ToolTip"));
            // 
            // extraUnknown2A8
            // 
            this.extraUnknown2A8.DecimalPlaces = 6;
            this.extraUnknown2A8.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2A8.Location = new System.Drawing.Point(250, 346);
            this.extraUnknown2A8.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2A8.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2A8.Name = "extraUnknown2A8";
            this.extraUnknown2A8.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2A8.TabIndex = 23;
            this.fieldTips.SetToolTip(this.extraUnknown2A8, resources.GetString("extraUnknown2A8.ToolTip"));
            // 
            // extraUnknown2ACLabel
            // 
            this.extraUnknown2ACLabel.Location = new System.Drawing.Point(444, 19);
            this.extraUnknown2ACLabel.Name = "extraUnknown2ACLabel";
            this.extraUnknown2ACLabel.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2ACLabel.TabIndex = 24;
            this.extraUnknown2ACLabel.Text = "Unknown 2AC";
            this.fieldTips.SetToolTip(this.extraUnknown2ACLabel, resources.GetString("extraUnknown2ACLabel.ToolTip"));
            // 
            // extraUnknown2AC
            // 
            this.extraUnknown2AC.DecimalPlaces = 6;
            this.extraUnknown2AC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2AC.Location = new System.Drawing.Point(682, 16);
            this.extraUnknown2AC.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2AC.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2AC.Name = "extraUnknown2AC";
            this.extraUnknown2AC.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2AC.TabIndex = 25;
            this.fieldTips.SetToolTip(this.extraUnknown2AC, resources.GetString("extraUnknown2AC.ToolTip"));
            // 
            // extraUnknown2B0Label
            // 
            this.extraUnknown2B0Label.Location = new System.Drawing.Point(444, 49);
            this.extraUnknown2B0Label.Name = "extraUnknown2B0Label";
            this.extraUnknown2B0Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2B0Label.TabIndex = 26;
            this.extraUnknown2B0Label.Text = "Unknown 2B0";
            this.fieldTips.SetToolTip(this.extraUnknown2B0Label, resources.GetString("extraUnknown2B0Label.ToolTip"));
            // 
            // extraUnknown2B0
            // 
            this.extraUnknown2B0.DecimalPlaces = 6;
            this.extraUnknown2B0.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2B0.Location = new System.Drawing.Point(682, 46);
            this.extraUnknown2B0.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2B0.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2B0.Name = "extraUnknown2B0";
            this.extraUnknown2B0.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2B0.TabIndex = 27;
            this.fieldTips.SetToolTip(this.extraUnknown2B0, resources.GetString("extraUnknown2B0.ToolTip"));
            // 
            // extraUnknown2B4Label
            // 
            this.extraUnknown2B4Label.Location = new System.Drawing.Point(444, 79);
            this.extraUnknown2B4Label.Name = "extraUnknown2B4Label";
            this.extraUnknown2B4Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2B4Label.TabIndex = 28;
            this.extraUnknown2B4Label.Text = "Unknown 2B4";
            this.fieldTips.SetToolTip(this.extraUnknown2B4Label, resources.GetString("extraUnknown2B4Label.ToolTip"));
            // 
            // extraUnknown2B4
            // 
            this.extraUnknown2B4.DecimalPlaces = 6;
            this.extraUnknown2B4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2B4.Location = new System.Drawing.Point(682, 76);
            this.extraUnknown2B4.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2B4.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2B4.Name = "extraUnknown2B4";
            this.extraUnknown2B4.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2B4.TabIndex = 29;
            this.fieldTips.SetToolTip(this.extraUnknown2B4, resources.GetString("extraUnknown2B4.ToolTip"));
            // 
            // extraUnknown2B8Label
            // 
            this.extraUnknown2B8Label.Location = new System.Drawing.Point(444, 109);
            this.extraUnknown2B8Label.Name = "extraUnknown2B8Label";
            this.extraUnknown2B8Label.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2B8Label.TabIndex = 30;
            this.extraUnknown2B8Label.Text = "Unknown 2B8";
            this.fieldTips.SetToolTip(this.extraUnknown2B8Label, resources.GetString("extraUnknown2B8Label.ToolTip"));
            // 
            // extraUnknown2B8
            // 
            this.extraUnknown2B8.DecimalPlaces = 6;
            this.extraUnknown2B8.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2B8.Location = new System.Drawing.Point(682, 106);
            this.extraUnknown2B8.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2B8.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2B8.Name = "extraUnknown2B8";
            this.extraUnknown2B8.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2B8.TabIndex = 31;
            this.fieldTips.SetToolTip(this.extraUnknown2B8, resources.GetString("extraUnknown2B8.ToolTip"));
            // 
            // extraUnknown2BCLabel
            // 
            this.extraUnknown2BCLabel.Location = new System.Drawing.Point(444, 139);
            this.extraUnknown2BCLabel.Name = "extraUnknown2BCLabel";
            this.extraUnknown2BCLabel.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2BCLabel.TabIndex = 32;
            this.extraUnknown2BCLabel.Text = "Unknown 2BC (type unverified)";
            this.fieldTips.SetToolTip(this.extraUnknown2BCLabel, resources.GetString("extraUnknown2BCLabel.ToolTip"));
            // 
            // extraUnknown2BC
            // 
            this.extraUnknown2BC.DecimalPlaces = 6;
            this.extraUnknown2BC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2BC.Location = new System.Drawing.Point(682, 136);
            this.extraUnknown2BC.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2BC.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2BC.Name = "extraUnknown2BC";
            this.extraUnknown2BC.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2BC.TabIndex = 33;
            this.fieldTips.SetToolTip(this.extraUnknown2BC, resources.GetString("extraUnknown2BC.ToolTip"));
            // 
            // setEnableDashPriorityLabel
            // 
            this.setEnableDashPriorityLabel.Location = new System.Drawing.Point(444, 169);
            this.setEnableDashPriorityLabel.Name = "setEnableDashPriorityLabel";
            this.setEnableDashPriorityLabel.Size = new System.Drawing.Size(230, 20);
            this.setEnableDashPriorityLabel.TabIndex = 34;
            this.setEnableDashPriorityLabel.Text = "Awake Super Armor";
            // 
            // setEnableDashPriority
            // 
            this.setEnableDashPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setEnableDashPriority.DropDownWidth = 380;
            this.setEnableDashPriority.Location = new System.Drawing.Point(682, 166);
            this.setEnableDashPriority.Name = "setEnableDashPriority";
            this.setEnableDashPriority.Size = new System.Drawing.Size(164, 21);
            this.setEnableDashPriority.TabIndex = 35;
            this.fieldTips.SetToolTip(this.setEnableDashPriority, "Nonzero permits chakra-dash priority 390 while awakened; otherwise 350. Outside a" +
        "wakening priority is 350. This is a separate integer setting from the huge-awake" +
        "ning bit.");
            // 
            // awakeRiskLabel
            // 
            this.awakeRiskLabel.Location = new System.Drawing.Point(444, 199);
            this.awakeRiskLabel.Name = "awakeRiskLabel";
            this.awakeRiskLabel.Size = new System.Drawing.Size(230, 20);
            this.awakeRiskLabel.TabIndex = 36;
            this.awakeRiskLabel.Text = "Awakening risk mode";
            this.fieldTips.SetToolTip(this.awakeRiskLabel, resources.GetString("awakeRiskLabel.ToolTip"));
            // 
            // awakeRisk
            // 
            this.awakeRisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.awakeRisk.DropDownWidth = 380;
            this.awakeRisk.Location = new System.Drawing.Point(682, 196);
            this.awakeRisk.Name = "awakeRisk";
            this.awakeRisk.Size = new System.Drawing.Size(164, 21);
            this.awakeRisk.TabIndex = 37;
            this.fieldTips.SetToolTip(this.awakeRisk, resources.GetString("awakeRisk.ToolTip"));
            // 
            // setChakraCostAwakeningLabel
            // 
            this.setChakraCostAwakeningLabel.Location = new System.Drawing.Point(444, 229);
            this.setChakraCostAwakeningLabel.Name = "setChakraCostAwakeningLabel";
            this.setChakraCostAwakeningLabel.Size = new System.Drawing.Size(230, 20);
            this.setChakraCostAwakeningLabel.TabIndex = 38;
            this.setChakraCostAwakeningLabel.Text = "Awakening chakra cost";
            // 
            // setChakraCostAwakening
            // 
            this.setChakraCostAwakening.DecimalPlaces = 5;
            this.setChakraCostAwakening.Location = new System.Drawing.Point(682, 226);
            this.setChakraCostAwakening.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setChakraCostAwakening.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setChakraCostAwakening.Name = "setChakraCostAwakening";
            this.setChakraCostAwakening.Size = new System.Drawing.Size(164, 20);
            this.setChakraCostAwakening.TabIndex = 39;
            this.fieldTips.SetToolTip(this.setChakraCostAwakening, "Float amount subtracted from maximum chakra on awakening start, with minimum resu" +
        "lting maximum 30; current chakra is clamped down if necessary. Zero bypasses thi" +
        "s adjustment.");
            // 
            // extraUnknown2CCLabel
            // 
            this.extraUnknown2CCLabel.Location = new System.Drawing.Point(444, 259);
            this.extraUnknown2CCLabel.Name = "extraUnknown2CCLabel";
            this.extraUnknown2CCLabel.Size = new System.Drawing.Size(230, 20);
            this.extraUnknown2CCLabel.TabIndex = 40;
            this.extraUnknown2CCLabel.Text = "Unknown 2CC";
            this.fieldTips.SetToolTip(this.extraUnknown2CCLabel, resources.GetString("extraUnknown2CCLabel.ToolTip"));
            // 
            // extraUnknown2CC
            // 
            this.extraUnknown2CC.DecimalPlaces = 6;
            this.extraUnknown2CC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.extraUnknown2CC.Location = new System.Drawing.Point(682, 256);
            this.extraUnknown2CC.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.extraUnknown2CC.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.extraUnknown2CC.Name = "extraUnknown2CC";
            this.extraUnknown2CC.Size = new System.Drawing.Size(164, 20);
            this.extraUnknown2CC.TabIndex = 41;
            this.fieldTips.SetToolTip(this.extraUnknown2CC, resources.GetString("extraUnknown2CC.ToolTip"));
            // 
            // setChakraBlockRecoveryLabel
            // 
            this.setChakraBlockRecoveryLabel.Location = new System.Drawing.Point(444, 289);
            this.setChakraBlockRecoveryLabel.Name = "setChakraBlockRecoveryLabel";
            this.setChakraBlockRecoveryLabel.Size = new System.Drawing.Size(230, 20);
            this.setChakraBlockRecoveryLabel.TabIndex = 42;
            this.setChakraBlockRecoveryLabel.Text = "Max chakra auto recovery rate";
            // 
            // setChakraBlockRecovery
            // 
            this.setChakraBlockRecovery.DecimalPlaces = 5;
            this.setChakraBlockRecovery.Location = new System.Drawing.Point(682, 286);
            this.setChakraBlockRecovery.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setChakraBlockRecovery.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setChakraBlockRecovery.Name = "setChakraBlockRecovery";
            this.setChakraBlockRecovery.Size = new System.Drawing.Size(164, 20);
            this.setChakraBlockRecovery.TabIndex = 43;
            this.fieldTips.SetToolTip(this.setChakraBlockRecovery, "Per-update recovery of maximum chakra, capped at 100. Nonpositive values fall bac" +
        "k to 0.1. Recovery depends on AwakeRisk and an additional runtime block flag.");
            // 
            // setAwakeningActionChargeLabel
            // 
            this.setAwakeningActionChargeLabel.Location = new System.Drawing.Point(444, 319);
            this.setAwakeningActionChargeLabel.Name = "setAwakeningActionChargeLabel";
            this.setAwakeningActionChargeLabel.Size = new System.Drawing.Size(230, 20);
            this.setAwakeningActionChargeLabel.TabIndex = 44;
            this.setAwakeningActionChargeLabel.Text = "Awakening action recovery rate";
            // 
            // setAwakeningActionCharge
            // 
            this.setAwakeningActionCharge.DecimalPlaces = 5;
            this.setAwakeningActionCharge.Location = new System.Drawing.Point(682, 316);
            this.setAwakeningActionCharge.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.setAwakeningActionCharge.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.setAwakeningActionCharge.Name = "setAwakeningActionCharge";
            this.setAwakeningActionCharge.Size = new System.Drawing.Size(164, 20);
            this.setAwakeningActionCharge.TabIndex = 45;
            this.fieldTips.SetToolTip(this.setAwakeningActionCharge, "Recovery increment for inactive special-type gauges, scaled by defaultFPS/current" +
        "FPS. Four gauge slots are considered; character-specific recovery overrides exis" +
        "t.");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.itemListToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1120, 24);
            this.menuStrip1.TabIndex = 0;
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
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.sortToolStripMenuItem.Text = "Sort";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // itemListToolStripMenuItem
            // 
            this.itemListToolStripMenuItem.Name = "itemListToolStripMenuItem";
            this.itemListToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.itemListToolStripMenuItem.Text = "Open item list";
            this.itemListToolStripMenuItem.Click += new System.EventHandler(this.itemListToolStripMenuItem_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(8, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(212, 446);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(117, 554);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 28);
            this.button3.TabIndex = 8;
            this.button3.Text = "Delete entry";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Search_TB
            // 
            this.Search_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Search_TB.Location = new System.Drawing.Point(8, 494);
            this.Search_TB.Name = "Search_TB";
            this.Search_TB.Size = new System.Drawing.Size(144, 20);
            this.Search_TB.TabIndex = 0;
            // 
            // Search
            // 
            this.Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Search.Location = new System.Drawing.Point(158, 494);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(62, 20);
            this.Search.TabIndex = 1;
            this.Search.Text = "Search";
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // copySettingsButton
            // 
            this.copySettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copySettingsButton.Location = new System.Drawing.Point(8, 588);
            this.copySettingsButton.Name = "copySettingsButton";
            this.copySettingsButton.Size = new System.Drawing.Size(103, 28);
            this.copySettingsButton.TabIndex = 12;
            this.copySettingsButton.Text = "Copy settings";
            this.copySettingsButton.Click += new System.EventHandler(this.copySettingsButton_Click);
            // 
            // pasteSettingsButton
            // 
            this.pasteSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pasteSettingsButton.Location = new System.Drawing.Point(117, 588);
            this.pasteSettingsButton.Name = "pasteSettingsButton";
            this.pasteSettingsButton.Size = new System.Drawing.Size(103, 28);
            this.pasteSettingsButton.TabIndex = 13;
            this.pasteSettingsButton.Text = "Paste settings";
            this.pasteSettingsButton.Click += new System.EventHandler(this.pasteSettingsButton_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(8, 554);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 28);
            this.button2.TabIndex = 10;
            this.button2.Text = "Save entry";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(8, 520);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 28);
            this.button1.TabIndex = 9;
            this.button1.Text = "Add/Duplicate entry";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fieldTips
            // 
            this.fieldTips.AutoPopDelay = 30000;
            this.fieldTips.InitialDelay = 450;
            this.fieldTips.ReshowDelay = 100;
            this.fieldTips.ShowAlways = true;
            // 
            // Tool_DuelPlayerParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new System.Drawing.Size(1120, 632);
            this.Controls.Add(this.editorTabs);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Search_TB);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.copySettingsButton);
            this.Controls.Add(this.pasteSettingsButton);
            this.MainMenuStrip = this.menuStrip1;

            this.Name = "Tool_DuelPlayerParamEditor";
            this.Text = "DuelPlayerParam Editor 2.0";
            this.Load += new System.EventHandler(this.Tool_DuelPlayerParamEditor_Load_1);
            this.editorTabs.ResumeLayout(false);
            this.identityTab.ResumeLayout(false);
            this.identityTab.PerformLayout();
            this.costumesGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.costumeGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setEvo1)).EndInit();
            this.flagsTab.ResumeLayout(false);
            this.groupConditionFlags.ResumeLayout(false);
            this.groupConditionFlags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.v_enableAwaSkill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown15C)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown16C)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown170)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown174)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown178)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown180)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown184)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown188)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown18C)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown190)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown194)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown198)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown19C)).EndInit();
            this.bodyTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.setCameraDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setVictoryCameraAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown4)).EndInit();
            this.baseTab.ResumeLayout(false);
            this.baseTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseMovement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseChakraDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setGuardPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown1CC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAttack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setDefense)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAssistDamage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setItemBuffDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeHpRequirement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseNinjaDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseAirDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseGroundedChakraDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraDashTrackTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraDashTurnRate)).EndInit();
            this.awakeTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeMovement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeChakraDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeNinjaDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeAirDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeGroundedChakraDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraAwakeDashTrackTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraAwakeDashTurnRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown298)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown29C)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2A0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2A4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2A8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2AC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2B0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2B4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2B8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2BC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCostAwakening)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraUnknown2CC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraBlockRecovery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeningActionCharge)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private CheckedListBox checkedListConditionFlags;
        private GroupBox groupConditionFlags;
        private Label setEvo1Label;
        private NumericUpDown setEvo1;
    }
}

