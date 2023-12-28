using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class _2dLineUtils
	{
		public static bool DoRaysPointsIntersect(_2dPoint p1, _2dPoint p2, _2dPoint n1, _2dPoint n2)
		{
			double u = (p1.y * n2.x + n2.y * p2.x - p2.y * n2.x - n2.y * p1.x) / (n1.x * n2.y - n1.y * n2.x);
			double v = (p1.x + n1.x * u - p2.x) / n1.x;

			return u > 0 && v > 0;
		}
		public static bool DoRaysIntersect(_2dLine line1, _2dLine line2) => DoRaysPointsIntersect(
			new _2dPoint(line1.RayData.xStart, line1.RayData.yStart)
			, new _2dPoint(line1.RayData.xEnd, line1.RayData.yEnd)
			, new _2dPoint(line2.RayData.xStart, line2.RayData.yStart)
			, new _2dPoint(line2.RayData.xEnd, line2.RayData.yEnd));
		public static _2dLine PointsToLine(_2dPoint a, _2dPoint b) => new _2dLine(a, b);
	}
}
