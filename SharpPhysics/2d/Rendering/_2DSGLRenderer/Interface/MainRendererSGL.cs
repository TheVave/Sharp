using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Renderer;

namespace SharpPhysics
{
	public static class MainRendererSGL
	{
		public static Internal2dRenderer renderer = new();

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

		/// <summary>
		/// The objects to render scene id
		/// </summary>
		public static short SceneIDToRender;

		/// <summary>
		/// The camera to render all of the objects to.
		/// </summary>
		public static _2dCamera Camera
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
		public static Color BackgroundColor
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
		/// The objects to render.
		/// </summary>
		public static SGLRenderedObject[] ObjectsToRender
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
		/// Starts rendering
		/// </summary>
		public static void InitRendering()
		{
			renderer.wndSize = new _2d._2DSGLRenderer.Size(1080, 1500);
			renderer.ISGL();
		}
	}
}