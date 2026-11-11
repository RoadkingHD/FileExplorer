using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FileExplorer.Dialogs;
using FileExplorer.Models;
using FileExplorer.Services;

namespace FileExplorer
{
    public partial class MainForm : Form
    {
        private const string FolderIconKey = "__folder__";
        private const string NoExtensionIconKey = "__noext__";
        private const string DummyNodeName = "__dummy__";

        private readonly List<string> _history = new List<string>();
        private int _historyPosition = -1;
        private bool _updatingTreeSelection;
        private string _currentPath;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetSplitterDistances();
            EnsureFolderIcons();
            BuildTreeRoots();
            Navigate(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        private void SetSplitterDistances()
        {
            try
            {
                splitMain.SplitterDistance = 220;
                splitContent.SplitterDistance = Math.Max(100, splitContent.Width - 340);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void EnsureFolderIcons()
        {
            string sampleDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!treeImageList.Images.ContainsKey(FolderIconKey))
            {
                treeImageList.Images.Add(FolderIconKey, ShellIconService.GetSmallIcon(sampleDirectory, true));
            }

            if (!listSmallImageList.Images.ContainsKey(FolderIconKey))
            {
                listSmallImageList.Images.Add(FolderIconKey, ShellIconService.GetSmallIcon(sampleDirectory, true));
            }

            if (!listLargeImageList.Images.ContainsKey(FolderIconKey))
            {
                listLargeImageList.Images.Add(FolderIconKey, ShellIconService.GetLargeIcon(sampleDirectory, true));
            }
        }

        #region Tree view

        private void BuildTreeRoots()
        {
            treeViewFolders.BeginUpdate();
            treeViewFolders.Nodes.Clear();

            treeViewFolders.Nodes.Add(CreateDirectoryNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop"));
            treeViewFolders.Nodes.Add(CreateDirectoryNode(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Documents"));

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    treeViewFolders.Nodes.Add(CreateDirectoryNode(drive.Name, drive.Name));
                }
            }

            treeViewFolders.EndUpdate();
        }

        private static TreeNode CreateDirectoryNode(string path, string text)
        {
            var node = new TreeNode(text)
            {
                Tag = path,
                ImageKey = FolderIconKey,
                SelectedImageKey = FolderIconKey
            };

            node.Nodes.Add(DummyNodeName, DummyNodeName);
            return node;
        }

        private void TreeViewFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode dummy = e.Node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Name == DummyNodeName);
            if (dummy == null)
            {
                return;
            }

            e.Node.Nodes.Clear();

            foreach (string directory in SafeGetDirectories((string)e.Node.Tag))
            {
                e.Node.Nodes.Add(CreateDirectoryNode(directory, Path.GetFileName(directory)));
            }

            if (e.Node.Nodes.Count == 0)
            {
                e.Node.Collapse();
            }
        }

        private void TreeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_updatingTreeSelection)
            {
                return;
            }

            string path = e.Node.Tag as string;
            if (!string.IsNullOrEmpty(path))
            {
                Navigate(path);
            }
        }

        private void SelectTreeNode(string path)
        {
            _updatingTreeSelection = true;

            try
            {
                TreeNode target = FindTreeNode(treeViewFolders.Nodes, path);

                if (target != null)
                {
                    ExpandAncestors(target);
                    treeViewFolders.SelectedNode = target;
                }
            }
            finally
            {
                _updatingTreeSelection = false;
            }
        }

