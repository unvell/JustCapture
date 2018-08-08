using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using unvell.JustCapture.Toolkits;
using unvell.Common;

namespace unvell.JustCapture.Editor
{
	internal partial class EditorForm
	{
		public void CaptureFreeRegion()
		{
			Thread.Sleep(200);
			DoSnapshot(WorkMode.OnlyCapture, CaptureMode.FreeRegion);
		}

		public void CaptureWindow()
		{
			Thread.Sleep(200);
			DoSnapshot(WorkMode.OnlyCapture, CaptureMode.SelectWindow);
		}

		public void CaptureFullWindow()
		{
			Thread.Sleep(200);
			DoSnapshot(WorkMode.AutoCapture, CaptureMode.FullScreen);
		}

		public void DoSnapshot(WorkMode mode, CaptureMode method)
		{
			EndDrawShape();
			EndRangeSelection();

			Opacity = 0;
			Thread.Sleep(200);

			CaptureForm.StartCapture(mode, method, (image) =>
			{
				if (image != null)
				{
					Bitmap copiedImage = new Bitmap(image);
					using (Graphics g = Graphics.FromImage(copiedImage))
					{
						g.DrawImageUnscaled(image, 0, 0);
					}

					AddHistory(copiedImage);
				}

				Opacity = 1;

				Activate();
				Focus();
			});
		}

		public void StartDrawRect()
		{
			if (rectToolStripButton.Checked)
			{
				EndRangeSelection();
				operation = Operation.DrawShape;

				lineShapeToolStripItem.Checked = false;
				arrowShapeToolStripItem.Checked = false;
				ellipseShapeToolStripItem.Checked = false;
				addNumberToolStripMenuItem.Checked = false;

				shapeType = ShapeType.Rect;
				startPoint = endPoint = Point.Empty;
				canvas.Cursor = Cursors.Cross;
			}
			else
			{
				if (shapeType == ShapeType.Rect) EndDrawShape();
			}
		}

		public void StartDrawLine()
		{
			if (lineShapeToolStripItem.Checked)
			{
				EndRangeSelection();
				operation = Operation.DrawShape;

				rectToolStripButton.Checked = false;
				arrowShapeToolStripItem.Checked = false;
				ellipseShapeToolStripItem.Checked = false;
				addNumberToolStripMenuItem.Checked = false;

				shapeType = ShapeType.Line;
				startPoint = endPoint = Point.Empty;
				canvas.Cursor = Cursors.Cross;
			}
			else
			{
				if (shapeType == ShapeType.Line) EndDrawShape();
			}
		}

		public void StartDrawArrow()
		{
			if (arrowShapeToolStripItem.Checked)
			{
				EndRangeSelection();
				operation = Operation.DrawShape;

				rectToolStripButton.Checked = false;
				lineShapeToolStripItem.Checked = false;
				ellipseShapeToolStripItem.Checked = false;
				addNumberToolStripMenuItem.Checked = false;

				shapeType = ShapeType.Arrow;
				startPoint = endPoint = Point.Empty;
				canvas.Cursor = Cursors.Cross;
			}
			else
			{
				if (shapeType == ShapeType.Arrow) EndDrawShape();
			}
		}

		public void StartDrawEllipse()
		{
			if (ellipseShapeToolStripItem.Checked)
			{
				EndRangeSelection();
				operation = Operation.DrawShape;

				rectToolStripButton.Checked = false;
				lineShapeToolStripItem.Checked = false;
				arrowShapeToolStripItem.Checked = false;
				addNumberToolStripMenuItem.Checked = false;

				shapeType = ShapeType.Ellipse;
				startPoint = endPoint = Point.Empty;
				canvas.Cursor = Cursors.Cross;
			}
			else
			{
				if (shapeType == ShapeType.Ellipse) EndDrawShape();
			}
		}

		private Cursor addNumberCursor = null;

		public void CheckToAddNumber()
		{
			if (numberShapeToolStripItem.Checked)
			{
				EndRangeSelection();
				operation = Operation.DrawShape;

				rectToolStripButton.Checked = false;
				lineShapeToolStripItem.Checked = false;
				arrowShapeToolStripItem.Checked = false;
				ellipseShapeToolStripItem.Checked = false;

				shapeType = ShapeType.Number;
				startPoint = endPoint = Point.Empty;
				canvas.Cursor = addNumberCursor == null ? Cursors.Cross : addNumberCursor;
			}
			else
			{
				if (shapeType == ShapeType.Number) EndDrawShape();
			}
		}

