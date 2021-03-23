using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace InductionTest.FtpTool
{
    partial class Settings
    {
        public View ContentView
        {
            get => !string.IsNullOrEmpty(_ContentView) ? (View)Enum.Parse(typeof(View), _ContentView) : View.LargeIcon;
            set => _ContentView = value.ToString();
        }

        public StringCollection RecentUriList
        {
            get
            {
                for (int i = 0; i < (_RecentUriList?.Count ?? 0); )
                {
                    if (Uri.TryCreate(_RecentUriList[i], UriKind.Absolute, out Uri uri))
                        ++i;
                    else
                        _RecentUriList.RemoveAt(i);
                }
                return _RecentUriList;
            }
            set
            {
                _RecentUriList = value;
            }
        }
    }
}
