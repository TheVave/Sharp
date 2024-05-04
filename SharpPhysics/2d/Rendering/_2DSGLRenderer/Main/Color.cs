using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics.Renderer
{
	public class Color : IAny
	{
		/// <summary>
		/// The colors possible with a standard RGBA style.
		/// </summary>
		public byte R = 255, G = 255, B = 255, A = 255;

		/// <summary>
		/// Creates a white color
		/// </summary>
		public Color() { }

		/// <summary>
		/// Creates a color from the inputs r,g,b, and a.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public Color(byte r, byte g, byte b, byte a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}
		/// <summary>
		/// Returns a color from the key.
		/// </summary>
		/// <param name="key">
		/// the color name, e.g gray.
		/// </param>
		public Color(ColorName key)
		{
			string rgbVal = ((int)key).ToString();
			try
			{
				// interprets the ColorName style.
				// the int for the ColorName enum is RGB, e.g 255255255 (White)

				// added dummy 1 at the start to trick .net into including the zeros
				R = byte.Parse(rgbVal[1..4]);
				G = byte.Parse(rgbVal[4..7]);
				B = byte.Parse(rgbVal[7..10]);
			}
			catch
			{
				// if some error has happened, like some color not being formatted correctly.
				//8
				ErrorHandler.ThrowError(8, true);
			}
		}
		/// <summary>
		/// Returns a color with the specified rgb values and a alpha of 255
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}

		/// <summary>
		/// an updated ToString method that shows R,G,B
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{R},{G},{B}";
		}
	}
}
