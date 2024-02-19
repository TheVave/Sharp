namespace SharpPhysics.Utilities.MISC
{
	public static class ParallelFor
	{
		public static unsafe void ParallelForLoop(Action<int> action, int executeCount)
		{
			for (int i = 0; i < executeCount; i++)
			{
				action(i);
			}
		}
	}
}