		private int lastSavedPoint = 0;

		public bool CloseImage()
		{
			if (lastSavedPoint != this.historyIndex)
			{
				DialogResult dr = MessageBox.Show(this, string.Format(LangResource.save_changes_confirm, this.fileName),
					LangResource.save_image, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				if (dr == System.Windows.Forms.DialogResult.Yes)
				{
					if (!SaveImage(this.currentFile))
					{
						return false;
					}
				}
				else if (dr == System.Windows.Forms.DialogResult.Cancel)
				{
					return false;
				}
			}

			ReleaseAllImages();
			this.lastSavedPoint = this.historyIndex;
			//this.Image = null;
			this.canvas.Invalidate();

			return true;
		}

		public void OpenImage()
		{
			if (CloseImage())
			{
				using (OpenFileDialog ofd = new OpenFileDialog())
				{
					ofd.FileName = CurrentFilename;
					ofd.Filter = LangResource.all_supported_files +
						"(*.png;*.jpg;*.gif;*.tif;*.jpeg;*.tiff)|*.png;*.jpg;*.gif;*.tif;*.jpeg;*.tiff|PNG Image(*.png)|*.png|JPEG Image(*.jpg)|*.jpg|GIF Image(*.gif)|*.gif|TIFF Image(*.tif;*.tff)|*.tif;*.tiff";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						this.operation = Operation.None;
						this.selectionRange = Rectangle.Empty;

						Image image;
						try
						{
							image = Image.FromFile(ofd.FileName);
						}
						catch (Exception ex)
						{
							image = null;
							MessageBox.Show(LangResource.incorrect_format + "\r\n\r\n" + "Error Details: " + ex.Message);
						}

						AddHistory(image);
						this.lastSavedPoint = this.historyIndex;

						this.currentFile = new FileInfo(ofd.FileName);
						this.fileName = this.currentFile.Name;

						UpdateFormTitle();
					}
				}
			}
		}

		public bool SaveImage(FileInfo file)
		{
			EndDrawShape();
			EndRangeSelection();

			if (Image != null)
			{
				FileInfo savedFile = FileToolkit.SaveImage(Image, file);

				if (savedFile == null)
				{
					return false;
				}
				else
				{
					this.lastSavedPoint = this.historyIndex;
					this.currentFile = savedFile;
					this.fileName = this.currentFile.Name;
					UpdateFormTitle();
				}
			}

			return true;
		}

		private FileInfo currentFile;
		private string fileName;

		private void UpdateFormTitle()
		{
			this.Text = string.IsNullOrEmpty(this.fileName) ?
				(string.Format("{0} - {1}", LangResource.image_editor, Application.ProductName))
				: (string.Format("{0} - {1} - {2}", this.fileName, LangResource.image_editor, Application.ProductName));
		}

		public void NewImage()
		{
			if (CloseImage())
			{
				this.history.Add(null);
				this.historyIndex = 0;
				this.lastSavedPoint = 0;

				this.numberShapeToolStripItem.CurrentNumber = 1;

				SelectionRange = Rectangle.Empty;
				if (partialImage != null)
				{
					try
					{
						partialImage.Dispose();
					}
					catch { }
					partialImage = null;
				}

				FitCanvasSize();

				canvas.Invalidate();
				UpdateMenus();

				this.fileName = LangResource.untitled;
				UpdateFormTitle();
			}
		}

		public void Undo()
		{
			if (historyIndex > 0)
			{
				historyIndex--;
				canvas.Invalidate();
			}

			FitCanvasSize();
			UpdateMenus();
		}
		public void Redo()
		{
			if (historyIndex < history.Count - 1)
			{
				historyIndex++;
				canvas.Invalidate();
			}

			FitCanvasSize();
			UpdateMenus();
		}

		public void CopyImage()
		{
			Image bmp;
			if (operation == Operation.SelectRange && !selectionRange.IsEmpty)
				bmp = GetPartialRangeImage(this.selectionRange);
			else
				bmp = Image;

			if (bmp != null) Clipboard.SetImage(bmp);
		}

