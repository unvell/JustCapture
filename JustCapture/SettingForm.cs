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
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using unvell.Common;
using unvell.JustCapture.Toolkits;

namespace unvell.JustCapture
{
	internal partial class SettingForm : Form
	{
		private List<ShortcutControlInfo> scis = new List<ShortcutControlInfo>(5);

		public SettingForm()
		{
			InitializeComponent();

			InitCulture();

			int r = 0;

			foreach (ShortcutAction action in ShortcutActionManager.Instance.Actions.Values)
			{
				if (action.HideInList) continue;

				ShortcutControlInfo sci = new ShortcutControlInfo
				{
					Action = action,
				};

				sci.CheckBox = new CheckBox() { Text = action.Name, Dock = DockStyle.Fill, Tag = sci };
				sci.CheckBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);

				unvell.JustCapture.XML.ActionInfo ai = (ConfigurationManager.Instance.GetCurrentUserConfiguration().GetAction(action.Id));

				if (ai != null)
				{
					sci.CheckBox.Checked = ai.activated;
				}
				sci.KeyField = new ShortcutTextbox()
					{
						ShortcutKeys = ai==null? Keys.None  :ShortcutKeyToolkit.StringToKeys(ai.shortcutKeys),
						Dock = DockStyle.Fill,
						Tag = sci
					};

				table.Controls.Add(sci.CheckBox, 0, r);
				table.Controls.Add(sci.KeyField, 1, r);

				scis.Add(sci);
				r++;
			}

			if (RegistryToolkit.PermissionToWriteSystemStartup())
			{
				chkStartup.Enabled = true;
			}
			else
			{
				chkStartup.Enabled = false;
				imgNoPermission.Visible = true;
				labNoPermission.Visible = true;
			}

			chkStartup.Checked = RegistryToolkit.IsStartupProcessExists(ProductInfo.ProductName);

			chkStartup.CheckedChanged += (s, e) =>
			{
				if (chkStartup.Checked)
				{
					RegistryToolkit.SetStartupProcess(ProductInfo.ProductName, Application.ExecutablePath);
				}
				else
				{
					// try delete value of registry
					if (!RegistryToolkit.RemoveStartupProcess(ProductInfo.ProductName))
					{
						// if deleting is failed, try write a non-existed value to start up registry
						if (!RegistryToolkit.SetStartupProcess(ProductInfo.ProductName, string.Empty))
						{
							// if value cannot be replaced with an invalid application path,
							// check on and tell user the startup item cannot be removed
							imgNoPermission.Visible = labNoPermission.Visible = true;
							chkStartup.Checked = true;
						}
					}
				}
			};

			// show toolbar
			chkShowToolboxInCapturer.Checked = ConfigurationManager.Instance.IsCurrentUserSetting(
				UserConfigKey.CF_Toolstrip, true);

			// restore last selected region
			chkRestorePreviousRegion.Checked = ConfigurationManager.Instance.IsCurrentUserSetting(
				UserConfigKey.User_RestoreLastSelectRegion, true);

			// check for updates
			chkCheckForUpdates.Checked = ConfigurationManager.Instance.IsCurrentUserSetting(
				UserConfigKey.User_EnableCheckForUpdates, true);

