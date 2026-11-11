namespace FileExplorer.Controls
{
    partial class PreviewPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            CleanupTempPreview();

            if (_pdfDocument != null)
            {
                _pdfDocument.Dispose();
                _pdfDocument = null;
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelPreviewFile = new System.Windows.Forms.Label();
            this.chkWrap = new System.Windows.Forms.CheckBox();
            this.btnText = new System.Windows.Forms.Button();
            this.btnTree = new System.Windows.Forms.Button();
            this.richTextBoxPreview = new System.Windows.Forms.RichTextBox();
            this.treeViewPreview = new System.Windows.Forms.TreeView();
            this.contextMenuStripTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowserPreview = new System.Windows.Forms.WebBrowser();
            this.pdfViewerPreview = new PdfiumViewer.PdfViewer();
            this.panelTop.SuspendLayout();
            this.contextMenuStripTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelPreviewFile);
            this.panelTop.Controls.Add(this.chkWrap);
            this.panelTop.Controls.Add(this.btnText);
            this.panelTop.Controls.Add(this.btnTree);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(300, 26);
            this.panelTop.TabIndex = 0;
            // 
            // labelPreviewFile
            // 
            this.labelPreviewFile.AutoEllipsis = true;
            this.labelPreviewFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPreviewFile.Location = new System.Drawing.Point(0, 0);
            this.labelPreviewFile.Name = "labelPreviewFile";
            this.labelPreviewFile.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.labelPreviewFile.Size = new System.Drawing.Size(123, 26);
            this.labelPreviewFile.TabIndex = 0;
            this.labelPreviewFile.Text = "No preview";
            this.labelPreviewFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkWrap
            // 
            this.chkWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWrap.Checked = true;
            this.chkWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWrap.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkWrap.Location = new System.Drawing.Point(241, 0);
            this.chkWrap.Name = "chkWrap";
            this.chkWrap.Size = new System.Drawing.Size(59, 24);
            this.chkWrap.TabIndex = 1;
            this.chkWrap.Text = "Wrap";
            this.chkWrap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWrap.UseVisualStyleBackColor = true;
            this.chkWrap.CheckedChanged += new System.EventHandler(this.ChkWrap_CheckedChanged);
            // 
            // btnText
            // 
            this.btnText.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnText.Location = new System.Drawing.Point(182, 0);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(59, 24);
            this.btnText.TabIndex = 2;
            this.btnText.Text = "Text";
            this.btnText.UseVisualStyleBackColor = true;
            this.btnText.Visible = false;
            this.btnText.Click += new System.EventHandler(this.BtnText_Click);
            // 
            // btnTree
            // 
            this.btnTree.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTree.Location = new System.Drawing.Point(123, 0);
            this.btnTree.Name = "btnTree";
            this.btnTree.Size = new System.Drawing.Size(59, 24);
            this.btnTree.TabIndex = 3;
            this.btnTree.Text = "Tree";
            this.btnTree.UseVisualStyleBackColor = true;
            this.btnTree.Visible = false;
            this.btnTree.Click += new System.EventHandler(this.BtnTree_Click);
            // 
            // richTextBoxPreview
            // 
            this.richTextBoxPreview.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxPreview.DetectUrls = false;
            this.richTextBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxPreview.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.richTextBoxPreview.HideSelection = false;
            this.richTextBoxPreview.Location = new System.Drawing.Point(0, 26);
            this.richTextBoxPreview.Name = "richTextBoxPreview";
            this.richTextBoxPreview.ReadOnly = true;
            this.richTextBoxPreview.Size = new System.Drawing.Size(300, 424);
            this.richTextBoxPreview.TabIndex = 1;
            this.richTextBoxPreview.Text = "";
            this.richTextBoxPreview.WordWrap = true;
            // 
            // treeViewPreview
            // 
            this.treeViewPreview.ContextMenuStrip = this.contextMenuStripTree;
            this.treeViewPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPreview.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewPreview.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.treeViewPreview.HideSelection = false;
            this.treeViewPreview.ItemHeight = 17;
            this.treeViewPreview.Location = new System.Drawing.Point(0, 26);
            this.treeViewPreview.Name = "treeViewPreview";
            this.treeViewPreview.Size = new System.Drawing.Size(300, 424);
            this.treeViewPreview.TabIndex = 3;
            this.treeViewPreview.Visible = false;
            this.treeViewPreview.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.TreeViewPreview_DrawNode);
            // 
            // contextMenuStripTree
            // 
            this.contextMenuStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem});
            this.contextMenuStripTree.Name = "contextMenuStripTree";
            this.contextMenuStripTree.Size = new System.Drawing.Size(140, 48);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.expandAllToolStripMenuItem.Text = "Expand all";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.ExpandAllTreeToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse all";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.CollapseAllTreeToolStripMenuItem_Click);
            // 
            // webBrowserPreview
            // 
            this.webBrowserPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserPreview.Location = new System.Drawing.Point(0, 26);
            this.webBrowserPreview.Name = "webBrowserPreview";
            this.webBrowserPreview.Size = new System.Drawing.Size(300, 424);
            this.webBrowserPreview.TabIndex = 2;
            this.webBrowserPreview.Visible = false;
            // 
            // pdfViewerPreview
            // 
            this.pdfViewerPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewerPreview.Location = new System.Drawing.Point(0, 26);
            this.pdfViewerPreview.Name = "pdfViewerPreview";
            this.pdfViewerPreview.Size = new System.Drawing.Size(300, 424);
            this.pdfViewerPreview.TabIndex = 4;
            this.pdfViewerPreview.Visible = false;
            // 
            // PreviewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBoxPreview);
            this.Controls.Add(this.treeViewPreview);
            this.Controls.Add(this.webBrowserPreview);
            this.Controls.Add(this.pdfViewerPreview);
            this.Controls.Add(this.panelTop);
            this.Name = "PreviewPanel";
            this.Size = new System.Drawing.Size(300, 450);
            this.panelTop.ResumeLayout(false);
            this.contextMenuStripTree.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelPreviewFile;
        private System.Windows.Forms.CheckBox chkWrap;
        private System.Windows.Forms.Button btnText;
        private System.Windows.Forms.Button btnTree;
        private System.Windows.Forms.RichTextBox richTextBoxPreview;
        private System.Windows.Forms.TreeView treeViewPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTree;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowserPreview;
        private PdfiumViewer.PdfViewer pdfViewerPreview;
    }
}
