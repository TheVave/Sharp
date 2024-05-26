using Sharp.StrangeDataTypes;
using Silk.NET.Core.Native;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;
using Silk.NET.Core.Native;
using Silk.NET.Core;
using System.Runtime.InteropServices;

namespace Sharp._2d._2DSVKRenderer.Main
{
	public class Internal2dSVKRenderer : IAny
	{
		/// <summary>
		/// The window
		/// </summary>
		public IView wnd;

		/// <summary>
		/// The window init settings
		/// </summary>
		public ViewOptions VO;

		/// <summary>
		/// The window width
		/// </summary>
		public int WindowWidth;

		/// <summary>
		/// The window height
		/// </summary>
		public int WindowHeight;

		/// <summary>
		/// Window name
		/// </summary>
		public string wndName;

		/// <summary>
		/// SDL instance
		/// </summary>
		public Sdl Sdl;

		/// <summary>
		/// Vulkan instance
		/// </summary>
		public Vk vk;

		/// <summary>
		/// IDK yet
		/// </summary>
		public Instance Instance;

		/// <summary>
		/// The window title. (Only applies to desktop)
		/// </summary>
		public string title;

		/// <summary>
		/// The api version to use.
		/// May be updated in the future for newer versions of vulkan.
		/// Default value: new(1,1)
		/// </summary>
		public APIVersion version = new(1, 1);

		/// <summary>
		/// If the program is to initialy be fullscreen. (Only applies to desktop)
		/// </summary>
		public bool Fullscreen;

		/// <summary>
		/// If the window should be borderless. (Only applies to desktop)
		/// </summary>
		public bool Borderless;

		public readonly uint SDL_WINDOWPOS_UNDEFINED = 0x80000000;
		public readonly uint SDL_WINDOW_FULLSCREEN = 0x1;
		public readonly uint SDL_WINDOW_SHOWN = 0x4;
		public readonly uint SDL_WINDOW_VULKAN = 0x10000000;

		// guide to reading method names:
		// VK=Vulkan (rendering library)
		// SDL=Simple DirectMedia Layer (windowing library)
		// LD=Load
		// CRT=Create
		// WND=Window
		// ES=Events
		// ST=Set
		// VW=View
		// GT=Get
		// RNDR=Render

		public virtual void INITSDL()
		{
			// SDL_INIT(SDL_INIT_VIDEO)
			Sdl.Init(0x20);
		}

		public virtual void INITRNDR()
		{
			// inits SDL
			INITSDL();
			// Creates a window with SDL
			CRTWND();
			// Inits vulkan on the window
			VKLD();

			
		}

		public virtual void STES(IView vw)
		{
			wnd.Load += LD;
		}

		public virtual IView CRTWND()
		{
			VO = new();
			VO.API = new(ContextAPI.Vulkan, version);
			return Silk.NET.Windowing.Window.GetView(VO);
        }

		public virtual Vk CRTANDGTWNDAPI()
		{
			CRTWND();
			return VKLD();
		}

		/// <summary>
		/// Post-Api initalization.
		/// Sets up things like textures, shaders, ect.
		/// </summary>
		public virtual void LD()
		{

		}

		/// <summary>
		/// Inits Vulkan.
		/// Sets vk to a new Vulkan api ref, and Instance.
		/// </summary>
		/// <exception cref="Exception"></exception>
		public virtual unsafe Vk VKLD()
		{
			Vk vk = Vk.GetApi();

			ApplicationInfo appInfo = new()
			{
				SType = StructureType.ApplicationInfo,
				PApplicationName = (byte*)Marshal.StringToHGlobalAnsi("Sharp viewport"),
				ApplicationVersion = new Version32(1, 0, 0),
				PEngineName = (byte*)Marshal.StringToHGlobalAnsi("No Engine"),
				EngineVersion = new Version32(1, 0, 0),
				ApiVersion = Vk.Version11
			};

			InstanceCreateInfo createInfo = new()
			{
				SType = StructureType.InstanceCreateInfo,
				PApplicationInfo = &appInfo
			};

			createInfo.EnabledLayerCount = 0;

			if (vk.CreateInstance(createInfo, null, out Instance) != Silk.NET.Vulkan.Result.Success)
			{
				throw new Exception("failed to create instance!");
			}

			Marshal.FreeHGlobal((IntPtr)appInfo.PApplicationName);
			Marshal.FreeHGlobal((IntPtr)appInfo.PEngineName);

			return vk;
		}
	}
}