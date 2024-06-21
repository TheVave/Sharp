using Sharp._2d._2DSVKRenderer.Main;

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
	}
}
