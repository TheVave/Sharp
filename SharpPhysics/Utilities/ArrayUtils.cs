using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class ArrayUtils
	{
		public static void ArrayAdd(ref object[] objs, object obj)
		{
			objs = objs.Append(obj).ToArray();
		}
	}
}
