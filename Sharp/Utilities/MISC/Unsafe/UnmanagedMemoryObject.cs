using Sharp.StrangeDataTypes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Sharp.Utilities.MISC.Unsafe.UnsafeUtils;

namespace Sharp.Utilities.MISC.Unsafe
{
	public class UnmanagedMemoryObject<T> : ISizeGettable, IAny where T : new()
	{
		/// <summary>
		/// The size of the unmanaged memory.
		/// </summary>
		public int Size { get; internal set; } = 0;

		/// <summary>
		/// The unmanaged memory pointer.
		/// </summary>
		public unsafe T* ObjectPtr { get; internal set; } = (T*)Utils.NULLPTR;

		/// <summary>
		/// The value the object holds.
		/// </summary>
		// BIG WRAPPER OF OBJECTPTR
		public T Value
		{
			get
			{
				unsafe
				{
					return Marshal.PtrToStructure<T>((nint)ObjectPtr);
				}
			}
			set
			{
				ObjectSet(value);
			}
		}

		~UnmanagedMemoryObject()
		{
			Dispose();
		}

		private void ObjectSet(T value)
		{
			try
			{
				if (Value is not ISizeGettable SizeGettableObject)
				{
					// Error, Internal Error, Mem: A UnmanagedMemoryObject<T> setter call has happened with a non-ISizeGettable class. Please call the setter with a ISizeGettable object.
					ErrorHandler.ThrowError(19, ErrorTweaks.CrashOnError19);
					Debug.Print("If you want this to crash, please set CrashOnError19 in ErrorTweaks to true.");
					return;
				}
				int newSize = SizeGettableObject.GetSize();

				if (newSize == /* original object size -> */Size)
				{
					// because the mem size is the same, no need to re-alloc,
					// just set the data.
					Set(value, newSize);
				}
				else
				{
					// re-alloc the object.
					Dispose();
					Create(SizeGettableObject, newSize);
				}
			}
			catch (InvalidCastException ice)
			{
				// Error, Internal Error, Mem: A UnmanagedMemoryObject<T> setter call has happened with a non-ISizeGettable class. Please call the setter with a ISizeGettable object.
				ErrorHandler.ThrowError(19, true);
			}
			catch
			{
				// Error, Internal/External Error, unknown cause.
				ErrorHandler.ThrowError(5, true);
			}
		}

		/// <summary>
		/// Disposes of the object.
		/// Automatically called when the garbage collector requests this object be collected.
		/// </summary>
		public unsafe void Dispose() =>
			free(ObjectPtr);

		/// <summary>
		/// Initializes the UnsafeMemoryObject and sets it to obj
		/// </summary>
		/// <param name="obj"></param>
		public unsafe void Create(ISizeGettable obj)
		{
			Size = obj.GetSize();
			ObjectPtr = (T*)malloc(Size);
			MarshalManagedMemToUnmanagedMem((ISizeGettable*)ObjectPtr, obj, false);
		}

		/// <summary>
		/// Initializes the UnsafeMemoryObject and sets it to obj
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="size"></param>
		public unsafe void Create(ISizeGettable obj, int size)
		{
			Size = size;
			ObjectPtr = (T*)malloc(Size);
			MarshalManagedMemToUnmanagedMem((ISizeGettable*)ObjectPtr, obj, false);
		}

		/// <summary>
		/// Sets the data inside of the UnmanagedMemoryObject
		/// </summary>
		/// <param name="toSet"></param>
		/// <param name="size"></param>
		public unsafe void Set(T toSet, int size) =>
			MarshalManagedMemToUnmanagedMem(ObjectPtr, toSet, true);

		/// <summary>
		/// Sets the data inside of the UnmanagedMemoryObject
		/// </summary>
		/// <param name="toSet"></param>
		/// <param name="size"></param>
		public unsafe void Set(ISizeGettable toSet, int size) =>
			MarshalManagedMemToUnmanagedMem((ISizeGettable*)ObjectPtr, toSet, true);

		/// <summary>
		/// Sets the data inside of the UnmanagedMemoryObject
		/// </summary>
		/// <param name="toSet"></param>
		public unsafe void Set(ISizeGettable toSet) =>
			MarshalManagedMemToUnmanagedMem((ISizeGettable*)ObjectPtr, toSet, true);

		/// <summary>
		/// So it's set but for copying from other UnsafeMemoryHandledObject's
		/// </summary>
		/// <param name="ptr"></param>
		public unsafe void SetFromUnmanaged(T* ptr, int len) =>
			mmemcpy(ptr, ObjectPtr, len);

		/// <summary>
		/// Gets the size of the memory to hold the unmanaged memory.
		/// Finds the size of the pointer and size, but NOT THE ACTUAL UNMANAGED MEMORY!
		/// </summary>
		/// <returns></returns>
		public unsafe int GetSize() => PtrSize + sizeof(int);
	}
}