using Sharp.Utilities.MISC.Unsafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.StrangeDataTypes
{
	public struct TestClass : ISizeGettable
	{
		public bool foo = false;
		public bool bar = false;
		public bool baz = false;

		public TestClass()
		{
		}

		public int GetSize()
		{
			return sizeof(bool) * 3;
		}
	}
}
