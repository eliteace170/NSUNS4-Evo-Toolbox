using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NSUNS4_Character_Manager
{
    internal enum ParticleSpawnType
    {
        SpawnAtBone = 0,
        RandomSpawnXZ = 1,
        RandomSpawnXYZ = 2
    }

    internal enum ParticleRandomRollType
    {
        DisabledRandomRotation = 0,
        RandomTrackballRotation = 1,
        Unknown2 = 2,
        RandomRotation = 3
    }

    internal enum ParticleType : byte
    {
        Default = 0,
        InvertAndAddRandomScale = 1,
        Unknown2 = 2,
        Unknown3 = 3,
        KillAtEnd = 4,
        Unknown5 = 5,
        Unknown6 = 6,
        Unknown7 = 7,
        Unknown8 = 8,
        Unknown9 = 9,
        Unknown10 = 10,
        Unknown11 = 11,
        Unknown12 = 12,
        Unknown13 = 13,
        Unknown14 = 14,
        Unknown15 = 15,
        Unknown16 = 16,
        Unknown17 = 17,
        Unknown18 = 18,
        Unknown19 = 19,
        Unknown20 = 20,
        Unknown21 = 21,
        Unknown22 = 22,
        Unknown23 = 23
    }

    internal enum ParticleEffectChunkType : uint
    {
        nuccChunkClump = 1,
        nuccChunkAnm = 2,
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
        public const int ParticlePositionSize = 0x38;
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
                StretchParticles = entry.StretchParticles,
                AttachmentType = entry.AttachmentType,
                VelocityType = entry.VelocityType,
                AttachToBone = entry.AttachToBone,
                RandomRollType = entry.RandomRollType,
                FrameRate = entry.FrameRate,
                Field16 = entry.Field16,
                ParticleType = entry.ParticleType,
                Field18 = entry.Field18,
                Field19 = entry.Field19,
                Field1A = entry.Field1A,
                Gravity = entry.Gravity,
                FrameEnd = entry.FrameEnd,
                Field1E = entry.Field1E,
                ParticleCount = entry.ParticleCount,
                SpawnRadiusStart = entry.SpawnRadiusStart,
                SpawnRadiusEnd = entry.SpawnRadiusEnd,
                Lifetime = entry.Lifetime,
                Field2E = entry.Field2E,
                LifetimeRandomness = entry.LifetimeRandomness,
                Velocity = entry.Velocity,
                ObjectAlignedVelocity = entry.ObjectAlignedVelocity,
                RandomRotationStart = entry.RandomRotationStart,
                RandomRotationMiddle = entry.RandomRotationMiddle,
                RandomRotationEnd = entry.RandomRotationEnd,
                RandomRotationFactor = entry.RandomRotationFactor,
                AlphaStart = entry.AlphaStart,
                AlphaMiddle = entry.AlphaMiddle,
                AlphaEnd = entry.AlphaEnd,
                AlphaFactor = entry.AlphaFactor,
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
                ScaleFactor = entry.ScaleFactor,
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
                ColorFactor = entry.ColorFactor,
                FieldC4 = entry.FieldC4,
                FieldC8 = entry.FieldC8,
                FieldCC = entry.FieldCC
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
                Field34 = entry.Field34
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
                Field6C = entry.Field6C
            };
        }

        public static ParticleNodeEntry CloneNode(ParticleNodeEntry entry)
        {
            ParticleNodeEntry clone = new ParticleNodeEntry();
            clone.Frames.AddRange(entry.Frames.Select(x => new ParticleFrameEntry { Flag = x.Flag, FrameRaw = x.FrameRaw }));
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
                "{0} | v0x{1:X2} | {2} | Links:{3} M:{4} R:{5} P:{6} F:{7} N:{8}",
                string.IsNullOrWhiteSpace(chunk.ChunkName) ? "(unnamed chunk)" : chunk.ChunkName,
                chunk.Version,
                path,
                chunk.References.Count,
                chunk.Managers.Count,
                chunk.Resources.Count,
                chunk.Positions.Count,
                chunk.ForceFields.Count,
                chunk.Nodes.Count);
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
                    Action = frame.Flag == 0 ? ParticleNodeAction.Off : ParticleNodeAction.On,
                    Frame = frame.FrameRaw / 33f
                });
            }

            return events;
        }

        public static ParticleNodeEntry EncodeNodeEvents(IEnumerable<ParticleNodeEvent> events)
        {
            ParticleNodeEntry node = new ParticleNodeEntry();
            foreach (ParticleNodeEvent particleEvent in events)
            {
                int frameRaw = (int)Math.Round(Math.Max(0f, particleEvent.Frame) * 33f, MidpointRounding.AwayFromZero);
                node.Frames.Add(new ParticleFrameEntry
                {
                    Flag = particleEvent.Action == ParticleNodeAction.On ? (ushort)1 : (ushort)0,
                    FrameRaw = checked((ushort)Math.Min(frameRaw, ushort.MaxValue))
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

            chunk = state;
            return true;
        }

        public static byte[] BuildChunkData(ParticleChunkState chunk)
        {
            byte[] managerBytes = BuildManagers(chunk.Managers);
            byte[] resourceBytes = BuildResources(chunk.Resources);
            byte[] positionBytes = BuildPositions(chunk.Positions);
            byte[] forceFieldBytes = BuildForceFields(chunk.ForceFields);
            byte[] nodeBytes = BuildNodes(chunk.Nodes);

            byte[] output = new byte[HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length + nodeBytes.Length];
            Buffer.BlockCopy(managerBytes, 0, output, HeaderTableSize, managerBytes.Length);
            Buffer.BlockCopy(resourceBytes, 0, output, HeaderTableSize + managerBytes.Length, resourceBytes.Length);
            Buffer.BlockCopy(positionBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length, positionBytes.Length);
            Buffer.BlockCopy(forceFieldBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length, forceFieldBytes.Length);
            Buffer.BlockCopy(nodeBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length, nodeBytes.Length);

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
                    StretchParticles = bytes[e + 0x10] != 0,
                    AttachmentType = (ParticleSpawnType)bytes[e + 0x11],
                    VelocityType = bytes[e + 0x12],
                    AttachToBone = bytes[e + 0x13] != 0,
                    RandomRollType = (ParticleRandomRollType)bytes[e + 0x14],
                    FrameRate = bytes[e + 0x15],
                    Field16 = bytes[e + 0x16],
                    ParticleType = (ParticleType)bytes[e + 0x17],
                    Field18 = bytes[e + 0x18],
                    Field19 = bytes[e + 0x19],
                    Field1A = bytes[e + 0x1A],
                    Gravity = bytes[e + 0x1B],
                    FrameEnd = ReadInt16BE(bytes, e + 0x1C),
                    Field1E = ReadUInt16BE(bytes, e + 0x1E),
                    ParticleCount = ReadSingleBE(bytes, e + 0x20),
                    SpawnRadiusStart = ReadSingleBE(bytes, e + 0x24),
                    SpawnRadiusEnd = ReadSingleBE(bytes, e + 0x28),
                    Lifetime = ReadUInt16BE(bytes, e + 0x2C),
                    Field2E = ReadUInt16BE(bytes, e + 0x2E),
                    LifetimeRandomness = ReadSingleBE(bytes, e + 0x30),
                    Velocity = ReadSingleBE(bytes, e + 0x34),
                    ObjectAlignedVelocity = ReadSingleBE(bytes, e + 0x38),
                    RandomRotationStart = ReadSingleBE(bytes, e + 0x3C),
                    RandomRotationMiddle = ReadSingleBE(bytes, e + 0x40),
                    RandomRotationEnd = ReadSingleBE(bytes, e + 0x44),
                    RandomRotationFactor = ReadSingleBE(bytes, e + 0x48),
                    AlphaStart = ReadSingleBE(bytes, e + 0x4C),
                    AlphaMiddle = ReadSingleBE(bytes, e + 0x50),
                    AlphaEnd = ReadSingleBE(bytes, e + 0x54),
                    AlphaFactor = ReadSingleBE(bytes, e + 0x58),
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
                    ScaleFactor = ReadSingleBE(bytes, e + 0x8C),
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
                    ColorFactor = ReadSingleBE(bytes, e + 0xC0),
                    FieldC4 = ReadSingleBE(bytes, e + 0xC4),
                    FieldC8 = ReadSingleBE(bytes, e + 0xC8),
                    FieldCC = ReadSingleBE(bytes, e + 0xCC)
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
            if (header.Size != header.Count * ParticlePositionSize)
                return false;

            for (int i = 0; i < header.Count; i++)
            {
                int e = offset + (i * ParticlePositionSize);
                output.Add(new ParticlePositionEntry
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
                    ClumpChunkIndex = ReadInt32BE(bytes, e + 0x30),
                    Field34 = ReadUInt32BE(bytes, e + 0x34)
                });
            }

            return true;
        }

        private static bool TryParseForceFields(byte[] bytes, int offset, ParticleSectionHeaderState header, List<ParticleForceFieldEntry> output)
        {
            output.Clear();
            if (header.Count == 0)
                return header.Size == 0;
            if (header.Size != header.Count * ParticleForceFieldSize)
                return false;

            for (int i = 0; i < header.Count; i++)
            {
                int e = offset + (i * ParticleForceFieldSize);
                output.Add(new ParticleForceFieldEntry
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
                    Field5C = ReadSingleBE(bytes, e + 0x5C),
                    ClumpChunkIndex = ReadInt32BE(bytes, e + 0x60),
                    Field64 = ReadInt32BE(bytes, e + 0x64),
                    Field68 = ReadInt32BE(bytes, e + 0x68),
                    Field6C = ReadInt32BE(bytes, e + 0x6C)
                });
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
                        Flag = ReadUInt16BE(bytes, current),
                        FrameRaw = ReadUInt16BE(bytes, current + 2)
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
                output[e + 0x10] = entry.StretchParticles ? (byte)1 : (byte)0;
                output[e + 0x11] = (byte)entry.AttachmentType;
                output[e + 0x12] = entry.VelocityType;
                output[e + 0x13] = entry.AttachToBone ? (byte)1 : (byte)0;
                output[e + 0x14] = (byte)entry.RandomRollType;
                output[e + 0x15] = entry.FrameRate;
                output[e + 0x16] = entry.Field16;
                output[e + 0x17] = (byte)entry.ParticleType;
                output[e + 0x18] = entry.Field18;
                output[e + 0x19] = entry.Field19;
                output[e + 0x1A] = entry.Field1A;
                output[e + 0x1B] = entry.Gravity;
                WriteInt16BE(output, e + 0x1C, entry.FrameEnd);
                WriteUInt16BE(output, e + 0x1E, entry.Field1E);
                WriteSingleBE(output, e + 0x20, entry.ParticleCount);
                WriteSingleBE(output, e + 0x24, entry.SpawnRadiusStart);
                WriteSingleBE(output, e + 0x28, entry.SpawnRadiusEnd);
                WriteUInt16BE(output, e + 0x2C, entry.Lifetime);
                WriteUInt16BE(output, e + 0x2E, entry.Field2E);
                WriteSingleBE(output, e + 0x30, entry.LifetimeRandomness);
                WriteSingleBE(output, e + 0x34, entry.Velocity);
                WriteSingleBE(output, e + 0x38, entry.ObjectAlignedVelocity);
                WriteSingleBE(output, e + 0x3C, entry.RandomRotationStart);
                WriteSingleBE(output, e + 0x40, entry.RandomRotationMiddle);
                WriteSingleBE(output, e + 0x44, entry.RandomRotationEnd);
                WriteSingleBE(output, e + 0x48, entry.RandomRotationFactor);
                WriteSingleBE(output, e + 0x4C, entry.AlphaStart);
                WriteSingleBE(output, e + 0x50, entry.AlphaMiddle);
                WriteSingleBE(output, e + 0x54, entry.AlphaEnd);
                WriteSingleBE(output, e + 0x58, entry.AlphaFactor);
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
                WriteSingleBE(output, e + 0x8C, entry.ScaleFactor);
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
                WriteSingleBE(output, e + 0xC0, entry.ColorFactor);
                WriteSingleBE(output, e + 0xC4, entry.FieldC4);
                WriteSingleBE(output, e + 0xC8, entry.FieldC8);
                WriteSingleBE(output, e + 0xCC, entry.FieldCC);
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

        private static byte[] BuildPositions(List<ParticlePositionEntry> entries)
        {
            byte[] output = new byte[entries.Count * ParticlePositionSize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticlePositionEntry entry = entries[i];
                int e = i * ParticlePositionSize;
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
                WriteInt32BE(output, e + 0x30, entry.ClumpChunkIndex);
                WriteUInt32BE(output, e + 0x34, entry.Field34);
            }

            return output;
        }

        private static byte[] BuildForceFields(List<ParticleForceFieldEntry> entries)
        {
            byte[] output = new byte[entries.Count * ParticleForceFieldSize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticleForceFieldEntry entry = entries[i];
                int e = i * ParticleForceFieldSize;
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
                WriteInt32BE(output, e + 0x60, entry.ClumpChunkIndex);
                WriteInt32BE(output, e + 0x64, entry.Field64);
                WriteInt32BE(output, e + 0x68, entry.Field68);
                WriteInt32BE(output, e + 0x6C, entry.Field6C);
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
                    WriteUInt16BE(frameBytes, 0, frame.Flag);
                    WriteUInt16BE(frameBytes, 2, frame.FrameRaw);
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
        public float Frame { get; set; }
    }

    internal sealed class ParticleManagerEntry
    {
        [Category("Links")]
        [Description("Offset 0x00. Index into this particle page's linked chunk map; normally a nuccChunkAnm.")]
        public uint AnimationChunkIndex { get; set; }
        [Category("Links")]
        [Description("Offset 0x04. Stable particle-setting identifier used by resource, position, force-field, and node entries.")]
        public uint EntryIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x08")]
        [Description("Unknown unsigned 32-bit value at offset 0x08.")]
        public uint Field08 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x0C")]
        [Description("Unknown unsigned 32-bit value at offset 0x0C.")]
        public uint Field0C { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x10. Enables particle stretching behavior.")]
        public bool StretchParticles { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x11. Selects bone, random XZ, or random XYZ attachment/spawn behavior.")]
        public ParticleSpawnType AttachmentType { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x12. Velocity behavior selector; values are not fully documented.")]
        public byte VelocityType { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x13. Keeps the particle attached to its source bone.")]
        public bool AttachToBone { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x14. Selects disabled, trackball, unknown type 2, or random roll behavior.")]
        public ParticleRandomRollType RandomRollType { get; set; }
        [Category("Timing")]
        [Description("Offset 0x15. Particle playback frame rate.")]
        public byte FrameRate { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x16")]
        [Description("Unknown byte at offset 0x16.")]
        public byte Field16 { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x17. Particle behavior type; the template currently names values 0 through 23.")]
        public ParticleType ParticleType { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x18")]
        [Description("Unknown byte at offset 0x18.")]
        public byte Field18 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x19")]
        [Description("Unknown byte at offset 0x19.")]
        public byte Field19 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x1A")]
        [Description("Unknown byte at offset 0x1A.")]
        public byte Field1A { get; set; }
        [Category("Gravity")]
        [Description("Offset 0x1B. Gravity control value.")]
        public byte Gravity { get; set; }
        [Category("Timing")]
        [Description("Offset 0x1C. End frame; -1 (0xFFFF) is commonly used for no fixed end.")]
        public short FrameEnd { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x1E")]
        [Description("Unknown unsigned 16-bit value at offset 0x1E.")]
        public ushort Field1E { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x20. Number or density of emitted particles.")]
        public float ParticleCount { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x24. Initial spawn radius.")]
        public float SpawnRadiusStart { get; set; }
        [Category("Spawn")]
        [Description("Offset 0x28. Final spawn radius.")]
        public float SpawnRadiusEnd { get; set; }
        [Category("Timing")]
        [Description("Offset 0x2C. Base particle lifetime.")]
        public ushort Lifetime { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x2E")]
        [Description("Unknown unsigned 16-bit value at offset 0x2E.")]
        public ushort Field2E { get; set; }
        [Category("Timing")]
        [Description("Offset 0x30. Random variation applied to lifetime.")]
        public float LifetimeRandomness { get; set; }
        [Category("Movement")]
        [Description("Offset 0x34. Primary particle velocity.")]
        public float Velocity { get; set; }
        [Category("Movement")]
        [DisplayName("Object-Aligned Velocity")]
        [Description("Offset 0x38. Velocity component aligned to the source object.")]
        public float ObjectAlignedVelocity { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x3C. Random rotation at the start phase.")]
        public float RandomRotationStart { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x40. Random rotation at the middle phase.")]
        public float RandomRotationMiddle { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x44. Random rotation at the end phase.")]
        public float RandomRotationEnd { get; set; }
        [Category("Rotation")]
        [Description("Offset 0x48. Interpolation factor for random rotation phases.")]
        public float RandomRotationFactor { get; set; }
        [Category("Alpha")]
        [Description("Offset 0x4C. Alpha at the start phase.")]
        public float AlphaStart { get; set; }
        [Category("Alpha")]
        [Description("Offset 0x50. Alpha at the middle phase.")]
        public float AlphaMiddle { get; set; }
        [Category("Alpha")]
        [Description("Offset 0x54. Alpha at the end phase.")]
        public float AlphaEnd { get; set; }
        [Category("Alpha")]
        [Description("Offset 0x58. Interpolation factor for alpha phases.")]
        public float AlphaFactor { get; set; }
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
        [Description("Offset 0x8C. Interpolation factor for scale phases.")]
        public float ScaleFactor { get; set; }
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
        [Description("Offset 0xC0. Interpolation factor for color phases.")]
        public float ColorFactor { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0xC4")]
        [Description("Unknown 32-bit floating-point value at offset 0xC4 (unk9 in br_particle.py).")]
        public float FieldC4 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0xC8")]
        [Description("Unknown 32-bit floating-point value at offset 0xC8 (unk10 in br_particle.py).")]
        public float FieldC8 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0xCC")]
        [Description("Unknown 32-bit floating-point value at offset 0xCC (unk11 in br_particle.py).")]
        public float FieldCC { get; set; }
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
        [Description("Offset 0x1C. Expected values: clump=1, animation=2, billboard=5.")]
        public ParticleEffectChunkType EffectChunkType { get; set; }
    }

    internal sealed class ParticlePositionEntry
    {
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
        [Description("Offset 0x30. Signed clump chunk index; negative values represent no link.")]
        public int ClumpChunkIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x34")]
        [Description("Unknown unsigned 32-bit value at offset 0x34.")]
        public uint Field34 { get; set; }
    }

    internal sealed class ParticleForceFieldEntry
    {
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
        [Description("Offset 0x60. Signed clump chunk index; negative values represent no link.")]
        public int ClumpChunkIndex { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x64")]
        [Description("Unknown signed 32-bit value at offset 0x64.")]
        public int Field64 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x68")]
        [Description("Unknown signed 32-bit value at offset 0x68.")]
        public int Field68 { get; set; }
        [Category("Unknown")]
        [DisplayName("Field 0x6C")]
        [Description("Unknown signed 32-bit value at offset 0x6C.")]
        public int Field6C { get; set; }
    }

    internal sealed class ParticleFrameEntry
    {
        public ushort Flag { get; set; }
        public ushort FrameRaw { get; set; }
    }

    internal sealed class ParticleNodeEntry
    {
        public readonly List<ParticleFrameEntry> Frames = new List<ParticleFrameEntry>();
        public byte[] Padding { get; set; } = new byte[0];
    }
}
