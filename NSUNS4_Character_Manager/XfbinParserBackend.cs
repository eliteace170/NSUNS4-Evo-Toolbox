using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NSUNS4_Character_Manager
{
    /// <summary>
    /// Reusable unpack/edit/repack backend built around xfbin_parser.exe.
    /// Use this for tools that need to add, remove, rename, or resize binary chunks
    /// inside an XFBIN without hand-editing the container structure.
    /// </summary>
    public sealed class XfbinParserBackend : IDisposable
    {
        private const string BinaryChunkType = "nuccChunkBinary";
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
        private readonly string parserExePath;
        private bool disposed;

        public string SourceFilePath { get; private set; }
        public string WorkingDirectory { get; private set; }

        public XfbinParserBackend(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentException("Source XFBIN path is required.", nameof(sourceFilePath));
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("Source XFBIN file was not found.", sourceFilePath);

            SourceFilePath = sourceFilePath;
            parserExePath = ResolveParserExePath();
            WorkingDirectory = CreateWorkingDirectory();
            RunParser(string.Format("\"{0}\" \"{1}\" -f", SourceFilePath, WorkingDirectory));
        }

        public List<XfbinBinaryChunkPage> GetBinaryChunkPages()
        {
            return GetChunkPages(BinaryChunkType);
        }

        public List<XfbinBinaryChunkPage> GetChunkPages(string chunkType)
        {
            List<XfbinBinaryChunkPage> result = new List<XfbinBinaryChunkPage>();
            if (!Directory.Exists(WorkingDirectory))
                return result;

            foreach (string directory in Directory.GetDirectories(WorkingDirectory))
            {
                string pageJsonPath = Path.Combine(directory, "_page.json");
                if (!File.Exists(pageJsonPath))
                    continue;

                XfbinParserPageDefinition definition = JsonConvert.DeserializeObject<XfbinParserPageDefinition>(File.ReadAllText(pageJsonPath));
                if (definition == null || definition.Chunks == null || definition.ChunkMaps == null)
                    continue;

                XfbinParserChunkEntry binaryChunk = definition.Chunks.FirstOrDefault(x =>
                    x != null &&
                    x.Chunk != null &&
                    string.Equals(x.Chunk.Type, chunkType, StringComparison.OrdinalIgnoreCase));
                if (binaryChunk == null || binaryChunk.Chunk == null)
                    continue;

                int index = ParseDirectoryIndex(Path.GetFileName(directory));
                result.Add(new XfbinBinaryChunkPage
                {
                    Index = index,
                    DirectoryPath = directory,
                    PageJsonPath = pageJsonPath,
                    Definition = definition,
                    BinaryFileName = binaryChunk.FileName ?? string.Empty,
                     BinaryFilePath = Path.Combine(directory, binaryChunk.FileName ?? string.Empty),
                     BinaryData = File.Exists(Path.Combine(directory, binaryChunk.FileName ?? string.Empty)) ? File.ReadAllBytes(Path.Combine(directory, binaryChunk.FileName ?? string.Empty)) : new byte[0],
                     ChunkName = binaryChunk.Chunk.Name ?? string.Empty,
                     ChunkPath = binaryChunk.Chunk.Path ?? string.Empty,
                     ChunkType = binaryChunk.Chunk.Type ?? string.Empty,
                     Version = binaryChunk.Version,
                     VersionAttribute = binaryChunk.VersionAttribute
                 });
             }

            result.Sort((left, right) => left.Index.CompareTo(right.Index));
            return result;
        }

        public List<XfbinBinaryChunkItem> GetBinaryChunks()
        {
            return GetChunkItems(BinaryChunkType);
        }

        public List<XfbinBinaryChunkItem> GetChunkItems(string chunkType)
        {
            List<XfbinBinaryChunkItem> result = new List<XfbinBinaryChunkItem>();
            if (!Directory.Exists(WorkingDirectory))
                return result;

            foreach (string directory in Directory.GetDirectories(WorkingDirectory))
            {
                string pageJsonPath = Path.Combine(directory, "_page.json");
                if (!File.Exists(pageJsonPath))
                    continue;

                XfbinParserPageDefinition definition = JsonConvert.DeserializeObject<XfbinParserPageDefinition>(File.ReadAllText(pageJsonPath));
                if (definition == null || definition.Chunks == null)
                    continue;

                int pageIndex = ParseDirectoryIndex(Path.GetFileName(directory));
                for (int i = 0; i < definition.Chunks.Count; i++)
                {
                    XfbinParserChunkEntry chunkEntry = definition.Chunks[i];
                    if (chunkEntry == null || chunkEntry.Chunk == null)
                        continue;
                    if (!string.Equals(chunkEntry.Chunk.Type, chunkType, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string fileName = chunkEntry.FileName ?? string.Empty;
                    string fullPath = Path.Combine(directory, fileName);
                    result.Add(new XfbinBinaryChunkItem
                    {
                        PageIndex = pageIndex,
                        ChunkIndex = i,
                        DirectoryPath = directory,
                        PageJsonPath = pageJsonPath,
                        FileName = fileName,
                        FilePath = fullPath,
                         BinaryData = File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : new byte[0],
                         ChunkName = chunkEntry.Chunk.Name ?? string.Empty,
                         ChunkPath = chunkEntry.Chunk.Path ?? string.Empty,
                         ChunkType = chunkEntry.Chunk.Type ?? string.Empty,
                         Version = chunkEntry.Version,
                         VersionAttribute = chunkEntry.VersionAttribute
                     });
                 }
             }

            result.Sort((left, right) =>
            {
                int compare = left.PageIndex.CompareTo(right.PageIndex);
                if (compare != 0) return compare;
                return left.ChunkIndex.CompareTo(right.ChunkIndex);
            });
            return result;
        }

        public XfbinBinaryChunkPage FindBinaryChunkPage(string chunkName)
        {
            return FindChunkPage(chunkName, BinaryChunkType);
        }

        public XfbinBinaryChunkPage FindChunkPage(string chunkName, string chunkType)
        {
            return GetChunkPages(chunkType).FirstOrDefault(x =>
                string.Equals(x.ChunkName, chunkName, StringComparison.OrdinalIgnoreCase));
        }

        public XfbinBinaryChunkItem FindBinaryChunkItem(string chunkName)
        {
            return FindChunkItem(chunkName, BinaryChunkType);
        }

        public XfbinBinaryChunkItem FindChunkItem(string chunkName, string chunkType)
        {
            return GetChunkItems(chunkType).FirstOrDefault(x =>
                string.Equals(x.ChunkName, chunkName, StringComparison.OrdinalIgnoreCase));
        }

        public void UpsertBinaryChunk(string oldChunkName, string newChunkName, string chunkPath, byte[] chunkData)
        {
            UpsertChunk(oldChunkName, newChunkName, BinaryChunkType, chunkPath, ".binary", chunkData, 99, 37494);
        }

        public void UpsertChunk(string oldChunkName, string newChunkName, string chunkType, string chunkPath, string fileExtension, byte[] chunkData, int version, int versionAttribute)
        {
            if (string.IsNullOrWhiteSpace(newChunkName))
                throw new ArgumentException("Chunk name is required.", nameof(newChunkName));
            if (string.IsNullOrWhiteSpace(chunkType))
                throw new ArgumentException("Chunk type is required.", nameof(chunkType));
            if (string.IsNullOrWhiteSpace(chunkPath))
                throw new ArgumentException("Chunk path is required.", nameof(chunkPath));
            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentException("File extension is required.", nameof(fileExtension));
            if (chunkData == null)
                throw new ArgumentNullException(nameof(chunkData));

            XfbinBinaryChunkItem existingChunk = null;
            if (!string.IsNullOrWhiteSpace(oldChunkName))
                existingChunk = FindChunkItem(oldChunkName, chunkType);
            if (existingChunk == null)
                existingChunk = FindChunkItem(newChunkName, chunkType);

            if (existingChunk != null && File.Exists(existingChunk.PageJsonPath))
            {
                XfbinParserPageDefinition definition = JsonConvert.DeserializeObject<XfbinParserPageDefinition>(File.ReadAllText(existingChunk.PageJsonPath));
                if (definition != null && definition.Chunks != null && definition.ChunkMaps != null)
                {
                    XfbinParserChunkEntry chunkEntry = definition.Chunks.FirstOrDefault(x =>
                        x != null &&
                        x.Chunk != null &&
                        string.Equals(x.Chunk.Type, chunkType, StringComparison.OrdinalIgnoreCase) &&
                        (string.Equals(x.Chunk.Name, oldChunkName, StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(x.Chunk.Name, newChunkName, StringComparison.OrdinalIgnoreCase)));

                    XfbinParserChunkMap chunkMap = definition.ChunkMaps.FirstOrDefault(x =>
                        x != null &&
                        string.Equals(x.Type, chunkType, StringComparison.OrdinalIgnoreCase) &&
                        (string.Equals(x.Name, oldChunkName, StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(x.Name, newChunkName, StringComparison.OrdinalIgnoreCase)));

                    if (chunkEntry != null)
                    {
                        string extension = Path.GetExtension(chunkEntry.FileName ?? string.Empty);
                        if (string.IsNullOrWhiteSpace(extension))
                            extension = fileExtension;

                        string newFileName = newChunkName + extension;
                        string oldBinaryPath = Path.Combine(existingChunk.DirectoryPath, chunkEntry.FileName ?? string.Empty);
                        string newBinaryPath = Path.Combine(existingChunk.DirectoryPath, newFileName);

                        if (!string.Equals(oldBinaryPath, newBinaryPath, StringComparison.OrdinalIgnoreCase) && File.Exists(oldBinaryPath))
                            File.Delete(oldBinaryPath);

                        File.WriteAllBytes(newBinaryPath, chunkData);

                        chunkEntry.FileName = newFileName;
                        chunkEntry.Version = version;
                        chunkEntry.VersionAttribute = versionAttribute;
                        chunkEntry.Chunk.Name = newChunkName;
                        chunkEntry.Chunk.Path = chunkPath;

                        if (chunkMap != null)
                        {
                            chunkMap.Name = newChunkName;
                            chunkMap.Path = chunkPath;
                        }

                        File.WriteAllText(existingChunk.PageJsonPath, JsonConvert.SerializeObject(definition, Formatting.Indented), Utf8NoBom);
                        return;
                    }
                }
            }

            XfbinBinaryChunkItem containerChunk = FindBestChunkContainer(chunkPath, chunkType);
            if (containerChunk != null && File.Exists(containerChunk.PageJsonPath))
            {
                XfbinParserPageDefinition definition = JsonConvert.DeserializeObject<XfbinParserPageDefinition>(File.ReadAllText(containerChunk.PageJsonPath));
                if (definition != null)
                {
                    if (definition.Chunks == null)
                        definition.Chunks = new List<XfbinParserChunkEntry>();
                    if (definition.ChunkMaps == null)
                        definition.ChunkMaps = new List<XfbinParserChunkMap>();

                    string fileName = newChunkName + fileExtension;
                    string containerBinaryFilePath = Path.Combine(containerChunk.DirectoryPath, fileName);
                    File.WriteAllBytes(containerBinaryFilePath, chunkData);

                    definition.ChunkMaps.Add(new XfbinParserChunkMap
                    {
                        Name = newChunkName,
                        Type = chunkType,
                        Path = chunkPath
                    });

                    definition.Chunks.Add(new XfbinParserChunkEntry
                    {
                        FileName = fileName,
                        Version = version,
                        VersionAttribute = versionAttribute,
                        Chunk = new XfbinParserChunkMap
                        {
                            Name = newChunkName,
                            Type = chunkType,
                            Path = chunkPath
                        }
                    });

                    File.WriteAllText(containerChunk.PageJsonPath, JsonConvert.SerializeObject(definition, Formatting.Indented), Utf8NoBom);
                    return;
                }
            }

            XfbinBinaryChunkPage page = null;
            if (!string.IsNullOrWhiteSpace(oldChunkName))
                page = FindChunkPage(oldChunkName, chunkType);
            if (page == null)
                page = FindChunkPage(newChunkName, chunkType);

            int pageIndex = page != null ? page.Index : GetNextPageIndex(chunkType);
            string targetDirectory = BuildDirectoryPath(pageIndex, newChunkName, chunkType);
            string binaryFileName = newChunkName + fileExtension;
            string binaryFilePath = Path.Combine(targetDirectory, binaryFileName);

            if (page != null && !string.Equals(page.DirectoryPath, targetDirectory, StringComparison.OrdinalIgnoreCase))
            {
                if (Directory.Exists(targetDirectory))
                    Directory.Delete(targetDirectory, true);
                Directory.Move(page.DirectoryPath, targetDirectory);
                page.DirectoryPath = targetDirectory;
                page.PageJsonPath = Path.Combine(targetDirectory, "_page.json");
            }
            else if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            if (page != null && !string.IsNullOrWhiteSpace(page.BinaryFileName))
            {
                string oldBinaryPath = Path.Combine(targetDirectory, page.BinaryFileName);
                if (!string.Equals(oldBinaryPath, binaryFilePath, StringComparison.OrdinalIgnoreCase) && File.Exists(oldBinaryPath))
                    File.Delete(oldBinaryPath);
            }

            File.WriteAllBytes(binaryFilePath, chunkData);
            File.WriteAllText(Path.Combine(targetDirectory, "_page.json"), BuildPageJson(newChunkName, chunkType, chunkPath, binaryFileName, version, versionAttribute), Utf8NoBom);
        }

        public void DeleteBinaryChunk(string chunkName)
        {
            DeleteChunk(chunkName, BinaryChunkType);
        }

        public void DeleteChunk(string chunkName, string chunkType)
        {
            XfbinBinaryChunkItem chunk = FindChunkItem(chunkName, chunkType);
            if (chunk == null || !Directory.Exists(chunk.DirectoryPath))
                return;

            XfbinParserPageDefinition definition = File.Exists(chunk.PageJsonPath)
                ? JsonConvert.DeserializeObject<XfbinParserPageDefinition>(File.ReadAllText(chunk.PageJsonPath))
                : null;

            if (definition == null || definition.Chunks == null || definition.ChunkMaps == null)
            {
                Directory.Delete(chunk.DirectoryPath, true);
                return;
            }

            definition.Chunks.RemoveAll(x =>
                x != null &&
                x.Chunk != null &&
                string.Equals(x.Chunk.Type, chunkType, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Chunk.Name, chunkName, StringComparison.OrdinalIgnoreCase));

            definition.ChunkMaps.RemoveAll(x =>
                x != null &&
                string.Equals(x.Type, chunkType, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Name, chunkName, StringComparison.OrdinalIgnoreCase));

            if (File.Exists(chunk.FilePath))
                File.Delete(chunk.FilePath);

            bool hasBinaryChunks = definition.Chunks.Any(x =>
                x != null &&
                x.Chunk != null &&
                string.Equals(x.Chunk.Type, chunkType, StringComparison.OrdinalIgnoreCase));

            if (!hasBinaryChunks)
            {
                Directory.Delete(chunk.DirectoryPath, true);
                return;
            }

            File.WriteAllText(chunk.PageJsonPath, JsonConvert.SerializeObject(definition, Formatting.Indented), Utf8NoBom);
        }

        public void SetChunkPageChunkMaps(string chunkName, string chunkType, List<XfbinParserChunkMap> referenceChunkMaps)
        {
            XfbinBinaryChunkPage page = FindChunkPage(chunkName, chunkType);
            if (page == null || !File.Exists(page.PageJsonPath))
                throw new FileNotFoundException("Chunk page was not found for chunk map update.", page != null ? page.PageJsonPath : chunkName);

            XfbinParserPageDefinition definition = JsonConvert.DeserializeObject<XfbinParserPageDefinition>(File.ReadAllText(page.PageJsonPath));
            if (definition == null || definition.Chunks == null)
                throw new InvalidOperationException("Chunk page JSON could not be parsed.");

            XfbinParserChunkEntry chunkEntry = definition.Chunks.FirstOrDefault(x =>
                x != null &&
                x.Chunk != null &&
                string.Equals(x.Chunk.Type, chunkType, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Chunk.Name, chunkName, StringComparison.OrdinalIgnoreCase));
            if (chunkEntry == null || chunkEntry.Chunk == null)
                throw new InvalidOperationException("Target chunk entry was not found in the page definition.");

            List<XfbinParserChunkMap> rebuiltChunkMaps = new List<XfbinParserChunkMap>();
            if (referenceChunkMaps != null)
            {
                rebuiltChunkMaps.AddRange(referenceChunkMaps.Where(x => x != null).Select(x => new XfbinParserChunkMap
                {
                    Name = x.Name ?? "",
                    Type = x.Type ?? "",
                    Path = x.Path ?? ""
                }));
            }

            rebuiltChunkMaps.Add(new XfbinParserChunkMap { Name = "", Type = "nuccChunkNull", Path = "" });
            rebuiltChunkMaps.Add(new XfbinParserChunkMap
            {
                Name = chunkEntry.Chunk.Name ?? "",
                Type = chunkEntry.Chunk.Type ?? "",
                Path = chunkEntry.Chunk.Path ?? ""
            });
            rebuiltChunkMaps.Add(new XfbinParserChunkMap { Name = "Page0", Type = "nuccChunkPage", Path = "" });
            rebuiltChunkMaps.Add(new XfbinParserChunkMap { Name = "index", Type = "nuccChunkIndex", Path = "" });

            definition.ChunkMaps = rebuiltChunkMaps;
            File.WriteAllText(page.PageJsonPath, JsonConvert.SerializeObject(definition, Formatting.Indented), Utf8NoBom);
        }

        private XfbinBinaryChunkItem FindBestChunkContainer(string chunkPath, string chunkType)
        {
            string targetDirectory = NormalizeChunkDirectory(chunkPath);
            List<XfbinBinaryChunkItem> chunks = GetChunkItems(chunkType);

            XfbinBinaryChunkItem match = chunks.FirstOrDefault(x => NormalizeChunkDirectory(x.ChunkPath) == targetDirectory);
            if (match != null)
                return match;

            return chunks
                .GroupBy(x => x.PageJsonPath)
                .OrderByDescending(x => x.Count())
                .Select(x => x.First())
                .FirstOrDefault();
        }

        private static string NormalizeChunkDirectory(string chunkPath)
        {
            if (string.IsNullOrWhiteSpace(chunkPath))
                return string.Empty;

            string normalized = chunkPath.Replace('\\', '/');
            int lastSlash = normalized.LastIndexOf('/');
            return lastSlash >= 0 ? normalized.Substring(0, lastSlash) : normalized;
        }

        public void RepackTo(string outputPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path is required.", nameof(outputPath));
            RunParser(string.Format("\"{0}\" \"{1}\" -f", WorkingDirectory, outputPath));
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(WorkingDirectory) && Directory.Exists(WorkingDirectory))
                    Directory.Delete(WorkingDirectory, true);
            }
            catch
            {
            }
        }

        private static string BuildPageJson(string chunkName, string chunkType, string chunkPath, string binaryFileName, int version, int versionAttribute)
        {
            XfbinParserPageDefinition definition = new XfbinParserPageDefinition
            {
                ChunkMaps = new List<XfbinParserChunkMap>
                {
                    new XfbinParserChunkMap { Name = "", Type = "nuccChunkNull", Path = "" },
                    new XfbinParserChunkMap { Name = chunkName, Type = chunkType, Path = chunkPath },
                    new XfbinParserChunkMap { Name = "Page0", Type = "nuccChunkPage", Path = "" },
                    new XfbinParserChunkMap { Name = "index", Type = "nuccChunkIndex", Path = "" }
                },
                ChunkReferences = new List<object>(),
                Chunks = new List<XfbinParserChunkEntry>
                {
                    new XfbinParserChunkEntry
                    {
                        FileName = binaryFileName,
                        Version = version,
                        VersionAttribute = versionAttribute,
                        Chunk = new XfbinParserChunkMap { Name = chunkName, Type = chunkType, Path = chunkPath }
                    }
                }
            };

            return JsonConvert.SerializeObject(definition, Formatting.Indented);
        }

        private static int ParseDirectoryIndex(string directoryName)
        {
            if (string.IsNullOrWhiteSpace(directoryName) || directoryName.Length < 5 || directoryName[0] != '[')
                return -1;
            int closing = directoryName.IndexOf(']');
            if (closing <= 1)
                return -1;
            int value;
            return int.TryParse(directoryName.Substring(1, closing - 1), out value) ? value : -1;
        }

        private int GetNextPageIndex(string chunkType)
        {
            List<XfbinBinaryChunkPage> pages = GetChunkPages(chunkType);
            return pages.Count == 0 ? 0 : pages.Max(x => x.Index) + 1;
        }

        private string BuildDirectoryPath(int index, string chunkName, string chunkType)
        {
            return Path.Combine(WorkingDirectory, string.Format("[{0:D3}] {1} ({2})", index, chunkName, chunkType));
        }

        private string ResolveParserExePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            string candidate = Path.Combine(baseDirectory, "xfbin_parser.exe");
            if (File.Exists(candidate))
                return candidate;

            candidate = Path.Combine(baseDirectory, "dependencies", "xfbin_parser.exe");
            if (File.Exists(candidate))
                return candidate;

            string baseParentDirectory = Directory.Exists(baseDirectory) ? Directory.GetParent(baseDirectory)?.FullName ?? string.Empty : string.Empty;
            if (!string.IsNullOrWhiteSpace(baseParentDirectory))
            {
                candidate = Path.Combine(baseParentDirectory, "xfbin_parser.exe");
                if (File.Exists(candidate))
                    return candidate;

                string baseGrandParentDirectory = Directory.Exists(baseParentDirectory) ? Directory.GetParent(baseParentDirectory)?.FullName ?? string.Empty : string.Empty;
                if (!string.IsNullOrWhiteSpace(baseGrandParentDirectory))
                {
                    candidate = Path.Combine(baseGrandParentDirectory, "xfbin_parser.exe");
                    if (File.Exists(candidate))
                        return candidate;
                }
            }

            candidate = Path.Combine(Path.GetDirectoryName(SourceFilePath) ?? string.Empty, "xfbin_parser.exe");
            if (File.Exists(candidate))
                return candidate;

            string sourceDirectory = Path.GetDirectoryName(SourceFilePath) ?? string.Empty;
            string parentDirectory = Directory.Exists(sourceDirectory) ? Directory.GetParent(sourceDirectory)?.FullName ?? string.Empty : string.Empty;
            if (!string.IsNullOrWhiteSpace(parentDirectory))
            {
                candidate = Path.Combine(parentDirectory, "xfbin_parser.exe");
                if (File.Exists(candidate))
                    return candidate;
            }

            string currentDirectory = Environment.CurrentDirectory ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(currentDirectory))
            {
                candidate = Path.Combine(currentDirectory, "xfbin_parser.exe");
                if (File.Exists(candidate))
                    return candidate;

                string currentParentDirectory = Directory.Exists(currentDirectory) ? Directory.GetParent(currentDirectory)?.FullName ?? string.Empty : string.Empty;
                if (!string.IsNullOrWhiteSpace(currentParentDirectory))
                {
                    candidate = Path.Combine(currentParentDirectory, "xfbin_parser.exe");
                    if (File.Exists(candidate))
                        return candidate;
                }
            }

            throw new FileNotFoundException("xfbin_parser.exe was not found next to the application or source files.");
        }

        private static string CreateWorkingDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), "NSUNS4_XfbinParser_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return path;
        }

        private void RunParser(string arguments)
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = parserExePath,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = Path.GetDirectoryName(parserExePath) ?? string.Empty
                };

                process.Start();
                string standardOutput = process.StandardOutput.ReadToEnd();
                string standardError = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new InvalidOperationException((standardOutput + Environment.NewLine + standardError).Trim());
            }
        }
    }

    public sealed class XfbinBinaryChunkPage
    {
        public int Index;
        public string DirectoryPath;
        public string PageJsonPath;
        public string BinaryFileName;
        public string BinaryFilePath;
        public byte[] BinaryData;
        public string ChunkName;
        public string ChunkPath;
        public string ChunkType;
        public int Version;
        public int VersionAttribute;
        public XfbinParserPageDefinition Definition;
    }

    public sealed class XfbinBinaryChunkItem
    {
        public int PageIndex;
        public int ChunkIndex;
        public string DirectoryPath;
        public string PageJsonPath;
        public string FileName;
        public string FilePath;
        public byte[] BinaryData;
        public string ChunkName;
        public string ChunkPath;
        public string ChunkType;
        public int Version;
        public int VersionAttribute;
    }

    public sealed class XfbinParserPageDefinition
    {
        [JsonProperty("Chunk Maps")]
        public List<XfbinParserChunkMap> ChunkMaps;

        [JsonProperty("Chunk References")]
        public List<object> ChunkReferences;

        [JsonProperty("Chunks")]
        public List<XfbinParserChunkEntry> Chunks;
    }

    public sealed class XfbinParserChunkEntry
    {
        [JsonProperty("File Name")]
        public string FileName;

        [JsonProperty("Version")]
        public int Version;

        [JsonProperty("Version Attribute")]
        public int VersionAttribute;

        [JsonProperty("Chunk")]
        public XfbinParserChunkMap Chunk;
    }

    public sealed class XfbinParserChunkMap
    {
        [JsonProperty("Name")]
        public string Name;

        [JsonProperty("Type")]
        public string Type;

        [JsonProperty("Path")]
        public string Path;
    }
}
