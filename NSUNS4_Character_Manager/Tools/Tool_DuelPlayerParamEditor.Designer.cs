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
		private Button b_costumeids;
		private Button b_awkcostumeids;
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
		private Button button4;
		private ToolStripMenuItem itemListToolStripMenuItem;
		private Label label11;
		private NumericUpDown v_enableAwaSkill;
		private NumericUpDown v_vic_cam_angle;
		private Label label13;
		private NumericUpDown v_vic_cam_pos;
		private Label label14;
		private NumericUpDown v_cam_unk;
		private Label label15;
		private TextBox w_item4;
		private Panel inlineSettingsPanel;
		private NumericUpDown setBaseMovement;
		private NumericUpDown setAwakeMovement;
		private NumericUpDown setEvo1;
		private NumericUpDown setEvo2;
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
		private NumericUpDown setAwaBodyPriority;
		private NumericUpDown setDefaultAwaSkillIndex;
		private NumericUpDown setCameraDistance;
		private NumericUpDown setCameraUnknown1;
		private NumericUpDown setVictoryCameraAngle;
		private NumericUpDown setCameraUnknown2;
		private NumericUpDown setCameraUnknown3;
		private NumericUpDown setCameraUnknown4;
		private CheckBox setEnableDashPriority;
		private CheckBox setAwakeningDebuff;
		private Label settingsTitleLabel;
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
		private Label setEvo1Label;
		private Label setEvo2Label;
		private Label setAwaBodyPriorityLabel;
		private Label setDefaultAwaSkillIndexLabel;
		private Label setCameraDistanceLabel;
		private Label setCameraUnknown1Label;
		private Label setVictoryCameraAngleLabel;
		private Label setCameraUnknown2Label;
		private Label setCameraUnknown3Label;
		private Label setCameraUnknown4Label;
		private Label setBaseHeaderLabel;
		private Label setAwakeHeaderLabel;

		private void InitializeComponent()
		{
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
            this.label9 = new System.Windows.Forms.Label();
            this.w_charaprmbas = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.w_characodeid = new System.Windows.Forms.TextBox();
            this.b_costumeids = new System.Windows.Forms.Button();
            this.b_awkcostumeids = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.w_awkaction = new System.Windows.Forms.TextBox();
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
            this.label3 = new System.Windows.Forms.Label();
            this.w_defaultassist1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.w_defaultassist2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.w_partner = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.v_enableAwaSkill = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.v_vic_cam_angle = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.v_vic_cam_pos = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.v_cam_unk = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.inlineSettingsPanel = new System.Windows.Forms.Panel();
            this.settingsTitleLabel = new System.Windows.Forms.Label();
            this.setBaseHeaderLabel = new System.Windows.Forms.Label();
            this.setAwakeHeaderLabel = new System.Windows.Forms.Label();
            this.setBaseMovementLabel = new System.Windows.Forms.Label();
            this.setBaseMovement = new System.Windows.Forms.NumericUpDown();
            this.setAwakeMovement = new System.Windows.Forms.NumericUpDown();
            this.setBaseChakraDashLabel = new System.Windows.Forms.Label();
            this.setBaseChakraDash = new System.Windows.Forms.NumericUpDown();
            this.setAwakeChakraDash = new System.Windows.Forms.NumericUpDown();
            this.setGuardPressureLabel = new System.Windows.Forms.Label();
            this.setGuardPressure = new System.Windows.Forms.NumericUpDown();
            this.setAttackLabel = new System.Windows.Forms.Label();
            this.setAttack = new System.Windows.Forms.NumericUpDown();
            this.setDefenseLabel = new System.Windows.Forms.Label();
            this.setDefense = new System.Windows.Forms.NumericUpDown();
            this.setAssistDamageLabel = new System.Windows.Forms.Label();
            this.setAssistDamage = new System.Windows.Forms.NumericUpDown();
            this.setAwakeningActionChargeLabel = new System.Windows.Forms.Label();
            this.setAwakeningActionCharge = new System.Windows.Forms.NumericUpDown();
            this.setChakraChargeLabel = new System.Windows.Forms.Label();
            this.setChakraCharge = new System.Windows.Forms.NumericUpDown();
            this.setBaseNinjaDashLabel = new System.Windows.Forms.Label();
            this.setBaseNinjaDash = new System.Windows.Forms.NumericUpDown();
            this.setAwakeNinjaDash = new System.Windows.Forms.NumericUpDown();
            this.setBaseAirDashDurationLabel = new System.Windows.Forms.Label();
            this.setBaseAirDashDuration = new System.Windows.Forms.NumericUpDown();
            this.setAwakeAirDashDuration = new System.Windows.Forms.NumericUpDown();
            this.setBaseGroundedChakraDashDurationLabel = new System.Windows.Forms.Label();
            this.setBaseGroundedChakraDashDuration = new System.Windows.Forms.NumericUpDown();
            this.setAwakeGroundedChakraDashDuration = new System.Windows.Forms.NumericUpDown();
            this.setItemBuffDurationLabel = new System.Windows.Forms.Label();
            this.setItemBuffDuration = new System.Windows.Forms.NumericUpDown();
            this.setAwakeHpRequirementLabel = new System.Windows.Forms.Label();
            this.setAwakeHpRequirement = new System.Windows.Forms.NumericUpDown();
            this.setChakraCostAwakeningLabel = new System.Windows.Forms.Label();
            this.setChakraCostAwakening = new System.Windows.Forms.NumericUpDown();
            this.setChakraBlockRecoveryLabel = new System.Windows.Forms.Label();
            this.setChakraBlockRecovery = new System.Windows.Forms.NumericUpDown();
            this.setEvo1Label = new System.Windows.Forms.Label();
            this.setEvo1 = new System.Windows.Forms.NumericUpDown();
            this.setAwaBodyPriorityLabel = new System.Windows.Forms.Label();
            this.setAwaBodyPriority = new System.Windows.Forms.NumericUpDown();
            this.setDefaultAwaSkillIndexLabel = new System.Windows.Forms.Label();
            this.setDefaultAwaSkillIndex = new System.Windows.Forms.NumericUpDown();
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
            this.setEnableDashPriority = new System.Windows.Forms.CheckBox();
            this.setAwakeningDebuff = new System.Windows.Forms.CheckBox();
            this.setEvo2Label = new System.Windows.Forms.Label();
            this.setEvo2 = new System.Windows.Forms.NumericUpDown();
            this.checkedListConditionFlags = new System.Windows.Forms.CheckedListBox();
            this.groupConditionFlags = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_enableAwaSkill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_vic_cam_angle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_vic_cam_pos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_cam_unk)).BeginInit();
            this.inlineSettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseMovement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeMovement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseChakraDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeChakraDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setGuardPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAttack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setDefense)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAssistDamage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeningActionCharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseNinjaDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeNinjaDash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseAirDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeAirDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseGroundedChakraDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeGroundedChakraDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setItemBuffDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeHpRequirement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCostAwakening)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraBlockRecovery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setEvo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwaBodyPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setDefaultAwaSkillIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setVictoryCameraAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setEvo2)).BeginInit();
            this.groupConditionFlags.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.itemListToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1260, 24);
            this.menuStrip1.TabIndex = 38;
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
            this.listBox1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(12, 36);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(280, 667);
            this.listBox1.TabIndex = 37;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 797);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(168, 26);
            this.button3.TabIndex = 36;
            this.button3.Text = "Delete selected entry";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Search_TB
            // 
            this.Search_TB.Location = new System.Drawing.Point(12, 707);
            this.Search_TB.Name = "Search_TB";
            this.Search_TB.Size = new System.Drawing.Size(168, 20);
            this.Search_TB.TabIndex = 35;
            // 
            // Search
            // 
            this.Search.Location = new System.Drawing.Point(186, 707);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(106, 23);
            this.Search.TabIndex = 34;
            this.Search.Text = "Search entry";
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // copySettingsButton
            // 
            this.copySettingsButton.Location = new System.Drawing.Point(186, 733);
            this.copySettingsButton.Name = "copySettingsButton";
            this.copySettingsButton.Size = new System.Drawing.Size(106, 26);
            this.copySettingsButton.TabIndex = 33;
            this.copySettingsButton.Text = "Copy settings";
            this.copySettingsButton.Click += new System.EventHandler(this.copySettingsButton_Click);
            // 
            // pasteSettingsButton
            // 
            this.pasteSettingsButton.Location = new System.Drawing.Point(186, 765);
            this.pasteSettingsButton.Name = "pasteSettingsButton";
            this.pasteSettingsButton.Size = new System.Drawing.Size(106, 26);
            this.pasteSettingsButton.TabIndex = 32;
            this.pasteSettingsButton.Text = "Paste settings";
            this.pasteSettingsButton.Click += new System.EventHandler(this.pasteSettingsButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 765);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 26);
            this.button2.TabIndex = 33;
            this.button2.Text = "Save selected entry";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 733);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 26);
            this.button1.TabIndex = 32;
            this.button1.Text = "Add duplicate entry";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(310, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Character code";
            // 
            // w_charaprmbas
            // 
            this.w_charaprmbas.Location = new System.Drawing.Point(309, 54);
            this.w_charaprmbas.MaxLength = 8;
            this.w_charaprmbas.Name = "w_charaprmbas";
            this.w_charaprmbas.Size = new System.Drawing.Size(290, 20);
            this.w_charaprmbas.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(310, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Motion code";
            // 
            // w_characodeid
            // 
            this.w_characodeid.Location = new System.Drawing.Point(309, 100);
            this.w_characodeid.MaxLength = 8;
            this.w_characodeid.Name = "w_characodeid";
            this.w_characodeid.Size = new System.Drawing.Size(290, 20);
            this.w_characodeid.TabIndex = 28;
            // 
            // b_costumeids
            // 
            this.b_costumeids.Location = new System.Drawing.Point(309, 132);
            this.b_costumeids.Name = "b_costumeids";
            this.b_costumeids.Size = new System.Drawing.Size(290, 25);
            this.b_costumeids.TabIndex = 27;
            this.b_costumeids.Text = "Edit costumes";
            this.b_costumeids.Click += new System.EventHandler(this.b_costumeids_Click);
            // 
            // b_awkcostumeids
            // 
            this.b_awkcostumeids.Location = new System.Drawing.Point(309, 132);
            this.b_awkcostumeids.Name = "b_awkcostumeids";
            this.b_awkcostumeids.Size = new System.Drawing.Size(1, 1);
            this.b_awkcostumeids.TabIndex = 26;
            this.b_awkcostumeids.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(306, 302);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Awakening condition";
            // 
            // w_awkaction
            // 
            this.w_awkaction.Location = new System.Drawing.Point(419, 299);
            this.w_awkaction.Name = "w_awkaction";
            this.w_awkaction.Size = new System.Drawing.Size(234, 20);
            this.w_awkaction.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(309, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Item 1";
            // 
            // w_item1
            // 
            this.w_item1.Location = new System.Drawing.Point(309, 178);
            this.w_item1.Name = "w_item1";
            this.w_item1.Size = new System.Drawing.Size(128, 20);
            this.w_item1.TabIndex = 22;
            // 
            // w_itemc1
            // 
            this.w_itemc1.Location = new System.Drawing.Point(443, 178);
            this.w_itemc1.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.w_itemc1.Name = "w_itemc1";
            this.w_itemc1.Size = new System.Drawing.Size(32, 20);
            this.w_itemc1.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(481, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Item 2";
            // 
            // w_item2
            // 
            this.w_item2.Location = new System.Drawing.Point(481, 179);
            this.w_item2.Name = "w_item2";
            this.w_item2.Size = new System.Drawing.Size(113, 20);
            this.w_item2.TabIndex = 19;
            // 
            // w_itemc2
            // 
            this.w_itemc2.Location = new System.Drawing.Point(600, 179);
            this.w_itemc2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.w_itemc2.Name = "w_itemc2";
            this.w_itemc2.Size = new System.Drawing.Size(37, 20);
            this.w_itemc2.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(309, 207);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Item 3";
            // 
            // w_item3
            // 
            this.w_item3.Location = new System.Drawing.Point(309, 223);
            this.w_item3.Name = "w_item3";
            this.w_item3.Size = new System.Drawing.Size(128, 20);
            this.w_item3.TabIndex = 16;
            // 
            // w_itemc3
            // 
            this.w_itemc3.Location = new System.Drawing.Point(443, 223);
            this.w_itemc3.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.w_itemc3.Name = "w_itemc3";
            this.w_itemc3.Size = new System.Drawing.Size(32, 20);
            this.w_itemc3.TabIndex = 15;
            this.w_itemc3.ValueChanged += new System.EventHandler(this.w_itemc3_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(481, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Item 4";
            // 
            // w_item4
            // 
            this.w_item4.Location = new System.Drawing.Point(481, 223);
            this.w_item4.Name = "w_item4";
            this.w_item4.Size = new System.Drawing.Size(113, 20);
            this.w_item4.TabIndex = 13;
            // 
            // w_itemc4
            // 
            this.w_itemc4.Location = new System.Drawing.Point(600, 223);
            this.w_itemc4.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.w_itemc4.Name = "w_itemc4";
            this.w_itemc4.Size = new System.Drawing.Size(37, 20);
            this.w_itemc4.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Support 1:";
            // 
            // w_defaultassist1
            // 
            this.w_defaultassist1.Location = new System.Drawing.Point(309, 273);
            this.w_defaultassist1.Name = "w_defaultassist1";
            this.w_defaultassist1.Size = new System.Drawing.Size(104, 20);
            this.w_defaultassist1.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(419, 257);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Support 2:";
            // 
            // w_defaultassist2
            // 
            this.w_defaultassist2.Location = new System.Drawing.Point(419, 273);
            this.w_defaultassist2.Name = "w_defaultassist2";
            this.w_defaultassist2.Size = new System.Drawing.Size(104, 20);
            this.w_defaultassist2.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(529, 257);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Partner:";
            // 
            // w_partner
            // 
            this.w_partner.Location = new System.Drawing.Point(529, 273);
            this.w_partner.Name = "w_partner";
            this.w_partner.Size = new System.Drawing.Size(124, 20);
            this.w_partner.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Condition flags:";
            // 
            // v_enableAwaSkill
            // 
            this.v_enableAwaSkill.Hexadecimal = true;
            this.v_enableAwaSkill.Location = new System.Drawing.Point(220, 14);
            this.v_enableAwaSkill.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.v_enableAwaSkill.Name = "v_enableAwaSkill";
            this.v_enableAwaSkill.Size = new System.Drawing.Size(112, 20);
            this.v_enableAwaSkill.TabIndex = 3;
            this.v_enableAwaSkill.ValueChanged += new System.EventHandler(this.v_enableAwaSkill_ValueChanged);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 0;
            this.label13.Visible = false;
            // 
            // v_vic_cam_angle
            // 
            this.v_vic_cam_angle.Location = new System.Drawing.Point(0, 0);
            this.v_vic_cam_angle.Name = "v_vic_cam_angle";
            this.v_vic_cam_angle.Size = new System.Drawing.Size(120, 20);
            this.v_vic_cam_angle.TabIndex = 0;
            this.v_vic_cam_angle.Visible = false;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 23);
            this.label14.TabIndex = 0;
            this.label14.Visible = false;
            // 
            // v_vic_cam_pos
            // 
            this.v_vic_cam_pos.Location = new System.Drawing.Point(0, 0);
            this.v_vic_cam_pos.Name = "v_vic_cam_pos";
            this.v_vic_cam_pos.Size = new System.Drawing.Size(120, 20);
            this.v_vic_cam_pos.TabIndex = 0;
            this.v_vic_cam_pos.Visible = false;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 23);
            this.label15.TabIndex = 0;
            this.label15.Visible = false;
            // 
            // v_cam_unk
            // 
            this.v_cam_unk.Location = new System.Drawing.Point(0, 0);
            this.v_cam_unk.Name = "v_cam_unk";
            this.v_cam_unk.Size = new System.Drawing.Size(120, 20);
            this.v_cam_unk.TabIndex = 0;
            this.v_cam_unk.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(309, 456);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(1, 1);
            this.button4.TabIndex = 5;
            this.button4.Visible = false;
            // 
            // inlineSettingsPanel
            // 
            this.inlineSettingsPanel.AutoScroll = true;
            this.inlineSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inlineSettingsPanel.Controls.Add(this.settingsTitleLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseHeaderLabel);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeHeaderLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseMovementLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseMovement);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeMovement);
            this.inlineSettingsPanel.Controls.Add(this.setBaseChakraDashLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseChakraDash);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeChakraDash);
            this.inlineSettingsPanel.Controls.Add(this.setGuardPressureLabel);
            this.inlineSettingsPanel.Controls.Add(this.setGuardPressure);
            this.inlineSettingsPanel.Controls.Add(this.setAttackLabel);
            this.inlineSettingsPanel.Controls.Add(this.setAttack);
            this.inlineSettingsPanel.Controls.Add(this.setDefenseLabel);
            this.inlineSettingsPanel.Controls.Add(this.setDefense);
            this.inlineSettingsPanel.Controls.Add(this.setAssistDamageLabel);
            this.inlineSettingsPanel.Controls.Add(this.setAssistDamage);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeningActionChargeLabel);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeningActionCharge);
            this.inlineSettingsPanel.Controls.Add(this.setChakraChargeLabel);
            this.inlineSettingsPanel.Controls.Add(this.setChakraCharge);
            this.inlineSettingsPanel.Controls.Add(this.setBaseNinjaDashLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseNinjaDash);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeNinjaDash);
            this.inlineSettingsPanel.Controls.Add(this.setBaseAirDashDurationLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseAirDashDuration);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeAirDashDuration);
            this.inlineSettingsPanel.Controls.Add(this.setBaseGroundedChakraDashDurationLabel);
            this.inlineSettingsPanel.Controls.Add(this.setBaseGroundedChakraDashDuration);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeGroundedChakraDashDuration);
            this.inlineSettingsPanel.Controls.Add(this.setItemBuffDurationLabel);
            this.inlineSettingsPanel.Controls.Add(this.setItemBuffDuration);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeHpRequirementLabel);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeHpRequirement);
            this.inlineSettingsPanel.Controls.Add(this.setChakraCostAwakeningLabel);
            this.inlineSettingsPanel.Controls.Add(this.setChakraCostAwakening);
            this.inlineSettingsPanel.Controls.Add(this.setChakraBlockRecoveryLabel);
            this.inlineSettingsPanel.Controls.Add(this.setChakraBlockRecovery);
            this.inlineSettingsPanel.Controls.Add(this.setEvo1Label);
            this.inlineSettingsPanel.Controls.Add(this.setEvo1);
            this.inlineSettingsPanel.Controls.Add(this.setAwaBodyPriorityLabel);
            this.inlineSettingsPanel.Controls.Add(this.setAwaBodyPriority);
            this.inlineSettingsPanel.Controls.Add(this.setDefaultAwaSkillIndexLabel);
            this.inlineSettingsPanel.Controls.Add(this.setDefaultAwaSkillIndex);
            this.inlineSettingsPanel.Controls.Add(this.setCameraDistanceLabel);
            this.inlineSettingsPanel.Controls.Add(this.setCameraDistance);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown1Label);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown1);
            this.inlineSettingsPanel.Controls.Add(this.setVictoryCameraAngleLabel);
            this.inlineSettingsPanel.Controls.Add(this.setVictoryCameraAngle);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown2Label);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown2);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown3Label);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown3);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown4Label);
            this.inlineSettingsPanel.Controls.Add(this.setCameraUnknown4);
            this.inlineSettingsPanel.Controls.Add(this.setEnableDashPriority);
            this.inlineSettingsPanel.Controls.Add(this.setAwakeningDebuff);
            this.inlineSettingsPanel.Location = new System.Drawing.Point(670, 54);
            this.inlineSettingsPanel.Name = "inlineSettingsPanel";
            this.inlineSettingsPanel.Size = new System.Drawing.Size(578, 769);
            this.inlineSettingsPanel.TabIndex = 0;
            // 
            // settingsTitleLabel
            // 
            this.settingsTitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.settingsTitleLabel.Location = new System.Drawing.Point(12, 10);
            this.settingsTitleLabel.Name = "settingsTitleLabel";
            this.settingsTitleLabel.Size = new System.Drawing.Size(200, 20);
            this.settingsTitleLabel.TabIndex = 0;
            this.settingsTitleLabel.Text = "Battle settings";
            this.settingsTitleLabel.Click += new System.EventHandler(this.settingsTitleLabel_Click);
            // 
            // setBaseHeaderLabel
            // 
            this.setBaseHeaderLabel.Location = new System.Drawing.Point(320, 12);
            this.setBaseHeaderLabel.Name = "setBaseHeaderLabel";
            this.setBaseHeaderLabel.Size = new System.Drawing.Size(50, 15);
            this.setBaseHeaderLabel.TabIndex = 1;
            this.setBaseHeaderLabel.Text = "Base";
            // 
            // setAwakeHeaderLabel
            // 
            this.setAwakeHeaderLabel.Location = new System.Drawing.Point(438, 12);
            this.setAwakeHeaderLabel.Name = "setAwakeHeaderLabel";
            this.setAwakeHeaderLabel.Size = new System.Drawing.Size(80, 15);
            this.setAwakeHeaderLabel.TabIndex = 2;
            this.setAwakeHeaderLabel.Text = "Awake";
            // 
            // setBaseMovementLabel
            // 
            this.setBaseMovementLabel.Location = new System.Drawing.Point(12, 44);
            this.setBaseMovementLabel.Name = "setBaseMovementLabel";
            this.setBaseMovementLabel.Size = new System.Drawing.Size(290, 15);
            this.setBaseMovementLabel.TabIndex = 3;
            this.setBaseMovementLabel.Text = "Movement speed";
            // 
            // setBaseMovement
            // 
            this.setBaseMovement.DecimalPlaces = 5;
            this.setBaseMovement.Location = new System.Drawing.Point(320, 40);
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
            this.setBaseMovement.Size = new System.Drawing.Size(110, 20);
            this.setBaseMovement.TabIndex = 4;
            // 
            // setAwakeMovement
            // 
            this.setAwakeMovement.DecimalPlaces = 5;
            this.setAwakeMovement.Location = new System.Drawing.Point(438, 40);
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
            this.setAwakeMovement.Size = new System.Drawing.Size(110, 20);
            this.setAwakeMovement.TabIndex = 5;
            // 
            // setBaseChakraDashLabel
            // 
            this.setBaseChakraDashLabel.Location = new System.Drawing.Point(12, 72);
            this.setBaseChakraDashLabel.Name = "setBaseChakraDashLabel";
            this.setBaseChakraDashLabel.Size = new System.Drawing.Size(290, 15);
            this.setBaseChakraDashLabel.TabIndex = 6;
            this.setBaseChakraDashLabel.Text = "Chakra dash speed";
            // 
            // setBaseChakraDash
            // 
            this.setBaseChakraDash.DecimalPlaces = 5;
            this.setBaseChakraDash.Location = new System.Drawing.Point(320, 68);
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
            this.setBaseChakraDash.Size = new System.Drawing.Size(110, 20);
            this.setBaseChakraDash.TabIndex = 7;
            // 
            // setAwakeChakraDash
            // 
            this.setAwakeChakraDash.DecimalPlaces = 5;
            this.setAwakeChakraDash.Location = new System.Drawing.Point(438, 68);
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
            this.setAwakeChakraDash.Size = new System.Drawing.Size(110, 20);
            this.setAwakeChakraDash.TabIndex = 8;
            // 
            // setGuardPressureLabel
            // 
            this.setGuardPressureLabel.Location = new System.Drawing.Point(12, 100);
            this.setGuardPressureLabel.Name = "setGuardPressureLabel";
            this.setGuardPressureLabel.Size = new System.Drawing.Size(290, 15);
            this.setGuardPressureLabel.TabIndex = 9;
            this.setGuardPressureLabel.Text = "Guard pressure";
            // 
            // setGuardPressure
            // 
            this.setGuardPressure.DecimalPlaces = 5;
            this.setGuardPressure.Location = new System.Drawing.Point(320, 96);
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
            this.setGuardPressure.Size = new System.Drawing.Size(110, 20);
            this.setGuardPressure.TabIndex = 10;
            // 
            // setAttackLabel
            // 
            this.setAttackLabel.Location = new System.Drawing.Point(12, 128);
            this.setAttackLabel.Name = "setAttackLabel";
            this.setAttackLabel.Size = new System.Drawing.Size(290, 15);
            this.setAttackLabel.TabIndex = 11;
            this.setAttackLabel.Text = "Attack";
            // 
            // setAttack
            // 
            this.setAttack.DecimalPlaces = 5;
            this.setAttack.Location = new System.Drawing.Point(320, 124);
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
            this.setAttack.Size = new System.Drawing.Size(110, 20);
            this.setAttack.TabIndex = 12;
            // 
            // setDefenseLabel
            // 
            this.setDefenseLabel.Location = new System.Drawing.Point(12, 156);
            this.setDefenseLabel.Name = "setDefenseLabel";
            this.setDefenseLabel.Size = new System.Drawing.Size(290, 15);
            this.setDefenseLabel.TabIndex = 13;
            this.setDefenseLabel.Text = "Defense";
            // 
            // setDefense
            // 
            this.setDefense.DecimalPlaces = 5;
            this.setDefense.Location = new System.Drawing.Point(320, 152);
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
            this.setDefense.Size = new System.Drawing.Size(110, 20);
            this.setDefense.TabIndex = 14;
            // 
            // setAssistDamageLabel
            // 
            this.setAssistDamageLabel.Location = new System.Drawing.Point(12, 184);
            this.setAssistDamageLabel.Name = "setAssistDamageLabel";
            this.setAssistDamageLabel.Size = new System.Drawing.Size(290, 15);
            this.setAssistDamageLabel.TabIndex = 15;
            this.setAssistDamageLabel.Text = "Assist damage";
            // 
            // setAssistDamage
            // 
            this.setAssistDamage.DecimalPlaces = 5;
            this.setAssistDamage.Location = new System.Drawing.Point(320, 180);
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
            this.setAssistDamage.Size = new System.Drawing.Size(110, 20);
            this.setAssistDamage.TabIndex = 16;
            // 
            // setAwakeningActionChargeLabel
            // 
            this.setAwakeningActionChargeLabel.Location = new System.Drawing.Point(12, 212);
            this.setAwakeningActionChargeLabel.Name = "setAwakeningActionChargeLabel";
            this.setAwakeningActionChargeLabel.Size = new System.Drawing.Size(290, 15);
            this.setAwakeningActionChargeLabel.TabIndex = 17;
            this.setAwakeningActionChargeLabel.Text = "Awakening action charge";
            // 
            // setAwakeningActionCharge
            // 
            this.setAwakeningActionCharge.DecimalPlaces = 5;
            this.setAwakeningActionCharge.Location = new System.Drawing.Point(320, 208);
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
            this.setAwakeningActionCharge.Size = new System.Drawing.Size(110, 20);
            this.setAwakeningActionCharge.TabIndex = 18;
            // 
            // setChakraChargeLabel
            // 
            this.setChakraChargeLabel.Location = new System.Drawing.Point(12, 240);
            this.setChakraChargeLabel.Name = "setChakraChargeLabel";
            this.setChakraChargeLabel.Size = new System.Drawing.Size(290, 15);
            this.setChakraChargeLabel.TabIndex = 19;
            this.setChakraChargeLabel.Text = "Chakra charge speed";
            // 
            // setChakraCharge
            // 
            this.setChakraCharge.DecimalPlaces = 5;
            this.setChakraCharge.Location = new System.Drawing.Point(320, 236);
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
            this.setChakraCharge.Size = new System.Drawing.Size(110, 20);
            this.setChakraCharge.TabIndex = 20;
            // 
            // setBaseNinjaDashLabel
            // 
            this.setBaseNinjaDashLabel.Location = new System.Drawing.Point(12, 268);
            this.setBaseNinjaDashLabel.Name = "setBaseNinjaDashLabel";
            this.setBaseNinjaDashLabel.Size = new System.Drawing.Size(290, 15);
            this.setBaseNinjaDashLabel.TabIndex = 21;
            this.setBaseNinjaDashLabel.Text = "Ninja dash speed";
            // 
            // setBaseNinjaDash
            // 
            this.setBaseNinjaDash.Hexadecimal = true;
            this.setBaseNinjaDash.Location = new System.Drawing.Point(320, 264);
            this.setBaseNinjaDash.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.setBaseNinjaDash.Name = "setBaseNinjaDash";
            this.setBaseNinjaDash.Size = new System.Drawing.Size(110, 20);
            this.setBaseNinjaDash.TabIndex = 22;
            // 
            // setAwakeNinjaDash
            // 
            this.setAwakeNinjaDash.Hexadecimal = true;
            this.setAwakeNinjaDash.Location = new System.Drawing.Point(438, 264);
            this.setAwakeNinjaDash.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.setAwakeNinjaDash.Name = "setAwakeNinjaDash";
            this.setAwakeNinjaDash.Size = new System.Drawing.Size(110, 20);
            this.setAwakeNinjaDash.TabIndex = 23;
            // 
            // setBaseAirDashDurationLabel
            // 
            this.setBaseAirDashDurationLabel.Location = new System.Drawing.Point(12, 296);
            this.setBaseAirDashDurationLabel.Name = "setBaseAirDashDurationLabel";
            this.setBaseAirDashDurationLabel.Size = new System.Drawing.Size(290, 15);
            this.setBaseAirDashDurationLabel.TabIndex = 24;
            this.setBaseAirDashDurationLabel.Text = "Air dash duration";
            // 
            // setBaseAirDashDuration
            // 
            this.setBaseAirDashDuration.Hexadecimal = true;
            this.setBaseAirDashDuration.Location = new System.Drawing.Point(320, 292);
            this.setBaseAirDashDuration.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.setBaseAirDashDuration.Name = "setBaseAirDashDuration";
            this.setBaseAirDashDuration.Size = new System.Drawing.Size(110, 20);
            this.setBaseAirDashDuration.TabIndex = 25;
            // 
            // setAwakeAirDashDuration
            // 
            this.setAwakeAirDashDuration.Hexadecimal = true;
            this.setAwakeAirDashDuration.Location = new System.Drawing.Point(438, 292);
            this.setAwakeAirDashDuration.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.setAwakeAirDashDuration.Name = "setAwakeAirDashDuration";
            this.setAwakeAirDashDuration.Size = new System.Drawing.Size(110, 20);
            this.setAwakeAirDashDuration.TabIndex = 26;
            // 
            // setBaseGroundedChakraDashDurationLabel
            // 
            this.setBaseGroundedChakraDashDurationLabel.Location = new System.Drawing.Point(12, 324);
            this.setBaseGroundedChakraDashDurationLabel.Name = "setBaseGroundedChakraDashDurationLabel";
            this.setBaseGroundedChakraDashDurationLabel.Size = new System.Drawing.Size(290, 15);
            this.setBaseGroundedChakraDashDurationLabel.TabIndex = 27;
            this.setBaseGroundedChakraDashDurationLabel.Text = "Grounded chakra dash dur";
            // 
            // setBaseGroundedChakraDashDuration
            // 
            this.setBaseGroundedChakraDashDuration.Hexadecimal = true;
            this.setBaseGroundedChakraDashDuration.Location = new System.Drawing.Point(320, 320);
            this.setBaseGroundedChakraDashDuration.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.setBaseGroundedChakraDashDuration.Name = "setBaseGroundedChakraDashDuration";
            this.setBaseGroundedChakraDashDuration.Size = new System.Drawing.Size(110, 20);
            this.setBaseGroundedChakraDashDuration.TabIndex = 28;
            // 
            // setAwakeGroundedChakraDashDuration
            // 
            this.setAwakeGroundedChakraDashDuration.Hexadecimal = true;
            this.setAwakeGroundedChakraDashDuration.Location = new System.Drawing.Point(438, 320);
            this.setAwakeGroundedChakraDashDuration.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.setAwakeGroundedChakraDashDuration.Name = "setAwakeGroundedChakraDashDuration";
            this.setAwakeGroundedChakraDashDuration.Size = new System.Drawing.Size(110, 20);
            this.setAwakeGroundedChakraDashDuration.TabIndex = 29;
            // 
            // setItemBuffDurationLabel
            // 
            this.setItemBuffDurationLabel.Location = new System.Drawing.Point(12, 352);
            this.setItemBuffDurationLabel.Name = "setItemBuffDurationLabel";
            this.setItemBuffDurationLabel.Size = new System.Drawing.Size(290, 15);
            this.setItemBuffDurationLabel.TabIndex = 30;
            this.setItemBuffDurationLabel.Text = "Item buff duration";
            // 
            // setItemBuffDuration
            // 
            this.setItemBuffDuration.DecimalPlaces = 5;
            this.setItemBuffDuration.Location = new System.Drawing.Point(320, 348);
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
            this.setItemBuffDuration.Size = new System.Drawing.Size(110, 20);
            this.setItemBuffDuration.TabIndex = 31;
            // 
            // setAwakeHpRequirementLabel
            // 
            this.setAwakeHpRequirementLabel.Location = new System.Drawing.Point(12, 380);
            this.setAwakeHpRequirementLabel.Name = "setAwakeHpRequirementLabel";
            this.setAwakeHpRequirementLabel.Size = new System.Drawing.Size(290, 15);
            this.setAwakeHpRequirementLabel.TabIndex = 32;
            this.setAwakeHpRequirementLabel.Text = "Awake HP requirement";
            // 
            // setAwakeHpRequirement
            // 
            this.setAwakeHpRequirement.DecimalPlaces = 5;
            this.setAwakeHpRequirement.Location = new System.Drawing.Point(320, 376);
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
            this.setAwakeHpRequirement.Size = new System.Drawing.Size(110, 20);
            this.setAwakeHpRequirement.TabIndex = 33;
            // 
            // setChakraCostAwakeningLabel
            // 
            this.setChakraCostAwakeningLabel.Location = new System.Drawing.Point(12, 408);
            this.setChakraCostAwakeningLabel.Name = "setChakraCostAwakeningLabel";
            this.setChakraCostAwakeningLabel.Size = new System.Drawing.Size(290, 15);
            this.setChakraCostAwakeningLabel.TabIndex = 34;
            this.setChakraCostAwakeningLabel.Text = "Chakra cost awakening";
            // 
            // setChakraCostAwakening
            // 
            this.setChakraCostAwakening.DecimalPlaces = 5;
            this.setChakraCostAwakening.Location = new System.Drawing.Point(320, 404);
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
            this.setChakraCostAwakening.Size = new System.Drawing.Size(110, 20);
            this.setChakraCostAwakening.TabIndex = 35;
            // 
            // setChakraBlockRecoveryLabel
            // 
            this.setChakraBlockRecoveryLabel.Location = new System.Drawing.Point(12, 436);
            this.setChakraBlockRecoveryLabel.Name = "setChakraBlockRecoveryLabel";
            this.setChakraBlockRecoveryLabel.Size = new System.Drawing.Size(290, 15);
            this.setChakraBlockRecoveryLabel.TabIndex = 36;
            this.setChakraBlockRecoveryLabel.Text = "Chakra block recovery";
            // 
            // setChakraBlockRecovery
            // 
            this.setChakraBlockRecovery.DecimalPlaces = 5;
            this.setChakraBlockRecovery.Location = new System.Drawing.Point(320, 432);
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
            this.setChakraBlockRecovery.Size = new System.Drawing.Size(110, 20);
            this.setChakraBlockRecovery.TabIndex = 37;
            // 
            // setEvo1Label
            // 
            this.setEvo1Label.Location = new System.Drawing.Point(12, 464);
            this.setEvo1Label.Name = "setEvo1Label";
            this.setEvo1Label.Size = new System.Drawing.Size(290, 15);
            this.setEvo1Label.TabIndex = 38;
            this.setEvo1Label.Text = "Evo Dup";
            // 
            // setEvo1
            // 
            this.setEvo1.Location = new System.Drawing.Point(320, 460);
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
            this.setEvo1.Size = new System.Drawing.Size(110, 20);
            this.setEvo1.TabIndex = 39;
            // 
            // setAwaBodyPriorityLabel
            // 
            this.setAwaBodyPriorityLabel.Location = new System.Drawing.Point(12, 493);
            this.setAwaBodyPriorityLabel.Name = "setAwaBodyPriorityLabel";
            this.setAwaBodyPriorityLabel.Size = new System.Drawing.Size(290, 15);
            this.setAwaBodyPriorityLabel.TabIndex = 40;
            this.setAwaBodyPriorityLabel.Text = "Awa body priority";
            // 
            // setAwaBodyPriority
            // 
            this.setAwaBodyPriority.Location = new System.Drawing.Point(320, 489);
            this.setAwaBodyPriority.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.setAwaBodyPriority.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.setAwaBodyPriority.Name = "setAwaBodyPriority";
            this.setAwaBodyPriority.Size = new System.Drawing.Size(110, 20);
            this.setAwaBodyPriority.TabIndex = 41;
            // 
            // setDefaultAwaSkillIndexLabel
            // 
            this.setDefaultAwaSkillIndexLabel.Location = new System.Drawing.Point(12, 521);
            this.setDefaultAwaSkillIndexLabel.Name = "setDefaultAwaSkillIndexLabel";
            this.setDefaultAwaSkillIndexLabel.Size = new System.Drawing.Size(290, 15);
            this.setDefaultAwaSkillIndexLabel.TabIndex = 42;
            this.setDefaultAwaSkillIndexLabel.Text = "Default awa skill index";
            // 
            // setDefaultAwaSkillIndex
            // 
            this.setDefaultAwaSkillIndex.Location = new System.Drawing.Point(320, 517);
            this.setDefaultAwaSkillIndex.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.setDefaultAwaSkillIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.setDefaultAwaSkillIndex.Name = "setDefaultAwaSkillIndex";
            this.setDefaultAwaSkillIndex.Size = new System.Drawing.Size(110, 20);
            this.setDefaultAwaSkillIndex.TabIndex = 43;
            // 
            // setCameraDistanceLabel
            // 
            this.setCameraDistanceLabel.Location = new System.Drawing.Point(12, 549);
            this.setCameraDistanceLabel.Name = "setCameraDistanceLabel";
            this.setCameraDistanceLabel.Size = new System.Drawing.Size(290, 15);
            this.setCameraDistanceLabel.TabIndex = 44;
            this.setCameraDistanceLabel.Text = "Camera distance";
            // 
            // setCameraDistance
            // 
            this.setCameraDistance.Location = new System.Drawing.Point(320, 545);
            this.setCameraDistance.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.setCameraDistance.Name = "setCameraDistance";
            this.setCameraDistance.Size = new System.Drawing.Size(110, 20);
            this.setCameraDistance.TabIndex = 45;
            // 
            // setCameraUnknown1Label
            // 
            this.setCameraUnknown1Label.Location = new System.Drawing.Point(12, 577);
            this.setCameraUnknown1Label.Name = "setCameraUnknown1Label";
            this.setCameraUnknown1Label.Size = new System.Drawing.Size(290, 15);
            this.setCameraUnknown1Label.TabIndex = 46;
            this.setCameraUnknown1Label.Text = "Camera unk 1";
            // 
            // setCameraUnknown1
            // 
            this.setCameraUnknown1.Location = new System.Drawing.Point(320, 573);
            this.setCameraUnknown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.setCameraUnknown1.Name = "setCameraUnknown1";
            this.setCameraUnknown1.Size = new System.Drawing.Size(110, 20);
            this.setCameraUnknown1.TabIndex = 47;
            // 
            // setVictoryCameraAngleLabel
            // 
            this.setVictoryCameraAngleLabel.Location = new System.Drawing.Point(12, 605);
            this.setVictoryCameraAngleLabel.Name = "setVictoryCameraAngleLabel";
            this.setVictoryCameraAngleLabel.Size = new System.Drawing.Size(290, 15);
            this.setVictoryCameraAngleLabel.TabIndex = 48;
            this.setVictoryCameraAngleLabel.Text = "Victory camera angle";
            // 
            // setVictoryCameraAngle
            // 
            this.setVictoryCameraAngle.Location = new System.Drawing.Point(320, 601);
            this.setVictoryCameraAngle.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.setVictoryCameraAngle.Name = "setVictoryCameraAngle";
            this.setVictoryCameraAngle.Size = new System.Drawing.Size(110, 20);
            this.setVictoryCameraAngle.TabIndex = 49;
            // 
            // setCameraUnknown2Label
            // 
            this.setCameraUnknown2Label.Location = new System.Drawing.Point(12, 633);
            this.setCameraUnknown2Label.Name = "setCameraUnknown2Label";
            this.setCameraUnknown2Label.Size = new System.Drawing.Size(290, 15);
            this.setCameraUnknown2Label.TabIndex = 50;
            this.setCameraUnknown2Label.Text = "Camera unk 2";
            // 
            // setCameraUnknown2
            // 
            this.setCameraUnknown2.Location = new System.Drawing.Point(320, 629);
            this.setCameraUnknown2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.setCameraUnknown2.Name = "setCameraUnknown2";
            this.setCameraUnknown2.Size = new System.Drawing.Size(110, 20);
            this.setCameraUnknown2.TabIndex = 51;
            // 
            // setCameraUnknown3Label
            // 
            this.setCameraUnknown3Label.Location = new System.Drawing.Point(12, 661);
            this.setCameraUnknown3Label.Name = "setCameraUnknown3Label";
            this.setCameraUnknown3Label.Size = new System.Drawing.Size(290, 15);
            this.setCameraUnknown3Label.TabIndex = 52;
            this.setCameraUnknown3Label.Text = "Camera unk 3";
            // 
            // setCameraUnknown3
            // 
            this.setCameraUnknown3.Location = new System.Drawing.Point(320, 657);
            this.setCameraUnknown3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.setCameraUnknown3.Name = "setCameraUnknown3";
            this.setCameraUnknown3.Size = new System.Drawing.Size(110, 20);
            this.setCameraUnknown3.TabIndex = 53;
            // 
            // setCameraUnknown4Label
            // 
            this.setCameraUnknown4Label.Location = new System.Drawing.Point(12, 689);
            this.setCameraUnknown4Label.Name = "setCameraUnknown4Label";
            this.setCameraUnknown4Label.Size = new System.Drawing.Size(290, 15);
            this.setCameraUnknown4Label.TabIndex = 54;
            this.setCameraUnknown4Label.Text = "Camera unk 4";
            // 
            // setCameraUnknown4
            // 
            this.setCameraUnknown4.Location = new System.Drawing.Point(320, 685);
            this.setCameraUnknown4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.setCameraUnknown4.Name = "setCameraUnknown4";
            this.setCameraUnknown4.Size = new System.Drawing.Size(110, 20);
            this.setCameraUnknown4.TabIndex = 55;
            // 
            // setEnableDashPriority
            // 
            this.setEnableDashPriority.Location = new System.Drawing.Point(12, 717);
            this.setEnableDashPriority.Name = "setEnableDashPriority";
            this.setEnableDashPriority.Size = new System.Drawing.Size(220, 19);
            this.setEnableDashPriority.TabIndex = 56;
            this.setEnableDashPriority.Text = "Enable chakra dash priority";
            // 
            // setAwakeningDebuff
            // 
            this.setAwakeningDebuff.Location = new System.Drawing.Point(12, 743);
            this.setAwakeningDebuff.Name = "setAwakeningDebuff";
            this.setAwakeningDebuff.Size = new System.Drawing.Size(220, 19);
            this.setAwakeningDebuff.TabIndex = 57;
            this.setAwakeningDebuff.Text = "Enable awakening debuff";
            // 
            // setEvo2Label
            // 
            this.setEvo2Label.Location = new System.Drawing.Point(0, 0);
            this.setEvo2Label.Name = "setEvo2Label";
            this.setEvo2Label.Size = new System.Drawing.Size(100, 23);
            this.setEvo2Label.TabIndex = 0;
            this.setEvo2Label.Visible = false;
            // 
            // setEvo2
            // 
            this.setEvo2.Location = new System.Drawing.Point(0, 0);
            this.setEvo2.Name = "setEvo2";
            this.setEvo2.Size = new System.Drawing.Size(120, 20);
            this.setEvo2.TabIndex = 0;
            this.setEvo2.Visible = false;
            // 
            // checkedListConditionFlags
            // 
            this.checkedListConditionFlags.CheckOnClick = true;
            this.checkedListConditionFlags.Location = new System.Drawing.Point(12, 40);
            this.checkedListConditionFlags.Name = "checkedListConditionFlags";
            this.checkedListConditionFlags.Size = new System.Drawing.Size(320, 454);
            this.checkedListConditionFlags.TabIndex = 1;
            this.checkedListConditionFlags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListConditionFlags_ItemCheck);
            this.checkedListConditionFlags.SelectedIndexChanged += new System.EventHandler(this.checkedListConditionFlags_SelectedIndexChanged);
            // 
            // groupConditionFlags
            // 
            this.groupConditionFlags.Controls.Add(this.checkedListConditionFlags);
            this.groupConditionFlags.Controls.Add(this.label11);
            this.groupConditionFlags.Controls.Add(this.v_enableAwaSkill);
            this.groupConditionFlags.Location = new System.Drawing.Point(309, 325);
            this.groupConditionFlags.Name = "groupConditionFlags";
            this.groupConditionFlags.Size = new System.Drawing.Size(344, 498);
            this.groupConditionFlags.TabIndex = 1;
            this.groupConditionFlags.TabStop = false;
            this.groupConditionFlags.Enter += new System.EventHandler(this.groupConditionFlags_Enter);
            // 
            // Tool_DuelPlayerParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 828);
            this.Controls.Add(this.inlineSettingsPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.w_awkaction);
            this.Controls.Add(this.groupConditionFlags);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.w_partner);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.w_defaultassist2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.w_defaultassist1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.w_itemc4);
            this.Controls.Add(this.w_item4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.w_itemc3);
            this.Controls.Add(this.w_item3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.w_itemc2);
            this.Controls.Add(this.w_item2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.w_itemc1);
            this.Controls.Add(this.w_item1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.b_awkcostumeids);
            this.Controls.Add(this.b_costumeids);
            this.Controls.Add(this.w_characodeid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.w_charaprmbas);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pasteSettingsButton);
            this.Controls.Add(this.copySettingsButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.Search_TB);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1276, 799);
            this.Name = "Tool_DuelPlayerParamEditor";
            this.Text = "DuelPlayerParam Editor";
            this.Load += new System.EventHandler(this.Tool_DuelPlayerParamEditor_Load_1);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_itemc4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_enableAwaSkill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_vic_cam_angle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_vic_cam_pos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.v_cam_unk)).EndInit();
            this.inlineSettingsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.setBaseMovement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeMovement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseChakraDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeChakraDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setGuardPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAttack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setDefense)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAssistDamage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeningActionCharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseNinjaDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeNinjaDash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseAirDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeAirDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setBaseGroundedChakraDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeGroundedChakraDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setItemBuffDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwakeHpRequirement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraCostAwakening)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setChakraBlockRecovery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setEvo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setAwaBodyPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setDefaultAwaSkillIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setVictoryCameraAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCameraUnknown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setEvo2)).EndInit();
            this.groupConditionFlags.ResumeLayout(false);
            this.groupConditionFlags.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private CheckedListBox checkedListConditionFlags;
        private GroupBox groupConditionFlags;
    }
}



