/*****************************************************************************
 * About JustCapture
 * 
 * Copyright © 2012 UNVELL All rights reserved
 * Copyright (c) 2019 Jingwood, all rights reserved.
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

namespace unvell.JustCapture
{
	internal partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();

			InitCulture();

			labDesc.Text = ProductInfo.Subtitle;
			labVersion.Text = "Version " + ProductInfo.Version;

			linkHP.Click += (s, e) => System.Diagnostics.Process.Start(linkHP.Text);
		}

		private void InitCulture()
		{
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Close();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				Close();
				return true;
			}
			else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		private void label3_MouseDown(object sender, MouseEventArgs e)
		{
			Close();
		}

	}
}
