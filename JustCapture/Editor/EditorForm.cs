using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;

using unvell.Common;
using unvell.JustCapture.Toolkits;
using unvell.JustCapture.Properties;

namespace unvell.JustCapture.Editor
{
	internal partial class EditorForm : Form
	{
		#region Canvas
		private Canvas canvas;

		private class Canvas : Control
		{
			private EditorForm owner;
			internal Canvas(EditorForm host)
			{
				this.owner = host;

				this.BackColor = SystemColors.AppWorkspace;
				this.DoubleBuffered = true;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				owner.DrawCanvas(e.Graphics, ClientRectangle, true);
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				owner.OnCanvasMouseDown(e);
			}
			protected override void OnMouseMove(MouseEventArgs e)
			{
				owner.OnCanvasMouseMove(e);
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				owner.OnCanvasMouseUp(e);
			}
			protected override void OnMouseDoubleClick(MouseEventArgs e)
			{
				owner.OnCanvasMouseDoubleClick(e);
			}
			protected override void OnKeyDown(KeyEventArgs e)
			{
				owner.OnKeyDown(e);
			}
			protected override void OnMouseEnter(EventArgs e)
			{
				owner.OnCanvasMouseEnter(e);
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				owner.OnCanvasMouseLeave(e);
			}
		}

		private class ScrollPanel : Panel
		{
			protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
			{
				// Returning the current location prevents the panel from
				// scrolling to the active control when the panel loses and regains focus
				return this.DisplayRectangle.Location;
			}
		}
		#endregion

		private ScrollPanel canvasPanel = new ScrollPanel();
		private System.Windows.Forms.Timer dashTimer = new System.Windows.Forms.Timer();

