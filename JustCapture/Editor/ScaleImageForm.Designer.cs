
namespace unvell.JustCapture.Editor
{
	partial class ScaleImageForm
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
			this.sizeGroup = new System.Windows.Forms.GroupBox();
			this.unitCombo = new System.Windows.Forms.ComboBox();
			this.labUnit = new System.Windows.Forms.Label();
			this.numHeight = new System.Windows.Forms.NumericUpDown();
			this.numWidth = new System.Windows.Forms.NumericUpDown();
			this.labHeight = new System.Windows.Forms.Label();
			this.labWidth = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkLockAspect = new System.Windows.Forms.CheckBox();
			this.sizeGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// sizeGroup
			// 
			this.sizeGroup.Controls.Add(this.chkLockAspect);
			this.sizeGroup.Controls.Add(this.unitCombo);
			this.sizeGroup.Controls.Add(this.labUnit);
			this.sizeGroup.Controls.Add(this.numHeight);
			this.sizeGroup.Controls.Add(this.numWidth);
			this.sizeGroup.Controls.Add(this.labHeight);
			this.sizeGroup.Controls.Add(this.labWidth);
			this.sizeGroup.Location = new System.Drawing.Point(16, 15);
			this.sizeGroup.Name = "sizeGroup";
			this.sizeGroup.Size = new System.Drawing.Size(252, 145);
			this.sizeGroup.TabIndex = 0;
			this.sizeGroup.TabStop = false;
			this.sizeGroup.Text = "Size";
			// 
			// unitCombo
			// 
			this.unitCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.unitCombo.FormattingEnabled = true;
			this.unitCombo.Location = new System.Drawing.Point(82, 26);
			this.unitCombo.Name = "unitCombo";
			this.unitCombo.Size = new System.Drawing.Size(88, 20);
			this.unitCombo.TabIndex = 1;
			// 
			// labUnit
			// 
			this.labUnit.AutoSize = true;
			this.labUnit.Location = new System.Drawing.Point(22, 29);
			this.labUnit.Name = "labUnit";
			this.labUnit.Size = new System.Drawing.Size(28, 12);
			this.labUnit.TabIndex = 0;
			this.labUnit.Text = "Unit:";
			// 
			// numHeight
			// 
			this.numHeight.Location = new System.Drawing.Point(82, 111);
			this.numHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numHeight.Name = "numHeight";
			this.numHeight.Size = new System.Drawing.Size(68, 19);
			this.numHeight.TabIndex = 6;
			this.numHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// numWidth
			// 
			this.numWidth.Location = new System.Drawing.Point(82, 86);
			this.numWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(68, 19);
			this.numWidth.TabIndex = 4;
			this.numWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// labHeight
			// 
			this.labHeight.AutoSize = true;
			this.labHeight.Location = new System.Drawing.Point(20, 113);
			this.labHeight.Name = "labHeight";
			this.labHeight.Size = new System.Drawing.Size(40, 12);
			this.labHeight.TabIndex = 5;
			this.labHeight.Text = "Height:";
			// 
			// labWidth
			// 
			this.labWidth.AutoSize = true;
			this.labWidth.Location = new System.Drawing.Point(20, 88);
			this.labWidth.Name = "labWidth";
			this.labWidth.Size = new System.Drawing.Size(35, 12);
			this.labWidth.TabIndex = 3;
			this.labWidth.Text = "Width:";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(274, 22);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(274, 51);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// chkLockAspect
			// 
			this.chkLockAspect.AutoSize = true;
			this.chkLockAspect.Location = new System.Drawing.Point(82, 54);
			this.chkLockAspect.Name = "chkLockAspect";
			this.chkLockAspect.Size = new System.Drawing.Size(88, 16);
			this.chkLockAspect.TabIndex = 2;
			this.chkLockAspect.Text = "Lock Aspect";
			this.chkLockAspect.UseVisualStyleBackColor = true;
			// 
			// ScaleImageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(360, 176);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.sizeGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScaleImageForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Image Size";
			this.sizeGroup.ResumeLayout(false);
			this.sizeGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox sizeGroup;
		private System.Windows.Forms.NumericUpDown numHeight;
		private System.Windows.Forms.NumericUpDown numWidth;
		private System.Windows.Forms.Label labHeight;
		private System.Windows.Forms.Label labWidth;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ComboBox unitCombo;
		private System.Windows.Forms.Label labUnit;
		private System.Windows.Forms.CheckBox chkLockAspect;
	}
}