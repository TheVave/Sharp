
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpPhysics
{
	/// <summary>
	/// This class (is going to) manage most of the rendering computation.
	/// Currently it does not need to be used when you are making the simulation using
	/// MonoGame
	/// </summary>
	public static class MainRenderer
	{
		/// <summary>
		/// Simlar to the render distance in games.
		/// Warning: Currently SharpPhysics does not have any sort
		/// of reduced appearance based on distance so be careful with
		/// this value
		/// </summary>
		public static int maxRenderLength = 100;

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
		public static Tuple<int, int> WndSize = Tuple.Create(1920, 1200);

		/// <summary>
		/// repersents the window size as a string
		/// </summary>
		private static string wndSizeStr = "1920x1200";

		/// <summary>
		/// The direct pixels that are being displayed. Can be set to, for one
		/// frame set the pixel(s) to be whatever you set it to. The array has
		/// a length of WndSize.Item1 * WndSize.Item2.
		/// </summary>
		public static Pixel[] Pixels = new Pixel[2304000];

		/// <summary>
		/// If the renderer should use MonoGame for rendering
		/// </summary>
		public static bool UseMonoGame = true;

		/// <summary>
		/// The scene to be rendered
		/// </summary>
		public static int sceneRendered = 0;

		/// <summary>
		/// The things that must be done before the main StartRenderer function.
		/// Initilizes pixels (reserves correct memory for them).
		/// </summary>
		internal static void StartRendererPres()
		{
			// the x pos is i
			int i = 0;
			// the y pos is j
			int j = 0;
			// starting loop to initilize the pixels and set they're positions
			// p is the pixel index (in the Pixels pixel array) the loop will be assigning the positions to
			for (int p = 0; p < Pixels.Length; p++)
			{
				// initilizing the pixels with a color of 255 255 255 (default white)
				Pixels[p] = new();
				// if the x pos is the size of the window, then increment the pixel y pos
				if (i == WndSize.Item1)
				{
					// incrementing the pixel y pos
					j++;
					// setting the x pos to zero so it doesn't go to a y pos of 1200 and a x pos of 2.3 million
					i = 0;
				}
				// setting the pixel x and y positions
				Pixels[p].x = (short)i;
				Pixels[p].y = (short)j;
				// incrementing x
				i++;
			}
		}

		/// <summary>
		/// Starts the main renderer.
		/// </summary>
		/// <param name="is2D">
		/// If the renderer should be 2d or if false, it will be 3d
		/// </param>
		public static void StartRenderer(bool is2D)
		{
			StartRendererPres();
			try
			{
				// starting the display.exe process to display the pixels
				// creating the pixels object.
				Process process = new();
				// setting the position of the exe file
				process.StartInfo = new ProcessStartInfo($"{Environment.CurrentDirectory}\\display.exe");
				// setting the window size in the args for the program
				process.StartInfo.Arguments = $"{WndSize.Item1}x{WndSize.Item2}";
				unsafe
				{
					// finding the ptr to the first pixel in the pixel[]
					fixed (Pixel* ptr = &Pixels[0])
					{
						// setting the next part of the argument. e.g args "1920x1200,0CD9B4" or something...
						process.StartInfo.Arguments += $",{(int)ptr:X}";
					}
				}
				process.Start();
			}
			catch (NullReferenceException excep)
			{
				// Error I don't remember the cause of
				ErrorHandler.ThrowError("Error, Internal Error, Error 1", true);
			}
			catch
			{
				ErrorHandler.ThrowError("Error, External Error, Error 2, missing display.exe", false);
				//ErrorHandler.YesNoQuestion("Do you want to attempt to fix this?", "Do you?", true);
				// /\
				// ||
				// soon to be implemented, currently just throws a NotImplementedException
			}
			Is2D = is2D;
			// starts the main rendering thread.
			new Thread(() =>
			{
				while (true)
				{
					// delays for frame rate, it does not need to calculate frames for things the user can't see
					Task.Delay(delayForFrameRate);
					// renders frames
					RenderFrame(Is2D);
				}
			});

			
		}
		// test for the canvas with a white color
		public static void TestCanvas()
		{
			for (int i = 0; i++ < Pixels.Length;)
			{
				// sets the pixel color to white
				Pixels[i].color = new char[] { (char)255, (char)255, (char)255 };
			}
		}
		/// <summary>
		/// Manages the frame rendering
		/// </summary>
		/// <param name="is2D"></param>
		internal static void RenderFrame(bool is2D)
		{
			if (is2D) _2dRenderer.RenderFrame();
			else ErrorHandler.ThrowNotImplementedExcepetion();
		}
		/// <summary>
		/// name is self-explanitory, it tests the display.exe window.
		/// Use this if nothing else is working, this just tests the ability for the window
		/// to display pixels that can't be controlled by the progam.
		/// </summary>
		public static void TestDisplayWindow()
		{
			try
			{
				Process process = new();
				process.StartInfo = new ProcessStartInfo($"{Environment.CurrentDirectory}\\display.exe");
				process.StartInfo.Arguments = $"false";
				process.Start();
			}
			catch
			{
				ErrorHandler.ThrowError("Error 2, External Error, Missing display.exe", true);
			}
		}
		/// <summary>
		/// a method that sets the frame rate to a spesific speed. called when you set the FrameRate property
		/// </summary>
		/// <param name="rate"></param>
		public static void SetFrameRate(int rate)
		{
			TargetFrameRate = rate;
			delayForFrameRate = 1000 / rate;
		}
	}
}