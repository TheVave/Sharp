using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Vector2d : ISizeGettable
	{
		public double VelocityX { get; set; }
		public double VelocityY { get; set; }

		public Vector2d()
		{
		}

		public Vector2d(double velocityX, double velocityY)
		{
			VelocityX = velocityX;
			VelocityY = velocityY;
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
