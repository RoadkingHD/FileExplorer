using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileExplorer.Services
{
    public enum DiffOperation
    {
        Equal,
        Delete,
        Insert
    }

    public class DiffLine
    {
        public DiffOperation Operation;
        public string Text = string.Empty;
        public int LeftNumber;
        public int RightNumber;
    }

    public static class DiffService
    {
        private const int MaxDiffLines = 5000;

        public static List<DiffLine> Diff(string[] leftLines, string[] rightLines)
        {
            var result = new List<DiffLine>();

            if (leftLines == null)
            {
                leftLines = new string[0];
            }

            if (rightLines == null)
            {
                rightLines = new string[0];
            }

            int start = 0;
            int leftEnd = leftLines.Length;
            int rightEnd = rightLines.Length;

            // Trim common prefix and suffix so the LCS table stays small.
            while (start < leftEnd && start < rightEnd
                && string.Equals(leftLines[start], rightLines[start], StringComparison.Ordinal))
            {
                start++;
            }

            while (leftEnd > start && rightEnd > start
                && string.Equals(leftLines[leftEnd - 1], rightLines[rightEnd - 1], StringComparison.Ordinal))
            {
                leftEnd--;
                rightEnd--;
            }

            for (int i = 0; i < start; i++)
            {
                result.Add(MakeLine(DiffOperation.Equal, leftLines[i], i + 1, i + 1));
            }

            int leftLength = leftEnd - start;
            int rightLength = rightEnd - start;

            if (leftLength == 0 && rightLength == 0)
            {
                return result;
            }

            if (leftLength > MaxDiffLines || rightLength > MaxDiffLines)
            {
                // Lines are too many for a full diff; report the difference in bulk.
                for (int i = start; i < leftEnd; i++)
                {
                    result.Add(MakeLine(DiffOperation.Delete, leftLines[i], i + 1, 0));
                }

                for (int i = start; i < rightEnd; i++)
                {
                    result.Add(MakeLine(DiffOperation.Insert, rightLines[i], 0, i + 1));
                }
            }
            else
            {
                int[,] table = BuildLcsTable(leftLines, rightLines, start, leftEnd, rightEnd);
                EmitDiff(result, table, leftLines, rightLines, start, leftEnd, rightEnd);
            }

            // Append the common suffix.
            for (int i = leftEnd; i < leftLines.Length; i++)
            {
                result.Add(MakeLine(DiffOperation.Equal, leftLines[i], i + 1, i + 1));
            }

            return result;
        }

        private static DiffLine MakeLine(DiffOperation operation, string text, int leftNumber, int rightNumber)
        {
            return new DiffLine
            {
                Operation = operation,
                Text = text,
                LeftNumber = leftNumber,
                RightNumber = rightNumber
            };
        }

        private static int[,] BuildLcsTable(string[] left, string[] right, int start, int leftEnd, int rightEnd)
        {
            int rows = leftEnd - start;
            int columns = rightEnd - start;
            var table = new int[rows + 1, columns + 1];

            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = columns - 1; j >= 0; j--)
                {
                    if (string.Equals(left[start + i], right[start + j], StringComparison.Ordinal))
                    {
                        table[i, j] = table[i + 1, j + 1] + 1;
                    }
                    else
                    {
                        table[i, j] = Math.Max(table[i + 1, j], table[i, j + 1]);
                    }
                }
            }

            return table;
        }

        private static void EmitDiff(
            List<DiffLine> result,
            int[,] table,
            string[] left,
            string[] right,
            int start,
            int leftEnd,
            int rightEnd)
        {
            int i = 0;
            int j = 0;
            int rows = leftEnd - start;
            int columns = rightEnd - start;

            while (i < rows && j < columns)
            {
                if (string.Equals(left[start + i], right[start + j], StringComparison.Ordinal))
                {
                    result.Add(MakeLine(DiffOperation.Equal, left[start + i], start + i + 1, start + j + 1));
                    i++;
                    j++;
                }
                else if (table[i + 1, j] >= table[i, j + 1])
                {
                    result.Add(MakeLine(DiffOperation.Delete, left[start + i], start + i + 1, 0));
                    i++;
                }
                else
                {
                    result.Add(MakeLine(DiffOperation.Insert, right[start + j], 0, start + j + 1));
                    j++;
                }
            }

            while (i < rows)
            {
                result.Add(MakeLine(DiffOperation.Delete, left[start + i], start + i + 1, 0));
                i++;
            }

            while (j < columns)
            {
                result.Add(MakeLine(DiffOperation.Insert, right[start + j], 0, start + j + 1));
                j++;
            }
        }

        public static bool LooksBinary(byte[] data, int length)
        {
            int limit = Math.Min(length, 8192);

            for (int i = 0; i < limit; i++)
            {
                if (data[i] == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static string FormatHexDump(byte[] data, int length, long baseOffset)
        {
            var builder = new StringBuilder();
            int offset = 0;

            while (offset < length)
            {
                builder.Append((baseOffset + offset).ToString("X8"));
                builder.Append("  ");

                var ascii = new StringBuilder();

                for (int i = 0; i < 16; i++)
                {
                    if (offset + i < length)
                    {
                        byte value = data[offset + i];
                        builder.Append(value.ToString("X2"));
                        ascii.Append(value >= 0x20 && value < 0x7F ? (char)value : '.');
                    }
                    else
                    {
                        builder.Append("  ");
                        ascii.Append(' ');
                    }

                    if (i == 7)
                    {
                        builder.Append(' ');
                    }
                    else if (i < 15)
                    {
                        builder.Append(' ');
                    }
                }

                builder.Append(" |");
                builder.Append(ascii);
                builder.Append('|');
                builder.AppendLine();
                offset += 16;
            }

            return builder.ToString();
        }

        public static bool FilesHaveEqualContent(string leftPath, string rightPath)
        {
            try
            {
                var leftInfo = new FileInfo(leftPath);
                var rightInfo = new FileInfo(rightPath);

                if (leftInfo.Length != rightInfo.Length)
                {
                    return false;
                }

                using (FileStream left = leftInfo.OpenRead())
                using (FileStream right = rightInfo.OpenRead())
                {
                    byte[] leftBuffer = new byte[64 * 1024];
                    byte[] rightBuffer = new byte[64 * 1024];

                    while (true)
                    {
                        int leftRead = ReadBlock(left, leftBuffer);
                        int rightRead = ReadBlock(right, rightBuffer);

                        if (leftRead != rightRead)
                        {
                            return false;
                        }

                        if (leftRead == 0)
                        {
                            return true;
                        }

                        for (int i = 0; i < leftRead; i++)
                        {
                            if (leftBuffer[i] != rightBuffer[i])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int ReadBlock(FileStream stream, byte[] buffer)
        {
            int read = 0;

            while (read < buffer.Length)
            {
                int chunk = stream.Read(buffer, read, buffer.Length - read);

                if (chunk == 0)
                {
                    break;
                }

                read += chunk;
            }

            return read;
        }
    }
}
