namespace SharpPhysics._2d.ObjectRepresentation.Translation
{
	public class _2dTranslation
	{
		public _2dTranslation(int xPos, int yPos, int zPos)
		{
			ObjectPosition = new _2dPosition(xPos, yPos, zPos);
			ObjectRotation = new _2dRotation(0);
			ObjectScale = new _2dScale(1, 1, 1);
		}
		public _2dTranslation()
		{
			ObjectPosition = new _2dPosition(0, 0, 0);
			ObjectRotation = new _2dRotation(0);
			ObjectScale = new _2dScale(1, 1, 1);
		}
		public override string ToString()
		{
			return $"POS: {ObjectPosition}, ROT: {ObjectRotation}, SCA: {ObjectScale}";
		}
		public _2dPosition ObjectPosition;
		public _2dRotation ObjectRotation;
		public _2dScale ObjectScale;
	}
}