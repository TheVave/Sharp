namespace SharpPhysics
{
	[Serializable]
	public class Button : UIElement
	{
		public bool CurrentlyDown = false;
		public Color MouseOverColor = new(ColorName.Purple);
		public Color NormalColor = new(ColorName.Blue);
		public Color MousePressedColor = new(ColorName.Black);
		public Color MouseReleasedColor = new(ColorName.Blue);
		public Color DisabledColor = new(ColorName.Grey);
		public bool Disabled = false;
		public delegate void MouseDown(int MouseX, int MouseY);
		public delegate void MouseOver(int MouseX, int MouseY);
		public delegate void DisabledDown(int MouseX, int MouseY);

		public override string ToString() => $"Button/CurrentlyDown=false/{MouseOverColor}/{NormalColor}/{MousePressedColor}/{MouseReleasedColor}/{DisabledColor}/{Disabled}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
