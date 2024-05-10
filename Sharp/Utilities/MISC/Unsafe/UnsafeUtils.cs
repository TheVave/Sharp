using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
namespace Sharp.Utilities.MISC.Unsafe;
public static class UnsafeUtils
{
	public static int PtrSize
	{
		get
		{
			if (Environment.Is64BitProcess)
				return 8;
			else
				return 4;
		}
	}

	/// <summary>
	/// Copies memory between two byte*'s.
	/// </summary>
	/// <param name="strt"></param>
	/// <param name="dest"></param>
	/// <param name="cpylen"></param>
	public static unsafe void mmemcpy(byte* strt, byte* dest, long cpylen)
	{
		try
		{
			long curWdthMngd = 0;
			while (curWdthMngd++ < cpylen) *(dest + curWdthMngd) = *(strt + curWdthMngd);
		}
		catch
		{
			ErrorHandler.ThrowError(16, false);
			throw;
		}
	}

	/// <summary>
	/// Gets the size of a simple object.
	/// e.g int, double, char
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	public static unsafe int GetSimpleObjectSize<T>(T obj)
	{
		// may not work
		if (typeof(T) is string)
			return Encoding.Unicode.GetByteCount(obj as string);
		return sizeof(T);
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
		long len = UnmanagedSizeof(valueToCopy);
		byte* bteEndPtr = (byte*)Malloc(UnmanagedSizeof(len));
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
	public static unsafe int UnmanagedSizeof<T>(T ToGetSizeOf)
	{
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

	/// <summary>
	/// Gets the size of an object.
	/// Counts each bit untill it finds an unalloced byte, then returns the current byte-1.
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	[HandleProcessCorruptedStateExceptions]
	public static int CountingSizeof<T>(T obj)
	{
		unsafe
		{
			T* objPtr = &obj;
			int i = 0;
			try
			{
				// while(true) but has loop count local var i
				for (; ; i++)
				{
					byte* ptr = (byte*)i;
				}
				//Error, Internal/External Error, unknown cause.
				ErrorHandler.ThrowError(5, true);
				return -1;
			}
			catch (AccessViolationException ave)
			{
				return i--;
			}
			catch (OutOfMemoryException oome)
			{
				// to try to reduce the used mem on the user's system
				Environment.Exit(0xFFFF);
				return -1;
			}
			catch
			{
				//Error, Internal/External Error, unknown cause.
				ErrorHandler.ThrowError(5, true);
				return -1;
			}
		}
	}

	public static int GetArraySize(double[] arr)
	{
		return sizeof(double) * arr.Length;
	}
	public static int GetArraySize(int[] arr)
	{
		return sizeof(int) * arr.Length;
	}
	public static int GetArraySize(char[] arr)
	{
		return sizeof(char) * arr.Length;
	}
	public static int GetArraySize(ISizeGettable[] arr)
	{
		int size = 0;
		foreach (ISizeGettable gettable in arr)
			size += gettable.GetSize();
		return size;
	}
}