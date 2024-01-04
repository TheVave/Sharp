using System.Runtime.InteropServices;

namespace SharpPhysics.Utilities.MISC
{
	public static class MessageBoxDisplay
	{
		[DllImport("User32.dll", CharSet = CharSet.Unicode)]
		public static extern int MessageBox(IntPtr h, string m, string c, int type);

		/// <summary>
		/// Displays for MessageBox
		/// </summary>
		public static int MB_ICONERROR = 0x10,
		MB_ICONSTOP = 0x10,
		MB_ICONHAND = 0x10,
		MB_ICONEXCLAMATION = 0x30,
		MB_ICONWARNING = 0x30,
		MB_ICONINFO = 0x40,
		MB_NOICON = 0x00,
		MB_ABORTRETRYIGNORE = 0x02,
		MB_CANCELTRYCONTINUE = 0x06,
		MB_HELP = 0x4000,
		MB_OK = 0x00,
		MB_OKCANCEL = 0x01,
		MB_RETRYCANCEL = 0x05,
		MB_YESNO = 0x04,
		MB_YESNOCANCEL = 0x03;

		public static void ThrowError(string message, bool crash)
		{
			MessageBox(IntPtr.Zero, message, "Error", MB_ICONERROR | MB_OK);
			if (crash) throw new Exception(message + " (Shown in message box)");
		}

		public static MessageBoxResult ShowMessageBox(int iconStyle, int buttonStyle, string message, string title) =>
			(MessageBoxResult)MessageBox(IntPtr.Zero, message, title, iconStyle | buttonStyle);
		public static void ThrowNotImplementedException() =>
			ThrowError("Not Implemented.", true);
		public static bool YesNoQuestion(string question, string title, bool crashOnNo) =>
			(ShowMessageBox(MB_ICONINFO, MB_YESNO, question, title) == MessageBoxResult.YES) ? true : false;
	}
}
