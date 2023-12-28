using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class _2dScale
	{
		public override string ToString()
		{
			return $"Scale:{xSca},{ySca},{zSca}";
		}
		public _2dScale(Translation.Scale scaleRef)
		{
			xSca = scaleRef.xSca;
			ySca = scaleRef.ySca;
			zSca = 0;
		}
		public _2dScale(short x, short y, short z)
		{
			xSca = x;
			ySca = y;
		}
		public _2dScale(short x, short y)
		{
			xSca = x;
			ySca = y;
		}
		public _2dScale()
		{
			xSca = 0;
			ySca = 0;
		}
		public short xSca;
		public short ySca;
		public short zSca;
	}
}
