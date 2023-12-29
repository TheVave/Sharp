namespace SharpPhysics.UI.Representation
{

	[Serializable]
	public abstract class UIElement
	{
		public int XPos = 0, YPos = 0;
		public bool IsAbsoloutePositionBased;
		protected string? objectString;
		internal UIElementList ElementStyle;
		public UIElement Parent;
		public abstract override string ToString();
	}
}
