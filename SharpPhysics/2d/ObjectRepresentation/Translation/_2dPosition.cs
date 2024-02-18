
namespace SharpPhysics._2d.ObjectRepresentation.Translation
{
	public class _2dPosition
	{
		/// <summary>
		/// Point located at the origin.
		/// </summary>
		public static readonly _2dPosition Blank00Pos = new _2dPosition();

		public static _2dPosition operator -(_2dPosition left, _2dPosition right) => new(left.X - right.X, left.Y - right.Y);

		public override string ToString()
		{
			return $"Pos:{X},{Y},{Z}";
		}
		public _2dPosition(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public _2dPosition(double x, double y)
		{
			X = x;
			Y = y;
		}
		public _2dPosition()
		{
			X = 0;
			Y = 0;
		}
		public double X = 0;
		public double Y = 0;
		public double Z = 0;
	}
}
