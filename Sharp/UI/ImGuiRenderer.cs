using ImGuiNET;
using Sharp.StrangeDataTypes;
using Sharp.UI.UIHierarchy;
using Silk.NET.Windowing;
using System.Numerics;

namespace Sharp.UI
{
	public class ImGuiRenderer : IAny
	{
		Silk.NET.OpenGL.Extensions.ImGui.ImGuiController controller;
		Silk.NET.OpenGLES.Extensions.ImGui.ImGuiController controlleres;

		public bool UseWindows = true;

		public virtual void LD(IWindow wnd, Silk.NET.OpenGL.GL gl)
		{
			// initializes ImGui
			InitCntxt(wnd, gl);
		}
		public virtual void LD(IView wnd, Silk.NET.OpenGLES.GL gl)
		{
			// initializes ImGui
			InitCntxt(wnd, gl);
		}
		public virtual void InitCntxt(IWindow wnd, Silk.NET.OpenGL.GL gl)
		{
			controller = new Silk.NET.OpenGL.Extensions.ImGui.ImGuiController(
				gl,
				wnd,
				MainRendererSGL.renderer.inputContext
			);
		}
		public virtual void InitCntxt(IView wnd, Silk.NET.OpenGLES.GL gl)
		{
			controlleres = new Silk.NET.OpenGLES.Extensions.ImGui.ImGuiController(
				gl,
				wnd,
				MainRendererSGL.renderer.inputContext
			);
		}
		public virtual unsafe void ImGuiRndr(double deltatime)
		{
			controller.Update((float)deltatime);


			//MainRendererSGL.renderer.gl.ClearColor(MainRendererSGL.renderer.clearBufferBit.R, MainRendererSGL.renderer.clearBufferBit.G, MainRendererSGL.renderer.clearBufferBit.B, MainRendererSGL.renderer.clearBufferBit.A);
			foreach (UIWindow wnd in UIRoot.Windows)
			{
				ImGui.Begin(wnd.Title);
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
