using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation.Translation
{
	public class Scale : ISizeGettable, IAny
	{
		public override string ToString()
		{
			return $"Scale:{{XSca:{XSca},YSca:{YSca},ZSca:{ZSca}}}";
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Scale);
			}
		}

		public Scale(short x, short y)
		{
			XSca = x;
			YSca = y;
		}
		public Scale()
		{
			XSca = 0;
			YSca = 0;
		}
		/// <summary>
		/// Scale in pixels
		/// </summary>
		public short XSca, YSca, ZSca = 0;
	}
}
