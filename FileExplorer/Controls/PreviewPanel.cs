using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;
using FileExplorer.Services;
using Microsoft.Win32;

namespace FileExplorer.Controls
{
    public partial class PreviewPanel : UserControl
    {
        private const long MaxOfficePreviewBytes = 20 * 1024 * 1024;
        private const int MaxTreeNodes = int.MaxValue;

        private static readonly Color KeyColor = Color.FromArgb(163, 21, 21);
        private static readonly Color StringColor = Color.FromArgb(0, 128, 0);
        private static readonly Color NumberColor = Color.FromArgb(43, 82, 163);
        private static readonly Color LiteralColor = Color.FromArgb(120, 0, 120);
        private static readonly Color PunctuationColor = Color.FromArgb(128, 128, 128);
        private static readonly Color ElementColor = Color.FromArgb(43, 82, 163);
        private static readonly Color AttributeColor = Color.FromArgb(163, 21, 21);
        private static readonly Color CommentColor = Color.FromArgb(0, 128, 0);
        private static readonly Color ContentColor = Color.FromArgb(0, 0, 0);

        private static readonly HashSet<string> HtmlExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".html",
            ".htm"
        };

        private static readonly HashSet<string> XmlExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".xml",
            ".xaml",
            ".config",
            ".resx",
            ".csproj",
            ".vbproj"
        };

        private static readonly HashSet<string> OfficeExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".doc",
            ".docx",
            ".xls",
            ".xlsx",
            ".ppt",
            ".pptx"
        };

        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };

        private string _textContent;
        private bool _jsonMode;
        private bool _xmlMode;
        private bool _treeBuilt;
        private int _treeNodeCount;
        private bool _treeTruncated;
        private int _previewSequence;
        private string _lastTempFolder;

        public PreviewPanel()
        {
            InitializeComponent();
        }

        public void ShowFile(string path)
        {
            try
            {
                var info = new FileInfo(path);

                if (!info.Exists)
                {
                    ShowMessage("The file no longer exists.");
                    return;
                }

                labelPreviewFile.Text = info.Name + "  (" + FormatSize(info.Length) + ")";

                string extension = Path.GetExtension(path);
                _jsonMode = extension.Equals(".json", StringComparison.OrdinalIgnoreCase);
                _xmlMode = XmlExtensions.Contains(extension);

                btnTree.Visible = _jsonMode || _xmlMode;
                btnText.Visible = _jsonMode || _xmlMode;

                if (HtmlExtensions.Contains(extension))
                {
                    ShowHtml(ReadText(path));
                    return;
                }

                if (extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    ShowPdf(path);
                    return;
                }

                if (OfficeExtensions.Contains(extension))
                {
                    if (info.Length > MaxOfficePreviewBytes)
                    {
                        ShowMessage("Document is too large to preview (limit is 20 MB).");
                        return;
                    }

                    ShowOfficePreview(path);
                    return;
                }

                if (extension.Equals(".exe", StringComparison.OrdinalIgnoreCase)
                    || extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    ShowPeBrowser(path);
                    return;
                }

                if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    ShowZipBrowser(path);
                    return;
                }

                if (IsBinary(path))
                {
                    ShowMessage("Binary file. Preview is not available.");
                    return;
                }

                string raw = ReadText(path);

                if (_jsonMode)
                {
                    _textContent = PrettyPrintJson(raw);
                }
                else if (_xmlMode)
                {
                    _textContent = IndentXml(raw);
                }
                else
                {
                    _textContent = raw;
                }

                _treeBuilt = false;

                if (_jsonMode || _xmlMode)
                {
                    ShowTree();
                }
                else
                {
                    ShowText(_textContent, false, false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Cannot preview this file.\r\n\r\n" + ex.Message);
            }
        }

        public void ShowEmpty()
        {
            labelPreviewFile.Text = "No preview available";
            btnTree.Visible = false;
            btnText.Visible = false;

            webBrowserPreview.Visible = false;
            treeViewPreview.Visible = false;
            richTextBoxPreview.Visible = true;
            chkWrap.Visible = true;
            richTextBoxPreview.Clear();
            richTextBoxPreview.ForeColor = SystemColors.GrayText;
            richTextBoxPreview.Text = "Select a single file to preview it here.\r\n\r\nSupported: text, JSON, XML, HTML, PDF, Office documents, EXE/DLL (PE browser).";
        }

        private void ShowText(string content, bool highlightJson, bool highlightMarkup)
        {
            webBrowserPreview.Visible = false;
            treeViewPreview.Visible = false;
            richTextBoxPreview.Visible = true;
            chkWrap.Visible = true;
            richTextBoxPreview.ForeColor = SystemColors.WindowText;
            richTextBoxPreview.Text = content;
            richTextBoxPreview.SelectionStart = 0;
            richTextBoxPreview.SelectionLength = 0;

            if (highlightJson)
            {
                HighlightJson();
            }
            else if (highlightMarkup)
            {
                HighlightMarkup();
            }
        }

        private void ShowTree()
        {
            if (!_treeBuilt)
            {
                BuildTree();
                _treeBuilt = true;
            }

            if (treeViewPreview.Nodes.Count == 0)
            {
                return;
            }

            webBrowserPreview.Visible = false;
            richTextBoxPreview.Visible = false;
            chkWrap.Visible = false;
            treeViewPreview.Visible = true;
        }

        private void ShowHtml(string content)
        {
            SetWebMode();
            webBrowserPreview.DocumentText = content;
        }

        private void ShowHtmlFile(string path)
        {
            SetWebMode();
            webBrowserPreview.Navigate(new Uri(path));
        }

        private void SetWebMode()
        {
            btnTree.Visible = false;
            btnText.Visible = false;
            treeViewPreview.Visible = false;
            richTextBoxPreview.Visible = false;
            chkWrap.Visible = false;
            webBrowserPreview.Visible = true;
        }

        private void ShowPdf(string path)
        {
            if (!IsPdfPreviewAvailable())
            {
                ShowMessage("PDF preview requires Adobe Reader or Adobe Acrobat with browser integration.");
                return;
            }

            SetWebMode();
            webBrowserPreview.Navigate(new Uri(path));
        }

        private static bool IsPdfPreviewAvailable()
        {
            try
            {
                using (RegistryKey acrobat = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Adobe\Adobe Acrobat"))
                using (RegistryKey reader = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Adobe\Acrobat Reader"))
                using (RegistryKey documentClass = Registry.ClassesRoot.OpenSubKey("AcroExch.Document"))
                {
                    return acrobat != null || reader != null || documentClass != null;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        private void ShowOfficePreview(string path)
        {
            int sequence = ++_previewSequence;
            ShowMessage("Converting document for preview...");
            Cursor = Cursors.WaitCursor;

            var thread = new Thread(() =>
            {
                string htmlPath;
                bool converted = TryConvertOfficeToHtml(path, out htmlPath);

                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        if (sequence != _previewSequence)
                        {
                            return;
                        }

                        Cursor = Cursors.Default;

                        if (converted && File.Exists(htmlPath))
                        {
                            ShowHtmlFile(htmlPath);
                        }
                        else
                        {
                            ShowMessage("Cannot preview this document.\r\n\r\nMicrosoft Office may not be installed, the file may be protected, or the format is not supported.");
                        }
                    }));
                }
                catch (InvalidOperationException)
                {
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private bool TryConvertOfficeToHtml(string path, out string htmlPath)
        {
            htmlPath = null;

            string extension = Path.GetExtension(path).ToLowerInvariant();
            string progId;
            string collectionName;
            int htmlFormat;

            if (extension == ".doc" || extension == ".docx")
            {
                progId = "Word.Application";
                collectionName = "Documents";
                htmlFormat = 10;
            }
            else if (extension == ".xls" || extension == ".xlsx")
            {
                progId = "Excel.Application";
                collectionName = "Workbooks";
                htmlFormat = 44;
            }
            else if (extension == ".ppt" || extension == ".pptx")
            {
                progId = "PowerPoint.Application";
                collectionName = "Presentations";
                htmlFormat = 12;
            }
            else
            {
                return false;
            }

            Type officeType = Type.GetTypeFromProgID(progId);

            if (officeType == null)
            {
                return false;
            }

            htmlPath = Path.Combine(PrepareTempPreviewFolder(), "preview.html");

            object app = null;

            try
            {
                app = Activator.CreateInstance(officeType);
                object collection = officeType.InvokeMember(collectionName, BindingFlags.GetProperty, null, app, null);

                object document;

                if (progId == "Excel.Application")
                {
                    document = collection.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, collection, new object[] { path, 0, true });
                }
                else if (progId == "PowerPoint.Application")
                {
                    document = collection.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, collection, new object[] { path, -1, 0, 0 });
                }
                else
                {
                    document = collection.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, collection, new object[] { path, false, true });
                }

                try
                {
                    document.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, document, new object[] { htmlPath, htmlFormat });
                }
                finally
                {
                    if (progId == "PowerPoint.Application")
                    {
                        document.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, document, null);
                    }
                    else
                    {
                        document.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, document, new object[] { false });
                    }
                }

                return File.Exists(htmlPath);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (app != null)
                {
                    try
                    {
                        app.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, app, null);
                    }
                    catch (Exception)
                    {
                    }

                    Marshal.FinalReleaseComObject(app);
                }
            }
        }

        private string PrepareTempPreviewFolder()
        {
            CleanupTempPreview();

            string folder = Path.Combine(Path.GetTempPath(), "FileExplorerPreview", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            _lastTempFolder = folder;
            return folder;
        }

        private void CleanupTempPreview()
        {
            if (_lastTempFolder == null)
            {
                return;
            }

            try
            {
                Directory.Delete(_lastTempFolder, true);
            }
            catch (Exception)
            {
            }

            _lastTempFolder = null;
        }

        private void ShowMessage(string message)
        {
            btnTree.Visible = false;
            btnText.Visible = false;
            webBrowserPreview.Visible = false;
            treeViewPreview.Visible = false;
            richTextBoxPreview.Visible = true;
            chkWrap.Visible = true;
            richTextBoxPreview.ForeColor = SystemColors.GrayText;
            richTextBoxPreview.Clear();
            richTextBoxPreview.Text = message;
        }

        #region Foldable tree

        private void BuildTree()
        {
            treeViewPreview.BeginUpdate();
            treeViewPreview.Nodes.Clear();
            _treeNodeCount = 0;
            _treeTruncated = false;

            bool built = _jsonMode ? TryBuildJsonTree() : TryBuildXmlTree();

            if (built && treeViewPreview.Nodes.Count > 0)
            {
                treeViewPreview.Nodes[0].Expand();
            }

            treeViewPreview.EndUpdate();

            if (!built)
            {
                ShowText(_textContent, _jsonMode, !_jsonMode);
            }
        }

        private bool TryBuildJsonTree()
        {
            object root;

            try
            {
                root = _serializer.DeserializeObject(_textContent);
            }
            catch (Exception)
            {
                return false;
            }

            var rootLine = new NodeLine();
            rootLine.Add("json", PunctuationColor);
            var rootNode = NewNode(rootLine);
            treeViewPreview.Nodes.Add(rootNode);
            AddJsonChildren(rootNode, root);
            return true;
        }

        private void AddJsonChildren(TreeNode parent, object value)
        {
            var dictionary = value as Dictionary<string, object>;

            if (dictionary != null)
            {
                foreach (KeyValuePair<string, object> pair in dictionary)
                {
                    if (_treeTruncated)
                    {
                        break;
                    }

                    AddJsonValue(parent, pair.Key, pair.Value);
                }

                return;
            }

            var array = value as object[];

            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (_treeTruncated)
                    {
                        break;
                    }

                    AddJsonValue(parent, "[" + i + "]", array[i]);
                }
            }
        }

        private void AddJsonValue(TreeNode parent, string key, object value)
        {
            if (_treeTruncated)
            {
                return;
            }

            bool isContainer = value is Dictionary<string, object> || value is object[];
            var line = new NodeLine();

            if (key != null)
            {
                line.Add(key, KeyColor);
                line.Add(": ", PunctuationColor);
            }

            var node = NewNode(line);

            if (isContainer)
            {
                var dictionary = value as Dictionary<string, object>;

                if (dictionary != null)
                {
                    line.Add("{ " + dictionary.Count + " }", PunctuationColor);
                }
                else
                {
                    line.Add("[ " + ((object[])value).Length + " ]", PunctuationColor);
                }

                parent.Nodes.Add(node);
                AddJsonChildren(node, value);
            }
            else
            {
                line.Add(FormatJsonPrimitive(value), JsonPrimitiveColor(value));
                parent.Nodes.Add(node);
            }

            if (_treeTruncated && parent.Nodes.Count > 0 && !(parent.Nodes[parent.Nodes.Count - 1].Tag is NodeLine truncatedLine && truncatedLine.IsTruncatedMarker))
            {
                var marker = new NodeLine();
                marker.IsTruncatedMarker = true;
                marker.Add("(truncated)", PunctuationColor);
                parent.Nodes.Add(NewNode(marker));
            }
        }

        private static string FormatJsonPrimitive(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is string)
            {
                return "\"" + value + "\"";
            }

            if (value is bool)
            {
                return ((bool)value) ? "true" : "false";
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private static Color JsonPrimitiveColor(object value)
        {
            if (value is string)
            {
                return StringColor;
            }

            if (value == null || value is bool)
            {
                return LiteralColor;
            }

            return NumberColor;
        }

        private bool TryBuildXmlTree()
        {
            XDocument document;

            try
            {
                document = XDocument.Parse(_textContent);
            }
            catch (Exception)
            {
                return false;
            }

            var rootLine = new NodeLine();
            rootLine.Add("xml", PunctuationColor);
            var rootNode = NewNode(rootLine);
            treeViewPreview.Nodes.Add(rootNode);

            if (document.Root != null)
            {
                rootNode.Nodes.Add(CreateXmlElementNode(document.Root));
            }

            return true;
        }

        private TreeNode CreateXmlElementNode(XElement element)
        {
            var line = new NodeLine();
            var node = NewNode(line);

            line.Add("<", PunctuationColor);
            line.Add(element.Name.LocalName, ElementColor);

            foreach (XAttribute attribute in element.Attributes())
            {
                line.Add(" " + attribute.Name.LocalName, AttributeColor);
                line.Add("=\"", PunctuationColor);
                line.Add(attribute.Value, StringColor);
                line.Add("\"", PunctuationColor);
            }

            bool hasElements = element.HasElements;
            bool hasText = element.Nodes().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));

            if (!hasElements && hasText)
            {
                line.Add(">", PunctuationColor);
                line.Add(element.Value.Trim(), ContentColor);
                line.Add("</" + element.Name.LocalName + ">", PunctuationColor);
                return node;
            }

            line.Add(hasElements || hasText ? ">" : "/>", PunctuationColor);

            foreach (XNode child in element.Nodes())
            {
                if (_treeTruncated)
                {
                    break;
                }

                var childElement = child as XElement;

                if (childElement != null)
                {
                    node.Nodes.Add(CreateXmlElementNode(childElement));
                    continue;
                }

                var comment = child as XComment;

                if (comment != null)
                {
                    node.Nodes.Add(CreateXmlCommentNode(comment.Value));
                    continue;
                }

                var text = child as XText;

                if (text != null && !string.IsNullOrWhiteSpace(text.Value))
                {
                    node.Nodes.Add(CreateXmlTextNode(text.Value.Trim()));
                }
            }

            if (hasElements || hasText)
            {
                var closeLine = new NodeLine();
                closeLine.Add("</" + element.Name.LocalName + ">", PunctuationColor);
                node.Nodes.Add(NewNode(closeLine));
            }

            return node;
        }

        private TreeNode CreateXmlCommentNode(string comment)
        {
            var line = new NodeLine();
            line.Add("<!-- " + comment.Trim() + " -->", CommentColor);
            return NewNode(line);
        }

        private TreeNode CreateXmlTextNode(string text)
        {
            var line = new NodeLine();
            line.Add(text, ContentColor);
            return NewNode(line);
        }

        private TreeNode NewNode(NodeLine line)
        {
            _treeNodeCount++;

            if (_treeNodeCount > MaxTreeNodes)
            {
                _treeTruncated = true;
            }

            return new TreeNode { Tag = line };
        }

        private void ExpandAllTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewPreview.ExpandAll();
        }

        private void CollapseAllTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewPreview.CollapseAll();

            if (treeViewPreview.Nodes.Count > 0)
            {
                treeViewPreview.Nodes[0].Expand();
            }
        }

        private void BtnTree_Click(object sender, EventArgs e)
        {
            if (_jsonMode || _xmlMode)
            {
                ShowTree();
            }
        }

        private void BtnText_Click(object sender, EventArgs e)
        {
            ShowText(_textContent, _jsonMode, !_jsonMode);
        }

        private void TreeViewPreview_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            using (var backgroundBrush = new SolidBrush(treeViewPreview.BackColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            }

            bool selected = (e.State & TreeNodeStates.Selected) != 0;

            if (selected)
            {
                var highlightBounds = new Rectangle(e.Bounds.X, e.Bounds.Y, treeViewPreview.ClientSize.Width - e.Bounds.X, e.Bounds.Height);
                using (var brush = new SolidBrush(Color.FromArgb(204, 228, 247)))
                {
                    e.Graphics.FillRectangle(brush, highlightBounds);
                }
            }

            var line = e.Node.Tag as NodeLine;

            if (line == null)
            {
                TextRenderer.DrawText(e.Graphics, e.Node.Text, treeViewPreview.Font, e.Bounds, treeViewPreview.ForeColor, TextFormatFlags.Default);
                return;
            }

            int x = e.Bounds.X;
            int height = e.Bounds.Height;

            foreach (Segment segment in line.Segments)
            {
                if (x >= treeViewPreview.ClientSize.Width)
                {
                    break;
                }

                Size size = TextRenderer.MeasureText(e.Graphics, segment.Text, treeViewPreview.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                var segmentBounds = new Rectangle(x, e.Bounds.Y, size.Width, height);
                TextRenderer.DrawText(e.Graphics, segment.Text, treeViewPreview.Font, segmentBounds, segment.Color, TextFormatFlags.NoPadding);
                x += size.Width;
            }
        }

        private sealed class Segment
        {
            public Segment(string text, Color color)
            {
                Text = text;
                Color = color;
            }

            public string Text { get; private set; }

            public Color Color { get; private set; }
        }

        private sealed class NodeLine
        {
            public NodeLine()
            {
                Segments = new List<Segment>();
            }

            public List<Segment> Segments { get; private set; }

            public bool IsTruncatedMarker { get; set; }

            public void Add(string text, Color color)
            {
                Segments.Add(new Segment(text, color));
            }
        }

        #endregion

        #region Zip archive browser

        private const int MaxZipEntries = int.MaxValue;

        private sealed class ZipEntryInfo
        {
            public string FullName = string.Empty;
            public long Length;
            public long CompressedLength;
        }

        private void ShowZipBrowser(string path)
        {
            int sequence = ++_previewSequence;
            ShowMessage("Reading archive...");
            Cursor = Cursors.WaitCursor;

            var thread = new Thread(() =>
            {
                List<ZipEntryInfo> entries = new List<ZipEntryInfo>();
                int totalEntries = 0;
                string error = null;

                try
                {
                    using (var archive = ZipFile.OpenRead(path))
                    {
                        totalEntries = archive.Entries.Count;

                        foreach (var entry in archive.Entries)
                        {
                            if (entries.Count >= MaxZipEntries)
                            {
                                break;
                            }

                            entries.Add(new ZipEntryInfo
                            {
                                FullName = entry.FullName,
                                Length = entry.Length,
                                CompressedLength = entry.CompressedLength
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        if (sequence != _previewSequence)
                        {
                            return;
                        }

                        Cursor = Cursors.Default;

                        if (error != null)
                        {
                            ShowMessage("Cannot read the zip archive.\r\n\r\n" + error);
                            return;
                        }

                        BuildZipTree(entries, totalEntries, Path.GetFileName(path));
                    }));
                }
                catch (InvalidOperationException)
                {
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void BuildZipTree(List<ZipEntryInfo> entries, int totalEntries, string fileName)
        {
            _jsonMode = false;
            _xmlMode = false;
            _textContent = null;
            _treeBuilt = true;

            treeViewPreview.BeginUpdate();
            treeViewPreview.Nodes.Clear();
            _treeNodeCount = 0;
            _treeTruncated = false;

            long totalUncompressed = 0;
            int fileCount = 0;

            foreach (ZipEntryInfo entry in entries)
            {
                if (entry.FullName.Length > 0
                    && !entry.FullName.EndsWith("/", StringComparison.Ordinal))
                {
                    totalUncompressed += entry.Length;
                    fileCount++;
                }
            }

            var rootLine = new NodeLine();
            rootLine.Add("Zip archive ", ElementColor);
            rootLine.Add(fileName, ContentColor);
            rootLine.Add("  \u2014  " + fileCount + " file(s), " + FormatSize(totalUncompressed)
                + (fileCount < totalEntries ? "  (" + totalEntries + " entries incl. folders)" : string.Empty),
                CommentColor);
            var rootNode = NewNode(rootLine);
            treeViewPreview.Nodes.Add(rootNode);

            var directoryNodes = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase);

            foreach (ZipEntryInfo entry in entries)
            {
                if (_treeTruncated)
                {
                    break;
                }

                string fullName = entry.FullName.Replace('/', Path.DirectorySeparatorChar);
                bool isDirectory = fullName.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
                string[] parts = fullName.TrimEnd(Path.DirectorySeparatorChar)
                    .Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                {
                    continue;
                }

                TreeNodeCollection currentNodes = rootNode.Nodes;
                string builtPath = string.Empty;

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    builtPath = builtPath.Length == 0
                        ? parts[i]
                        : builtPath + Path.DirectorySeparatorChar + parts[i];

                    TreeNode directoryNode;

                    if (!directoryNodes.TryGetValue(builtPath, out directoryNode))
                    {
                        var line = new NodeLine();
                        line.Add(parts[i] + Path.DirectorySeparatorChar, ElementColor);
                        directoryNode = NewNode(line);
                        directoryNodes[builtPath] = directoryNode;
                        currentNodes.Add(directoryNode);
                    }

                    currentNodes = directoryNode.Nodes;
                }

                if (isDirectory)
                {
                    string directoryPath = builtPath.Length == 0
                        ? parts[0]
                        : builtPath + Path.DirectorySeparatorChar + parts[parts.Length - 1];

                    if (!directoryNodes.ContainsKey(directoryPath))
                    {
                        var line = new NodeLine();
                        line.Add(parts[parts.Length - 1] + Path.DirectorySeparatorChar, ElementColor);
                        var node = NewNode(line);
                        directoryNodes[directoryPath] = node;
                        currentNodes.Add(node);
                    }
                }
                else
                {
                    var line = new NodeLine();
                    line.Add(parts[parts.Length - 1], ContentColor);
                    line.Add("  " + FormatSize(entry.Length)
                        + " (" + FormatSize(entry.CompressedLength) + " compressed)", CommentColor);
                    currentNodes.Add(NewNode(line));
                }
            }

            if (_treeTruncated)
            {
                var marker = new NodeLine();
                marker.Add("... (output truncated)", CommentColor);
                marker.IsTruncatedMarker = true;
                rootNode.Nodes.Add(NewNode(marker));
            }

            rootNode.Expand();
            treeViewPreview.EndUpdate();

            btnTree.Visible = false;
            btnText.Visible = false;
            webBrowserPreview.Visible = false;
            richTextBoxPreview.Visible = false;
            chkWrap.Visible = false;
            treeViewPreview.Visible = true;
        }

        #endregion

        #region PE browser (EXE/DLL)

        private void ShowPeBrowser(string path)
        {
            int sequence = ++_previewSequence;
            ShowMessage("Analyzing PE image...");
            Cursor = Cursors.WaitCursor;

            var thread = new Thread(() =>
            {
                PeInfo peInfo = PeBrowserService.ReadPeInfo(path);

                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        if (sequence != _previewSequence)
                        {
                            return;
                        }

                        Cursor = Cursors.Default;

                        if (!peInfo.IsValid)
                        {
                            ShowMessage("Cannot read the PE image.\r\n\r\n" + peInfo.Error);
                            return;
                        }

                        BuildPeTree(peInfo, Path.GetFileName(path));
                    }));
                }
                catch (InvalidOperationException)
                {
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void BuildPeTree(PeInfo info, string fileName)
        {
            _jsonMode = false;
            _xmlMode = false;
            _textContent = null;
            _treeBuilt = true;

            treeViewPreview.BeginUpdate();
            treeViewPreview.Nodes.Clear();
            _treeNodeCount = 0;
            _treeTruncated = false;

            var rootLine = new NodeLine();
            rootLine.Add("PE image ", ElementColor);
            rootLine.Add(fileName, ContentColor);
            rootLine.Add("  \u2014  " + info.KindText + ", " + info.MachineName, CommentColor);
            var rootNode = NewNode(rootLine);
            treeViewPreview.Nodes.Add(rootNode);

            AddPeGeneralNodes(rootNode, info);
            AddPeManagedNodes(rootNode, info);
            AddPeSectionNodes(rootNode, info);
            AddPeImportNodes(rootNode, info);
            AddPeExportNodes(rootNode, info);

            if (_treeTruncated)
            {
                var marker = new NodeLine();
                marker.Add("... (output truncated)", CommentColor);
                marker.IsTruncatedMarker = true;
                rootNode.Nodes.Add(NewNode(marker));
            }

            treeViewPreview.ExpandAll();
            treeViewPreview.EndUpdate();

            btnTree.Visible = false;
            btnText.Visible = false;
            webBrowserPreview.Visible = false;
            richTextBoxPreview.Visible = false;
            chkWrap.Visible = false;
            treeViewPreview.Visible = true;
        }

        private void AddPeGeneralNodes(TreeNode parent, PeInfo info)
        {
            var node = AddPeKeyValueNode(parent, "General", string.Empty);

            AddPeKeyValueNode(node, "Type", info.KindText);
            AddPeKeyValueNode(node, "Machine", info.MachineName + " (0x" + info.Machine.ToString("X4") + ")");
            AddPeKeyValueNode(node, "Format", info.IsPe32Plus ? "PE32+ (64-bit)" : "PE32 (32-bit)");
            AddPeKeyValueNode(node, "Subsystem", info.SubsystemName);
            AddPeKeyValueNode(node, "Linker timestamp", info.LinkerTimestampText);
            AddPeKeyValueNode(node, "Entry point", "RVA 0x" + info.EntryPointRva.ToString("X"));
            AddPeKeyValueNode(node, "Image base", "0x" + info.ImageBase.ToString("X"));
            AddPeKeyValueNode(node, "Size of image", "0x" + info.SizeOfImage.ToString("X") + " (" + FormatSize(info.SizeOfImage) + ")");
        }

        private void AddPeManagedNodes(TreeNode parent, PeInfo info)
        {
            var node = AddPeKeyValueNode(parent, "Code type", info.ManagedText);

            if (!info.IsManaged)
            {
                AddPeKeyValueNode(node, "Runtime", "No CLR header \u2014 native machine code");
                return;
            }

            AddPeKeyValueNode(node, "Runtime version", info.RuntimeVersionText);

            if (info.TargetFramework.Length > 0)
            {
                AddPeKeyValueNode(node, "Target framework", info.TargetFramework);
            }
            else
            {
                AddPeKeyValueNode(node, "Target framework", "not recorded in metadata");
            }

            if (info.UsesMscoree)
            {
                AddPeKeyValueNode(node, "Runtime host", "mscoree.dll");
            }
        }

        private void AddPeSectionNodes(TreeNode parent, PeInfo info)
        {
            var node = AddPeKeyValueNode(parent, "Sections", info.Sections.Count.ToString());

            foreach (PeSectionInfo section in info.Sections)
            {
                if (_treeTruncated)
                {
                    return;
                }

                var line = new NodeLine();
                line.Add(section.Name, ElementColor);
                line.Add("  raw " + FormatSize(section.SizeOfRawData)
                    + ", virtual " + FormatSize(section.VirtualSize)
                    + (section.Characteristics.Length > 0 ? ", " + section.Characteristics : string.Empty),
                    CommentColor);
                node.Nodes.Add(NewNode(line));
            }
        }

        private void AddPeImportNodes(TreeNode parent, PeInfo info)
        {
            int totalFunctions = 0;

            foreach (PeImportInfo import in info.Imports)
            {
                totalFunctions += import.Functions.Count;
            }

            var node = AddPeKeyValueNode(
                parent,
                "Imports",
                info.Imports.Count + " module(s), " + totalFunctions + " function(s)");

            foreach (PeImportInfo import in info.Imports)
            {
                if (_treeTruncated)
                {
                    return;
                }

                var dllNode = NewNode(MakePeLine(import.DllName, import.Functions.Count + " function(s)"));
                node.Nodes.Add(dllNode);

                foreach (string function in import.Functions)
                {
                    if (_treeTruncated)
                    {
                        return;
                    }

                    var line = new NodeLine();
                    line.Add(function, ContentColor);
                    dllNode.Nodes.Add(NewNode(line));
                }

                if (import.Truncated)
                {
                    var truncatedLine = new NodeLine();
                    truncatedLine.Add("... (list truncated)", CommentColor);
                    dllNode.Nodes.Add(NewNode(truncatedLine));
                }
            }
        }

        private void AddPeExportNodes(TreeNode parent, PeInfo info)
        {
            if (info.ExportFunctionCount == 0 && info.Exports.Count == 0)
            {
                AddPeKeyValueNode(parent, "Exports", "none");
                return;
            }

            var node = AddPeKeyValueNode(
                parent,
                "Exports",
                info.ExportFunctionCount + " function(s), " + info.ExportNameCount + " name(s)");

            foreach (string export in info.Exports)
            {
                if (_treeTruncated)
                {
                    return;
                }

                var line = new NodeLine();
                line.Add(export, ContentColor);
                node.Nodes.Add(NewNode(line));
            }

            if (info.ExportsTruncated)
            {
                var truncatedLine = new NodeLine();
                truncatedLine.Add("... (list truncated)", CommentColor);
                node.Nodes.Add(NewNode(truncatedLine));
            }
        }

        private TreeNode AddPeKeyValueNode(TreeNode parent, string key, string value)
        {
            var node = NewNode(MakePeLine(key, value));
            parent.Nodes.Add(node);
            return node;
        }

        private static NodeLine MakePeLine(string key, string value)
        {
            var line = new NodeLine();
            line.Add(key, ElementColor);

            if (!string.IsNullOrEmpty(value))
            {
                line.Add(": " + value, ContentColor);
            }

            return line;
        }

        #endregion

        #region Text reading and formatting

        private static string ReadText(string path)
        {
            using (var reader = new StreamReader(path, new UTF8Encoding(false), true))
            {
                return reader.ReadToEnd();
            }
        }

        private static bool IsBinary(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var buffer = new byte[1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                return buffer.Take(read).Any(b => b == 0);
            }
        }

        private static string IndentXml(string content)
        {
            try
            {
                return XElement.Parse(content).ToString(SaveOptions.None) == content
                    ? content
                    : XElement.Parse(content).ToString();
            }
            catch (Exception)
            {
                return content;
            }
        }

        private static string PrettyPrintJson(string json)
        {
            var builder = new StringBuilder();
            bool inString = false;
            bool escaped = false;
            int indent = 0;

            foreach (char c in json)
            {
                if (inString)
                {
                    builder.Append(c);

                    if (escaped)
                    {
                        escaped = false;
                    }
                    else if (c == '\\')
                    {
                        escaped = true;
                    }
                    else if (c == '"')
                    {
                        inString = false;
                    }

                    continue;
                }

                switch (c)
                {
                    case '"':
                        inString = true;
                        builder.Append(c);
                        break;
                    case '{':
                    case '[':
                        builder.Append(c);
                        builder.AppendLine();
                        indent++;
                        builder.Append(' ', indent * 2);
                        break;
                    case '}':
                    case ']':
                        builder.AppendLine();
                        indent--;
                        builder.Append(' ', indent * 2);
                        builder.Append(c);
                        break;
                    case ',':
                        builder.Append(c);
                        builder.AppendLine();
                        builder.Append(' ', indent * 2);
                        break;
                    case ':':
                        builder.Append(": ");
                        break;
                    default:
                        if (!char.IsWhiteSpace(c))
                        {
                            builder.Append(c);
                        }
                        break;
                }
            }

            return builder.ToString();
        }

        #endregion

        #region Text highlighting

        private void HighlightJson()
        {
            string text = richTextBoxPreview.Text;
            bool inString = false;
            bool escaped = false;
            int stringStart = -1;
            bool expectValue = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (inString)
                {
                    if (escaped)
                    {
                        escaped = false;
                    }
                    else if (c == '\\')
                    {
                        escaped = true;
                    }
                    else if (c == '"')
                    {
                        inString = false;
                        int colonIndex = text.IndexOf(':', i + 1);
                        int closeIndex = text.IndexOfAny(new[] { '}', ']', ',' }, i + 1);
                        bool isKey = colonIndex >= 0 && (closeIndex < 0 || colonIndex < closeIndex);

                        Color color = isKey ? KeyColor : StringColor;
                        ColorRange(stringStart, i - stringStart + 1, color);
                    }

                    continue;
                }

                switch (c)
                {
                    case '"':
                        inString = true;
                        stringStart = i;
                        expectValue = false;
                        break;
                    case ':':
                    case '{':
                    case '[':
                    case ',':
                        expectValue = true;
                        break;
                    default:
                        if (!expectValue || char.IsWhiteSpace(c))
                        {
                            break;
                        }

                        if (char.IsDigit(c) || c == '-')
                        {
                            ColorRange(i, NumberLength(text, i), NumberColor);
                        }
                        else if (char.IsLetter(c))
                        {
                            int length = LetterLength(text, i);
                            string word = text.Substring(i, length);
                            Color wordColor = word == "true" || word == "false" || word == "null"
                                ? LiteralColor
                                : NumberColor;
                            ColorRange(i, length, wordColor);
                            i += length - 1;
                            expectValue = false;
                        }

                        break;
                }
            }
        }

        private void HighlightMarkup()
        {
            string text = richTextBoxPreview.Text;
            int i = 0;

            while (i < text.Length)
            {
                if (text[i] != '<')
                {
                    i++;
                    continue;
                }

                int end = FindTagEnd(text, i);

                if (end < 0)
                {
                    break;
                }

                if (end - i >= 4 && text[i + 1] == '!' && text[i + 2] == '-' && text[i + 3] == '-')
                {
                    ColorRange(i, end - i + 1, CommentColor);
                }
                else
                {
                    ColorTag(i, end);
                }

                i = end + 1;
            }
        }

        private static int FindTagEnd(string text, int start)
        {
            char quote = '\0';

            for (int i = start + 1; i < text.Length; i++)
            {
                char c = text[i];

                if (quote != '\0')
                {
                    if (c == quote)
                    {
                        quote = '\0';
                    }
                }
                else if (c == '"' || c == '\'')
                {
                    quote = c;
                }
                else if (c == '>')
                {
                    return i;
                }
            }

            return -1;
        }

        private void ColorTag(int start, int end)
        {
            string text = richTextBoxPreview.Text;
            int i = start;
            bool elementNameSeen = false;

            while (i < end)
            {
                char c = text[i];

                if (c == '<' || c == '>' || c == '/' || c == '?' || c == '=')
                {
                    ColorRange(i, 1, PunctuationColor);
                    i++;
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                if (c == '"' || c == '\'')
                {
                    char quote = c;
                    int j = i + 1;

                    while (j < end && text[j] != quote)
                    {
                        j++;
                    }

                    ColorRange(i, j - i + 1, StringColor);
                    i = j + 1;
                    continue;
                }

                int tokenEnd = i;

                while (tokenEnd < end && !char.IsWhiteSpace(text[tokenEnd]) && "\"'<>=/".IndexOf(text[tokenEnd]) < 0)
                {
                    tokenEnd++;
                }

                int lookahead = tokenEnd;

                while (lookahead < end && char.IsWhiteSpace(text[lookahead]))
                {
                    lookahead++;
                }

                bool isAttribute = lookahead < end && text[lookahead] == '=';
                Color color = isAttribute ? AttributeColor : ElementColor;

                if (!elementNameSeen)
                {
                    elementNameSeen = true;
                }

                ColorRange(i, tokenEnd - i, color);
                i = tokenEnd;
            }
        }

        private static int NumberLength(string text, int start)
        {
            int length = 0;

            while (start + length < text.Length && (char.IsDigit(text[start + length]) || text[start + length] == '.' || text[start + length] == '-' || text[start + length] == '+' || text[start + length] == 'e' || text[start + length] == 'E'))
            {
                length++;
            }

            return length;
        }

        private static int LetterLength(string text, int start)
        {
            int length = 0;

            while (start + length < text.Length && char.IsLetterOrDigit(text[start + length]))
            {
                length++;
            }

            return length;
        }

        private void ColorRange(int start, int length, Color color)
        {
            richTextBoxPreview.SelectionStart = start;
            richTextBoxPreview.SelectionLength = length;
            richTextBoxPreview.SelectionColor = color;
            richTextBoxPreview.SelectionLength = 0;
        }

        #endregion

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

        private void ChkWrap_CheckedChanged(object sender, EventArgs e)
        {
            richTextBoxPreview.WordWrap = chkWrap.Checked;
        }
    }
}
