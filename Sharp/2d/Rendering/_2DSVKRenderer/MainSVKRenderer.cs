﻿using Sharp._2d._2DSVKRenderer.Main;
using Sharp.Utilities;

namespace Sharp._2d.Rendering._2DSVKRenderer
{
	public class MainSVKRenderer
	{
		static Internal2dSVKRenderer rndr = new();

		// starts rendering
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
