/*****************************************************************************
 * 
 * Color select panel control
 * 
 * Standard Windows Control Compatible
 * 
 * Copyright © 2012 UNVELL All rights reserved.
 * Copyright (c) 2019 Jingwood, all rights reserved.
 *
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using unvell.Common;
using unvell.JustCapture;

namespace unvell.UIControls
{
	internal class ColorPickPanel : Control
	{
		public Color CurrentColor
		{
			get { return colorPickerPanel.CurrentColor; }
			set { colorPickerPanel.CurrentColor = value; }
		}

		private SolidColorPickerPanel colorPickerPanel = new SolidColorPickerPanel();

		private Panel panel;

		public ColorPickPanel()
			: base()
		{
			this.TabStop = true;
			this.Margin = this.Padding = new Padding(1);
			this.AutoSize = false;

			panel = new Panel();
			panel.AutoSize = false;
			panel.Location = new Point(0, 0);
			panel.Dock = DockStyle.Fill;

			colorPickerPanel.Dock = DockStyle.Fill;
			panel.Controls.Add(colorPickerPanel);
			colorPickerPanel.ColorPicked += new EventHandler(colorPickerPanel_ColorPicked);
			colorPickerPanel.BringToFront();

			Controls.Add(panel);

			this.Size = new Size(172, 195);
		}

		void colorPickerPanel_ColorPicked(object sender, EventArgs e)
		{
			if (ColorPicked != null) ColorPicked(this, e);
		}

		public event EventHandler ColorPicked;
	}

	internal class SolidColorPickerPanel : Control
	{
		private static Color[] recentColor = new Color[8]{
			Color.White,
			Color.White,
			Color.White,
			Color.White,
			Color.White,
			Color.White,
			Color.White,
			Color.White,
		};
		
		internal event EventHandler ColorPicked;
	
		public SolidColorPickerPanel()
			: base()
		{
			DoubleBuffered = true;
		}

		private int hoverColorIndex = -1;

		private int selectedColorIndex;

		private Color currentColor;

		public Color CurrentColor
		{
			get { return currentColor; }
			set { currentColor = value;
			selectedColorIndex = GetIndexByColor(value);
			}
		}

		bool isAlphaPressed = false;

		private void PickColor(Color color)
		{
			currentColor = color;

			if (ColorPicked != null) ColorPicked.Invoke(this, null);

			Invalidate();
		}

		public static void AddRecentColor(Color color)
		{
			if (recentColor[0] != color)
			{
				Array.Copy(recentColor, 0, recentColor, 1, 7);
				recentColor[0] = color;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			hoverColorIndex = GetColorIndexByPoint(e.Location);
			if (hoverColorIndex != -1)
			{
				Color color;

				if (hoverColorIndex == 41)
				{
					using (ColorDialog cd = new ColorDialog())
					{
						cd.FullOpen = true;
						cd.Color = currentColor;
						if (cd.ShowDialog() == DialogResult.OK)
						{
							color = GetTranparentedColor(cd.Color);
						}
						else
							return;
					}

					PickColor(color);
				}
				else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
				{
					
					using (ColorDialog cd = new ColorDialog())
					{
						cd.FullOpen = true;
						cd.Color = GetColorByIndex(hoverColorIndex);
						if (cd.ShowDialog() == DialogResult.OK)
						{
							color = GetTranparentedColor(cd.Color);
						}
						else
							return;
					}
				}
				else
				{
					PickColor(GetColorByIndex(hoverColorIndex));
				}
			}
			else if (isAlphaPressed)
			{
				isAlphaPressed = false;

				int a = (int)((e.X - transparentRect.Left) * 255 / transparentRect.Width);
				if (a < 0) a = 0;
				else if (a > 255) a = 255;

				PickColor(Color.FromArgb(a, currentColor.R, currentColor.G, currentColor.B));
			}
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			hoverColorIndex = GetColorIndexByPoint(e.Location);
			if (hoverColorIndex == -1)
			{
				if (e.Button == MouseButtons.Left
					&& isAlphaPressed)
				{
					int a = (int)((e.X - transparentRect.Left) * 255 / transparentRect.Width);
					if (a < 0) a = 0;
					else if (a > 255) a = 255;

					currentColor = Color.FromArgb(a, currentColor.R, currentColor.G, currentColor.B);
					Invalidate();
				}
			}
			Invalidate();
			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			hoverColorIndex = GetColorIndexByPoint(e.Location);
			if (hoverColorIndex == -1)
			{
				if (transparentRect.Y < e.Y && transparentRect.Bottom + 9 > e.Y)
				{
					int a = (int)((e.X - transparentRect.Left) * 255 / transparentRect.Width);
					if (a < 0) a = 0;
					else if (a > 255) a = 255;

					currentColor = Color.FromArgb(a, currentColor.R, currentColor.G, currentColor.B);

					isAlphaPressed = true;
					Invalidate();
				}
			}
			else
			{
				base.OnMouseDown(e);
			}
		}

		#region Predefined Colors
		protected static readonly Color[,] fixedColor =  {
														{ // gray
														Color.FromArgb(255,255,255),
														Color.FromArgb(238,238,238),
														Color.FromArgb(208,208,208),
														Color.FromArgb(80,80, 80),
														Color.FromArgb(0, 0, 0),
														}, { // red
														Color.FromArgb(255,238,238),
														Color.FromArgb(255,208,208),
														Color.FromArgb(255,153,153),
														Color.FromArgb(255,0, 0),
														Color.FromArgb(153, 0, 0),
														}, { // oarign
														Color.FromArgb(255,238,218),
														Color.FromArgb(255,208,153),
														Color.FromArgb(255,153,100),
														Color.FromArgb(255,153,0),
														Color.FromArgb(153,50,0),
														}, { // yellow
														Color.FromArgb(255,255,238),
														Color.FromArgb(255,255,208),
														Color.FromArgb(255,255,153),
														Color.FromArgb(255,255,0),
														Color.FromArgb(153, 153, 0),
														}, { // green
														Color.FromArgb(238,255,238),
														Color.FromArgb(208,255,208),
														Color.FromArgb(112, 255,112),
														Color.FromArgb(0, 255, 0),
														Color.FromArgb(0, 153, 0),
														}, { // sky
														Color.FromArgb(238,255,255),
														Color.FromArgb(192,255,255),
														Color.FromArgb(153,255,255),
														Color.FromArgb(0, 255,255),
														Color.FromArgb(0, 153, 153),
														}, { // blue
														Color.FromArgb(238,238,255),
														Color.FromArgb(208,208,255),
														Color.FromArgb(153,153,255),
														Color.FromArgb(0,0, 255),
														Color.FromArgb(0, 0, 153),
														}, { // maginate
														Color.FromArgb(255,238,255),
														Color.FromArgb(255,208,255),
														Color.FromArgb(255,153,255),
														Color.FromArgb(255, 0,255),
														Color.FromArgb(153, 0, 153),
														},
												 };
		#endregion

		Rectangle transparentRect = new Rectangle(7, 170, 155, 10);

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			int index = -1;
			using (SolidBrush b = new SolidBrush(Color.White))
			{
				for (int x = 8, i = 0; x < 160 && i < fixedColor.Length; i++, x += 20)
				{
					for (int y = 8, p = 0; y < 100; p++, y += 20)
					{
						index++;

						if (index == hoverColorIndex)
						{
							b.Color = SystemColors.Highlight;
							g.FillRectangle(b, x - 2, y - 2, 18, 18);
							g.DrawRectangle(SystemPens.WindowFrame, x - 2, y - 2, 18, 18);
						}

						if (index == selectedColorIndex)
						{
							g.DrawRectangle(SystemPens.Highlight, x - 2, y - 2, 18, 18);
						}

						b.Color = fixedColor[i, p];

						g.DrawRectangle(Pens.Black, x, y, 14, 14);
						g.FillRectangle(b, x + 1, y + 1, 13, 13);
					}
				}

				g.DrawLine(SystemPens.ControlDark, 4, 110, ClientRectangle.Width - 4, 110);

				// no color
				index++;

				if (index == hoverColorIndex)
				{
					b.Color = SystemColors.Highlight;
					g.FillRectangle(b, 6, 113, 80, 21);
					g.DrawRectangle(SystemPens.WindowFrame, 6, 113, 80, 21);
				}

				if (index == selectedColorIndex)
				{
					g.DrawRectangle(SystemPens.Highlight, 6, 113, 80, 21);
				}

				g.DrawRectangle(Pens.Black, 8, 116, 14, 14);
				g.DrawLine(Pens.Black, 8, 130, 22, 116);
				g.DrawString(LangResource.no_color, Font,
					index == hoverColorIndex ?
					SystemBrushes.HighlightText : SystemBrushes.WindowText,
					new Rectangle(26, 116, 60, 20));

				// more 
				index++;

				if (index == hoverColorIndex)
				{
					b.Color = SystemColors.Highlight;
					g.FillRectangle(b, 95, 113, 68, 21);
					g.DrawRectangle(SystemPens.WindowFrame, 95, 113, 68, 21);
				}

				if (index == selectedColorIndex)
				{
					g.DrawRectangle(SystemPens.Highlight, 95, 113, 68, 21);
				}

				g.DrawLine(SystemPens.ControlDark, 90, 115, 90, 132);

				b.Color = currentColor;
				g.FillRectangle(b, 100, 116, 14, 14);

				g.DrawRectangle(Pens.Black, 100, 116, 14, 14);
				g.DrawString(LangResource.btn_more, Font,
					index == hoverColorIndex ?
					SystemBrushes.HighlightText : SystemBrushes.WindowText,
					new Rectangle(118, 116, 60, 20));

				g.DrawLine(SystemPens.ControlDark, 4, 138, ClientRectangle.Width - 4, 138);

				// recent colors
				for (int x = 8, k = 0; x < 160; k++, x += 20)
				{
					index++;

					if (index == hoverColorIndex)
					{
						b.Color = SystemColors.Highlight;
						g.FillRectangle(b, x - 2, 142, 18, 18);
						g.DrawRectangle(SystemPens.WindowFrame, x - 2, 142, 18, 18);
					}

					if (index == selectedColorIndex)
					{
						g.DrawRectangle(SystemPens.Highlight, x - 2, 142, 18, 18);
					}

					b.Color = recentColor[k];
					using (SolidBrush sb = new SolidBrush(b.Color))
					{
						g.FillRectangle(sb, x, 144, 14, 14);
						g.DrawRectangle(Pens.Black, x, 144, 14, 14);
					}
				}

				// transparent

				g.SetClip(transparentRect);

				int u = 0;
				for (int y = transparentRect.Top; y < transparentRect.Bottom; y += 5)
					for (int x = transparentRect.Left; x < transparentRect.Right; x += 5)
					{
						g.FillRectangle((u++ % 2) == 1 ? Brushes.White : Brushes.Gainsboro,
							x, y, 5, 5);
					}

				using (Brush lineBrush = new LinearGradientBrush(transparentRect,
					Color.FromArgb(0, Color.White), Color.FromArgb(255, Color.White),
					LinearGradientMode.Horizontal))
					g.FillRectangle(lineBrush, transparentRect);

				g.ResetClip();
				g.DrawRectangle(Pens.DimGray, transparentRect);

				GraphicsToolkit.FillTriangle(g, 10,
					new Point(transparentRect.Left  + (int)(currentColor.A * transparentRect.Width / 255),
					transparentRect.Bottom+6), GraphicsToolkit.TriangleDirection.Up);
			}
		}

		protected virtual Color GetTranparentedColor(Color c)
		{
			if (currentColor == Color.Empty || currentColor == Color.Transparent) return c;
			return Color.FromArgb(currentColor.A, c.R, c.G, c.B);
		}

		public virtual int GetColorIndexByPoint(Point p)
		{
			int i = -1;
			for (int x = 8; x < 160; x += 20)
			{
				for (int y = 8; y < 100; y += 20)
				{
					i++;
					if (GraphicsToolkit.PointInRect(new Rectangle(x - 2, y - 2, 19, 19), p))
						return i;
				}
			}

			i++;
			// no color : 40
			if (GraphicsToolkit.PointInRect(new Rectangle(8, 112, 88, 20), p))
				return i;

			i++;
			// more : 41
			if (GraphicsToolkit.PointInRect(new Rectangle(96, 112, 70, 20), p))
				return i;

			// recent : 42 ~ 50
			for (int x = 8; x < 160; x += 20)
			{
				i++;
				if (GraphicsToolkit.PointInRect(new Rectangle(x - 2, 142, 19, 19), p))
					return i;
			}

			return -1;
		}

		public virtual Color GetColorByIndex(int i, bool modify)
		{
			Color color = GetColorByIndex(i);
			if (modify && i != 41 /* more color */)
			{
				using (ColorDialog cd = new ColorDialog())
				{
					cd.FullOpen = true;
					cd.Color = color;
					if (cd.ShowDialog() == DialogResult.OK)
					{
						return GetTranparentedColor(cd.Color);
					}
				}
			}
			
			return color;
		}

		private Color GetColorByIndex(int i)
		{
			if (i < 0)
				return Color.Empty;
			else if (i < 40)
				return GetTranparentedColor(fixedColor[(i / 5), i % 5]);
			else if (i == 40)
				return Color.Empty;
			else if (i == 41)
			{
				return Color.Empty;
			}
			else if (i <= 49)
				return recentColor[i - 42];
			else
				return Color.Empty;
		}

		private int GetIndexByColor(Color color)
		{
			// no color : 40
			if (color.IsEmpty) return 40;

			int i = 0;

			int ex = fixedColor.GetLength(0);
			int ey = fixedColor.GetLength(1);

			// fixed color : 0 - 39
			for (int x = 0; x < ex; x++)
			{
				for (int y = 0; y < ey; y++)
				{
					if (fixedColor[x, y].R == color.R && fixedColor[x, y].G == color.G && fixedColor[x, y].B == color.B)
						return i;
					i++;
				}
			}

			i++;

			// recent : 42 ~ 50
			for (int k = 0; k < recentColor.Length; k++)
			{
				i++;
				if (recentColor[k].R == color.R && recentColor[k].G == color.G && recentColor[k].B == color.B)
					return i;
			}

			// can't find, return more : 41
			
			return 41;
		}

	}


}
