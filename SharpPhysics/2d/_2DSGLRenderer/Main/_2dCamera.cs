using SharpPhysics._2d.ObjectRepresentation;
using System.Numerics;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	public class _2dCamera
	{
		/// <summary>
		/// The point the camera will focus on.
		/// </summary>
		public Point CenterPoint = new(0,0);

		/// <summary>
		/// The object that the camera will follow.
		/// If this value is null it will follow CenterPoint
		/// </summary>
		public _2dSimulatedObject? obj;

		/// <summary>
		/// Camera zoom.
		/// 1 would be one unit is one pixel.
		/// </summary>
		public float Zoom = 1;

		/// <summary>
		/// The Field Of View.
		/// Normal is 90.
		/// DOES NOT CURRENTLY WORK!!!
		/// </summary>
		public double FOV = 90;

		/// <summary>
		/// Gets the camera projection matrix.
		/// </summary>
		/// <param name="rndr"></param>
		/// <returns></returns>
		public Matrix4x4 GetProjectionMatrix(Internal2dRenderer rndr)
		{
			double left = 0;
			double right = 0;
			double top = 0;
			double bottom = 0;
			if (obj is null)
			{
				left = CenterPoint.X - rndr.wndSize.Width / 2;
				right = CenterPoint.X + rndr.wndSize.Width / 2;

				top = CenterPoint.Y - rndr.wndSize.Height / 2;
				bottom = CenterPoint.Y + rndr.wndSize.Height / 2;
			}
			else
			{
				left = obj.Translation.ObjectPosition.xPos - rndr.wndSize.Width / 2;
				right = obj.Translation.ObjectPosition.xPos + rndr.wndSize.Width / 2;

				top = obj.Translation.ObjectPosition.yPos - rndr.wndSize.Height / 2;
				bottom = obj.Translation.ObjectPosition.yPos + rndr.wndSize.Height / 2;
			}

			Matrix4x4 focus = Matrix4x4.CreateOrthographicOffCenter((float)left, (float)right, (float)top, (float)bottom, 0.1f, 100f);
			Matrix4x4 zoom = Matrix4x4.CreateScale(Zoom);

			return focus * zoom;
		}
	}
}
