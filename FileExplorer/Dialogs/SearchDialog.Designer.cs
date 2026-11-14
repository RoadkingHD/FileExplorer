namespace FileExplorer.Dialogs
{
    partial class SearchDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelPath = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.labelPattern = new System.Windows.Forms.Label();
            this.textBoxPattern = new System.Windows.Forms.TextBox();
            this.chkContents = new System.Windows.Forms.CheckBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnLine = new System.Windows.Forms.ColumnHeader();
            this.columnPreview = new System.Windows.Forms.ColumnHeader();
            this.columnDirectory = new System.Windows.Forms.ColumnHeader();
            this.columnSize = new System.Windows.Forms.ColumnHeader();
            this.columnModified = new System.Windows.Forms.ColumnHeader();
            this.resultsTimer = new System.Windows.Forms.Timer(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // labelPath
            //
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(12, 12);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(60, 13);
            this.labelPath.TabIndex = 0;
            this.labelPath.Text = "Search in:";
            //
            // textBoxPath
            //
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(90, 9);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(442, 20);
            this.textBoxPath.TabIndex = 1;
            //
            // btnBrowse
            //
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(538, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(60, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            //
            // labelPattern
            //
            this.labelPattern.AutoSize = true;
            this.labelPattern.Location = new System.Drawing.Point(12, 39);
            this.labelPattern.Name = "labelPattern";
            this.labelPattern.Size = new System.Drawing.Size(75, 13);
            this.labelPattern.TabIndex = 3;
            this.labelPattern.Text = "File name(s):";
            //
            // textBoxPattern
            //
            this.textBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPattern.Location = new System.Drawing.Point(90, 36);
            this.textBoxPattern.Name = "textBoxPattern";
            this.textBoxPattern.Size = new System.Drawing.Size(442, 20);
            this.textBoxPattern.TabIndex = 4;
            this.textBoxPattern.Text = "*";
            //
            // chkContents
            //
            this.chkContents.AutoSize = true;
            this.chkContents.Location = new System.Drawing.Point(15, 65);
            this.chkContents.Name = "chkContents";
            this.chkContents.Size = new System.Drawing.Size(152, 17);
            this.chkContents.TabIndex = 5;
            this.chkContents.Text = "Search file contents for:";
            this.chkContents.UseVisualStyleBackColor = true;
            this.chkContents.CheckedChanged += new System.EventHandler(this.ChkContents_CheckedChanged);
            //
            // lblContent
            //
            this.lblContent.AutoSize = true;
            this.lblContent.Enabled = false;
            this.lblContent.Location = new System.Drawing.Point(173, 66);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(34, 13);
            this.lblContent.TabIndex = 6;
            this.lblContent.Text = "Text:";
            //
            // textBoxContent
            //
            this.textBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContent.Enabled = false;
            this.textBoxContent.Location = new System.Drawing.Point(213, 63);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(254, 20);
            this.textBoxContent.TabIndex = 7;
            //
            // chkMatchCase
            //
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Enabled = false;
            this.chkMatchCase.Location = new System.Drawing.Point(480, 65);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(80, 17);
            this.chkMatchCase.TabIndex = 8;
            this.chkMatchCase.Text = "Match case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            //
            // btnSearch
            //
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(471, 90);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 26);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(538, 90);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 26);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            //
            // listViewResults
            //
            this.listViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnLine,
            this.columnPreview,
            this.columnDirectory,
            this.columnSize,
            this.columnModified});
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(12, 124);
            this.listViewResults.MultiSelect = false;
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.ShowItemToolTips = true;
            this.listViewResults.Size = new System.Drawing.Size(586, 296);
            this.listViewResults.TabIndex = 11;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.DoubleClick += new System.EventHandler(this.ListViewResults_DoubleClick);
            this.listViewResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListViewResults_KeyDown);
            //
            // columnName
            //
            this.columnName.Text = "Name";
            this.columnName.Width = 180;
            //
            // columnLine
            //
            this.columnLine.Text = "Line";
            this.columnLine.Width = 45;
            this.columnLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // columnPreview
            //
            this.columnPreview.Text = "Matching line";
            this.columnPreview.Width = 320;
            //
            // columnDirectory
            //
            this.columnDirectory.Text = "Folder";
            this.columnDirectory.Width = 260;
            //
            // columnSize
            //
            this.columnSize.Text = "Size";
            this.columnSize.Width = 70;
            this.columnSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // columnModified
            //
            this.columnModified.Text = "Date modified";
            this.columnModified.Width = 120;
            //
            // resultsTimer
            //
            this.resultsTimer.Interval = 150;
            this.resultsTimer.Tick += new System.EventHandler(this.ResultsTimer_Tick);
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(12, 425);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(586, 18);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Ready.";
            //
            // SearchDialog
            //
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(610, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.listViewResults);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.chkContents);
            this.Controls.Add(this.textBoxPattern);
            this.Controls.Add(this.labelPattern);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.labelPath);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 380);
            this.Name = "SearchDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label labelPattern;
        private System.Windows.Forms.TextBox textBoxPattern;
        private System.Windows.Forms.CheckBox chkContents;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnLine;
        private System.Windows.Forms.ColumnHeader columnPreview;
        private System.Windows.Forms.ColumnHeader columnDirectory;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ColumnHeader columnModified;
        private System.Windows.Forms.Timer resultsTimer;
        private System.Windows.Forms.Label lblStatus;
    }
}
