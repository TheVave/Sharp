namespace SharpPhysics
{
	[Serializable]
	public class Comment : UIElement
	{
		public string commentText;

		public override string ToString() => $"Comment/{commentText}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
