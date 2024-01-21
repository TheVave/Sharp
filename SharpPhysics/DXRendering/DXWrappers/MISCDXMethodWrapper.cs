using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.DXRendering.DXWrappers
{
	public static class MISCDXMethodWrapper
	{
		[DllImport("CppTester.dll")]
		public static extern void mn();
	}
}
