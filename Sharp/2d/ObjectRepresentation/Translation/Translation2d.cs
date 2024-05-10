using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC.Unsafe;

namespace Sharp._2d.ObjectRepresentation.Translation
{
	public class Translation2d : ISizeGettable, IAny
	{
		public Translation2d(double xPos, double yPos, double zPos)
		{
			ObjectPosition = new Position(xPos, yPos, zPos);
			ObjectRotation = new Rotation(0);
			ObjectScale = new Scale(128, 128);
		}

		public Translation2d()
		{
			ObjectPosition = new Position(0, 0, 0);
			ObjectRotation = new Rotation(0);
			ObjectScale = new Scale(128, 128);
		}
		public override string ToString()
		{
			return $"Translation2d:{{ObjectPosition:{ObjectPosition}, ObjectRotation:{ObjectRotation}, ObjectScale:{ObjectScale}}}";
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Translation2d);
			}
		}

		public Position ObjectPosition;
		public Rotation ObjectRotation;
		public Scale ObjectScale;
	}
}