using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InductionTest.FtpTool.Ftp;
using InductionTest.FtpTool.Common;

namespace InductionTest.FtpTool
{
    public partial class DownloadForm : Form, IProgress<FtpDownloadProgressValue>
    {
        public DownloadForm(FtpFile ftpFile)
        {
            InitializeComponent();

            FtpFile = ftpFile;

            sourceTextBox.Text = ftpFile.Uri;
            sourceTextBox.Select(0, -1);
            destTextBox.Text = Settings.Default.DownloadsFolderPath;

            progressBar.Minimum = 0;
            progressBar.Maximum = 1000;

            DownloadYOffset = okButton.Location.Y - statusLabel1.Location.Y;
            statusLabel1.Text = string.Empty;
            statusLabel2.Text = string.Empty;
            statusLabel1.Visible = false;
            statusLabel2.Visible = false;
            progressBar.Visible = false;
            Height -= DownloadYOffset;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            okButton.Focus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            CancellationTokenSource.Cancel();
            if (DownloadTask != null)
            {
                Cursor = Cursors.WaitCursor;
                cancelButton.Enabled = false;
            }
            e.Cancel = DownloadTask != null;
        }

        void IProgress<FtpDownloadProgressValue>.Report(FtpDownloadProgressValue value)
        {
            // update progress 25 times/sec
            if (DateTime.Now - LastUpdateProgressTime > TimeSpan.FromMilliseconds(45)
                || value.DownloadLength >= value.Length)
            {
                progressBar.Value = (int)Math.Round(value.Persantage * progressBar.Maximum);

                LastUpdateProgressTime = DateTime.Now;

                // show progress and status only if expected duration more than 1 sec
                if (DownloadYOffset > 0 && value.Duration > TimeSpan.FromSeconds(1))
                {
                    Height += DownloadYOffset;
                    statusLabel1.Visible = true;
                    statusLabel2.Visible = true;
                    progressBar.Visible = true;

                    DownloadYOffset = 0;
                }
            }

            // update status 1 time/sec or when completed
            if (DateTime.Now - LastUpdateStatusTime > TimeSpan.FromSeconds(1)
                || value.DownloadLength >= value.Length)
            {
                statusLabel1.Text = string.Format(
                    "Download: {0}/s, {1}",
                    Utils.FormatFileLength(value.DownloadLengthPerSecond),
                    Utils.FormatTime(value.RemainDuration));
                statusLabel2.Text = string.Format(
                    "{0}% ({1} / {2})",
                    Math.Round(value.Persantage * 100),
                    Utils.FormatFileLength(value.DownloadLength),
                    Utils.FormatFileLength(value.Length));
                int right = progressBar.Location.X + progressBar.Size.Width;
                statusLabel2.Location = new Point(right - statusLabel2.Size.Width, statusLabel2.Location.Y);

                LastUpdateStatusTime = DateTime.Now;
            }
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            okButton.Enabled = false;

            string tempFileName = Path.Combine(Settings.Default.DownloadsFolderPath, $"~ftp.{DateTime.Now.Ticks}.tmp");
            DownloadTask = FtpFile.DownloadAsync(tempFileName, this, CancellationTokenSource.Token);
            try
            {
                await DownloadTask;

                string downloadFileName = Path.Combine(Settings.Default.DownloadsFolderPath, FtpFile.Name);
                for (int i = 1; ; ++i)
                {
                    try
                    {
                        if (!File.Exists(downloadFileName))
                        {
                            File.Move(tempFileName, downloadFileName);
                            break;
                        }
                    }
                    catch (IOException err)
                    {
                        if (err.HResult != Win32.HResultFromWin32(Win32.ERROR_ALREADY_EXISTS))
                            throw;
                    }
                    downloadFileName = Path.Combine(
                        Settings.Default.DownloadsFolderPath,
                        $"{Path.GetFileNameWithoutExtension(FtpFile.Name)} ({i}){Path.GetExtension(FtpFile.Name)}");
                }
            }
            catch (OperationCanceledException)
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);
            }
            finally
            {
                DownloadTask = null;
                Close();
            }
        }

        FtpFile FtpFile { get; }
        int DownloadYOffset { get; set; }
        DateTime LastUpdateStatusTime { get; set; }
        DateTime LastUpdateProgressTime { get; set; }
        Task DownloadTask { get; set; }
        CancellationTokenSource CancellationTokenSource { get; }
            = new CancellationTokenSource();
    }
}
