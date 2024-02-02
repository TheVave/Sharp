using System.Runtime.InteropServices;

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
		public static T[] ConcatArray<T>(T[] baseArr, T[] Concater)
		{
			T[] values = new T[baseArr.Length + Concater.Length];
			baseArr.CopyTo(values, 0);
			Concater.CopyTo(values, baseArr.Length);
			return values;
		}
	}
}
