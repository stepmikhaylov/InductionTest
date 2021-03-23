using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using InductionTest.FtpTool.Ftp;
using InductionTest.FtpTool.Common;

namespace InductionTest.FtpTool
{
    public partial class DownloadForm : Form
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

            ProgressControlsHeight = okButton.Location.Y - statusLabel1.Location.Y;
            statusLabel1.Text = string.Empty;
            statusLabel2.Text = string.Empty;
            statusLabel1.Visible = false;
            statusLabel2.Visible = false;
            progressBar.Visible = false;
            Height -= ProgressControlsHeight;
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
            if (IsDownloading)
            {
                Cursor = Cursors.WaitCursor;
                cancelButton.Enabled = false;
            }
            e.Cancel = IsDownloading;
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            okButton.Enabled = false;

            string tempFileName = Path.Combine(Settings.Default.DownloadsFolderPath, $"~ftp.{DateTime.Now.Ticks}.tmp");
            try
            {
                IsDownloading = true;

                DateTime
                    startTime = DateTime.Now,
                    lastUpdateProgressTime = DateTime.MinValue,
                    lastUpdateStatusTime = DateTime.MinValue;
                long totalDownloadLength = 0;
                await foreach (var downloadLength in FtpFile.DownloadAsync(tempFileName))
                {
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                    totalDownloadLength += downloadLength;
                    bool completed = totalDownloadLength >= FtpFile.Length;
                    double persantage = Utils.SafeDiv(totalDownloadLength, FtpFile.Length);
                    TimeSpan downloadDuration = DateTime.Now - startTime;
                    double downloadLengthPerSecond = Utils.SafeDiv(totalDownloadLength, downloadDuration.TotalSeconds);
                    TimeSpan totalDuration = TimeSpan.FromSeconds(Utils.SafeDiv(FtpFile.Length, downloadLengthPerSecond));
                    DateTime endTime = startTime + totalDuration;
                    TimeSpan remainDuration = endTime - DateTime.Now;

                    // update progress 25 times/sec
                    if (DateTime.Now - lastUpdateProgressTime > TimeSpan.FromMilliseconds(40)
                        || completed)
                    {
                        progressBar.Value = (int)Math.Round(persantage * progressBar.Maximum);
                        lastUpdateProgressTime = DateTime.Now;

                        // show progress and status only if expected duration more than 1 sec
                        if (ProgressControlsHeight > 0 && totalDuration > TimeSpan.FromSeconds(1))
                        {
                            Height += ProgressControlsHeight;
                            ProgressControlsHeight = 0;
                            statusLabel1.Visible = true;
                            statusLabel2.Visible = true;
                            progressBar.Visible = true;
                        }
                    }

                    // update status 1 time/sec or when completed
                    if (DateTime.Now - lastUpdateStatusTime > TimeSpan.FromSeconds(1)
                        || completed)
                    {
                        lastUpdateStatusTime = DateTime.Now;

                        statusLabel1.Text = string.Format(
                            "Download: {0}/s, {1}",
                            Utils.FormatFileLength(downloadLengthPerSecond),
                            Utils.FormatTime(remainDuration));
                        statusLabel2.Text = string.Format(
                            "{0}% ({1} / {2})",
                            Math.Round(persantage * 100),
                            Utils.FormatFileLength(totalDownloadLength),
                            Utils.FormatFileLength(FtpFile.Length));
                        int right = progressBar.Location.X + progressBar.Size.Width;
                        statusLabel2.Location = new Point(right - statusLabel2.Size.Width, statusLabel2.Location.Y);
                    }
                }

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
                IsDownloading = false;
                Close();
            }
        }

        FtpFile FtpFile { get; }
        int ProgressControlsHeight { get; set; }
        DateTime LastUpdateStatusTime { get; set; }
        DateTime LastUpdateProgressTime { get; set; }
        bool IsDownloading { get; set; }
        CancellationTokenSource CancellationTokenSource { get; }
            = new CancellationTokenSource();
    }
}
