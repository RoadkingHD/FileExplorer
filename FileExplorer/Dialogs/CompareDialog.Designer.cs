namespace FileExplorer.Dialogs
{
    partial class CompareDialog
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
            this.labelLeft = new System.Windows.Forms.Label();
            this.textBoxLeft = new System.Windows.Forms.TextBox();
            this.btnBrowseLeft = new System.Windows.Forms.Button();
            this.labelRight = new System.Windows.Forms.Label();
            this.textBoxRight = new System.Windows.Forms.TextBox();
            this.btnBrowseRight = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.splitContainerDiff = new System.Windows.Forms.SplitContainer();
            this.labelDiffLeft = new System.Windows.Forms.Label();
            this.richTextBoxDiffLeft = new System.Windows.Forms.RichTextBox();
            this.labelDiffRight = new System.Windows.Forms.Label();
            this.richTextBoxDiffRight = new System.Windows.Forms.RichTextBox();
            this.panelHex = new System.Windows.Forms.Panel();
            this.splitContainerHex = new System.Windows.Forms.SplitContainer();
            this.labelHexLeft = new System.Windows.Forms.Label();
            this.richTextBoxHexLeft = new System.Windows.Forms.RichTextBox();
            this.labelHexRight = new System.Windows.Forms.Label();
            this.richTextBoxHexRight = new System.Windows.Forms.RichTextBox();
            this.labelFilter = new System.Windows.Forms.Label();
            this.comboBoxFilter = new System.Windows.Forms.ComboBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.splitContainerFolders = new System.Windows.Forms.SplitContainer();
            this.labelFoldersLeft = new System.Windows.Forms.Label();
            this.treeViewFoldersLeft = new FileExplorer.Controls.ScrollableTreeView();
            this.labelFoldersRight = new System.Windows.Forms.Label();
            this.treeViewFoldersRight = new FileExplorer.Controls.ScrollableTreeView();
            this.panelHex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDiff)).BeginInit();
            this.splitContainerDiff.Panel1.SuspendLayout();
            this.splitContainerDiff.Panel2.SuspendLayout();
            this.splitContainerDiff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHex)).BeginInit();
            this.splitContainerHex.Panel1.SuspendLayout();
            this.splitContainerHex.Panel2.SuspendLayout();
            this.splitContainerHex.SuspendLayout();
            this.SuspendLayout();
            //
            // labelLeft
            //
            this.labelLeft.AutoSize = true;
            this.labelLeft.Location = new System.Drawing.Point(12, 12);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new System.Drawing.Size(33, 13);
            this.labelLeft.TabIndex = 0;
            this.labelLeft.Text = "Left:";
            //
            // textBoxLeft
            //
            this.textBoxLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLeft.Location = new System.Drawing.Point(60, 9);
            this.textBoxLeft.Name = "textBoxLeft";
            this.textBoxLeft.Size = new System.Drawing.Size(700, 20);
            this.textBoxLeft.TabIndex = 1;
            //
            // btnBrowseLeft
            //
            this.btnBrowseLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLeft.Location = new System.Drawing.Point(766, 7);
            this.btnBrowseLeft.Name = "btnBrowseLeft";
            this.btnBrowseLeft.Size = new System.Drawing.Size(60, 23);
            this.btnBrowseLeft.TabIndex = 2;
            this.btnBrowseLeft.Text = "Browse...";
            this.btnBrowseLeft.UseVisualStyleBackColor = true;
            this.btnBrowseLeft.Click += new System.EventHandler(this.BtnBrowseLeft_Click);
            //
            // labelRight
            //
            this.labelRight.AutoSize = true;
            this.labelRight.Location = new System.Drawing.Point(12, 39);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new System.Drawing.Size(40, 13);
            this.labelRight.TabIndex = 3;
            this.labelRight.Text = "Right:";
            //
            // textBoxRight
            //
            this.textBoxRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRight.Location = new System.Drawing.Point(60, 36);
            this.textBoxRight.Name = "textBoxRight";
            this.textBoxRight.Size = new System.Drawing.Size(700, 20);
            this.textBoxRight.TabIndex = 4;
            //
            // btnBrowseRight
            //
            this.btnBrowseRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRight.Location = new System.Drawing.Point(766, 34);
            this.btnBrowseRight.Name = "btnBrowseRight";
            this.btnBrowseRight.Size = new System.Drawing.Size(60, 23);
            this.btnBrowseRight.TabIndex = 5;
            this.btnBrowseRight.Text = "Browse...";
            this.btnBrowseRight.UseVisualStyleBackColor = true;
            this.btnBrowseRight.Click += new System.EventHandler(this.BtnBrowseRight_Click);
            //
            // btnCompare
            //
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Location = new System.Drawing.Point(691, 63);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(69, 26);
            this.btnCompare.TabIndex = 6;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(766, 63);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            //
            // lblSummary
            //
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSummary.AutoEllipsis = true;
            this.lblSummary.Location = new System.Drawing.Point(12, 69);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(350, 18);
            this.lblSummary.TabIndex = 8;
            this.lblSummary.Text = "Choose two files or two folders and click Compare.";
            //
            // labelFilter
            //
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(368, 69);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(37, 13);
            this.labelFilter.TabIndex = 12;
            this.labelFilter.Text = "Filter:";
            //
            // comboBoxFilter
            //
            this.comboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Items.AddRange(new object[] {
            "All",
            "Differences only",
            "Only in left",
            "Only in right"});
            this.comboBoxFilter.Location = new System.Drawing.Point(410, 66);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new System.Drawing.Size(125, 21);
            this.comboBoxFilter.TabIndex = 13;
            this.comboBoxFilter.SelectedIndexChanged += new System.EventHandler(this.ComboBoxFilter_SelectedIndexChanged);
            this.comboBoxFilter.SelectedIndex = 0;
            //
            // btnBack
            //
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(560, 63);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 26);
            this.btnBack.TabIndex = 14;
            this.btnBack.Text = "< Results";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Visible = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
            //
            // splitContainerFolders
            //
            this.splitContainerFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerFolders.Location = new System.Drawing.Point(12, 95);
            this.splitContainerFolders.Name = "splitContainerFolders";
            this.splitContainerFolders.Orientation = System.Windows.Forms.Orientation.Vertical;
            //
            // splitContainerFolders.Panel1
            //
            this.splitContainerFolders.Panel1.Controls.Add(this.treeViewFoldersLeft);
            this.splitContainerFolders.Panel1.Controls.Add(this.labelFoldersLeft);
            //
            // splitContainerFolders.Panel2
            //
            this.splitContainerFolders.Panel2.Controls.Add(this.treeViewFoldersRight);
            this.splitContainerFolders.Panel2.Controls.Add(this.labelFoldersRight);
            this.splitContainerFolders.Size = new System.Drawing.Size(814, 445);
            this.splitContainerFolders.SplitterDistance = 400;
            this.splitContainerFolders.TabIndex = 9;
            this.splitContainerFolders.Visible = false;
            //
            // labelFoldersLeft
            //
            this.labelFoldersLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFoldersLeft.Location = new System.Drawing.Point(0, 0);
            this.labelFoldersLeft.Name = "labelFoldersLeft";
            this.labelFoldersLeft.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.labelFoldersLeft.Size = new System.Drawing.Size(400, 22);
            this.labelFoldersLeft.TabIndex = 0;
            this.labelFoldersLeft.Text = "Left folder";
            //
            // treeViewFoldersLeft
            //
            this.treeViewFoldersLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFoldersLeft.HideSelection = false;
            this.treeViewFoldersLeft.Location = new System.Drawing.Point(0, 22);
            this.treeViewFoldersLeft.Name = "treeViewFoldersLeft";
            this.treeViewFoldersLeft.ShowNodeToolTips = true;
            this.treeViewFoldersLeft.Size = new System.Drawing.Size(400, 423);
            this.treeViewFoldersLeft.TabIndex = 1;
            this.treeViewFoldersLeft.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.FolderTree_AfterCollapse);
            this.treeViewFoldersLeft.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.FolderTree_AfterExpand);
            this.treeViewFoldersLeft.ScrollMoved += new System.EventHandler<FileExplorer.Controls.TreeViewScrollEventArgs>(this.FolderTree_Scrolled);
            this.treeViewFoldersLeft.DoubleClick += new System.EventHandler(this.FolderTree_DoubleClick);
            //
            // labelFoldersRight
            //
            this.labelFoldersRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFoldersRight.Location = new System.Drawing.Point(0, 0);
            this.labelFoldersRight.Name = "labelFoldersRight";
            this.labelFoldersRight.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.labelFoldersRight.Size = new System.Drawing.Size(410, 22);
            this.labelFoldersRight.TabIndex = 0;
            this.labelFoldersRight.Text = "Right folder";
            //
            // treeViewFoldersRight
            //
            this.treeViewFoldersRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFoldersRight.HideSelection = false;
            this.treeViewFoldersRight.Location = new System.Drawing.Point(0, 22);
            this.treeViewFoldersRight.Name = "treeViewFoldersRight";
            this.treeViewFoldersRight.ShowNodeToolTips = true;
            this.treeViewFoldersRight.Size = new System.Drawing.Size(410, 423);
            this.treeViewFoldersRight.TabIndex = 1;
            this.treeViewFoldersRight.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.FolderTree_AfterCollapse);
            this.treeViewFoldersRight.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.FolderTree_AfterExpand);
            this.treeViewFoldersRight.ScrollMoved += new System.EventHandler<FileExplorer.Controls.TreeViewScrollEventArgs>(this.FolderTree_Scrolled);
            this.treeViewFoldersRight.DoubleClick += new System.EventHandler(this.FolderTree_DoubleClick);
            //
            // splitContainerDiff
            //
            this.splitContainerDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerDiff.Location = new System.Drawing.Point(12, 95);
            this.splitContainerDiff.Name = "splitContainerDiff";
            this.splitContainerDiff.Orientation = System.Windows.Forms.Orientation.Vertical;
            //
            // splitContainerDiff.Panel1
            //
            this.splitContainerDiff.Panel1.Controls.Add(this.richTextBoxDiffLeft);
            this.splitContainerDiff.Panel1.Controls.Add(this.labelDiffLeft);
            //
            // splitContainerDiff.Panel2
            //
            this.splitContainerDiff.Panel2.Controls.Add(this.richTextBoxDiffRight);
            this.splitContainerDiff.Panel2.Controls.Add(this.labelDiffRight);
            this.splitContainerDiff.Size = new System.Drawing.Size(814, 445);
            this.splitContainerDiff.SplitterDistance = 400;
            this.splitContainerDiff.TabIndex = 10;
            this.splitContainerDiff.Visible = false;
            //
            // labelDiffLeft
            //
            this.labelDiffLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDiffLeft.Location = new System.Drawing.Point(0, 0);
            this.labelDiffLeft.Name = "labelDiffLeft";
            this.labelDiffLeft.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.labelDiffLeft.Size = new System.Drawing.Size(400, 22);
            this.labelDiffLeft.TabIndex = 0;
            this.labelDiffLeft.Text = "Left";
            //
            // richTextBoxDiffLeft
            //
            this.richTextBoxDiffLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDiffLeft.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxDiffLeft.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.richTextBoxDiffLeft.HideSelection = false;
            this.richTextBoxDiffLeft.Location = new System.Drawing.Point(0, 22);
            this.richTextBoxDiffLeft.Name = "richTextBoxDiffLeft";
            this.richTextBoxDiffLeft.ReadOnly = true;
            this.richTextBoxDiffLeft.Size = new System.Drawing.Size(400, 423);
            this.richTextBoxDiffLeft.TabIndex = 1;
            this.richTextBoxDiffLeft.Text = "";
            this.richTextBoxDiffLeft.WordWrap = false;
            this.richTextBoxDiffLeft.HScroll += new System.EventHandler(this.DiffBox_Scroll);
            this.richTextBoxDiffLeft.VScroll += new System.EventHandler(this.DiffBox_Scroll);
            //
            // labelDiffRight
            //
            this.labelDiffRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDiffRight.Location = new System.Drawing.Point(0, 0);
            this.labelDiffRight.Name = "labelDiffRight";
            this.labelDiffRight.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.labelDiffRight.Size = new System.Drawing.Size(410, 22);
            this.labelDiffRight.TabIndex = 0;
            this.labelDiffRight.Text = "Right";
            //
            // richTextBoxDiffRight
            //
            this.richTextBoxDiffRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDiffRight.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxDiffRight.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.richTextBoxDiffRight.HideSelection = false;
            this.richTextBoxDiffRight.Location = new System.Drawing.Point(0, 22);
            this.richTextBoxDiffRight.Name = "richTextBoxDiffRight";
            this.richTextBoxDiffRight.ReadOnly = true;
            this.richTextBoxDiffRight.Size = new System.Drawing.Size(410, 423);
            this.richTextBoxDiffRight.TabIndex = 1;
            this.richTextBoxDiffRight.Text = "";
            this.richTextBoxDiffRight.WordWrap = false;
            this.richTextBoxDiffRight.HScroll += new System.EventHandler(this.DiffBox_Scroll);
            this.richTextBoxDiffRight.VScroll += new System.EventHandler(this.DiffBox_Scroll);
            //
            // panelHex
            //
            this.panelHex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHex.Controls.Add(this.splitContainerHex);
            this.panelHex.Location = new System.Drawing.Point(12, 95);
            this.panelHex.Name = "panelHex";
            this.panelHex.Size = new System.Drawing.Size(814, 445);
            this.panelHex.TabIndex = 11;
            this.panelHex.Visible = false;
            //
            // splitContainerHex
            //
            this.splitContainerHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHex.Location = new System.Drawing.Point(0, 0);
            this.splitContainerHex.Name = "splitContainerHex";
            this.splitContainerHex.Orientation = System.Windows.Forms.Orientation.Vertical;
            //
            // splitContainerHex.Panel1
            //
            this.splitContainerHex.Panel1.Controls.Add(this.richTextBoxHexLeft);
            this.splitContainerHex.Panel1.Controls.Add(this.labelHexLeft);
            //
            // splitContainerHex.Panel2
            //
            this.splitContainerHex.Panel2.Controls.Add(this.richTextBoxHexRight);
            this.splitContainerHex.Panel2.Controls.Add(this.labelHexRight);
            this.splitContainerHex.Size = new System.Drawing.Size(814, 445);
            this.splitContainerHex.SplitterDistance = 400;
            this.splitContainerHex.TabIndex = 0;
            //
            // labelHexLeft
            //
            this.labelHexLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHexLeft.Location = new System.Drawing.Point(0, 0);
            this.labelHexLeft.Name = "labelHexLeft";
            this.labelHexLeft.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.labelHexLeft.Size = new System.Drawing.Size(400, 22);
            this.labelHexLeft.TabIndex = 0;
            this.labelHexLeft.Text = "Left";
            //
            // richTextBoxHexLeft
            //
            this.richTextBoxHexLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxHexLeft.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxHexLeft.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.richTextBoxHexLeft.HideSelection = false;
            this.richTextBoxHexLeft.Location = new System.Drawing.Point(0, 22);
            this.richTextBoxHexLeft.Name = "richTextBoxHexLeft";
            this.richTextBoxHexLeft.ReadOnly = true;
            this.richTextBoxHexLeft.Size = new System.Drawing.Size(400, 423);
            this.richTextBoxHexLeft.TabIndex = 1;
            this.richTextBoxHexLeft.Text = "";
            this.richTextBoxHexLeft.WordWrap = false;
            this.richTextBoxHexLeft.HScroll += new System.EventHandler(this.DiffBox_Scroll);
            this.richTextBoxHexLeft.VScroll += new System.EventHandler(this.DiffBox_Scroll);
            //
            // labelHexRight
            //
            this.labelHexRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHexRight.Location = new System.Drawing.Point(0, 0);
            this.labelHexRight.Name = "labelHexRight";
            this.labelHexRight.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.labelHexRight.Size = new System.Drawing.Size(410, 22);
            this.labelHexRight.TabIndex = 0;
            this.labelHexRight.Text = "Right";
            //
            // richTextBoxHexRight
            //
            this.richTextBoxHexRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxHexRight.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxHexRight.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.richTextBoxHexRight.HideSelection = false;
            this.richTextBoxHexRight.Location = new System.Drawing.Point(0, 22);
            this.richTextBoxHexRight.Name = "richTextBoxHexRight";
            this.richTextBoxHexRight.ReadOnly = true;
            this.richTextBoxHexRight.Size = new System.Drawing.Size(410, 423);
            this.richTextBoxHexRight.TabIndex = 1;
            this.richTextBoxHexRight.Text = "";
            this.richTextBoxHexRight.WordWrap = false;
            this.richTextBoxHexRight.HScroll += new System.EventHandler(this.DiffBox_Scroll);
            this.richTextBoxHexRight.VScroll += new System.EventHandler(this.DiffBox_Scroll);
            //
            // CompareDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(838, 552);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.comboBoxFilter);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBrowseRight);
            this.Controls.Add(this.textBoxRight);
            this.Controls.Add(this.labelRight);
            this.Controls.Add(this.btnBrowseLeft);
            this.Controls.Add(this.textBoxLeft);
            this.Controls.Add(this.labelLeft);
            this.Controls.Add(this.panelHex);
            this.Controls.Add(this.splitContainerDiff);
            this.Controls.Add(this.splitContainerFolders);
            this.MinimumSize = new System.Drawing.Size(640, 420);
            this.Name = "CompareDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compare files or folders";
            this.panelHex.ResumeLayout(false);
            this.splitContainerDiff.Panel1.ResumeLayout(false);
            this.splitContainerDiff.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDiff)).EndInit();
            this.splitContainerDiff.ResumeLayout(false);
            this.splitContainerHex.Panel1.ResumeLayout(false);
            this.splitContainerHex.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHex)).EndInit();
            this.splitContainerHex.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelLeft;
        private System.Windows.Forms.TextBox textBoxLeft;
        private System.Windows.Forms.Button btnBrowseLeft;
        private System.Windows.Forms.Label labelRight;
        private System.Windows.Forms.TextBox textBoxRight;
        private System.Windows.Forms.Button btnBrowseRight;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.SplitContainer splitContainerDiff;
        private System.Windows.Forms.Label labelDiffLeft;
        private System.Windows.Forms.RichTextBox richTextBoxDiffLeft;
        private System.Windows.Forms.Label labelDiffRight;
        private System.Windows.Forms.RichTextBox richTextBoxDiffRight;
        private System.Windows.Forms.Panel panelHex;
        private System.Windows.Forms.SplitContainer splitContainerHex;
        private System.Windows.Forms.Label labelHexLeft;
        private System.Windows.Forms.RichTextBox richTextBoxHexLeft;
        private System.Windows.Forms.Label labelHexRight;
        private System.Windows.Forms.RichTextBox richTextBoxHexRight;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.ComboBox comboBoxFilter;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.SplitContainer splitContainerFolders;
        private System.Windows.Forms.Label labelFoldersLeft;
        private FileExplorer.Controls.ScrollableTreeView treeViewFoldersLeft;
        private System.Windows.Forms.Label labelFoldersRight;
        private FileExplorer.Controls.ScrollableTreeView treeViewFoldersRight;
    }
}
