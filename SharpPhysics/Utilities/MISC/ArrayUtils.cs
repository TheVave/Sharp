using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC.Unsafe;
using System;
using System.Diagnostics;
using System.Reflection;
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
		/// <summary>
		/// Quickly removes an object from an array
		/// Similar speed as List.RemoveAt
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr"></param>
		/// <param name="objToRem"></param>
		// I spend a few hours on this until I looked at List.Remove -> List.RemoveAt (ln 970 in .net 8 list.cs)
		public static unsafe void RemoveArrayObjFast<T>(T[] arr, int objToRem)
		{
			try
			{
				if (objToRem > arr.Length || GenericMathUtils.IsNegative(objToRem)) throw new ArgumentOutOfRangeException();
				// got idea from List<T>.RemoveAt
				Array.Copy(arr, objToRem + 1, arr, objToRem, (sizeof(T) * arr.Length) - objToRem);
			}
			catch (ArgumentOutOfRangeException aoore)
			{
				// Error, Internal Error, RemoveArrayObjFast failed with IndexOutOfRangeException. The value to remove was outside the bounds of the array.
				// if this happens then just don't remove the obj!
				ErrorHandler.ThrowError(17, ErrorTweaks.CrashOnError17);
				Debug.Write("Object will not removed, as it doesn't exist. \n If you wish for this to crash, set CrashOnError17 in ErrorTweaks to true.");
			}
			catch
			{
				//Error, Internal Error, Fast array object removal failure. This is often a example of uninialized values.
				ErrorHandler.ThrowError(15, true);
			}
		}
	}
}
