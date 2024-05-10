namespace Sharp.Utilities.MISC
{
	public static class ErrorTweaks
	{
		/// <summary>
		/// If the program should crash on error 17.<br/>
		/// Error 17: Error, Internal Error, RemoveArrayObjFast failed with IndexOutOfRangeException. The value to remove was outside the bounds of the array.
		/// </summary>
		public static bool CrashOnError17 = false;

		/// <summary>
		/// If the program should crash on error 18 <br/>
		/// (double free) Error, Internal Error, A double free has occurred. This happens when memory is treated in strange ways, and you attempt to free memory twice.
		/// </summary>
		public static bool CrashOnError18 = false;

		/// <summary>
		/// If the program should crash on error 19.<br/>
		/// (Invalid input type) Error, Internal Error, Mem: A UnmanagedMemoryObject<T> setter call has happened with a non-ISizeGettable class. Please call the setter with a ISizeGettable object.
		/// </summary>
		public static bool CrashOnError19 = false;

		/// <summary>
		/// If the program should say the current OS is not windows in various places where it calls windows methods.
		/// </summary>
		public static bool SayNonWindowsOS = true;
	}
}
