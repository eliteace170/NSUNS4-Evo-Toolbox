using System;
using System.Linq;
using XFBIN_LIB.XFBIN;

namespace XFBIN_LIB
{
    public static class XFBIN_EDITOR
    {
        public static CHUNK AddBinaryChunk(XFBIN.XFBIN xfbin, PAGE page, string chunkName, string filePath, byte[] chunkData, UInt16 version = 121, UInt16 versionAttribute = 0)
        {
            return AddChunk(xfbin, page, "nuccChunkBinary", chunkName, filePath, chunkData, version, versionAttribute);
        }

        public static CHUNK AddChunk(XFBIN.XFBIN xfbin, PAGE page, string chunkTypeName, string chunkName, string filePath, byte[] chunkData, UInt16 version = 121, UInt16 versionAttribute = 0)
        {
            if (xfbin == null) throw new ArgumentNullException("xfbin");
            if (page == null) throw new ArgumentNullException("page");
            if (string.IsNullOrWhiteSpace(chunkTypeName)) throw new ArgumentException("Chunk type is required.", "chunkTypeName");
            if (string.IsNullOrWhiteSpace(chunkName)) throw new ArgumentException("Chunk name is required.", "chunkName");
            if (filePath == null) filePath = "";
            if (chunkData == null) chunkData = new byte[0];

            CHUNK_TABLE table = xfbin.ChunkTable ?? new CHUNK_TABLE();
            xfbin.ChunkTable = table;
            page.ChunkTable = table;

            uint chunkTypeIndex = EnsureChunkType(table, chunkTypeName);
            uint filePathIndex = EnsureFilePath(table, filePath);
            uint chunkNameIndex = EnsureChunkName(table, chunkName);
            uint globalChunkMapIndex = EnsureChunkMap(table, chunkTypeIndex, filePathIndex, chunkNameIndex);
            uint localChunkMapIndex = EnsurePageChunkMapping(table, page, globalChunkMapIndex);

            CHUNK chunk = new CHUNK();
            chunk.Version = version;
            chunk.VersionAttribute = versionAttribute;
            chunk.ChunkData = chunkData;
            chunk.Size = (uint)chunkData.Length;
            chunk.ChunkMapIndex = localChunkMapIndex;
            page.Chunks.Add(chunk);

            RecalculateMetadata(xfbin);
            return chunk;
        }

        public static void RecalculateMetadata(XFBIN.XFBIN xfbin)
        {
            if (xfbin == null || xfbin.ChunkTable == null)
                return;

            CHUNK_TABLE table = xfbin.ChunkTable;
            table.ChunkTypeCount = (uint)table.ChunkTypes.Count;
            table.ChunkTypeSize = (uint)table.ChunkTypes.Sum(x => (x.ChunkTypeName ?? "").Length + 1);
            table.FilePathCount = (uint)table.FilePaths.Count;
            table.FilePathSize = (uint)table.FilePaths.Sum(x => (x.FilePathName ?? "").Length + 1);
            table.ChunkNameCount = (uint)table.ChunkNames.Count;
            table.ChunkNameSize = (uint)table.ChunkNames.Sum(x => (x.ChunkName ?? "").Length + 1);
            table.ChunkMapCount = (uint)table.ChunkMaps.Count;
            table.ChunkMapSize = (uint)(table.ChunkMaps.Count * 0xC);
            table.ChunkMapIndicesCount = (uint)table.ChunkMapIndices.Count;
            table.ExtraIndicesCount = (uint)table.ExtraMappings.Count;

            uint stringSectionSize = 0x28 + table.ChunkTypeSize + table.FilePathSize + table.ChunkNameSize;
            uint stringPadding = (4 - (stringSectionSize % 4)) % 4;
            xfbin.ChunkTableSize = stringSectionSize + stringPadding + table.ChunkMapSize + (table.ExtraIndicesCount * 8) + (table.ChunkMapIndicesCount * 4);

            foreach (PAGE page in xfbin.Pages)
            {
                if (page != null)
                    page.ChunkTable = table;
            }
        }

        private static uint EnsureChunkType(CHUNK_TABLE table, string chunkTypeName)
        {
            int existing = table.ChunkTypes.ToList().FindIndex(x => string.Equals(x.ChunkTypeName, chunkTypeName, StringComparison.Ordinal));
            if (existing >= 0) return (uint)existing;
            CHUNK_TYPE chunkType = new CHUNK_TYPE();
            chunkType.ChunkTypeName = chunkTypeName;
            table.ChunkTypes.Add(chunkType);
            return (uint)(table.ChunkTypes.Count - 1);
        }

        private static uint EnsureFilePath(CHUNK_TABLE table, string filePath)
        {
            int existing = table.FilePaths.ToList().FindIndex(x => string.Equals(x.FilePathName, filePath, StringComparison.Ordinal));
            if (existing >= 0) return (uint)existing;
            FILE_PATH path = new FILE_PATH();
            path.FilePathName = filePath;
            table.FilePaths.Add(path);
            return (uint)(table.FilePaths.Count - 1);
        }

        private static uint EnsureChunkName(CHUNK_TABLE table, string chunkName)
        {
            int existing = table.ChunkNames.ToList().FindIndex(x => string.Equals(x.ChunkName, chunkName, StringComparison.Ordinal));
            if (existing >= 0) return (uint)existing;
            CHUNK_NAME name = new CHUNK_NAME();
            name.ChunkName = chunkName;
            table.ChunkNames.Add(name);
            return (uint)(table.ChunkNames.Count - 1);
        }

        private static uint EnsureChunkMap(CHUNK_TABLE table, uint chunkTypeIndex, uint filePathIndex, uint chunkNameIndex)
        {
            int existing = table.ChunkMaps.ToList().FindIndex(x => x.ChunkTypeIndex == chunkTypeIndex && x.FilePathIndex == filePathIndex && x.ChunkNameIndex == chunkNameIndex);
            if (existing >= 0) return (uint)existing;
            CHUNK_MAP map = new CHUNK_MAP();
            map.ChunkTypeIndex = chunkTypeIndex;
            map.FilePathIndex = filePathIndex;
            map.ChunkNameIndex = chunkNameIndex;
            table.ChunkMaps.Add(map);
            return (uint)(table.ChunkMaps.Count - 1);
        }

        private static uint EnsurePageChunkMapping(CHUNK_TABLE table, PAGE page, uint globalChunkMapIndex)
        {
            CHUNK_MAP globalMap = table.ChunkMaps[(int)globalChunkMapIndex];
            int localIndex = page.ChunkMappings.ToList().FindIndex(x =>
                x.ChunkTypeIndex == globalMap.ChunkTypeIndex &&
                x.FilePathIndex == globalMap.FilePathIndex &&
                x.ChunkNameIndex == globalMap.ChunkNameIndex);

            if (localIndex < 0)
            {
                page.ChunkMappings.Add(globalMap);
                localIndex = page.ChunkMappings.Count - 1;
            }

            while (table.ChunkMapIndices.Count <= localIndex)
            {
                table.ChunkMapIndices.Add(new CHUNK_MAP_INDICES());
            }

            table.ChunkMapIndices[localIndex].ChunkMapIndex = globalChunkMapIndex;
            return (uint)localIndex;
        }
    }
}
