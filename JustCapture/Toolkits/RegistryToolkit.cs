using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Security;

namespace unvell.JustCapture.Toolkits
{
	internal sealed class RegistryToolkit
	{
		public static bool HavePermissionsOnKey(RegistryPermissionAccess accessLevel, string key)
		{
			try
			{
				RegistryPermission r = new RegistryPermission(accessLevel, key);
				r.Demand();
				return true;
			}
			catch (SecurityException)
			{
				return false;
			}
		}

		public static bool CanWriteKey(string key)
		{
			try
			{
				RegistryPermission r = new RegistryPermission(RegistryPermissionAccess.Write, key);
				r.Demand();
				return true;
			}
			catch (SecurityException)
			{
				return false;
			}
		}

		public static bool CanReadKey(string key)
		{
			try
			{
				RegistryPermission r = new RegistryPermission(RegistryPermissionAccess.Read, key);
				r.Demand();
				return true;
			}
			catch (SecurityException)
			{
				return false;
			}
		}

		public static bool PermissionToWriteSystemStartup()
		{
			return CanWriteKey(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run");
		}

		public static bool SetStartupProcess(string name, string filepath)
		{
			try
			{
				Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", name, 
					string.Format("\"{0}\" /tray", filepath));
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool RemoveStartupProcess(string name)
		{
			try
			{
				Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").DeleteValue(name);
				return true;
			}
			catch {
				return false;
			}
		}

		internal static bool IsStartupProcessExists(string name)
		{
			try
			{
				return Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").GetValue(name) != null;
			}
			catch
			{
				return false;
			}
		}
	}
}
