using System;
using InductionTest.FtpTool.Common;

namespace InductionTest.FtpTool.Ftp
{
    /// <summary>
    /// Information about progress of asynchronous download operation with <see cref="FtpFile"/>.
    /// </summary>
    [Obsolete]
    public class FtpDownloadProgressValue : IProgress<long>
    {
        public FtpDownloadProgressValue(FtpFile file, IProgress<FtpDownloadProgressValue> progress)
        {
            File = file;
            Progress = progress;
            StartTime = DateTime.Now;
        }

        public FtpFile File { get; }
        public IProgress<FtpDownloadProgressValue> Progress { get; }
        public long Length => File.Length;
        public long DownloadLength { get; private set; }
        public double Persantage => Utils.SafeDiv(DownloadLength, Length);
        public double DownloadLengthPerSecond => Utils.SafeDiv(DownloadLength, DownloadDuration.TotalSeconds);
        public DateTime StartTime { get; }
        public DateTime CurrentTime { get; private set; }
        public DateTime EndTime => StartTime + Duration;
        public TimeSpan DownloadDuration => CurrentTime - StartTime;
        public TimeSpan Duration => TimeSpan.FromSeconds(Utils.SafeDiv(Length, DownloadLengthPerSecond));
        public TimeSpan RemainDuration => EndTime - CurrentTime;

        void IProgress<long>.Report(long value)
        {
            CurrentTime = DateTime.Now;
            DownloadLength = value;
            Progress.Report(this);
        }
    }
}
