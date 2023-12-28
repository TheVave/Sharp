using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class MathLibExt
	{
		public static double RoundToDecimalPlace(int decimalPlace, double value)
		{
			return Math.Floor(value * (decimalPlace ^ 10)) / (decimalPlace ^ 10);
		}
	}
}
