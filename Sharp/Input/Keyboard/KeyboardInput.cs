using Sharp.StrangeDataTypes;
using Silk.NET.Input;
using System.Runtime.InteropServices;
namespace Sharp.Input.Keyboard
{
	public static class KeyboardInput
	{
		public static IInputContext context;


		///// <summary>
		///// The keys that are down
		///// WARNING!!! RUN InitKybrdThreads BEFORE ATTEMPTING TO USE
		///// </summary>
		//public static VirtualKey[] KeysDwn = Array.Empty<VirtualKey>();

		/// <summary>
		/// Loads the threads that manage KeysDwn, must be run for any keyboard input
		/// to work
		/// </summary>
		//public static void InitKybrdThreads()
		//{
		//	for (int i = 0; i < 255; i++)
		//	{
		//		Thread thrd = new Thread(() =>
		//		{
		//			// the keystroke number.
		//			// for more info see Virtual Key Codes (https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes)
		//			int keystroke;

		//			// the key to detect. In the form of a VirtualKey enum copied from the above link
		//			VirtualKey keyToDetect;

		//			// the result from GetAsyncKeyState, put here for porformance reasons
		//			byte[] result;
		//			while (true)
		//			{
		//				Task.Delay(17).Wait();
		//				keyToDetect = (VirtualKey)i;
		//				keystroke = GetAsyncKeyState(i);

		//				// For info on how this works see ||
		//				//                                ||
		//				//                                \/
		//				// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getasynckeystate

		//				result = BitConverter.GetBytes(GetAsyncKeyState(keystroke));

		//				// In the link above (the Microsoft Learn link)
		//				// this is not recommended, yet I am using it because I do not beleive that
		//				// there is much of a reason not to.
		//				// this may change with testing.

		//				// If the button has been pressed since the privious call to GetAysncKeyState

		//				if (result[0] == 1)
		//				{
		//					if (!KeysDwn.Contains(keyToDetect)) KeysDwn = KeysDwn.Append(keyToDetect).ToArray();
		//					// else do nothing because that means the key is still down. 
		//				}

		//				// If the button is still down

		//				if (result[0] == 0x80)
		//				{
		//					if (!KeysDwn.Contains(keyToDetect)) KeysDwn = KeysDwn.Append(keyToDetect).ToArray();
		//					// the above will only happen in very strange cases.
		//				}
		//			}
		//		});
		//		thrd.Start();
		//	}
		//}
		internal static bool IsKeyDown(VirtualKey key)
		{
			try
			{
				[DllImport("user32.dll")]
				static extern short GetAsyncKeyState(int VirtualKeyPressed);
				byte[] result = BitConverter.GetBytes(GetAsyncKeyState((int)key));
				if (result[0] == 1) return true;
				else if (result[0] == 0x80) return true;
				else return false;
			}
			catch
			{
				return false;
			}
		}
		internal static void AddKeyDown(Action<IKeyboard, Key, int> action)
		{
			context.Keyboards[0].KeyDown += action;
		}
		internal static void AddKeyUp(Action<IKeyboard, Key, int> action)
		{
			context.Keyboards[0].KeyUp += action;
		}
	}
}
