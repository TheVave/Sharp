
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
		static extern short GetAsyncKeyState(int VirtualKeyPressed);
		public static VirtualKey[] KeysDwn = Array.Empty<VirtualKey>();
		public static void InitKybrdThreads()
		{
			try
			{
				[DllImport("user32.dll")]
				static extern short GetAsyncKeyState(int VirtualKeyPressed);
			}
			catch
			{
				ErrorHandler.ThrowError("Error, External, User32.dll missing, the program cannot continue.", true);
			}
			for (int i = 0; i < 255; i++)
			{
				Thread thrd = new Thread(() =>
				{
					int keystroke;
					VirtualKey keyToDetect;
					byte[] result;
					while (true)
					{
						Task.Delay(17).Wait();
						keyToDetect = (VirtualKey)i;
						keystroke = GetAsyncKeyState(i);

						result = BitConverter.GetBytes(GetAsyncKeyState(keystroke));

						if (result[0] == 1)
						{
							KeysDwn = KeysDwn.Append(keyToDetect).ToArray();

						}

						if (result[1] == 0x80)
						{
							KeysDwn = KeysDwn.Append(keyToDetect).ToArray();

						} 

						if (KeysDwn.Contains(keyToDetect))
						{
							KeysDwn[Array.IndexOf(KeysDwn, keyToDetect)] = VirtualKey.CANCEL;
						}
					}
				});
					
			}
		}
	}
}
