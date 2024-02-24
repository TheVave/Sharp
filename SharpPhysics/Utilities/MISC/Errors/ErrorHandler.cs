using SharpPhysics.Exceptions;
using System.Runtime.InteropServices;

namespace SharpPhysics.Utilities.MISC.Errors
{
	public static class ErrorHandler
	{
		public static bool IsWindows = true;
		public static bool InitCalled = false;
		public static string[] errors = File.ReadAllLines($"{Environment.CurrentDirectory}\\errors.tx");
		public static extern int MessageBox(nint h, string m, string c, int type);
		public static void ThrowError(int messageIdx, bool crash)
		{
			string message = errors[messageIdx - 1];
			ThrowError(message, crash);
		}
		public static void ThrowError(string message,bool crash)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				[DllImport("user32.dll")]
				static extern int MessageBox(System.IntPtr h, string m, string c, int type);
				_ = MessageBox(nint.Zero, message, "Error", /* 0x01 is MB_ICONERROR (error symbol) and 0x00 is MB_OK (ok message box) */ 0x10 | 0x00);
				if (crash) throw new MessageBoxException(message + " (Shown in message box)");
			}
			else
			{
				throw new Exception(message + ". Non Windows OS.");
			}
		}
		public static bool YesNoQuestion(string question, string title, bool crashOnNo)
		{
			return true;
		}
	}
}
