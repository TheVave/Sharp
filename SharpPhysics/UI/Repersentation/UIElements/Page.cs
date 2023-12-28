namespace SharpPhysics
{
	[Serializable]
	public class Page : UIElement
	{
		public Color BackgroundColor { get; set; } = new();
		public UIElement[] Elements = Array.Empty<UIElement>();
		public override string ToString()
		{
			string toReturn = $"Container/{BackgroundColor}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
			int loopCount = 0;
			foreach (UIElement element in Elements)
			{
				toReturn += $"{loopCount}:{element},";
				loopCount++;
			}
			return toReturn;
		}
	}
}
