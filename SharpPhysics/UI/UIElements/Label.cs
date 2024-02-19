using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Reflection.Metadata.Ecma335;

namespace SharpPhysics.UI.UIElements
{
	public class Label : IUIElement
	{
		public string Txt;
		public bool Visible = true;
		public Action OnDraw = () => { };

		public Label(string txt)
		{
			Txt = txt;
		}

		public bool Draw(ImGuiController controller, bool useNormalImGuiWnd)
		{
			try
			{
				if (Visible)
				{
					ImGui.Text(Txt);
				}
				OnDraw.Invoke();
				return true;
			}
			catch
			{
				return false;
			}

		}
	}
}
