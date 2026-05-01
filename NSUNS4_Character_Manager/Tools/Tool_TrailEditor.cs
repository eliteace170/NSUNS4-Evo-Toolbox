using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_TrailEditor : Form
    {
        private const string TrailChunkType = "nuccChunkTrail";
        private const int HeaderCount = 5;
        private const int HeaderSize = 8;
        private const int ManagerSize = 0x60;
        private const int ResourceSize = 0x20;
        private const int PositionSize = 0x30;
        private const int ForceFieldSize = 0x3C;
        private const int NodeBaseSize = 0x14;

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private Panel detailsPanel;
        private ComboBox chunkComboBox;
        private TextBox chunkNameTextBox;
        private TextBox chunkPathTextBox;
        private TabControl tabControl1;
        private ListBox managersListBox;
        private ListBox resourcesListBox;
        private ListBox positionsListBox;
        private ListBox forceFieldsListBox;
        private ListBox mapIdsListBox;
        private ListBox nodesListBox;
        private ListBox framesListBox;
        private PropertyGrid managersPropertyGrid;
        private PropertyGrid resourcesPropertyGrid;
        private PropertyGrid positionsPropertyGrid;
        private PropertyGrid forceFieldsPropertyGrid;
        private PropertyGrid mapIdsPropertyGrid;
        private PropertyGrid nodesPropertyGrid;
        private PropertyGrid framesPropertyGrid;

        private XfbinParserBackend backend;
        private TrailFileState fileState;
        private bool suppressChunkSelection;
        private bool suppressChunkFields;

        public Tool_TrailEditor()
        {
            InitializeComponent();
            WireListFormatting();
            WirePropertyGridEvents();
            UpdateUiState();
        }

        private void WireListFormatting()
        {
            this.chunkComboBox.Format += chunkComboBox_Format;
            this.managersListBox.Format += managersListBox_Format;
            this.resourcesListBox.Format += resourcesListBox_Format;
            this.positionsListBox.Format += positionsListBox_Format;
            this.forceFieldsListBox.Format += forceFieldsListBox_Format;
            this.mapIdsListBox.Format += mapIdsListBox_Format;
            this.nodesListBox.Format += nodesListBox_Format;
            this.framesListBox.Format += framesListBox_Format;

            this.managersListBox.SelectedIndexChanged += managersListBox_SelectedIndexChanged;
            this.resourcesListBox.SelectedIndexChanged += resourcesListBox_SelectedIndexChanged;
            this.positionsListBox.SelectedIndexChanged += positionsListBox_SelectedIndexChanged;
            this.forceFieldsListBox.SelectedIndexChanged += forceFieldsListBox_SelectedIndexChanged;
            this.mapIdsListBox.SelectedIndexChanged += mapIdsListBox_SelectedIndexChanged;
            this.nodesListBox.SelectedIndexChanged += nodesListBox_SelectedIndexChanged;
            this.framesListBox.SelectedIndexChanged += framesListBox_SelectedIndexChanged;
        }

        private void WirePropertyGridEvents()
        {
            this.managersPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
            this.resourcesPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
            this.positionsPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
            this.forceFieldsPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
            this.mapIdsPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
            this.nodesPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
            this.framesPropertyGrid.PropertyValueChanged += AnyPropertyGrid_PropertyValueChanged;
        }

        private TrailChunkState CurrentChunk
        {
            get
            {
                if (fileState == null)
                    return null;

                int index = this.chunkComboBox.SelectedIndex;
                if (index < 0 || index >= fileState.Chunks.Count)
                    return null;

                return fileState.Chunks[index];
            }
        }

        private TrailNodeEntry CurrentNode
        {
            get { return this.nodesListBox.SelectedItem as TrailNodeEntry; }
        }

        private TrailFrameEntry CurrentFrame
        {
            get { return this.framesListBox.SelectedItem as TrailFrameEntry; }
        }

        private sealed class TrailFileState
        {
            public string FilePath;
            public List<TrailChunkState> Chunks = new List<TrailChunkState>();
        }

        private sealed class TrailChunkState
        {
            public string OriginalChunkName;
            public string ChunkName;
            public string ChunkPath;
            public XfbinBinaryChunkPage SourcePage;
            public XfbinParserPageDefinition OriginalDefinition;
            public List<TrailMapEntry> MapEntries = new List<TrailMapEntry>();
            public List<TrailManagerEntry> Managers = new List<TrailManagerEntry>();
            public List<TrailResourceEntry> Resources = new List<TrailResourceEntry>();
            public List<TrailPositionEntry> Positions = new List<TrailPositionEntry>();
            public List<TrailForceFieldEntry> ForceFields = new List<TrailForceFieldEntry>();
            public List<TrailNodeEntry> Nodes = new List<TrailNodeEntry>();
        }

        public sealed class TrailMapEntry
        {
            public string Name = string.Empty;
            public string Type = string.Empty;
            public string Path = string.Empty;

            public TrailMapEntry Clone()
            {
                return new TrailMapEntry { Name = Name, Type = Type, Path = Path };
            }
        }

        public sealed class TrailManagerEntry
        {
            public int AnimationChunkMapId;
            public int EntryIndex;
            public uint Unk1;
            public uint Unk2;
            public uint Unk3;
            public uint Lifetime;
            public ushort TrailType;
            public ushort Unk4;
            public float Unk5;
            public float Unk6;
            public float ColorRStart;
            public float ColorGStart;
            public float ColorBStart;
            public float ColorAStart;
            public float ColorRMiddle;
            public float ColorGMiddle;
            public float ColorBMiddle;
            public float ColorAMiddle;
            public float ColorREnd;
            public float ColorGEnd;
            public float ColorBEnd;
            public float ColorAEnd;
            public float ColorFactor;
            public float ScaleFirstBoneStart;
            public float ScaleSecondBoneStart;
            public float ScaleFirstBoneMiddle;
            public float ScaleSecondBoneMiddle;
            public float ScaleFirstBoneEnd;
            public float ScaleSecondBoneEnd;

            public TrailManagerEntry Clone()
            {
                return (TrailManagerEntry)MemberwiseClone();
            }
        }

        public sealed class TrailResourceEntry
        {
            public int EffectChunkMapId;
            public int TrailEntryIndex;
            public uint Unk1;
            public uint Unk2;
            public uint Unk3;
            public uint Unk4;
            public uint Unk5;
            public uint Unk6;

            public TrailResourceEntry Clone()
            {
                return (TrailResourceEntry)MemberwiseClone();
            }
        }

        public sealed class TrailPositionEntry
        {
            public int CoordChunkMapId;
            public int TrailEntryIndex;
            public int Unk1;
            public int Unk2;
            public int Unk3;
            public int Unk4;
            public int Unk5;
            public int Unk6;
            public int ClumpChunkMapId;
            public int Unk7;
            public int Unk8;
            public int Unk9;

            public TrailPositionEntry Clone()
            {
                return (TrailPositionEntry)MemberwiseClone();
            }
        }

        public sealed class TrailForceFieldEntry
        {
            public int Unk1;
            public int TrailEntryIndex;
            public int Unk2;
            public int Unk3;
            public float Unk4;
            public float Unk5;
            public float Unk6;
            public int Unk7;
            public int Unk8;
            public int Unk9;
            public float Unk10;
            public float Unk11;
            public int Unk12;
            public int Unk13;
            public int Unk14;
            public int Unk15;
            public int Unk16;

            public TrailForceFieldEntry Clone()
            {
                return (TrailForceFieldEntry)MemberwiseClone();
            }
        }

        public sealed class TrailNodeEntry
        {
            public uint Unk1;
            public int TrailEntryIndex;
            public uint Unk2;
            public uint Unk3;
            public List<TrailFrameEntry> Frames = new List<TrailFrameEntry>();

            public TrailNodeEntry Clone()
            {
                TrailNodeEntry clone = (TrailNodeEntry)MemberwiseClone();
                clone.Frames = Frames.Select(x => x.Clone()).ToList();
                return clone;
            }
        }

        public sealed class TrailFrameEntry
        {
            public ushort Flag;
            public float Frame;

            public TrailFrameEntry Clone()
            {
                return (TrailFrameEntry)MemberwiseClone();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "XFBIN files (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                LoadFile(dialog.FileName);
            }
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
            CloseFile();
        }

        private void LoadFile(string filePath)
        {
            CloseFile();

            backend = new XfbinParserBackend(filePath);
            fileState = new TrailFileState();
            fileState.FilePath = filePath;

            foreach (XfbinBinaryChunkPage page in backend.GetChunkPages(TrailChunkType))
            {
                TrailChunkState chunk;
                if (!TryParseTrailChunk(page, out chunk))
                    continue;
                fileState.Chunks.Add(chunk);
            }

            if (fileState.Chunks.Count == 0)
            {
                CloseFile();
                MessageBox.Show(this, "This XFBIN does not contain a readable nuccChunkTrail page.", "Trail Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RefreshChunkCombo();
            this.chunkComboBox.SelectedIndex = 0;
            UpdateUiState();
        }

        private void SaveFile(bool saveAs)
        {
            if (fileState == null || backend == null)
                return;

            string outputPath = fileState.FilePath;
            if (saveAs)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "XFBIN files (*.xfbin)|*.xfbin|All files (*.*)|*.*";
                    dialog.FileName = Path.GetFileName(fileState.FilePath);
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;
                    outputPath = dialog.FileName;
                }
            }

            foreach (TrailChunkState chunk in fileState.Chunks)
            {
                byte[] data = BuildChunkData(chunk);
                backend.UpsertChunk(TrailChunkType, chunk.OriginalChunkName, chunk.ChunkName, chunk.ChunkPath, data, ".trail");
                backend.SaveChunkPageDefinition(TrailChunkType, chunk.ChunkName, BuildPageDefinition(chunk));
                chunk.OriginalChunkName = chunk.ChunkName;
            }

            backend.RepackTo(outputPath);
            if (saveAs)
                LoadFile(outputPath);
        }

        private void CloseFile()
        {
            if (backend != null)
            {
                backend.Dispose();
                backend = null;
            }

            fileState = null;
            suppressChunkFields = true;
            this.chunkComboBox.Items.Clear();
            this.chunkComboBox.SelectedIndex = -1;
            this.chunkNameTextBox.Text = string.Empty;
            this.chunkPathTextBox.Text = string.Empty;
            suppressChunkFields = false;
            ClearLists();
            UpdateUiState();
        }

        private void ClearLists()
        {
            this.managersListBox.Items.Clear();
            this.resourcesListBox.Items.Clear();
            this.positionsListBox.Items.Clear();
            this.forceFieldsListBox.Items.Clear();
            this.mapIdsListBox.Items.Clear();
            this.nodesListBox.Items.Clear();
            this.framesListBox.Items.Clear();

            this.managersPropertyGrid.SelectedObject = null;
            this.resourcesPropertyGrid.SelectedObject = null;
            this.positionsPropertyGrid.SelectedObject = null;
            this.forceFieldsPropertyGrid.SelectedObject = null;
            this.mapIdsPropertyGrid.SelectedObject = null;
            this.nodesPropertyGrid.SelectedObject = null;
            this.framesPropertyGrid.SelectedObject = null;
        }

        private bool TryParseTrailChunk(XfbinBinaryChunkPage page, out TrailChunkState chunk)
        {
            chunk = null;
            byte[] data = page.BinaryData ?? new byte[0];
            if (data.Length < HeaderCount * HeaderSize)
                return false;

            TrailSectionHeader[] headers = new TrailSectionHeader[HeaderCount];
            for (int i = 0; i < HeaderCount; i++)
            {
                int offset = i * HeaderSize;
                headers[i] = new TrailSectionHeader
                {
                    Offset = (int)ReadUInt32BE(data, offset),
                    Count = ReadUInt16BE(data, offset + 4),
                    Size = ReadUInt16BE(data, offset + 6)
                };
            }

            chunk = new TrailChunkState
            {
                OriginalChunkName = page.ChunkName ?? string.Empty,
                ChunkName = page.ChunkName ?? string.Empty,
                ChunkPath = page.ChunkPath ?? string.Empty,
                SourcePage = page,
                OriginalDefinition = DeepClone(page.Definition),
                MapEntries = LoadMapEntries(page.Definition)
            };

            if (!TryParseManagers(data, headers[0], chunk.Managers))
                return false;
            if (!TryParseResources(data, headers[1], chunk.Resources))
                return false;
            if (!TryParsePositions(data, headers[2], chunk.Positions))
                return false;
            if (!TryParseForceFields(data, headers[3], chunk.ForceFields))
                return false;
            if (!TryParseNodes(data, headers[4], chunk.Nodes))
                return false;

            return true;
        }

        private static List<TrailMapEntry> LoadMapEntries(XfbinParserPageDefinition definition)
        {
            List<TrailMapEntry> result = new List<TrailMapEntry>();
            if (definition != null && definition.ChunkMaps != null)
            {
                result.AddRange(definition.ChunkMaps.Select(x => new TrailMapEntry
                {
                    Name = x != null ? (x.Name ?? string.Empty) : string.Empty,
                    Type = x != null ? (x.Type ?? string.Empty) : string.Empty,
                    Path = x != null ? (x.Path ?? string.Empty) : string.Empty
                }));
            }

            if (result.Count == 0)
            {
                result.Add(new TrailMapEntry { Name = string.Empty, Type = "nuccChunkNull", Path = string.Empty });
                result.Add(new TrailMapEntry { Name = string.Empty, Type = TrailChunkType, Path = string.Empty });
                result.Add(new TrailMapEntry { Name = "Page0", Type = "nuccChunkPage", Path = string.Empty });
                result.Add(new TrailMapEntry { Name = "index", Type = "nuccChunkIndex", Path = string.Empty });
            }

            return result;
        }

        private bool TryParseManagers(byte[] data, TrailSectionHeader header, List<TrailManagerEntry> list)
        {
            if (!ValidateSectionBounds(data, header))
                return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + (i * ManagerSize);
                if (offset + ManagerSize > data.Length)
                    return false;

                TrailManagerEntry entry = new TrailManagerEntry();
                entry.AnimationChunkMapId = (int)ReadUInt32BE(data, offset + 0x00);
                entry.EntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                entry.Unk1 = ReadUInt32BE(data, offset + 0x08);
                entry.Unk2 = ReadUInt32BE(data, offset + 0x0C);
                entry.Unk3 = ReadUInt32BE(data, offset + 0x10);
                entry.Lifetime = ReadUInt32BE(data, offset + 0x14);
                entry.TrailType = ReadUInt16BE(data, offset + 0x18);
                entry.Unk4 = ReadUInt16BE(data, offset + 0x1A);
                entry.Unk5 = ReadUInt16BE(data, offset + 0x1C) / 65535f;
                entry.Unk6 = ReadUInt16BE(data, offset + 0x1E) / 65535f;
                entry.ColorRStart = ReadSingleBE(data, offset + 0x20);
                entry.ColorGStart = ReadSingleBE(data, offset + 0x24);
                entry.ColorBStart = ReadSingleBE(data, offset + 0x28);
                entry.ColorAStart = ReadSingleBE(data, offset + 0x2C);
                entry.ColorRMiddle = ReadSingleBE(data, offset + 0x30);
                entry.ColorGMiddle = ReadSingleBE(data, offset + 0x34);
                entry.ColorBMiddle = ReadSingleBE(data, offset + 0x38);
                entry.ColorAMiddle = ReadSingleBE(data, offset + 0x3C);
                entry.ColorREnd = ReadSingleBE(data, offset + 0x40);
                entry.ColorGEnd = ReadSingleBE(data, offset + 0x44);
                entry.ColorBEnd = ReadSingleBE(data, offset + 0x48);
                entry.ColorAEnd = ReadSingleBE(data, offset + 0x4C);
                entry.ColorFactor = ReadSingleBE(data, offset + 0x50);
                entry.ScaleFirstBoneStart = ReadUInt16BE(data, offset + 0x54) / 65535f * 100f;
                entry.ScaleSecondBoneStart = ReadUInt16BE(data, offset + 0x56) / 65535f * 100f;
                entry.ScaleFirstBoneMiddle = ReadUInt16BE(data, offset + 0x58) / 65535f * 100f;
                entry.ScaleSecondBoneMiddle = ReadUInt16BE(data, offset + 0x5A) / 65535f * 100f;
                entry.ScaleFirstBoneEnd = ReadUInt16BE(data, offset + 0x5C) / 65535f * 100f;
                entry.ScaleSecondBoneEnd = ReadUInt16BE(data, offset + 0x5E) / 65535f * 100f;
                list.Add(entry);
            }
            return true;
        }

        private bool TryParseResources(byte[] data, TrailSectionHeader header, List<TrailResourceEntry> list)
        {
            if (!ValidateSectionBounds(data, header))
                return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + (i * ResourceSize);
                if (offset + ResourceSize > data.Length)
                    return false;
                list.Add(new TrailResourceEntry
                {
                    EffectChunkMapId = (int)ReadUInt32BE(data, offset + 0x00),
                    TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04),
                    Unk1 = ReadUInt32BE(data, offset + 0x08),
                    Unk2 = ReadUInt32BE(data, offset + 0x0C),
                    Unk3 = ReadUInt32BE(data, offset + 0x10),
                    Unk4 = ReadUInt32BE(data, offset + 0x14),
                    Unk5 = ReadUInt32BE(data, offset + 0x18),
                    Unk6 = ReadUInt32BE(data, offset + 0x1C)
                });
            }
            return true;
        }

        private bool TryParsePositions(byte[] data, TrailSectionHeader header, List<TrailPositionEntry> list)
        {
            if (!ValidateSectionBounds(data, header))
                return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + (i * PositionSize);
                if (offset + PositionSize > data.Length)
                    return false;
                list.Add(new TrailPositionEntry
                {
                    CoordChunkMapId = ReadInt32BE(data, offset + 0x00),
                    TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04),
                    Unk1 = ReadInt32BE(data, offset + 0x08),
                    Unk2 = ReadInt32BE(data, offset + 0x0C),
                    Unk3 = ReadInt32BE(data, offset + 0x10),
                    Unk4 = ReadInt32BE(data, offset + 0x14),
                    Unk5 = ReadInt32BE(data, offset + 0x18),
                    Unk6 = ReadInt32BE(data, offset + 0x1C),
                    ClumpChunkMapId = ReadInt32BE(data, offset + 0x20),
                    Unk7 = ReadInt32BE(data, offset + 0x24),
                    Unk8 = ReadInt32BE(data, offset + 0x28),
                    Unk9 = ReadInt32BE(data, offset + 0x2C)
                });
            }
            return true;
        }

        private bool TryParseForceFields(byte[] data, TrailSectionHeader header, List<TrailForceFieldEntry> list)
        {
            if (!ValidateSectionBounds(data, header))
                return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + (i * ForceFieldSize);
                if (offset + ForceFieldSize > data.Length)
                    return false;
                list.Add(new TrailForceFieldEntry
                {
                    Unk1 = ReadInt32BE(data, offset + 0x00),
                    TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04),
                    Unk2 = ReadInt32BE(data, offset + 0x08),
                    Unk3 = ReadInt32BE(data, offset + 0x0C),
                    Unk4 = ReadUInt16BE(data, offset + 0x10) / 65535f,
                    Unk5 = ReadUInt16BE(data, offset + 0x12) / 65535f,
                    Unk6 = ReadSingleBE(data, offset + 0x14),
                    Unk7 = ReadInt32BE(data, offset + 0x18),
                    Unk8 = ReadInt32BE(data, offset + 0x1C),
                    Unk9 = ReadInt32BE(data, offset + 0x20),
                    Unk10 = ReadSingleBE(data, offset + 0x24),
                    Unk11 = ReadSingleBE(data, offset + 0x28),
                    Unk12 = ReadInt32BE(data, offset + 0x2C),
                    Unk13 = ReadInt32BE(data, offset + 0x30),
                    Unk14 = ReadInt32BE(data, offset + 0x34),
                    Unk15 = ReadInt32BE(data, offset + 0x38),
                    Unk16 = ReadInt32BE(data, offset + 0x3C - 4)
                });
            }
            return true;
        }

        private bool TryParseNodes(byte[] data, TrailSectionHeader header, List<TrailNodeEntry> list)
        {
            if (!ValidateSectionBounds(data, header))
                return false;
            int offset = header.Offset;
            for (int i = 0; i < header.Count; i++)
            {
                if (offset + NodeBaseSize > data.Length)
                    return false;

                TrailNodeEntry node = new TrailNodeEntry();
                node.Unk1 = ReadUInt32BE(data, offset + 0x00);
                node.TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                node.Unk2 = ReadUInt32BE(data, offset + 0x08);
                node.Unk3 = ReadUInt32BE(data, offset + 0x0C);
                int frameCount = (int)ReadUInt32BE(data, offset + 0x10);
                offset += NodeBaseSize;

                for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                {
                    if (offset + 4 > data.Length)
                        return false;
                    node.Frames.Add(new TrailFrameEntry
                    {
                        Flag = ReadUInt16BE(data, offset + 0x00),
                        Frame = ReadUInt16BE(data, offset + 0x02) / 100f
                    });
                    offset += 4;
                }

                if ((frameCount & 1) == 0 && offset + 4 <= data.Length)
                    offset += 4;

                list.Add(node);
            }
            return true;
        }

        private byte[] BuildChunkData(TrailChunkState chunk)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            for (int i = 0; i < HeaderCount * HeaderSize; i++)
                writer.Write((byte)0);

            TrailSectionHeader[] headers = new TrailSectionHeader[HeaderCount];

            headers[0] = WriteManagers(writer, chunk.Managers);
            headers[1] = WriteResources(writer, chunk.Resources);
            headers[2] = WritePositions(writer, chunk.Positions);
            headers[3] = WriteForceFields(writer, chunk.ForceFields);
            headers[4] = WriteNodes(writer, chunk.Nodes);

            stream.Position = 0;
            for (int i = 0; i < headers.Length; i++)
            {
                WriteUInt32BE(writer, (uint)headers[i].Offset);
                WriteUInt16BE(writer, (ushort)headers[i].Count);
                WriteUInt16BE(writer, (ushort)headers[i].Size);
            }

            return stream.ToArray();
        }

        private TrailSectionHeader WriteManagers(BinaryWriter writer, List<TrailManagerEntry> entries)
        {
            TrailSectionHeader header = new TrailSectionHeader();
            if (entries.Count == 0)
                return header;
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = ManagerSize;
            foreach (TrailManagerEntry entry in entries)
            {
                WriteUInt32BE(writer, (uint)entry.AnimationChunkMapId);
                WriteUInt32BE(writer, (uint)entry.EntryIndex);
                WriteUInt32BE(writer, entry.Unk1);
                WriteUInt32BE(writer, entry.Unk2);
                WriteUInt32BE(writer, entry.Unk3);
                WriteUInt32BE(writer, entry.Lifetime);
                WriteUInt16BE(writer, entry.TrailType);
                WriteUInt16BE(writer, entry.Unk4);
                WriteUInt16BE(writer, FloatToUInt16(entry.Unk5));
                WriteUInt16BE(writer, FloatToUInt16(entry.Unk6));
                WriteSingleBE(writer, entry.ColorRStart);
                WriteSingleBE(writer, entry.ColorGStart);
                WriteSingleBE(writer, entry.ColorBStart);
                WriteSingleBE(writer, entry.ColorAStart);
                WriteSingleBE(writer, entry.ColorRMiddle);
                WriteSingleBE(writer, entry.ColorGMiddle);
                WriteSingleBE(writer, entry.ColorBMiddle);
                WriteSingleBE(writer, entry.ColorAMiddle);
                WriteSingleBE(writer, entry.ColorREnd);
                WriteSingleBE(writer, entry.ColorGEnd);
                WriteSingleBE(writer, entry.ColorBEnd);
                WriteSingleBE(writer, entry.ColorAEnd);
                WriteSingleBE(writer, entry.ColorFactor);
                WriteUInt16BE(writer, FloatToUInt16(entry.ScaleFirstBoneStart / 100f));
                WriteUInt16BE(writer, FloatToUInt16(entry.ScaleSecondBoneStart / 100f));
                WriteUInt16BE(writer, FloatToUInt16(entry.ScaleFirstBoneMiddle / 100f));
                WriteUInt16BE(writer, FloatToUInt16(entry.ScaleSecondBoneMiddle / 100f));
                WriteUInt16BE(writer, FloatToUInt16(entry.ScaleFirstBoneEnd / 100f));
                WriteUInt16BE(writer, FloatToUInt16(entry.ScaleSecondBoneEnd / 100f));
            }
            return header;
        }

        private TrailSectionHeader WriteResources(BinaryWriter writer, List<TrailResourceEntry> entries)
        {
            TrailSectionHeader header = new TrailSectionHeader();
            if (entries.Count == 0)
                return header;
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = ResourceSize;
            foreach (TrailResourceEntry entry in entries)
            {
                WriteUInt32BE(writer, (uint)entry.EffectChunkMapId);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteUInt32BE(writer, entry.Unk1);
                WriteUInt32BE(writer, entry.Unk2);
                WriteUInt32BE(writer, entry.Unk3);
                WriteUInt32BE(writer, entry.Unk4);
                WriteUInt32BE(writer, entry.Unk5);
                WriteUInt32BE(writer, entry.Unk6);
            }
            return header;
        }

        private TrailSectionHeader WritePositions(BinaryWriter writer, List<TrailPositionEntry> entries)
        {
            TrailSectionHeader header = new TrailSectionHeader();
            if (entries.Count == 0)
                return header;
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = PositionSize;
            foreach (TrailPositionEntry entry in entries)
            {
                WriteInt32BE(writer, entry.CoordChunkMapId);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteInt32BE(writer, entry.Unk1);
                WriteInt32BE(writer, entry.Unk2);
                WriteInt32BE(writer, entry.Unk3);
                WriteInt32BE(writer, entry.Unk4);
                WriteInt32BE(writer, entry.Unk5);
                WriteInt32BE(writer, entry.Unk6);
                WriteInt32BE(writer, entry.ClumpChunkMapId);
                WriteInt32BE(writer, entry.Unk7);
                WriteInt32BE(writer, entry.Unk8);
                WriteInt32BE(writer, entry.Unk9);
            }
            return header;
        }

        private TrailSectionHeader WriteForceFields(BinaryWriter writer, List<TrailForceFieldEntry> entries)
        {
            TrailSectionHeader header = new TrailSectionHeader();
            if (entries.Count == 0)
                return header;
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = ForceFieldSize;
            foreach (TrailForceFieldEntry entry in entries)
            {
                WriteInt32BE(writer, entry.Unk1);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteInt32BE(writer, entry.Unk2);
                WriteInt32BE(writer, entry.Unk3);
                WriteUInt16BE(writer, FloatToUInt16(entry.Unk4));
                WriteUInt16BE(writer, FloatToUInt16(entry.Unk5));
                WriteSingleBE(writer, entry.Unk6);
                WriteInt32BE(writer, entry.Unk7);
                WriteInt32BE(writer, entry.Unk8);
                WriteInt32BE(writer, entry.Unk9);
                WriteSingleBE(writer, entry.Unk10);
                WriteSingleBE(writer, entry.Unk11);
                WriteInt32BE(writer, entry.Unk12);
                WriteInt32BE(writer, entry.Unk13);
                WriteInt32BE(writer, entry.Unk14);
                WriteInt32BE(writer, entry.Unk15);
                WriteInt32BE(writer, entry.Unk16);
            }
            return header;
        }

        private TrailSectionHeader WriteNodes(BinaryWriter writer, List<TrailNodeEntry> entries)
        {
            TrailSectionHeader header = new TrailSectionHeader();
            if (entries.Count == 0)
                return header;
            long start = writer.BaseStream.Position;
            header.Offset = (int)start;
            header.Count = entries.Count;
            foreach (TrailNodeEntry entry in entries)
            {
                WriteUInt32BE(writer, entry.Unk1);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteUInt32BE(writer, entry.Unk2);
                WriteUInt32BE(writer, entry.Unk3);
                WriteUInt32BE(writer, (uint)entry.Frames.Count);
                foreach (TrailFrameEntry frame in entry.Frames)
                {
                    WriteUInt16BE(writer, frame.Flag);
                    WriteUInt16BE(writer, (ushort)Math.Max(0, Math.Min(65535, (int)Math.Round(frame.Frame * 100f))));
                }
                if ((entry.Frames.Count & 1) == 0)
                    WriteUInt32BE(writer, 0);
            }
            header.Size = (int)(writer.BaseStream.Position - start);
            return header;
        }

        private XfbinParserPageDefinition BuildPageDefinition(TrailChunkState chunk)
        {
            XfbinParserPageDefinition definition = DeepClone(chunk.OriginalDefinition) ?? new XfbinParserPageDefinition();
            definition.ChunkReferences = definition.ChunkReferences ?? new List<object>();
            definition.ChunkMaps = chunk.MapEntries.Select(x => new XfbinParserChunkMap
            {
                Name = x.Name ?? string.Empty,
                Type = x.Type ?? string.Empty,
                Path = x.Path ?? string.Empty
            }).ToList();

            XfbinParserChunkMap trailMap = definition.ChunkMaps.FirstOrDefault(x => string.Equals(x.Type, TrailChunkType, StringComparison.OrdinalIgnoreCase));
            if (trailMap == null)
            {
                trailMap = new XfbinParserChunkMap();
                definition.ChunkMaps.Add(trailMap);
            }
            trailMap.Name = chunk.ChunkName;
            trailMap.Type = TrailChunkType;
            trailMap.Path = chunk.ChunkPath;

            List<XfbinParserChunkEntry> preservedChunks = new List<XfbinParserChunkEntry>();
            if (definition.Chunks != null)
            {
                preservedChunks.AddRange(definition.Chunks.Where(x =>
                    x != null &&
                    x.Chunk != null &&
                    !string.Equals(x.Chunk.Type, TrailChunkType, StringComparison.OrdinalIgnoreCase))
                    .Select(CloneChunkEntry));
            }

            preservedChunks.Add(new XfbinParserChunkEntry
            {
                FileName = chunk.ChunkName + ".trail",
                Version = 99,
                VersionAttribute = 37494,
                Chunk = new XfbinParserChunkMap
                {
                    Name = chunk.ChunkName,
                    Type = TrailChunkType,
                    Path = chunk.ChunkPath
                }
            });

            definition.Chunks = preservedChunks;
            return definition;
        }

        private static XfbinParserChunkEntry CloneChunkEntry(XfbinParserChunkEntry value)
        {
            return new XfbinParserChunkEntry
            {
                FileName = value.FileName,
                Version = value.Version,
                VersionAttribute = value.VersionAttribute,
                Chunk = value.Chunk == null ? null : new XfbinParserChunkMap
                {
                    Name = value.Chunk.Name,
                    Type = value.Chunk.Type,
                    Path = value.Chunk.Path
                }
            };
        }

        private static T DeepClone<T>(T value)
        {
            if (Equals(value, default(T)))
                return default(T);
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
        }

        private void RefreshChunkCombo()
        {
            suppressChunkSelection = true;
            this.chunkComboBox.Items.Clear();
            if (fileState != null)
                this.chunkComboBox.Items.AddRange(fileState.Chunks.Cast<object>().ToArray());
            suppressChunkSelection = false;
        }

        private void RefreshCurrentChunk()
        {
            TrailChunkState chunk = CurrentChunk;
            suppressChunkFields = true;
            this.chunkNameTextBox.Text = chunk != null ? chunk.ChunkName : string.Empty;
            this.chunkPathTextBox.Text = chunk != null ? chunk.ChunkPath : string.Empty;
            suppressChunkFields = false;

            RefreshListBox(this.managersListBox, chunk != null ? chunk.Managers.Cast<object>().ToList() : null);
            RefreshListBox(this.resourcesListBox, chunk != null ? chunk.Resources.Cast<object>().ToList() : null);
            RefreshListBox(this.positionsListBox, chunk != null ? chunk.Positions.Cast<object>().ToList() : null);
            RefreshListBox(this.forceFieldsListBox, chunk != null ? chunk.ForceFields.Cast<object>().ToList() : null);
            RefreshListBox(this.mapIdsListBox, chunk != null ? chunk.MapEntries.Cast<object>().ToList() : null);
            RefreshListBox(this.nodesListBox, chunk != null ? chunk.Nodes.Cast<object>().ToList() : null);

            if (this.managersListBox.Items.Count > 0 && this.managersListBox.SelectedIndex < 0) this.managersListBox.SelectedIndex = 0;
            if (this.resourcesListBox.Items.Count > 0 && this.resourcesListBox.SelectedIndex < 0) this.resourcesListBox.SelectedIndex = 0;
            if (this.positionsListBox.Items.Count > 0 && this.positionsListBox.SelectedIndex < 0) this.positionsListBox.SelectedIndex = 0;
            if (this.forceFieldsListBox.Items.Count > 0 && this.forceFieldsListBox.SelectedIndex < 0) this.forceFieldsListBox.SelectedIndex = 0;
            if (this.mapIdsListBox.Items.Count > 0 && this.mapIdsListBox.SelectedIndex < 0) this.mapIdsListBox.SelectedIndex = 0;
            if (this.nodesListBox.Items.Count > 0 && this.nodesListBox.SelectedIndex < 0) this.nodesListBox.SelectedIndex = 0;

            RefreshFrames();
            RefreshPropertyGrids();
            UpdateUiState();
        }

        private void RefreshFrames()
        {
            TrailNodeEntry node = CurrentNode;
            RefreshListBox(this.framesListBox, node != null ? node.Frames.Cast<object>().ToList() : null);
            if (this.framesListBox.Items.Count > 0 && this.framesListBox.SelectedIndex < 0)
                this.framesListBox.SelectedIndex = 0;
            if (this.framesListBox.Items.Count == 0)
                this.framesPropertyGrid.SelectedObject = null;
        }

        private static void RefreshListBox(ListBox listBox, List<object> items)
        {
            object selected = listBox.SelectedItem;
            listBox.BeginUpdate();
            listBox.Items.Clear();
            if (items != null && items.Count > 0)
                listBox.Items.AddRange(items.ToArray());
            if (selected != null)
            {
                int selectedIndex = listBox.Items.IndexOf(selected);
                if (selectedIndex >= 0)
                    listBox.SelectedIndex = selectedIndex;
            }
            listBox.EndUpdate();
        }

        private void RefreshPropertyGrids()
        {
            TrailChunkState chunk = CurrentChunk;
            this.managersPropertyGrid.SelectedObject = chunk != null && this.managersListBox.SelectedItem is TrailManagerEntry ? new TrailManagerViewModel(this, chunk, (TrailManagerEntry)this.managersListBox.SelectedItem) : null;
            this.resourcesPropertyGrid.SelectedObject = chunk != null && this.resourcesListBox.SelectedItem is TrailResourceEntry ? new TrailResourceViewModel(this, chunk, (TrailResourceEntry)this.resourcesListBox.SelectedItem) : null;
            this.positionsPropertyGrid.SelectedObject = chunk != null && this.positionsListBox.SelectedItem is TrailPositionEntry ? new TrailPositionViewModel(this, chunk, (TrailPositionEntry)this.positionsListBox.SelectedItem) : null;
            this.forceFieldsPropertyGrid.SelectedObject = chunk != null && this.forceFieldsListBox.SelectedItem is TrailForceFieldEntry ? new TrailForceFieldViewModel(this, chunk, (TrailForceFieldEntry)this.forceFieldsListBox.SelectedItem) : null;
            this.mapIdsPropertyGrid.SelectedObject = chunk != null && this.mapIdsListBox.SelectedItem is TrailMapEntry ? new TrailMapViewModel((TrailMapEntry)this.mapIdsListBox.SelectedItem) : null;
            this.nodesPropertyGrid.SelectedObject = chunk != null && this.nodesListBox.SelectedItem is TrailNodeEntry ? new TrailNodeViewModel(this, chunk, (TrailNodeEntry)this.nodesListBox.SelectedItem) : null;
            this.framesPropertyGrid.SelectedObject = chunk != null && this.framesListBox.SelectedItem is TrailFrameEntry ? new TrailFrameViewModel((TrailFrameEntry)this.framesListBox.SelectedItem) : null;
        }

        private void UpdateUiState()
        {
            bool hasFile = fileState != null && CurrentChunk != null;
            this.chunkComboBox.Enabled = hasFile;
            this.chunkNameTextBox.Enabled = hasFile;
            this.chunkPathTextBox.Enabled = hasFile;
            this.saveToolStripMenuItem.Enabled = fileState != null;
            this.saveAsToolStripMenuItem.Enabled = fileState != null;
            this.closeToolStripMenuItem.Enabled = fileState != null;
        }

        private void chunkComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressChunkSelection)
                return;
            RefreshCurrentChunk();
        }

        private void chunkNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressChunkFields || CurrentChunk == null)
                return;
            CurrentChunk.ChunkName = this.chunkNameTextBox.Text.Trim();
            int index = this.chunkComboBox.SelectedIndex;
            RefreshChunkCombo();
            if (index >= 0 && index < this.chunkComboBox.Items.Count)
                this.chunkComboBox.SelectedIndex = index;
            RefreshAllDisplays();
        }

        private void chunkPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressChunkFields || CurrentChunk == null)
                return;
            CurrentChunk.ChunkPath = this.chunkPathTextBox.Text.Trim();
            RefreshAllDisplays();
        }

        private void RefreshAllDisplays()
        {
            this.chunkComboBox.Refresh();
            this.managersListBox.Refresh();
            this.resourcesListBox.Refresh();
            this.positionsListBox.Refresh();
            this.forceFieldsListBox.Refresh();
            this.mapIdsListBox.Refresh();
            this.nodesListBox.Refresh();
            this.framesListBox.Refresh();
            this.managersPropertyGrid.Refresh();
            this.resourcesPropertyGrid.Refresh();
            this.positionsPropertyGrid.Refresh();
            this.forceFieldsPropertyGrid.Refresh();
            this.mapIdsPropertyGrid.Refresh();
            this.nodesPropertyGrid.Refresh();
            this.framesPropertyGrid.Refresh();
        }

        private void AnyPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RefreshAllDisplays();
        }

        private string GetChunkDisplay(TrailChunkState chunk)
        {
            return string.Format("{0} [{1}]", chunk.ChunkName, chunk.ChunkPath);
        }

        private string GetMapDisplay(TrailChunkState chunk, int index)
        {
            if (index < 0)
                return "-1: None";
            if (chunk == null || index >= chunk.MapEntries.Count)
                return string.Format("{0}: <invalid>", index);
            TrailMapEntry entry = chunk.MapEntries[index];
            return string.Format("{0}: {1} ({2})", index, string.IsNullOrWhiteSpace(entry.Name) ? "<unnamed>" : entry.Name, string.IsNullOrWhiteSpace(entry.Type) ? "unknown" : entry.Type);
        }

        private string GetManagerDisplay(TrailChunkState chunk, int index)
        {
            if (index < 0)
                return "-1: None";
            if (chunk == null || index >= chunk.Managers.Count)
                return string.Format("{0}: <invalid>", index);
            TrailManagerEntry manager = chunk.Managers[index];
            return string.Format("{0}: Trail {1} -> {2}", index, manager.EntryIndex, GetMapDisplay(chunk, manager.AnimationChunkMapId));
        }

        private void chunkComboBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailChunkState ? GetChunkDisplay((TrailChunkState)e.ListItem) : string.Empty; }
        private void managersListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailManagerEntry ? GetManagerDisplay(CurrentChunk, CurrentChunk.Managers.IndexOf((TrailManagerEntry)e.ListItem)) : string.Empty; }
        private void resourcesListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailResourceEntry ? string.Format("#{0}: {1}", CurrentChunk.Resources.IndexOf((TrailResourceEntry)e.ListItem), GetMapDisplay(CurrentChunk, ((TrailResourceEntry)e.ListItem).EffectChunkMapId)) : string.Empty; }
        private void positionsListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailPositionEntry ? string.Format("#{0}: Coord {1}", CurrentChunk.Positions.IndexOf((TrailPositionEntry)e.ListItem), GetMapDisplay(CurrentChunk, ((TrailPositionEntry)e.ListItem).CoordChunkMapId)) : string.Empty; }
        private void forceFieldsListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailForceFieldEntry ? string.Format("#{0}: Trail {1}", CurrentChunk.ForceFields.IndexOf((TrailForceFieldEntry)e.ListItem), ((TrailForceFieldEntry)e.ListItem).TrailEntryIndex) : string.Empty; }
        private void mapIdsListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailMapEntry ? GetMapDisplay(CurrentChunk, CurrentChunk.MapEntries.IndexOf((TrailMapEntry)e.ListItem)) : string.Empty; }
        private void nodesListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailNodeEntry ? string.Format("#{0}: Trail {1} ({2} frames)", CurrentChunk.Nodes.IndexOf((TrailNodeEntry)e.ListItem), ((TrailNodeEntry)e.ListItem).TrailEntryIndex, ((TrailNodeEntry)e.ListItem).Frames.Count) : string.Empty; }
        private void framesListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailFrameEntry ? string.Format("#{0}: Flag {1} @ {2:0.00}", CurrentNode != null ? CurrentNode.Frames.IndexOf((TrailFrameEntry)e.ListItem) : -1, ((TrailFrameEntry)e.ListItem).Flag, ((TrailFrameEntry)e.ListItem).Frame) : string.Empty; }

        private void managersListBox_SelectedIndexChanged(object sender, EventArgs e) { this.managersPropertyGrid.SelectedObject = CurrentChunk != null && this.managersListBox.SelectedItem is TrailManagerEntry ? new TrailManagerViewModel(this, CurrentChunk, (TrailManagerEntry)this.managersListBox.SelectedItem) : null; }
        private void resourcesListBox_SelectedIndexChanged(object sender, EventArgs e) { this.resourcesPropertyGrid.SelectedObject = CurrentChunk != null && this.resourcesListBox.SelectedItem is TrailResourceEntry ? new TrailResourceViewModel(this, CurrentChunk, (TrailResourceEntry)this.resourcesListBox.SelectedItem) : null; }
        private void positionsListBox_SelectedIndexChanged(object sender, EventArgs e) { this.positionsPropertyGrid.SelectedObject = CurrentChunk != null && this.positionsListBox.SelectedItem is TrailPositionEntry ? new TrailPositionViewModel(this, CurrentChunk, (TrailPositionEntry)this.positionsListBox.SelectedItem) : null; }
        private void forceFieldsListBox_SelectedIndexChanged(object sender, EventArgs e) { this.forceFieldsPropertyGrid.SelectedObject = CurrentChunk != null && this.forceFieldsListBox.SelectedItem is TrailForceFieldEntry ? new TrailForceFieldViewModel(this, CurrentChunk, (TrailForceFieldEntry)this.forceFieldsListBox.SelectedItem) : null; }
        private void mapIdsListBox_SelectedIndexChanged(object sender, EventArgs e) { this.mapIdsPropertyGrid.SelectedObject = CurrentChunk != null && this.mapIdsListBox.SelectedItem is TrailMapEntry ? new TrailMapViewModel((TrailMapEntry)this.mapIdsListBox.SelectedItem) : null; }
        private void nodesListBox_SelectedIndexChanged(object sender, EventArgs e) { this.nodesPropertyGrid.SelectedObject = CurrentChunk != null && this.nodesListBox.SelectedItem is TrailNodeEntry ? new TrailNodeViewModel(this, CurrentChunk, (TrailNodeEntry)this.nodesListBox.SelectedItem) : null; RefreshFrames(); }
        private void framesListBox_SelectedIndexChanged(object sender, EventArgs e) { this.framesPropertyGrid.SelectedObject = CurrentChunk != null && this.framesListBox.SelectedItem is TrailFrameEntry ? new TrailFrameViewModel((TrailFrameEntry)this.framesListBox.SelectedItem) : null; }

        private void managersPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }
        private void resourcesPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }
        private void positionsPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }
        private void forceFieldsPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }
        private void mapIdsPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }
        private void nodesPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }
        private void framesPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) { }

        private void AddManagerButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null) return;
            TrailManagerEntry entry = new TrailManagerEntry();
            CurrentChunk.Managers.Add(entry);
            RefreshCurrentChunk();
            this.managersListBox.SelectedItem = entry;
        }

        private void DuplicateManagerButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.managersListBox.SelectedItem is TrailManagerEntry)) return;
            TrailManagerEntry clone = ((TrailManagerEntry)this.managersListBox.SelectedItem).Clone();
            int index = CurrentChunk.Managers.IndexOf((TrailManagerEntry)this.managersListBox.SelectedItem) + 1;
            CurrentChunk.Managers.Insert(index, clone);
            RefreshCurrentChunk();
            this.managersListBox.SelectedItem = clone;
        }

        private void DeleteManagerButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.managersListBox.SelectedItem is TrailManagerEntry)) return;
            int index = this.managersListBox.SelectedIndex;
            CurrentChunk.Managers.RemoveAt(index);
            RemapTrailEntryReferences(index, -1);
            RefreshCurrentChunk();
        }

        private void AddResourceButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null) return;
            TrailResourceEntry entry = new TrailResourceEntry();
            CurrentChunk.Resources.Add(entry);
            RefreshCurrentChunk();
            this.resourcesListBox.SelectedItem = entry;
        }

        private void DuplicateResourceButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.resourcesListBox.SelectedItem is TrailResourceEntry)) return;
            TrailResourceEntry clone = ((TrailResourceEntry)this.resourcesListBox.SelectedItem).Clone();
            CurrentChunk.Resources.Insert(this.resourcesListBox.SelectedIndex + 1, clone);
            RefreshCurrentChunk();
            this.resourcesListBox.SelectedItem = clone;
        }

        private void DeleteResourceButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.resourcesListBox.SelectedIndex < 0) return;
            CurrentChunk.Resources.RemoveAt(this.resourcesListBox.SelectedIndex);
            RefreshCurrentChunk();
        }

        private void AddPositionButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null) return;
            TrailPositionEntry entry = new TrailPositionEntry();
            CurrentChunk.Positions.Add(entry);
            RefreshCurrentChunk();
            this.positionsListBox.SelectedItem = entry;
        }

        private void DuplicatePositionButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.positionsListBox.SelectedItem is TrailPositionEntry)) return;
            TrailPositionEntry clone = ((TrailPositionEntry)this.positionsListBox.SelectedItem).Clone();
            CurrentChunk.Positions.Insert(this.positionsListBox.SelectedIndex + 1, clone);
            RefreshCurrentChunk();
            this.positionsListBox.SelectedItem = clone;
        }

        private void DeletePositionButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.positionsListBox.SelectedIndex < 0) return;
            CurrentChunk.Positions.RemoveAt(this.positionsListBox.SelectedIndex);
            RefreshCurrentChunk();
        }

        private void AddForceFieldButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null) return;
            TrailForceFieldEntry entry = new TrailForceFieldEntry();
            CurrentChunk.ForceFields.Add(entry);
            RefreshCurrentChunk();
            this.forceFieldsListBox.SelectedItem = entry;
        }

        private void DuplicateForceFieldButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.forceFieldsListBox.SelectedItem is TrailForceFieldEntry)) return;
            TrailForceFieldEntry clone = ((TrailForceFieldEntry)this.forceFieldsListBox.SelectedItem).Clone();
            CurrentChunk.ForceFields.Insert(this.forceFieldsListBox.SelectedIndex + 1, clone);
            RefreshCurrentChunk();
            this.forceFieldsListBox.SelectedItem = clone;
        }

        private void DeleteForceFieldButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.forceFieldsListBox.SelectedIndex < 0) return;
            CurrentChunk.ForceFields.RemoveAt(this.forceFieldsListBox.SelectedIndex);
            RefreshCurrentChunk();
        }

        private void AddMapButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null) return;
            TrailMapEntry entry = new TrailMapEntry { Name = "NewMap", Type = "nuccChunkCoord", Path = string.Empty };
            CurrentChunk.MapEntries.Add(entry);
            RefreshCurrentChunk();
            this.mapIdsListBox.SelectedItem = entry;
        }

        private void DuplicateMapButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.mapIdsListBox.SelectedItem is TrailMapEntry)) return;
            int sourceIndex = this.mapIdsListBox.SelectedIndex;
            int newIndex = sourceIndex + 1;
            TrailMapEntry clone = ((TrailMapEntry)this.mapIdsListBox.SelectedItem).Clone();
            CurrentChunk.MapEntries.Insert(newIndex, clone);
            ShiftMapReferences(newIndex, 1);
            RefreshCurrentChunk();
            this.mapIdsListBox.SelectedItem = clone;
        }

        private void DeleteMapButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.mapIdsListBox.SelectedIndex < 0) return;
            int index = this.mapIdsListBox.SelectedIndex;
            CurrentChunk.MapEntries.RemoveAt(index);
            RemapDeletedMapReference(index);
            RefreshCurrentChunk();
        }

        private void AddNodeButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null) return;
            TrailNodeEntry entry = new TrailNodeEntry();
            CurrentChunk.Nodes.Add(entry);
            RefreshCurrentChunk();
            this.nodesListBox.SelectedItem = entry;
        }

        private void DuplicateNodeButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || !(this.nodesListBox.SelectedItem is TrailNodeEntry)) return;
            TrailNodeEntry clone = ((TrailNodeEntry)this.nodesListBox.SelectedItem).Clone();
            CurrentChunk.Nodes.Insert(this.nodesListBox.SelectedIndex + 1, clone);
            RefreshCurrentChunk();
            this.nodesListBox.SelectedItem = clone;
        }

        private void DeleteNodeButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.nodesListBox.SelectedIndex < 0) return;
            int index = this.nodesListBox.SelectedIndex;
            CurrentChunk.Nodes.RemoveAt(index);
            RefreshCurrentChunk();
        }

        private void AddFrameButton_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;
            TrailFrameEntry entry = new TrailFrameEntry();
            CurrentNode.Frames.Add(entry);
            RefreshFrames();
            this.framesListBox.SelectedItem = entry;
            RefreshAllDisplays();
        }

        private void DuplicateFrameButton_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null || !(this.framesListBox.SelectedItem is TrailFrameEntry)) return;
            TrailFrameEntry clone = ((TrailFrameEntry)this.framesListBox.SelectedItem).Clone();
            CurrentNode.Frames.Insert(this.framesListBox.SelectedIndex + 1, clone);
            RefreshFrames();
            this.framesListBox.SelectedItem = clone;
            RefreshAllDisplays();
        }

        private void DeleteFrameButton_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null || this.framesListBox.SelectedIndex < 0) return;
            CurrentNode.Frames.RemoveAt(this.framesListBox.SelectedIndex);
            RefreshFrames();
            RefreshAllDisplays();
        }

        private void ShiftMapReferences(int startIndex, int delta)
        {
            foreach (TrailManagerEntry entry in CurrentChunk.Managers)
                if (entry.AnimationChunkMapId >= startIndex) entry.AnimationChunkMapId += delta;
            foreach (TrailResourceEntry entry in CurrentChunk.Resources)
                if (entry.EffectChunkMapId >= startIndex) entry.EffectChunkMapId += delta;
            foreach (TrailPositionEntry entry in CurrentChunk.Positions)
            {
                if (entry.CoordChunkMapId >= startIndex) entry.CoordChunkMapId += delta;
                if (entry.ClumpChunkMapId >= startIndex) entry.ClumpChunkMapId += delta;
            }
        }

        private void RemapDeletedMapReference(int removedIndex)
        {
            foreach (TrailManagerEntry entry in CurrentChunk.Managers)
                entry.AnimationChunkMapId = RemapDeletedIndex(entry.AnimationChunkMapId, removedIndex);
            foreach (TrailResourceEntry entry in CurrentChunk.Resources)
                entry.EffectChunkMapId = RemapDeletedIndex(entry.EffectChunkMapId, removedIndex);
            foreach (TrailPositionEntry entry in CurrentChunk.Positions)
            {
                entry.CoordChunkMapId = RemapDeletedIndex(entry.CoordChunkMapId, removedIndex);
                entry.ClumpChunkMapId = RemapDeletedIndex(entry.ClumpChunkMapId, removedIndex);
            }
        }

        private void RemapTrailEntryReferences(int removedIndex, int replacement)
        {
            foreach (TrailResourceEntry entry in CurrentChunk.Resources)
                entry.TrailEntryIndex = RemapDeletedIndex(entry.TrailEntryIndex, removedIndex, replacement);
            foreach (TrailPositionEntry entry in CurrentChunk.Positions)
                entry.TrailEntryIndex = RemapDeletedIndex(entry.TrailEntryIndex, removedIndex, replacement);
            foreach (TrailForceFieldEntry entry in CurrentChunk.ForceFields)
                entry.TrailEntryIndex = RemapDeletedIndex(entry.TrailEntryIndex, removedIndex, replacement);
            foreach (TrailNodeEntry entry in CurrentChunk.Nodes)
                entry.TrailEntryIndex = RemapDeletedIndex(entry.TrailEntryIndex, removedIndex, replacement);
        }

        private static int RemapDeletedIndex(int value, int removedIndex, int replacement = -1)
        {
            if (value == removedIndex)
                return replacement;
            if (value > removedIndex)
                return value - 1;
            return value;
        }

        private interface IChunkViewModel
        {
            Tool_TrailEditor Editor { get; }
            TrailChunkState Chunk { get; }
        }

        private sealed class TrailMapViewModel
        {
            private readonly TrailMapEntry entry;
            public TrailMapViewModel(TrailMapEntry entry) { this.entry = entry; }
            [Category("Map")] public string Name { get { return entry.Name; } set { entry.Name = value ?? string.Empty; } }
            [Category("Map")] public string Type { get { return entry.Type; } set { entry.Type = value ?? string.Empty; } }
            [Category("Map")] public string Path { get { return entry.Path; } set { entry.Path = value ?? string.Empty; } }
        }

        private sealed class TrailManagerViewModel : IChunkViewModel
        {
            private readonly TrailManagerEntry entry;
            public TrailManagerViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailManagerEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            public Tool_TrailEditor Editor { get; private set; }
            public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int AnimationMap { get { return entry.AnimationChunkMapId; } set { entry.AnimationChunkMapId = value; } }
            [Category("Main")] public int EntryIndex { get { return entry.EntryIndex; } set { entry.EntryIndex = value; } }
            [Category("Main")] public uint Lifetime { get { return entry.Lifetime; } set { entry.Lifetime = value; } }
            [Category("Main")] public ushort TrailType { get { return entry.TrailType; } set { entry.TrailType = value; } }
            [Category("Unknown")] public uint Unk1 { get { return entry.Unk1; } set { entry.Unk1 = value; } }
            [Category("Unknown")] public uint Unk2 { get { return entry.Unk2; } set { entry.Unk2 = value; } }
            [Category("Unknown")] public uint Unk3 { get { return entry.Unk3; } set { entry.Unk3 = value; } }
            [Category("Unknown")] public ushort Unk4 { get { return entry.Unk4; } set { entry.Unk4 = value; } }
            [Category("Unknown")] public float Unk5 { get { return entry.Unk5; } set { entry.Unk5 = value; } }
            [Category("Unknown")] public float Unk6 { get { return entry.Unk6; } set { entry.Unk6 = value; } }
            [Category("Start Color")] public float ColorRStart { get { return entry.ColorRStart; } set { entry.ColorRStart = value; } }
            [Category("Start Color")] public float ColorGStart { get { return entry.ColorGStart; } set { entry.ColorGStart = value; } }
            [Category("Start Color")] public float ColorBStart { get { return entry.ColorBStart; } set { entry.ColorBStart = value; } }
            [Category("Start Color")] public float ColorAStart { get { return entry.ColorAStart; } set { entry.ColorAStart = value; } }
            [Category("Middle Color")] public float ColorRMiddle { get { return entry.ColorRMiddle; } set { entry.ColorRMiddle = value; } }
            [Category("Middle Color")] public float ColorGMiddle { get { return entry.ColorGMiddle; } set { entry.ColorGMiddle = value; } }
            [Category("Middle Color")] public float ColorBMiddle { get { return entry.ColorBMiddle; } set { entry.ColorBMiddle = value; } }
            [Category("Middle Color")] public float ColorAMiddle { get { return entry.ColorAMiddle; } set { entry.ColorAMiddle = value; } }
            [Category("End Color")] public float ColorREnd { get { return entry.ColorREnd; } set { entry.ColorREnd = value; } }
            [Category("End Color")] public float ColorGEnd { get { return entry.ColorGEnd; } set { entry.ColorGEnd = value; } }
            [Category("End Color")] public float ColorBEnd { get { return entry.ColorBEnd; } set { entry.ColorBEnd = value; } }
            [Category("End Color")] public float ColorAEnd { get { return entry.ColorAEnd; } set { entry.ColorAEnd = value; } }
            [Category("Other")] public float ColorFactor { get { return entry.ColorFactor; } set { entry.ColorFactor = value; } }
            [Category("Width")] public float ScaleFirstBoneStart { get { return entry.ScaleFirstBoneStart; } set { entry.ScaleFirstBoneStart = value; } }
            [Category("Width")] public float ScaleSecondBoneStart { get { return entry.ScaleSecondBoneStart; } set { entry.ScaleSecondBoneStart = value; } }
            [Category("Width")] public float ScaleFirstBoneMiddle { get { return entry.ScaleFirstBoneMiddle; } set { entry.ScaleFirstBoneMiddle = value; } }
            [Category("Width")] public float ScaleSecondBoneMiddle { get { return entry.ScaleSecondBoneMiddle; } set { entry.ScaleSecondBoneMiddle = value; } }
            [Category("Width")] public float ScaleFirstBoneEnd { get { return entry.ScaleFirstBoneEnd; } set { entry.ScaleFirstBoneEnd = value; } }
            [Category("Width")] public float ScaleSecondBoneEnd { get { return entry.ScaleSecondBoneEnd; } set { entry.ScaleSecondBoneEnd = value; } }
        }

        private sealed class TrailResourceViewModel : IChunkViewModel
        {
            private readonly TrailResourceEntry entry;
            public TrailResourceViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailResourceEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            public Tool_TrailEditor Editor { get; private set; }
            public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int EffectMap { get { return entry.EffectChunkMapId; } set { entry.EffectChunkMapId = value; } }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntry { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Unknown")] public uint Unk1 { get { return entry.Unk1; } set { entry.Unk1 = value; } }
            [Category("Unknown")] public uint Unk2 { get { return entry.Unk2; } set { entry.Unk2 = value; } }
            [Category("Unknown")] public uint Unk3 { get { return entry.Unk3; } set { entry.Unk3 = value; } }
            [Category("Unknown")] public uint Unk4 { get { return entry.Unk4; } set { entry.Unk4 = value; } }
            [Category("Unknown")] public uint Unk5 { get { return entry.Unk5; } set { entry.Unk5 = value; } }
            [Category("Unknown")] public uint Unk6 { get { return entry.Unk6; } set { entry.Unk6 = value; } }
        }

        private sealed class TrailPositionViewModel : IChunkViewModel
        {
            private readonly TrailPositionEntry entry;
            public TrailPositionViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailPositionEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            public Tool_TrailEditor Editor { get; private set; }
            public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int CoordMap { get { return entry.CoordChunkMapId; } set { entry.CoordChunkMapId = value; } }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int ClumpMap { get { return entry.ClumpChunkMapId; } set { entry.ClumpChunkMapId = value; } }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntry { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Unknown")] public int Unk1 { get { return entry.Unk1; } set { entry.Unk1 = value; } }
            [Category("Unknown")] public int Unk2 { get { return entry.Unk2; } set { entry.Unk2 = value; } }
            [Category("Unknown")] public int Unk3 { get { return entry.Unk3; } set { entry.Unk3 = value; } }
            [Category("Unknown")] public int Unk4 { get { return entry.Unk4; } set { entry.Unk4 = value; } }
            [Category("Unknown")] public int Unk5 { get { return entry.Unk5; } set { entry.Unk5 = value; } }
            [Category("Unknown")] public int Unk6 { get { return entry.Unk6; } set { entry.Unk6 = value; } }
            [Category("Unknown")] public int Unk7 { get { return entry.Unk7; } set { entry.Unk7 = value; } }
            [Category("Unknown")] public int Unk8 { get { return entry.Unk8; } set { entry.Unk8 = value; } }
            [Category("Unknown")] public int Unk9 { get { return entry.Unk9; } set { entry.Unk9 = value; } }
        }

        private sealed class TrailForceFieldViewModel : IChunkViewModel
        {
            private readonly TrailForceFieldEntry entry;
            public TrailForceFieldViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailForceFieldEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            public Tool_TrailEditor Editor { get; private set; }
            public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntry { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Unknown")] public int Unk1 { get { return entry.Unk1; } set { entry.Unk1 = value; } }
            [Category("Unknown")] public int Unk2 { get { return entry.Unk2; } set { entry.Unk2 = value; } }
            [Category("Unknown")] public int Unk3 { get { return entry.Unk3; } set { entry.Unk3 = value; } }
            [Category("Unknown")] public float Unk4 { get { return entry.Unk4; } set { entry.Unk4 = value; } }
            [Category("Unknown")] public float Unk5 { get { return entry.Unk5; } set { entry.Unk5 = value; } }
            [Category("Unknown")] public float Unk6 { get { return entry.Unk6; } set { entry.Unk6 = value; } }
            [Category("Unknown")] public int Unk7 { get { return entry.Unk7; } set { entry.Unk7 = value; } }
            [Category("Unknown")] public int Unk8 { get { return entry.Unk8; } set { entry.Unk8 = value; } }
            [Category("Unknown")] public int Unk9 { get { return entry.Unk9; } set { entry.Unk9 = value; } }
            [Category("Unknown")] public float Unk10 { get { return entry.Unk10; } set { entry.Unk10 = value; } }
            [Category("Unknown")] public float Unk11 { get { return entry.Unk11; } set { entry.Unk11 = value; } }
            [Category("Unknown")] public int Unk12 { get { return entry.Unk12; } set { entry.Unk12 = value; } }
            [Category("Unknown")] public int Unk13 { get { return entry.Unk13; } set { entry.Unk13 = value; } }
            [Category("Unknown")] public int Unk14 { get { return entry.Unk14; } set { entry.Unk14 = value; } }
            [Category("Unknown")] public int Unk15 { get { return entry.Unk15; } set { entry.Unk15 = value; } }
            [Category("Unknown")] public int Unk16 { get { return entry.Unk16; } set { entry.Unk16 = value; } }
        }

        private sealed class TrailNodeViewModel : IChunkViewModel
        {
            private readonly TrailNodeEntry entry;
            public TrailNodeViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailNodeEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            public Tool_TrailEditor Editor { get; private set; }
            public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntry { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Main")] public int FrameCount { get { return entry.Frames.Count; } }
            [Category("Unknown")] public uint Unk1 { get { return entry.Unk1; } set { entry.Unk1 = value; } }
            [Category("Unknown")] public uint Unk2 { get { return entry.Unk2; } set { entry.Unk2 = value; } }
            [Category("Unknown")] public uint Unk3 { get { return entry.Unk3; } set { entry.Unk3 = value; } }
        }

        private sealed class TrailFrameViewModel
        {
            private readonly TrailFrameEntry entry;
            public TrailFrameViewModel(TrailFrameEntry entry) { this.entry = entry; }
            [Category("Main")] public ushort Flag { get { return entry.Flag; } set { entry.Flag = value; } }
            [Category("Main")] public float Frame { get { return entry.Frame; } set { entry.Frame = value; } }
        }

        private sealed class MapReferenceConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return false; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                IChunkViewModel vm = context != null ? context.Instance as IChunkViewModel : null;
                List<int> values = new List<int> { -1 };
                if (vm != null) values.AddRange(Enumerable.Range(0, vm.Chunk.MapEntries.Count));
                return new StandardValuesCollection(values);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is int && context != null && context.Instance is IChunkViewModel)
                {
                    IChunkViewModel vm = (IChunkViewModel)context.Instance;
                    return vm.Editor.GetMapDisplay(vm.Chunk, (int)value);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    int parsed;
                    if (int.TryParse(((string)value).Split(':')[0], out parsed))
                        return parsed;
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        private sealed class ManagerReferenceConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return false; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                IChunkViewModel vm = context != null ? context.Instance as IChunkViewModel : null;
                List<int> values = new List<int> { -1 };
                if (vm != null) values.AddRange(Enumerable.Range(0, vm.Chunk.Managers.Count));
                return new StandardValuesCollection(values);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is int && context != null && context.Instance is IChunkViewModel)
                {
                    IChunkViewModel vm = (IChunkViewModel)context.Instance;
                    return vm.Editor.GetManagerDisplay(vm.Chunk, (int)value);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    int parsed;
                    if (int.TryParse(((string)value).Split(':')[0], out parsed))
                        return parsed;
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        private struct TrailSectionHeader
        {
            public int Offset;
            public int Count;
            public int Size;
        }

        private static bool ValidateSectionBounds(byte[] data, TrailSectionHeader header)
        {
            if (header.Count == 0)
                return true;
            if (header.Offset < HeaderCount * HeaderSize || header.Offset >= data.Length)
                return false;
            return true;
        }

        private static ushort FloatToUInt16(float value)
        {
            value = Math.Max(0f, Math.Min(1f, value));
            return (ushort)Math.Round(value * 65535f);
        }

        private static uint ReadUInt32BE(byte[] data, int offset)
        {
            return (uint)((data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3]);
        }

        private static int ReadInt32BE(byte[] data, int offset)
        {
            return unchecked((int)ReadUInt32BE(data, offset));
        }

        private static ushort ReadUInt16BE(byte[] data, int offset)
        {
            return (ushort)((data[offset] << 8) | data[offset + 1]);
        }

        private static float ReadSingleBE(byte[] data, int offset)
        {
            byte[] bytes = new byte[4];
            bytes[0] = data[offset + 3];
            bytes[1] = data[offset + 2];
            bytes[2] = data[offset + 1];
            bytes[3] = data[offset];
            return BitConverter.ToSingle(bytes, 0);
        }

        private static void WriteUInt32BE(BinaryWriter writer, uint value)
        {
            writer.Write((byte)(value >> 24));
            writer.Write((byte)(value >> 16));
            writer.Write((byte)(value >> 8));
            writer.Write((byte)value);
        }

        private static void WriteInt32BE(BinaryWriter writer, int value)
        {
            WriteUInt32BE(writer, unchecked((uint)value));
        }

        private static void WriteUInt16BE(BinaryWriter writer, ushort value)
        {
            writer.Write((byte)(value >> 8));
            writer.Write((byte)value);
        }

        private static void WriteSingleBE(BinaryWriter writer, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            writer.Write(bytes[3]);
            writer.Write(bytes[2]);
            writer.Write(bytes[1]);
            writer.Write(bytes[0]);
        }
    }
}
