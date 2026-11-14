using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileExplorer.Services
{
    public class SearchOptions
    {
        public string RootPath = string.Empty;
        public string NamePattern = "*";
        public bool SearchContents;
        public string ContentText = string.Empty;
        public bool MatchCase;
        public int MaxResults = 5000;
    }

    public class SearchResult
    {
        public string Path = string.Empty;
        public string Name = string.Empty;
        public string Directory = string.Empty;
        public long Length;
        public DateTime Modified;
        public int LineNumber;
        public string LinePreview = string.Empty;
    }

    public static class SearchService
    {
        private const long MaxContentSearchBytes = 10 * 1024 * 1024;
        private const int MaxMatchesPerFile = 20;

        public static List<SearchResult> Search(
            SearchOptions options,
            Action<SearchResult> resultCallback,
            Func<bool> cancelRequested)
        {
            var results = new List<SearchResult>();

            if (options == null || string.IsNullOrEmpty(options.RootPath)
                || !Directory.Exists(options.RootPath))
            {
                return results;
            }

            string[] patterns = SplitPatterns(options.NamePattern);
            string content = options.SearchContents ? (options.ContentText ?? string.Empty) : null;

            if (options.SearchContents && content.Length == 0)
            {
                content = null;
            }

            StringComparison comparison = options.MatchCase
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            var pending = new Stack<string>();
            pending.Push(options.RootPath);

            while (pending.Count > 0)
            {
                if (cancelRequested != null && cancelRequested())
                {
                    break;
                }

                if (results.Count >= options.MaxResults)
                {
                    break;
                }

                string directory = pending.Pop();

                try
                {
                    foreach (string subDirectory in SafeGetDirectories(directory))
                    {
                        pending.Push(subDirectory);
                    }
                }
                catch (Exception)
                {
                }

                List<string> files;

                try
                {
                    files = new List<string>(Directory.EnumerateFiles(directory));
                }
                catch (Exception)
                {
                    continue;
                }

                foreach (string file in files)
                {
                    if (cancelRequested != null && cancelRequested())
                    {
                        break;
                    }

                    if (results.Count >= options.MaxResults)
                    {
                        break;
                    }

                    string name = Path.GetFileName(file);

                    if (patterns != null && !MatchesAnyPattern(name, patterns, comparison))
                    {
                        continue;
                    }

                    FileInfo info;

                    try
                    {
                        info = new FileInfo(file);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (content == null)
                    {
                        var nameResult = new SearchResult
                        {
                            Path = file,
                            Name = name,
                            Directory = directory,
                            Length = info.Length,
                            Modified = info.LastWriteTime,
                            LineNumber = 0,
                            LinePreview = string.Empty
                        };

                        results.Add(nameResult);

                        if (resultCallback != null)
                        {
                            resultCallback(nameResult);
                        }

                        continue;
                    }

                    if (info.Length == 0 || info.Length > MaxContentSearchBytes)
                    {
                        continue;
                    }

                    foreach (SearchResult contentResult in SearchInFile(file, info, content, comparison, options))
                    {
                        results.Add(contentResult);

                        if (resultCallback != null)
                        {
                            resultCallback(contentResult);
                        }

                        if (results.Count >= options.MaxResults)
                        {
                            break;
                        }
                    }
                }
            }

            return results;
        }

        private static IEnumerable<SearchResult> SearchInFile(
            string file,
            FileInfo info,
            string content,
            StringComparison comparison,
            SearchOptions options)
        {
            var matches = new List<SearchResult>();

            FileStream stream;

            try
            {
                stream = new FileStream(
                    file,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite);
            }
            catch (Exception)
            {
                return matches;
            }

            try
            {
                if (LooksBinary(stream))
                {
                    return matches;
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
                {
                    string line;
                    int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;

                        if (line.IndexOf(content, comparison) >= 0)
                        {
                            matches.Add(new SearchResult
                            {
                                Path = file,
                                Name = info.Name,
                                Directory = info.DirectoryName,
                                Length = info.Length,
                                Modified = info.LastWriteTime,
                                LineNumber = lineNumber,
                                LinePreview = TrimPreview(line)
                            });

                            if (matches.Count >= MaxMatchesPerFile)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                stream.Dispose();
            }

            return matches;
        }

        private static bool LooksBinary(FileStream stream)
        {
            byte[] buffer = new byte[4096];
            int read = stream.Read(buffer, 0, buffer.Length);

            for (int i = 0; i < read; i++)
            {
                if (buffer[i] == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static string TrimPreview(string line)
        {
            string trimmed = line.Trim();

            if (trimmed.Length > 200)
            {
                trimmed = trimmed.Substring(0, 200) + "...";
            }

            return trimmed;
        }

        private static string[] SplitPatterns(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern) || pattern.Trim() == "*")
            {
                return null;
            }

            string[] parts = pattern.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var cleaned = new List<string>();

            foreach (string part in parts)
            {
                string candidate = part.Trim();

                if (candidate.Length > 0)
                {
                    cleaned.Add(candidate);
                }
            }

            return cleaned.Count == 0 ? null : cleaned.ToArray();
        }

        private static bool MatchesAnyPattern(string name, string[] patterns, StringComparison comparison)
        {
            foreach (string pattern in patterns)
            {
                if (WildcardMatch(name, pattern, comparison))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool WildcardMatch(string text, string pattern, StringComparison comparison)
        {
            if (string.Equals(pattern, "*", StringComparison.Ordinal))
            {
                return true;
            }

            return WildcardMatchCore(text, pattern, comparison);
        }

        private static bool WildcardMatchCore(string text, string pattern, StringComparison comparison)
        {
            // Iterative matcher with backtracking support for '*'.
            int textIndex = 0;
            int patternIndex = 0;
            int starIndex = -1;
            int textAfterStar = -1;

            while (textIndex < text.Length)
            {
                if (patternIndex < pattern.Length
                    && (pattern[patternIndex] == '?'
                        || string.Compare(text, textIndex, pattern, patternIndex, 1, comparison) == 0))
                {
                    textIndex++;
                    patternIndex++;
                }
                else if (patternIndex < pattern.Length && pattern[patternIndex] == '*')
                {
                    starIndex = patternIndex;
                    textAfterStar = textIndex;
                    patternIndex++;
                }
                else if (starIndex >= 0)
                {
                    patternIndex = starIndex + 1;
                    textAfterStar++;
                    textIndex = textAfterStar;
                }
                else
                {
                    return false;
                }
            }

            while (patternIndex < pattern.Length && pattern[patternIndex] == '*')
            {
                patternIndex++;
            }

            return patternIndex == pattern.Length;
        }

        private static IEnumerable<string> SafeGetDirectories(string directory)
        {
            try
            {
                return Directory.EnumerateDirectories(directory);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }
    }
}
