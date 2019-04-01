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
using System.Diagnostics;

using unvell.JustCapture.XML;
using unvell.JustCapture.Editor;
using unvell.JustCapture.Toolkits;
using System.Drawing.Printing;
using System.Drawing;

namespace unvell.JustCapture
{
	internal class ShortcutActionManager
	{
		private static readonly ShortcutActionManager instance = new ShortcutActionManager();
		public static ShortcutActionManager Instance { get { return instance; } }

		private Dictionary<int, ShortcutAction> actions = new Dictionary<int, ShortcutAction>();

		internal Dictionary<int, ShortcutAction> Actions
		{
			get { return actions; }
			set { actions = value; }
		}
		
		private ShortcutActionManager()
		{
			actions.Add((int)ActionIds.CaptureRegionOfScreen_Clipboard, new FreeRegionCaptureClipboardAction()
			{
				Id = (int)ActionIds.CaptureRegionOfScreen_Clipboard,
				ShortcutKeys = Keys.Control | Keys.D1,
				Activated = true,
			});

			actions.Add((int)ActionIds.CaptureWindow_Clipboard, new SelectWindowCaptureClipboardAction()
			{
				Id = (int)ActionIds.CaptureWindow_Clipboard,
				ShortcutKeys = Keys.Control | Keys.D2,
				Activated = true,
			});

			actions.Add((int)ActionIds.CaptureRegionOfScreen_Save, new FreeRegionCaptureSaveAction()
			{
				Id = (int)ActionIds.CaptureRegionOfScreen_Save,
				ShortcutKeys = Keys.Control | Keys.Shift | Keys.D3,
				Activated = false,
			});

			actions.Add((int)ActionIds.CaptureRegionOfScreen_Edit, new FreeRegionCaptureEditAction()
			{
				Id = (int)ActionIds.CaptureRegionOfScreen_Edit,
				ShortcutKeys = Keys.Control | Keys.Shift | Keys.D4,
				Activated = false,
			});

			actions.Add((int)ActionIds.CaptureFullScreen_Clipboard, new FullScreenCaptureClipboardAction()
			{
				Id = (int)ActionIds.CaptureFullScreen_Clipboard,
				ShortcutKeys = Keys.Control | Keys.Shift | Keys.D5,
				Activated = false,
			});

			actions.Add((int)ActionIds.SaveLastPicture, new SaveLastPictureAction()
			{
				Id = (int)ActionIds.SaveLastPicture,
				ShortcutKeys = Keys.None,
				HideInList = true,
				Activated = false,
			});

			actions.Add((int)ActionIds.EditLastPicture, new EditLastPictureAction()
			{
				Id = (int)ActionIds.EditLastPicture,
				ShortcutKeys = Keys.Control | Keys.D3,
				Activated = true,
			});

			actions.Add((int)ActionIds.MailLastPicture, new MailLastPictureAction()
			{
				Id = (int)ActionIds.MailLastPicture,
				ShortcutKeys = Keys.None,
				HideInList = true,
				Activated = false,
			});

			actions.Add((int)ActionIds.PrintLastPicture, new PrintLastPictureAction()
			{
				Id = (int)ActionIds.PrintLastPicture,
				ShortcutKeys = Keys.None,
				HideInList = true,
				Activated = false,
			});

			actions.Add((int)ActionIds.CaptureSmartScan_Clipboard, new SmartScanCaptureToClipboardAction()
			{
				Id = (int)ActionIds.CaptureSmartScan_Clipboard,
				ShortcutKeys = Keys.None,
				HideInList = true,
				Activated = false,
			});
		}

		public bool DoActionById(int id)
		{
			ShortcutAction sa;
			if (actions.TryGetValue(id, out sa))
			{
				sa.DoAction();
				return true;
			}
			else
				return false;
		}

		public void ChangeAction(int id, bool activated, Keys shortcutKeys)
		{
			if (actions.ContainsKey(id))
			{
				actions[id].Activated = activated;
				actions[id].ShortcutKeys = shortcutKeys;
			}
		}

