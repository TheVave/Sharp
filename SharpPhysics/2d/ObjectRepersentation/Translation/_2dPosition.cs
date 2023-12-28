using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class _2dPosition
	{
		public override string ToString()
		{
			return $"Pos:{xPos},{yPos},{zPos}";
		}
		public _2dPosition(double x, double y, double z)
		{
			xPos = x;
			yPos = y;
			zPos = z;
		}
		public _2dPosition(double x, double y)
		{
			xPos = x;
			yPos = y;
		}
		public _2dPosition()
		{
			xPos = 0;
			yPos = 0;
		}
		public double xPos;
		public double yPos;
		public double zPos;
	}
}
