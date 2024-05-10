using Sharp.StrangeDataTypes;
using System.Numerics;
namespace Sharp.Renderer.Cameras
{
	public class SGLCamera2D(Vector2 focusPosition, float zoom) : IAny
	{
		public Vector2 FocusPosition = focusPosition;
		public float Zoom = zoom;
	}
}