		public void CutImage()
		{
			copyToolStripButton.PerformClick();

			Rectangle sourceRect;
			if (operation == Operation.SelectRange)
				sourceRect = new Rectangle(this.selectionRange.X, this.selectionRange.Y,
					this.selectionRange.Width + 1, this.selectionRange.Height + 1);
			else
				sourceRect = new Rectangle(0, 0, this.canvasSize.Width + 1, this.canvasSize.Height + 1);

			EndRangeSelection();

			Image bmp = new Bitmap(canvasSize.Width, canvasSize.Height);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				DrawCanvas(g, new Rectangle(new Point(0, 0), canvasSize), false);
				g.FillRectangle(Brushes.White, sourceRect);
			}

			AddHistory(bmp);
		}

		public void PasteImage()
		{
			if (Clipboard.ContainsImage())
			{
				EndDrawShape();
				EndRangeSelection();

				this.partialImage = Clipboard.GetImage();
				if (canvasSize.Width < partialImage.Width) canvasSize.Width = partialImage.Width;
				if (canvasSize.Height < partialImage.Height) canvasSize.Height = partialImage.Height;

				CanvasSize = canvasSize;
				canvas.Size = canvasSize;

				selectionRange = new Rectangle(0, 0, partialImage.Width, partialImage.Height);

				canvas.Invalidate();
				dashTimer.Enabled = true;
				operation = Operation.SelectRange;

				UpdateMenus();
			}
		}

		public void TranslateImage(RotateFlipType rotateFlipType)
		{
			if (partialImage != null )
			{
				partialImage.RotateFlip(rotateFlipType);

				if (!selectionRange.IsEmpty)
				{
					Point origin = new Point(selectionRange.X + selectionRange.Width / 2,
						selectionRange.Y + selectionRange.Height / 2);

					int oldWidth = selectionRange.Width;
					int oldHeight = selectionRange.Height;

					selectionRange.Width = oldHeight;
					selectionRange.Height = oldWidth;

					selectionRange.X += (oldWidth / 2 - selectionRange.Width / 2);
					selectionRange.Y += (oldHeight / 2 - selectionRange.Height / 2);
				}
			}
			else
			{
				Image newImage = GetPartialRangeImage(new Rectangle(0, 0, (int)this.canvas.Width, (int)this.canvas.Height));
				newImage.RotateFlip(rotateFlipType);

				FitCanvasSize();
				AddHistory(newImage);

				this.selectionRange = Rectangle.Empty;

				if (!selectionRange.IsEmpty)
				{
					this.selectionRange = Rectangle.Empty;
					
					// TODO:
					//if (rotateFlipType == RotateFlipType.Rotate90FlipNone)
					//{
					//	Rectangle r = new Rectangle();
					//	r.X = selectionRange.Y - selectionRange.Height;
					//	r.Y = selectionRange.X;
					//	r.Width = selectionRange.Height;
					//	r.Height = selectionRange.Width;

					//	this.selectionRange = r;
					//}
				}
			}
		}

		private void AddText(Point location)
		{
			using (EditTextForm etf = new EditTextForm())
			{
				if (etf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					if (!string.IsNullOrEmpty(etf.EditText))
					{
						using (Graphics g = Graphics.FromHwnd(canvas.Handle))
						{
							using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
							{
								SizeF size = g.MeasureString(etf.EditText, etf.TextFont, canvas.Size, sf);

								var textImage = new Bitmap((int)Math.Round(size.Width), (int)Math.Round(size.Height));

								using (Graphics g2 = Graphics.FromImage(textImage))
								{
									g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

									using (Brush b = new SolidBrush(textToolStripButton.CurrentColor))
									{
										g2.DrawString(etf.EditText, etf.TextFont, b, new Point(0, 0), sf);
									}
								}

								this.partialImage = textImage;
								this.selectionRange.Width = textImage.Width;
								this.selectionRange.Height = textImage.Height;
								this.selectionRange.Location = location;

								this.textToolStripButton.Checked = false;
								this.operation = Operation.SelectRange;
								this.dashTimer.Enabled = true;
								this.canvas.Invalidate();
							}
						}
					}
				}
			}
		}

	}
}
