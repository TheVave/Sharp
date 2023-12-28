
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
		/// <summary>
		/// True if the left mouse is down, otherwise false.
		/// </summary>
		public static bool LeftMouseDown = false;

		/// <summary>
		/// True if the right mouse is down, otherwise false.
		/// </summary>
		public static bool RightMouseDown = false;

		/// <summary>
		/// True if the middle mouse is down, otherwise false.
		/// </summary>
		public static bool MiddleMouseDown = false;

		/// <summary>
		/// Used for detecting if the mouse buttons are pressed.
		/// </summary>
		/// <param name="VirtualKeyPressed"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		static extern short GetAsyncKeyState(int VirtualKeyPressed);

		/// <summary>
		/// Functions for finding the cursor position as a POINT struct.
		/// The POINT struct is defined inside POINT.cs
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT point);

		/// <summary>
		/// Currently not implemented.
		/// </summary>
		public static void CheckMouse()
		{
			
		}
	}
}
