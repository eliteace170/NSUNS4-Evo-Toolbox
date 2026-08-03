using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager.Misc
{
    /// <summary>
    /// Editor for the StageInfo/advStageInfo binary chunk used by NSUNS4.
    ///
    /// The legacy implementation treated most of the 0x130-byte stage record as
    /// unrelated lists of values.  This implementation mirrors the typed
    /// StageInfoModel/StageInfoViewModel layout: every known stage and object field
    /// is edited through one model and serialized from the documented offsets.
    /// Unknown/reserved bytes are retained in RawRecord when a file is reopened.
    /// </summary>
    public partial class Tool_StageInfoEditor : Form
    {
        private readonly BindingList<StageInfoStage> _stages = new BindingList<StageInfoStage>();
        private StageInfoStage _currentStage;
        private StageInfoStage _stageEditBuffer;
        private StageInfoPath _currentPath;
        private StageInfoObject _currentObject;
        private StageInfoObject _objectEditBuffer;
        private StageInfoStage _copiedStageProperties;
        private bool _stageEditDirty;
        private bool _pathEditDirty;
        private bool _objectEditDirty;
        private bool _loadingUi;
        private bool _dirty;
        private bool _initialFileLoadAttempted;
        private string _fileBinName = "stageInfo";

        // Kept public for compatibility with callers and older toolbox code.
        public byte[] fileBytes = new byte[0];
        public byte[] header = new byte[0];
        public bool FileOpen = false;
        public string FilePath = "";
        public int EntryCount = 0;

        public Tool_StageInfoEditor()
        {
            InitializeComponent();
            DoubleBuffered = true;
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                RefreshCommandState();
        }

        #region Selection, list and editing commands

        // These named handlers are intentionally wired by InitializeComponent so
        // Visual Studio can load and edit the complete form in the WinForms designer.
        private void OpenCommand_Click(object sender, EventArgs e) { OpenFile(); }
        private void SaveCommand_Click(object sender, EventArgs e) { SaveFile(); }
        private void SaveAsCommand_Click(object sender, EventArgs e) { SaveFileAs(); }
        private void CloseDocumentCommand_Click(object sender, EventArgs e) { CloseFile(); }
        private void ExitCommand_Click(object sender, EventArgs e) { Close(); }
        private void AddStageCommand_Click(object sender, EventArgs e) { AddStage(false); }
        private void DuplicateStageCommand_Click(object sender, EventArgs e) { AddStage(true); }
        private void DeleteStageCommand_Click(object sender, EventArgs e) { DeleteStage(); }
        private void CopySettingsCommand_Click(object sender, EventArgs e) { CopyStageProperties(); }
        private void PasteSettingsCommand_Click(object sender, EventArgs e) { PasteStageProperties(); }
        private void SyncResourcesCommand_Click(object sender, EventArgs e) { SyncCurrentManagedResources(); }
        private void ValidateCommand_Click(object sender, EventArgs e) { ValidateDocumentWithMessage(); }
        private void MoveStageUpCommand_Click(object sender, EventArgs e) { MoveStage(-1); }
        private void MoveStageDownCommand_Click(object sender, EventArgs e) { MoveStage(1); }
        private void AddPathCommand_Click(object sender, EventArgs e) { AddPath(); }
        private void DeletePathCommand_Click(object sender, EventArgs e) { DeletePath(); }
        private void MovePathUpCommand_Click(object sender, EventArgs e) { MovePath(-1); }
        private void MovePathDownCommand_Click(object sender, EventArgs e) { MovePath(1); }
        private void AddObjectCommand_Click(object sender, EventArgs e) { AddObject(); }
        private void DuplicateObjectCommand_Click(object sender, EventArgs e) { DuplicateObject(); }
        private void DeleteObjectCommand_Click(object sender, EventArgs e) { DeleteObject(); }
        private void MoveObjectUpCommand_Click(object sender, EventArgs e) { MoveObject(-1); }
        private void MoveObjectDownCommand_Click(object sender, EventArgs e) { MoveObject(1); }
        private void SaveStageEntryCommand_Click(object sender, EventArgs e) { SaveStageEntry(); }
        private void SavePathCommand_Click(object sender, EventArgs e) { SavePathEntry(); }
        private void SaveObjectCommand_Click(object sender, EventArgs e) { SaveObjectEntry(); }

        private void StageSearchBox_TextChanged(object sender, EventArgs e)
        {
            if (!_loadingUi) RefreshStageList(_currentStage);
        }

        private void StageNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_loadingUi || _stageEditBuffer == null) return;
            _stageEditBuffer.StageName = _stageNameTextBox.Text;
            SetStageEditDirty();
        }

        private void StageMessageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_loadingUi || _stageEditBuffer == null) return;
            _stageEditBuffer.StageMessageID = _stageMessageTextBox.Text;
            SetStageEditDirty();
        }

        private void StageFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_loadingUi || _stageEditBuffer == null) return;
            _stageEditBuffer.StageFilter = _stageFilterTextBox.Text;
            SetStageEditDirty();
        }

        private void StageWeatherNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (_loadingUi || _stageEditBuffer == null) return;
            _stageEditBuffer.Weather = decimal.ToInt32(_stageWeatherNumeric.Value);
            SetStageEditDirty();
        }

        private void PathList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingUi) return;
            StageInfoPath selected = _pathList.SelectedItem as StageInfoPath;
            if (!ReferenceEquals(selected, _currentPath) && _pathEditDirty && !ConfirmDiscardEdit("resource path"))
            {
                SelectListItem(_pathList, _currentPath);
                return;
            }
            SetCurrentPath(selected);
        }

        private void PathEditorTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_loadingUi || _currentPath == null) return;
            _pathEditDirty = !string.Equals(_pathEditorTextBox.Text, _currentPath.FilePath ?? "", StringComparison.Ordinal);
            RefreshCommandState();
            UpdatePendingStatus();
        }

        private void ObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingUi) return;
            StageInfoObject selected = _objectList.SelectedItem as StageInfoObject;
            if (!ReferenceEquals(selected, _currentObject) && _objectEditDirty && !ConfirmDiscardEdit("object entry"))
            {
                SelectListItem(_objectList, _currentObject);
                return;
            }
            SetCurrentObject(selected);
        }

        private void StagePropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (_loadingUi || _stageEditBuffer == null) return;
            RefreshStagePropertyGrids();
            SetStageEditDirty();
        }

        private void ObjectPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (_loadingUi || _objectEditBuffer == null) return;
            RefreshObjectPropertyGrids();
            _objectEditDirty = true;
            RefreshCommandState();
            UpdatePendingStatus();
        }

        private void Tool_StageInfoEditor_Shown(object sender, EventArgs e)
        {
            if (_initialFileLoadAttempted) return;
            _initialFileLoadAttempted = true;
            if (!string.IsNullOrWhiteSpace(Main.stageInfoPath) && File.Exists(Main.stageInfoPath))
                OpenFile(Main.stageInfoPath);
        }

        private void Tool_StageInfoEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                _stageSearchBox.Focus();
                _stageSearchBox.SelectAll();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && _stageList.Focused)
            {
                DeleteStage();
                e.Handled = true;
            }
        }

        private void Tool_StageInfoEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmDiscardAllEntryEdits() || !PromptSaveIfDirty())
                e.Cancel = true;
        }

        private void StageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingUi) return;
            StageListItem item = _stageList.SelectedItem as StageListItem;
            StageInfoStage selected = item == null ? null : item.Stage;
            if (!ReferenceEquals(selected, _currentStage) && HasPendingEntryEdits && !ConfirmDiscardAllEntryEdits())
            {
                SelectStageInList(_currentStage);
                return;
            }
            SetCurrentStage(selected);
        }

        private void SetCurrentStage(StageInfoStage stage)
        {
            _loadingUi = true;
            try
            {
                _currentStage = stage;
                _stageEditBuffer = stage == null ? null : stage.DeepClone();
                _stageEditDirty = false;
                _currentPath = null;
                _currentObject = null;
                _objectEditBuffer = null;
                _pathEditDirty = false;
                _objectEditDirty = false;
                RefreshStageIdentityFields(_stageEditBuffer);
                BindStagePropertyGrids(_stageEditBuffer);
                RefreshPathList(stage != null && stage.FilePaths.Count > 0 ? stage.FilePaths[0] : null);
                RefreshObjectList(stage != null && stage.Objects.Count > 0 ? stage.Objects[0] : null);
                UpdateRawInformation();
            }
            finally
            {
                _loadingUi = false;
            }
            RefreshCommandState();
            UpdateStatus();
        }

        private void SetCurrentObject(StageInfoObject stageObject)
        {
            bool previousLoading = _loadingUi;
            _loadingUi = true;
            try
            {
                _currentObject = stageObject;
                _objectEditBuffer = stageObject == null ? null : stageObject.DeepClone();
                _objectEditDirty = false;
                BindObjectPropertyGrids(_objectEditBuffer);
                UpdateRawInformation();
            }
            finally
            {
                _loadingUi = previousLoading;
            }
            RefreshCommandState();
            UpdateStatus();
        }

        private void SetCurrentPath(StageInfoPath path)
        {
            bool previousLoading = _loadingUi;
            _loadingUi = true;
            try
            {
                _currentPath = path;
                _pathEditDirty = false;
                _pathEditorTextBox.Text = path == null ? "" : path.FilePath ?? "";
            }
            finally { _loadingUi = previousLoading; }
            RefreshCommandState();
        }

        private void RefreshStageIdentityFields(StageInfoStage stage)
        {
            _stageNameTextBox.Text = stage == null ? "" : stage.StageName ?? "";
            _stageMessageTextBox.Text = stage == null ? "" : stage.StageMessageID ?? "";
            _stageFilterTextBox.Text = stage == null ? "" : stage.StageFilter ?? "";
            SetNumericValue(_stageWeatherNumeric, stage == null ? 0 : stage.Weather);
        }

        private void BindStagePropertyGrids(StageInfoStage stage)
        {
            SetPropertySubset(_fogMonoGrid, stage,
                "EnableFog", "FogStartDistance", "FogEndDistance", "FogStrength", "FogColor",
                "EnableMonoColorFilter", "MonoBlueTone", "MonoRedTone", "MonoAlpha");
            SetPropertySubset(_glareSoftFocusGrid, stage,
                "EnableGlareEffect", "GlareLuminanceThreshold", "GlareSubtracted", "GlareCompositionStrength",
                "EnableSoftFocus", "SoftFocusStrength");
            SetPropertySubset(_sunShaftGrid, stage,
                "EnableSunShaft", "SunShaftStartDistance", "SunShaftEndDistance", "SunShaftAlpha", "SunShaftColor",
                "SunShaftDirectionX", "SunShaftDirectionY", "SunShaftDirectionZ", "SunShaftBlurWidth", "SunShaftAttenuationCoefficient");
            SetPropertySubset(_depthOfFieldGrid, stage,
                "EnableDOFBlur", "DOFFocalLength", "DOFShortDistance", "DOFLongDistance", "DOFAlpha", "EnableDOFEdgeBlur");
            SetPropertySubset(_lightPointGrid, stage,
                "LightPointDirectionX", "LightPointDirectionY", "LightPointDirectionZ", "EnableShadowColor");
            SetPropertySubset(_colorsGrid, stage,
                "PlayerAmbientColor", "RayCutOffShadeColor", "EffectAmbientColor", "UnknownColor",
                "ParallelAmbientColor", "RayCutOffNormalColor", "ShadowColor", "RockColor");
            SetPropertySubset(_lensExposureGrid, stage,
                "EnableBrightnessAdjustment", "Brightness", "Contrast", "EnableLensFlare", "LensFlare",
                "LensFlarePositionX", "LensFlarePositionY", "LensFlarePositionZ", "LensFlareAlpha");
        }

        private void BindObjectPropertyGrids(StageInfoObject stageObject)
        {
            SetPropertySubset(_objectSettingsGrid, stageObject,
                "ObjectFilePath", "ObjectName", "PositionFilePath", "PositionBoneName",
                "EntryType", "AnimationSpeed", "EnableCameraHideObject", "IsRigidBody");
            SetPropertySubset(_breakableObjectGrid, stageObject,
                "BreakableObjectPath", "BreakableObjectEffect01", "BreakableObjectSpeed01",
                "BreakableObjectEffect02", "BreakableObjectSpeed02", "BreakableObjectEffect03", "BreakableObjectSpeed03");
            SetPropertySubset(_breakableWallGrid, stageObject,
                "BreakableWallEffect01", "BreakableWallValue1", "BreakableWallValue2", "BreakableWallEffect02",
                "BreakableWallEffect03", "BreakableWallVolume", "BreakableWallSound");
            SetPropertySubset(_objectAdvancedGrid, stageObject, "BreakableConstantA", "BreakableConstantB");
        }

        private static void SetPropertySubset(PropertyGrid grid, object target, params string[] propertyNames)
        {
            grid.SelectedObject = target == null ? null : new PropertySubsetView(target, propertyNames);
        }

        private void RefreshStagePropertyGrids()
        {
            foreach (PropertyGrid grid in new[] { _fogMonoGrid, _glareSoftFocusGrid, _sunShaftGrid, _depthOfFieldGrid, _lightPointGrid, _colorsGrid, _lensExposureGrid })
                grid.Refresh();
        }

        private void RefreshObjectPropertyGrids()
        {
            foreach (PropertyGrid grid in new[] { _objectSettingsGrid, _breakableObjectGrid, _breakableWallGrid, _objectAdvancedGrid })
                grid.Refresh();
        }

        private void RefreshStageList(StageInfoStage preferred)
        {
            string filter = (_stageSearchBox.Text ?? "").Trim();
            _loadingUi = true;
            try
            {
                _stageList.BeginUpdate();
                _stageList.Items.Clear();
                for (int i = 0; i < _stages.Count; i++)
                {
                    StageInfoStage stage = _stages[i];
                    if (!StageMatches(stage, filter)) continue;
                    _stageList.Items.Add(new StageListItem(stage, i));
                }
                _stageList.EndUpdate();

                int selectedIndex = -1;
                if (preferred != null)
                {
                    for (int i = 0; i < _stageList.Items.Count; i++)
                    {
                        StageListItem item = (StageListItem)_stageList.Items[i];
                        if (ReferenceEquals(item.Stage, preferred)) { selectedIndex = i; break; }
                    }
                }
                if (selectedIndex < 0 && _stageList.Items.Count > 0)
                    selectedIndex = 0;
                _stageList.SelectedIndex = selectedIndex;
            }
            finally
            {
                _loadingUi = false;
            }

            StageListItem selected = _stageList.SelectedItem as StageListItem;
            StageInfoStage selectedStage = selected == null ? null : selected.Stage;
            if (!ReferenceEquals(selectedStage, _currentStage))
                SetCurrentStage(selectedStage);
            UpdateStatus();
        }

        private static bool StageMatches(StageInfoStage stage, string filter)
        {
            if (string.IsNullOrEmpty(filter)) return true;
            return ContainsIgnoreCase(stage.StageName, filter) ||
                   ContainsIgnoreCase(stage.StageMessageID, filter) ||
                   ContainsIgnoreCase(stage.StageFilter, filter);
        }

        private static bool ContainsIgnoreCase(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void AddStage(bool duplicate)
        {
            if (!ConfirmDiscardAllEntryEdits()) return;
            StageInfoStage stage = duplicate && _currentStage != null ? _currentStage.DeepClone() : StageInfoStage.CreateDefault();
            if (duplicate)
                stage.StageName = MakeUniqueStageName(stage.StageName + "_copy");
            else
                stage.StageName = MakeUniqueStageName("STAGE_");
            _stages.Add(stage);
            EntryCount = _stages.Count;
            MarkDirty();
            RefreshStageList(stage);
        }

        private string MakeUniqueStageName(string baseName)
        {
            string candidate = baseName;
            int suffix = 2;
            while (_stages.Any(stage => string.Equals(stage.StageName, candidate, StringComparison.OrdinalIgnoreCase)))
                candidate = baseName + suffix++;
            return candidate;
        }

        private void DeleteStage()
        {
            if (_currentStage == null) return;
            if (!ConfirmDiscardAllEntryEdits()) return;
            if (MessageBox.Show(this, "Delete stage '" + _currentStage.StageName + "' and all of its paths/objects?", "Delete stage",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            int index = _stages.IndexOf(_currentStage);
            _stages.Remove(_currentStage);
            EntryCount = _stages.Count;
            StageInfoStage next = _stages.Count == 0 ? null : _stages[Math.Min(index, _stages.Count - 1)];
            MarkDirty();
            RefreshStageList(next);
        }

        private void MoveStage(int delta)
        {
            if (_currentStage == null) return;
            if (!ConfirmDiscardAllEntryEdits()) return;
            int oldIndex = _stages.IndexOf(_currentStage);
            int newIndex = oldIndex + delta;
            if (newIndex < 0 || newIndex >= _stages.Count) return;
            _stages.RaiseListChangedEvents = false;
            _stages.RemoveAt(oldIndex);
            _stages.Insert(newIndex, _currentStage);
            _stages.RaiseListChangedEvents = true;
            _stages.ResetBindings();
            MarkDirty();
            RefreshStageList(_currentStage);
            SetCurrentStage(_currentStage);
        }

        private void CopyStageProperties()
        {
            if (_stageEditBuffer == null) return;
            _copiedStageProperties = _stageEditBuffer.DeepClone();
            RefreshCommandState();
            SetStatusMessage("Copied the displayed stage properties. No entry was saved.");
        }

        private void PasteStageProperties()
        {
            if (_stageEditBuffer == null || _copiedStageProperties == null) return;
            _stageEditBuffer.CopySettingsFrom(_copiedStageProperties);
            RefreshStageEditors();
            SetStageEditDirty();
            SetStatusMessage("Pasted into the stage edit buffer. Click the stage Save button to apply.");
        }

        private void SaveStageEntry()
        {
            if (_currentStage == null || _stageEditBuffer == null) return;
            if (_pathEditDirty)
            {
                MessageBox.Show(this, "Save or discard the current resource-path edit before saving the stage entry.",
                    "Pending path edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _currentStage.StageName = _stageEditBuffer.StageName ?? "";
            _currentStage.StageMessageID = _stageEditBuffer.StageMessageID ?? "";
            _currentStage.StageFilter = _stageEditBuffer.StageFilter ?? "";
            _currentStage.CopySettingsFrom(_stageEditBuffer);
            _stageEditDirty = false;
            SyncManagedResources(_currentStage, false);
            _stageEditBuffer = _currentStage.DeepClone();
            BindStagePropertyGrids(_stageEditBuffer);
            _stageList.Refresh();
            MarkDirty();
            RefreshCommandState();
            SetStatusMessage("Stage entry applied. Use File > Save to write the document.");
        }

        private void AddPath()
        {
            if (_currentStage == null) return;
            if (_pathEditDirty && !ConfirmDiscardEdit("resource path")) return;
            StageInfoPath path = new StageInfoPath { FilePath = "data/stage/" };
            _currentStage.FilePaths.Add(path);
            RefreshPathList(path);
            _pathEditorTextBox.Focus();
            _pathEditorTextBox.SelectAll();
            MarkDirty();
            RefreshCommandState();
        }

        private void SavePathEntry()
        {
            if (_currentPath == null) return;
            _currentPath.FilePath = _pathEditorTextBox.Text ?? "";
            _pathEditDirty = false;
            RefreshPathList(_currentPath);
            MarkDirty();
            SetStatusMessage("Resource-path entry applied. Use File > Save to write the document.");
        }

        private void DeletePath()
        {
            if (_currentStage == null || _currentPath == null) return;
            if (_pathEditDirty && !ConfirmDiscardEdit("resource path")) return;
            int index = _currentStage.FilePaths.IndexOf(_currentPath);
            _currentStage.FilePaths.Remove(_currentPath);
            StageInfoPath next = _currentStage.FilePaths.Count == 0 ? null : _currentStage.FilePaths[Math.Min(index, _currentStage.FilePaths.Count - 1)];
            RefreshPathList(next);
            MarkDirty();
            RefreshCommandState();
        }

        private void MovePath(int delta)
        {
            if (_currentStage == null || _currentPath == null) return;
            if (_pathEditDirty && !ConfirmDiscardEdit("resource path")) return;
            StageInfoPath path = _currentPath;
            int oldIndex = _currentStage.FilePaths.IndexOf(path);
            int newIndex = oldIndex + delta;
            if (oldIndex < 0 || newIndex < 0 || newIndex >= _currentStage.FilePaths.Count) return;
            _currentStage.FilePaths.RaiseListChangedEvents = false;
            _currentStage.FilePaths.RemoveAt(oldIndex);
            _currentStage.FilePaths.Insert(newIndex, path);
            _currentStage.FilePaths.RaiseListChangedEvents = true;
            _currentStage.FilePaths.ResetBindings();
            RefreshPathList(path);
            MarkDirty();
            RefreshCommandState();
        }

        private void RefreshPathList(StageInfoPath preferred)
        {
            bool previousLoading = _loadingUi;
            _loadingUi = true;
            try
            {
                _pathList.BeginUpdate();
                _pathList.Items.Clear();
                if (_currentStage != null)
                    foreach (StageInfoPath path in _currentStage.FilePaths) _pathList.Items.Add(path);
                _pathList.EndUpdate();
                SelectListItemCore(_pathList, preferred);
            }
            finally { _loadingUi = previousLoading; }
            SetCurrentPath(_pathList.SelectedItem as StageInfoPath);
        }

        private void AddObject()
        {
            if (_currentStage == null) return;
            if (_objectEditDirty && !ConfirmDiscardEdit("object entry")) return;
            StageInfoObject stageObject = StageInfoObject.CreateDefault();
            _currentStage.Objects.Add(stageObject);
            RefreshObjectList(stageObject);
            MarkDirty();
        }

        private void DuplicateObject()
        {
            if (_currentStage == null || _currentObject == null) return;
            if (_objectEditDirty && !ConfirmDiscardEdit("object entry")) return;
            StageInfoObject clone = _currentObject.DeepClone();
            clone.ObjectName = string.IsNullOrEmpty(clone.ObjectName) ? "object_copy" : clone.ObjectName + "_copy";
            int insertAt = _currentStage.Objects.IndexOf(_currentObject) + 1;
            _currentStage.Objects.Insert(insertAt, clone);
            RefreshObjectList(clone);
            MarkDirty();
        }

        private void SaveObjectEntry()
        {
            if (_currentStage == null || _currentObject == null || _objectEditBuffer == null) return;
            int index = _currentStage.Objects.IndexOf(_currentObject);
            if (index < 0) return;
            StageInfoObject saved = _objectEditBuffer.DeepClone();
            _currentStage.Objects[index] = saved;
            _objectEditDirty = false;
            RefreshObjectList(saved);
            MarkDirty();
            SetStatusMessage("Object entry applied. Use File > Save to write the document.");
        }

        private void DeleteObject()
        {
            if (_currentStage == null || _currentObject == null) return;
            if (_objectEditDirty && !ConfirmDiscardEdit("object entry")) return;
            if (MessageBox.Show(this, "Delete object '" + _currentObject.ObjectName + "'?", "Delete object",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            int index = _currentStage.Objects.IndexOf(_currentObject);
            _currentStage.Objects.Remove(_currentObject);
            StageInfoObject next = _currentStage.Objects.Count == 0 ? null : _currentStage.Objects[Math.Min(index, _currentStage.Objects.Count - 1)];
            RefreshObjectList(next);
            MarkDirty();
        }

        private void MoveObject(int delta)
        {
            if (_currentStage == null || _currentObject == null) return;
            if (_objectEditDirty && !ConfirmDiscardEdit("object entry")) return;
            int oldIndex = _currentStage.Objects.IndexOf(_currentObject);
            int newIndex = oldIndex + delta;
            if (newIndex < 0 || newIndex >= _currentStage.Objects.Count) return;
            StageInfoObject item = _currentObject;
            _currentStage.Objects.RaiseListChangedEvents = false;
            _currentStage.Objects.RemoveAt(oldIndex);
            _currentStage.Objects.Insert(newIndex, item);
            _currentStage.Objects.RaiseListChangedEvents = true;
            _currentStage.Objects.ResetBindings();
            RefreshObjectList(item);
            MarkDirty();
        }

        private void RefreshObjectList(StageInfoObject preferred)
        {
            bool previousLoading = _loadingUi;
            _loadingUi = true;
            try
            {
                _objectList.BeginUpdate();
                _objectList.Items.Clear();
                if (_currentStage != null)
                    foreach (StageInfoObject stageObject in _currentStage.Objects) _objectList.Items.Add(stageObject);
                _objectList.EndUpdate();
                SelectListItemCore(_objectList, preferred);
            }
            finally { _loadingUi = previousLoading; }
            SetCurrentObject(_objectList.SelectedItem as StageInfoObject);
        }

        private bool HasPendingEntryEdits
        {
            get { return _stageEditDirty || _pathEditDirty || _objectEditDirty; }
        }

        private void SetStageEditDirty()
        {
            _stageEditDirty = true;
            RefreshCommandState();
            UpdatePendingStatus();
        }

        private bool ConfirmDiscardEdit(string entryName)
        {
            return MessageBox.Show(this,
                "The selected " + entryName + " has changes that have not been applied. Discard those changes?",
                "Unsaved entry changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private bool ConfirmDiscardAllEntryEdits()
        {
            if (!HasPendingEntryEdits) return true;
            if (MessageBox.Show(this,
                    "One or more entries have unapplied changes. Discard all unapplied entry changes?",
                    "Unsaved entry changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return false;
            _stageEditDirty = false;
            _pathEditDirty = false;
            _objectEditDirty = false;
            return true;
        }

        private void SelectStageInList(StageInfoStage stage)
        {
            bool previousLoading = _loadingUi;
            _loadingUi = true;
            try
            {
                _stageList.SelectedIndex = -1;
                for (int i = 0; i < _stageList.Items.Count; i++)
                {
                    StageListItem item = _stageList.Items[i] as StageListItem;
                    if (item != null && ReferenceEquals(item.Stage, stage))
                    {
                        _stageList.SelectedIndex = i;
                        break;
                    }
                }
            }
            finally { _loadingUi = previousLoading; }
        }

        private void SelectListItem(ListBox list, object item)
        {
            bool previousLoading = _loadingUi;
            _loadingUi = true;
            try { SelectListItemCore(list, item); }
            finally { _loadingUi = previousLoading; }
        }

        private static void SelectListItemCore(ListBox list, object item)
        {
            list.SelectedIndex = -1;
            if (item == null) return;
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (!ReferenceEquals(list.Items[i], item)) continue;
                list.SelectedIndex = i;
                break;
            }
        }

        private void UpdatePendingStatus()
        {
            UpdateStatus();
        }

        private void SyncCurrentManagedResources()
        {
            if (_currentStage == null) return;
            if (_stageEditDirty || _pathEditDirty)
            {
                MessageBox.Show(this, "Save or discard the pending stage/path entry edit before synchronizing resources.",
                    "Pending entry edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SyncManagedResources(_currentStage, true);
        }

        private void SyncManagedResources(StageInfoStage stage, bool report)
        {
            if (stage == null) return;
            string[] managed =
            {
                "data/stage/sae_snow.xfbin",
                "data/stage/sae_rain.xfbin",
                "data/stage/lensFlare/uviolet_lensFlare.xfbin",
                "data/stage/lensFlare/oprism_lensFlare.xfbin",
                "data/stage/lensFlare/phalo_lensFlare.xfbin",
                "data/stage/lensFlare/gpurpose_lensFlare.xfbin",
                "data/stage/lensFlare/mlight_lensFlare.xfbin",
                "data/stage/lensFlare/sunset_lensFlare.xfbin"
            };
            List<StageInfoPath> retained = stage.FilePaths
                .Where(path => !managed.Contains(path.FilePath ?? "", StringComparer.OrdinalIgnoreCase))
                .Select(path => path.DeepClone()).ToList();

            if (stage.Weather == 1)
                retained.Add(new StageInfoPath { FilePath = managed[0] });
            else if (stage.Weather == 2)
                retained.Add(new StageInfoPath { FilePath = managed[1] });

            if (stage.EnableLensFlare && stage.LensFlare >= 0 && stage.LensFlare < Program.lensFlareList.Length)
                retained.Add(new StageInfoPath { FilePath = managed[2 + stage.LensFlare] });

            bool changed = retained.Count != stage.FilePaths.Count ||
                           retained.Where((path, index) => !string.Equals(path.FilePath, stage.FilePaths[index].FilePath, StringComparison.OrdinalIgnoreCase)).Any();
            if (!changed) return;

            stage.FilePaths.RaiseListChangedEvents = false;
            stage.FilePaths.Clear();
            foreach (StageInfoPath path in retained)
                stage.FilePaths.Add(path);
            stage.FilePaths.RaiseListChangedEvents = true;
            stage.FilePaths.ResetBindings();

            if (ReferenceEquals(stage, _currentStage))
                RefreshPathList(stage.FilePaths.Count > 0 ? stage.FilePaths[0] : null);
            MarkDirty();
            if (report)
                SetStatusMessage("Weather/lens flare resource paths synchronized.");
        }

        private void RefreshStageEditors()
        {
            _loadingUi = true;
            try
            {
                RefreshStageIdentityFields(_stageEditBuffer);
                BindStagePropertyGrids(_stageEditBuffer);
                RefreshStagePropertyGrids();
                UpdateRawInformation();
            }
            finally { _loadingUi = false; }
            _stageList.Refresh();
        }

        #endregion

        #region File operations

        public void OpenFile(string FileName = "")
        {
            if (!ConfirmDiscardAllEntryEdits() || !PromptSaveIfDirty()) return;
            if (string.IsNullOrEmpty(FileName))
            {
                using (OpenFileDialog dialog = new OpenFileDialog
                {
                    DefaultExt = "xfbin",
                    Filter = "XFBIN StageInfo (*.xfbin)|*.xfbin|All files (*.*)|*.*",
                    CheckFileExists = true,
                    Multiselect = false,
                    Title = "Open StageInfo"
                })
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    FileName = dialog.FileName;
                }
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(FileName);
                StageInfoDocument document = StageInfoSerializer.Read(bytes);
                LoadDocument(document, bytes, FileName);
                SetStatusMessage("Opened " + Path.GetFileName(FileName) + ".");
            }
            catch (Exception ex)
            {
                if (Visible)
                    MessageBox.Show(this, "Could not open StageInfo:\n\n" + ex.Message, "StageInfo error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    SetStatusMessage("Could not open StageInfo: " + ex.Message);
            }
        }

        private void LoadDocument(StageInfoDocument document, byte[] bytes, string path)
        {
            _loadingUi = true;
            try
            {
                _stages.Clear();
                foreach (StageInfoStage stage in document.Stages)
                    _stages.Add(stage);
                fileBytes = bytes;
                FilePath = path ?? "";
                FileOpen = true;
                EntryCount = _stages.Count;
                _fileBinName = string.IsNullOrWhiteSpace(document.BinName) ? "stageInfo" : document.BinName;
                header = document.MarkerOffset >= 0
                    ? bytes.Take(Math.Min(bytes.Length, document.MarkerOffset + 0x10)).ToArray()
                    : new byte[0];
                _stageSearchBox.Text = "";
                SetDirty(false);
            }
            finally
            {
                _loadingUi = false;
            }
            RefreshStageList(_stages.Count > 0 ? _stages[0] : null);
            RefreshCommandState();
            UpdateStatus();
        }

        public void CloseFile()
        {
            if (!ConfirmDiscardAllEntryEdits() || !PromptSaveIfDirty()) return;
            ClearDocument();
        }

        private void ClearDocument()
        {
            _stages.Clear();
            fileBytes = new byte[0];
            header = new byte[0];
            FilePath = "";
            FileOpen = false;
            EntryCount = 0;
            _fileBinName = "stageInfo";
            _copiedStageProperties = null;
            _stageEditBuffer = null;
            _objectEditBuffer = null;
            _currentPath = null;
            _stageEditDirty = false;
            _pathEditDirty = false;
            _objectEditDirty = false;
            SetDirty(false);
            RefreshStageList(null);
            SetStatusMessage("Document closed.");
        }

        public void SaveFile()
        {
            SaveDocument(false, "");
        }

        public void SaveFileAs(string basepath = "")
        {
            SaveDocument(true, basepath);
        }

        private bool SaveDocument(bool saveAs, string requestedPath)
        {
            if (!EnsureEntriesAppliedBeforeFileSave()) return false;
            string outputPath = requestedPath;
            if (!saveAs && string.IsNullOrEmpty(outputPath))
                outputPath = FilePath;

            if (string.IsNullOrEmpty(outputPath))
            {
                using (SaveFileDialog dialog = new SaveFileDialog
                {
                    DefaultExt = "xfbin",
                    Filter = "XFBIN StageInfo (*.xfbin)|*.xfbin|All files (*.*)|*.*",
                    AddExtension = true,
                    OverwritePrompt = true,
                    FileName = string.IsNullOrEmpty(outputPath) ? (string.IsNullOrEmpty(FilePath) ? _fileBinName + ".bin.xfbin" : Path.GetFileName(FilePath)) : Path.GetFileName(outputPath),
                    InitialDirectory = string.IsNullOrEmpty(outputPath) ? (string.IsNullOrEmpty(FilePath) ? "" : Path.GetDirectoryName(FilePath)) : Path.GetDirectoryName(outputPath),
                    Title = "Save StageInfo"
                })
                {
                    if (!string.IsNullOrEmpty(outputPath)) dialog.FileName = outputPath;
                    if (dialog.ShowDialog(this) != DialogResult.OK) return false;
                    outputPath = dialog.FileName;
                }
            }

            try
            {
                List<string> problems = ValidateDocument();
                if (problems.Count > 0)
                    throw new InvalidDataException(string.Join(Environment.NewLine, problems));

                byte[] converted = ConvertToFile();
                StageInfoDocument validation = StageInfoSerializer.Read(converted);
                if (validation.Stages.Count != _stages.Count)
                    throw new InvalidDataException("Round-trip validation returned a different stage count.");

                bool backupCreated = File.Exists(outputPath);
                if (backupCreated)
                    File.Copy(outputPath, outputPath + ".backup", true);
                File.WriteAllBytes(outputPath, converted);

                fileBytes = converted;
                FilePath = outputPath;
                FileOpen = true;
                EntryCount = _stages.Count;
                StageInfoDocument savedDocument = StageInfoSerializer.Read(converted);
                header = converted.Take(savedDocument.MarkerOffset + 0x10).ToArray();
                SetDirty(false);
                UpdateRawRecordsFrom(savedDocument);
                UpdateRawInformation();
                SetStatusMessage("Saved " + Path.GetFileName(outputPath) + " (round-trip validated)." +
                    (backupCreated ? " Backup: " + Path.GetFileName(outputPath) + ".backup" : ""));
                return true;
            }
            catch (Exception ex)
            {
                if (Visible)
                    MessageBox.Show(this, "Could not save StageInfo:\n\n" + ex.Message, "StageInfo save error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    SetStatusMessage("Could not save StageInfo: " + ex.Message);
                return false;
            }
        }

        public byte[] ConvertToFile()
        {
            return StageInfoSerializer.Write(_stages, _fileBinName);
        }

        private bool EnsureEntriesAppliedBeforeFileSave()
        {
            if (!HasPendingEntryEdits) return true;
            string message = "One or more entries have unapplied edits. Click the Save button for each edited stage, path, or object before saving the file.";
            if (Visible)
                MessageBox.Show(this, message, "Unapplied entry edits", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                SetStatusMessage(message);
            return false;
        }

        private void UpdateRawRecordsFrom(StageInfoDocument savedDocument)
        {
            for (int i = 0; i < Math.Min(_stages.Count, savedDocument.Stages.Count); i++)
            {
                _stages[i].RawRecord = (byte[])savedDocument.Stages[i].RawRecord.Clone();
                for (int j = 0; j < Math.Min(_stages[i].Objects.Count, savedDocument.Stages[i].Objects.Count); j++)
                    _stages[i].Objects[j].RawRecord = (byte[])savedDocument.Stages[i].Objects[j].RawRecord.Clone();
            }
        }

        private bool PromptSaveIfDirty()
        {
            if (!_dirty) return true;
            DialogResult result = MessageBox.Show(this, "Save changes to the current StageInfo document?", "Unsaved changes",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return false;
            if (result == DialogResult.No) return true;
            return SaveDocument(false, "");
        }

        private List<string> ValidateDocument()
        {
            List<string> problems = new List<string>();
            for (int i = 0; i < _stages.Count; i++)
            {
                StageInfoStage stage = _stages[i];
                if (stage.StageName == null) problems.Add("Stage " + i + " has a null name.");
                if (stage.StageMessageID == null) problems.Add("Stage " + i + " has a null message ID.");
                if (stage.StageFilter == null) problems.Add("Stage " + i + " has a null filter.");
                if (stage.FilePaths.Any(path => path == null || path.FilePath == null)) problems.Add("Stage " + i + " contains a null resource path.");
                if (stage.Objects.Any(stageObject => stageObject == null)) problems.Add("Stage " + i + " contains a null object.");
            }
            return problems;
        }

        private void ValidateDocumentWithMessage()
        {
            try
            {
                if (!EnsureEntriesAppliedBeforeFileSave()) return;
                List<string> problems = ValidateDocument();
                if (problems.Count > 0)
                {
                    MessageBox.Show(this, string.Join(Environment.NewLine, problems), "Validation problems",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                byte[] bytes = ConvertToFile();
                StageInfoDocument roundTrip = StageInfoSerializer.Read(bytes);
                MessageBox.Show(this,
                    "Validation passed.\n\n" + roundTrip.Stages.Count + " stages\n" +
                    roundTrip.Stages.Sum(stage => stage.FilePaths.Count) + " resource paths\n" +
                    roundTrip.Stages.Sum(stage => stage.Objects.Count) + " objects\n" +
                    bytes.Length.ToString("N0", CultureInfo.InvariantCulture) + " generated bytes",
                    "StageInfo validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Validation failed:\n\n" + ex.Message, "StageInfo validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Status and display helpers

        private void MarkDirty()
        {
            if (_loadingUi) return;
            SetDirty(true);
        }

        private void SetDirty(bool value)
        {
            _dirty = value;
            UpdateStatus();
        }

        private void SetStatusMessage(string message)
        {
            _fileStatusLabel.Text = message;
            UpdateTitle();
        }

        private void UpdateStatus()
        {
            if (_fileStatusLabel == null) return;
            if (string.IsNullOrEmpty(_fileStatusLabel.Text) || _fileStatusLabel.Text == "No file open" || _fileStatusLabel.Text.StartsWith("File: ", StringComparison.Ordinal))
                _fileStatusLabel.Text = FileOpen ? "File: " + FilePath : "No file open";

            int shown = _stageList == null ? 0 : _stageList.Items.Count;
            int selectedIndex = _currentStage == null ? -1 : _stages.IndexOf(_currentStage);
            string selected = selectedIndex >= 0 ? "  |  selected " + (selectedIndex + 1) + "/" + _stages.Count : "";
            _selectionStatusLabel.Text = shown == _stages.Count
                ? _stages.Count + " stages" + selected
                : shown + " of " + _stages.Count + " stages" + selected;
            _stageCountLabel.Text = _stages.Count + " total  •  " + shown + " shown";
            _dirtyStatusLabel.Text = HasPendingEntryEdits ? "Entry edit pending" : (_dirty ? "Modified" : "Saved");
            UpdateTitle();
            UpdateFormatSummary();
        }

        private void UpdateTitle()
        {
            Text = "StageInfo Editor" + (_dirty ? " *" : "") + (HasPendingEntryEdits ? " [entry edit]" : "") +
                   (string.IsNullOrEmpty(FilePath) ? "" : " — " + Path.GetFileName(FilePath));
        }

        private void RefreshCommandState()
        {
            bool hasDocument = FileOpen || _stages.Count > 0;
            bool hasStage = _currentStage != null;
            bool hasPath = _currentPath != null;
            bool hasObject = _currentObject != null;
            bool canMoveStageUp = hasStage && _stages.IndexOf(_currentStage) > 0;
            bool canMoveStageDown = hasStage && _stages.IndexOf(_currentStage) >= 0 && _stages.IndexOf(_currentStage) < _stages.Count - 1;
            bool canMoveObjectUp = hasObject && _currentStage.Objects.IndexOf(_currentObject) > 0;
            bool canMoveObjectDown = hasObject && _currentStage.Objects.IndexOf(_currentObject) >= 0 && _currentStage.Objects.IndexOf(_currentObject) < _currentStage.Objects.Count - 1;

            _stageIdentityGroup.Enabled = hasStage;
            _resourcePathsGroup.Enabled = hasStage;
            _objectsGroup.Enabled = hasStage;
            _stageTabs.Enabled = hasStage;
            _objectTabs.Enabled = hasObject;
            _saveMenuItem.Enabled = hasDocument;
            _saveAsMenuItem.Enabled = hasDocument;
            _closeDocumentMenuItem.Enabled = hasDocument;
            _saveStageButton.Enabled = hasStage && _stageEditDirty;
            _validateMenuItem.Enabled = hasDocument;
            _validateButton.Enabled = hasDocument;

            _duplicateStageMenuItem.Enabled = hasStage;
            _deleteStageMenuItem.Enabled = hasStage;
            _copySettingsMenuItem.Enabled = hasStage;
            _pasteSettingsMenuItem.Enabled = hasStage && _copiedStageProperties != null;
            _syncResourcesMenuItem.Enabled = hasStage;
            _deleteStageButton.Enabled = hasStage;
            _duplicateStageButton.Enabled = hasStage;
            _moveStageUpButton.Enabled = canMoveStageUp;
            _moveStageDownButton.Enabled = canMoveStageDown;
            _copySettingsButton.Enabled = hasStage;
            _pasteSettingsButton.Enabled = hasStage && _copiedStageProperties != null;

            _addPathButton.Enabled = hasStage;
            _savePathButton.Enabled = hasPath && _pathEditDirty;
            _deletePathButton.Enabled = hasPath;
            _movePathUpButton.Enabled = hasPath && _currentStage.FilePaths.IndexOf(_currentPath) > 0;
            _movePathDownButton.Enabled = hasPath && _currentStage.FilePaths.IndexOf(_currentPath) < _currentStage.FilePaths.Count - 1;
            _syncPathButton.Enabled = hasStage;

            _addObjectButton.Enabled = hasStage;
            _duplicateObjectButton.Enabled = hasObject;
            _saveObjectButton.Enabled = hasObject && _objectEditDirty;
            _deleteObjectButton.Enabled = hasObject;
            _moveObjectUpButton.Enabled = canMoveObjectUp;
            _moveObjectDownButton.Enabled = canMoveObjectDown;
        }

        private void UpdateRawInformation()
        {
            if (_rawStageText != null)
                _rawStageText.Text = _currentStage == null ? "" : FormatHex(_currentStage.RawRecord, 16);
            if (_rawObjectText != null)
                _rawObjectText.Text = _currentObject == null ? "" : FormatHex(_currentObject.RawRecord, 16);
            UpdateFormatSummary();
        }

        private void UpdateFormatSummary()
        {
            if (_formatSummaryLabel == null) return;
            int stageIndex = _currentStage == null ? -1 : _stages.IndexOf(_currentStage);
            int objectIndex = _currentStage == null || _currentObject == null ? -1 : _currentStage.Objects.IndexOf(_currentObject);
            _formatSummaryLabel.Text =
                "Binary name: " + _fileBinName + Environment.NewLine +
                "Container: " + (string.IsNullOrEmpty(FilePath) ? "unsaved/new" : FilePath) + Environment.NewLine +
                "Stages: " + _stages.Count + "   Resource paths: " + _stages.Sum(stage => stage.FilePaths.Count) + "   Objects: " + _stages.Sum(stage => stage.Objects.Count) + Environment.NewLine +
                "Selected stage index: " + (stageIndex < 0 ? "none" : stageIndex + " (record offset = chunk + 0x10 + index × 0x130)") + Environment.NewLine +
                "Selected object index: " + (objectIndex < 0 ? "none" : objectIndex + " (record size 0xB0)");
        }

        private static void SetNumericValue(NumericUpDown numeric, float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) value = 0F;
            decimal converted;
            try { converted = (decimal)value; }
            catch { converted = 0M; }
            numeric.Value = Math.Max(numeric.Minimum, Math.Min(numeric.Maximum, converted));
        }

        private static void SetNumericValue(NumericUpDown numeric, int value)
        {
            numeric.Value = Math.Max(numeric.Minimum, Math.Min(numeric.Maximum, value));
        }

        private static string FormatHex(byte[] bytes, int columns)
        {
            if (bytes == null || bytes.Length == 0) return "No source record (new entry).";
            StringBuilder result = new StringBuilder(bytes.Length * 3 + bytes.Length / columns * 10);
            for (int i = 0; i < bytes.Length; i += columns)
            {
                result.Append(i.ToString("X4")).Append(":  ");
                int end = Math.Min(bytes.Length, i + columns);
                for (int j = i; j < end; j++) result.Append(bytes[j].ToString("X2")).Append(' ');
                result.AppendLine();
            }
            return result.ToString();
        }

        private static string GetObjectTypeLabel(int value)
        {
            if (value >= 0 && value < Program.TypeSectionList.Length)
                return Program.TypeSectionList[value];
            return value.ToString("X2") + " Unknown / custom";
        }

        #endregion

        #region Property-grid subsets

        /// <summary>
        /// Presents a focused set of properties from a model to a standard
        /// PropertyGrid.  This keeps the reference-style category tabs compact
        /// without creating a second copy of the StageInfo data.
        /// </summary>
        private sealed class PropertySubsetView : ICustomTypeDescriptor
        {
            private readonly object _target;
            private readonly string[] _propertyNames;

            public PropertySubsetView(object target, string[] propertyNames)
            {
                _target = target ?? throw new ArgumentNullException("target");
                _propertyNames = propertyNames ?? new string[0];
            }

            public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(_target, true); }
            public string GetClassName() { return TypeDescriptor.GetClassName(_target, true); }
            public string GetComponentName() { return TypeDescriptor.GetComponentName(_target, true); }
            public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(_target, true); }
            public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(_target, true); }
            public PropertyDescriptor GetDefaultProperty() { return null; }
            public object GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(_target, editorBaseType, true); }
            public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(_target, true); }
            public EventDescriptorCollection GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(_target, attributes, true); }
            public PropertyDescriptorCollection GetProperties() { return GetProperties(new Attribute[0]); }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                PropertyDescriptorCollection allProperties = TypeDescriptor.GetProperties(_target, attributes, true);
                List<PropertyDescriptor> selected = new List<PropertyDescriptor>();
                foreach (string propertyName in _propertyNames)
                {
                    PropertyDescriptor property = allProperties[propertyName];
                    if (property != null)
                        selected.Add(new ForwardingPropertyDescriptor(_target, property));
                }
                return new PropertyDescriptorCollection(selected.ToArray(), true);
            }

            public object GetPropertyOwner(PropertyDescriptor propertyDescriptor) { return this; }
        }

        private sealed class ForwardingPropertyDescriptor : PropertyDescriptor
        {
            private readonly object _target;
            private readonly PropertyDescriptor _property;

            public ForwardingPropertyDescriptor(object target, PropertyDescriptor property)
                : base(property)
            {
                _target = target;
                _property = property;
            }

            public override Type ComponentType { get { return typeof(PropertySubsetView); } }
            public override bool IsReadOnly { get { return _property.IsReadOnly; } }
            public override Type PropertyType { get { return _property.PropertyType; } }
            public override bool CanResetValue(object component) { return _property.CanResetValue(_target); }
            public override object GetValue(object component) { return _property.GetValue(_target); }
            public override void ResetValue(object component) { _property.ResetValue(_target); }
            public override bool ShouldSerializeValue(object component) { return _property.ShouldSerializeValue(_target); }

            public override void SetValue(object component, object value)
            {
                _property.SetValue(_target, value);
                OnValueChanged(component, EventArgs.Empty);
            }
        }

        #endregion

        #region Typed models

        private sealed class StageInfoDocument
        {
            public string BinName = "stageInfo";
            public int MarkerOffset;
            public readonly List<StageInfoStage> Stages = new List<StageInfoStage>();
        }

        private sealed class StageInfoPath
        {
            [DisplayName("Resource path")]
            [Description("XFBIN resource path referenced by this stage. Managed entries can be regenerated from the stage settings.")]
            public string FilePath { get; set; } = "";
            public StageInfoPath DeepClone() { return new StageInfoPath { FilePath = FilePath ?? "" }; }
            public override string ToString() { return string.IsNullOrWhiteSpace(FilePath) ? "<empty resource path>" : FilePath; }
        }

        private sealed class StageInfoObject
        {
            [Category("Object identity")]
            [DisplayName("Object file path  [0x00]")]
            [Description("Pointer to the object's XFBIN resource path in the 0xB0-byte object record.")]
            public string ObjectFilePath { get; set; } = "";

            [Category("Object identity")]
            [DisplayName("Object name  [0x08]")]
            [Description("Object/model name stored by pointer in the object record.")]
            public string ObjectName { get; set; } = "";

            [Category("Placement")]
            [DisplayName("Position file path  [0x10]")]
            [Description("Optional resource that supplies the object's placement transform.")]
            public string PositionFilePath { get; set; } = "";

            [Category("Placement")]
            [DisplayName("Position bone name  [0x18]")]
            [Description("Optional bone or locator used when attaching the object.")]
            public string PositionBoneName { get; set; } = "";

            [Category("Object identity")]
            [DisplayName("Entry type  [0x20]")]
            [Description("Integer object-entry discriminator used by the game.")]
            public int EntryType { get; set; }

            [Category("Rendering and physics")]
            [DisplayName("Camera-hide object  [0x28]")]
            [Description("When enabled, the object can be hidden by the camera visibility system.")]
            public bool EnableCameraHideObject { get; set; }

            [Category("Rendering and physics")]
            [DisplayName("Rigid body  [0x2C]")]
            [Description("Marks the object as using rigid-body behavior.")]
            public bool IsRigidBody { get; set; }

            [Category("Animation")]
            [DisplayName("Animation speed  [0x24]")]
            [Description("Playback speed multiplier. A value of 1.0 is normal speed.")]
            public float AnimationSpeed { get; set; }

            [Category("Breakable wall")]
            [DisplayName("Value 1  [0x80]")]
            [Description("Unknown signed 32-bit breakable-wall parameter preserved at object offset 0x80.")]
            public int BreakableWallValue1 { get; set; }

            [Category("Breakable wall")]
            [DisplayName("Value 2  [0x84]")]
            [Description("Unknown signed 32-bit breakable-wall parameter preserved at object offset 0x84.")]
            public int BreakableWallValue2 { get; set; }

            [Category("Breakable wall")]
            [DisplayName("Effect 1  [0x78]")]
            [Description("Primary breakable-wall effect resource/name.")]
            public string BreakableWallEffect01 { get; set; } = "";

            [Category("Breakable wall")]
            [DisplayName("Effect 2  [0x88]")]
            [Description("Secondary breakable-wall effect resource/name.")]
            public string BreakableWallEffect02 { get; set; } = "";

            [Category("Breakable wall")]
            [DisplayName("Effect 3  [0x90]")]
            [Description("Tertiary breakable-wall effect resource/name.")]
            public string BreakableWallEffect03 { get; set; } = "";

            [Category("Breakable wall")]
            [DisplayName("Sound  [0xA0]")]
            [Description("Sound cue or resource used when the wall breaks.")]
            public string BreakableWallSound { get; set; } = "";

            [Category("Breakable wall")]
            [DisplayName("Volume  [0x98]")]
            [Description("Break sound volume multiplier.")]
            public float BreakableWallVolume { get; set; }

            [Category("Breakable object")]
            [DisplayName("Object path  [0x38]")]
            [Description("Resource path used for breakable-object behavior.")]
            public string BreakableObjectPath { get; set; } = "";

            [Category("Breakable object")]
            [DisplayName("Effect 1  [0x40]")]
            [Description("First breakable-object effect resource/name.")]
            public string BreakableObjectEffect01 { get; set; } = "";

            [Category("Breakable object")]
            [DisplayName("Effect 2  [0x50]")]
            [Description("Second breakable-object effect resource/name.")]
            public string BreakableObjectEffect02 { get; set; } = "";

            [Category("Breakable object")]
            [DisplayName("Effect 3  [0x60]")]
            [Description("Third breakable-object effect resource/name.")]
            public string BreakableObjectEffect03 { get; set; } = "";

            [Category("Breakable object")]
            [DisplayName("Effect 1 speed  [0x48]")]
            [Description("Speed/intensity paired with breakable-object effect 1.")]
            public float BreakableObjectSpeed01 { get; set; }

            [Category("Breakable object")]
            [DisplayName("Effect 2 speed  [0x58]")]
            [Description("Speed/intensity paired with breakable-object effect 2.")]
            public float BreakableObjectSpeed02 { get; set; }

            [Category("Breakable object")]
            [DisplayName("Effect 3 speed  [0x68]")]
            [Description("Speed/intensity paired with breakable-object effect 3.")]
            public float BreakableObjectSpeed03 { get; set; }

            [Category("Advanced / preserved")]
            [DisplayName("Breakable constant A  [0x70]")]
            [Description("Preserved integer normally equal to 0x3C (60). Change only when the target format requires it.")]
            public int BreakableConstantA { get; set; } = 0x3C;

            [Category("Advanced / preserved")]
            [DisplayName("Breakable constant B  [0x74]")]
            [Description("Preserved integer normally equal to 0x78 (120). Change only when the target format requires it.")]
            public int BreakableConstantB { get; set; } = 0x78;

            [Browsable(false)]
            public byte[] RawRecord { get; set; } = new byte[0xB0];

            public static StageInfoObject CreateDefault()
            {
                return new StageInfoObject { AnimationSpeed = 1F, BreakableConstantA = 0x3C, BreakableConstantB = 0x78 };
            }

            public StageInfoObject DeepClone()
            {
                return new StageInfoObject
                {
                    ObjectFilePath = ObjectFilePath ?? "",
                    ObjectName = ObjectName ?? "",
                    PositionFilePath = PositionFilePath ?? "",
                    PositionBoneName = PositionBoneName ?? "",
                    EntryType = EntryType,
                    EnableCameraHideObject = EnableCameraHideObject,
                    IsRigidBody = IsRigidBody,
                    AnimationSpeed = AnimationSpeed,
                    BreakableWallValue1 = BreakableWallValue1,
                    BreakableWallValue2 = BreakableWallValue2,
                    BreakableWallEffect01 = BreakableWallEffect01 ?? "",
                    BreakableWallEffect02 = BreakableWallEffect02 ?? "",
                    BreakableWallEffect03 = BreakableWallEffect03 ?? "",
                    BreakableWallSound = BreakableWallSound ?? "",
                    BreakableWallVolume = BreakableWallVolume,
                    BreakableObjectPath = BreakableObjectPath ?? "",
                    BreakableObjectEffect01 = BreakableObjectEffect01 ?? "",
                    BreakableObjectEffect02 = BreakableObjectEffect02 ?? "",
                    BreakableObjectEffect03 = BreakableObjectEffect03 ?? "",
                    BreakableObjectSpeed01 = BreakableObjectSpeed01,
                    BreakableObjectSpeed02 = BreakableObjectSpeed02,
                    BreakableObjectSpeed03 = BreakableObjectSpeed03,
                    BreakableConstantA = BreakableConstantA,
                    BreakableConstantB = BreakableConstantB,
                    RawRecord = RawRecord == null ? new byte[0xB0] : (byte[])RawRecord.Clone()
                };
            }

            public override string ToString()
            {
                string name = string.IsNullOrWhiteSpace(ObjectName) ? "<unnamed object>" : ObjectName;
                return GetObjectTypeLabel(EntryType) + "  " + name;
            }
        }

        private sealed class StageInfoStage
        {
            [Category("Identity and environment")]
            [DisplayName("Stage name  [0x00]")]
            [Description("Stage identifier stored as an 8-byte string pointer in the 0x130-byte stage record.")]
            public string StageName { get; set; } = "";

            [Category("Identity and environment")]
            [DisplayName("Stage message ID  [0x08]")]
            [Description("Message/localization identifier associated with the stage.")]
            public string StageMessageID { get; set; } = "";

            [Category("Identity and environment")]
            [DisplayName("Stage filter  [0x10]")]
            [Description("Optional post-processing stage-filter resource or identifier.")]
            public string StageFilter { get; set; } = "";

            [Category("Identity and environment")]
            [DisplayName("Weather  [0x38]")]
            [Description("Weather preset index. Resource synchronization uses this value to maintain weather-related paths.")]
            public int Weather { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Player ambient color  [0x3C]")]
            [Description("Packed ARGB player ambient-light color.")]
            public Color PlayerAmbientColor { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Ray cutoff shade color  [0x40]")]
            [Description("Packed ARGB shade color used by ray cutoff lighting.")]
            public Color RayCutOffShadeColor { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Effect ambient color  [0x44]")]
            [Description("Packed ARGB ambient color applied to effects.")]
            public Color EffectAmbientColor { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Unknown color  [0x48]")]
            [Description("Preserved packed ARGB color at stage offset 0x48; its game-facing purpose is not yet confirmed.")]
            public Color UnknownColor { get; set; }

            [Category("Brightness and contrast")]
            [DisplayName("Enable adjustment  [0x4C]")]
            [Description("Enables the stage brightness/contrast adjustment block.")]
            public bool EnableBrightnessAdjustment { get; set; }

            [Category("Brightness and contrast")]
            [DisplayName("Brightness  [0x50]")]
            [Description("Stage brightness adjustment value.")]
            public float Brightness { get; set; }

            [Category("Brightness and contrast")]
            [DisplayName("Contrast  [0x54]")]
            [Description("Stage contrast adjustment value.")]
            public float Contrast { get; set; }

            [Category("Lens flare")]
            [DisplayName("Enable lens flare  [0x58]")]
            [Description("Enables the lens-flare effect and its managed resource path.")]
            public bool EnableLensFlare { get; set; }

            [Category("Lens flare")]
            [DisplayName("Preset  [0x5C]")]
            [Description("Lens-flare preset/index. Resource synchronization derives the flare resource from this value.")]
            public int LensFlare { get; set; }

            [Category("Lens flare")]
            [DisplayName("Position X  [0x60]")]
            [Description("Lens-flare position or direction X component.")]
            public float LensFlarePositionX { get; set; }

            [Category("Lens flare")]
            [DisplayName("Position Y  [0x64]")]
            [Description("Lens-flare position or direction Y component.")]
            public float LensFlarePositionY { get; set; }

            [Category("Lens flare")]
            [DisplayName("Position Z  [0x68]")]
            [Description("Lens-flare position or direction Z component.")]
            public float LensFlarePositionZ { get; set; }

            [Category("Lens flare")]
            [DisplayName("Alpha  [0x6C]")]
            [Description("Lens-flare opacity/intensity multiplier.")]
            public float LensFlareAlpha { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Parallel ambient color  [0x70]")]
            [Description("Packed ARGB parallel/directional ambient-light color.")]
            public Color ParallelAmbientColor { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Ray cutoff normal color  [0x74]")]
            [Description("Packed ARGB normal color used by ray cutoff lighting.")]
            public Color RayCutOffNormalColor { get; set; }

            [Category("Light direction and shadow")]
            [DisplayName("Light direction X  [0x78]")]
            [Description("Main light direction X component.")]
            public float LightPointDirectionX { get; set; }

            [Category("Light direction and shadow")]
            [DisplayName("Light direction Y  [0x7C]")]
            [Description("Main light direction Y component.")]
            public float LightPointDirectionY { get; set; }

            [Category("Light direction and shadow")]
            [DisplayName("Light direction Z  [0x80]")]
            [Description("Main light direction Z component.")]
            public float LightPointDirectionZ { get; set; }

            [Category("Light direction and shadow")]
            [DisplayName("Enable shadow color  [0x84]")]
            [Description("Enables the custom packed shadow color.")]
            public bool EnableShadowColor { get; set; }

            [Category("Light direction and shadow")]
            [DisplayName("Shadow color  [0x88]")]
            [Description("Packed ARGB custom shadow color.")]
            public Color ShadowColor { get; set; }

            [Category("Fog")]
            [DisplayName("Enable fog  [0x8C]")]
            [Description("Enables the stage fog block.")]
            public bool EnableFog { get; set; }

            [Category("Fog")]
            [DisplayName("Start distance  [0x90]")]
            [Description("Distance at which fog begins.")]
            public float FogStartDistance { get; set; }

            [Category("Fog")]
            [DisplayName("End distance  [0x94]")]
            [Description("Distance at which fog reaches its configured extent.")]
            public float FogEndDistance { get; set; }

            [Category("Fog")]
            [DisplayName("Strength  [0x98]")]
            [Description("Fog density/strength value.")]
            public float FogStrength { get; set; }

            [Category("Fog")]
            [DisplayName("Color  [0x9C]")]
            [Description("Packed ARGB fog color.")]
            public Color FogColor { get; set; }

            [Category("Monochrome filter")]
            [DisplayName("Enable mono filter  [0xA0]")]
            [Description("Enables the monochrome color-filter block.")]
            public bool EnableMonoColorFilter { get; set; }

            [Category("Monochrome filter")]
            [DisplayName("Blue tone  [0xA4]")]
            [Description("Blue contribution to the monochrome filter.")]
            public float MonoBlueTone { get; set; }

            [Category("Monochrome filter")]
            [DisplayName("Red tone  [0xA8]")]
            [Description("Red contribution to the monochrome filter.")]
            public float MonoRedTone { get; set; }

            [Category("Monochrome filter")]
            [DisplayName("Alpha  [0xAC]")]
            [Description("Monochrome filter blend amount.")]
            public float MonoAlpha { get; set; }

            [Category("Glare")]
            [DisplayName("Enable glare  [0xB0]")]
            [Description("Enables the glare/bloom block.")]
            public bool EnableGlareEffect { get; set; }

            [Category("Glare")]
            [DisplayName("Luminance threshold  [0xB4]")]
            [Description("Minimum luminance used to generate glare.")]
            public float GlareLuminanceThreshold { get; set; }

            [Category("Glare")]
            [DisplayName("Subtracted amount  [0xB8]")]
            [Description("Amount subtracted during glare extraction.")]
            public float GlareSubtracted { get; set; }

            [Category("Glare")]
            [DisplayName("Composition strength  [0xBC]")]
            [Description("Strength used when compositing glare back into the scene.")]
            public float GlareCompositionStrength { get; set; }

            [Category("Soft focus")]
            [DisplayName("Enable soft focus  [0xC4]")]
            [Description("Enables the soft-focus post-processing block.")]
            public bool EnableSoftFocus { get; set; }

            [Category("Soft focus")]
            [DisplayName("Strength  [0xC8]")]
            [Description("Soft-focus intensity.")]
            public float SoftFocusStrength { get; set; }

            [Category("Depth of field")]
            [DisplayName("Enable DOF blur  [0xCC]")]
            [Description("Enables depth-of-field blur.")]
            public bool EnableDOFBlur { get; set; }

            [Category("Depth of field")]
            [DisplayName("Focal length  [0xD0]")]
            [Description("Depth-of-field focal length/focus point.")]
            public float DOFFocalLength { get; set; }

            [Category("Depth of field")]
            [DisplayName("Short distance  [0xD4]")]
            [Description("Near depth-of-field distance.")]
            public float DOFShortDistance { get; set; }

            [Category("Depth of field")]
            [DisplayName("Long distance  [0xD8]")]
            [Description("Far depth-of-field distance.")]
            public float DOFLongDistance { get; set; }

            [Category("Depth of field")]
            [DisplayName("Alpha  [0xDC]")]
            [Description("Depth-of-field blend/opacity amount.")]
            public float DOFAlpha { get; set; }

            [Category("Depth of field")]
            [DisplayName("Enable edge blur  [0xE0]")]
            [Description("Enables the depth-of-field edge-blur option.")]
            public bool EnableDOFEdgeBlur { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Enable sun shaft  [0xE4]")]
            [Description("Enables the sun-shaft/god-ray block.")]
            public bool EnableSunShaft { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Start distance  [0xE8]")]
            [Description("Sun-shaft start distance.")]
            public float SunShaftStartDistance { get; set; }

            [Category("Sun shaft")]
            [DisplayName("End distance  [0xEC]")]
            [Description("Sun-shaft end distance.")]
            public float SunShaftEndDistance { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Alpha  [0xF0]")]
            [Description("Sun-shaft opacity/intensity multiplier.")]
            public float SunShaftAlpha { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Color  [0xF4]")]
            [Description("Packed ARGB sun-shaft color.")]
            public Color SunShaftColor { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Direction X  [0xF8]")]
            [Description("Sun-shaft direction X component.")]
            public float SunShaftDirectionX { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Direction Y  [0xFC]")]
            [Description("Sun-shaft direction Y component.")]
            public float SunShaftDirectionY { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Direction Z  [0x100]")]
            [Description("Sun-shaft direction Z component.")]
            public float SunShaftDirectionZ { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Blur width  [0x104]")]
            [Description("Width/radius of the sun-shaft blur.")]
            public float SunShaftBlurWidth { get; set; }

            [Category("Sun shaft")]
            [DisplayName("Attenuation coefficient  [0x108]")]
            [Description("Distance attenuation coefficient for sun shafts.")]
            public float SunShaftAttenuationCoefficient { get; set; }

            [Category("Lighting colors")]
            [DisplayName("Rock color  [0x10C]")]
            [Description("Packed ARGB color adjustment applied to rock/environment rendering.")]
            public Color RockColor { get; set; }

            [Browsable(false)]
            public BindingList<StageInfoPath> FilePaths { get; private set; } = new BindingList<StageInfoPath>();

            [Browsable(false)]
            public BindingList<StageInfoObject> Objects { get; private set; } = new BindingList<StageInfoObject>();

            [Browsable(false)]
            public byte[] RawRecord { get; set; } = new byte[0x130];

            public static StageInfoStage CreateDefault()
            {
                return new StageInfoStage
                {
                    StageName = "STAGE_",
                    PlayerAmbientColor = Color.FromArgb(255, 0, 0, 0),
                    RayCutOffShadeColor = Color.FromArgb(255, 0, 0, 0),
                    EffectAmbientColor = Color.FromArgb(255, 0, 0, 0),
                    UnknownColor = Color.FromArgb(255, 255, 255, 255),
                    LensFlareAlpha = 1F,
                    ParallelAmbientColor = Color.FromArgb(255, 0, 0, 0),
                    RayCutOffNormalColor = Color.FromArgb(255, 0, 0, 0),
                    ShadowColor = Color.FromArgb(255, 0, 0, 0),
                    FogColor = Color.FromArgb(255, 0, 0, 0),
                    MonoAlpha = 0.3F,
                    SunShaftColor = Color.FromArgb(255, 0, 0, 0),
                    RockColor = Color.FromArgb(255, 0, 0, 0)
                };
            }

            public StageInfoStage DeepClone()
            {
                StageInfoStage clone = CreateDefault();
                clone.StageName = StageName ?? "";
                clone.StageMessageID = StageMessageID ?? "";
                clone.StageFilter = StageFilter ?? "";
                clone.CopySettingsFrom(this);
                clone.RawRecord = RawRecord == null ? new byte[0x130] : (byte[])RawRecord.Clone();
                foreach (StageInfoPath path in FilePaths) clone.FilePaths.Add(path.DeepClone());
                foreach (StageInfoObject stageObject in Objects) clone.Objects.Add(stageObject.DeepClone());
                return clone;
            }

            public void CopySettingsFrom(StageInfoStage source)
            {
                Weather = source.Weather;
                PlayerAmbientColor = source.PlayerAmbientColor;
                RayCutOffShadeColor = source.RayCutOffShadeColor;
                EffectAmbientColor = source.EffectAmbientColor;
                UnknownColor = source.UnknownColor;
                EnableBrightnessAdjustment = source.EnableBrightnessAdjustment;
                Brightness = source.Brightness;
                Contrast = source.Contrast;
                EnableLensFlare = source.EnableLensFlare;
                LensFlare = source.LensFlare;
                LensFlarePositionX = source.LensFlarePositionX;
                LensFlarePositionY = source.LensFlarePositionY;
                LensFlarePositionZ = source.LensFlarePositionZ;
                LensFlareAlpha = source.LensFlareAlpha;
                ParallelAmbientColor = source.ParallelAmbientColor;
                RayCutOffNormalColor = source.RayCutOffNormalColor;
                LightPointDirectionX = source.LightPointDirectionX;
                LightPointDirectionY = source.LightPointDirectionY;
                LightPointDirectionZ = source.LightPointDirectionZ;
                EnableShadowColor = source.EnableShadowColor;
                ShadowColor = source.ShadowColor;
                EnableFog = source.EnableFog;
                FogStartDistance = source.FogStartDistance;
                FogEndDistance = source.FogEndDistance;
                FogStrength = source.FogStrength;
                FogColor = source.FogColor;
                EnableMonoColorFilter = source.EnableMonoColorFilter;
                MonoBlueTone = source.MonoBlueTone;
                MonoRedTone = source.MonoRedTone;
                MonoAlpha = source.MonoAlpha;
                EnableGlareEffect = source.EnableGlareEffect;
                GlareLuminanceThreshold = source.GlareLuminanceThreshold;
                GlareSubtracted = source.GlareSubtracted;
                GlareCompositionStrength = source.GlareCompositionStrength;
                EnableSoftFocus = source.EnableSoftFocus;
                SoftFocusStrength = source.SoftFocusStrength;
                EnableDOFBlur = source.EnableDOFBlur;
                DOFFocalLength = source.DOFFocalLength;
                DOFShortDistance = source.DOFShortDistance;
                DOFLongDistance = source.DOFLongDistance;
                DOFAlpha = source.DOFAlpha;
                EnableDOFEdgeBlur = source.EnableDOFEdgeBlur;
                EnableSunShaft = source.EnableSunShaft;
                SunShaftStartDistance = source.SunShaftStartDistance;
                SunShaftEndDistance = source.SunShaftEndDistance;
                SunShaftAlpha = source.SunShaftAlpha;
                SunShaftColor = source.SunShaftColor;
                SunShaftDirectionX = source.SunShaftDirectionX;
                SunShaftDirectionY = source.SunShaftDirectionY;
                SunShaftDirectionZ = source.SunShaftDirectionZ;
                SunShaftBlurWidth = source.SunShaftBlurWidth;
                SunShaftAttenuationCoefficient = source.SunShaftAttenuationCoefficient;
                RockColor = source.RockColor;
            }
        }

        private sealed class StageListItem
        {
            public readonly StageInfoStage Stage;
            public readonly int OriginalIndex;
            public StageListItem(StageInfoStage stage, int originalIndex) { Stage = stage; OriginalIndex = originalIndex; }
            public override string ToString()
            {
                string name = string.IsNullOrWhiteSpace(Stage.StageName) ? "<unnamed stage>" : Stage.StageName;
                return OriginalIndex.ToString("D3") + "  " + name;
            }
        }

        #endregion

        #region Binary codec

        private static class StageInfoSerializer
        {
            private const int StageRecordSize = 0x130;
            private const int ObjectRecordSize = 0xB0;
            private static readonly byte[] Marker = { 0xF2, 0x03, 0x00, 0x00 };

            public static StageInfoDocument Read(byte[] data)
            {
                if (data == null || data.Length < 0x80)
                    throw new InvalidDataException("The file is too small to be a StageInfo XFBIN.");
                if (data[0] != 0x4E || data[1] != 0x55 || data[2] != 0x43 || data[3] != 0x43)
                    throw new InvalidDataException("Missing NUCC XFBIN signature.");

                int marker = FindMarker(data);
                if (marker < 0)
                    throw new InvalidDataException("Could not locate the StageInfo binary marker F2 03 00 00.");

                int count = ReadInt32(data, marker + 4);
                if (count < 0 || count > 100000)
                    throw new InvalidDataException("Invalid stage count: " + count + ".");
                int tableStart = checked(marker + 0x10);
                EnsureRange(data, tableStart, checked(count * StageRecordSize), "stage record table");

                StageInfoDocument document = new StageInfoDocument
                {
                    MarkerOffset = marker,
                    BinName = ReadBinName(data)
                };

                for (int index = 0; index < count; index++)
                {
                    int entry = checked(tableStart + index * StageRecordSize);
                    StageInfoStage stage = StageInfoStage.CreateDefault();
                    stage.RawRecord = Slice(data, entry, StageRecordSize);
                    stage.StageName = ReadRelativeString(data, entry + 0x00);
                    stage.StageMessageID = ReadRelativeString(data, entry + 0x08);
                    stage.StageFilter = ReadRelativeString(data, entry + 0x10);

                    int pathCount = ReadCount(data, entry + 0x18, "resource path", index);
                    int pathListBase = CheckedRelative(data, entry + 0x20, "resource path list");
                    EnsureRange(data, pathListBase, checked(pathCount * 0x08), "resource path list");
                    for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
                    {
                        int slot = checked(pathListBase + pathIndex * 0x08);
                        stage.FilePaths.Add(new StageInfoPath { FilePath = ReadRelativeString(data, slot) });
                    }

                    int objectCount = ReadCount(data, entry + 0x28, "object", index);
                    int objectListBase = CheckedRelative(data, entry + 0x30, "object list");
                    EnsureRange(data, objectListBase, checked(objectCount * ObjectRecordSize), "object list");
                    for (int objectIndex = 0; objectIndex < objectCount; objectIndex++)
                    {
                        int objectEntry = checked(objectListBase + objectIndex * ObjectRecordSize);
                        StageInfoObject stageObject = StageInfoObject.CreateDefault();
                        stageObject.RawRecord = Slice(data, objectEntry, ObjectRecordSize);
                        stageObject.ObjectFilePath = ReadRelativeString(data, objectEntry + 0x00);
                        stageObject.ObjectName = ReadRelativeString(data, objectEntry + 0x08);
                        stageObject.PositionFilePath = ReadRelativeString(data, objectEntry + 0x10);
                        stageObject.PositionBoneName = ReadRelativeString(data, objectEntry + 0x18);
                        stageObject.EntryType = ReadInt32(data, objectEntry + 0x20);
                        stageObject.AnimationSpeed = ReadSingle(data, objectEntry + 0x24);
                        stageObject.EnableCameraHideObject = ReadInt32(data, objectEntry + 0x28) != 0;
                        stageObject.IsRigidBody = ReadInt32(data, objectEntry + 0x2C) != 0;
                        stageObject.BreakableObjectPath = ReadRelativeString(data, objectEntry + 0x38);
                        stageObject.BreakableObjectEffect01 = ReadRelativeString(data, objectEntry + 0x40);
                        stageObject.BreakableObjectSpeed01 = ReadSingle(data, objectEntry + 0x48);
                        stageObject.BreakableObjectEffect02 = ReadRelativeString(data, objectEntry + 0x50);
                        stageObject.BreakableObjectSpeed02 = ReadSingle(data, objectEntry + 0x58);
                        stageObject.BreakableObjectEffect03 = ReadRelativeString(data, objectEntry + 0x60);
                        stageObject.BreakableObjectSpeed03 = ReadSingle(data, objectEntry + 0x68);
                        stageObject.BreakableConstantA = ReadInt32(data, objectEntry + 0x70);
                        stageObject.BreakableConstantB = ReadInt32(data, objectEntry + 0x74);
                        stageObject.BreakableWallEffect01 = ReadRelativeString(data, objectEntry + 0x78);
                        stageObject.BreakableWallValue1 = ReadInt32(data, objectEntry + 0x80);
                        stageObject.BreakableWallValue2 = ReadInt32(data, objectEntry + 0x84);
                        stageObject.BreakableWallEffect02 = ReadRelativeString(data, objectEntry + 0x88);
                        stageObject.BreakableWallEffect03 = ReadRelativeString(data, objectEntry + 0x90);
                        stageObject.BreakableWallVolume = ReadSingle(data, objectEntry + 0x98);
                        stageObject.BreakableWallSound = ReadRelativeString(data, objectEntry + 0xA0);
                        stage.Objects.Add(stageObject);
                    }

                    stage.Weather = ReadInt32(data, entry + 0x38);
                    stage.PlayerAmbientColor = ReadColor(data, entry + 0x3C);
                    stage.RayCutOffShadeColor = ReadColor(data, entry + 0x40);
                    stage.EffectAmbientColor = ReadColor(data, entry + 0x44);
                    stage.UnknownColor = ReadColor(data, entry + 0x48);
                    stage.EnableBrightnessAdjustment = ReadInt32(data, entry + 0x4C) != 0;
                    stage.Brightness = ReadSingle(data, entry + 0x50);
                    stage.Contrast = ReadSingle(data, entry + 0x54);
                    stage.EnableLensFlare = ReadInt32(data, entry + 0x58) != 0;
                    stage.LensFlare = ReadInt32(data, entry + 0x5C);
                    stage.LensFlarePositionX = ReadSingle(data, entry + 0x60);
                    stage.LensFlarePositionY = ReadSingle(data, entry + 0x64);
                    stage.LensFlarePositionZ = ReadSingle(data, entry + 0x68);
                    stage.LensFlareAlpha = ReadSingle(data, entry + 0x6C);
                    stage.ParallelAmbientColor = ReadColor(data, entry + 0x70);
                    stage.RayCutOffNormalColor = ReadColor(data, entry + 0x74);
                    stage.LightPointDirectionX = ReadSingle(data, entry + 0x78);
                    stage.LightPointDirectionY = ReadSingle(data, entry + 0x7C);
                    stage.LightPointDirectionZ = ReadSingle(data, entry + 0x80);
                    stage.EnableShadowColor = ReadInt32(data, entry + 0x84) != 0;
                    stage.ShadowColor = ReadColor(data, entry + 0x88);
                    stage.EnableFog = ReadInt32(data, entry + 0x8C) != 0;
                    stage.FogStartDistance = ReadSingle(data, entry + 0x90);
                    stage.FogEndDistance = ReadSingle(data, entry + 0x94);
                    stage.FogStrength = ReadSingle(data, entry + 0x98);
                    stage.FogColor = ReadColor(data, entry + 0x9C);
                    stage.EnableMonoColorFilter = ReadInt32(data, entry + 0xA0) != 0;
                    stage.MonoBlueTone = ReadSingle(data, entry + 0xA4);
                    stage.MonoRedTone = ReadSingle(data, entry + 0xA8);
                    stage.MonoAlpha = ReadSingle(data, entry + 0xAC);
                    stage.EnableGlareEffect = ReadInt32(data, entry + 0xB0) != 0;
                    stage.GlareLuminanceThreshold = ReadSingle(data, entry + 0xB4);
                    stage.GlareSubtracted = ReadSingle(data, entry + 0xB8);
                    stage.GlareCompositionStrength = ReadSingle(data, entry + 0xBC);
                    stage.EnableSoftFocus = ReadInt32(data, entry + 0xC4) != 0;
                    stage.SoftFocusStrength = ReadSingle(data, entry + 0xC8);
                    stage.EnableDOFBlur = ReadInt32(data, entry + 0xCC) != 0;
                    stage.DOFFocalLength = ReadSingle(data, entry + 0xD0);
                    stage.DOFShortDistance = ReadSingle(data, entry + 0xD4);
                    stage.DOFLongDistance = ReadSingle(data, entry + 0xD8);
                    stage.DOFAlpha = ReadSingle(data, entry + 0xDC);
                    stage.EnableDOFEdgeBlur = ReadInt32(data, entry + 0xE0) != 0;
                    stage.EnableSunShaft = ReadInt32(data, entry + 0xE4) != 0;
                    stage.SunShaftStartDistance = ReadSingle(data, entry + 0xE8);
                    stage.SunShaftEndDistance = ReadSingle(data, entry + 0xEC);
                    stage.SunShaftAlpha = ReadSingle(data, entry + 0xF0);
                    stage.SunShaftColor = ReadColor(data, entry + 0xF4);
                    stage.SunShaftDirectionX = ReadSingle(data, entry + 0xF8);
                    stage.SunShaftDirectionY = ReadSingle(data, entry + 0xFC);
                    stage.SunShaftDirectionZ = ReadSingle(data, entry + 0x100);
                    stage.SunShaftBlurWidth = ReadSingle(data, entry + 0x104);
                    stage.SunShaftAttenuationCoefficient = ReadSingle(data, entry + 0x108);
                    stage.RockColor = ReadColor(data, entry + 0x10C);
                    document.Stages.Add(stage);
                }
                return document;
            }

            public static byte[] Write(IList<StageInfoStage> stages, string fileBinName)
            {
                string binName = string.IsNullOrWhiteSpace(fileBinName) ? "stageInfo" : fileBinName;
                string binPath = "bin_le/x64/" + binName + ".bin";
                ByteBuilder writer = new ByteBuilder(8192);
                writer.Write(ContainerHeader);
                writer.WriteByte(0);
                writer.WriteCString(binPath);

                int pathPointer = writer.Length;
                writer.WriteByte(0);
                writer.WriteCString(binName);
                writer.WriteCString("Page0");
                writer.WriteCString("index");

                int namePointer = writer.Length;
                int beforeAlign = writer.Length;
                writer.Align4();
                int addedBytes = writer.Length - beforeAlign;

                writer.Write(new byte[48]
                {
                    0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,1,
                    0,0,0,1, 0,0,0,1, 0,0,0,2, 0,0,0,0,
                    0,0,0,2, 0,0,0,3, 0,0,0,0, 0,0,0,3
                });
                int sectionPointer = writer.Length;
                writer.Write(new byte[16] { 0,0,0,0, 0,0,0,1, 0,0,0,2, 0,0,0,3 });

                int totalLength = writer.Length;
                int pathLength = pathPointer - 127;
                int nameLength = namePointer - pathPointer;
                int sectionLength = sectionPointer - namePointer - addedBytes;
                int fullLength = totalLength - 68 + 40;
                writer.WriteBE32At(16, fullLength);
                writer.WriteBE32At(36, 2);
                writer.WriteBE32At(40, pathLength);
                writer.WriteBE32At(44, 4);
                writer.WriteBE32At(48, nameLength);
                writer.WriteBE32At(52, 4);
                writer.WriteBE32At(56, sectionLength);
                writer.WriteBE32At(60, 4);

                writer.Write(new byte[40]
                {
                    0,0,0,0, 0,0,0,0, 0,0x79,0,0, 0,0,0,0,
                    0,0,0,0, 0,0x79,0,0, 0,0,0,0, 0,0,0,1,
                    0,0x79,0,0, 0,0,0,0
                });
                int size1Index = writer.Length - 0x10;
                int size2Index = writer.Length - 0x04;
                int countIndex = writer.Length + 0x04;
                writer.Write(new byte[0x10] { 0xF2,0x03,0,0, 0,0,0,0, 0x08,0,0,0, 0,0,0,0 });

                int start = writer.Length;
                writer.WriteZeros(checked(stages.Count * StageRecordSize));
                int[] pathBases = new int[stages.Count];
                int[] objectBases = new int[stages.Count];

                for (int stageIndex = 0; stageIndex < stages.Count; stageIndex++)
                {
                    StageInfoStage stage = stages[stageIndex];
                    int entry = start + stageIndex * StageRecordSize;
                    if (stage.RawRecord != null && stage.RawRecord.Length == StageRecordSize)
                        writer.WriteAt(entry, stage.RawRecord);

                    int stageNamePosition = writer.Length; writer.WriteCString(stage.StageName);
                    int messagePosition = writer.Length; writer.WriteCString(stage.StageMessageID);
                    int filterPosition = writer.Length; writer.WriteCString(stage.StageFilter);
                    pathBases[stageIndex] = writer.Length; writer.WriteZeros(checked(stage.FilePaths.Count * 0x08));
                    objectBases[stageIndex] = writer.Length; writer.WriteZeros(checked(stage.Objects.Count * ObjectRecordSize));

                    writer.WriteLE32At(entry + 0x00, stageNamePosition - (entry + 0x00));
                    writer.WriteLE32At(entry + 0x08, messagePosition - (entry + 0x08));
                    writer.WriteLE32At(entry + 0x10, filterPosition - (entry + 0x10));
                    writer.WriteLE32At(entry + 0x18, stage.FilePaths.Count);
                    writer.WriteLE32At(entry + 0x20, pathBases[stageIndex] - (entry + 0x20));
                    writer.WriteLE32At(entry + 0x28, stage.Objects.Count);
                    writer.WriteLE32At(entry + 0x30, objectBases[stageIndex] - (entry + 0x30));
                    writer.WriteLE32At(entry + 0x38, stage.Weather);
                    writer.WriteColorAt(entry + 0x3C, stage.PlayerAmbientColor);
                    writer.WriteColorAt(entry + 0x40, stage.RayCutOffShadeColor);
                    writer.WriteColorAt(entry + 0x44, stage.EffectAmbientColor);
                    writer.WriteColorAt(entry + 0x48, stage.UnknownColor);
                    writer.WriteBool32At(entry + 0x4C, stage.EnableBrightnessAdjustment);
                    writer.WriteSingleAt(entry + 0x50, stage.Brightness);
                    writer.WriteSingleAt(entry + 0x54, stage.Contrast);
                    writer.WriteBool32At(entry + 0x58, stage.EnableLensFlare);
                    writer.WriteLE32At(entry + 0x5C, stage.LensFlare);
                    writer.WriteSingleAt(entry + 0x60, stage.LensFlarePositionX);
                    writer.WriteSingleAt(entry + 0x64, stage.LensFlarePositionY);
                    writer.WriteSingleAt(entry + 0x68, stage.LensFlarePositionZ);
                    writer.WriteSingleAt(entry + 0x6C, stage.LensFlareAlpha);
                    writer.WriteColorAt(entry + 0x70, stage.ParallelAmbientColor);
                    writer.WriteColorAt(entry + 0x74, stage.RayCutOffNormalColor);
                    writer.WriteSingleAt(entry + 0x78, stage.LightPointDirectionX);
                    writer.WriteSingleAt(entry + 0x7C, stage.LightPointDirectionY);
                    writer.WriteSingleAt(entry + 0x80, stage.LightPointDirectionZ);
                    writer.WriteBool32At(entry + 0x84, stage.EnableShadowColor);
                    writer.WriteColorAt(entry + 0x88, stage.ShadowColor);
                    writer.WriteBool32At(entry + 0x8C, stage.EnableFog);
                    writer.WriteSingleAt(entry + 0x90, stage.FogStartDistance);
                    writer.WriteSingleAt(entry + 0x94, stage.FogEndDistance);
                    writer.WriteSingleAt(entry + 0x98, stage.FogStrength);
                    writer.WriteColorAt(entry + 0x9C, stage.FogColor);
                    writer.WriteBool32At(entry + 0xA0, stage.EnableMonoColorFilter);
                    writer.WriteSingleAt(entry + 0xA4, stage.MonoBlueTone);
                    writer.WriteSingleAt(entry + 0xA8, stage.MonoRedTone);
                    writer.WriteSingleAt(entry + 0xAC, stage.MonoAlpha);
                    writer.WriteBool32At(entry + 0xB0, stage.EnableGlareEffect);
                    writer.WriteSingleAt(entry + 0xB4, stage.GlareLuminanceThreshold);
                    writer.WriteSingleAt(entry + 0xB8, stage.GlareSubtracted);
                    writer.WriteSingleAt(entry + 0xBC, stage.GlareCompositionStrength);
                    writer.WriteBool32At(entry + 0xC4, stage.EnableSoftFocus);
                    writer.WriteSingleAt(entry + 0xC8, stage.SoftFocusStrength);
                    writer.WriteBool32At(entry + 0xCC, stage.EnableDOFBlur);
                    writer.WriteSingleAt(entry + 0xD0, stage.DOFFocalLength);
                    writer.WriteSingleAt(entry + 0xD4, stage.DOFShortDistance);
                    writer.WriteSingleAt(entry + 0xD8, stage.DOFLongDistance);
                    writer.WriteSingleAt(entry + 0xDC, stage.DOFAlpha);
                    writer.WriteBool32At(entry + 0xE0, stage.EnableDOFEdgeBlur);
                    writer.WriteBool32At(entry + 0xE4, stage.EnableSunShaft);
                    writer.WriteSingleAt(entry + 0xE8, stage.SunShaftStartDistance);
                    writer.WriteSingleAt(entry + 0xEC, stage.SunShaftEndDistance);
                    writer.WriteSingleAt(entry + 0xF0, stage.SunShaftAlpha);
                    writer.WriteColorAt(entry + 0xF4, stage.SunShaftColor);
                    writer.WriteSingleAt(entry + 0xF8, stage.SunShaftDirectionX);
                    writer.WriteSingleAt(entry + 0xFC, stage.SunShaftDirectionY);
                    writer.WriteSingleAt(entry + 0x100, stage.SunShaftDirectionZ);
                    writer.WriteSingleAt(entry + 0x104, stage.SunShaftBlurWidth);
                    writer.WriteSingleAt(entry + 0x108, stage.SunShaftAttenuationCoefficient);
                    writer.WriteColorAt(entry + 0x10C, stage.RockColor);

                    for (int pathIndex = 0; pathIndex < stage.FilePaths.Count; pathIndex++)
                    {
                        int slot = pathBases[stageIndex] + pathIndex * 0x08;
                        writer.WriteLE32At(slot, writer.Length - slot);
                        writer.WriteCString(stage.FilePaths[pathIndex].FilePath);
                    }

                    for (int objectIndex = 0; objectIndex < stage.Objects.Count; objectIndex++)
                    {
                        StageInfoObject stageObject = stage.Objects[objectIndex];
                        int objectEntry = objectBases[stageIndex] + objectIndex * ObjectRecordSize;
                        if (stageObject.RawRecord != null && stageObject.RawRecord.Length == ObjectRecordSize)
                            writer.WriteAt(objectEntry, stageObject.RawRecord);

                        WriteRelativeString(writer, objectEntry + 0x00, stageObject.ObjectFilePath);
                        WriteRelativeString(writer, objectEntry + 0x08, stageObject.ObjectName);
                        WriteRelativeString(writer, objectEntry + 0x10, stageObject.PositionFilePath);
                        WriteRelativeString(writer, objectEntry + 0x18, stageObject.PositionBoneName);
                        writer.WriteLE32At(objectEntry + 0x20, stageObject.EntryType);
                        writer.WriteSingleAt(objectEntry + 0x24, stageObject.AnimationSpeed);
                        writer.WriteBool32At(objectEntry + 0x28, stageObject.EnableCameraHideObject);
                        writer.WriteBool32At(objectEntry + 0x2C, stageObject.IsRigidBody);
                        WriteRelativeString(writer, objectEntry + 0x38, stageObject.BreakableObjectPath);
                        WriteRelativeString(writer, objectEntry + 0x40, stageObject.BreakableObjectEffect01);
                        writer.WriteSingleAt(objectEntry + 0x48, stageObject.BreakableObjectSpeed01);
                        WriteRelativeString(writer, objectEntry + 0x50, stageObject.BreakableObjectEffect02);
                        writer.WriteSingleAt(objectEntry + 0x58, stageObject.BreakableObjectSpeed02);
                        WriteRelativeString(writer, objectEntry + 0x60, stageObject.BreakableObjectEffect03);
                        writer.WriteSingleAt(objectEntry + 0x68, stageObject.BreakableObjectSpeed03);
                        writer.WriteLE32At(objectEntry + 0x70, stageObject.BreakableConstantA);
                        writer.WriteLE32At(objectEntry + 0x74, stageObject.BreakableConstantB);
                        WriteRelativeString(writer, objectEntry + 0x78, stageObject.BreakableWallEffect01);
                        writer.WriteLE32At(objectEntry + 0x80, stageObject.BreakableWallValue1);
                        writer.WriteLE32At(objectEntry + 0x84, stageObject.BreakableWallValue2);
                        WriteRelativeString(writer, objectEntry + 0x88, stageObject.BreakableWallEffect02);
                        WriteRelativeString(writer, objectEntry + 0x90, stageObject.BreakableWallEffect03);
                        writer.WriteSingleAt(objectEntry + 0x98, stageObject.BreakableWallVolume);
                        WriteRelativeString(writer, objectEntry + 0xA0, stageObject.BreakableWallSound);
                    }
                }

                writer.WriteBE32At(size1Index, writer.Length - start + 0x14);
                writer.WriteBE32At(size2Index, writer.Length - start + 0x10);
                writer.WriteLE32At(countIndex, stages.Count);
                writer.Write(new byte[20] { 0,0,0,8, 0,0,0,2, 0,0x79,0xE9,0x77, 0,0,0,4, 0,0,0,0 });
                return writer.ToArray();
            }

            private static void WriteRelativeString(ByteBuilder writer, int pointerSlot, string value)
            {
                writer.WriteLE32At(pointerSlot, writer.Length - pointerSlot);
                writer.WriteCString(value);
            }

            private static int FindMarker(byte[] data)
            {
                int headerCandidate = -1;
                if (data.Length >= 20)
                {
                    int sectionSize = ReadInt32BigEndian(data, 16);
                    long candidate = 0x44L + sectionSize;
                    if (candidate >= 0 && candidate <= int.MaxValue && IsMarkerAt(data, (int)candidate))
                        headerCandidate = (int)candidate;
                }
                if (headerCandidate >= 0 && IsPlausibleMarker(data, headerCandidate)) return headerCandidate;
                for (int i = 0; i <= data.Length - Marker.Length; i++)
                    if (IsMarkerAt(data, i) && IsPlausibleMarker(data, i)) return i;
                return -1;
            }

            private static bool IsMarkerAt(byte[] data, int index)
            {
                return index >= 0 && index + 4 <= data.Length && data[index] == Marker[0] && data[index + 1] == Marker[1] && data[index + 2] == 0 && data[index + 3] == 0;
            }

            private static bool IsPlausibleMarker(byte[] data, int index)
            {
                if (index + 0x10 > data.Length) return false;
                int count = ReadInt32(data, index + 4);
                if (count < 0 || count > 100000) return false;
                long end = index + 0x10L + count * (long)StageRecordSize;
                return end <= data.Length;
            }

            private static string ReadBinName(byte[] data)
            {
                try
                {
                    if (data.Length <= 128) return "stageInfo";
                    int pathEnd;
                    ReadCString(data, 128, out pathEnd);
                    int nameStart = pathEnd + 1;
                    while (nameStart < data.Length && data[nameStart] == 0) nameStart++;
                    int ignored;
                    string name = ReadCString(data, nameStart, out ignored);
                    return string.IsNullOrWhiteSpace(name) ? "stageInfo" : name;
                }
                catch { return "stageInfo"; }
            }

            private static int ReadCount(byte[] data, int offset, string label, int stageIndex)
            {
                int count = ReadInt32(data, offset);
                if (count < 0 || count > 100000)
                    throw new InvalidDataException("Invalid " + label + " count " + count + " in stage " + stageIndex + ".");
                return count;
            }

            private static int CheckedRelative(byte[] data, int pointerSlot, string label)
            {
                int relative = ReadInt32(data, pointerSlot);
                long target = pointerSlot + (long)relative;
                if (target < 0 || target > data.Length)
                    throw new InvalidDataException("The " + label + " pointer at 0x" + pointerSlot.ToString("X") + " points outside the file.");
                return (int)target;
            }

            private static string ReadRelativeString(byte[] data, int pointerSlot)
            {
                int target = CheckedRelative(data, pointerSlot, "string");
                int ignored;
                return ReadCString(data, target, out ignored);
            }

            private static string ReadCString(byte[] data, int offset, out int terminator)
            {
                if (offset < 0 || offset >= data.Length)
                    throw new InvalidDataException("String offset 0x" + offset.ToString("X") + " is outside the file.");
                terminator = offset;
                while (terminator < data.Length && data[terminator] != 0) terminator++;
                if (terminator >= data.Length)
                    throw new InvalidDataException("Unterminated string at 0x" + offset.ToString("X") + ".");
                return Encoding.UTF8.GetString(data, offset, terminator - offset);
            }

            private static Color ReadColor(byte[] data, int offset)
            {
                EnsureRange(data, offset, 4, "color");
                return Color.FromArgb(data[offset], data[offset + 1], data[offset + 2], data[offset + 3]);
            }

            private static float ReadSingle(byte[] data, int offset)
            {
                EnsureRange(data, offset, 4, "float");
                return BitConverter.ToSingle(data, offset);
            }

            private static int ReadInt32(byte[] data, int offset)
            {
                EnsureRange(data, offset, 4, "int32");
                return data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16 | data[offset + 3] << 24;
            }

            private static int ReadInt32BigEndian(byte[] data, int offset)
            {
                EnsureRange(data, offset, 4, "big-endian int32");
                return data[offset] << 24 | data[offset + 1] << 16 | data[offset + 2] << 8 | data[offset + 3];
            }

            private static byte[] Slice(byte[] data, int offset, int count)
            {
                EnsureRange(data, offset, count, "record");
                byte[] result = new byte[count];
                Buffer.BlockCopy(data, offset, result, 0, count);
                return result;
            }

            private static void EnsureRange(byte[] data, int offset, int count, string label)
            {
                if (offset < 0 || count < 0 || (long)offset + count > data.Length)
                    throw new InvalidDataException("The " + label + " range at 0x" + offset.ToString("X") + " is outside the file.");
            }

            private static readonly byte[] ContainerHeader =
            {
                0x4E,0x55,0x43,0x43,0,0,0,0x79,0,0,0,0,0,0,0,0,
                0,0,0x80,0xBC,0,0,0,3,0,0x79,0,0,0,0,4,
                0,0,0,0x3B,0,0,1,0x49,0,0,0x4C,0xE3,0,0,1,0x4B,
                0,0,0x0F,0x6F,0,0,1,0x4B,0,0,0x0F,0x84,0,0,5,0x20,
                0,0,0,0,0x6E,0x75,0x63,0x63,0x43,0x68,0x75,0x6E,0x6B,0x4E,0x75,0x6C,
                0x6C,0,0x6E,0x75,0x63,0x63,0x43,0x68,0x75,0x6E,0x6B,0x42,0x69,0x6E,0x61,0x72,
                0x79,0,0x6E,0x75,0x63,0x63,0x43,0x68,0x75,0x6E,0x6B,0x50,0x61,0x67,0x65,0,
                0x6E,0x75,0x63,0x63,0x43,0x68,0x75,0x6E,0x6B,0x49,0x6E,0x64,0x65,0x78,0
            };
        }

        private sealed class ByteBuilder
        {
            private byte[] _buffer;
            public int Length { get; private set; }

            public ByteBuilder(int capacity) { _buffer = new byte[Math.Max(256, capacity)]; }

            public void Write(byte[] bytes)
            {
                if (bytes == null || bytes.Length == 0) return;
                EnsureCapacity(checked(Length + bytes.Length));
                Buffer.BlockCopy(bytes, 0, _buffer, Length, bytes.Length);
                Length += bytes.Length;
            }

            public void WriteByte(byte value)
            {
                EnsureCapacity(Length + 1);
                _buffer[Length++] = value;
            }

            public void WriteZeros(int count)
            {
                if (count < 0) throw new ArgumentOutOfRangeException("count");
                EnsureCapacity(checked(Length + count));
                Array.Clear(_buffer, Length, count);
                Length += count;
            }

            public void WriteCString(string value)
            {
                Write(Encoding.UTF8.GetBytes(value ?? ""));
                WriteByte(0);
            }

            public void Align4()
            {
                while ((Length & 3) != 0) WriteByte(0);
            }

            public void WriteAt(int offset, byte[] bytes)
            {
                if (offset < 0 || bytes == null || (long)offset + bytes.Length > Length)
                    throw new ArgumentOutOfRangeException("offset");
                Buffer.BlockCopy(bytes, 0, _buffer, offset, bytes.Length);
            }

            public void WriteLE32At(int offset, int value)
            {
                EnsurePatch(offset, 4);
                _buffer[offset] = (byte)value;
                _buffer[offset + 1] = (byte)(value >> 8);
                _buffer[offset + 2] = (byte)(value >> 16);
                _buffer[offset + 3] = (byte)(value >> 24);
            }

            public void WriteBE32At(int offset, int value)
            {
                EnsurePatch(offset, 4);
                _buffer[offset] = (byte)(value >> 24);
                _buffer[offset + 1] = (byte)(value >> 16);
                _buffer[offset + 2] = (byte)(value >> 8);
                _buffer[offset + 3] = (byte)value;
            }

            public void WriteSingleAt(int offset, float value)
            {
                WriteAt(offset, BitConverter.GetBytes(value));
            }

            public void WriteBool32At(int offset, bool value)
            {
                WriteLE32At(offset, value ? 1 : 0);
            }

            public void WriteColorAt(int offset, Color color)
            {
                WriteAt(offset, new[] { color.A, color.R, color.G, color.B });
            }

            public byte[] ToArray()
            {
                byte[] result = new byte[Length];
                Buffer.BlockCopy(_buffer, 0, result, 0, Length);
                return result;
            }

            private void EnsurePatch(int offset, int count)
            {
                if (offset < 0 || count < 0 || (long)offset + count > Length)
                    throw new ArgumentOutOfRangeException("offset");
            }

            private void EnsureCapacity(int required)
            {
                if (required <= _buffer.Length) return;
                int next = _buffer.Length;
                while (next < required) next = checked(next * 2);
                Array.Resize(ref _buffer, next);
            }
        }

        #endregion

        private void Tool_StageInfoEditor_Load(object sender, EventArgs e)
        {

        }

        private void _objectsGroup_Enter(object sender, EventArgs e)
        {

        }
    }
}
