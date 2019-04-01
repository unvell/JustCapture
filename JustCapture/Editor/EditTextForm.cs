/*****************************************************************************
 * 
 * JustCapture
 * https://github.com/unvell/JustCapture
 * 
 * MIT License
 * 
 * Copyright(c) 2010-2019 Jingwood, all rights reserved.
 * 
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using unvell.Common;

namespace unvell.JustCapture.Editor
{
	public partial class EditTextForm : Form
	{
		private bool uiUpdating=false;

		public EditTextForm()
		{
			InitializeComponent();

			this.Text = LangResource.edit_text;
			this.largerToolStripButton.Text = LangResource.text_larger;
			this.smallerToolStripButton.Text = LangResource.text_smaller;
			this.boldToolStripButton.Text = LangResource.bold;
			this.italicToolStripButton.Text = LangResource.italic;
			this.strikethroughToolStripButton.Text = LangResource.strikethrough;
			this.underlineToolStripButton.Text = LangResource.underline;
			this.btnOK.Text = LangResource.btn_ok;
			this.btnCancel.Text = LangResource.btn_cancel;

			foreach (var f in FontToolkit.FontSizeList)
			{
				fontSizeToolStripTextBox.Items.Add(f);
			}

			fontToolStripButton.SelectedFont = FontToolkit.GetFontFamilyInfo(txtEditor.Font.FontFamily);
			fontSizeToolStripTextBox.SelectedItem = txtEditor.Font.Size;

			fontToolStripButton.SelectedIndexChanged += (s, e) =>
				txtEditor.Font = ResourcePoolManager.Instance.GetFont(fontToolStripButton.SelectedFont.CultureName,
					txtEditor.Font.Size, txtEditor.Font.Style);

			fontSizeToolStripTextBox.SelectedIndexChanged += (s, e) =>
				{
					if (!uiUpdating)
					{
						txtEditor.Font = ResourcePoolManager.Instance.GetFont(fontToolStripButton.SelectedFont.CultureName,
						(float)fontSizeToolStripTextBox.SelectedItem, txtEditor.Font.Style);
					}
				};

			largerToolStripButton.Click += (s, e) =>
				{
					txtEditor.Font = ResourcePoolManager.Instance.GetFont(fontToolStripButton.SelectedFont.CultureName,
					FontToolkit.GetLargerSize(txtEditor.Font.Size), txtEditor.Font.Style);
					
					uiUpdating = true;
					fontSizeToolStripTextBox.SelectedItem = txtEditor.Font.Size;
					uiUpdating = false;
				};

			smallerToolStripButton.Click += (s, e) =>
			{
				txtEditor.Font = ResourcePoolManager.Instance.GetFont(fontToolStripButton.SelectedFont.CultureName,
				FontToolkit.GetSmallerSize(txtEditor.Font.Size), txtEditor.Font.Style);
				
				uiUpdating = true;
				fontSizeToolStripTextBox.SelectedItem = txtEditor.Font.Size;
				uiUpdating = false;
			};

			boldToolStripButton.CheckedChanged += (s, e) =>
				{
					FontStyle style = txtEditor.Font.Style;
					if (boldToolStripButton.Checked)
						style |= FontStyle.Bold;
					else
						style &= ~FontStyle.Bold;

					txtEditor.Font = ResourcePoolManager.Instance.GetFont(txtEditor.Font.Name, txtEditor.Font.Size, style);
				};

			italicToolStripButton.CheckedChanged += (s, e) =>
				{
					FontStyle style = txtEditor.Font.Style;
					if (italicToolStripButton.Checked)
						style |= FontStyle.Italic;
					else
						style &= ~FontStyle.Italic;

					txtEditor.Font = ResourcePoolManager.Instance.GetFont(txtEditor.Font.Name, txtEditor.Font.Size, style);
				};

			underlineToolStripButton.CheckedChanged += (s, e) =>
				{
					FontStyle style = txtEditor.Font.Style;
					if (underlineToolStripButton.Checked)
						style |= FontStyle.Underline;
					else
						style &= ~FontStyle.Underline;

					txtEditor.Font = ResourcePoolManager.Instance.GetFont(txtEditor.Font.Name, txtEditor.Font.Size, style);
				};

			strikethroughToolStripButton.CheckedChanged += (s, e) =>
				{
					FontStyle style = txtEditor.Font.Style;
					if (strikethroughToolStripButton.Checked)
						style |= FontStyle.Strikeout;
					else
						style &= ~FontStyle.Strikeout;

					txtEditor.Font = ResourcePoolManager.Instance.GetFont(txtEditor.Font.Name, txtEditor.Font.Size, style);
				};

			btnOK.Text = LangResource.btn_ok;
			btnCancel.Text = LangResource.btn_cancel;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		public Font TextFont { get; set; }

		public string EditText { get; set; }

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.EditText = txtEditor.Text;
			this.TextFont = txtEditor.Font;
		}
	}
}
