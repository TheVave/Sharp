using Sharp.StrangeDataTypes;
using Silk.NET.GLFW;
using Silk.NET.Windowing;

namespace Sharp._2d._2DSVKRenderer.Main
{
	public class Internal2dSVKRenderer : IAny
	{
		public IWindow wnd;
		public string wndName;
		public Glfw glfwInst;
		public const int WndWidth = 1920;
		public const int WndHeight = 1080;
		/// <summary>
		/// Initialization.
		/// To be called.
		/// </summary>
		public virtual void INITSVK()
		{
			INITWND();
		}

		/// <summary>
		/// Initializes Vulkan
		/// </summary>
		public virtual void INITVK()
		{

		}

		/// <summary>
		/// The main loop method
		/// </summary>
		public unsafe virtual void MNLP()
		{
			while (!glfwInst.WindowShouldClose((WindowHandle*)wnd.Handle))
			{

			}
		}

		/// <summary>
		/// Clean up
		/// </summary>
		public virtual void CLNUP()
		{
			DELGLFW();
			TERGLFW();
		}

		public virtual unsafe void DELGLFW() => glfwInst.DestroyWindow((WindowHandle*)wnd.Handle);
		public virtual unsafe void TERGLFW() => glfwInst.Terminate();

		/// <summary>
		/// Inits the window with glfw
		/// </summary>
		public virtual unsafe void INITWND()
		{
			glfwInst = Glfw.GetApi();
			StWndHnt();

			CRTWND();
		}

		public virtual void StWndHnt()
		{
			glfwInst.Init();
			glfwInst.WindowHint(0, 0);
		}

		public unsafe virtual void CRTWND() => glfwInst.CreateWindow(WndWidth, WndHeight, wndName, null, null);
	}
}
