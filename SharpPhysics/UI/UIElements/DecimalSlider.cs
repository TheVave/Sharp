using ImGuiNET;
using SharpPhysics.StrangeDataTypes;
using System.Numerics;

namespace SharpPhysics.UI.UIElements
{
	public class DecimalSlider : IUIElement, IAny
	{
		public float SliderVal = 0;
		public float SliderMin;
		public float SliderMax;
		public bool Visible { get; set; } = true;
		public string Label = "Slider";
		public Vector2 Position { get; set; } = Vector2.Zero;
		public Action OnDraw = () => { };

		public DecimalSlider(float sliderMin, float sliderMax, string label)
		{
			SliderMin = sliderMin;
			SliderMax = sliderMax;
			Label = label;
		}

		public DecimalSlider(float sliderMin, float sliderMax)
		{
			SliderMin = sliderMin;
			SliderMax = sliderMax;
		}

		public bool Draw()
		{
			ImGui.SliderFloat(Label, ref SliderVal, SliderMin, SliderMax);

			OnDraw.Invoke();
			return true;
		}
	}
}
