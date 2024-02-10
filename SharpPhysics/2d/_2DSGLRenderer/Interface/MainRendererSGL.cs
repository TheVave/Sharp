using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Renderer;

namespace SharpPhysics
{
	public static class MainRendererSGL
	{
		public static Internal2dRenderer renderer = new();

		/// <summary>
		/// The objects to render
		/// </summary>
		public static SGLRenderedObject[] ObjectsToRender
		{
			get
			{
				return renderer.objectToRender;
			}
			set
			{
				renderer.objectToRender = value;
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
		public static string Title { get
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
		public static Action<_2dSimulatedObject>[] OnRender { 
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
		public static Action<_2dSimulatedObject>[] OnUpdate
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
			renderer.ISGL();
		}
	}
}