using System;
using System.Windows.Forms;

namespace MinimizeToNotifyIcon
{
    public partial class Form1 : Form
    {
        private const int NotifyIconShowTime = 300;
        private const string NotifyIconTitle = "Minimize to Notify Icon";

        // Used to control visibility other than the first startups
        private bool _hideForm = true;

        // Used to handle form startup
        private bool _hideOnStartup = true;

        // Used to allow for real form closing
        private bool _reallyClose;

        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_hideOnStartup) { _hideOnStartup = false; } // Reverses the startup case
            _hideForm = false;
            Show();
            WindowState = FormWindowState.Normal; // Guarentees the form does not just show to the taskbar, but has focus
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true; // Needed to ensure after we hide the form (startup) that a notify Icon appears
            notifyIcon1.ShowBalloonTip(NotifyIconShowTime, NotifyIconTitle, "Starting Minimized to Tray", ToolTipIcon.Info);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (_hideOnStartup) { Visible = false; } // Handle Startup

            if (!_hideForm) { Visible = true; } // Handle Visibilty beyond Startup
            base.OnVisibleChanged(e);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(NotifyIconShowTime, NotifyIconTitle, "I was minimized", ToolTipIcon.Info);
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_reallyClose) // Really close if we push the Really close button
            {
                _hideForm = true;  // Set so visibiity is hidden
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _reallyClose = true;
            Close();
        }
    }
}
