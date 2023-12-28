namespace SharpPhysics
{
	public class _2dPoint
	{
		public double x = 0;
		public double y = 0;
		public double zPos = 0;
		internal bool Is3d = false;
		internal _2dPoint(double xPos, double yPos, double zPos, bool is3d)
		{
			this.x = xPos;
			this.y = yPos;
			this.zPos = zPos;
			Is3d = is3d;
		}
		public _2dPoint(double xPos, double yPos)
		{
			this.x = xPos;
			this.y = yPos;
			Is3d = false;
		}
		public _2dPoint(double xPos, double yPos, double zPos) 
		{
			this.x = xPos;
			this.y = yPos;
			this.zPos = zPos;
			Is3d = true;
		}

		public static _2dPoint Rotate2dPoint(double radians, _2dPoint pnt)
		{
			_2dPoint resltpnt = pnt;
			resltpnt.x = pnt.x * Math.Cos(radians) - pnt.y * Math.Cos(radians);
			resltpnt.y = pnt.x * Math.Sin(radians) + pnt.y * Math.Cos(radians);
			return resltpnt;
		}

		public static _2dPoint FlattenPoint(_2dPoint point)
		{
			point.zPos = 0;
			return point;
		}

		public static _2dPoint FlattenPosition(Translation.Position pos)
		{
			return new _2dPoint(pos.xPos, pos.zPos);
		}

		public static Translation.Position FlattenPositionToPosition(Translation.Position pos)
		{
			return new Translation.Position(pos.xPos, pos.yPos, 0);
		}
	}
}
