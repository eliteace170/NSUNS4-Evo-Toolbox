using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    internal partial class Tool_ParticleChunkReferenceEditor : Form
    {
        private sealed class EditableReferenceRow
        {
            public int OriginalIndex { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Path { get; set; }
        }

        private readonly BindingSource bindingSource = new BindingSource();
        private readonly List<EditableReferenceRow> rows = new List<EditableReferenceRow>();

        public Tool_ParticleChunkReferenceEditor(IEnumerable<ParticleChunkReferenceEntry> references)
        {
            InitializeComponent();
            InitializeGrid();
            Text = "Particle Linked Chunks";

            int index = 0;
            foreach (ParticleChunkReferenceEntry reference in references)
            {
                rows.Add(new EditableReferenceRow
                {
                    OriginalIndex = index,
                    Name = reference.Name,
                    Type = reference.Type,
                    Path = reference.Path
                });
                index++;
            }

            bindingSource.DataSource = rows;
            referencesGrid.DataSource = bindingSource;
            UpdateStatus();
        }

        private void InitializeGrid()
        {
            referencesGrid.AutoGenerateColumns = false;
            referencesGrid.Columns.Clear();
            referencesGrid.AllowUserToAddRows = false;
            referencesGrid.AllowUserToDeleteRows = false;
            referencesGrid.MultiSelect = false;
            referencesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            referencesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            referencesGrid.SelectionChanged += referencesGrid_SelectionChanged;

            referencesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "#",
                ReadOnly = true,
                Width = 42
            });
            referencesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                Width = 220
            });

            referencesGrid.Columns.Add(new DataGridViewComboBoxColumn
            {
                DataPropertyName = "Type",
                HeaderText = "Type",
                Width = 160,
                DataSource = new[] { "nuccChunkAnm", "nuccChunkCoord", "nuccChunkClump", "nuccChunkBillboard", "nuccChunkSprite", "nuccChunkSprite2" }
            });

            referencesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Path",
                HeaderText = "Path",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        public Dictionary<int, int> BuildIndexMap()
        {
            Dictionary<int, int> map = new Dictionary<int, int>();
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].OriginalIndex >= 0)
                    map[rows[i].OriginalIndex] = i;
            }

            return map;
        }

        public List<ParticleChunkReferenceEntry> BuildResult()
        {
            return rows.Select(x => new ParticleChunkReferenceEntry
            {
                Name = x.Name ?? "",
                Type = x.Type ?? "",
                Path = x.Path ?? ""
            }).ToList();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            rows.Add(new EditableReferenceRow
            {
                OriginalIndex = -1,
                Name = "",
                Type = "nuccChunkAnm",
                Path = ""
            });
            RefreshGrid();
            if (rows.Count > 0)
                referencesGrid.CurrentCell = referencesGrid.Rows[rows.Count - 1].Cells[1];
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int index = GetSelectedRowIndex();
            if (index < 0 || index >= rows.Count)
                return;

            rows.RemoveAt(index);
            RefreshGrid();
            SelectRow(Math.Min(index, rows.Count - 1));
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            int index = GetSelectedRowIndex();
            if (index <= 0 || index >= rows.Count)
                return;

            EditableReferenceRow row = rows[index];
            rows.RemoveAt(index);
            rows.Insert(index - 1, row);
            RefreshGrid();
            SelectRow(index - 1);
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            int index = GetSelectedRowIndex();
            if (index < 0 || index >= rows.Count - 1)
                return;

            EditableReferenceRow row = rows[index];
            rows.RemoveAt(index);
            rows.Insert(index + 1, row);
            RefreshGrid();
            SelectRow(index + 1);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            referencesGrid.EndEdit();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void referencesGrid_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void RefreshGrid()
        {
            bindingSource.ResetBindings(false);
            UpdateRowNumbers();
            UpdateStatus();
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < referencesGrid.Rows.Count; i++)
            {
                DataGridViewRow row = referencesGrid.Rows[i];
                if (!row.IsNewRow && row.Cells.Count > 0)
                    row.Cells[0].Value = i.ToString();
            }
        }

        private void UpdateStatus()
        {
            int selectedIndex = GetSelectedRowIndex();
            countLabel.Text = rows.Count.ToString() + " linked chunk" + (rows.Count == 1 ? "" : "s");
            selectionLabel.Text = selectedIndex >= 0 && selectedIndex < rows.Count
                ? "Selected: " + (string.IsNullOrWhiteSpace(rows[selectedIndex].Name) ? "(unnamed)" : rows[selectedIndex].Name)
                : "Selected: none";
            moveUpButton.Enabled = selectedIndex > 0;
            moveDownButton.Enabled = selectedIndex >= 0 && selectedIndex < rows.Count - 1;
            deleteButton.Enabled = selectedIndex >= 0;
        }

        private int GetSelectedRowIndex()
        {
            return referencesGrid.CurrentRow != null ? referencesGrid.CurrentRow.Index : -1;
        }

        private void SelectRow(int index)
        {
            if (index < 0 || index >= referencesGrid.Rows.Count)
                return;

            referencesGrid.ClearSelection();
            referencesGrid.Rows[index].Selected = true;
            referencesGrid.CurrentCell = referencesGrid.Rows[index].Cells[Math.Min(1, referencesGrid.Rows[index].Cells.Count - 1)];
            UpdateStatus();
        }
    }
}
