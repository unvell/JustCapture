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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace unvell.JustCapture.Editor
{
	public partial class ScaleImageForm : Form
	{
		public ScaleImageForm()
		{
			InitializeComponent();

			this.Text = LangResource.scale_image;

			this.sizeGroup.Text = LangResource.size;
			this.chkLockAspect.Text = LangResource.btn_lock_aspect_ratio;
			this.labUnit.Text = LangResource.lab_unit;
			this.labWidth.Text = LangResource.lab_width;
			this.labHeight.Text = LangResource.lab_height;

			this.btnOK.Text = LangResource.btn_ok;
			this.btnCancel.Text = LangResource.btn_cancel;

			numWidth.GotFocus += numWidth_GotFocus;
			numHeight.GotFocus += numWidth_GotFocus;

			numWidth.ValueChanged += (s, e) =>
			{
				if (isUpdating) return;

				if (chkLockAspect.Checked)
				{
					isUpdating = true;

					if (unitCombo.SelectedIndex == 0)
					{
						int value = (int)Math.Round((float)numWidth.Value * this.aspect);
						if (value < numHeight.Minimum) value = (int)numHeight.Minimum;
						if (value > numHeight.Maximum) value = (int)numHeight.Maximum;
						numHeight.Value = value;
					}
					else if (unitCombo.SelectedIndex == 1)
					{
						numHeight.Value = numWidth.Value;
					}

					isUpdating = false;
				}
			};
			numHeight.ValueChanged += (s, e) =>
			{
				if (isUpdating) return;

				if (chkLockAspect.Checked)
				{
					isUpdating = true;

					if (unitCombo.SelectedIndex == 0)
					{
						int value = (int)Math.Round((float)numHeight.Value * this.aspect);
						if (value < numWidth.Minimum) value = (int)numWidth.Minimum;
						if (value > numWidth.Maximum) value = (int)numWidth.Maximum;
						numWidth.Value = value;
					}
					else if (unitCombo.SelectedIndex == 1)
					{
						numWidth.Value = numHeight.Value;
					}

					isUpdating = false;
				}
			};

			unitCombo.Items.Add(LangResource.pixel);
			unitCombo.Items.Add(LangResource.percent);

			unitCombo.SelectedIndexChanged += (s, e) =>
			{
				if (isUpdating) return;

				if (unitCombo.SelectedIndex == 1)
				{
					isUpdating = true;
					numWidth.Value = (int)((float)numWidth.Value / ImageSize.Width * 100f);
					numHeight.Value = (int)((float)numHeight.Value / ImageSize.Height * 100f);
					isUpdating = false;
				}
				else if (unitCombo.SelectedIndex == 0)
				{
					isUpdating = true;
					numWidth.Value = (int)((float)numWidth.Value * ImageSize.Width / 100f);
					numHeight.Value = (int)((float)numHeight.Value * ImageSize.Height / 100f);
					isUpdating = false;
				}
			};

		}

		private bool isUpdating = false;

		void numWidth_GotFocus(object sender, EventArgs e)
		{
			var num = ((NumericUpDown)sender);
			num.Select(0, num.Text.Length);
		}

		private float aspect;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			numWidth.Value = ImageSize.Width;
			numHeight.Value = ImageSize.Height;

			aspect = (float)ImageSize.Height / ImageSize.Width;

			chkLockAspect.Checked = ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.IEF_LockAspect, true);

			int scaleUnitIndex = ConfigurationManager.Instance.GetCurrentUserSettingIntValue(UserConfigKey.IEF_ScaleSizeUnit, 0);
			if (scaleUnitIndex == 0)
			{
				isUpdating = true;
				unitCombo.SelectedIndex = scaleUnitIndex;
				isUpdating = false;
			}
			else
			{
				unitCombo.SelectedIndex = scaleUnitIndex;
			}
		}

		public Size ImageSize { get; set; }

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (unitCombo.SelectedIndex == 1)
			{
				ImageSize = new Size((int)Math.Round((float)numWidth.Value * ImageSize.Width / 100f),
					(int)(Math.Round((float)numHeight.Value * ImageSize.Height / 100f)));
			}
			else //if (unitCombo.SelectedIndex == 0)
			{
				ImageSize = new Size((int)numWidth.Value, (int)numHeight.Value);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.IEF_LockAspect, chkLockAspect.Checked);
			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.IEF_ScaleSizeUnit, unitCombo.SelectedIndex.ToString());

			ConfigurationManager.Instance.SaveCurrentUserConfiguration();
		}
	}
}
