
using System.Runtime.InteropServices;

namespace Sharp.Input.Mouse
{
	/// <summary>
	/// Win32 class. 
	/// Used inside MouseInput to find where the cursor is.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int x;
		public int y;
	}
}
