namespace Logger_Viewer
{
    partial class LoggerViewerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggerViewerForm));
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.cbAutoScroll = new System.Windows.Forms.CheckBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.cbStopRefresh = new System.Windows.Forms.CheckBox();
			this.btnClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox.Location = new System.Drawing.Point(13, 13);
			this.richTextBox.Name = "richTextBox1";
			this.richTextBox.ReadOnly = true;
			this.richTextBox.Size = new System.Drawing.Size(585, 295);
			this.richTextBox.TabIndex = 0;
			this.richTextBox.Text = "";
			// 
			// cbAutoScroll
			// 
			this.cbAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbAutoScroll.AutoSize = true;
			this.cbAutoScroll.Checked = true;
			this.cbAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbAutoScroll.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cbAutoScroll.Location = new System.Drawing.Point(13, 314);
			this.cbAutoScroll.Name = "cbAutoScroll";
			this.cbAutoScroll.Size = new System.Drawing.Size(78, 17);
			this.cbAutoScroll.TabIndex = 1;
			this.cbAutoScroll.Text = "AutoScrool";
			this.cbAutoScroll.UseVisualStyleBackColor = true;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(523, 314);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
			// 
			// cbStopRefresh
			// 
			this.cbStopRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbStopRefresh.AutoSize = true;
			this.cbStopRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cbStopRefresh.Location = new System.Drawing.Point(230, 316);
			this.cbStopRefresh.Name = "cbStopRefresh";
			this.cbStopRefresh.Size = new System.Drawing.Size(88, 17);
			this.cbStopRefresh.TabIndex = 3;
			this.cbStopRefresh.Text = "Stop Refresh";
			this.cbStopRefresh.UseVisualStyleBackColor = true;
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.Location = new System.Drawing.Point(428, 314);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 4;
			this.btnClear.Text = "C&lear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.BtnClearClick);
			// 
			// LoggerViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(610, 343);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.cbStopRefresh);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.cbAutoScroll);
			this.Controls.Add(this.richTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "LoggerViewerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WizzAir Checker - Log Visualizer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugViewerFormFormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.CheckBox cbAutoScroll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbStopRefresh;
        private System.Windows.Forms.Button btnClear;
    }
}

