namespace unvell.JustCapture
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.captureRegionOfScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.captureWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.captureFullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smartScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.saveLastPictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printLastCapturedPictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "JustCapture";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureRegionOfScreenToolStripMenuItem,
            this.captureWindowToolStripMenuItem,
            this.captureFullScreenToolStripMenuItem,
            this.smartScanToolStripMenuItem,
            this.toolStripMenuItem3,
            this.saveLastPictureToolStripMenuItem,
            this.printLastCapturedPictureToolStripMenuItem,
            this.imageEditorToolStripMenuItem,
            this.toolStripMenuItem1,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(461, 400);
			// 
			// captureRegionOfScreenToolStripMenuItem
			// 
			this.captureRegionOfScreenToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.capture_region;
			this.captureRegionOfScreenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.captureRegionOfScreenToolStripMenuItem.Name = "captureRegionOfScreenToolStripMenuItem";
			this.captureRegionOfScreenToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.captureRegionOfScreenToolStripMenuItem.Text = "Capture a region of the screen";
			// 
			// captureWindowToolStripMenuItem
			// 
			this.captureWindowToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.select_window;
			this.captureWindowToolStripMenuItem.Name = "captureWindowToolStripMenuItem";
			this.captureWindowToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.captureWindowToolStripMenuItem.Text = "Capture window in foreground desktop";
			// 
			// captureFullScreenToolStripMenuItem
			// 
			this.captureFullScreenToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.select_full_screen;
			this.captureFullScreenToolStripMenuItem.Name = "captureFullScreenToolStripMenuItem";
			this.captureFullScreenToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.captureFullScreenToolStripMenuItem.Text = "Capture full screen";
			// 
			// smartScanToolStripMenuItem
			// 
			this.smartScanToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.SetHotSpotTool_221_32;
			this.smartScanToolStripMenuItem.Name = "smartScanToolStripMenuItem";
			this.smartScanToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.smartScanToolStripMenuItem.Text = "Smart Scan";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(457, 6);
			// 
			// saveLastPictureToolStripMenuItem
			// 
			this.saveLastPictureToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.saveHS;
			this.saveLastPictureToolStripMenuItem.Name = "saveLastPictureToolStripMenuItem";
			this.saveLastPictureToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.saveLastPictureToolStripMenuItem.Text = "Save last captured picture...";
			// 
			// printLastCapturedPictureToolStripMenuItem
			// 
			this.printLastCapturedPictureToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.PrintHS;
			this.printLastCapturedPictureToolStripMenuItem.Name = "printLastCapturedPictureToolStripMenuItem";
			this.printLastCapturedPictureToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.printLastCapturedPictureToolStripMenuItem.Text = "Print last captured picture";
			// 
			// imageEditorToolStripMenuItem
			// 
			this.imageEditorToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.ColorHS;
			this.imageEditorToolStripMenuItem.Name = "imageEditorToolStripMenuItem";
			this.imageEditorToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.imageEditorToolStripMenuItem.Text = "Image Editor...";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(457, 6);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.OptionsHS;
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.settingsToolStripMenuItem.Text = "&Settings...";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.homepageToolStripMenuItem,
            this.tipsToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.camera;
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// homepageToolStripMenuItem
			// 
			this.homepageToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.unvell_small_logo;
			this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
			this.homepageToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
			this.homepageToolStripMenuItem.Text = "&Homepage...";
			// 
			// tipsToolStripMenuItem
			// 
			this.tipsToolStripMenuItem.Image = global::unvell.JustCapture.Properties.Resources.tips;
			this.tipsToolStripMenuItem.Name = "tipsToolStripMenuItem";
			this.tipsToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
			this.tipsToolStripMenuItem.Text = "Tips...";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(457, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(460, 34);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 484);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.Name = "MainForm";
			this.ShowInTaskbar = false;
			this.Text = "JustCapture";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem captureRegionOfScreenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveLastPictureToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem imageEditorToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem captureWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem homepageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tipsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem captureFullScreenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printLastCapturedPictureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem smartScanToolStripMenuItem;
	}
}

