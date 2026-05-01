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
        }

        private void InitializeGrid()
        {
            referencesGrid.AutoGenerateColumns = false;
            referencesGrid.Columns.Clear();

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
            bindingSource.ResetBindings(false);
            if (rows.Count > 0)
                referencesGrid.CurrentCell = referencesGrid.Rows[rows.Count - 1].Cells[0];
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in referencesGrid.SelectedRows.Cast<DataGridViewRow>().OrderByDescending(x => x.Index))
            {
                if (row.Index >= 0 && row.Index < rows.Count)
                    rows.RemoveAt(row.Index);
            }
            bindingSource.ResetBindings(false);
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
    }
}
