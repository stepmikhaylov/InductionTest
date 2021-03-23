using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using InductionTest.FtpTool.Common;

namespace InductionTest.FtpTool.Ftp
{
    /// <summary>
    /// Provides remote directory, supporting enumeration operations via FTP.
    /// </summary>
    public class FtpDirectory : FtpEntry
    {
        private FtpDirectory(string uri)
            : base(uri)
        { }

        private FtpDirectory(FtpDirectory parent, string name, DateTime lastWriteTime)
            : base(parent, name, lastWriteTime)
        { }

        public override bool IsDirectory => true;

        /// <summary>
        /// Connects to remote directory.
        /// </summary>
        /// <param name="uri">FTP address in format ftp://x.x.x.x</param>
        public static FtpDirectory Connect(string uri)
        {
            var _uri = new Uri(uri);
            if (_uri.Scheme != System.Uri.UriSchemeFtp)
                throw new Exception("Only FTP connections supported");
            var ftpDir = new FtpDirectory($"{_uri.Scheme}://{_uri.Authority}");
            // request root directory to validate connection
            ftpDir.Enumerate().GetEnumerator().MoveNext();
            return ftpDir;
        }

        /// <summary>
        /// Enumerates content of remote directory.<br/>
        /// Based on FTP LIST command due to testing on IIS FTP server that does not support MLSD command.<br/>
        /// TODO: Consider using FTP MLSD command if server supports.
        /// </summary>
        public IEnumerable<FtpEntry> Enumerate()
        {
            using (var context = MakeRequest(WebRequestMethods.Ftp.ListDirectoryDetails))
            using (var reader = new StreamReader(context.ResponseStream))
            {
                foreach (var entryInfo in reader.ReadLines())
                    yield return Parse(entryInfo);
            }
        }

        /// <example>
        /// 02-08-21  03:45PM                19874 certificates.zip<br/>
        /// 03-19-21  03:52PM                 1000 1000b.dat<br/>
        /// 03-18-21  07:59PM       &lt;DIR&gt;          mail_ext<br/>
        /// </example>
        static readonly Regex EntryRegex = new Regex(
            @"^\s*(?<date>\d\d-\d\d-\d\d)\s+(?<time>\d\d:\d\d(AM|PM))\s+((?<length>\d+)|<DIR>)\s+(?<name>.+?)\s*$");

        /// <summary>
        /// Parses string line of result of FTP LIST command that provides information about remote file or directory.
        /// </summary>
        /// <returns>
        /// Either <see cref="FtpFile"/> or <see cref="FtpDirectory"/> object.
        /// </returns>
        FtpEntry Parse(string entryInfo)
        {
            var m = EntryRegex.Match(entryInfo);
            if (!m.Success)
                throw new ArgumentException($"Unknown FTP entry string: {entryInfo}");
            string name = m.Groups["name"].Value;
            DateTime lastWriteTime = DateTime.ParseExact(
                m.Groups["date"].Value + " " + m.Groups["time"].Value,
                "MM-dd-yy hh:mmtt",
                CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(m.Groups["length"].Value))
            {
                long length = long.Parse(m.Groups["length"].Value);
                return new FtpFile(this, name, lastWriteTime, length);
            }
            else
            {
                return new FtpDirectory(this, name, lastWriteTime);
            }
        }
    }
}
