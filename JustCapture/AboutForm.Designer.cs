
namespace unvell.JustCapture
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.labName = new System.Windows.Forms.Label();
			this.labDesc = new System.Windows.Forms.Label();
			this.imgLogoSmall = new System.Windows.Forms.PictureBox();
			this.linkHP = new System.Windows.Forms.LinkLabel();
			this.labVersion = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imgLogoSmall)).BeginInit();
			this.SuspendLayout();
			// 
			// labName
			// 
			this.labName.BackColor = System.Drawing.Color.Transparent;
			this.labName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labName.Location = new System.Drawing.Point(440, 19);
			this.labName.Name = "labName";
			this.labName.Size = new System.Drawing.Size(223, 32);
			this.labName.TabIndex = 0;
			this.labName.Text = "JustCapture";
			this.labName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
			// 
			// labDesc
			// 
			this.labDesc.BackColor = System.Drawing.Color.Transparent;
			this.labDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labDesc.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.labDesc.Location = new System.Drawing.Point(276, 51);
			this.labDesc.Name = "labDesc";
			this.labDesc.Size = new System.Drawing.Size(412, 24);
			this.labDesc.TabIndex = 1;
			this.labDesc.Text = "A powerful screen snapshot Maker";
			this.labDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labDesc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
			// 
			// imgLogoSmall
			// 
			this.imgLogoSmall.BackColor = System.Drawing.Color.Transparent;
			this.imgLogoSmall.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgLogoSmall.Image = global::unvell.JustCapture.Properties.Resources.unvell_logo_small;
			this.imgLogoSmall.Location = new System.Drawing.Point(14, 465);
			this.imgLogoSmall.Name = "imgLogoSmall";
			this.imgLogoSmall.Size = new System.Drawing.Size(64, 21);
			this.imgLogoSmall.TabIndex = 14;
			this.imgLogoSmall.TabStop = false;
			// 
			// linkHP
			// 
			this.linkHP.AutoSize = true;
			this.linkHP.BackColor = System.Drawing.Color.Transparent;
			this.linkHP.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkHP.Location = new System.Drawing.Point(80, 468);
			this.linkHP.Name = "linkHP";
			this.linkHP.Size = new System.Drawing.Size(384, 25);
			this.linkHP.TabIndex = 13;
			this.linkHP.TabStop = true;
			this.linkHP.Text = "https://www.unvell.com/products/justcapture";
			// 
			// labVersion
			// 
			this.labVersion.BackColor = System.Drawing.Color.Transparent;
			this.labVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labVersion.ForeColor = System.Drawing.Color.Black;
			this.labVersion.Location = new System.Drawing.Point(391, 90);
			this.labVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labVersion.Name = "labVersion";
			this.labVersion.Size = new System.Drawing.Size(297, 20);
			this.labVersion.TabIndex = 12;
			this.labVersion.Text = "version 1.0";
			this.labVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label3.Location = new System.Drawing.Point(11, 406);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(677, 43);
			this.label3.TabIndex = 11;
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.DimGray;
			this.label4.Location = new System.Drawing.Point(299, 462);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(389, 23);
			this.label4.TabIndex = 15;
			this.label4.Text = "COPYRIGHT (C) 2012-2018 UNVELL ALL RIGHTS RESERVED";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Location = new System.Drawing.Point(660, 19);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 20);
			this.label5.TabIndex = 16;
			this.label5.Text = "(TM)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// AboutForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(700, 498);
			this.Controls.Add(this.labName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.imgLogoSmall);
			this.Controls.Add(this.linkHP);
			this.Controls.Add(this.labVersion);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labDesc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About JustCapture";
			((System.ComponentModel.ISupportInitialize)(this.imgLogoSmall)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labName;
		private System.Windows.Forms.Label labDesc;
		private System.Windows.Forms.PictureBox imgLogoSmall;
		private System.Windows.Forms.LinkLabel linkHP;
		private System.Windows.Forms.Label labVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
	}
}