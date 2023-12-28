using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	/// <summary>
	/// Gives a top level overveiw of all of the buttons pressed/mouse position
	/// </summary>
	public class InputManager
	{
		/// <summary>
		/// The keys pressed.
		/// This includes mouse keys.
		/// </summary>
		public VirtualKey[] KeysPressed { get; private set; } = new VirtualKey[0];

		/// <summary>
		/// True if the left mouse button is down and false otherwise
		/// </summary>
		public bool LeftMouseDown { get; private set; } = false;

		/// <summary>
		/// True if the right mouse button is down and false otherwise
		/// </summary>
		public bool RightMouseDown { get; private set; } = false;

		/// <summary>
		/// True if the middle mouse button is down and false otherwise
		/// </summary>
		public bool MiddleMouseDown { get; private set; } = false;

		/// <summary>
		/// The mouse position stored as a Tuple<ushort,ushort> for speed reasons.
		/// </summary>
		public Tuple<ushort, ushort> MousePos { get; private set; } = Tuple.Create(ushort.MinValue, ushort.MinValue);

		/// <summary>
		/// internal method for quickly updating some values.
		/// Used in the KeyboardInput class.
		/// </summary>
		/// <param name="keys"></param>
		internal void UpdateKeyboardValues(VirtualKey[] keys)
		{
			KeysPressed = keys;
		}

		/// <summary>
		/// internal method for quickly updating some values.
		/// Used for the MouseInput class.
		/// </summary>
		/// <param name="leftDown"></param>
		/// <param name="rightDown"></param>
		/// <param name="middleDown"></param>
		/// <param name="mousePos"></param>
		internal void UpdateMouseValues(bool leftDown, bool rightDown,bool middleDown, Tuple<ushort, ushort> mousePos)
		{
			LeftMouseDown = leftDown;
			RightMouseDown = rightDown;
			MiddleMouseDown = middleDown;
			MousePos = mousePos;
		}
	}
}
