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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;

namespace unvell.Common
{
	/// <summary>
	/// Summary description for SingleApp.
	/// </summary>
	internal sealed class SingleApplication
	{
		/// <summary>
		/// GetCurrentInstanceWindowHandle
		/// </summary>
		/// <returns></returns>
		private static IntPtr GetCurrentInstanceWindowHandle()
		{    
			IntPtr hWnd = IntPtr.Zero;
			Process process = Process.GetCurrentProcess();
			Process[] processes = Process.GetProcessesByName(process.ProcessName);
			foreach(Process _process in processes)
			{
				// Get the first instance that is not this instance, has the
				// same process name and was started from the same file name
				// and location. Also check that the process has a valid
				// window handle in this session to filter out other user's
				// processes.
				if (_process.Id != process.Id &&
					_process.MainModule.FileName == process.MainModule.FileName &&
					_process.MainWindowHandle != IntPtr.Zero)    
				{
					hWnd = _process.MainWindowHandle;
					break;
				}
			}
			return hWnd;
		}
		/// <summary>
		/// SwitchToCurrentInstance
		/// </summary>
		internal static void SwitchToCurrentInstance()
		{    
			IntPtr hWnd = GetCurrentInstanceWindowHandle();
			if (hWnd != IntPtr.Zero)    
			{    
				// Restore window if minimised. Do not restore if already in
				// normal or maximised window state, since we don't want to
				// change the current state of the window.
				if (Win32.IsIconic(hWnd) != 0)
				{
					Win32.ShowWindow(hWnd, (int)Win32.ShowWindowCmd.SW_RESTORE);
				}

				// Set foreground window.
				Win32.SetForegroundWindow(hWnd);
			}
		}

		/// <summary>
		/// Execute a form base application if another instance already running on
		/// the system activate previous one
		/// </summary>
		/// <param name="frmMain">main form</param>
		/// <returns>true if no previous instance is running</returns>
		public static bool Run(System.Windows.Forms.Form frmMain)
		{
			if(IsAlreadyRunning())
			{
				//set focus on previously running app
				SwitchToCurrentInstance();
				return false;
			}
			Application.Run(frmMain);
			return true;
		}

		/// <summary>
		/// for console base application
		/// </summary>
		/// <returns></returns>
		public static bool Run()
		{
			if(IsAlreadyRunning()) 
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// check if given exe alread running or not
		/// </summary>
		/// <returns>returns true if already running</returns>
		internal static bool IsAlreadyRunning()
		{
			string strLoc = Assembly.GetExecutingAssembly().Location;
			FileSystemInfo fileInfo = new FileInfo(strLoc);
			string sExeName = fileInfo.Name;
			bool bCreatedNew;

			mutex = new Mutex(true, "Global\\"+sExeName, out bCreatedNew);
			if (bCreatedNew)
				mutex.ReleaseMutex();

			return !bCreatedNew;
		}
		
		static Mutex mutex;
	}
}
