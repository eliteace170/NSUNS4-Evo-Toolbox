using System;
using System.Collections.Generic;
using System.Text;

namespace NSUNS4_Character_Manager.Tools
{
    internal enum CpuParamChunkKind
    {
        Script,
        Strength,
        Action,
        Player
    }

    internal enum CpuScriptType : ushort
    {
        CtrlIf = 0,
        NextProcessIf = 1,
        Skip2 = 2,
        CtrlSwitch = 3,
        NextProcessSwitch = 4,
        Skip5 = 5,
        CtrlJump = 6,
        Skip7 = 7,
        End = 8,
        CtrlCommand = 9,
        Unknown10 = 10
    }

    internal enum CpuScriptCommand : ushort
    {
        SetProb = 0,
        AddProb = 1,
        JudgeGauge = 2,
        JudgeSituation = 3,
        JudgeDistance = 4,
        SetParam = 5,
        JudgeAction = 6,
        None = ushort.MaxValue
    }

    internal enum CpuScriptTarget
    {
        Self = 0,
        Enemy = 1
    }

    internal enum CpuGaugeType
    {
        Life = 0,
        Chakra = 1,
        GuardPower = 2,
        TeamPower = 3
    }

    internal enum CpuSituationId
    {
        Unused00 = 0,
        Unused01 = 1,
        Unused02 = 2,
        IsSupportSkillL = 3,
        IsSupportSkillR = 4,
        IsAttackTypeSupportL = 5,
        IsAttackTypeSupportR = 6,
        IsAwake = 7,
        IsHugeAwake08 = 8,
        IsHugeAwake09 = 9,
        IsEnableConditionLifeDecrease = 10,
        IsEnableConditionSleep = 11,
        IsEnableConditionSeal = 12,
        IsEnableConditionAutoDodge = 13,
        Unused14 = 14,
        IsSuperArmor = 15,
        Unused16 = 16,
        IsEnableChakraInfinity = 17,
        IsInvincibleAbove30 = 18,
        IsSpecialSupportL = 19,
        IsSpecialSupportR = 20
    }

    internal enum CpuActionId
    {
        IsActionFree = 0,
        IsActionNinjaMove = 1,
        IsActionGuard = 2,
        IsActionDamage = 3,
        IsActionDown = 4,
        IsActionCharge = 5,
        IsActionAwake = 6,
        IsActionSkill = 7,
        IsActionDefenseless = 8,
        IsAwakeNow = 9
    }

    internal enum CpuCompareOperation
    {
        Equal = 0,
        NotEqual = 1,
        Greater = 2,
        GreaterEqual = 3,
        Less = 4,
        LessEqual = 5
    }

    internal enum CpuBitTestMode
    {
        MustBeSet = 0,
        MustBeClear = 1
    }

    internal enum CpuPlayerType
    {
        Normal = 0,
        UnobservedType01 = 1,
        ProjectileType = 2,
        UnobservedType03 = 3,
        UnobservedType04 = 4,
        AwakenMoveset = 5,
        ProjectileAwakenType = 6,
        PuppetType = 7,
        UnobservedType08 = 8,
        UnobservedType09 = 9,
        UnobservedType10 = 10
    }

    internal abstract class CpuParamChunkState
    {
        public CpuParamChunkKind Kind;
        public string OriginalChunkName = string.Empty;
        public string ChunkName = string.Empty;
        public string ChunkPath = string.Empty;
        public int ContainerVersion;
        public int ContainerVersionAttribute;
    }

    internal sealed class CpuScriptChunkState : CpuParamChunkState
    {
        public readonly List<CpuScriptGroup> Groups = new List<CpuScriptGroup>();
    }

    internal sealed class CpuStrengthChunkState : CpuParamChunkState
    {
        public readonly List<CpuStrengthGroup> Groups = new List<CpuStrengthGroup>();
    }

    internal sealed class CpuActionChunkState : CpuParamChunkState
    {
        public uint FormatVersion = 1000;
        public uint PointerSize = 8;
        public uint Reserved;
        public readonly List<CpuActionEntry> Entries = new List<CpuActionEntry>();
    }

    internal sealed class CpuPlayerChunkState : CpuParamChunkState
    {
        public uint FormatVersion = 1000;
        public uint PointerSize = 8;
        public uint Reserved;
        public readonly List<CpuPlayerEntry> Entries = new List<CpuPlayerEntry>();
    }

