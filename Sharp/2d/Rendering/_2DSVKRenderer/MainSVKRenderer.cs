using Sharp._2d._2DSVKRenderer.Main;
using Sharp.Utilities;

namespace Sharp._2d.Rendering._2DSVKRenderer
{
	public class MainSVKRenderer
	{
		public static Internal2dSVKRenderer rndr = new();

		public static VKDrawPlatform DrawingPlatform { get
			{
				return rndr.Platform;
			} 
		set
			{
				rndr.Platform = value;
			}
		}

		/// <summary>
		/// This begins rendering!
		/// Make sure that you have DrawingPlatform set to your current platform.
		/// </summary>
		public static void InitRendering()
		{
			rndr.INITRNDRNG();
		}

		public static void RunExampleApplication()
		{
			var app = new HelloTriangleApplication();
			app.Run();
		}
	}
}
