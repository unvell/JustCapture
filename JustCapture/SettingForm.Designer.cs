namespace unvell.JustCapture
{
	partial class SettingForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.startOperationCombo = new System.Windows.Forms.ComboBox();
			this.labStartOperation = new System.Windows.Forms.Label();
			this.languageCombo = new System.Windows.Forms.ComboBox();
			this.labLanguage = new System.Windows.Forms.Label();
			this.grpStartUp = new System.Windows.Forms.GroupBox();
			this.chkCheckForUpdates = new System.Windows.Forms.CheckBox();
			this.imgNoPermission = new System.Windows.Forms.PictureBox();
			this.labNoPermission = new System.Windows.Forms.Label();
			this.chkStartup = new System.Windows.Forms.CheckBox();
			this.tabShortcutKeys = new System.Windows.Forms.TabPage();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.labShortcutPrompt = new System.Windows.Forms.Label();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.tabCaptureTool = new System.Windows.Forms.TabPage();
			this.labMagnifierValue = new System.Windows.Forms.Label();
			this.labMagnifierScale = new System.Windows.Forms.Label();
			this.trkMagnifierScale = new System.Windows.Forms.TrackBar();
			this.chkShowCoordinateInfo = new System.Windows.Forms.CheckBox();
			this.chkRestorePreviousRegion = new System.Windows.Forms.CheckBox();
			this.chkShowToolboxInCapturer = new System.Windows.Forms.CheckBox();
			this.grpRegionSelection = new System.Windows.Forms.GroupBox();
			this.regionSelectionSamplePanel = new System.Windows.Forms.Panel();
			this.thumbColorComboBox = new unvell.UIControls.ColorComboBox();
			this.labThumb = new System.Windows.Forms.Label();
			this.innerLineColorComboBox = new unvell.UIControls.ColorComboBox();
			this.labInnerLine = new System.Windows.Forms.Label();
			this.outerLineColorComboBox = new unvell.UIControls.ColorComboBox();
			this.labOuterline = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.grpStartUp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgNoPermission)).BeginInit();
			this.tabShortcutKeys.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tabCaptureTool.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkMagnifierScale)).BeginInit();
			this.grpRegionSelection.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabGeneral);
			this.tabControl.Controls.Add(this.tabShortcutKeys);
			this.tabControl.Controls.Add(this.tabCaptureTool);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(4, 4);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(563, 340);
			this.tabControl.TabIndex = 1;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.groupBox1);
			this.tabGeneral.Controls.Add(this.grpStartUp);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Size = new System.Drawing.Size(555, 314);
			this.tabGeneral.TabIndex = 1;
			this.tabGeneral.Text = "General";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.startOperationCombo);
			this.groupBox1.Controls.Add(this.labStartOperation);
			this.groupBox1.Controls.Add(this.languageCombo);
			this.groupBox1.Controls.Add(this.labLanguage);
			this.groupBox1.Location = new System.Drawing.Point(7, 129);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(518, 106);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Generic";
			// 
			// startOperationCombo
			// 
			this.startOperationCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.startOperationCombo.DropDownWidth = 200;
			this.startOperationCombo.FormattingEnabled = true;
			this.startOperationCombo.Location = new System.Drawing.Point(200, 67);
			this.startOperationCombo.Name = "startOperationCombo";
			this.startOperationCombo.Size = new System.Drawing.Size(276, 21);
			this.startOperationCombo.TabIndex = 6;
			this.startOperationCombo.Visible = false;
			// 
			// labStartOperation
			// 
			this.labStartOperation.AutoSize = true;
			this.labStartOperation.Location = new System.Drawing.Point(23, 71);
			this.labStartOperation.Name = "labStartOperation";
			this.labStartOperation.Size = new System.Drawing.Size(81, 13);
			this.labStartOperation.TabIndex = 5;
			this.labStartOperation.Text = "Start Operation:";
			this.labStartOperation.Visible = false;
			// 
			// languageCombo
			// 
			this.languageCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.languageCombo.DropDownWidth = 200;
			this.languageCombo.FormattingEnabled = true;
			this.languageCombo.Location = new System.Drawing.Point(200, 31);
			this.languageCombo.Name = "languageCombo";
			this.languageCombo.Size = new System.Drawing.Size(189, 21);
			this.languageCombo.TabIndex = 3;
			// 
			// labLanguage
			// 
			this.labLanguage.AutoSize = true;
			this.labLanguage.Location = new System.Drawing.Point(23, 34);
			this.labLanguage.Name = "labLanguage";
			this.labLanguage.Size = new System.Drawing.Size(58, 13);
			this.labLanguage.TabIndex = 2;
			this.labLanguage.Text = "Language:";
			// 
			// grpStartUp
			// 
			this.grpStartUp.Controls.Add(this.chkCheckForUpdates);
			this.grpStartUp.Controls.Add(this.imgNoPermission);
			this.grpStartUp.Controls.Add(this.labNoPermission);
			this.grpStartUp.Controls.Add(this.chkStartup);
			this.grpStartUp.Location = new System.Drawing.Point(7, 8);
			this.grpStartUp.Name = "grpStartUp";
			this.grpStartUp.Size = new System.Drawing.Size(519, 115);
			this.grpStartUp.TabIndex = 1;
			this.grpStartUp.TabStop = false;
			this.grpStartUp.Text = "Start up";
			// 
			// chkCheckForUpdates
			// 
			this.chkCheckForUpdates.AutoSize = true;
			this.chkCheckForUpdates.Location = new System.Drawing.Point(25, 25);
			this.chkCheckForUpdates.Name = "chkCheckForUpdates";
			this.chkCheckForUpdates.Size = new System.Drawing.Size(258, 17);
			this.chkCheckForUpdates.TabIndex = 4;
			this.chkCheckForUpdates.Text = "Check for updates on every startup(Recommend)";
			this.chkCheckForUpdates.UseVisualStyleBackColor = true;
			// 
			// imgNoPermission
			// 
			this.imgNoPermission.Image = global::unvell.JustCapture.Properties.Resources.WarningHS;
			this.imgNoPermission.Location = new System.Drawing.Point(44, 77);
			this.imgNoPermission.Name = "imgNoPermission";
			this.imgNoPermission.Size = new System.Drawing.Size(22, 21);
			this.imgNoPermission.TabIndex = 2;
			this.imgNoPermission.TabStop = false;
			this.imgNoPermission.Visible = false;
			// 
			// labNoPermission
			// 
			this.labNoPermission.AutoSize = true;
			this.labNoPermission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.labNoPermission.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labNoPermission.Location = new System.Drawing.Point(67, 80);
			this.labNoPermission.Name = "labNoPermission";
			this.labNoPermission.Size = new System.Drawing.Size(352, 13);
			this.labNoPermission.TabIndex = 1;
			this.labNoPermission.Text = "Not enough permission to add startup items, please run with administrator.";
			this.labNoPermission.Visible = false;
			// 
			// chkStartup
			// 
			this.chkStartup.AutoSize = true;
			this.chkStartup.Enabled = false;
			this.chkStartup.Location = new System.Drawing.Point(25, 51);
			this.chkStartup.Name = "chkStartup";
			this.chkStartup.Size = new System.Drawing.Size(273, 17);
			this.chkStartup.TabIndex = 0;
			this.chkStartup.Text = "Run JustCapture automatically when Windows starts";
			this.chkStartup.UseVisualStyleBackColor = true;
			// 
			// tabShortcutKeys
			// 
			this.tabShortcutKeys.Controls.Add(this.pictureBox1);
			this.tabShortcutKeys.Controls.Add(this.labShortcutPrompt);
			this.tabShortcutKeys.Controls.Add(this.table);
			this.tabShortcutKeys.Location = new System.Drawing.Point(4, 22);
			this.tabShortcutKeys.Name = "tabShortcutKeys";
			this.tabShortcutKeys.Padding = new System.Windows.Forms.Padding(3);
			this.tabShortcutKeys.Size = new System.Drawing.Size(555, 314);
			this.tabShortcutKeys.TabIndex = 0;
			this.tabShortcutKeys.Text = "Shortcut Keys";
			this.tabShortcutKeys.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::unvell.JustCapture.Properties.Resources.keyboard;
			this.pictureBox1.Location = new System.Drawing.Point(11, 9);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(58, 61);
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// labShortcutPrompt
			// 
			this.labShortcutPrompt.Location = new System.Drawing.Point(75, 22);
			this.labShortcutPrompt.Name = "labShortcutPrompt";
			this.labShortcutPrompt.Size = new System.Drawing.Size(417, 39);
			this.labShortcutPrompt.TabIndex = 5;
			this.labShortcutPrompt.Text = "Check following actions to activate it and you can set different shortcut keys fo" +
    "r every actions.\r\n";
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.table.Location = new System.Drawing.Point(27, 74);
			this.table.Name = "table";
			this.table.RowCount = 1;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(465, 24);
			this.table.TabIndex = 4;
			// 
			// tabCaptureTool
			// 
			this.tabCaptureTool.Controls.Add(this.labMagnifierValue);
			this.tabCaptureTool.Controls.Add(this.labMagnifierScale);
			this.tabCaptureTool.Controls.Add(this.trkMagnifierScale);
			this.tabCaptureTool.Controls.Add(this.chkShowCoordinateInfo);
			this.tabCaptureTool.Controls.Add(this.chkRestorePreviousRegion);
			this.tabCaptureTool.Controls.Add(this.chkShowToolboxInCapturer);
			this.tabCaptureTool.Controls.Add(this.grpRegionSelection);
			this.tabCaptureTool.Location = new System.Drawing.Point(4, 22);
			this.tabCaptureTool.Name = "tabCaptureTool";
			this.tabCaptureTool.Size = new System.Drawing.Size(555, 314);
			this.tabCaptureTool.TabIndex = 2;
			this.tabCaptureTool.Text = "Capture Tool";
			this.tabCaptureTool.UseVisualStyleBackColor = true;
			// 
			// labMagnifierValue
			// 
			this.labMagnifierValue.AutoSize = true;
			this.labMagnifierValue.Location = new System.Drawing.Point(484, 154);
			this.labMagnifierValue.Name = "labMagnifierValue";
			this.labMagnifierValue.Size = new System.Drawing.Size(18, 13);
			this.labMagnifierValue.TabIndex = 9;
			this.labMagnifierValue.Text = "2x";
			// 
			// labMagnifierScale
			// 
			this.labMagnifierScale.AutoSize = true;
			this.labMagnifierScale.Location = new System.Drawing.Point(292, 154);
			this.labMagnifierScale.Name = "labMagnifierScale";
			this.labMagnifierScale.Size = new System.Drawing.Size(83, 13);
			this.labMagnifierScale.TabIndex = 8;
			this.labMagnifierScale.Text = "Magnifier Scale:";
			// 
			// trkMagnifierScale
			// 
			this.trkMagnifierScale.BackColor = System.Drawing.SystemColors.Control;
			this.trkMagnifierScale.Location = new System.Drawing.Point(384, 150);
			this.trkMagnifierScale.Maximum = 8;
			this.trkMagnifierScale.Name = "trkMagnifierScale";
			this.trkMagnifierScale.Size = new System.Drawing.Size(94, 45);
			this.trkMagnifierScale.TabIndex = 7;
			this.trkMagnifierScale.Value = 2;
			// 
			// chkShowCoordinateInfo
			// 
			this.chkShowCoordinateInfo.AutoSize = true;
			this.chkShowCoordinateInfo.Location = new System.Drawing.Point(30, 153);
			this.chkShowCoordinateInfo.Name = "chkShowCoordinateInfo";
			this.chkShowCoordinateInfo.Size = new System.Drawing.Size(128, 17);
			this.chkShowCoordinateInfo.TabIndex = 5;
			this.chkShowCoordinateInfo.Text = "Show Coordinate Info";
			this.chkShowCoordinateInfo.UseVisualStyleBackColor = true;
			// 
			// chkRestorePreviousRegion
			// 
			this.chkRestorePreviousRegion.AutoSize = true;
			this.chkRestorePreviousRegion.Location = new System.Drawing.Point(30, 200);
			this.chkRestorePreviousRegion.Name = "chkRestorePreviousRegion";
			this.chkRestorePreviousRegion.Size = new System.Drawing.Size(182, 17);
			this.chkRestorePreviousRegion.TabIndex = 4;
			this.chkRestorePreviousRegion.Text = "Remember Last Selected Region";
			this.chkRestorePreviousRegion.UseVisualStyleBackColor = true;
			// 
			// chkShowToolboxInCapturer
			// 
			this.chkShowToolboxInCapturer.AutoSize = true;
			this.chkShowToolboxInCapturer.Location = new System.Drawing.Point(30, 177);
			this.chkShowToolboxInCapturer.Name = "chkShowToolboxInCapturer";
			this.chkShowToolboxInCapturer.Size = new System.Drawing.Size(182, 17);
			this.chkShowToolboxInCapturer.TabIndex = 0;
			this.chkShowToolboxInCapturer.Text = "Show &toolbox in capturer window";
			this.chkShowToolboxInCapturer.UseVisualStyleBackColor = true;
			// 
			// grpRegionSelection
			// 
			this.grpRegionSelection.Controls.Add(this.regionSelectionSamplePanel);
			this.grpRegionSelection.Controls.Add(this.thumbColorComboBox);
			this.grpRegionSelection.Controls.Add(this.labThumb);
			this.grpRegionSelection.Controls.Add(this.innerLineColorComboBox);
			this.grpRegionSelection.Controls.Add(this.labInnerLine);
			this.grpRegionSelection.Controls.Add(this.outerLineColorComboBox);
			this.grpRegionSelection.Controls.Add(this.labOuterline);
			this.grpRegionSelection.Location = new System.Drawing.Point(9, 11);
			this.grpRegionSelection.Name = "grpRegionSelection";
			this.grpRegionSelection.Size = new System.Drawing.Size(518, 120);
			this.grpRegionSelection.TabIndex = 3;
			this.grpRegionSelection.TabStop = false;
			this.grpRegionSelection.Text = "Region Selection";
			// 
			// regionSelectionSamplePanel
			// 
			this.regionSelectionSamplePanel.BackColor = System.Drawing.Color.White;
			this.regionSelectionSamplePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.regionSelectionSamplePanel.Location = new System.Drawing.Point(324, 24);
			this.regionSelectionSamplePanel.Name = "regionSelectionSamplePanel";
			this.regionSelectionSamplePanel.Size = new System.Drawing.Size(135, 82);
			this.regionSelectionSamplePanel.TabIndex = 6;
			// 
			// thumbColorComboBox
			// 
			this.thumbColorComboBox.BackColor = System.Drawing.Color.White;
			this.thumbColorComboBox.CurrentColor = System.Drawing.Color.Empty;
			this.thumbColorComboBox.dropdown = false;
			this.thumbColorComboBox.Location = new System.Drawing.Point(124, 85);
			this.thumbColorComboBox.Name = "thumbColorComboBox";
			this.thumbColorComboBox.Size = new System.Drawing.Size(120, 24);
			this.thumbColorComboBox.TabIndex = 5;
			this.thumbColorComboBox.Text = "colorComboBox2";
			// 
			// labThumb
			// 
			this.labThumb.Location = new System.Drawing.Point(19, 89);
			this.labThumb.Name = "labThumb";
			this.labThumb.Size = new System.Drawing.Size(80, 17);
			this.labThumb.TabIndex = 4;
			this.labThumb.Text = "Thumb:";
			this.labThumb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// innerLineColorComboBox
			// 
			this.innerLineColorComboBox.BackColor = System.Drawing.Color.White;
			this.innerLineColorComboBox.CurrentColor = System.Drawing.Color.Empty;
			this.innerLineColorComboBox.dropdown = false;
			this.innerLineColorComboBox.Location = new System.Drawing.Point(124, 54);
			this.innerLineColorComboBox.Name = "innerLineColorComboBox";
			this.innerLineColorComboBox.Size = new System.Drawing.Size(120, 24);
			this.innerLineColorComboBox.TabIndex = 3;
			this.innerLineColorComboBox.Text = "colorComboBox2";
			// 
			// labInnerLine
			// 
			this.labInnerLine.Location = new System.Drawing.Point(19, 61);
			this.labInnerLine.Name = "labInnerLine";
			this.labInnerLine.Size = new System.Drawing.Size(80, 17);
			this.labInnerLine.TabIndex = 2;
			this.labInnerLine.Text = "Inner line:";
			this.labInnerLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// outerLineColorComboBox
			// 
			this.outerLineColorComboBox.BackColor = System.Drawing.Color.White;
			this.outerLineColorComboBox.CurrentColor = System.Drawing.Color.Empty;
			this.outerLineColorComboBox.dropdown = false;
			this.outerLineColorComboBox.Location = new System.Drawing.Point(124, 24);
			this.outerLineColorComboBox.Name = "outerLineColorComboBox";
			this.outerLineColorComboBox.Size = new System.Drawing.Size(120, 24);
			this.outerLineColorComboBox.TabIndex = 1;
			this.outerLineColorComboBox.Text = "colorComboBox1";
			// 
			// labOuterline
			// 
			this.labOuterline.Location = new System.Drawing.Point(19, 30);
			this.labOuterline.Name = "labOuterline";
			this.labOuterline.Size = new System.Drawing.Size(80, 17);
			this.labOuterline.TabIndex = 0;
			this.labOuterline.Text = "Outer line:";
			this.labOuterline.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(4, 344);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(4, 4, 2, 2);
			this.panel1.Size = new System.Drawing.Size(563, 33);
			this.panel1.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnOK.Location = new System.Drawing.Point(387, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(85, 27);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Enabled = false;
			this.splitter1.Location = new System.Drawing.Point(472, 4);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(4, 27);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(476, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(85, 27);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// SettingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(571, 381);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingForm";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.Text = "Settings";
			this.tabControl.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.grpStartUp.ResumeLayout(false);
			this.grpStartUp.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgNoPermission)).EndInit();
			this.tabShortcutKeys.ResumeLayout(false);
			this.tabShortcutKeys.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tabCaptureTool.ResumeLayout(false);
			this.tabCaptureTool.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkMagnifierScale)).EndInit();
			this.grpRegionSelection.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabShortcutKeys;
		private System.Windows.Forms.Label labShortcutPrompt;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.GroupBox grpStartUp;
		private System.Windows.Forms.CheckBox chkStartup;
		private System.Windows.Forms.Label labNoPermission;
		private System.Windows.Forms.PictureBox imgNoPermission;
		private System.Windows.Forms.TabPage tabCaptureTool;
		private System.Windows.Forms.CheckBox chkShowToolboxInCapturer;
		private System.Windows.Forms.GroupBox grpRegionSelection;
		private System.Windows.Forms.Label labOuterline;
		private unvell.UIControls.ColorComboBox innerLineColorComboBox;
		private System.Windows.Forms.Label labInnerLine;
		private unvell.UIControls.ColorComboBox outerLineColorComboBox;
		private unvell.UIControls.ColorComboBox thumbColorComboBox;
		private System.Windows.Forms.Label labThumb;
		private System.Windows.Forms.Panel regionSelectionSamplePanel;
		private System.Windows.Forms.ComboBox languageCombo;
		private System.Windows.Forms.Label labLanguage;
		private System.Windows.Forms.CheckBox chkRestorePreviousRegion;
		private System.Windows.Forms.CheckBox chkCheckForUpdates;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox startOperationCombo;
		private System.Windows.Forms.Label labStartOperation;
		private System.Windows.Forms.CheckBox chkShowCoordinateInfo;
		private System.Windows.Forms.Label labMagnifierValue;
		private System.Windows.Forms.Label labMagnifierScale;
		private System.Windows.Forms.TrackBar trkMagnifierScale;
	}
}