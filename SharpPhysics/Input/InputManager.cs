using SharpPhysics.Input.Keyboard;
using Silk.NET.Input;

namespace SharpPhysics.Input
{
	/// <summary>
	/// Gives a top level overview of all of the buttons pressed/mouse position
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
		/// Action is called when a key is pressed
		/// </summary>
		internal static Action<VirtualKey> OnKeyDown = (VirtualKey key) => { };

		/// <summary>
		/// The mouse position stored as a Tuple<ushort,ushort> for speed reasons.
		/// </summary>
		public static Tuple<ushort, ushort> MousePos { get; private set; } = Tuple.Create(ushort.MinValue, ushort.MinValue);

		public static bool IsKeyDown(VirtualKey key)
		{
			if (Keyboard.KeyboardInput.IsKeyDown(key))
			{
				OnKeyDown.Invoke(key);
				return true;
			}
			return false;
		}

		public static void AddKeyDownEvent(Action<IKeyboard, Key, int> action)
		{
			KeyboardInput.AddKeyDown(action);
		}
		public static void AddKeyDownEvent(Action<IKeyboard, Key, int> action)
		{
			KeyboardInput.AddKeyUp(action);
		}

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
