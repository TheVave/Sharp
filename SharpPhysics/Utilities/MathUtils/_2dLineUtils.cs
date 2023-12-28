namespace SharpPhysics
{
	public static class _2dLineUtils
	{
		public static bool DoRaysPointsIntersect(Point p1, Point p2, Point n1, Point n2)
		{
			double u = (p1.Y * n2.X + n2.Y * p2.X - p2.Y * n2.X - n2.Y * p1.X) / (n1.X * n2.Y - n1.Y * n2.X);
			double v = (p1.X + n1.X * u - p2.X) / n1.X;

			return u > 0 && v > 0;
		}
		public static bool DoRaysIntersect(_2dLine line1, _2dLine line2) => DoRaysPointsIntersect(
			new Point(line1.RayData.xStart, line1.RayData.yStart)
			, new Point(line1.RayData.xEnd, line1.RayData.yEnd)
			, new Point(line2.RayData.xStart, line2.RayData.yStart)
			, new Point(line2.RayData.xEnd, line2.RayData.yEnd));
		public static _2dLine PointsToLine(Point a, Point b) => new _2dLine(a, b);
	}
}
