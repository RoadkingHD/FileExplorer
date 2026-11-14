using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FileExplorer.Services;

namespace FileExplorer.Dialogs
{
    public partial class CompareDialog : Form
    {
        private const int MaxDiffBytes = 4 * 1024 * 1024;
        private const int MaxHexBytes = 1024 * 1024;
        private const int MaxFolderEntries = 20000;

        private static readonly Color EqualColor = Color.FromArgb(96, 96, 96);
        private static readonly Color DeletedColor = Color.FromArgb(180, 0, 0);
        private static readonly Color InsertedColor = Color.FromArgb(0, 128, 0);
        private static readonly Color DifferenceHighlight = Color.FromArgb(255, 224, 192);

        private bool _comparing;

        public CompareDialog()
            : this(null, null)
        {
        }

        public CompareDialog(string leftPath, string rightPath)
        {
            InitializeComponent();

            textBoxLeft.Text = leftPath ?? string.Empty;
            textBoxRight.Text = rightPath ?? string.Empty;
        }

        private void BtnBrowseLeft_Click(object sender, EventArgs e)
        {
            BrowseTo(textBoxLeft);
        }

        private void BtnBrowseRight_Click(object sender, EventArgs e)
        {
            BrowseTo(textBoxRight);
        }

        private void BrowseTo(TextBox target)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a file or type a folder path";
                dialog.CheckFileExists = false;
                dialog.ValidateNames = false;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    target.Text = dialog.FileName;
                }
            }
        }

        private void BtnCompare_Click(object sender, EventArgs e)
        {
            if (_comparing)
            {
                return;
            }

            string left = NormalizePath(textBoxLeft.Text);
            string right = NormalizePath(textBoxRight.Text);

            if (left == null || right == null)
            {
                MessageBox.Show(
                    this,
                    "Please choose two existing files or two existing folders.",
                    "Compare",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool leftIsDirectory = Directory.Exists(left);
            bool rightIsDirectory = Directory.Exists(right);

            if (leftIsDirectory != rightIsDirectory)
            {
                MessageBox.Show(
                    this,
                    "Please compare two files or two folders. A file cannot be compared with a folder.",
                    "Compare",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (leftIsDirectory)
            {
                CompareFolders(left, right);
            }
            else
            {
                CompareFiles(left, right);
            }
        }

        private static string NormalizePath(string text)
        {
            string trimmed = (text ?? string.Empty).Trim().Trim('"');

            if (trimmed.Length == 0)
            {
                return null;
            }

            if (Directory.Exists(trimmed) || File.Exists(trimmed))
            {
                return Path.GetFullPath(trimmed);
            }

            return null;
        }

        private void SetBusy(bool busy, string message)
        {
            _comparing = busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            btnCompare.Enabled = !busy;
            lblSummary.Text = message ?? string.Empty;
        }

        private void ShowOnly(Control control)
        {
            listViewResults.Visible = control == listViewResults;
            richTextBoxDiff.Visible = control == richTextBoxDiff;
            panelHex.Visible = control == panelHex;
        }

        private void CompareFolders(string left, string right)
        {
            SetBusy(true, "Comparing folders...");
            listViewResults.Items.Clear();
            ShowOnly(listViewResults);

            string leftRoot = left;
            string rightRoot = right;

            var thread = new Thread(() =>
            {
                List<string[]> rows;

                try
                {
                    rows = BuildFolderComparison(leftRoot, rightRoot);
                }
                catch (Exception ex)
                {
                    rows = new List<string[]> { new[] { "Error", ex.Message, string.Empty, string.Empty } };
                }

                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        SetBusy(false, null);
                        PopulateFolderResults(rows);

                        int different = 0;
                        int onlyLeft = 0;
                        int onlyRight = 0;
                        int same = 0;

                        foreach (string[] row in rows)
                        {
                            if (row[0] == "Different")
                            {
                                different++;
                            }
                            else if (row[0] == "Only in left")
                            {
                                onlyLeft++;
                            }
                            else if (row[0] == "Only in right")
                            {
                                onlyRight++;
                            }
                            else if (row[0] == "Same")
                            {
                                same++;
                            }
                        }

                        lblSummary.Text = rows.Count + " item(s): "
                            + different + " different, "
                            + onlyLeft + " only in left, "
                            + onlyRight + " only in right, "
                            + same + " identical.";
                    }));
                }
                catch (InvalidOperationException)
                {
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private List<string[]> BuildFolderComparison(string leftRoot, string rightRoot)
        {
            var rows = new List<string[]>();
            var leftFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var rightFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var leftDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var rightDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            CollectEntries(leftRoot, string.Empty, leftFiles, leftDirs);
            CollectEntries(rightRoot, string.Empty, rightFiles, rightDirs);

            var allPaths = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string path in leftFiles.Keys)
            {
                allPaths.Add(path);
            }

            foreach (string path in rightFiles.Keys)
            {
                allPaths.Add(path);
            }

            foreach (string path in allPaths)
            {
                if (rows.Count >= MaxFolderEntries)
                {
                    rows.Add(new[] { "Truncated", "... (result limit reached)", string.Empty, string.Empty });
                    break;
                }

                bool inLeft = leftFiles.ContainsKey(path);
                bool inRight = rightFiles.ContainsKey(path);
                string leftPath = inLeft ? leftFiles[path] : null;
                string rightPath = inRight ? rightFiles[path] : null;

                string status;
                string sizeLeft = string.Empty;
                string sizeRight = string.Empty;

                if (inLeft && inRight)
                {
                    bool equal = DiffService.FilesHaveEqualContent(leftPath, rightPath);
                    status = equal ? "Same" : "Different";
                    sizeLeft = FormatSize(new FileInfo(leftPath).Length);
                    sizeRight = FormatSize(new FileInfo(rightPath).Length);
                }
                else if (inLeft)
                {
                    status = "Only in left";
                    sizeLeft = FormatSize(new FileInfo(leftPath).Length);
                }
                else
                {
                    status = "Only in right";
                    sizeRight = FormatSize(new FileInfo(rightPath).Length);
                }

                rows.Add(new[]
                {
                    status,
                    path,
                    sizeLeft,
                    sizeRight,
                    leftPath ?? string.Empty,
                    rightPath ?? string.Empty
                });
            }

            var allDirs = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string path in leftDirs)
            {
                allDirs.Add(path);
            }

            foreach (string path in rightDirs)
            {
                allDirs.Add(path);
            }

            foreach (string path in allDirs)
            {
                bool inLeft = leftDirs.Contains(path);
                bool inRight = rightDirs.Contains(path);

                if (inLeft && inRight)
                {
                    continue;
                }

                rows.Add(new[]
                {
                    inLeft ? "Only in left" : "Only in right",
                    path + Path.DirectorySeparatorChar,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                });
            }

            return rows;
        }

        private void PopulateFolderResults(List<string[]> rows)
        {
            listViewResults.BeginUpdate();

            foreach (string[] row in rows)
            {
                var item = new ListViewItem(row[0]);
                item.SubItems.Add(row[1]);
                item.SubItems.Add(row[2]);
                item.SubItems.Add(row[3]);
                item.Tag = new[] { row.Length > 4 ? row[4] : string.Empty, row.Length > 5 ? row[5] : string.Empty };

                switch (row[0])
                {
                    case "Different":
                        item.ForeColor = Color.Firebrick;
                        break;
                    case "Only in left":
                    case "Only in right":
                        item.ForeColor = Color.DarkGoldenrod;
                        break;
                    case "Same":
                        item.ForeColor = Color.Gray;
                        break;
                }

                listViewResults.Items.Add(item);
            }

            listViewResults.EndUpdate();
        }

        private void CollectEntries(
            string root,
            string relative,
            Dictionary<string, string> files,
            HashSet<string> directories)
        {
            string current = relative.Length == 0 ? root : Path.Combine(root, relative);

            foreach (string directory in SafeEnumerateDirectories(current))
            {
                string childRelative = relative.Length == 0
                    ? Path.GetFileName(directory)
                    : relative + Path.DirectorySeparatorChar + Path.GetFileName(directory);

                directories.Add(childRelative);
                CollectEntries(root, childRelative, files, directories);
            }

            foreach (string file in SafeEnumerateFiles(current))
            {
                string fileRelative = relative.Length == 0
                    ? Path.GetFileName(file)
                    : relative + Path.DirectorySeparatorChar + Path.GetFileName(file);

                if (!files.ContainsKey(fileRelative))
                {
                    files.Add(fileRelative, file);
                }
            }
        }

        private static IEnumerable<string> SafeEnumerateDirectories(string path)
        {
            try
            {
                return Directory.EnumerateDirectories(path);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private static IEnumerable<string> SafeEnumerateFiles(string path)
        {
            try
            {
                return Directory.EnumerateFiles(path);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private void CompareFiles(string left, string right)
        {
            SetBusy(true, "Comparing files...");

            var thread = new Thread(() =>
            {
                try
                {
                    CompareFilesBackground(left, right);
                }
                catch (Exception ex)
                {
                    try
                    {
                        BeginInvoke((MethodInvoker)(() =>
                        {
                            SetBusy(false, null);
                            ShowOnly(richTextBoxDiff);
                            richTextBoxDiff.Text = "Cannot compare files.\r\n\r\n" + ex.Message;
                        }));
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void CompareFilesBackground(string left, string right)
        {
            byte[] leftBytes = ReadBytes(left, MaxDiffBytes);
            byte[] rightBytes = ReadBytes(right, MaxDiffBytes);

            bool binary = DiffService.LooksBinary(leftBytes, leftBytes.Length)
                || DiffService.LooksBinary(rightBytes, rightBytes.Length);

            if (binary)
            {
                ShowBinaryComparison(left, right, leftBytes, rightBytes);
            }
            else
            {
                ShowTextComparison(left, right, leftBytes, rightBytes);
            }
        }

        private void ShowTextComparison(string left, string right, byte[] leftBytes, byte[] rightBytes)
        {
            string[] leftLines = SplitLines(leftBytes);
            string[] rightLines = SplitLines(rightBytes);
            List<DiffLine> diff = DiffService.Diff(leftLines, rightLines);

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    SetBusy(false, null);
                    ShowOnly(richTextBoxDiff);
                    RenderTextDiff(diff);
                    lblSummary.Text = BuildTextSummary(left, right, leftLines.Length, rightLines.Length, diff);
                }));
            }
            catch (InvalidOperationException)
            {
            }
        }

        private string BuildTextSummary(
            string left,
            string right,
            int leftLines,
            int rightLines,
            List<DiffLine> diff)
        {
            int deleted = 0;
            int inserted = 0;

            foreach (DiffLine line in diff)
            {
                if (line.Operation == DiffOperation.Delete)
                {
                    deleted++;
                }
                else if (line.Operation == DiffOperation.Insert)
                {
                    inserted++;
                }
            }

            if (deleted == 0 && inserted == 0)
            {
                return "The text files are identical (" + leftLines + " lines each).";
            }

            return Path.GetFileName(left) + " (" + leftLines + " lines) vs "
                + Path.GetFileName(right) + " (" + rightLines + " lines): "
                + deleted + " line(s) only in left, " + inserted + " line(s) only in right.";
        }

        private void RenderTextDiff(List<DiffLine> diff)
        {
            richTextBoxDiff.Clear();

            foreach (DiffLine line in diff)
            {
                string prefix;
                Color color;

                switch (line.Operation)
                {
                    case DiffOperation.Delete:
                        prefix = "- ";
                        color = DeletedColor;
                        break;
                    case DiffOperation.Insert:
                        prefix = "+ ";
                        color = InsertedColor;
                        break;
                    default:
                        prefix = "  ";
                        color = EqualColor;
                        break;
                }

                AppendDiffText(prefix, color);
                AppendDiffText(line.Text + "\r\n", color);
            }

            richTextBoxDiff.SelectionStart = 0;
            richTextBoxDiff.SelectionLength = 0;
        }

        private void AppendDiffText(string text, Color color)
        {
            richTextBoxDiff.SelectionStart = richTextBoxDiff.TextLength;
            richTextBoxDiff.SelectionLength = 0;
            richTextBoxDiff.SelectionColor = color;
            richTextBoxDiff.AppendText(text);
        }

        private void ShowBinaryComparison(string left, string right, byte[] leftBytes, byte[] rightBytes)
        {
            long firstDifference = -1;
            int limit = Math.Min(leftBytes.Length, rightBytes.Length);

            for (int i = 0; i < limit; i++)
            {
                if (leftBytes[i] != rightBytes[i])
                {
                    firstDifference = i;
                    break;
                }
            }

            if (firstDifference < 0 && leftBytes.Length != rightBytes.Length)
            {
                firstDifference = limit;
            }

            string leftDump = DiffService.FormatHexDump(leftBytes, Math.Min(leftBytes.Length, MaxHexBytes), 0);
            string rightDump = DiffService.FormatHexDump(rightBytes, Math.Min(rightBytes.Length, MaxHexBytes), 0);

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    SetBusy(false, null);
                    ShowOnly(panelHex);

                    labelHexLeft.Text = Path.GetFileName(left) + " (" + FormatSize(leftBytes.Length) + ")";
                    labelHexRight.Text = Path.GetFileName(right) + " (" + FormatSize(rightBytes.Length) + ")";

                    richTextBoxHexLeft.Text = leftDump;
                    richTextBoxHexRight.Text = rightDump;
                    HighlightHexDifferences(richTextBoxHexLeft, leftDump, rightDump);
                    HighlightHexDifferences(richTextBoxHexRight, rightDump, leftDump);

                    if (firstDifference < 0)
                    {
                        lblSummary.Text = "The binary files are identical.";
                    }
                    else
                    {
                        lblSummary.Text = "The binary files differ. First difference at offset 0x"
                            + firstDifference.ToString("X") + " (decimal " + firstDifference + ").";
                    }
                }));
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void HighlightHexDifferences(RichTextBox target, string ownDump, string otherDump)
        {
            string[] ownLines = ownDump.Split('\n');
            string[] otherLines = otherDump.Split('\n');
            int offset = 0;


            for (int i = 0; i < ownLines.Length; i++)
            {
                string line = ownLines[i];
                int lineLength = line.Length;
                bool differs = i >= otherLines.Length
                    || !string.Equals(line.TrimEnd('\r'), otherLines[i].TrimEnd('\r'), StringComparison.Ordinal);

                if (differs && lineLength > 0)
                {
                    target.SelectionStart = offset;
                    target.SelectionLength = lineLength;
                    target.SelectionBackColor = DifferenceHighlight;
                }

                offset += lineLength + 1;
            }

            target.SelectionStart = 0;
            target.SelectionLength = 0;
        }

        private void ListViewResults_DoubleClick(object sender, EventArgs e)
        {
            if (listViewResults.SelectedItems.Count == 0)
            {
                return;
            }

            var paths = listViewResults.SelectedItems[0].Tag as string[];

            if (paths == null || paths.Length < 2)
            {
                return;
            }

            if (paths[0].Length > 0 && paths[1].Length > 0
                && File.Exists(paths[0]) && File.Exists(paths[1]))
            {
                textBoxLeft.Text = paths[0];
                textBoxRight.Text = paths[1];
                CompareFiles(paths[0], paths[1]);
            }
        }

        private static byte[] ReadBytes(string path, int maxBytes)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] buffer = new byte[maxBytes];
                int read = 0;

                while (read < maxBytes)
                {
                    int chunk = stream.Read(buffer, read, maxBytes - read);

                    if (chunk == 0)
                    {
                        break;
                    }

                    read += chunk;
                }

                if (read < maxBytes)
                {
                    Array.Resize(ref buffer, read);
                }

                return buffer;
            }
        }

        private static string[] SplitLines(byte[] data)
        {
            string text = Encoding.UTF8.GetString(data);
            return text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        }

        private static string FormatSize(long length)
        {
            if (length < 1024L)
            {
                return length + " B";
            }

            double size = length;

            if (size < 1024d * 1024d)
            {
                return (size / 1024d).ToString("0.#") + " KB";
            }

            if (size < 1024d * 1024d * 1024d)
            {
                return (size / (1024d * 1024d)).ToString("0.#") + " MB";
            }

            return (size / (1024d * 1024d * 1024d)).ToString("0.#") + " GB";
        }
    }
}
