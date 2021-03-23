using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using InductionTest.FtpTool.Ftp;
using InductionTest.FtpTool.Common;

namespace InductionTest.FtpTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            if (Settings.Default.MainFormWidth > 0 && Settings.Default.MainFormHeight > 0)
            {
                Size = new Size(Settings.Default.MainFormWidth, Settings.Default.MainFormHeight);
            }

            uriToolStripComboBox.Items.AddRange(
                Settings.Default.RecentUriList?
                    .Cast<string>()
                    .Where(uri => !string.IsNullOrEmpty(uri))
                    .Cast<object>()
                    .ToArray() ?? new object[0]);

            viewToolStripComboBox.Items.AddRange(Enum.GetValues(typeof(View)).Cast<object>().ToArray());
            viewToolStripComboBox.SelectedItem = Settings.Default.ContentView;

            contentListView.SmallImageList = new ImageList { ImageSize = new Size(16, 16) };
            contentListView.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
            contentListView.LargeImageList = new ImageList { ImageSize = new Size(32, 32) };
            contentListView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
            contentListView.View = Settings.Default.ContentView;

            if (Settings.Default.NameColumnHeaderWidth > 0
                && Settings.Default.SizeColumnHeaderWidth > 0
                && Settings.Default.DateColumnHeaderWidth > 0)
            {
                nameColumnHeader.Width = Settings.Default.NameColumnHeaderWidth;
                sizeColumnHeader.Width = Settings.Default.SizeColumnHeaderWidth;
                dateColumnHeader.Width = Settings.Default.DateColumnHeaderWidth;
            }


            if (string.IsNullOrEmpty(Settings.Default.DownloadsFolderPath)
                || !Directory.Exists(Settings.Default.DownloadsFolderPath))
            {
                Settings.Default.DownloadsFolderPath = Utils.GetDefaultDownloadsFolderPath();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if ((Settings.Default.RecentUriList?.Count ?? 0) > 0)
            {
                Connect(Settings.Default.RecentUriList[0]);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.Default.MainFormWidth = Width;
            Settings.Default.MainFormHeight = Height;
            Settings.Default.RecentUriList = new System.Collections.Specialized.StringCollection();
            Settings.Default.RecentUriList.AddRange(uriToolStripComboBox.Items.Cast<string>().ToArray());
            Settings.Default.ContentView = contentListView.View;
            Settings.Default.NameColumnHeaderWidth = nameColumnHeader.Width;
            Settings.Default.SizeColumnHeaderWidth = sizeColumnHeader.Width;
            Settings.Default.DateColumnHeaderWidth = dateColumnHeader.Width;
            Settings.Default.Save();

            base.OnClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int occupiedWidth = mainToolStrip.Items.Cast<ToolStripItem>().Sum(item
                => (item != uriToolStripComboBox ? item.Width : 0) + item.Margin.Horizontal);
            int restWidth = Math.Max(mainToolStrip.DisplayRectangle.Width - occupiedWidth, 300);
            uriToolStripComboBox.Size = new Size(restWidth, uriToolStripComboBox.Height);
        }

        private void uriToolStripComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(uriToolStripComboBox.Text))
            {
                Connect(uriToolStripComboBox.Text);
            }
        }

        private void uriToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uriToolStripComboBox_Lock)
                return;
            Connect((string)uriToolStripComboBox.SelectedItem);
        }

        private void contentContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            openToolStripMenuItem.Enabled = contentListView.SelectedItems.Count > 0;
            openToolStripMenuItem.Text
                = contentListView.SelectedItems.Count > 0
                    && contentListView.SelectedItems[0].Tag is FtpFile
                ? "&Download"
                : "&Open";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSelectedItem();
        }

        private void contentListView_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedItem();
        }

        private void contentListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter
                && contentListView.SelectedItems.Count > 0)
            {
                OpenSelectedItem();
            }
            else if (e.KeyCode == Keys.Back
                && CurrentDirectory?.Parent != null)
            {
                CurrentDirectory = CurrentDirectory.Parent;
                UpdateView();
            }
        }

        private void browsePrevToolStripButton_Click(object sender, EventArgs e)
        {
            --BrowseHistoryIndex;
            UpdateView();
        }

        private void browseNextToolStripButton_Click(object sender, EventArgs e)
        {
            ++BrowseHistoryIndex;
            UpdateView();
        }

        private void browseUpToolStripButton_Click(object sender, EventArgs e)
        {
            CurrentDirectory = CurrentDirectory.Parent;
            UpdateView();
        }

        private void viewToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            contentListView.View = (View)viewToolStripComboBox.SelectedItem;
        }

        void Connect(string uri)
        {
            try
            {
                using (new WaitCursorScope(this))
                {
                    var ftpDirectory = FtpDirectory.Connect(uri);
                    BrowseHistory.Clear();
                    BrowseHistoryIndex = -1;
                    CurrentDirectory = ftpDirectory;

                    try
                    {
                        uriToolStripComboBox_Lock = true;

                        uriToolStripComboBox.Items.Remove(ftpDirectory.Uri);
                        uriToolStripComboBox.Items.Insert(0, ftpDirectory.Uri);
                        uriToolStripComboBox.SelectedIndex = 0;
                    }
                    finally
                    {
                        uriToolStripComboBox_Lock = false;
                    }

                    UpdateView();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void UpdateView()
        {
            try
            {
                contentListView.Items.Clear();

                try
                {
                    uriToolStripComboBox_Lock = true;
                    uriToolStripComboBox.Text = CurrentDirectory?.Uri ?? string.Empty;
                }
                finally
                {
                    uriToolStripComboBox_Lock = false;
                }

                if (CurrentDirectory != null)
                {
                    using (new WaitCursorScope(this))
                    {
                        foreach (var ftpEntry in CurrentDirectory.Enumerate())
                        {
                            var item = CreateFtpEntryItem(ftpEntry);
                            contentListView.Items.Add(item);
                        }
                        browsePrevToolStripButton.Enabled = BrowseHistoryIndex > 0;
                        browseNextToolStripButton.Enabled = BrowseHistoryIndex < BrowseHistory.Count - 1;
                        browseUpToolStripButton.Enabled = CurrentDirectory.Parent != null;
                    }
                }
                else
                {
                    browsePrevToolStripButton.Enabled = false;
                    browseNextToolStripButton.Enabled = false;
                    browseUpToolStripButton.Enabled = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        ListViewItem CreateFtpEntryItem(FtpEntry ftpEntry)
        {
            var ftpFile = ftpEntry as FtpFile;

            var item = new ListViewItem
            {
                Tag = ftpEntry,
                Text = ftpEntry.Name,
            };
            item.SubItems.Add(new ListViewItem.ListViewSubItem
            {
                Tag = ftpFile?.Length,
                Text = Utils.FormatFileLength(ftpFile?.Length) ?? "<DIR>",
            });
            item.SubItems.Add(new ListViewItem.ListViewSubItem
            {
                Tag = ftpEntry.LastWriteTime,
                Text = $"{ftpEntry.LastWriteTime.ToShortDateString()} {ftpEntry.LastWriteTime.ToShortTimeString()}",
            });

            string iconName = ftpEntry.IsDirectory ? string.Empty
                : !string.IsNullOrEmpty(ftpFile.Extension) ? ftpFile.Extension
                : ".dat"; ;
            if (IconIndex.TryGetValue(iconName, out int index))
            {
                item.ImageIndex = index;
            }
            else
            {
                Icon smallIcon, largeIcon;
                if (ftpEntry.IsDirectory)
                {
                    smallIcon = Utils.GetStockFolderIcon(true);
                    largeIcon = Utils.GetStockFolderIcon(false);
                }
                else
                {
                    smallIcon = Utils.GetFileIcon(iconName, true);
                    largeIcon = Utils.GetFileIcon(iconName, false);
                }

                contentListView.SmallImageList.Images.Add(smallIcon);
                contentListView.LargeImageList.Images.Add(largeIcon);

                item.ImageIndex
                    = IconIndex[iconName]
                    = contentListView.SmallImageList.Images.Count - 1;
            }

            return item;
        }

        void OpenSelectedItem()
        {
            var item = contentListView.SelectedItems[0];
            if (item.Tag is FtpDirectory ftpDir)
            {
                CurrentDirectory = ftpDir;
                UpdateView();
            }
            else if (item.Tag is FtpFile ftpFile)
            {
                var downloadForm = new DownloadForm(ftpFile);
                downloadForm.ShowDialog(this);
            }
        }

        FtpDirectory CurrentDirectory
        {
            get
            {
                return 0 <= BrowseHistoryIndex && BrowseHistoryIndex < BrowseHistory.Count
                    ? BrowseHistory[BrowseHistoryIndex]
                    : null;
            }
            set
            {
                if (value != null)
                {
                    if (BrowseHistoryIndex < BrowseHistory.Count - 1)
                        BrowseHistory.RemoveRange(BrowseHistoryIndex + 1, BrowseHistory.Count - BrowseHistoryIndex - 1);
                    BrowseHistory.Add(value);
                    BrowseHistoryIndex = BrowseHistory.Count - 1;
                }
                else
                {
                    BrowseHistory.Clear();
                    BrowseHistoryIndex = -1;
                }
            }
        }

        List<FtpDirectory> BrowseHistory { get; } = new List<FtpDirectory>();
        int BrowseHistoryIndex { get; set; } = -1;
        Dictionary<string, int> IconIndex { get; } = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        bool uriToolStripComboBox_Lock = false;
    }
}
