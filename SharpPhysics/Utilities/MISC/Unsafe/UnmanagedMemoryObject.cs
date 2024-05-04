using SharpPhysics.StrangeDataTypes;
using System.Diagnostics;
using static SharpPhysics.Utilities.MISC.Unsafe.UnsafeUtils;

namespace SharpPhysics.Utilities.MISC.Unsafe
{
	public class UnmanagedMemoryObject<T> : ISizeGettable, IAny
	{
		public int Size { get; internal set; } = 0;
		public unsafe T* ObjectPtr { get; internal set; } = (T*)Utils.NULLPTR;
		public T HeldObject
		{
			get
			{
				unsafe
				{
					return *ObjectPtr;
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
				if (HeldObject is not ISizeGettable SizeGettableObject)
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

		public unsafe void Dispose() =>
			Free(ObjectPtr);

		public unsafe void Create(ISizeGettable obj)
		{
			Size = obj.GetSize();
			ObjectPtr = (T*)Malloc(Size);
			Set(obj, Size);
		}
		public unsafe void Create(ISizeGettable obj, int size)
		{
			Size = size;
			ObjectPtr = (T*)Malloc(Size);
			Set(obj, Size);
		}

		public unsafe void Set(T toSet, int size)
		{
			mmemcpy((byte*)&toSet, (byte*)ObjectPtr, size);
		}
		public unsafe void Set(ISizeGettable toSet, int size)
		{
			mmemcpy((byte*)&toSet, (byte*)ObjectPtr, size);
		}
		public unsafe void Set(ISizeGettable toSet)
		{
			mmemcpy((byte*)&toSet, (byte*)ObjectPtr, toSet.GetSize());
		}

		public unsafe int GetSize() =>
			PtrSize +
			sizeof(int);
	}
}