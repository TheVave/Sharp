using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation.Translation
{
	public class _2dRotation : ISizeGettable
	{
		public override string ToString()
		{
			return $"Rot:{xRot}";
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(_2dRotation);
			}
		}

		public _2dRotation(float x)
		{
			xRot = x;
		}
		public _2dRotation()
		{
			xRot = 0;
		}
		public float xRot = 0;
	}
}
