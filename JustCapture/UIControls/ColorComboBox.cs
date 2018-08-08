/*****************************************************************************
 * 
 * Color select combo box
 * 
 * Standard Windows Control Compatible
 * 
 * Copyright © 2012 UNVELL All rights reserved.
 *
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace unvell.UIControls
{
	public class ColorComboBox : Control
	{
		private ColorPickerWindow pickerWindow = new ColorPickerWindow();

		public ColorComboBox()
		{
			TabStop = true;
			DoubleBuffered = true;
			BackColor = Color.White;

			pickerWindow.VisibleChanged += (sender, e) =>
			{
				if (!pickerWindow.Visible)
				{
					Pullup();
				}
			};

			pickerWindow.ColorPicked += (sender, e) =>
			{
				SelectColor(pickerWindow.CurrentColor);
			};
		}

		public void SelectColor(Color c)
		{
			color = c;
			Pullup();
			if (ColorSelected != null) ColorSelected(this, null);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			btnRect = new Rectangle(ClientRectangle.Right - 22, ClientRectangle.Top + 1, 21, ClientRectangle.Bottom - 2);
			colorRect = new Rectangle(ClientRectangle.Left + 6, ClientRectangle.Top + 6,
				ClientRectangle.Width - 32, ClientRectangle.Bottom - 12);
		}

		private Color color = Color.Empty;

		public Color CurrentColor
		{
			get { return color; }
			set { SelectColor(value); }
		}

		private bool pressed = false;

		public bool dropdown
		{
			get { return pressed; }
			set { pressed = value; }
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Focus();

			if (dropdown)
			{
				Pullup();
			}
			else
			{
				Dropdown();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Space:
				case Keys.Enter:
					if (dropdown)
						Pullup();
					else
						Dropdown();
					break;
			}

		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}

		Rectangle colorRect;
		Rectangle btnRect;

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (color.IsEmpty)
			{
				g.DrawRectangle(Pens.Black, colorRect);
			}
			else
			{
				using (Brush b = new SolidBrush(color))
				{
					g.FillRectangle(b, colorRect);
				}
			}

			if (Focused)
			{
				Rectangle focusRect = colorRect;
				focusRect.Inflate(3, 3);
				ControlPaint.DrawFocusRectangle(g, focusRect);
			}

			ControlPaint.DrawComboButton(g, btnRect, pressed ? ButtonState.Pushed : ButtonState.Normal);
			ControlPaint.DrawBorder3D(g, ClientRectangle, Border3DStyle.Sunken);
		}

		public void Dropdown()
		{
			pickerWindow.Show(this, 0, Height);

			dropdown = true;
			Invalidate();
		}

		public void Pullup()
		{
			dropdown = false;
			pickerWindow.Hide();
			Invalidate();
		}

		public event EventHandler ColorSelected;
	}

}
