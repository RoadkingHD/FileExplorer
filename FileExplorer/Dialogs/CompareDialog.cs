using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FileExplorer.Controls;
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
        private static readonly Color FolderDifferentColor = Color.Firebrick;
        private static readonly Color FolderOnlyColor = Color.DarkGoldenrod;
        private static readonly Color FolderSameColor = Color.Gray;
        private static readonly Color MissingColor = Color.FromArgb(170, 170, 170);

        private bool _comparing;
        private bool _syncingScroll;
        private bool _syncingTree;
        private List<string[]> _folderRows;
        private string _leftRoot = string.Empty;
        private string _rightRoot = string.Empty;

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

            btnBack.Visible = false;

            if (leftIsDirectory)
            {
                CompareFolders(left, right);
            }
            else
            {
                CompareFiles(left, right);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (_folderRows != null)
            {
                btnBack.Visible = false;
                ShowOnly(splitContainerFolders);
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
            splitContainerFolders.Visible = control == splitContainerFolders;
            splitContainerDiff.Visible = control == splitContainerDiff;
            panelHex.Visible = control == panelHex;
        }

        #region Folder comparison

        private void CompareFolders(string left, string right)
        {
            SetBusy(true, "Comparing folders...");
            treeViewFoldersLeft.Nodes.Clear();
            treeViewFoldersRight.Nodes.Clear();
            _folderRows = null;
            _leftRoot = left;
            _rightRoot = right;
            ShowOnly(splitContainerFolders);

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
                        _folderRows = rows;
                        SetBusy(false, null);
                        RepopulateFolderViews();

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

        private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_folderRows != null && !_comparing)
            {
                RepopulateFolderViews();
            }
        }

        private List<string[]> GetFilteredRows()
        {
            if (_folderRows == null)
            {
                return new List<string[]>();
            }

            string filter = comboBoxFilter.SelectedItem as string;

            if (filter == null || filter == "All")
            {
                return _folderRows;
            }

            var rows = new List<string[]>();

            foreach (string[] row in _folderRows)
            {
                string status = row[0];

                if (filter == "Differences only")
                {
                    if (status == "Different" || status == "Only in left" || status == "Only in right"
                        || status == "Error" || status == "Truncated")
                    {
                        rows.Add(row);
                    }
                }
                else if (filter == "Only in left")
                {
                    if (status == "Only in left")
                    {
                        rows.Add(row);
                    }
                }
                else if (filter == "Only in right")
                {
                    if (status == "Only in right")
                    {
                        rows.Add(row);
                    }
                }
            }

            return rows;
        }

        private void RepopulateFolderViews()
        {
            BuildSideBySideTrees(GetFilteredRows());
        }

        private static Color StatusColor(string status)
        {
            switch (status)
            {
                case "Different":
                    return FolderDifferentColor;
                case "Only in left":
                case "Only in right":
                    return FolderOnlyColor;
                case "Same":
                    return FolderSameColor;
                default:
                    return SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Builds a merged tree of the comparison rows. When placeholderSide is
        /// "left" or "right", rows that exist only on the opposite side are shown
        /// as dimmed placeholder nodes (used for the side-by-side views).
        /// </summary>
        private void BuildFolderTree(TreeView target, List<string[]> rows, string placeholderSide)
        {
            target.BeginUpdate();
            target.Nodes.Clear();

            var directoryNodes = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase);
            var directoryStatus = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (string[] row in rows)
            {
                string relativePath = row[1];
                string status = row[0];
                string[] paths = new[]
                {
                    row.Length > 4 ? row[4] : string.Empty,
                    row.Length > 5 ? row[5] : string.Empty
                };

                bool isDirectory = relativePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
                string[] parts = relativePath.TrimEnd(Path.DirectorySeparatorChar)
                    .Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                {
                    continue;
                }

                bool placeholder = (placeholderSide == "left" && status == "Only in right")
                    || (placeholderSide == "right" && status == "Only in left");

                TreeNodeCollection currentNodes = target.Nodes;
                string builtPath = string.Empty;

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    builtPath = builtPath.Length == 0
                        ? parts[i]
                        : builtPath + Path.DirectorySeparatorChar + parts[i];

                    TreeNode directoryNode;

                    if (!directoryNodes.TryGetValue(builtPath, out directoryNode))
                    {
                        directoryNode = new TreeNode(parts[i]);
                        directoryNodes[builtPath] = directoryNode;
                        directoryStatus[builtPath] = 0;
                        currentNodes.Add(directoryNode);
                    }

                    currentNodes = directoryNode.Nodes;
                }

                string name = parts[parts.Length - 1];
                TreeNode leaf;

                if (placeholder)
                {
                    leaf = new TreeNode(name + "  (only in " + (placeholderSide == "left" ? "right" : "left") + ")");
                    leaf.ForeColor = MissingColor;
                    leaf.Tag = null;
                }
                else
                {
                    leaf = new TreeNode(isDirectory ? name + Path.DirectorySeparatorChar : name);
                    leaf.ForeColor = StatusColor(status);
                    leaf.Tag = paths;
                    leaf.ToolTipText = status;
                }

                currentNodes.Add(leaf);

                if (!placeholder)
                {
                    int severity = StatusSeverity(status);

                    if (severity > 0)
                    {
                        string path = builtPath;

                        while (path.Length > 0)
                        {
                            int existing;

                            if (!directoryStatus.TryGetValue(path, out existing) || severity > existing)
                            {
                                directoryStatus[path] = severity;
                            }

                            int separatorIndex = path.LastIndexOf(Path.DirectorySeparatorChar);
                            path = separatorIndex >= 0 ? path.Substring(0, separatorIndex) : string.Empty;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, TreeNode> pair in directoryNodes)
            {
                int severity;

                if (directoryStatus.TryGetValue(pair.Key, out severity))
                {
                    if (severity == 2)
                    {
                        pair.Value.ForeColor = FolderDifferentColor;
                    }
                    else if (severity == 1)
                    {
                        pair.Value.ForeColor = FolderOnlyColor;
                    }
                    else
                    {
                        pair.Value.ForeColor = FolderSameColor;
                    }
                }
            }

            target.EndUpdate();
        }

        private void BuildSideBySideTrees(List<string[]> rows)
        {
            labelFoldersLeft.Text = _leftRoot;
            labelFoldersRight.Text = _rightRoot;

            BuildFolderTree(treeViewFoldersLeft, rows, "left");
            BuildFolderTree(treeViewFoldersRight, rows, "right");

            foreach (TreeNode node in treeViewFoldersLeft.Nodes)
            {
                node.Expand();
            }

            foreach (TreeNode node in treeViewFoldersRight.Nodes)
            {
                node.Expand();
            }
        }

        private static int StatusSeverity(string status)
        {
            switch (status)
            {
                case "Different":
                    return 2;
                case "Only in left":
                case "Only in right":
                    return 1;
                default:
                    return 0;
            }
        }

        private void FolderTree_DoubleClick(object sender, EventArgs e)
        {
            OpenFromTree(sender as TreeView);
        }

        private void OpenFromTree(TreeView tree)
        {
            if (tree != null)
            {
                OpenCompareFromPaths(tree.SelectedNode == null ? null : tree.SelectedNode.Tag as string[]);
            }
        }

        private void OpenCompareFromPaths(string[] paths)
        {
            if (paths == null || paths.Length < 2)
            {
                return;
            }

            if (paths[0].Length > 0 && paths[1].Length > 0
                && File.Exists(paths[0]) && File.Exists(paths[1]))
            {
                textBoxLeft.Text = paths[0];
                textBoxRight.Text = paths[1];
                btnBack.Visible = true;
                CompareFiles(paths[0], paths[1]);
            }
        }

        #region Side-by-side tree synchronization

        private void FolderTree_Scrolled(object sender, TreeViewScrollEventArgs e)
        {
            if (_syncingScroll)
            {
                return;
            }

            var source = sender as TreeView;

            if (source == null)
            {
                return;
            }

            TreeView target = source == treeViewFoldersLeft ? treeViewFoldersRight : treeViewFoldersLeft;

            _syncingScroll = true;

            try
            {
                if (e.Message == WmHScroll || e.Message == WmMouseHWheel)
                {
                    SendMessage(target.Handle, WmHScroll, e.WParam, IntPtr.Zero);
                }
                else if (source.TopNode != null)
                {
                    target.TopNode = source.TopNode;
                }
            }
            finally
            {
                _syncingScroll = false;
            }
        }

        private void FolderTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            SyncTreeExpansion(sender as TreeView, e.Node, true);
        }

        private void FolderTree_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            SyncTreeExpansion(sender as TreeView, e.Node, false);
        }

        private void SyncTreeExpansion(TreeView source, TreeNode node, bool expand)
        {
            if (_syncingTree || source == null || node == null)
            {
                return;
            }

            TreeView target = source == treeViewFoldersLeft ? treeViewFoldersRight : treeViewFoldersLeft;

            _syncingTree = true;

            try
            {
                TreeNode other = FindNodeByFullPath(target, node.Text);

                if (other != null)
                {
                    if (expand)
                    {
                        other.Expand();
                    }
                    else
                    {
                        other.Collapse();
                    }
                }

                if (source.TopNode != null)
                {
                    target.TopNode = source.TopNode;
                }
            }
            finally
            {
                _syncingTree = false;
            }
        }

        private static TreeNode FindNodeByFullPath(TreeView tree, string fullPath)
        {
            string[] parts = fullPath.Split(new[] { tree.PathSeparator }, StringSplitOptions.None);
            TreeNodeCollection nodes = tree.Nodes;
            TreeNode current = null;

            foreach (string part in parts)
            {
                bool found = false;

                foreach (TreeNode node in nodes)
                {
                    if (string.Equals(NormalizeNodeName(node.Text), part, StringComparison.OrdinalIgnoreCase))
                    {
                        current = node;
                        nodes = node.Nodes;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return null;
                }
            }

            return current;
        }

        private static string NormalizeNodeName(string text)
        {
            int suffixIndex = text.IndexOf("  (", StringComparison.Ordinal);

            if (suffixIndex >= 0)
            {
                text = text.Substring(0, suffixIndex);
            }

            return text.TrimEnd(Path.DirectorySeparatorChar);
        }

        #endregion

        #endregion

        #region File comparison

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
                            ShowOnly(splitContainerDiff);
                            richTextBoxDiffLeft.Text = "Cannot compare files.\r\n\r\n" + ex.Message;
                            richTextBoxDiffRight.Text = string.Empty;
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
            List<DiffLine> diff = DiffService.Diff(leftLines, rightLines, true);

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    SetBusy(false, null);
                    ShowOnly(splitContainerDiff);

                    labelDiffLeft.Text = Path.GetFileName(left) + "  (" + leftLines.Length + " lines)";
                    labelDiffRight.Text = Path.GetFileName(right) + "  (" + rightLines.Length + " lines)";

                    RenderSideBySideDiff(diff);

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
                        lblSummary.Text = "The text files are identical (" + leftLines.Length + " lines each).";
                    }
                    else
                    {
                        lblSummary.Text = Path.GetFileName(left) + " vs " + Path.GetFileName(right) + ": "
                            + deleted + " line(s) only in left, " + inserted + " line(s) only in right.";
                    }
                }));
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void RenderSideBySideDiff(List<DiffLine> diff)
        {
            richTextBoxDiffLeft.Clear();
            richTextBoxDiffRight.Clear();

            foreach (DiffLine line in diff)
            {
                switch (line.Operation)
                {
                    case DiffOperation.Equal:
                        AppendDiffLine(richTextBoxDiffLeft, line.LeftNumber, line.Text, EqualColor);
                        AppendDiffLine(richTextBoxDiffRight, line.RightNumber, line.Text, EqualColor);
                        break;

                    case DiffOperation.Delete:
                        AppendDiffLine(richTextBoxDiffLeft, line.LeftNumber, line.Text, DeletedColor);
                        AppendDiffLine(richTextBoxDiffRight, 0, string.Empty, EqualColor);
                        break;

                    case DiffOperation.Insert:
                        AppendDiffLine(richTextBoxDiffLeft, 0, string.Empty, EqualColor);
                        AppendDiffLine(richTextBoxDiffRight, line.RightNumber, line.Text, InsertedColor);
                        break;
                }
            }

            richTextBoxDiffLeft.SelectionStart = 0;
            richTextBoxDiffLeft.SelectionLength = 0;
            richTextBoxDiffRight.SelectionStart = 0;
            richTextBoxDiffRight.SelectionLength = 0;
        }

        private void AppendDiffLine(RichTextBox box, int lineNumber, string text, Color color)
        {
            string number = lineNumber > 0 ? lineNumber.ToString() : string.Empty;

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = Color.FromArgb(128, 128, 128);
            box.AppendText(number.PadLeft(5) + " | ");

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text + "\r\n");
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

        #endregion

        #region Synchronized scrolling (diff and hex panes)

        private void DiffBox_Scroll(object sender, EventArgs e)
        {
            if (_syncingScroll)
            {
                return;
            }

            var source = (RichTextBox)sender;
            RichTextBox target;

            if (sender == richTextBoxDiffLeft)
            {
                target = richTextBoxDiffRight;
            }
            else if (sender == richTextBoxDiffRight)
            {
                target = richTextBoxDiffLeft;
            }
            else if (sender == richTextBoxHexLeft)
            {
                target = richTextBoxHexRight;
            }
            else if (sender == richTextBoxHexRight)
            {
                target = richTextBoxHexLeft;
            }
            else
            {
                return;
            }

            _syncingScroll = true;

            try
            {
                Point position = GetScrollPosition(source);
                SetScrollPosition(target, position);
            }
            finally
            {
                _syncingScroll = false;
            }
        }

        private static Point GetScrollPosition(RichTextBox box)
        {
            var position = new Point();
            SendMessage(box.Handle, EmGetScrollPos, IntPtr.Zero, ref position);
            return position;
        }

        private static void SetScrollPosition(RichTextBox box, Point position)
        {
            var copy = new Point(position.X, position.Y);
            SendMessage(box.Handle, EmSetScrollPos, IntPtr.Zero, ref copy);
        }

        private const int WmUser = 0x0400;
        private const int WmHScroll = 0x0114;
        private const int WmMouseHWheel = 0x020E;
        private const int EmGetScrollPos = WmUser + 221;
        private const int EmSetScrollPos = WmUser + 222;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref Point lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        #endregion

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
