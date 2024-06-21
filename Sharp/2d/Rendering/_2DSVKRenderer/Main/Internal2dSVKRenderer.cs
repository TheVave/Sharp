using Sharp.StrangeDataTypes;
using Silk.NET.Core.Native;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;
using Silk.NET.Core;
using System.Runtime.InteropServices;
using Sharp.Utilities;
using Sharp.Utilities.MISC;
using Sharp.Utilities.MISC.Unsafe;
using System.Runtime.CompilerServices;

namespace Sharp._2d._2DSVKRenderer.Main
{
	public struct QueueFamilyIndices
	{
		public uint? GraphicsFamily { get; set; }
		public bool IsComplete()
		{
			return GraphicsFamily.HasValue;
		}
	}

	public class Internal2dSVKRenderer : IAny
	{
		/// <summary>
		/// The window
		/// </summary>
		public IView wnd;

		/// <summary>
		/// The what vulkan renderers onto directly.
		/// Similar to a window handle.
		/// </summary>
		public Surface WindowSurface;

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
		/// The main logical device to render to.
		/// Essentially a subdivision of the gpu.
		/// </summary>
		public Device MainLogicalDevice;

		/// <summary>
		/// The Queue family props for MainDevice
		/// </summary>
		public QueueFamilyProperties[] QFamilyProps;

		/// <summary>
		/// The Queue family for MainDevice.
		/// Little confused what the difference between this and QFamilyProps is.
		/// </summary>
		public uint QueueFamilyIndex;

		/// <summary>
		/// The Queue families that support graphics calculation
		/// </summary>
		public QueueFamilyProperties[] GraphicsFamilyProps;

		/// <summary>
		/// The main queue to run commands on.
		/// kind of the gl object handle in ogl.
		/// </summary>
		public Queue MainQueue;

		/// <summary>
		/// Window name
		/// </summary>
		public string wndName;

		/// <summary>
		/// SDL instance
		/// </summary>
		public Sdl Sdl;

		/// <summary>
		/// The GPU to render on.
		/// </summary>
		public PhysicalDevice MainDevice;

		/// <summary>
		/// Vulkan instance
		/// </summary>
		public Vk vk;

		/// <summary>
		/// If the program should use validation layers.
		/// They are useful for debugging.
		/// </summary>
		public bool EnableValidationLayers;

		/// <summary>
		/// Interface between vulkan and Sharp
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
		/// If the program is to initially be fullscreen. (Only applies to desktop)
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
		// significant reading required and this is not a highly direct list.
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
		// NG (Suffix)=ING
		// CLN=Clean
		// PHY=Physics/Physical
		// DVC=Device
		// LGCL=Logical
		// FND=Find
		// Q=Queue
		// FMLY=Family
		// INDCS=Indices
		// HNDL=Handle
		// SRFC=Surface

		// example:
		// CRTLGCLDVC
		// CRT=Create LGCL=Logical DVC=Device
		// Create logical device.

		public virtual void INITRNDRNG()
		{
			// VULKAN SETUP AND WINDOW CREATION 
			{
				// Creates a window with SDL
				wnd = CRTWND();
				// Inits vulkan
				vk = VKLD(out Instance);
				// gets a physical device
				MainDevice = GTPHYSDVC(Instance, vk);
				// gets queue family properties
				QFamilyProps = GTQFMLYPROP(MainDevice, vk);
				// gets a logical device
				MainLogicalDevice = CRTLGCLDVC(MainDevice, vk);
				// gets a graphics queue, which allows us to run commands.
				MainQueue = GTQHNDL(MainLogicalDevice, QueueFamilyIndex, vk);
				// Creates a surface to render onto
				
			}

			// inits events
			STES(wnd);
			// starts rendering
			INITVWRNDRNG(wnd);
			// clean up
			CLNUP(MainLogicalDevice, vk);
		}

		public unsafe virtual Device CRTLGCLDVC(PhysicalDevice MainDevice, Vk vk)
		{
			var indices = GTQFMLYIDX(MainDevice, vk);
			QueueFamilyIndex = indices.GraphicsFamily!.Value;
			float priority = 1.0f;
			DeviceQueueCreateInfo info = new()
			{
				SType = StructureType.DeviceCreateInfo,
				QueueFamilyIndex = QueueFamilyIndex,
				QueueCount = 1,
				PQueuePriorities = &priority
			};

			PhysicalDeviceFeatures DeviceFeatures;

			DeviceCreateInfo createInfo = new()
			{
				SType = StructureType.DeviceCreateInfo,
				PQueueCreateInfos = &info,
				QueueCreateInfoCount = 1,
				PEnabledFeatures = &DeviceFeatures,
				EnabledExtensionCount = 0,
				EnabledLayerCount = 0
			};

			Device dvcRef;

			Silk.NET.Vulkan.Result result = vk.CreateDevice(MainDevice, &createInfo, null, &dvcRef);
			if (result != Silk.NET.Vulkan.Result.Success)
			{
				// Error, Internal Error, Failed to create a logical device. This may be an error with drivers though I'm not sure. Exact error code (Vulkan error code): #1
				// Ill put some research into this later.
				ErrorHandler.ThrowError(25, [result.ToString()], true);
			}

			return dvcRef;
		}

		// will be implemented in the near (very near) future.
		public virtual void CRTSRFC() { }

		public virtual unsafe void CLNUP(Device DeviceToDestroy, Vk vk)
		{
			vk.DestroyDevice(DeviceToDestroy, null);
		}

