namespace SharpPhysics
{
	public class _2dVector
	{
		public Angle VectorAngle;
		public double Magnitude;

		public _2dVector(Angle vectorAngle)
		{
			this.VectorAngle = vectorAngle;
			Magnitude = 1;
		}

		public _2dVector(Angle vectorAngle, double magnitude)
		{
			this.VectorAngle = vectorAngle ?? throw new ArgumentNullException(nameof(vectorAngle));
			this.Magnitude = magnitude;
		}

		public _2dLineMaker To2dLineMaker()
		{
			return new _2dLineMaker(0, 0, VectorAngle.AngleValue, Magnitude);
		}

		public _2dLine ToLine()
		{
			return new _2dLineMaker(0, 0, VectorAngle.AngleValue, Magnitude).Get_2DRay();
		}
	}
}
