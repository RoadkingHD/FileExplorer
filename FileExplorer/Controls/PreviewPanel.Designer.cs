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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelPreviewFile = new System.Windows.Forms.Label();
            this.chkWrap = new System.Windows.Forms.CheckBox();
            this.richTextBoxPreview = new System.Windows.Forms.RichTextBox();
            this.webBrowserPreview = new System.Windows.Forms.WebBrowser();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelPreviewFile);
            this.panelTop.Controls.Add(this.chkWrap);
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
            this.labelPreviewFile.Size = new System.Drawing.Size(221, 26);
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
            this.chkWrap.Location = new System.Drawing.Point(221, 0);
            this.chkWrap.Name = "chkWrap";
            this.chkWrap.Size = new System.Drawing.Size(79, 24);
            this.chkWrap.TabIndex = 1;
            this.chkWrap.Text = "Wrap";
            this.chkWrap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWrap.UseVisualStyleBackColor = true;
            this.chkWrap.CheckedChanged += new System.EventHandler(this.ChkWrap_CheckedChanged);
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
            // webBrowserPreview
            // 
            this.webBrowserPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserPreview.Location = new System.Drawing.Point(0, 26);
            this.webBrowserPreview.Name = "webBrowserPreview";
            this.webBrowserPreview.Size = new System.Drawing.Size(300, 424);
            this.webBrowserPreview.TabIndex = 2;
            this.webBrowserPreview.Visible = false;
            // 
            // PreviewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBoxPreview);
            this.Controls.Add(this.webBrowserPreview);
            this.Controls.Add(this.panelTop);
            this.Name = "PreviewPanel";
            this.Size = new System.Drawing.Size(300, 450);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelPreviewFile;
        private System.Windows.Forms.CheckBox chkWrap;
        private System.Windows.Forms.RichTextBox richTextBoxPreview;
        private System.Windows.Forms.WebBrowser webBrowserPreview;
    }
}
