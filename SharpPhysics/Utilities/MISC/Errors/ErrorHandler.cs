using System.Runtime.InteropServices;

namespace SharpPhysics
{
    public static class ErrorHandler
    {
		[DllImport("User32.dll", CharSet = CharSet.Unicode)]
		public static extern int MessageBox(IntPtr h, string m, string c, int type);
		public static void ThrowError(string message, bool crash)
        {
			MessageBox(IntPtr.Zero, message, "Error", 0x10 | 0x00);
            if (crash) throw new Exception(message + " (Shown in message box)");
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
