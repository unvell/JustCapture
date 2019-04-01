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
using System.Windows.Forms;
using System.Globalization;

using unvell.Common;
using unvell.JustCapture.Editor;
using unvell.JustCapture.Toolkits;
using System.Threading;

namespace unvell.JustCapture
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//TipsForm.ShowTipSlide(UserConfigKey.Tips_ChangeNumber);

#if !DEBUG
			if (SingleApplication.IsAlreadyRunning())
			{
				MessageBox.Show(string.Format(LangResource.already_running,
					Application.ProductName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
#endif

			//new SettingForm().ShowDialog();

			string lang = ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.User_Language, string.Empty);
			if (lang != string.Empty)
			{
				try
				{
					LangResource.Culture = new CultureInfo(lang);
				}
				catch { }
			}
			
			// check startup setting
			if (RegistryToolkit.IsStartupProcessExists(Application.ProductName))
			{
				// update startup shortcut to current exe file
				RegistryToolkit.SetStartupProcess(Application.ProductName, Application.ExecutablePath);
			}

			int startOperation = ConfigurationManager.Instance.GetCurrentUserSettingIntValue(
				UserConfigKey.User_StartOperation, 0);

			switch (startOperation)
			{
				default:
				case 0: // run in background
					Application.Run(new MainForm(true, args));
					break;

				case 1: // start capture
					Thread.Sleep(200);
					ShortcutActionManager.Instance.DoActionById((int)ActionIds.CaptureRegionOfScreen_Clipboard);
					ConfigurationManager.Instance.SaveCurrentUserConfiguration();
					break;

				case 2: // image editor
					new MainForm(false, args);
					new EditorForm().ShowDialog();
					ConfigurationManager.Instance.SaveCurrentUserConfiguration();
					break;
			}
			//new EditTextForm().ShowDialog();
			//Application.Run(new MainForm(args));

			ResourcePoolManager.Instance.ReleaseAllResources();
		}
	}
}
