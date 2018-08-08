using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace unvell.JustCapture
{
	[ProductHomepage("JustCapture")]

	internal class ProductInfo
	{
		private static string homepage;
		public static string Homepage { get { return homepage; } }

		private static string productName;
		public static string ProductName { get { return productName; } }

		private static string version;
		public static string Version { get { return version; } }

		private static string subtitle;
		public static string Subtitle { get { return subtitle; } }

		private static string copyright;
		public static string Copyright { get { return copyright; } }

		private static string maker;
		public static string Maker { get { return maker; } }

		static ProductInfo()
		{
			productName = Application.ProductName;
			version = Application.ProductVersion;

			Type type = typeof(ProductInfo);

			ProductHomepageAttribute[] pathAttrs = type.GetCustomAttributes(
				typeof(ProductHomepageAttribute), false) as ProductHomepageAttribute[];
			homepage = "https://www.unvell.com/products/" + ((pathAttrs == null || pathAttrs.Length == 0) ? string.Empty : pathAttrs[0].PathName);

			AssemblyCopyrightAttribute[] copyrightAttrs =
				type.Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)
				as AssemblyCopyrightAttribute[];
			copyright = (copyrightAttrs == null || copyrightAttrs.Length == 0) ? string.Empty : copyrightAttrs[0].Copyright;

			AssemblyCompanyAttribute[] makerAttrs =
				type.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
				as AssemblyCompanyAttribute[];
			maker = (copyrightAttrs == null || makerAttrs.Length == 0) ? string.Empty : makerAttrs[0].Company;

			AssemblyDescriptionAttribute[] descAttrs =
				type.Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
				as AssemblyDescriptionAttribute[];
			subtitle = (descAttrs == null || descAttrs.Length == 0) ? string.Empty : descAttrs[0].Description;

		}

		public static void OpenProductHomepage()
		{
			Process.Start(Homepage);
		}
	}

	class ProductHomepageAttribute : Attribute
	{
		public string PathName { get; set; }
		public ProductHomepageAttribute(string pathName)
		{
			this.PathName = pathName;
		}
	}
}
