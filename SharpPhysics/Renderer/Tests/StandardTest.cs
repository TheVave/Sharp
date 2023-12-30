using SharpPhysics.Renderer.GameLoop;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;

namespace SharpPhysics.Renderer.Tests
{
	internal class StandardTest : Game
	{
		

		public StandardTest(int initialWindowWidth, int initialWindowHeight, string windowTitle) : base(initialWindowWidth, initialWindowHeight, windowTitle)
		{
		}

		protected override void Draw()
		{
			glClearColor((float)(MathF.Sin((float)GameTime.TotalElapsedSeconds) * 0.5) + 0.5f, 0, 0, 1);
			glClear(GL_COLOR_BUFFER_BIT);

			Glfw.SwapBuffers(Display.DisplayManager.Window);
		}

		protected override void Init()
		{
		}

		protected override void LoadContent()
		{
		}

		protected override void Update()
		{
		}
	}
}
