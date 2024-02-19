using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SharpPhysics.UI.UIElements
{
	public class Container : IUIElement
	{
		public IUIElement[] Children;
		public bool Visible = true;
		public Action OnDraw = () => { };

		public bool Draw(ImGuiController controller, bool useNormalImGuiWnd)
		{
			if (Visible)
			{
				foreach (IUIElement child in Children)
				{
					child.Draw(controller, useNormalImGuiWnd);
				}
			}
			OnDraw.Invoke();
			return true;
		}
	}
}
