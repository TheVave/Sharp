using Silk.NET.OpenGL.Extensions.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.UI
{
	public interface IUIElement
	{
		public bool Draw(ImGuiController controller, bool useNormalImGuiWnd);
	}
}
