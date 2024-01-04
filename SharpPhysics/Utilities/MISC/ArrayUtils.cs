namespace SharpPhysics.Utilities.MISC
{
	public static class ArrayUtils
	{
		public static Span<T> AddSpanObject<T>(Span<T> inputArray, T valueToAdd)
		{
			Span<T> values = new T[inputArray.Length + 1];
			inputArray.CopyTo(values);
			values[^1] = valueToAdd;
			return values;
		}
	}
}
