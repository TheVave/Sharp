using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class InputManager
	{
		public VirtualKey[] KeysPressed { get; private set; } = new VirtualKey[0];
		public bool LeftMouseDown { get; private set; } = false;
		public bool RightMouseDown { get; private set; } = false;
		public bool MiddleMouseDown { get; private set; } = false;
		public Tuple<ushort, ushort> MousePos { get; private set; } = Tuple.Create(ushort.MinValue, ushort.MinValue);
		internal void UpdateKeyboardValues(VirtualKey[] keys)
		{
			KeysPressed = keys;
		}
		internal void UpdateMouseValues(bool leftDown, bool rightDown,bool middleDown, Tuple<ushort, ushort> mousePos)
		{
			LeftMouseDown = leftDown;
			RightMouseDown = rightDown;
			MiddleMouseDown = middleDown;
			MousePos = mousePos;
		}
	}
}
