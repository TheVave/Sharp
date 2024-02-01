using SharpPhysics._2d._2DSGLRenderer.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class MainRendererSGL
	{
		public static InternalMainRenderer2d renderer = new();

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

		}
	}
}
