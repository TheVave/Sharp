namespace SharpPhysics.UI.Representation.UIElements
{
	[Serializable]
	public class ProgressBar : UIElement
	{
		byte percent = 0;

		public override string ToString() => $"ProgressBar/0/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
