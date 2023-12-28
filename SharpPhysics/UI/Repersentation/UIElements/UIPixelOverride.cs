namespace SharpPhysics
{
	public class UIPixelOverride : UIElement
	{
		public Color PixelColor = new();
		public override string ToString() => $"UIPixelOverride/{PixelColor}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
