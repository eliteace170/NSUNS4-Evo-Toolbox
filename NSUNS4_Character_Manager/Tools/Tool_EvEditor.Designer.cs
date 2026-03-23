using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_EvEditor
    {
        private IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem sortToolStripMenuItem;
        private ToolStripMenuItem infoToolStripMenuItem;
        private TabControl mainTabControl;
        private TabPage battleTabPage;
        private TabPage ultimateTabPage;
        private ListBox battleListBox;
        private Button battleAddButton;
        private Button battleDuplicateButton;
        private Button battleDeleteButton;
        private Button battleSaveSelectedButton;
        private Button battleCopyButton;
        private Button battlePasteButton;
        private TextBox battleSearchText;
        private Button battleSearchButton;
        private Panel battleEditPanel;
        private Label battleLabelPan;
        private TextBox battleSoundPlText;
        private NumericUpDown battlePanValue;
        private NumericUpDown battleVolumeValue;
        private NumericUpDown battlePitchValue;
        private ComboBox battleEventTypeValue;
        private NumericUpDown battleSoundDelayValue;
        private NumericUpDown battleHighPassValue;
        private NumericUpDown battleLowPassValue;
        private TextBox battleSoundXfbinText;
        private TextBox battleAnimationNameText;
        private TextBox battleHitboxText;
        private NumericUpDown battleLocXValue;
        private NumericUpDown battleLocYValue;
        private NumericUpDown battleLocZValue;
        private NumericUpDown battleSoundDelayResetValue;
        private NumericUpDown battleUnknownValue;
        private NumericUpDown battleSoundDelayResetAnmValue;
        private CheckBox battleLoopFlagValue;
        private TextBox battlePlAnmText;
        private ListBox ultimateChunkListBox;
        private ListBox ultimateEntryListBox;
        private Button ultimateAddEntryButton;
        private Button ultimateDuplicateEntryButton;
        private Button ultimateDeleteEntryButton;
        private Button ultimateSaveSelectedButton;
        private Button ultimateCopyButton;
        private Button ultimatePasteButton;
        private TextBox ultimateSearchText;
        private Button ultimateSearchButton;
        private Panel ultimateEditPanel;
        private Label ultimateLabelSoundPl;
        private Label ultimateLabelPan;
        private Label ultimateLabelVolume;
        private Label ultimateLabelPitch;
        private Label ultimateLabelEventType;
        private Label ultimateLabelSoundDelay;
        private Label ultimateLabelHighPass;
        private Label ultimateLabelLowPass;
        private Label ultimateLabelIndex;
        private Label ultimateLabelSoundCutframe;
        private Label ultimateLabelUnknown;
        private Label ultimateLabelFadeParam;
        private TextBox ultimateSoundPlText;
        private NumericUpDown ultimatePanValue;
        private NumericUpDown ultimateVolumeValue;
        private NumericUpDown ultimatePitchValue;
        private ComboBox ultimateEventTypeValue;
        private NumericUpDown ultimateSoundDelayValue;
        private NumericUpDown ultimateHighPassValue;
        private NumericUpDown ultimateLowPassValue;
        private NumericUpDown ultimateIndexValue;
        private NumericUpDown ultimateSoundCutframeValue;
        private NumericUpDown ultimateUnknownValue;
        private NumericUpDown ultimateFadeParamValue;

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.battleTabPage = new System.Windows.Forms.TabPage();
            this.battleListBox = new System.Windows.Forms.ListBox();
            this.battleSearchText = new System.Windows.Forms.TextBox();
            this.battleEditPanel = new System.Windows.Forms.Panel();
            this.battleLabelSoundPl = new System.Windows.Forms.Label();
            this.battleLabelPan = new System.Windows.Forms.Label();
            this.battleSoundDelayResetAnmValue = new System.Windows.Forms.NumericUpDown();
            this.battleLabelLocX = new System.Windows.Forms.Label();
            this.battleUnknownValue = new System.Windows.Forms.NumericUpDown();
            this.battleLabelVolume = new System.Windows.Forms.Label();
            this.battleSoundDelayResetValue = new System.Windows.Forms.NumericUpDown();
            this.battleLabelSoundXfbin = new System.Windows.Forms.Label();
            this.battleLabelSoundDelayResetAnm = new System.Windows.Forms.Label();
            this.battleLabelAnimationName = new System.Windows.Forms.Label();
            this.battleLabelUnknown = new System.Windows.Forms.Label();
            this.battleLocZValue = new System.Windows.Forms.NumericUpDown();
            this.battleLabelSoundDelayReset = new System.Windows.Forms.Label();
            this.battleLabelPitch = new System.Windows.Forms.Label();
            this.battleLocYValue = new System.Windows.Forms.NumericUpDown();
            this.battleLabelHitbox = new System.Windows.Forms.Label();
            this.battleLocXValue = new System.Windows.Forms.NumericUpDown();
            this.battleLabelLoopFlag = new System.Windows.Forms.Label();
            this.battleLabelPlAnm = new System.Windows.Forms.Label();
            this.battleLabelSoundDelay = new System.Windows.Forms.Label();
            this.battleLabelEventType = new System.Windows.Forms.Label();
            this.battleLabelLocY = new System.Windows.Forms.Label();
            this.battleSoundPlText = new System.Windows.Forms.TextBox();
            this.battleLabelLocZ = new System.Windows.Forms.Label();
            this.battleLabelHighPass = new System.Windows.Forms.Label();
            this.battleSoundXfbinText = new System.Windows.Forms.TextBox();
            this.battleAnimationNameText = new System.Windows.Forms.TextBox();
            this.battleLabelLowPass = new System.Windows.Forms.Label();
            this.battleHitboxText = new System.Windows.Forms.TextBox();
            this.battleLoopFlagValue = new System.Windows.Forms.CheckBox();
            this.battlePlAnmText = new System.Windows.Forms.TextBox();
            this.battleEventTypeValue = new System.Windows.Forms.ComboBox();
            this.battleSoundDelayValue = new System.Windows.Forms.NumericUpDown();
            this.battleHighPassValue = new System.Windows.Forms.NumericUpDown();
            this.battlePitchValue = new System.Windows.Forms.NumericUpDown();
            this.battleLowPassValue = new System.Windows.Forms.NumericUpDown();
            this.battlePanValue = new System.Windows.Forms.NumericUpDown();
            this.battleVolumeValue = new System.Windows.Forms.NumericUpDown();
            this.battleSearchButton = new System.Windows.Forms.Button();
            this.battleAddButton = new System.Windows.Forms.Button();
            this.battleDuplicateButton = new System.Windows.Forms.Button();
            this.battleDeleteButton = new System.Windows.Forms.Button();
            this.battleSaveSelectedButton = new System.Windows.Forms.Button();
            this.battleCopyButton = new System.Windows.Forms.Button();
            this.battlePasteButton = new System.Windows.Forms.Button();
            this.ultimateTabPage = new System.Windows.Forms.TabPage();
            this.ultimateChunkListBox = new System.Windows.Forms.ListBox();
            this.ultimateEntryListBox = new System.Windows.Forms.ListBox();
            this.ultimateAddEntryButton = new System.Windows.Forms.Button();
            this.ultimateDuplicateEntryButton = new System.Windows.Forms.Button();
            this.ultimateDeleteEntryButton = new System.Windows.Forms.Button();
            this.ultimateSaveSelectedButton = new System.Windows.Forms.Button();
            this.ultimateCopyButton = new System.Windows.Forms.Button();
            this.ultimatePasteButton = new System.Windows.Forms.Button();
            this.ultimateSearchText = new System.Windows.Forms.TextBox();
            this.ultimateSearchButton = new System.Windows.Forms.Button();
            this.ultimateEditPanel = new System.Windows.Forms.Panel();
            this.ultimateLabelSoundPl = new System.Windows.Forms.Label();
            this.ultimateLabelPan = new System.Windows.Forms.Label();
            this.ultimateLabelVolume = new System.Windows.Forms.Label();
            this.ultimateLabelPitch = new System.Windows.Forms.Label();
            this.ultimateLabelEventType = new System.Windows.Forms.Label();
            this.ultimateLabelSoundDelay = new System.Windows.Forms.Label();
            this.ultimateLabelHighPass = new System.Windows.Forms.Label();
            this.ultimateLabelLowPass = new System.Windows.Forms.Label();
            this.ultimateLabelIndex = new System.Windows.Forms.Label();
            this.ultimateLabelSoundCutframe = new System.Windows.Forms.Label();
            this.ultimateLabelUnknown = new System.Windows.Forms.Label();
            this.ultimateLabelFadeParam = new System.Windows.Forms.Label();
            this.ultimateSoundPlText = new System.Windows.Forms.TextBox();
            this.ultimatePanValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateVolumeValue = new System.Windows.Forms.NumericUpDown();
            this.ultimatePitchValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateEventTypeValue = new System.Windows.Forms.ComboBox();
            this.ultimateSoundDelayValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateHighPassValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateLowPassValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateIndexValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateSoundCutframeValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateUnknownValue = new System.Windows.Forms.NumericUpDown();
            this.ultimateFadeParamValue = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.battleTabPage.SuspendLayout();
            this.battleEditPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.battleSoundDelayResetAnmValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleUnknownValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleSoundDelayResetValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLocZValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLocYValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLocXValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleSoundDelayValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleHighPassValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battlePitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLowPassValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battlePanValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleVolumeValue)).BeginInit();
            this.ultimateTabPage.SuspendLayout();
            this.ultimateEditPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultimatePanValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateVolumeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimatePitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateSoundDelayValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateHighPassValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateLowPassValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateIndexValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateSoundCutframeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateUnknownValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateFadeParamValue)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(783, 24);
            this.menuStrip1.TabIndex = 1;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
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
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.battleTabPage);
            this.mainTabControl.Controls.Add(this.ultimateTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(12, 32);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(770, 717);
            this.mainTabControl.TabIndex = 0;
            // 
            // battleTabPage
            // 
            this.battleTabPage.Controls.Add(this.battleListBox);
            this.battleTabPage.Controls.Add(this.battleSearchText);
            this.battleTabPage.Controls.Add(this.battleEditPanel);
            this.battleTabPage.Controls.Add(this.battleSearchButton);
            this.battleTabPage.Controls.Add(this.battleAddButton);
            this.battleTabPage.Controls.Add(this.battleDuplicateButton);
            this.battleTabPage.Controls.Add(this.battleDeleteButton);
            this.battleTabPage.Controls.Add(this.battleSaveSelectedButton);
            this.battleTabPage.Controls.Add(this.battleCopyButton);
            this.battleTabPage.Controls.Add(this.battlePasteButton);
            this.battleTabPage.Location = new System.Drawing.Point(4, 22);
            this.battleTabPage.Name = "battleTabPage";
            this.battleTabPage.Padding = new System.Windows.Forms.Padding(8);
            this.battleTabPage.Size = new System.Drawing.Size(762, 691);
            this.battleTabPage.TabIndex = 0;
            this.battleTabPage.Text = "Battle";
            // 
            // battleListBox
            // 
            this.battleListBox.Location = new System.Drawing.Point(8, 8);
            this.battleListBox.Name = "battleListBox";
            this.battleListBox.Size = new System.Drawing.Size(360, 550);
            this.battleListBox.TabIndex = 0;
            this.battleListBox.SelectedIndexChanged += new System.EventHandler(this.battleListBox_SelectedIndexChanged);
            // 
            // battleSearchText
            // 
            this.battleSearchText.Location = new System.Drawing.Point(8, 576);
            this.battleSearchText.Name = "battleSearchText";
            this.battleSearchText.Size = new System.Drawing.Size(250, 23);
            this.battleSearchText.TabIndex = 1;
            // 
            // battleEditPanel
            // 
            this.battleEditPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battleEditPanel.Controls.Add(this.battleLabelSoundPl);
            this.battleEditPanel.Controls.Add(this.battleLabelPan);
            this.battleEditPanel.Controls.Add(this.battleSoundDelayResetAnmValue);
            this.battleEditPanel.Controls.Add(this.battleLabelLocX);
            this.battleEditPanel.Controls.Add(this.battleUnknownValue);
            this.battleEditPanel.Controls.Add(this.battleLabelVolume);
            this.battleEditPanel.Controls.Add(this.battleSoundDelayResetValue);
            this.battleEditPanel.Controls.Add(this.battleLabelSoundXfbin);
            this.battleEditPanel.Controls.Add(this.battleLabelSoundDelayResetAnm);
            this.battleEditPanel.Controls.Add(this.battleLabelAnimationName);
            this.battleEditPanel.Controls.Add(this.battleLabelUnknown);
            this.battleEditPanel.Controls.Add(this.battleLocZValue);
            this.battleEditPanel.Controls.Add(this.battleLabelSoundDelayReset);
            this.battleEditPanel.Controls.Add(this.battleLabelPitch);
            this.battleEditPanel.Controls.Add(this.battleLocYValue);
            this.battleEditPanel.Controls.Add(this.battleLabelHitbox);
            this.battleEditPanel.Controls.Add(this.battleLocXValue);
            this.battleEditPanel.Controls.Add(this.battleLabelLoopFlag);
            this.battleEditPanel.Controls.Add(this.battleLabelPlAnm);
            this.battleEditPanel.Controls.Add(this.battleLabelSoundDelay);
            this.battleEditPanel.Controls.Add(this.battleLabelEventType);
            this.battleEditPanel.Controls.Add(this.battleLabelLocY);
            this.battleEditPanel.Controls.Add(this.battleSoundPlText);
            this.battleEditPanel.Controls.Add(this.battleLabelLocZ);
            this.battleEditPanel.Controls.Add(this.battleLabelHighPass);
            this.battleEditPanel.Controls.Add(this.battleSoundXfbinText);
            this.battleEditPanel.Controls.Add(this.battleAnimationNameText);
            this.battleEditPanel.Controls.Add(this.battleLabelLowPass);
            this.battleEditPanel.Controls.Add(this.battleHitboxText);
            this.battleEditPanel.Controls.Add(this.battleLoopFlagValue);
            this.battleEditPanel.Controls.Add(this.battlePlAnmText);
            this.battleEditPanel.Controls.Add(this.battleEventTypeValue);
            this.battleEditPanel.Controls.Add(this.battleSoundDelayValue);
            this.battleEditPanel.Controls.Add(this.battleHighPassValue);
            this.battleEditPanel.Controls.Add(this.battlePitchValue);
            this.battleEditPanel.Controls.Add(this.battleLowPassValue);
            this.battleEditPanel.Controls.Add(this.battlePanValue);
            this.battleEditPanel.Controls.Add(this.battleVolumeValue);
            this.battleEditPanel.Location = new System.Drawing.Point(374, 8);
            this.battleEditPanel.Name = "battleEditPanel";
            this.battleEditPanel.Size = new System.Drawing.Size(385, 668);
            this.battleEditPanel.TabIndex = 7;
            // 
            // battleLabelSoundPl
            // 
            this.battleLabelSoundPl.AutoSize = true;
            this.battleLabelSoundPl.Location = new System.Drawing.Point(2, 27);
            this.battleLabelSoundPl.Name = "battleLabelSoundPl";
            this.battleLabelSoundPl.Size = new System.Drawing.Size(61, 15);
            this.battleLabelSoundPl.TabIndex = 0;
            this.battleLabelSoundPl.Text = "SFX Name";
            this.battleLabelSoundPl.Click += new System.EventHandler(this.battleLabelSoundPl_Click);
            // 
            // battleLabelPan
            // 
            this.battleLabelPan.AutoSize = true;
            this.battleLabelPan.Location = new System.Drawing.Point(3, 209);
            this.battleLabelPan.Name = "battleLabelPan";
            this.battleLabelPan.Size = new System.Drawing.Size(27, 15);
            this.battleLabelPan.TabIndex = 2;
            this.battleLabelPan.Text = "Pan";
            // 
            // battleSoundDelayResetAnmValue
            // 
            this.battleSoundDelayResetAnmValue.Location = new System.Drawing.Point(133, 542);
            this.battleSoundDelayResetAnmValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.battleSoundDelayResetAnmValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.battleSoundDelayResetAnmValue.Name = "battleSoundDelayResetAnmValue";
            this.battleSoundDelayResetAnmValue.Size = new System.Drawing.Size(130, 23);
            this.battleSoundDelayResetAnmValue.TabIndex = 33;
            // 
            // battleLabelLocX
            // 
            this.battleLabelLocX.AutoSize = true;
            this.battleLabelLocX.Location = new System.Drawing.Point(4, 389);
            this.battleLabelLocX.Name = "battleLabelLocX";
            this.battleLabelLocX.Size = new System.Drawing.Size(63, 15);
            this.battleLabelLocX.TabIndex = 22;
            this.battleLabelLocX.Text = "Location X";
            // 
            // battleUnknownValue
            // 
            this.battleUnknownValue.Location = new System.Drawing.Point(133, 510);
            this.battleUnknownValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.battleUnknownValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.battleUnknownValue.Name = "battleUnknownValue";
            this.battleUnknownValue.Size = new System.Drawing.Size(130, 23);
            this.battleUnknownValue.TabIndex = 31;
            // 
            // battleLabelVolume
            // 
            this.battleLabelVolume.AutoSize = true;
            this.battleLabelVolume.Location = new System.Drawing.Point(3, 238);
            this.battleLabelVolume.Name = "battleLabelVolume";
            this.battleLabelVolume.Size = new System.Drawing.Size(47, 15);
            this.battleLabelVolume.TabIndex = 4;
            this.battleLabelVolume.Text = "Volume";
            // 
            // battleSoundDelayResetValue
            // 
            this.battleSoundDelayResetValue.Location = new System.Drawing.Point(133, 481);
            this.battleSoundDelayResetValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.battleSoundDelayResetValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.battleSoundDelayResetValue.Name = "battleSoundDelayResetValue";
            this.battleSoundDelayResetValue.Size = new System.Drawing.Size(130, 23);
            this.battleSoundDelayResetValue.TabIndex = 29;
            // 
            // battleLabelSoundXfbin
            // 
            this.battleLabelSoundXfbin.AutoSize = true;
            this.battleLabelSoundXfbin.Location = new System.Drawing.Point(2, 61);
            this.battleLabelSoundXfbin.Name = "battleLabelSoundXfbin";
            this.battleLabelSoundXfbin.Size = new System.Drawing.Size(62, 15);
            this.battleLabelSoundXfbin.TabIndex = 16;
            this.battleLabelSoundXfbin.Text = "Xfbin Path";
            // 
            // battleLabelSoundDelayResetAnm
            // 
            this.battleLabelSoundDelayResetAnm.AutoSize = true;
            this.battleLabelSoundDelayResetAnm.Location = new System.Drawing.Point(4, 550);
            this.battleLabelSoundDelayResetAnm.Name = "battleLabelSoundDelayResetAnm";
            this.battleLabelSoundDelayResetAnm.Size = new System.Drawing.Size(96, 15);
            this.battleLabelSoundDelayResetAnm.TabIndex = 32;
            this.battleLabelSoundDelayResetAnm.Text = "Delay Reset Anm";
            // 
            // battleLabelAnimationName
            // 
            this.battleLabelAnimationName.AutoSize = true;
            this.battleLabelAnimationName.Location = new System.Drawing.Point(2, 88);
            this.battleLabelAnimationName.Name = "battleLabelAnimationName";
            this.battleLabelAnimationName.Size = new System.Drawing.Size(63, 15);
            this.battleLabelAnimationName.TabIndex = 18;
            this.battleLabelAnimationName.Text = "Animation";
            // 
            // battleLabelUnknown
            // 
            this.battleLabelUnknown.AutoSize = true;
            this.battleLabelUnknown.Location = new System.Drawing.Point(4, 518);
            this.battleLabelUnknown.Name = "battleLabelUnknown";
            this.battleLabelUnknown.Size = new System.Drawing.Size(58, 15);
            this.battleLabelUnknown.TabIndex = 30;
            this.battleLabelUnknown.Text = "Unknown";
            // 
            // battleLocZValue
            // 
            this.battleLocZValue.DecimalPlaces = 3;
            this.battleLocZValue.Location = new System.Drawing.Point(133, 452);
            this.battleLocZValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.battleLocZValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.battleLocZValue.Name = "battleLocZValue";
            this.battleLocZValue.Size = new System.Drawing.Size(130, 23);
            this.battleLocZValue.TabIndex = 27;
            // 
            // battleLabelSoundDelayReset
            // 
            this.battleLabelSoundDelayReset.AutoSize = true;
            this.battleLabelSoundDelayReset.Location = new System.Drawing.Point(3, 489);
            this.battleLabelSoundDelayReset.Name = "battleLabelSoundDelayReset";
            this.battleLabelSoundDelayReset.Size = new System.Drawing.Size(103, 15);
            this.battleLabelSoundDelayReset.TabIndex = 28;
            this.battleLabelSoundDelayReset.Text = "Delay Reset Frame";
            // 
            // battleLabelPitch
            // 
            this.battleLabelPitch.AutoSize = true;
            this.battleLabelPitch.Location = new System.Drawing.Point(3, 265);
            this.battleLabelPitch.Name = "battleLabelPitch";
            this.battleLabelPitch.Size = new System.Drawing.Size(34, 15);
            this.battleLabelPitch.TabIndex = 6;
            this.battleLabelPitch.Text = "Pitch";
            // 
            // battleLocYValue
            // 
            this.battleLocYValue.DecimalPlaces = 3;
            this.battleLocYValue.Location = new System.Drawing.Point(133, 423);
            this.battleLocYValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.battleLocYValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.battleLocYValue.Name = "battleLocYValue";
            this.battleLocYValue.Size = new System.Drawing.Size(130, 23);
            this.battleLocYValue.TabIndex = 25;
            // 
            // battleLabelHitbox
            // 
            this.battleLabelHitbox.AutoSize = true;
            this.battleLabelHitbox.Location = new System.Drawing.Point(4, 120);
            this.battleLabelHitbox.Name = "battleLabelHitbox";
            this.battleLabelHitbox.Size = new System.Drawing.Size(91, 15);
            this.battleLabelHitbox.TabIndex = 20;
            this.battleLabelHitbox.Text = "Spawn Location";
            this.battleLabelHitbox.Click += new System.EventHandler(this.battleLabelHitbox_Click);
            // 
            // battleLocXValue
            // 
            this.battleLocXValue.DecimalPlaces = 3;
            this.battleLocXValue.Location = new System.Drawing.Point(133, 389);
            this.battleLocXValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.battleLocXValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.battleLocXValue.Name = "battleLocXValue";
            this.battleLocXValue.Size = new System.Drawing.Size(130, 23);
            this.battleLocXValue.TabIndex = 23;
            // 
            // battleLabelLoopFlag
            // 
            this.battleLabelLoopFlag.AutoSize = true;
            this.battleLabelLoopFlag.Location = new System.Drawing.Point(4, 576);
            this.battleLabelLoopFlag.Name = "battleLabelLoopFlag";
            this.battleLabelLoopFlag.Size = new System.Drawing.Size(34, 15);
            this.battleLabelLoopFlag.TabIndex = 34;
            this.battleLabelLoopFlag.Text = "Loop";
            // 
            // battleLabelPlAnm
            // 
            this.battleLabelPlAnm.AutoSize = true;
            this.battleLabelPlAnm.Location = new System.Drawing.Point(4, 149);
            this.battleLabelPlAnm.Name = "battleLabelPlAnm";
            this.battleLabelPlAnm.Size = new System.Drawing.Size(91, 15);
            this.battleLabelPlAnm.TabIndex = 36;
            this.battleLabelPlAnm.Text = "PL_ANM Action";
            // 
            // battleLabelSoundDelay
            // 
            this.battleLabelSoundDelay.AutoSize = true;
            this.battleLabelSoundDelay.Location = new System.Drawing.Point(3, 296);
            this.battleLabelSoundDelay.Name = "battleLabelSoundDelay";
            this.battleLabelSoundDelay.Size = new System.Drawing.Size(40, 15);
            this.battleLabelSoundDelay.TabIndex = 10;
            this.battleLabelSoundDelay.Text = "Frame";
            // 
            // battleLabelEventType
            // 
            this.battleLabelEventType.AutoSize = true;
            this.battleLabelEventType.Location = new System.Drawing.Point(3, 179);
            this.battleLabelEventType.Name = "battleLabelEventType";
            this.battleLabelEventType.Size = new System.Drawing.Size(64, 15);
            this.battleLabelEventType.TabIndex = 8;
            this.battleLabelEventType.Text = "Event Type";
            // 
            // battleLabelLocY
            // 
            this.battleLabelLocY.AutoSize = true;
            this.battleLabelLocY.Location = new System.Drawing.Point(4, 423);
            this.battleLabelLocY.Name = "battleLabelLocY";
            this.battleLabelLocY.Size = new System.Drawing.Size(63, 15);
            this.battleLabelLocY.TabIndex = 24;
            this.battleLabelLocY.Text = "Location Y";
            // 
            // battleSoundPlText
            // 
            this.battleSoundPlText.Location = new System.Drawing.Point(100, 19);
            this.battleSoundPlText.Name = "battleSoundPlText";
            this.battleSoundPlText.Size = new System.Drawing.Size(260, 23);
            this.battleSoundPlText.TabIndex = 1;
            // 
            // battleLabelLocZ
            // 
            this.battleLabelLocZ.AutoSize = true;
            this.battleLabelLocZ.Location = new System.Drawing.Point(4, 454);
            this.battleLabelLocZ.Name = "battleLabelLocZ";
            this.battleLabelLocZ.Size = new System.Drawing.Size(63, 15);
            this.battleLabelLocZ.TabIndex = 26;
            this.battleLabelLocZ.Text = "Location Z";
            // 
            // battleLabelHighPass
            // 
            this.battleLabelHighPass.AutoSize = true;
            this.battleLabelHighPass.Location = new System.Drawing.Point(3, 325);
            this.battleLabelHighPass.Name = "battleLabelHighPass";
            this.battleLabelHighPass.Size = new System.Drawing.Size(59, 15);
            this.battleLabelHighPass.TabIndex = 12;
            this.battleLabelHighPass.Text = "High Pass";
            // 
            // battleSoundXfbinText
            // 
            this.battleSoundXfbinText.Location = new System.Drawing.Point(100, 53);
            this.battleSoundXfbinText.Name = "battleSoundXfbinText";
            this.battleSoundXfbinText.Size = new System.Drawing.Size(260, 23);
            this.battleSoundXfbinText.TabIndex = 17;
            // 
            // battleAnimationNameText
            // 
            this.battleAnimationNameText.Location = new System.Drawing.Point(100, 85);
            this.battleAnimationNameText.Name = "battleAnimationNameText";
            this.battleAnimationNameText.Size = new System.Drawing.Size(260, 23);
            this.battleAnimationNameText.TabIndex = 19;
            // 
            // battleLabelLowPass
            // 
            this.battleLabelLowPass.AutoSize = true;
            this.battleLabelLowPass.Location = new System.Drawing.Point(4, 360);
            this.battleLabelLowPass.Name = "battleLabelLowPass";
            this.battleLabelLowPass.Size = new System.Drawing.Size(55, 15);
            this.battleLabelLowPass.TabIndex = 14;
            this.battleLabelLowPass.Text = "Low Pass";
            // 
            // battleHitboxText
            // 
            this.battleHitboxText.Location = new System.Drawing.Point(100, 117);
            this.battleHitboxText.Name = "battleHitboxText";
            this.battleHitboxText.Size = new System.Drawing.Size(260, 23);
            this.battleHitboxText.TabIndex = 21;
            this.battleHitboxText.TextChanged += new System.EventHandler(this.battleHitboxText_TextChanged);
            // 
            // battleLoopFlagValue
            // 
            this.battleLoopFlagValue.Location = new System.Drawing.Point(133, 576);
            this.battleLoopFlagValue.Name = "battleLoopFlagValue";
            this.battleLoopFlagValue.Size = new System.Drawing.Size(140, 24);
            this.battleLoopFlagValue.TabIndex = 35;
            this.battleLoopFlagValue.Text = "Enabled";
            // 
            // battlePlAnmText
            // 
            this.battlePlAnmText.Location = new System.Drawing.Point(100, 146);
            this.battlePlAnmText.Name = "battlePlAnmText";
            this.battlePlAnmText.Size = new System.Drawing.Size(260, 23);
            this.battlePlAnmText.TabIndex = 37;
            // 
            // battleEventTypeValue
            // 
            this.battleEventTypeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.battleEventTypeValue.Location = new System.Drawing.Point(100, 179);
            this.battleEventTypeValue.Name = "battleEventTypeValue";
            this.battleEventTypeValue.Size = new System.Drawing.Size(260, 21);
            this.battleEventTypeValue.TabIndex = 9;
            // 
            // battleSoundDelayValue
            // 
            this.battleSoundDelayValue.Location = new System.Drawing.Point(133, 294);
            this.battleSoundDelayValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.battleSoundDelayValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.battleSoundDelayValue.Name = "battleSoundDelayValue";
            this.battleSoundDelayValue.Size = new System.Drawing.Size(130, 23);
            this.battleSoundDelayValue.TabIndex = 11;
            // 
            // battleHighPassValue
            // 
            this.battleHighPassValue.DecimalPlaces = 3;
            this.battleHighPassValue.Location = new System.Drawing.Point(133, 323);
            this.battleHighPassValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.battleHighPassValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.battleHighPassValue.Name = "battleHighPassValue";
            this.battleHighPassValue.Size = new System.Drawing.Size(130, 23);
            this.battleHighPassValue.TabIndex = 13;
            // 
            // battlePitchValue
            // 
            this.battlePitchValue.Location = new System.Drawing.Point(133, 265);
            this.battlePitchValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.battlePitchValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.battlePitchValue.Name = "battlePitchValue";
            this.battlePitchValue.Size = new System.Drawing.Size(130, 23);
            this.battlePitchValue.TabIndex = 7;
            // 
            // battleLowPassValue
            // 
            this.battleLowPassValue.DecimalPlaces = 3;
            this.battleLowPassValue.Location = new System.Drawing.Point(133, 352);
            this.battleLowPassValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.battleLowPassValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.battleLowPassValue.Name = "battleLowPassValue";
            this.battleLowPassValue.Size = new System.Drawing.Size(130, 23);
            this.battleLowPassValue.TabIndex = 15;
            // 
            // battlePanValue
            // 
            this.battlePanValue.Location = new System.Drawing.Point(133, 207);
            this.battlePanValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.battlePanValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.battlePanValue.Name = "battlePanValue";
            this.battlePanValue.Size = new System.Drawing.Size(130, 23);
            this.battlePanValue.TabIndex = 3;
            // 
            // battleVolumeValue
            // 
            this.battleVolumeValue.DecimalPlaces = 3;
            this.battleVolumeValue.Location = new System.Drawing.Point(133, 236);
            this.battleVolumeValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.battleVolumeValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.battleVolumeValue.Name = "battleVolumeValue";
            this.battleVolumeValue.Size = new System.Drawing.Size(130, 23);
            this.battleVolumeValue.TabIndex = 5;
            // 
            // battleSearchButton
            // 
            this.battleSearchButton.Location = new System.Drawing.Point(266, 576);
            this.battleSearchButton.Name = "battleSearchButton";
            this.battleSearchButton.Size = new System.Drawing.Size(102, 24);
            this.battleSearchButton.TabIndex = 2;
            this.battleSearchButton.Text = "Search Next";
            this.battleSearchButton.Click += new System.EventHandler(this.battleSearchButton_Click);
            // 
            // battleAddButton
            // 
            this.battleAddButton.Location = new System.Drawing.Point(8, 612);
            this.battleAddButton.Name = "battleAddButton";
            this.battleAddButton.Size = new System.Drawing.Size(114, 28);
            this.battleAddButton.TabIndex = 3;
            this.battleAddButton.Text = "Add";
            this.battleAddButton.Click += new System.EventHandler(this.battleAddButton_Click);
            // 
            // battleDuplicateButton
            // 
            this.battleDuplicateButton.Location = new System.Drawing.Point(128, 612);
            this.battleDuplicateButton.Name = "battleDuplicateButton";
            this.battleDuplicateButton.Size = new System.Drawing.Size(114, 28);
            this.battleDuplicateButton.TabIndex = 4;
            this.battleDuplicateButton.Text = "Duplicate";
            this.battleDuplicateButton.Click += new System.EventHandler(this.battleDuplicateButton_Click);
            // 
            // battleDeleteButton
            // 
            this.battleDeleteButton.Location = new System.Drawing.Point(128, 648);
            this.battleDeleteButton.Name = "battleDeleteButton";
            this.battleDeleteButton.Size = new System.Drawing.Size(114, 28);
            this.battleDeleteButton.TabIndex = 7;
            this.battleDeleteButton.Text = "Delete";
            this.battleDeleteButton.Click += new System.EventHandler(this.battleDeleteButton_Click);
            // 
            // battleSaveSelectedButton
            // 
            this.battleSaveSelectedButton.Location = new System.Drawing.Point(8, 646);
            this.battleSaveSelectedButton.Name = "battleSaveSelectedButton";
            this.battleSaveSelectedButton.Size = new System.Drawing.Size(114, 28);
            this.battleSaveSelectedButton.TabIndex = 8;
            this.battleSaveSelectedButton.Text = "Save Selected";
            this.battleSaveSelectedButton.Click += new System.EventHandler(this.battleSaveSelectedButton_Click);
            // 
            // battleCopyButton
            // 
            this.battleCopyButton.Location = new System.Drawing.Point(248, 612);
            this.battleCopyButton.Name = "battleCopyButton";
            this.battleCopyButton.Size = new System.Drawing.Size(114, 28);
            this.battleCopyButton.TabIndex = 5;
            this.battleCopyButton.Text = "Copy";
            this.battleCopyButton.Click += new System.EventHandler(this.battleCopyButton_Click);
            // 
            // battlePasteButton
            // 
            this.battlePasteButton.Location = new System.Drawing.Point(248, 646);
            this.battlePasteButton.Name = "battlePasteButton";
            this.battlePasteButton.Size = new System.Drawing.Size(114, 28);
            this.battlePasteButton.TabIndex = 6;
            this.battlePasteButton.Text = "Paste";
            this.battlePasteButton.Click += new System.EventHandler(this.battlePasteButton_Click);
            // 
            // ultimateTabPage
            // 
            this.ultimateTabPage.Controls.Add(this.ultimateChunkListBox);
            this.ultimateTabPage.Controls.Add(this.ultimateEntryListBox);
            this.ultimateTabPage.Controls.Add(this.ultimateAddEntryButton);
            this.ultimateTabPage.Controls.Add(this.ultimateDuplicateEntryButton);
            this.ultimateTabPage.Controls.Add(this.ultimateDeleteEntryButton);
            this.ultimateTabPage.Controls.Add(this.ultimateSaveSelectedButton);
            this.ultimateTabPage.Controls.Add(this.ultimateCopyButton);
            this.ultimateTabPage.Controls.Add(this.ultimatePasteButton);
            this.ultimateTabPage.Controls.Add(this.ultimateSearchText);
            this.ultimateTabPage.Controls.Add(this.ultimateSearchButton);
            this.ultimateTabPage.Controls.Add(this.ultimateEditPanel);
            this.ultimateTabPage.Location = new System.Drawing.Point(4, 22);
            this.ultimateTabPage.Name = "ultimateTabPage";
            this.ultimateTabPage.Padding = new System.Windows.Forms.Padding(8);
            this.ultimateTabPage.Size = new System.Drawing.Size(736, 691);
            this.ultimateTabPage.TabIndex = 1;
            this.ultimateTabPage.Text = "Ultimate";
            // 
            // ultimateChunkListBox
            // 
            this.ultimateChunkListBox.Location = new System.Drawing.Point(8, 8);
            this.ultimateChunkListBox.Name = "ultimateChunkListBox";
            this.ultimateChunkListBox.Size = new System.Drawing.Size(260, 95);
            this.ultimateChunkListBox.TabIndex = 0;
            this.ultimateChunkListBox.SelectedIndexChanged += new System.EventHandler(this.ultimateChunkListBox_SelectedIndexChanged);
            // 
            // ultimateEntryListBox
            // 
            this.ultimateEntryListBox.Location = new System.Drawing.Point(8, 105);
            this.ultimateEntryListBox.Name = "ultimateEntryListBox";
            this.ultimateEntryListBox.Size = new System.Drawing.Size(260, 472);
            this.ultimateEntryListBox.TabIndex = 1;
            this.ultimateEntryListBox.SelectedIndexChanged += new System.EventHandler(this.ultimateEntryListBox_SelectedIndexChanged);
            // 
            // ultimateAddEntryButton
            // 
            this.ultimateAddEntryButton.Location = new System.Drawing.Point(8, 612);
            this.ultimateAddEntryButton.Name = "ultimateAddEntryButton";
            this.ultimateAddEntryButton.Size = new System.Drawing.Size(59, 28);
            this.ultimateAddEntryButton.TabIndex = 2;
            this.ultimateAddEntryButton.Text = "Add";
            this.ultimateAddEntryButton.Click += new System.EventHandler(this.ultimateAddEntryButton_Click);
            // 
            // ultimateDuplicateEntryButton
            // 
            this.ultimateDuplicateEntryButton.Location = new System.Drawing.Point(199, 613);
            this.ultimateDuplicateEntryButton.Name = "ultimateDuplicateEntryButton";
            this.ultimateDuplicateEntryButton.Size = new System.Drawing.Size(69, 28);
            this.ultimateDuplicateEntryButton.TabIndex = 3;
            this.ultimateDuplicateEntryButton.Text = "Duplicate";
            this.ultimateDuplicateEntryButton.Click += new System.EventHandler(this.ultimateDuplicateEntryButton_Click);
            // 
            // ultimateDeleteEntryButton
            // 
            this.ultimateDeleteEntryButton.Location = new System.Drawing.Point(199, 643);
            this.ultimateDeleteEntryButton.Name = "ultimateDeleteEntryButton";
            this.ultimateDeleteEntryButton.Size = new System.Drawing.Size(69, 28);
            this.ultimateDeleteEntryButton.TabIndex = 6;
            this.ultimateDeleteEntryButton.Text = "Delete";
            this.ultimateDeleteEntryButton.Click += new System.EventHandler(this.ultimateDeleteEntryButton_Click);
            // 
            // ultimateSaveSelectedButton
            // 
            this.ultimateSaveSelectedButton.Location = new System.Drawing.Point(8, 646);
            this.ultimateSaveSelectedButton.Name = "ultimateSaveSelectedButton";
            this.ultimateSaveSelectedButton.Size = new System.Drawing.Size(185, 28);
            this.ultimateSaveSelectedButton.TabIndex = 7;
            this.ultimateSaveSelectedButton.Text = "Save Selected";
            this.ultimateSaveSelectedButton.Click += new System.EventHandler(this.ultimateSaveSelectedButton_Click);
            // 
            // ultimateCopyButton
            // 
            this.ultimateCopyButton.Location = new System.Drawing.Point(69, 612);
            this.ultimateCopyButton.Name = "ultimateCopyButton";
            this.ultimateCopyButton.Size = new System.Drawing.Size(59, 28);
            this.ultimateCopyButton.TabIndex = 4;
            this.ultimateCopyButton.Text = "Copy";
            this.ultimateCopyButton.Click += new System.EventHandler(this.ultimateCopyButton_Click);
            // 
            // ultimatePasteButton
            // 
            this.ultimatePasteButton.Location = new System.Drawing.Point(134, 612);
            this.ultimatePasteButton.Name = "ultimatePasteButton";
            this.ultimatePasteButton.Size = new System.Drawing.Size(59, 28);
            this.ultimatePasteButton.TabIndex = 5;
            this.ultimatePasteButton.Text = "Paste";
            this.ultimatePasteButton.Click += new System.EventHandler(this.ultimatePasteButton_Click);
            // 
            // ultimateSearchText
            // 
            this.ultimateSearchText.Location = new System.Drawing.Point(8, 583);
            this.ultimateSearchText.Name = "ultimateSearchText";
            this.ultimateSearchText.Size = new System.Drawing.Size(185, 23);
            this.ultimateSearchText.TabIndex = 8;
            // 
            // ultimateSearchButton
            // 
            this.ultimateSearchButton.Location = new System.Drawing.Point(199, 583);
            this.ultimateSearchButton.Name = "ultimateSearchButton";
            this.ultimateSearchButton.Size = new System.Drawing.Size(69, 24);
            this.ultimateSearchButton.TabIndex = 9;
            this.ultimateSearchButton.Text = "Search Next";
            this.ultimateSearchButton.Click += new System.EventHandler(this.ultimateSearchButton_Click);
            // 
            // ultimateEditPanel
            // 
            this.ultimateEditPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelSoundPl);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelPan);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelVolume);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelPitch);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelEventType);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelSoundDelay);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelHighPass);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelLowPass);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelIndex);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelSoundCutframe);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelUnknown);
            this.ultimateEditPanel.Controls.Add(this.ultimateLabelFadeParam);
            this.ultimateEditPanel.Controls.Add(this.ultimateSoundPlText);
            this.ultimateEditPanel.Controls.Add(this.ultimatePanValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateVolumeValue);
            this.ultimateEditPanel.Controls.Add(this.ultimatePitchValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateEventTypeValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateSoundDelayValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateHighPassValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateLowPassValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateIndexValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateSoundCutframeValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateUnknownValue);
            this.ultimateEditPanel.Controls.Add(this.ultimateFadeParamValue);
            this.ultimateEditPanel.Location = new System.Drawing.Point(274, 8);
            this.ultimateEditPanel.Name = "ultimateEditPanel";
            this.ultimateEditPanel.Size = new System.Drawing.Size(485, 672);
            this.ultimateEditPanel.TabIndex = 11;
            this.ultimateEditPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ultimateEditPanel_Paint);
            // 
            // ultimateLabelSoundPl
            // 
            this.ultimateLabelSoundPl.AutoSize = true;
            this.ultimateLabelSoundPl.Location = new System.Drawing.Point(3, 19);
            this.ultimateLabelSoundPl.Name = "ultimateLabelSoundPl";
            this.ultimateLabelSoundPl.Size = new System.Drawing.Size(61, 15);
            this.ultimateLabelSoundPl.TabIndex = 0;
            this.ultimateLabelSoundPl.Text = "SFX Name";
            // 
            // ultimateLabelPan
            // 
            this.ultimateLabelPan.AutoSize = true;
            this.ultimateLabelPan.Location = new System.Drawing.Point(3, 48);
            this.ultimateLabelPan.Name = "ultimateLabelPan";
            this.ultimateLabelPan.Size = new System.Drawing.Size(27, 15);
            this.ultimateLabelPan.TabIndex = 2;
            this.ultimateLabelPan.Text = "Pan";
            // 
            // ultimateLabelVolume
            // 
            this.ultimateLabelVolume.AutoSize = true;
            this.ultimateLabelVolume.Location = new System.Drawing.Point(3, 80);
            this.ultimateLabelVolume.Name = "ultimateLabelVolume";
            this.ultimateLabelVolume.Size = new System.Drawing.Size(47, 15);
            this.ultimateLabelVolume.TabIndex = 4;
            this.ultimateLabelVolume.Text = "Volume";
            // 
            // ultimateLabelPitch
            // 
            this.ultimateLabelPitch.AutoSize = true;
            this.ultimateLabelPitch.Location = new System.Drawing.Point(3, 112);
            this.ultimateLabelPitch.Name = "ultimateLabelPitch";
            this.ultimateLabelPitch.Size = new System.Drawing.Size(34, 15);
            this.ultimateLabelPitch.TabIndex = 6;
            this.ultimateLabelPitch.Text = "Pitch";
            // 
            // ultimateLabelEventType
            // 
            this.ultimateLabelEventType.AutoSize = true;
            this.ultimateLabelEventType.Location = new System.Drawing.Point(3, 144);
            this.ultimateLabelEventType.Name = "ultimateLabelEventType";
            this.ultimateLabelEventType.Size = new System.Drawing.Size(64, 15);
            this.ultimateLabelEventType.TabIndex = 8;
            this.ultimateLabelEventType.Text = "Event Type";
            // 
            // ultimateLabelSoundDelay
            // 
            this.ultimateLabelSoundDelay.AutoSize = true;
            this.ultimateLabelSoundDelay.Location = new System.Drawing.Point(3, 176);
            this.ultimateLabelSoundDelay.Name = "ultimateLabelSoundDelay";
            this.ultimateLabelSoundDelay.Size = new System.Drawing.Size(40, 15);
            this.ultimateLabelSoundDelay.TabIndex = 10;
            this.ultimateLabelSoundDelay.Text = "Frame";
            // 
            // ultimateLabelHighPass
            // 
            this.ultimateLabelHighPass.AutoSize = true;
            this.ultimateLabelHighPass.Location = new System.Drawing.Point(3, 208);
            this.ultimateLabelHighPass.Name = "ultimateLabelHighPass";
            this.ultimateLabelHighPass.Size = new System.Drawing.Size(59, 15);
            this.ultimateLabelHighPass.TabIndex = 12;
            this.ultimateLabelHighPass.Text = "High Pass";
            // 
            // ultimateLabelLowPass
            // 
            this.ultimateLabelLowPass.AutoSize = true;
            this.ultimateLabelLowPass.Location = new System.Drawing.Point(3, 240);
            this.ultimateLabelLowPass.Name = "ultimateLabelLowPass";
            this.ultimateLabelLowPass.Size = new System.Drawing.Size(55, 15);
            this.ultimateLabelLowPass.TabIndex = 14;
            this.ultimateLabelLowPass.Text = "Low Pass";
            // 
            // ultimateLabelIndex
            // 
            this.ultimateLabelIndex.AutoSize = true;
            this.ultimateLabelIndex.Location = new System.Drawing.Point(3, 272);
            this.ultimateLabelIndex.Name = "ultimateLabelIndex";
            this.ultimateLabelIndex.Size = new System.Drawing.Size(35, 15);
            this.ultimateLabelIndex.TabIndex = 16;
            this.ultimateLabelIndex.Text = "Index";
            // 
            // ultimateLabelSoundCutframe
            // 
            this.ultimateLabelSoundCutframe.AutoSize = true;
            this.ultimateLabelSoundCutframe.Location = new System.Drawing.Point(3, 304);
            this.ultimateLabelSoundCutframe.Name = "ultimateLabelSoundCutframe";
            this.ultimateLabelSoundCutframe.Size = new System.Drawing.Size(97, 15);
            this.ultimateLabelSoundCutframe.TabIndex = 18;
            this.ultimateLabelSoundCutframe.Text = "Sound Cut frame";
            // 
            // ultimateLabelUnknown
            // 
            this.ultimateLabelUnknown.AutoSize = true;
            this.ultimateLabelUnknown.Location = new System.Drawing.Point(3, 336);
            this.ultimateLabelUnknown.Name = "ultimateLabelUnknown";
            this.ultimateLabelUnknown.Size = new System.Drawing.Size(58, 15);
            this.ultimateLabelUnknown.TabIndex = 20;
            this.ultimateLabelUnknown.Text = "Unknown";
            // 
            // ultimateLabelFadeParam
            // 
            this.ultimateLabelFadeParam.AutoSize = true;
            this.ultimateLabelFadeParam.Location = new System.Drawing.Point(3, 368);
            this.ultimateLabelFadeParam.Name = "ultimateLabelFadeParam";
            this.ultimateLabelFadeParam.Size = new System.Drawing.Size(67, 15);
            this.ultimateLabelFadeParam.TabIndex = 22;
            this.ultimateLabelFadeParam.Text = "Unknown 2";
            this.ultimateLabelFadeParam.Click += new System.EventHandler(this.ultimateLabelFadeParam_Click);
            // 
            // ultimateSoundPlText
            // 
            this.ultimateSoundPlText.Location = new System.Drawing.Point(177, 16);
            this.ultimateSoundPlText.Name = "ultimateSoundPlText";
            this.ultimateSoundPlText.Size = new System.Drawing.Size(260, 23);
            this.ultimateSoundPlText.TabIndex = 1;
            // 
            // ultimatePanValue
            // 
            this.ultimatePanValue.Location = new System.Drawing.Point(177, 45);
            this.ultimatePanValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ultimatePanValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ultimatePanValue.Name = "ultimatePanValue";
            this.ultimatePanValue.Size = new System.Drawing.Size(130, 23);
            this.ultimatePanValue.TabIndex = 3;
            // 
            // ultimateVolumeValue
            // 
            this.ultimateVolumeValue.DecimalPlaces = 3;
            this.ultimateVolumeValue.Location = new System.Drawing.Point(177, 77);
            this.ultimateVolumeValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ultimateVolumeValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.ultimateVolumeValue.Name = "ultimateVolumeValue";
            this.ultimateVolumeValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateVolumeValue.TabIndex = 5;
            // 
            // ultimatePitchValue
            // 
            this.ultimatePitchValue.Location = new System.Drawing.Point(177, 109);
            this.ultimatePitchValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ultimatePitchValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ultimatePitchValue.Name = "ultimatePitchValue";
            this.ultimatePitchValue.Size = new System.Drawing.Size(130, 23);
            this.ultimatePitchValue.TabIndex = 7;
            // 
            // ultimateEventTypeValue
            // 
            this.ultimateEventTypeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ultimateEventTypeValue.Location = new System.Drawing.Point(177, 141);
            this.ultimateEventTypeValue.Name = "ultimateEventTypeValue";
            this.ultimateEventTypeValue.Size = new System.Drawing.Size(260, 21);
            this.ultimateEventTypeValue.TabIndex = 9;
            // 
            // ultimateSoundDelayValue
            // 
            this.ultimateSoundDelayValue.Location = new System.Drawing.Point(177, 173);
            this.ultimateSoundDelayValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ultimateSoundDelayValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ultimateSoundDelayValue.Name = "ultimateSoundDelayValue";
            this.ultimateSoundDelayValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateSoundDelayValue.TabIndex = 11;
            // 
            // ultimateHighPassValue
            // 
            this.ultimateHighPassValue.DecimalPlaces = 3;
            this.ultimateHighPassValue.Location = new System.Drawing.Point(177, 205);
            this.ultimateHighPassValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ultimateHighPassValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.ultimateHighPassValue.Name = "ultimateHighPassValue";
            this.ultimateHighPassValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateHighPassValue.TabIndex = 13;
            // 
            // ultimateLowPassValue
            // 
            this.ultimateLowPassValue.DecimalPlaces = 3;
            this.ultimateLowPassValue.Location = new System.Drawing.Point(177, 237);
            this.ultimateLowPassValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ultimateLowPassValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.ultimateLowPassValue.Name = "ultimateLowPassValue";
            this.ultimateLowPassValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateLowPassValue.TabIndex = 15;
            // 
            // ultimateIndexValue
            // 
            this.ultimateIndexValue.Location = new System.Drawing.Point(177, 269);
            this.ultimateIndexValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ultimateIndexValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ultimateIndexValue.Name = "ultimateIndexValue";
            this.ultimateIndexValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateIndexValue.TabIndex = 17;
            // 
            // ultimateSoundCutframeValue
            // 
            this.ultimateSoundCutframeValue.Location = new System.Drawing.Point(177, 301);
            this.ultimateSoundCutframeValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ultimateSoundCutframeValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ultimateSoundCutframeValue.Name = "ultimateSoundCutframeValue";
            this.ultimateSoundCutframeValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateSoundCutframeValue.TabIndex = 19;
            // 
            // ultimateUnknownValue
            // 
            this.ultimateUnknownValue.Location = new System.Drawing.Point(177, 333);
            this.ultimateUnknownValue.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ultimateUnknownValue.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ultimateUnknownValue.Name = "ultimateUnknownValue";
            this.ultimateUnknownValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateUnknownValue.TabIndex = 21;
            // 
            // ultimateFadeParamValue
            // 
            this.ultimateFadeParamValue.DecimalPlaces = 3;
            this.ultimateFadeParamValue.Location = new System.Drawing.Point(177, 365);
            this.ultimateFadeParamValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ultimateFadeParamValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.ultimateFadeParamValue.Name = "ultimateFadeParamValue";
            this.ultimateFadeParamValue.Size = new System.Drawing.Size(130, 23);
            this.ultimateFadeParamValue.TabIndex = 23;
            // 
            // Tool_EvEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(783, 761);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1918, 800);
            this.Name = "Tool_EvEditor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Event Sound Editor";
            this.Load += new System.EventHandler(this.Tool_EvEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.battleTabPage.ResumeLayout(false);
            this.battleTabPage.PerformLayout();
            this.battleEditPanel.ResumeLayout(false);
            this.battleEditPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.battleSoundDelayResetAnmValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleUnknownValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleSoundDelayResetValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLocZValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLocYValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLocXValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleSoundDelayValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleHighPassValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battlePitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleLowPassValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battlePanValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battleVolumeValue)).EndInit();
            this.ultimateTabPage.ResumeLayout(false);
            this.ultimateTabPage.PerformLayout();
            this.ultimateEditPanel.ResumeLayout(false);
            this.ultimateEditPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultimatePanValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateVolumeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimatePitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateSoundDelayValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateHighPassValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateLowPassValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateIndexValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateSoundCutframeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateUnknownValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultimateFadeParamValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private Label battleLabelSoundPl;
        private Label battleLabelLocX;
        private Label battleLabelVolume;
        private Label battleLabelSoundXfbin;
        private Label battleLabelSoundDelayResetAnm;
        private Label battleLabelAnimationName;
        private Label battleLabelUnknown;
        private Label battleLabelSoundDelayReset;
        private Label battleLabelPitch;
        private Label battleLabelHitbox;
        private Label battleLabelLoopFlag;
        private Label battleLabelPlAnm;
        private Label battleLabelSoundDelay;
        private Label battleLabelEventType;
        private Label battleLabelLocY;
        private Label battleLabelLocZ;
        private Label battleLabelHighPass;
        private Label battleLabelLowPass;    }
}



