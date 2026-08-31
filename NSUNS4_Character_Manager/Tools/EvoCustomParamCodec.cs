using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NSUNS4_Character_Manager.Tools
{
    // evocustompatam.bt: BE byte count, followed by 0x200-byte LE entries.
    internal sealed class EvoCustomParamEntry
    {
        internal const int Size = 0x200;
        private readonly byte[] bytes;

        public EvoCustomParamEntry() : this(new byte[Size]) { }
        internal EvoCustomParamEntry(byte[] data) { bytes = (byte[])data.Clone(); }

        public string CharacterCode
        {
            get
            {
                int length = Array.IndexOf(bytes, (byte)0, 0, 8);
                return Encoding.ASCII.GetString(bytes, 0, length < 0 ? 8 : length);
            }
            set
            {
                if (value == null || value.Length > 8)
                    throw new ArgumentException("Character code must contain at most 8 ASCII characters.");
                foreach (char c in value)
                    if (c < 32 || c > 126) throw new ArgumentException("Character code must use printable ASCII characters.");
                if (value == CharacterCode) return; // Preserve original terminator/padding bytes.
                Array.Clear(bytes, 0, 8);
                Encoding.ASCII.GetBytes(value).CopyTo(bytes, 0);
            }
        }

        public uint CostumeFlags
        {
            get { return (uint)(bytes[8] | bytes[9] << 8 | bytes[10] << 16 | bytes[11] << 24); }
            set
            {
                for (int i = 0; i < 4; i++) bytes[8 + i] = (byte)(value >> (8 * i));
            }
        }

        public EvoCustomParamEntry Clone() { return new EvoCustomParamEntry(bytes); }
        internal byte[] GetBytes() { return (byte[])bytes.Clone(); }
        public override string ToString() { return CharacterCode.Length == 0 ? "(empty character code)" : CharacterCode; }
    }

    internal static class EvoCustomParamCodec
    {
        public static List<EvoCustomParamEntry> Read(byte[] data)
        {
            if (data == null || data.Length < 4) throw new InvalidDataException("Missing EvoCustomParam size header.");
            uint size = ((uint)data[0] << 24) | ((uint)data[1] << 16) | ((uint)data[2] << 8) | data[3];
            if (size != data.Length - 4 || size % EvoCustomParamEntry.Size != 0)
                throw new InvalidDataException("EvoCustomParam payload size must match the big-endian header and be a multiple of 512 bytes.");
            var entries = new List<EvoCustomParamEntry>();
            for (int offset = 4; offset < data.Length; offset += EvoCustomParamEntry.Size)
            {
                var raw = new byte[EvoCustomParamEntry.Size];
                Buffer.BlockCopy(data, offset, raw, 0, raw.Length);
                entries.Add(new EvoCustomParamEntry(raw));
            }
            return entries;
        }

        public static byte[] Write(IList<EvoCustomParamEntry> entries)
        {
            int size = checked(entries.Count * EvoCustomParamEntry.Size);
            var data = new byte[checked(size + 4)];
            for (int i = 0; i < 4; i++) data[i] = (byte)((uint)size >> (24 - 8 * i));
            for (int i = 0; i < entries.Count; i++)
                Buffer.BlockCopy(entries[i].GetBytes(), 0, data, 4 + i * EvoCustomParamEntry.Size, EvoCustomParamEntry.Size);
            return data;
        }
    }
}
