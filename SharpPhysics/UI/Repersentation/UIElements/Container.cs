namespace SharpPhysics.UI.Representation.UIElements
{
	[Serializable]
	public class Container : UIElement
	{
		public UIElement[] Children { get; private set; }

		public override string ToString()
		{
			string toReturn = string.Empty;
			int loopCount = 0;
			foreach (UIElement element in Children)
			{
				toReturn += $"{loopCount}:{element},";
				loopCount++;
			}
			return "Container" + toReturn + "/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
		}
	}
}
