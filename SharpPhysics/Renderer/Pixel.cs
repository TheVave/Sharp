namespace SharpPhysics.Renderer
{
	/// <summary>
	/// default pixel class designed for compatibility with C++
	/// </summary>
	// It is made with compatibility with C++ because of the way I am planning on doing
	// rendering. This uses a C++ function to display things on a window. See FastPixelWriter
	public class Pixel
	{
		/// <summary>
		/// the x pos of the pixel
		/// </summary>
		public short x;
		/// <summary>
		/// the y pos of the pixel.
		/// </summary>
		public short y;
		/// <summary>
		/// the color of the pixel
		/// </summary>
		public char[] color = { (char)255, (char)255, (char)255 };

		/// <summary>
		/// ToString method that returns the format "R,G,B"
		/// </summary>
		/// <returns></returns>
		// may be changed with more implementations later.
		public override string ToString()
		{
			return $"{color[0]},{color[1]},{color[2]}";
		}
	}
}
