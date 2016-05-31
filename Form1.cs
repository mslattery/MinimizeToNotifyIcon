using System;
using System.Windows.Forms;

namespace MinimizeToNotifyIcon
{
    public partial class Form1 : Form
    {
        private const int NotifyIconShowTime = 300;
        private const string NotifyIconTitle = "Minimize to Notify Icon";

        // Properties based override for hiding behavior
        private readonly bool _canUiHide = Properties.Settings.Default.CanUIHide;

        // Used to control visibility other than the first startups
        private bool _hideForm = true;

        // Used to handle form startup
        private bool _hideFormOnStartup = true;

        // Used to allow for real form closing
        private bool _reallyClose;

        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_hideFormOnStartup) { _hideFormOnStartup = false; } // Reverses the startup case
            _hideForm = false;
            ShowInTaskbar = true;
            Show();
            WindowState = FormWindowState.Normal; // Guarantees the form does not just show to the task bar, but has focus
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_canUiHide)
            {
                notifyIcon1.Visible = true; // Needed to ensure after we hide the form (startup) that a notify Icon appears
                notifyIcon1.ShowBalloonTip(NotifyIconShowTime, NotifyIconTitle, "Starting Minimized to Tray", ToolTipIcon.Info);               
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (_canUiHide)
            {
                if (_hideFormOnStartup) { Visible = false; } // Handle Startup

                if (!_hideForm) { Visible = true; } // Handle Visibilty beyond Startup
            }
            base.OnVisibleChanged(e);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Minimized:
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(NotifyIconShowTime, NotifyIconTitle, "I was minimized", ToolTipIcon.Info);
                    ShowInTaskbar = false;
                    break;
                case FormWindowState.Normal:
                    notifyIcon1.Visible = false;
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_reallyClose) // Really close if we push the Really close button
            {
                _hideForm = true; // Set so visibiity is hidden
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
