/*****************************************************************************
 * 
 * FontToolkit
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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

using unvell.Common;

namespace unvell.Common
{
	internal static class FontToolkit
	{
		//internal static bool Has(this FontStyle flag, FontStyle check)
		//{
		//	return (flag & check) == check;
		//}
		//internal static bool HasAny(this FontStyle flag, FontStyle check)
		//{
		//	return (flag & check) > 0;
		//}
		
		public static string GetFontStyleName(FontStyle style, string regularText,
			string italicText, string boldText)
		{
			List<string> names = new List<string>();

			if ((style & FontStyle.Bold) == FontStyle.Bold)
			{
				names.Add(boldText);
			}
			if ((style & FontStyle.Italic) == FontStyle.Italic)
			{
				names.Add(italicText);
			}

			if (names.Count == 0)
				return regularText;
			else
				return string.Join(" ", names.ToArray());
		}

		public static FontStyle GetFontStyleByName(string text, string italicText, string boldText)
		{
			string[] styleNames = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			FontStyle fs = FontStyle.Regular;

			foreach (string style in styleNames)
			{
				if (string.Compare(style, italicText, true) == 0)
				{
					fs |= FontStyle.Italic;
				}
				else if (string.Compare(style, boldText, true) == 0)
				{
					fs |= FontStyle.Bold;
				}
			}

			return fs;
		}

		public static readonly float[] FontSizeList = new float[] {
            5f, 6f, 7f, 8f, 8.25f, 9f, 10f, 10.5f, 11f, 11.5f, 12f, 12.5f, 14f, 16f, 18f,
            20f, 22f, 24f, 26f, 28f, 30f, 32f, 34f, 38f, 46f, 58f, 64f, 78f, 92f};

		private static readonly float FixedDrawFontSize = 14f;

		public static void DrawFontItem(Graphics g, FontFamilyInfo fontFamily,
			Rectangle rect, bool isSelected)
		{
			using (StringFormat sf = new StringFormat()
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center,
			})
			{
				sf.FormatFlags |= StringFormatFlags.NoWrap;

				using (FontFamily ff = new FontFamily(fontFamily.CultureName))
				{
					Font font = null;

					// some fonts are not support Regular style, we need find 
					// a style which is supported by current font

					if (ff.IsStyleAvailable(FontStyle.Regular))
						font = new Font(fontFamily.CultureName, FixedDrawFontSize, FontStyle.Regular);
					else if (ff.IsStyleAvailable(FontStyle.Bold))
						font = new Font(fontFamily.CultureName, FixedDrawFontSize, FontStyle.Bold);
					else if (ff.IsStyleAvailable(FontStyle.Italic))
						font = new Font(fontFamily.CultureName, FixedDrawFontSize, FontStyle.Italic);
					else if (ff.IsStyleAvailable(FontStyle.Strikeout))
						font = new Font(fontFamily.CultureName, FixedDrawFontSize, FontStyle.Strikeout);
					else if (ff.IsStyleAvailable(FontStyle.Underline))
						font = new Font(fontFamily.CultureName, FixedDrawFontSize, FontStyle.Underline);

					if (font != null)
					{
						g.DrawString(font.Name, font,
							isSelected ? SystemBrushes.HighlightText : Brushes.Black, rect, sf);

						font.Dispose();
					}
				}
			}
		}

		public static float GetLargerSize(float size)
		{
			float largest = FontToolkit.FontSizeList[FontToolkit.FontSizeList.Length - 1];
			if (size > largest) return size;

			for (int i = FontToolkit.FontSizeList.Length - 2; i >= 0; i--)
			{
				if (size >= FontToolkit.FontSizeList[i])
					return FontToolkit.FontSizeList[i + 1];
			}

			return FontToolkit.FontSizeList[0];
		}

		public static float GetSmallerSize(float size)
		{
			float smallest = FontToolkit.FontSizeList[0];
			if (size < smallest) return size;

			for (int i = 1; i < FontToolkit.FontSizeList.Length; i++)
			{
				if (size <= FontToolkit.FontSizeList[i])
					return FontToolkit.FontSizeList[i - 1];
			}

			return FontToolkit.FontSizeList[FontToolkit.FontSizeList.Length - 1];
		}

		#region Font Unicode Support

		public static bool IsAvailableUnicode(Font font, char c)
		{
			List<FontUnicodeRange> ranges = GetFontUnicodeRange(font);
			if (ranges == null) return true;

			foreach (var r in ranges)
			{
				if ((int)c >= r.Low && (int)c <= r.High)
				{
					return true;
				}
			}
			return false;
		}

		private static Dictionary<FontInfo, List<FontUnicodeRange>> cachedUnicodeRanges 
			= new Dictionary<FontInfo, List<FontUnicodeRange>>();

		public static List<FontUnicodeRange> GetFontUnicodeRange(Font font)
		{
			var g = ResourcePoolManager.Instance.CachedGraphics;
			List<FontUnicodeRange> ranges = null;
			foreach (var fr in cachedUnicodeRanges)
			{
				if (fr.Key.FontFamilyInfo != null
					&& fr.Key.FontFamilyInfo.FontFamily != null
					&& fr.Key.FontFamilyInfo.FontFamily.Equals(font.FontFamily))
				{
					ranges = fr.Value;
					break;
				}
			}
			if (ranges == null)
			{
				IntPtr hfont = IntPtr.Zero;
				try
				{
					IntPtr hdc = g.GetHdc();
					hfont = font.ToHfont();

					ranges = GetFontUnicodeRange(hdc, hfont);
					cachedUnicodeRanges[new FontInfo(font)] = ranges;
				}
				finally
				{
					if (hfont != IntPtr.Zero) Win32.DeleteObject(hfont);
					g.ReleaseHdc();
				}
			}
			return ranges;
		}

		public static List<FontUnicodeRange> GetFontUnicodeRange(IntPtr hdc, IntPtr hfont)
		{
			Stopwatch sw = Stopwatch.StartNew();
			Logger.Log("font toolkit", "start collect font unicode range...");

			IntPtr oldFont = IntPtr.Zero;
			IntPtr glyphSet = IntPtr.Zero;

			try
			{
				oldFont = Win32.SelectObject(hdc, hfont);

				uint size = Win32.GetFontUnicodeRanges(hdc, IntPtr.Zero);

				glyphSet = Marshal.AllocHGlobal((int)size);
				Win32.GetFontUnicodeRanges(hdc, glyphSet);

				List<FontUnicodeRange> ranges = new List<FontUnicodeRange>();
				int count = Marshal.ReadInt32(glyphSet, 12);
				for (int i = 0; i < count; i++)
				{
					FontUnicodeRange range = new FontUnicodeRange();
					range.Low = (UInt16)Marshal.ReadInt16(glyphSet, 16 + i * 4);
					range.High = (UInt16)(range.Low + Marshal.ReadInt16(glyphSet, 18 + i * 4) - 1);
					ranges.Add(range);
				}

				return ranges;
			}
			finally
			{
				if (oldFont != IntPtr.Zero) Win32.SelectObject(hdc, oldFont);
				if (glyphSet != IntPtr.Zero) Marshal.FreeHGlobal(glyphSet);

				sw.Stop();
				Logger.Log("font toolkit", "finish collect font unicode range. (" + sw.ElapsedMilliseconds + " ms)");
			}

		}

		#endregion

		public static Font GetFontIfExisted(string name, float size, FontStyle style)
		{
			Font f = null;
			try
			{
				FontFamily ff = new FontFamily(name);
				if (!ff.IsStyleAvailable(style)) style = FontStyle.Regular;
				f = new Font(ff, size, style);
			}
			catch (Exception ex)
			{
				Logger.Log("font toolkit", "info: " + ex.Message);

				f = SystemFonts.DefaultFont;
			}
			return f;
		}

		public static FontFamily GetFontFamilyIfExisted(string name)
		{
			try
			{
				return new FontFamily(name);
			}
			catch (Exception ex)
			{
				Logger.Log("font toolkit", "font family does not exist: " + ex.Message);
				return null;
			}
		}

		public static bool AreCloseSize(float s1, float s2)
		{
			return Math.Abs(s1 - s2) < 0.3f;
		}

		static FontToolkit()
		{
			InitFontFamilies();
		}

		private static FontFamilyInfo[] fontFamilies;
		public static FontFamilyInfo[] FontFamilies { get { return FontToolkit.fontFamilies; } }

		public static FontFamilyInfo GetFontFamilyInfo(string fontName)
		{
			if (fontFamilies == null)
			{
				InitFontFamilies();

				if (fontFamilies == null) return null;
			}

			foreach (var ffi in fontFamilies)
			{
				foreach (var name in ffi.Names)
				{
					if (name.Equals(fontName, StringComparison.CurrentCultureIgnoreCase))
					{
						return ffi;
					}
				}
			}

			return null;
		}

		public static FontFamilyInfo GetFontFamilyInfo(FontFamily fontFamily)
		{
			if (fontFamilies == null)
			{
				InitFontFamilies();

				if (fontFamilies == null) return null;
			}

			foreach (var ffi in fontFamilies)
			{
				if (ffi.FontFamily.Equals(fontFamily))
				{
					return ffi;
				}
			}

			return null;
		}

		public static FontFamilyInfo GetFontFamilyInfo(Font font)
		{
			return font == null ? null : GetFontFamilyInfo(font.FontFamily);
		}

		public static void InitFontFamilies()
		{
			Stopwatch sw = Stopwatch.StartNew();
			Logger.Log("font toolkit", "start cache fonts family names...");

			fontFamilies = new FontFamilyInfo[FontFamily.Families.Length];

			for (int i = 0; i < FontFamily.Families.Length; i++)
			{
				var ff = FontFamily.Families[i];

				string[] names = new string[3];

				// us english font name 
				names[0] = ff.GetName((int)OTFLanguageID.WLID_English_UnitedStates);
				// japanese font name
				names[1] = ff.GetName((int)OTFLanguageID.WLID_Japanese_Japan);
				// chinese font name
				names[2] = ff.GetName((int)OTFLanguageID.WLID_Chinese_PRC);

				fontFamilies[i] = new FontFamilyInfo(ff, names);
			}

			sw.Stop();
			Logger.Log("font toolkit", "finish cache fonts family names. (" + sw.ElapsedMilliseconds + " ms)");
		}
	}

	public struct FontUnicodeRange
	{
		public UInt16 Low;
		public UInt16 High;
	}

	public class FontFamilyInfo
	{
		private string[] names;
		public string[] Names { get { return names; } }

		public FontFamily FontFamily { get; private set; }

		public FontFamilyInfo(FontFamily fontFamily, params string[] names)
		{
			this.FontFamily = fontFamily;
			this.cultureName = fontFamily.Name;
			this.names = names;
		}

		private string cultureName;

		public string CultureName
		{
			get
			{
				return cultureName; // TODO: get culture name reference to OS
			}
		}

		public bool IsFamilyName(string name)
		{
			if(names == null)return false;

			foreach (var n in names)
			{
				if (n.StartsWith(name, StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		public override string ToString()
		{
			return cultureName;
		}
	}

	public class FontInfo
	{
		private FontFamilyInfo fontFamilyInfo;

		public FontFamilyInfo FontFamilyInfo
		{
			get { return fontFamilyInfo; }
			set { fontFamilyInfo = value; }
		}

		public string Name { get { return fontFamilyInfo.CultureName; } }

		private float size;
		public float Size { get { return size; } set { size = value; } }

		private FontStyle style;
		public FontStyle Style { get { return style; } set { style = value; } }
		
		//public FontInfo()
		//{
		//}

		public FontInfo(Font font)
			: this(font.Name, font.Size, font.Style)
		{
		}

		public FontInfo(FontInfo proto)
		{
			this.fontFamilyInfo = proto.fontFamilyInfo;
			this.size = proto.size;
			this.style = proto.style;
		}

		public FontInfo(string name, float size, FontStyle style)
			: this(FontToolkit.GetFontFamilyInfo(name), size, style)
		{
		}

		public FontInfo(FontFamilyInfo fontFamilyInfo, float size, FontStyle style)
		{
#if DEBUG
			Debug.Assert(fontFamilyInfo != null);
#endif

			this.fontFamilyInfo = fontFamilyInfo;
			this.size = size;
			this.style = style;
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is FontInfo)) return false;
			FontInfo f2 = (FontInfo)obj;
			return string.Equals(Name, f2.Name, StringComparison.CurrentCultureIgnoreCase)
				&& size == f2.size && style == f2.style;
		}

		public bool Equals(string name, float size, FontStyle fontStyle)
		{
			return string.Equals(Name, name, StringComparison.CurrentCultureIgnoreCase)
				&& this.size == size
				&& this.style == fontStyle;
		}

		public bool Equals(Font font)
		{
			return font != null && Equals(font.Name, font.Size, font.Style);
		}

		public override int GetHashCode()
		{
			return fontFamilyInfo.GetHashCode() ^ (int)size ^ (int)style;
		}

		//public static bool operator ==(FontInfo f1, FontInfo f2)
		//{
		//  return (f1.Equals(f2));
		//}

		//public static bool operator !=(FontInfo f1, FontInfo f2)
		//{
		//  return !(f1.Equals(f2));
		//}
	}

	public enum OTFLanguageID : int
	{
		WLID_Afrikaans_SouthAfrica = 0x0436,
		WLID_Armenian_Albania = 0x042B,
		WLID_Chinese_HK = 0x0C04,
		WLID_Chinese_Macao = 0x1404,
		WLID_Chinese_PRC = 0x0804,
		WLID_Chinese_Singapore = 0x1004,
		WLID_Chinese_Taiwan = 0x0404,
		WLID_English_UnitedKingdom = 0x0809,
		WLID_English_UnitedStates = 0x0409,
		WLID_German_Germany = 0x0407,
		WLID_Japanese_Japan = 0x0411,
		WLID_Portuguese_Brazil = 0x0416,
	}
}
