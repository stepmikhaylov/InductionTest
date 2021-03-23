using System;

namespace InductionTest.FtpTool.Ftp
{
    /// <summary>
    /// Base class for <see cref="FtpDirectory"/> and <see cref="FtpFile"/> classes.
    /// </summary>
    public abstract class FtpEntry
    {
        protected FtpEntry(FtpDirectory parent, string name, DateTime lastWriteTime)
        {
            Parent = parent;
            Uri = $"{parent.Uri}/{name}";
            Name = name;
            LastWriteTime = lastWriteTime;
        }

        protected FtpEntry(string uri)
        {
            Uri = uri;
        }

        public FtpDirectory Parent { get; }
        public string Uri { get; }
        public string Name { get; }
        public DateTime LastWriteTime { get; }
        public abstract bool IsDirectory { get; }

        public override string ToString()
            => Uri;

        protected FtpContext MakeRequest(string method)
            => new FtpContext(Uri, method);
    }
}
