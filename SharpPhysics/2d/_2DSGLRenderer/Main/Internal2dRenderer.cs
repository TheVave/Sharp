using SharpPhysics.Renderer;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	/// <summary>
	/// Handled by the MainRendererSGL class.
	/// Please do not interface directly unless you know what you're doing,
	/// though useful if you want to make custom rendering code.
	/// </summary>
	public class Internal2dRenderer
	{
		/// <summary>
		/// Window ref
		/// </summary>
		public IWindow Wnd;

		/// <summary>
		/// wnd title
		/// </summary>
		public string title = "SharpPhysics View Port";

		/// <summary>
		/// The size of the window
		/// </summary>
		public Size wndSize;

		/// <summary>
		/// The window options to create Wnd with
		/// </summary>
		public WindowOptions WndOptions = WindowOptions.Default;

		/// <summary>
		/// GL context
		/// </summary>
		public GL gl;

		/// <summary>
		/// The color to clear to
		/// </summary>
		public Color clearBufferBit = new(ColorName.Black);

		/// <summary>
		/// Initializes SGL (Silk.net openGL) and the Wnd object
		/// 
		/// </summary>
		public virtual void ISGL() 
		{
			// window init
			SWCNFG();
			INITWND();
			// wnd events
			WES();
			// calling
			CLWND();
		}

		/// <summary>
		/// Sets the window configuration
		/// </summary>
		public virtual void SWCNFG()
		{
			WndOptions.Title = title;
			WndOptions.Size = new(wndSize.Width, wndSize.Height);
		}

		/// <summary>
		/// Sets window events
		/// </summary>
		public virtual void WES()
		{
			Wnd.Update += UDT;
			Wnd.Render += RNDR;
			Wnd.Load += LD;
		}

		/// <summary>
		/// Initializes the window
		/// </summary>
		public virtual void INITWND()
		{
			Wnd = Window.Create(WndOptions);
		}

		/// <summary>
		/// Starts the window
		/// </summary>
		public virtual void CLWND()
		{
			Wnd.Run();
		}

		/// <summary>
		/// Called every frame to render the object(s)
		/// </summary>
		public virtual void RNDR(double deltaTime)
		{

		}

		/// <summary>
		/// Update. Called before render.
		/// </summary>
		public virtual void UDT(double deltaTime)
		{

		}

		/// <summary>
		/// Called before anything else, only called once.
		/// </summary>
		public virtual void LD()
		{
			// inits the OpenGL context
			INTGLCNTXT();

		}



		/// <summary>
		/// Initializes the OpenGL context
		/// </summary>
		public virtual void INTGLCNTXT()
		{
			gl = Wnd.CreateOpenGL();
		}

		/// <summary>
		/// sets the clear color.
		/// </summary>
		/// <param name="name">
		/// The color to set to.
		/// </param>
		public virtual void STCLRCOLR(ColorName name)
		{
			Color clr = new Color(name);
			gl.ClearColor(clr.R, clr.G, clr.B, clr.A);
			clearBufferBit = clr;
		}

		/// <summary>
		/// Uses the rgba syntax to set the buffer bit
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public virtual void STCLRCOLR(byte r, byte g, byte b, byte a)
		{
			gl.ClearColor(r, g, b, a);
			clearBufferBit = new Color(r, g, b, a);
		}

		/// <summary>
		/// Clears the screen to clearBufferBit
		/// </summary>
		public virtual void CLR()
		{
			gl.Clear(ClearBufferMask.ColorBufferBit);
		}

		/// <summary>
		/// Gets float[] from matrix4x4
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		private float[] GetMatrix4x4Values(Matrix4x4 m)
		{
			return new float[]
			{
		m.M11, m.M12, m.M13, m.M14,
		m.M21, m.M22, m.M23, m.M24,
		m.M31, m.M32, m.M33, m.M34,
		m.M41, m.M42, m.M43, m.M44
			};
		}
	}
}
