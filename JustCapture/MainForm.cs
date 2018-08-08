using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

using unvell.JustCapture.Properties;
using unvell.Common;
using unvell.JustCapture.Toolkits;

namespace unvell.JustCapture
{
	internal partial class MainForm : Form
	{
		private static MainForm instance;
		public static MainForm Instance { get { return instance; } }

		private string[] args;

		public MainForm(bool systemTray, string[] args)
		{
			this.SystemTray = systemTray;
			this.args = args;
			instance = this;

			InitializeComponent();

			InitCulture();

			notifyIcon1.DoubleClick += (s, e) =>
				ShortcutActionManager.Instance.DoActionById((int)ActionIds.CaptureRegionOfScreen_Clipboard);

			captureRegionOfScreenToolStripMenuItem.Click += (s, e) =>
			{
				Thread.Sleep(200);
				ShortcutActionManager.Instance.DoActionById((int)ActionIds.CaptureRegionOfScreen_Clipboard);
			};

			captureWindowToolStripMenuItem.Click += (s, e) =>
			{
				Thread.Sleep(200);
				ShortcutActionManager.Instance.DoActionById((int)ActionIds.CaptureWindow_Clipboard);
			};

			captureFullScreenToolStripMenuItem.Click += (s, e) =>
			{
				Thread.Sleep(200);
				ShortcutActionManager.Instance.DoActionById((int)ActionIds.CaptureFullScreen_Clipboard);
			};

			smartScanToolStripMenuItem.Click += (s, e) =>
			{
				Thread.Sleep(200);
				ShortcutActionManager.Instance.DoActionById((int)ActionIds.CaptureSmartScan_Clipboard);
			};

			saveLastPictureToolStripMenuItem.Click += (s, e) =>
			{
				if (CaptureForm.LastCapturedImage == null)
				{
					MessageBox.Show(LangResource.no_picture_captured);
				}
				else
				{
					ShortcutActionManager.Instance.DoActionById((int)ActionIds.SaveLastPicture);
				}
			};

			printLastCapturedPictureToolStripMenuItem.Click += (s, e) =>
				{
					if (CaptureForm.LastCapturedImage == null)
					{
						MessageBox.Show(LangResource.no_picture_captured);
					}
					else
					{
						ShortcutActionManager.Instance.DoActionById((int)ActionIds.PrintLastPicture);
					}
				};

			imageEditorToolStripMenuItem.Click += (s, e) =>
			{
				ShortcutActionManager.Instance.DoActionById((int)ActionIds.EditLastPicture);
			};

			settingsToolStripMenuItem.Click += (s, e) =>
			{
				// unregister hotkeys
				ShortcutActionManager.UnregisterAllShortcutActions(this.Handle);

				new SettingForm().ShowDialog();

				MainForm.Instance.InitCulture();
				CaptureForm.InitInstanceCulture();

				if (!this.IsDisposed)
				{
					// reregister hotkeys
					ShortcutActionManager.RegisterAllShortcutActions(this.Handle);
				}
			};

			homepageToolStripMenuItem.Click += (s, e) => ProductInfo.OpenProductHomepage();

			aboutToolStripMenuItem.Click += (s, e) =>
			{
				new AboutForm().ShowDialog();
			};

			contextMenuStrip1.Opening += (s, e) =>
			{
				saveLastPictureToolStripMenuItem.Enabled = 
					printLastCapturedPictureToolStripMenuItem.Enabled = 
					CaptureForm.LastCapturedImage != null;
			};

			tipsToolStripMenuItem.Click += (s, e) => TipsForm.ShowTipSlides(StartTipsKey, false);
		}

		private static readonly UserConfigKey[] StartTipsKey = new UserConfigKey[]{
			UserConfigKey.Tips_SystemTray,
			UserConfigKey.Tips_Hotkey,
			UserConfigKey.Tips_SelectWindow};

		internal void InitCulture()
		{
			captureRegionOfScreenToolStripMenuItem.Text = LangResource.btn_capture_free_region;
			captureWindowToolStripMenuItem.Text = LangResource.btn_capture_window;
			captureFullScreenToolStripMenuItem.Text = LangResource.btn_capture_current_full_screen;
			saveLastPictureToolStripMenuItem.Text = LangResource.btn_save_last_picture;
			printLastCapturedPictureToolStripMenuItem.Text = LangResource.btn_print_last_picture;
			imageEditorToolStripMenuItem.Text = LangResource.btn_image_editor;
			settingsToolStripMenuItem.Text = LangResource.btn_settings;
			helpToolStripMenuItem.Text = LangResource.btn_help;
			aboutToolStripMenuItem.Text = LangResource.btn_about;
			homepageToolStripMenuItem.Text = LangResource.btn_homepage;
			exitToolStripMenuItem.Text = LangResource.btn_exit;
			tipsToolStripMenuItem.Text = LangResource.btn_start_tips;
		}

		public static readonly CultureInfo[] SupportedCultures = new CultureInfo[]{
			new CultureInfo("en-US"),
			new CultureInfo("ja-JP"),
			new CultureInfo("zh-CN"),
		};

		internal bool SystemTray { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			// set form invisible
			Visible = false;
			Hide();

			base.OnLoad(e);

			if (SystemTray)
			{
				notifyIcon1.Text = Application.ProductName + " " + Application.ProductVersion;
				notifyIcon1.Visible = true;

				if (args != null && args.Length > 0)
				{
					// check system tray tips
					if (Array.IndexOf(args, "/tray") < 0)
					{
						TipsForm.CheckAndShowTips(StartTipsKey);
					}
				}

				if (IsDisposed) return;

				EnableHotKey();
			}
			else
			{
				notifyIcon1.Visible = false;
			}

			// version check
#if !DEBUG
			if (ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.User_EnableCheckForUpdates, true))
			{
				CheckNewVersion(false);
			}
#endif

		}

		protected override void OnClosing(CancelEventArgs e)
		{
			//ConfigurationManager.Instance.SaveCurrentUserConfiguration();
			
			DisableHotKey();

			try
			{
				CaptureForm.Shutdown();
			}
			catch { }

			base.OnClosing(e);
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case (int)Win32.WMessages.WM_HOTKEY:
					if (CaptureForm.Instance == null || !CaptureForm.Instance.Visible)
					{
						ShortcutActionManager.Instance.DoActionById(m.WParam.ToInt32());
					}
					break;

				case (int)Win32.WMessages.WM_QUERYENDSESSION:
					Close();
					break;
			}

			base.WndProc(ref m);
		}

		public void EnableHotKey()
		{
			ShortcutActionManager.RegisterAllShortcutActions(this.Handle);
		}

		public void DisableHotKey()
		{
			ShortcutActionManager.UnregisterAllShortcutActions(this.Handle);
		}
	
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
