using GLFW;
using SharpPhysics.Utilities.MISC;
namespace SharpPhysics.Renderer
{
	abstract public class Game
	{

		protected int InitialWindowWidth = 800;
		protected int InitialWindowHeight = 600;
		protected string WindowTitle = "SharpPhysics View Port";

		protected Game(int initialWindowWidth, int initialWindowHeight, string windowTitle)
		{
			InitialWindowWidth = initialWindowWidth;
			InitialWindowHeight = initialWindowHeight;
			WindowTitle = windowTitle;
		}
		public void Run()
		{
			Init();

			DisplayManager.CreateWindow(InitialWindowWidth, InitialWindowHeight, WindowTitle);

			LoadContent();

			while (!Glfw.WindowShouldClose(DisplayManager.Window))
			{
				GameTime.DeltaTime = Glfw.Time - GameTime.TotalElapsedSeconds;
				GameTime.TotalElapsedSeconds = Glfw.Time;

				Update();

				Glfw.PollEvents();

				Draw();
			}
			DisplayManager.CloseWindow();
			Environment.Exit(0);
		}

		/// <summary>
		/// You can't use OpenGL here
		/// </summary>
		protected abstract void Init();
		protected abstract void Update();
		protected abstract void Draw();
		protected abstract void LoadContent();
	}
}
