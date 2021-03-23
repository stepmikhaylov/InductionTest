using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InductionTest.FtpTool.Common;

namespace InductionTest.FtpTool.Ftp
{
    /// <summary>
    /// Provides remote file, supporting synchronous and asynchronous download operations via FTP.
    /// </summary>
    public class FtpFile : FtpEntry
    {
        public FtpFile(FtpDirectory parent, string name, DateTime lastWriteTime, long length)
            : base(parent, name, lastWriteTime)
        {
            Extension = Path.GetExtension(name);
            Length = length;
        }

        public override bool IsDirectory => false;
        public string Extension { get; }
        public long Length { get; }

        /// <summary>
        /// Downloads remote file to local one.
        /// </summary>
        public void Download(string filePath)
        {
            using (var context = MakeRequest(WebRequestMethods.Ftp.DownloadFile))
            using (var fileStream = File.Create(filePath))
            {
                context.ResponseStream.CopyTo(fileStream);
            }
            File.SetCreationTime(filePath, LastWriteTime);
            File.SetLastWriteTime(filePath, LastWriteTime);
        }

        /// <summary>
        /// Asynchronously downloads remote file to local one.<br/>
        /// Obsolete, use <see cref="DownloadAsync(string)"/> instead.
        /// </summary>
        [Obsolete("Use DownloadAsync")]
        public async Task DownloadAsync(
            string filePath,
            IProgress<FtpDownloadProgressValue> progress,
            CancellationToken cancellationToken)
        {
            using (var context = MakeRequest(WebRequestMethods.Ftp.DownloadFile))
            using (var fileStream = File.Create(filePath))
            {
                var progressValue = new FtpDownloadProgressValue(this, progress);
                await context.ResponseStream.CopyToAsync(fileStream, progressValue, cancellationToken);
            }
            File.SetCreationTime(filePath, LastWriteTime);
            File.SetLastWriteTime(filePath, LastWriteTime);
        }

        /// <summary>
        /// Asynchronously downloads remote file to local one.<br/>
        /// </summary>
        public async IAsyncEnumerable<int> DownloadAsync(string filePath)
        {
            using (var context = MakeRequest(WebRequestMethods.Ftp.DownloadFile))
            using (var fileStream = File.Create(filePath))
            {
                await foreach (int bytesWritten in context.ResponseStream.EnumerableCopyToAsync(fileStream))
                    yield return bytesWritten;
            }
            File.SetCreationTime(filePath, LastWriteTime);
            File.SetLastWriteTime(filePath, LastWriteTime);
        }
    }
}
