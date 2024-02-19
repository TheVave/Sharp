using ImGuiNET;
using SharpPhysics.UI.UIHierarchy;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System.Numerics;

namespace SharpPhysics.UI
{
	public class ImGuiRenderer
	{
		ImGuiController controller;

		public bool UseWindows = true;

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

			
			//MainRendererSGL.renderer.gl.ClearColor(MainRendererSGL.renderer.clearBufferBit.R, MainRendererSGL.renderer.clearBufferBit.G, MainRendererSGL.renderer.clearBufferBit.B, MainRendererSGL.renderer.clearBufferBit.A);
			foreach (UIWindow wnd in UIRoot.Windows)
			{				ImGui.Begin(wnd.Title);
				foreach (IUIElement element in wnd.Elements)
				{
					if (element.Position != Vector2.Zero)
						ImGui.SetCursorPos(element.Position);
					element.Draw();
				}
				ImGui.End();
			}
			ImGui.Begin("#", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoBackground);
			//MainRendererSGL.renderer.gl.ClearColor(0, 0, 0, 0);
			foreach (IUIElement element in UIRoot.NonWindowedObjects)
			{
				if (element.Visible)
				{
					if (element.Position != Vector2.Zero)
						ImGui.SetCursorPos(element.Position);
					element.Draw();
				}
			}
			ImGui.End();

			controller.Render();
		}
	}
}
