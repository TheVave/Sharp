namespace Sharp.Utilities.MathUtils
{
	public static class MathLibExt
	{
		public static double RoundToDecimalPlace(int decimalPlace, double value)
		{
			return Math.Floor(value * (decimalPlace ^ 10)) / (decimalPlace ^ 10);
		}
	}
}
