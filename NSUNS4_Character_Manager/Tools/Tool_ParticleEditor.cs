using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_ParticleEditor : Form
    {
        private sealed class ReferenceComboItem
        {
            public int Index;
            public string Label = "";
            public override string ToString() { return Label; }
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
            frameColumn.HeaderText = "Time";
            frameColumn.Width = 140;
            nodeEventsGrid.Columns.Add(frameColumn);
        }

        private void ResetUi()
        {
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
                string.Format("{0} @ {1}", x.Action == ParticleNodeAction.On ? "Spawn" : "Despawn", x.Frame)).ToArray());
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

            managerPropertyGrid.SelectedObject = GetSelectedItem(chunk.Managers, managerListBox);
            resourcePropertyGrid.SelectedObject = GetSelectedItem(chunk.Resources, resourceListBox);
            positionPropertyGrid.SelectedObject = GetSelectedItem(chunk.Positions, positionListBox);
            forceFieldPropertyGrid.SelectedObject = GetSelectedItem(chunk.ForceFields, forceFieldListBox);
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
            int index = listBox.SelectedIndex;
            return index >= 0 && index < list.Count ? list[index] : null;
        }

        private void PopulateReferenceCombos()
        {
            ParticleChunkState chunk = SelectedChunk;
            suppressReferenceSelection = true;
            PopulateReferenceCombo(managerAnimationComboBox, chunk, "nuccChunkAnm", chunk.Managers.Count > 0 && managerListBox.SelectedIndex >= 0 ? chunk.Managers[managerListBox.SelectedIndex].AnimationChunkIndex : 0);
            PopulateReferenceCombo(resourceEffectComboBox, chunk, null, chunk.Resources.Count > 0 && resourceListBox.SelectedIndex >= 0 ? chunk.Resources[resourceListBox.SelectedIndex].EffectChunkIndex : 0);
            PopulateReferenceCombo(positionCoordComboBox, chunk, "nuccChunkCoord", chunk.Positions.Count > 0 && positionListBox.SelectedIndex >= 0 ? chunk.Positions[positionListBox.SelectedIndex].CoordChunkIndex : 0);
            PopulateReferenceCombo(positionClumpComboBox, chunk, "nuccChunkClump", chunk.Positions.Count > 0 && positionListBox.SelectedIndex >= 0 ? chunk.Positions[positionListBox.SelectedIndex].ClumpChunkIndex : 0);
            PopulateReferenceCombo(forceFieldCoordComboBox, chunk, "nuccChunkCoord", chunk.ForceFields.Count > 0 && forceFieldListBox.SelectedIndex >= 0 ? chunk.ForceFields[forceFieldListBox.SelectedIndex].CoordChunkIndex : 0);
            PopulateReferenceCombo(forceFieldClumpComboBox, chunk, "nuccChunkClump", chunk.ForceFields.Count > 0 && forceFieldListBox.SelectedIndex >= 0 ? chunk.ForceFields[forceFieldListBox.SelectedIndex].ClumpChunkIndex : 0);
            suppressReferenceSelection = false;
        }

        private static void PopulateReferenceCombo(ComboBox comboBox, ParticleChunkState chunk, string chunkTypeFilter, uint selectedIndex)
        {
            comboBox.Items.Clear();
            if (chunk == null)
            {
                comboBox.SelectedIndex = -1;
                return;
            }

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

                int frame;
                if (!int.TryParse(frameValue.ToString(), out frame))
                    throw new InvalidOperationException("Node frame values must be integers.");

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

            foreach (ParticleManagerEntry entry in chunk.Managers)
                entry.AnimationChunkIndex = remap(entry.AnimationChunkIndex);
            foreach (ParticleResourceEntry entry in chunk.Resources)
                entry.EffectChunkIndex = remap(entry.EffectChunkIndex);
            foreach (ParticlePositionEntry entry in chunk.Positions)
            {
                entry.CoordChunkIndex = remap(entry.CoordChunkIndex);
                entry.ClumpChunkIndex = remap(entry.ClumpChunkIndex);
            }
            foreach (ParticleForceFieldEntry entry in chunk.ForceFields)
            {
                entry.CoordChunkIndex = remap(entry.CoordChunkIndex);
                entry.ClumpChunkIndex = remap(entry.ClumpChunkIndex);
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

        private void ApplySelectedIndex(NumericUpDown numericUpDown, Action<uint> apply)
        {
            if (suppressIndexSelection)
                return;

            apply((uint)numericUpDown.Value);
            RefreshSectionLists();
            LoadSelectedObjects();
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
            if (chunk != null && managerListBox.SelectedIndex >= 0 && managerListBox.SelectedIndex < chunk.Managers.Count)
                ApplySelectedReferenceCombo(managerAnimationComboBox, v => chunk.Managers[managerListBox.SelectedIndex].AnimationChunkIndex = v);
        }

        private void managerEntryIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && managerListBox.SelectedIndex >= 0 && managerListBox.SelectedIndex < chunk.Managers.Count)
                ApplySelectedIndex(managerEntryIndexNumericUpDown, v => chunk.Managers[managerListBox.SelectedIndex].EntryIndex = v);
        }

        private void resourceEffectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && resourceListBox.SelectedIndex >= 0 && resourceListBox.SelectedIndex < chunk.Resources.Count)
            {
                ApplySelectedReferenceCombo(resourceEffectComboBox, v =>
                {
                    chunk.Resources[resourceListBox.SelectedIndex].EffectChunkIndex = v;
                    if (v < chunk.References.Count)
                    {
                        ParticleEffectChunkType type;
                        if (Enum.TryParse(chunk.References[(int)v].Type, true, out type))
                            chunk.Resources[resourceListBox.SelectedIndex].EffectChunkType = type;
                    }
                });
            }
        }

        private void resourceParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && resourceListBox.SelectedIndex >= 0 && resourceListBox.SelectedIndex < chunk.Resources.Count)
                ApplySelectedIndex(resourceParticleIndexNumericUpDown, v => chunk.Resources[resourceListBox.SelectedIndex].ParticleEntryIndex = v);
        }

        private void positionCoordComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && positionListBox.SelectedIndex >= 0 && positionListBox.SelectedIndex < chunk.Positions.Count)
                ApplySelectedReferenceCombo(positionCoordComboBox, v => chunk.Positions[positionListBox.SelectedIndex].CoordChunkIndex = v);
        }

        private void positionParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && positionListBox.SelectedIndex >= 0 && positionListBox.SelectedIndex < chunk.Positions.Count)
                ApplySelectedIndex(positionParticleIndexNumericUpDown, v => chunk.Positions[positionListBox.SelectedIndex].ParticleEntryIndex = v);
        }

        private void positionClumpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && positionListBox.SelectedIndex >= 0 && positionListBox.SelectedIndex < chunk.Positions.Count)
                ApplySelectedReferenceCombo(positionClumpComboBox, v => chunk.Positions[positionListBox.SelectedIndex].ClumpChunkIndex = v);
        }

        private void forceFieldCoordComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && forceFieldListBox.SelectedIndex >= 0 && forceFieldListBox.SelectedIndex < chunk.ForceFields.Count)
                ApplySelectedReferenceCombo(forceFieldCoordComboBox, v => chunk.ForceFields[forceFieldListBox.SelectedIndex].CoordChunkIndex = v);
        }

        private void forceFieldParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && forceFieldListBox.SelectedIndex >= 0 && forceFieldListBox.SelectedIndex < chunk.ForceFields.Count)
                ApplySelectedIndex(forceFieldParticleIndexNumericUpDown, v => chunk.ForceFields[forceFieldListBox.SelectedIndex].ParticleEntryIndex = v);
        }

        private void forceFieldClumpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && forceFieldListBox.SelectedIndex >= 0 && forceFieldListBox.SelectedIndex < chunk.ForceFields.Count)
                ApplySelectedReferenceCombo(forceFieldClumpComboBox, v => chunk.ForceFields[forceFieldListBox.SelectedIndex].ClumpChunkIndex = v);
        }

        private void nodeParticleIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ParticleChunkState chunk = SelectedChunk;
            if (chunk != null && nodeListBox.SelectedIndex >= 0 && nodeListBox.SelectedIndex < chunk.Managers.Count)
            {
                ApplySelectedIndex(nodeParticleIndexNumericUpDown, v => chunk.Managers[nodeListBox.SelectedIndex].EntryIndex = v);
            }
        }

        private void managerAddButton_Click(object sender, EventArgs e) { if (SelectedChunk != null) { ParticleManagerEntry newEntry = managerPropertyGrid.SelectedObject is ParticleManagerEntry entry ? ParticleChunkCodec.CloneManager(entry) : new ParticleManagerEntry(); newEntry.EntryIndex = (uint)SelectedChunk.Managers.Count; SelectedChunk.Managers.Add(newEntry); EnsureNodeLinks(SelectedChunk); RefreshSectionLists(); } }
        private void managerDuplicateButton_Click(object sender, EventArgs e) { managerAddButton_Click(sender, e); }
        private void managerDeleteButton_Click(object sender, EventArgs e) { if (SelectedChunk != null && managerListBox.SelectedIndex >= 0 && managerListBox.SelectedIndex < SelectedChunk.Managers.Count) { int removedIndex = managerListBox.SelectedIndex; uint removedEntryIndex = SelectedChunk.Managers[removedIndex].EntryIndex; SelectedChunk.Managers.RemoveAt(removedIndex); if (removedIndex >= 0 && removedIndex < SelectedChunk.Nodes.Count) SelectedChunk.Nodes.RemoveAt(removedIndex); foreach (ParticleResourceEntry entry in SelectedChunk.Resources.Where(x => x.ParticleEntryIndex == removedEntryIndex)) entry.ParticleEntryIndex = 0; foreach (ParticlePositionEntry entry in SelectedChunk.Positions.Where(x => x.ParticleEntryIndex == removedEntryIndex)) entry.ParticleEntryIndex = 0; foreach (ParticleForceFieldEntry entry in SelectedChunk.ForceFields.Where(x => x.ParticleEntryIndex == removedEntryIndex)) entry.ParticleEntryIndex = 0; RefreshSectionLists(); LoadSelectedObjects(); } }
        private void managerSaveButton_Click(object sender, EventArgs e) { managerPropertyGrid.Refresh(); RefreshSectionLists(); }
        private void resourceAddButton_Click(object sender, EventArgs e) { if (SelectedChunk != null) { SelectedChunk.Resources.Add(resourcePropertyGrid.SelectedObject is ParticleResourceEntry entry ? ParticleChunkCodec.CloneResource(entry) : new ParticleResourceEntry()); RefreshSectionLists(); } }
        private void resourceDuplicateButton_Click(object sender, EventArgs e) { resourceAddButton_Click(sender, e); }
        private void resourceDeleteButton_Click(object sender, EventArgs e) { if (SelectedChunk != null && resourceListBox.SelectedIndex >= 0 && resourceListBox.SelectedIndex < SelectedChunk.Resources.Count) { SelectedChunk.Resources.RemoveAt(resourceListBox.SelectedIndex); RefreshSectionLists(); LoadSelectedObjects(); } }
        private void resourceSaveButton_Click(object sender, EventArgs e) { resourcePropertyGrid.Refresh(); RefreshSectionLists(); }
        private void positionAddButton_Click(object sender, EventArgs e) { if (SelectedChunk != null) { SelectedChunk.Positions.Add(positionPropertyGrid.SelectedObject is ParticlePositionEntry entry ? ParticleChunkCodec.ClonePosition(entry) : new ParticlePositionEntry()); RefreshSectionLists(); } }
        private void positionDuplicateButton_Click(object sender, EventArgs e) { positionAddButton_Click(sender, e); }
        private void positionDeleteButton_Click(object sender, EventArgs e) { if (SelectedChunk != null && positionListBox.SelectedIndex >= 0 && positionListBox.SelectedIndex < SelectedChunk.Positions.Count) { SelectedChunk.Positions.RemoveAt(positionListBox.SelectedIndex); RefreshSectionLists(); LoadSelectedObjects(); } }
        private void positionSaveButton_Click(object sender, EventArgs e) { positionPropertyGrid.Refresh(); RefreshSectionLists(); }
        private void forceFieldAddButton_Click(object sender, EventArgs e) { if (SelectedChunk != null) { SelectedChunk.ForceFields.Add(forceFieldPropertyGrid.SelectedObject is ParticleForceFieldEntry entry ? ParticleChunkCodec.CloneForceField(entry) : new ParticleForceFieldEntry()); RefreshSectionLists(); } }
        private void forceFieldDuplicateButton_Click(object sender, EventArgs e) { forceFieldAddButton_Click(sender, e); }
        private void forceFieldDeleteButton_Click(object sender, EventArgs e) { if (SelectedChunk != null && forceFieldListBox.SelectedIndex >= 0 && forceFieldListBox.SelectedIndex < SelectedChunk.ForceFields.Count) { SelectedChunk.ForceFields.RemoveAt(forceFieldListBox.SelectedIndex); RefreshSectionLists(); LoadSelectedObjects(); } }
        private void forceFieldSaveButton_Click(object sender, EventArgs e) { forceFieldPropertyGrid.Refresh(); RefreshSectionLists(); }
        private void nodeAddButton_Click(object sender, EventArgs e) { if (SelectedChunk != null) { ParticleManagerEntry manager = new ParticleManagerEntry { EntryIndex = (uint)SelectedChunk.Managers.Count }; SelectedChunk.Managers.Add(manager); SelectedChunk.Nodes.Add(new ParticleNodeEntry()); RefreshSectionLists(); } }
        private void nodeDuplicateButton_Click(object sender, EventArgs e) { if (SelectedChunk != null && nodeListBox.SelectedIndex >= 0 && nodeListBox.SelectedIndex < SelectedChunk.Nodes.Count) { ParticleManagerEntry sourceManager = nodeListBox.SelectedIndex < SelectedChunk.Managers.Count ? ParticleChunkCodec.CloneManager(SelectedChunk.Managers[nodeListBox.SelectedIndex]) : new ParticleManagerEntry(); sourceManager.EntryIndex = (uint)SelectedChunk.Managers.Count; SelectedChunk.Managers.Add(sourceManager); SelectedChunk.Nodes.Add(ParticleChunkCodec.CloneNode(SelectedChunk.Nodes[nodeListBox.SelectedIndex])); RefreshSectionLists(); } }
        private void nodeDeleteButton_Click(object sender, EventArgs e) { if (SelectedChunk != null && nodeListBox.SelectedIndex >= 0 && nodeListBox.SelectedIndex < SelectedChunk.Nodes.Count) { int index = nodeListBox.SelectedIndex; if (index < SelectedChunk.Managers.Count) SelectedChunk.Managers.RemoveAt(index); SelectedChunk.Nodes.RemoveAt(index); RefreshSectionLists(); LoadSelectedNode(); } }
        private void nodeSaveButton_Click(object sender, EventArgs e) { ApplyNodeGrid(); RefreshSectionLists(); }
        private void nodeAddEventButton_Click(object sender, EventArgs e) { nodeEventsGrid.Rows.Add(GetNodeActionLabel(ParticleNodeAction.On), 0); }
        private void nodeDeleteEventButton_Click(object sender, EventArgs e) { foreach (DataGridViewRow row in nodeEventsGrid.SelectedRows) if (!row.IsNewRow) nodeEventsGrid.Rows.Remove(row); }
    }
}
