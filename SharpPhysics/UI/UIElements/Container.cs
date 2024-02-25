using System.Numerics;

namespace SharpPhysics.UI.UIElements
{
	public class Container : IUIElement
	{
		public IUIElement[] Children;
		public bool Visible { get; set; } = true;
		public Action OnDraw = () => { };
		public Vector2 Position { get; set; } = Vector2.Zero;

		public bool Draw()
		{
			foreach (IUIElement child in Children)
			{
				child.Draw();
			}
			OnDraw.Invoke();
			return true;
		}
	}
}
