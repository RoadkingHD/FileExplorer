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
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabPageTree = new System.Windows.Forms.TabPage();
            this.treeViewResults = new System.Windows.Forms.TreeView();
            this.tabPageList = new System.Windows.Forms.TabPage();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.columnStatus = new System.Windows.Forms.ColumnHeader();
            this.columnPath = new System.Windows.Forms.ColumnHeader();
            this.columnSizeLeft = new System.Windows.Forms.ColumnHeader();
            this.columnSizeRight = new System.Windows.Forms.ColumnHeader();
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
            this.tabControlResults.SuspendLayout();
            this.tabPageTree.SuspendLayout();
            this.tabPageList.SuspendLayout();
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
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSummary.AutoEllipsis = true;
            this.lblSummary.Location = new System.Drawing.Point(12, 69);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(660, 18);
            this.lblSummary.TabIndex = 8;
            this.lblSummary.Text = "Choose two files or two folders and click Compare.";
            //
            // tabControlResults
            //
            this.tabControlResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlResults.Controls.Add(this.tabPageTree);
            this.tabControlResults.Controls.Add(this.tabPageList);
            this.tabControlResults.Location = new System.Drawing.Point(12, 95);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(814, 445);
            this.tabControlResults.TabIndex = 9;
            this.tabControlResults.Visible = false;
            //
            // tabPageTree
            //
            this.tabPageTree.Controls.Add(this.treeViewResults);
            this.tabPageTree.Location = new System.Drawing.Point(4, 22);
            this.tabPageTree.Name = "tabPageTree";
            this.tabPageTree.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTree.Size = new System.Drawing.Size(806, 419);
            this.tabPageTree.TabIndex = 0;
            this.tabPageTree.Text = "Tree view";
            this.tabPageTree.UseVisualStyleBackColor = true;
            //
            // treeViewResults
            //
            this.treeViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewResults.HideSelection = false;
            this.treeViewResults.Location = new System.Drawing.Point(3, 3);
            this.treeViewResults.Name = "treeViewResults";
            this.treeViewResults.ShowNodeToolTips = true;
            this.treeViewResults.Size = new System.Drawing.Size(800, 413);
            this.treeViewResults.TabIndex = 0;
            this.treeViewResults.DoubleClick += new System.EventHandler(this.TreeViewResults_DoubleClick);
            //
            // tabPageList
            //
            this.tabPageList.Controls.Add(this.listViewResults);
            this.tabPageList.Location = new System.Drawing.Point(4, 22);
            this.tabPageList.Name = "tabPageList";
            this.tabPageList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageList.Size = new System.Drawing.Size(806, 419);
            this.tabPageList.TabIndex = 1;
            this.tabPageList.Text = "List view";
            this.tabPageList.UseVisualStyleBackColor = true;
            //
            // listViewResults
            //
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnStatus,
            this.columnPath,
            this.columnSizeLeft,
            this.columnSizeRight});
            this.listViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(3, 3);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.ShowItemToolTips = true;
            this.listViewResults.Size = new System.Drawing.Size(800, 413);
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.DoubleClick += new System.EventHandler(this.ListViewResults_DoubleClick);
            //
            // columnStatus
            //
            this.columnStatus.Text = "Status";
            this.columnStatus.Width = 110;
            //
            // columnPath
            //
            this.columnPath.Text = "Relative path";
            this.columnPath.Width = 430;
            //
            // columnSizeLeft
            //
            this.columnSizeLeft.Text = "Size (left)";
            this.columnSizeLeft.Width = 90;
            this.columnSizeLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // columnSizeRight
            //
            this.columnSizeRight.Text = "Size (right)";
            this.columnSizeRight.Width = 90;
            this.columnSizeRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.Controls.Add(this.tabControlResults);
            this.MinimumSize = new System.Drawing.Size(640, 420);
            this.Name = "CompareDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compare files or folders";
            this.tabControlResults.ResumeLayout(false);
            this.tabPageTree.ResumeLayout(false);
            this.tabPageList.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabPageTree;
        private System.Windows.Forms.TreeView treeViewResults;
        private System.Windows.Forms.TabPage tabPageList;
        private System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.ColumnHeader columnStatus;
        private System.Windows.Forms.ColumnHeader columnPath;
        private System.Windows.Forms.ColumnHeader columnSizeLeft;
        private System.Windows.Forms.ColumnHeader columnSizeRight;
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
    }
}
