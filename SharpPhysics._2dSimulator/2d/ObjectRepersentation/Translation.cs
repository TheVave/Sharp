namespace SharpPhysics
{
	public struct _2dTranslation
	{
		public _2dTranslation(int xPos, int yPos, int zPos)
		{
			ObjectPosition = new _2dPosition(xPos, yPos, zPos);
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
		public struct _2dPosition
		{
			public override string ToString()
			{
				return $"{xPos},{yPos},{zPos}";
			}
			public _2dPosition(int x, int y, int z)
			{
				xPos = x;
				yPos = y;
			}
			public _2dPosition()
			{
				xPos = 0;
				yPos = 0;
			}
			public double xPos;
			public double yPos;
			public double zPos;
		}
		public struct _2dRotation
		{
			public override string ToString()
			{
				return $"{xRot}";
			}
			public _2dRotation(float x)
			{
				xRot = x;
			}
			public _2dRotation()
			{
				xRot = 0;
			}
			public float xRot;
			public float yRot;
		}
		public struct _2dScale
		{
			public override string ToString()
			{
				return $"{xSca},{ySca},{zSca}";
			}
			public _2dScale(int x, int y, int z)
			{
				xSca = x;
				ySca = y;
			}
			public _2dScale()
			{
				xSca = 0;
				ySca = 0;
			}
			public int xSca;
			public int ySca;
			public int zSca;
		}
	}
}