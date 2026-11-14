using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileExplorer.Services
{
    public class PeSectionInfo
    {
        public string Name = string.Empty;
        public uint VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public string Characteristics = string.Empty;
    }

    public class PeImportInfo
    {
        public string DllName = string.Empty;
        public List<string> Functions = new List<string>();
        public bool Truncated;
    }

    public class PeInfo
    {
        public bool IsValid;
        public string Error = string.Empty;
        public bool IsDll;
        public ushort Machine;
        public string MachineName = string.Empty;
        public ushort Subsystem;
        public string SubsystemName = string.Empty;
        public uint TimeDateStamp;
        public string LinkerTimestampText = string.Empty;
        public ulong ImageBase;
        public uint EntryPointRva;
        public bool IsPe32Plus;
        public uint SizeOfImage;
        public List<PeSectionInfo> Sections = new List<PeSectionInfo>();
        public List<PeImportInfo> Imports = new List<PeImportInfo>();
        public int ImportTruncatedCount;
        public List<string> Exports = new List<string>();
        public uint ExportFunctionCount;
        public uint ExportNameCount;
        public bool ExportsTruncated;
        public bool IsManaged;
        public ushort RuntimeVersionMajor;
        public ushort RuntimeVersionMinor;
        public string MetadataRuntimeVersion = string.Empty;
        public string TargetFramework = string.Empty;
        public bool UsesMscoree;

        public string KindText
        {
            get { return IsDll ? "Dynamic-link library (DLL)" : "Application (EXE)"; }
        }

        public string ManagedText
        {
            get
            {
                if (!IsManaged)
                {
                    return "Native";
                }

                return "Managed (CLI/.NET image)";
            }
        }

        public string RuntimeVersionText
        {
            get
            {
                if (!IsManaged)
                {
                    return string.Empty;
                }

                string text = "Runtime " + RuntimeVersionMajor + "." + RuntimeVersionMinor;

                if (MetadataRuntimeVersion.Length > 0)
                {
                    text += "  (metadata: " + MetadataRuntimeVersion + ")";
                }

                return text;
            }
        }
    }

    public static class PeBrowserService
    {
        private const int MaxImportsPerDll = 512;
        private const int MaxImportDlls = 256;
        private const int MaxExportNames = 2048;
        private const int MaxMetadataScanBytes = 8 * 1024 * 1024;

        private static readonly Dictionary<ushort, string> MachineNames = new Dictionary<ushort, string>
        {
            { 0x014c, "x86 (Intel 386 or later)" },
            { 0x8664, "x64 (AMD64 or Intel 64)" },
            { 0x01c0, "ARM" },
            { 0x01c4, "ARM Thumb-2" },
            { 0xAA64, "ARM64" },
            { 0x0200, "IA-64 (Itanium)" }
        };

        private static readonly Dictionary<ushort, string> SubsystemNames = new Dictionary<ushort, string>
        {
            { 1, "Native (driver)" },
            { 2, "Windows GUI" },
            { 3, "Windows console (CUI)" },
            { 5, "OS/2 console" },
            { 7, "POSIX console" },
            { 8, "Native Win9x" },
            { 9, "Windows CE GUI" },
            { 10, "EFI application" },
            { 11, "EFI boot driver" },
            { 12, "EFI runtime driver" },
            { 13, "EFI ROM" },
            { 14, "Xbox" }
        };

        public static PeInfo ReadPeInfo(string path)
        {
            var info = new PeInfo();

            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Parse(stream, info);
                }
            }
            catch (Exception ex)
            {
                info.IsValid = false;
                info.Error = ex.Message;
            }

            return info;
        }

        private static void Parse(FileStream stream, PeInfo info)
        {
            if (stream.Length < 64)
            {
                info.Error = "File is too small to be a PE image.";
                return;
            }

            byte[] header = ReadBlock(stream, 0, 4096);

            if (header.Length < 64 || header[0] != 0x4D || header[1] != 0x5A)
            {
                info.Error = "Missing MZ signature; not a PE image.";
                return;
            }

            int peOffset = ReadInt32(header, 0x3C);

            if (peOffset <= 0 || peOffset + 24 > header.Length)
            {
                header = ReadBlock(stream, 0, peOffset + 4096);

                if (peOffset <= 0 || peOffset + 24 > header.Length)
                {
                    info.Error = "PE header is out of range.";
                    return;
                }
            }

            if (ReadUInt16(header, peOffset) != 0x4550) // "PE\0\0"
            {
                info.Error = "Missing PE signature.";
                return;
            }

            int coffOffset = peOffset + 4;
            ushort machine = ReadUInt16(header, coffOffset);
            ushort numberOfSections = ReadUInt16(header, coffOffset + 2);
            uint timeDateStamp = ReadUInt32(header, coffOffset + 4);
            ushort sizeOfOptionalHeader = ReadUInt16(header, coffOffset + 16);
            ushort characteristics = ReadUInt16(header, coffOffset + 18);

            info.Machine = machine;
            info.MachineName = LookupName(MachineNames, machine, "Unknown (0x" + machine.ToString("X4") + ")");
            info.IsDll = (characteristics & 0x2000) != 0;
            info.TimeDateStamp = timeDateStamp;
            info.LinkerTimestampText = FormatTimestamp(timeDateStamp);

            int optionalOffset = coffOffset + 20;

            if (sizeOfOptionalHeader < 16 || optionalOffset + sizeOfOptionalHeader > header.Length)
            {
                info.Error = "Optional header is truncated.";
                return;
            }

            ushort magic = ReadUInt16(header, optionalOffset);
            bool pe32Plus;

            if (magic == 0x10B)
            {
                pe32Plus = false;
            }
            else if (magic == 0x20B)
            {
                pe32Plus = true;
            }
            else
            {
                info.Error = "Unknown optional header magic 0x" + magic.ToString("X4") + ".";
                return;
            }

            info.IsPe32Plus = pe32Plus;
            info.SizeOfImage = ReadUInt32(header, optionalOffset + 56);

            if (pe32Plus)
            {
                info.ImageBase = ReadUInt64(header, optionalOffset + 24);
                info.EntryPointRva = ReadUInt32(header, optionalOffset + 16);
                info.Subsystem = ReadUInt16(header, optionalOffset + 68);
            }
            else
            {
                info.ImageBase = ReadUInt32(header, optionalOffset + 28);
                info.EntryPointRva = ReadUInt32(header, optionalOffset + 16);
                info.Subsystem = ReadUInt16(header, optionalOffset + 68);
            }

            info.SubsystemName = LookupName(SubsystemNames, info.Subsystem, "Unknown");

            int numberOfRvaAndSizes = (int)ReadUInt32(header, optionalOffset + (pe32Plus ? 108 : 92));
            int dataDirectoryOffset = optionalOffset + (pe32Plus ? 112 : 96);

            uint importRva = 0;
            uint exportRva = 0;
            uint clrRva = 0;
            uint clrSize = 0;

            if (numberOfRvaAndSizes > 0)
            {
                exportRva = ReadUInt32(header, dataDirectoryOffset + 0);
            }

            if (numberOfRvaAndSizes > 1)
            {
                importRva = ReadUInt32(header, dataDirectoryOffset + 8);
            }

            if (numberOfRvaAndSizes > 14)
            {
                clrRva = ReadUInt32(header, dataDirectoryOffset + 14 * 8);
                clrSize = ReadUInt32(header, dataDirectoryOffset + 14 * 8 + 4);
            }

            // Section table.
            int sectionOffset = optionalOffset + sizeOfOptionalHeader;

            for (int i = 0; i < numberOfSections; i++)
            {
                int entryOffset = sectionOffset + i * 40;

                if (entryOffset + 40 > header.Length)
                {
                    break;
                }

                uint characteristicsSection = ReadUInt32(header, entryOffset + 36);

                info.Sections.Add(new PeSectionInfo
                {
                    Name = ReadNullTerminatedAscii(header, entryOffset, 8),
                    VirtualSize = ReadUInt32(header, entryOffset + 8),
                    VirtualAddress = ReadUInt32(header, entryOffset + 12),
                    SizeOfRawData = ReadUInt32(header, entryOffset + 16),
                    PointerToRawData = ReadUInt32(header, entryOffset + 20),
                    Characteristics = DescribeSectionCharacteristics(characteristicsSection)
                });
            }

            // CLR header (managed detection).
            if (clrRva != 0)
            {
                info.IsManaged = true;

                byte[] clrHeader = ReadAtRva(stream, info.Sections, clrRva, 72);

                if (clrHeader != null && clrHeader.Length >= 16)
                {
                    info.RuntimeVersionMajor = ReadUInt16(clrHeader, 4);
                    info.RuntimeVersionMinor = ReadUInt16(clrHeader, 6);
                    uint metadataRva = ReadUInt32(clrHeader, 8);
                    uint metadataSize = ReadUInt32(clrHeader, 12);

                    ReadMetadataInfo(stream, info, metadataRva, metadataSize);
                }
            }

            info.UsesMscoree = HasImport(info, "mscoree.dll");

            // Imports.
            if (importRva != 0)
            {
                ReadImports(stream, info, importRva);
            }

            // Exports.
            if (exportRva != 0)
            {
                ReadExports(stream, info, exportRva);
            }

            info.IsValid = true;
        }

        private static void ReadMetadataInfo(FileStream stream, PeInfo info, uint metadataRva, uint metadataSize)
        {
            if (metadataRva == 0)
            {
                return;
            }

            int scanLength = (int)Math.Min(metadataSize == 0 ? 65536 : metadataSize, MaxMetadataScanBytes);
            byte[] metadata = ReadAtRva(stream, info.Sections, metadataRva, scanLength);

            if (metadata == null || metadata.Length < 16)
            {
                return;
            }

            if (ReadUInt32(metadata, 0) != 0x424A5342) // "BSJB"
            {
                return;
            }

            uint versionLength = ReadUInt32(metadata, 12);

            if (versionLength > 0 && 16 + versionLength <= (uint)metadata.Length)
            {
                string version = Encoding.ASCII.GetString(metadata, 16, (int)versionLength);
                info.MetadataRuntimeVersion = version.TrimEnd('\0').Trim();
            }

            // Best-effort: find the TargetFrameworkAttribute string in the metadata
            // heaps, e.g. ".NETFramework,Version=v4.7.2" or ".NETCoreApp,Version=v8.0".
            string target = FindMetadataString(metadata, ".NETFramework,Version=");

            if (target == null)
            {
                target = FindMetadataString(metadata, ".NETCoreApp,Version=");
            }

            if (target == null)
            {
                target = FindMetadataString(metadata, ".NETStandard,Version=");
            }

            if (target == null)
            {
                target = FindMetadataString(metadata, "Silverlight,Version=");
            }

            info.TargetFramework = target ?? string.Empty;
        }

        private static string FindMetadataString(byte[] data, string prefix)
        {
            byte[] prefixBytes = Encoding.ASCII.GetBytes(prefix);

            for (int i = 0; i <= data.Length - prefixBytes.Length; i++)
            {
                bool match = true;

                for (int j = 0; j < prefixBytes.Length; j++)
                {
                    if (data[i + j] != prefixBytes[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (!match)
                {
                    continue;
                }

                int end = i + prefixBytes.Length;

                while (end < data.Length && data[end] >= 0x20 && data[end] < 0x7F)
                {
                    end++;
                }

                return Encoding.ASCII.GetString(data, i, end - i);
            }

            return null;
        }

        private static void ReadImports(FileStream stream, PeInfo info, uint importRva)
        {
            int descriptorIndex = 0;

            while (true)
            {
                if (info.Imports.Count >= MaxImportDlls)
                {
                    return;
                }

                byte[] descriptor = ReadAtRva(stream, info.Sections, (uint)(importRva + descriptorIndex * 20), 20);

                if (descriptor == null || descriptor.Length < 20)
                {
                    return;
                }

                uint originalFirstThunk = ReadUInt32(descriptor, 0);
                uint nameRva = ReadUInt32(descriptor, 12);
                uint firstThunk = ReadUInt32(descriptor, 16);

                if (originalFirstThunk == 0 && nameRva == 0 && firstThunk == 0)
                {
                    return;
                }

                var import = new PeImportInfo
                {
                    DllName = ReadAsciiAtRva(stream, info.Sections, nameRva, 256)
                };

                uint thunkRva = originalFirstThunk != 0 ? originalFirstThunk : firstThunk;
                int functionIndex = 0;

                while (thunkRva != 0 && functionIndex < MaxImportsPerDll)
                {
                    byte[] thunk;

                    if (info.IsPe32Plus)
                    {
                        thunk = ReadAtRva(stream, info.Sections, (uint)(thunkRva + functionIndex * 8), 8);

                        if (thunk == null || thunk.Length < 8)
                        {
                            break;
                        }

                        ulong value = ReadUInt64(thunk, 0);

                        if (value == 0)
                        {
                            break;
                        }

                        if ((value & 0x8000000000000000UL) != 0)
                        {
                            import.Functions.Add("#" + (value & 0xFFFF).ToString());
                        }
                        else
                        {
                            import.Functions.Add(ReadAsciiAtRva(stream, info.Sections, (uint)(value & 0xFFFFFFFFUL) + 2, 512));
                        }
                    }
                    else
                    {
                        thunk = ReadAtRva(stream, info.Sections, (uint)(thunkRva + functionIndex * 4), 4);

                        if (thunk == null || thunk.Length < 4)
                        {
                            break;
                        }

                        uint value = ReadUInt32(thunk, 0);

                        if (value == 0)
                        {
                            break;
                        }

                        if ((value & 0x80000000) != 0)
                        {
                            import.Functions.Add("#" + (value & 0xFFFF).ToString());
                        }
                        else
                        {
                            import.Functions.Add(ReadAsciiAtRva(stream, info.Sections, value + 2, 512));
                        }
                    }

                    functionIndex++;
                }

                if (functionIndex >= MaxImportsPerDll)
                {
                    import.Truncated = true;
                }

                info.Imports.Add(import);
                descriptorIndex++;
            }
        }

        private static void ReadExports(FileStream stream, PeInfo info, uint exportRva)
        {
            byte[] directory = ReadAtRva(stream, info.Sections, exportRva, 40);

            if (directory == null || directory.Length < 40)
            {
                return;
            }

            uint numberOfFunctions = ReadUInt32(directory, 20);
            uint numberOfNames = ReadUInt32(directory, 24);
            uint addressOfNames = ReadUInt32(directory, 32);

            info.ExportFunctionCount = numberOfFunctions;
            info.ExportNameCount = numberOfNames;

            uint count = Math.Min(numberOfNames, MaxExportNames);
            info.ExportsTruncated = numberOfNames > MaxExportNames;

            for (uint i = 0; i < count; i++)
            {
                byte[] namePointer = ReadAtRva(stream, info.Sections, (uint)(addressOfNames + i * 4), 4);

                if (namePointer == null || namePointer.Length < 4)
                {
                    break;
                }

                uint nameRva = ReadUInt32(namePointer, 0);
                info.Exports.Add(ReadAsciiAtRva(stream, info.Sections, nameRva, 512));
            }
        }

        private static bool HasImport(PeInfo info, string dllName)
        {
            foreach (PeImportInfo import in info.Imports)
            {
                if (string.Equals(import.DllName, dllName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string DescribeSectionCharacteristics(uint characteristics)
        {
            var flags = new List<string>();

            if ((characteristics & 0x00000020) != 0)
            {
                flags.Add("code");
            }

            if ((characteristics & 0x00000040) != 0)
            {
                flags.Add("initialized data");
            }

            if ((characteristics & 0x00000080) != 0)
            {
                flags.Add("uninitialized data");
            }

            if ((characteristics & 0x02000000) != 0)
            {
                flags.Add("discardable");
            }

            if ((characteristics & 0x04000000) != 0)
            {
                flags.Add("not paged");
            }

            if ((characteristics & 0x10000000) != 0)
            {
                flags.Add("shared");
            }

            if ((characteristics & 0x20000000) != 0)
            {
                flags.Add("executable");
            }

            if ((characteristics & 0x40000000) != 0)
            {
                flags.Add("readable");
            }

            if ((characteristics & 0x80000000) != 0)
            {
                flags.Add("writable");
            }

            return string.Join(", ", flags.ToArray());
        }

        private static byte[] ReadAtRva(FileStream stream, List<PeSectionInfo> sections, uint rva, int count)
        {
            foreach (PeSectionInfo section in sections)
            {
                uint virtualSize = Math.Max(section.VirtualSize, section.SizeOfRawData);

                if (rva >= section.VirtualAddress && rva < section.VirtualAddress + virtualSize)
                {
                    if (section.SizeOfRawData == 0 || section.PointerToRawData == 0)
                    {
                        return new byte[0];
                    }

                    long offsetInSection = rva - section.VirtualAddress;

                    if (offsetInSection >= section.SizeOfRawData)
                    {
                        return new byte[0];
                    }

                    int readable = (int)Math.Min((long)count, section.SizeOfRawData - offsetInSection);
                    return ReadBlock(stream, section.PointerToRawData + offsetInSection, readable);
                }
            }

            return null;
        }

        private static string ReadAsciiAtRva(FileStream stream, List<PeSectionInfo> sections, uint rva, int maxLength)
        {
            byte[] data = ReadAtRva(stream, sections, rva, maxLength);

            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            return ReadNullTerminatedAscii(data, 0, data.Length);
        }

        private static byte[] ReadBlock(FileStream stream, long offset, int count)
        {
            if (count <= 0)
            {
                return new byte[0];
            }

            if (offset < 0 || offset >= stream.Length)
            {
                return new byte[0];
            }

            count = (int)Math.Min((long)count, stream.Length - offset);

            byte[] buffer = new byte[count];
            int read = 0;
            stream.Seek(offset, SeekOrigin.Begin);

            while (read < count)
            {
                int chunk = stream.Read(buffer, read, count - read);

                if (chunk == 0)
                {
                    break;
                }

                read += chunk;
            }

            if (read < count)
            {
                Array.Resize(ref buffer, read);
            }

            return buffer;
        }

        private static string ReadNullTerminatedAscii(byte[] data, int offset, int maxLength)
        {
            int end = offset;

            while (end < data.Length && end - offset < maxLength && data[end] != 0)
            {
                end++;
            }

            if (end == offset)
            {
                return string.Empty;
            }

            return Encoding.ASCII.GetString(data, offset, end - offset);
        }

        private static ushort ReadUInt16(byte[] data, int offset)
        {
            return (ushort)(data[offset] | (data[offset + 1] << 8));
        }

        private static uint ReadUInt32(byte[] data, int offset)
        {
            return (uint)(data[offset]
                | (data[offset + 1] << 8)
                | (data[offset + 2] << 16)
                | (data[offset + 3] << 24));
        }

        private static ulong ReadUInt64(byte[] data, int offset)
        {
            return ReadUInt32(data, offset) | ((ulong)ReadUInt32(data, offset + 4) << 32);
        }

        private static int ReadInt32(byte[] data, int offset)
        {
            return (int)ReadUInt32(data, offset);
        }

        private static string LookupName(Dictionary<ushort, string> table, ushort key, string fallback)
        {
            string name;

            if (table.TryGetValue(key, out name))
            {
                return name;
            }

            return fallback;
        }

        private static string FormatTimestamp(uint timestamp)
        {
            try
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime utc = epoch.AddSeconds(timestamp);
                return utc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
                    + " (UTC " + utc.ToString("yyyy-MM-dd HH:mm:ss") + ")";
            }
            catch (Exception)
            {
                return timestamp.ToString();
            }
        }
    }
}
