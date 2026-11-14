using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FileExplorer.Services;

namespace FileExplorer.Dialogs
{
    public partial class SearchDialog : Form
    {
        private readonly List<SearchResult> _pendingResults = new List<SearchResult>();
        private Thread _searchThread;
        private volatile bool _cancelRequested;
        private bool _searchRunning;
        private SearchResult _selectedResult;

        public SearchDialog()
        {
            InitializeComponent();
            resultsTimer.Start();
        }

        public string RootPath
        {
            get { return textBoxPath.Text.Trim(); }
            set { textBoxPath.Text = value ?? string.Empty; }
        }

        public SearchResult SelectedResult
        {
            get { return _selectedResult; }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the folder to search in.";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (_searchRunning)
            {
                RequestCancel();
                return;
            }

            StartSearch();
        }

        private void StartSearch()
        {
            string root = textBoxPath.Text.Trim();

            if (root.Length == 0 || !Directory.Exists(root))
            {
                MessageBox.Show(
                    this,
                    "Please choose an existing folder to search in.",
                    "Search",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (chkContents.Checked && textBoxContent.Text.Length == 0)
            {
                MessageBox.Show(
                    this,
                    "Please enter the text to search for, or clear the 'Search file contents' checkbox.",
                    "Search",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var options = new SearchOptions
            {
                RootPath = root,
                NamePattern = string.IsNullOrWhiteSpace(textBoxPattern.Text) ? "*" : textBoxPattern.Text.Trim(),
                SearchContents = chkContents.Checked,
                ContentText = textBoxContent.Text,
                MatchCase = chkMatchCase.Checked
            };

            listViewResults.Items.Clear();
            _pendingResults.Clear();
            _cancelRequested = false;
            _searchRunning = true;
            btnSearch.Text = "Stop";
            lblStatus.Text = "Searching...";
            Cursor = Cursors.WaitCursor;

            int maxResults = options.MaxResults;

            _searchThread = new Thread(() =>
            {
                try
                {
                    SearchService.Search(
                        options,
                        result =>
                        {
                            lock (_pendingResults)
                            {
                                if (_pendingResults.Count < maxResults)
                                {
                                    _pendingResults.Add(result);
                                }
                            }
                        },
                        () => _cancelRequested);
                }
                catch (Exception)
                {
                }

                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        _searchRunning = false;
                        Cursor = Cursors.Default;
                        btnSearch.Text = "Search";
                        lblStatus.Text = _cancelRequested
                            ? "Search canceled. " + listViewResults.Items.Count + " result(s) shown."
                            : listViewResults.Items.Count + " result(s) found.";
                    }));
                }
                catch (InvalidOperationException)
                {
                }
            });

            _searchThread.IsBackground = true;
            _searchThread.Start();
        }

        private void RequestCancel()
        {
            _cancelRequested = true;
            lblStatus.Text = "Stopping...";
        }

        private void ResultsTimer_Tick(object sender, EventArgs e)
        {
            lock (_pendingResults)
            {
                if (_pendingResults.Count == 0)
                {
                    return;
                }

                listViewResults.BeginUpdate();

                foreach (SearchResult result in _pendingResults)
                {
                    var item = new ListViewItem(result.Name);
                    item.SubItems.Add(result.LineNumber > 0 ? result.LineNumber.ToString() : string.Empty);
                    item.SubItems.Add(result.LinePreview);
                    item.SubItems.Add(result.Directory);
                    item.SubItems.Add(FormatSize(result.Length));
                    item.SubItems.Add(result.Modified.ToString("yyyy-MM-dd HH:mm"));
                    item.Tag = result;
                    listViewResults.Items.Add(item);
                }

                _pendingResults.Clear();
                listViewResults.EndUpdate();

                if (_searchRunning)
                {
                    lblStatus.Text = "Searching... " + listViewResults.Items.Count + " result(s) so far.";
                }
                else
                {
                    lblStatus.Text = _cancelRequested
                        ? "Search canceled. " + listViewResults.Items.Count + " result(s) shown."
                        : listViewResults.Items.Count + " result(s) found.";
                }
            }
        }

        private void ListViewResults_DoubleClick(object sender, EventArgs e)
        {
            AcceptSelectedResult();
        }

        private void ListViewResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AcceptSelectedResult();
            }
        }

        private void AcceptSelectedResult()
        {
            if (listViewResults.SelectedItems.Count == 0)
            {
                return;
            }

            _selectedResult = listViewResults.SelectedItems[0].Tag as SearchResult;

            if (_selectedResult != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ChkContents_CheckedChanged(object sender, EventArgs e)
        {
            lblContent.Enabled = chkContents.Checked;
            textBoxContent.Enabled = chkContents.Checked;
            chkMatchCase.Enabled = chkContents.Checked;
        }

        private void SearchDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_searchRunning)
            {
                _cancelRequested = true;

                if (e.CloseReason == CloseReason.UserClosing && _searchThread != null)
                {
                    _searchThread.Join(3000);
                }
            }
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
