using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics.Renderer
{
	public class Color
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
				
				R = byte.Parse(rgbVal[0..3]);
				G = byte.Parse(rgbVal[3..6]);
				B = byte.Parse(rgbVal[6..9]);
			}
			catch
			{
				// if some error has happened, like some color not being formatted correctly.
				ErrorHandler.ThrowError("Error, Internal Error, Color.Color(ColorName key) failed with input " + key.ToString(), true);
			}
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
