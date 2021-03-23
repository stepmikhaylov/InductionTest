using System;
using System.Windows.Forms;

namespace InductionTest.FtpTool.Common
{
    class WaitCursorScope : IDisposable
    {
        public WaitCursorScope(Form form)
        {
            Form = form;
            Cursor = form.Cursor;
            form.Cursor = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Form.Cursor = Cursor;
        }

        Form Form { get; }
        Cursor Cursor { get; }
    }
}
