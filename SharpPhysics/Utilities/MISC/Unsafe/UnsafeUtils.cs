using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace SharpPhysics.Utilities.MISC.Unsafe;
public static class UnsafeUtils
{
	/// <summary>
	/// Copies memory between two memory pointers with lengths (and types)
	/// </summary>
	/// <param name="strt"></param>
	/// <param name="dest"></param>
	/// <param name="cpylen"></param>
    public static unsafe void mmemcpy(LenMemPtr strt, LenMemPtr dest, long cpylen)
    {
		try
		{
			long curWdthMngd = 0;
			// used to be for (byte* i = (byte*)strt.ptr; curWdthMngd < cpylen; curWdthMngd++)
			while (curWdthMngd++ < cpylen) *(strt.ptr + curWdthMngd) = *(dest.ptr + curWdthMngd);
		}
		catch
		{
			ErrorHandler.ThrowError(16, false);
		}
	}
	/// <summary>
	/// Copies memory between two byte*s, (slightly) faster than with LenMemPtr.
	/// </summary>
	/// <param name="strt"></param>
	/// <param name="dest"></param>
	/// <param name="cpylen"></param>
	public static unsafe void mmemcpy(byte* strt, byte* dest, long cpylen)
	{
		try
		{
			long curWdthMngd = 0;
			while (curWdthMngd++ < cpylen) *(strt + curWdthMngd) = *(dest + curWdthMngd);
		}
		catch
		{
			ErrorHandler.ThrowError(16, false);
		}
	}
	/// <summary>
	/// Copies managed memory to an unmanaged pointer.
	/// Must be <see cref="UnsafeUtils.Free(void*)"/>'d later.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="valueToCopy"></param>
	/// <returns></returns>
	public static unsafe T* CopyToUnmanagedPointer<T>(T valueToCopy)
	{
		long len = RuntimeSizeof(valueToCopy);
		byte* bteEndPtr = (byte*)Malloc(RuntimeSizeof(len));
		byte* bteStartPtr = (byte*)&valueToCopy;
		mmemcpy(bteStartPtr, bteEndPtr, len);
		return (T*)bteEndPtr;
	}

	/// <summary>
	/// Gets the size of an object at runtime,
	/// and analyzes the currect structure and not compile-time struct.
	/// Main difference: Correctly finds sizes with arrays of any length in a class/struct.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ToGetSizeOf"></param>
	public static unsafe int RuntimeSizeof<T>(T ToGetSizeOf)
	{
		ArgumentNullException.ThrowIfNull(ToGetSizeOf, nameof(ToGetSizeOf));
		return Marshal.SizeOf(ToGetSizeOf);
	}

	/// <summary>
	/// Allocates a specified size.
	/// That memory must be freed by the <see cref="UnsafeUtils.Free(void*)"/> method.
	/// </summary>
	/// <param name="size"></param>
	public static unsafe void* Malloc(int size)
	{
		return (void*)Marshal.AllocHGlobal(size);
	}

	/// <summary>
	/// Frees memory allocated by <see cref="UnsafeUtils.Malloc(int)"/>.
	/// </summary>
	/// <param name="ptr"></param>
	/// <returns></returns>
	public static unsafe bool Free(void* ptr)
	{
		try
		{
			Marshal.FreeHGlobal((nint)ptr);
			return true;
		}
		catch
		{
			// if crash on double free
			if (ErrorTweaks.CrashOnError18)
			{
				ErrorHandler.ThrowError(18, true);
				return false;
			}
			else
			{
				Debug.Print("See ErrorTweaks.CrashOnError18, a double free has occurred.");
				return false;
			}
		}
	}
}