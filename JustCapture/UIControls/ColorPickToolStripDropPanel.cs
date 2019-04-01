/*****************************************************************************
 * Color Picker Popup Panel
 *
 * Copyright © 2012 UNVELL All rights reserved.
 * Copyright (c) 2019 Jingwood, all rights reserved.
 *
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.ComponentModel;

namespace unvell.UIControls
{
	internal class ColorPickerWindow : ToolStripDropDown
	{
		private ColorPickPanel colorPickPanel = new ColorPickPanel();

		private ToolStripControlHost controlHost;

		public ColorPickerWindow()
			: base()
		{
			this.TabStop = true;
			this.Margin = this.Padding = new Padding(1);
			this.AutoSize = true;
		
			colorPickPanel.Dock = DockStyle.Fill;
			colorPickPanel.Location = new Point(0, 0);
			colorPickPanel.ColorPicked += (s, e) =>
			{
				if (ColorPicked != null) ColorPicked(this, null);
			};

			controlHost = new ToolStripControlHost(colorPickPanel);
			controlHost.AutoSize = false;

			Items.Add(controlHost);
		}

		public Color CurrentColor
		{
			get { return colorPickPanel.CurrentColor; }
			set { colorPickPanel.CurrentColor = value; }
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (controlHost != null) controlHost.Size = new Size(ClientRectangle.Width - 2, ClientRectangle.Height - 3);
		}

		internal event EventHandler ColorPicked;
	}
}
