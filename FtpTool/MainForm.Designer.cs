
namespace InductionTest.FtpTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contentListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.browsePrevToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.browseNextToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.browseUpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.uriToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.viewToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.contentContextMenuStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentListView
            // 
            this.contentListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.sizeColumnHeader,
            this.dateColumnHeader});
            this.contentListView.ContextMenuStrip = this.contentContextMenuStrip;
            this.contentListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentListView.FullRowSelect = true;
            this.contentListView.HideSelection = false;
            this.contentListView.Location = new System.Drawing.Point(0, 25);
            this.contentListView.MultiSelect = false;
            this.contentListView.Name = "contentListView";
            this.contentListView.Size = new System.Drawing.Size(686, 365);
            this.contentListView.TabIndex = 0;
            this.contentListView.UseCompatibleStateImageBehavior = false;
            this.contentListView.DoubleClick += new System.EventHandler(this.contentListView_DoubleClick);
            this.contentListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contentListView_KeyDown);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            // 
            // sizeColumnHeader
            // 
            this.sizeColumnHeader.Text = "Size";
            // 
            // dateColumnHeader
            // 
            this.dateColumnHeader.Text = "Date";
            // 
            // contentContextMenuStrip
            // 
            this.contentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.contentContextMenuStrip.Name = "contentContextMenuStrip";
            this.contentContextMenuStrip.Size = new System.Drawing.Size(104, 26);
            this.contentContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contentContextMenuStrip_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browsePrevToolStripButton,
            this.browseNextToolStripButton,
            this.browseUpToolStripButton,
            this.uriToolStripComboBox,
            this.viewToolStripComboBox});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(686, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "mainToolStrip";
            // 
            // browsePrevToolStripButton
            // 
            this.browsePrevToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.browsePrevToolStripButton.Enabled = false;
            this.browsePrevToolStripButton.Image = global::InductionTest.FtpTool.Properties.Resources.arrow_left;
            this.browsePrevToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.browsePrevToolStripButton.Name = "browsePrevToolStripButton";
            this.browsePrevToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.browsePrevToolStripButton.Click += new System.EventHandler(this.browsePrevToolStripButton_Click);
            // 
            // browseNextToolStripButton
            // 
            this.browseNextToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.browseNextToolStripButton.Enabled = false;
            this.browseNextToolStripButton.Image = global::InductionTest.FtpTool.Properties.Resources.arrow_right;
            this.browseNextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.browseNextToolStripButton.Name = "browseNextToolStripButton";
            this.browseNextToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.browseNextToolStripButton.Click += new System.EventHandler(this.browseNextToolStripButton_Click);
            // 
            // browseUpToolStripButton
            // 
            this.browseUpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.browseUpToolStripButton.Enabled = false;
            this.browseUpToolStripButton.Image = global::InductionTest.FtpTool.Properties.Resources.arrow_up;
            this.browseUpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.browseUpToolStripButton.Name = "browseUpToolStripButton";
            this.browseUpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.browseUpToolStripButton.Click += new System.EventHandler(this.browseUpToolStripButton_Click);
            // 
            // uriToolStripComboBox
            // 
            this.uriToolStripComboBox.DropDownWidth = 121;
            this.uriToolStripComboBox.Name = "uriToolStripComboBox";
            this.uriToolStripComboBox.Size = new System.Drawing.Size(300, 25);
            this.uriToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.uriToolStripComboBox_SelectedIndexChanged);
            this.uriToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uriToolStripComboBox_KeyDown);
            // 
            // viewToolStripComboBox
            // 
            this.viewToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewToolStripComboBox.Name = "viewToolStripComboBox";
            this.viewToolStripComboBox.Size = new System.Drawing.Size(104, 25);
            this.viewToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.viewToolStripComboBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 390);
            this.Controls.Add(this.contentListView);
            this.Controls.Add(this.mainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "FTP Tool";
            this.contentContextMenuStrip.ResumeLayout(false);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView contentListView;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader sizeColumnHeader;
        private System.Windows.Forms.ColumnHeader dateColumnHeader;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripComboBox viewToolStripComboBox;
        private System.Windows.Forms.ToolStripButton browseUpToolStripButton;
        private System.Windows.Forms.ToolStripButton browsePrevToolStripButton;
        private System.Windows.Forms.ToolStripButton browseNextToolStripButton;
        private System.Windows.Forms.ToolStripComboBox uriToolStripComboBox;
        private System.Windows.Forms.ContextMenuStrip contentContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    }
}

