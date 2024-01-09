
using GLFW;
using SharpPhysics.Renderer.Demos;
using SharpPhysics.Renderer.Tests;
namespace SharpPhysics.Renderer
{
	/// <summary>
	/// This class (is going to) manage most of the rendering computation.
	/// Currently it does not need to be used when you are making the simulation using
	/// MonoGame
	/// </summary>
	public static class MainRenderer
	{
		/// <summary>
		/// Similar to the render distance in games.
		/// Warning: Currently SharpPhysics does not have any sort
		/// of reduced appearance based on distance so be careful with
		/// this value
		/// </summary>
		public static int maxRenderLength = 100;

		/// <summary>
		/// The title to display for the app window.
		/// </summary>
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static string WindowTitle = "SharpPhysics";
#pragma warning restore CA2211 // Non-constant fields should not be visible

		public static StandardDisplay Display;

		/// <summary>
		/// the target FPS, set with SetFrameRate.
		/// </summary>
		public static int TargetFrameRate { get; private set; } = 60;

		/// <summary>
		/// internally used for speed reasons, to reduce division, relative to targetFrameRate
		/// </summary>
		internal static int delayForFrameRate = 16;

		/// <summary>
		/// bool that shows if the Renderer should treat the canvas element as a 2d or 3d display port
		/// </summary>
		public static bool Is2D;

		/// <summary>
		/// Window size as a Tuple<int,int> for x and y, x to Item1, and y to Item2
		/// </summary>
		public static Tuple<int, int> WndSize = Tuple.Create(800, 600);

		/// <summary>
		/// The scene to be rendered
		/// </summary>
		public static int sceneRendered = 0;

		/// <summary>
		/// What the renderer should execute before the StandardDisplay.Init method.
		/// </summary>
		public static event EventHandler ExecuteBeforeLoad;

		public static event EventHandler ExecuteAfterLoad;

		public static event EventHandler TextureLoader;

		private static bool startRun = false;
		public static void InitRendering()
		{
			Glfw.Init();
			Game game = new StandardDisplay(800, 600, "SharpPhysics");
			Display = (StandardDisplay)game;
			if (ExecuteBeforeLoad is not null)
				ExecuteBeforeLoad.Invoke(null, null);
			game.Run();
		}

		internal static void ExecuteAfterLoad_()
		{
			if (ExecuteAfterLoad is not null)
				ExecuteAfterLoad.Invoke(null, null);
		}

		internal static void TextureLoad()
		{
			if (TextureLoader is not null)
				TextureLoader.Invoke(null, null);
		}

		public static void TriangleTest()
		{
			HelloTriangle.Main([]);
		}

		/// <summary>
		/// a method that sets the frame rate to a specific speed. called when you set the FrameRate property
		/// </summary>
		/// <param name="rate"></param>
		public static void SetFrameRate(int rate)
		{
			TargetFrameRate = rate;
			delayForFrameRate = 1000 / rate;
		}
	}
}