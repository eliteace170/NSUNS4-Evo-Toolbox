using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_XmlBinaryEditor : Form
    {
        private static readonly Encoding ShiftJisEncoding = Encoding.GetEncoding("shift_jis");
        private static readonly string[] CommandSuggestions =
        {
            "SKILL_EVENT_COMMAND_CHANGE_ACTION",
            "SKILL_EVENT_COMMAND_KILL",
            "SKILL_EVENT_COMMAND_REMOVE",
            "SKILL_EVENT_COMMAND_STICK"
        };
        private static readonly string[] TypeSuggestions =
        {
            "SKILL_ACTION_TYPE_ARROW",
            "SKILL_ACTION_TYPE_BOUNDBALL",
            "SKILL_ACTION_TYPE_CRAWLER",
            "SKILL_ACTION_TYPE_ELEVATOR",
            "SKILL_ACTION_TYPE_NONE",
            "SKILL_ACTION_TYPE_SINCURVE",
            "SKILL_EVENT_TYPE_ANIMATION_END",
            "SKILL_EVENT_TYPE_FRAME_ELAPSED",
            "SKILL_EVENT_TYPE_FRAME_FIXED",
            "SKILL_EVENT_TYPE_HIT_CHARACTER_DEFAULT",
            "SKILL_EVENT_TYPE_HIT_CHARACTER_GUARD",
            "SKILL_EVENT_TYPE_HIT_CHARACTER_KAWARIMI",
            "SKILL_EVENT_TYPE_HIT_POINT",
            "SKILL_EVENT_TYPE_HIT_POINT_ACTION",
            "SKILL_EVENT_TYPE_HIT_SKILL_DEFAULT",
            "SKILL_EVENT_TYPE_HIT_WORLD_DEFAULT",
            "SKILL_EVENT_TYPE_HIT_WORLD_DIRT",
            "SKILL_EVENT_TYPE_HIT_WORLD_FLOOR",
            "SKILL_EVENT_TYPE_HIT_WORLD_GRASS",
            "SKILL_EVENT_TYPE_HIT_WORLD_IRONSAND",
            "SKILL_EVENT_TYPE_HIT_WORLD_PLWALL",
            "SKILL_EVENT_TYPE_HIT_WORLD_SNOW",
            "SKILL_EVENT_TYPE_HIT_WORLD_STONE",
            "SKILL_EVENT_TYPE_HIT_WORLD_WALL",
            "SKILL_EVENT_TYPE_HIT_WORLD_WATER",
            "SKILL_TYPE_EFFECT",
            "SKILL_TYPE_MOTION"
        };
        private static readonly string[] PriorityCategorySuggestions =
        {
            "SKILL_PRIPRITY_CATEGORY_CHAKURA_SYURIKEN",
            "SKILL_PRIPRITY_CATEGORY_RUSH",
            "SKILL_PRIPRITY_CATEGORY_SYURIKEN",
            "SKILL_PRIPRITY_CATEGORY_TOBIDOUGU_NORMAL",
            "SKILL_PRIPRITY_CATEGORY_TOBIDOUGU_STRONG",
            "SKILL_PRIPRITY_CATEGORY_TOBIDOUGU_WEAK"
        };
        private static readonly string[] SkillAttributeTypeSuggestions =
        {
            "SKILL_ATTRIBUTE_TYPE_FIRE",
            "SKILL_ATTRIBUTE_TYPE_SOIL",
            "SKILL_ATTRIBUTE_TYPE_THUNDER",
            "SKILL_ATTRIBUTE_TYPE_WATER",
            "SKILL_ATTRIBUTE_TYPE_WIND"
        };
        private static readonly string[] AllSuggestions = BuildAllSuggestions();
        private readonly XmlBinaryFileState fileState = new XmlBinaryFileState();
        private bool loadingEditor;
        private bool suppressSuggestionPopup;
        private bool suggestionValueNeedsQuotes;
        private int suggestionReplaceStart;
        private int suggestionReplaceLength;
        private XmlBinaryChunkEntry copiedChunkEntry;

        private sealed class XmlBinaryChunkEntry
        {
            public string OriginalChunkName = "";
            public string ChunkName = "";
            public string ChunkPath = "";
            public string XmlText = "";
            public int Version;
            public int VersionAttribute;
        }

        private sealed class XmlBinaryFileState
        {
            public bool FileOpen;
            public string FilePath = "";
            public readonly List<XmlBinaryChunkEntry> Entries = new List<XmlBinaryChunkEntry>();
            public readonly List<string> DeletedOriginalChunkNames = new List<string>();
        }

        private sealed class ShiftJisStringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return ShiftJisEncoding; }
            }
        }

        public Tool_XmlBinaryEditor()
        {
            InitializeComponent();
            suggestionListBox.Visible = false;
            suggestionListBox.BringToFront();
            ResetUi();
        }

        private void ResetUi()
        {
            chunkListBox.Items.Clear();
            chunkListBox.Items.Add("No XML binary chunks loaded...");
            chunkListBox.SelectedIndex = -1;
            chunkNameTextBox.Text = "";
            chunkPathTextBox.Text = "";
            xmlTextBox.Text = "";
            SetEditorEnabled(false);
            UpdateStatus("Open an XFBIN that contains XML binary chunks.");
        }

        private void ClearFileState()
        {
            fileState.FileOpen = false;
            fileState.FilePath = "";
            fileState.Entries.Clear();
            fileState.DeletedOriginalChunkNames.Clear();
            ResetUi();
        }

        private void SetEditorEnabled(bool enabled)
        {
            chunkNameTextBox.Enabled = enabled;
            chunkPathTextBox.Enabled = enabled;
            xmlTextBox.Enabled = enabled;
            addChunkButton.Enabled = fileState.FileOpen;
            duplicateChunkButton.Enabled = enabled;
            deleteChunkButton.Enabled = enabled;
            copyChunkButton.Enabled = enabled;
            pasteChunkButton.Enabled = fileState.FileOpen;
            saveChunkButton.Enabled = enabled;
            saveToolStripMenuItem.Enabled = fileState.FileOpen;
            saveAsToolStripMenuItem.Enabled = fileState.FileOpen;
            closeToolStripMenuItem.Enabled = fileState.FileOpen;
        }

        private void OpenFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = ".xfbin";
                dialog.Filter = "XFBIN Files (*.xfbin)|*.xfbin";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                LoadFile(dialog.FileName);
            }
        }

        private void LoadFile(string filePath)
        {
            List<XmlBinaryChunkEntry> entries = new List<XmlBinaryChunkEntry>();

            using (XfbinParserBackend backend = new XfbinParserBackend(filePath))
            {
                foreach (XfbinBinaryChunkItem chunk in backend.GetBinaryChunks())
                {
                    string xmlText;
                    if (!TryParseXmlBinary(chunk.BinaryData, out xmlText))
                        continue;

                    entries.Add(new XmlBinaryChunkEntry
                    {
                        OriginalChunkName = chunk.ChunkName ?? "",
                        ChunkName = chunk.ChunkName ?? "",
                        ChunkPath = chunk.ChunkPath ?? "",
                        XmlText = xmlText,
                        Version = chunk.Version,
                        VersionAttribute = chunk.VersionAttribute
                    });
                }
            }

            if (entries.Count == 0)
            {
                MessageBox.Show("No XML binary chunks were found in this XFBIN.");
                return;
            }

            fileState.FileOpen = true;
            fileState.FilePath = filePath;
            fileState.Entries.Clear();
            fileState.Entries.AddRange(entries);
            fileState.DeletedOriginalChunkNames.Clear();
            RefreshChunkList();
            UpdateStatus("Loaded " + entries.Count + " XML binary chunk(s) from " + Path.GetFileName(filePath) + ".");
        }

        private void RefreshChunkList()
        {
            int selectedIndex = chunkListBox.SelectedIndex;
            chunkListBox.Items.Clear();

            if (fileState.Entries.Count == 0)
            {
                chunkListBox.Items.Add("No XML binary chunks loaded...");
                chunkListBox.SelectedIndex = -1;
                SetEditorEnabled(false);
                return;
            }

            foreach (XmlBinaryChunkEntry entry in fileState.Entries)
                chunkListBox.Items.Add(BuildChunkLabel(entry));

            chunkListBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < fileState.Entries.Count ? selectedIndex : 0;
            SetEditorEnabled(true);
        }

        private static string BuildChunkLabel(XmlBinaryChunkEntry entry)
        {
            string name = string.IsNullOrWhiteSpace(entry.ChunkName) ? "(unnamed chunk)" : entry.ChunkName;
            string path = string.IsNullOrWhiteSpace(entry.ChunkPath) ? "(no path)" : entry.ChunkPath;
            return name + " | " + path;
        }

        private XmlBinaryChunkEntry GetSelectedEntry()
        {
            int index = chunkListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
                return null;

            return fileState.Entries[index];
        }

        private void LoadSelectedEntryToEditor()
        {
            XmlBinaryChunkEntry entry = GetSelectedEntry();
            loadingEditor = true;
            try
            {
                if (entry == null)
                {
                    chunkNameTextBox.Text = "";
                    chunkPathTextBox.Text = "";
                    xmlTextBox.Text = "";
                    SetEditorEnabled(false);
                    return;
                }

                chunkNameTextBox.Text = entry.ChunkName ?? "";
                chunkPathTextBox.Text = entry.ChunkPath ?? "";
                xmlTextBox.Text = entry.XmlText ?? "";
                SetEditorEnabled(true);
                UpdateStatus("Editing " + entry.ChunkName + ".");
            }
            finally
            {
                loadingEditor = false;
            }
        }

        private bool ApplyEditorToSelectedEntry()
        {
            XmlBinaryChunkEntry entry = GetSelectedEntry();
            if (entry == null)
            {
                MessageBox.Show("No XML binary chunk selected.");
                return false;
            }

            string chunkName = (chunkNameTextBox.Text ?? "").Trim();
            string chunkPath = (chunkPathTextBox.Text ?? "").Trim();
            string xmlText = xmlTextBox.Text ?? "";

            if (string.IsNullOrWhiteSpace(chunkName))
            {
                MessageBox.Show("Chunk name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(chunkPath))
            {
                MessageBox.Show("Chunk path is required.");
                return false;
            }

            if (!ValidateXmlText(xmlText, true))
                return false;

            entry.ChunkName = chunkName;
            entry.ChunkPath = chunkPath;
            entry.XmlText = xmlText;
            chunkListBox.Items[chunkListBox.SelectedIndex] = BuildChunkLabel(entry);
            UpdateStatus("Saved chunk changes to " + chunkName + ".");
            return true;
        }

        private void AddChunk()
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("Open an XFBIN first.");
                return;
            }

            string chunkName = BuildUniqueChunkName("new_skill_xml");
            XmlBinaryChunkEntry sourceEntry = fileState.Entries.FirstOrDefault();
            XmlBinaryChunkEntry entry = new XmlBinaryChunkEntry
            {
                ChunkName = chunkName,
                ChunkPath = "Z:/param/skill/e/" + chunkName + ".xml",
                XmlText = BuildDefaultXml(chunkName),
                Version = sourceEntry != null ? sourceEntry.Version : 99,
                VersionAttribute = sourceEntry != null ? sourceEntry.VersionAttribute : 37494
            };

            fileState.Entries.Add(entry);
            RefreshChunkList();
            chunkListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DuplicateChunk()
        {
            XmlBinaryChunkEntry selected = GetSelectedEntry();
            if (selected == null)
            {
                MessageBox.Show("No XML binary chunk selected.");
                return;
            }

            string copyName = BuildUniqueChunkName(selected.ChunkName + "_copy");
            XmlBinaryChunkEntry copy = new XmlBinaryChunkEntry
            {
                ChunkName = copyName,
                ChunkPath = ReplacePathFileName(selected.ChunkPath, copyName + ".xml"),
                XmlText = ReplaceRootSkillId(selected.XmlText, selected.ChunkName, copyName),
                Version = selected.Version,
                VersionAttribute = selected.VersionAttribute
            };

            fileState.Entries.Add(copy);
            RefreshChunkList();
            chunkListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DeleteChunk()
        {
            int index = chunkListBox.SelectedIndex;
            if (index < 0 || index >= fileState.Entries.Count)
            {
                MessageBox.Show("No XML binary chunk selected.");
                return;
            }

            XmlBinaryChunkEntry entry = fileState.Entries[index];
            if (!string.IsNullOrWhiteSpace(entry.OriginalChunkName) &&
                !fileState.DeletedOriginalChunkNames.Contains(entry.OriginalChunkName, StringComparer.OrdinalIgnoreCase))
            {
                fileState.DeletedOriginalChunkNames.Add(entry.OriginalChunkName);
            }

            fileState.Entries.RemoveAt(index);
            RefreshChunkList();
            if (fileState.Entries.Count > 0)
                chunkListBox.SelectedIndex = Math.Min(index, fileState.Entries.Count - 1);
        }

        private void CopyChunk()
        {
            XmlBinaryChunkEntry selected = GetSelectedEntry();
            if (selected == null)
            {
                MessageBox.Show("No XML binary chunk selected.");
                return;
            }

            copiedChunkEntry = CloneChunkEntry(selected);
            if (!string.IsNullOrEmpty(selected.XmlText))
                Clipboard.SetText(selected.XmlText);
            UpdateStatus("Copied chunk " + selected.ChunkName + ".");
        }

        private void PasteChunk()
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("Open an XFBIN first.");
                return;
            }

            XmlBinaryChunkEntry pasted;
            if (copiedChunkEntry != null)
            {
                pasted = CloneChunkEntry(copiedChunkEntry);
                string oldName = pasted.ChunkName;
                pasted.OriginalChunkName = "";
                pasted.ChunkName = BuildUniqueChunkName(pasted.ChunkName + "_copy");
                pasted.ChunkPath = ReplacePathFileName(pasted.ChunkPath, pasted.ChunkName + ".xml");
                pasted.XmlText = ReplaceRootSkillId(pasted.XmlText, oldName, pasted.ChunkName);
            }
            else if (Clipboard.ContainsText())
            {
                string xmlText = Clipboard.GetText();
                if (!ValidateXmlText(xmlText, true))
                    return;

                string chunkName = BuildUniqueChunkName(TryGetRootSkillId(xmlText) ?? "pasted_skill_xml");
                XmlBinaryChunkEntry sourceEntry = fileState.Entries.FirstOrDefault();
                pasted = new XmlBinaryChunkEntry
                {
                    ChunkName = chunkName,
                    ChunkPath = "Z:/param/skill/e/" + chunkName + ".xml",
                    XmlText = ReplaceRootSkillId(xmlText, "", chunkName),
                    Version = sourceEntry != null ? sourceEntry.Version : 99,
                    VersionAttribute = sourceEntry != null ? sourceEntry.VersionAttribute : 37494
                };
            }
            else
            {
                MessageBox.Show("No copied chunk is available.");
                return;
            }

            fileState.Entries.Add(pasted);
            RefreshChunkList();
            chunkListBox.SelectedIndex = fileState.Entries.Count - 1;
            UpdateStatus("Pasted chunk " + pasted.ChunkName + ".");
        }

        private void SaveFile(bool saveAs)
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("No file loaded.");
                return;
            }

            if (chunkListBox.SelectedIndex >= 0 && chunkListBox.SelectedIndex < fileState.Entries.Count)
            {
                if (!ApplyEditorToSelectedEntry())
                    return;
            }

            string errorMessage;
            if (!ValidateEntries(out errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }

            string outputPath = fileState.FilePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.DefaultExt = ".xfbin";
                    dialog.Filter = "XFBIN Files (*.xfbin)|*.xfbin";
                    dialog.FileName = Path.GetFileName(fileState.FilePath);
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    outputPath = dialog.FileName;
                }
            }

            using (XfbinParserBackend backend = new XfbinParserBackend(fileState.FilePath))
            {
                foreach (string deletedChunkName in fileState.DeletedOriginalChunkNames)
                {
                    bool stillUsed = fileState.Entries.Any(x =>
                        string.Equals(x.OriginalChunkName, deletedChunkName, StringComparison.OrdinalIgnoreCase));
                    if (!stillUsed)
                        backend.DeleteBinaryChunk(deletedChunkName);
                }

                foreach (XmlBinaryChunkEntry entry in fileState.Entries)
                {
                    backend.UpsertChunk(
                        entry.OriginalChunkName,
                        entry.ChunkName,
                        "nuccChunkBinary",
                        entry.ChunkPath,
                        ".binary",
                        BuildXmlBinary(entry.XmlText),
                        entry.Version,
                        entry.VersionAttribute);
                }

                backend.RepackTo(outputPath);
            }

            if (!File.Exists(outputPath))
            {
                MessageBox.Show("XFBIN write failed.");
                return;
            }

            fileState.FilePath = outputPath;
            foreach (XmlBinaryChunkEntry entry in fileState.Entries)
                entry.OriginalChunkName = entry.ChunkName;
            fileState.DeletedOriginalChunkNames.Clear();
            UpdateStatus("Saved " + Path.GetFileName(outputPath) + ".");
        }

        private bool ValidateEntries(out string errorMessage)
        {
            errorMessage = "";
            HashSet<string> chunkNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (XmlBinaryChunkEntry entry in fileState.Entries)
            {
                if (string.IsNullOrWhiteSpace(entry.ChunkName))
                {
                    errorMessage = "All chunks must have a name.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(entry.ChunkPath))
                {
                    errorMessage = "All chunks must have a path.";
                    return false;
                }

                if (!chunkNames.Add(entry.ChunkName))
                {
                    errorMessage = "Duplicate chunk name: " + entry.ChunkName;
                    return false;
                }

                if (!ValidateXmlText(entry.XmlText, false))
                {
                    errorMessage = "Invalid XML in chunk: " + entry.ChunkName;
                    return false;
                }
            }

            return true;
        }

        private bool ValidateXmlText(string xmlText, bool showMessage)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.PreserveWhitespace = true;
                document.LoadXml(xmlText ?? "");
                if (showMessage)
                    UpdateStatus("XML is valid.");
                return true;
            }
            catch (Exception ex)
            {
                if (showMessage)
                    MessageBox.Show("XML is invalid: " + ex.Message);
                return false;
            }
        }

        private static bool TryParseXmlBinary(byte[] bytes, out string xmlText)
        {
            xmlText = "";
            if (bytes == null || bytes.Length < 5)
                return false;

            int declaredLength = ReadInt32BE(bytes, 0);
            if (declaredLength < 0 || declaredLength > bytes.Length)
                return false;

            int xmlLength = declaredLength == bytes.Length ? bytes.Length - 4 : declaredLength;
            if (xmlLength == 0)
                xmlLength = bytes.Length - 4;
            if (xmlLength <= 0 || 4 + xmlLength > bytes.Length)
                return false;

            xmlText = ShiftJisEncoding.GetString(bytes, 4, xmlLength);
            string trimmed = xmlText.TrimStart();
            if (!trimmed.StartsWith("<", StringComparison.Ordinal))
                return false;

            return ValidateXmlString(xmlText);
        }

        private static bool ValidateXmlString(string xmlText)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.PreserveWhitespace = true;
                document.LoadXml(xmlText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static byte[] BuildXmlBinary(string xmlText)
        {
            byte[] xmlBytes = ShiftJisEncoding.GetBytes(xmlText ?? "");
            byte[] output = new byte[xmlBytes.Length + 4];
            WriteInt32BE(output, 0, xmlBytes.Length);
            Array.Copy(xmlBytes, 0, output, 4, xmlBytes.Length);
            return output;
        }

        private string BuildUniqueChunkName(string baseName)
        {
            string cleaned = string.IsNullOrWhiteSpace(baseName) ? "new_skill_xml" : baseName.Trim();
            string candidate = cleaned;
            int suffix = 1;
            while (fileState.Entries.Any(x => string.Equals(x.ChunkName, candidate, StringComparison.OrdinalIgnoreCase)))
            {
                candidate = cleaned + "_" + suffix;
                suffix++;
            }

            return candidate;
        }

        private static string BuildDefaultXml(string skillId)
        {
            return "<?xml version=\"1.0\" encoding=\"Shift_JIS\" standalone=\"yes\"?>\r\n" +
                   "<Skill id=\"" + skillId + "\" type=\"SKILL_TYPE_EFFECT\">\r\n" +
                   "\t<Files num=\"0\">\r\n" +
                   "\t</Files>\r\n" +
                   "\t<Actions num=\"0\">\r\n" +
                   "\t</Actions>\r\n" +
                   "</Skill>\r\n";
        }

        private static string ReplacePathFileName(string path, string fileName)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "Z:/param/skill/e/" + fileName;

            string normalized = path.Replace('\\', '/');
            int slash = normalized.LastIndexOf('/');
            return slash >= 0 ? normalized.Substring(0, slash + 1) + fileName : fileName;
        }

        private static string ReplaceRootSkillId(string xmlText, string oldName, string newName)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.PreserveWhitespace = true;
                document.LoadXml(xmlText);
                if (document.DocumentElement != null &&
                    string.Equals(document.DocumentElement.Name, "Skill", StringComparison.OrdinalIgnoreCase))
                {
                    XmlAttribute idAttribute = document.DocumentElement.Attributes["id"];
                    if (idAttribute != null &&
                        (string.IsNullOrWhiteSpace(oldName) ||
                         string.Equals(idAttribute.Value, oldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        idAttribute.Value = newName;
                    }
                }

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = ShiftJisEncoding,
                    Indent = true,
                    IndentChars = "\t",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = false
                };

                using (StringWriter textWriter = new ShiftJisStringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(textWriter, settings))
                        document.Save(writer);

                    return textWriter.ToString();
                }
            }
            catch
            {
                return xmlText;
            }
        }

        private static string TryGetRootSkillId(string xmlText)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.PreserveWhitespace = true;
                document.LoadXml(xmlText);
                if (document.DocumentElement == null)
                    return null;

                XmlAttribute idAttribute = document.DocumentElement.Attributes["id"];
                return idAttribute != null && !string.IsNullOrWhiteSpace(idAttribute.Value)
                    ? idAttribute.Value.Trim()
                    : null;
            }
            catch
            {
                return null;
            }
        }

        private static XmlBinaryChunkEntry CloneChunkEntry(XmlBinaryChunkEntry entry)
        {
            return new XmlBinaryChunkEntry
            {
                OriginalChunkName = entry.OriginalChunkName,
                ChunkName = entry.ChunkName,
                ChunkPath = entry.ChunkPath,
                XmlText = entry.XmlText,
                Version = entry.Version,
                VersionAttribute = entry.VersionAttribute
            };
        }

        private static string[] BuildAllSuggestions()
        {
            return CommandSuggestions
                .Concat(TypeSuggestions)
                .Concat(PriorityCategorySuggestions)
                .Concat(SkillAttributeTypeSuggestions)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private static int ReadInt32BE(byte[] bytes, int offset)
        {
            return (bytes[offset] << 24) |
                   (bytes[offset + 1] << 16) |
                   (bytes[offset + 2] << 8) |
                   bytes[offset + 3];
        }

        private static void WriteInt32BE(byte[] bytes, int offset, int value)
        {
            bytes[offset] = (byte)((value >> 24) & 0xFF);
            bytes[offset + 1] = (byte)((value >> 16) & 0xFF);
            bytes[offset + 2] = (byte)((value >> 8) & 0xFF);
            bytes[offset + 3] = (byte)(value & 0xFF);
        }

        private void UpdateStatus(string text)
        {
            statusLabel.Text = text ?? "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearFileState();
        }

        private void chunkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingEditor)
                LoadSelectedEntryToEditor();
        }

        private void addChunkButton_Click(object sender, EventArgs e)
        {
            AddChunk();
        }

        private void duplicateChunkButton_Click(object sender, EventArgs e)
        {
            DuplicateChunk();
        }

        private void deleteChunkButton_Click(object sender, EventArgs e)
        {
            DeleteChunk();
        }

        private void copyChunkButton_Click(object sender, EventArgs e)
        {
            CopyChunk();
        }

        private void pasteChunkButton_Click(object sender, EventArgs e)
        {
            PasteChunk();
        }

        private void saveChunkButton_Click(object sender, EventArgs e)
        {
            ApplyEditorToSelectedEntry();
        }

        private void xmlTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loadingEditor && !suppressSuggestionPopup)
                UpdateSuggestionPopup();
        }

        private void xmlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!suggestionListBox.Visible)
                return;

            if (e.KeyCode == Keys.Down)
            {
                if (suggestionListBox.SelectedIndex < suggestionListBox.Items.Count - 1)
                    suggestionListBox.SelectedIndex++;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (suggestionListBox.SelectedIndex > 0)
                    suggestionListBox.SelectedIndex--;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                InsertSelectedSuggestion();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideSuggestionPopup();
                e.SuppressKeyPress = true;
            }
        }

        private void xmlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Up ||
                e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Tab ||
                e.KeyCode == Keys.Escape)
                return;

            if (!loadingEditor && !suppressSuggestionPopup)
                UpdateSuggestionPopup();
        }

        private void suggestionListBox_DoubleClick(object sender, EventArgs e)
        {
            InsertSelectedSuggestion();
        }

        private void UpdateSuggestionPopup()
        {
            string text = xmlTextBox.Text ?? "";
            int caret = xmlTextBox.SelectionStart;
            if (caret < 0 || caret > text.Length)
            {
                HideSuggestionPopup();
                return;
            }

            string prefix;
            IEnumerable<string> sourceSuggestions = GetSuggestionSource(text, caret, out prefix);
            if (sourceSuggestions == null)
            {
                HideSuggestionPopup();
                return;
            }

            if (prefix.Length < 2 && !IsInsideQuotedSuggestionValue(text, caret) && !suggestionValueNeedsQuotes)
            {
                HideSuggestionPopup();
                return;
            }

            List<string> matches = sourceSuggestions
                .Where(suggestion => suggestion.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Take(12)
                .ToList();

            if (matches.Count == 0)
            {
                HideSuggestionPopup();
                return;
            }

            suggestionListBox.BeginUpdate();
            suggestionListBox.Items.Clear();
            foreach (string match in matches)
                suggestionListBox.Items.Add(match);
            suggestionListBox.SelectedIndex = 0;
            suggestionListBox.Height = Math.Min(160, 4 + (matches.Count * suggestionListBox.ItemHeight));
            suggestionListBox.EndUpdate();

            System.Drawing.Point caretPoint = xmlTextBox.GetPositionFromCharIndex(caret);
            int x = xmlTextBox.Left + caretPoint.X;
            int y = xmlTextBox.Top + caretPoint.Y + (int)Math.Ceiling(xmlTextBox.Font.GetHeight()) + 4;
            x = Math.Max(0, Math.Min(x, editorPanel.ClientSize.Width - suggestionListBox.Width - 4));
            y = Math.Max(0, Math.Min(y, editorPanel.ClientSize.Height - suggestionListBox.Height - 4));
            suggestionListBox.Location = new System.Drawing.Point(x, y);
            suggestionListBox.Visible = true;
            suggestionListBox.BringToFront();
        }

        private IEnumerable<string> GetSuggestionSource(string text, int caret, out string prefix)
        {
            int start = caret;
            while (start > 0 && IsSuggestionCharacter(text[start - 1]))
                start--;

            prefix = text.Substring(start, caret - start);
            suggestionReplaceStart = start;
            suggestionReplaceLength = caret - start;
            suggestionValueNeedsQuotes = false;

            string beforeCaret = text.Substring(0, caret);
            Match commandMatch = Regex.Match(beforeCaret, "command\\s*=\\s*(\"?)([A-Z0-9_]*)$", RegexOptions.IgnoreCase);
            if (commandMatch.Success)
            {
                prefix = commandMatch.Groups[2].Value;
                suggestionReplaceStart = caret - prefix.Length;
                suggestionReplaceLength = prefix.Length;
                suggestionValueNeedsQuotes = commandMatch.Groups[1].Value.Length == 0;
                return CommandSuggestions;
            }

            Match priorityCategoryMatch = Regex.Match(beforeCaret, "priorityCategory\\s*=\\s*(\"?)([A-Z0-9_]*)$", RegexOptions.IgnoreCase);
            if (priorityCategoryMatch.Success)
            {
                prefix = priorityCategoryMatch.Groups[2].Value;
                suggestionReplaceStart = caret - prefix.Length;
                suggestionReplaceLength = prefix.Length;
                suggestionValueNeedsQuotes = priorityCategoryMatch.Groups[1].Value.Length == 0;
                return PriorityCategorySuggestions;
            }

            Match skillAttributeTypeMatch = Regex.Match(beforeCaret, "skillAttributeType\\s*=\\s*(\"?)([A-Z0-9_]*)$", RegexOptions.IgnoreCase);
            if (skillAttributeTypeMatch.Success)
            {
                prefix = skillAttributeTypeMatch.Groups[2].Value;
                suggestionReplaceStart = caret - prefix.Length;
                suggestionReplaceLength = prefix.Length;
                suggestionValueNeedsQuotes = skillAttributeTypeMatch.Groups[1].Value.Length == 0;
                return SkillAttributeTypeSuggestions;
            }

            Match typeMatch = Regex.Match(beforeCaret, "type\\s*=\\s*(\"?)([A-Z0-9_]*)$", RegexOptions.IgnoreCase);
            if (typeMatch.Success)
            {
                prefix = typeMatch.Groups[2].Value;
                suggestionReplaceStart = caret - prefix.Length;
                suggestionReplaceLength = prefix.Length;
                suggestionValueNeedsQuotes = typeMatch.Groups[1].Value.Length == 0;
                return TypeSuggestions;
            }

            return string.IsNullOrWhiteSpace(prefix) ? null : AllSuggestions;
        }

        private static bool IsSuggestionCharacter(char value)
        {
            return char.IsLetterOrDigit(value) || value == '_';
        }

        private static bool IsInsideQuotedSuggestionValue(string text, int caret)
        {
            int previousLessThan = text.LastIndexOf('<', Math.Max(0, caret - 1));
            int previousGreaterThan = text.LastIndexOf('>', Math.Max(0, caret - 1));
            if (previousLessThan < 0 || previousGreaterThan > previousLessThan)
                return false;

            int quoteCount = 0;
            for (int i = previousLessThan; i < caret; i++)
            {
                if (text[i] == '"')
                    quoteCount++;
            }

            return (quoteCount % 2) == 1;
        }

        private void InsertSelectedSuggestion()
        {
            if (!suggestionListBox.Visible || suggestionListBox.SelectedItem == null)
                return;

            string value = suggestionListBox.SelectedItem.ToString();
            string insertedValue = suggestionValueNeedsQuotes ? "\"" + value + "\"" : value;
            suppressSuggestionPopup = true;
            try
            {
                xmlTextBox.Select(suggestionReplaceStart, suggestionReplaceLength);
                xmlTextBox.SelectedText = insertedValue;
                xmlTextBox.SelectionStart = suggestionReplaceStart + insertedValue.Length;
            }
            finally
            {
                suppressSuggestionPopup = false;
                HideSuggestionPopup();
                xmlTextBox.Focus();
            }
        }

        private void HideSuggestionPopup()
        {
            suggestionListBox.Visible = false;
        }
    }
}