		public void SaveConfig()
		{
			Configuration config = ConfigurationManager.Instance.GetCurrentUserConfiguration();
			config.UserData.actions.Clear();

			foreach (ShortcutAction sa in actions.Values)
			{
				config.UserData.actions.Add(new unvell.JustCapture.XML.ActionInfo
				{
					id = sa.Id,
					activated = sa.Activated,
					shortcutKeys = ShortcutKeyToolkit.KeysToString(sa.ShortcutKeys),
				});
			}

			config.Dirty = true;

			ConfigurationManager.Instance.SaveConfiguration(config);
		}

		public static void RegisterAllShortcutActions(IntPtr hwnd)
		{
			foreach (ActionInfo ai in ConfigurationManager.Instance.GetCurrentUserConfiguration().UserData.actions)
			{
				if (ai.activated)
				{
					ShortcutKeyToolkit.RegisterShortcutKey(hwnd, ai.id,
						ShortcutKeyToolkit.StringToKeys(ai.shortcutKeys));
				}
			}
		}

		public static void UnregisterAllShortcutActions(IntPtr hwnd)
		{
			foreach (int id in ShortcutActionManager.Instance.Actions.Keys)
			{
				ShortcutKeyToolkit.UnregisterShortcutKey(hwnd, id);
			}
		}
	}

	internal enum ActionIds : int
	{
		CaptureRegionOfScreen_Clipboard = 1,
		CaptureWindow_Clipboard = 2,
		CaptureRegionOfScreen_Save = 3,
		CaptureRegionOfScreen_Edit = 4,
		CaptureFullScreen_Clipboard = 5,
		CaptureSmartScan_Clipboard = 6,
		SaveLastPicture = 51,
		EditLastPicture = 52,
		MailLastPicture = 55,
		PrintLastPicture = 56,
	}

	abstract class ShortcutAction
	{
		public ShortcutAction()
		{
		}

		public int Id { get; set; }

		public Keys ShortcutKeys { get; set; }

		public abstract string Name { get; }

		public bool Activated { get;set;}

		public bool HideInList { get; set; }

		public abstract void DoAction();
	}

	class FreeRegionCaptureClipboardAction : ShortcutAction
	{
		public override void DoAction()
		{
			CaptureForm.StartCapture(WorkMode.General, (image) =>
			{
				if (image != null)
				{
					Clipboard.SetImage(image);
				}
			});
		}

		public override string Name
		{
			get { return LangResource.action_free_region; }
		}
	}

	class SelectWindowCaptureClipboardAction : ShortcutAction
	{
		public override void DoAction()
		{
			CaptureForm.StartCapture(WorkMode.General, CaptureMode.SelectWindow, (image) =>
			{
				if (image != null)
				{
					Clipboard.SetImage(image);
				}
			});
		}

		public override string Name
		{
			get { return LangResource.action_select_window; }
		}
	}

	class SmartScanCaptureToClipboardAction : ShortcutAction
	{
		public override void DoAction()
		{
			CaptureForm.StartCapture(WorkMode.General, CaptureMode.SmartScan, (image) =>
			{
				if (image != null)
				{
					Clipboard.SetImage(image);
				}
			});
		}

		public override string Name
		{
			get { return LangResource.action_select_window; }
		}
	}

	class FullScreenCaptureClipboardAction : ShortcutAction
	{
		public override void DoAction()
		{
			CaptureForm.StartCapture(WorkMode.AutoCapture, CaptureMode.FullScreen, (image) =>
			{
				if (image != null)
				{
					Clipboard.SetImage(image);
				}
			});
		}

		public override string Name
		{
			get { return LangResource.action_capture_full_screen_into_clipboard; }
		}
	}

	class FreeRegionCaptureSaveAction : ShortcutAction
	{
		private static readonly SaveLastPictureAction saveAction = new SaveLastPictureAction();

