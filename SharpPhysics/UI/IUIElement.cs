using Silk.NET.OpenGL.Extensions.ImGui;

namespace SharpPhysics.UI
{
	public interface IUIElement
	{
		public bool Draw(ImGuiController controller, bool useNormalImGuiWnd);
	}
}
