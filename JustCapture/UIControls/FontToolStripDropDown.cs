/*****************************************************************************
 * 
 * Font ToolStripDropDown Control
 * 
 * - Show font sample as every dropdown item
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
 * PURPOSE.
 *
 * Copyright (c) 2012-2018 unvell.com, all rights reserved.
 * Copyright (c) 2019 Jingwood, all rights reserved.
 *
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using unvell.Common;

namespace unvell.UIControls
{
	public class FontToolStripDropDown : ToolStripComboBox
	{
		public FontToolStripDropDown()
		{
			ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
			ComboBox.DropDownHeight = 400;
			//ComboBox.ItemHeight = 20;
			ComboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);

			foreach (FontFamilyInfo family in FontToolkit.FontFamilies)
			{
				ComboBox.Items.Add(family);
			}

			if (ComboBox.Items.Count > 0) ComboBox.Text = Font.FontFamily.Name;
		}

		[DefaultValue(500)]
		public new int DropDownHeight
		{
			get
			{
				return base.DropDownHeight;
			}
			set { base.DropDownHeight = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new object Items { get { return null; } set { } }

		void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;

			e.DrawBackground();

			FontToolkit.DrawFontItem(g, (FontFamilyInfo)ComboBox.Items[e.Index], e.Bounds,
				(e.State & DrawItemState.Selected) == DrawItemState.Selected);
		}

		private bool textUpdating = false;

		public FontFamilyInfo SelectedFont
		{
			get
			{
				return ComboBox.SelectedItem as FontFamilyInfo;
			}
			set
			{
				ComboBox.SelectedItem = value;
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				if (!textUpdating)
				{
					textUpdating = true;

					bool found = false;

					for (int i = 0; i < ComboBox.Items.Count; i++)
					{
						FontFamilyInfo item = (FontFamilyInfo)ComboBox.Items[i];

						if (item.IsFamilyName(Text))
						{
							ComboBox.SelectedIndex = i;
							//SelectionStart = 0;
							SelectionLength = Text.Length;
							found = true;
							break;
						}
					}

					if (!found)
					{
						ComboBox.SelectedIndex = -1;
						this.Text = string.Empty;
					}

					textUpdating = false;
				}
			}
		}
	}


}
