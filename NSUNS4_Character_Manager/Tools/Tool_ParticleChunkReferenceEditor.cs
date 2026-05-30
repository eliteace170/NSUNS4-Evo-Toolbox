using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    internal partial class Tool_ParticleChunkReferenceEditor : Form
    {
        private static readonly string[] StandardChunkTypes =
        {
            "nuccChunkAnm",
            "nuccChunkCoord",
            "nuccChunkClump",
            "nuccChunkBillboard",
            "nuccChunkSprite",
            "nuccChunkSprite2"
        };

        private sealed class EditableReferenceRow : INotifyPropertyChanged
        {
            private string name;
            private string type;
            private string path;

            public event PropertyChangedEventHandler PropertyChanged;

            public int OriginalIndex { get; set; }

            public string Name
            {
                get { return name; }
                set { SetField(ref name, value ?? "", "Name"); }
            }

            public string Type
            {
                get { return type; }
                set { SetField(ref type, value ?? "", "Type"); }
            }

            public string Path
            {
                get { return path; }
                set { SetField(ref path, value ?? "", "Path"); }
            }

            public EditableReferenceRow CloneAsNew()
            {
                return new EditableReferenceRow
                {
                    OriginalIndex = -1,
                    Name = Name,
                    Type = Type,
                    Path = Path
                };
            }

            private void SetField(ref string field, string value, string propertyName)
            {
                if (string.Equals(field, value, StringComparison.Ordinal))
                    return;

                field = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private readonly BindingSource bindingSource = new BindingSource();
        private readonly BindingList<EditableReferenceRow> rows = new BindingList<EditableReferenceRow>();

        public Tool_ParticleChunkReferenceEditor(IEnumerable<ParticleChunkReferenceEntry> references)
        {
            InitializeComponent();
            ConfigureEditor();
            Text = "Particle Linked Chunks";

            int index = 0;
            foreach (ParticleChunkReferenceEntry reference in references ?? Enumerable.Empty<ParticleChunkReferenceEntry>())
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
            RefreshTypeChoices();
            SelectRow(rows.Count > 0 ? 0 : -1);
            UpdateStatus();
        }

        private void ConfigureEditor()
        {
            referencesGrid.SelectionChanged += referencesGrid_SelectionChanged;
            referencesGrid.CellFormatting += referencesGrid_CellFormatting;
            referencesGrid.DataError += referencesGrid_DataError;
            referencesGrid.DataBindingComplete += referencesGrid_DataBindingComplete;

            bindingSource.CurrentChanged += bindingSource_CurrentChanged;
            rows.ListChanged += rows_ListChanged;

            nameTextBox.DataBindings.Add("Text", bindingSource, "Name", true, DataSourceUpdateMode.OnPropertyChanged, "");
            typeComboBox.DataBindings.Add("Text", bindingSource, "Type", true, DataSourceUpdateMode.OnPropertyChanged, "");
            pathTextBox.DataBindings.Add("Text", bindingSource, "Path", true, DataSourceUpdateMode.OnPropertyChanged, "");
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
            EndCurrentEdit();

            int insertIndex = GetSelectedRowIndex();
            if (insertIndex < 0)
                insertIndex = rows.Count - 1;

            EditableReferenceRow row = new EditableReferenceRow
            {
                OriginalIndex = -1,
                Name = "",
                Type = "nuccChunkAnm",
                Path = ""
            };

            rows.Insert(insertIndex + 1, row);
            RefreshAfterListEdit(insertIndex + 1);
            nameTextBox.Focus();
            nameTextBox.SelectAll();
        }

        private void duplicateButton_Click(object sender, EventArgs e)
        {
            EndCurrentEdit();

            int index = GetSelectedRowIndex();
            if (index < 0 || index >= rows.Count)
                return;

            rows.Insert(index + 1, rows[index].CloneAsNew());
            RefreshAfterListEdit(index + 1);
            nameTextBox.Focus();
            nameTextBox.SelectAll();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            EndCurrentEdit();

            int index = GetSelectedRowIndex();
            if (index < 0 || index >= rows.Count)
                return;

            rows.RemoveAt(index);
            RefreshAfterListEdit(Math.Min(index, rows.Count - 1));
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveSelectedRow(-1);
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveSelectedRow(1);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            EndCurrentEdit();

            int invalidRow = rows.ToList().FindIndex(x => string.IsNullOrWhiteSpace(x.Name) || string.IsNullOrWhiteSpace(x.Type));
            if (invalidRow >= 0)
            {
                SelectRow(invalidRow);
                DialogResult result = MessageBox.Show(
                    this,
                    "One or more linked chunks are missing a name or type. Apply the reference list anyway?",
                    "Particle Linked Chunks",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                    return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            SelectFirstSearchMatch();
            UpdateStatus();
        }

        private void referencesGrid_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void referencesGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != rowNumberColumn.Index)
                return;

            e.Value = e.RowIndex.ToString(CultureInfo.InvariantCulture);
            e.FormattingApplied = true;
        }

        private void referencesGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateStatus();
        }

        private void referencesGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void rows_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
                RefreshTypeChoices();

            UpdateStatus();
        }

        private void MoveSelectedRow(int direction)
        {
            EndCurrentEdit();

            int index = GetSelectedRowIndex();
            int targetIndex = index + direction;
            if (index < 0 || index >= rows.Count || targetIndex < 0 || targetIndex >= rows.Count)
                return;

            EditableReferenceRow row = rows[index];
            rows.RemoveAt(index);
            rows.Insert(targetIndex, row);
            RefreshAfterListEdit(targetIndex);
        }

        private void RefreshAfterListEdit(int selectedIndex)
        {
            RefreshTypeChoices();
            referencesGrid.Invalidate();
            SelectRow(selectedIndex);
            UpdateStatus();
        }

        private void RefreshTypeChoices()
        {
            string currentType = typeComboBox.Text;
            string[] values = StandardChunkTypes
                .Concat(rows.Select(x => x.Type))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => Array.IndexOf(StandardChunkTypes, x) < 0 ? 1 : 0)
                .ThenBy(x => Array.IndexOf(StandardChunkTypes, x) < 0 ? x : Array.IndexOf(StandardChunkTypes, x).ToString("D2", CultureInfo.InvariantCulture), StringComparer.OrdinalIgnoreCase)
                .ToArray();

            referenceTypeColumn.DataSource = values;

            typeComboBox.Items.Clear();
            typeComboBox.Items.AddRange(values);
            typeComboBox.Text = currentType;
        }

        private void SelectFirstSearchMatch()
        {
            string search = searchTextBox.Text;
            if (string.IsNullOrWhiteSpace(search))
                return;

            int startIndex = Math.Max(GetSelectedRowIndex(), 0);
            int matchIndex = FindSearchMatch(search, startIndex);
            if (matchIndex < 0 && startIndex > 0)
                matchIndex = FindSearchMatch(search, 0);

            if (matchIndex >= 0)
                SelectRow(matchIndex);
        }

        private int FindSearchMatch(string search, int startIndex)
        {
            for (int i = startIndex; i < rows.Count; i++)
            {
                if (ContainsSearchText(rows[i], search))
                    return i;
            }

            return -1;
        }

        private static bool ContainsSearchText(EditableReferenceRow row, string search)
        {
            return IndexOf(row.Name, search) >= 0 ||
                IndexOf(row.Type, search) >= 0 ||
                IndexOf(row.Path, search) >= 0;
        }

        private static int IndexOf(string value, string search)
        {
            return (value ?? "").IndexOf(search ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateStatus()
        {
            int selectedIndex = GetSelectedRowIndex();
            countLabel.Text = rows.Count.ToString() + " linked chunk" + (rows.Count == 1 ? "" : "s");
            duplicateButton.Enabled = selectedIndex >= 0;
            moveUpButton.Enabled = selectedIndex > 0;
            moveDownButton.Enabled = selectedIndex >= 0 && selectedIndex < rows.Count - 1;
            deleteButton.Enabled = selectedIndex >= 0;
            nameTextBox.Enabled = selectedIndex >= 0;
            typeComboBox.Enabled = selectedIndex >= 0;
            pathTextBox.Enabled = selectedIndex >= 0;

            if (selectedIndex >= 0 && selectedIndex < rows.Count)
            {
                EditableReferenceRow row = rows[selectedIndex];
                string name = string.IsNullOrWhiteSpace(row.Name) ? "(unnamed)" : row.Name;
                selectionLabel.Text = "Selected: " + name;
                indexValueLabel.Text = selectedIndex.ToString(CultureInfo.InvariantCulture);
                originalIndexValueLabel.Text = row.OriginalIndex >= 0
                    ? row.OriginalIndex.ToString(CultureInfo.InvariantCulture)
                    : "New";
                statusLabel.Text = BuildStatusText(selectedIndex);
            }
            else
            {
                selectionLabel.Text = "Selected: none";
                indexValueLabel.Text = "-";
                originalIndexValueLabel.Text = "-";
                statusLabel.Text = rows.Count == 0
                    ? "No linked chunks. Add a link to create the first reference."
                    : "Select a linked chunk to edit it.";
            }
        }

        private string BuildStatusText(int selectedIndex)
        {
            string search = searchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(search))
            {
                int matches = rows.Count(x => ContainsSearchText(x, search));
                return matches == 0
                    ? "No matches for \"" + search + "\"."
                    : matches.ToString(CultureInfo.InvariantCulture) + " match" + (matches == 1 ? "" : "es") + " for \"" + search + "\".";
            }

            EditableReferenceRow row = rows[selectedIndex];
            if (string.IsNullOrWhiteSpace(row.Name) || string.IsNullOrWhiteSpace(row.Type))
                return "Name and type are required before saving a reliable reference list.";

            return "Reference indexes will be remapped when you apply changes.";
        }

        private int GetSelectedRowIndex()
        {
            return bindingSource.Position;
        }

        private void SelectRow(int index)
        {
            if (index < 0 || index >= rows.Count)
            {
                bindingSource.Position = -1;
                return;
            }

            bindingSource.Position = index;
            referencesGrid.ClearSelection();
            if (index < referencesGrid.Rows.Count)
            {
                referencesGrid.Rows[index].Selected = true;
                referencesGrid.CurrentCell = referencesGrid.Rows[index].Cells[Math.Min(1, referencesGrid.Rows[index].Cells.Count - 1)];
            }

            UpdateStatus();
        }

        private void EndCurrentEdit()
        {
            referencesGrid.EndEdit();
            Validate();
            bindingSource.EndEdit();
        }
    }
}
