using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NSUNS4_Character_Manager
{
    public enum ParticleEmitMode : byte
    {
        Continuous = 0,
        Burst = 1
    }

    [Flags]
    public enum ParticleGeneratorFlags : ushort
    {
        NoFlags = 0x00,
        RandomScaleXYZ = 0x01,
        UseColor = 0x02,
        ScaleAndColor = 0x03,
        SmoothPath = 0x10,
        PathAndScale = 0x11,
        PathAndColor = 0x12,
        AllFlags = 0x13
    }

    [Flags]
    public enum ParticleForceScope : uint
    {
        NoForces = 0x00000,
        OwnGenerator = 0x00001,
        ThisAnimation = 0x00100,
        GeneratorAnimation = 0x00101,
        WorldForces = 0x10000,
        GeneratorWorld = 0x10001,
        AnimationWorld = 0x10100,
        AllForces = 0x10101
    }

    [Flags]
    public enum ParticleDrawFlags : uint
    {
        NoDrawFlags = 0x00000,
        DrawColor = 0x00020,
        SortParticles = 0x00800,
        VertexColor = 0x10000
    }

    public enum ParticleSpace : byte
    {
        LocalSpace = 0,
        WorldSpace = 1
    }

    public enum ParticleRadiusMode : byte
    {
        Unlimited = 0,
        WithinRadius = 1
    }

    public enum ParticleResourceType : uint
    {
        Clump = 1,
        Animation = 2,
        Sprite3D = 3,
        Sprite2D = 4,
        Billboard = 5
    }

    public enum ParticleFollowMode : byte
    {
        Free = 0,
        Follow = 1,
        Inherit = 2
    }

    public enum ParticleSpawnType : byte
    {
        Point = 0,
        Circle = 1,
        Sphere = 2,
        TubeRandom = 3,
        TubeForward = 4,
        TubeReverse = 5
    }

    public enum ParticleDirectionType : byte
    {
        Outward = 0,
        Inward = 1,
        RandomDirection = 2,
        Cone = 3
    }

    public enum ParticleRotationType : byte
    {
        NoRotation = 0,
        RandomRotation = 1,
        Aligned = 2,
        AlignedRandom = 3
    }

    public enum ParticleForceType : sbyte
    {
        Orbit = 0,
        Speed = 1,
        Radial = 2,
        Move = 3,
        Rotate = 4,
        Scale = 5,
        Accelerate = 6
    }

    public enum ParticleForceFalloff : uint
    {
        Constant = 0,
        InwardFalloff = 1,
        OutwardFalloff = 2
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
            clone.CutoutData = source.CutoutData == null ? null : ReadCutoutData(BuildCutoutData(source.CutoutData));
            return clone;
        }

        public static ParticleManagerEntry CloneManager(ParticleManagerEntry entry)
        {
            return new ParticleManagerEntry
            {
                AnimationChunkIndex = entry.AnimationChunkIndex,
                EntryIndex = entry.EntryIndex,
                Reserved08Word0 = entry.Reserved08Word0,
                Reserved08Word1 = entry.Reserved08Word1,
                AllocationMode = entry.AllocationMode,
                SpawnType = entry.SpawnType,
                DirectionType = entry.DirectionType,
                ParticleInstanceMode = entry.ParticleInstanceMode,
                RotationType = entry.RotationType,
                ControlRate = entry.ControlRate,
                GeneratorFlags = entry.GeneratorFlags,
                ForceFieldMask = entry.ForceFieldMask,
                GeneratorEndTime = entry.GeneratorEndTime,
                Cutout = entry.Cutout,
                Reserved1F = entry.Reserved1F,
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
                FadeIn = entry.FadeIn,
                FadeOut = entry.FadeOut,
                ForceMultiplier = entry.ForceMultiplier,
                ForceRandomness = entry.ForceRandomness,
                StartScaleX = entry.StartScaleX,
                StartScaleY = entry.StartScaleY,
                StartScaleZ = entry.StartScaleZ,
                RandomScaleX = entry.RandomScaleX,
                RandomScaleY = entry.RandomScaleY,
                RandomScaleZ = entry.RandomScaleZ,
                MiddleScaleX = entry.MiddleScaleX,
                MiddleScaleY = entry.MiddleScaleY,
                MiddleScaleZ = entry.MiddleScaleZ,
                EndScaleX = entry.EndScaleX,
                EndScaleY = entry.EndScaleY,
                EndScaleZ = entry.EndScaleZ,
                ScaleInterpolationPoint = entry.ScaleInterpolationPoint,
                StartColorR = entry.StartColorR,
                StartColorG = entry.StartColorG,
                StartColorB = entry.StartColorB,
                StartColorA = entry.StartColorA,
                MiddleColorR = entry.MiddleColorR,
                MiddleColorG = entry.MiddleColorG,
                MiddleColorB = entry.MiddleColorB,
                MiddleColorA = entry.MiddleColorA,
                EndColorR = entry.EndColorR,
                EndColorG = entry.EndColorG,
                EndColorB = entry.EndColorB,
                EndColorA = entry.EndColorA,
                ColorInterpolationPoint = entry.ColorInterpolationPoint,
                FieldC4 = entry.FieldC4,
                FieldC8 = entry.FieldC8,
                FieldCC = entry.FieldCC,
            };
        }

        public static ParticleResourceEntry CloneResource(ParticleResourceEntry entry)
        {
            return new ParticleResourceEntry
            {
                EffectChunkIndex = entry.EffectChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Reserved08Word0 = entry.Reserved08Word0,
                Reserved08Word1 = entry.Reserved08Word1,
                Field10 = entry.Field10,
                Field14 = entry.Field14,
                DrawFlags = entry.DrawFlags,
                ResourceType = entry.ResourceType,
            };
        }

        public static ParticlePositionEntry ClonePosition(ParticlePositionEntry entry)
        {
            return new ParticlePositionEntry
            {
                CoordChunkIndex = entry.CoordChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Field08 = entry.Field08,
                Reserved0C = entry.Reserved0C,
                DirectionX = entry.DirectionX,
                DirectionY = entry.DirectionY,
                DirectionZ = entry.DirectionZ,
                NodeEnd = entry.NodeEnd,
                WorldSpace = entry.WorldSpace,
                Reserved24Word0 = entry.Reserved24Word0,
                Reserved24Word1 = entry.Reserved24Word1,
                Reserved24Word2 = entry.Reserved24Word2,
                ClumpChunkIndex = entry.ClumpChunkIndex,
                Field34 = entry.Field34,
                HasVersion78Fields = entry.HasVersion78Fields,
            };
        }

        public static ParticleForceFieldEntry CloneForceField(ParticleForceFieldEntry entry)
        {
            return new ParticleForceFieldEntry
            {
                CoordChunkIndex = entry.CoordChunkIndex,
                ParticleEntryIndex = entry.ParticleEntryIndex,
                Field08 = entry.Field08,
                Reserved0C = entry.Reserved0C,
                DirectionX = entry.DirectionX,
                DirectionY = entry.DirectionY,
                DirectionZ = entry.DirectionZ,
                NodeEnd = entry.NodeEnd,
                WorldSpace = entry.WorldSpace,
                Reserved24Word0 = entry.Reserved24Word0,
                Reserved24Word1 = entry.Reserved24Word1,
                Reserved24Word2 = entry.Reserved24Word2,
                CalcType = entry.CalcType,
                DirectionSpace = entry.DirectionSpace,
                UseRadius = entry.UseRadius,
                Reserved33 = entry.Reserved33,
                ForceScope = entry.ForceScope,
                Radius = entry.Radius,
                Falloff = entry.Falloff,
                Strength = entry.Strength,
                StrengthAdjustment = entry.StrengthAdjustment,
                Reserved48Word0 = entry.Reserved48Word0,
                Reserved48Word1 = entry.Reserved48Word1,
                RotationScaleX = entry.RotationScaleX,
                RotationScaleY = entry.RotationScaleY,
                RotationScaleZ = entry.RotationScaleZ,
                Field5C = entry.Field5C,
                ClumpChunkIndex = entry.ClumpChunkIndex,
                Field64 = entry.Field64,
                Reserved68Word0 = entry.Reserved68Word0,
                Reserved68Word1 = entry.Reserved68Word1,
                HasVersion78Fields = entry.HasVersion78Fields,
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
                    Action = (frame.RawValue & 0x80000000u) != 0 ? ParticleNodeAction.On : (frame.RawValue & 0x40000000u) != 0 ? ParticleNodeAction.Clear : ParticleNodeAction.Off,
                    TimeMilliseconds = frame.RawValue & 0x0FFFFFFFu,
                    PreservedFlags = frame.RawValue & ((frame.RawValue & 0x80000000u) != 0 ? 0x70000000u : 0x30000000u)
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
                    RawValue = (particleEvent.TimeMilliseconds & 0x0FFFFFFFu) | (particleEvent.PreservedFlags & (particleEvent.Action == ParticleNodeAction.On ? 0x70000000u : 0x30000000u)) | (particleEvent.Action == ParticleNodeAction.On ? 0x80000000u : particleEvent.Action == ParticleNodeAction.Clear ? 0x40000000u : 0u)
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

            state.UsesVersion78Layout = state.Version == 0 ? DetectVersion78Layout(state) : state.Version > 0x77;
            state.Headers[0].Size = state.Headers[0].Count * ParticleManagerSize;
            state.Headers[1].Size = state.Headers[1].Count * ParticleResourceSize;
            state.Headers[2].Size = state.Headers[2].Count * (state.UsesVersion78Layout ? ParticlePositionSize : ParticlePositionLegacySize);
            state.Headers[3].Size = state.Headers[3].Count * (state.UsesVersion78Layout ? ParticleForceFieldSize : ParticleForceFieldLegacySize);

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

            if (HasCutoutData(state))
            {
                try { state.CutoutData = ReadCutoutData(state.ExtendedData); }
                catch (InvalidDataException) { return false; }
            }

            chunk = state;
            return true;
        }

        public static byte[] BuildChunkData(ParticleChunkState chunk)
        {
            if (chunk.Version != 0)
                chunk.UsesVersion78Layout = chunk.Version > 0x77;
            byte[] managerBytes = BuildManagers(chunk.Managers);
            byte[] resourceBytes = BuildResources(chunk.Resources);
            byte[] positionBytes = BuildPositions(chunk.Positions, chunk.UsesVersion78Layout);
            byte[] forceFieldBytes = BuildForceFields(chunk.ForceFields, chunk.UsesVersion78Layout);
            byte[] nodeBytes = BuildNodes(chunk.Nodes);
            byte[] extendedData = chunk.CutoutData != null ? BuildCutoutData(chunk.CutoutData)
                : HasCutoutData(chunk) ? BuildCutoutData(new ParticleCutoutData()) : chunk.ExtendedData ?? new byte[0];

            byte[] output = new byte[HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length + nodeBytes.Length + extendedData.Length];
            Buffer.BlockCopy(managerBytes, 0, output, HeaderTableSize, managerBytes.Length);
            Buffer.BlockCopy(resourceBytes, 0, output, HeaderTableSize + managerBytes.Length, resourceBytes.Length);
            Buffer.BlockCopy(positionBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length, positionBytes.Length);
            Buffer.BlockCopy(forceFieldBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length, forceFieldBytes.Length);
            Buffer.BlockCopy(nodeBytes, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length, nodeBytes.Length);
            Buffer.BlockCopy(extendedData, 0, output, HeaderTableSize + managerBytes.Length + resourceBytes.Length + positionBytes.Length + forceFieldBytes.Length + nodeBytes.Length, extendedData.Length);

            WriteSectionHeader(output, 0x00, chunk.Headers[0].Value == 0 ? DefaultHeaderValues[0] : chunk.Headers[0].Value, chunk.Managers.Count, ParticleManagerSize);
            WriteSectionHeader(output, 0x08, chunk.Headers[1].Value == 0 ? DefaultHeaderValues[1] : chunk.Headers[1].Value, chunk.Resources.Count, ParticleResourceSize);
            WriteSectionHeader(output, 0x10, chunk.Headers[2].Value == 0 ? DefaultHeaderValues[2] : chunk.Headers[2].Value, chunk.Positions.Count, chunk.UsesVersion78Layout ? ParticlePositionSize : ParticlePositionLegacySize);
            WriteSectionHeader(output, 0x18, chunk.Headers[3].Value == 0 ? DefaultHeaderValues[3] : chunk.Headers[3].Value, chunk.ForceFields.Count, chunk.UsesVersion78Layout ? ParticleForceFieldSize : ParticleForceFieldLegacySize);
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
                return chunk.Headers[2].Size == ParticlePositionSize || chunk.Headers[2].Size == chunk.Headers[2].Count * ParticlePositionSize;
            if (chunk.Headers[3].Count > 0)
                return chunk.Headers[3].Size == ParticleForceFieldSize || chunk.Headers[3].Size == chunk.Headers[3].Count * ParticleForceFieldSize;
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
                int e = offset + i * ParticleManagerSize;
                var entry = new ParticleManagerEntry();
                entry.AnimationChunkIndex = ReadUInt32BE(bytes, e + 0x00);
                entry.EntryIndex = ReadUInt32BE(bytes, e + 0x04);
                entry.Reserved08Word0 = ReadUInt32BE(bytes, e + 0x08);
                entry.Reserved08Word1 = ReadUInt32BE(bytes, e + 0x0C);
                entry.AllocationMode = (ParticleEmitMode)bytes[e + 0x10];
                entry.SpawnType = (ParticleSpawnType)bytes[e + 0x11];
                entry.DirectionType = (ParticleDirectionType)bytes[e + 0x12];
                entry.ParticleInstanceMode = (ParticleFollowMode)bytes[e + 0x13];
                entry.RotationType = (ParticleRotationType)bytes[e + 0x14];
                entry.ControlRate = bytes[e + 0x15];
                entry.GeneratorFlags = (ParticleGeneratorFlags)ReadUInt16BE(bytes, e + 0x16);
                entry.ForceFieldMask = (ParticleForceScope)ReadUInt32BE(bytes, e + 0x18);
                entry.GeneratorEndTime = ReadInt16BE(bytes, e + 0x1C);
                entry.Cutout = bytes[e + 0x1E];
                entry.Reserved1F = bytes[e + 0x1F];
                entry.ParticleCountOrRate = ReadSingleBE(bytes, e + 0x20);
                entry.SpawnRadius = ReadSingleBE(bytes, e + 0x24);
                entry.SpawnRadiusRandomness = ReadSingleBE(bytes, e + 0x28);
                entry.Lifetime = ReadUInt16BE(bytes, e + 0x2C);
                entry.Reserved2E = ReadUInt16BE(bytes, e + 0x2E);
                entry.LifetimeRandomness = ReadSingleBE(bytes, e + 0x30);
                entry.InitialSpeed = ReadSingleBE(bytes, e + 0x34);
                entry.InitialSpeedRandomness = ReadSingleBE(bytes, e + 0x38);
                entry.EmissionAngle1 = ReadSingleBE(bytes, e + 0x3C);
                entry.EmissionAngle2 = ReadSingleBE(bytes, e + 0x40);
                entry.EmissionAngle1Randomness = ReadSingleBE(bytes, e + 0x44);
                entry.EmissionAngle2Randomness = ReadSingleBE(bytes, e + 0x48);
                entry.FadeIn = ReadSingleBE(bytes, e + 0x4C);
                entry.FadeOut = ReadSingleBE(bytes, e + 0x50);
                entry.ForceMultiplier = ReadSingleBE(bytes, e + 0x54);
                entry.ForceRandomness = ReadSingleBE(bytes, e + 0x58);
                entry.StartScaleX = ReadSingleBE(bytes, e + 0x5C);
                entry.StartScaleY = ReadSingleBE(bytes, e + 0x60);
                entry.StartScaleZ = ReadSingleBE(bytes, e + 0x64);
                entry.RandomScaleX = ReadSingleBE(bytes, e + 0x68);
                entry.RandomScaleY = ReadSingleBE(bytes, e + 0x6C);
                entry.RandomScaleZ = ReadSingleBE(bytes, e + 0x70);
                entry.MiddleScaleX = ReadSingleBE(bytes, e + 0x74);
                entry.MiddleScaleY = ReadSingleBE(bytes, e + 0x78);
                entry.MiddleScaleZ = ReadSingleBE(bytes, e + 0x7C);
                entry.EndScaleX = ReadSingleBE(bytes, e + 0x80);
                entry.EndScaleY = ReadSingleBE(bytes, e + 0x84);
                entry.EndScaleZ = ReadSingleBE(bytes, e + 0x88);
                entry.ScaleInterpolationPoint = ReadSingleBE(bytes, e + 0x8C);
                entry.StartColorR = ReadSingleBE(bytes, e + 0x90);
                entry.StartColorG = ReadSingleBE(bytes, e + 0x94);
                entry.StartColorB = ReadSingleBE(bytes, e + 0x98);
                entry.StartColorA = ReadSingleBE(bytes, e + 0x9C);
                entry.MiddleColorR = ReadSingleBE(bytes, e + 0xA0);
                entry.MiddleColorG = ReadSingleBE(bytes, e + 0xA4);
                entry.MiddleColorB = ReadSingleBE(bytes, e + 0xA8);
                entry.MiddleColorA = ReadSingleBE(bytes, e + 0xAC);
                entry.EndColorR = ReadSingleBE(bytes, e + 0xB0);
                entry.EndColorG = ReadSingleBE(bytes, e + 0xB4);
                entry.EndColorB = ReadSingleBE(bytes, e + 0xB8);
                entry.EndColorA = ReadSingleBE(bytes, e + 0xBC);
                entry.ColorInterpolationPoint = ReadSingleBE(bytes, e + 0xC0);
                entry.FieldC4 = ReadUInt32BE(bytes, e + 0xC4);
                entry.FieldC8 = ReadUInt32BE(bytes, e + 0xC8);
                entry.FieldCC = ReadUInt32BE(bytes, e + 0xCC);
                output.Add(entry);
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
                int e = offset + i * ParticleResourceSize;
                var entry = new ParticleResourceEntry();
                entry.EffectChunkIndex = ReadUInt32BE(bytes, e + 0x00);
                entry.ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04);
                entry.Reserved08Word0 = ReadUInt32BE(bytes, e + 0x08);
                entry.Reserved08Word1 = ReadUInt32BE(bytes, e + 0x0C);
                entry.Field10 = ReadUInt32BE(bytes, e + 0x10);
                entry.Field14 = ReadUInt32BE(bytes, e + 0x14);
                entry.DrawFlags = (ParticleDrawFlags)ReadUInt32BE(bytes, e + 0x18);
                entry.ResourceType = (ParticleResourceType)ReadUInt32BE(bytes, e + 0x1C);
                output.Add(entry);
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
                int e = offset + i * entrySize;
                var entry = new ParticlePositionEntry();
                entry.HasVersion78Fields = hasVersion78Fields;
                entry.CoordChunkIndex = ReadInt32BE(bytes, e + 0x00);
                entry.ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04);
                entry.Field08 = ReadUInt32BE(bytes, e + 0x08);
                entry.Reserved0C = ReadUInt32BE(bytes, e + 0x0C);
                entry.DirectionX = ReadSingleBE(bytes, e + 0x10);
                entry.DirectionY = ReadSingleBE(bytes, e + 0x14);
                entry.DirectionZ = ReadSingleBE(bytes, e + 0x18);
                entry.NodeEnd = ReadUInt32BE(bytes, e + 0x1C);
                entry.WorldSpace = ReadUInt32BE(bytes, e + 0x20);
                entry.Reserved24Word0 = ReadUInt32BE(bytes, e + 0x24);
                entry.Reserved24Word1 = ReadUInt32BE(bytes, e + 0x28);
                entry.Reserved24Word2 = ReadUInt32BE(bytes, e + 0x2C);
                if (hasVersion78Fields) entry.ClumpChunkIndex = ReadInt32BE(bytes, e + 0x30);
                if (hasVersion78Fields) entry.Field34 = ReadUInt32BE(bytes, e + 0x34);
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
                int e = offset + i * entrySize;
                var entry = new ParticleForceFieldEntry();
                entry.HasVersion78Fields = hasVersion78Fields;
                entry.CoordChunkIndex = ReadInt32BE(bytes, e + 0x00);
                entry.ParticleEntryIndex = ReadUInt32BE(bytes, e + 0x04);
                entry.Field08 = ReadUInt32BE(bytes, e + 0x08);
                entry.Reserved0C = ReadUInt32BE(bytes, e + 0x0C);
                entry.DirectionX = ReadSingleBE(bytes, e + 0x10);
                entry.DirectionY = ReadSingleBE(bytes, e + 0x14);
                entry.DirectionZ = ReadSingleBE(bytes, e + 0x18);
                entry.NodeEnd = ReadUInt32BE(bytes, e + 0x1C);
                entry.WorldSpace = ReadUInt32BE(bytes, e + 0x20);
                entry.Reserved24Word0 = ReadUInt32BE(bytes, e + 0x24);
                entry.Reserved24Word1 = ReadUInt32BE(bytes, e + 0x28);
                entry.Reserved24Word2 = ReadUInt32BE(bytes, e + 0x2C);
                entry.CalcType = (ParticleForceType)bytes[e + 0x30];
                entry.DirectionSpace = (ParticleSpace)bytes[e + 0x31];
                entry.UseRadius = (ParticleRadiusMode)bytes[e + 0x32];
                entry.Reserved33 = bytes[e + 0x33];
                entry.ForceScope = (ParticleForceScope)ReadUInt32BE(bytes, e + 0x34);
                entry.Radius = ReadSingleBE(bytes, e + 0x38);
                entry.Falloff = (ParticleForceFalloff)ReadUInt32BE(bytes, e + 0x3C);
                entry.Strength = ReadSingleBE(bytes, e + 0x40);
                entry.StrengthAdjustment = ReadSingleBE(bytes, e + 0x44);
                entry.Reserved48Word0 = ReadUInt32BE(bytes, e + 0x48);
                entry.Reserved48Word1 = ReadUInt32BE(bytes, e + 0x4C);
                entry.RotationScaleX = ReadSingleBE(bytes, e + 0x50);
                entry.RotationScaleY = ReadSingleBE(bytes, e + 0x54);
                entry.RotationScaleZ = ReadSingleBE(bytes, e + 0x58);
                entry.Field5C = ReadSingleBE(bytes, e + 0x5C);
                if (hasVersion78Fields) entry.ClumpChunkIndex = ReadInt32BE(bytes, e + 0x60);
                if (hasVersion78Fields) entry.Field64 = ReadUInt32BE(bytes, e + 0x64);
                if (hasVersion78Fields) entry.Reserved68Word0 = ReadUInt32BE(bytes, e + 0x68);
                if (hasVersion78Fields) entry.Reserved68Word1 = ReadUInt32BE(bytes, e + 0x6C);
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
                if (frameCount > (end - current) / 4)
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
                WriteUInt32BE(output, e + 0x08, entry.Reserved08Word0);
                WriteUInt32BE(output, e + 0x0C, entry.Reserved08Word1);
                output[e + 0x10] = (byte)entry.AllocationMode;
                output[e + 0x11] = (byte)entry.SpawnType;
                output[e + 0x12] = (byte)entry.DirectionType;
                output[e + 0x13] = (byte)entry.ParticleInstanceMode;
                output[e + 0x14] = (byte)entry.RotationType;
                output[e + 0x15] = entry.ControlRate;
                WriteUInt16BE(output, e + 0x16, (ushort)entry.GeneratorFlags);
                WriteUInt32BE(output, e + 0x18, (uint)entry.ForceFieldMask);
                WriteInt16BE(output, e + 0x1C, entry.GeneratorEndTime);
                output[e + 0x1E] = entry.Cutout;
                output[e + 0x1F] = entry.Reserved1F;
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
                WriteSingleBE(output, e + 0x4C, entry.FadeIn);
                WriteSingleBE(output, e + 0x50, entry.FadeOut);
                WriteSingleBE(output, e + 0x54, entry.ForceMultiplier);
                WriteSingleBE(output, e + 0x58, entry.ForceRandomness);
                WriteSingleBE(output, e + 0x5C, entry.StartScaleX);
                WriteSingleBE(output, e + 0x60, entry.StartScaleY);
                WriteSingleBE(output, e + 0x64, entry.StartScaleZ);
                WriteSingleBE(output, e + 0x68, entry.RandomScaleX);
                WriteSingleBE(output, e + 0x6C, entry.RandomScaleY);
                WriteSingleBE(output, e + 0x70, entry.RandomScaleZ);
                WriteSingleBE(output, e + 0x74, entry.MiddleScaleX);
                WriteSingleBE(output, e + 0x78, entry.MiddleScaleY);
                WriteSingleBE(output, e + 0x7C, entry.MiddleScaleZ);
                WriteSingleBE(output, e + 0x80, entry.EndScaleX);
                WriteSingleBE(output, e + 0x84, entry.EndScaleY);
                WriteSingleBE(output, e + 0x88, entry.EndScaleZ);
                WriteSingleBE(output, e + 0x8C, entry.ScaleInterpolationPoint);
                WriteSingleBE(output, e + 0x90, entry.StartColorR);
                WriteSingleBE(output, e + 0x94, entry.StartColorG);
                WriteSingleBE(output, e + 0x98, entry.StartColorB);
                WriteSingleBE(output, e + 0x9C, entry.StartColorA);
                WriteSingleBE(output, e + 0xA0, entry.MiddleColorR);
                WriteSingleBE(output, e + 0xA4, entry.MiddleColorG);
                WriteSingleBE(output, e + 0xA8, entry.MiddleColorB);
                WriteSingleBE(output, e + 0xAC, entry.MiddleColorA);
                WriteSingleBE(output, e + 0xB0, entry.EndColorR);
                WriteSingleBE(output, e + 0xB4, entry.EndColorG);
                WriteSingleBE(output, e + 0xB8, entry.EndColorB);
                WriteSingleBE(output, e + 0xBC, entry.EndColorA);
                WriteSingleBE(output, e + 0xC0, entry.ColorInterpolationPoint);
                WriteUInt32BE(output, e + 0xC4, entry.FieldC4);
                WriteUInt32BE(output, e + 0xC8, entry.FieldC8);
                WriteUInt32BE(output, e + 0xCC, entry.FieldCC);
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
                WriteUInt32BE(output, e + 0x08, entry.Reserved08Word0);
                WriteUInt32BE(output, e + 0x0C, entry.Reserved08Word1);
                WriteUInt32BE(output, e + 0x10, entry.Field10);
                WriteUInt32BE(output, e + 0x14, entry.Field14);
                WriteUInt32BE(output, e + 0x18, (uint)entry.DrawFlags);
                WriteUInt32BE(output, e + 0x1C, (uint)entry.ResourceType);
            }
            return output;
        }

        private static byte[] BuildPositions(List<ParticlePositionEntry> entries, bool usesVersion78Layout)
        {
            bool hasVersion78Fields = usesVersion78Layout;
            int entrySize = hasVersion78Fields ? ParticlePositionSize : ParticlePositionLegacySize;
            byte[] output = new byte[entries.Count * entrySize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticlePositionEntry entry = entries[i];
                int e = i * entrySize;
                WriteInt32BE(output, e + 0x00, entry.CoordChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteUInt32BE(output, e + 0x08, entry.Field08);
                WriteUInt32BE(output, e + 0x0C, entry.Reserved0C);
                WriteSingleBE(output, e + 0x10, entry.DirectionX);
                WriteSingleBE(output, e + 0x14, entry.DirectionY);
                WriteSingleBE(output, e + 0x18, entry.DirectionZ);
                WriteUInt32BE(output, e + 0x1C, entry.NodeEnd);
                WriteUInt32BE(output, e + 0x20, entry.WorldSpace);
                WriteUInt32BE(output, e + 0x24, entry.Reserved24Word0);
                WriteUInt32BE(output, e + 0x28, entry.Reserved24Word1);
                WriteUInt32BE(output, e + 0x2C, entry.Reserved24Word2);
                if (hasVersion78Fields) WriteInt32BE(output, e + 0x30, entry.ClumpChunkIndex);
                if (hasVersion78Fields) WriteUInt32BE(output, e + 0x34, entry.Field34);
            }
            return output;
        }

        private static byte[] BuildForceFields(List<ParticleForceFieldEntry> entries, bool usesVersion78Layout)
        {
            bool hasVersion78Fields = usesVersion78Layout;
            int entrySize = hasVersion78Fields ? ParticleForceFieldSize : ParticleForceFieldLegacySize;
            byte[] output = new byte[entries.Count * entrySize];
            for (int i = 0; i < entries.Count; i++)
            {
                ParticleForceFieldEntry entry = entries[i];
                int e = i * entrySize;
                WriteInt32BE(output, e + 0x00, entry.CoordChunkIndex);
                WriteUInt32BE(output, e + 0x04, entry.ParticleEntryIndex);
                WriteUInt32BE(output, e + 0x08, entry.Field08);
                WriteUInt32BE(output, e + 0x0C, entry.Reserved0C);
                WriteSingleBE(output, e + 0x10, entry.DirectionX);
                WriteSingleBE(output, e + 0x14, entry.DirectionY);
                WriteSingleBE(output, e + 0x18, entry.DirectionZ);
                WriteUInt32BE(output, e + 0x1C, entry.NodeEnd);
                WriteUInt32BE(output, e + 0x20, entry.WorldSpace);
                WriteUInt32BE(output, e + 0x24, entry.Reserved24Word0);
                WriteUInt32BE(output, e + 0x28, entry.Reserved24Word1);
                WriteUInt32BE(output, e + 0x2C, entry.Reserved24Word2);
                output[e + 0x30] = (byte)(sbyte)entry.CalcType;
                output[e + 0x31] = (byte)entry.DirectionSpace;
                output[e + 0x32] = (byte)entry.UseRadius;
                output[e + 0x33] = entry.Reserved33;
                WriteUInt32BE(output, e + 0x34, (uint)entry.ForceScope);
                WriteSingleBE(output, e + 0x38, entry.Radius);
                WriteUInt32BE(output, e + 0x3C, (uint)entry.Falloff);
                WriteSingleBE(output, e + 0x40, entry.Strength);
                WriteSingleBE(output, e + 0x44, entry.StrengthAdjustment);
                WriteUInt32BE(output, e + 0x48, entry.Reserved48Word0);
                WriteUInt32BE(output, e + 0x4C, entry.Reserved48Word1);
                WriteSingleBE(output, e + 0x50, entry.RotationScaleX);
                WriteSingleBE(output, e + 0x54, entry.RotationScaleY);
                WriteSingleBE(output, e + 0x58, entry.RotationScaleZ);
                WriteSingleBE(output, e + 0x5C, entry.Field5C);
                if (hasVersion78Fields) WriteInt32BE(output, e + 0x60, entry.ClumpChunkIndex);
                if (hasVersion78Fields) WriteUInt32BE(output, e + 0x64, entry.Field64);
                if (hasVersion78Fields) WriteUInt32BE(output, e + 0x68, entry.Reserved68Word0);
                if (hasVersion78Fields) WriteUInt32BE(output, e + 0x6C, entry.Reserved68Word1);
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

        private static bool HasCutoutData(ParticleChunkState chunk)
        {
            return chunk.Version >= 0x7B && chunk.Managers.Any(manager => manager.Cutout != 0 &&
                chunk.Resources.Any(resource => resource.ParticleEntryIndex == manager.EntryIndex));
        }

        private static ParticleCutoutData ReadCutoutData(byte[] bytes)
        {
            if (bytes.Length < 12) throw new InvalidDataException("Missing particle cutout size or entry count.");
            ulong size = ((ulong)ReadUInt32BE(bytes, 0) << 32) | ReadUInt32BE(bytes, 4);
            if (size < 4 || size > (ulong)(bytes.Length - 8)) throw new InvalidDataException("Invalid particle cutout size.");
            int end = 8 + (int)size;
            int offset = 8;
            var data = new ParticleCutoutData();
            uint entryCount = ReadCutoutCount(bytes, ref offset, end, 4);
            data.Entries = new ParticleCutoutEntry[entryCount];
            for (int i = 0; i < data.Entries.Length; i++)
            {
                var entry = new ParticleCutoutEntry();
                uint frameCount = ReadCutoutCount(bytes, ref offset, end, 12);
                entry.Frames = new ParticleCutoutFrame[frameCount];
                for (int j = 0; j < entry.Frames.Length; j++)
                {
                    if (end - offset < 12) throw new InvalidDataException("Truncated cutout frame.");
                    var frame = new ParticleCutoutFrame();
                    uint vertexCount = ReadCutoutCount(bytes, ref offset, end - 8, 44);
                    frame.Vertices = new ParticleCutoutVertex[vertexCount];
                    for (int k = 0; k < frame.Vertices.Length; k++)
                    {
                        frame.Vertices[k] = new ParticleCutoutVertex
                        {
                            PositionX = ReadSingleBE(bytes, offset), PositionY = ReadSingleBE(bytes, offset + 4), PositionZ = ReadSingleBE(bytes, offset + 8),
                            UV0U = ReadSingleBE(bytes, offset + 12), UV0V = ReadSingleBE(bytes, offset + 16),
                            UV1U = ReadSingleBE(bytes, offset + 20), UV1V = ReadSingleBE(bytes, offset + 24),
                            R = ReadSingleBE(bytes, offset + 28), G = ReadSingleBE(bytes, offset + 32), B = ReadSingleBE(bytes, offset + 36), A = ReadSingleBE(bytes, offset + 40)
                        };
                        offset += 44;
                    }
                    uint indexCount = ReadCutoutCount(bytes, ref offset, end - 4, 2);
                    frame.Indices = new ushort[indexCount];
                    for (int k = 0; k < frame.Indices.Length; k++, offset += 2)
                        frame.Indices[k] = ReadUInt16BE(bytes, offset);
                    frame.FrameIndex = ReadInt32BE(bytes, offset);
                    offset += 4;
                    entry.Frames[j] = frame;
                }
                data.Entries[i] = entry;
            }
            data.UnparsedTail = bytes.Skip(offset).Take(end - offset).ToArray();
            data.TrailingData = bytes.Skip(end).ToArray();
            return data;
        }

        private static uint ReadCutoutCount(byte[] bytes, ref int offset, int end, int stride)
        {
            if (end - offset < 4) throw new InvalidDataException("Truncated cutout count.");
            uint count = ReadUInt32BE(bytes, offset);
            offset += 4;
            if (count > (end - offset) / stride) throw new InvalidDataException("Cutout count exceeds block size.");
            return count;
        }

        private static byte[] BuildCutoutData(ParticleCutoutData data)
        {
            using (var stream = new MemoryStream())
            {
                Action<uint> word = value => { var bytes = new byte[4]; WriteUInt32BE(bytes, 0, value); stream.Write(bytes, 0, 4); };
                Action<float> single = value => { var bytes = new byte[4]; WriteSingleBE(bytes, 0, value); stream.Write(bytes, 0, 4); };
                word(0); word(0);
                var entries = data.Entries ?? new ParticleCutoutEntry[0];
                word((uint)entries.Length);
                foreach (var entry in entries)
                {
                    if (entry == null) throw new InvalidDataException("Cutout entries cannot be null.");
                    var frames = entry.Frames ?? new ParticleCutoutFrame[0];
                    word((uint)frames.Length);
                    foreach (var frame in frames)
                    {
                        if (frame == null) throw new InvalidDataException("Cutout frames cannot be null.");
                        var vertices = frame.Vertices ?? new ParticleCutoutVertex[0];
                        word((uint)vertices.Length);
                        foreach (var vertex in vertices)
                        {
                            if (vertex == null) throw new InvalidDataException("Cutout vertices cannot be null.");
                            single(vertex.PositionX); single(vertex.PositionY); single(vertex.PositionZ);
                            single(vertex.UV0U); single(vertex.UV0V); single(vertex.UV1U); single(vertex.UV1V);
                            single(vertex.R); single(vertex.G); single(vertex.B); single(vertex.A);
                        }
                        var indices = frame.Indices ?? new ushort[0];
                        word((uint)indices.Length);
                        foreach (ushort index in indices) { stream.WriteByte((byte)(index >> 8)); stream.WriteByte((byte)index); }
                        word(unchecked((uint)frame.FrameIndex));
                    }
                }
                var tail = data.UnparsedTail ?? new byte[0];
                stream.Write(tail, 0, tail.Length);
                long size = stream.Length - 8;
                var trailing = data.TrailingData ?? new byte[0];
                stream.Write(trailing, 0, trailing.Length);
                var output = stream.ToArray();
                WriteUInt32BE(output, 0, (uint)(size >> 32));
                WriteUInt32BE(output, 4, (uint)size);
                return output;
            }
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
        public int Size { get; set; }
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
        public ParticleCutoutData CutoutData { get; set; }
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
        On = 1,
        Clear = 2
    }

    internal sealed class ParticleNodeEvent
    {
        public ParticleNodeAction Action { get; set; }
        public uint TimeMilliseconds { get; set; }
        public uint PreservedFlags { get; set; }
        public float Frame => TimeMilliseconds / 33f;
    }

    internal sealed class ParticleManagerEntry
    {
        [Category("Links"), Description("Offset 0x00 (u32). ")]
        public uint AnimationChunkIndex { get; set; }
        [Category("Links"), Description("Offset 0x04 (u32). Unused for NX attachment (ordinal + 1); used by Connections cutout lookup.")]
        public uint EntryIndex { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x08 (u32). +08: replaced on load with animation reference. +0C: unknown runtime slot.")]
        public uint Reserved08Word0 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x0C (u32). +08: replaced on load with animation reference. +0C: unknown runtime slot.")]
        public uint Reserved08Word1 { get; set; }
        [Category("Parameters"), Description("Offset 0x10 (u8). ")]
        public ParticleEmitMode AllocationMode { get; set; }
        [Category("Parameters"), Description("Offset 0x11 (u8). ")]
        public ParticleSpawnType SpawnType { get; set; }
        [Category("Parameters"), Description("Offset 0x12 (u8). ")]
        public ParticleDirectionType DirectionType { get; set; }
        [Category("Parameters"), Description("Offset 0x13 (u8). ")]
        public ParticleFollowMode ParticleInstanceMode { get; set; }
        [Category("Parameters"), Description("Offset 0x14 (u8). ")]
        public ParticleRotationType RotationType { get; set; }
        [Category("Parameters"), Description("Offset 0x15 (u8). Low 6 bits: time rate; 0 uses game default. High 2 bits: unknown use.")]
        public byte ControlRate { get; set; }
        [Category("Parameters"), Description("Offset 0x16 (u16). Named bits are used; other bits have no confirmed game use.")]
        public ParticleGeneratorFlags GeneratorFlags { get; set; }
        [Category("Parameters"), Description("Offset 0x18 (u32). Bits 0, 8 and 16 are used; other bits have no confirmed game use.")]
        public ParticleForceScope ForceFieldMask { get; set; }
        [Category("Parameters"), Description("Offset 0x1C (s16). -1 = endless; otherwise generator update count.")]
        public short GeneratorEndTime { get; set; }
        [Category("Parameters"), Description("Offset 0x1E (u8). Nonzero enables cutouts in 0x7B+; Connections clears it for older versions.")]
        public byte Cutout { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x1F (u8). Unknown: no confirmed game use.")]
        public byte Reserved1F { get; set; }
        [Category("Parameters"), Description("Offset 0x20 (f32). Burst count, or particles per second in continuous mode.")]
        public float ParticleCountOrRate { get; set; }
        [Category("Parameters"), Description("Offset 0x24 (f32). Unused in Point mode, which uses radius 0.01 (NX).")]
        public float SpawnRadius { get; set; }
        [Category("Parameters"), Description("Offset 0x28 (f32). Fraction of the radius sampled inward from the surface.")]
        public float SpawnRadiusRandomness { get; set; }
        [Category("Parameters"), Description("Offset 0x2C (u16). ")]
        public ushort Lifetime { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x2E (u16). Unknown: no endian conversion or game use established.")]
        public ushort Reserved2E { get; set; }
        [Category("Parameters"), Description("Offset 0x30 (f32). Lifetime *= 1 + random(0, value).")]
        public float LifetimeRandomness { get; set; }
        [Category("Parameters"), Description("Offset 0x34 (f32). ")]
        public float InitialSpeed { get; set; }
        [Category("Parameters"), Description("Offset 0x38 (f32). InitialSpeed *= 1 + random(0, value).")]
        public float InitialSpeedRandomness { get; set; }
        [Category("Parameters"), Description("Offset 0x3C (f32). Cone half-angle in radians; unused outside Cone direction mode (NX).")]
        public float EmissionAngle1 { get; set; }
        [Category("Parameters"), Description("Offset 0x40 (f32). Second cone half-angle; unused outside Cone direction mode (NX).")]
        public float EmissionAngle2 { get; set; }
        [Category("Parameters"), Description("Offset 0x44 (f32). Expands Angle1 by (1 + value); unused outside Cone mode (NX).")]
        public float EmissionAngle1Randomness { get; set; }
        [Category("Parameters"), Description("Offset 0x48 (f32). Expands Angle2 by (1 + value); unused outside Cone mode (NX).")]
        public float EmissionAngle2Randomness { get; set; }
        [Category("Parameters"), Description("Offset 0x4C (f32). Fraction of particle lifetime.")]
        public float FadeIn { get; set; }
        [Category("Parameters"), Description("Offset 0x50 (f32). Fraction of particle lifetime.")]
        public float FadeOut { get; set; }
        [Category("Parameters"), Description("Offset 0x54 (f32). Response to force fields.")]
        public float ForceMultiplier { get; set; }
        [Category("Parameters"), Description("Offset 0x58 (f32). Added to ForceMultiplier as random(0, value).")]
        public float ForceRandomness { get; set; }
        [Category("Parameters"), Description("Offset 0x5C (f32). ")]
        public float StartScaleX { get; set; }
        [Category("Parameters"), Description("Offset 0x60 (f32). ")]
        public float StartScaleY { get; set; }
        [Category("Parameters"), Description("Offset 0x64 (f32). ")]
        public float StartScaleZ { get; set; }
        [Category("Parameters"), Description("Offset 0x68 (f32). ")]
        public float RandomScaleX { get; set; }
        [Category("Parameters"), Description("Offset 0x6C (f32). ")]
        public float RandomScaleY { get; set; }
        [Category("Parameters"), Description("Offset 0x70 (f32). ")]
        public float RandomScaleZ { get; set; }
        [Category("Parameters"), Description("Offset 0x74 (f32). ")]
        public float MiddleScaleX { get; set; }
        [Category("Parameters"), Description("Offset 0x78 (f32). ")]
        public float MiddleScaleY { get; set; }
        [Category("Parameters"), Description("Offset 0x7C (f32). ")]
        public float MiddleScaleZ { get; set; }
        [Category("Parameters"), Description("Offset 0x80 (f32). ")]
        public float EndScaleX { get; set; }
        [Category("Parameters"), Description("Offset 0x84 (f32). ")]
        public float EndScaleY { get; set; }
        [Category("Parameters"), Description("Offset 0x88 (f32). ")]
        public float EndScaleZ { get; set; }
        [Category("Parameters"), Description("Offset 0x8C (f32). Middle key position within lifetime, 0..1.")]
        public float ScaleInterpolationPoint { get; set; }
        [Category("Parameters"), Description("Offset 0x90 (f32). ")]
        public float StartColorR { get; set; }
        [Category("Parameters"), Description("Offset 0x94 (f32). ")]
        public float StartColorG { get; set; }
        [Category("Parameters"), Description("Offset 0x98 (f32). ")]
        public float StartColorB { get; set; }
        [Category("Parameters"), Description("Offset 0x9C (f32). ")]
        public float StartColorA { get; set; }
        [Category("Parameters"), Description("Offset 0xA0 (f32). ")]
        public float MiddleColorR { get; set; }
        [Category("Parameters"), Description("Offset 0xA4 (f32). ")]
        public float MiddleColorG { get; set; }
        [Category("Parameters"), Description("Offset 0xA8 (f32). ")]
        public float MiddleColorB { get; set; }
        [Category("Parameters"), Description("Offset 0xAC (f32). ")]
        public float MiddleColorA { get; set; }
        [Category("Parameters"), Description("Offset 0xB0 (f32). ")]
        public float EndColorR { get; set; }
        [Category("Parameters"), Description("Offset 0xB4 (f32). ")]
        public float EndColorG { get; set; }
        [Category("Parameters"), Description("Offset 0xB8 (f32). ")]
        public float EndColorB { get; set; }
        [Category("Parameters"), Description("Offset 0xBC (f32). ")]
        public float EndColorA { get; set; }
        [Category("Parameters"), Description("Offset 0xC0 (f32). Middle key position within lifetime, 0..1.")]
        public float ColorInterpolationPoint { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0xC4 (u32). Unknown: no confirmed game use; preserve raw value.")]
        public uint FieldC4 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0xC8 (u32). Unknown: no confirmed game use; preserve raw value.")]
        public uint FieldC8 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0xCC (u32). Unknown: no confirmed game use; preserve raw value.")]
        public uint FieldCC { get; set; }
    }

    internal sealed class ParticleResourceEntry
    {
        [Category("Links"), Description("Offset 0x00 (u32). ")]
        public uint EffectChunkIndex { get; set; }
        [Category("Links"), Description("Offset 0x04 (u32). ")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x08 (u32). Unknown use: expanded into runtime/cache slots; not confirmed unused.")]
        public uint Reserved08Word0 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x0C (u32). Unknown use: expanded into runtime/cache slots; not confirmed unused.")]
        public uint Reserved08Word1 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x10 (u32). Replaced on reference resolution: cached chunk pointer for EffectChunkIndex.")]
        public uint Field10 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x14 (u32). Unknown use: expanded runtime slot; not an established effect control.")]
        public uint Field14 { get; set; }
        [Category("Parameters"), Description("Offset 0x18 (u32). Used by sprite types 3/4; unnamed bits may also be used by the renderer.")]
        public ParticleDrawFlags DrawFlags { get; set; }
        [Category("Parameters"), Description("Offset 0x1C (u32). ")]
        public ParticleResourceType ResourceType { get; set; }
    }

    internal sealed class ParticlePositionEntry
    {
        [Browsable(false)]
        public bool HasVersion78Fields { get; set; }
        [Category("Links"), Description("Offset 0x00 (s32). ")]
        public int CoordChunkIndex { get; set; }
        [Category("Links"), Description("Offset 0x04 (u32). ")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x08 (u32). Replaced on load: resolved CoordChunkIndex pointer.")]
        public uint Field08 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x0C (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved0C { get; set; }
        [Category("Parameters"), Description("Offset 0x10 (f32). ")]
        public float DirectionX { get; set; }
        [Category("Parameters"), Description("Offset 0x14 (f32). ")]
        public float DirectionY { get; set; }
        [Category("Parameters"), Description("Offset 0x18 (f32). ")]
        public float DirectionZ { get; set; }
        [Category("Parameters"), Description("Offset 0x1C (u32). ")]
        public uint NodeEnd { get; set; }
        [Category("Parameters"), Description("Offset 0x20 (u32). ")]
        public uint WorldSpace { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x24 (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved24Word0 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x28 (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved24Word1 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x2C (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved24Word2 { get; set; }
        [Category("Links"), Description("Offset 0x30 (s32). ")]
        public int ClumpChunkIndex { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x34 (u32). Replaced on load when ClumpChunkIndex != -1: clump reference pointer.")]
        public uint Field34 { get; set; }
    }

    internal sealed class ParticleForceFieldEntry
    {
        [Browsable(false)]
        public bool HasVersion78Fields { get; set; }
        [Category("Links"), Description("Offset 0x00 (s32). ")]
        public int CoordChunkIndex { get; set; }
        [Category("Links"), Description("Offset 0x04 (u32). ")]
        public uint ParticleEntryIndex { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x08 (u32). Replaced on load: resolved CoordChunkIndex pointer.")]
        public uint Field08 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x0C (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved0C { get; set; }
        [Category("Parameters"), Description("Offset 0x10 (f32). ")]
        public float DirectionX { get; set; }
        [Category("Parameters"), Description("Offset 0x14 (f32). ")]
        public float DirectionY { get; set; }
        [Category("Parameters"), Description("Offset 0x18 (f32). ")]
        public float DirectionZ { get; set; }
        [Category("Parameters"), Description("Offset 0x1C (u32). Unused in traced NX/S4 force calculation; inherited position field.")]
        public uint NodeEnd { get; set; }
        [Category("Parameters"), Description("Offset 0x20 (u32). Unused in traced NX/S4 force calculation; DirectionSpace is used instead.")]
        public uint WorldSpace { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x24 (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved24Word0 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x28 (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved24Word1 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x2C (u32). Unknown: copied; no confirmed game use.")]
        public uint Reserved24Word2 { get; set; }
        [Category("Parameters"), Description("Offset 0x30 (s8). ")]
        public ParticleForceType CalcType { get; set; }
        [Category("Parameters"), Description("Offset 0x31 (u8). ")]
        public ParticleSpace DirectionSpace { get; set; }
        [Category("Parameters"), Description("Offset 0x32 (u8). Enables the radius limit and falloff.")]
        public ParticleRadiusMode UseRadius { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x33 (u8). Unknown: no use found in traced NX/S4 force methods.")]
        public byte Reserved33 { get; set; }
        [Category("Parameters"), Description("Offset 0x34 (u32). ")]
        public ParticleForceScope ForceScope { get; set; }
        [Category("Parameters"), Description("Offset 0x38 (f32). Unused when UseRadius is Unlimited (NX/S4).")]
        public float Radius { get; set; }
        [Category("Parameters"), Description("Offset 0x3C (u32). Unused when UseRadius is Unlimited or CalcType is Speed (NX/S4).")]
        public ParticleForceFalloff Falloff { get; set; }
        [Category("Parameters"), Description("Offset 0x40 (f32). ")]
        public float Strength { get; set; }
        [Category("Parameters"), Description("Offset 0x44 (f32). Strength *= 1 + value; no random sampling.")]
        public float StrengthAdjustment { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x48 (u32). Unknown: copied; no use found in traced NX/S4 force methods.")]
        public uint Reserved48Word0 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x4C (u32). Unknown: copied; no use found in traced NX/S4 force methods.")]
        public uint Reserved48Word1 { get; set; }
        [Category("Parameters"), Description("Offset 0x50 (f32). XYZ increments for Rotate/Scale; unused by other force modes (NX/S4).")]
        public float RotationScaleX { get; set; }
        [Category("Parameters"), Description("Offset 0x54 (f32). XYZ increments for Rotate/Scale; unused by other force modes (NX/S4).")]
        public float RotationScaleY { get; set; }
        [Category("Parameters"), Description("Offset 0x58 (f32). XYZ increments for Rotate/Scale; unused by other force modes (NX/S4).")]
        public float RotationScaleZ { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x5C (f32). Unused by all seven traced NX/S4 force modes; copied fourth component.")]
        public float Field5C { get; set; }
        [Category("Links"), Description("Offset 0x60 (s32). ")]
        public int ClumpChunkIndex { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x64 (u32). Replaced on load when ClumpChunkIndex != -1: clump reference pointer.")]
        public uint Field64 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x68 (u32). Unused by NX/S4 loader: record tail is not copied into runtime data.")]
        public uint Reserved68Word0 { get; set; }
        [Category("Reserved / Runtime"), Description("Offset 0x6C (u32). Unused by NX/S4 loader: record tail is not copied into runtime data.")]
        public uint Reserved68Word1 { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class ParticleCutoutData
    {
        public ParticleCutoutEntry[] Entries { get; set; } = new ParticleCutoutEntry[0];
        [Description("Unknown extension bytes inside the size-prefixed cutout block. Preserved when saving.")]
        public byte[] UnparsedTail { get; set; } = new byte[0];
        [Browsable(false)]
        public byte[] TrailingData { get; set; } = new byte[0];
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class ParticleCutoutEntry
    {
        public ParticleCutoutFrame[] Frames { get; set; } = new ParticleCutoutFrame[0];
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class ParticleCutoutFrame
    {
        public ParticleCutoutVertex[] Vertices { get; set; } = new ParticleCutoutVertex[0];
        public ushort[] Indices { get; set; } = new ushort[0];
        [Description("-1 uses this frame's geometry; otherwise references another frame.")]
        public int FrameIndex { get; set; } = -1;
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class ParticleCutoutVertex
    {
        [Category("Position")] public float PositionX { get; set; }
        [Category("Position")] public float PositionY { get; set; }
        [Category("Position")] public float PositionZ { get; set; }
        [Category("UV0")] public float UV0U { get; set; }
        [Category("UV0")] public float UV0V { get; set; }
        [Category("UV1"), Description("If the first vertex's UV1U is float.MaxValue, the second UV stream is omitted by the game.")]
        public float UV1U { get; set; }
        [Category("UV1")] public float UV1V { get; set; }
        [Category("Color")] public float R { get; set; }
        [Category("Color")] public float G { get; set; }
        [Category("Color")] public float B { get; set; }
        [Category("Color")] public float A { get; set; }
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
