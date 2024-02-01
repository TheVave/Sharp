using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	/// <summary>
	/// Handled by the MainRendererSGL class.
	/// Please do not interface directly unless you know what you're doing,
	/// though useful if you want to make custom rendering code.
	/// </summary>
	public abstract class Internal2dRenderer
	{
		/// <summary>
		/// Window ref
		/// </summary>
		public IWindow Wnd;

		/// <summary>
		/// wnd title
		/// </summary>
		public string title;

		/// <summary>
		/// The size of the window
		/// </summary>
		public Size wndSize;

		/// <summary>
		/// The window options to create Wnd with
		/// </summary>
		public WindowOptions WndOptions = WindowOptions.Default;

		/// <summary>
		/// Initializes SGL (Silk.net openGL)
		/// 
		/// </summary>
		public virtual void ISGL() 
		{
			// window init
			SWCNFG();

		}

		/// <summary>
		/// Sets the window configuration
		/// </summary>
		public virtual void SWCNFG()
		{
			WndOptions.Title = title;
			WndOptions.Size = new(wndSize.Width, wndSize.Height);
		}
	}
}
