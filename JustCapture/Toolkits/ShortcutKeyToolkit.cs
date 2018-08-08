using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using unvell.Common;

namespace unvell.JustCapture.Toolkits
{
	internal sealed class ShortcutKeyToolkit
	{
		private static readonly KeysConverter kc = new KeysConverter();

		public static string KeysToString(Keys keys)
		{
			string text = kc.ConvertToString(keys);
			text = text.Replace("+", " + ");
			return text;
		}

		public static Keys StringToKeys(string keys)
		{
			return (Keys)kc.ConvertFromString(keys.Replace(" + ", "+"));
		}

		public static Enum[] ConvertToEnumArray(Keys keys)
		{
			return (Enum[])kc.ConvertTo(keys, typeof(Enum[]));
		}

		public static void RegisterShortcutKey(IntPtr hwnd, int id, Keys shortcutKeys)
		{
			Keys keys = shortcutKeys;

			uint mods = 0;

			if ((keys & Keys.Control) == Keys.Control)
			{
				mods |= (uint)Win32.Modifiers.MOD_CONTROL;
				keys &= ~Keys.Control;
			}
			if ((keys & Keys.Shift) == Keys.Shift)
			{
				mods |= (uint)Win32.Modifiers.MOD_SHIFT;
				keys &= ~Keys.Shift;
			}
			if ((keys & Keys.Alt) == Keys.Alt)
			{
				mods |= (uint)Win32.Modifiers.MOD_ALT;
				keys &= ~Keys.Alt;
			}

			// register hotkey for screen capture
			Win32.RegisterHotKey(hwnd, id, mods, keys);
		}

		public static void UnregisterShortcutKey(IntPtr hwnd, int id)
		{
			Win32.UnregisterHotKey(hwnd, id);
		}
	}
}
