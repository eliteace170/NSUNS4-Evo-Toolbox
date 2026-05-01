using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NSUNS4_Character_Manager
{
    internal enum ParticleSpawnType
    {
        Bone = 0,
        RandomXZ = 1,
        RandomXYZ = 2
    }

    internal enum ParticleRandomRollType
    {
        Type0 = 0,
        Type1 = 1,
        Type2 = 2
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
                Reserved0 = entry.Reserved0,
                DisableStretch = entry.DisableStretch,
                SpawnType = entry.SpawnType,
                BlowoutDisabler = entry.BlowoutDisabler,
                AttachToBone = entry.AttachToBone,
                RandomRollType = entry.RandomRollType,
                FrameRate = entry.FrameRate,
                ReservedBehaviorFlag = entry.ReservedBehaviorFlag,
                ParticleType = entry.ParticleType,
                Reserved1 = entry.Reserved1,
                GravityModeFlagCandidate = entry.GravityModeFlagCandidate,
                StartGravity = entry.StartGravity,
                FrameEnd = entry.FrameEnd,
                LifetimeModeFlagCandidate = entry.LifetimeModeFlagCandidate,
                ParticleCount = entry.ParticleCount,
                SpawnRadiusStart = entry.SpawnRadiusStart,
                SpawnRadiusEnd = entry.SpawnRadiusEnd,
                FadeTime = entry.FadeTime,
                Reserved2 = entry.Reserved2,
                FadeTime2 = entry.FadeTime2,
                Blowout = entry.Blowout,
                RotationW = entry.RotationW,
                RotationX = entry.RotationX,
                RotationY = entry.RotationY,
                RotationZ = entry.RotationZ,
                RandomRotationFactorCandidate = entry.RandomRotationFactorCandidate,
                Transparency = entry.Transparency,
                AlphaMiddleCandidate = entry.AlphaMiddleCandidate,
                AlphaEndCandidate = entry.AlphaEndCandidate,
                AlphaBlendFactorCandidate = entry.AlphaBlendFactorCandidate,
                ScaleStartX = entry.ScaleStartX,
                ScaleStartY = entry.ScaleStartY,
                ScaleStartZ = entry.ScaleStartZ,
                ScaleMiddleX = entry.ScaleMiddleX,
                ScaleMiddleY = entry.ScaleMiddleY,
                ScaleMiddleZ = entry.ScaleMiddleZ,
                ScaleEndX = entry.ScaleEndX,
                ScaleEndY = entry.ScaleEndY,
                ScaleEndZ = entry.ScaleEndZ,
                AddRandomScaleStartX = entry.AddRandomScaleStartX,
                AddRandomScaleStartY = entry.AddRandomScaleStartY,
                AddRandomScaleStartZ = entry.AddRandomScaleStartZ,
                MiddleScalePoint = entry.MiddleScalePoint,
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
                Reserved3 = entry.Reserved3,
                Reserved4 = entry.Reserved4
            };
        }

        public static ParticleResourceEntry CloneResource(ParticleResourceEntry entry)
        {
            return new ParticleResourceEntry
            {
                EffectChunkIndex = entry.EffectChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Reserved0 = entry.Reserved0,
                ResourceParam1Candidate = entry.ResourceParam1Candidate,
                ResourceParam2Candidate = entry.ResourceParam2Candidate,
                Reserved1 = entry.Reserved1,
                Reserved2 = entry.Reserved2,
                EffectChunkType = entry.EffectChunkType
            };
        }

        public static ParticlePositionEntry ClonePosition(ParticlePositionEntry entry)
        {
            return new ParticlePositionEntry
            {
                CoordChunkIndex = entry.CoordChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                PositionFlag1Candidate = entry.PositionFlag1Candidate,
                PositionFlag2Candidate = entry.PositionFlag2Candidate,
                PositionFlag3Candidate = entry.PositionFlag3Candidate,
                InheritAxisValueCandidate = entry.InheritAxisValueCandidate,
                PositionValue2Candidate = entry.PositionValue2Candidate,
                PositionFlag4Candidate = entry.PositionFlag4Candidate,
                PositionFlag5Candidate = entry.PositionFlag5Candidate,
                PositionFlag6Candidate = entry.PositionFlag6Candidate,
                PositionFlag7Candidate = entry.PositionFlag7Candidate,
                PositionFlag8Candidate = entry.PositionFlag8Candidate,
                ClumpChunkIndex = entry.ClumpChunkIndex,
                PositionFlag9Candidate = entry.PositionFlag9Candidate
            };
        }

        public static ParticleForceFieldEntry CloneForceField(ParticleForceFieldEntry entry)
        {
            return new ParticleForceFieldEntry
            {
                CoordChunkIndex = entry.CoordChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                ForceFlag1Candidate = entry.ForceFlag1Candidate,
                ForceFlag2Candidate = entry.ForceFlag2Candidate,
                ForceFlag3Candidate = entry.ForceFlag3Candidate,
                InheritAxisValueCandidate = entry.InheritAxisValueCandidate,
                ForceValue1Candidate = entry.ForceValue1Candidate,
                ForceFlag4Candidate = entry.ForceFlag4Candidate,
                ForceFlag5Candidate = entry.ForceFlag5Candidate,
                ForceFlag6Candidate = entry.ForceFlag6Candidate,
                ForceFlag7Candidate = entry.ForceFlag7Candidate,
                ForceFlag8Candidate = entry.ForceFlag8Candidate,
                ForceFlag9aCandidate = entry.ForceFlag9aCandidate,
                ForceFlag9bCandidate = entry.ForceFlag9bCandidate,
                ForceFlag10Candidate = entry.ForceFlag10Candidate,
                ParticleSpeedCandidate = entry.ParticleSpeedCandidate,
                ForceEnableFlagCandidate = entry.ForceEnableFlagCandidate,
                ForceValue2Candidate = entry.ForceValue2Candidate,
                ForceValue3Candidate = entry.ForceValue3Candidate,
                AngleParam1Candidate = entry.AngleParam1Candidate,
                AngleParam2Candidate = entry.AngleParam2Candidate,
                AngleParam3Candidate = entry.AngleParam3Candidate,
                AngleParam4Candidate = entry.AngleParam4Candidate,
                AngleOrFalloff1Candidate = entry.AngleOrFalloff1Candidate,
                AngleOrFalloff2Candidate = entry.AngleOrFalloff2Candidate,
                ForceValue4Candidate = entry.ForceValue4Candidate,
                ForceValue5Candidate = entry.ForceValue5Candidate,
                ClumpChunkIndex = entry.ClumpChunkIndex,
                TrailingReserved1 = entry.TrailingReserved1,
                TrailingReserved2 = entry.TrailingReserved2,
                TrailingReserved3 = entry.TrailingReserved3
            };
        }

        public static ParticleNodeEntry CloneNode(ParticleNodeEntry entry)
        {
            ParticleNodeEntry clone = new ParticleNodeEntry();
            clone.Frames.AddRange(entry.Frames);
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
                "{0} | {1} | M:{2} R:{3} P:{4} F:{5} N:{6}",
                string.IsNullOrWhiteSpace(chunk.ChunkName) ? "(unnamed chunk)" : chunk.ChunkName,
                path,
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

        public static List<ParticleNodeEvent> DecodeNodeEvents(ParticleNodeEntry node)
        {
            List<ParticleNodeEvent> events = new List<ParticleNodeEvent>();
            if (node == null)
                return events;

            foreach (uint raw in node.Frames)
            {
                bool enabled = raw > 0x7FFFFFFF;
                uint value = enabled ? raw - 0x80000000u : raw;
                events.Add(new ParticleNodeEvent
                {
                    Action = enabled ? ParticleNodeAction.On : ParticleNodeAction.Off,
                    Frame = (int)(value / 33u)
                });
            }

            return events;
        }

        public static ParticleNodeEntry EncodeNodeEvents(IEnumerable<ParticleNodeEvent> events)
        {
            ParticleNodeEntry node = new ParticleNodeEntry();
            foreach (ParticleNodeEvent particleEvent in events)
            {
                uint value = (uint)Math.Max(0, particleEvent.Frame) * 33u;
                if (particleEvent.Action == ParticleNodeAction.On)
                    value += 0x80000000u;
                node.Frames.Add(value);
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
                    Reserved0 = ReadUInt64BE(bytes, e + 0x08),
                    DisableStretch = bytes[e + 0x10] != 0,
                    SpawnType = (ParticleSpawnType)bytes[e + 0x11],
                    BlowoutDisabler = bytes[e + 0x12],
                    AttachToBone = bytes[e + 0x13] != 0,
                    RandomRollType = (ParticleRandomRollType)bytes[e + 0x14],
                    FrameRate = bytes[e + 0x15],
                    ReservedBehaviorFlag = bytes[e + 0x16],
                    ParticleType = bytes[e + 0x17],
                    Reserved1 = ReadUInt16BE(bytes, e + 0x18),
                    GravityModeFlagCandidate = bytes[e + 0x1A],
                    StartGravity = bytes[e + 0x1B],
                    FrameEnd = ReadInt16BE(bytes, e + 0x1C),
                    LifetimeModeFlagCandidate = ReadUInt16BE(bytes, e + 0x1E),
                    ParticleCount = ReadSingleBE(bytes, e + 0x20),
                    SpawnRadiusStart = ReadSingleBE(bytes, e + 0x24),
                    SpawnRadiusEnd = ReadSingleBE(bytes, e + 0x28),
                    FadeTime = ReadUInt16BE(bytes, e + 0x2C),
                    Reserved2 = ReadUInt16BE(bytes, e + 0x2E),
                    FadeTime2 = ReadSingleBE(bytes, e + 0x30),
                    Blowout = ReadSingleBE(bytes, e + 0x34),
                    RotationW = ReadSingleBE(bytes, e + 0x38),
                    RotationX = ReadSingleBE(bytes, e + 0x3C),
                    RotationY = ReadSingleBE(bytes, e + 0x40),
                    RotationZ = ReadSingleBE(bytes, e + 0x44),
                    RandomRotationFactorCandidate = ReadSingleBE(bytes, e + 0x48),
                    Transparency = ReadSingleBE(bytes, e + 0x4C),
                    AlphaMiddleCandidate = ReadSingleBE(bytes, e + 0x50),
                    AlphaEndCandidate = ReadSingleBE(bytes, e + 0x54),
                    AlphaBlendFactorCandidate = ReadSingleBE(bytes, e + 0x58),
                    ScaleStartX = ReadSingleBE(bytes, e + 0x5C),
                    ScaleStartY = ReadSingleBE(bytes, e + 0x60),
                    ScaleStartZ = ReadSingleBE(bytes, e + 0x64),
                    ScaleMiddleX = ReadSingleBE(bytes, e + 0x68),
                    ScaleMiddleY = ReadSingleBE(bytes, e + 0x6C),
                    ScaleMiddleZ = ReadSingleBE(bytes, e + 0x70),
                    ScaleEndX = ReadSingleBE(bytes, e + 0x74),
                    ScaleEndY = ReadSingleBE(bytes, e + 0x78),
                    ScaleEndZ = ReadSingleBE(bytes, e + 0x7C),
                    AddRandomScaleStartX = ReadSingleBE(bytes, e + 0x80),
                    AddRandomScaleStartY = ReadSingleBE(bytes, e + 0x84),
                    AddRandomScaleStartZ = ReadSingleBE(bytes, e + 0x88),
                    MiddleScalePoint = ReadSingleBE(bytes, e + 0x8C),
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
                    Reserved3 = ReadUInt32BE(bytes, e + 0xC4),
                    Reserved4 = ReadUInt64BE(bytes, e + 0xC8)
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
                    Reserved0 = ReadUInt64BE(bytes, e + 0x08),
                    ResourceParam1Candidate = ReadHalfBE(bytes, e + 0x10),
                    ResourceParam2Candidate = ReadHalfBE(bytes, e + 0x12),
                    Reserved1 = ReadUInt32BE(bytes, e + 0x14),
                    Reserved2 = ReadUInt32BE(bytes, e + 0x18),
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
                    CoordChunkIndex = ReadUInt32BE(bytes, e + 0x00),
                    ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04),
                    PositionFlag1Candidate = ReadUInt32BE(bytes, e + 0x08),
                    PositionFlag2Candidate = ReadUInt32BE(bytes, e + 0x0C),
                    PositionFlag3Candidate = ReadUInt32BE(bytes, e + 0x10),
                    InheritAxisValueCandidate = ReadSingleBE(bytes, e + 0x14),
                    PositionValue2Candidate = ReadSingleBE(bytes, e + 0x18),
                    PositionFlag4Candidate = ReadUInt32BE(bytes, e + 0x1C),
                    PositionFlag5Candidate = ReadUInt32BE(bytes, e + 0x20),
                    PositionFlag6Candidate = ReadUInt32BE(bytes, e + 0x24),
                    PositionFlag7Candidate = ReadUInt32BE(bytes, e + 0x28),
                    PositionFlag8Candidate = ReadUInt32BE(bytes, e + 0x2C),
                    ClumpChunkIndex = ReadUInt32BE(bytes, e + 0x30),
                    PositionFlag9Candidate = ReadUInt32BE(bytes, e + 0x34)
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
                    CoordChunkIndex = ReadUInt32BE(bytes, e + 0x00),
                    ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04),
                    ForceFlag1Candidate = ReadUInt32BE(bytes, e + 0x08),
                    ForceFlag2Candidate = ReadUInt32BE(bytes, e + 0x0C),
                    ForceFlag3Candidate = ReadUInt32BE(bytes, e + 0x10),
                    InheritAxisValueCandidate = ReadSingleBE(bytes, e + 0x14),
                    ForceValue1Candidate = ReadSingleBE(bytes, e + 0x18),
                    ForceFlag4Candidate = ReadUInt32BE(bytes, e + 0x1C),
                    ForceFlag5Candidate = ReadUInt32BE(bytes, e + 0x20),
                    ForceFlag6Candidate = ReadUInt32BE(bytes, e + 0x24),
                    ForceFlag7Candidate = ReadUInt32BE(bytes, e + 0x28),
                    ForceFlag8Candidate = ReadUInt32BE(bytes, e + 0x2C),
                    ForceFlag9aCandidate = ReadUInt16BE(bytes, e + 0x30),
                    ForceFlag9bCandidate = ReadUInt16BE(bytes, e + 0x32),
                    ForceFlag10Candidate = ReadUInt32BE(bytes, e + 0x34),
                    ParticleSpeedCandidate = ReadSingleBE(bytes, e + 0x38),
                    ForceEnableFlagCandidate = ReadUInt32BE(bytes, e + 0x3C),
                    ForceValue2Candidate = ReadSingleBE(bytes, e + 0x40),
                    ForceValue3Candidate = ReadSingleBE(bytes, e + 0x44),
                    AngleParam1Candidate = ReadHalfBE(bytes, e + 0x48),
                    AngleParam2Candidate = ReadHalfBE(bytes, e + 0x4A),
                    AngleParam3Candidate = ReadHalfBE(bytes, e + 0x4C),
                    AngleParam4Candidate = ReadHalfBE(bytes, e + 0x4E),
                    AngleOrFalloff1Candidate = ReadSingleBE(bytes, e + 0x50),
                    AngleOrFalloff2Candidate = ReadSingleBE(bytes, e + 0x54),
                    ForceValue4Candidate = ReadSingleBE(bytes, e + 0x58),
                    ForceValue5Candidate = ReadSingleBE(bytes, e + 0x5C),
                    ClumpChunkIndex = ReadUInt32BE(bytes, e + 0x60),
                    TrailingReserved1 = ReadUInt32BE(bytes, e + 0x64),
                    TrailingReserved2 = ReadUInt32BE(bytes, e + 0x68),
                    TrailingReserved3 = ReadUInt32BE(bytes, e + 0x6C)
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
                    entry.Frames.Add(ReadUInt32BE(bytes, current));
                    current += 4;
                }

                if (((frameCount * 4) + 4) % 8 != 0)
                    current += 4;

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
                WriteUInt64BE(output, e + 0x08, entry.Reserved0);
                output[e + 0x10] = entry.DisableStretch ? (byte)1 : (byte)0;
                output[e + 0x11] = (byte)entry.SpawnType;
                output[e + 0x12] = entry.BlowoutDisabler;
                output[e + 0x13] = entry.AttachToBone ? (byte)1 : (byte)0;
                output[e + 0x14] = (byte)entry.RandomRollType;
                output[e + 0x15] = entry.FrameRate;
                output[e + 0x16] = entry.ReservedBehaviorFlag;
                output[e + 0x17] = (byte)entry.ParticleType;
                WriteUInt16BE(output, e + 0x18, entry.Reserved1);
                output[e + 0x1A] = entry.GravityModeFlagCandidate;
                output[e + 0x1B] = entry.StartGravity;
                WriteInt16BE(output, e + 0x1C, entry.FrameEnd);
                WriteUInt16BE(output, e + 0x1E, entry.LifetimeModeFlagCandidate);
                WriteSingleBE(output, e + 0x20, entry.ParticleCount);
                WriteSingleBE(output, e + 0x24, entry.SpawnRadiusStart);
                WriteSingleBE(output, e + 0x28, entry.SpawnRadiusEnd);
                WriteUInt16BE(output, e + 0x2C, entry.FadeTime);
                WriteUInt16BE(output, e + 0x2E, entry.Reserved2);
                WriteSingleBE(output, e + 0x30, entry.FadeTime2);
                WriteSingleBE(output, e + 0x34, entry.Blowout);
                WriteSingleBE(output, e + 0x38, entry.RotationW);
                WriteSingleBE(output, e + 0x3C, entry.RotationX);
                WriteSingleBE(output, e + 0x40, entry.RotationY);
                WriteSingleBE(output, e + 0x44, entry.RotationZ);
                WriteSingleBE(output, e + 0x48, entry.RandomRotationFactorCandidate);
                WriteSingleBE(output, e + 0x4C, entry.Transparency);
                WriteSingleBE(output, e + 0x50, entry.AlphaMiddleCandidate);
                WriteSingleBE(output, e + 0x54, entry.AlphaEndCandidate);
                WriteSingleBE(output, e + 0x58, entry.AlphaBlendFactorCandidate);
                WriteSingleBE(output, e + 0x5C, entry.ScaleStartX);
                WriteSingleBE(output, e + 0x60, entry.ScaleStartY);
                WriteSingleBE(output, e + 0x64, entry.ScaleStartZ);
                WriteSingleBE(output, e + 0x68, entry.ScaleMiddleX);
                WriteSingleBE(output, e + 0x6C, entry.ScaleMiddleY);
                WriteSingleBE(output, e + 0x70, entry.ScaleMiddleZ);
                WriteSingleBE(output, e + 0x74, entry.ScaleEndX);
                WriteSingleBE(output, e + 0x78, entry.ScaleEndY);
                WriteSingleBE(output, e + 0x7C, entry.ScaleEndZ);
                WriteSingleBE(output, e + 0x80, entry.AddRandomScaleStartX);
                WriteSingleBE(output, e + 0x84, entry.AddRandomScaleStartY);
                WriteSingleBE(output, e + 0x88, entry.AddRandomScaleStartZ);
                WriteSingleBE(output, e + 0x8C, entry.MiddleScalePoint);
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
                WriteUInt32BE(output, e + 0xC4, entry.Reserved3);
                WriteUInt64BE(output, e + 0xC8, entry.Reserved4);
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
                WriteUInt64BE(output, e + 0x08, entry.Reserved0);
                WriteHalfBE(output, e + 0x10, entry.ResourceParam1Candidate);
                WriteHalfBE(output, e + 0x12, entry.ResourceParam2Candidate);
                WriteUInt32BE(output, e + 0x14, entry.Reserved1);
                WriteUInt32BE(output, e + 0x18, entry.Reserved2);
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
                WriteUInt32BE(output, e + 0x00, entry.CoordChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteUInt32BE(output, e + 0x08, entry.PositionFlag1Candidate);
                WriteUInt32BE(output, e + 0x0C, entry.PositionFlag2Candidate);
                WriteUInt32BE(output, e + 0x10, entry.PositionFlag3Candidate);
                WriteSingleBE(output, e + 0x14, entry.InheritAxisValueCandidate);
                WriteSingleBE(output, e + 0x18, entry.PositionValue2Candidate);
                WriteUInt32BE(output, e + 0x1C, entry.PositionFlag4Candidate);
                WriteUInt32BE(output, e + 0x20, entry.PositionFlag5Candidate);
                WriteUInt32BE(output, e + 0x24, entry.PositionFlag6Candidate);
                WriteUInt32BE(output, e + 0x28, entry.PositionFlag7Candidate);
                WriteUInt32BE(output, e + 0x2C, entry.PositionFlag8Candidate);
                WriteUInt32BE(output, e + 0x30, entry.ClumpChunkIndex);
                WriteUInt32BE(output, e + 0x34, entry.PositionFlag9Candidate);
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
                WriteUInt32BE(output, e + 0x00, entry.CoordChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteUInt32BE(output, e + 0x08, entry.ForceFlag1Candidate);
                WriteUInt32BE(output, e + 0x0C, entry.ForceFlag2Candidate);
                WriteUInt32BE(output, e + 0x10, entry.ForceFlag3Candidate);
                WriteSingleBE(output, e + 0x14, entry.InheritAxisValueCandidate);
                WriteSingleBE(output, e + 0x18, entry.ForceValue1Candidate);
                WriteUInt32BE(output, e + 0x1C, entry.ForceFlag4Candidate);
                WriteUInt32BE(output, e + 0x20, entry.ForceFlag5Candidate);
                WriteUInt32BE(output, e + 0x24, entry.ForceFlag6Candidate);
                WriteUInt32BE(output, e + 0x28, entry.ForceFlag7Candidate);
                WriteUInt32BE(output, e + 0x2C, entry.ForceFlag8Candidate);
                WriteUInt16BE(output, e + 0x30, entry.ForceFlag9aCandidate);
                WriteUInt16BE(output, e + 0x32, entry.ForceFlag9bCandidate);
                WriteUInt32BE(output, e + 0x34, entry.ForceFlag10Candidate);
                WriteSingleBE(output, e + 0x38, entry.ParticleSpeedCandidate);
                WriteUInt32BE(output, e + 0x3C, entry.ForceEnableFlagCandidate);
                WriteSingleBE(output, e + 0x40, entry.ForceValue2Candidate);
                WriteSingleBE(output, e + 0x44, entry.ForceValue3Candidate);
                WriteHalfBE(output, e + 0x48, entry.AngleParam1Candidate);
                WriteHalfBE(output, e + 0x4A, entry.AngleParam2Candidate);
                WriteHalfBE(output, e + 0x4C, entry.AngleParam3Candidate);
                WriteHalfBE(output, e + 0x4E, entry.AngleParam4Candidate);
                WriteSingleBE(output, e + 0x50, entry.AngleOrFalloff1Candidate);
                WriteSingleBE(output, e + 0x54, entry.AngleOrFalloff2Candidate);
                WriteSingleBE(output, e + 0x58, entry.ForceValue4Candidate);
                WriteSingleBE(output, e + 0x5C, entry.ForceValue5Candidate);
                WriteUInt32BE(output, e + 0x60, entry.ClumpChunkIndex);
                WriteUInt32BE(output, e + 0x64, entry.TrailingReserved1);
                WriteUInt32BE(output, e + 0x68, entry.TrailingReserved2);
                WriteUInt32BE(output, e + 0x6C, entry.TrailingReserved3);
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

                foreach (uint frame in entry.Frames)
                {
                    byte[] frameBytes = new byte[4];
                    WriteUInt32BE(frameBytes, 0, frame);
                    output.AddRange(frameBytes);
                }

                if (((entry.Frames.Count * 4) + 4) % 8 != 0)
                    output.AddRange(new byte[4]);
            }

            return output.ToArray();
        }

        public static uint ReadUInt32BE(byte[] bytes, int offset) => (uint)((bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3]);
        public static ushort ReadUInt16BE(byte[] bytes, int offset) => (ushort)((bytes[offset] << 8) | bytes[offset + 1]);
        public static short ReadInt16BE(byte[] bytes, int offset) => (short)ReadUInt16BE(bytes, offset);
        public static ulong ReadUInt64BE(byte[] bytes, int offset) => ((ulong)ReadUInt32BE(bytes, offset) << 32) | ReadUInt32BE(bytes, offset + 4);

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

        public static void WriteUInt16BE(byte[] bytes, int offset, ushort value)
        {
            bytes[offset] = (byte)(value >> 8);
            bytes[offset + 1] = (byte)value;
        }

        public static void WriteInt16BE(byte[] bytes, int offset, short value) => WriteUInt16BE(bytes, offset, unchecked((ushort)value));
        public static void WriteUInt64BE(byte[] bytes, int offset, ulong value)
        {
            WriteUInt32BE(bytes, offset, (uint)(value >> 32));
            WriteUInt32BE(bytes, offset + 4, (uint)value);
        }

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
        public int Frame { get; set; }
    }

    internal sealed class ParticleManagerEntry
    {
        [Category("Links")]
        public uint AnimationChunkIndex { get; set; }
        [Category("Links")]
        public uint EntryIndex { get; set; }
        [Category("Reserved")]
        public ulong Reserved0 { get; set; }
        [Category("Spawn")]
        [DisplayName("Stretch Disabled")]
        public bool DisableStretch { get; set; }
        [Category("Spawn")]
        public ParticleSpawnType SpawnType { get; set; }
        [Category("Spawn")]
        public byte BlowoutDisabler { get; set; }
        [Category("Spawn")]
        public bool AttachToBone { get; set; }
        [Category("Rotation")]
        public ParticleRandomRollType RandomRollType { get; set; }
        [Category("Timing")]
        public byte FrameRate { get; set; }
        [Category("Reserved")]
        public byte ReservedBehaviorFlag { get; set; }
        [Category("Spawn")]
        public int ParticleType { get; set; }
        [Category("Reserved")]
        public ushort Reserved1 { get; set; }
        [Category("Gravity")]
        public byte GravityModeFlagCandidate { get; set; }
        [Category("Gravity")]
        public byte StartGravity { get; set; }
        [Category("Timing")]
        public short FrameEnd { get; set; }
        [Category("Timing")]
        public ushort LifetimeModeFlagCandidate { get; set; }
        [Category("Spawn")]
        public float ParticleCount { get; set; }
        [Category("Spawn")]
        public float SpawnRadiusStart { get; set; }
        [Category("Spawn")]
        public float SpawnRadiusEnd { get; set; }
        [Category("Timing")]
        public ushort FadeTime { get; set; }
        [Category("Reserved")]
        public ushort Reserved2 { get; set; }
        [Category("Timing")]
        public float FadeTime2 { get; set; }
        [Category("Movement")]
        public float Blowout { get; set; }
        [Category("Rotation")]
        public float RotationW { get; set; }
        [Category("Rotation")]
        public float RotationX { get; set; }
        [Category("Rotation")]
        public float RotationY { get; set; }
        [Category("Rotation")]
        public float RotationZ { get; set; }
        [Category("Rotation")]
        public float RandomRotationFactorCandidate { get; set; }
        [Category("Alpha")]
        public float Transparency { get; set; }
        [Category("Alpha")]
        public float AlphaMiddleCandidate { get; set; }
        [Category("Alpha")]
        public float AlphaEndCandidate { get; set; }
        [Category("Alpha")]
        public float AlphaBlendFactorCandidate { get; set; }
        [Category("Scale")]
        public float ScaleStartX { get; set; }
        [Category("Scale")]
        public float ScaleStartY { get; set; }
        [Category("Scale")]
        public float ScaleStartZ { get; set; }
        [Category("Scale")]
        public float ScaleMiddleX { get; set; }
        [Category("Scale")]
        public float ScaleMiddleY { get; set; }
        [Category("Scale")]
        public float ScaleMiddleZ { get; set; }
        [Category("Scale")]
        public float ScaleEndX { get; set; }
        [Category("Scale")]
        public float ScaleEndY { get; set; }
        [Category("Scale")]
        public float ScaleEndZ { get; set; }
        [Category("Scale")]
        [DisplayName("Random Scale Start X")]
        public float AddRandomScaleStartX { get; set; }
        [Category("Scale")]
        [DisplayName("Random Scale Start Y")]
        public float AddRandomScaleStartY { get; set; }
        [Category("Scale")]
        [DisplayName("Random Scale Start Z")]
        public float AddRandomScaleStartZ { get; set; }
        [Category("Scale")]
        public float MiddleScalePoint { get; set; }
        [Category("Color")]
        public float ColorStartR { get; set; }
        [Category("Color")]
        public float ColorStartG { get; set; }
        [Category("Color")]
        public float ColorStartB { get; set; }
        [Category("Color")]
        public float ColorStartA { get; set; }
        [Category("Color")]
        public float ColorMiddleR { get; set; }
        [Category("Color")]
        public float ColorMiddleG { get; set; }
        [Category("Color")]
        public float ColorMiddleB { get; set; }
        [Category("Color")]
        public float ColorMiddleA { get; set; }
        [Category("Color")]
        public float ColorEndR { get; set; }
        [Category("Color")]
        public float ColorEndG { get; set; }
        [Category("Color")]
        public float ColorEndB { get; set; }
        [Category("Color")]
        public float ColorEndA { get; set; }
        [Category("Color")]
        public float ColorFactor { get; set; }
        [Category("Reserved")]
        public uint Reserved3 { get; set; }
        [Category("Reserved")]
        public ulong Reserved4 { get; set; }
    }

    internal sealed class ParticleResourceEntry
    {
        [Category("Links")]
        public uint EffectChunkIndex { get; set; }
        [Category("Links")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Reserved")]
        public ulong Reserved0 { get; set; }
        [Category("Resource")]
        public float ResourceParam1Candidate { get; set; }
        [Category("Resource")]
        public float ResourceParam2Candidate { get; set; }
        [Category("Reserved")]
        public uint Reserved1 { get; set; }
        [Category("Reserved")]
        public uint Reserved2 { get; set; }
        [Category("Resource")]
        public ParticleEffectChunkType EffectChunkType { get; set; }
    }

    internal sealed class ParticlePositionEntry
    {
        [Category("Links")]
        public uint CoordChunkIndex { get; set; }
        [Category("Links")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Flags")]
        public uint PositionFlag1Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag2Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag3Candidate { get; set; }
        [Category("Values")]
        public float InheritAxisValueCandidate { get; set; }
        [Category("Values")]
        public float PositionValue2Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag4Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag5Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag6Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag7Candidate { get; set; }
        [Category("Flags")]
        public uint PositionFlag8Candidate { get; set; }
        [Category("Links")]
        public uint ClumpChunkIndex { get; set; }
        [Category("Flags")]
        public uint PositionFlag9Candidate { get; set; }
    }

    internal sealed class ParticleForceFieldEntry
    {
        [Category("Links")]
        public uint CoordChunkIndex { get; set; }
        [Category("Links")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Flags")]
        public uint ForceFlag1Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag2Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag3Candidate { get; set; }
        [Category("Values")]
        public float InheritAxisValueCandidate { get; set; }
        [Category("Values")]
        public float ForceValue1Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag4Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag5Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag6Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag7Candidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag8Candidate { get; set; }
        [Category("Flags")]
        public ushort ForceFlag9aCandidate { get; set; }
        [Category("Flags")]
        public ushort ForceFlag9bCandidate { get; set; }
        [Category("Flags")]
        public uint ForceFlag10Candidate { get; set; }
        [Category("Values")]
        public float ParticleSpeedCandidate { get; set; }
        [Category("Flags")]
        public uint ForceEnableFlagCandidate { get; set; }
        [Category("Values")]
        public float ForceValue2Candidate { get; set; }
        [Category("Values")]
        public float ForceValue3Candidate { get; set; }
        [Category("Angles")]
        public float AngleParam1Candidate { get; set; }
        [Category("Angles")]
        public float AngleParam2Candidate { get; set; }
        [Category("Angles")]
        public float AngleParam3Candidate { get; set; }
        [Category("Angles")]
        public float AngleParam4Candidate { get; set; }
        [Category("Angles")]
        public float AngleOrFalloff1Candidate { get; set; }
        [Category("Angles")]
        public float AngleOrFalloff2Candidate { get; set; }
        [Category("Values")]
        public float ForceValue4Candidate { get; set; }
        [Category("Values")]
        public float ForceValue5Candidate { get; set; }
        [Category("Links")]
        public uint ClumpChunkIndex { get; set; }
        [Category("Reserved")]
        public uint TrailingReserved1 { get; set; }
        [Category("Reserved")]
        public uint TrailingReserved2 { get; set; }
        [Category("Reserved")]
        public uint TrailingReserved3 { get; set; }
    }

    internal sealed class ParticleNodeEntry
    {
        public readonly List<uint> Frames = new List<uint>();
    }
}
