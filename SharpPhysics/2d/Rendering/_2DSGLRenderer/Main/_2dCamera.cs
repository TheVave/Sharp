using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.StrangeDataTypes;
using System.Numerics;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	public class _2dCamera : IAny
	{
		/// <summary>
		/// The point the camera will focus on.
		/// </summary>
		public Point CenterPoint = new(0, 0);

		/// <summary>
		/// The object that the camera will follow.
		/// If this value is null it will follow CenterPoint
		/// </summary>
		public SimulatedObject2d? obj;

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
		/// The original window size.
		/// </summary>
		public Size originalWindowSize { get; internal set; }

		/// <summary>
		/// The matrix to multiply by to handle window resizng
		/// </summary>
		public Matrix4x4 WindowResizeMatrix = Matrix4x4.Identity;

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
				left = obj.Value.Translation.ObjectPosition.X - rndr.wndSize.Width / 2;
				right = obj.Value.Translation.ObjectPosition.X + rndr.wndSize.Width / 2;

				top = obj.Value.Translation.ObjectPosition.Y - rndr.wndSize.Height / 2;
				bottom = obj.Value.Translation.ObjectPosition.Y + rndr.wndSize.Height / 2;
			}

			Matrix4x4 focus = Matrix4x4.CreateOrthographicOffCenter((float)left, (float)right, (float)top, (float)bottom, 0.1f, 100f);
			Matrix4x4 zoom = /* WindowResizeMatrix * */ Matrix4x4.CreateScale(Zoom);

			return focus * zoom;
		}

		public void HandleResize(Size newSize)
		{
			// using fractions because the number b small

			int xMultTop = 1;
			int xMultBottom = originalWindowSize.Width;
			int yMultTop = 1;
			int yMultBottom = originalWindowSize.Height;



			//WindowResizeMatrix = Matrix4x4.Identity + Matrix4x4.CreateScale((float)((newSize.Width - originalWindowSize.Width) / xMultBottom), (float)((newSize.Height - originalWindowSize.Height) / yMultBottom),1);
		}
	}
}
