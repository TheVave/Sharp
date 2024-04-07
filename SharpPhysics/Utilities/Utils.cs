using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics.Utilities
{
	public static unsafe class Utils
	{
		public static void* NULLVOIDPTR = (void*)NULLPTR;
		public static nint NULLPTR = IntPtr.Zero;
		public static bool RenderingStarted = false;

		/// <summary>
		/// Higher values are better for cpu performance and higher ones for accuracy.
		/// </summary>
		public static int DelayTestTime = 2;

		public static unsafe void AwaitUntilValue<T>(T* reference, T valueToAwait)
		{
			while (!(*reference).Equals(valueToAwait)) Task.Delay(DelayTestTime);
		}
		public static unsafe void AwaitUntilValue<T>(T* reference, T valueToAwait, int timeToWait)
		{
			while (!(*reference).Equals(valueToAwait)) Task.Delay(timeToWait);
		}

		public static unsafe void ParallelForLoop(Action<int> action, int executeCount)
		{
			for (int i = 0; i < executeCount; i++)
			{
				action(i);
			}
		}

		public static HowChanged[] FindChange(object[] original, object[] updated)
		{
			if (original == null && updated == null)
			{
				return new HowChanged[0];
			}

			if (original == null)
			{
				return Enumerable.Repeat(HowChanged.Added, updated.Length).ToArray();
			}

			if (updated == null)
			{
				return Enumerable.Repeat(HowChanged.Removed, original.Length).ToArray();
			}

			HowChanged[] changes = new HowChanged[Math.Max(original.Length, updated.Length)];

			for (int i = 0; i < changes.Length; i++)
			{
				if (i < original.Length && i < updated.Length)
				{
					if (original[i].Equals(updated[i]))
					{
						changes[i] = HowChanged.NotChanged;
					}
					else
					{
						changes[i] = HowChanged.Moved;
					}
				}
				else if (i < original.Length)
				{
					changes[i] = HowChanged.Removed;
				}
				else if (i < updated.Length)
				{
					changes[i] = HowChanged.Added;
				}
			}

			return changes;
		}
	}
}
