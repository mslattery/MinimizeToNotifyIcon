using System;
using System.Windows.Forms;

namespace MinimizeToNotifyIcon
{
    public partial class Form1 : Form
    {
        private const int NOTIFY_ICON_SHOW_TIME = 300;
        private const string NOTIFY_ICON_TITLE = "Minimize to Notify Icon";

        // Used to control visibility other than the first startups
        private bool HideForm = true;

        // Used to handle form startup
        private bool HideOnStartup = true;

        // Used to allow for real form closing
        private bool ReallyClose = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (HideOnStartup) { HideOnStartup = false; } // Reverses the startup case
            HideForm = false;
            this.Show();
            WindowState = FormWindowState.Normal; // Guarentees the form does not just show to the taskbar, but has focus
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true; // Needed to ensure after we hide the form (startup) that a notify Icon appears
            notifyIcon1.ShowBalloonTip(NOTIFY_ICON_SHOW_TIME, NOTIFY_ICON_TITLE, "Starting Minimized to Tray", ToolTipIcon.Info);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (HideOnStartup) { this.Visible = false; } // Handle Startup

            if (!HideForm) { this.Visible = true; } // Handle Visibilty beyond Startup
            base.OnVisibleChanged(e);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(NOTIFY_ICON_SHOW_TIME, NOTIFY_ICON_TITLE, "I was minimized", ToolTipIcon.Info);
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ReallyClose) // Really close if we push the Really close button
            {
                HideForm = true;  // Set so visibiity is hidden
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ReallyClose = true;
            Close();
        }
    }
}
