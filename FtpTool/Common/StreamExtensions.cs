using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InductionTest.FtpTool.Common
{
    static class StreamExtensions
    {
        public static IEnumerable<string> ReadLines(this StreamReader reader)
        {
            string line;
            while (null != (line = reader.ReadLine()))
                yield return line;
        }

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

        const int DefaultBufferSize = 1024 * 1024; /*1M*/
    }
}
