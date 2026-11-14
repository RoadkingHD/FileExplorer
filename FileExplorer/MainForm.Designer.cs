namespace FileExplorer
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zipSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.manageCustomActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripAddress = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZip = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCompare = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPreview = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelItems = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treeViewFolders = new System.Windows.Forms.TreeView();
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContent = new System.Windows.Forms.SplitContainer();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModified = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.listSmallImageList = new System.Windows.Forms.ImageList(this.components);
            this.listLargeImageList = new System.Windows.Forms.ImageList(this.components);
            this.previewPanel1 = new FileExplorer.Controls.PreviewPanel();
            this.contextMenuStripFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.contextZipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextExtractHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomActionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCopyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.contextRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextPasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextNewFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextRenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextFolderOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.contextFolderCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextFolderCutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextFolderPasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextFolderNewFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextFolderRenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.contextFolderRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContent)).BeginInit();
            this.splitContent.Panel1.SuspendLayout();
            this.splitContent.Panel2.SuspendLayout();
            this.splitContent.SuspendLayout();
            this.contextMenuStripFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "&Open folder...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewPanelToolStripMenuItem,
            this.previewPositionToolStripMenuItem,
            this.toolStripSeparator2,
            this.largeIconsToolStripMenuItem,
            this.smallIconsToolStripMenuItem,
            this.detailsToolStripMenuItem,
            this.toolStripSeparator3,
            this.refreshToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // previewPanelToolStripMenuItem
            // 
            this.previewPanelToolStripMenuItem.CheckOnClick = true;
            this.previewPanelToolStripMenuItem.Checked = true;
            this.previewPanelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewPanelToolStripMenuItem.Name = "previewPanelToolStripMenuItem";
            this.previewPanelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.previewPanelToolStripMenuItem.Text = "&Preview panel";
            this.previewPanelToolStripMenuItem.CheckedChanged += new System.EventHandler(this.PreviewPanelToolStripMenuItem_CheckedChanged);
            // 
            // previewPositionToolStripMenuItem
            // 
            this.previewPositionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewRightToolStripMenuItem,
            this.previewBottomToolStripMenuItem});
            this.previewPositionToolStripMenuItem.Name = "previewPositionToolStripMenuItem";
            this.previewPositionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.previewPositionToolStripMenuItem.Text = "Preview &position";
            // 
            // previewRightToolStripMenuItem
            // 
            this.previewRightToolStripMenuItem.Checked = true;
            this.previewRightToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewRightToolStripMenuItem.Name = "previewRightToolStripMenuItem";
            this.previewRightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.previewRightToolStripMenuItem.Text = "Docked &right";
            this.previewRightToolStripMenuItem.Click += new System.EventHandler(this.PreviewRightToolStripMenuItem_Click);
            // 
            // previewBottomToolStripMenuItem
            // 
            this.previewBottomToolStripMenuItem.Name = "previewBottomToolStripMenuItem";
            this.previewBottomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.previewBottomToolStripMenuItem.Text = "Docked &bottom";
            this.previewBottomToolStripMenuItem.Click += new System.EventHandler(this.PreviewBottomToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // largeIconsToolStripMenuItem
            // 
            this.largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
            this.largeIconsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.largeIconsToolStripMenuItem.Text = "Lar&ge icons";
            this.largeIconsToolStripMenuItem.Click += new System.EventHandler(this.LargeIconsToolStripMenuItem_Click);
            // 
            // smallIconsToolStripMenuItem
            // 
            this.smallIconsToolStripMenuItem.Name = "smallIconsToolStripMenuItem";
            this.smallIconsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.smallIconsToolStripMenuItem.Text = "Small &icons";
            this.smallIconsToolStripMenuItem.Click += new System.EventHandler(this.SmallIconsToolStripMenuItem_Click);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Checked = true;
            this.detailsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.detailsToolStripMenuItem.Text = "&Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.DetailsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.refreshToolStripMenuItem.Text = "&Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zipSelectionToolStripMenuItem,
            this.toolStripSeparator4,
            this.searchToolStripMenuItem,
            this.compareToolStripMenuItem,
            this.manageCustomActionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // zipSelectionToolStripMenuItem
            // 
            this.zipSelectionToolStripMenuItem.Name = "zipSelectionToolStripMenuItem";
            this.zipSelectionToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.zipSelectionToolStripMenuItem.Text = "&Zip selected items...";
            this.zipSelectionToolStripMenuItem.Click += new System.EventHandler(this.ZipSelectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(221, 6);
            //
            // searchToolStripMenuItem
            //
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.searchToolStripMenuItem.Text = "&Search files...";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.SearchToolStripMenuItem_Click);
            //
            // compareToolStripMenuItem
            //
            this.compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            this.compareToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.compareToolStripMenuItem.Text = "&Compare files or folders...";
            this.compareToolStripMenuItem.Click += new System.EventHandler(this.CompareToolStripMenuItem_Click);
            // 
            // manageCustomActionsToolStripMenuItem
            // 
            this.manageCustomActionsToolStripMenuItem.Name = "manageCustomActionsToolStripMenuItem";
            this.manageCustomActionsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.manageCustomActionsToolStripMenuItem.Text = "&Manage custom actions...";
            this.manageCustomActionsToolStripMenuItem.Click += new System.EventHandler(this.ManageCustomActionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBack,
            this.toolStripButtonForward,
            this.toolStripButtonUp,
            this.toolStripSeparator5,
            this.toolStripAddress,
            this.toolStripButtonGo,
            this.toolStripButtonRefresh,
            this.toolStripSeparator6,
            this.toolStripButtonZip,
            this.toolStripSeparator7,
            this.toolStripButtonSearch,
            this.toolStripButtonCompare,
            this.toolStripSeparator14,
            this.toolStripButtonPreview});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonBack
            // 
            this.toolStripButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonBack.Enabled = false;
            this.toolStripButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBack.Name = "toolStripButtonBack";
            this.toolStripButtonBack.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonBack.Text = "←";
            this.toolStripButtonBack.ToolTipText = "Back";
            this.toolStripButtonBack.Click += new System.EventHandler(this.ToolStripButtonBack_Click);
            // 
            // toolStripButtonForward
            // 
            this.toolStripButtonForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonForward.Enabled = false;
            this.toolStripButtonForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonForward.Name = "toolStripButtonForward";
            this.toolStripButtonForward.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonForward.Text = "→";
            this.toolStripButtonForward.ToolTipText = "Forward";
            this.toolStripButtonForward.Click += new System.EventHandler(this.ToolStripButtonForward_Click);
            // 
            // toolStripButtonUp
            // 
            this.toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUp.Enabled = false;
            this.toolStripButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUp.Name = "toolStripButtonUp";
            this.toolStripButtonUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUp.Text = "↑";
            this.toolStripButtonUp.ToolTipText = "Up one level";
            this.toolStripButtonUp.Click += new System.EventHandler(this.ToolStripButtonUp_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripAddress
            // 
            this.toolStripAddress.Name = "toolStripAddress";
            this.toolStripAddress.Size = new System.Drawing.Size(400, 25);
            this.toolStripAddress.ToolTipText = "Type a folder path and press Enter";
            this.toolStripAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToolStripAddress_KeyDown);
            // 
            // toolStripButtonGo
            // 
            this.toolStripButtonGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGo.Name = "toolStripButtonGo";
            this.toolStripButtonGo.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonGo.Text = "Go";
            this.toolStripButtonGo.Click += new System.EventHandler(this.ToolStripButtonGo_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRefresh.Text = "⟳";
            this.toolStripButtonRefresh.ToolTipText = "Refresh (F5)";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.ToolStripButtonRefresh_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonZip
            // 
            this.toolStripButtonZip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZip.Name = "toolStripButtonZip";
            this.toolStripButtonZip.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZip.Text = "Zip";
            this.toolStripButtonZip.ToolTipText = "Zip the selected files and folders";
            this.toolStripButtonZip.Click += new System.EventHandler(this.ToolStripButtonZip_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonPreview
            // 
            this.toolStripButtonPreview.CheckOnClick = true;
            this.toolStripButtonPreview.Checked = true;
            this.toolStripButtonPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreview.Name = "toolStripButtonPreview";
            this.toolStripButtonPreview.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPreview.Text = "Preview";
            this.toolStripButtonPreview.ToolTipText = "Show or hide the preview panel";
            this.toolStripButtonPreview.CheckedChanged += new System.EventHandler(this.ToolStripButtonPreview_CheckedChanged);
            //
            // toolStripButtonSearch
            //
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSearch.Text = "Search";
            this.toolStripButtonSearch.ToolTipText = "Search for files by name and content (Ctrl+F)";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.ToolStripButtonSearch_Click);
            //
            // toolStripButtonCompare
            //
            this.toolStripButtonCompare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCompare.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCompare.Name = "toolStripButtonCompare";
            this.toolStripButtonCompare.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCompare.Text = "Compare";
            this.toolStripButtonCompare.ToolTipText = "Compare two files or folders (text diff or hex view)";
            this.toolStripButtonCompare.Click += new System.EventHandler(this.ToolStripButtonCompare_Click);
            //
            // toolStripSeparator14
            //
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelItems});
            this.statusStrip1.Location = new System.Drawing.Point(0, 630);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelItems
            // 
            this.toolStripStatusLabelItems.Name = "toolStripStatusLabelItems";
            this.toolStripStatusLabelItems.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabelItems.Text = "Ready";
            this.toolStripStatusLabelItems.Spring = true;
            this.toolStripStatusLabelItems.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 49);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.treeViewFolders);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitContent);
            this.splitMain.Size = new System.Drawing.Size(984, 581);
            this.splitMain.SplitterWidth = 5;
            this.splitMain.TabIndex = 3;
            this.splitMain.TabStop = false;
            // 
            // treeViewFolders
            // 
            this.treeViewFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFolders.ContextMenuStrip = this.contextMenuStripFolders;
            this.treeViewFolders.HideSelection = false;
            this.treeViewFolders.ImageIndex = 0;
            this.treeViewFolders.ImageList = this.treeImageList;
            this.treeViewFolders.ItemHeight = 18;
            this.treeViewFolders.Location = new System.Drawing.Point(0, 0);
            this.treeViewFolders.Name = "treeViewFolders";
            this.treeViewFolders.SelectedImageIndex = 0;
            this.treeViewFolders.Size = new System.Drawing.Size(220, 581);
            this.treeViewFolders.TabIndex = 0;
            this.treeViewFolders.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewFolders_BeforeExpand);
            this.treeViewFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewFolders_AfterSelect);
            this.treeViewFolders.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TreeViewFolders_MouseClick);
            this.treeViewFolders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeViewFolders_KeyDown);
            // 
            // treeImageList
            // 
            this.treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.treeImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContent
            // 
            this.splitContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContent.Location = new System.Drawing.Point(0, 0);
            this.splitContent.Name = "splitContent";
            this.splitContent.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitContent.Panel1
            // 
            this.splitContent.Panel1.Controls.Add(this.listViewFiles);
            // 
            // splitContent.Panel2
            // 
            this.splitContent.Panel2.Controls.Add(this.previewPanel1);
            this.splitContent.Panel2MinSize = 180;
            this.splitContent.Size = new System.Drawing.Size(759, 581);
            this.splitContent.SplitterWidth = 5;
            this.splitContent.TabIndex = 0;
            this.splitContent.TabStop = false;
            // 
            // listViewFiles
            // 
            this.listViewFiles.AllowDrop = true;
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderModified,
            this.columnHeaderType,
            this.columnHeaderSize});
            this.listViewFiles.ContextMenuStrip = this.contextMenuStripFiles;
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.LargeImageList = this.listLargeImageList;
            this.listViewFiles.Location = new System.Drawing.Point(0, 0);
            this.listViewFiles.MultiSelect = true;
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.ShowItemToolTips = true;
            this.listViewFiles.Size = new System.Drawing.Size(759, 581);
            this.listViewFiles.SmallImageList = this.listSmallImageList;
            this.listViewFiles.TabIndex = 0;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.DoubleClick += new System.EventHandler(this.ListViewFiles_DoubleClick);
            this.listViewFiles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewFiles_ItemSelectionChanged);
            this.listViewFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListViewFiles_KeyDown);
            this.listViewFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ListViewFiles_ItemDrag);
            this.listViewFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListViewFiles_DragEnter);
            this.listViewFiles.DragOver += new System.Windows.Forms.DragEventHandler(this.ListViewFiles_DragOver);
            this.listViewFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListViewFiles_DragDrop);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 300;
            // 
            // columnHeaderModified
            // 
            this.columnHeaderModified.Text = "Date modified";
            this.columnHeaderModified.Width = 140;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 150;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.Width = 90;
            // 
            // listSmallImageList
            // 
            this.listSmallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.listSmallImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.listSmallImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listLargeImageList
            // 
            this.listLargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.listLargeImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.listLargeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // previewPanel1
            // 
            this.previewPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel1.Location = new System.Drawing.Point(0, 0);
            this.previewPanel1.Name = "previewPanel1";
            this.previewPanel1.Size = new System.Drawing.Size(300, 581);
            this.previewPanel1.TabIndex = 0;
            // 
            // contextMenuStripFiles
            // 
            this.contextMenuStripFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextOpenToolStripMenuItem,
            this.contextCopyToolStripMenuItem,
            this.contextCutToolStripMenuItem,
            this.contextPasteToolStripMenuItem,
            this.contextNewFolderToolStripMenuItem,
            this.contextRenameToolStripMenuItem,
            this.toolStripSeparator8,
            this.contextZipToolStripMenuItem,
            this.contextExtractHereToolStripMenuItem,
            this.toolStripSeparator9,
            this.contextCustomActionsMenuItem,
            this.toolStripSeparator10,
            this.contextCompareToolStripMenuItem,
            this.contextCopyPathToolStripMenuItem,
            this.contextDeleteToolStripMenuItem,
            this.toolStripSeparator11,
            this.contextRefreshToolStripMenuItem});
            this.contextMenuStripFiles.Name = "contextMenuStripFiles";
            this.contextMenuStripFiles.Size = new System.Drawing.Size(181, 376);
            this.contextMenuStripFiles.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStripFiles_Opening);
            // 
            // contextOpenToolStripMenuItem
            // 
            this.contextOpenToolStripMenuItem.Name = "contextOpenToolStripMenuItem";
            this.contextOpenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextOpenToolStripMenuItem.Text = "&Open";
            this.contextOpenToolStripMenuItem.Click += new System.EventHandler(this.ContextOpenToolStripMenuItem_Click);
            // 
            // contextCopyToolStripMenuItem
            // 
            this.contextCopyToolStripMenuItem.Name = "contextCopyToolStripMenuItem";
            this.contextCopyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.contextCopyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextCopyToolStripMenuItem.Text = "Cop&y";
            this.contextCopyToolStripMenuItem.Click += new System.EventHandler(this.ContextCopyToolStripMenuItem_Click);
            // 
            // contextCutToolStripMenuItem
            // 
            this.contextCutToolStripMenuItem.Name = "contextCutToolStripMenuItem";
            this.contextCutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.contextCutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextCutToolStripMenuItem.Text = "Cu&t";
            this.contextCutToolStripMenuItem.Click += new System.EventHandler(this.ContextCutToolStripMenuItem_Click);
            // 
            // contextPasteToolStripMenuItem
            // 
            this.contextPasteToolStripMenuItem.Name = "contextPasteToolStripMenuItem";
            this.contextPasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.contextPasteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextPasteToolStripMenuItem.Text = "&Paste";
            this.contextPasteToolStripMenuItem.Click += new System.EventHandler(this.ContextPasteToolStripMenuItem_Click);
            // 
            // contextNewFolderToolStripMenuItem
            // 
            this.contextNewFolderToolStripMenuItem.Name = "contextNewFolderToolStripMenuItem";
            this.contextNewFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextNewFolderToolStripMenuItem.Text = "New &folder";
            this.contextNewFolderToolStripMenuItem.Click += new System.EventHandler(this.ContextNewFolderToolStripMenuItem_Click);
            // 
            // contextRenameToolStripMenuItem
            // 
            this.contextRenameToolStripMenuItem.Name = "contextRenameToolStripMenuItem";
            this.contextRenameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.contextRenameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextRenameToolStripMenuItem.Text = "Re&name";
            this.contextRenameToolStripMenuItem.Click += new System.EventHandler(this.ContextRenameToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(177, 6);
            // 
            // contextZipToolStripMenuItem
            // 
            this.contextZipToolStripMenuItem.Name = "contextZipToolStripMenuItem";
            this.contextZipToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextZipToolStripMenuItem.Text = "&Zip...";
            this.contextZipToolStripMenuItem.Click += new System.EventHandler(this.ContextZipToolStripMenuItem_Click);
            // 
            // contextExtractHereToolStripMenuItem
            // 
            this.contextExtractHereToolStripMenuItem.Enabled = false;
            this.contextExtractHereToolStripMenuItem.Name = "contextExtractHereToolStripMenuItem";
            this.contextExtractHereToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextExtractHereToolStripMenuItem.Text = "&Extract here";
            this.contextExtractHereToolStripMenuItem.Click += new System.EventHandler(this.ContextExtractHereToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(177, 6);
            // 
            // contextCustomActionsMenuItem
            // 
            this.contextCustomActionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[0]);
            this.contextCustomActionsMenuItem.Enabled = false;
            this.contextCustomActionsMenuItem.Name = "contextCustomActionsMenuItem";
            this.contextCustomActionsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextCustomActionsMenuItem.Text = "C&ustom actions";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(177, 6);
            // 
            // contextCompareToolStripMenuItem
            // 
            this.contextCompareToolStripMenuItem.Enabled = false;
            this.contextCompareToolStripMenuItem.Name = "contextCompareToolStripMenuItem";
            this.contextCompareToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextCompareToolStripMenuItem.Text = "Co&mpare...";
            this.contextCompareToolStripMenuItem.Click += new System.EventHandler(this.ContextCompareToolStripMenuItem_Click);
            // 
            // contextCopyPathToolStripMenuItem
            // 
            this.contextCopyPathToolStripMenuItem.Name = "contextCopyPathToolStripMenuItem";
            this.contextCopyPathToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextCopyPathToolStripMenuItem.Text = "&Copy path";
            this.contextCopyPathToolStripMenuItem.Click += new System.EventHandler(this.ContextCopyPathToolStripMenuItem_Click);
            // 
            // contextDeleteToolStripMenuItem
            // 
            this.contextDeleteToolStripMenuItem.Name = "contextDeleteToolStripMenuItem";
            this.contextDeleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextDeleteToolStripMenuItem.Text = "&Delete";
            this.contextDeleteToolStripMenuItem.Click += new System.EventHandler(this.ContextDeleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(177, 6);
            // 
            // contextRefreshToolStripMenuItem
            // 
            this.contextRefreshToolStripMenuItem.Name = "contextRefreshToolStripMenuItem";
            this.contextRefreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextRefreshToolStripMenuItem.Text = "Re&fresh";
            this.contextRefreshToolStripMenuItem.Click += new System.EventHandler(this.ContextRefreshToolStripMenuItem_Click);
            // 
            // contextMenuStripFolders
            // 
            this.contextMenuStripFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextFolderOpenToolStripMenuItem,
            this.toolStripSeparator12,
            this.contextFolderCopyToolStripMenuItem,
            this.contextFolderCutToolStripMenuItem,
            this.contextFolderPasteToolStripMenuItem,
            this.contextFolderNewFolderToolStripMenuItem,
            this.contextFolderRenameToolStripMenuItem,
            this.toolStripSeparator13,
            this.contextFolderRefreshToolStripMenuItem});
            this.contextMenuStripFolders.Name = "contextMenuStripFolders";
            this.contextMenuStripFolders.Size = new System.Drawing.Size(181, 210);
            this.contextMenuStripFolders.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStripFolders_Opening);
            // 
            // contextFolderOpenToolStripMenuItem
            // 
            this.contextFolderOpenToolStripMenuItem.Name = "contextFolderOpenToolStripMenuItem";
            this.contextFolderOpenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderOpenToolStripMenuItem.Text = "&Open";
            this.contextFolderOpenToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderOpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(177, 6);
            // 
            // contextFolderCopyToolStripMenuItem
            // 
            this.contextFolderCopyToolStripMenuItem.Name = "contextFolderCopyToolStripMenuItem";
            this.contextFolderCopyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderCopyToolStripMenuItem.Text = "Cop&y";
            this.contextFolderCopyToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderCopyToolStripMenuItem_Click);
            // 
            // contextFolderCutToolStripMenuItem
            // 
            this.contextFolderCutToolStripMenuItem.Name = "contextFolderCutToolStripMenuItem";
            this.contextFolderCutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderCutToolStripMenuItem.Text = "Cu&t";
            this.contextFolderCutToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderCutToolStripMenuItem_Click);
            // 
            // contextFolderPasteToolStripMenuItem
            // 
            this.contextFolderPasteToolStripMenuItem.Name = "contextFolderPasteToolStripMenuItem";
            this.contextFolderPasteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderPasteToolStripMenuItem.Text = "&Paste";
            this.contextFolderPasteToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderPasteToolStripMenuItem_Click);
            // 
            // contextFolderNewFolderToolStripMenuItem
            // 
            this.contextFolderNewFolderToolStripMenuItem.Name = "contextFolderNewFolderToolStripMenuItem";
            this.contextFolderNewFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderNewFolderToolStripMenuItem.Text = "New &folder";
            this.contextFolderNewFolderToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderNewFolderToolStripMenuItem_Click);
            // 
            // contextFolderRenameToolStripMenuItem
            // 
            this.contextFolderRenameToolStripMenuItem.Name = "contextFolderRenameToolStripMenuItem";
            this.contextFolderRenameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderRenameToolStripMenuItem.Text = "Re&name";
            this.contextFolderRenameToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderRenameToolStripMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(177, 6);
            // 
            // contextFolderRefreshToolStripMenuItem
            // 
            this.contextFolderRefreshToolStripMenuItem.Name = "contextFolderRefreshToolStripMenuItem";
            this.contextFolderRefreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contextFolderRefreshToolStripMenuItem.Text = "Re&fresh";
            this.contextFolderRefreshToolStripMenuItem.Click += new System.EventHandler(this.ContextFolderRefreshToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 652);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Explorer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitContent.Panel1.ResumeLayout(false);
            this.splitContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContent)).EndInit();
            this.splitContent.ResumeLayout(false);
            this.contextMenuStripFiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zipSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem manageCustomActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;
        private System.Windows.Forms.ToolStripButton toolStripButtonForward;
        private System.Windows.Forms.ToolStripButton toolStripButtonUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripTextBox toolStripAddress;
        private System.Windows.Forms.ToolStripButton toolStripButtonGo;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonZip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ToolStripButton toolStripButtonCompare;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreview;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelItems;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treeViewFolders;
        private System.Windows.Forms.ImageList treeImageList;
        private System.Windows.Forms.SplitContainer splitContent;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderModified;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ImageList listSmallImageList;
        private System.Windows.Forms.ImageList listLargeImageList;
        private FileExplorer.Controls.PreviewPanel previewPanel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFiles;
        private System.Windows.Forms.ToolStripMenuItem contextOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem contextZipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextExtractHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem contextCustomActionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextCompareToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem contextCopyPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextDeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem contextRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextCutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextPasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextNewFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextRenameToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFolders;
        private System.Windows.Forms.ToolStripMenuItem contextFolderOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem contextFolderCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextFolderCutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextFolderPasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextFolderNewFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextFolderRenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem contextFolderRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewRightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewBottomToolStripMenuItem;
    }
}
