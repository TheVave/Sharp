using System.Drawing;
using System.Numerics;
namespace SharpPhysics.Renderer.Cameras
{
	public class SGLCamera2D
	{
		public Vector2 FocusPosition;
		public float Zoom;

		public SGLCamera2D(Vector2 focusPosition, float zoom)
		{
			FocusPosition = focusPosition;
			Zoom = zoom;
		}
	}
}