		public unsafe virtual QueueFamilyIndices GTQFMLYIDX(PhysicalDevice device, Vk vk)
		{
			var indices = new QueueFamilyIndices();

			uint queueFamilityCount = 0;
			vk!.GetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilityCount, null);

			var queueFamilies = new QueueFamilyProperties[queueFamilityCount];
			fixed (QueueFamilyProperties* queueFamiliesPtr = queueFamilies)
			{
				vk!.GetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilityCount, queueFamiliesPtr);
			}


			uint i = 0;
			foreach (var queueFamily in queueFamilies)
			{
				if (queueFamily.QueueFlags.HasFlag(QueueFlags.GraphicsBit))
				{
					indices.GraphicsFamily = i;
				}

				if (indices.IsComplete())
				{
					break;
				}

				i++;
			}

			return indices;
		}

		public unsafe virtual QueueFamilyProperties[] GTQFMLYPROP(PhysicalDevice dvc, Vk vk)
		{
			uint qFamilyCount = 0;
			vk.GetPhysicalDeviceQueueFamilyProperties(dvc, &qFamilyCount, null);

			QueueFamilyProperties[] QFamilyProps = new QueueFamilyProperties[qFamilyCount];
			fixed (QueueFamilyProperties* ptr = QFamilyProps) {
				vk.GetPhysicalDeviceQueueFamilyProperties(dvc, &qFamilyCount, ptr);
			}

			return QFamilyProps;
		}

		public unsafe virtual PhysicalDevice GTPHYSDVC(Instance instance, Vk vk)
		{
			PhysicalDevice dvc = new();
			uint dvcCount = 0;
			vk.EnumeratePhysicalDevices(instance, &dvcCount, (PhysicalDevice*)Utils.NULLVOIDPTR);
			// failed to find a suitable vulkan gpu
			if (dvcCount == 0) ErrorHandler.ThrowError(24, true);
			PhysicalDevice[] devices = new PhysicalDevice[dvcCount];
			fixed (PhysicalDevice* ptr = &devices[0])
				vk.EnumeratePhysicalDevices(instance, &dvcCount, ptr);
			int[] scores = new int[devices.Length];

			int i = 0;
			foreach (PhysicalDevice physdvc in devices)
				GTVLDPHYSDVC(physdvc, ref scores[i++]);
			int chosenDvc = ArrayUtils.GetHighestValueIndex(scores) - 1;

			return devices[chosenDvc];
		}

		public virtual Queue GTQHNDL(Device dvc, uint qfmlyidx, Vk vk) =>
			vk.GetDeviceQueue(dvc, qfmlyidx, 0);

		public virtual void GTVLDPHYSDVC(PhysicalDevice physdvc, ref int score)
		{
			PhysicalDeviceProperties physdvcprop = vk.GetPhysicalDeviceProperties(physdvc);
			PhysicalDeviceFeatures physdvcfturs = vk.GetPhysicalDeviceFeatures(physdvc);

			if (physdvcprop.DeviceType == PhysicalDeviceType.DiscreteGpu) score += 1000;
			if (physdvcprop.DeviceType == PhysicalDeviceType.IntegratedGpu) score += 100;
			if (physdvcprop.DeviceType == PhysicalDeviceType.Other) score+=300;
			if (physdvcprop.DeviceType == PhysicalDeviceType.VirtualGpu) score += 3000;
			score += (int)physdvcprop.Limits.MaxImageDimension2D;

			if (!physdvcfturs.GeometryShader) score = -1;
		}

		public virtual void INITVWRNDRNG(IView vw)
		{
			vw.Run();
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
			return VKLD(out Instance);
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
		public virtual unsafe Vk VKLD(out Instance inst)
		{
			Vk vk = Vk.GetApi();

			ApplicationInfo appInfo = new()
			{
				SType = StructureType.ApplicationInfo,
				PApplicationName = (byte*)Marshal.StringToHGlobalAnsi(title),
				ApplicationVersion = new Version32(1, 0, 0),
				PEngineName = (byte*)Marshal.StringToHGlobalAnsi("No Engine"),
				EngineVersion = new Version32(1, 0, 0),
				ApiVersion = Vk.Version11
			};

			string[] ext = ["VK_KHR_surface"];
			byte* firstByte = Utils.NULLBYTEPTR;
			fixed (string* firstExtPtr = &ext[0])
			{
				firstByte = (byte*)firstExtPtr;
			}


			InstanceCreateInfo createInfo = new()
			{
				SType = StructureType.InstanceCreateInfo,
				EnabledExtensionCount = (uint)ext.Length,
				PpEnabledExtensionNames = &firstByte,
				PApplicationInfo = &appInfo
			};

			createInfo.EnabledLayerCount = 0;

			if (vk.CreateInstance(createInfo, null, out inst) != Silk.NET.Vulkan.Result.Success)
			{
				//Error, Internal Error, Failed to create a Vulkan instance. This is most likely a hardware issue, but drivers could be the culprit as well. Sharp will be unable to run on your hardware.
				ErrorHandler.ThrowError(26, true);
			}

			Marshal.FreeHGlobal((IntPtr)appInfo.PApplicationName);
			Marshal.FreeHGlobal((IntPtr)appInfo.PEngineName);
			SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

			if (EnableValidationLayers)
			{
				SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
			}

			return vk;
		}
	}
}