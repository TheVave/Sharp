//#define UseCountSizeof
#define UseRecursiveSizeof
namespace SharpPhysics.Utilities.MISC.Unsafe
{
	public interface ISizeGettable
	{
#if UseCountSizeof
		[HandleProcessCorruptedStateExceptions]
		public static int GetSize(ISizeGettable obj)
		{
			unsafe
			{
				ISizeGettable* objPtr = &obj;
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
#endif
#if UseRecursiveSizeof
		public int GetSize();
#endif
	}
}
