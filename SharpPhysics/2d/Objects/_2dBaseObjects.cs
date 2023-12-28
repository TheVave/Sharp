namespace SharpPhysics
{
	public static class _2dBaseObjects
	{
		public static Mesh LoadSquareMesh()
		{
			return new(new double[] { 2, -2, -2, 2 }, new double[] { 2, 2, -2, -2 }, new double[] { 0, 0, 0, 0 });
		}
	}
}
