using Silk.NET.Windowing.Sdl.Android;
using Silk.NET.Windowing;
namespace PhoneTester;

[Activity(Label = "Debugging Application", MainLauncher = true)]
public class MainActivity : SilkActivity
{
	protected override void OnRun()
	{
		IView vw;

		var options = ViewOptions.Default;
		options.API = new GraphicsAPI(ContextAPI.OpenGLES, ContextProfile.Compatability, ContextFlags.Default, new APIVersion(3, 0));
		vw = Silk.NET.Windowing.Window.GetView(options);
		vw.Run();
	}
}