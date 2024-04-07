using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation.Translation
{
	public class Translation2d : ISizeGettable
	{
		public Translation2d(double xPos, double yPos, double zPos)
		{
			ObjectPosition = new _2dPosition(xPos, yPos, zPos);
			ObjectRotation = new _2dRotation(0);
			ObjectScale = new _2dScale(128, 128);
		}

		public Translation2d()
		{
			ObjectPosition = new _2dPosition(0, 0, 0);
			ObjectRotation = new _2dRotation(0);
			ObjectScale = new _2dScale(128, 128);
		}
		public override string ToString()
		{
			return $"{ObjectPosition}, {ObjectRotation}, {ObjectScale}";
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Translation2d);
			}
		}

		public _2dPosition ObjectPosition;
		public _2dRotation ObjectRotation;
		public _2dScale ObjectScale;
	}
}