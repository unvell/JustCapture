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
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace unvell.JustCapture.Toolkits
{
	internal sealed class FileToolkit
	{
		internal static string EncodeColor(Color c)
		{
			return c.IsEmpty ? "none" :
				(c.IsNamedColor ? c.Name :
				(c.A == 255 ? (string.Format("#{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B))
				: string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", c.A, c.R, c.G, c.B)));
		}

		internal static readonly Regex RGBColorRegex =
			new Regex(@"rgb\s*\(\s*((\d+)\s*,)?(\d+)\s*,(\d+)\s*,(\d+)\s*\)");
		internal static readonly Regex WebColorRegex = new
				Regex(@"\#([0-9a-fA-F]{2})?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})");
		
		public static Color DecodeColor(string data)
		{
			if (data == null || data.Length == 0 || data.ToLower().Equals("none"))
			{
				return Color.Empty;
			}

			Match m = RGBColorRegex.Match(data.ToLower());
			if (m.Success)
			{
				return Color.FromArgb(m.Groups[2].Value.Length > 0 ?
					Convert.ToInt32(m.Groups[2].Value) : 255,
					Convert.ToInt32(m.Groups[3].Value),
					Convert.ToInt32(m.Groups[4].Value),
					Convert.ToInt32(m.Groups[5].Value));
			}
			else if ((m = WebColorRegex.Match(data)).Success)
			{
				return Color.FromArgb(m.Groups[1].Value.Length > 0 ?
									Convert.ToInt32(m.Groups[1].Value, 16) : 255,
									Convert.ToInt32(m.Groups[2].Value, 16),
									Convert.ToInt32(m.Groups[3].Value, 16),
									Convert.ToInt32(m.Groups[4].Value, 16));
			}
			else
			{
				try { return Color.FromName(data); }
				catch { }
			}
			return Color.Empty;
		}

		public static FileInfo SaveImage(Image image, string filename)
		{
			return SaveImage(image, null, filename);
		}

		public static FileInfo SaveImage(Image image, FileInfo file)
		{
			return SaveImage(image, file, null);
		}

		public static FileInfo SaveImage(Image image, FileInfo file, string filename)
		{
			if (file == null)
			{
				using (SaveFileDialog sfd = new SaveFileDialog())
				{
					sfd.FileName = filename;
					sfd.Filter = "PNG Image(*.png)|*.png|JPEG Image(*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp|GIF Image(*.gif)|*.gif|TIFF Image(*.tiff)|*.tiff";

					if (sfd.ShowDialog() != DialogResult.OK)
					{
						return null;
					}
					
					file = new FileInfo(sfd.FileName);
				}
			}

			string ext = file.Extension.ToLower();

			ImageFormat format;
			switch (ext)
			{
				default:
				case "png":
					format = ImageFormat.Png;
					break;

				case "bmp":
					format = ImageFormat.Bmp;
					break;

				case "jpeg":
				case "jpg":
					format = ImageFormat.Jpeg;
					break;

				case "gif":
					format = ImageFormat.Gif;
					break;

				case "tif":
				case "tiff":
					format = ImageFormat.Tiff;
					break;
			}

			try
			{
				image.Save(file.FullName, format);
				return file;
			}
			catch
			{
				return null;
			}
		}
	}
}
