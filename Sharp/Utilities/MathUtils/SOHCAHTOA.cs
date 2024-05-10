namespace Sharp.Utilities.MathUtils
{
	public static class SOHCAHTOA
	{
		public static double SOH(double oLength, double hLength) => Math.Acos(oLength / hLength);
		public static double CAH(double aLength, double hLength) => Math.Acos(aLength / hLength);
		public static double TOA(double oLength, double aLength) => Math.Acos(oLength / aLength);
	}
}
