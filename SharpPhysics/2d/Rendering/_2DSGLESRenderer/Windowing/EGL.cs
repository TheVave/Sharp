using Silk.NET.Windowing;
using SharpPhysics._2d._2DSGLRenderer;
using Silk.NET.SDL;
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
			options.API = new GraphicsAPI(ContextAPI.OpenGLES, ContextProfile.Compatability, ContextFlags.Default, new APIVersion(3, 0));
			vw = Silk.NET.Windowing.Window.GetView(options);
			vw.Load += Vw_Load;
			vw.Render += Vw_Render;
			vw.Update += Vw_Update;
			vw.Run();
		}

		private static void Vw_Update(double obj)
		{
			throw new NotImplementedException();
		}

		private static void Vw_Render(double obj)
		{
			throw new NotImplementedException();
		}

		private static void Vw_Load()
		{
			throw new NotImplementedException();
		}
	}
}