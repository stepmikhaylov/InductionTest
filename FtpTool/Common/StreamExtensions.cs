using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InductionTest.FtpTool.Common
{
    static class StreamExtensions
    {
        /// <summary>
        /// Read lines from <see cref="StreamReader"/>
        /// </summary>
        public static IEnumerable<string> ReadLines(this StreamReader reader)
        {
            string line;
            while (null != (line = reader.ReadLine()))
                yield return line;
        }

        /// <summary>
        /// Asynchronously copies one stream to another one, monitors cancellation requests.<br/>
        /// Obsolete, use <see cref="EnumerableCopyToAsync"/> instead.
        /// </summary>
        [Obsolete("Use EnumerableCopyToAsync")]
        public static async Task CopyToAsync(
            this Stream source,
            Stream destination,
            IProgress<long> progress,
            CancellationToken cancellationToken = default(CancellationToken),
            int bufferSize = DefaultBufferSize)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            long totalBytes = 0;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                totalBytes += bytesRead;
                progress.Report(totalBytes);
            }
        }
        /// <summary>
        /// Asynchronously copies one stream to another one.
        /// </summary>
        public static async IAsyncEnumerable<int> EnumerableCopyToAsync(
            this Stream source,
            Stream destination,
            int bufferSize = DefaultBufferSize)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead);
                yield return bytesRead;
            }
        }

        const int DefaultBufferSize = 1024 * 1024; /*1M*/
    }
}
