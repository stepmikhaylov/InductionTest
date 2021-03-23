using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace InductionTest.FtpTool.Common
{
    static class Utils
    {
        /// <summary>
        /// Devides numbers safelly, returns 0 if divisor is 0.
        /// </summary>
        public static double SafeDiv(double n, double div)
            => div != 0 ? n / div : 0;

        /// <summary>
        /// Converts size in bytes to string using dynamic format: x.x b/k/M/G.
        /// </summary>
        public static string FormatFileLength(double length)
        {
            if (length < 0)
                throw new ArgumentException("Negative value", nameof(length));

            char unit = '\0';
            foreach (char _unit in "bkMG")
            {
                unit = _unit;
                if (Math.Round(length, 0) < 1000 || unit == 'G')
                    break;
                length /= 1024;
            }
            int digits
                = Math.Round(length, 2) < 10 ? 2
                : Math.Round(length, 1) < 100 ? 1
                : 0;
            return $"{Math.Round(length, digits)} {unit}";
        }

        /// <summary>
        /// Converts size in bytes to string using dynamic format: x.x b/k/M/G.
        /// Returns null, if size not specified.
        /// </summary>
        public static string FormatFileLength(double? length)
            => length != null ? FormatFileLength(length.Value) : null;

        /// <summary>
        /// Converts <see cref="TimeSpan"/> to <see cref="string"/> using dynamic format: MM m SS s/HH h MM m/DD d HH h.
        /// </summary>
        public static string FormatTime(TimeSpan time)
        {
            if (time.TotalSeconds < 60)
                return $"{time.Seconds} s";
            if (time.TotalMinutes < 60)
                return time.Seconds != 0 ? $"{time.Minutes} m {time.Seconds} s" : $"{time.Minutes} m";
            if (time.TotalHours < 24)
                return time.Minutes != 0 ? $"{time.Hours} h {time.Minutes} m" : $"{time.Hours} h";
            else
                return time.Hours != 0 ? $"{(int)time.TotalDays} d {time.Hours} h" : $"{(int)time.TotalDays} d";
        }

        /// <summary>
        /// Retrieves full path of system downloads folder.
        /// </summary>
        public static string GetDefaultDownloadsFolderPath()
        {
            IntPtr handle;
            Shell32.SHGetKnownFolderPath(Shell32.KnownFolder.Downloads, 0, IntPtr.Zero, out handle);
            string res = Marshal.PtrToStringUni(handle);
            Marshal.FreeCoTaskMem(handle);
            return res;
        }

        /// <summary>
        /// Retrieves 16x16 or 32x32 system icon for specified file extension.
        /// </summary>
        public static Icon GetFileIcon(string ext, bool smallIcon)
        {
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(ext,
                Win32.FILE_ATTRIBUTE.Normal,
                ref shfi,
                (uint)Marshal.SizeOf(shfi),
                Shell32.SHGFI.Icon
                    | Shell32.SHGFI.UseFileAttributes
                    | (smallIcon ? Shell32.SHGFI.SmallIcon : Shell32.SHGFI.LargeIcon));
            Icon icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon);
            return icon;
        }

        /// <summary>
        /// Retrieves 16x16 or 32x32 system icon for folder.
        /// Obsolete, use <see cref="GetStockFolderIcon" /> instead.
        /// </summary>
        [Obsolete("Use GetStockFolderIcon")]
        public static Icon GetFolderIcon(bool smallIcon)
        {
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(null,
                Win32.FILE_ATTRIBUTE.Directory,
                ref shfi,
                (uint)Marshal.SizeOf(shfi),
                Shell32.SHGFI.Icon
                    | Shell32.SHGFI.UseFileAttributes
                    | Shell32.SHGFI.OpenIcon
                    | (smallIcon ? Shell32.SHGFI.SmallIcon : Shell32.SHGFI.LargeIcon));
            Icon icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon);
            return icon;
        }

        /// <summary>
        /// Retrieves 16x16 or 32x32 system icon for folder.
        /// </summary>
        public static Icon GetStockFolderIcon(bool smallIcon)
        {
            var sii = new Shell32.SHSTOCKICONINFO();
            sii.cbSize = (uint)Marshal.SizeOf(typeof(Shell32.SHSTOCKICONINFO));
            Marshal.ThrowExceptionForHR(
                Shell32.SHGetStockIconInfo(
                    Shell32.SHSTOCKICONID.Folder,
                    Shell32.SHGSI.Icon
                        | (smallIcon ? Shell32.SHGSI.SmallIcon : Shell32.SHGSI.LargeIcon),
                    ref sii));
            Icon icon = (Icon)Icon.FromHandle(sii.hIcon).Clone();
            User32.DestroyIcon(sii.hIcon);
            return icon;
        }
    }
}
