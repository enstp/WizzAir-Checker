namespace WizzAir_Checker
{
    partial class ScrapperForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrapperForm));
            this.wizzAirNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.wizzAirContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showHideLogWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wizzAirContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizzAirNotifyIcon
            // 
            this.wizzAirNotifyIcon.ContextMenuStrip = this.wizzAirContextMenuStrip;
            this.wizzAirNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("wizzAirNotifyIcon.Icon")));
            this.wizzAirNotifyIcon.Text = "WizzAir Checker";
            this.wizzAirNotifyIcon.Visible = true;
            // 
            // wizzAirContextMenuStrip
            // 
            this.wizzAirContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideLogWindowToolStripMenuItem,
            this.openLogFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.wizzAirContextMenuStrip.Name = "wizzAirContextMenuStrip";
            this.wizzAirContextMenuStrip.Size = new System.Drawing.Size(153, 92);
            // 
            // showHideLogWindowToolStripMenuItem
            // 
            this.showHideLogWindowToolStripMenuItem.Name = "showHideLogWindowToolStripMenuItem";
            this.showHideLogWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showHideLogWindowToolStripMenuItem.Text = "Show log";
            this.showHideLogWindowToolStripMenuItem.Click += new System.EventHandler(this.ShowHideLogWindowToolStripMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // openLogFileToolStripMenuItem
            // 
            this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
            this.openLogFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openLogFileToolStripMenuItem.Text = "Open log file";
            this.openLogFileToolStripMenuItem.Click += new System.EventHandler(this.OpenLogFileToolStripMenuItemClick);
            // 
            // ScrapperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScrapperForm";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.Text = "ScrapperForm";
            this.wizzAirContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon wizzAirNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip wizzAirContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideLogWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLogFileToolStripMenuItem;
    }
}