		public override void DoAction()
		{
			CaptureForm.StartCapture(WorkMode.OnlyCapture, (image) =>
			{
				if (image != null)
				{
					saveAction.DoAction();
				}
			});
		}

		public override string Name
		{
			get { return LangResource.action_capture_and_save_free_region; }
		}
	}

	class FreeRegionCaptureEditAction : ShortcutAction
	{
		private static readonly EditLastPictureAction editAction = new EditLastPictureAction();

		public override void DoAction()
		{
			CaptureForm.StartCapture(WorkMode.OnlyCapture, (image) =>
			{
				if (image != null)
				{
					editAction.DoAction();
				}
			});
		}

		public override string Name
		{
			get { return LangResource.action_capture_and_edit_free_region; }
		}
	}

	class SaveLastPictureAction : ShortcutAction
	{
		public override void DoAction()
		{
			if (CaptureForm.LastCapturedImage != null)
			{
				FileToolkit.SaveImage(CaptureForm.LastCapturedImage, LangResource.captured_image + " (" +
					DateTime.Now.ToString("yyyyMMdd HHmmss") + ")");
			}
		}

		public override string Name
		{
			get { return LangResource.action_save_last_captured_picture; }
		}
	}

	class EditLastPictureAction : ShortcutAction
	{
		public override void DoAction()
		{
			EditorForm ef = new EditorForm();

			if (CaptureForm.LastCapturedImage != null)
			{
				ef.Image = CaptureForm.LastCapturedImage;
			}

			// if not in daemon mode, start edit form with getting a block from main thread
			if (MainForm.Instance == null || !MainForm.Instance.SystemTray)
			{
				ef.ShowDialog();
			}
			else
			{
				ef.Show();
			}
		}

		public override string Name
		{
			get { return LangResource.action_image_editor; }
		}
	}

	class MailLastPictureAction : ShortcutAction
	{
		public override void DoAction()
		{
			if (CaptureForm.LastCapturedImage != null)
			{
				string file = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

				try
				{
					CaptureForm.LastCapturedImage.Save(file, ImageFormat.Png);
				}
				catch { }
				Process.Start("mailto://?attachment=" + file);
			}
		}

		public override string Name
		{
			get { return LangResource.action_mail_last_captured_picture; }
		}
	}

	class PrintLastPictureAction : ShortcutAction
	{
		public override void DoAction()
		{
			var image = CaptureForm.LastCapturedImage;

			if (image != null)
			{
				var doc = new PrintDocument();

				SizeF imageSize = image.Size;

				SizeF paperSize = doc.DefaultPageSettings.PrintableArea.Size;
				paperSize.Width -= doc.DefaultPageSettings.Margins.Left + doc.DefaultPageSettings.Margins.Right;
				paperSize.Height -= doc.DefaultPageSettings.Margins.Top + doc.DefaultPageSettings.Margins.Bottom;

				if (imageSize.Width > paperSize.Width)
				{
					//imageSize = new SizeF(imageSize.Height, imageSize.Width);

					doc.DefaultPageSettings.Landscape = true;
				}

				float imageMax = Math.Max(imageSize.Width, imageSize.Height);

				doc.PrintPage += (s, e) =>
					{
						int pageMax = Math.Max(e.MarginBounds.Width, e.MarginBounds.Height);

						if (imageMax > pageMax)
						{
							float scale = pageMax / imageMax;
							imageSize = new SizeF(imageSize.Width * scale, imageSize.Height * scale);
						}

						DrawImageToGraphics(e.Graphics, image, imageSize, e.MarginBounds);

						e.HasMorePages = false;
					};

				doc.Print();
			}
		}

		private static void DrawImageToGraphics(Graphics g, Image image, SizeF imageSize, RectangleF pageRect)
		{
			float x = (pageRect.Width - imageSize.Width) / 2;
			float y = (pageRect.Height - imageSize.Height) / 2;

			g.DrawImage(image, pageRect.X + x, pageRect.Y + y, imageSize.Width, imageSize.Height);
		}

		public override string Name
		{
			get { return LangResource.print; }
		}
	}

}
