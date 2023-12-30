namespace SharpPhysics.Input
{
	/// <summary>
	/// Gives a top level overveiw of all of the buttons pressed/mouse position
	/// </summary>
	public static class InputManager
	{

		/// <summary>
		/// True if the left mouse button is down and false otherwise
		/// </summary>
		public static bool LeftMouseDown { get; private set; } = false;

		/// <summary>
		/// True if the right mouse button is down and false otherwise
		/// </summary>
		public static bool RightMouseDown { get; private set; } = false;

		/// <summary>
		/// True if the middle mouse button is down and false otherwise
		/// </summary>
		public static bool MiddleMouseDown { get; private set; } = false;

		/// <summary>
		/// The mouse position stored as a Tuple<ushort,ushort> for speed reasons.
		/// </summary>
		public static Tuple<ushort, ushort> MousePos { get; private set; } = Tuple.Create(ushort.MinValue, ushort.MinValue);

		public static bool IsKeyDown(VirtualKey key) => Keyboard.KeyboardInput.IsKeyDown(key);

		/// <summary>
		/// internal method for quickly updating some values.
		/// Used for the MouseInput class.
		/// </summary>
		/// <param name="leftDown"></param>
		/// <param name="rightDown"></param>
		/// <param name="middleDown"></param>
		/// <param name="mousePos"></param>
		internal static void UpdateMouseValues(bool leftDown, bool rightDown, bool middleDown, Tuple<ushort, ushort> mousePos)
		{
			LeftMouseDown = leftDown;
			RightMouseDown = rightDown;
			MiddleMouseDown = middleDown;
			MousePos = mousePos;
		}
	}
}
