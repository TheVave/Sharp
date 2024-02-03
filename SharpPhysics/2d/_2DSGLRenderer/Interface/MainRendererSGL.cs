using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class MainRendererSGL
	{
		public static Internal2dRenderer renderer = new();

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
		/// Starts rendering
		/// </summary>
		public static void InitRendering()
		{
			renderer.wndSize = new _2d._2DSGLRenderer.Size(1080, 1500);
			renderer.ISGL();
		}
	}
}
