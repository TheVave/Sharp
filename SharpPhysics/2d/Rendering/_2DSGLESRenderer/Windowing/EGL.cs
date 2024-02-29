using Silk.NET.Windowing.Sdl;
using Silk.NET.Windowing;
using SharpPhysics._2d._2DSGLRenderer;
namespace SharpPhysics._2d.Rendering._2DSGLESRenderer.Windowing
{
	public class SDLTester
	{
		public static Size WndSze { get; internal set; }
		public static string WndTitle { get; internal set; }
		public static IView vw { get; internal set; }

		public static unsafe void Run()
		{
			var options = ViewOptions.Default;
			options.API = new GraphicsAPI(ContextAPI.OpenGLES, ContextProfile.Core, ContextFlags.Default, new APIVersion(3, 2));
			vw = Window.GetView(options);
			vw.Run();
		}
	}
}