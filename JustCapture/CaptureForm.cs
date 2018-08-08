/*****************************************************************************
 * Main Capture Form
 * 
 *   - capture a region of screen
 * 
 * Copyright © 2012 UNVELL All rights reserved
 *
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using unvell.Common;
using unvell.JustCapture.Toolkits;

namespace unvell.JustCapture
{
	internal partial class CaptureForm : Form
	{
		#region Constructor & Attributes

		public CaptureForm()
		{
			InitializeComponent();

			this.AutoSize = false;
			this.Cursor = Cursors.Cross;
			this.DoubleBuffered = true;

			toolbarPanel.Visible = false;
			this.Controls.Add(this.infoLabel);

			InitCulture();

			using (var ms = new System.IO.MemoryStream(Properties.Resources.SmartScan))
			{
				this.smartCursor = new Cursor(ms);
			}

			toolStrip.MouseMove += (s, e) => toolStrip.Cursor = Cursors.Default;
			toolbarPanel.MouseMove += (s, e) => toolbarPanel.Cursor = Cursors.Default;

			fullScreenToolStripButton.Click += (s, e) => SelectFullScreen();
			selectWindowToolStripButton.Click += (s, e) => StartSelectWindow();
			autoScanToolStripButton.Click += (s, e) => StartAutoScan();
			copyToolStripButton.Click += (s, e) => CaptureAndCopyImage();
			editToolStripButton.Click += (s, e) => CaptureAndEditImage();
			saveToolStripButton.Click += (s, e) => CaptureAndSaveImage();
			printToolStripButton.Click += (s, e) => CaptureAndPrintImage();
			cancelToolStripButton.Click += (s, e) => EndCapture();
			resetRegionToolStripButton.Click += (s, e) => ResetSelection();

			fullScreenToolStripMenuItem.Click += (s, e) => SelectFullScreen();
			selectWindowToolStripMenuItem.Click += (s, e) => StartSelectWindow();
			autoScanToolStripMenuItem.Click += (s, e) => StartAutoScan();
			copyToolStripMenuItem.Click += (s, e) => CaptureAndCopyImage();
			editToolStripMenuItem.Click += (s, e) => CaptureAndEditImage();
			saveToolStripMenuItem.Click += (s, e) => CaptureAndSaveImage();
			printToolStripMenuItem.Click += (s, e) => CaptureAndPrintImage();
			cancelToolStripMenuItem.Click += (s, e) => EndCapture();
			resetRegionToolStripMenuItem.Click += (s, e) => ResetSelection();

			closeImage.MouseDown += (s, e) => HideToolbar();

			contextMenuStrip1.Opening += (s, e) =>
			{
				showToolbarToolStripMenuItem.Checked =
					ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.CF_Toolstrip, true);
			};

			showToolbarToolStripMenuItem.CheckedChanged += (s, e) =>
			{
				if (showToolbarToolStripMenuItem.Checked)
				{
					ShowToolbar();
				}
				else
				{
					HideToolbar();
				}
			};

			captureToolStripButton.Click += (s, e) => DoCaptureScreen();

			settingsToolStripMenuItem.Click += (s, e) =>
			{
				// unregister hotkeys
				ShortcutActionManager.UnregisterAllShortcutActions(this.Handle);

				using (var settingsForm = new SettingForm())
				{
					settingsForm.TopMost = true;

					if (settingsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						ApplySettings();
					}
				};

				if (!this.IsDisposed)
				{
					// reregister hotkeys
					ShortcutActionManager.RegisterAllShortcutActions(this.Handle);
				}
			};
		}

		private void InitCulture()
		{
			fullScreenToolStripButton.Text = LangResource.btn_full_screen;
			selectWindowToolStripButton.Text = LangResource.btn_select_window;
			resetRegionToolStripButton.Text = LangResource.btn_reset_region;
			copyToolStripButton.Text = LangResource.btn_capture_to_clipboard;
			saveToolStripButton.Text = LangResource.btn_save;
			editToolStripButton.Text = LangResource.btn_edit;
			printToolStripButton.Text = LangResource.btn_print;
			captureToolStripButton.Text = LangResource.capture;
			cancelToolStripButton.Text = LangResource.btn_cancel;

			fullScreenToolStripMenuItem.Text = LangResource.btn_full_screen;
			selectWindowToolStripMenuItem.Text = LangResource.btn_select_window;
			resetRegionToolStripMenuItem.Text = LangResource.btn_reset_region;
			copyToolStripMenuItem.Text = LangResource.btn_copy;
			saveToolStripMenuItem.Text = LangResource.btn_save;
			editToolStripMenuItem.Text = LangResource.btn_edit;
			printToolStripMenuItem.Text = LangResource.btn_print;
			captureToolStripMenuItem.Text = LangResource.btn_capture;
			cancelToolStripMenuItem.Text = LangResource.btn_cancel;

			showToolbarToolStripMenuItem.Text = LangResource.btn_show_toolbar;
			settingsToolStripMenuItem.Text = LangResource.btn_settings;
			settingsToolStripMenuItem.ToolTipText = LangResource.settings;
		}

		internal static void InitInstanceCulture()
		{
			if (captureForm != null)
			{
				captureForm.InitCulture();
			}
		}

		private Color outerColor;
		private Color innerColor;
		private Color thumbColor;

		public WorkMode WorkMode { get; set; }
		public CaptureMode InitCaptureMethod { get; set; }

		public void SelectFullScreen()
		{
			Selection = new Rectangle(0, 0, Width, Height);
			operation = Operation.RangeSelection;
			this.infoLabel.Visible = false;

			Cursor = Cursors.SizeAll;
			Invalidate();
		}

		public void StartSelectWindow()
		{
			Selection = Rectangle.Empty;
			operation = Operation.FindWindow;
			this.infoLabel.Visible = false;

			lastHoverHwnd = IntPtr.Zero;
			SelectedWindow = new WindowInfo();

			toolbarPanel.Visible = false;

			Cursor = Cursors.Default;
			Invalidate();
		}

		private Cursor smartCursor;

		public void StartAutoScan()
		{
			Selection = Rectangle.Empty;
			operation = Operation.AutoScanRange;

			this.Cursor = this.smartCursor;
			toolbarPanel.Visible = false;
			this.infoLabel.Visible = false;

			Cursor = Cursors.Default;
			Invalidate();
		}

		public void ResetSelection()
		{
			Selection = Rectangle.Empty;
			operation = Operation.RangeSelection;

			toolbarPanel.Visible = false;

			Cursor = Cursors.Cross;
			Invalidate();
		}

		public void CaptureAndCopyImage()
		{
			if (MainForm.Instance == null || !MainForm.Instance.SystemTray) allProcessFinished = true;
			DoCaptureScreen();
			if (CapturedImage != null) Clipboard.SetImage(CapturedImage);
		}

		public void CaptureAndEditImage()
		{
			if (MainForm.Instance == null || !MainForm.Instance.SystemTray) allProcessFinished = false;
			DoCaptureScreen();
			ShortcutActionManager.Instance.DoActionById((int)ActionIds.EditLastPicture);
		}

		public void CaptureAndSaveImage()
		{
			if (MainForm.Instance == null || !MainForm.Instance.SystemTray) allProcessFinished = false;
			DoCaptureScreen();
			ShortcutActionManager.Instance.DoActionById((int)ActionIds.SaveLastPicture);
		}

		public void CaptureAndPrintImage()
		{
			if (MainForm.Instance == null || !MainForm.Instance.SystemTray) allProcessFinished = false;
			DoCaptureScreen();
			ShortcutActionManager.Instance.DoActionById((int)ActionIds.PrintLastPicture);
		}

		/// <summary>
		/// Show toolbar
		/// </summary>
		public void ShowToolbar()
		{
			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.CF_Toolstrip, true);
			MoveToolstrip();
			toolbarPanel.Visible = true;
		}

		/// <summary>
		/// Hide toolbar
		/// </summary>
		public void HideToolbar()
		{
			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.CF_Toolstrip, false);
			toolbarPanel.Visible = false;
		}

		/// <summary>
		/// update buttons status of menu and toolbar
		/// </summary>
		private void UpdateMenus()
		{
			copyToolStripButton.Enabled =
				copyToolStripMenuItem.Enabled =
				editToolStripButton.Enabled =
				editToolStripMenuItem.Enabled =
				saveToolStripButton.Enabled =
				saveToolStripMenuItem.Enabled =
				selection.Width > 0 && selection.Height > 0;
		}

		private Action<Image> onCaptured;

		/// <summary>
		/// Action will be invoked when screen captured
		/// </summary>
		internal Action<Image> OnCaptured
		{
			get { return onCaptured; }
			set { onCaptured = value; }
		}

		/// <summary>
		/// Last captured image
		/// </summary>
		public Image CapturedImage { get; set; }

		#endregion

		#region Capture Routing
		private static CaptureForm captureForm;

		private bool allProcessFinished = false;

		/// <summary>
		/// End current capture operation
		/// </summary>
		public void EndCapture()
		{
			Hide();
			TopMost = false;

			if (OnCaptured != null) OnCaptured(CapturedImage);

			if (this.magnifierBitmap != null)
			{
				this.magnifierBitmap.Dispose();
				this.magnifierBitmap = null;
			}

			if (allProcessFinished)
			{
				// once capture
				captureForm.Close();
			}
		}

		/// <summary>
		/// Last captured image
		/// </summary>
		public static Image LastCapturedImage
		{
			get
			{
				if (captureForm == null)
					return null;
				else
					return captureForm.CapturedImage;
			}
		}

		/// <summary>
		/// Start capture
		/// </summary>
		/// <param name="captureHandler">action will be invoked when image captured</param>
		public static void StartCapture(Action<Image> captureHandler)
		{
			StartCapture(WorkMode.General, CaptureMode.FreeRegion, captureHandler);
		}

		/// <summary>
		/// Start capture
		/// </summary>
		/// <param name="mode">capture mode for current action</param>
		/// <param name="captureHandler">action will be invoked when image captured</param>
		public static void StartCapture(WorkMode mode, Action<Image> captureHandler)
		{
			StartCapture(mode, CaptureMode.FreeRegion, captureHandler);
		}

		/// <summary>
		/// Start a capture session
		/// </summary>
		/// 
		/// <param name="mode">Capture mode specifies what features can be used by user in this session.
		/// If OnlyCapture is specified, user will not able to use the 'save' or 'edit' button.
		/// If captured image will be used after capture is end, the OnlyCapture should be speicified.</param>
		/// 
		/// <param name="method">Capture method that can be specified to select a region or window to capture</param>
		/// 
		/// <param name="captureHandler">A callback method is executed when capture is finished. 
		/// If capture operation is cancelled by user, the parameter Image is null.</param>
		public static void StartCapture(WorkMode mode, CaptureMode method, Action<Image> captureHandler)
		{
			if (captureForm == null)
			{
				captureForm = new CaptureForm();
			}

			if (captureForm == null) return;

			captureForm.OnCaptured = captureHandler;
			captureForm.WorkMode = mode;
			captureForm.InitCaptureMethod = method;
			captureForm.PrepareCaptureForm();

			if (MainForm.Instance == null || !MainForm.Instance.SystemTray)
			{
				Application.Run(captureForm);
			}
		}

		#endregion

		#region Capture Window

		/// <summary>
		/// List to save all foreground windows which visible to user on current screen
		/// </summary>
		private List<WindowInfo> allWindowBounds = new List<WindowInfo>();

		/// <summary>
		/// Hover info when user moves cursor into a window
		/// </summary>
		private WindowInfo HoverWindow { get; set; }

		/// <summary>
		/// Hover info of last window which user selected
		/// </summary>
		private IntPtr lastHoverHwnd;

		/// <summary>
		/// Selected window by cursor
		/// </summary>
		private WindowInfo SelectedWindow { get; set; }

		/// <summary>
		/// Initialize capture form 
		/// get current actived screen and copy whole screen as background image of capture-window
		/// set the init operation by given capture mode (select range or window)
		/// </summary>
		private void PrepareCaptureForm()
		{
			if (WorkMode == WorkMode.General)
			{
				toolbarSpliterToolStripSeparator.Visible =
				menuSpliterToolStripMenuItem.Visible =
				copyToolStripButton.Visible =
				copyToolStripMenuItem.Visible =
				editToolStripButton.Visible =
				editToolStripMenuItem.Visible =
				saveToolStripButton.Visible =
				saveToolStripMenuItem.Visible = true;

				captureToolStripMenuItem.Visible =
					captureToolStripButton.Visible = false;
			}
			else
			{
				toolbarSpliterToolStripSeparator.Visible =
				menuSpliterToolStripMenuItem.Visible =
				copyToolStripButton.Visible =
				copyToolStripMenuItem.Visible =
				editToolStripButton.Visible =
				editToolStripMenuItem.Visible =
				saveToolStripButton.Visible =
				saveToolStripMenuItem.Visible = false;

				captureToolStripMenuItem.Visible =
					captureToolStripButton.Visible = true;
			}

			// get screen
			Point p = new Point();
			Win32.GetCursorPos(ref p);
			Screen currentScreen = Screen.FromPoint(p);

			FetchWindows(currentScreen);

			startX = null;
			startY = null;
			endX = 0;
			endY = 0;

			// capture current screen
			Rectangle rect = currentScreen.Bounds;
			var screenBitmap = new Bitmap(rect.Width, rect.Height);
			using (Graphics g = Graphics.FromImage(screenBitmap))
			{
				g.CopyFromScreen(currentScreen.Bounds.Location, new Point(0, 0), currentScreen.Bounds.Size);
			}

			// dispose background image which used in last time
			if (this.BackgroundImage != null)
			{
				try
				{
					this.BackgroundImage.Dispose();
				}
				catch { }
			}

			this.BackgroundImage = new Bitmap(screenBitmap);

			ApplySettings();

			SetBounds(rect.Left, rect.Top, rect.Width, rect.Height);

			TopMost = true;
			WindowState = FormWindowState.Normal;
			Show();
			Focus();

			if (InitCaptureMethod == CaptureMode.FreeRegion)
			{
				operation = Operation.RangeSelection;

				// if user do not want to remember last region, we hide the selection
				if (!ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.User_RestoreLastSelectRegion, true)

					// or do not remember the last selection what capture full screen,
					// full screen selection can not select a range so user may worries about this
					|| (Selection == currentScreen.Bounds))
				{
					// clear selection
					selection = Rectangle.Empty;

					// hide toolbar 
					toolbarPanel.Visible = false;
				}
			}
			else if (InitCaptureMethod == CaptureMode.SelectWindow)
			{
				StartSelectWindow();
			}
			else if (InitCaptureMethod == CaptureMode.GetScrollContent)
			{
				StartSelectWindow();
				operation = Operation.FindControl;
			}

			if (WorkMode == WorkMode.AutoCapture)
			{
				Selection = currentScreen.Bounds;

				// hide toolbar
				toolbarPanel.Visible = false;

				BackgroundWorker bg = new BackgroundWorker();

				bg.DoWork += (s, e) =>
				{
					Thread.Sleep(300);

					flashBrightness = 255;
					while (flashBrightness > 0)
					{
						flashBrightness -= 20;
						Invalidate();
						Application.DoEvents();
						Thread.Sleep(50);
					}
					flashBrightness = 0;

					Thread.Sleep(100);

					bg.Dispose();

					this.Invoke((MethodInvoker)(() => { DoCaptureScreen(); }));
				};

				bg.RunWorkerAsync();
			}
		}

		private void ApplySettings()
		{
			// read capture tool colors

			this.outerColor = FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(
						UserConfigKey.CF_Selection_OuterColor, Color.Black.Name));
			this.innerColor = FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(
						UserConfigKey.CF_Selection_InnerColor, Color.Gold.Name));
			this.thumbColor = FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(
						UserConfigKey.CF_Selection_ThumbColor, Color.SkyBlue.Name));

			// update magnifier bitmap

			var magnifierSetting = ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.CF_Magnifier_Scale, "4");
			float.TryParse(magnifierSetting, out magnifierScale);

			this.AllowMagnifier = magnifierScale > 0;

			if (this.AllowMagnifier)
			{
				if (this.magnifierBitmap != null)
				{
					try
					{
						this.magnifierBitmap.Dispose();
						this.magnifierBitmap = null;
					}
					catch { }
				}

				Rectangle rect = Screen.FromPoint(Cursor.Position).Bounds;

				this.magnifierRect.Width = (int)Math.Round(rect.Width * 0.2f);
				this.magnifierRect.Height = (int)Math.Round(rect.Height * 0.2f);

				this.magnifierBitmap = new Bitmap(this.magnifierRect.Width, this.magnifierRect.Height);
			}
		}

		private int flashBrightness = 0;

		private void FetchWindows(Screen currentScreen)
		{
			Rectangle screenRect = currentScreen.Bounds;

			// get foreground windows
			allWindowBounds.Clear();

			Win32.EnumDesktopWindows(IntPtr.Zero, (hwnd, lparam) =>
			{
				if (Win32.IsWindowVisible(hwnd))
				{
					Rectangle windowBounds = new Rectangle();

					// get standard window rect
					Win32.GetWindowRect(hwnd, ref windowBounds);

					// transform to managed Rectangle type
					windowBounds.Width -= windowBounds.X;
					windowBounds.Height -= windowBounds.Y;

					// do hit test with centre of window to check whether window is visible by user
					Point hitTestPoint = new Point(windowBounds.X + windowBounds.Width / 2,
						windowBounds.Y + windowBounds.Height / 2);

					// get window 
					IntPtr hitTestWindow = Win32.WindowFromPoint(hitTestPoint);

					// get native window
					IntPtr rootWindow = Win32.GetAncestor(hitTestWindow, (uint)Win32.GAFlag.GA_ROOT);

					if (rootWindow == hwnd)
					{
						// get window bounds
						Rectangle winRect = GetWindowRect(hwnd);

						// if window visible in current active screen
						// add this window into selection candidate list
						if (screenRect.Left <= winRect.Left && screenRect.Top <= winRect.Top
							&& screenRect.Right >= winRect.Right && screenRect.Bottom >= winRect.Bottom)
						{
							// move window rect to put it into screen bounds range
							winRect.Offset(-currentScreen.Bounds.Left, -currentScreen.Bounds.Top);

							allWindowBounds.Add(new WindowInfo(winRect, hwnd, true));
						}
					}
				}
				return true;
			}, IntPtr.Zero);

		}

		private static Rectangle FixWindowRect(Rectangle rect, Screen screen)
		{
			rect.Width -= rect.X;
			rect.Height -= rect.Y;
			rect.Offset(-screen.Bounds.Left, -screen.Bounds.Top);

			return rect;
		}

		/// <summary>
		/// Capture current selected range and end capture window
		/// </summary>
		public void DoCaptureScreen()
		{
			if (selection.Width > 0 && selection.Height > 0)
			{
				CapturedImage = new Bitmap(BackgroundImage, selection.Width, selection.Height);

				using (Graphics g = Graphics.FromImage(CapturedImage))
				{
					g.DrawImage(this.BackgroundImage, 0, 0, selection, GraphicsUnit.Pixel);
				}

				EndCapture();
			}
		}

		#endregion

		#region Paint
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.PixelOffsetMode = PixelOffsetMode.Half;
			g.SmoothingMode = SmoothingMode.None;
			g.CompositingQuality = CompositingQuality.HighSpeed;
			//g.CompositingMode = CompositingMode.SourceCopy;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			//g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

			if (operation == Operation.FindWindow
				|| operation == Operation.FindControl
				&& !HoverWindow.IsEmpty)
			{
				using (Pen p = new Pen(Color.Red, 3))
				{
					using (Brush b = new SolidBrush(Color.FromArgb(100, Color.White)))
					{
						g.FillRectangle(b, HoverWindow.rect);
					}
					g.DrawRectangle(p, HoverWindow.rect);
				}
			}

			if (selection.Width > 0 && selection.Height > 0)
			{
				Rectangle rect = selection;

				rect.Inflate(3, 3);

				GraphicsToolkit.DrawDoubleLineRectangle(g, rect, outerColor, innerColor);

				if (!designing)
				{
					RangeHandlerEdit.DrawRangeHandlerPos(g, hands, thumbColor);
				}
			}

			if (flashBrightness > 0)
			{
				Rectangle flashRect = ClientRectangle;
				using (Brush b = new SolidBrush(Color.FromArgb(flashBrightness, Color.White)))
				{
					g.FillRectangle(b, flashRect);
				}
			}
			else
			{
				if (AllowMagnifier && this.magnifierBitmap != null)
				{
					var rect = magnifierRect;
					rect.Inflate(2, 2);

					var p = ResourcePoolManager.Instance.GetPen(this.innerColor, 2f, DashStyle.Solid);
					if (p != null)
					{
						g.DrawRectangle(p, rect);
					}

					g.DrawImage(this.magnifierBitmap, this.magnifierRect.Location);
				}


				if (ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.CF_Show_Coordinate_Info, true))
				{
					if ((this.operation == Operation.RangeSelection
						|| this.operation == Operation.MoveSelection
						|| this.operation == Operation.ResizeSelection)
						&& (this.selection.Width > 0
						&& this.selection.Height > 0))
					{
						using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic)
						{
							LineAlignment = StringAlignment.Center,
							Alignment = StringAlignment.Center
						})
						{
							string text = string.Format("{0},{1} - {2},{3} [{4}x{5}]",
								this.selection.X, this.selection.Y, this.selection.Right, this.selection.Bottom,
								this.selection.Width, this.selection.Height);

							Size size = Size.Round(g.MeasureString(text, Font, ClientRectangle.Size));
							size.Width += 10;
							size.Height += 5;

							int x = (ClientRectangle.Right - this.selection.Right - 10) < size.Width
								? (ClientRectangle.Right - size.Width) : (this.selection.Right + 10);
							int y = ClientRectangle.Bottom - this.selection.Bottom - 5 < size.Height
								? (ClientRectangle.Bottom - size.Height) : (this.selection.Bottom + 5);

							Rectangle r = new Rectangle(x, y, size.Width, size.Height);
							g.FillRectangle(Brushes.LightYellow, r);
							g.DrawRectangle(Pens.DarkRed, r);
							g.DrawString(text, Font, Brushes.Black, r, sf);
						}
					}
				}

			}
		}
		#endregion

		#region Keyboard & Mouse
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Escape:
					EndCapture();
					return true;

				case Keys.Up:
					if (/*ShortcutTextbox.IsKeyDown(Win32.VKey.VK_LBUTTON)
						||*/ operation == Operation.ResizeSelection
						|| (operation == Operation.RangeSelection && selection.IsEmpty))
					{
						Point p = new Point();
						Win32.GetCursorPos(ref p);
						if (p.Y > -10) Win32.SetCursorPos(p.X, p.Y - 1);
					}
					else if (operation == Operation.RangeSelection && !selection.IsEmpty)
					{
						OffsetSelection(0, -1, true);
					}
					return true;

				case Keys.Down:
					if (/*ShortcutTextbox.IsKeyDown(Win32.VKey.VK_LBUTTON)
						||*/ operation == Operation.ResizeSelection
						|| (operation == Operation.RangeSelection && selection.IsEmpty))
					{
						Point p = new Point();
						Win32.GetCursorPos(ref p);
						if (p.Y < Height+10) Win32.SetCursorPos(p.X, p.Y + 1);
					}
					else if (operation == Operation.RangeSelection && !selection.IsEmpty)
					{
						OffsetSelection(0, 1, true);
					}
					return true;

				case Keys.Left:
					if (/*ShortcutTextbox.IsKeyDown(Win32.VKey.VK_LBUTTON)
						||*/ operation == Operation.ResizeSelection
						|| (operation == Operation.RangeSelection && selection.IsEmpty))
					{
						Point p = new Point();
						Win32.GetCursorPos(ref p);
						if (p.X > -10) Win32.SetCursorPos(p.X - 1, p.Y);
					}
					else if (operation == Operation.RangeSelection && !selection.IsEmpty)
					{
						OffsetSelection(-1, 0, true);
					}
					return true;

				case Keys.Right:
					if (/*ShortcutTextbox.IsKeyDown(Win32.VKey.VK_LBUTTON)
						||*/ operation == Operation.ResizeSelection
						|| (operation == Operation.RangeSelection && selection.IsEmpty))
					{
						Point p = new Point();
						Win32.GetCursorPos(ref p);
						if (p.X < Width+10) Win32.SetCursorPos(p.X + 1, p.Y);
					}
					else if (operation == Operation.RangeSelection && !selection.IsEmpty)
					{
						OffsetSelection(1, 0, true);
					}
					return true;

				case Keys.F:
					SelectFullScreen();
					return true;

				case Keys.W:
					StartSelectWindow();
					return true;

				case Keys.R:
					resetRegionToolStripButton.PerformClick();
					return true;

				case Keys.Control | Keys.C:
				case Keys.C:
					copyToolStripMenuItem.PerformClick();
					return true;

				case Keys.E:
					editToolStripMenuItem.PerformClick();
					return true;

				case Keys.S:
					saveToolStripMenuItem.PerformClick();
					return true;

				case Keys.P:
					printToolStripMenuItem.PerformClick();
					return true;

				case Keys.Enter:
					if (WorkMode == WorkMode.General)
						copyToolStripButton.PerformClick();
					else
						captureToolStripButton.PerformClick();
					return true;

				default:
					return base.ProcessCmdKey(ref m, keyData);
			}
		}

		private int? startX;
		private int? startY;
		private int endX;
		private int endY;

		private Rectangle selection= Rectangle.Empty;
		public Rectangle Selection
		{
			get { return selection; }
			set
			{
				if (selection != value || !designing)
				{
					selection = value;

					RangeHandlerEdit.UpdateRangeHandlers(selection, hands);

					MoveToolstrip();
					UpdateMenus();

					Invalidate();
				}
			}
		}
		private Rectangle[] hands = new Rectangle[8];

		private void OffsetSelection(int offx, int offy, bool updateMagnifier = false)
		{
			if (selection.X + offx < 0) offx = -selection.X;
			if (selection.Y + offy < 0) offy = -selection.Y;
			if (selection.Right + offx > ClientRectangle.Right)
				offx = ClientRectangle.Right - selection.Right;
			if (selection.Bottom + offy > ClientRectangle.Bottom)
				offy = ClientRectangle.Bottom - selection.Bottom;

			selection.Offset(offx, offy);

			RangeHandlerEdit.UpdateRangeHandlers(selection, hands);

			hoverPosition.Offset(offx, offy);

			if (updateMagnifier)
			{
				UpdateMagnifier();
			}

			Invalidate();
		}

		/// <summary>
		/// Resize selection rectangle
		/// </summary>
		/// <param name="pos">thumb on rectangle refered to resize given rectangle</param>
		/// <param name="p">target point to resize</param>
		private void ResizeSelection(RangeHandlerEdit.RangeHandlerPos pos, Point p)
		{
			if (p.X < 0) p.X = 0;
			if (p.X > Width) p.X = Width;
			if (p.Y < 0) p.Y = 0;
			if (p.Y > Height) p.Y = Height;

			switch (pos)
			{
				case RangeHandlerEdit.RangeHandlerPos.Left:
					selection.X = p.X;
					selection.Width = backupSelection.Right - p.X;
					break;
				case RangeHandlerEdit.RangeHandlerPos.Right:
					selection.Width = p.X - selection.X;
					break;
				case RangeHandlerEdit.RangeHandlerPos.Top:
					selection.Y = p.Y;
					selection.Height = backupSelection.Bottom - p.Y;
					break;
				case RangeHandlerEdit.RangeHandlerPos.Bottom:
					selection.Height = p.Y - selection.Y;
					break;

				case RangeHandlerEdit.RangeHandlerPos.LeftTop:
					selection.X = p.X;
					selection.Width = backupSelection.Right - p.X;
					selection.Y = p.Y;
					selection.Height = backupSelection.Bottom - p.Y;
					break;
				case RangeHandlerEdit.RangeHandlerPos.RightTop:
					selection.Y = p.Y;
					selection.Height = backupSelection.Bottom - p.Y;
					selection.Width = p.X - selection.X;
					break;
				case RangeHandlerEdit.RangeHandlerPos.LeftBottom:
					selection.X = p.X;
					selection.Width = backupSelection.Right - p.X;
					selection.Height = p.Y - selection.Y;
					break;
				case RangeHandlerEdit.RangeHandlerPos.RightBottom:
					selection.Width = p.X - selection.X;
					selection.Height = p.Y - selection.Y;
					break;
			}

			RangeHandlerEdit.UpdateRangeHandlers(selection, hands);
			Invalidate();
		}

		private void MoveToolstrip()
		{
			if (ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.CF_Toolstrip, true))
			{
				int y = selection.Y - toolbarPanel.Height - 10;
				if (y < 0)
				{
					y = selection.Bottom + 10;

					if (y > Bottom)
					{
						y = selection.Y + 10;
					}
				}

				int x = selection.X;
				if (x < 0)
				{
					x = 0;
				}
				else if (x + toolbarPanel.Width > Width)
				{
					x = Width - toolbarPanel.Width - 10;
				}

				toolbarPanel.Location = new Point(x, y);
				toolbarPanel.Visible = (selection.Width > 0 || selection.Height > 0);
			}
		}

		private bool designing = false;
		private Operation operation = Operation.RangeSelection;
		private Point movingOffset;
		private RangeHandlerEdit.RangeHandlerPos resizePos = RangeHandlerEdit.RangeHandlerPos.None;
		private Rectangle backupSelection;
		private Point hoverPosition;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			infoLabel.Visible = false;

			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				switch (this.operation)
				{
					case Operation.RangeSelection:
						#region RangeSelection
						{
							RangeHandlerEdit.RangeHandlerPos pos = RangeHandlerEdit.InRangeHandler(hands, e.Location);

							if (pos != RangeHandlerEdit.RangeHandlerPos.None)
							{
								operation = Operation.ResizeSelection;
								resizePos = pos;
								backupSelection = selection;
							}
							else if (selection.Contains(e.Location))
							{
								movingOffset = new Point(e.X - selection.X, e.Y - selection.Y);
								operation = Operation.MoveSelection;
							}
							else
							{
								resizePos = RangeHandlerEdit.RangeHandlerPos.None;

								startX = endX = e.X;
								startY = endY = e.Y;

								designing = true;
							}

							Invalidate();
						}
						#endregion RangeSelection
						break;

					case Operation.AutoScanRange:
						this.Cursor = Cursors.WaitCursor;
						break;
				}
			}
		}

		#region SmartScan
		void SmartScan(int x, int y, AutoScanSession ass)
		{
			ass.left = ass.right = x;
			ass.top = ass.bottom = y;
			ass.c = ((Bitmap)this.BackgroundImage).GetPixel(x, y);

			ass.image = (Bitmap)this.BackgroundImage;
			ass.c = ass.image.GetPixel(x, y);
			ass.scanedBits = new bool[this.BackgroundImage.Width, this.BackgroundImage.Height];

			ScanTop(x, y, ass);

			Invalidate();
		}

		class AutoScanSession : IDisposable
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
			public bool[,] scanedBits;

			public Color c;
			public Bitmap image;

			public void Dispose()
			{

			}
		}

		private void ScanRight(int x, int y, AutoScanSession ass)
		{
			if (ass.scanedBits[x, y] || ass.image.GetPixel(x, y) != ass.c) return;

			for (; x < ass.image.Width; x++)
			{
				if (!ass.scanedBits[x, y] && ass.image.GetPixel(x, y) == ass.c)
				{
					ass.scanedBits[x, y] = true;
					//ass.image.SetPixel(x, y, Color.Red);

					if (ass.right < x) ass.right = x;

					ScanTop(ass.right, y - 1, ass);
				}
				else break;
			}

			ScanTop(ass.right, y - 1, ass);
		}

		private void ScanBottom(int x, int y, AutoScanSession ass)
		{
			if (ass.scanedBits[x, y] || ass.image.GetPixel(x, y) != ass.c) return;

			for (; y < ass.image.Height; y++)
			{
				if (!ass.scanedBits[x, y] && ass.image.GetPixel(x, y) == ass.c)
				{
					ass.scanedBits[x, y] = true;
					//ass.image.SetPixel(x, y, Color.Red);

					if (ass.bottom < y) ass.bottom = y;

					ScanRight(x + 1, ass.bottom, ass);
				}
				else break;
			}

			ScanRight(x + 1, ass.bottom, ass);
		}

		private void ScanLeft(int x, int y, AutoScanSession ass)
		{
			if (ass.scanedBits[x, y] || ass.image.GetPixel(x, y) != ass.c) return;

			for (; x > 0; x--)
			{
				if (!ass.scanedBits[x, y] && ass.image.GetPixel(x, y) == ass.c)
				{
					ass.scanedBits[x, y] = true;
					//ass.image.SetPixel(x, y, Color.Red);

					if (ass.left > x) ass.left = x;

					ScanBottom(ass.left, y + 1, ass);
				}
				else break;
			}

			ScanBottom(ass.left, y + 1, ass);
		}

		private void ScanTop(int x, int y, AutoScanSession ass)
		{
			if (ass.scanedBits[x, y] || ass.image.GetPixel(x, y) != ass.c) return;

			for (; y > 0; y--)
			{
				if (!ass.scanedBits[x, y] && ass.image.GetPixel(x, y) == ass.c)
				{
					if (ass.top > y) ass.top = y;

					ass.scanedBits[x, y] = true;
					//ass.image.SetPixel(x, y, Color.Red);

					ScanLeft(x - 1, y, ass);
				}
				else break;
			}

			ScanLeft(x - 1, ass.top, ass);
		}
		#endregion // SmartScan
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				toolbarPanel.Visible = false;
			}

			if (operation == Operation.FindWindow
				|| operation == Operation.FindControl)
			{
				WindowInfo winfo = GetWindowInfoByPos(e.Location);
				if (lastHoverHwnd != winfo.hwnd)
				{
					HoverWindow = winfo;
					lastHoverHwnd = winfo.hwnd;
					Invalidate();
				}
			}
			else if (operation == Operation.RangeSelection)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (startX != null && startY != null)
					{
						endX = e.X;
						endY = e.Y;

						int sx = Math.Min(startX ?? 0, endX);
						int sy = Math.Min(startY ?? 0, endY);
						int ex = Math.Max(startX ?? 0, endX);
						int ey = Math.Max(startY ?? 0, endY);

						selection = new Rectangle(sx, sy, ex - sx, ey - sy);

						Invalidate();
					}

					infoLabel.Visible = false;
				}
				else
				{
					Cursor cur = Cursors.Default;

					if (!designing && hands != null)
					{
						cur = RangeHandlerEdit.CursorOnRangeHandler(hands, e.Location);
					}

					if (cur != Cursors.Default)
					{
						Cursor = cur;
					}
					else if (GraphicsToolkit.PointInRect(selection, e.Location))
					{
						Cursor = Cursors.SizeAll;
					}
					else
					{
						Cursor = Cursors.Cross;
					}

					if (ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.CF_Show_Coordinate_Info, true))
					{
						infoLabel.Text = string.Format("{0},{1}", e.X, e.Y);
						infoLabel.Left = e.X + 12;
						infoLabel.Top = e.Y + 12;
						infoLabel.Visible = true;
					}
					else
					{
						infoLabel.Visible = false;
					}
				}

			}
			else if (operation == Operation.MoveSelection)
			{
				int offx = e.X - selection.X - movingOffset.X;
				int offy = e.Y - selection.Y - movingOffset.Y;

				OffsetSelection(offx, offy);
			}
			else if (operation == Operation.ResizeSelection)
			{
				ResizeSelection(resizePos, e.Location);
			}
			else if (operation == Operation.FindWindow)
			{
				Cursor = Cursors.Default;
			}

			hoverPosition = e.Location;
			UpdateMagnifier();
		}

		private void UpdateMagnifier()
		{
			if (!AllowMagnifier)
			{
				return;
			}

			if (hoverPosition.X < magnifierRect.Width && hoverPosition.Y < magnifierRect.Height)
			{
				magnifierRect.Location = new Point(this.ClientRectangle.Width - magnifierRect.Width,
					this.ClientRectangle.Height - magnifierRect.Height);
			}
			else if (hoverPosition.X > this.ClientRectangle.Width - magnifierRect.Width
				&& hoverPosition.Y > this.ClientRectangle.Height - magnifierRect.Height)
			{
				magnifierRect.Location = new Point(0, 0);
			}

			using (Graphics gm = Graphics.FromImage(this.magnifierBitmap))
			{
				int mw = (int)Math.Round(this.magnifierRect.Width / this.magnifierScale);
				int mh = (int)Math.Round(this.magnifierRect.Height / this.magnifierScale);

				gm.PixelOffsetMode = PixelOffsetMode.Half;
				gm.SmoothingMode = SmoothingMode.None;
				gm.CompositingQuality = CompositingQuality.HighSpeed;
				gm.CompositingMode = CompositingMode.SourceCopy;
				gm.InterpolationMode = InterpolationMode.NearestNeighbor;

				var src = new Rectangle(hoverPosition.X - mw / 2,
					hoverPosition.Y - mh / 2, mw, mh);

				if (src.X < 0) src.X = 0;
				if (src.Y < 0) src.Y = 0;
				if (src.Right > Right) src.X = Right - src.Width;
				if (src.Bottom > Bottom) src.Y = Bottom - src.Height;

				gm.DrawImage(this.BackgroundImage, new Rectangle(0, 0,
					this.magnifierBitmap.Width, this.magnifierBitmap.Height),
					src, GraphicsUnit.Pixel);

				float cx = this.magnifierRect.Width / 2;
				float cy = this.magnifierRect.Height / 2;

				var p = ResourcePoolManager.Instance.GetPen(this.thumbColor);
				if (thumbColor != null)
				{
					gm.DrawLine(p, cx, cy - 10, cx, cy + 10);
					gm.DrawLine(p, cx - 10, cy, cx + 10, cy);
				}

				var innerPen = ResourcePoolManager.Instance.GetPen(this.innerColor);
				if (innerPen != null)
				{
					RectangleF rect = this.selection;
					rect.Offset((int)(-src.X), (int)(-src.Y));
					rect.X *= this.magnifierScale;
					rect.Y *= this.magnifierScale;
					rect.Width *= this.magnifierScale;
					rect.Height *= this.magnifierScale;

					if (rect.Width > 0 && rect.Height > 0)
					{
						gm.DrawRectangle(innerPen, rect.X, rect.Y, rect.Width, rect.Height);
					}
				}
			}

			Invalidate();
		}

		private Label infoLabel = new Label()
		{
			BackColor = Color.LightYellow,
			Visible = false,
			TextAlign = ContentAlignment.MiddleCenter,
			AutoSize = true,
			Padding = new Padding(5, 3, 5, 3),
		};

		private WindowInfo GetWindowInfoByPos(Point point)
		{
			foreach (WindowInfo winfo in allWindowBounds)
			{
				if (operation == Operation.FindWindow)
				{
					if (GraphicsToolkit.PointInRect(winfo.rect, point) && winfo.isWindow)
					{
						return winfo;
					}
				}
				else if (GraphicsToolkit.PointInRect(winfo.rect, point) && !winfo.isWindow)
				{
					return winfo;
				}
			}
			return WindowInfo.Empty;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			designing = false;

			if (operation == Operation.FindWindow)
			{
				if (!HoverWindow.IsEmpty)
				{
					this.Selection = HoverWindow.rect;
				}
				else
				{
					this.Selection = Rectangle.Empty;
				}

				this.operation = Operation.RangeSelection;

				if (ShortcutTextbox.IsKeyDown(Win32.VKey.VK_CONTROL) || ShortcutTextbox.IsKeyDown(Win32.VKey.VK_SHIFT)
					|| InitCaptureMethod == CaptureMode.SelectWindow
					&& e.Button == MouseButtons.Left)
				{
					this.DoCaptureScreen();
				}
			}
			else if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				switch (this.operation)
				{
					case Operation.RangeSelection:
						#region RangeSelection
						{
							int sx = Math.Min(startX ?? 0, endX);
							int sy = Math.Min(startY ?? 0, endY);
							int ex = Math.Max(startX ?? 0, endX);
							int ey = Math.Max(startY ?? 0, endY);

							int w = ex - sx;
							int h = ey - sy;

							Selection = new Rectangle(sx, sy, w, h);
						}
						#endregion // RangeSelection
						break;

					case Operation.AutoScanRange:
						#region AutoScanRange
						{
							// go top
							int maxWH = Math.Max(this.BackgroundImage.Width, this.BackgroundImage.Height);

							int y = e.Location.Y, x = e.Location.X;

							AutoScanSession ass = new AutoScanSession();

							SmartScan(x, y, ass);

							int w = ass.right - ass.left - 1;
							int h = ass.bottom - ass.top - 1;

							if (w > 0 && h > 0)
							{
								this.Selection = new Rectangle(ass.left + 1, ass.top + 1, w, h);
							}

							operation = Operation.RangeSelection;

							MoveToolstrip();
							Invalidate();
						}
						#endregion // AutoScanRange
						break;

					default:
						#region Default
						{
							if (selection.Width > 0 && selection.Height > 0)
							{
								if (ShortcutTextbox.IsKeyDown(Win32.VKey.VK_CONTROL) || ShortcutTextbox.IsKeyDown(Win32.VKey.VK_SHIFT))
								{
									DoCaptureScreen();
								}
							}

							startX = null;
							startY = null;
							operation = Operation.RangeSelection;

							MoveToolstrip();
							Invalidate();
						}
						#endregion // Default
						break;
				}
			}
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			DoCaptureScreen();
		}

		#endregion

		#region Window Control

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			try
			{
				if (BackgroundImage != null) BackgroundImage.Dispose();
			}
			catch { }
		}

		//protected override void OnClosing(CancelEventArgs e)
		//{
		//  e.Cancel = true;
		//  Visible = false;
		//}

		/// <summary>
		/// dispose and shutdown capture form instance
		/// </summary>
		internal static void Shutdown()
		{
			if (captureForm != null)
			{
				captureForm.Close();
				captureForm.Dispose();
			}
		}

		internal static CaptureForm Instance { get { return captureForm; } }

		/// <summary>
		/// Get a window rectangle bounds
		/// </summary>
		/// <param name="hwnd">handle to window</param>
		/// <returns>rectangle of specified window</returns>
		public static Rectangle GetWindowRect(IntPtr hwnd)
		{
			Rectangle rect = new Rectangle();

			Win32.WindowPlacement windowPlacement = new Win32.WindowPlacement();
			Win32.GetWindowPlacement(hwnd, ref windowPlacement);

			if (windowPlacement.showCmd == (int)Win32.ShowWindowCmd.SW_MAXIMIZE)
			{
				Screen screen = Screen.FromHandle(hwnd);
				rect = screen.WorkingArea;
				//Win32.GetWindowRect(hwnd, ref rect);
			}
			else
			{
				Win32.OSVersionInfo osVersion = new Win32.OSVersionInfo();
				osVersion.dwOSVersionInfoSize = Marshal.SizeOf(typeof(Win32.OSVersionInfo));
				Win32.GetVersionEx(ref osVersion);

				if (osVersion.dwMajorVersion >= 6) // vista and after
				{
					int hr = Win32.DwmGetWindowAttribute(hwnd,
						(int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS,
						 ref rect, Marshal.SizeOf(rect));

					Exception ex = Marshal.GetExceptionForHR(hr);
					if (ex != null)
					{
						Win32.GetWindowRect(hwnd, ref rect);
					}
				}
				else
				{
					Win32.GetWindowRect(hwnd, ref rect);
				}
			}

			if (!rect.IsEmpty)
			{
				rect.Width -= rect.Left;
				rect.Height -= rect.Top;
			}

			// rect(2200,100,1194,768) in secondary screen

			return rect;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			Focus();
		}
		#endregion

		#region Magnifier
		public bool AllowMagnifier { get; set; }

		private float magnifierScale = 2f;

		private Rectangle magnifierRect = new Rectangle(0, 0, 200, 200);

		private Bitmap magnifierBitmap = null;

		#endregion // Magnifier

		enum Operation
		{
			RangeSelection,
			MoveSelection,
			ResizeSelection,

			FindWindow,
			FindControl,
			AutoScanRange,
		}

		struct WindowInfo
		{
			public Rectangle rect;
			public IntPtr hwnd;
			public bool isWindow;

			public WindowInfo(Rectangle rect, IntPtr hwnd, bool isWindow)
			{
				this.rect = rect;
				this.hwnd = hwnd;
				this.isWindow = isWindow;
			}

			public static readonly WindowInfo Empty = new WindowInfo
			{
				rect = Rectangle.Empty,
				hwnd = IntPtr.Zero,
			};

			public bool IsEmpty
			{
				get
				{
					return this.rect == Empty.rect && this.hwnd == Empty.hwnd;
				}
			}
		}

	}

	enum WorkMode
	{
		General,
		OnlyCapture,
		AutoCapture,
	}

	enum CaptureMode
	{
		FreeRegion,
		SelectWindow,
		GetScrollContent,
		FullScreen,
		SmartScan,
	}

	enum MultiScreenMode
	{
		MouseActiveScreen,
		AllScreens,
		OnlyPrimary,
	}
}