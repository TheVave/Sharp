using SharpPhysics.Renderer;

namespace SharpPhysics.UI.Representation.UIElements
{
	[Serializable]
	public class Label : UIElement
	{
		public string Content;
		public int FontSize;
		public Color BackgroundColor;
		public Color ForegroundColor;

		public override string ToString() => $"Label/{Content}/{FontSize}/{BackgroundColor}/{ForegroundColor}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
