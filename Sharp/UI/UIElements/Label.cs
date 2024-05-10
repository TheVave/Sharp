using ImGuiNET;
using Sharp.StrangeDataTypes;
using System.Numerics;

namespace Sharp.UI.UIElements
{
	public class Label : IUIElement, IAny
	{
		public string Txt;
		public bool Visible { get; set; } = true;
		public float FontScale = 1;
		public Action OnDraw = () => { };
		public Vector2 Position { get; set; } = Vector2.Zero;

		public Label(string txt)
		{
			Txt = txt;
		}

		public Label(string txt, Vector2 position)
		{
			Txt = txt;
			Position = position;
		}

		public bool Draw()
		{
			try
			{
				ImGui.SetWindowFontScale(FontScale);
				ImGui.Text(Txt);
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