        private static TreeNode FindTreeNode(TreeNodeCollection nodes, string path)
        {
            foreach (TreeNode node in nodes)
            {
                string nodePath = node.Tag as string;

                if (string.Equals(nodePath, path, StringComparison.OrdinalIgnoreCase))
                {
                    return node;
                }

                bool hasDummy = node.Nodes.Cast<TreeNode>().Any(n => n.Name == DummyNodeName);
                if (nodePath != null && hasDummy && path.StartsWith(nodePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    node.Nodes.Clear();
                    foreach (string directory in SafeGetDirectories(nodePath))
                    {
                        node.Nodes.Add(CreateDirectoryNode(directory, Path.GetFileName(directory)));
                    }
                }

                TreeNode result = FindTreeNode(node.Nodes, path);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static void ExpandAncestors(TreeNode node)
        {
            TreeNode current = node.Parent;
            while (current != null)
            {
                current.Expand();
                current = current.Parent;
            }
        }

        #endregion

        #region Navigation

        private void Navigate(string path)
        {
            Navigate(path, true);
        }

        private void Navigate(string path, bool recordHistory)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return;
            }

            _currentPath = path;

            if (recordHistory)
            {
                if (_historyPosition < _history.Count - 1)
                {
                    _history.RemoveRange(_historyPosition + 1, _history.Count - _historyPosition - 1);
                }

                bool isSameAsCurrent = _historyPosition >= 0 && string.Equals(_history[_historyPosition], path, StringComparison.OrdinalIgnoreCase);
                if (!isSameAsCurrent)
                {
                    _history.Add(path);
                    _historyPosition = _history.Count - 1;
                }
            }

            toolStripAddress.Text = path;
            LoadDirectory(path);
            SelectTreeNode(path);
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            toolStripButtonBack.Enabled = _historyPosition > 0;
            toolStripButtonForward.Enabled = _historyPosition < _history.Count - 1;
            toolStripButtonUp.Enabled = _currentPath != null && Path.GetDirectoryName(_currentPath) != null && Directory.Exists(Path.GetDirectoryName(_currentPath));
        }

        private void LoadDirectory(string path)
        {
            listViewFiles.BeginUpdate();

            try
            {
                listViewFiles.Items.Clear();

                foreach (string directory in SafeGetDirectories(path))
                {
                    var item = new ListViewItem(Path.GetFileName(directory))
                    {
                        Tag = directory,
                        ImageKey = FolderIconKey
                    };
                    item.SubItems.Add(Directory.GetLastWriteTime(directory).ToString("g"));
                    item.SubItems.Add("File folder");
                    item.SubItems.Add(string.Empty);
                    listViewFiles.Items.Add(item);
                }

                foreach (string file in SafeGetFiles(path))
                {
                    var info = new FileInfo(file);
                    string iconKey = EnsureFileIcon(file);

                    var item = new ListViewItem(info.Name)
                    {
                        Tag = file,
                        ImageKey = iconKey
                    };
                    item.SubItems.Add(info.LastWriteTime.ToString("g"));
                    item.SubItems.Add(ShellIconService.GetTypeDescription(file));
                    item.SubItems.Add(FormatSize(info.Length));
                    listViewFiles.Items.Add(item);
                }
            }
            finally
            {
                listViewFiles.EndUpdate();
            }

            toolStripStatusLabelItems.Text = listViewFiles.Items.Count + " item(s)";
            UpdatePreview();
        }

        private string EnsureFileIcon(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            string key = extension.Length == 0 ? NoExtensionIconKey : extension.ToLowerInvariant();

            if (!listSmallImageList.Images.ContainsKey(key))
            {
                listSmallImageList.Images.Add(key, ShellIconService.GetSmallIcon(filePath, false));
            }

            if (!listLargeImageList.Images.ContainsKey(key))
            {
                listLargeImageList.Images.Add(key, ShellIconService.GetLargeIcon(filePath, false));
            }

            return key;
        }

        private static string[] SafeGetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private static string[] SafeGetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private static string FormatSize(long length)
        {
            if (length < 1024L)
            {
                return length + " B";
            }

            double size = length;
            string[] units = { "KB", "MB", "GB", "TB" };
            int unitIndex = -1;

            while (size >= 1024.0 && unitIndex < units.Length - 1)
            {
                size /= 1024.0;
                unitIndex++;
            }

            return size.ToString(unitIndex == 0 ? "0" : "0.0") + " " + units[unitIndex];
        }

        #endregion

        #region Toolbar and menu events

        private void ToolStripButtonBack_Click(object sender, EventArgs e)
        {
            if (_historyPosition > 0)
            {
                _historyPosition--;
                Navigate(_history[_historyPosition], false);
            }
        }

        private void ToolStripButtonForward_Click(object sender, EventArgs e)
        {
            if (_historyPosition < _history.Count - 1)
            {
                _historyPosition++;
                Navigate(_history[_historyPosition], false);
            }
        }

        private void ToolStripButtonUp_Click(object sender, EventArgs e)
        {
            string parent = _currentPath == null ? null : Path.GetDirectoryName(_currentPath);
            if (!string.IsNullOrEmpty(parent))
            {
                Navigate(parent);
            }
        }

        private void ToolStripAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Navigate(NormalizeAddress(toolStripAddress.Text));
            }
        }

        private void ToolStripButtonGo_Click(object sender, EventArgs e)
        {
            Navigate(NormalizeAddress(toolStripAddress.Text));
        }

        private static string NormalizeAddress(string address)
        {
            string trimmed = (address ?? string.Empty).Trim().Trim('"');

            if (trimmed.Length == 0)
            {
                return trimmed;
            }

            if (trimmed.StartsWith("~", StringComparison.Ordinal))
            {
                trimmed = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + trimmed.Substring(1);
            }

            return Environment.ExpandEnvironmentVariables(trimmed);
        }

        private void ToolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshCurrentFolder();
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshCurrentFolder();
        }

        private void RefreshCurrentFolder()
        {
            if (!string.IsNullOrEmpty(_currentPath))
            {
                LoadDirectory(_currentPath);
                SelectTreeNode(_currentPath);
            }
        }

        private void ToolStripButtonZip_Click(object sender, EventArgs e)
        {
            ZipSelectedItems();
        }

        private void ToolStripButtonPreview_CheckedChanged(object sender, EventArgs e)
        {
            TogglePreviewPanel(toolStripButtonPreview.Checked);
        }

        private void PreviewPanelToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (previewPanelToolStripMenuItem.Checked != (toolStripButtonPreview.Checked))
            {
                toolStripButtonPreview.Checked = previewPanelToolStripMenuItem.Checked;
            }
        }

        private void TogglePreviewPanel(bool visible)
        {
            splitContent.Panel2Collapsed = !visible;

            if (previewPanelToolStripMenuItem.Checked != visible)
            {
                previewPanelToolStripMenuItem.Checked = visible;
            }

            if (visible)
            {
                UpdatePreview();
            }
            else
            {
                previewPanel1.ShowEmpty();
            }
        }

        private void PreviewRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPreviewDock(false);
        }

        private void PreviewBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPreviewDock(true);
        }

        private void SetPreviewDock(bool bottom)
        {
            previewRightToolStripMenuItem.Checked = !bottom;
            previewBottomToolStripMenuItem.Checked = bottom;

            splitContent.Orientation = bottom ? Orientation.Horizontal : Orientation.Vertical;

            try
            {
                splitContent.SplitterDistance = bottom
                    ? Math.Max(100, splitContent.Height - 260)
                    : Math.Max(100, splitContent.Width - 340);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void LargeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewFiles.View = View.LargeIcon;
        }

        private void SmallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewFiles.View = View.SmallIcon;
        }

        private void DetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewFiles.View = View.Details;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog
            {
                Description = "Open a folder.",
                SelectedPath = _currentPath ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Navigate(dialog.SelectedPath);
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ZipSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZipSelectedItems();
        }

        private void ManageCustomActionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new CustomActionsDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                this,
                "FileExplorer\r\n\r\nA Windows Explorer style file browser with zip support,\ncustom right-click actions and a preview panel for text, JSON, XML and HTML files.",
                "About FileExplorer",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion

        #region ListView events

        private void ListViewFiles_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewFiles.SelectedItems)
            {
                OpenPath((string)item.Tag);
            }
        }

        private void ListViewFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void ListViewFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listViewFiles.SelectedItems.Count > 0)
            {
                DeleteSelectedItems();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                RefreshCurrentFolder();
                e.Handled = true;
            }
        }

        private void OpenPath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Navigate(path);
                }
                else if (File.Exists(path))
                {
                    Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot open item.\r\n\r\n" + ex.Message, "Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Preview panel

        private void UpdatePreview()
        {
            if (splitContent.Panel2Collapsed)
            {
                return;
            }

            if (listViewFiles.SelectedItems.Count == 1)
            {
                string path = (string)listViewFiles.SelectedItems[0].Tag;

                if (File.Exists(path))
                {
                    previewPanel1.ShowFile(path);
                    return;
                }
            }

            previewPanel1.ShowEmpty();
        }

        #endregion

        #region Context menu

        private void ContextMenuStripFiles_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<string> selected = GetSelectedPaths();
            bool hasSelection = selected.Count > 0;

            contextOpenToolStripMenuItem.Enabled = hasSelection;
            contextZipToolStripMenuItem.Enabled = hasSelection;
            contextCopyPathToolStripMenuItem.Enabled = hasSelection;
            contextDeleteToolStripMenuItem.Enabled = hasSelection;
            contextCopyToolStripMenuItem.Enabled = hasSelection;
            contextPasteToolStripMenuItem.Enabled = Clipboard.ContainsFileDropList();

            contextExtractHereToolStripMenuItem.Enabled = selected.Count == 1
                && File.Exists(selected[0])
                && string.Equals(Path.GetExtension(selected[0]), ".zip", StringComparison.OrdinalIgnoreCase);

            BuildCustomActionsMenu(selected);
        }

        private List<string> GetSelectedPaths()
        {
            return listViewFiles.SelectedItems.Cast<ListViewItem>().Select(item => (string)item.Tag).ToList();
        }

        private List<string> GetSelectedFiles()
        {
            return GetSelectedPaths().Where(File.Exists).ToList();
        }

        private void BuildCustomActionsMenu(List<string> selectedPaths)
        {
            contextCustomActionsMenuItem.DropDownItems.Clear();

            List<string> selectedFiles = selectedPaths.Where(File.Exists).ToList();
            List<CustomAction> actions = CustomActionStore.Load();

            IEnumerable<CustomAction> matchingActions = actions
                .Where(a => selectedFiles.Count > 0 && a.MatchesAll(selectedFiles.Select(Path.GetFileName)));

            int added = 0;

            foreach (CustomAction action in matchingActions)
            {
                var menuItem = new ToolStripMenuItem(action.Name)
                {
                    ToolTipText = action.ExtensionFilter
                };
                string captured = action.Name;
                menuItem.Click += (s, args) => RunCustomAction(captured, selectedFiles);
                contextCustomActionsMenuItem.DropDownItems.Add(menuItem);
                added++;
            }

            contextCustomActionsMenuItem.Enabled = added > 0;

            if (added == 0)
            {
                contextCustomActionsMenuItem.DropDownItems.Add(new ToolStripMenuItem("(no matching custom actions)")
                {
                    Enabled = false
                });
            }
        }

        private void RunCustomAction(string actionName, List<string> files)
        {
            CustomAction action = CustomActionStore.Load().FirstOrDefault(a => a.Name == actionName);

            if (action == null || files.Count == 0)
            {
                return;
            }

            try
            {
                if (action.RunPerFile)
                {
                    foreach (string file in files)
                    {
                        Process.Start(BuildActionStartInfo(action, QuotePath(file), null));
                    }
                }
                else
                {
                    string fileList = string.Join(" ", files.Select(QuotePath));
                    Process.Start(BuildActionStartInfo(action, null, fileList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Custom action failed.\r\n\r\n" + ex.Message, action.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static string QuotePath(string path)
        {
            return "\"" + path + "\"";
        }

        private static ProcessStartInfo BuildActionStartInfo(CustomAction action, string singleFile, string fileList)
        {
            string arguments = action.Arguments ?? string.Empty;
            arguments = arguments.Replace("{file}", singleFile ?? string.Empty).Replace("{files}", fileList ?? string.Empty);
            string command = action.FilePath ?? string.Empty;
            command = command.Replace("{file}", singleFile ?? string.Empty).Replace("{files}", fileList ?? string.Empty);

            return new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = true
            };
        }

        private void ContextOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewFiles_DoubleClick(sender, e);
        }

        private void ContextZipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZipSelectedItems();
        }

        private void ContextExtractHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractSelectedItem();
        }

        private void ContextCopyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> selected = GetSelectedPaths();

            if (selected.Count > 0)
            {
                Clipboard.SetText(string.Join(Environment.NewLine, selected));
            }
        }

        private void ContextDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedItems();
        }

        private void ContextRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshCurrentFolder();
        }

        private void ContextCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPathsToClipboard(GetSelectedPaths());
        }

        private void ContextPasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboardInto(_currentPath);
        }

        private void ContextNewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewFolder(_currentPath);
        }

        #endregion

        #region Folder context menu

        private void TreeViewFolders_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeViewFolders.GetNodeAt(e.Location);

                if (node != null)
                {
                    treeViewFolders.SelectedNode = node;
                }
            }
        }

        private void ContextMenuStripFolders_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = treeViewFolders.SelectedNode == null ? null : treeViewFolders.SelectedNode.Tag as string;
            bool hasPath = !string.IsNullOrEmpty(path) && Directory.Exists(path);

            contextFolderOpenToolStripMenuItem.Enabled = hasPath;
            contextFolderCopyToolStripMenuItem.Enabled = hasPath;
            contextFolderPasteToolStripMenuItem.Enabled = hasPath && Clipboard.ContainsFileDropList();
            contextFolderNewFolderToolStripMenuItem.Enabled = hasPath;
        }

        private void ContextFolderOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = GetSelectedTreePath();

            if (!string.IsNullOrEmpty(path))
            {
                Navigate(path);
            }
        }

        private void ContextFolderCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = GetSelectedTreePath();

            if (!string.IsNullOrEmpty(path))
            {
                CopyPathsToClipboard(new List<string> { path });
            }
        }

        private void ContextFolderPasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = GetSelectedTreePath();

            if (!string.IsNullOrEmpty(path))
            {
                PasteClipboardInto(path);
            }
        }

        private void ContextFolderNewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = GetSelectedTreePath();

            if (!string.IsNullOrEmpty(path))
            {
                CreateNewFolder(path);
            }
        }

        private void ContextFolderRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshCurrentFolder();
        }

        private string GetSelectedTreePath()
        {
            return treeViewFolders.SelectedNode == null ? null : treeViewFolders.SelectedNode.Tag as string;
        }

        #endregion

        #region Clipboard and folder creation

        private static void CopyPathsToClipboard(IList<string> paths)
        {
            if (paths == null || paths.Count == 0)
            {
                return;
            }

            var list = new System.Collections.Specialized.StringCollection();
            list.AddRange(paths.ToArray());
            Clipboard.SetFileDropList(list);
        }

        private void PasteClipboardInto(string targetDirectory)
        {
            if (string.IsNullOrEmpty(targetDirectory) || !Directory.Exists(targetDirectory) || !Clipboard.ContainsFileDropList())
            {
                return;
            }

            var sources = Clipboard.GetFileDropList();
            var errors = new List<string>();
            int pasted = 0;

            foreach (string source in sources)
            {
                try
                {
                    string name = Path.GetFileName(source.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

                    if (name.Length == 0)
                    {
                        continue;
                    }

                    string destination = GetAvailableName(targetDirectory, name);

                    if (Directory.Exists(source))
                    {
                        CopyDirectory(source, destination);
                        pasted++;
                    }
                    else if (File.Exists(source))
                    {
                        File.Copy(source, destination, false);
                        pasted++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(source + ": " + ex.Message);
                }
            }

            if (pasted > 0 && string.Equals(targetDirectory, _currentPath, StringComparison.OrdinalIgnoreCase))
            {
                RefreshCurrentFolder();
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(this, "Some items could not be pasted.\r\n\r\n" + string.Join(Environment.NewLine, errors), "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static string GetAvailableName(string directory, string name)
        {
            string candidate = Path.Combine(directory, name);

            if (!Directory.Exists(candidate) && !File.Exists(candidate))
            {
                return candidate;
            }

            string baseName = Path.GetFileNameWithoutExtension(name);
            string extension = Path.GetExtension(name);
            int counter = 2;

            while (true)
            {
                candidate = Path.Combine(directory, baseName + " (" + counter + ")" + extension);

                if (!Directory.Exists(candidate) && !File.Exists(candidate))
                {
                    return candidate;
                }

                counter++;
            }
        }

        private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);

            foreach (string file in Directory.GetFiles(sourceDirectory))
            {
                File.Copy(file, Path.Combine(destinationDirectory, Path.GetFileName(file)), false);
            }

            foreach (string subDirectory in Directory.GetDirectories(sourceDirectory))
            {
                CopyDirectory(subDirectory, Path.Combine(destinationDirectory, Path.GetFileName(subDirectory)));
            }
        }

        private void CreateNewFolder(string parentDirectory)
        {
            if (string.IsNullOrEmpty(parentDirectory) || !Directory.Exists(parentDirectory))
            {
                return;
            }

            string suggestedName = Path.GetFileName(GetAvailableName(parentDirectory, "New folder"));
            string createdPath = null;

            using (var dialog = new InputDialog("New folder", "Folder name:", suggestedName))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                string name = dialog.Value;

                if (name.Length == 0)
                {
                    return;
                }

                try
                {
                    createdPath = Path.Combine(parentDirectory, name);

                    if (Directory.Exists(createdPath) || File.Exists(createdPath))
                    {
                        MessageBox.Show(this, "An item named \"" + name + "\" already exists.", "New folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Directory.CreateDirectory(createdPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not create the folder.\r\n\r\n" + ex.Message, "New folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (string.Equals(parentDirectory, _currentPath, StringComparison.OrdinalIgnoreCase))
            {
                RefreshCurrentFolder();
            }

            RefreshTreeNode(parentDirectory);
            SelectTreeNode(createdPath);
        }

        private void RefreshTreeNode(string path)
        {
            TreeNode node = FindTreeNode(treeViewFolders.Nodes, path);

            if (node == null)
            {
                return;
            }

            node.Nodes.Clear();
            node.Nodes.Add(DummyNodeName, DummyNodeName);

            if (node.IsExpanded)
            {
                node.Nodes.Clear();

                foreach (string directory in SafeGetDirectories(path))
                {
                    node.Nodes.Add(CreateDirectoryNode(directory, Path.GetFileName(directory)));
                }

                if (node.Nodes.Count == 0)
                {
                    node.Collapse();
                }
            }
        }

        #endregion

        #region Zip and extract

        private void ZipSelectedItems()
        {
            List<string> selected = GetSelectedPaths();

            if (selected.Count == 0)
            {
                return;
            }

            string defaultName = selected.Count == 1
                ? Path.GetFileName(selected[0].TrimEnd(Path.DirectorySeparatorChar))
                : Path.GetFileName(_currentPath ?? "Archive");

            if (string.IsNullOrEmpty(defaultName))
            {
                defaultName = "Archive";
            }

            using (var dialog = new SaveFileDialog
            {
                Title = "Zip selected items",
                Filter = "Zip archive (*.zip)|*.zip",
                FileName = defaultName + ".zip",
                InitialDirectory = _currentPath
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    ZipService.CreateZip(selected, dialog.FileName);
                    toolStripStatusLabelItems.Text = "Created " + Path.GetFileName(dialog.FileName);
                    RefreshCurrentFolder();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not create the zip archive.\r\n\r\n" + ex.Message, "Zip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExtractSelectedItem()
        {
            List<string> selected = GetSelectedPaths();

            if (selected.Count != 1 || !File.Exists(selected[0]))
            {
                return;
            }

            string zipPath = selected[0];

            try
            {
                string destination = ZipService.GetAvailableDestination(zipPath);
                ZipService.ExtractZip(zipPath, destination);
                toolStripStatusLabelItems.Text = "Extracted to " + Path.GetFileName(destination);
                RefreshCurrentFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not extract the archive.\r\n\r\n" + ex.Message, "Extract", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Delete

        private void DeleteSelectedItems()
        {
            List<string> selected = GetSelectedPaths();

            if (selected.Count == 0)
            {
                return;
            }

            string message = selected.Count == 1
                ? "Delete \"" + Path.GetFileName(selected[0].TrimEnd(Path.DirectorySeparatorChar)) + "\" permanently?"
                : "Delete these " + selected.Count + " items permanently?";

            if (MessageBox.Show(this, message, "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            var errors = new List<string>();

            foreach (string path in selected)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                    else if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(Path.GetFileName(path) + ": " + ex.Message);
                }
            }

            RefreshCurrentFolder();

            if (errors.Count > 0)
            {
                MessageBox.Show(this, "Some items could not be deleted.\r\n\r\n" + string.Join(Environment.NewLine, errors), "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
