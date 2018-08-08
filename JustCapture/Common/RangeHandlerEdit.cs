using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace unvell.Common
{
	internal sealed class RangeHandlerEdit
	{
		private static readonly int size = 8;
		private static readonly int hs = size / 2;
		
		public static void DrawRangeHandlerPos(Graphics g, Rectangle[] hands, Color color)
		{
			using (Pen p = new Pen(color, 2))
			{
				foreach (Rectangle hand in hands)
				{
					g.DrawRectangle(p, hand);
				}
			}
		}

		public static RangeHandlerPos InRangeHandler(Rectangle[] hands, Point p)
		{
			if (hands == null) return RangeHandlerPos.None;

			for (int i = 0; i < hands.Length; i++)
			{
				Rectangle rect = hands[i];
				if (GraphicsToolkit.PointInRect(rect, p)) return (RangeHandlerPos)i;
			}
			return RangeHandlerPos.None;
		}

		public static Cursor CursorOnRangeHandler(Rectangle[] hands, Point p)
		{
			RangeHandlerPos pos = InRangeHandler(hands, p);
			switch (pos)
			{
				default:
					return Cursors.Default;

				case RangeHandlerPos.Left:
				case RangeHandlerPos.Right:
					return Cursors.SizeWE;

				case RangeHandlerPos.Top:
				case RangeHandlerPos.Bottom:
					return Cursors.SizeNS;

				case RangeHandlerPos.LeftTop:
				case RangeHandlerPos.RightBottom:
					return Cursors.SizeNWSE;

				case RangeHandlerPos.RightTop:
				case RangeHandlerPos.LeftBottom:
					return Cursors.SizeNESW;
			}
		}

		public static void UpdateRangeHandlers(Rectangle rect, Rectangle[] hands)
		{
			UpdateRangeHandlers(rect, hands, RangeHandlerPos.None);
		}

		public static void UpdateRangeHandlers(Rectangle rect, Rectangle[] hands, RangeHandlerPos pos)
		{
			int hx = rect.Left + rect.Width / 2 - hs - 1;
			int hy = rect.Top + rect.Height / 2 - hs - 1;

			if(pos == RangeHandlerPos.None || pos == RangeHandlerPos.LeftTop)
				hands[(int)RangeHandlerPos.LeftTop] = new Rectangle(rect.X - hs - 1, rect.Y - hs - 1, size, size);
			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.LeftBottom)
				hands[(int)RangeHandlerPos.LeftBottom] = new Rectangle(rect.X - hs - 1, rect.Bottom - hs + 1, size, size);
			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.RightTop)
				hands[(int)RangeHandlerPos.RightTop] = new Rectangle(rect.Right - hs + 1, rect.Y - hs - 1, size, size);
			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.RightBottom)
				hands[(int)RangeHandlerPos.RightBottom] = new Rectangle(rect.Right - hs + 1, rect.Bottom - hs + 1, size, size);

			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.Left)
				hands[(int)RangeHandlerPos.Left] = new Rectangle(rect.X - hs - 1, hy, size, size);
			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.Right)
				hands[(int)RangeHandlerPos.Right] = new Rectangle(rect.Right - hs + 1, hy, size, size);
			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.Top)
				hands[(int)RangeHandlerPos.Top] = new Rectangle(hx, rect.Top - hs - 1, size, size);
			if (pos == RangeHandlerPos.None || pos == RangeHandlerPos.Bottom)
				hands[(int)RangeHandlerPos.Bottom] = new Rectangle(hx, rect.Bottom - hs + 1, size, size);
		}

		public enum RangeHandlerPos : int
		{
			None = -1,
			Left = 0,
			Top = 1,
			Bottom = 2,
			Right = 3,
			LeftTop = 4,
			RightTop = 5,
			LeftBottom = 6,
			RightBottom = 7,
		}
	}
}