    internal sealed class CpuScriptGroup
    {
        public readonly List<CpuScriptInstruction> Instructions = new List<CpuScriptInstruction>();

        public CpuScriptGroup Clone()
        {
            CpuScriptGroup copy = new CpuScriptGroup();
            foreach (CpuScriptInstruction instruction in Instructions)
                copy.Instructions.Add(instruction.Clone());
            return copy;
        }
    }

    internal sealed class CpuStrengthGroup
    {
        public readonly List<CpuStrengthEntry> Entries = new List<CpuStrengthEntry>();

        public CpuStrengthGroup Clone()
        {
            CpuStrengthGroup copy = new CpuStrengthGroup();
            foreach (CpuStrengthEntry entry in Entries)
                copy.Entries.Add(entry.Clone());
            return copy;
        }
    }

    internal sealed class CpuScriptInstruction
    {
        public ushort Type;
        public ushort CommandNumber;
        public int[] Arguments = CreateFilledIntArray(8, -1);

        public CpuScriptInstruction Clone()
        {
            return new CpuScriptInstruction
            {
                Type = Type,
                CommandNumber = CommandNumber,
                Arguments = (int[])Arguments.Clone()
            };
        }

        private static int[] CreateFilledIntArray(int count, int value)
        {
            int[] result = new int[count];
            for (int i = 0; i < result.Length; i++)
                result[i] = value;
            return result;
        }
    }

    internal sealed class CpuStrengthEntry
    {
        public ushort Type = (ushort)CpuScriptType.CtrlCommand;
        public ushort CommandNumber = (ushort)CpuScriptCommand.SetParam;
        public int ParamId;
        public int Value;
        public int[] UnusedArguments = CreateFilledIntArray(6, -1);

        public CpuStrengthEntry Clone()
        {
            return new CpuStrengthEntry
            {
                Type = Type,
                CommandNumber = CommandNumber,
                ParamId = ParamId,
                Value = Value,
                UnusedArguments = (int[])UnusedArguments.Clone()
            };
        }

        private static int[] CreateFilledIntArray(int count, int value)
        {
            int[] result = new int[count];
            for (int i = 0; i < result.Length; i++)
                result[i] = value;
            return result;
        }
    }

    internal sealed class CpuActionEntry
    {
        public string Name = string.Empty;
        public int RuntimeValue08;
        public int RuntimeValue0C;
        public int RuntimeValue10;
        public int RuntimeValue14;
        public int RuntimeValue18Base;
        public int RuntimeValue18Addend;
        public int[] ActionMaskBitIndices = CreateFilledIntArray(4, 9999);

        public CpuActionEntry Clone()
        {
            return new CpuActionEntry
            {
                Name = Name,
                RuntimeValue08 = RuntimeValue08,
                RuntimeValue0C = RuntimeValue0C,
                RuntimeValue10 = RuntimeValue10,
                RuntimeValue14 = RuntimeValue14,
                RuntimeValue18Base = RuntimeValue18Base,
                RuntimeValue18Addend = RuntimeValue18Addend,
                ActionMaskBitIndices = (int[])ActionMaskBitIndices.Clone()
            };
        }

        private static int[] CreateFilledIntArray(int count, int value)
        {
            int[] result = new int[count];
            for (int i = 0; i < result.Length; i++)
                result[i] = value;
            return result;
        }
    }

    internal sealed class CpuPlayerEntry
    {
        public int Characode;
        public int Type;
        public string[] ActionSlots = CreateStringArray(32);
        public uint AwakeningScriptType;
        public uint AwakeningScriptTypeFlag;
        public uint InstantAwakeningScriptType;
        public uint InstantAwakeningScriptTypeFlag;

        public CpuPlayerEntry Clone()
        {
            return new CpuPlayerEntry
            {
                Characode = Characode,
                Type = Type,
                ActionSlots = (string[])ActionSlots.Clone(),
                AwakeningScriptType = AwakeningScriptType,
                AwakeningScriptTypeFlag = AwakeningScriptTypeFlag,
                InstantAwakeningScriptType = InstantAwakeningScriptType,
                InstantAwakeningScriptTypeFlag = InstantAwakeningScriptTypeFlag
            };
        }

        private static string[] CreateStringArray(int count)
        {
            string[] result = new string[count];
            for (int i = 0; i < result.Length; i++)
                result[i] = string.Empty;
            return result;
        }
    }

