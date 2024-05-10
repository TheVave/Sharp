using ImGuiNET;
using Sharp.StrangeDataTypes;
using System.Numerics;

namespace Sharp.UI.UIElements
{
	public class Button : IUIElement, IAny
	{
		public Action OnClick { get; set; } = () => { };
		public bool StartThreadForClick { get; set; } = true;
		public string Label;
		public Vector2 Position { get; set; }
		public Vector2? Size { get; set; }
		public bool Visible { get; set; } = true;

		public bool Draw()
		{
			if (Size is not null)
			{
				if (ImGui.Button(Label, (Vector2)Size))
				{
					if (StartThreadForClick)
					{
						new Thread(OnClick.Invoke) { Name = "Render offshoot" }.Start();
					}
					else
					{
						OnClick.Invoke();
					}
				}
			}
			else
			{
				if (ImGui.Button(Label))
				{
					if (StartThreadForClick)
					{
						new Thread(OnClick.Invoke) { Name = "Render offshoot" }.Start();
					}
					else
					{
						OnClick.Invoke();
					}
				}
			}
			return true;
		}
	}
}
