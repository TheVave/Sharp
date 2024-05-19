using Sharp.Utilities.MathUtils;
using Sharp.Utilities.MathUtils.DelaunayTriangulator;
using System.Diagnostics;

namespace Sharp.Utilities.MISC
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
		public static T[] AppendArrayObject<T>(T[] start, T valueToAppend)
		{
			T[] ValueToReturn = new T[start.Length + 1];
			start.CopyTo(ValueToReturn, 0);
			ValueToReturn[^1] = valueToAppend;
			return ValueToReturn;
		}
		// in the future?||
		//               \/
		//public static T[] AddArrayObjectAtIndex<T>
		public static T[] ConcatArray<T>(T[] baseArr, T[] Concater)
		{
			T[] values = new T[baseArr.Length + Concater.Length];
			baseArr.CopyTo(values, 0);
			Concater.CopyTo(values, baseArr.Length);
			return values;
		}
		/// <summary>
		/// Exact same behavior as Array.IndexOf
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="val"></param>
		/// <param name="array"></param>
		/// <returns></returns>
		public static int SpanIndexOf<T>(T val, T[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(val)) return i;
			}
			return -1;
		}
		public static int[] GetObjectIndexes<T>(T[] objs, T objectToCount)
		{
			int[] toReturn = [];
			int idx = 0;
			foreach (T obj in objs)
				if (obj.Equals(objectToCount))
					toReturn = AppendArrayObject(toReturn, idx++);
			return toReturn;
		}
	}
}
