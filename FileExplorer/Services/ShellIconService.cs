using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace FileExplorer.Services
{
    public static class ShellIconService
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_LARGEICON = 0x000000000;
        private const uint SHGFI_SMALLICON = 0x000000001;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
        private const uint SHGFI_TYPENAME = 0x000000400;

        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

        private static readonly object SyncRoot = new object();
        private static readonly Dictionary<string, Icon> SmallIconCache = new Dictionary<string, Icon>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, Icon> LargeIconCache = new Dictionary<string, Icon>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, string> TypeNameCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static Icon GetSmallIcon(string path, bool isDirectory)
        {
            return GetIcon("s:", path, isDirectory, true);
        }

        public static Icon GetLargeIcon(string path, bool isDirectory)
        {
            return GetIcon("l:", path, isDirectory, false);
        }

        public static string GetTypeDescription(string path)
        {
            string key = path;
            lock (SyncRoot)
            {
                string cached;
                if (TypeNameCache.TryGetValue(key, out cached))
                {
                    return cached;
                }
            }

            var info = new SHFILEINFO();
            IntPtr result = SHGetFileInfo(path, FILE_ATTRIBUTE_NORMAL, ref info, (uint)Marshal.SizeOf(typeof(SHFILEINFO)), SHGFI_TYPENAME | SHGFI_USEFILEATTRIBUTES);
            string typeName = result != IntPtr.Zero && !string.IsNullOrEmpty(info.szTypeName) ? info.szTypeName : (Path.GetExtension(path) + " file").TrimStart(' ');

            lock (SyncRoot)
            {
                TypeNameCache[key] = typeName;
            }

            return typeName;
        }

        private static Icon GetIcon(string prefix, string path, bool isDirectory, bool small)
        {
            string cacheKey = prefix + (isDirectory ? "dir:" : "file:") + path;
            Dictionary<string, Icon> cache = small ? SmallIconCache : LargeIconCache;

            lock (SyncRoot)
            {
                Icon cached;
                if (cache.TryGetValue(cacheKey, out cached))
                {
                    return cached;
                }
            }

            uint flags = SHGFI_ICON | (small ? SHGFI_SMALLICON : SHGFI_LARGEICON);
            uint attributes = FILE_ATTRIBUTE_NORMAL;

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                flags |= SHGFI_USEFILEATTRIBUTES;
                if (isDirectory)
                {
                    attributes = FILE_ATTRIBUTE_DIRECTORY;
                }
            }

            var info = new SHFILEINFO();
            Icon icon = null;

            try
            {
                IntPtr result = SHGetFileInfo(path, attributes, ref info, (uint)Marshal.SizeOf(typeof(SHFILEINFO)), flags);
                if (result != IntPtr.Zero && info.hIcon != IntPtr.Zero)
                {
                    icon = (Icon)Icon.FromHandle(info.hIcon).Clone();
                }
            }
            finally
            {
                if (info.hIcon != IntPtr.Zero)
                {
                    DestroyIcon(info.hIcon);
                }
            }

            if (icon == null)
            {
                icon = SystemIcons.Application;
            }

            lock (SyncRoot)
            {
                cache[cacheKey] = icon;
            }

            return icon;
        }
    }
}
