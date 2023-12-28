namespace SharpPhysics
{
	public static class ArrayUtils
	{
		public static void ArrayAdd(ref object[] objs, object obj)
		{
			objs = objs.Append(obj).ToArray();
		}
	}
}
