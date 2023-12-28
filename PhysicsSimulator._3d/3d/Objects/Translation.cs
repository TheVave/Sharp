namespace SharpPhysics
{
	public struct Translation
	{
		public Translation(int xPos, int yPos, int zPos)
		{
			ObjectPosition = new Position(xPos, yPos, zPos);
			ObjectRotation = new Rotation(0, 0, 0);
			ObjectScale = new Scale(1, 1, 1);
		}
		public override string ToString()
		{
			return $"POS: {ObjectPosition}, ROT: {ObjectRotation}, SCA: {ObjectScale}";
		}
		public Position ObjectPosition;
		public Rotation ObjectRotation;
		public Scale ObjectScale;
		public struct Position
		{
			public override string ToString()
			{
				return $"{xPos},{yPos},{zPos}";
			}
			public Position(int x, int y, int z)
			{
				xPos = x;
				yPos = y;
				zPos = z;
			}
			public Position()
			{
				xPos = 0;
				yPos = 0;
				zPos = 0;
			}
			public double xPos;
			public double yPos;
			public double zPos;
		}
		public struct Rotation
		{
			public override string ToString()
			{
				return $"{xRot},{yRot},{zRot}";
			}
			public Rotation(float x, float y, float z)
			{
				xRot = x;
				yRot = y;
				zRot = z;
			}
			public Rotation()
			{
				xRot = 0;
				yRot = 0;
				zRot = 0;
			}
			public float xRot;
			public float yRot;
			public float zRot;
		}
		public struct Scale
		{
			public override string ToString()
			{
				return $"{xSca},{ySca},{zSca}";
			}
			public Scale(int x, int y, int z)
			{
				xSca = x;
				ySca = y;
				zSca = z;
			}
			public Scale()
			{
				xSca = 0;
				ySca = 0;
				zSca = 0;
			}
			public int xSca;
			public int ySca;
			public int zSca;
		}
	}
}