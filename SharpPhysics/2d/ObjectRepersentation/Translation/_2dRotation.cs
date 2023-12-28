using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class _2dRotation
	{
		public override string ToString()
		{
			return $"Rot:{xRot}";
		}
		public _2dRotation(Translation.Rotation rotRef)
		{
			xRot = rotRef.xRot;
			yRot = 0;
		}
		public _2dRotation(float x, float y)
		{
			xRot = x;
			yRot = y;
		}
		public _2dRotation(float x)
		{
			xRot = x;
		}
		public _2dRotation()
		{
			xRot = 0;
		}
		public float xRot;
		public float yRot;
	}
}
