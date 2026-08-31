using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_XmlBinaryEditor : Form
    {
        private static readonly Encoding ShiftJisEncoding = Encoding.GetEncoding("shift_jis");
        private static readonly Encoding StrictShiftJisEncoding = Encoding.GetEncoding(
            "shift_jis",
            EncoderFallback.ExceptionFallback,
            DecoderFallback.ExceptionFallback);
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
        private readonly XmlBinaryFileState fileState = new XmlBinaryFileState();
        private bool loadingEditor;
        private bool editorDirty;
        private bool fileDirty;
        private bool highlightingXml;
        private bool suppressSuggestionPopup;
        private bool suggestionValueNeedsQuotes;
        private int suggestionReplaceStart;
        private int suggestionReplaceLength;
        private int loadedEntryIndex = -1;
        private XmlBinaryChunkEntry copiedChunkEntry;
        private const int WmSetRedraw = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

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
            chunkNameTextBox.TextChanged += editorField_TextChanged;
            chunkPathTextBox.TextChanged += editorField_TextChanged;
            chunkNameTextBox.Enter += dismissSuggestionPopup_Event;
            chunkPathTextBox.Enter += dismissSuggestionPopup_Event;
            chunkListBox.MouseDown += dismissSuggestionPopup_Event;
            xmlTextBox.MouseDown += dismissSuggestionPopup_Event;
            FormClosing += Tool_XmlBinaryEditor_FormClosing;
            suggestionListBox.Visible = false;
            suggestionListBox.BringToFront();
            ResetUi();
        }

        private void ResetUi()
        {
            HideSuggestionPopup();
            loadingEditor = true;
            try
            {
                chunkListBox.Items.Clear();
                chunkListBox.Items.Add("No XML binary chunks loaded...");
                chunkListBox.SelectedIndex = -1;
                chunkNameTextBox.Text = "";
                chunkPathTextBox.Text = "";
                xmlTextBox.Text = "";
            }
            finally
            {
                loadedEntryIndex = -1;
                editorDirty = false;
                loadingEditor = false;
            }
            SetEditorEnabled(false);
            UpdateStatus("Open an XFBIN that contains XML binary chunks.");
        }

        private void ClearFileState()
        {
            fileState.FileOpen = false;
            fileState.FilePath = "";
            fileState.Entries.Clear();
            fileState.DeletedOriginalChunkNames.Clear();
            fileDirty = false;
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

                if (ConfirmSaveChanges())
                    LoadFile(dialog.FileName);
            }
        }

        private void LoadFile(string filePath)
        {
            List<XmlBinaryChunkEntry> entries = new List<XmlBinaryChunkEntry>();

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the XFBIN: " + ex.Message);
                UpdateStatus("Open failed.");
                return;
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
            fileDirty = false;
            editorDirty = false;
            loadedEntryIndex = -1;
            RefreshChunkList();
            UpdateStatus("Loaded " + entries.Count + " XML binary chunk(s) from " + Path.GetFileName(filePath) + ".");
        }

        private void RefreshChunkList()
        {
            int selectedIndex = chunkListBox.SelectedIndex;
            loadingEditor = true;
            chunkListBox.BeginUpdate();
            try
            {
                chunkListBox.Items.Clear();

                if (fileState.Entries.Count == 0)
                {
                    chunkListBox.Items.Add("No XML binary chunks loaded...");
                    chunkListBox.SelectedIndex = -1;
                }
                else
                {
                    foreach (XmlBinaryChunkEntry entry in fileState.Entries)
                        chunkListBox.Items.Add(BuildChunkLabel(entry));

                    chunkListBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < fileState.Entries.Count
                        ? selectedIndex
                        : 0;
                }
            }
            finally
            {
                chunkListBox.EndUpdate();
                loadingEditor = false;
            }

            LoadSelectedEntryToEditor();
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
            HideSuggestionPopup();
            loadingEditor = true;
            try
            {
                if (entry == null)
                {
                    chunkNameTextBox.Text = "";
                    chunkPathTextBox.Text = "";
                    xmlTextBox.Text = "";
                    loadedEntryIndex = -1;
                    editorDirty = false;
                    SetEditorEnabled(false);
                    return;
                }

                chunkNameTextBox.Text = entry.ChunkName ?? "";
                chunkPathTextBox.Text = entry.ChunkPath ?? "";
                xmlTextBox.Text = entry.XmlText ?? "";
                loadedEntryIndex = chunkListBox.SelectedIndex;
                editorDirty = false;
                SetEditorEnabled(true);
                UpdateStatus("Editing " + entry.ChunkName + ".");
            }
            finally
            {
                loadingEditor = false;
            }

            HighlightXmlSyntax();
        }

        private bool ApplyEditorToSelectedEntry()
        {
            if (loadedEntryIndex < 0 || loadedEntryIndex >= fileState.Entries.Count)
            {
                MessageBox.Show("No XML binary chunk selected.");
                return false;
            }

            XmlBinaryChunkEntry entry = fileState.Entries[loadedEntryIndex];
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

            bool changed = !string.Equals(entry.ChunkName, chunkName, StringComparison.Ordinal) ||
                           !string.Equals(entry.ChunkPath, chunkPath, StringComparison.Ordinal) ||
                           !string.Equals(entry.XmlText, xmlText, StringComparison.Ordinal);
            entry.ChunkName = chunkName;
            entry.ChunkPath = chunkPath;
            entry.XmlText = xmlText;
            if (loadedEntryIndex < chunkListBox.Items.Count)
                chunkListBox.Items[loadedEntryIndex] = BuildChunkLabel(entry);
            editorDirty = false;
            if (changed)
                fileDirty = true;
            UpdateStatus("Saved chunk changes to " + chunkName + ".");
            return true;
        }

        private bool ConfirmDiscardEditorChanges(bool reloadEditor)
        {
            if (!editorDirty)
                return true;

            DialogResult result = MessageBox.Show(
                "The selected entry has changes that were not saved with the entry Save button. Discard them?",
                "Unsaved Entry Changes",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return false;

            editorDirty = false;
            if (reloadEditor)
                LoadSelectedEntryToEditor();
            return true;
        }

        private void AddChunk()
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("Open an XFBIN first.");
                return;
            }

            if (!ConfirmDiscardEditorChanges(true))
                return;

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
            fileDirty = true;
            RefreshChunkList();
            chunkListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DuplicateChunk()
        {
            if (!ConfirmDiscardEditorChanges(true))
                return;

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
            fileDirty = true;
            RefreshChunkList();
            chunkListBox.SelectedIndex = fileState.Entries.Count - 1;
        }

        private void DeleteChunk()
        {
            if (!ConfirmDiscardEditorChanges(true))
                return;

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

            editorDirty = false;
            loadedEntryIndex = -1;
            fileState.Entries.RemoveAt(index);
            fileDirty = true;
            RefreshChunkList();
            if (fileState.Entries.Count > 0)
                chunkListBox.SelectedIndex = Math.Min(index, fileState.Entries.Count - 1);
        }

        private void CopyChunk()
        {
            if (!ConfirmDiscardEditorChanges(true))
                return;

            XmlBinaryChunkEntry selected = GetSelectedEntry();
            if (selected == null)
            {
                MessageBox.Show("No XML binary chunk selected.");
                return;
            }

            copiedChunkEntry = CloneChunkEntry(selected);
            try
            {
                if (!string.IsNullOrEmpty(selected.XmlText))
                    Clipboard.SetText(selected.XmlText);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show("The chunk was copied inside the editor, but Windows clipboard access failed: " + ex.Message);
            }
            UpdateStatus("Copied chunk " + selected.ChunkName + ".");
        }

        private void PasteChunk()
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("Open an XFBIN first.");
                return;
            }

            if (!ConfirmDiscardEditorChanges(true))
                return;

            XmlBinaryChunkEntry pasted;
            string clipboardText = null;
            try
            {
                if (Clipboard.ContainsText())
                    clipboardText = Clipboard.GetText();
            }
            catch (ExternalException)
            {
                // The in-editor copy remains available when another process has the clipboard locked.
            }

            bool clipboardMatchesCopiedChunk = copiedChunkEntry != null &&
                                               string.Equals(clipboardText, copiedChunkEntry.XmlText, StringComparison.Ordinal);
            if (clipboardMatchesCopiedChunk || (copiedChunkEntry != null && clipboardText == null))
            {
                pasted = CloneChunkEntry(copiedChunkEntry);
                string oldName = pasted.ChunkName;
                pasted.OriginalChunkName = "";
                pasted.ChunkName = BuildUniqueChunkName(pasted.ChunkName + "_copy");
                pasted.ChunkPath = ReplacePathFileName(pasted.ChunkPath, pasted.ChunkName + ".xml");
                pasted.XmlText = ReplaceRootSkillId(pasted.XmlText, oldName, pasted.ChunkName);
            }
            else if (!string.IsNullOrWhiteSpace(clipboardText))
            {
                string xmlText = clipboardText;
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
            fileDirty = true;
            RefreshChunkList();
            chunkListBox.SelectedIndex = fileState.Entries.Count - 1;
            UpdateStatus("Pasted chunk " + pasted.ChunkName + ".");
        }

        private bool SaveFile(bool saveAs)
        {
            if (!fileState.FileOpen)
            {
                MessageBox.Show("No file loaded.");
                return false;
            }

            if (editorDirty)
            {
                MessageBox.Show("The selected entry has unsaved changes. Click the entry Save button before saving the XFBIN.");
                return false;
            }

            string errorMessage;
            if (!ValidateEntries(out errorMessage))
            {
                MessageBox.Show(errorMessage);
                return false;
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
                        return false;
                    outputPath = dialog.FileName;
                }
            }

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save the XFBIN: " + ex.Message);
                UpdateStatus("Save failed.");
                return false;
            }

            if (!File.Exists(outputPath))
            {
                MessageBox.Show("XFBIN write failed.");
                return false;
            }

            fileState.FilePath = outputPath;
            foreach (XmlBinaryChunkEntry entry in fileState.Entries)
                entry.OriginalChunkName = entry.ChunkName;
            fileState.DeletedOriginalChunkNames.Clear();
            fileDirty = false;
            editorDirty = false;
            UpdateStatus("Saved " + Path.GetFileName(outputPath) + ".");
            return true;
        }

        private bool ConfirmSaveChanges()
        {
            if (!fileState.FileOpen)
                return true;

            if (!ConfirmDiscardEditorChanges(true))
                return false;

            if (!fileDirty)
                return true;

            DialogResult result = MessageBox.Show(
                "Save changes to " + Path.GetFileName(fileState.FilePath) + "?",
                "XML Binary Editor",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Cancel)
                return false;
            if (result == DialogResult.Yes)
                return SaveFile(false);

            return true;
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

                try
                {
                    StrictShiftJisEncoding.GetByteCount(entry.XmlText ?? "");
                }
                catch (EncoderFallbackException ex)
                {
                    errorMessage = "Chunk " + entry.ChunkName +
                                   " contains text that cannot be saved as Shift-JIS: " + ex.Message;
                    return false;
                }
            }

            return true;
        }

        private bool ValidateXmlText(string xmlText, bool showMessage)
        {
            try
            {
                LoadXmlDocument(xmlText ?? "");
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
                LoadXmlDocument(xmlText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static XmlDocument LoadXmlDocument(string xmlText)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };
            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true,
                XmlResolver = null
            };
            using (StringReader textReader = new StringReader(xmlText ?? ""))
            using (XmlReader reader = XmlReader.Create(textReader, settings))
                document.Load(reader);
            return document;
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
                XmlDocument document = LoadXmlDocument(xmlText);
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
                XmlDocument document = LoadXmlDocument(xmlText);
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

        private void editorField_TextChanged(object sender, EventArgs e)
        {
            if (!loadingEditor)
                editorDirty = true;
        }

        private void dismissSuggestionPopup_Event(object sender, EventArgs e)
        {
            HideSuggestionPopup();
        }

        private void Tool_XmlBinaryEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmSaveChanges())
                e.Cancel = true;
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
            if (ConfirmSaveChanges())
                ClearFileState();
        }

        private void chunkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingEditor)
                return;

            HideSuggestionPopup();
            int requestedIndex = chunkListBox.SelectedIndex;
            if (requestedIndex != loadedEntryIndex && !ConfirmDiscardEditorChanges(false))
            {
                loadingEditor = true;
                try
                {
                    chunkListBox.SelectedIndex = loadedEntryIndex >= 0 && loadedEntryIndex < fileState.Entries.Count
                        ? loadedEntryIndex
                        : -1;
                }
                finally
                {
                    loadingEditor = false;
                }
                return;
            }

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
            HideSuggestionPopup();
            ApplyEditorToSelectedEntry();
        }

        private void xmlTextBox_TextChanged(object sender, EventArgs e)
        {
            // RichTextBox raises TextChanged for syntax-color formatting changes too.
            // Ignore those internal changes so they do not mark the file dirty or flash autocomplete.
            if (highlightingXml)
                return;

            if (!loadingEditor)
                editorDirty = true;

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
            bool movedCaret = e.KeyCode == Keys.Left ||
                              e.KeyCode == Keys.Right ||
                              e.KeyCode == Keys.Home ||
                              e.KeyCode == Keys.End ||
                              e.KeyCode == Keys.PageUp ||
                              e.KeyCode == Keys.PageDown;
            if (!movedCaret)
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

            return null;
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
            suggestionListBox.Items.Clear();
            suggestionValueNeedsQuotes = false;
            suggestionReplaceStart = 0;
            suggestionReplaceLength = 0;
        }

        private void HighlightXmlSyntax()
        {
            string text = xmlTextBox.Text ?? "";
            if (text.Length == 0 || text.Length > 500000)
                return;

            highlightingXml = true;
            int selectionStart = xmlTextBox.SelectionStart;
            int selectionLength = xmlTextBox.SelectionLength;

            SendMessage(xmlTextBox.Handle, WmSetRedraw, IntPtr.Zero, IntPtr.Zero);
            try
            {
                xmlTextBox.SelectAll();
                xmlTextBox.SelectionColor = Color.Black;

                foreach (Match match in Regex.Matches(text, "<!--[\\s\\S]*?-->|<!\\[CDATA\\[[\\s\\S]*?\\]\\]>|<[^>]+>"))
                {
                    string token = match.Value;
                    if (token.StartsWith("<!--", StringComparison.Ordinal) ||
                        token.StartsWith("<![CDATA[", StringComparison.Ordinal))
                    {
                        ApplyXmlSyntaxColor(match.Index, match.Length, Color.FromArgb(0, 128, 0));
                        continue;
                    }

                    ApplyTagHighlight(match.Index, token);
                }

                foreach (Match entityMatch in Regex.Matches(text, "&[A-Za-z0-9#]+;"))
                    ApplyXmlSyntaxColor(entityMatch.Index, entityMatch.Length, Color.FromArgb(128, 0, 128));
            }
            finally
            {
                int safeStart = Math.Min(selectionStart, xmlTextBox.TextLength);
                int safeLength = Math.Min(selectionLength, xmlTextBox.TextLength - safeStart);
                xmlTextBox.Select(safeStart, safeLength);
                SendMessage(xmlTextBox.Handle, WmSetRedraw, new IntPtr(1), IntPtr.Zero);
                xmlTextBox.Invalidate();
                highlightingXml = false;
            }
        }

        private void ApplyTagHighlight(int tagStart, string tagText)
        {
            ApplyXmlSyntaxColor(tagStart, tagText.Length, Color.FromArgb(0, 0, 180));

            Match tagNameMatch = Regex.Match(tagText, "^<\\s*[!?/]?\\s*([A-Za-z_][A-Za-z0-9_.:-]*)");
            if (tagNameMatch.Success)
                ApplyXmlSyntaxColor(tagStart + tagNameMatch.Groups[1].Index, tagNameMatch.Groups[1].Length, Color.FromArgb(0, 0, 180));

            foreach (Match attributeMatch in Regex.Matches(tagText, "([A-Za-z_][A-Za-z0-9_.:-]*)(\\s*=\\s*)(\"[^\"]*\"|'[^']*')"))
            {
                Group nameGroup = attributeMatch.Groups[1];
                Group equalsGroup = attributeMatch.Groups[2];
                Group valueGroup = attributeMatch.Groups[3];
                ApplyXmlSyntaxColor(tagStart + nameGroup.Index, nameGroup.Length, Color.FromArgb(160, 80, 0));
                ApplyXmlSyntaxColor(tagStart + equalsGroup.Index, equalsGroup.Length, Color.FromArgb(90, 90, 90));
                ApplyXmlSyntaxColor(tagStart + valueGroup.Index, valueGroup.Length, Color.FromArgb(163, 21, 21));
            }
        }

        private void ApplyXmlSyntaxColor(int start, int length, Color color)
        {
            if (length <= 0 || start < 0 || start + length > xmlTextBox.TextLength)
                return;

            xmlTextBox.Select(start, length);
            xmlTextBox.SelectionColor = color;
        }
    }
}