		public EditorForm()
		{
			InitializeComponent();

			BackColor = SystemColors.AppWorkspace;
			statusToolStripStatusLabel.BackColor = SystemColors.Control;

			canvasPanel.Dock = DockStyle.Fill;

			canvas = new Canvas(this) { BackColor = Color.White };
			canvasPanel.Controls.Add(canvas);
			canvasPanel.AutoScroll = true;
			canvasPanel.BorderStyle = BorderStyle.Fixed3D;

			Controls.Add(canvasPanel);
			canvasPanel.Controls.Add(resizeThumb);
			canvasPanel.BringToFront();

			selectToolStripButton.CheckOnClick = true;
			rectToolStripButton.CheckOnClick = true;
			lineShapeToolStripItem.CheckOnClick = true;
			arrowShapeToolStripItem.CheckOnClick = true;

			InitCulture();

			rectToolStripButton.CurrentColor =
				FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.IEF_RectColor, "Red"));
			lineShapeToolStripItem.CurrentColor =
				FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.IEF_LineColor, "Lime"));
			arrowShapeToolStripItem.CurrentColor =
				FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.IEF_ArrowColor, "Blue"));
			ellipseShapeToolStripItem.CurrentColor =
				FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.IEF_EllipseColor, "Orange"));
			numberShapeToolStripItem.CurrentColor =
				FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.IEF_NumberColor, "Red"));

			UpdateMenus();

			captureToolStripButton.Click += (s, e) => CaptureFreeRegion();
			captureARegionOfScreenToolStripMenuItem.Click += (s, e) => CaptureFreeRegion();

			captureWindowToolStripMenuItem.Click += (s, e) => CaptureWindow();
			captureWindowToolStripButton.Click += (s, e) => CaptureWindow();

			captureCurrentFullScreenToolStripMenuItem.Click += (s, e) => CaptureFullWindow();
			captureFullScreenToolStripButton.Click += (s, e) => CaptureFullWindow();

			newToolStripButton.Click += (s, e) => NewImage();
			openToolStripButton.Click += (s, e) => OpenImage();
			saveToolStripButton.Click += (s, e) => SaveImage(this.currentFile);

			undoToolStripButton.Click += (s, e) => Undo();
			redoToolStripButton.Click += (s, e) => Redo();

			copyToolStripButton.Click += (s, e) => CopyImage();
			cutToolStripButton.Click += (s, e) => CutImage();
			pasteToolStripButton.Click += (s, e) => PasteImage();

			selectToolStripButton.CheckedChanged += (s, e) =>
			{
				if (selectToolStripButton.Checked)
				{
					StartSelectRange();
				}
				else
				{
					EndRangeSelection();
				}
			};

			settingsToolStripMenuItem.Click += (s, e) =>
			{
				// unregister hotkeys
				ShortcutActionManager.UnregisterAllShortcutActions(this.Handle);

				if (new SettingForm().ShowDialog() == DialogResult.OK)
				{
					MainForm.Instance.InitCulture();
					CaptureForm.InitInstanceCulture();
					InitCulture();
				}

				// reregister hotkeys
				ShortcutActionManager.RegisterAllShortcutActions(this.Handle);
			};

			dashTimer.Interval = 100;
			dashTimer.Tick += (s, e) =>
			{
				dashOffset++;
				if (dashOffset > 3) dashOffset = 0;

				canvas.Invalidate();
			};

			// files
			newToolStripMenuItem.Click += (s, e) => NewImage();
			openImageToolStripMenuItem.Click += (s, e) => OpenImage();
			saveImageToolStripMenuItem.Click += (s, e) => SaveImage(this.currentFile);
			saveAsToolStripMenuItem.Click += (s, e) => SaveImage(null);
			closeToolStripMenuItem.Click += (s, e) => Close();

			// edit
			undoToolStripMenuItem.Click += (s, e) => Undo();
			redoToolStripMenuItem.Click += (s, e) => Redo();
			copyToolStripMenuItem.Click += (s, e) => CopyImage();
			cutToolStripMenuItem.Click += (s, e) => CutImage();
			pasteToolStripMenuItem.Click += (s, e) => PasteImage();

			selectToolStripMenuItem.Click += (s, e) => StartSelectRange();
			selectAllToolStripMenuItem.Click += (s, e) => SelectAll();
			deselectToolStripMenuItem.Click += (s, e) => EndRangeSelection();

			clipSelectedImageToolStripMenuItem.Click += (s, e) => CutSelectedImageRange();

			// image
			drawRectangleToolStripMenuItem.Click += (s, e) => { rectToolStripButton.Checked = true; StartDrawRect(); };
			drawLineToolStripMenuItem.Click += (s, e) => { lineShapeToolStripItem.Checked = true; StartDrawLine(); };
			drawArrowToolStripMenuItem.Click += (s, e) => { arrowShapeToolStripItem.Checked = true; StartDrawArrow(); };
			drawEllipseToolStripMenuItem.Click += (s, e) => { ellipseShapeToolStripItem.Checked = true; StartDrawEllipse(); };
			addNumberToolStripMenuItem.Click += (s, e) => { numberShapeToolStripItem.Checked = true; CheckToAddNumber(); };
		
			rectToolStripButton.CheckChanged += (s, e) => StartDrawRect();
			rectToolStripButton.ColorPicked += (s, e) =>
			{
				ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.IEF_RectColor,
					FileToolkit.EncodeColor(rectToolStripButton.CurrentColor));
			};

			lineShapeToolStripItem.CheckChanged += (s, e) => StartDrawLine();
			lineShapeToolStripItem.ColorPicked += (s, e) =>
			{
				ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.IEF_LineColor,
					FileToolkit.EncodeColor(lineShapeToolStripItem.CurrentColor));
			};

			arrowShapeToolStripItem.CheckChanged += (s, e) => StartDrawArrow();
			arrowShapeToolStripItem.ColorPicked += (s, e) =>
			{
				ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.IEF_ArrowColor,
					FileToolkit.EncodeColor(arrowShapeToolStripItem.CurrentColor));
			};

			ellipseShapeToolStripItem.CheckChanged += (s, e) => StartDrawEllipse();
			ellipseShapeToolStripItem.ColorPicked += (s, e) =>
			{
				ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.IEF_EllipseColor,
					FileToolkit.EncodeColor(ellipseShapeToolStripItem.CurrentColor));
			};

			numberShapeToolStripItem.CheckChanged += (s, e) => CheckToAddNumber();
			numberShapeToolStripItem.ColorPicked += (s, e) =>
			{
				ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.IEF_NumberColor,
					FileToolkit.EncodeColor(numberShapeToolStripItem.CurrentColor));
			};
			numberShapeToolStripItem.CurrentNumberChanged += (s, e) => canvas.Invalidate();

			resetNumberToolStripMenuItem.Click += (s, e) => numberShapeToolStripItem.CurrentNumber = 1;
			resetNumberToolStripButton.Click += (s, e) => numberShapeToolStripItem.CurrentNumber = 1;

			// help
			homepageToolStripMenuItem.Click += (s, e) => ProductInfo.OpenProductHomepage();
			aboutToolStripMenuItem.Click += (s, e) => new AboutForm().ShowDialog();

			history.Add(null);

			using (MemoryStream ms = new MemoryStream(unvell.JustCapture.Properties.Resources.addnum))
			{
				addNumberCursor = new Cursor(ms);
			}

			resizeThumb.MouseDown += (s, e) =>
			{
				resizeThumbOffset = e.Location;
				canvasPanel.Capture = true;

				Log("{0} x {1}", canvas.Width, canvas.Height);
			};

			canvasPanel.MouseMove += (s, e) =>
			{
				if (resizeThumbOffset != Point.Empty)
				{
					Size size = new Size(e.X - resizeThumbOffset.X, e.Y - resizeThumbOffset.Y);
					if (size.Width < 10) size.Width = 1;
					if (size.Height < 10) size.Height = 1;

					canvas.Size = size;

					resizeThumb.Location = new Point(canvas.Right, canvas.Bottom);
					Log("{0} x {1}", canvas.Width, canvas.Height);
				}
			};

			canvasPanel.MouseUp += (s, e) =>
			{
				CommitCurrentImage();
				this.resizeThumbOffset = Point.Empty;
			};

			resizeToolStripButton.Click += resizeToolStripButton_Click;
			resizeToolStripMenuItem.Click += resizeToolStripButton_Click;
			scaleToolStripButton.Click += scaleToolStripButton_Click;
			scaleToolStripMenuItem.Click += scaleToolStripButton_Click;

			rotateRightToolStripButton.Click += rotateRightToolStripButton_Click;
			rotateRightToolStripMenuItem.Click += rotateRightToolStripButton_Click;
			rotateLeftToolStripButton.Click += rotateLeftToolStripButton_Click;
			rotateLeftToolStripMenuItem.Click += rotateLeftToolStripButton_Click;

			canvas.SizeChanged += (s, e) => canvasSizeToolStripStatusLabel.Text =
				string.Format("{0} x {1}", canvas.Width, canvas.Height);

			canvasSizeToolStripStatusLabel.Click += resizeToolStripButton_Click;

			toolbarToolStripMenuItem.Checked = mainToolStrip.Visible =
				ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.IEF_Toolbar_Visible, true);

			toolbarToolStripMenuItem.CheckedChanged += (s, e) =>
			{
				mainToolStrip.Visible = toolbarToolStripMenuItem.Checked;
				ConfigurationManager.Instance.SwitchCurrentUserSetting(
					UserConfigKey.IEF_Toolbar_Visible, toolbarToolStripMenuItem.Checked);

				if (mainToolStrip.Visible) menuStrip1.SendToBack();
			};

			statusBarToolStripMenuItem.Checked = mainStatusStrip.Visible =
				ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.IEF_StatusBar_Visible, true);

			statusBarToolStripMenuItem.CheckedChanged += (s, e) =>
			{
				mainStatusStrip.Visible = statusBarToolStripMenuItem.Checked;
				ConfigurationManager.Instance.SwitchCurrentUserSetting(
					UserConfigKey.IEF_StatusBar_Visible, statusBarToolStripMenuItem.Checked);
			};

			textToolStripButton.CheckChanged += (s, e) =>
			{
				if (textToolStripButton.Checked)
				{
					EndDrawShape();
					EndRangeSelection();
					this.operation = Operation.AddText;
					canvas.Cursor = Cursors.IBeam;
				}
				else
				{
					this.operation = Operation.None;
					canvas.Cursor = Cursors.Default;
				}
			};
			textToolStripButton.CheckOnClick = true;
		}

		void resizeToolStripButton_Click(object sender, EventArgs e)
		{
			using (ImageSizeForm isf = new ImageSizeForm())
			{
				isf.ImageSize = canvasSize;

				if (isf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					Size size = isf.ImageSize;
					if (size.Width < 1) size.Width = 1;
					if (size.Height < 1) size.Height = 1;

					this.canvas.Size = size;
					resizeThumb.Location = new Point(canvas.Right, canvas.Bottom);
					CommitCurrentImage();
				}
			}
		}

		void scaleToolStripButton_Click(object sender, EventArgs e)
		{
			using (ScaleImageForm isf = new ScaleImageForm())
			{
				isf.ImageSize = canvasSize;

				if (isf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					Size size = isf.ImageSize;
					if (size.Width < 1) size.Width = 1;
					if (size.Height < 1) size.Height = 1;

					this.canvas.Size = size;
					resizeThumb.Location = new Point(canvas.Right, canvas.Bottom);

					if (this.Image != null)
					{
						EndDrawShape();
						EndRangeSelection();

						Image newImage = new Bitmap(this.Image, size);

						using (Graphics g = Graphics.FromImage(newImage))
						{
							g.DrawImage(this.Image, 0, 0, size.Width, size.Height);
						}

						AddHistory(newImage);
					}
				}
			}
		}

		void rotateRightToolStripButton_Click(object sender, EventArgs e)
		{
			TranslateImage(RotateFlipType.Rotate90FlipNone);
		}

		void rotateLeftToolStripButton_Click(object sender, EventArgs e)
		{
			TranslateImage(RotateFlipType.Rotate270FlipNone);
		}

		private Point resizeThumbOffset;

		internal void InitCulture()
		{
			this.Text = LangResource.image_editor + " - " + ProductInfo.ProductName;

			// file
			fileToolStripMenuItem.Text = LangResource.btn_file;
			captureARegionOfScreenToolStripMenuItem.Text = LangResource.btn_capture_free_region;
			captureWindowToolStripMenuItem.Text = LangResource.btn_capture_window;
			captureCurrentFullScreenToolStripMenuItem.Text = LangResource.btn_capture_current_full_screen;
			newToolStripMenuItem.Text = LangResource.btn_new;
			openImageToolStripMenuItem.Text = LangResource.btn_open;
			saveImageToolStripMenuItem.Text = LangResource.btn_save;
			saveAsToolStripMenuItem.Text = LangResource.btn_save_as;
			closeToolStripMenuItem.Text = LangResource.btn_close;

			// edit
			editToolStripMenuItem.Text = LangResource.btn_edit;
			undoToolStripMenuItem.Text = LangResource.btn_undo;
			redoToolStripMenuItem.Text = LangResource.btn_redo;
			copyToolStripMenuItem.Text = LangResource.btn_copy;
			cutToolStripMenuItem.Text = LangResource.btn_cut;
			pasteToolStripMenuItem.Text = LangResource.btn_paste;
			selectToolStripMenuItem.Text = LangResource.btn_select;
			selectAllToolStripMenuItem.Text = LangResource.btn_select_all;
			deselectToolStripMenuItem.Text = LangResource.btn_deselect;
			clipSelectedImageToolStripMenuItem.Text = LangResource.btn_clip_image;

			// view
			viewToolStripMenuItem.Text = LangResource.btn_view;
			viewToolStripMenuItem.ToolTipText = LangResource.view;
			toolbarToolStripMenuItem.Text = LangResource.btn_toolbar;
			statusBarToolStripMenuItem.Text = LangResource.btn_status_Bar;

			// image
			imageToolStripMenuItem.Text = LangResource.btn_image;
			drawRectangleToolStripMenuItem.Text = LangResource.btn_draw_rectangle;
			drawLineToolStripMenuItem.Text = LangResource.btn_draw_line;
			drawArrowToolStripMenuItem.Text = LangResource.btn_draw_arrow;
			drawEllipseToolStripMenuItem.Text = LangResource.btn_draw_ellipse;
			addNumberToolStripMenuItem.Text = LangResource.btn_add_number;
			resetNumberToolStripMenuItem.Text = LangResource.btn_reset_number;
			shadowToolStripMenuItem.Text = LangResource.btn_shadow;
			shadowToolStripButton.ToolTipText = LangResource.show_shadow;
			resizeToolStripMenuItem.Text = LangResource.btn_resize_image;
			resizeToolStripMenuItem.ToolTipText = LangResource.resize_image;
			scaleToolStripMenuItem.Text = LangResource.btn_scale;
			scaleToolStripMenuItem.ToolTipText = LangResource.scale;
			rotateRightToolStripMenuItem.Text = LangResource.rotate_90cw;
			rotateRightToolStripMenuItem.ToolTipText = LangResource.rotate_90cw;
			rotateLeftToolStripMenuItem.Text = LangResource.rotate_90ccw;
			rotateLeftToolStripMenuItem.ToolTipText = LangResource.rotate_90ccw;
			
			// tools
			toolsToolStripMenuItem.Text = LangResource.btn_tools;
			settingsToolStripMenuItem.Text = LangResource.btn_settings;

			// help
			helpToolStripMenuItem.Text = LangResource.btn_help;
			aboutToolStripMenuItem.Text = LangResource.btn_about;
			homepageToolStripMenuItem.Text = LangResource.btn_homepage;

			// toolbar
			captureToolStripButton.ToolTipText = LangResource.capture_tool;
			captureARegionOfScreenToolStripMenuItem.ToolTipText = LangResource.capture_free_region;
			captureWindowToolStripButton.ToolTipText = LangResource.select_window;
			captureFullScreenToolStripButton.ToolTipText = LangResource.capture_full_screen;
			resizeToolStripButton.Text = LangResource.btn_resize_image;
			resizeToolStripButton.ToolTipText = LangResource.resize_image;
			scaleToolStripButton.Text = LangResource.btn_scale;
			scaleToolStripButton.ToolTipText = LangResource.scale;
			rotateRightToolStripButton.Text = LangResource.rotate_90cw;
			rotateRightToolStripButton.ToolTipText = LangResource.rotate_90cw;
			rotateLeftToolStripButton.Text = LangResource.rotate_90ccw;
			rotateLeftToolStripButton.ToolTipText = LangResource.rotate_90ccw;
	

			newToolStripButton.ToolTipText = LangResource.new_image;
			openToolStripButton.ToolTipText = LangResource.open_image;
			saveToolStripButton.ToolTipText = LangResource.save_image;
			undoToolStripButton.ToolTipText = LangResource.undo;
			redoToolStripButton.ToolTipText = LangResource.redo;
			cutToolStripButton.ToolTipText = LangResource.cut;
			copyToolStripButton.ToolTipText = LangResource.copy;
			pasteToolStripButton.ToolTipText = LangResource.paste;
			selectToolStripButton.ToolTipText = LangResource.select_region;
			resetNumberToolStripButton.ToolTipText = LangResource.reset_number;
			
		}

		private int dashOffset = 0;

		private Operation operation;
		private Rectangle[] hands { get; set; }

		public string CurrentFilename { get; set; }

		private Image partialImage { get; set; }

		private Rectangle selectionRange;

		public Rectangle SelectionRange
		{
			get { return selectionRange; }
			set { selectionRange = value; }
		}

		private Point rangeMovingOffset;

		#region Image Management

		private int historyIndex = 0;
		private List<Image> history = new List<Image>();

		public Image Image
		{
			get
			{
				return history == null || history.Count == 0 ? null : history[historyIndex];
			}
			set
			{
				ReleaseAllImages();
				historyIndex = -1;
				AddHistory(value);
			}
		}

		private Control resizeThumb = new Control() { Size = new Size(6, 6), 
			BackColor = SystemColors.Highlight,
			Cursor = Cursors.SizeNWSE,
		};

		private Size canvasSize;
		public Size CanvasSize
		{
			get
			{
				return canvasSize;
			}
			set
			{
				if (canvasSize != value)
				{
					canvasSize = value;

					canvas.Size = canvasSize;
					canvas.Invalidate();

					resizeThumb.Location = new Point(canvas.Right, canvas.Bottom);
				}
			}
		}

		private void AddHistory(Image image)
		{
			while (history.Count > historyIndex + 1)
			{
				history[history.Count - 1].Dispose();
				history.RemoveAt(history.Count - 1);
			}

			historyIndex++;

			Bitmap copiedImage = new Bitmap(image, image.Size);
			using (Graphics g = Graphics.FromImage(copiedImage))
			{
				g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
			}
			history.Add(copiedImage);

			FitCanvasSize();

			canvas.Invalidate();
			UpdateMenus();
		}

		public void ReleaseAllImages()
		{
			foreach (Image image in history)
			{
				if (image != null) image.Dispose();
			}

			history.Clear();
			historyIndex = -1;
		}

		protected override void OnLoad(EventArgs e)
		{
			if (this.Image == null)
			{
				NewImage();
			}
			else
			{
				this.fileName = LangResource.untitled;
			}
		}

		private void FitCanvasSize()
		{
			if (Image != null)
			{
				//try
				//{
					CanvasSize = Image.Size;
				//}
				//catch
				//{
				//  CanvasSize = new Size(canvasPanel.ClientRectangle.Width,
				//    canvasPanel.ClientRectangle.Height);
				//}
					resizeThumb.Location = new Point(canvas.Right, canvas.Bottom);
			}
			else
			{
				CanvasSize = new Size(canvasPanel.ClientRectangle.Width - resizeThumb.Width - 1,
					canvasPanel.ClientRectangle.Height - resizeThumb.Height - 1);
			}

			canvas.Size = CanvasSize;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (CloseImage())
			{
				ConfigurationManager.Instance.SaveCurrentUserConfiguration();
			}
			else
			{
				e.Cancel = true;
			}
		}
		#endregion

		private void EndDrawShape()
		{
			if (shapeType != ShapeType.None)
			{
				if (startPoint != endPoint)
				{
					CommitCurrentImage();
				}

				canvas.Cursor = Cursors.Default;

				shapeType = ShapeType.None;

				rectToolStripButton.Checked = false;
				lineShapeToolStripItem.Checked = false;
				arrowShapeToolStripItem.Checked = false;
				ellipseShapeToolStripItem.Checked = false;
				numberShapeToolStripItem.Checked = false;

				canvas.Invalidate();
				UpdateMenus();
				Cursor.Show();
			}
		}

		/// <summary>
		/// Save and put current drawing image to history
		/// </summary>
		private void CommitCurrentImage()
		{
			Image newImage = new Bitmap(canvas.Width, canvas.Height);

			using (Graphics g = Graphics.FromImage(newImage))
			{
				DrawCanvas(g, new Rectangle(new Point(0, 0), newImage.Size), false);
			}

			AddHistory(newImage);
		}

		public void StartSelectRange()
		{
			canvas.Cursor = Cursors.Cross;
			operation = Operation.SelectRange;
			endPoint = startPoint = Point.Empty;
			selectionRange = Rectangle.Empty;
			dashTimer.Enabled = true;
		}

		public void EndRangeSelection()
		{
			if (operation == Operation.SelectRange)
			{
				if ((selectionRange.Width > 0 || selectionRange.Height > 0 )
					&& partialImage != null)
				{
					Bitmap bmp = new Bitmap(canvasSize.Width, canvas.Height);
					using (Graphics g = Graphics.FromImage(bmp))
					{
						DrawCanvas(g, new Rectangle(0, 0, canvasSize.Width, canvasSize.Height), false);
					}

					AddHistory(bmp);
				}

				if (partialImage != null)
				{
					partialImage.Dispose();
					partialImage = null;
				}

				operation = Operation.None;
				
				selectToolStripButton.Checked = false;
				dashTimer.Enabled = false;
				canvas.Cursor = Cursors.Default;
				canvas.Invalidate();
			}
		}

		public void SelectAll()
		{
			operation = Operation.SelectRange;
			selectionRange = new Rectangle(0, 0, canvas.Width-1, canvas.Height-1); 
			canvas.Invalidate();
			canvas.Cursor = Cursors.SizeAll;
			dashTimer.Enabled = true;
		}

		private void UpdateMenus()
		{
			undoToolStripMenuItem.Enabled = undoToolStripButton.Enabled = historyIndex > 0;
			redoToolStripMenuItem.Enabled = redoToolStripButton.Enabled = historyIndex < history.Count - 1;

			deselectToolStripMenuItem.Enabled =
				operation == Operation.SelectRange
				&& !selectionRange.IsEmpty;

			clipSelectedImageToolStripMenuItem.Enabled =
				operation == Operation.SelectRange
				&& selectionRange.Width > 0 && selectionRange.Height > 0;

			saveImageToolStripMenuItem.Enabled =
				saveToolStripButton.Enabled =
				saveAsToolStripMenuItem.Enabled =
				this.Image != null;
		}

		#region Keyboard
		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case (Keys.Control | Keys.C):
					copyToolStripButton.PerformClick();
					break;

				case Keys.Control | Keys.X:
					cutToolStripButton.PerformClick();
					break;

				case Keys.Control | Keys.V:
					pasteToolStripButton.PerformClick();
					break;

				case Keys.Escape:
					if (shapeType == ShapeType.Number)
					{
						shapeType = ShapeType.Rect;
						startPoint = endPoint = Point.Empty;
					}
					EndDrawShape();
					EndRangeSelection();
					break;

				case Keys.Control | Keys.Z:
					undoToolStripButton.PerformClick();
					break;

				case Keys.Control | Keys.Y:
					redoToolStripButton.PerformClick();
					break;

				case Keys.Control | Keys.D1:
					captureToolStripButton.PerformClick();
					break;

				case Keys.F5:
					rectToolStripButton.PerformClick();
					break;

				case Keys.Control | Keys.S:
					saveToolStripButton.PerformClick();
					break;

				case Keys.Control | Keys.O:
					openToolStripButton.PerformClick();
					break;

				case Keys.D1:
					if (shapeType == ShapeType.Number)
					{
						numberShapeToolStripItem.CurrentNumber = 1;
					}
					else
					{
						rectToolStripButton.Checked = true;
					}
					break;

				case Keys.D2:
					if (shapeType == ShapeType.Number)
						numberShapeToolStripItem.CurrentNumber = 2;
					else
						ellipseShapeToolStripItem.Checked = true;
					break;

				case Keys.D3:
					if (shapeType == ShapeType.Number)
						numberShapeToolStripItem.CurrentNumber = 3;
					else
						lineShapeToolStripItem.Checked = true;
					break;

				case Keys.D4:
					if (shapeType == ShapeType.Number)
						numberShapeToolStripItem.CurrentNumber = 4;
					else
						arrowShapeToolStripItem.Checked = true;
					break;

				case Keys.Left:
					if (!this.selectionRange.IsEmpty)
					{
						this.selectionRange.X--;
						canvas.Invalidate();
						ShowCoordinateInfo(this.selectionRange.Location);
					}
					break;

				default:
					if (shapeType == ShapeType.Number && e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
					{
						numberShapeToolStripItem.CurrentNumber = e.KeyCode - Keys.D0;
					}
					base.OnKeyDown(e);
					break;
			}
		}

		#endregion

		#region Mouse
		private bool isMouseDown = false;

		void OnCanvasMouseMove(MouseEventArgs e)
		{
			if (operation == Operation.SelectRange)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (ShortcutTextbox.IsKeyDown(Win32.VKey.VK_SHIFT))
					{
						endPoint = Point.Round(GraphicsToolkit.GetStraightLinePoint(startPoint, e.Location));
					}
					else
					{
						endPoint = e.Location;
					}

					int sx = Math.Min(startPoint.X, endPoint.X);
					int sy = Math.Min(startPoint.Y, endPoint.Y);
					int ex = Math.Max(startPoint.X, endPoint.X);
					int ey = Math.Max(startPoint.Y, endPoint.Y);

					if (sx < 0) sx = 0;
					if (sy < 0) sy = 0;
					if (ex > canvasSize.Width - 1) ex = canvasSize.Width - 1;
					if (ey > canvasSize.Height - 1) ey = canvasSize.Height - 1;

					SelectionRange = new Rectangle(sx, sy, ex - sx, ey - sy);
					ShowCoordinateInfo(startPoint, endPoint);

					canvas.Invalidate();
				}
				else
				{
					if (GraphicsToolkit.PointInRect(SelectionRange, e.Location))
					{
						canvas.Cursor = Cursors.SizeAll;
						ShowCoordinateInfo(this.selectionRange);
					}
					else
					{
						canvas.Cursor = Cursors.Cross;
						ShowCoordinateInfo(e.Location);
					}
				}
			}
			else if (operation == Operation.MoveRange)
			{
				int offx = e.X - selectionRange.X - rangeMovingOffset.X;
				int offy = e.Y - selectionRange.Y - rangeMovingOffset.Y;

				if (selectionRange.X + offx < 0) offx = -selectionRange.X;
				if (selectionRange.Y + offy < 0) offy = -selectionRange.Y;
				if (selectionRange.Right + offx > canvasSize.Width - 1)
					offx = canvasSize.Width - selectionRange.Right - 1;
				if (selectionRange.Bottom + offy > canvasSize.Height - 1)
					offy = canvasSize.Height - selectionRange.Bottom - 1;

				selectionRange.Offset(offx, offy);
				canvas.Invalidate();

				ShowCoordinateInfo(selectionRange.Location, new Point(selectionRange.Right, selectionRange.Bottom));
			}
			else if (operation == Operation.DrawShape)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (shapeType != ShapeType.None)
					{
						if (shapeType == ShapeType.Rect)
						{
							startPoint = new Point(Math.Min(e.X, basePoint.X), Math.Min(e.Y, basePoint.Y));
							endPoint = new Point(Math.Max(e.X, basePoint.X), Math.Max(e.Y, basePoint.Y));
						}
						else
						{
							if (ShortcutTextbox.IsKeyDown(Win32.VKey.VK_SHIFT))
							{
								endPoint = Point.Round(GraphicsToolkit.GetStraightLinePoint(startPoint, e.Location));
							}
							else
							{
								endPoint = e.Location;
							}
						}
						ShowCoordinateInfo(startPoint, endPoint);
						canvas.Invalidate();
					}
				}
				else if (shapeType == ShapeType.Number)
				{
					endPoint = e.Location;
					canvas.Invalidate();

					ShowCoordinateInfo(startPoint);
				}
				else
				{
					ShowCoordinateInfo(e.Location);
				}
			}
			else if (this.operation == Operation.AddText)
			{
				ShowCoordinateInfo(e.Location);
			}
			else
			{
				ShowCoordinateInfo(e.Location);
			}
		}

		void OnCanvasMouseDown(MouseEventArgs e)
		{
			if (operation == Operation.SelectRange)
			{
				if (GraphicsToolkit.PointInRect(SelectionRange, e.Location))
				{
					operation = Operation.MoveRange;
					rangeMovingOffset = new Point(e.X - SelectionRange.X, e.Y - SelectionRange.Y);
				}
				else
				{
					EndRangeSelection();
					StartSelectRange();

					endPoint = startPoint = e.Location;
				}
			}
			else if (operation == Operation.DrawShape)
			{
				isMouseDown = true;
				dashTimer.Enabled = false;
				endPoint = startPoint = basePoint = e.Location;
				canvas.Invalidate();
			}
		}

		void OnCanvasMouseUp(MouseEventArgs e)
		{
			isMouseDown = false;

			if (operation == Operation.SelectRange)
			{
				//dashTimer.Enabled = true;
			}
			else if (operation == Operation.MoveRange)
			{
				operation = Operation.SelectRange;
			}
			else if (operation == Operation.DrawShape)
			{
				if (shapeType != ShapeType.None && shapeType != ShapeType.Number)
				{
					EndDrawShape();
				}

				if (shapeType == ShapeType.Number)
				{
					CommitCurrentImage();
					numberShapeToolStripItem.CurrentNumber++;

					TipsForm.CheckAndShowTip(UserConfigKey.Tips_ChangeNumber);

					canvas.Invalidate();
				}
			}
			else if (operation == Operation.AddText)
			{
				AddText(e.Location);
			}

			UpdateMenus();
		}

		void OnCanvasMouseDoubleClick(MouseEventArgs e)
		{
			if (GraphicsToolkit.PointInRect(selectionRange, e.Location))
			{
				operation = Operation.SelectRange;
				CutSelectedImageRange();
			}
		}

		private bool isMouseEnter = false;
		void OnCanvasMouseEnter(EventArgs e)
		{
			isMouseEnter = true;
		}
		void OnCanvasMouseLeave(EventArgs e)
		{
			isMouseEnter = false;
			canvas.Invalidate();
		}
		#endregion

		private void CutSelectedImageRange()
		{
			AddHistory(GetPartialRangeImage(this.selectionRange));
			selectionRange = Rectangle.Empty;

			EndRangeSelection();
			UpdateMenus();
		}

		private Image GetPartialRangeImage(Rectangle rect)
		{
			Bitmap bmp;
			if (Image == null)
			{
				bmp = new Bitmap(rect.Width, rect.Height);
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.Clear(Color.White);

					if (this.partialImage != null)
					{
						g.DrawImage(this.partialImage, 0, 0);
					}
				}
			}
			else
			{
				bmp = new Bitmap(Image, rect.Size);
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.DrawImage(Image, new Rectangle(0, 0, rect.Width + 1, rect.Height + 1),
						new Rectangle(rect.X, rect.Y,
							rect.Width + 1, rect.Height + 1), GraphicsUnit.Pixel);

					if (this.partialImage != null)
					{
						g.DrawImage(this.partialImage, 0, 0);
					}
				}
			}
			return bmp;
		}

		#region Paint
		private static readonly Font numberFont = new Font("Arias", 10f);
		private static readonly int numberSize = 22;
		
		void DrawCanvas(Graphics g, Rectangle bounds, bool drawDesigning)
		{
			g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), CanvasSize));

			Image image = Image;
			if (image != null)
			{
				g.DrawImage(image, 0, 0, canvasSize.Width,canvasSize.Height);
			}

			if (operation == Operation.SelectRange
				|| operation == Operation.MoveRange)
			{
				if (partialImage != null)
				{
					g.DrawImage(partialImage, selectionRange);
				}

				if (drawDesigning)
				{
					using (Pen p = new Pen(Color.Gray))
					{
						p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
						p.DashOffset = dashOffset;

						g.DrawRectangle(p, SelectionRange);
					}
				}
			}
			else if (operation == Operation.DrawShape)
			{
				if (shapeType != ShapeType.None
				&& (shapeType == ShapeType.Number || (startPoint != endPoint && !startPoint.IsEmpty && !endPoint.IsEmpty)))
				{
					Rectangle rect = new Rectangle(startPoint, new Size(endPoint.X - startPoint.X,
						endPoint.Y - startPoint.Y));

					Color color;
					switch (shapeType)
					{
						default:
						case ShapeType.Rect:
							color = rectToolStripButton.CurrentColor;
							break;

						case ShapeType.Line:
							color = lineShapeToolStripItem.CurrentColor;
							break;

						case ShapeType.Arrow:
							color = arrowShapeToolStripItem.CurrentColor;
							break;
					
						case ShapeType.Ellipse:
							color = ellipseShapeToolStripItem.CurrentColor;
							break;

						case ShapeType.Number:
							color = numberShapeToolStripItem.CurrentColor;
							break;
					}

					using (Pen p = new Pen(color, 2f))
					{
						using (Pen shadowPen = new Pen(Color.FromArgb(20, Color.Black), 2f))
						{
							bool drawShadow = shadowToolStripButton.Checked;

							switch (shapeType)
							{
								default:
								case ShapeType.Rect:
									if (drawShadow)
									{
										Rectangle shadowRect = rect;
										shadowRect.Offset(4, 4);
										g.DrawRectangle(shadowPen, shadowRect);
									}
									g.DrawRectangle(p, rect);
									break;

								case ShapeType.Ellipse:
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

									if (drawShadow)
									{
										Rectangle shadowRect = rect;
										shadowRect.Offset(4, 4);
										g.DrawEllipse(shadowPen, shadowRect);
									}
									g.DrawEllipse(p, rect);

									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
									break;

								case ShapeType.Line:
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									if (drawShadow)
									{
										Point shadowStart = startPoint;
										shadowStart.Offset(4, 4);
										Point shadowEnd = endPoint;
										shadowEnd.Offset(4, 4);
										g.DrawLine(shadowPen, shadowStart, shadowEnd);
									}
									g.DrawLine(p, startPoint, endPoint);
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
									break;

								case ShapeType.Arrow:
									p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									if (drawShadow)
									{
										shadowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);

										Point shadowStart = startPoint;
										shadowStart.Offset(4, 4);
										Point shadowEnd = endPoint;
										shadowEnd.Offset(4, 4);
										g.DrawLine(shadowPen, shadowStart, shadowEnd);
									}
									g.DrawLine(p, startPoint, endPoint);
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
									break;

								case ShapeType.Number:
									if (!endPoint.IsEmpty && isMouseEnter)
									{
										Rectangle numberRect = new Rectangle(endPoint.X - numberSize / 2, endPoint.Y - numberSize / 2,
											numberSize, numberSize);

										if (isMouseDown)
										{
											numberRect.Offset(6, 6);
										}
										else if (drawShadow)
										{
											numberRect.Offset(3, 3);
											DrawNumber(g, shadowPen.Color, numberShapeToolStripItem.CurrentNumber, numberRect);
										}

										numberRect.Offset(-3, -3);
										DrawNumber(g, color, numberShapeToolStripItem.CurrentNumber, numberRect);
									}
									break;
							}
						}
					}
				}
			}
		}

		private void DrawNumber(Graphics g, Color color, int number, Rectangle rect)
		{
			using (Pen ep = new Pen(color, 1.5f))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.DrawEllipse(ep, rect);
				g.SmoothingMode = SmoothingMode.Default;
			}

			using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
			{
				sf.LineAlignment = StringAlignment.Center;
				sf.Alignment = StringAlignment.Center;

				string str = number.ToString();
				SizeF s = g.MeasureString(str, numberFont, rect.Width, sf);
				using (Brush b = new SolidBrush(color))
				{
					g.DrawString(str, numberFont, b, rect, sf);
				}
			}
		}
		#endregion

		private void ShowCoordinateInfo(Point p)
		{
			Log("{0}, {1}", p.X, p.Y);
		}

		private void ShowCoordinateInfo(Rectangle rect)
		{
			ShowCoordinateInfo(rect.Location, new Point(rect.Right, rect.Bottom));
		}

		private void ShowCoordinateInfo(Point sp, Point ep)
		{
			Log("{0}, {1} - {2}, {3} [{4} x {5}]", sp.X, sp.Y, ep.X, ep.Y, ep.X - sp.X, ep.Y - sp.Y);
		}

		public void Log(string format, params object[] args)
		{
			Log(string.Format(format, args));
		}

		public void Log(string msg)
		{
			statusToolStripStatusLabel.Text = msg;
		}

		private Point basePoint;
		private Point startPoint;
		private Point endPoint;

		private ShapeType shapeType;

		enum Operation
		{
			None,
			SelectRange,
			MoveRange,
			DrawShape,
			AddText,
		}

		enum ShapeType
		{
			None,
			Rect,
			Line,
			Arrow,
			Ellipse,
			Number,
		}
	}



}
