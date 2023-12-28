
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class KeyboardInput
	{
		// external import from user32.dll that handles the keyboard and left or right mouse input
		static extern short GetAsyncKeyState(int VirtualKeyPressed);
		
		/// <summary>
		/// The keys that are down
		/// WARNING!!! RUN InitKybrdThreads BEFORE ATTEMPTING TO USE
		/// </summary>
		public static VirtualKey[] KeysDwn = Array.Empty<VirtualKey>();

		/// <summary>
		/// Loads the threads that manage KeysDwn, must be run for any keyboard input
		/// to work
		/// </summary>
		public static void InitKybrdThreads()
		{
			try
			{
				[DllImport("user32.dll")]
				static extern short GetAsyncKeyState(int VirtualKeyPressed);
			}
			catch
			{
				// should only happen if the user OS is non-Windows
				// If they have Windows and user32.dll is missing they'll have
				// more important things to do than try to run a game/simulation.
				ErrorHandler.ThrowError("Error, External, User32.dll missing, the program cannot continue.", true);
			}
			for (int i = 0; i < 255; i++)
			{
				Thread thrd = new Thread(() =>
				{
					// the keystroke number.
					// for more info see Virtual Key Codes (https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes)
					int keystroke;

					// the key to detect. In the form of a VirtualKey enum copied from the above link
					VirtualKey keyToDetect;

					// the result from GetAsyncKeyState, put here for porformance reasons
					byte[] result;
					while (true)
					{
						Task.Delay(17).Wait();
						keyToDetect = (VirtualKey)i;
						keystroke = GetAsyncKeyState(i);

						// For info on how this works see ||
						//                                ||
						//                                \/
						// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getasynckeystate

						result = BitConverter.GetBytes(GetAsyncKeyState(keystroke));

						// In the link above (the Microsoft Learn link)
						// this is not recommended, yet I am using it because I do not beleive that
						// there is much of a reason not to.
						// this may change with testing.

						// If the button has been pressed since the privious call to GetAysncKeyState

						if (result[0] == 1)
						{
							if (!KeysDwn.Contains(keyToDetect)) KeysDwn = KeysDwn.Append(keyToDetect).ToArray();
							// else do nothing because that means the key is still down. 
						}

						// If the button is still down

						if (result[1] == 0x80)
						{
							if (!KeysDwn.Contains(keyToDetect)) KeysDwn = KeysDwn.Append(keyToDetect).ToArray();
							// the above will only happen in very strange cases.
						} 
					}
				});
					
			}
		}
	}
}
