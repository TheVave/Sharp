using Sharp.Input.Keyboard;
using Sharp.StrangeDataTypes;
using Silk.NET.Input;

namespace Sharp.Simulation
{
	internal class AutoRuns : IAny
	{
		public AutoRuns()
		{
			KeyboardInput.AddKeyDown((IKeyboard keybrd, Key key, int i) =>
			{
				if (key == Key.Menu)
					Environment.Exit(3);
			});
		}
	}
}
