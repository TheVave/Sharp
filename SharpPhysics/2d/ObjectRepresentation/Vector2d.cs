using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Vector2d : ISizeGettable, IAny
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Vector2d()
		{
		}

		public Vector2d(double velocityX, double velocityY)
		{
			X = velocityX;
			Y = velocityY;
		}

		public override string ToString()
		{
			return $"Vector2d{{X:{X},Y:{Y}}}";
		}
		public int GetSize()
		{
			unsafe
			{
				return sizeof(Vector2d);
			}
		}
	}
}
