using System.Numerics;

namespace SharpPhysics.Renderer.Cameras
{
	public class Camera2D
	{
		public Vector2 FocusPosition;
		public float Zoom;

		public Camera2D(Vector2 focusPosition, float zoom)
		{
			FocusPosition = focusPosition;
			Zoom = zoom;
		}

		public Matrix4x4 GetProjectionMatrix()
		{
			float left = FocusPosition.X - Display.DisplayManager.WindowSize.X / 2;
			float right = FocusPosition.X + Display.DisplayManager.WindowSize.X / 2;

			float top = FocusPosition.Y - Display.DisplayManager.WindowSize.Y / 2;
			float bottom = FocusPosition.Y + Display.DisplayManager.WindowSize.Y / 2;

			Matrix4x4 focus = Matrix4x4.CreateOrthographicOffCenter(left, right, top, bottom, 0.1f, 100f);
			Matrix4x4 zoom = Matrix4x4.CreateScale(Zoom);

			return focus * zoom;
		}
	}
}
