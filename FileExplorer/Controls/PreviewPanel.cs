using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FileExplorer.Controls
{
    public partial class PreviewPanel : UserControl
    {
        private const long MaxPreviewBytes = 2 * 1024 * 1024;

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

                if (HtmlExtensions.Contains(extension))
                {
                    ShowHtml(ReadText(path));
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

                string content = ReadText(path);

                if (extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    ShowText(PrettyPrintJson(content), true);
                }
                else if (XmlExtensions.Contains(extension))
                {
                    ShowText(IndentXml(content), false);
                }
                else
                {
                    ShowText(content, false);
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

            webBrowserPreview.Visible = false;
            richTextBoxPreview.Visible = true;
            richTextBoxPreview.Clear();
            richTextBoxPreview.ForeColor = SystemColors.GrayText;
            richTextBoxPreview.Text = "Select a single file to preview it here.\r\n\r\nSupported: text, JSON, XML, HTML.";
        }

        private void ShowText(string content, bool highlightJson)
        {
            webBrowserPreview.Visible = false;
            richTextBoxPreview.Visible = true;
            richTextBoxPreview.ForeColor = SystemColors.WindowText;
            richTextBoxPreview.Text = content;
            richTextBoxPreview.SelectionStart = 0;
            richTextBoxPreview.SelectionLength = 0;

            if (highlightJson)
            {
                HighlightJson();
            }
        }

        private void ShowHtml(string content)
        {
            richTextBoxPreview.Visible = false;
            webBrowserPreview.Visible = true;
            webBrowserPreview.DocumentText = content;
        }

        private void ShowMessage(string message)
        {
            webBrowserPreview.Visible = false;
            richTextBoxPreview.Visible = true;
            richTextBoxPreview.ForeColor = SystemColors.GrayText;
            richTextBoxPreview.Clear();
            richTextBoxPreview.Text = message;
        }

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

                        Color color = isKey ? Color.FromArgb(163, 21, 21) : Color.FromArgb(0, 100, 0);
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
                        expectValue = true;
                        break;
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
                            ColorRange(i, NumberLength(text, i), Color.FromArgb(43, 82, 163));
                        }
                        else if (char.IsLetter(c))
                        {
                            int length = LetterLength(text, i);
                            string word = text.Substring(i, length);
                            Color wordColor = word == "true" || word == "false" || word == "null"
                                ? Color.FromArgb(120, 0, 120)
                                : Color.FromArgb(43, 82, 163);
                            ColorRange(i, length, wordColor);
                            i += length - 1;
                            expectValue = false;
                        }

                        break;
                }
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