			outerLineColorComboBox.CurrentColor = FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(
						UserConfigKey.CF_Selection_OuterColor, Color.Black.Name));
			innerLineColorComboBox.CurrentColor = FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(
						UserConfigKey.CF_Selection_InnerColor, Color.Gold.Name));
			thumbColorComboBox.CurrentColor = FileToolkit.DecodeColor(ConfigurationManager.Instance.GetCurrentUserSetting(
						UserConfigKey.CF_Selection_ThumbColor, Color.SkyBlue.Name));

			// show coordinate info
			chkShowCoordinateInfo.Checked = ConfigurationManager.Instance.IsCurrentUserSetting(UserConfigKey.CF_Show_Coordinate_Info, true);

			// magnifier scale
			string magnifierScaleStr = ConfigurationManager.Instance.GetCurrentUserSetting(UserConfigKey.CF_Magnifier_Scale, "2");
			int magnifierScale = 2;
			int.TryParse(magnifierScaleStr, out magnifierScale);
			trkMagnifierScale.ValueChanged += (s, e) => UpdateMagnifierValue();
			trkMagnifierScale.Value = magnifierScale;

			regionSelectionSamplePanel.Paint += (s, e) =>
			{
				Graphics g = e.Graphics;

				Rectangle rect = regionSelectionSamplePanel.ClientRectangle;

				rect.Inflate(-10, -10);
				RangeHandlerEdit.UpdateRangeHandlers(rect, thumbs);

				rect.Inflate(3, 3);
				GraphicsToolkit.DrawDoubleLineRectangle(g, rect, outerLineColorComboBox.CurrentColor,
					innerLineColorComboBox.CurrentColor);

				RangeHandlerEdit.DrawRangeHandlerPos(g, thumbs, thumbColorComboBox.CurrentColor);
			};

			outerLineColorComboBox.ColorSelected += (s, e) => regionSelectionSamplePanel.Invalidate();
			innerLineColorComboBox.ColorSelected += (s, e) => regionSelectionSamplePanel.Invalidate();
			thumbColorComboBox.ColorSelected += (s, e) => regionSelectionSamplePanel.Invalidate();

			languageCombo.SelectedValueChanged += (s, e) =>
			{
				CultureInfo selectedCulture = MainForm.SupportedCultures[languageCombo.SelectedIndex];

				if (LangResource.Culture != selectedCulture)
				{
					LangResource.Culture = selectedCulture;
					InitCulture();
				}
			};

			ReloadSupportedLanguageList();

			chkShowCoordinateInfo.Checked = ConfigurationManager.Instance.IsCurrentUserSetting(
				UserConfigKey.CF_Show_Coordinate_Info, true);
		}

		private void ReloadSupportedLanguageList()
		{
			// languages
			languageCombo.Items.Clear();

			foreach (CultureInfo culture in MainForm.SupportedCultures)
			{
				languageCombo.Items.Add(culture.DisplayName);
			}

			languageCombo.Text = LangResource.Culture == null ?
				System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName :
				LangResource.Culture.DisplayName;
		}

		private readonly Rectangle[] thumbs = new Rectangle[8];

		private void InitCulture()
		{
			Text = LangResource.settings;

			tabShortcutKeys.Text = LangResource.shortcut_keys;
			tabGeneral.Text = LangResource.general;
			tabCaptureTool.Text = LangResource.capture_tool;
			
			labShortcutPrompt.Text = LangResource.hotkeys_custom_tip;
			labLanguage.Text = LangResource.btn_language;

			// start operation
			//labStartOperation.Text = LangResource.lab_start_operation;
			//startOperationCombo.Items.Clear();
			//startOperationCombo.Items.Add(LangResource.so_tray);
			//startOperationCombo.Items.Add(LangResource.so_start_capture);
			//startOperationCombo.Items.Add(LangResource.so_image_editor);
			//startOperationCombo.SelectedIndex =
			//	ConfigurationManager.Instance.GetCurrentUserSettingIntValue(
			//	UserConfigKey.User_StartOperation, 0);

			grpStartUp.Text = LangResource.start_up;
			chkStartup.Text = LangResource.btn_run_on_windows_starts;
			labNoPermission.Text = LangResource.not_enough_permission;

			grpRegionSelection.Text = LangResource.region_selection;
			labOuterline.Text = LangResource.btn_outer_line;
			labInnerLine.Text = LangResource.btn_inner_line;
			labThumb.Text = LangResource.btn_thumb;

			chkShowCoordinateInfo.Text = LangResource.chk_show_coordinate_info;
			chkShowToolboxInCapturer.Text = LangResource.btn_show_toolbar_in_capture_tool;

			foreach (ShortcutControlInfo sci in scis)
			{
				sci.CheckBox.Text = sci.Action.Name;
			}

			chkCheckForUpdates.Text = LangResource.btn_enable_check_for_updates;
			chkRestorePreviousRegion.Text = LangResource.btn_restore_last_region;
			labMagnifierScale.Text = LangResource.lab_magnifier_scale;
			UpdateMagnifierValue();
			
			btnOK.Text = LangResource.btn_ok;
			btnCancel.Text = LangResource.btn_cancel;
		}

		void CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox chk = sender as CheckBox;
			if (chk != null)
			{
				ShortcutControlInfo sci = chk.Tag as ShortcutControlInfo;
				if (sci != null)
				{
					sci.CheckBox.ForeColor = sci.CheckBox.Checked ? SystemColors.WindowText : SystemColors.GrayText;
				}
			}
		}

		private void UpdateMagnifierValue()
		{
			if (trkMagnifierScale.Value <= 0)
			{
				labMagnifierValue.Text = LangResource.lab_no_magnifier_scale;
			}
			else
			{
				labMagnifierValue.Text = string.Format("x{0}", trkMagnifierScale.Value);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			try
			{
				trkMagnifierScale.BackColor = tabGeneral.BackColor;
			}
			catch { }
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			// show toolbar
			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.CF_Toolstrip, chkShowToolboxInCapturer.Checked);
			// check for updates
			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.User_EnableCheckForUpdates, chkCheckForUpdates.Checked);
			// remember last region
			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.User_RestoreLastSelectRegion, chkRestorePreviousRegion.Checked);
			// show coordinate info
			ConfigurationManager.Instance.SwitchCurrentUserSetting(UserConfigKey.CF_Show_Coordinate_Info, chkShowCoordinateInfo.Checked);
			// magnifier scale
			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.CF_Magnifier_Scale, trkMagnifierScale.Value.ToString());

			foreach (ShortcutControlInfo sci in scis)
			{
				ShortcutActionManager.Instance.ChangeAction(sci.Action.Id,
					sci.CheckBox.Checked && sci.KeyField.ShortcutKeys != Keys.None, sci.KeyField.ShortcutKeys);
			}

			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.CF_Selection_OuterColor,
				FileToolkit.EncodeColor(outerLineColorComboBox.CurrentColor));
			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.CF_Selection_InnerColor,
				FileToolkit.EncodeColor(innerLineColorComboBox.CurrentColor));
			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.CF_Selection_ThumbColor, 
				FileToolkit.EncodeColor(thumbColorComboBox.CurrentColor));

			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.User_StartOperation,
				startOperationCombo.SelectedIndex.ToString());

			// language
			ConfigurationManager.Instance.SetCurrentUserSetting(UserConfigKey.User_Language, LangResource.Culture.Name);

			// update hotkey mapping
			ShortcutActionManager.Instance.SaveConfig();

			// save config
			ConfigurationManager.Instance.SaveCurrentUserConfiguration();
		}
	}

	class ShortcutControlInfo
	{
		public ShortcutAction Action { get; set; }
		public CheckBox CheckBox { get; set; }
		public ShortcutTextbox KeyField { get; set; }
	}

	class ShortcutTextbox : TextBox
	{
		public ShortcutTextbox()
		{
			base.ReadOnly = true;
			base.BackColor = SystemColors.Window;
		}

		private List<string> sl = new List<string>();

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			UpdateShortcutKeys(keyData);
			return true;
		}

		private Keys shortcutKeys;

		public Keys ShortcutKeys
		{
			get { return shortcutKeys; }
			set
			{
				UpdateShortcutKeys(value);
			}
		}

		private static readonly List<Keys> modifierKeys = new List<Keys>(new Keys[]{
			Keys.Control, Keys.ControlKey,
			Keys.ShiftKey, Keys.Shift,
			Keys.Alt, Keys.Menu,
		});
		
		private void UpdateShortcutKeys(Keys keys)
		{
			Enum[] splittedKeys = ShortcutKeyToolkit.ConvertToEnumArray(keys);
			bool found = false;
			foreach (Keys k in splittedKeys)
			{
				if (modifierKeys.IndexOf(k) == -1)
				{
					found = true;
					break;
				}
			}

			Keys rsKeys;

			if(!found)
			{
				Text = string.Empty;
				rsKeys = Keys.None;
			}
			else
			{
				Text = ShortcutKeyToolkit.KeysToString(keys);

				rsKeys = keys;
			}

			if (rsKeys != shortcutKeys)
			{
				shortcutKeys = rsKeys;

				if (ShortcutKeyChanged != null)
				{
					ShortcutKeyChanged(this, null);
				}
			}
		}

		public event EventHandler ShortcutKeyChanged;

		public static bool IsKeyDown(Win32.VKey vkey)
		{
			return ((Win32.GetKeyState(vkey) >> 15) & 1) == 1;
		}

		internal void SetShortcutKey(string keys)
		{
			UpdateShortcutKeys(ShortcutKeyToolkit.StringToKeys(keys));
		}

		protected override void WndProc(ref Message m)
		{
			Debug.WriteLine(m.ToString());
			base.WndProc(ref m);
		}
	}
}
