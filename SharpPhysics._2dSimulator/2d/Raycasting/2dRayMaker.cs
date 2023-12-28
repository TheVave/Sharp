using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics;

namespace SharpPhysics
{
	internal class _2dRayMaker
	{
		public double xStart, yStart;
		public double xRot, yRot;
		public double Length;

		public _2dRay Get_2DRay()
		{
			_2dRay _2DRay = new();
			_2DRay.RayData.xEnd = xStart + ((xRot + 1) * Length);
			_2DRay.RayData.yEnd = yStart + ((yRot + 1) * Length);
			_2DRay.RayData.xStart = xStart;
			_2DRay.RayData.yStart = yStart;
			return _2DRay;
		}
	}
}
