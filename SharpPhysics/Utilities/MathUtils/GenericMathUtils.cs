namespace SharpPhysics.Utilities.MathUtils
{
	public static class GenericMathUtils
	{
		/// <summary>
		/// Returns true if the input value is positive. False otherwise.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsPositive(double x) => x > 0;

		/// <summary>
		/// Returns the opposite of IsPositive.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsNegative(double x) => x < 0;

		/// <summary>
		/// Returns true if the value is zero. False if it is any other value.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsZero(double x) => x == 0;

		/// <summary>
		/// Sets a value to the negative value if the value is negative and positive if the value is positive.
		/// Example: valueToSet = -3 setTo = 4 output: -4
		/// Example: valueToSet = 6 setTo = 3 output: 3
		/// </summary>
		/// <param name="valueToSet"></param>
		/// <param name="setTo"></param>
		/// <returns></returns>
		// for cleaning up the code:
		public static double NegativePositiveSet(double valueToSet, double setTo)
		{
			if (IsPositive(valueToSet)) 
				 return setTo;

			else return -setTo;
		}

		/// <summary>
		/// Subtracts to zero and not past it.
		/// Also works for negative values with addition.
		/// Example: a = 8 toSubtract = 10 output = 0
		/// a = -4 toSubtract = 3 output = -1
		/// a = -5 toSubtract = 90 output = 0
		/// </summary>
		/// <param name="a"></param>
		/// <param name="toSubtract"></param>
		/// <returns></returns>
		public static double SubtractToZero(double a, double toSubtract)
		{
			if (IsNegative(a))
			{
				if (toSubtract < a) return 0;
				else return a + toSubtract;
			}
			else /* if (IsPositive(a)) */
			{
				if (toSubtract > a) return 0;
				else return a - toSubtract;
			}
		}
	}
}
