using System.Numerics;

namespace SharpPhysics.UI
{
	public interface IUIElement
	{
		/// <summary>
		/// The object's position.
		/// Vector2.Zero for automatic positioning.
		/// </summary>
		public Vector2 Position { get; set; }

		/// <summary>
		/// If the object is visible
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// The draw method for the UI Element
		/// </summary>
		/// <returns></returns>
		public bool Draw();
	}
}
