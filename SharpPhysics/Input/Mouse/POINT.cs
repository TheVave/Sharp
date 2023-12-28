using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
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
