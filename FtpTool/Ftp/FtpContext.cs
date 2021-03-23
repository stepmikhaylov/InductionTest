using System;
using System.IO;
using System.Net;

namespace InductionTest.FtpTool.Ftp
{
    /// <summary>
    /// FTP request context: request and response.
    /// </summary>
    public class FtpContext : IDisposable
    {
        public FtpContext(string uri, string method)
        {
            Request = (FtpWebRequest)WebRequest.Create(uri);
            Request.Method = method;
            Response = (FtpWebResponse)Request.GetResponse();
            ResponseStream = Response.GetResponseStream();
        }

        public void Dispose()
        {
            Response?.Dispose();
        }

        public FtpWebRequest Request { get; }
        public string Method { get; }
        public FtpWebResponse Response { get; }
        public Stream ResponseStream { get; }
    }
}
