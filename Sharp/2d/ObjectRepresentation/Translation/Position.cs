using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC.Unsafe;

namespace Sharp._2d.ObjectRepresentation.Translation
{
	public class Position : ISizeGettable, IAny
	{
		/// <summary>
		/// Point located at the origin.
		/// </summary>
		public static readonly Position Blank00Pos = new();

		public static Position operator -(Position left, Position right) => new(left.X - right.X, left.Y - right.Y);

		public override string ToString()
		{
			return $"_2dPosition:{{X:{X},Y:{Y},Z:{Z}}}";
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Position);
			}
		}

		public Position(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public Position(double x, double y)
		{
			X = x;
			Y = y;
		}
		public Position()
		{
			X = 0;
			Y = 0;
		}
		public double X = 0;
		public double Y = 0;
		public double Z = 0;
	}
}
