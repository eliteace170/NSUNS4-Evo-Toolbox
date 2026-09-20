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
    [Flags]
    public enum TrailFlags : ushort
    {
        NoFade = 0x00,
        FadeAlpha = 0x01,
        FadeWidth = 0x02,
        FadeBoth = 0x03,
        WidthProfile = 0x10,
        ProfileFadeAlpha = 0x11,
        ProfileFadeWidth = 0x12,
        ProfileFadeBoth = 0x13
    }

    public enum TrailForceType : uint
    {
        NoForce = 0,
        Directional = 1
    }

    [Flags]
    public enum TrailForceFlags : uint
    {
        DefaultForce = 0x00,
        HalfStrength = 0x01,
        InwardWeight = 0x02,
        OutwardWeight = 0x04,
        WorldDirection = 0x10,
        WorldHalfStrength = 0x11,
        WorldInward = 0x12,
        WorldOutward = 0x14
    }

    public partial class Tool_TrailEditor : Form
    {
        private const string ClipboardPrefix = "NS4_TRAIL_EDITOR_ENTRY_V2:";
        private const string TrailChunkType = "nuccChunkTrail";
        private const int HeaderCount = 5;
        private const int HeaderSize = 8;
        private const int ManagerSize = 0x60;
        private const int ResourceSize = 0x20;
        private const int PositionSize = 0x30;
        private const int ForceFieldSize = 0x40;
        private const int NodeBaseSize = 0x14;

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

        private TrailManagerEntry CurrentManager
        {
            get { return this.managersListBox.SelectedItem as TrailManagerEntry; }
        }

        private TrailResourceEntry CurrentResource
        {
            get { return this.resourcesListBox.SelectedItem as TrailResourceEntry; }
        }

        private TrailPositionEntry CurrentPosition
        {
            get { return this.positionsListBox.SelectedItem as TrailPositionEntry; }
        }

        private TrailForceFieldEntry CurrentForceField
        {
            get { return this.forceFieldsListBox.SelectedItem as TrailForceFieldEntry; }
        }

        private TrailMapEntry CurrentMapEntry
        {
            get { return this.mapIdsListBox.SelectedItem as TrailMapEntry; }
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

        private sealed class TrailClipboardPayload
        {
            public string EntryType;
            public string Json;
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
            [Category("Parameters"), Description("Offset 0x00 (u32). ")]
            public int AnimationChunkMapId { get; set; }
            [Category("Links"), Description("Offset 0x04 (u32). Matches resources, positions and forces; timelines use array order.")]
            public int EntryIndex { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x08 (u32). Replaced on load: resolved AnimationChunkIndex pointer.")]
            public uint AnimationRef { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x0C (u32). Unused by game loader: skipped prefix word.")]
            public uint Field0C { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x10 (u32). Unknown: copied; no use found in traced NX/S4/Connections trail code.")]
            public uint Field10 { get; set; }
            [Category("Parameters"), Description("Offset 0x14 (u32). History length in 30 FPS frames.")]
            public uint Lifetime { get; set; }
            [Category("Parameters"), Description("Offset 0x18 (u32). Extra samples used to smooth the trail.")]
            public uint Subdivisions { get; set; }
            [Category("Parameters"), Description("Offset 0x1C (u16). Named bits are used; other bits have no confirmed game use.")]
            public TrailFlags TrailFlags { get; set; }
            [Category("Parameters"), Description("Offset 0x1E (u8). Unused without FadeAlpha bit; otherwise fade per update after stop, raw / 255. Stored raw; divide by 255 for the game value.")]
            public byte AlphaFade { get; set; }
            [Category("Parameters"), Description("Offset 0x1F (u8). Unused without FadeWidth bit; otherwise shrink per update after stop, raw / 255. Stored raw; divide by 255 for the game value.")]
            public byte WidthFade { get; set; }
            [Category("Parameters"), Description("Offset 0x20 (f32). ")]
            public float ColorStartR { get; set; }
            [Category("Parameters"), Description("Offset 0x24 (f32). ")]
            public float ColorStartG { get; set; }
            [Category("Parameters"), Description("Offset 0x28 (f32). ")]
            public float ColorStartB { get; set; }
            [Category("Parameters"), Description("Offset 0x2C (f32). ")]
            public float ColorStartA { get; set; }
            [Category("Parameters"), Description("Offset 0x30 (f32). ")]
            public float ColorMiddleR { get; set; }
            [Category("Parameters"), Description("Offset 0x34 (f32). ")]
            public float ColorMiddleG { get; set; }
            [Category("Parameters"), Description("Offset 0x38 (f32). ")]
            public float ColorMiddleB { get; set; }
            [Category("Parameters"), Description("Offset 0x3C (f32). ")]
            public float ColorMiddleA { get; set; }
            [Category("Parameters"), Description("Offset 0x40 (f32). ")]
            public float ColorEndR { get; set; }
            [Category("Parameters"), Description("Offset 0x44 (f32). ")]
            public float ColorEndG { get; set; }
            [Category("Parameters"), Description("Offset 0x48 (f32). ")]
            public float ColorEndB { get; set; }
            [Category("Parameters"), Description("Offset 0x4C (f32). ")]
            public float ColorEndA { get; set; }
            [Category("Parameters"), Description("Offset 0x50 (f32). Middle color position along trail distance, 0..1.")]
            public float ColorFactor { get; set; }
            [Category("Parameters"), Description("Offset 0x54 (u16). Width samples use raw / 255. Stored raw; divide by 255 for the game value.")]
            public ushort WidthStart { get; set; }
            [Category("Parameters"), Description("Offset 0x56 (u16).  Stored raw; divide by 255 for the game value.")]
            public ushort WidthMiddle { get; set; }
            [Category("Parameters"), Description("Offset 0x58 (u16).  Stored raw; divide by 255 for the game value.")]
            public ushort WidthEnd { get; set; }
            [Category("Parameters"), Description("Offset 0x5A (u8). Middle width position in the history; raw / 255. Stored raw; divide by 255 for the game value.")]
            public byte WidthMiddlePoint { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x5B (u8). Unknown use: copied byte, seen as 0x00 and 0x20. Preserve it.")]
            public byte Field5B { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x5C (u32). Unknown: copied without byte swapping; no confirmed game use.")]
            public uint Field5C { get; set; }
            public TrailManagerEntry Clone() { return (TrailManagerEntry)MemberwiseClone(); }
        }

        public sealed class TrailResourceEntry
        {
            [Category("Parameters"), Description("Offset 0x00 (u32). ")]
            public int EffectChunkMapId { get; set; }
            [Category("Links"), Description("Offset 0x04 (u32). ")]
            public int TrailEntryIndex { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x08 (u32). Replaced on load: resolved EffectChunkIndex pointer.")]
            public uint EffectRef { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x0C (u32). Unused by game loader: skipped prefix word.")]
            public uint Field0C { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x10 (u32). Used as cached billboard pointer; replaced by lookup when CacheState is 0.")]
            public uint BillboardPtr { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x14 (u32). 0 allows lookup; runtime becomes 2. Keep file value unchanged.")]
            public uint CacheState { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x18 (u32). Unknown: copied beside cache state; no confirmed game use.")]
            public uint Field18 { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x1C (u32). Unknown: copied resource tail; no confirmed game use.")]
            public uint Field1C { get; set; }
            public TrailResourceEntry Clone() { return (TrailResourceEntry)MemberwiseClone(); }
        }

        public sealed class TrailPositionEntry
        {
            [Category("Parameters"), Description("Offset 0x00 (s32). ")]
            public int CoordChunkMapId { get; set; }
            [Category("Links"), Description("Offset 0x04 (u32). Matching records supply the trail edges, in file order.")]
            public int TrailEntryIndex { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x08 (s32). Replaced on load: resolved CoordChunkIndex pointer.")]
            public int CoordRef { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x0C (s32). Unknown prefix word: no confirmed game use.")]
            public int Field0C { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x10 (s32). Unknown: copied; no endpoint offset or other game use established.")]
            public int Field10 { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x14 (s32). Unknown: copied; no confirmed game use.")]
            public int Field14 { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x18 (s32). Unknown: copied; no confirmed game use.")]
            public int Field18 { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x1C (s32). Unknown: copied; no confirmed game use.")]
            public int Field1C { get; set; }
            [Category("Parameters"), Description("Offset 0x20 (s32). ")]
            public int ClumpChunkMapId { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x24 (s32). Replaced on load when ClumpChunkIndex != -1: clump reference pointer.")]
            public int ClumpRef { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x28 (s32). Unused by game loader (0x79+): not mapped into runtime data.")]
            public int Field28 { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x2C (s32). Unused by game loader (0x79+): not mapped into runtime data.")]
            public int Field2C { get; set; }
            public TrailPositionEntry Clone() { return (TrailPositionEntry)MemberwiseClone(); }
        }

        public sealed class TrailForceFieldEntry
        {
            [Category("Parameters"), Description("Offset 0x00 (s32). ")]
            public int CoordChunkMapId { get; set; }
            [Category("Links"), Description("Offset 0x04 (u32). ")]
            public int TrailEntryIndex { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x08 (s32). Replaced on load: resolved CoordChunkIndex pointer.")]
            public int CoordRef { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x0C (s32). Unknown prefix word: no confirmed game use.")]
            public int Field0C { get; set; }
            [Category("Parameters"), Description("Offset 0x10 (f32). ")]
            public float DirectionX { get; set; }
            [Category("Parameters"), Description("Offset 0x14 (f32). ")]
            public float DirectionY { get; set; }
            [Category("Parameters"), Description("Offset 0x18 (f32). ")]
            public float DirectionZ { get; set; }
            [Category("Parameters"), Description("Offset 0x1C (f32). Each update: velocity += velocity * value.")]
            public float VelocityGrowth { get; set; }
            [Category("Parameters"), Description("Offset 0x20 (u32). ")]
            public TrailForceType ForceType { get; set; }
            [Category("Parameters"), Description("Offset 0x24 (f32). Game multiplies this value by 100.")]
            public float Radius { get; set; }
            [Category("Parameters"), Description("Offset 0x28 (f32). ")]
            public float Strength { get; set; }
            [Category("Parameters"), Description("Offset 0x2C (u32). Named bits are used; other bits have no confirmed game use.")]
            public TrailForceFlags ForceFlags { get; set; }
            [Category("Parameters"), Description("Offset 0x30 (s32). ")]
            public int ClumpChunkMapId { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x34 (s32). Replaced on load when ClumpChunkIndex != -1: clump reference pointer.")]
            public int ClumpRef { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x38 (s32). Unused by game loader (0x79+): not mapped into runtime data.")]
            public int Field38 { get; set; }
            [Category("Reserved / Runtime"), Description("Offset 0x3C (s32). Unused by game loader (0x79+): not mapped into runtime data.")]
            public int Field3C { get; set; }
            public TrailForceFieldEntry Clone() { return (TrailForceFieldEntry)MemberwiseClone(); }
        }

        public sealed class TrailNodeEntry
        {
            public uint Field00;
            public int TrailEntryIndex;
            public uint Field08;
            public uint Field0C;
            public uint Padding;
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
            public uint Raw;
            public bool Enabled { get { return (Raw & 0x80000000u) != 0; } set { Raw = (Raw & 0x7FFFFFFFu) | (value ? 0x80000000u : 0u); } }
            public uint TimeUnits { get { return Raw & 0x7FFFFFFFu; } set { if (value > 0x7FFFFFFFu) throw new ArgumentOutOfRangeException("value"); Raw = (Raw & 0x80000000u) | value; } }

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
                int version = chunk.SourcePage != null ? chunk.SourcePage.Version : 99;
                int versionAttribute = chunk.SourcePage != null ? chunk.SourcePage.VersionAttribute : 37494;
                backend.UpsertChunk(chunk.OriginalChunkName, chunk.ChunkName, TrailChunkType, chunk.ChunkPath, ".trail", data, version, versionAttribute);
                backend.SetChunkPageChunkMaps(chunk.ChunkName, TrailChunkType, BuildReferenceChunkMaps(chunk));
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

            // The loader ignores stored positions and fixed-record sizes.
            int sequentialOffset = HeaderCount * HeaderSize;
            int[] sizes = { ManagerSize, ResourceSize, page.Version >= 0x79 ? PositionSize : 0x20, page.Version >= 0x79 ? ForceFieldSize : 0x30 };
            for (int i = 0; i < HeaderCount; i++)
            {
                headers[i].Offset = sequentialOffset;
                if (i < 4) headers[i].Size = sizes[i];
                sequentialOffset += i < 4 ? headers[i].Count * sizes[i] : headers[i].Size;
            }
            if (sequentialOffset > data.Length) return false;

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
            int entrySize = header.Size;
            if (!ValidateSectionBounds(data, header)) return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + i * entrySize;
                if (offset > data.Length - entrySize) return false;
                var entry = new TrailManagerEntry();
                entry.AnimationChunkMapId = (int)ReadUInt32BE(data, offset + 0x00);
                entry.EntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                entry.AnimationRef = ReadUInt32BE(data, offset + 0x08);
                entry.Field0C = ReadUInt32BE(data, offset + 0x0C);
                entry.Field10 = ReadUInt32BE(data, offset + 0x10);
                entry.Lifetime = ReadUInt32BE(data, offset + 0x14);
                entry.Subdivisions = ReadUInt32BE(data, offset + 0x18);
                entry.TrailFlags = (TrailFlags)ReadUInt16BE(data, offset + 0x1C);
                entry.AlphaFade = data[offset + 0x1E];
                entry.WidthFade = data[offset + 0x1F];
                entry.ColorStartR = ReadSingleBE(data, offset + 0x20);
                entry.ColorStartG = ReadSingleBE(data, offset + 0x24);
                entry.ColorStartB = ReadSingleBE(data, offset + 0x28);
                entry.ColorStartA = ReadSingleBE(data, offset + 0x2C);
                entry.ColorMiddleR = ReadSingleBE(data, offset + 0x30);
                entry.ColorMiddleG = ReadSingleBE(data, offset + 0x34);
                entry.ColorMiddleB = ReadSingleBE(data, offset + 0x38);
                entry.ColorMiddleA = ReadSingleBE(data, offset + 0x3C);
                entry.ColorEndR = ReadSingleBE(data, offset + 0x40);
                entry.ColorEndG = ReadSingleBE(data, offset + 0x44);
                entry.ColorEndB = ReadSingleBE(data, offset + 0x48);
                entry.ColorEndA = ReadSingleBE(data, offset + 0x4C);
                entry.ColorFactor = ReadSingleBE(data, offset + 0x50);
                entry.WidthStart = ReadUInt16BE(data, offset + 0x54);
                entry.WidthMiddle = ReadUInt16BE(data, offset + 0x56);
                entry.WidthEnd = ReadUInt16BE(data, offset + 0x58);
                entry.WidthMiddlePoint = data[offset + 0x5A];
                entry.Field5B = data[offset + 0x5B];
                entry.Field5C = ReadUInt32BE(data, offset + 0x5C);
                list.Add(entry);
            }
            return true;
        }

        private bool TryParseResources(byte[] data, TrailSectionHeader header, List<TrailResourceEntry> list)
        {
            int entrySize = header.Size;
            if (!ValidateSectionBounds(data, header)) return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + i * entrySize;
                if (offset > data.Length - entrySize) return false;
                var entry = new TrailResourceEntry();
                entry.EffectChunkMapId = (int)ReadUInt32BE(data, offset + 0x00);
                entry.TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                entry.EffectRef = ReadUInt32BE(data, offset + 0x08);
                entry.Field0C = ReadUInt32BE(data, offset + 0x0C);
                entry.BillboardPtr = ReadUInt32BE(data, offset + 0x10);
                entry.CacheState = ReadUInt32BE(data, offset + 0x14);
                entry.Field18 = ReadUInt32BE(data, offset + 0x18);
                entry.Field1C = ReadUInt32BE(data, offset + 0x1C);
                list.Add(entry);
            }
            return true;
        }

        private bool TryParsePositions(byte[] data, TrailSectionHeader header, List<TrailPositionEntry> list)
        {
            int entrySize = header.Size;
            if (!ValidateSectionBounds(data, header)) return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + i * entrySize;
                if (offset > data.Length - entrySize) return false;
                var entry = new TrailPositionEntry();
                entry.CoordChunkMapId = ReadInt32BE(data, offset + 0x00);
                entry.TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                entry.CoordRef = ReadInt32BE(data, offset + 0x08);
                entry.Field0C = ReadInt32BE(data, offset + 0x0C);
                entry.Field10 = ReadInt32BE(data, offset + 0x10);
                entry.Field14 = ReadInt32BE(data, offset + 0x14);
                entry.Field18 = ReadInt32BE(data, offset + 0x18);
                entry.Field1C = ReadInt32BE(data, offset + 0x1C);
                if (entrySize > 0x20) entry.ClumpChunkMapId = ReadInt32BE(data, offset + 0x20);
                if (entrySize > 0x20) entry.ClumpRef = ReadInt32BE(data, offset + 0x24);
                if (entrySize > 0x20) entry.Field28 = ReadInt32BE(data, offset + 0x28);
                if (entrySize > 0x20) entry.Field2C = ReadInt32BE(data, offset + 0x2C);
                list.Add(entry);
            }
            return true;
        }

        private bool TryParseForceFields(byte[] data, TrailSectionHeader header, List<TrailForceFieldEntry> list)
        {
            int entrySize = header.Size;
            if (!ValidateSectionBounds(data, header)) return false;
            for (int i = 0; i < header.Count; i++)
            {
                int offset = header.Offset + i * entrySize;
                if (offset > data.Length - entrySize) return false;
                var entry = new TrailForceFieldEntry();
                entry.CoordChunkMapId = ReadInt32BE(data, offset + 0x00);
                entry.TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                entry.CoordRef = ReadInt32BE(data, offset + 0x08);
                entry.Field0C = ReadInt32BE(data, offset + 0x0C);
                entry.DirectionX = ReadSingleBE(data, offset + 0x10);
                entry.DirectionY = ReadSingleBE(data, offset + 0x14);
                entry.DirectionZ = ReadSingleBE(data, offset + 0x18);
                entry.VelocityGrowth = ReadSingleBE(data, offset + 0x1C);
                entry.ForceType = (TrailForceType)ReadUInt32BE(data, offset + 0x20);
                entry.Radius = ReadSingleBE(data, offset + 0x24);
                entry.Strength = ReadSingleBE(data, offset + 0x28);
                entry.ForceFlags = (TrailForceFlags)ReadUInt32BE(data, offset + 0x2C);
                if (entrySize > 0x30) entry.ClumpChunkMapId = ReadInt32BE(data, offset + 0x30);
                if (entrySize > 0x30) entry.ClumpRef = ReadInt32BE(data, offset + 0x34);
                if (entrySize > 0x30) entry.Field38 = ReadInt32BE(data, offset + 0x38);
                if (entrySize > 0x30) entry.Field3C = ReadInt32BE(data, offset + 0x3C);
                list.Add(entry);
            }
            return true;
        }

        private bool TryParseNodes(byte[] data, TrailSectionHeader header, List<TrailNodeEntry> list)
        {
            if (!ValidateSectionBounds(data, header))
                return false;
            int offset = header.Offset;
            int end = offset + header.Size;
            if (end > data.Length) return false;
            for (int i = 0; i < header.Count; i++)
            {
                if (end - offset < NodeBaseSize)
                    return false;

                TrailNodeEntry node = new TrailNodeEntry();
                node.Field00 = ReadUInt32BE(data, offset + 0x00);
                node.TrailEntryIndex = (int)ReadUInt32BE(data, offset + 0x04);
                node.Field08 = ReadUInt32BE(data, offset + 0x08);
                node.Field0C = ReadUInt32BE(data, offset + 0x0C);
                int frameCount = (int)ReadUInt32BE(data, offset + 0x10);
                offset += NodeBaseSize;
                if (frameCount < 0 || frameCount > (end - offset) / 4) return false;

                for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                {
                    if (end - offset < 4)
                        return false;
                    node.Frames.Add(new TrailFrameEntry
                    {
                        Raw = ReadUInt32BE(data, offset)
                    });
                    offset += 4;
                }

                if ((frameCount & 1) == 0)
                {
                    if (end - offset < 4) return false;
                    node.Padding = ReadUInt32BE(data, offset);
                    offset += 4;
                }

                list.Add(node);
            }
            return offset == end;
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
            headers[2] = WritePositions(writer, chunk.Positions, chunk.SourcePage == null || chunk.SourcePage.Version >= 0x79);
            headers[3] = WriteForceFields(writer, chunk.ForceFields, chunk.SourcePage == null || chunk.SourcePage.Version >= 0x79);
            headers[4] = WriteNodes(writer, chunk.Nodes);

            stream.Position = 0;
            for (int i = 0; i < headers.Length; i++)
            {
                WriteUInt32BE(writer, (uint)headers[i].Offset);
                WriteUInt16BE(writer, checked((ushort)headers[i].Count));
                WriteUInt16BE(writer, checked((ushort)headers[i].Size));
            }

            return stream.ToArray();
        }

        private TrailSectionHeader WriteManagers(BinaryWriter writer, List<TrailManagerEntry> entries)
        {
            var header = new TrailSectionHeader();
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = ManagerSize;
            foreach (var entry in entries)
            {
                WriteUInt32BE(writer, (uint)entry.AnimationChunkMapId);
                WriteUInt32BE(writer, (uint)entry.EntryIndex);
                WriteUInt32BE(writer, entry.AnimationRef);
                WriteUInt32BE(writer, entry.Field0C);
                WriteUInt32BE(writer, entry.Field10);
                WriteUInt32BE(writer, entry.Lifetime);
                WriteUInt32BE(writer, entry.Subdivisions);
                WriteUInt16BE(writer, (ushort)entry.TrailFlags);
                writer.Write(entry.AlphaFade);
                writer.Write(entry.WidthFade);
                WriteSingleBE(writer, entry.ColorStartR);
                WriteSingleBE(writer, entry.ColorStartG);
                WriteSingleBE(writer, entry.ColorStartB);
                WriteSingleBE(writer, entry.ColorStartA);
                WriteSingleBE(writer, entry.ColorMiddleR);
                WriteSingleBE(writer, entry.ColorMiddleG);
                WriteSingleBE(writer, entry.ColorMiddleB);
                WriteSingleBE(writer, entry.ColorMiddleA);
                WriteSingleBE(writer, entry.ColorEndR);
                WriteSingleBE(writer, entry.ColorEndG);
                WriteSingleBE(writer, entry.ColorEndB);
                WriteSingleBE(writer, entry.ColorEndA);
                WriteSingleBE(writer, entry.ColorFactor);
                WriteUInt16BE(writer, entry.WidthStart);
                WriteUInt16BE(writer, entry.WidthMiddle);
                WriteUInt16BE(writer, entry.WidthEnd);
                writer.Write(entry.WidthMiddlePoint);
                writer.Write(entry.Field5B);
                WriteUInt32BE(writer, entry.Field5C);
            }
            return header;
        }

        private TrailSectionHeader WriteResources(BinaryWriter writer, List<TrailResourceEntry> entries)
        {
            var header = new TrailSectionHeader();
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = ResourceSize;
            foreach (var entry in entries)
            {
                WriteUInt32BE(writer, (uint)entry.EffectChunkMapId);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteUInt32BE(writer, entry.EffectRef);
                WriteUInt32BE(writer, entry.Field0C);
                WriteUInt32BE(writer, entry.BillboardPtr);
                WriteUInt32BE(writer, entry.CacheState);
                WriteUInt32BE(writer, entry.Field18);
                WriteUInt32BE(writer, entry.Field1C);
            }
            return header;
        }

        private TrailSectionHeader WritePositions(BinaryWriter writer, List<TrailPositionEntry> entries, bool modern)
        {
            var header = new TrailSectionHeader();
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = modern ? PositionSize : 0x20;
            foreach (var entry in entries)
            {
                WriteInt32BE(writer, entry.CoordChunkMapId);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteInt32BE(writer, entry.CoordRef);
                WriteInt32BE(writer, entry.Field0C);
                WriteInt32BE(writer, entry.Field10);
                WriteInt32BE(writer, entry.Field14);
                WriteInt32BE(writer, entry.Field18);
                WriteInt32BE(writer, entry.Field1C);
                if (modern) WriteInt32BE(writer, entry.ClumpChunkMapId);
                if (modern) WriteInt32BE(writer, entry.ClumpRef);
                if (modern) WriteInt32BE(writer, entry.Field28);
                if (modern) WriteInt32BE(writer, entry.Field2C);
            }
            return header;
        }

        private TrailSectionHeader WriteForceFields(BinaryWriter writer, List<TrailForceFieldEntry> entries, bool modern)
        {
            var header = new TrailSectionHeader();
            header.Offset = (int)writer.BaseStream.Position;
            header.Count = entries.Count;
            header.Size = modern ? ForceFieldSize : 0x30;
            foreach (var entry in entries)
            {
                WriteInt32BE(writer, entry.CoordChunkMapId);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteInt32BE(writer, entry.CoordRef);
                WriteInt32BE(writer, entry.Field0C);
                WriteSingleBE(writer, entry.DirectionX);
                WriteSingleBE(writer, entry.DirectionY);
                WriteSingleBE(writer, entry.DirectionZ);
                WriteSingleBE(writer, entry.VelocityGrowth);
                WriteUInt32BE(writer, (uint)entry.ForceType);
                WriteSingleBE(writer, entry.Radius);
                WriteSingleBE(writer, entry.Strength);
                WriteUInt32BE(writer, (uint)entry.ForceFlags);
                if (modern) WriteInt32BE(writer, entry.ClumpChunkMapId);
                if (modern) WriteInt32BE(writer, entry.ClumpRef);
                if (modern) WriteInt32BE(writer, entry.Field38);
                if (modern) WriteInt32BE(writer, entry.Field3C);
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
                WriteUInt32BE(writer, entry.Field00);
                WriteUInt32BE(writer, (uint)entry.TrailEntryIndex);
                WriteUInt32BE(writer, entry.Field08);
                WriteUInt32BE(writer, entry.Field0C);
                WriteUInt32BE(writer, (uint)entry.Frames.Count);
                foreach (TrailFrameEntry frame in entry.Frames)
                {
                    WriteUInt32BE(writer, frame.Raw);
                }
                if ((entry.Frames.Count & 1) == 0)
                    WriteUInt32BE(writer, entry.Padding);
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

        private static List<XfbinParserChunkMap> BuildReferenceChunkMaps(TrailChunkState chunk)
        {
            if (chunk == null)
                return new List<XfbinParserChunkMap>();

            return chunk.MapEntries
                .Where(x =>
                    x != null &&
                    !string.Equals(x.Type, "nuccChunkNull", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(x.Type, "nuccChunkPage", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(x.Type, "nuccChunkIndex", StringComparison.OrdinalIgnoreCase) &&
                    !(string.Equals(x.Type, TrailChunkType, StringComparison.OrdinalIgnoreCase) &&
                      string.Equals(x.Name, chunk.ChunkName, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new XfbinParserChunkMap
                {
                    Name = x.Name ?? string.Empty,
                    Type = x.Type ?? string.Empty,
                    Path = x.Path ?? string.Empty
                })
                .ToList();
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
            (s as PropertyGrid)?.Refresh();
            RefreshAllDisplays();
        }

        private string GetChunkDisplay(TrailChunkState chunk)
        {
            return chunk != null ? chunk.ChunkName : string.Empty;
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
            return string.Format("Trail Entry {0} | Animation {1}", manager.EntryIndex, GetMapDisplay(chunk, manager.AnimationChunkMapId));
        }

        private string GetTrailEntryDisplay(TrailChunkState chunk, int trailEntryIndex)
        {
            if (trailEntryIndex < 0)
                return "Unlinked trail";

            TrailManagerEntry manager = chunk != null ? chunk.Managers.FirstOrDefault(x => x.EntryIndex == trailEntryIndex) : null;
            return manager != null
                ? string.Format("Trail Entry {0}", manager.EntryIndex)
                : string.Format("Unlinked trail ({0})", trailEntryIndex);
        }

        private void chunkComboBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailChunkState ? GetChunkDisplay((TrailChunkState)e.ListItem) : string.Empty; }
        private void managersListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailManagerEntry ? GetManagerDisplay(CurrentChunk, CurrentChunk.Managers.IndexOf((TrailManagerEntry)e.ListItem)) : string.Empty; }
        private void resourcesListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailResourceEntry ? string.Format("{0} | Effect {1}", GetTrailEntryDisplay(CurrentChunk, ((TrailResourceEntry)e.ListItem).TrailEntryIndex), GetMapDisplay(CurrentChunk, ((TrailResourceEntry)e.ListItem).EffectChunkMapId)) : string.Empty; }
        private void positionsListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailPositionEntry ? string.Format("{0} | Coord {1}", GetTrailEntryDisplay(CurrentChunk, ((TrailPositionEntry)e.ListItem).TrailEntryIndex), GetMapDisplay(CurrentChunk, ((TrailPositionEntry)e.ListItem).CoordChunkMapId)) : string.Empty; }
        private void forceFieldsListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailForceFieldEntry ? string.Format("{0} | Force Field", GetTrailEntryDisplay(CurrentChunk, ((TrailForceFieldEntry)e.ListItem).TrailEntryIndex)) : string.Empty; }
        private void mapIdsListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailMapEntry ? GetMapDisplay(CurrentChunk, CurrentChunk.MapEntries.IndexOf((TrailMapEntry)e.ListItem)) : string.Empty; }
        private void nodesListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            var node = e.ListItem as TrailNodeEntry;
            if (node == null || CurrentChunk == null) { e.Value = string.Empty; return; }
            int ordinal = CurrentChunk.Nodes.IndexOf(node);
            string manager = ordinal >= 0 && ordinal < CurrentChunk.Managers.Count
                ? GetManagerDisplay(CurrentChunk, ordinal) : "Unlinked timeline";
            e.Value = string.Format("{0} | {1} events", manager, node.Frames.Count);
        }
        private void framesListBox_Format(object sender, ListControlConvertEventArgs e) { e.Value = e.ListItem is TrailFrameEntry ? string.Format("Frame {0} | Enabled {1} | Time units {2}", CurrentNode != null ? CurrentNode.Frames.IndexOf((TrailFrameEntry)e.ListItem) : -1, ((TrailFrameEntry)e.ListItem).Enabled, ((TrailFrameEntry)e.ListItem).TimeUnits) : string.Empty; }

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

        private void AddManagerButton_Click(object sender, EventArgs e) { AddListEntry(CurrentChunk != null ? CurrentChunk.Managers : null, new TrailManagerEntry(), this.managersListBox); }

        private void DuplicateManagerButton_Click(object sender, EventArgs e) { DuplicateListEntry(CurrentChunk != null ? CurrentChunk.Managers : null, CurrentManager, x => x.Clone(), this.managersListBox); }

        private void DeleteManagerButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || CurrentManager == null) return;
            int index = this.managersListBox.SelectedIndex;
            CurrentChunk.Managers.RemoveAt(index);
            RemapTrailEntryReferences(index, -1);
            RefreshCurrentChunk();
        }

        private void AddResourceButton_Click(object sender, EventArgs e) { AddListEntry(CurrentChunk != null ? CurrentChunk.Resources : null, new TrailResourceEntry(), this.resourcesListBox); }

        private void DuplicateResourceButton_Click(object sender, EventArgs e) { DuplicateListEntry(CurrentChunk != null ? CurrentChunk.Resources : null, CurrentResource, x => x.Clone(), this.resourcesListBox); }

        private void DeleteResourceButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.resourcesListBox.SelectedIndex < 0) return;
            CurrentChunk.Resources.RemoveAt(this.resourcesListBox.SelectedIndex);
            RefreshCurrentChunk();
        }

        private void AddPositionButton_Click(object sender, EventArgs e) { AddListEntry(CurrentChunk != null ? CurrentChunk.Positions : null, new TrailPositionEntry(), this.positionsListBox); }

        private void DuplicatePositionButton_Click(object sender, EventArgs e) { DuplicateListEntry(CurrentChunk != null ? CurrentChunk.Positions : null, CurrentPosition, x => x.Clone(), this.positionsListBox); }

        private void DeletePositionButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.positionsListBox.SelectedIndex < 0) return;
            CurrentChunk.Positions.RemoveAt(this.positionsListBox.SelectedIndex);
            RefreshCurrentChunk();
        }

        private void AddForceFieldButton_Click(object sender, EventArgs e) { AddListEntry(CurrentChunk != null ? CurrentChunk.ForceFields : null, new TrailForceFieldEntry(), this.forceFieldsListBox); }

        private void DuplicateForceFieldButton_Click(object sender, EventArgs e) { DuplicateListEntry(CurrentChunk != null ? CurrentChunk.ForceFields : null, CurrentForceField, x => x.Clone(), this.forceFieldsListBox); }

        private void DeleteForceFieldButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || this.forceFieldsListBox.SelectedIndex < 0) return;
            CurrentChunk.ForceFields.RemoveAt(this.forceFieldsListBox.SelectedIndex);
            RefreshCurrentChunk();
        }

        private void AddMapButton_Click(object sender, EventArgs e) { AddListEntry(CurrentChunk != null ? CurrentChunk.MapEntries : null, new TrailMapEntry { Name = "NewMap", Type = "nuccChunkCoord", Path = string.Empty }, this.mapIdsListBox); }

        private void DuplicateMapButton_Click(object sender, EventArgs e)
        {
            if (CurrentChunk == null || CurrentMapEntry == null) return;
            int sourceIndex = this.mapIdsListBox.SelectedIndex;
            int newIndex = sourceIndex + 1;
            TrailMapEntry clone = CurrentMapEntry.Clone();
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

        private void AddNodeButton_Click(object sender, EventArgs e) { AddListEntry(CurrentChunk != null ? CurrentChunk.Nodes : null, new TrailNodeEntry(), this.nodesListBox); }

        private void DuplicateNodeButton_Click(object sender, EventArgs e) { DuplicateListEntry(CurrentChunk != null ? CurrentChunk.Nodes : null, CurrentNode, x => x.Clone(), this.nodesListBox); }

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

        private void CopyManagerButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentManager, "manager"); }
        private void PasteManagerButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentChunk != null ? CurrentChunk.Managers : null, this.managersListBox, "manager", entry => entry.EntryIndex = GetNextTrailEntryIndex()); }
        private void CopyResourceButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentResource, "resource"); }
        private void PasteResourceButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentChunk != null ? CurrentChunk.Resources : null, this.resourcesListBox, "resource"); }
        private void CopyPositionButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentPosition, "position"); }
        private void PastePositionButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentChunk != null ? CurrentChunk.Positions : null, this.positionsListBox, "position"); }
        private void CopyForceFieldButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentForceField, "forcefield"); }
        private void PasteForceFieldButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentChunk != null ? CurrentChunk.ForceFields : null, this.forceFieldsListBox, "forcefield"); }
        private void CopyMapButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentMapEntry, "map"); }
        private void PasteMapButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentChunk != null ? CurrentChunk.MapEntries : null, this.mapIdsListBox, "map"); }
        private void CopyNodeButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentNode, "node"); }
        private void PasteNodeButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentChunk != null ? CurrentChunk.Nodes : null, this.nodesListBox, "node"); }
        private void CopyFrameButton_Click(object sender, EventArgs e) { CopyClipboardEntry(CurrentFrame, "frame"); }
        private void PasteFrameButton_Click(object sender, EventArgs e) { PasteClipboardEntry(CurrentNode != null ? CurrentNode.Frames : null, this.framesListBox, "frame", null, RefreshFramesAndDisplays); }

        private void AddListEntry<T>(List<T> list, T entry, ListBox listBox) where T : class
        {
            if (list == null || entry == null)
                return;

            list.Add(entry);
            RefreshCurrentChunk();
            listBox.SelectedItem = entry;
        }

        private void DuplicateListEntry<T>(List<T> list, T selected, Func<T, T> clone, ListBox listBox) where T : class
        {
            if (list == null || selected == null)
                return;

            T cloneEntry = clone(selected);
            list.Insert(listBox.SelectedIndex + 1, cloneEntry);
            RefreshCurrentChunk();
            listBox.SelectedItem = cloneEntry;
        }

        private void CopyClipboardEntry<T>(T entry, string entryType) where T : class
        {
            if (entry == null)
                return;

            TrailClipboardPayload payload = new TrailClipboardPayload
            {
                EntryType = entryType,
                Json = JsonConvert.SerializeObject(entry)
            };

            Clipboard.SetText(ClipboardPrefix + JsonConvert.SerializeObject(payload));
        }

        private void PasteClipboardEntry<T>(List<T> list, ListBox listBox, string entryType, Action<T> prepare = null, Action refresh = null) where T : class
        {
            if (list == null || !TryReadClipboardEntry(entryType, out T entry))
                return;

            if (prepare != null)
                prepare(entry);

            list.Add(entry);
            if (refresh != null)
            {
                refresh();
                listBox.SelectedItem = entry;
                return;
            }

            RefreshCurrentChunk();
            listBox.SelectedItem = entry;
        }

        private bool TryReadClipboardEntry<T>(string entryType, out T entry) where T : class
        {
            entry = null;
            if (!Clipboard.ContainsText())
                return false;

            string text = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(text) || !text.StartsWith(ClipboardPrefix, StringComparison.Ordinal))
                return false;

            TrailClipboardPayload payload = JsonConvert.DeserializeObject<TrailClipboardPayload>(text.Substring(ClipboardPrefix.Length));
            if (payload == null || !string.Equals(payload.EntryType, entryType, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(payload.Json))
                return false;

            entry = JsonConvert.DeserializeObject<T>(payload.Json);
            return entry != null;
        }

        private int GetNextTrailEntryIndex()
        {
            if (CurrentChunk == null || CurrentChunk.Managers.Count == 0)
                return 0;

            return CurrentChunk.Managers[CurrentChunk.Managers.Count - 1].EntryIndex + 1;
        }

        private void RefreshFramesAndDisplays()
        {
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
            foreach (TrailForceFieldEntry entry in CurrentChunk.ForceFields)
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
            foreach (TrailForceFieldEntry entry in CurrentChunk.ForceFields)
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

        private static int ToScaleRaw(float value, int maximum)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < 0 || value > maximum / 255f)
                throw new ArgumentOutOfRangeException("value", "Value must be between 0 and " + (maximum / 255f).ToString(CultureInfo.CurrentCulture) + ".");
            return (int)Math.Round(value * 255.0, MidpointRounding.AwayFromZero);
        }

        private sealed class TrailManagerViewModel : IChunkViewModel
        {
            private readonly TrailManagerEntry entry;
            public TrailManagerViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailManagerEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            [Browsable(false)] public Tool_TrailEditor Editor { get; private set; }
            [Browsable(false)] public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int AnimationChunkMapId { get { return entry.AnimationChunkMapId; } set { entry.AnimationChunkMapId = value; } }
            [Category("Parameters"), Description("Offset 0x04. Matches resources, positions and forces; timelines use array order.")] public int EntryIndex { get { return entry.EntryIndex; } set { entry.EntryIndex = value; } }
            [Category("Parameters"), Description("Offset 0x08. Replaced on load: resolved AnimationChunkIndex pointer.")] public uint AnimationRef { get { return entry.AnimationRef; } set { entry.AnimationRef = value; } }
            [Category("Parameters"), Description("Offset 0x0C. Unused by game loader: skipped prefix word.")] public uint Field0C { get { return entry.Field0C; } set { entry.Field0C = value; } }
            [Category("Parameters"), Description("Offset 0x10. Unknown: copied; no use found in traced NX/S4/Connections trail code.")] public uint Field10 { get { return entry.Field10; } set { entry.Field10 = value; } }
            [Category("Parameters"), Description("Offset 0x14. History length in 30 FPS frames.")] public uint Lifetime { get { return entry.Lifetime; } set { entry.Lifetime = value; } }
            [Category("Parameters"), Description("Offset 0x18. Extra samples used to smooth the trail.")] public uint Subdivisions { get { return entry.Subdivisions; } set { entry.Subdivisions = value; } }
            [Category("Parameters"), Description("Offset 0x1C. Named bits are used; other bits have no confirmed game use.")] public TrailFlags TrailFlags { get { return entry.TrailFlags; } set { entry.TrailFlags = value; } }
            [Category("Parameters"), Description("Offset 0x1E. Unused without FadeAlpha bit; otherwise fade per update after stop, raw / 255.")] public byte AlphaFade { get { return entry.AlphaFade; } set { entry.AlphaFade = value; } }
            [Category("Scaled values"), RefreshProperties(RefreshProperties.All), Description("Editable game value (raw / 255), rounded to the nearest stored step.")] public float AlphaFadeValue { get { return entry.AlphaFade / 255f; } set { entry.AlphaFade = (byte)ToScaleRaw(value, byte.MaxValue); } }
            [Category("Parameters"), Description("Offset 0x1F. Unused without FadeWidth bit; otherwise shrink per update after stop, raw / 255.")] public byte WidthFade { get { return entry.WidthFade; } set { entry.WidthFade = value; } }
            [Category("Scaled values"), RefreshProperties(RefreshProperties.All), Description("Editable game value (raw / 255), rounded to the nearest stored step.")] public float WidthFadeValue { get { return entry.WidthFade / 255f; } set { entry.WidthFade = (byte)ToScaleRaw(value, byte.MaxValue); } }
            [Category("Parameters"), Description("Offset 0x20. ")] public float ColorStartR { get { return entry.ColorStartR; } set { entry.ColorStartR = value; } }
            [Category("Parameters"), Description("Offset 0x24. ")] public float ColorStartG { get { return entry.ColorStartG; } set { entry.ColorStartG = value; } }
            [Category("Parameters"), Description("Offset 0x28. ")] public float ColorStartB { get { return entry.ColorStartB; } set { entry.ColorStartB = value; } }
            [Category("Parameters"), Description("Offset 0x2C. ")] public float ColorStartA { get { return entry.ColorStartA; } set { entry.ColorStartA = value; } }
            [Category("Parameters"), Description("Offset 0x30. ")] public float ColorMiddleR { get { return entry.ColorMiddleR; } set { entry.ColorMiddleR = value; } }
            [Category("Parameters"), Description("Offset 0x34. ")] public float ColorMiddleG { get { return entry.ColorMiddleG; } set { entry.ColorMiddleG = value; } }
            [Category("Parameters"), Description("Offset 0x38. ")] public float ColorMiddleB { get { return entry.ColorMiddleB; } set { entry.ColorMiddleB = value; } }
            [Category("Parameters"), Description("Offset 0x3C. ")] public float ColorMiddleA { get { return entry.ColorMiddleA; } set { entry.ColorMiddleA = value; } }
            [Category("Parameters"), Description("Offset 0x40. ")] public float ColorEndR { get { return entry.ColorEndR; } set { entry.ColorEndR = value; } }
            [Category("Parameters"), Description("Offset 0x44. ")] public float ColorEndG { get { return entry.ColorEndG; } set { entry.ColorEndG = value; } }
            [Category("Parameters"), Description("Offset 0x48. ")] public float ColorEndB { get { return entry.ColorEndB; } set { entry.ColorEndB = value; } }
            [Category("Parameters"), Description("Offset 0x4C. ")] public float ColorEndA { get { return entry.ColorEndA; } set { entry.ColorEndA = value; } }
            [Category("Parameters"), Description("Offset 0x50. Middle color position along trail distance, 0..1.")] public float ColorFactor { get { return entry.ColorFactor; } set { entry.ColorFactor = value; } }
            [Category("Parameters"), Description("Offset 0x54. Width samples use raw / 255.")] public ushort WidthStart { get { return entry.WidthStart; } set { entry.WidthStart = value; } }
            [Category("Scaled values"), RefreshProperties(RefreshProperties.All), Description("Editable game value (raw / 255), rounded to the nearest stored step.")] public float WidthStartValue { get { return entry.WidthStart / 255f; } set { entry.WidthStart = (ushort)ToScaleRaw(value, ushort.MaxValue); } }
            [Category("Parameters"), Description("Offset 0x56. ")] public ushort WidthMiddle { get { return entry.WidthMiddle; } set { entry.WidthMiddle = value; } }
            [Category("Scaled values"), RefreshProperties(RefreshProperties.All), Description("Editable game value (raw / 255), rounded to the nearest stored step.")] public float WidthMiddleValue { get { return entry.WidthMiddle / 255f; } set { entry.WidthMiddle = (ushort)ToScaleRaw(value, ushort.MaxValue); } }
            [Category("Parameters"), Description("Offset 0x58. ")] public ushort WidthEnd { get { return entry.WidthEnd; } set { entry.WidthEnd = value; } }
            [Category("Scaled values"), RefreshProperties(RefreshProperties.All), Description("Editable game value (raw / 255), rounded to the nearest stored step.")] public float WidthEndValue { get { return entry.WidthEnd / 255f; } set { entry.WidthEnd = (ushort)ToScaleRaw(value, ushort.MaxValue); } }
            [Category("Parameters"), Description("Offset 0x5A. Middle width position in the history; raw / 255.")] public byte WidthMiddlePoint { get { return entry.WidthMiddlePoint; } set { entry.WidthMiddlePoint = value; } }
            [Category("Scaled values"), RefreshProperties(RefreshProperties.All), Description("Editable game value (raw / 255), rounded to the nearest stored step.")] public float WidthMiddlePointValue { get { return entry.WidthMiddlePoint / 255f; } set { entry.WidthMiddlePoint = (byte)ToScaleRaw(value, byte.MaxValue); } }
            [Category("Parameters"), Description("Offset 0x5B. Unknown use: copied byte, seen as 0x00 and 0x20. Preserve it.")] public byte Field5B { get { return entry.Field5B; } set { entry.Field5B = value; } }
            [Category("Parameters"), Description("Offset 0x5C. Unknown: copied without byte swapping; no confirmed game use.")] public uint Field5C { get { return entry.Field5C; } set { entry.Field5C = value; } }
        }

        private sealed class TrailResourceViewModel : IChunkViewModel
        {
            private readonly TrailResourceEntry entry;
            public TrailResourceViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailResourceEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            [Browsable(false)] public Tool_TrailEditor Editor { get; private set; }
            [Browsable(false)] public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int EffectChunkMapId { get { return entry.EffectChunkMapId; } set { entry.EffectChunkMapId = value; } }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntryIndex { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Parameters"), Description("Offset 0x08. Replaced on load: resolved EffectChunkIndex pointer.")] public uint EffectRef { get { return entry.EffectRef; } set { entry.EffectRef = value; } }
            [Category("Parameters"), Description("Offset 0x0C. Unused by game loader: skipped prefix word.")] public uint Field0C { get { return entry.Field0C; } set { entry.Field0C = value; } }
            [Category("Parameters"), Description("Offset 0x10. Used as cached billboard pointer; replaced by lookup when CacheState is 0.")] public uint BillboardPtr { get { return entry.BillboardPtr; } set { entry.BillboardPtr = value; } }
            [Category("Parameters"), Description("Offset 0x14. 0 allows lookup; runtime becomes 2. Keep file value unchanged.")] public uint CacheState { get { return entry.CacheState; } set { entry.CacheState = value; } }
            [Category("Parameters"), Description("Offset 0x18. Unknown: copied beside cache state; no confirmed game use.")] public uint Field18 { get { return entry.Field18; } set { entry.Field18 = value; } }
            [Category("Parameters"), Description("Offset 0x1C. Unknown: copied resource tail; no confirmed game use.")] public uint Field1C { get { return entry.Field1C; } set { entry.Field1C = value; } }
        }

        private sealed class TrailPositionViewModel : IChunkViewModel
        {
            private readonly TrailPositionEntry entry;
            public TrailPositionViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailPositionEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            [Browsable(false)] public Tool_TrailEditor Editor { get; private set; }
            [Browsable(false)] public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int CoordChunkMapId { get { return entry.CoordChunkMapId; } set { entry.CoordChunkMapId = value; } }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntryIndex { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Parameters"), Description("Offset 0x08. Replaced on load: resolved CoordChunkIndex pointer.")] public int CoordRef { get { return entry.CoordRef; } set { entry.CoordRef = value; } }
            [Category("Parameters"), Description("Offset 0x0C. Unknown prefix word: no confirmed game use.")] public int Field0C { get { return entry.Field0C; } set { entry.Field0C = value; } }
            [Category("Parameters"), Description("Offset 0x10. Unknown: copied; no endpoint offset or other game use established.")] public int Field10 { get { return entry.Field10; } set { entry.Field10 = value; } }
            [Category("Parameters"), Description("Offset 0x14. Unknown: copied; no confirmed game use.")] public int Field14 { get { return entry.Field14; } set { entry.Field14 = value; } }
            [Category("Parameters"), Description("Offset 0x18. Unknown: copied; no confirmed game use.")] public int Field18 { get { return entry.Field18; } set { entry.Field18 = value; } }
            [Category("Parameters"), Description("Offset 0x1C. Unknown: copied; no confirmed game use.")] public int Field1C { get { return entry.Field1C; } set { entry.Field1C = value; } }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int ClumpChunkMapId { get { return entry.ClumpChunkMapId; } set { entry.ClumpChunkMapId = value; } }
            [Category("Parameters"), Description("Offset 0x24. Replaced on load when ClumpChunkIndex != -1: clump reference pointer.")] public int ClumpRef { get { return entry.ClumpRef; } set { entry.ClumpRef = value; } }
            [Category("Parameters"), Description("Offset 0x28. Unused by game loader (0x79+): not mapped into runtime data.")] public int Field28 { get { return entry.Field28; } set { entry.Field28 = value; } }
            [Category("Parameters"), Description("Offset 0x2C. Unused by game loader (0x79+): not mapped into runtime data.")] public int Field2C { get { return entry.Field2C; } set { entry.Field2C = value; } }
        }

        private sealed class TrailForceFieldViewModel : IChunkViewModel
        {
            private readonly TrailForceFieldEntry entry;
            public TrailForceFieldViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailForceFieldEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            [Browsable(false)] public Tool_TrailEditor Editor { get; private set; }
            [Browsable(false)] public TrailChunkState Chunk { get; private set; }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int CoordChunkMapId { get { return entry.CoordChunkMapId; } set { entry.CoordChunkMapId = value; } }
            [Category("Links"), TypeConverter(typeof(ManagerReferenceConverter))] public int TrailEntryIndex { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Parameters"), Description("Offset 0x08. Replaced on load: resolved CoordChunkIndex pointer.")] public int CoordRef { get { return entry.CoordRef; } set { entry.CoordRef = value; } }
            [Category("Parameters"), Description("Offset 0x0C. Unknown prefix word: no confirmed game use.")] public int Field0C { get { return entry.Field0C; } set { entry.Field0C = value; } }
            [Category("Parameters"), Description("Offset 0x10. ")] public float DirectionX { get { return entry.DirectionX; } set { entry.DirectionX = value; } }
            [Category("Parameters"), Description("Offset 0x14. ")] public float DirectionY { get { return entry.DirectionY; } set { entry.DirectionY = value; } }
            [Category("Parameters"), Description("Offset 0x18. ")] public float DirectionZ { get { return entry.DirectionZ; } set { entry.DirectionZ = value; } }
            [Category("Parameters"), Description("Offset 0x1C. Each update: velocity += velocity * value.")] public float VelocityGrowth { get { return entry.VelocityGrowth; } set { entry.VelocityGrowth = value; } }
            [Category("Parameters"), Description("Offset 0x20. ")] public TrailForceType ForceType { get { return entry.ForceType; } set { entry.ForceType = value; } }
            [Category("Parameters"), Description("Offset 0x24. Game multiplies this value by 100.")] public float Radius { get { return entry.Radius; } set { entry.Radius = value; } }
            [Category("Parameters"), Description("Offset 0x28. ")] public float Strength { get { return entry.Strength; } set { entry.Strength = value; } }
            [Category("Parameters"), Description("Offset 0x2C. Named bits are used; other bits have no confirmed game use.")] public TrailForceFlags ForceFlags { get { return entry.ForceFlags; } set { entry.ForceFlags = value; } }
            [Category("Links"), TypeConverter(typeof(MapReferenceConverter))] public int ClumpChunkMapId { get { return entry.ClumpChunkMapId; } set { entry.ClumpChunkMapId = value; } }
            [Category("Parameters"), Description("Offset 0x34. Replaced on load when ClumpChunkIndex != -1: clump reference pointer.")] public int ClumpRef { get { return entry.ClumpRef; } set { entry.ClumpRef = value; } }
            [Category("Parameters"), Description("Offset 0x38. Unused by game loader (0x79+): not mapped into runtime data.")] public int Field38 { get { return entry.Field38; } set { entry.Field38 = value; } }
            [Category("Parameters"), Description("Offset 0x3C. Unused by game loader (0x79+): not mapped into runtime data.")] public int Field3C { get { return entry.Field3C; } set { entry.Field3C = value; } }
        }

        private sealed class TrailNodeViewModel : IChunkViewModel
        {
            private readonly TrailNodeEntry entry;
            public TrailNodeViewModel(Tool_TrailEditor editor, TrailChunkState chunk, TrailNodeEntry entry) { Editor = editor; Chunk = chunk; this.entry = entry; }
            [Browsable(false)] public Tool_TrailEditor Editor { get; private set; }
            [Browsable(false)] public TrailChunkState Chunk { get; private set; }
            [Category("Stored metadata"), Description("Unused stored ID. Timelines attach to managers by node order.")] public int TrailEntry { get { return entry.TrailEntryIndex; } set { entry.TrailEntryIndex = value; } }
            [Category("Main")] public int FrameCount { get { return entry.Frames.Count; } }
            [Category("Unknown")] public uint Field00 { get { return entry.Field00; } set { entry.Field00 = value; } }
            [Category("Unknown")] public uint Field08 { get { return entry.Field08; } set { entry.Field08 = value; } }
            [Category("Unknown")] public uint Field0C { get { return entry.Field0C; } set { entry.Field0C = value; } }
        }

        private sealed class TrailFrameViewModel
        {
            private readonly TrailFrameEntry entry;
            public TrailFrameViewModel(TrailFrameEntry entry) { this.entry = entry; }
            [Category("Main")] public bool Enabled { get { return entry.Enabled; } set { entry.Enabled = value; } }
            [Category("Main")] [Description("Ticks in version 0x79 and later; legacy units in older versions.")] public uint TimeUnits { get { return entry.TimeUnits; } set { entry.TimeUnits = value; } }
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

        private void managersPropertyGrid_Click(object sender, EventArgs e)
        {

        }

        private void resourcesListBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void managersSplitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void managersSplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void resourcesSplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void positionsSplitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
