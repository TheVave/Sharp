using SharpPhysics._2d._2DSGLESRenderer.Main;
using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.Renderer._2DSGLESRenderer.Main;
using SharpPhysics.Renderer;
using System.Drawing;

namespace SharpPhysics
{
	public static class MainRendererSGLES
	{
		public static Internal2dRendererES renderer = new();

		/// <summary>
		/// If the program should instruct OpenGL to use 8-bit style textures.
		/// </summary>
		public static bool Use8BitStyleTextures = false;

		/// <summary>
		/// If the program should zoom the camera on the scroll wheel.
		/// </summary>
		public static bool UseCamZoom = true;

		/// <summary>
		/// The zoom speed multiplier
		/// </summary>
		public static double ZoomIntensity = 1;

		public static Size WndSize = new Size(600, 800);

		/// <summary>
		/// The objects to render
		/// </summary>
		public static SGLESRenderedObject[] ObjectsToRender
		{
			get
			{
				return renderer.ObjectsToRender;
			}
			set
			{
				renderer.ObjectsToRender = value;
			}
		}

		/// <summary>
		/// The camera to render all of the objects to.
		/// </summary>
		public static _2dESCamera Camera
		{
			get
			{
				return renderer.Cam;
			}
			set
			{
				renderer.Cam = value;
			}
		}

		/// <summary>
		/// The background color
		/// </summary>
		public static Renderer.Color BackgroundColor
		{
			get
			{
				return renderer.clearBufferBit;
			}
			set
			{
				renderer.clearBufferBit = value;
			}
		}

		/// <summary>
		/// The window title. Cannot be changed after InitRendering is called.
		/// </summary>
		public static string Title
		{
			get
			{
				return renderer.title;
			}
			set
			{
				renderer.title = value;
			}
		}

		/// <summary>
		/// Called once per frame
		/// </summary>
		public static Action<SimulatedObject2d>[] OnRender
		{
			get
			{
				return renderer.OR;
			}
			set
			{
				renderer.OR = value;
			}
		}

		/// <summary>
		/// Called once per frame. You can't use SGL here.
		/// </summary>
		public static Action<SimulatedObject2d>[] OnUpdate
		{
			get
			{
				return renderer.OU;
			}
			set
			{
				renderer.OU = value;
			}
		}

		/// <summary>
		/// Called once.
		/// </summary>
		public static Action OnLoad
		{
			get
			{
				return renderer.OL;
			}
			set
			{
				renderer.OL = value;
			}
		}

		/// <summary>
		/// Starts rendering
		/// </summary>
		public static void InitRendering()
		{
			renderer.wndSize = new _2d._2DSGLRenderer.Size(1080, 1500);
			renderer.ISGLES();
		}
	}
}