using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace FileExplorer.Services
{
    public static class ZipService
    {
        public static void CreateZip(IList<string> paths, string zipPath)
        {
            if (paths == null || paths.Count == 0)
            {
                throw new ArgumentException("Nothing to compress.", "paths");
            }

            string fullZipPath = Path.GetFullPath(zipPath);

            foreach (string path in paths)
            {
                if (Path.GetFullPath(path).Equals(fullZipPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Cannot compress an archive into itself.");
                }
            }

            if (File.Exists(fullZipPath))
            {
                File.Delete(fullZipPath);
            }

            using (ZipArchive archive = ZipFile.Open(fullZipPath, ZipArchiveMode.Create))
            {
                foreach (string path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        string rootName = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                        if (string.IsNullOrEmpty(rootName))
                        {
                            throw new ArgumentException("Cannot compress a drive root directly.", "paths");
                        }

                        AddDirectory(archive, path, rootName);
                    }
                    else if (File.Exists(path))
                    {
                        archive.CreateEntryFromFile(path, Path.GetFileName(path), CompressionLevel.Optimal);
                    }
                    else
                    {
                        throw new FileNotFoundException("Item no longer exists.", path);
                    }
                }
            }
        }

        public static void ExtractZip(string zipPath, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            string destinationRoot = Path.GetFullPath(destinationDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryName = entry.FullName.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                    string targetPath = Path.GetFullPath(Path.Combine(destinationRoot, entryName));

                    if (!targetPath.StartsWith(destinationRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new IOException("The archive contains an unsafe path: " + entry.FullName);
                    }

                    if (entryName.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) || entry.Name.Length == 0)
                    {
                        Directory.CreateDirectory(targetPath);
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    entry.ExtractToFile(targetPath, true);
                }
            }
        }

        public static string GetAvailableDestination(string zipPath)
        {
            string baseDirectory = Path.Combine(Path.GetDirectoryName(zipPath), Path.GetFileNameWithoutExtension(zipPath));
            string candidate = baseDirectory;
            int counter = 2;

            while (Directory.Exists(candidate))
            {
                candidate = baseDirectory + " (" + counter + ")";
                counter++;
            }

            return candidate;
        }

        private static void AddDirectory(ZipArchive archive, string directory, string entryRoot)
        {
            archive.CreateEntry(entryRoot + "/");

            foreach (string file in Directory.GetFiles(directory))
            {
                archive.CreateEntryFromFile(file, entryRoot + "/" + Path.GetFileName(file), CompressionLevel.Optimal);
            }

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                AddDirectory(archive, subDirectory, entryRoot + "/" + Path.GetFileName(subDirectory));
            }
        }
    }
}
