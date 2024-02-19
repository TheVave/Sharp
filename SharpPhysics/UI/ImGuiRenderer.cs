using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;
using SharpPhysics.UI.UIHierarchy;

namespace SharpPhysics.UI
{
	public class ImGuiRenderer
	{
		ImGuiController controller;
		float f = 0;
		string content = "Test";
		byte[] buf = [];

		public virtual void LD(IWindow wnd, GL gl)
		{
			// initializes ImGui
			InitCntxt(wnd, gl);
		}
		public virtual void InitCntxt(IWindow wnd, GL gl)
		{
			controller = new ImGuiController(
				gl, // load OpenGL
				wnd, // pass in our window
				MainRendererSGL.renderer.inputContext
			);
		}
		public virtual unsafe void ImGuiRndr(double deltatime)
		{
			controller.Update((float)deltatime);

			foreach (UIWindow wnd in UIRoot.Windows)
			{
				ImGui.Begin(wnd.Title);
				foreach (IUIElement element in wnd.Elements)
				{
					element.Draw(controller, false);
				}
				ImGui.End();
			}

			controller.Render();
		}
	}
}
