using System;
using System.Collections.Generic;
using System.IO;

namespace NSUNS4_Character_Manager.Tools
{
    internal sealed class PrivateCameraEntry
    {
        internal const int Size = 0x2C;
        private readonly byte[] bytes;
        public int CharacodeIndex { get; internal set; }
        public PrivateCameraEntry() : this(new byte[Size])
        {
            for (int i = 0; i < 11; i++) WriteFloat(i * 4, -1f);
            FOV = FOV2 = 50f;
        }
        internal PrivateCameraEntry(byte[] data) { bytes = (byte[])data.Clone(); }
        public float CameraDistance { get { return ReadFloat(0); } set { WriteFloat(0, value); } }
        public float CameraSpeed { get { return ReadFloat(4); } set { WriteFloat(4, value); } }
        public float CameraMovement { get { return ReadFloat(8); } set { WriteFloat(8, value); } }
        public float Unk1 { get { return ReadFloat(12); } set { WriteFloat(12, value); } }
        public float CameraHeight { get { return ReadFloat(16); } set { WriteFloat(16, value); } }
        public float CameraAngle { get { return ReadFloat(20); } set { WriteFloat(20, value); } }
        public float CameraHeight2 { get { return ReadFloat(24); } set { WriteFloat(24, value); } }
        public float FOV { get { return ReadFloat(28); } set { WriteFloat(28, value); } }
        public float Unk2 { get { return ReadFloat(32); } set { WriteFloat(32, value); } }
        public float CameraDistance2 { get { return ReadFloat(36); } set { WriteFloat(36, value); } }
        public float FOV2 { get { return ReadFloat(40); } set { WriteFloat(40, value); } }
        private float ReadFloat(int offset)
        {
            var value = new byte[4];
            Buffer.BlockCopy(bytes, offset, value, 0, 4);
            if (!BitConverter.IsLittleEndian) Array.Reverse(value);
            return BitConverter.ToSingle(value, 0);
        }
        private void WriteFloat(int offset, float value)
        {
            var raw = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(raw);
            Buffer.BlockCopy(raw, 0, bytes, offset, 4);
        }
        public PrivateCameraEntry Clone() { return new PrivateCameraEntry(bytes) { CharacodeIndex = CharacodeIndex }; }
        internal byte[] GetBytes() { return (byte[])bytes.Clone(); }
        public override string ToString() { return "Characode " + CharacodeIndex; }
    }

    internal static class PrivateCameraCodec
    {
        public static List<PrivateCameraEntry> Read(byte[] data)
        {
            if (data == null || data.Length < 8) throw new InvalidDataException("Missing privateCamera size/count header.");
            uint size = ((uint)data[0] << 24) | ((uint)data[1] << 16) | ((uint)data[2] << 8) | data[3];
            uint count = data[4] | ((uint)data[5] << 8) | ((uint)data[6] << 16) | ((uint)data[7] << 24);
            if (size != data.Length - 4 || 8L + (long)count * PrivateCameraEntry.Size != data.Length)
                throw new InvalidDataException("privateCamera size/count does not match its 44-byte entries.");
            var entries = new List<PrivateCameraEntry>();
            for (int i = 0; i < (int)count; i++)
            {
                var raw = new byte[PrivateCameraEntry.Size];
                Buffer.BlockCopy(data, 8 + i * PrivateCameraEntry.Size, raw, 0, raw.Length);
                entries.Add(new PrivateCameraEntry(raw) { CharacodeIndex = i + 1 });
            }
            return entries;
        }
        public static byte[] Write(IList<PrivateCameraEntry> entries)
        {
            var data = new byte[checked(8 + entries.Count * PrivateCameraEntry.Size)];
            uint size = (uint)(data.Length - 4);
            for (int i = 0; i < 4; i++)
            {
                data[i] = (byte)(size >> (24 - 8 * i));
                data[4 + i] = (byte)((uint)entries.Count >> (8 * i));
            }
            for (int i = 0; i < entries.Count; i++)
                Buffer.BlockCopy(entries[i].GetBytes(), 0, data, 8 + i * PrivateCameraEntry.Size, PrivateCameraEntry.Size);
            return data;
        }
    }
}
