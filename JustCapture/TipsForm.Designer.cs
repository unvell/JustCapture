namespace unvell.JustCapture
{
	partial class TipsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipsForm));
			this.labTip = new System.Windows.Forms.Label();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.btnSkip = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.btnOK = new System.Windows.Forms.Button();
			this.chkNoShowNext = new System.Windows.Forms.CheckBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.imageBox = new System.Windows.Forms.PictureBox();
			this.bottomPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
			this.SuspendLayout();
			// 
			// labTip
			// 
			this.labTip.BackColor = System.Drawing.Color.Transparent;
			this.labTip.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labTip.Location = new System.Drawing.Point(61, 23);
			this.labTip.Name = "labTip";
			this.labTip.Size = new System.Drawing.Size(709, 139);
			this.labTip.TabIndex = 0;
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.btnSkip);
			this.bottomPanel.Controls.Add(this.splitter1);
			this.bottomPanel.Controls.Add(this.btnOK);
			this.bottomPanel.Controls.Add(this.chkNoShowNext);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 462);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Padding = new System.Windows.Forms.Padding(0, 3, 4, 4);
			this.bottomPanel.Size = new System.Drawing.Size(796, 34);
			this.bottomPanel.TabIndex = 1;
			// 
			// btnSkip
			// 
			this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSkip.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnSkip.Location = new System.Drawing.Point(629, 3);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new System.Drawing.Size(80, 27);
			this.btnSkip.TabIndex = 2;
			this.btnSkip.Text = "&Skip";
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(709, 3);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 27);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnOK.Location = new System.Drawing.Point(712, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 27);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// chkNoShowNext
			// 
			this.chkNoShowNext.AutoSize = true;
			this.chkNoShowNext.Checked = true;
			this.chkNoShowNext.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNoShowNext.Location = new System.Drawing.Point(11, 11);
			this.chkNoShowNext.Name = "chkNoShowNext";
			this.chkNoShowNext.Size = new System.Drawing.Size(15, 14);
			this.chkNoShowNext.TabIndex = 0;
			this.chkNoShowNext.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = global::unvell.JustCapture.Properties.Resources.tips_big;
			this.pictureBox1.Location = new System.Drawing.Point(17, 23);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(42, 38);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// imageBox
			// 
			this.imageBox.Location = new System.Drawing.Point(170, 156);
			this.imageBox.Name = "imageBox";
			this.imageBox.Size = new System.Drawing.Size(448, 239);
			this.imageBox.TabIndex = 2;
			this.imageBox.TabStop = false;
			this.imageBox.Visible = false;
			// 
			// TipsForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(796, 496);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.imageBox);
			this.Controls.Add(this.labTip);
			this.Controls.Add(this.bottomPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TipsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AddGroupedItemTipForm";
			this.bottomPanel.ResumeLayout(false);
			this.bottomPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labTip;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.CheckBox chkNoShowNext;
		private System.Windows.Forms.Button btnSkip;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PictureBox imageBox;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}