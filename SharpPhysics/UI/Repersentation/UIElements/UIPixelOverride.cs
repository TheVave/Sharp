using SharpPhysics.Renderer;

namespace SharpPhysics.UI.Representation.UIElements
{
	public class UIPixelOverride : UIElement
	{
		public Color PixelColor = new();
		public override string ToString() => $"UIPixelOverride/{PixelColor}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
