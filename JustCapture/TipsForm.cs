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

namespace unvell.JustCapture
{
	internal partial class TipsForm : Form
	{
		public TipsForm()
		{
			InitializeComponent();

			Text = ProductName + " Tips";
			chkNoShowNext.Text = LangResource.btn_do_not_show_again;
			btnSkip.Text = LangResource.btn_skip;
			bottomPanel.Paint += new PaintEventHandler(bottomPanel_Paint);

			//animalUpdateTimer.Tick += (s, e) =>
			//{
			//  switch(TipKeys[currentIndex]){
			//    case UserConfigKey.Tips_ChangeNumber:
			//      if(imageBox.Image!=null){
			//        ImageAnimator.UpdateFrames(imageBox.Image);
			//      }
			//      break;
			//  }
			//};
		}

		//private Timer animalUpdateTimer = new Timer() { Interval = 100 };

		private int currentIndex = -1;

		protected override void OnLoad(EventArgs e)
		{
			NextTip();

			base.OnLoad(e);
		}

		private void SkipShownTips()
		{
			if (!IsOnlyFirstShow) return;

			// skip tips that displayed once
			while (currentIndex < TipKeys.Length-1)
			{
				if (!ConfigurationManager.Instance.IsCurrentUserSetting(TipKeys[currentIndex], false))
				{
					break;
				}

				currentIndex++;
			}
		}

		private void NextTip()
		{
			currentIndex++;

			SkipShownTips();

			if (currentIndex > TipKeys.Length - 1)
			{
				DialogResult = DialogResult.OK;
				Close();
				return;
			}


			UserConfigKey key = TipKeys[currentIndex];

			btnOK.Text = currentIndex >= TipKeys.Length - 1 ? LangResource.btn_ok : LangResource.btn_next;
			imageBox.Visible = false;

			switch (key)
			{
				case UserConfigKey.Tips_SystemTray:
					BackgroundImage = ImageResources.tips_tray;
					labTip.Text = LangResource.tips_tray;
					break;

				case UserConfigKey.Tips_Hotkey:
					BackgroundImage = ImageResources.tips_hotkey_en;
					labTip.Text = LangResource.tips_hotkey;
					break;

				case UserConfigKey.Tips_SelectWindow:
					BackgroundImage = ImageResources.tips_select_window_en;
					labTip.Text = LangResource.tips_select_window;
					break;

				case UserConfigKey.Tips_ChangeNumber:
					BackColor = Color.White;
					labTip.Text = LangResource.tips_changenumber;
					imageBox.Image = ImageResources.addnumber_tip;
					imageBox.Visible = true;
					break;

				default:
					labTip.Text = string.Empty;

					DialogResult = DialogResult.OK;
					Close();

					break;
			}
		}

		public bool IsOnlyFirstShow { get; set; }

		void bottomPanel_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.DrawLine(Pens.Silver, 0, 0, bottomPanel.Width, 0);
		}

		public UserConfigKey[] TipKeys { get; set; }

		public static void CheckAndShowTips(UserConfigKey[] tipKeys)
		{
			ShowTipSlides(tipKeys);
		}

		public static void CheckAndShowTip(UserConfigKey tipKey)
		{
			ShowTipSlides(new UserConfigKey[] { tipKey });
		}
	
		public static void ShowTipSlides(UserConfigKey[] keys)
		{
			ShowTipSlides(keys, true);
		}

		public static void ShowTipSlides(UserConfigKey[] keys, bool isOnlyFirstShow)
		{
			if (isOnlyFirstShow)
			{
				bool allTipsShown = true;
				for (int i = 0; i < keys.Length; i++)
				{
					if (!ConfigurationManager.Instance.IsCurrentUserSetting(keys[i], false))
					{
						allTipsShown = false;
						break;
					}
				}

				if (allTipsShown) return;
			}

			using (TipsForm tipForm = new TipsForm())
			{
				tipForm.IsOnlyFirstShow = isOnlyFirstShow;
				tipForm.TipKeys = keys;
				tipForm.ShowDialog();
			}
		}

		public static void ShowTipSlide(UserConfigKey key)
		{
			ShowTipSlides(new UserConfigKey[] { key });
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (chkNoShowNext.Checked)
			{
				ConfigurationManager.Instance.SwitchCurrentUserSetting(
					TipKeys[currentIndex], true);
			}

			// unshown tips exists
			if (currentIndex < TipKeys.Length)
			{
				NextTip();
			}
			else
			{
				// all tips shown, close form
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void btnSkip_Click(object sender, EventArgs e)
		{
			foreach (UserConfigKey key in TipKeys)
			{
				ConfigurationManager.Instance.SwitchCurrentUserSetting(key, true);
			}
		}
	}
}
