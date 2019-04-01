/*****************************************************************
 * UNVELL VisualTouch
 * 
 * COPYRIGHT(C) 2011 UNVELL ALL RIGHTS RESERVED
 * Copyright (c) 2019 Jingwood, all rights reserved.
 * 
 *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

using unvell.JustCapture.XML;
using unvell.JustCapture.Toolkits;
using unvell.Common;

namespace unvell.JustCapture
{
	internal class ConfigurationManager
	{
		public static ConfigurationManager Instance { get; } = new ConfigurationManager();

		internal string GetUserConfigFileName()
		{
			return Path.Combine(Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				"unvell\\" + Application.ProductName), "user-settings.xml");
		}

		private Configuration currentUserConfig;

		internal Configuration GetCurrentUserConfiguration()
		{
			if (currentUserConfig != null)
			{
				return currentUserConfig;
			}
			else
			{
				currentUserConfig = new Configuration();

				FileInfo file = new FileInfo(GetUserConfigFileName());
				if (!file.Exists)
				{
					currentUserConfig.UserData = new UserData();

					foreach (var action in ShortcutActionManager.Instance.Actions.Values)
					{
						currentUserConfig.AddShortcutAction(action.Id,
							ShortcutKeyToolkit.KeysToString(action.ShortcutKeys), action.Activated);
					}

					Logger.Log("config", "user config not exists, new config created in memory.");
				}
				else
				{
					try
					{
						using (FileStream fs = file.Open(FileMode.Open, 
							FileAccess.Read, FileShare.Read))
						{
							XmlSerializer serializer = new XmlSerializer(typeof(UserData));
							currentUserConfig.UserData = serializer.Deserialize(fs) as UserData;
						}
					}
					catch (Exception ex)
					{
						Logger.Log("config", "load user config failed: " + ex.Message);
					}

					if (currentUserConfig.UserData == null)
					{
						currentUserConfig.UserData = new UserData();
					}
				}
			}

			return currentUserConfig;
		}
		internal string GetCurrentUserSetting(UserConfigKey key, string def)
		{
			return GetCurrentUserSetting(key.ToString(), def);
		}
		internal string GetCurrentUserSetting(string key, string def)
		{
			Configuration config = GetCurrentUserConfiguration();
			if (config == null) return def;
			else
			{
				return (config.GetUserSettings(key, def));
			}
		}
		internal int GetCurrentUserSettingIntValue(UserConfigKey key, int def)
		{
			return GetCurrentUserSettingIntValue(key.ToString(), def);
		}
		internal int GetCurrentUserSettingIntValue(string key, int def)
		{
			int value = def;
			int.TryParse(GetCurrentUserSetting(key, def.ToString()), out value);
			return value;
		}
		public static readonly string SettingValue_On = "yes";
		public static readonly string SettingValue_Off = "no";
		internal bool IsCurrentUserSetting(UserConfigKey key, bool def)
		{
			return IsCurrentUserSetting(key.ToString(), def);
		}
		internal bool IsCurrentUserSetting(string key, bool def)
		{
			return GetCurrentUserSetting(key, def ? SettingValue_On : SettingValue_Off) == SettingValue_On;
		}
		internal void SetCurrentUserSetting(UserConfigKey key, string value)
		{
			SetCurrentUserSetting(key.ToString(), value);
		}
		internal void SetCurrentUserSetting(string key, string value)
		{
			Configuration config = GetCurrentUserConfiguration();
			if (config == null)
			{
				Logger.Log("config", "current user config is null.");
				return;
			}
			else
				config.SetUserSettings(key, value);
		}
		internal void SwitchCurrentUserSetting(UserConfigKey key, bool value)
		{
			SwitchCurrentUserSetting(key.ToString(), value);
		}
		internal void SwitchCurrentUserSetting(string key, bool value)
		{
			SetCurrentUserSetting(key, value ? SettingValue_On : SettingValue_Off);
		}

		internal void SaveConfiguration(Configuration config)
		{
			if (!config.Dirty) return;

			string filename = GetUserConfigFileName();
			string path = Path.GetDirectoryName(filename);
			DirectoryInfo di = new DirectoryInfo(path);
			if (!di.Exists)
			{
				di.Create();
			}

			XmlSerializer serializer = new XmlSerializer(typeof(UserData));
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Create))
				{
					serializer.Serialize(fs, config.UserData);
				}

				Logger.Log("config", "user data saved");
			}
			catch (Exception ex)
			{
				Logger.Log("config", "save user data failed:" + ex.Message);
			}

			config.Dirty = false;
		}

		internal void SaveCurrentUserConfiguration()
		{
			SaveConfiguration(GetCurrentUserConfiguration());
		}
	}

	// CF  = Capture Form
	// IEF = Image Edit Form
	internal enum UserConfigKey
	{
		Language,
		RecentFiles,

		User_EnableCheckForUpdates,
		User_LastCheckedVersion,
		//User_HasShowIntro,   // unused from v1.2
		User_Language,
		User_RestoreLastSelectRegion,
		User_StartOperation,

		CF_Toolstrip,
		CF_Selection_OuterColor,
		CF_Selection_InnerColor,
		CF_Selection_ThumbColor,
		CF_Show_Coordinate_Info,// added from 1.4
		CF_Magnifier_Scale,			// added from 1.4

		IEF_RectColor,
		IEF_LineColor,
		IEF_ArrowColor,
		IEF_EllipseColor,
		IEF_RoundedRect,
		IEF_NumberColor,
		IEF_LockAspect,
		IEF_ScaleSizeUnit,
		IEF_Toolbar_Visible,
		IEF_StatusBar_Visible,

		Tips_SystemTray,
		Tips_Hotkey,
		Tips_SelectWindow,
		Tips_ChangeNumber,
	}

	/// <summary>
	/// Configuration file to store user settings
	/// </summary>
	internal class Configuration
	{
		private UserData userData;

		public UserData UserData
		{
			get { return userData; }
			set { userData = value; }
		}

		public bool Dirty { get; set; }

		internal string GetUserSettings(UserConfigKey key, string def)
		{
			return GetUserSettings(key.ToString(), def);
		}

		internal string GetUserSettings(string key, string def)
		{
			foreach (UserSetting setting in userData.userSettings)
			{
				if (string.Compare(setting.name, key, true) == 0)
				{
					return setting.value;
				}
			}
			return def;
		}

		internal void SetUserSettings(string key, string value)
		{
			foreach (UserSetting setting in userData.userSettings)
			{
				if (string.Compare(setting.name, key, true) == 0)
				{
					setting.value = value;
					return;
				}
			}

			userData.userSettings.Add(new UserSetting()
			{
				name = key,
				value = value,
			});

			Dirty = true;
		}

		public void AddRecentFile(string filename)
		{
			try
			{
				if (userData.recentFiles.Contains(filename))
					userData.recentFiles.Remove(filename);

				userData.recentFiles.Insert(0, filename);

				int recentFiles = 10;
				int.TryParse(GetUserSettings(UserConfigKey.RecentFiles, "10"), out recentFiles);
				while (userData.recentFiles.Count > recentFiles)
					userData.recentFiles.RemoveAt(userData.recentFiles.Count - 1);
			}
			catch (Exception)
			{
				//Logger.Log("config", "add recent file failed: " + ex.Message);
			}

			Dirty = true;
		}

		public void AddShortcutAction(int id, string shortcutKeys, bool activated)
		{
			foreach (ActionInfo ai in userData.actions)
			{
				if (ai.id == id)
				{
					ai.shortcutKeys = shortcutKeys;
					ai.activated = activated;
					return;
				}
			}

			userData.actions.Add(new ActionInfo()
			{
				id = id,
				activated = activated,
				shortcutKeys = shortcutKeys,
			});

			Dirty = true;
		}

		public ActionInfo GetAction(int id)
		{
			if (userData.actions == null) return null;
			foreach (ActionInfo ai in userData.actions)
			{
				if (ai.id == id) return ai;
			}
			return null;
		}
	}

	namespace XML
	{
		[XmlRoot("user-data")]
		public class UserData
		{
			[XmlArray("settings"), XmlArrayItem("user-setting")]
			public List<UserSetting> userSettings = new List<UserSetting>();

			[XmlArray("recent-files"), XmlArrayItem("file")]
			public List<string> recentFiles = new List<string>();

			[XmlArray("actions"), XmlArrayItem("action")]
			public List<ActionInfo> actions = new List<ActionInfo>();
		}

		public class UserSetting
		{
			[XmlAttribute]
			public string name;
			[XmlAttribute]
			public string value;
		}

		public class RecentFile
		{
			[XmlAttribute]
			public string path;

			public RecentFile() { }

			public RecentFile(string path)
			{
				this.path = path;
			}
		}

		public class ActionInfo
		{
			[XmlAttribute("id")]
			public int id;
			[XmlAttribute("shortcut")]
			public string shortcutKeys;
			[XmlAttribute("activated")]
			public bool activated;
		}
	}
}
