using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Logger_Viewer
{
    internal partial class LoggerViewerForm : Form
    {
        private readonly int maxNumberOfLines = 4096;

        public LoggerViewerForm()
        {
            InitializeComponent();
            Opacity = 0;
            Show();
            Hide();
            Opacity = 1;
        }

        public delegate void ClosedEvent();

        public event ClosedEvent MyFormClosed;

        public void AppendText(string text, bool overrideLastLine)
        {
            ThreadPool.QueueUserWorkItem(x =>
                {
                    try
                    {
                        if (richTextBox.InvokeRequired)
                            richTextBox.Invoke(new MethodInvoker(delegate { AddText(text, overrideLastLine); }));
                        else
                            AddText(text, overrideLastLine);
                    }
                    catch
                    {
                        // ignored
                    }
                },
                text);
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void AddText(string text, bool overrideLastLine)
        {
            bool Checked = false;
            if (!cbStopRefresh.InvokeRequired)
                Checked = cbStopRefresh.Checked;
            else
                cbStopRefresh.Invoke(new MethodInvoker(delegate { Checked = cbStopRefresh.Checked; }));
            if (Checked)
                return;
            richTextBox.Suspend();
            richTextBox.SuspendLayout();
            try
            {
                int currCarret = richTextBox.SelectionStart;
                int sellLenght = richTextBox.SelectionLength;

                if (overrideLastLine)
                    EditLastLine(text);
                else
                    richTextBox.AppendText(text);
                if (richTextBox.Lines.Length > maxNumberOfLines)
                {
                    string[] newLines = new string[maxNumberOfLines];
                    Array.Copy(richTextBox.Lines, richTextBox.Lines.Length - maxNumberOfLines, newLines, 0, maxNumberOfLines);
                    richTextBox.Lines = newLines;
                    newLines = null;
                }
                Checked = false;
                if (!cbAutoScroll.InvokeRequired)
                    Checked = cbAutoScroll.Checked;
                else
                    cbAutoScroll.Invoke(new MethodInvoker(delegate { Checked = cbAutoScroll.Checked; }));

                if (Checked)
                {
                    currCarret = richTextBox.Text.Length;
                    sellLenght = 0;
                }
                richTextBox.SelectionStart = currCarret;
                richTextBox.SelectionLength = sellLenght;
                richTextBox.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                richTextBox.Resume();
                richTextBox.ResumeLayout();
            }
        }

        private void EditLastLine(string line)
        {
            int last = richTextBox.Text.LastIndexOf("\n");
            if (last > 0)
            {
                richTextBox.Text = richTextBox.Text.Substring(0, last - 2);
                int beforelast = richTextBox.Text.LastIndexOf("\n");
                richTextBox.Text = richTextBox.Text.Substring(0, beforelast + 1);
                richTextBox.AppendText(line);
            }
            else
            {
                richTextBox.Text = "";
            }
        }

        private void BtnClearClick(object sender, EventArgs e)
        {
            richTextBox.Suspend();
            richTextBox.SuspendLayout();
            try
            {
                richTextBox.Clear();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally
            {
                richTextBox.Resume();
                richTextBox.ResumeLayout();
            }
        }

        private void DebugViewerFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            MyFormClosed?.Invoke();
        }
    }
}
