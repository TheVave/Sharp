using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class GenericMathUtils
	{
		public static bool IsPositive(double x) => x > 0;
		public static bool IsNegative(double x) => x < 0;
		public static bool IsZero(double x) => x == 0;
	}
}
