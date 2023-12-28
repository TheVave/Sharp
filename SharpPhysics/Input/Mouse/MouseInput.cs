
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class MouseInput
	{
		public static bool LeftMouseDown = false;
		public static bool RightMouseDown = false;
		public static bool MiddleMouseDown = false;
		[DllImport("user32.dll")]
		static extern short GetAsyncKeyState(int VirtualKeyPressed);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT point);
		public static void CheckMouse()
		{
			
		}
	}
}
