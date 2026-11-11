using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using PdfiumViewer;

namespace FileExplorer.Controls
{
    public partial class PreviewPanel : UserControl
    {
        private const long MaxPreviewBytes = 2 * 1024 * 1024;
        private const long MaxOfficePreviewBytes = 20 * 1024 * 1024;
        private const int MaxExtractedTextChars = 1000000;
        private const int MaxTreeNodes = 10000;

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
        private PdfDocument _pdfDocument;

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

                if (IsBinary(path))
                {
                    ShowMessage("Binary file. Preview is not available.");
                    return;
                }

                if (info.Length > MaxPreviewBytes)
                {
                    ShowMessage("File is too large to preview (limit is 2 MB).");
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
            richTextBoxPreview.Text = "Select a single file to preview it here.\r\n\r\nSupported: text, JSON, XML, HTML.";
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
            try
            {
                btnTree.Visible = false;
                btnText.Visible = false;
                treeViewPreview.Visible = false;
                richTextBoxPreview.Visible = false;
                chkWrap.Visible = false;
                webBrowserPreview.Visible = false;
                pdfViewerPreview.Visible = true;

                PdfDocument document = PdfDocument.Load(path);

                if (_pdfDocument != null)
                {
                    _pdfDocument.Dispose();
                }

                _pdfDocument = document;
                pdfViewerPreview.Document = document;
            }
            catch (Exception ex)
            {
                ShowMessage("Cannot preview this PDF.\r\n\r\n" + ex.Message);
            }
        }

        private void ShowOfficePreview(string path)
        {
            int sequence = ++_previewSequence;
            ShowMessage("Extracting content for preview...");
            Cursor = Cursors.WaitCursor;

            var thread = new Thread(() =>
            {
                string text = TryExtractOfficeText(path);
                string htmlPath = null;
                bool converted = false;

                if (string.IsNullOrWhiteSpace(text))
                {
                    converted = TryConvertOfficeToHtml(path, out htmlPath);
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

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            ShowText(text, false, false);
                        }
                        else if (converted && File.Exists(htmlPath))
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

        private static string TryExtractOfficeText(string path)
        {
            try
            {
                string extension = Path.GetExtension(path).ToLowerInvariant();
                string text;

                if (extension == ".docx")
                {
                    text = ExtractDocxText(path);
                }
                else if (extension == ".xls" || extension == ".xlsx")
                {
                    text = ExtractWorkbookText(path);
                }
                else
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    return null;
                }

                if (text.Length > MaxExtractedTextChars)
                {
                    text = text.Substring(0, MaxExtractedTextChars) + "\r\n\r\n(... preview truncated ...)";
                }

                return text;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string ExtractDocxText(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var document = new XWPFDocument(stream);
                var builder = new StringBuilder();

                foreach (XWPFParagraph paragraph in document.Paragraphs)
                {
                    builder.AppendLine(paragraph.Text);
                }

                foreach (XWPFTable table in document.Tables)
                {
                    foreach (XWPFTableRow row in table.Rows)
                    {
                        foreach (XWPFTableCell cell in row.GetTableCells())
                        {
                            builder.Append(cell.GetText());
                            builder.Append('\t');
                        }

                        builder.AppendLine();
                    }
                }

                return builder.ToString();
            }
        }

        private static string ExtractWorkbookText(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                IWorkbook workbook = WorkbookFactory.Create(stream);
                var builder = new StringBuilder();

                for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    builder.AppendLine("[" + sheet.SheetName + "]");

                    foreach (IRow row in sheet)
                    {
                        foreach (NPOI.SS.UserModel.ICell cell in row)
                        {
                            builder.Append(cell.ToString());
                            builder.Append('\t');
                        }

                        builder.AppendLine();
                    }

                    builder.AppendLine();
                }

                return builder.ToString();
            }
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
