/*****************************************************************************
 * 
 * Color Picker ToolStrip Button
 * 
 * - commonly be used as toolstrip button to let user pick a color quickly
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
using System.Threading;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.ComponentModel;

using unvell.Common;

namespace unvell.UIControls
{
	internal class ColoredShapeToolStripItem : ToolStripItem
	{
		private ShapeMode mode = ShapeMode.Rectangle;

		[DefaultValue(ShapeMode.Rectangle)]
		public ShapeMode Mode
		{
			get { return mode; }
			set { mode = value;
			Invalidate();
			}
		}

		private ColorPickerWindow dropPanel = new ColorPickerWindow()
			{
			};

		private static readonly StringFormat sf = new StringFormat();

		public ColoredShapeToolStripItem()
			: base()
		{
			dropPanel.ColorPicked += new EventHandler(dropPanel_ColorPicked);
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			Width = 60;
		}

		public override Size GetPreferredSize(Size constrainingSize)
		{
			return new Size(37, 23);
		}

		void dropPanel_ColorPicked(object sender, EventArgs e)
		{
			currentColor = dropPanel.CurrentColor;
			dropPanel.Hide();

			SolidColorPickerPanel.AddRecentColor(currentColor);

			if (ColorPicked != null)
				ColorPicked.Invoke(this, new EventArgs());
		}

		public event EventHandler ColorPicked;

		protected override void OnLocationChanged(EventArgs e)
		{
			dropPanel.VisibleChanged += new EventHandler(dropPanel_VisibleChanged);
			base.OnLocationChanged(e);
	}

		void dropPanel_VisibleChanged(object sender, EventArgs e)
		{
			if (!dropPanel.Visible)
			{
				if (!CheckOnClick)
				{
					Checked = false;
				}
				Invalidate();
			}
			isPickerPressed = dropPanel.Visible;
		}

		private bool isButtonPressed = false;

		private bool isPickerPressed = false;

		public bool IsPickerPressed
		{
			get { return isPickerPressed; }
			set { isPickerPressed = value; }
		}

		public bool CheckOnClick { get; set; }

		private bool buttonChecked;
		public bool Checked
		{
			get{
				return buttonChecked;
			}
			set
			{
				if (buttonChecked != value)
				{
					buttonChecked = value;

					if (CheckChanged != null)
					{
						CheckChanged(this, null);
					}

					Invalidate();
				}
			}
		}

		public event EventHandler<EventArgs> CheckChanged;

		Rectangle bounds;
		Rectangle buttonBounds;
		Rectangle arrowBounds;

		protected override void OnBoundsChanged()
		{
			base.OnBoundsChanged();

			bounds = new Rectangle(1, 1, Size.Width - 3, Size.Height - 3);
			buttonBounds = new Rectangle(bounds.X, bounds.Y, bounds.Width - arrowBoundSize - 1, bounds.Height - 1);
			arrowBounds = new Rectangle(buttonBounds.Right + 1, bounds.Y, arrowBoundSize, bounds.Height - 1);
		}

		//private bool check = false;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (buttonBounds.Contains(e.Location))
			{	
				isButtonPressed = true;
			}
			else if (arrowBounds.Contains(e.Location))
			{
				if (isPickerPressed)
				{
					isPickerPressed = false;
					dropPanel.Hide();
				}
				else
				{
					isButtonPressed = true;
					isPickerPressed = true;

					Rectangle panelRect = Parent.RectangleToScreen(Bounds);
					dropPanel.CurrentColor = CurrentColor;
					dropPanel.Show(panelRect.X, panelRect.Y + panelRect.Height);
				}
			}
			else
			{
				Checked = false;
				isPickerPressed = false;
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (CheckOnClick)
			{
				if (Checked)
					Checked = false;
				else
					Checked = true;
			}

			//isPickerPressed = false;
			isButtonPressed = false;
		}

		private const int arrowBoundSize = 12;
		private int arrowSize = arrowBoundSize - 6;

		private int currentNumber = 1;
		public int CurrentNumber
		{
			get { return currentNumber; }
			set
			{
				if (currentNumber != value)
				{
					currentNumber = value;
					Invalidate();

					if (CurrentNumberChanged != null)
					{
						CurrentNumberChanged(this, null);
					}
				}
			}
		}

		public event EventHandler CurrentNumberChanged;

		private Font numberFont = new Font("Arias", 8f);

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (CheckOnClick)
			{
				using (HatchBrush hb = new HatchBrush(HatchStyle.SmallConfetti, SystemColors.Window, SystemColors.ControlLight))
				{
					g.FillRectangle(Checked ? hb : SystemBrushes.Window, buttonBounds);
				}
			}
			else
			{
				g.FillRectangle(SystemBrushes.Window, buttonBounds);
			}

			// left button 
			g.DrawLine(SystemPens.ControlLight, buttonBounds.X + 1, buttonBounds.Y, buttonBounds.Right, buttonBounds.Y); // top
			g.DrawLine(SystemPens.ControlLight, buttonBounds.X + 1, buttonBounds.Bottom, buttonBounds.Right, buttonBounds.Bottom); // bottom
			g.DrawLine(SystemPens.ControlLight, buttonBounds.X, buttonBounds.Y + 1, buttonBounds.X, buttonBounds.Bottom - 1); // left
			g.DrawLine(SystemPens.ControlLight, buttonBounds.Right, buttonBounds.Y + 1, buttonBounds.Right, buttonBounds.Bottom - 1); // right

			if (isButtonPressed || Checked)
			{
				g.DrawLine(SystemPens.ControlDark, buttonBounds.X + 1, buttonBounds.Y + 1, buttonBounds.Right, buttonBounds.Y + 1);
				g.DrawLine(SystemPens.ControlDark, buttonBounds.X+1, buttonBounds.Y + 2, buttonBounds.X+1, buttonBounds.Bottom - 1); // left
			}

			Rectangle arrowRect = arrowBounds;
			if (isPickerPressed)
			{
				g.DrawLine(SystemPens.ButtonShadow, arrowRect.X, arrowRect.Y+1, arrowRect.Right - 1, arrowRect.Y+1); // top
				g.DrawLine(SystemPens.ControlDark, arrowRect.X, buttonBounds.Y + 2, arrowRect.X, buttonBounds.Bottom - 1); // right
			}

			// right button
			g.DrawLine(SystemPens.ControlLight, arrowRect.X, arrowRect.Y, arrowRect.Right - 1, arrowRect.Y); // top
			g.DrawLine(SystemPens.ControlLight, arrowRect.X, arrowRect.Bottom, arrowRect.Right - 1, arrowRect.Bottom); // bottom
			g.DrawLine(SystemPens.ControlLight, arrowRect.Right, arrowRect.Y + 1, arrowRect.Right, arrowRect.Bottom - 1); // right

			int arrowoffset = isPickerPressed ? 1: 0;

			// arrow
			GraphicsToolkit.FillTriangle(g, arrowSize, new Point(
				arrowBounds.X + (arrowBounds.Width) / 2 + arrowoffset,
				arrowBounds.Y + (arrowBounds.Height) / 2 - 1 + arrowoffset));

			// color
			Rectangle buttonRect = new Rectangle(3, 3, 16, 16);
			if (isButtonPressed || Checked) buttonRect.Offset(1, 1);

			if (Image != null)
			{
				g.DrawImage(Image, buttonRect);
			}
			else
			{
				Rectangle drawRect = new Rectangle(buttonRect.X + (buttonRect.Width - 14) / 2,
					buttonRect.Y + (buttonRect.Height - 14) / 2, 14, 14);

				if (currentColor.IsEmpty)
				{
					g.DrawRectangle(Pens.Black, drawRect);
					g.DrawLine(Pens.Black, drawRect.Left, drawRect.Bottom, drawRect.Right, drawRect.Top);
				}
				else
				{
					using (Pen p = new Pen(currentColor, 2))
					{
						using (Pen ep = new Pen(currentColor))
						{
							using (Brush b = new SolidBrush(currentColor))
							{
								switch (mode)
								{
									case ShapeMode.RoundedRect:
										g.DrawLine(p, drawRect.X + 1, drawRect.Y, drawRect.Right - 1, drawRect.Top);
										g.DrawLine(p, drawRect.X + 1, drawRect.Bottom, drawRect.Right - 1, drawRect.Bottom);
										g.DrawLine(p, drawRect.X, drawRect.Y + 1, drawRect.X, drawRect.Bottom - 1);
										g.DrawLine(p, drawRect.Right, drawRect.Y + 1, drawRect.Right, drawRect.Bottom - 1);
										break;
									default:
									case ShapeMode.Rectangle:
										g.DrawRectangle(p, drawRect);
										break;
									case ShapeMode.Ellipse:
										g.SmoothingMode = SmoothingMode.AntiAlias;
										g.DrawEllipse(ep, drawRect);
										g.SmoothingMode = SmoothingMode.Default;
										break;
									case ShapeMode.Line:
										g.DrawLine(p, drawRect.X, drawRect.Bottom, drawRect.Right, drawRect.Top);
										break;
									case ShapeMode.Arrow:
										int y = drawRect.Top + drawRect.Height / 2;
										using (Pen arrow = new Pen(currentColor))
										{
											arrow.CustomEndCap = new AdjustableArrowCap(4, 4);
											g.DrawLine(arrow, drawRect.X, y, drawRect.Right, y);
										}
										break;
									case ShapeMode.Number:
										g.SmoothingMode = SmoothingMode.AntiAlias;
										g.DrawEllipse(ep, drawRect);
										g.SmoothingMode = SmoothingMode.Default;

										using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
										{
											sf.LineAlignment = StringAlignment.Center;
											sf.Alignment = StringAlignment.Center;

											string str = currentNumber.ToString();
											SizeF s = g.MeasureString(str, numberFont, drawRect.Width, sf);

											g.DrawString(str, numberFont, b, drawRect, sf);
										}
										break;

									case ShapeMode.Text:
										using (StringFormat sf = new StringFormat()
										{
											LineAlignment = StringAlignment.Center,
											Alignment = StringAlignment.Center,
										})
										{
											g.DrawString("A", SystemFonts.DialogFont, b, drawRect, sf);
										}
										break;
								}
							}
						}
					}
				}
			}
		}

		private Color currentColor = Color.Black;

		public Color CurrentColor
		{
			get { return currentColor; }
			set { currentColor = value;
			if (currentColor.IsEmpty) currentColor = Color.Black;
			Invalidate();
			}
		}
	}

	public enum ShapeMode
	{
		Rectangle,
		RoundedRect,
		Ellipse,
		Line,
		Arrow,
		Number,
		Text,
	}
}
