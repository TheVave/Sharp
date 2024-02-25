using System.Numerics;

namespace SharpPhysics._2d.Renderer._2DSGLESRenderer.Cameras
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
