namespace unvell.JustCapture.Editor
{
	partial class EditTextForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTextForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.fontToolStripButton = new unvell.UIControls.FontToolStripDropDown();
			this.fontSizeToolStripTextBox = new System.Windows.Forms.ToolStripComboBox();
			this.largerToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.smallerToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.boldToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.italicToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.underlineToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.strikethroughToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.btnCancel = new System.Windows.Forms.Button();
			this.toolStrip1.SuspendLayout();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripButton,
            this.fontSizeToolStripTextBox,
            this.largerToolStripButton,
            this.smallerToolStripButton,
            this.toolStripSeparator1,
            this.boldToolStripButton,
            this.italicToolStripButton,
            this.underlineToolStripButton,
            this.strikethroughToolStripButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(540, 26);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// fontToolStripButton
			// 
			this.fontToolStripButton.DropDownHeight = 400;
			this.fontToolStripButton.IntegralHeight = false;
			this.fontToolStripButton.Name = "fontToolStripButton";
			this.fontToolStripButton.Size = new System.Drawing.Size(150, 26);
			this.fontToolStripButton.Text = "Meiryo UI";
			// 
			// fontSizeToolStripTextBox
			// 
			this.fontSizeToolStripTextBox.Name = "fontSizeToolStripTextBox";
			this.fontSizeToolStripTextBox.Size = new System.Drawing.Size(75, 26);
			// 
			// largerToolStripButton
			// 
			this.largerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.largerToolStripButton.Image = global::unvell.JustCapture.Properties.Resources.font_larger;
			this.largerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.largerToolStripButton.Name = "largerToolStripButton";
			this.largerToolStripButton.Size = new System.Drawing.Size(23, 23);
			this.largerToolStripButton.Text = "toolStripButton2";
			// 
			// smallerToolStripButton
			// 
			this.smallerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.smallerToolStripButton.Image = global::unvell.JustCapture.Properties.Resources.font_smaller;
			this.smallerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.smallerToolStripButton.Name = "smallerToolStripButton";
			this.smallerToolStripButton.Size = new System.Drawing.Size(23, 23);
			this.smallerToolStripButton.Text = "toolStripButton3";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
			// 
			// boldToolStripButton
			// 
			this.boldToolStripButton.CheckOnClick = true;
			this.boldToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.boldToolStripButton.Image = global::unvell.JustCapture.Properties.Resources.bold;
			this.boldToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.boldToolStripButton.Name = "boldToolStripButton";
			this.boldToolStripButton.Size = new System.Drawing.Size(23, 23);
			this.boldToolStripButton.Text = "toolStripButton1";
			// 
			// italicToolStripButton
			// 
			this.italicToolStripButton.CheckOnClick = true;
			this.italicToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.italicToolStripButton.Image = global::unvell.JustCapture.Properties.Resources.italic;
			this.italicToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.italicToolStripButton.Name = "italicToolStripButton";
			this.italicToolStripButton.Size = new System.Drawing.Size(23, 23);
			this.italicToolStripButton.Text = "toolStripButton2";
			// 
			// underlineToolStripButton
			// 
			this.underlineToolStripButton.CheckOnClick = true;
			this.underlineToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.underlineToolStripButton.Image = global::unvell.JustCapture.Properties.Resources.underlinee;
			this.underlineToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.underlineToolStripButton.Name = "underlineToolStripButton";
			this.underlineToolStripButton.Size = new System.Drawing.Size(23, 23);
			this.underlineToolStripButton.Text = "toolStripButton3";
			// 
			// strikethroughToolStripButton
			// 
			this.strikethroughToolStripButton.CheckOnClick = true;
			this.strikethroughToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.strikethroughToolStripButton.Image = global::unvell.JustCapture.Properties.Resources.strikethrough;
			this.strikethroughToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.strikethroughToolStripButton.Name = "strikethroughToolStripButton";
			this.strikethroughToolStripButton.Size = new System.Drawing.Size(23, 23);
			this.strikethroughToolStripButton.Text = "toolStripButton4";
			// 
			// txtEditor
			// 
			this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtEditor.Location = new System.Drawing.Point(0, 26);
			this.txtEditor.Multiline = true;
			this.txtEditor.Name = "txtEditor";
			this.txtEditor.Size = new System.Drawing.Size(540, 249);
			this.txtEditor.TabIndex = 1;
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.btnOK);
			this.bottomPanel.Controls.Add(this.splitter1);
			this.bottomPanel.Controls.Add(this.btnCancel);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 275);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Padding = new System.Windows.Forms.Padding(5);
			this.bottomPanel.Size = new System.Drawing.Size(540, 34);
			this.bottomPanel.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnOK.Location = new System.Drawing.Point(374, 5);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(78, 24);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Enabled = false;
			this.splitter1.Location = new System.Drawing.Point(452, 5);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(5, 24);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(457, 5);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(78, 24);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// EditTextForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(540, 309);
			this.Controls.Add(this.txtEditor);
			this.Controls.Add(this.bottomPanel);
			this.Controls.Add(this.toolStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EditTextForm";
			this.Text = "Edit Text";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.bottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private unvell.UIControls.FontToolStripDropDown fontToolStripButton;
		private System.Windows.Forms.ToolStripButton largerToolStripButton;
		private System.Windows.Forms.TextBox txtEditor;
		private System.Windows.Forms.ToolStripButton smallerToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton boldToolStripButton;
		private System.Windows.Forms.ToolStripButton italicToolStripButton;
		private System.Windows.Forms.ToolStripButton underlineToolStripButton;
		private System.Windows.Forms.ToolStripButton strikethroughToolStripButton;
		private System.Windows.Forms.ToolStripComboBox fontSizeToolStripTextBox;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btnCancel;
	}
}