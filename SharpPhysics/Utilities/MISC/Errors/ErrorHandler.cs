using SharpPhysics.Exceptions;
using System.Runtime.InteropServices;

namespace SharpPhysics.Utilities.MISC.Errors
{
	public static class ErrorHandler
	{
		public static bool IsWindows = true;
		public static bool InitCalled = false;
		public static extern int MessageBox(System.IntPtr h, string m, string c, int type);
		public static void ThrowError(string message, bool crash)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				[DllImport("user32.dll")]
				static extern int MessageBox(System.IntPtr h, string m, string c, int type);
				MessageBox(System.IntPtr.Zero, message, "Error", /* 0x01 is MB_ICONERROR (error symbol) and 0x00 is MB_OK (ok message box) */ 0x10 | 0x00);
				if (crash) throw new MessageBoxException(message + " (Shown in message box)");
			}
			else
			{
				throw new Exception(message + ". Non Windows OS.");
			}
		}
		public static void ThrowNotImplementedExcepetion()
		{
			ThrowError("Not Implemented.", true);
		}
		public static bool YesNoQuestion(string question, string title, bool crashOnNo)
		{
			return true;
		}
	}
}
