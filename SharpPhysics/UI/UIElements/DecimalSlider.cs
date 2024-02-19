using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SharpPhysics.UI.UIElements
{
	public class DecimalSlider : IUIElement
	{
		internal float sliderVal;
		public float SliderVal = 0;
		public float SliderMin;
		public float SliderMax;
		public bool Visible = true;
		public string Label = "Slider";
		public string SliderValStr;
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

		public bool Draw(ImGuiController controller, bool useNormalImGuiWnd)
		{
			if (Visible)
			{
				ImGui.SliderFloat(Label, ref SliderVal, SliderMin, SliderMax);
			}
			OnDraw.Invoke();
			return true;
		}
	}
}