    internal static class CpuParamCodec
    {
        internal const string BinaryChunkType = "nuccChunkBinary";
        internal const int ScriptEntrySize = 36;
        internal const int StrengthEntrySize = 36;
        internal const int ActionEntrySize = 48;
        internal const int PlayerEntrySize = 264;

        internal static readonly int[] PlayerStringSlotIndices =
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17,
            19, 20, 21, 22, 23,
            25, 26, 27, 28, 29, 30, 31
        };

        internal static string GetDefaultChunkName(CpuParamChunkKind kind)
        {
            switch (kind)
            {
                case CpuParamChunkKind.Script: return "cpu_script";
                case CpuParamChunkKind.Strength: return "cpu_strength";
                case CpuParamChunkKind.Action: return "cpuActionParam";
                case CpuParamChunkKind.Player: return "cpuPlayerParam";
                default: throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        internal static string GetDefaultChunkPath(CpuParamChunkKind kind)
        {
            switch (kind)
            {
                case CpuParamChunkKind.Script: return "Z:/param/cpu/cpu_script.bin";
                case CpuParamChunkKind.Strength: return "Z:/param/cpu/cpu_strength.bin";
                case CpuParamChunkKind.Action: return "Z:/param/cpu/ActionParam/bin_le/x64/cpuActionParam.bin";
                case CpuParamChunkKind.Player: return "Z:/param/cpu/PlayerAction/bin_le/x64/cpuPlayerParam.bin";
                default: throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        internal static bool TryGetChunkKind(string chunkName, out CpuParamChunkKind kind)
        {
            foreach (CpuParamChunkKind candidate in Enum.GetValues(typeof(CpuParamChunkKind)))
            {
                if (string.Equals(chunkName, GetDefaultChunkName(candidate), StringComparison.OrdinalIgnoreCase))
                {
                    kind = candidate;
                    return true;
                }
            }

            kind = default(CpuParamChunkKind);
            return false;
        }

        internal static bool TryGetChunkKind(string chunkName, string chunkPath, string fileName, out CpuParamChunkKind kind)
        {
            if (TryGetChunkKind(chunkName, out kind))
                return true;

            string combined = ((chunkName ?? string.Empty) + "|" + (chunkPath ?? string.Empty) + "|" + (fileName ?? string.Empty)).ToLowerInvariant();
            if (combined.Contains("cpuactionparam"))
            {
                kind = CpuParamChunkKind.Action;
                return true;
            }
            if (combined.Contains("cpuplayerparam"))
            {
                kind = CpuParamChunkKind.Player;
                return true;
            }
            if (combined.Contains("cpu_strength") || combined.Contains("cpustrength"))
            {
                kind = CpuParamChunkKind.Strength;
                return true;
            }
            if (combined.Contains("cpu_script") || combined.Contains("cpuscript"))
            {
                kind = CpuParamChunkKind.Script;
                return true;
            }

            kind = default(CpuParamChunkKind);
            return false;
        }

        internal static CpuParamChunkState Parse(CpuParamChunkKind kind, byte[] bytes)
        {
            switch (kind)
            {
                case CpuParamChunkKind.Script: return ParseScript(bytes);
                case CpuParamChunkKind.Strength: return ParseStrength(bytes);
                case CpuParamChunkKind.Action: return ParseAction(bytes);
                case CpuParamChunkKind.Player: return ParsePlayer(bytes);
                default: throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        internal static byte[] Build(CpuParamChunkState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            switch (state.Kind)
            {
                case CpuParamChunkKind.Script: return BuildScript((CpuScriptChunkState)state);
                case CpuParamChunkKind.Strength: return BuildStrength((CpuStrengthChunkState)state);
                case CpuParamChunkKind.Action: return BuildAction((CpuActionChunkState)state);
                case CpuParamChunkKind.Player: return BuildPlayer((CpuPlayerChunkState)state);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static CpuScriptChunkState ParseScript(byte[] bytes)
        {
            ValidatePayloadSize(bytes, 8, "cpu_script");
            uint groupCount = ReadUInt32LE(bytes, 4);
            int recordStart = CheckedTableStart(groupCount, bytes.Length, "cpu_script group table");
            uint[] counts = ReadCounts(bytes, groupCount, 8);
            ValidateFixedRecordTable(bytes.Length, recordStart, counts, ScriptEntrySize, "cpu_script");

            CpuScriptChunkState state = new CpuScriptChunkState { Kind = CpuParamChunkKind.Script };
            int offset = recordStart;
            foreach (uint count in counts)
            {
                CpuScriptGroup group = new CpuScriptGroup();
                for (uint i = 0; i < count; i++)
                {
                    CpuScriptInstruction instruction = new CpuScriptInstruction
                    {
                        Type = ReadUInt16LE(bytes, offset),
                        CommandNumber = ReadUInt16LE(bytes, offset + 2),
                        Arguments = new int[8]
                    };
                    for (int argument = 0; argument < 8; argument++)
                        instruction.Arguments[argument] = ReadInt32LE(bytes, offset + 4 + (argument * 4));
                    group.Instructions.Add(instruction);
                    offset += ScriptEntrySize;
                }
                state.Groups.Add(group);
            }
            return state;
        }

        private static CpuStrengthChunkState ParseStrength(byte[] bytes)
        {
            ValidatePayloadSize(bytes, 8, "cpu_strength");
            uint groupCount = ReadUInt32LE(bytes, 4);
            int recordStart = CheckedTableStart(groupCount, bytes.Length, "cpu_strength group table");
            uint[] counts = ReadCounts(bytes, groupCount, 8);
            ValidateFixedRecordTable(bytes.Length, recordStart, counts, StrengthEntrySize, "cpu_strength");

            CpuStrengthChunkState state = new CpuStrengthChunkState { Kind = CpuParamChunkKind.Strength };
            int offset = recordStart;
            foreach (uint count in counts)
            {
                CpuStrengthGroup group = new CpuStrengthGroup();
                for (uint i = 0; i < count; i++)
                {
                    CpuStrengthEntry entry = new CpuStrengthEntry
                    {
                        Type = ReadUInt16LE(bytes, offset),
                        CommandNumber = ReadUInt16LE(bytes, offset + 2),
                        ParamId = ReadInt32LE(bytes, offset + 4),
                        Value = ReadInt32LE(bytes, offset + 8),
                        UnusedArguments = new int[6]
                    };
                    for (int argument = 0; argument < 6; argument++)
                        entry.UnusedArguments[argument] = ReadInt32LE(bytes, offset + 12 + (argument * 4));
                    group.Entries.Add(entry);
                    offset += StrengthEntrySize;
                }
                state.Groups.Add(group);
            }
            return state;
        }

        private static CpuActionChunkState ParseAction(byte[] bytes)
        {
            ValidatePayloadSize(bytes, 20, "cpuActionParam");
            uint count = ReadUInt32LE(bytes, 8);
            int stringTableStart = CheckedEntryTableEnd(20, count, ActionEntrySize, bytes.Length, "cpuActionParam");
            CpuActionChunkState state = new CpuActionChunkState
            {
                Kind = CpuParamChunkKind.Action,
                FormatVersion = ReadUInt32LE(bytes, 4),
                PointerSize = ReadUInt32LE(bytes, 12),
                Reserved = ReadUInt32LE(bytes, 16)
            };
            ValidateStructuredHeader(state.FormatVersion, state.PointerSize, state.Reserved, "cpuActionParam");

            int nextExpectedString = stringTableStart;
            for (uint i = 0; i < count; i++)
            {
                int entryOffset = checked(20 + ((int)i * ActionEntrySize));
                CpuActionEntry entry = new CpuActionEntry();
                entry.Name = ReadSequentialRelativeString(bytes, entryOffset, stringTableStart, ref nextExpectedString, false, "cpuActionParam name");
                entry.RuntimeValue08 = ReadInt32LE(bytes, entryOffset + 8);
                entry.RuntimeValue0C = ReadInt32LE(bytes, entryOffset + 12);
                entry.RuntimeValue10 = ReadInt32LE(bytes, entryOffset + 16);
                entry.RuntimeValue14 = ReadInt32LE(bytes, entryOffset + 20);
                entry.RuntimeValue18Base = ReadInt32LE(bytes, entryOffset + 24);
                entry.RuntimeValue18Addend = ReadInt32LE(bytes, entryOffset + 28);
                for (int bit = 0; bit < 4; bit++)
                    entry.ActionMaskBitIndices[bit] = ReadInt32LE(bytes, entryOffset + 32 + (bit * 4));
                state.Entries.Add(entry);
            }

            if (nextExpectedString != bytes.Length)
                throw new InvalidOperationException("cpuActionParam contains unreferenced or out-of-order string data.");
            return state;
        }

        private static CpuPlayerChunkState ParsePlayer(byte[] bytes)
        {
            ValidatePayloadSize(bytes, 20, "cpuPlayerParam");
            uint count = ReadUInt32LE(bytes, 8);
            int stringTableStart = CheckedEntryTableEnd(20, count, PlayerEntrySize, bytes.Length, "cpuPlayerParam");
            CpuPlayerChunkState state = new CpuPlayerChunkState
            {
                Kind = CpuParamChunkKind.Player,
                FormatVersion = ReadUInt32LE(bytes, 4),
                PointerSize = ReadUInt32LE(bytes, 12),
                Reserved = ReadUInt32LE(bytes, 16)
            };
            ValidateStructuredHeader(state.FormatVersion, state.PointerSize, state.Reserved, "cpuPlayerParam");

            int nextExpectedString = stringTableStart;
            for (uint i = 0; i < count; i++)
            {
                int entryOffset = checked(20 + ((int)i * PlayerEntrySize));
                CpuPlayerEntry entry = new CpuPlayerEntry
                {
                    Characode = ReadInt32LE(bytes, entryOffset),
                    Type = ReadInt32LE(bytes, entryOffset + 4)
                };

                foreach (int slot in PlayerStringSlotIndices)
                {
                    int fieldOffset = GetPlayerStringFieldOffset(entryOffset, slot);
                    entry.ActionSlots[slot] = ReadSequentialRelativeString(bytes, fieldOffset, stringTableStart, ref nextExpectedString, true, "cpuPlayerParam action slot");
                }

                entry.AwakeningScriptType = ReadUInt32LE(bytes, entryOffset + 152);
                entry.AwakeningScriptTypeFlag = ReadUInt32LE(bytes, entryOffset + 156);
                entry.InstantAwakeningScriptType = ReadUInt32LE(bytes, entryOffset + 200);
                entry.InstantAwakeningScriptTypeFlag = ReadUInt32LE(bytes, entryOffset + 204);
                state.Entries.Add(entry);
            }

            if (nextExpectedString != bytes.Length)
                throw new InvalidOperationException("cpuPlayerParam contains unreferenced or out-of-order string data.");
            return state;
        }

        private static byte[] BuildScript(CpuScriptChunkState state)
        {
            if (state.Groups.Count == 0)
                throw new InvalidOperationException("cpu_script must contain at least one group.");

            int totalEntries = 0;
            foreach (CpuScriptGroup group in state.Groups)
                totalEntries = checked(totalEntries + group.Instructions.Count);
            int recordStart = checked(8 + (state.Groups.Count * 4));
            byte[] output = new byte[checked(recordStart + (totalEntries * ScriptEntrySize))];
            WriteUInt32BE(output, 0, checked((uint)(output.Length - 4)));
            WriteUInt32LE(output, 4, checked((uint)state.Groups.Count));

            int offset = recordStart;
            for (int groupIndex = 0; groupIndex < state.Groups.Count; groupIndex++)
            {
                CpuScriptGroup group = state.Groups[groupIndex];
                WriteUInt32LE(output, 8 + (groupIndex * 4), checked((uint)group.Instructions.Count));
                foreach (CpuScriptInstruction instruction in group.Instructions)
                {
                    ValidateArrayLength(instruction.Arguments, 8, "cpu_script arguments");
                    WriteUInt16LE(output, offset, instruction.Type);
                    WriteUInt16LE(output, offset + 2, instruction.CommandNumber);
                    for (int argument = 0; argument < 8; argument++)
                        WriteInt32LE(output, offset + 4 + (argument * 4), instruction.Arguments[argument]);
                    offset += ScriptEntrySize;
                }
            }
            return output;
        }

        private static byte[] BuildStrength(CpuStrengthChunkState state)
        {
            if (state.Groups.Count == 0)
                throw new InvalidOperationException("cpu_strength must contain at least one group.");

            int totalEntries = 0;
            foreach (CpuStrengthGroup group in state.Groups)
                totalEntries = checked(totalEntries + group.Entries.Count);
            int recordStart = checked(8 + (state.Groups.Count * 4));
            byte[] output = new byte[checked(recordStart + (totalEntries * StrengthEntrySize))];
            WriteUInt32BE(output, 0, checked((uint)(output.Length - 4)));
            WriteUInt32LE(output, 4, checked((uint)state.Groups.Count));

            int offset = recordStart;
            for (int groupIndex = 0; groupIndex < state.Groups.Count; groupIndex++)
            {
                CpuStrengthGroup group = state.Groups[groupIndex];
                WriteUInt32LE(output, 8 + (groupIndex * 4), checked((uint)group.Entries.Count));
                foreach (CpuStrengthEntry entry in group.Entries)
                {
                    ValidateArrayLength(entry.UnusedArguments, 6, "cpu_strength unused arguments");
                    WriteUInt16LE(output, offset, entry.Type);
                    WriteUInt16LE(output, offset + 2, entry.CommandNumber);
                    WriteInt32LE(output, offset + 4, entry.ParamId);
                    WriteInt32LE(output, offset + 8, entry.Value);
                    for (int argument = 0; argument < 6; argument++)
                        WriteInt32LE(output, offset + 12 + (argument * 4), entry.UnusedArguments[argument]);
                    offset += StrengthEntrySize;
                }
            }
            return output;
        }

        private static byte[] BuildAction(CpuActionChunkState state)
        {
            ValidateStructuredHeader(state.FormatVersion, state.PointerSize, state.Reserved, "cpuActionParam");
            List<byte[]> strings = new List<byte[]>();
            int stringBytesLength = 0;
            foreach (CpuActionEntry entry in state.Entries)
            {
                ValidateArrayLength(entry.ActionMaskBitIndices, 4, "cpuActionParam action mask bit indices");
                byte[] stringBytes = BuildNullTerminatedString(entry.Name, false, "Action name");
                strings.Add(stringBytes);
                stringBytesLength = checked(stringBytesLength + stringBytes.Length);
            }

            int stringStart = checked(20 + (state.Entries.Count * ActionEntrySize));
            byte[] output = new byte[checked(stringStart + stringBytesLength)];
            WriteStructuredHeader(output, state.FormatVersion, state.Entries.Count, state.PointerSize, state.Reserved);
            int nextString = stringStart;
            for (int i = 0; i < state.Entries.Count; i++)
            {
                CpuActionEntry entry = state.Entries[i];
                int entryOffset = 20 + (i * ActionEntrySize);
                WriteUInt64LE(output, entryOffset, checked((ulong)(nextString - entryOffset)));
                WriteInt32LE(output, entryOffset + 8, entry.RuntimeValue08);
                WriteInt32LE(output, entryOffset + 12, entry.RuntimeValue0C);
                WriteInt32LE(output, entryOffset + 16, entry.RuntimeValue10);
                WriteInt32LE(output, entryOffset + 20, entry.RuntimeValue14);
                WriteInt32LE(output, entryOffset + 24, entry.RuntimeValue18Base);
                WriteInt32LE(output, entryOffset + 28, entry.RuntimeValue18Addend);
                for (int bit = 0; bit < 4; bit++)
                    WriteInt32LE(output, entryOffset + 32 + (bit * 4), entry.ActionMaskBitIndices[bit]);
                Array.Copy(strings[i], 0, output, nextString, strings[i].Length);
                nextString += strings[i].Length;
            }
            return output;
        }

        private static byte[] BuildPlayer(CpuPlayerChunkState state)
        {
            ValidateStructuredHeader(state.FormatVersion, state.PointerSize, state.Reserved, "cpuPlayerParam");
            List<byte[]> strings = new List<byte[]>();
            int stringBytesLength = 0;
            foreach (CpuPlayerEntry entry in state.Entries)
            {
                ValidateArrayLength(entry.ActionSlots, 32, "cpuPlayerParam action slots");
                foreach (int slot in PlayerStringSlotIndices)
                {
                    string value = entry.ActionSlots[slot] ?? string.Empty;
                    if (value.Length == 0)
                    {
                        strings.Add(null);
                        continue;
                    }

                    byte[] stringBytes = BuildNullTerminatedString(value, true, "Player action name");
                    strings.Add(stringBytes);
                    stringBytesLength = checked(stringBytesLength + stringBytes.Length);
                }
            }

            int stringStart = checked(20 + (state.Entries.Count * PlayerEntrySize));
            byte[] output = new byte[checked(stringStart + stringBytesLength)];
            WriteStructuredHeader(output, state.FormatVersion, state.Entries.Count, state.PointerSize, state.Reserved);
            int nextString = stringStart;
            int stringIndex = 0;
            for (int i = 0; i < state.Entries.Count; i++)
            {
                CpuPlayerEntry entry = state.Entries[i];
                int entryOffset = 20 + (i * PlayerEntrySize);
                WriteInt32LE(output, entryOffset, entry.Characode);
                WriteInt32LE(output, entryOffset + 4, entry.Type);
                foreach (int slot in PlayerStringSlotIndices)
                {
                    int fieldOffset = GetPlayerStringFieldOffset(entryOffset, slot);
                    byte[] stringBytes = strings[stringIndex++];
                    if (stringBytes == null)
                    {
                        WriteUInt64LE(output, fieldOffset, 0);
                        continue;
                    }

                    WriteUInt64LE(output, fieldOffset, checked((ulong)(nextString - fieldOffset)));
                    Array.Copy(stringBytes, 0, output, nextString, stringBytes.Length);
                    nextString += stringBytes.Length;
                }

                WriteUInt32LE(output, entryOffset + 152, entry.AwakeningScriptType);
                WriteUInt32LE(output, entryOffset + 156, entry.AwakeningScriptTypeFlag);
                WriteUInt32LE(output, entryOffset + 200, entry.InstantAwakeningScriptType);
                WriteUInt32LE(output, entryOffset + 204, entry.InstantAwakeningScriptTypeFlag);
            }
            return output;
        }

        private static int GetPlayerStringFieldOffset(int entryOffset, int slot)
        {
            if (slot >= 0 && slot <= 17)
                return entryOffset + 8 + (slot * 8);
            if (slot >= 19 && slot <= 23)
                return entryOffset + 160 + ((slot - 19) * 8);
            if (slot >= 25 && slot <= 31)
                return entryOffset + 208 + ((slot - 25) * 8);
            throw new ArgumentOutOfRangeException(nameof(slot), "Slot 18 and slot 24 contain script group fields, not strings.");
        }

        private static void ValidatePayloadSize(byte[] bytes, int minimumLength, string formatName)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length < minimumLength)
                throw new InvalidOperationException(formatName + " is shorter than its header.");
            uint payloadSize = ReadUInt32BE(bytes, 0);
            if (payloadSize != bytes.Length - 4)
                throw new InvalidOperationException(formatName + " has an invalid big-endian payload size.");
        }

        private static int CheckedTableStart(uint count, int dataLength, string description)
        {
            long value = 8L + (count * 4L);
            if (count == 0 || value > dataLength || value > int.MaxValue)
                throw new InvalidOperationException(description + " is invalid.");
            return (int)value;
        }

        private static int CheckedEntryTableEnd(int headerSize, uint count, int entrySize, int dataLength, string description)
        {
            long value = headerSize + (count * (long)entrySize);
            if (value > dataLength || value > int.MaxValue)
                throw new InvalidOperationException(description + " entry table exceeds the payload.");
            return (int)value;
        }

        private static uint[] ReadCounts(byte[] bytes, uint count, int offset)
        {
            if (count > int.MaxValue)
                throw new InvalidOperationException("Group count is too large.");
            uint[] values = new uint[(int)count];
            for (int i = 0; i < values.Length; i++)
                values[i] = ReadUInt32LE(bytes, offset + (i * 4));
            return values;
        }

        private static void ValidateFixedRecordTable(int dataLength, int recordStart, uint[] counts, int entrySize, string formatName)
        {
            long totalCount = 0;
            foreach (uint count in counts)
                totalCount += count;
            long expectedLength = recordStart + (totalCount * entrySize);
            if (expectedLength != dataLength)
                throw new InvalidOperationException(formatName + " group counts do not match its payload length.");
        }

        private static void ValidateStructuredHeader(uint version, uint pointerSize, uint reserved, string formatName)
        {
            if (version != 1000)
                throw new InvalidOperationException(formatName + " format version must be 1000.");
            if (pointerSize != 8)
                throw new InvalidOperationException(formatName + " pointer size must be 8.");
            if (reserved != 0)
                throw new InvalidOperationException(formatName + " reserved header field must be zero.");
        }

        private static string ReadSequentialRelativeString(byte[] bytes, int fieldOffset, int stringTableStart, ref int nextExpectedString, bool allowNull, string description)
        {
            ulong relativeOffset = ReadUInt64LE(bytes, fieldOffset);
            if (relativeOffset == 0)
            {
                if (!allowNull)
                    throw new InvalidOperationException(description + " cannot be null.");
                return string.Empty;
            }

            ulong target = checked((ulong)fieldOffset + relativeOffset);
            if (target > int.MaxValue || target < (ulong)stringTableStart || target >= (ulong)bytes.Length)
                throw new InvalidOperationException(description + " has an invalid relative offset.");
            if ((int)target != nextExpectedString)
                throw new InvalidOperationException(description + " strings are not stored in template order.");

            int end = nextExpectedString;
            while (end < bytes.Length && bytes[end] != 0)
                end++;
            if (end >= bytes.Length)
                throw new InvalidOperationException(description + " is not null terminated.");
            string value = Encoding.UTF8.GetString(bytes, nextExpectedString, end - nextExpectedString);
            nextExpectedString = end + 1;
            return value;
        }

        private static byte[] BuildNullTerminatedString(string value, bool allowEmpty, string description)
        {
            value = value ?? string.Empty;
            if (!allowEmpty && value.Length == 0)
                throw new InvalidOperationException(description + " cannot be empty.");
            if (value.IndexOf('\0') >= 0)
                throw new InvalidOperationException(description + " cannot contain a null character.");
            byte[] encoded = Encoding.UTF8.GetBytes(value);
            byte[] output = new byte[encoded.Length + 1];
            Array.Copy(encoded, output, encoded.Length);
            return output;
        }

        private static void WriteStructuredHeader(byte[] output, uint version, int entryCount, uint pointerSize, uint reserved)
        {
            WriteUInt32BE(output, 0, checked((uint)(output.Length - 4)));
            WriteUInt32LE(output, 4, version);
            WriteUInt32LE(output, 8, checked((uint)entryCount));
            WriteUInt32LE(output, 12, pointerSize);
            WriteUInt32LE(output, 16, reserved);
        }

        private static void ValidateArrayLength(Array array, int expectedLength, string description)
        {
            if (array == null || array.Length != expectedLength)
                throw new InvalidOperationException(description + " must contain exactly " + expectedLength + " values.");
        }

        private static ushort ReadUInt16LE(byte[] bytes, int offset)
        {
            return (ushort)(bytes[offset] | (bytes[offset + 1] << 8));
        }

        private static uint ReadUInt32LE(byte[] bytes, int offset)
        {
            return (uint)(bytes[offset]
                | (bytes[offset + 1] << 8)
                | (bytes[offset + 2] << 16)
                | (bytes[offset + 3] << 24));
        }

        private static int ReadInt32LE(byte[] bytes, int offset)
        {
            return unchecked((int)ReadUInt32LE(bytes, offset));
        }

        private static ulong ReadUInt64LE(byte[] bytes, int offset)
        {
            uint low = ReadUInt32LE(bytes, offset);
            uint high = ReadUInt32LE(bytes, offset + 4);
            return low | ((ulong)high << 32);
        }

        private static uint ReadUInt32BE(byte[] bytes, int offset)
        {
            return (uint)((bytes[offset] << 24)
                | (bytes[offset + 1] << 16)
                | (bytes[offset + 2] << 8)
                | bytes[offset + 3]);
        }

        private static void WriteUInt16LE(byte[] bytes, int offset, ushort value)
        {
            bytes[offset] = (byte)value;
            bytes[offset + 1] = (byte)(value >> 8);
        }

        private static void WriteUInt32LE(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)value;
            bytes[offset + 1] = (byte)(value >> 8);
            bytes[offset + 2] = (byte)(value >> 16);
            bytes[offset + 3] = (byte)(value >> 24);
        }

        private static void WriteInt32LE(byte[] bytes, int offset, int value)
        {
            WriteUInt32LE(bytes, offset, unchecked((uint)value));
        }

        private static void WriteUInt64LE(byte[] bytes, int offset, ulong value)
        {
            WriteUInt32LE(bytes, offset, (uint)value);
            WriteUInt32LE(bytes, offset + 4, (uint)(value >> 32));
        }

        private static void WriteUInt32BE(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)value;
        }
    }
}
