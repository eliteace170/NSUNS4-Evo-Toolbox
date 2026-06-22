using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_ParticleEditor : Form
    {
        private const string ClipboardPrefix = "NS4_PARTICLE_EDITOR_ENTRY:";

        private sealed class ReferenceComboItem
        {
            public int Index;
            public string Label = "";
            public override string ToString() { return Label; }
        }

        private sealed class ParticleClipboardPayload
        {
            public string EntryType = "";
            public string Json = "";
        }

        private sealed class ParticleNodeClipboardEntry
        {
            public ParticleManagerEntry Manager;
            public ParticleNodeEntry Node;
        }

        private readonly List<ParticleChunkState> chunks = new List<ParticleChunkState>();
        private string filePath = "";
        private bool suppressChunkSelection;
        private bool suppressReferenceSelection;
        private bool suppressIndexSelection;
        private int lastNodeIndex = -1;

        public Tool_ParticleEditor()
        {
            InitializeComponent();
            InitializeNodeGrid();
            ResetUi();
        }

        private ParticleChunkState SelectedChunk
        {
            get
            {
                int visibleIndex = chunkComboBox.SelectedIndex;
                if (visibleIndex < 0)
                    return null;

                int current = 0;
                foreach (ParticleChunkState chunk in chunks.Where(x => !x.DeletePending))
                {
                    if (current == visibleIndex)
                        return chunk;
                    current++;
                }

                return null;
            }
        }

        private int SelectedManagerIndex { get { return managerListBox.SelectedIndex; } }
        private int SelectedResourceIndex { get { return resourceListBox.SelectedIndex; } }
        private int SelectedPositionIndex { get { return positionListBox.SelectedIndex; } }
        private int SelectedForceFieldIndex { get { return forceFieldListBox.SelectedIndex; } }
        private int SelectedNodeRowIndex { get { return nodeListBox.SelectedIndex; } }

        private ParticleManagerEntry SelectedManagerEntry { get { return GetSelectedItem(SelectedChunk != null ? SelectedChunk.Managers : null, managerListBox); } }
        private ParticleResourceEntry SelectedResourceEntry { get { return GetSelectedItem(SelectedChunk != null ? SelectedChunk.Resources : null, resourceListBox); } }
        private ParticlePositionEntry SelectedPositionEntry { get { return GetSelectedItem(SelectedChunk != null ? SelectedChunk.Positions : null, positionListBox); } }
        private ParticleForceFieldEntry SelectedForceFieldEntry { get { return GetSelectedItem(SelectedChunk != null ? SelectedChunk.ForceFields : null, forceFieldListBox); } }

        private void InitializeNodeGrid()
        {
            nodeEventsGrid.AutoGenerateColumns = false;
            nodeEventsGrid.Columns.Clear();

            DataGridViewComboBoxColumn actionColumn = new DataGridViewComboBoxColumn();
            actionColumn.Name = "actionColumn";
            actionColumn.HeaderText = "State";
            actionColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            actionColumn.ValueType = typeof(string);
            actionColumn.DataSource = new[] { "On / Spawn", "Off / Despawn" };
            actionColumn.Width = 180;
            nodeEventsGrid.Columns.Add(actionColumn);

            DataGridViewTextBoxColumn frameColumn = new DataGridViewTextBoxColumn();
            frameColumn.Name = "frameColumn";
            frameColumn.HeaderText = "Frame";
            frameColumn.ToolTipText = "Frame value stored in 1/33-frame units. Decimal values are supported.";
            frameColumn.ValueType = typeof(float);
            frameColumn.DefaultCellStyle.Format = "0.###";
            frameColumn.Width = 140;
            nodeEventsGrid.Columns.Add(frameColumn);
        }

        private void ResetUi()
        {
            Text = "Particle Chunk Editor";
            suppressChunkSelection = true;
            chunkComboBox.Items.Clear();
            chunkComboBox.SelectedIndex = -1;
            suppressChunkSelection = false;

            chunkNameTextBox.Text = "";
            chunkPathTextBox.Text = "";
            managerListBox.Items.Clear();
            resourceListBox.Items.Clear();
            positionListBox.Items.Clear();
            forceFieldListBox.Items.Clear();
            nodeListBox.Items.Clear();
            managerPropertyGrid.SelectedObject = null;
            resourcePropertyGrid.SelectedObject = null;
            positionPropertyGrid.SelectedObject = null;
            forceFieldPropertyGrid.SelectedObject = null;
            ClearReferenceCombos();
            suppressIndexSelection = true;
            managerEntryIndexNumericUpDown.Value = 0;
            resourceParticleIndexNumericUpDown.Value = 0;
            positionParticleIndexNumericUpDown.Value = 0;
            forceFieldParticleIndexNumericUpDown.Value = 0;
            nodeParticleIndexNumericUpDown.Value = 0;
            suppressIndexSelection = false;
            nodeEventsGrid.Rows.Clear();
            chunkEditorPanel.Enabled = false;
            particleTabControl.Enabled = false;
            lastNodeIndex = -1;
        }

        private void ClearReferenceCombos()
        {
            suppressReferenceSelection = true;
            managerAnimationComboBox.Items.Clear();
            resourceEffectComboBox.Items.Clear();
            positionCoordComboBox.Items.Clear();
            positionClumpComboBox.Items.Clear();
            forceFieldCoordComboBox.Items.Clear();
            forceFieldClumpComboBox.Items.Clear();
            managerAnimationComboBox.SelectedIndex = -1;
            resourceEffectComboBox.SelectedIndex = -1;
            positionCoordComboBox.SelectedIndex = -1;
            positionClumpComboBox.SelectedIndex = -1;
            forceFieldCoordComboBox.SelectedIndex = -1;
            forceFieldClumpComboBox.SelectedIndex = -1;
            suppressReferenceSelection = false;
        }

        private void LoadFile(string path)
        {
            List<ParticleChunkState> parsedChunks = new List<ParticleChunkState>();
            using (XfbinParserBackend backend = new XfbinParserBackend(path))
            {
                foreach (XfbinBinaryChunkPage page in backend.GetChunkPages(ParticleChunkCodec.ChunkType).OrderBy(x => x.Index))
                {
                    ParticleChunkState chunk;
                    XfbinBinaryChunkItem item = new XfbinBinaryChunkItem
                    {
                        BinaryData = page.BinaryData,
                        ChunkName = page.ChunkName,
                        ChunkPath = page.ChunkPath,
                        ChunkType = page.ChunkType,
                        Version = page.Version,
                        VersionAttribute = page.VersionAttribute
                    };

                    if (!ParticleChunkCodec.TryParseChunk(item, out chunk))
                        continue;

                    chunk.References.AddRange(ParticleChunkCodec.ExtractReferenceEntries(page.Definition, page.ChunkName, page.ChunkType));
                    parsedChunks.Add(chunk);
                }
            }

            if (parsedChunks.Count == 0)
            {
                MessageBox.Show("No nuccChunkParticle chunk was found in this XFBIN.");
                return;
            }

            filePath = path;
            Text = "Particle Chunk Editor - " + Path.GetFileName(path);
            chunks.Clear();
            chunks.AddRange(parsedChunks);
            RefreshChunkList(0);
        }

        private void RefreshChunkList(int selectedIndex)
        {
            suppressChunkSelection = true;
            chunkComboBox.Items.Clear();
            foreach (ParticleChunkState chunk in chunks.Where(x => !x.DeletePending))
                chunkComboBox.Items.Add(ParticleChunkCodec.BuildChunkLabel(chunk));
            chunkComboBox.SelectedIndex = chunkComboBox.Items.Count == 0 ? -1 : Math.Max(0, Math.Min(selectedIndex, chunkComboBox.Items.Count - 1));
            suppressChunkSelection = false;

            if (chunkComboBox.SelectedIndex >= 0)
                LoadSelectedChunk();
            else
                ResetUi();
        }

        private void LoadSelectedChunk()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
            {
                ResetUi();
                return;
            }

            chunkEditorPanel.Enabled = true;
            particleTabControl.Enabled = true;
            chunkNameTextBox.Text = chunk.ChunkName;
            chunkPathTextBox.Text = chunk.ChunkPath;
            EnsureNodeLinks(chunk);
            RefreshSectionLists();
            LoadSelectedObjects();
        }

        private void RefreshSectionLists()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;

            RefreshSectionList(managerListBox, chunk.Managers, x => BuildManagerLabel(chunk, x));
            RefreshSectionList(resourceListBox, chunk.Resources, x => BuildParticleLinkLabel(chunk, x.ParticleEntryIndex) + " | " + ParticleChunkCodec.ResolveReferenceLabel(chunk, x.EffectChunkIndex));
            RefreshSectionList(positionListBox, chunk.Positions, x => BuildParticleLinkLabel(chunk, x.ParticleEntryIndex) + " | Coord: " + ParticleChunkCodec.ResolveReferenceLabel(chunk, x.CoordChunkIndex) + " | Clump: " + ParticleChunkCodec.ResolveReferenceLabel(chunk, x.ClumpChunkIndex));
            RefreshSectionList(forceFieldListBox, chunk.ForceFields, x => BuildParticleLinkLabel(chunk, x.ParticleEntryIndex) + " | Coord: " + ParticleChunkCodec.ResolveReferenceLabel(chunk, x.CoordChunkIndex) + " | Clump: " + ParticleChunkCodec.ResolveReferenceLabel(chunk, x.ClumpChunkIndex));
            RefreshSectionList(nodeListBox, chunk.Nodes.Select((node, index) => new { node, index }).ToList(), x => BuildNodeLabel(chunk, x.index, x.node));
        }

        private static string BuildManagerLabel(ParticleChunkState chunk, ParticleManagerEntry entry)
        {
            int slot = GetManagerSlotByEntryIndex(chunk, entry.EntryIndex);
            string slotLabel = slot >= 0 ? "Particle Setting " + slot : "Particle Setting ?";
            return slotLabel + " | " + ParticleChunkCodec.ResolveReferenceLabel(chunk, entry.AnimationChunkIndex);
        }

        private static string BuildParticleLinkLabel(ParticleChunkState chunk, uint entryIndex)
        {
            int slot = GetManagerSlotByEntryIndex(chunk, entryIndex);
            return slot >= 0 ? "Particle Setting " + slot : "Unlinked Setting (" + entryIndex + ")";
        }

        private static int GetManagerSlotByEntryIndex(ParticleChunkState chunk, uint entryIndex)
        {
            if (chunk == null)
                return -1;

            for (int i = 0; i < chunk.Managers.Count; i++)
            {
                if (chunk.Managers[i].EntryIndex == entryIndex)
                    return i;
            }

            return -1;
        }

        private static string BuildNodeLabel(ParticleChunkState chunk, int nodeIndex, ParticleNodeEntry node)
        {
            List<ParticleNodeEvent> events = ParticleChunkCodec.DecodeNodeEvents(node);
            string linkLabel = nodeIndex >= 0 && nodeIndex < chunk.Managers.Count
                ? "Particle Setting " + nodeIndex
                : "Unlinked Setting";
            if (events.Count == 0)
                return linkLabel + " | No timing";

            return linkLabel + " | " + string.Join(", ", events.Select(x =>
                string.Format(CultureInfo.InvariantCulture, "{0} @ {1:0.###}", x.Action == ParticleNodeAction.On ? "Spawn" : "Despawn", x.Frame)).ToArray());
        }

        private static void EnsureNodeLinks(ParticleChunkState chunk)
        {
            if (chunk == null)
                return;

            while (chunk.Nodes.Count < chunk.Managers.Count)
                chunk.Nodes.Add(new ParticleNodeEntry());

            while (chunk.Nodes.Count > chunk.Managers.Count && chunk.Managers.Count >= 0)
                chunk.Nodes.RemoveAt(chunk.Nodes.Count - 1);
        }

        private static void RefreshSectionList<T>(ListBox listBox, List<T> items, Func<T, string> labelBuilder)
        {
            int selectedIndex = listBox.SelectedIndex;
            listBox.Items.Clear();
            if (items.Count == 0)
            {
                listBox.Items.Add("No entries found...");
                listBox.SelectedIndex = -1;
                return;
            }

            foreach (T item in items)
                listBox.Items.Add(labelBuilder(item));
            listBox.SelectedIndex = Math.Max(0, Math.Min(selectedIndex, items.Count - 1));
        }

        private void LoadSelectedObjects()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;

            managerPropertyGrid.SelectedObject = SelectedManagerEntry;
            resourcePropertyGrid.SelectedObject = SelectedResourceEntry;
            positionPropertyGrid.SelectedObject = SelectedPositionEntry;
            forceFieldPropertyGrid.SelectedObject = SelectedForceFieldEntry;
            PopulateReferenceCombos();
            PopulateIndexControls();
            LoadSelectedNode();
        }

        private void PopulateIndexControls()
        {
            ParticleChunkState chunk = SelectedChunk;
            suppressIndexSelection = true;
            try
            {
                SetIndexNumeric(managerEntryIndexNumericUpDown, chunk != null && managerListBox.SelectedIndex >= 0 && managerListBox.SelectedIndex < chunk.Managers.Count
                    ? chunk.Managers[managerListBox.SelectedIndex].EntryIndex
                    : 0u);
                SetIndexNumeric(resourceParticleIndexNumericUpDown, chunk != null && resourceListBox.SelectedIndex >= 0 && resourceListBox.SelectedIndex < chunk.Resources.Count
                    ? chunk.Resources[resourceListBox.SelectedIndex].ParticleEntryIndex
                    : 0u);
                SetIndexNumeric(positionParticleIndexNumericUpDown, chunk != null && positionListBox.SelectedIndex >= 0 && positionListBox.SelectedIndex < chunk.Positions.Count
                    ? chunk.Positions[positionListBox.SelectedIndex].ParticleEntryIndex
                    : 0u);
                SetIndexNumeric(forceFieldParticleIndexNumericUpDown, chunk != null && forceFieldListBox.SelectedIndex >= 0 && forceFieldListBox.SelectedIndex < chunk.ForceFields.Count
                    ? chunk.ForceFields[forceFieldListBox.SelectedIndex].ParticleEntryIndex
                    : 0u);
                SetIndexNumeric(nodeParticleIndexNumericUpDown, chunk != null && nodeListBox.SelectedIndex >= 0 && nodeListBox.SelectedIndex < chunk.Managers.Count
                    ? chunk.Managers[nodeListBox.SelectedIndex].EntryIndex
                    : 0u);
            }
            finally
            {
                suppressIndexSelection = false;
            }
        }

        private static void SetIndexNumeric(NumericUpDown control, uint value)
        {
            decimal decimalValue = value;
            if (decimalValue < control.Minimum)
                decimalValue = control.Minimum;
            if (decimalValue > control.Maximum)
                decimalValue = control.Maximum;
            control.Value = decimalValue;
        }

        private static T GetSelectedItem<T>(List<T> list, ListBox listBox) where T : class
        {
            if (list == null)
                return null;
            int index = listBox.SelectedIndex;
            return index >= 0 && index < list.Count ? list[index] : null;
        }

        private void PopulateReferenceCombos()
        {
            ParticleChunkState chunk = SelectedChunk;
            suppressReferenceSelection = true;
            PopulateReferenceCombo(managerAnimationComboBox, chunk, "nuccChunkAnm", chunk.Managers.Count > 0 && managerListBox.SelectedIndex >= 0 ? (int)chunk.Managers[managerListBox.SelectedIndex].AnimationChunkIndex : 0, false);
            PopulateReferenceCombo(resourceEffectComboBox, chunk, null, chunk.Resources.Count > 0 && resourceListBox.SelectedIndex >= 0 ? (int)chunk.Resources[resourceListBox.SelectedIndex].EffectChunkIndex : 0, false);
            PopulateReferenceCombo(positionCoordComboBox, chunk, "nuccChunkCoord", chunk.Positions.Count > 0 && positionListBox.SelectedIndex >= 0 ? chunk.Positions[positionListBox.SelectedIndex].CoordChunkIndex : -1, true);
            PopulateReferenceCombo(positionClumpComboBox, chunk, "nuccChunkClump", chunk.Positions.Count > 0 && positionListBox.SelectedIndex >= 0 ? chunk.Positions[positionListBox.SelectedIndex].ClumpChunkIndex : -1, true);
            PopulateReferenceCombo(forceFieldCoordComboBox, chunk, "nuccChunkCoord", chunk.ForceFields.Count > 0 && forceFieldListBox.SelectedIndex >= 0 ? chunk.ForceFields[forceFieldListBox.SelectedIndex].CoordChunkIndex : -1, true);
            PopulateReferenceCombo(forceFieldClumpComboBox, chunk, "nuccChunkClump", chunk.ForceFields.Count > 0 && forceFieldListBox.SelectedIndex >= 0 ? chunk.ForceFields[forceFieldListBox.SelectedIndex].ClumpChunkIndex : -1, true);
            suppressReferenceSelection = false;
        }

        private static void PopulateReferenceCombo(ComboBox comboBox, ParticleChunkState chunk, string chunkTypeFilter, int selectedIndex, bool allowNone)
        {
            comboBox.Items.Clear();
            if (chunk == null)
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            if (allowNone)
                comboBox.Items.Add(new ReferenceComboItem { Index = -1, Label = "(none / -1)" });

            foreach (var pair in chunk.References.Select((entry, index) => new { entry, index }))
            {
                if (!string.IsNullOrWhiteSpace(chunkTypeFilter) && !string.Equals(pair.entry.Type, chunkTypeFilter, StringComparison.OrdinalIgnoreCase))
                    continue;

                comboBox.Items.Add(new ReferenceComboItem
                {
                    Index = pair.index,
                    Label = pair.entry.ToString()
                });
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                ReferenceComboItem item = comboBox.Items[i] as ReferenceComboItem;
                if (item != null && item.Index == selectedIndex)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1;
        }

        private void LoadSelectedNode()
        {
            ParticleChunkState chunk = SelectedChunk;
            nodeEventsGrid.Rows.Clear();
            if (chunk != null && nodeListBox.SelectedIndex >= 0 && nodeListBox.SelectedIndex < chunk.Nodes.Count)
            {
                foreach (ParticleNodeEvent particleEvent in ParticleChunkCodec.DecodeNodeEvents(chunk.Nodes[nodeListBox.SelectedIndex]))
                    nodeEventsGrid.Rows.Add(GetNodeActionLabel(particleEvent.Action), particleEvent.Frame);
            }
            PopulateIndexControls();
            lastNodeIndex = nodeListBox.SelectedIndex;
        }

        private void ApplyNodeGrid()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;

            int targetIndex = nodeListBox.SelectedIndex >= 0 ? nodeListBox.SelectedIndex : lastNodeIndex;
            ApplyNodeGrid(targetIndex);
        }

        private void ApplyNodeGrid(int targetIndex)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;
            if (targetIndex < 0 || targetIndex >= chunk.Nodes.Count)
                return;

            if (nodeEventsGrid.IsCurrentCellInEditMode)
                nodeEventsGrid.EndEdit();
            nodeEventsGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);

            List<ParticleNodeEvent> events = new List<ParticleNodeEvent>();
            foreach (DataGridViewRow row in nodeEventsGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;
                object actionValue = row.Cells[0].Value;
                object frameValue = row.Cells[1].Value;
                if (actionValue == null || frameValue == null)
                    continue;

                float frame;
                if (!float.TryParse(frameValue.ToString(), NumberStyles.Float, CultureInfo.CurrentCulture, out frame) &&
                    !float.TryParse(frameValue.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out frame))
                    throw new InvalidOperationException("Node frame values must be numeric.");
                if (float.IsNaN(frame) || float.IsInfinity(frame) || frame < 0f || frame > ushort.MaxValue / 33f)
                    throw new InvalidOperationException("Node frame values must be between 0 and 1985.909.");

                ParticleNodeAction action;
                if (actionValue is ParticleNodeAction)
                    action = (ParticleNodeAction)actionValue;
                else
                    action = ParseNodeActionLabel(actionValue.ToString());

                events.Add(new ParticleNodeEvent { Action = action, Frame = frame });
            }

            chunk.Nodes[targetIndex] = ParticleChunkCodec.EncodeNodeEvents(events);
        }

        private void ApplyChunkMetadata()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;
            if (string.IsNullOrWhiteSpace(chunkNameTextBox.Text) || string.IsNullOrWhiteSpace(chunkPathTextBox.Text))
                throw new InvalidOperationException("Chunk name and chunk path are required.");
            chunk.ChunkName = chunkNameTextBox.Text.Trim();
            chunk.ChunkPath = chunkPathTextBox.Text.Trim();
        }

        private void SaveFile(bool saveAs)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                MessageBox.Show("No file loaded...");
                return;
            }

            ApplyChunkMetadata();
            EnsureNodeLinks(SelectedChunk);
            ApplyNodeGrid();

            string outputPath = filePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = ".xfbin";
                    dialog.Filter = "XFBIN Files (*.xfbin)|*.xfbin";
                    dialog.FileName = Path.GetFileName(filePath);
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    outputPath = dialog.FileName;
                }
            }

            using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
            {
                foreach (ParticleChunkState chunk in chunks.Where(x => x.DeletePending && !string.IsNullOrWhiteSpace(x.OriginalChunkName)))
                    backend.DeleteChunk(chunk.OriginalChunkName, ParticleChunkCodec.ChunkType);

                foreach (ParticleChunkState chunk in chunks.Where(x => !x.DeletePending))
                {
                    backend.UpsertChunk(chunk.OriginalChunkName, chunk.ChunkName, ParticleChunkCodec.ChunkType, chunk.ChunkPath, ParticleChunkCodec.FileExtension, ParticleChunkCodec.BuildChunkData(chunk), chunk.Version, chunk.VersionAttribute);
                    backend.SetChunkPageChunkMaps(chunk.ChunkName, ParticleChunkCodec.ChunkType, ParticleChunkCodec.BuildReferenceChunkMaps(chunk.References));
                }

                backend.RepackTo(outputPath);
            }

            filePath = outputPath;
            Text = "Particle Chunk Editor - " + Path.GetFileName(outputPath);
            foreach (ParticleChunkState chunk in chunks.Where(x => !x.DeletePending))
                chunk.OriginalChunkName = chunk.ChunkName;
            chunks.RemoveAll(x => x.DeletePending);
            RefreshChunkList(chunkComboBox.SelectedIndex);
        }

        private void AddChunk()
        {
            ParticleChunkState source = SelectedChunk;
            ParticleChunkState chunk = source != null ? ParticleChunkCodec.CloneChunk(source) : ParticleChunkCodec.CreateDefaultChunk();
            chunk.OriginalChunkName = "";
            chunk.DeletePending = false;
            chunk.ChunkName = BuildUniqueChunkName(chunk.ChunkName + "_copy");
            chunks.Add(chunk);
            RefreshChunkList(chunks.Count - 1);
        }

        private string BuildUniqueChunkName(string baseName)
        {
            string name = baseName;
            int suffix = 1;
            while (chunks.Any(x => !x.DeletePending && string.Equals(x.ChunkName, name, StringComparison.OrdinalIgnoreCase)))
            {
                name = baseName + "_" + suffix;
                suffix++;
            }

            return name;
        }

        private void DeleteChunk()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;
            chunk.DeletePending = true;
            RefreshChunkList(Math.Max(0, chunkComboBox.SelectedIndex - 1));
        }

        private void EditReferences()
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk == null)
                return;

            using (Tool_ParticleChunkReferenceEditor editor = new Tool_ParticleChunkReferenceEditor(chunk.References))
            {
                if (editor.ShowDialog(this) != DialogResult.OK)
                    return;

                RemapReferenceIndexes(chunk, editor);
                chunk.References.Clear();
                chunk.References.AddRange(editor.BuildResult());
                RefreshSectionLists();
                LoadSelectedObjects();
            }
        }

        private static void RemapReferenceIndexes(ParticleChunkState chunk, Tool_ParticleChunkReferenceEditor editor)
        {
            Dictionary<int, int> map = editor.BuildIndexMap();
            Func<uint, uint> remap = oldIndex =>
            {
                int newIndex;
                return map.TryGetValue((int)oldIndex, out newIndex) ? (uint)newIndex : 0u;
            };
            Func<int, int> remapSigned = oldIndex =>
            {
                if (oldIndex < 0)
                    return oldIndex;
                int newIndex;
                return map.TryGetValue(oldIndex, out newIndex) ? newIndex : 0;
            };

            foreach (ParticleManagerEntry entry in chunk.Managers)
                entry.AnimationChunkIndex = remap(entry.AnimationChunkIndex);
            foreach (ParticleResourceEntry entry in chunk.Resources)
                entry.EffectChunkIndex = remap(entry.EffectChunkIndex);
            foreach (ParticlePositionEntry entry in chunk.Positions)
            {
                entry.CoordChunkIndex = remapSigned(entry.CoordChunkIndex);
                entry.ClumpChunkIndex = remapSigned(entry.ClumpChunkIndex);
            }
            foreach (ParticleForceFieldEntry entry in chunk.ForceFields)
            {
                entry.CoordChunkIndex = remapSigned(entry.CoordChunkIndex);
                entry.ClumpChunkIndex = remapSigned(entry.ClumpChunkIndex);
            }
        }

        private void ApplySelectedReferenceCombo(ComboBox comboBox, Action<uint> apply)
        {
            if (suppressReferenceSelection)
                return;
            ReferenceComboItem item = comboBox.SelectedItem as ReferenceComboItem;
            if (item == null)
                return;
            apply((uint)item.Index);
            RefreshSectionLists();
        }

        private void ApplySelectedSignedReferenceCombo(ComboBox comboBox, Action<int> apply)
        {
            if (suppressReferenceSelection)
                return;
            ReferenceComboItem item = comboBox.SelectedItem as ReferenceComboItem;
            if (item == null)
                return;
            apply(item.Index);
            RefreshSectionLists();
        }

        private void ApplySelectedIndex(NumericUpDown numericUpDown, Action<uint> apply)
        {
            if (suppressIndexSelection)
                return;

            apply((uint)numericUpDown.Value);
            RefreshSectionLists();
            LoadSelectedObjects();
        }

        private void RefreshSelectedSection(bool reloadObjects)
        {
            RefreshSectionLists();
            if (reloadObjects)
                LoadSelectedObjects();
        }

        private void AddManagerEntry(ParticleManagerEntry template)
        {
            if (SelectedChunk == null)
                return;

            ParticleManagerEntry newEntry = template != null ? ParticleChunkCodec.CloneManager(template) : new ParticleManagerEntry();
            newEntry.EntryIndex = (uint)SelectedChunk.Managers.Count;
            SelectedChunk.Managers.Add(newEntry);
            EnsureNodeLinks(SelectedChunk);
            RefreshSelectedSection(false);
        }

        private void DeleteManagerEntry()
        {
            if (SelectedChunk == null || SelectedManagerIndex < 0 || SelectedManagerIndex >= SelectedChunk.Managers.Count)
                return;

            int removedIndex = SelectedManagerIndex;
            uint removedEntryIndex = SelectedChunk.Managers[removedIndex].EntryIndex;
            SelectedChunk.Managers.RemoveAt(removedIndex);
            if (removedIndex >= 0 && removedIndex < SelectedChunk.Nodes.Count)
                SelectedChunk.Nodes.RemoveAt(removedIndex);

            foreach (ParticleResourceEntry entry in SelectedChunk.Resources.Where(x => x.ParticleEntryIndex == removedEntryIndex))
                entry.ParticleEntryIndex = 0;
            foreach (ParticlePositionEntry entry in SelectedChunk.Positions.Where(x => x.ParticleEntryIndex == removedEntryIndex))
                entry.ParticleEntryIndex = 0;
            foreach (ParticleForceFieldEntry entry in SelectedChunk.ForceFields.Where(x => x.ParticleEntryIndex == removedEntryIndex))
                entry.ParticleEntryIndex = 0;

            RefreshSelectedSection(true);
        }

        private void AddNodeEntry(bool duplicateCurrent)
        {
            if (SelectedChunk == null)
                return;

            ParticleManagerEntry manager = duplicateCurrent && SelectedNodeRowIndex >= 0 && SelectedNodeRowIndex < SelectedChunk.Managers.Count
                ? ParticleChunkCodec.CloneManager(SelectedChunk.Managers[SelectedNodeRowIndex])
                : new ParticleManagerEntry();
            manager.EntryIndex = (uint)SelectedChunk.Managers.Count;
            SelectedChunk.Managers.Add(manager);

            ParticleNodeEntry node = duplicateCurrent && SelectedNodeRowIndex >= 0 && SelectedNodeRowIndex < SelectedChunk.Nodes.Count
                ? ParticleChunkCodec.CloneNode(SelectedChunk.Nodes[SelectedNodeRowIndex])
                : new ParticleNodeEntry();
            SelectedChunk.Nodes.Add(node);
            RefreshSelectedSection(false);
        }

        private void DeleteNodeEntry()
        {
            if (SelectedChunk == null || SelectedNodeRowIndex < 0 || SelectedNodeRowIndex >= SelectedChunk.Nodes.Count)
                return;

            if (SelectedNodeRowIndex < SelectedChunk.Managers.Count)
                SelectedChunk.Managers.RemoveAt(SelectedNodeRowIndex);
            SelectedChunk.Nodes.RemoveAt(SelectedNodeRowIndex);
            RefreshSelectedSection(false);
            LoadSelectedNode();
        }

        private void AddListEntry<T>(List<T> list, T template, Func<T, T> clone) where T : class, new()
        {
            if (SelectedChunk == null || list == null)
                return;

            list.Add(template != null ? clone(template) : new T());
            RefreshSelectedSection(false);
        }

        private void DeleteListEntry<T>(List<T> list, int selectedIndex) where T : class
        {
            if (SelectedChunk == null || list == null || selectedIndex < 0 || selectedIndex >= list.Count)
                return;

            list.RemoveAt(selectedIndex);
            RefreshSelectedSection(true);
        }

        private void CopyClipboardEntry<T>(T entry, string entryType) where T : class
        {
            if (entry == null)
                return;

            ParticleClipboardPayload payload = new ParticleClipboardPayload
            {
                EntryType = entryType,
                Json = JsonConvert.SerializeObject(entry)
            };

            Clipboard.SetText(ClipboardPrefix + JsonConvert.SerializeObject(payload));
        }

        private bool TryReadClipboardEntry<T>(string entryType, out T entry) where T : class
        {
            entry = null;
            if (!Clipboard.ContainsText())
                return false;

            string text = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(text) || !text.StartsWith(ClipboardPrefix, StringComparison.Ordinal))
                return false;

            ParticleClipboardPayload payload = JsonConvert.DeserializeObject<ParticleClipboardPayload>(text.Substring(ClipboardPrefix.Length));
            if (payload == null || !string.Equals(payload.EntryType, entryType, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(payload.Json))
                return false;

            entry = JsonConvert.DeserializeObject<T>(payload.Json);
            return entry != null;
        }

        private void CopyManagerEntry() { CopyClipboardEntry(SelectedManagerEntry, "manager"); }

        private void PasteManagerEntry()
        {
            ParticleManagerEntry entry;
            if (!TryReadClipboardEntry("manager", out entry) || SelectedChunk == null)
                return;

            AddManagerEntry(entry);
        }

        private void CopyResourceEntry() { CopyClipboardEntry(SelectedResourceEntry, "resource"); }

        private void PasteResourceEntry()
        {
            ParticleResourceEntry entry;
            if (!TryReadClipboardEntry("resource", out entry) || SelectedChunk == null)
                return;

            AddListEntry(SelectedChunk.Resources, entry, ParticleChunkCodec.CloneResource);
        }

        private void CopyPositionEntry() { CopyClipboardEntry(SelectedPositionEntry, "position"); }

        private void PastePositionEntry()
        {
            ParticlePositionEntry entry;
            if (!TryReadClipboardEntry("position", out entry) || SelectedChunk == null)
                return;

            AddListEntry(SelectedChunk.Positions, entry, ParticleChunkCodec.ClonePosition);
        }

        private void CopyForceFieldEntry() { CopyClipboardEntry(SelectedForceFieldEntry, "forcefield"); }

        private void PasteForceFieldEntry()
        {
            ParticleForceFieldEntry entry;
            if (!TryReadClipboardEntry("forcefield", out entry) || SelectedChunk == null)
                return;

            AddListEntry(SelectedChunk.ForceFields, entry, ParticleChunkCodec.CloneForceField);
        }

        private void CopyNodeEntry()
        {
            if (SelectedChunk == null || SelectedNodeRowIndex < 0 || SelectedNodeRowIndex >= SelectedChunk.Nodes.Count || SelectedNodeRowIndex >= SelectedChunk.Managers.Count)
                return;

            CopyClipboardEntry(new ParticleNodeClipboardEntry
            {
                Manager = ParticleChunkCodec.CloneManager(SelectedChunk.Managers[SelectedNodeRowIndex]),
                Node = ParticleChunkCodec.CloneNode(SelectedChunk.Nodes[SelectedNodeRowIndex])
            }, "nodebundle");
        }

        private void PasteNodeEntry()
        {
            ParticleNodeClipboardEntry entry;
            if (!TryReadClipboardEntry("nodebundle", out entry) || SelectedChunk == null || entry == null)
                return;

            ParticleManagerEntry manager = entry.Manager != null ? ParticleChunkCodec.CloneManager(entry.Manager) : new ParticleManagerEntry();
            manager.EntryIndex = (uint)SelectedChunk.Managers.Count;
            SelectedChunk.Managers.Add(manager);
            SelectedChunk.Nodes.Add(entry.Node != null ? ParticleChunkCodec.CloneNode(entry.Node) : new ParticleNodeEntry());
            RefreshSelectedSection(false);
        }

        private static string GetNodeActionLabel(ParticleNodeAction action)
        {
            return action == ParticleNodeAction.On ? "On / Spawn" : "Off / Despawn";
        }

        private static ParticleNodeAction ParseNodeActionLabel(string value)
        {
            return string.Equals(value, "Off / Despawn", StringComparison.OrdinalIgnoreCase)
                ? ParticleNodeAction.Off
                : ParticleNodeAction.On;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = ".xfbin";
                dialog.Filter = "XFBIN Files (*.xfbin)|*.xfbin";
                if (dialog.ShowDialog() == DialogResult.OK)
                    LoadFile(dialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { SaveFile(false); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) { SaveFile(true); }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) { chunks.Clear(); filePath = ""; ResetUi(); }
        private void chunkComboBox_SelectedIndexChanged(object sender, EventArgs e) { if (!suppressChunkSelection) LoadSelectedChunk(); }
        private void saveChunkMetaButton_Click(object sender, EventArgs e) { ApplyChunkMetadata(); RefreshChunkList(chunkComboBox.SelectedIndex); }
        private void addChunkButton_Click(object sender, EventArgs e) { AddChunk(); }
        private void deleteChunkButton_Click(object sender, EventArgs e) { DeleteChunk(); }
        private void editReferencesButton_Click(object sender, EventArgs e) { EditReferences(); }
        private void managerListBox_SelectedIndexChanged(object sender, EventArgs e) { LoadSelectedObjects(); }
        private void resourceListBox_SelectedIndexChanged(object sender, EventArgs e) { LoadSelectedObjects(); }
        private void positionListBox_SelectedIndexChanged(object sender, EventArgs e) { LoadSelectedObjects(); }
        private void forceFieldListBox_SelectedIndexChanged(object sender, EventArgs e) { LoadSelectedObjects(); }
        private void nodeListBox_SelectedIndexChanged(object sender, EventArgs e) { if (lastNodeIndex >= 0 && lastNodeIndex != nodeListBox.SelectedIndex) ApplyNodeGrid(lastNodeIndex); LoadSelectedNode(); }

        private void managerAnimationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedManagerEntry != null)
                ApplySelectedReferenceCombo(managerAnimationComboBox, v => SelectedManagerEntry.AnimationChunkIndex = v);
        }

        private void managerEntryIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedManagerEntry != null)
                ApplySelectedIndex(managerEntryIndexNumericUpDown, v => SelectedManagerEntry.EntryIndex = v);
        }

        private void resourceEffectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedResourceEntry != null)
            {
                ApplySelectedReferenceCombo(resourceEffectComboBox, v =>
                {
                    SelectedResourceEntry.EffectChunkIndex = v;
                    if (chunk != null && v < chunk.References.Count)
                    {
                        ParticleEffectChunkType type;
                        if (Enum.TryParse(chunk.References[(int)v].Type, true, out type))
                            SelectedResourceEntry.EffectChunkType = type;
                    }
                });
            }
        }

        private void resourceParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedResourceEntry != null)
                ApplySelectedIndex(resourceParticleIndexNumericUpDown, v => SelectedResourceEntry.ParticleEntryIndex = v);
        }

        private void positionCoordComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedPositionEntry != null)
                ApplySelectedSignedReferenceCombo(positionCoordComboBox, v => SelectedPositionEntry.CoordChunkIndex = v);
        }

        private void positionParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedPositionEntry != null)
                ApplySelectedIndex(positionParticleIndexNumericUpDown, v => SelectedPositionEntry.ParticleEntryIndex = v);
        }

        private void positionClumpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedPositionEntry != null)
                ApplySelectedSignedReferenceCombo(positionClumpComboBox, v => SelectedPositionEntry.ClumpChunkIndex = v);
        }

        private void forceFieldCoordComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedForceFieldEntry != null)
                ApplySelectedSignedReferenceCombo(forceFieldCoordComboBox, v => SelectedForceFieldEntry.CoordChunkIndex = v);
        }

        private void forceFieldParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedForceFieldEntry != null)
                ApplySelectedIndex(forceFieldParticleIndexNumericUpDown, v => SelectedForceFieldEntry.ParticleEntryIndex = v);
        }

        private void forceFieldClumpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (SelectedForceFieldEntry != null)
                ApplySelectedSignedReferenceCombo(forceFieldClumpComboBox, v => SelectedForceFieldEntry.ClumpChunkIndex = v);
        }

        private void nodeParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && SelectedNodeRowIndex >= 0 && SelectedNodeRowIndex < chunk.Managers.Count)
                ApplySelectedIndex(nodeParticleIndexNumericUpDown, v => chunk.Managers[SelectedNodeRowIndex].EntryIndex = v);
        }

        private void managerAddButton_Click(object sender, EventArgs e) { AddManagerEntry(null); }
        private void managerCopyButton_Click(object sender, EventArgs e) { CopyManagerEntry(); }
        private void managerPasteButton_Click(object sender, EventArgs e) { PasteManagerEntry(); }
        private void managerDuplicateButton_Click(object sender, EventArgs e) { AddManagerEntry(SelectedManagerEntry); }
        private void managerDeleteButton_Click(object sender, EventArgs e) { DeleteManagerEntry(); }
        private void managerSaveButton_Click(object sender, EventArgs e) { managerPropertyGrid.Refresh(); RefreshSelectedSection(false); }
        private void resourceAddButton_Click(object sender, EventArgs e) { AddListEntry(SelectedChunk != null ? SelectedChunk.Resources : null, null, ParticleChunkCodec.CloneResource); }
        private void resourceCopyButton_Click(object sender, EventArgs e) { CopyResourceEntry(); }
        private void resourcePasteButton_Click(object sender, EventArgs e) { PasteResourceEntry(); }
        private void resourceDuplicateButton_Click(object sender, EventArgs e) { AddListEntry(SelectedChunk != null ? SelectedChunk.Resources : null, SelectedResourceEntry, ParticleChunkCodec.CloneResource); }
        private void resourceDeleteButton_Click(object sender, EventArgs e) { DeleteListEntry(SelectedChunk != null ? SelectedChunk.Resources : null, SelectedResourceIndex); }
        private void resourceSaveButton_Click(object sender, EventArgs e) { resourcePropertyGrid.Refresh(); RefreshSelectedSection(false); }
        private void positionAddButton_Click(object sender, EventArgs e) { AddListEntry(SelectedChunk != null ? SelectedChunk.Positions : null, null, ParticleChunkCodec.ClonePosition); }
        private void positionCopyButton_Click(object sender, EventArgs e) { CopyPositionEntry(); }
        private void positionPasteButton_Click(object sender, EventArgs e) { PastePositionEntry(); }
        private void positionDuplicateButton_Click(object sender, EventArgs e) { AddListEntry(SelectedChunk != null ? SelectedChunk.Positions : null, SelectedPositionEntry, ParticleChunkCodec.ClonePosition); }
        private void positionDeleteButton_Click(object sender, EventArgs e) { DeleteListEntry(SelectedChunk != null ? SelectedChunk.Positions : null, SelectedPositionIndex); }
        private void positionSaveButton_Click(object sender, EventArgs e) { positionPropertyGrid.Refresh(); RefreshSelectedSection(false); }
        private void forceFieldAddButton_Click(object sender, EventArgs e) { AddListEntry(SelectedChunk != null ? SelectedChunk.ForceFields : null, null, ParticleChunkCodec.CloneForceField); }
        private void forceFieldCopyButton_Click(object sender, EventArgs e) { CopyForceFieldEntry(); }
        private void forceFieldPasteButton_Click(object sender, EventArgs e) { PasteForceFieldEntry(); }
        private void forceFieldDuplicateButton_Click(object sender, EventArgs e) { AddListEntry(SelectedChunk != null ? SelectedChunk.ForceFields : null, SelectedForceFieldEntry, ParticleChunkCodec.CloneForceField); }
        private void forceFieldDeleteButton_Click(object sender, EventArgs e) { DeleteListEntry(SelectedChunk != null ? SelectedChunk.ForceFields : null, SelectedForceFieldIndex); }
        private void forceFieldSaveButton_Click(object sender, EventArgs e) { forceFieldPropertyGrid.Refresh(); RefreshSelectedSection(false); }
        private void nodeAddButton_Click(object sender, EventArgs e) { AddNodeEntry(false); }
        private void nodeCopyButton_Click(object sender, EventArgs e) { CopyNodeEntry(); }
        private void nodePasteButton_Click(object sender, EventArgs e) { PasteNodeEntry(); }
        private void nodeDuplicateButton_Click(object sender, EventArgs e) { AddNodeEntry(true); }
        private void nodeDeleteButton_Click(object sender, EventArgs e) { DeleteNodeEntry(); }
        private void nodeSaveButton_Click(object sender, EventArgs e) { ApplyNodeGrid(); RefreshSelectedSection(false); }
        private void nodeAddEventButton_Click(object sender, EventArgs e) { nodeEventsGrid.Rows.Add(GetNodeActionLabel(ParticleNodeAction.On), 0); }
        private void nodeDeleteEventButton_Click(object sender, EventArgs e) { foreach (DataGridViewRow row in nodeEventsGrid.SelectedRows) if (!row.IsNewRow) nodeEventsGrid.Rows.Remove(row); }
    }
}
