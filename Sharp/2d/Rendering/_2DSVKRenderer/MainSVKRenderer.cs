using Sharp._2d._2DSVKRenderer.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp._2d.Rendering._2DSVKRenderer
{
	public class MainSVKRenderer
	{
		static Internal2dSVKRenderer rndr = new();

		// starts rendering
		public static void InitRendering()
		{
			rndr.INITRNDR();
		}
	}
}
