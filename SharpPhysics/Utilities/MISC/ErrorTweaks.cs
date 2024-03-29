using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Utilities.MISC
{
	public static class ErrorTweaks
	{
		/// <summary>
		/// If the program should crash on error 17.<br/>
		/// Error 17: Error, Internal Error, RemoveArrayObjFast failed with IndexOutOfRangeException. The value to remove was outside the bounds of the array.
		/// </summary>
		public static bool CrashOnError17 = false;

		/// <summary>
		/// If the program should crash on error 18
		/// (double free) Error, Internal Error, A double free has occurred. This happens when memory is treated in strange ways, and you attempt to free memory twice.
		/// </summary>
		public static bool CrashOnError18 = false;
	}
}
