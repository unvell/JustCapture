/*****************************************************************************
 * Common Graphics Toolkit
 * 
 * Copyright © 2012 UNVELL All rights reserved
 *
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace unvell.Common
{
	internal sealed class GraphicsToolkit
	{
		#region Calclatue
		internal static double DistancePointToLine(PointF startPoint, PointF endPoint,
			PointF target)
		{
			return DistancePointToLine(startPoint.X, startPoint.Y,
				endPoint.X, endPoint.Y, target);
		}
		internal static double DistancePointToLine(float x1, float y1, float x2, float y2, PointF target)
		{
			double a, b, c;
			a = y2 - y1;
			b = x1 - x2;
			c = x2 * y1 - x1 * y2;
			return Math.Abs(a * target.X + b * target.Y + c) / Math.Sqrt(a * a + b * b);
		}
		internal static double DistancePointToLines(PointF[] points, PointF target, double min)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				float x1 = Math.Min(points[i].X, points[i + 1].X);
				float x2 = Math.Max(points[i].X, points[i + 1].X);
				float y1 = Math.Min(points[i].Y, points[i + 1].Y);
				float y2 = Math.Max(points[i].Y, points[i + 1].Y);
				if (target.X > x1 - min && target.X < x2 + min
						&& target.Y > y1 - min && target.Y < y2 + min)
					min = Math.Min(min, DistancePointToLine(points[i], points[i + 1], target));
			}
			return min;
		}
		/* 
		 * @refer http://www.geometrictools.com/Documentation/DistancePointToEllipse2.pdf
		 */
		internal static double DistancePointEllipseSpecial(double dU, double dV, double dA,
			double dB, double dEpsilon, int iMax, int riIFinal,
			double rdX, double rdY)
		{
			// initial guess
			double dT = dB * (dV - dB);
			// Newton’s method
			int i;
			for (i = 0; i < iMax; i++)
			{
				double dTpASqr = dT + dA * dA;
				double dTpBSqr = dT + dB * dB;
				double dInvTpASqr = 1.0 / dTpASqr;
				double dInvTpBSqr = 1.0 / dTpBSqr;
				double dXDivA = dA * dU * dInvTpASqr;
				double dYDivB = dB * dV * dInvTpBSqr;
				double dXDivASqr = dXDivA * dXDivA;
				double dYDivBSqr = dYDivB * dYDivB;
				double dF = dXDivASqr + dYDivBSqr - 1.0;
				if (dF < dEpsilon)
				{
					// F(t0) is close enough to zero, terminate the iteration
					rdX = dXDivA * dA;
					rdY = dYDivB * dB;
					riIFinal = i;
					break;
				}
				double dFDer = 2.0 * (dXDivASqr * dInvTpASqr + dYDivBSqr * dInvTpBSqr);
				double dRatio = dF / dFDer;
				if (dRatio < dEpsilon)
				{
					// t1-t0 is close enough to zero, terminate the iteration
					rdX = dXDivA * dA;
					rdY = dYDivB * dB;
					riIFinal = i;
					break;
				}
				dT += dRatio;
			}
			if (i == iMax)
			{
				// method failed to converge, let caller know
				riIFinal = -1;
				return -999999;
			}
			double dDelta0 = rdX - dU, dDelta1 = rdY - dV;
			return Math.Sqrt(dDelta0 * dDelta0 + dDelta1 * dDelta1);
		}
		internal static double DistancePointEllipse(
			double dU, double dV, // test point (u,v)
			double dA, double dB, // ellipse is (x/a)^2 + (y/b)^2 = 1
			double dEpsilon, // zero tolerance for Newton’s method
			int iMax, // maximum iterations in Newton’s method
			ref int riIFinal, // number of iterations used
			ref double rdX, ref double rdY) // a closest point (x,y)
		{
			// special case of circle
			if (Math.Abs(dA - dB) < dEpsilon)
			{
				double dLength = Math.Sqrt(dU * dU + dV * dV);

				return Math.Abs(dLength - dA);
			}
			// reflect U = -U if necessary, clamp to zero if necessary
			bool bXReflect;
			if (dU > dEpsilon)
			{
				bXReflect = false;
			}
			else if (dU < -dEpsilon)
			{
				bXReflect = true;
				dU = -dU;
			}
			else
			{
				bXReflect = false;
				dU = 0.0;
			}
			// reflect V = -V if necessary, clamp to zero if necessary
			bool bYReflect;
			if (dV > dEpsilon)
			{
				bYReflect = false;
			}
			else if (dV < -dEpsilon)
			{
				bYReflect = true;
				dV = -dV;
			}
			else
			{
				bYReflect = false;
				dV = 0.0;
			}
			// transpose if necessary
			double dSave;
			bool bTranspose;
			if (dA >= dB)
			{
				bTranspose = false;
			}
			else
			{
				bTranspose = true;
				dSave = dA;
				dA = dB;
				dB = dSave;
				dSave = dU;
				dU = dV;

				dV = dSave;
			}
			double dDistance;
			if (dU != 0.0)
			{
				if (dV != 0.0)
				{
					dDistance = DistancePointEllipseSpecial(dU, dV, dA, dB, dEpsilon, iMax,
					riIFinal, rdX, rdY);
				}
				else
				{
					double dBSqr = dB * dB;
					if (dU < dA - dBSqr / dA)
					{
						double dASqr = dA * dA;
						rdX = dASqr * dU / (dASqr - dBSqr);
						double dXDivA = rdX / dA;
						rdY = dB * Math.Sqrt(Math.Abs(1.0 - dXDivA * dXDivA));
						double dXDelta = rdX - dU;
						dDistance = Math.Sqrt(dXDelta * dXDelta + rdY * rdY);
						riIFinal = 0;
					}
					else
					{
						dDistance = Math.Abs(dU - dA);
						rdX = dA;
						rdY = 0.0;
						riIFinal = 0;
					}
				}
			}
			else
			{
				dDistance = Math.Abs(dV - dB);
				rdX = 0.0;
				rdY = dB;
				riIFinal = 0;
			}
			if (bTranspose)
			{
				dSave = rdX;
				rdX = rdY;
				rdY = dSave;
			}
			if (bYReflect)
			{
				rdY = -rdY;
			}
			if (bXReflect)
			{
				rdX = -rdX;
			}
			return dDistance;
		}
		internal static double DistancePointToPolygonBound(Point[] points, Point p, double min)
		{
			double mindis = min + 1;
			Point linePoint = points[points.Length - 1];
			foreach (Point linePoint2 in points)
				{
					mindis = Math.Min(mindis,
						DistancePointToLine(linePoint, linePoint2, p));
					linePoint = linePoint2;
				};
			return mindis;
		}
		internal static bool PointInPolygon(Point[] points, Point p)
		{
			int i, j = points.Length - 1;
			bool oddNodes = false;

			for (i = 0; i < points.Length; i++)
			{
				PointF polyi = points[i];
				PointF polyj = points[j];
				if (polyi.Y < p.Y && polyj.Y >= p.Y
				|| polyj.Y < p.Y && polyi.Y >= p.Y)
				{
					if (polyi.X + (p.Y - polyi.Y) / (polyj.Y - polyi.Y) * (polyj.X - polyi.X) < p.X)
					{
						oddNodes = !oddNodes;
					}
				}
				j = i;
			}

			return oddNodes;
		}
		internal static bool PointInRect(RectangleF rect, PointF p)
		{
			return rect.Left <= p.X && rect.Top <= p.Y
				&& rect.Right >= p.X && rect.Bottom >= p.Y;
		}
		internal static double DistancePointToRectBound(Rectangle rect, Point p)
		{
			return DistancePointToPolygonBound(new Point[]{
				new Point(rect.Left,rect.Top),
				new	Point(rect.Right,rect.Top),
				new Point(rect.Right,rect.Bottom),
				new Point(rect.Left,rect.Bottom)}, p, 4);
		}
		internal static PointF GetStraightLinePoint(PointF a, PointF b)
		{
			PointF dir = new PointF(b.X - a.X, b.Y - a.Y);
			double theta = Math.Atan2(dir.Y, dir.X);
			double len = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);

			theta = Math.Round(4 * theta / Math.PI) * Math.PI / 4;
			return new PointF((float)(a.X + len * Math.Cos(theta)),
				(float)(a.Y + len * Math.Sin(theta)));
		}
		#endregion

		#region Drawing
		public enum TriangleDirection
		{
			Left,
			Up,
			Right,
			Down,
		}
		public static void FillTriangle(Graphics g, int width, Point loc)
		{
			FillTriangle(g, width, loc, TriangleDirection.Down);
		}
		public static void FillTriangle(Graphics g, int size, Point loc, TriangleDirection dir)
		{
			int x = loc.X;
			int y = loc.Y;

			switch (dir)
			{
				case TriangleDirection.Up:
					loc.X -= size / 2;
					for (x = 0; x < size / 2 ; x++)
					{
						g.DrawLine(Pens.Black, loc.X + x, y, loc.X + size - x-1, y);
						y--;
					}
					break;

				case TriangleDirection.Down:
					loc.X -= size / 2;
					for (x = 0; x < size / 2 ; x++)
					{
						g.DrawLine(Pens.Black, loc.X + x, y, loc.X + size - x-1, y);
						y++;
					}
					break;

				case TriangleDirection.Left:
					loc.Y -= size / 2;
					for (y = 0; y < size / 2 ; y++)
					{
						g.DrawLine(Pens.Black, x,loc.Y + y,x, loc.Y + size - y-1);
						x--;
					}
					break;	

				case TriangleDirection.Right:
					loc.Y -= size / 2;
					for (y = 0; y < size / 2 ; y++)
					{
						g.DrawLine(Pens.Black, x, loc.Y + y, x, loc.Y + size - y - 1);
						x++;
					}
					break;

			}
		}

		public static void DrawDropdownButton(Graphics g, Rectangle rect, bool isPressed)
		{
			GraphicsToolkit.Draw3DButton(g, rect, isPressed);

			// arrow
			int sx = rect.Left + (rect.Width - 9) / 2;
			GraphicsToolkit.FillTriangle(g, 9, new Point(sx, rect.Top + 7 + (isPressed ? 1 : 0)));
		}

		/// <summary>
		/// Draw a rectangle with double line frame
		/// </summary>
		/// <param name="g">Graphics object</param>
		/// <param name="rect">Rectangle will be draw</param>
		/// <param name="outerColor">Outer line color of frame</param>
		/// <param name="innerColor">Inner line color of frame</param>
		public static void DrawDoubleLineRectangle(Graphics g, Rectangle rect, Color outerColor, Color innerColor)
		{
			using (Pen pOuter = new Pen(outerColor))
			{
				using (Pen pInner = new Pen(innerColor))
				{
					g.DrawRectangle(pOuter, rect);
					rect.Inflate(-1, -1);
					g.DrawRectangle(pInner, rect);
					rect.Inflate(-1, -1);
					g.DrawRectangle(pInner, rect);
					rect.Inflate(-1, -1);
					g.DrawRectangle(pOuter, rect);
				}
			}
		}
		#endregion

		#region Toolkit
		public static void Draw3DButton(Graphics g, Rectangle rect, bool isPressed)
		{
			// background
			Rectangle bgRect = rect;
			//bgRect.Inflate(-1, -1);
			bgRect.Offset(1, 1);
			g.FillRectangle(isPressed ? Brushes.Black : Brushes.White, bgRect);

			// outter frame
			g.DrawLine(Pens.Black, rect.X + 1, rect.Y, rect.Right - 1, rect.Y);
			g.DrawLine(Pens.Black, rect.X + 1, rect.Bottom, rect.Right - 1, rect.Bottom);
			g.DrawLine(Pens.Black, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
			g.DrawLine(Pens.Black, rect.Right, rect.Y + 1, rect.Right, rect.Bottom - 1);

			// content
			Rectangle bodyRect = rect;
			bodyRect.Inflate(-1, -1);
			bodyRect.Offset(1, 1);
			g.FillRectangle(Brushes.LightGray, bodyRect);

			// shadow
			g.DrawLines(isPressed ? Pens.White : Pens.DimGray, new Point[] {
				new Point(rect.Left+1,rect.Bottom-1),
				new Point(rect.Right-1,rect.Bottom-1),
				new Point(rect.Right-1,rect.Top+1),
			});
		}
		public static GraphicsPath AddRoundedRectangle(GraphicsPath path, Rectangle bounds, int c1, int c2, int c3, int c4)
		{
			if (c1 > 0)
				path.AddArc(bounds.Left, bounds.Top, c1, c1, 180, 90);
			else
				path.AddLine(bounds.Left, bounds.Top, bounds.Left, bounds.Top);

			if (c2 > 0)
				path.AddArc(bounds.Right - c2, bounds.Top, c2, c2, 270, 90);
			else
				path.AddLine(bounds.Right, bounds.Top, bounds.Right, bounds.Top);

			if (c3 > 0)
				path.AddArc(bounds.Right - c3, bounds.Bottom - c3, c3, c3, 0, 90);
			else
				path.AddLine(bounds.Right, bounds.Bottom, bounds.Right, bounds.Bottom);

			if (c4 > 0)
				path.AddArc(bounds.Left, bounds.Bottom - c4, c4, c4, 90, 90);
			else
				path.AddLine(bounds.Left, bounds.Bottom, bounds.Left, bounds.Bottom);

			path.CloseFigure();

			return path;
		}

		internal static Color ColorFromWeb(string code)
		{
			if (code.StartsWith("#"))
			{
				code = code.Substring(1);
			}

			if (code.Length == 3)
			{
				return Color.FromArgb(
					Convert.ToInt32(code.Substring(0, 1), 16),
					Convert.ToInt32(code.Substring(1, 1), 16),
					Convert.ToInt32(code.Substring(2, 1), 16));
			}
			if (code.Length == 6)
			{
				return Color.FromArgb(
					Convert.ToInt32(code.Substring(0, 2), 16),
					Convert.ToInt32(code.Substring(2, 2), 16),
					Convert.ToInt32(code.Substring(4, 2), 16));
			}
			else if (code.Length == 8)
			{
				return Color.FromArgb(
					Convert.ToInt32(code.Substring(0, 2), 16),
					Convert.ToInt32(code.Substring(2, 2), 16),
					Convert.ToInt32(code.Substring(3, 2), 16),
					Convert.ToInt32(code.Substring(4, 2), 16));
			}
			else
				return Color.Empty;
		}

		#endregion

	}
}
