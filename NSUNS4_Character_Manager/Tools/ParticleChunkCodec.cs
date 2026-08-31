using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NSUNS4_Character_Manager
{
    internal enum ParticleSpawnType
    {
        SpawnPoint = 0,
        SpawnPlanarCircle = 1,
        SpawnSphere = 2,
        SpawnTubeRandomEnd = 3,
        SpawnTubeForward = 4,
        SpawnTubeReverse = 5
    }

    internal enum ParticleDirectionType : byte
    {
        DirectionOutward = 0,
        DirectionInward = 1,
        DirectionRandomXYZ = 2,
        DirectionLocalCone = 3
    }

    internal enum ParticleRotationType : byte
    {
        RotationZero = 0,
        RotationRandomXYZ = 1,
        RotationAxisAligned = 2,
        RotationAxisRandomY = 3
    }

    internal enum ParticleEffectChunkType : uint
    {
        nuccChunkClump = 1,
        nuccChunkAnm = 2,
        nuccChunkSprite = 3,
        nuccChunkSprite2 = 4,
        nuccChunkBillboard = 5
    }

    internal static class ParticleChunkCodec
    {
        public const string ChunkType = "nuccChunkParticle";
        public const string FileExtension = ".particle";
        public const int SectionCount = 5;
        public const int HeaderTableSize = 0x28;
        public const int ParticleManagerSize = 0xD0;
        public const int ParticleResourceSize = 0x20;
        public const int ParticlePositionLegacySize = 0x30;
        public const int ParticlePositionSize = 0x38;
        public const int ParticleForceFieldLegacySize = 0x60;
        public const int ParticleForceFieldSize = 0x70;
        public static readonly uint[] DefaultHeaderValues = { 0x28u, 0x0D28u, 0x1768u, 0x21F8u, 0x21F8u };

        public static ParticleChunkState CreateDefaultChunk()
        {
            ParticleChunkState chunk = new ParticleChunkState
            {
                ChunkName = "new_particle",
                ChunkPath = @"c\particle\new_particle.max",
                Version = 121,
                VersionAttribute = 2024
            };

            for (int i = 0; i < chunk.Headers.Length; i++)
                chunk.Headers[i].Value = DefaultHeaderValues[i];

            return chunk;
        }

        public static ParticleChunkState CloneChunk(ParticleChunkState source)
        {
            ParticleChunkState clone = new ParticleChunkState
            {
                OriginalChunkName = source.OriginalChunkName,
                ChunkName = source.ChunkName,
                ChunkPath = source.ChunkPath,
                Version = source.Version,
                VersionAttribute = source.VersionAttribute,
                UsesVersion78Layout = source.UsesVersion78Layout,
                DeletePending = source.DeletePending
            };

            for (int i = 0; i < source.Headers.Length; i++)
            {
                clone.Headers[i].Value = source.Headers[i].Value;
                clone.Headers[i].Count = source.Headers[i].Count;
                clone.Headers[i].Size = source.Headers[i].Size;
            }

            clone.Managers.AddRange(source.Managers.Select(CloneManager));
            clone.Resources.AddRange(source.Resources.Select(CloneResource));
            clone.Positions.AddRange(source.Positions.Select(ClonePosition));
            clone.ForceFields.AddRange(source.ForceFields.Select(CloneForceField));
            clone.Nodes.AddRange(source.Nodes.Select(CloneNode));
            clone.References.AddRange(CloneReferences(source.References));
            clone.ExtendedData = source.ExtendedData != null ? (byte[])source.ExtendedData.Clone() : new byte[0];
            return clone;
        }

        public static ParticleManagerEntry CloneManager(ParticleManagerEntry entry)
        {
            return new ParticleManagerEntry
            {
                AnimationChunkIndex = entry.AnimationChunkIndex,
                EntryIndex = entry.EntryIndex,
                Field08 = entry.Field08,
                Field0C = entry.Field0C,
                AllocationMode = entry.AllocationMode,
                SpawnType = entry.SpawnType,
                DirectionType = entry.DirectionType,
                ParticleInstanceMode = entry.ParticleInstanceMode,
                RotationType = entry.RotationType,
                ControlRate = entry.ControlRate,
                GeneratorFlags = entry.GeneratorFlags,
                ParticleBehaviorType = entry.ParticleBehaviorType,
                ParticleInstanceFlag0 = entry.ParticleInstanceFlag0,
                ParticleInstanceFlag1 = entry.ParticleInstanceFlag1,
                ParticleInstanceFlag2 = entry.ParticleInstanceFlag2,
                ParticleInstanceFlag3 = entry.ParticleInstanceFlag3,
                GeneratorEndTime = entry.GeneratorEndTime,
                ExtendedDataFlags = entry.ExtendedDataFlags,
                ParticleCountOrRate = entry.ParticleCountOrRate,
                SpawnRadius = entry.SpawnRadius,
                SpawnRadiusRandomness = entry.SpawnRadiusRandomness,
                Lifetime = entry.Lifetime,
                Reserved2E = entry.Reserved2E,
                LifetimeRandomness = entry.LifetimeRandomness,
                InitialSpeed = entry.InitialSpeed,
                InitialSpeedRandomness = entry.InitialSpeedRandomness,
                EmissionAngle1 = entry.EmissionAngle1,
                EmissionAngle2 = entry.EmissionAngle2,
                EmissionAngle1Randomness = entry.EmissionAngle1Randomness,
                EmissionAngle2Randomness = entry.EmissionAngle2Randomness,
                FadeParameter1 = entry.FadeParameter1,
                FadeParameter2 = entry.FadeParameter2,
                InitialRotation = entry.InitialRotation,
                InitialRotationRandomness = entry.InitialRotationRandomness,
                ScaleStartX = entry.ScaleStartX,
                ScaleStartY = entry.ScaleStartY,
                ScaleStartZ = entry.ScaleStartZ,
                AddRandomScaleStartX = entry.AddRandomScaleStartX,
                AddRandomScaleStartY = entry.AddRandomScaleStartY,
                AddRandomScaleStartZ = entry.AddRandomScaleStartZ,
                ScaleMiddleX = entry.ScaleMiddleX,
                ScaleMiddleY = entry.ScaleMiddleY,
                ScaleMiddleZ = entry.ScaleMiddleZ,
                ScaleEndX = entry.ScaleEndX,
                ScaleEndY = entry.ScaleEndY,
                ScaleEndZ = entry.ScaleEndZ,
                ScaleInterpolationPoint = entry.ScaleInterpolationPoint,
                ColorStartR = entry.ColorStartR,
                ColorStartG = entry.ColorStartG,
                ColorStartB = entry.ColorStartB,
                ColorStartA = entry.ColorStartA,
                ColorMiddleR = entry.ColorMiddleR,
                ColorMiddleG = entry.ColorMiddleG,
                ColorMiddleB = entry.ColorMiddleB,
                ColorMiddleA = entry.ColorMiddleA,
                ColorEndR = entry.ColorEndR,
                ColorEndG = entry.ColorEndG,
                ColorEndB = entry.ColorEndB,
                ColorEndA = entry.ColorEndA,
                ColorInterpolationPoint = entry.ColorInterpolationPoint,
                UnknownC4 = entry.UnknownC4,
                UnknownC8 = entry.UnknownC8,
                UnknownCC = entry.UnknownCC
            };
        }

        public static ParticleResourceEntry CloneResource(ParticleResourceEntry entry)
        {
            return new ParticleResourceEntry
            {
                EffectChunkIndex = entry.EffectChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Field08 = entry.Field08,
                Field0C = entry.Field0C,
                Parameter10 = entry.Parameter10,
                Parameter12 = entry.Parameter12,
                Parameter14 = entry.Parameter14,
                Parameter16 = entry.Parameter16,
                Parameter18 = entry.Parameter18,
                Parameter1A = entry.Parameter1A,
                EffectChunkType = entry.EffectChunkType
            };
        }

        public static ParticlePositionEntry ClonePosition(ParticlePositionEntry entry)
        {
            return new ParticlePositionEntry
            {
                CoordChunkIndex = entry.CoordChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Field08 = entry.Field08,
                Field0C = entry.Field0C,
                Field10 = entry.Field10,
                Field14 = entry.Field14,
                Field18 = entry.Field18,
                Field1C = entry.Field1C,
                Field20 = entry.Field20,
                Field24 = entry.Field24,
                Field28 = entry.Field28,
                Field2C = entry.Field2C,
                ClumpChunkIndex = entry.ClumpChunkIndex,
                Field34 = entry.Field34,
                HasVersion78Fields = entry.HasVersion78Fields
            };
        }

        public static ParticleForceFieldEntry CloneForceField(ParticleForceFieldEntry entry)
        {
            return new ParticleForceFieldEntry
            {
                CoordChunkIndex = entry.CoordChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Field08 = entry.Field08,
                Field0C = entry.Field0C,
                Field10 = entry.Field10,
                Field14 = entry.Field14,
                Field18 = entry.Field18,
                Field1C = entry.Field1C,
                Field20 = entry.Field20,
                Field24 = entry.Field24,
                Field28 = entry.Field28,
                Field2C = entry.Field2C,
                Field30 = entry.Field30,
                Field32 = entry.Field32,
                Field34 = entry.Field34,
                Field38 = entry.Field38,
                Field3C = entry.Field3C,
                Field40 = entry.Field40,
                Field44 = entry.Field44,
                Field48 = entry.Field48,
                Field4A = entry.Field4A,
                Field4C = entry.Field4C,
                Field4E = entry.Field4E,
                Field50 = entry.Field50,
                ParticleSpeed = entry.ParticleSpeed,
                Field58 = entry.Field58,
                Field5C = entry.Field5C,
                ClumpChunkIndex = entry.ClumpChunkIndex,
                Field64 = entry.Field64,
                Field68 = entry.Field68,
                Field6C = entry.Field6C,
                HasVersion78Fields = entry.HasVersion78Fields
            };
        }

        public static ParticleNodeEntry CloneNode(ParticleNodeEntry entry)
        {
            ParticleNodeEntry clone = new ParticleNodeEntry();
            clone.Frames.AddRange(entry.Frames.Select(x => new ParticleFrameEntry { RawValue = x.RawValue }));
            clone.Padding = entry.Padding != null ? (byte[])entry.Padding.Clone() : new byte[0];
            return clone;
        }

        public static List<ParticleChunkReferenceEntry> CloneReferences(IEnumerable<ParticleChunkReferenceEntry> entries)
        {
            return entries.Select(x => new ParticleChunkReferenceEntry
            {
                Name = x.Name,
                Type = x.Type,
                Path = x.Path
            }).ToList();
        }

        public static string BuildChunkLabel(ParticleChunkState chunk)
        {
            string path = string.IsNullOrWhiteSpace(chunk.ChunkPath) ? "(no path)" : chunk.ChunkPath;
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} | {1} | {2} | {3} | Links:{4} M:{5} R:{6} P:{7} F:{8} T:{9} Ext:{10}B",
                string.IsNullOrWhiteSpace(chunk.ChunkName) ? "(unnamed chunk)" : chunk.ChunkName,
                chunk.Version == 0 ? "version not supplied" : "v0x" + chunk.Version.ToString("X2", CultureInfo.InvariantCulture),
                chunk.UsesVersion78Layout ? "v0x78+ entry layout" : "legacy entry layout",
                path,
                chunk.References.Count,
                chunk.Managers.Count,
                chunk.Resources.Count,
                chunk.Positions.Count,
                chunk.ForceFields.Count,
                chunk.Nodes.Count,
                chunk.ExtendedData != null ? chunk.ExtendedData.Length : 0);
        }

        public static List<ParticleChunkReferenceEntry> ExtractReferenceEntries(XfbinParserPageDefinition definition, string chunkName, string chunkType)
        {
            List<ParticleChunkReferenceEntry> result = new List<ParticleChunkReferenceEntry>();
            if (definition == null || definition.ChunkMaps == null)
                return result;

            foreach (XfbinParserChunkMap map in definition.ChunkMaps)
            {
                if (map == null)
                    continue;
                if (string.Equals(map.Type, "nuccChunkNull", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(map.Type, "nuccChunkPage", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(map.Type, "nuccChunkIndex", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (string.Equals(map.Type, chunkType, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(map.Name, chunkName, StringComparison.OrdinalIgnoreCase))
                    continue;

                result.Add(new ParticleChunkReferenceEntry
                {
                    Name = map.Name ?? "",
                    Type = map.Type ?? "",
                    Path = map.Path ?? ""
                });
            }

            return result;
        }

        public static List<XfbinParserChunkMap> BuildReferenceChunkMaps(IEnumerable<ParticleChunkReferenceEntry> references)
        {
            return references.Select(x => new XfbinParserChunkMap
            {
                Name = x.Name ?? "",
                Type = x.Type ?? "",
                Path = x.Path ?? ""
            }).ToList();
        }

        public static string ResolveReferenceLabel(ParticleChunkState chunk, uint referenceIndex)
        {
            if (chunk == null)
                return "(none)";
            if (referenceIndex >= chunk.References.Count)
                return "(invalid index " + referenceIndex.ToString(CultureInfo.InvariantCulture) + ")";

            ParticleChunkReferenceEntry entry = chunk.References[(int)referenceIndex];
            string name = string.IsNullOrWhiteSpace(entry.Name) ? "(unnamed)" : entry.Name;
            return name + " [" + entry.Type + "]";
        }

        public static string ResolveReferenceLabel(ParticleChunkState chunk, int referenceIndex)
        {
            if (referenceIndex < 0)
                return "(none: " + referenceIndex.ToString(CultureInfo.InvariantCulture) + ")";
            return ResolveReferenceLabel(chunk, (uint)referenceIndex);
        }

        public static List<ParticleNodeEvent> DecodeNodeEvents(ParticleNodeEntry node)
        {
            List<ParticleNodeEvent> events = new List<ParticleNodeEvent>();
            if (node == null)
                return events;

            foreach (ParticleFrameEntry frame in node.Frames)
            {
                events.Add(new ParticleNodeEvent
                {
                    Action = (frame.RawValue & 0x80000000u) == 0 ? ParticleNodeAction.Off : ParticleNodeAction.On,
                    TimeMilliseconds = frame.RawValue & 0x7FFFFFFFu
                });
            }

            return events;
        }

        public static ParticleNodeEntry EncodeNodeEvents(IEnumerable<ParticleNodeEvent> events)
        {
            ParticleNodeEntry node = new ParticleNodeEntry();
            foreach (ParticleNodeEvent particleEvent in events)
            {
                node.Frames.Add(new ParticleFrameEntry
                {
                    RawValue = particleEvent.TimeMilliseconds | (particleEvent.Action == ParticleNodeAction.On ? 0x80000000u : 0u)
                });
            }

            return node;
        }

        public static bool TryParseChunk(XfbinBinaryChunkItem item, out ParticleChunkState chunk)
        {
            chunk = null;
            byte[] bytes = item.BinaryData;
            if (bytes == null || bytes.Length < HeaderTableSize)
                return false;

            ParticleChunkState state = new ParticleChunkState
            {
                OriginalChunkName = item.ChunkName ?? "",
                ChunkName = item.ChunkName ?? "",
                ChunkPath = item.ChunkPath ?? "",
                Version = item.Version,
                VersionAttribute = item.VersionAttribute
            };

            for (int i = 0; i < SectionCount; i++)
            {
                int headerOffset = i * 8;
                state.Headers[i].Value = ReadUInt32BE(bytes, headerOffset);
                state.Headers[i].Count = ReadUInt16BE(bytes, headerOffset + 4);
                state.Headers[i].Size = ReadUInt16BE(bytes, headerOffset + 6);
            }

            state.UsesVersion78Layout = DetectVersion78Layout(state);

            int managerOffset = HeaderTableSize;
            int resourceOffset = managerOffset + state.Headers[0].Size;
            int positionOffset = resourceOffset + state.Headers[1].Size;
            int forceFieldOffset = positionOffset + state.Headers[2].Size;
            int nodeOffset = forceFieldOffset + state.Headers[3].Size;

            if (nodeOffset + state.Headers[4].Size > bytes.Length)
                return false;

            if (!TryParseManagers(bytes, managerOffset, state.Headers[0], state.Managers))
                return false;
            if (!TryParseResources(bytes, resourceOffset, state.Headers[1], state.Resources))
                return false;
            if (!TryParsePositions(bytes, positionOffset, state.Headers[2], state.Positions))
                return false;
            if (!TryParseForceFields(bytes, forceFieldOffset, state.Headers[3], state.ForceFields))
                return false;
            if (!TryParseNodes(bytes, nodeOffset, state.Headers[4], state.Nodes))
                return false;

            int extendedDataOffset = nodeOffset + state.Headers[4].Size;
            if (extendedDataOffset < bytes.Length)
            {
                state.ExtendedData = new byte[bytes.Length - extendedDataOffset];
                Buffer.BlockCopy(bytes, extendedDataOffset, state.ExtendedData, 0, state.ExtendedData.Length);
            }

            chunk = state;
            return true;
        }

        public static byte[] BuildChunkData(ParticleChunkState chunk)
        {
            byte[] managerBytes = BuildManagers(chunk.Managers);
            byte[] resourceBytes = BuildResources(chunk.Resources);
            byte[] positionBytes = BuildPositions(chunk.Positions, chunk.UsesVersion78Layout);
            byte[] forceFieldBytes = BuildForceFields(chunk.ForceFields, chunk.UsesVersion78Layout);
            byte[] nodeBytes = BuildNodes(chunk.Nodes);
            byte[] extendedData = chunk.ExtendedData ?? new byte[0];

            byte[] output = new byte[HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length + nodeBytes.Length + extendedData.Length];
            Buffer.BlockCopy(managerBytes, 0, output, HeaderTableSize, managerBytes.Length);
            Buffer.BlockCopy(resourceBytes, 0, output, HeaderTableSize + managerBytes.Length, resourceBytes.Length);
            Buffer.BlockCopy(positionBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length, positionBytes.Length);
            Buffer.BlockCopy(forceFieldBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length, forceFieldBytes.Length);
            Buffer.BlockCopy(nodeBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length, nodeBytes.Length);
            Buffer.BlockCopy(extendedData, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length + nodeBytes.Length, extendedData.Length);

            WriteSectionHeader(output, 0x00, chunk.Headers[0].Value == 0 ? DefaultHeaderValues[0] : chunk.Headers[0].Value, chunk.Managers.Count, managerBytes.Length);
            WriteSectionHeader(output, 0x08, chunk.Headers[1].Value == 0 ? DefaultHeaderValues[1] : chunk.Headers[1].Value, chunk.Resources.Count, resourceBytes.Length);
            WriteSectionHeader(output, 0x10, chunk.Headers[2].Value == 0 ? DefaultHeaderValues[2] : chunk.Headers[2].Value, chunk.Positions.Count, positionBytes.Length);
            WriteSectionHeader(output, 0x18, chunk.Headers[3].Value == 0 ? DefaultHeaderValues[3] : chunk.Headers[3].Value, chunk.ForceFields.Count, forceFieldBytes.Length);
            WriteSectionHeader(output, 0x20, chunk.Headers[4].Value == 0 ? DefaultHeaderValues[4] : chunk.Headers[4].Value, chunk.Nodes.Count, nodeBytes.Length);
            return output;
        }

        private static void WriteSectionHeader(byte[] output, int offset, uint value, int count, int size)
        {
            WriteUInt32BE(output, offset, value);
            WriteUInt16BE(output, offset + 4, checked((ushort)count));
            WriteUInt16BE(output, offset + 6, checked((ushort)size));
        }

        private static bool DetectVersion78Layout(ParticleChunkState chunk)
        {
            if (chunk.Headers[2].Count > 0)
                return chunk.Headers[2].Size == chunk.Headers[2].Count * ParticlePositionSize;
            if (chunk.Headers[3].Count > 0)
                return chunk.Headers[3].Size == chunk.Headers[3].Count * ParticleForceFieldSize;
            return chunk.Version == 0 || chunk.Version > 0x77;
        }

        private static bool TryParseManagers(byte[] bytes, int offset, ParticleSectionHeaderState header, List<ParticleManagerEntry> output)
        {
            output.Clear();
            if (header.Count == 0)
                return header.Size == 0;
            if (header.Size != header.Count * ParticleManagerSize)
                return false;

            for (int i = 0; i < header.Count; i++)
            {
                int e = offset + (i * ParticleManagerSize);
                output.Add(new ParticleManagerEntry
                {
                    AnimationChunkIndex = ReadUInt32BE(bytes, e + 0x00),
                    EntryIndex = ReadUInt32BE(bytes, e + 0x04),
                    Field08 = ReadUInt32BE(bytes, e + 0x08),
                    Field0C = ReadUInt32BE(bytes, e + 0x0C),
                    AllocationMode = bytes[e + 0x10],
                    SpawnType = (ParticleSpawnType)bytes[e + 0x11],
                    DirectionType = (ParticleDirectionType)bytes[e + 0x12],
                    ParticleInstanceMode = bytes[e + 0x13],
                    RotationType = (ParticleRotationType)bytes[e + 0x14],
                    ControlRate = bytes[e + 0x15],
                    GeneratorFlags = bytes[e + 0x16],
                    ParticleBehaviorType = bytes[e + 0x17],
                    ParticleInstanceFlag0 = bytes[e + 0x18],
                    ParticleInstanceFlag1 = bytes[e + 0x19],
                    ParticleInstanceFlag2 = bytes[e + 0x1A],
                    ParticleInstanceFlag3 = bytes[e + 0x1B],
                    GeneratorEndTime = ReadInt16BE(bytes, e + 0x1C),
                    ExtendedDataFlags = ReadUInt16BE(bytes, e + 0x1E),
                    ParticleCountOrRate = ReadSingleBE(bytes, e + 0x20),
                    SpawnRadius = ReadSingleBE(bytes, e + 0x24),
                    SpawnRadiusRandomness = ReadSingleBE(bytes, e + 0x28),
                    Lifetime = ReadUInt16BE(bytes, e + 0x2C),
                    Reserved2E = ReadUInt16BE(bytes, e + 0x2E),
                    LifetimeRandomness = ReadSingleBE(bytes, e + 0x30),
                    InitialSpeed = ReadSingleBE(bytes, e + 0x34),
                    InitialSpeedRandomness = ReadSingleBE(bytes, e + 0x38),
                    EmissionAngle1 = ReadSingleBE(bytes, e + 0x3C),
                    EmissionAngle2 = ReadSingleBE(bytes, e + 0x40),
                    EmissionAngle1Randomness = ReadSingleBE(bytes, e + 0x44),
                    EmissionAngle2Randomness = ReadSingleBE(bytes, e + 0x48),
                    FadeParameter1 = ReadSingleBE(bytes, e + 0x4C),
                    FadeParameter2 = ReadSingleBE(bytes, e + 0x50),
                    InitialRotation = ReadSingleBE(bytes, e + 0x54),
                    InitialRotationRandomness = ReadSingleBE(bytes, e + 0x58),
                    ScaleStartX = ReadSingleBE(bytes, e + 0x5C),
                    ScaleStartY = ReadSingleBE(bytes, e + 0x60),
                    ScaleStartZ = ReadSingleBE(bytes, e + 0x64),
                    AddRandomScaleStartX = ReadSingleBE(bytes, e + 0x68),
                    AddRandomScaleStartY = ReadSingleBE(bytes, e + 0x6C),
                    AddRandomScaleStartZ = ReadSingleBE(bytes, e + 0x70),
                    ScaleMiddleX = ReadSingleBE(bytes, e + 0x74),
                    ScaleMiddleY = ReadSingleBE(bytes, e + 0x78),
                    ScaleMiddleZ = ReadSingleBE(bytes, e + 0x7C),
                    ScaleEndX = ReadSingleBE(bytes, e + 0x80),
                    ScaleEndY = ReadSingleBE(bytes, e + 0x84),
                    ScaleEndZ = ReadSingleBE(bytes, e + 0x88),
                    ScaleInterpolationPoint = ReadSingleBE(bytes, e + 0x8C),
                    ColorStartR = ReadSingleBE(bytes, e + 0x90),
                    ColorStartG = ReadSingleBE(bytes, e + 0x94),
                    ColorStartB = ReadSingleBE(bytes, e + 0x98),
                    ColorStartA = ReadSingleBE(bytes, e + 0x9C),
                    ColorMiddleR = ReadSingleBE(bytes, e + 0xA0),
                    ColorMiddleG = ReadSingleBE(bytes, e + 0xA4),
                    ColorMiddleB = ReadSingleBE(bytes, e + 0xA8),
                    ColorMiddleA = ReadSingleBE(bytes, e + 0xAC),
                    ColorEndR = ReadSingleBE(bytes, e + 0xB0),
                    ColorEndG = ReadSingleBE(bytes, e + 0xB4),
                    ColorEndB = ReadSingleBE(bytes, e + 0xB8),
                    ColorEndA = ReadSingleBE(bytes, e + 0xBC),
                    ColorInterpolationPoint = ReadSingleBE(bytes, e + 0xC0),
                    UnknownC4 = ReadUInt32BE(bytes, e + 0xC4),
                    UnknownC8 = ReadUInt32BE(bytes, e + 0xC8),
                    UnknownCC = ReadUInt32BE(bytes, e + 0xCC)
                });
            }

            return true;
        }

        private static bool TryParseResources(byte[] bytes, int offset, ParticleSectionHeaderState header, List<ParticleResourceEntry> output)
        {
            output.Clear();
            if (header.Count == 0)
                return header.Size == 0;
            if (header.Size != header.Count * ParticleResourceSize)
                return false;

            for (int i = 0; i < header.Count; i++)
            {
                int e = offset + (i * ParticleResourceSize);
                output.Add(new ParticleResourceEntry
                {
                    EffectChunkIndex = ReadUInt32BE(bytes, e + 0x00),
                    ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04),
                    Field08 = ReadInt32BE(bytes, e + 0x08),
                    Field0C = ReadInt32BE(bytes, e + 0x0C),
                    Parameter10 = ReadUInt16BE(bytes, e + 0x10) / 65535f,
                    Parameter12 = ReadUInt16BE(bytes, e + 0x12) / 65535f,
                    Parameter14 = ReadUInt16BE(bytes, e + 0x14) / 65535f,
                    Parameter16 = ReadUInt16BE(bytes, e + 0x16) / 65535f,
                    Parameter18 = ReadUInt16BE(bytes, e + 0x18) / 65535f,
                    Parameter1A = ReadUInt16BE(bytes, e + 0x1A) / 65535f,
                    EffectChunkType = (ParticleEffectChunkType)ReadUInt32BE(bytes, e + 0x1C)
                });
            }

            return true;
        }

        private static bool TryParsePositions(byte[] bytes, int offset, ParticleSectionHeaderState header, List<ParticlePositionEntry> output)
        {
            output.Clear();
            if (header.Count == 0)
                return header.Size == 0;
            bool hasVersion78Fields = header.Size == header.Count * ParticlePositionSize;
            int entrySize = hasVersion78Fields ? ParticlePositionSize : ParticlePositionLegacySize;
            if (header.Size != header.Count * entrySize)
                return false;

            for (int i = 0; i < header.Count; i++)
            {
                int e = offset + (i * entrySize);
                ParticlePositionEntry entry = new ParticlePositionEntry
                {
                    CoordChunkIndex = ReadInt32BE(bytes, e + 0x00),
                    ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04),
                    Field08 = ReadInt32BE(bytes, e + 0x08),
                    Field0C = ReadInt32BE(bytes, e + 0x0C),
                    Field10 = ReadInt32BE(bytes, e + 0x10),
                    Field14 = ReadSingleBE(bytes, e + 0x14),
                    Field18 = ReadSingleBE(bytes, e + 0x18),
                    Field1C = ReadInt32BE(bytes, e + 0x1C),
                    Field20 = ReadInt32BE(bytes, e + 0x20),
                    Field24 = ReadInt32BE(bytes, e + 0x24),
                    Field28 = ReadInt32BE(bytes, e + 0x28),
                    Field2C = ReadInt32BE(bytes, e + 0x2C)
                };
                entry.HasVersion78Fields = hasVersion78Fields;
                if (hasVersion78Fields)
                {
                    entry.ClumpChunkIndex = ReadInt32BE(bytes, e + 0x30);
                    entry.Field34 = ReadUInt32BE(bytes, e + 0x34);
                }
                output.Add(entry);
            }

            return true;
        }

        private static bool TryParseForceFields(byte[] bytes, int offset, ParticleSectionHeaderState header, List<ParticleForceFieldEntry> output)
        {
            output.Clear();
            if (header.Count == 0)
                return header.Size == 0;
            bool hasVersion78Fields = header.Size == header.Count * ParticleForceFieldSize;
            int entrySize = hasVersion78Fields ? ParticleForceFieldSize : ParticleForceFieldLegacySize;
            if (header.Size != header.Count * entrySize)
                return false;

            for (int i = 0; i < header.Count; i++)
            {
                int e = offset + (i * entrySize);
                ParticleForceFieldEntry entry = new ParticleForceFieldEntry
                {
                    CoordChunkIndex = ReadInt32BE(bytes, e + 0x00),
                    ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04),
                    Field08 = ReadInt32BE(bytes, e + 0x08),
                    Field0C = ReadInt32BE(bytes, e + 0x0C),
                    Field10 = ReadInt32BE(bytes, e + 0x10),
                    Field14 = ReadSingleBE(bytes, e + 0x14),
                    Field18 = ReadSingleBE(bytes, e + 0x18),
                    Field1C = ReadInt32BE(bytes, e + 0x1C),
                    Field20 = ReadInt32BE(bytes, e + 0x20),
                    Field24 = ReadInt32BE(bytes, e + 0x24),
                    Field28 = ReadInt32BE(bytes, e + 0x28),
                    Field2C = ReadInt32BE(bytes, e + 0x2C),
                    Field30 = ReadUInt16BE(bytes, e + 0x30),
                    Field32 = ReadUInt16BE(bytes, e + 0x32),
                    Field34 = ReadInt32BE(bytes, e + 0x34),
                    Field38 = ReadSingleBE(bytes, e + 0x38),
                    Field3C = ReadInt32BE(bytes, e + 0x3C),
                    Field40 = ReadSingleBE(bytes, e + 0x40),
                    Field44 = ReadSingleBE(bytes, e + 0x44),
                    Field48 = ReadHalfBE(bytes, e + 0x48),
                    Field4A = ReadHalfBE(bytes, e + 0x4A),
                    Field4C = ReadHalfBE(bytes, e + 0x4C),
                    Field4E = ReadHalfBE(bytes, e + 0x4E),
                    Field50 = ReadSingleBE(bytes, e + 0x50),
                    ParticleSpeed = ReadSingleBE(bytes, e + 0x54),
                    Field58 = ReadSingleBE(bytes, e + 0x58),
                    Field5C = ReadSingleBE(bytes, e + 0x5C)
                };
                entry.HasVersion78Fields = hasVersion78Fields;
                if (hasVersion78Fields)
                {
                    entry.ClumpChunkIndex = ReadInt32BE(bytes, e + 0x60);
                    entry.Field64 = ReadInt32BE(bytes, e + 0x64);
                    entry.Field68 = ReadInt32BE(bytes, e + 0x68);
                    entry.Field6C = ReadInt32BE(bytes, e + 0x6C);
                }
                output.Add(entry);
            }

            return true;
        }

        private static bool TryParseNodes(byte[] bytes, int offset, ParticleSectionHeaderState header, List<ParticleNodeEntry> output)
        {
            output.Clear();
            int end = offset + header.Size;
            if (end > bytes.Length)
                return false;

            int current = offset;
            for (int i = 0; i < header.Count; i++)
            {
                if (current + 4 > end)
                    return false;

                uint frameCount = ReadUInt32BE(bytes, current);
                current += 4;
                if (current + (frameCount * 4) > end)
                    return false;

                ParticleNodeEntry entry = new ParticleNodeEntry();
                for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                {
                    entry.Frames.Add(new ParticleFrameEntry
                    {
                        RawValue = ReadUInt32BE(bytes, current)
                    });
                    current += 4;
                }

                if (((frameCount * 4) + 4) % 8 != 0)
                {
                    if (current + 4 > end)
                        return false;
                    entry.Padding = new byte[4];
                    Buffer.BlockCopy(bytes, current, entry.Padding, 0, 4);
                    current += 4;
                }

                output.Add(entry);
            }

            return current == end;
        }

        private static byte[] BuildManagers(List<ParticleManagerEntry> entries)
        {
            byte[] output = new byte[entries.Count * ParticleManagerSize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticleManagerEntry entry = entries[i];
                int e = i * ParticleManagerSize;
                WriteUInt32BE(output, e + 0x00, entry.AnimationChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.EntryIndex);
                WriteUInt32BE(output, e + 0x08, entry.Field08);
                WriteUInt32BE(output, e + 0x0C, entry.Field0C);
                output[e + 0x10] = entry.AllocationMode;
                output[e + 0x11] = (byte)entry.SpawnType;
                output[e + 0x12] = (byte)entry.DirectionType;
                output[e + 0x13] = entry.ParticleInstanceMode;
                output[e + 0x14] = (byte)entry.RotationType;
                output[e + 0x15] = entry.ControlRate;
                output[e + 0x16] = entry.GeneratorFlags;
                output[e + 0x17] = entry.ParticleBehaviorType;
                output[e + 0x18] = entry.ParticleInstanceFlag0;
                output[e + 0x19] = entry.ParticleInstanceFlag1;
                output[e + 0x1A] = entry.ParticleInstanceFlag2;
                output[e + 0x1B] = entry.ParticleInstanceFlag3;
                WriteInt16BE(output, e + 0x1C, entry.GeneratorEndTime);
                WriteUInt16BE(output, e + 0x1E, entry.ExtendedDataFlags);
                WriteSingleBE(output, e + 0x20, entry.ParticleCountOrRate);
                WriteSingleBE(output, e + 0x24, entry.SpawnRadius);
                WriteSingleBE(output, e + 0x28, entry.SpawnRadiusRandomness);
                WriteUInt16BE(output, e + 0x2C, entry.Lifetime);
                WriteUInt16BE(output, e + 0x2E, entry.Reserved2E);
                WriteSingleBE(output, e + 0x30, entry.LifetimeRandomness);
                WriteSingleBE(output, e + 0x34, entry.InitialSpeed);
                WriteSingleBE(output, e + 0x38, entry.InitialSpeedRandomness);
                WriteSingleBE(output, e + 0x3C, entry.EmissionAngle1);
                WriteSingleBE(output, e + 0x40, entry.EmissionAngle2);
                WriteSingleBE(output, e + 0x44, entry.EmissionAngle1Randomness);
                WriteSingleBE(output, e + 0x48, entry.EmissionAngle2Randomness);
                WriteSingleBE(output, e + 0x4C, entry.FadeParameter1);
                WriteSingleBE(output, e + 0x50, entry.FadeParameter2);
                WriteSingleBE(output, e + 0x54, entry.InitialRotation);
                WriteSingleBE(output, e + 0x58, entry.InitialRotationRandomness);
                WriteSingleBE(output, e + 0x5C, entry.ScaleStartX);
                WriteSingleBE(output, e + 0x60, entry.ScaleStartY);
                WriteSingleBE(output, e + 0x64, entry.ScaleStartZ);
                WriteSingleBE(output, e + 0x68, entry.AddRandomScaleStartX);
                WriteSingleBE(output, e + 0x6C, entry.AddRandomScaleStartY);
                WriteSingleBE(output, e + 0x70, entry.AddRandomScaleStartZ);
                WriteSingleBE(output, e + 0x74, entry.ScaleMiddleX);
                WriteSingleBE(output, e + 0x78, entry.ScaleMiddleY);
                WriteSingleBE(output, e + 0x7C, entry.ScaleMiddleZ);
                WriteSingleBE(output, e + 0x80, entry.ScaleEndX);
                WriteSingleBE(output, e + 0x84, entry.ScaleEndY);
                WriteSingleBE(output, e + 0x88, entry.ScaleEndZ);
                WriteSingleBE(output, e + 0x8C, entry.ScaleInterpolationPoint);
                WriteSingleBE(output, e + 0x90, entry.ColorStartR);
                WriteSingleBE(output, e + 0x94, entry.ColorStartG);
                WriteSingleBE(output, e + 0x98, entry.ColorStartB);
                WriteSingleBE(output, e + 0x9C, entry.ColorStartA);
                WriteSingleBE(output, e + 0xA0, entry.ColorMiddleR);
                WriteSingleBE(output, e + 0xA4, entry.ColorMiddleG);
                WriteSingleBE(output, e + 0xA8, entry.ColorMiddleB);
                WriteSingleBE(output, e + 0xAC, entry.ColorMiddleA);
                WriteSingleBE(output, e + 0xB0, entry.ColorEndR);
                WriteSingleBE(output, e + 0xB4, entry.ColorEndG);
                WriteSingleBE(output, e + 0xB8, entry.ColorEndB);
                WriteSingleBE(output, e + 0xBC, entry.ColorEndA);
                WriteSingleBE(output, e + 0xC0, entry.ColorInterpolationPoint);
                WriteUInt32BE(output, e + 0xC4, entry.UnknownC4);
                WriteUInt32BE(output, e + 0xC8, entry.UnknownC8);
                WriteUInt32BE(output, e + 0xCC, entry.UnknownCC);
            }

            return output;
        }

        private static byte[] BuildResources(List<ParticleResourceEntry> entries)
        {
            byte[] output = new byte[entries.Count * ParticleResourceSize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticleResourceEntry entry = entries[i];
                int e = i * ParticleResourceSize;
                WriteUInt32BE(output, e + 0x00, entry.EffectChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteInt32BE(output, e + 0x08, entry.Field08);
                WriteInt32BE(output, e + 0x0C, entry.Field0C);
                WriteUInt16BE(output, e + 0x10, ToNormalizedUInt16(entry.Parameter10));
                WriteUInt16BE(output, e + 0x12, ToNormalizedUInt16(entry.Parameter12));
                WriteUInt16BE(output, e + 0x14, ToNormalizedUInt16(entry.Parameter14));
                WriteUInt16BE(output, e + 0x16, ToNormalizedUInt16(entry.Parameter16));
                WriteUInt16BE(output, e + 0x18, ToNormalizedUInt16(entry.Parameter18));
                WriteUInt16BE(output, e + 0x1A, ToNormalizedUInt16(entry.Parameter1A));
                WriteUInt32BE(output, e + 0x1C, (uint)entry.EffectChunkType);
            }

            return output;
        }

        private static byte[] BuildPositions(List<ParticlePositionEntry> entries, bool usesVersion78Layout)
        {
            bool hasVersion78Fields = usesVersion78Layout || entries.Any(x => x.HasVersion78Fields);
            int entrySize = hasVersion78Fields ? ParticlePositionSize : ParticlePositionLegacySize;
            byte[] output = new byte[entries.Count * entrySize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticlePositionEntry entry = entries[i];
                int e = i * entrySize;
                WriteInt32BE(output, e + 0x00, entry.CoordChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteInt32BE(output, e + 0x08, entry.Field08);
                WriteInt32BE(output, e + 0x0C, entry.Field0C);
                WriteInt32BE(output, e + 0x10, entry.Field10);
                WriteSingleBE(output, e + 0x14, entry.Field14);
                WriteSingleBE(output, e + 0x18, entry.Field18);
                WriteInt32BE(output, e + 0x1C, entry.Field1C);
                WriteInt32BE(output, e + 0x20, entry.Field20);
                WriteInt32BE(output, e + 0x24, entry.Field24);
                WriteInt32BE(output, e + 0x28, entry.Field28);
                WriteInt32BE(output, e + 0x2C, entry.Field2C);
                if (hasVersion78Fields)
                {
                    WriteInt32BE(output, e + 0x30, entry.ClumpChunkIndex);
                    WriteUInt32BE(output, e + 0x34, entry.Field34);
                }
            }

            return output;
        }

        private static byte[] BuildForceFields(List<ParticleForceFieldEntry> entries, bool usesVersion78Layout)
        {
            bool hasVersion78Fields = usesVersion78Layout || entries.Any(x => x.HasVersion78Fields);
            int entrySize = hasVersion78Fields ? ParticleForceFieldSize : ParticleForceFieldLegacySize;
            byte[] output = new byte[entries.Count * entrySize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticleForceFieldEntry entry = entries[i];
                int e = i * entrySize;
                WriteInt32BE(output, e + 0x00, entry.CoordChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteInt32BE(output, e + 0x08, entry.Field08);
                WriteInt32BE(output, e + 0x0C, entry.Field0C);
                WriteInt32BE(output, e + 0x10, entry.Field10);
                WriteSingleBE(output, e + 0x14, entry.Field14);
                WriteSingleBE(output, e + 0x18, entry.Field18);
                WriteInt32BE(output, e + 0x1C, entry.Field1C);
                WriteInt32BE(output, e + 0x20, entry.Field20);
                WriteInt32BE(output, e + 0x24, entry.Field24);
                WriteInt32BE(output, e + 0x28, entry.Field28);
                WriteInt32BE(output, e + 0x2C, entry.Field2C);
                WriteUInt16BE(output, e + 0x30, entry.Field30);
                WriteUInt16BE(output, e + 0x32, entry.Field32);
                WriteInt32BE(output, e + 0x34, entry.Field34);
                WriteSingleBE(output, e + 0x38, entry.Field38);
                WriteInt32BE(output, e + 0x3C, entry.Field3C);
                WriteSingleBE(output, e + 0x40, entry.Field40);
                WriteSingleBE(output, e + 0x44, entry.Field44);
                WriteHalfBE(output, e + 0x48, entry.Field48);
                WriteHalfBE(output, e + 0x4A, entry.Field4A);
                WriteHalfBE(output, e + 0x4C, entry.Field4C);
                WriteHalfBE(output, e + 0x4E, entry.Field4E);
                WriteSingleBE(output, e + 0x50, entry.Field50);
                WriteSingleBE(output, e + 0x54, entry.ParticleSpeed);
                WriteSingleBE(output, e + 0x58, entry.Field58);
                WriteSingleBE(output, e + 0x5C, entry.Field5C);
                if (hasVersion78Fields)
                {
                    WriteInt32BE(output, e + 0x60, entry.ClumpChunkIndex);
                    WriteInt32BE(output, e + 0x64, entry.Field64);
                    WriteInt32BE(output, e + 0x68, entry.Field68);
                    WriteInt32BE(output, e + 0x6C, entry.Field6C);
                }
            }

            return output;
        }

        private static byte[] BuildNodes(List<ParticleNodeEntry> entries)
        {
            List<byte> output = new List<byte>();
            foreach (ParticleNodeEntry entry in entries)
            {
                byte[] countBytes = new byte[4];
                WriteUInt32BE(countBytes, 0, (uint)entry.Frames.Count);
                output.AddRange(countBytes);

                foreach (ParticleFrameEntry frame in entry.Frames)
                {
                    byte[] frameBytes = new byte[4];
                    WriteUInt32BE(frameBytes, 0, frame.RawValue);
                    output.AddRange(frameBytes);
                }

                if (((entry.Frames.Count * 4) + 4) % 8 != 0)
                    output.AddRange(entry.Padding != null && entry.Padding.Length == 4 ? entry.Padding : new byte[4]);
            }

            return output.ToArray();
        }

        public static uint ReadUInt32BE(byte[] bytes, int offset) => (uint)((bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3]);
        public static int ReadInt32BE(byte[] bytes, int offset) => unchecked((int)ReadUInt32BE(bytes, offset));
        public static ushort ReadUInt16BE(byte[] bytes, int offset) => (ushort)((bytes[offset] << 8) | bytes[offset + 1]);
        public static short ReadInt16BE(byte[] bytes, int offset) => (short)ReadUInt16BE(bytes, offset);
        public static float ReadSingleBE(byte[] bytes, int offset)
        {
            byte[] buffer = { bytes[offset + 3], bytes[offset + 2], bytes[offset + 1], bytes[offset] };
            return BitConverter.ToSingle(buffer, 0);
        }

        public static float ReadHalfBE(byte[] bytes, int offset)
        {
            ushort value = ReadUInt16BE(bytes, offset);
            int sign = (value >> 15) & 1;
            int exponent = (value >> 10) & 0x1F;
            int fraction = value & 0x3FF;
            double result;

            if (exponent == 0)
                result = fraction == 0 ? 0.0 : Math.Pow(2, -14) * (fraction / 1024.0);
            else if (exponent == 31)
                result = fraction == 0 ? double.PositiveInfinity : double.NaN;
            else
                result = Math.Pow(2, exponent - 15) * (1.0 + (fraction / 1024.0));

            return (float)(sign == 0 ? result : -result);
        }

        public static void WriteUInt32BE(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)value;
        }

        public static void WriteInt32BE(byte[] bytes, int offset, int value) => WriteUInt32BE(bytes, offset, unchecked((uint)value));

        private static ushort ToNormalizedUInt16(float value)
        {
            float clamped = Math.Max(0f, Math.Min(1f, value));
            return (ushort)Math.Round(clamped * ushort.MaxValue, MidpointRounding.AwayFromZero);
        }

        public static void WriteUInt16BE(byte[] bytes, int offset, ushort value)
        {
            bytes[offset] = (byte)(value >> 8);
            bytes[offset + 1] = (byte)value;
        }

        public static void WriteInt16BE(byte[] bytes, int offset, short value) => WriteUInt16BE(bytes, offset, unchecked((ushort)value));
        public static void WriteSingleBE(byte[] bytes, int offset, float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            bytes[offset] = buffer[3];
            bytes[offset + 1] = buffer[2];
            bytes[offset + 2] = buffer[1];
            bytes[offset + 3] = buffer[0];
        }

        public static void WriteHalfBE(byte[] bytes, int offset, float value)
        {
            WriteUInt16BE(bytes, offset, SingleToHalf(value));
        }

        private static ushort SingleToHalf(float value)
        {
            uint bits = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            uint sign = (bits >> 16) & 0x8000u;
            uint exponent = (bits >> 23) & 0xFFu;
            uint mantissa = bits & 0x7FFFFFu;

            if (exponent == 255)
                return (ushort)(sign | 0x7C00u | (mantissa == 0 ? 0u : 1u));

            int halfExponent = (int)exponent - 127 + 15;
            if (halfExponent >= 31)
                return (ushort)(sign | 0x7C00u);
            if (halfExponent <= 0)
            {
                if (halfExponent < -10)
                    return (ushort)sign;
                mantissa |= 0x800000u;
                return (ushort)(sign | (mantissa >> (1 - halfExponent + 13)));
            }

            return (ushort)(sign | ((uint)halfExponent << 10) | (mantissa >> 13));
        }
    }

    internal sealed class ParticleSectionHeaderState
    {
        public uint Value { get; set; }
        public ushort Count { get; set; }
        public ushort Size { get; set; }
    }

    internal sealed class ParticleChunkState
    {
        public string OriginalChunkName { get; set; } = "";
        public string ChunkName { get; set; } = "";
        public string ChunkPath { get; set; } = "";
        public int Version { get; set; } = 121;
        public int VersionAttribute { get; set; } = 2024;
        public bool UsesVersion78Layout { get; set; } = true;
        public bool DeletePending { get; set; }
        public readonly ParticleSectionHeaderState[] Headers =
        {
            new ParticleSectionHeaderState(),
            new ParticleSectionHeaderState(),
            new ParticleSectionHeaderState(),
            new ParticleSectionHeaderState(),
            new ParticleSectionHeaderState()
        };
        public readonly List<ParticleManagerEntry> Managers = new List<ParticleManagerEntry>();
        public readonly List<ParticleResourceEntry> Resources = new List<ParticleResourceEntry>();
        public readonly List<ParticlePositionEntry> Positions = new List<ParticlePositionEntry>();
        public readonly List<ParticleForceFieldEntry> ForceFields = new List<ParticleForceFieldEntry>();
        public readonly List<ParticleNodeEntry> Nodes = new List<ParticleNodeEntry>();
        public readonly List<ParticleChunkReferenceEntry> References = new List<ParticleChunkReferenceEntry>();
        public byte[] ExtendedData { get; set; } = new byte[0];
    }

    internal sealed class ParticleChunkReferenceEntry
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Path { get; set; } = "";

        public override string ToString()
        {
            string name = string.IsNullOrWhiteSpace(Name) ? "(unnamed)" : Name;
            return name + " [" + Type + "]";
        }
    }

    internal enum ParticleNodeAction
    {
        Off = 0,
        On = 1
    }

    internal sealed class ParticleNodeEvent
    {
        public ParticleNodeAction Action { get; set; }
        public uint TimeMilliseconds { get; set; }
        public float Frame => TimeMilliseconds / 33f;
    }

    internal sealed class ParticleManagerEntry
    {
        [Category("Links")]
        [DisplayName("Animation Chunk Map")]
        [Description("Offset 0x00 (u32). Index into this particle page's linked chunk map; normally a nuccChunkAnm.")]
        public uint AnimationChunkIndex { get; set; }
        [Category("Links")]
        [DisplayName("Entry ID")]
        [Description("Offset 0x04 (u32). Stable particle-setting identifier used by resource, position, force-field, and timeline entries.")]
        public uint EntryIndex { get; set; }
        [Category("Reserved")]
        [DisplayName("Reserved 0x08-0x0B")]
        [Description("Offsets 0x08-0x0B. Four reserved bytes preserved as a raw u32 value.")]
        public uint Field08 { get; set; }
        [Category("Reserved")]
        [DisplayName("Reserved 0x0C-0x0F")]
        [Description("Offsets 0x0C-0x0F. Four reserved bytes preserved as a raw u32 value.")]
        public uint Field0C { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x10 (u8). Particle allocation mode recovered from nuccParticleGenParam.")]
        public byte AllocationMode { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x11 (u8). Generator shape: point, planar circle, sphere, or one of three tube modes.")]
        public ParticleSpawnType SpawnType { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x12 (u8). Initial direction: outward, inward, random XYZ, or local cone.")]
        public ParticleDirectionType DirectionType { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x13 (u8). Particle instance mode; values are not yet named by the executable analysis.")]
        public byte ParticleInstanceMode { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x14 (u8). Initial rotation mode.")]
        public ParticleRotationType RotationType { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x15 (u8). Generator control/update rate.")]
        public byte ControlRate { get; set; }
        [Category("Flags")]
        [Description("Offset 0x16 (u8). Generator flags. Bit 0 enables independent random XYZ scale; bit 4 enables position-node path adjustment.")]
        public byte GeneratorFlags { get; set; }
        [Category("Generator Modes")]
        [Description("Offset 0x17 (u8). Particle behavior type. The recovered template leaves its numeric values unnamed.")]
        public byte ParticleBehaviorType { get; set; }
        [Category("Flags")]
        [DisplayName("Instance Flag 0 (0x18)")]
        [Description("Offset 0x18 (u8). First particle-instance flag byte.")]
        public byte ParticleInstanceFlag0 { get; set; }
        [Category("Flags")]
        [DisplayName("Instance Flag 1 (0x19)")]
        [Description("Offset 0x19 (u8). Second particle-instance flag byte.")]
        public byte ParticleInstanceFlag1 { get; set; }
        [Category("Flags")]
        [DisplayName("Instance Flag 2 (0x1A)")]
        [Description("Offset 0x1A (u8). Third particle-instance flag byte.")]
        public byte ParticleInstanceFlag2 { get; set; }
        [Category("Flags")]
        [DisplayName("Instance Flag 3 (0x1B)")]
        [Description("Offset 0x1B (u8). Fourth particle-instance flag byte.")]
        public byte ParticleInstanceFlag3 { get; set; }
        [Category("Timing")]
        [Description("Offset 0x1C (s16). Generator end time; -1 means endless.")]
        public short GeneratorEndTime { get; set; }
        [Category("Flags")]
        [Description("Offset 0x1E (u16). The low byte enables the v0x7B extended particle-data block.")]
        public ushort ExtendedDataFlags { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x20 (f32). Particle count or emission rate, depending on allocation mode.")]
        public float ParticleCountOrRate { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x24 (f32). Base spawn radius.")]
        public float SpawnRadius { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x28 (f32). Random variation applied to spawn radius.")]
        public float SpawnRadiusRandomness { get; set; }
        [Category("Timing")]
        [Description("Offset 0x2C (u16). Base particle lifetime.")]
        public ushort Lifetime { get; set; }
        [Category("Reserved")]
        [DisplayName("Reserved 0x2E")]
        [Description("Offset 0x2E (u16). Reserved value preserved when saving.")]
        public ushort Reserved2E { get; set; }
        [Category("Timing")]
        [Description("Offset 0x30 (f32). Random variation applied to lifetime.")]
        public float LifetimeRandomness { get; set; }
        [Category("Movement")]
        [Description("Offset 0x34 (f32). Initial particle speed.")]
        public float InitialSpeed { get; set; }
        [Category("Movement")]
        [Description("Offset 0x38 (f32). Random variation applied to initial speed.")]
        public float InitialSpeedRandomness { get; set; }
        [Category("Emission")]
        [Description("Offset 0x3C (f32). First emission-angle parameter.")]
        public float EmissionAngle1 { get; set; }
        [Category("Emission")]
        [Description("Offset 0x40 (f32). Second emission-angle parameter.")]
        public float EmissionAngle2 { get; set; }
        [Category("Emission")]
        [Description("Offset 0x44 (f32). Random variation applied to emission angle 1.")]
        public float EmissionAngle1Randomness { get; set; }
        [Category("Emission")]
        [Description("Offset 0x48 (f32). Random variation applied to emission angle 2.")]
        public float EmissionAngle2Randomness { get; set; }
        [Category("Fade")]
        [Description("Offset 0x4C (f32). First fade parameter recovered from the generator block.")]
        public float FadeParameter1 { get; set; }
        [Category("Fade")]
        [Description("Offset 0x50 (f32). Second fade parameter recovered from the generator block.")]
        public float FadeParameter2 { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x54 (f32). Initial particle rotation.")]
        public float InitialRotation { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x58 (f32). Random variation applied to initial rotation.")]
        public float InitialRotationRandomness { get; set; }
        [Category("Scale")]
        [Description("Offset 0x5C. Start scale on X.")]
        public float ScaleStartX { get; set; }
        [Category("Scale")]
        [Description("Offset 0x60. Start scale on Y.")]
        public float ScaleStartY { get; set; }
        [Category("Scale")]
        [Description("Offset 0x64. Start scale on Z.")]
        public float ScaleStartZ { get; set; }
        [Category("Scale")]
        [Description("Offset 0x68. Random amount added to start scale on X.")]
        public float AddRandomScaleStartX { get; set; }
        [Category("Scale")]
        [Description("Offset 0x6C. Random amount added to start scale on Y.")]
        public float AddRandomScaleStartY { get; set; }
        [Category("Scale")]
        [Description("Offset 0x70. Random amount added to start scale on Z.")]
        public float AddRandomScaleStartZ { get; set; }
        [Category("Scale")]
        [Description("Offset 0x74. Middle scale on X.")]
        public float ScaleMiddleX { get; set; }
        [Category("Scale")]
        [Description("Offset 0x78. Middle scale on Y.")]
        public float ScaleMiddleY { get; set; }
        [Category("Scale")]
        [Description("Offset 0x7C. Middle scale on Z.")]
        public float ScaleMiddleZ { get; set; }
        [Category("Scale")]
        [Description("Offset 0x80. End scale on X.")]
        public float ScaleEndX { get; set; }
        [Category("Scale")]
        [Description("Offset 0x84. End scale on Y.")]
        public float ScaleEndY { get; set; }
        [Category("Scale")]
        [Description("Offset 0x88. End scale on Z.")]
        public float ScaleEndZ { get; set; }
        [Category("Scale")]
        [Description("Offset 0x8C (f32). Point used to interpolate the start, middle, and end scales.")]
        public float ScaleInterpolationPoint { get; set; }
        [Category("Color")]
        [Description("Offset 0x90. Start color red channel.")]
        public float ColorStartR { get; set; }
        [Category("Color")]
        [Description("Offset 0x94. Start color green channel.")]
        public float ColorStartG { get; set; }
        [Category("Color")]
        [Description("Offset 0x98. Start color blue channel.")]
        public float ColorStartB { get; set; }
        [Category("Color")]
        [Description("Offset 0x9C. Start color alpha channel.")]
        public float ColorStartA { get; set; }
        [Category("Color")]
        [Description("Offset 0xA0. Middle color red channel.")]
        public float ColorMiddleR { get; set; }
        [Category("Color")]
        [Description("Offset 0xA4. Middle color green channel.")]
        public float ColorMiddleG { get; set; }
        [Category("Color")]
        [Description("Offset 0xA8. Middle color blue channel.")]
        public float ColorMiddleB { get; set; }
        [Category("Color")]
        [Description("Offset 0xAC. Middle color alpha channel.")]
        public float ColorMiddleA { get; set; }
        [Category("Color")]
        [Description("Offset 0xB0. End color red channel.")]
        public float ColorEndR { get; set; }
        [Category("Color")]
        [Description("Offset 0xB4. End color green channel.")]
        public float ColorEndG { get; set; }
        [Category("Color")]
        [Description("Offset 0xB8. End color blue channel.")]
        public float ColorEndB { get; set; }
        [Category("Color")]
        [Description("Offset 0xBC. End color alpha channel.")]
        public float ColorEndA { get; set; }
        [Category("Color")]
        [Description("Offset 0xC0 (f32). Point used to interpolate the start, middle, and end colors.")]
        public float ColorInterpolationPoint { get; set; }
        [Category("Unknown")]
        [DisplayName("Unknown 0xC4")]
        [Description("Offset 0xC4 (u32). Unknown raw value. It is intentionally not interpreted as a float.")]
        public uint UnknownC4 { get; set; }
        [Category("Unknown")]
        [DisplayName("Unknown 0xC8")]
        [Description("Offset 0xC8 (u32). Unknown raw value. It is intentionally not interpreted as a float.")]
        public uint UnknownC8 { get; set; }
        [Category("Unknown")]
        [DisplayName("Unknown 0xCC")]
        [Description("Offset 0xCC (u32). Unknown raw value. It is intentionally not interpreted as a float.")]
        public uint UnknownCC { get; set; }
    }

    internal sealed class ParticleResourceEntry
    {
        [Category("Links")]
        [Description("Offset 0x00. Linked effect chunk index.")]
        public uint EffectChunkIndex { get; set; }
        [Category("Links")]
        [Description("Offset 0x04. EntryIndex of the particle setting that uses this resource.")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x08")]
        [Description("Unknown signed 32-bit value at offset 0x08.")]
        public int Field08 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x0C")]
        [Description("Unknown signed 32-bit value at offset 0x0C.")]
        public int Field0C { get; set; }
        [Category("Normalized Parameters")]
        [DisplayName("Parameter 0x10")]
        [Description("Unsigned 16-bit normalized value at offset 0x10. Edit as a value from 0.0 to 1.0.")]
        public float Parameter10 { get; set; }
        [Category("Normalized Parameters")]
        [DisplayName("Parameter 0x12")]
        [Description("Unsigned 16-bit normalized value at offset 0x12. Edit as a value from 0.0 to 1.0.")]
        public float Parameter12 { get; set; }
        [Category("Normalized Parameters")]
        [DisplayName("Parameter 0x14")]
        [Description("Unsigned 16-bit normalized value at offset 0x14. Edit as a value from 0.0 to 1.0.")]
        public float Parameter14 { get; set; }
        [Category("Normalized Parameters")]
        [DisplayName("Parameter 0x16")]
        [Description("Unsigned 16-bit normalized value at offset 0x16. Edit as a value from 0.0 to 1.0.")]
        public float Parameter16 { get; set; }
        [Category("Normalized Parameters")]
        [DisplayName("Parameter 0x18")]
        [Description("Unsigned 16-bit normalized value at offset 0x18. Edit as a value from 0.0 to 1.0.")]
        public float Parameter18 { get; set; }
        [Category("Normalized Parameters")]
        [DisplayName("Parameter 0x1A")]
        [Description("Unsigned 16-bit normalized value at offset 0x1A. Edit as a value from 0.0 to 1.0.")]
        public float Parameter1A { get; set; }
        [Category("Resource")]
        [Description("Offset 0x1C (u32). Resource type: clump=1, animation=2, sprite=3, sprite2=4, billboard=5.")]
        public ParticleEffectChunkType EffectChunkType { get; set; }
    }

    internal sealed class ParticlePositionEntry
    {
        [Browsable(false)]
        public bool HasVersion78Fields { get; set; }

        [Category("Links")]
        [Description("Offset 0x00. Signed coord chunk index; negative values represent no link.")]
        public int CoordChunkIndex { get; set; }
        [Category("Links")]
        [Description("Offset 0x04. EntryIndex of the linked particle setting.")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x08")]
        [Description("Unknown signed 32-bit value at offset 0x08.")]
        public int Field08 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x0C")]
        [Description("Unknown signed 32-bit value at offset 0x0C.")]
        public int Field0C { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x10")]
        [Description("Unknown signed 32-bit value at offset 0x10.")]
        public int Field10 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x14")]
        [Description("Unknown 32-bit float at offset 0x14; often -1.0 in observed files.")]
        public float Field14 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x18")]
        [Description("Unknown 32-bit float at offset 0x18.")]
        public float Field18 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x1C")]
        [Description("Unknown signed 32-bit value at offset 0x1C.")]
        public int Field1C { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x20")]
        [Description("Unknown signed 32-bit value at offset 0x20.")]
        public int Field20 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x24")]
        [Description("Unknown signed 32-bit value at offset 0x24.")]
        public int Field24 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x28")]
        [Description("Unknown signed 32-bit value at offset 0x28.")]
        public int Field28 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x2C")]
        [Description("Unknown signed 32-bit value at offset 0x2C.")]
        public int Field2C { get; set; }
        [Category("Links")]
        [Description("Offset 0x30 (v0x78+ only). Clump chunk-map index. Negative values are shown as no link for compatibility with observed files.")]
        public int ClumpChunkIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x34")]
        [Description("Offset 0x34 (u32, v0x78+ only). Unknown value.")]
        public uint Field34 { get; set; }
    }

    internal sealed class ParticleForceFieldEntry
    {
        [Browsable(false)]
        public bool HasVersion78Fields { get; set; }

        [Category("Links")]
        [Description("Offset 0x00. Signed coord chunk index; negative values represent no link.")]
        public int CoordChunkIndex { get; set; }
        [Category("Links")]
        [Description("Offset 0x04. EntryIndex of the linked particle setting.")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x08")]
        [Description("Unknown signed 32-bit value at offset 0x08.")]
        public int Field08 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x0C")]
        [Description("Unknown signed 32-bit value at offset 0x0C.")]
        public int Field0C { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x10")]
        [Description("Unknown signed 32-bit value at offset 0x10.")]
        public int Field10 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x14")]
        [Description("Unknown 32-bit float at offset 0x14.")]
        public float Field14 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x18")]
        [Description("Unknown 32-bit float at offset 0x18.")]
        public float Field18 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x1C")]
        [Description("Unknown signed 32-bit value at offset 0x1C.")]
        public int Field1C { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x20")]
        [Description("Unknown signed 32-bit value at offset 0x20.")]
        public int Field20 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x24")]
        [Description("Unknown signed 32-bit value at offset 0x24.")]
        public int Field24 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x28")]
        [Description("Unknown signed 32-bit value at offset 0x28.")]
        public int Field28 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x2C")]
        [Description("Unknown signed 32-bit value at offset 0x2C.")]
        public int Field2C { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x30")]
        [Description("Unknown unsigned 16-bit value at offset 0x30.")]
        public ushort Field30 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x32")]
        [Description("Unknown unsigned 16-bit value at offset 0x32.")]
        public ushort Field32 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x34")]
        [Description("Unknown signed 32-bit value at offset 0x34.")]
        public int Field34 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x38")]
        [Description("Unknown 32-bit float at offset 0x38.")]
        public float Field38 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x3C")]
        [Description("Unknown signed 32-bit value at offset 0x3C.")]
        public int Field3C { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x40")]
        [Description("Unknown 32-bit float at offset 0x40.")]
        public float Field40 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x44")]
        [Description("Unknown 32-bit float at offset 0x44.")]
        public float Field44 { get; set; }
        [Category("Half-Float Values")]
        [DisplayName("Field 0x48")]
        [Description("Unknown IEEE 754 half-float at offset 0x48.")]
        public float Field48 { get; set; }
        [Category("Half-Float Values")]
        [DisplayName("Field 0x4A")]
        [Description("Unknown IEEE 754 half-float at offset 0x4A.")]
        public float Field4A { get; set; }
        [Category("Half-Float Values")]
        [DisplayName("Field 0x4C")]
        [Description("Unknown IEEE 754 half-float at offset 0x4C.")]
        public float Field4C { get; set; }
        [Category("Half-Float Values")]
        [DisplayName("Field 0x4E")]
        [Description("Unknown IEEE 754 half-float at offset 0x4E.")]
        public float Field4E { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x50")]
        [Description("Unknown 32-bit float at offset 0x50.")]
        public float Field50 { get; set; }
        [Category("Movement")]
        [Description("Offset 0x54. Particle speed used by this force-field entry.")]
        public float ParticleSpeed { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x58")]
        [Description("Unknown 32-bit float at offset 0x58.")]
        public float Field58 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x5C")]
        [Description("Unknown 32-bit float at offset 0x5C.")]
        public float Field5C { get; set; }
        [Category("Links")]
        [Description("Offset 0x60 (v0x78+ only). Clump chunk-map index. Negative values are shown as no link for compatibility with observed files.")]
        public int ClumpChunkIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x64")]
        [Description("Offset 0x64 (v0x78+ only). First unknown 32-bit value in the 12-byte extension.")]
        public int Field64 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x68")]
        [Description("Offset 0x68 (v0x78+ only). Second unknown 32-bit value in the 12-byte extension.")]
        public int Field68 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x6C")]
        [Description("Offset 0x6C (v0x78+ only). Third unknown 32-bit value in the 12-byte extension.")]
        public int Field6C { get; set; }
    }

    internal sealed class ParticleFrameEntry
    {
        public uint RawValue { get; set; }
    }

    internal sealed class ParticleNodeEntry
    {
        public readonly List<ParticleFrameEntry> Frames = new List<ParticleFrameEntry>();
        public byte[] Padding { get; set; } = new byte[0];
    }
}
