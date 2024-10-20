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
using Silk.NET.Core.Contexts;
using Silk.NET.Vulkan.Extensions.KHR;
using Sharp.Utilities.MISC.Unsafe;
using System.Diagnostics;
using Sharp._2d.Rendering._2DSVKRenderer;
using Sharp._2d.Rendering._2DSVKRenderer.Main;

namespace Sharp._2d._2DSVKRenderer.Main
{
	/// <summary>
	/// The queue family indices.
	/// Queue families are types of queues
	/// No idea why they are indices.
	/// </summary>
	public struct QueueFamilyIndices
	{
		public uint? GraphicsFamily { get; set; }
		public uint? PresentFamily { get; set; }

		public readonly bool IsComplete()
		{
			return GraphicsFamily.HasValue && PresentFamily.HasValue;
		}
	}

	/// <summary>
	/// Details of the graphics card's support for swapchains (if it has any)
	/// </summary>
	public struct SwapChainSupportDetails
	{
		public SurfaceCapabilitiesKHR Capabilities;
		public SurfaceFormatKHR[] Formats;
		public PresentModeKHR[] PresentModes;
	}

	/// <summary>
	/// The actual renderer. You found it.
	/// It's all here. (over 800 lines of code)
	/// All methods inside of this should be virtual, so you can rewrite it all.
	/// </summary>
	public class Internal2dSVKRenderer : IAny
	{
		/// <summary>
		/// The SDL window object.
		/// </summary>
		public IView wnd;

		/// <summary>
		/// KhrSurface / SurfaceKhr
		/// The window handles.
		/// </summary>
		public KhrSurface KhrWindowSurface;

		/// <summary>
		/// KhrSurface / SurfaceKhr
		/// The window handles.
		/// </summary>
		public SurfaceKHR WindowSurfaceKhr;

		//public Surface SDLSurface;

		/// <summary>
		/// The device extensions.
		/// </summary>
		private readonly string[] deviceExtensions =
		[
			KhrSwapchain.ExtensionName
		];

		/// <summary>
		/// The view options.
		/// This will be applied to the created window.
		/// </summary>
		public ViewOptions VO;

		public int TestEnvIdx;

		/// <summary>
		/// The width of the window to be created.
		/// As of 9/7/24, this is very important as the program's window cannot be resized.
		/// </summary>
		public int WindowWidth;

		/// <summary>
		/// The elapsed time since the Swap chain was created in Vulkan (since rendering started)
		/// </summary>
		double TotalElapsed = 0;
		
		/// <summary>
		/// The elapsed time since the last frame.
		/// 
		/// </summary>
		double Elapsed = 0;

		/// <summary>
		/// The swap chain for the program to use for rendering.
		/// Is created somewhat late in initialization.
		/// </summary>
		public KhrSwapchain Swapchain;

		/// <summary>
		/// The (extension-specific?) for the program to use for rendering.
		/// Is created somewhat late in initialization.
		/// </summary>
		public SwapchainKHR SwapchainKHR;

		/// <summary>
		/// The swap chain image format.
		/// </summary>
		private Format swapChainImageFormat;

		/// <summary>
		/// The size of the swap chain images.
		/// Vulkan's 2d size struct.
		/// </summary>
		private Extent2D swapChainExtent;

		/// <summary>
		/// The array of swap chain images.
		/// Will be set by the renderer.
		/// </summary>
		public Image[] SwapChainImages;

		/// <summary>
		/// The views to images.
		/// For the main swapchain
		/// </summary>
		public ImageView[] SwapchainImageViews;

		public SVKTexture[] Textures;

		/// <summary>
		/// The width of the window to be created.
		/// As of 9/7/24, this is very important as the program's window cannot be resized.
		/// </summary>
		public int WindowHeight;

		/// <summary>
		/// The main logical device (handle) to render to.
		/// This is a subdivision of the GPU.
		/// </summary>
		public Device MainLogicalDevice;

		/// <summary>
		/// Properties about the <see cref="QueueFamilyIndices"/>.
		/// </summary>
		public QueueFamilyProperties[] QFamilyProps;

		/// <summary>
		/// The graphics pipeline queue object.
		/// This is what you use to actually draw things.
		/// (Or to send messages to the GPU to actually draw things)
		/// </summary>
		public Queue Q;

		/// <summary>
		/// The platform to draw to.
		/// </summary>
		public VKDrawPlatform Platform;

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
		/// The queue family indices (not properties)
		/// </summary>
		public QueueFamilyIndices QueueFamilyIndices;

		/// <summary>
		/// This q can handle presenting the pixels to the screen.
		/// </summary>
		public Queue MainQueuePresent;

		/// <summary>
		/// This q draws the pixels to render
		/// </summary>
		public Queue MainQueueDraw;

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
		/// NOT SUPPORTED CURRENTLY!!!!!
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
		/// If the program should use VSync to stabilize the video.
		/// If there are horizontal lines appearing, then you should turn on VSync.
		/// </summary>
		public bool VSync = true;

		/// <summary>
		/// The views to the SwapChainImages
		/// </summary>
		public ImageView[] VkImageViews;

		/// <summary>
		/// The api version to use.
		/// May be updated in the future for newer versions of vulkan.
		/// Default value: 1.1
		/// </summary>
		public APIVersion version = GraphicsAPI.DefaultVulkan.Version;

		/// <summary>
		/// If the program is to initially be fullscreen. (Only applies to desktop)
		/// </summary>
		public bool Fullscreen;

		/// <summary>
		/// If the window should be borderless. (Only applies to desktop)
		/// </summary>
		public bool Borderless;

		public const uint SDL_WINDOWPOS_UNDEFINED = 0x80000000;
		public const uint SDL_WINDOW_FULLSCREEN = 0x1;
		public const uint SDL_WINDOW_SHOWN = 0x4;
		public const uint SDL_WINDOW_VULKAN = 0x10000000;
		public const uint VK_COMPONENT_SWIZZLE_IDENTITY = 0;
		public const uint VK_IMAGE_VIEW_TYPE_1D = 0;
		public const uint VK_IMAGE_VIEW_TYPE_2D = 1;
		public const uint VK_IMAGE_VIEW_TYPE_3D = 2;
		public const uint VK_IMAGE_VIEW_TYPE_CUBE = 3;
		public const uint VK_IMAGE_VIEW_TYPE_1D_ARRAY = 4;
		public const uint VK_IMAGE_VIEW_TYPE_2D_ARRAY = 5;
		public const uint VK_IMAGE_VIEW_TYPE_CUBE_ARRAY = 6;

		/// <summary>
		/// Temporary holder that contains image view create info.
		/// Should not be used!
		/// </summary>
		public ImageViewCreateInfo TempCreateInfo = new();

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
		// FRM=From
		// SWPCHN=Swap chain
		// QRY=Query
		// SUP=Support
		// EXTNT=Extent

		// example:
		// CRTLGCLDVC
		// CRT=Create LGCL=Logical DVC=Device
		// Create logical device is what CRTLGCLDVC does.

		public virtual void INITRNDRNG()
		{
			// VULKAN SETUP AND WINDOW CREATION 
			{
				Debug.WriteLine("Initialized");
				// Creates a window with SDL
				wnd = CRTWND();
				Debug.WriteLine("Window created");
				// Inits vulkan
				vk = VKLD(out Instance);
				Debug.WriteLine("Vulkan loaded");
				// gets window surfaces
				CRTSRFCS(out WindowSurfaceKhr, out KhrWindowSurface, Instance, wnd);
				Debug.WriteLine("Created surfaces");
				// gets a physical device
				MainDevice = GTPHYSDVC(Instance, vk);
				Debug.WriteLine("Created physical device.");
				// gets queue families
				QueueFamilyIndices = GTQFMLYIDX(MainDevice, vk);
				Debug.WriteLine("Collected queue family indices.");
				// gets queue family properties
				QFamilyProps = GTQFMLYPROP(MainDevice, vk);
				Debug.WriteLine("Collected queue family properties.");
				// gets a logical device
				MainLogicalDevice = CRTLGCLDVC(MainDevice, vk, QueueFamilyIndices, out QueueFamilyIndex);
				Debug.WriteLine("Created logical device.");
				// gets a graphics queue, to run commands.
				GTQHNDL(MainLogicalDevice, QueueFamilyIndex, vk);
				Debug.WriteLine("Created queue.");
				// Creates a swapchain
				CRTSWPCHN(MainDevice, WindowSurfaceKhr, KhrWindowSurface, QueueFamilyIndices, Instance, MainLogicalDevice, ref Swapchain, ref SwapchainKHR, ref SwapChainImages, ref swapChainImageFormat, ref swapChainExtent);
				Debug.WriteLine("Created swapchain.");
				// Set up swapchain images

			}

			// inits events
			STES(wnd);
			
			// starts rendering
			INITVWRNDRNG(wnd);
			// clean up
			CLNUP(MainLogicalDevice, Instance, KhrWindowSurface, WindowSurfaceKhr, vk);
		}

		public unsafe virtual void INITSWPCHNIMGVWS(ref Image[] swapchainImages, ref ImageView[] imageViews)
		{
			imageViews = new ImageView[swapchainImages.Length];
			for (int i = 0; i < SwapChainImages.Length; i++)
				CRTIMGVW();

		}

		public unsafe virtual void CRTIMGVW(Image image, Format format, uint ImageType, ComponentMapping componentParam = new(), bool doMipMaping = false, ImageType type)
		{
			TempCreateInfo = new()
			{
				SType = StructureType.ImageViewCreateInfo,
				Image = image,
				Format = format,
				ViewType = ImageViewType.Type2D,
				Components = componentParam,

			};

		}

		public unsafe virtual void CRTSWPCHN(PhysicalDevice MainDevice, SurfaceKHR surfacekhr, KhrSurface khrsurface, QueueFamilyIndices idcs, Instance inst, Device dvc, ref KhrSwapchain Swapchain, ref SwapchainKHR swapchainKHR, ref Image[] imgs, ref Format imgfrmt, ref Extent2D imgsze)
		{
			var swapChainSupport = QRYSWPCHNSUP(khrsurface, surfacekhr, MainDevice);

			var surfaceFormat = CHSWPSRFCFRMT(swapChainSupport.Formats);
			var presentMode = ChoosePresentMode(swapChainSupport.PresentModes);
			var extent = CHSWPCHNEXTNT(swapChainSupport.Capabilities);

			var imageCount = swapChainSupport.Capabilities.MinImageCount + 1;
			if (swapChainSupport.Capabilities.MaxImageCount > 0 && imageCount > swapChainSupport.Capabilities.MaxImageCount)
			{
				imageCount = swapChainSupport.Capabilities.MaxImageCount;
			}

			SwapchainCreateInfoKHR creatInfo = new()
			{
				SType = StructureType.SwapchainCreateInfoKhr,
				Surface = surfacekhr,

				MinImageCount = imageCount,
				ImageFormat = surfaceFormat.Format,
				ImageColorSpace = surfaceFormat.ColorSpace,
				ImageExtent = extent,
				ImageArrayLayers = 1,
				ImageUsage = ImageUsageFlags.ColorAttachmentBit,
			};

			var indices = QueueFamilyIndices;
			var queueFamilyIndices = stackalloc[] { indices.GraphicsFamily!.Value, indices.PresentFamily!.Value };

			if (indices.GraphicsFamily != indices.PresentFamily)
			{
				creatInfo = creatInfo with
				{
					ImageSharingMode = SharingMode.Concurrent,
					QueueFamilyIndexCount = 2,
					PQueueFamilyIndices = queueFamilyIndices,
				};
			}
			else
			{
				creatInfo.ImageSharingMode = SharingMode.Exclusive;
			}

			creatInfo = creatInfo with
			{
				PreTransform = swapChainSupport.Capabilities.CurrentTransform,
				CompositeAlpha = CompositeAlphaFlagsKHR.OpaqueBitKhr,
				PresentMode = presentMode,
				Clipped = true,

				OldSwapchain = default
			};

			if (!vk!.TryGetDeviceExtension(inst, dvc, out Swapchain))
			{
				throw new NotSupportedException("VK_KHR_swapchain extension not found.");
			}
			fixed (SwapchainKHR* pswap = &swapchainKHR)
			{
				if (Swapchain!.CreateSwapchain(dvc, &creatInfo, null, pswap) != Silk.NET.Vulkan.Result.Success)
				{
					throw new Exception("failed to create swap chain!");
				}
			}

			Swapchain.GetSwapchainImages(dvc, SwapchainKHR, &imageCount, null);
			imgs = new Image[imageCount];
			fixed (Image* swapChainImagesPtr = imgs)
			{
				Swapchain.GetSwapchainImages(dvc, SwapchainKHR, ref imageCount, swapChainImagesPtr);
			}

			imgfrmt = surfaceFormat.Format;
			imgsze = extent;

			
		}

		public unsafe virtual SwapChainSupportDetails QRYSWPCHNSUP(KhrSurface khrSurface, SurfaceKHR surface, PhysicalDevice physicalDevice)
		{
			var details = new SwapChainSupportDetails();

			khrSurface!.GetPhysicalDeviceSurfaceCapabilities(physicalDevice, surface, out details.Capabilities);

			uint formatCount = 0;
			khrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, surface, ref formatCount, null);

			if (formatCount != 0)
			{
				details.Formats = new SurfaceFormatKHR[formatCount];
				fixed (SurfaceFormatKHR* formatsPtr = details.Formats)
				{
					khrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, surface, ref formatCount, formatsPtr);
				}
			}
			else
			{
				details.Formats = Array.Empty<SurfaceFormatKHR>();
			}

			uint presentModeCount = 0;
			khrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, surface, ref presentModeCount, null);

			if (presentModeCount != 0)
			{
				details.PresentModes = new PresentModeKHR[presentModeCount];
				fixed (PresentModeKHR* formatsPtr = details.PresentModes)
				{
					khrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, surface, ref presentModeCount, formatsPtr);
				}

			}
			else
			{
				details.PresentModes = Array.Empty<PresentModeKHR>();
			}

			return details;
		}

		public unsafe virtual Device CRTLGCLDVC(PhysicalDevice MainDevice, Vk vk, QueueFamilyIndices QueueFamilyIndices, out uint QueueFamilyIndex)
		{
			QueueFamilyIndex = QueueFamilyIndices.GraphicsFamily!.Value;

			var uniqueQueueFamilies = new[] { QueueFamilyIndices.GraphicsFamily!.Value, QueueFamilyIndices.PresentFamily!.Value };
			uniqueQueueFamilies = uniqueQueueFamilies.Distinct().ToArray();

			DeviceQueueCreateInfo* queueCreateInfos = (DeviceQueueCreateInfo*)UnsafeUtils.malloc(uniqueQueueFamilies.Length * sizeof(DeviceQueueCreateInfo));

			float queuePriority = 1.0f;
			for (int i = 0; i < uniqueQueueFamilies.Length; i++)
			{
				queueCreateInfos[i] = new()
				{
					SType = StructureType.DeviceQueueCreateInfo,
					QueueFamilyIndex = uniqueQueueFamilies[i],
					QueueCount = 1,
					PQueuePriorities = &queuePriority
				};
			}

			PhysicalDeviceFeatures DeviceFeatures;

			DeviceCreateInfo createInfo = new()
			{
				SType = StructureType.DeviceCreateInfo,
				PQueueCreateInfos = queueCreateInfos,
				QueueCreateInfoCount = (uint)uniqueQueueFamilies.Length,
				PEnabledFeatures = &DeviceFeatures,
				EnabledExtensionCount = (uint)deviceExtensions.Length,
				PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(deviceExtensions),
				EnabledLayerCount = 0,
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

		public virtual unsafe void CRTSRFCS(out SurfaceKHR srfc, out KhrSurface ksrf, Instance inst, IView wnd) 
		{
			if (!vk!.TryGetInstanceExtension(inst, out ksrf))
				// Error, Internal/External Error, Failed to get a KHRSurface. This is most likely a driver or windowing system issue. Please make sure your operating system and hardware support Vulkan.
				ErrorHandler.ThrowError(27, true);
			srfc = wnd!.VkSurface!.Create<AllocationCallbacks>(inst.ToHandle(), null).ToSurface();
		}

		public virtual unsafe void CLNUP(Device DeviceToDestroy, Instance InstanceToDestroy, KhrSurface KhrSurfaceToDestroy, SurfaceKHR SurfaceKHRToDestroy, Vk vk)
		{
			vk.DestroyDevice(DeviceToDestroy, null);
			KhrSurfaceToDestroy.DestroySurface(InstanceToDestroy, SurfaceKHRToDestroy, null);
			vk.DestroyInstance(InstanceToDestroy, null);
		}

		public unsafe virtual QueueFamilyIndices GTQFMLYIDX(PhysicalDevice device, Vk vk)
		{
			QueueFamilyIndices = new QueueFamilyIndices();


			
			// if only c# had functional programming.
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
					QueueFamilyIndices.GraphicsFamily = i;
				}

				Bool32 presentSupport = false;
				KhrWindowSurface.GetPhysicalDeviceSurfaceSupport(device, i, WindowSurfaceKhr, &presentSupport);

				if (presentSupport)
				{
					QueueFamilyIndices.PresentFamily = i;
				}

				if (QueueFamilyIndices.IsComplete())
				{
					break;
				}

				i++;
			}


			return QueueFamilyIndices;
		}

		//get q from indices and logical dvc
		public unsafe virtual void GTQFRMIDCSALGCLDVC(QueueFamilyIndices idx, Device MainDevice, Vk vk, out Queue Pres, out Queue Draw)
		{
			vk!.GetDeviceQueue(MainDevice, idx.GraphicsFamily!.Value, 0, out Draw);
			vk!.GetDeviceQueue(MainDevice, idx.PresentFamily!.Value, 0, out Pres);
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
			// failed to find a gpu
			if (dvcCount == 0) ErrorHandler.ThrowError(24, true);
			PhysicalDevice[] devices = new PhysicalDevice[dvcCount];
			fixed (PhysicalDevice* ptr = &devices[0])
				vk.EnumeratePhysicalDevices(instance, &dvcCount, ptr);
			int[] scores = new int[devices.Length];

			int i = 0;
			foreach (PhysicalDevice physdvc in devices)
				GTVLDPHYSDVC(devices[i], ref scores[i++]);

			int chosenDvc = ArrayUtils.GetHighestValueIndex(scores) - 1;
			if (scores[chosenDvc] == -1)
				//Error, Internal Error, Failed to find a suitable Vulkan GPU. This means that your GPU(s) are not compatible with Vulkan. Sharp will not be able to run on them.
				ErrorHandler.ThrowError(24, true);

			return devices[chosenDvc];
		}

		public virtual Queue GTQHNDL(Device dvc, uint qfmlyidx, Vk vk) =>
			vk.GetDeviceQueue(dvc, qfmlyidx, 0);

		public virtual void GTVLDPHYSDVC(PhysicalDevice physdvc, ref int score)
		{
			PhysicalDeviceProperties physdvcprop = vk.GetPhysicalDeviceProperties(physdvc);
			PhysicalDeviceFeatures physdvcfturs = vk.GetPhysicalDeviceFeatures(physdvc);

			if (physdvcprop.DeviceType == PhysicalDeviceType.DiscreteGpu) score += 5000;
			if (physdvcprop.DeviceType == PhysicalDeviceType.IntegratedGpu) score += 500;
			if (physdvcprop.DeviceType == PhysicalDeviceType.Other) score+=1500;
			if (physdvcprop.DeviceType == PhysicalDeviceType.VirtualGpu) score += 15000;
			score += (int)physdvcprop.Limits.MaxImageDimension2D;

			bool extensionsSupported = VLDEXT(physdvc, deviceExtensions);

			bool swapChainAdequate = false;
			if (extensionsSupported)
			{
				var swapChainSupport = QRYSWPCHNSUP(KhrWindowSurface, WindowSurfaceKhr, physdvc);
				swapChainAdequate = swapChainSupport.Formats.Any() && swapChainSupport.PresentModes.Any();
			}

			if (!physdvcfturs.GeometryShader) score = -1;
			//if (!QueueFamilyIndices.IsComplete()) score = -1;
			if (!extensionsSupported) score = -1;
			if (!swapChainAdequate) score = -1;
		}

		public virtual unsafe bool VLDEXT(PhysicalDevice MainDevice, string[] deviceExts)
		{
			uint extentionsCount = 0;
			Silk.NET.Vulkan.Result res = vk!.EnumerateDeviceExtensionProperties(MainDevice, (byte*)null, ref extentionsCount, null);

			var availableExtensions = new ExtensionProperties[extentionsCount];
			fixed (ExtensionProperties* availableExtensionsPtr = availableExtensions)
			{
				vk!.EnumerateDeviceExtensionProperties(MainDevice, (byte*)null, ref extentionsCount, availableExtensionsPtr);
			}

			var availableExtensionNames = availableExtensions.Select(extension => Marshal.PtrToStringAnsi((IntPtr)extension.ExtensionName)).ToHashSet();

			return deviceExts.All(availableExtensionNames.Contains);
		}

		public unsafe virtual PresentModeKHR ChoosePresentMode(IReadOnlyList<PresentModeKHR> availablePresentModes)
		{
			if (Platform == VKDrawPlatform.Android || Platform == VKDrawPlatform.iOS || Platform == VKDrawPlatform.Other)
				if (VSync)
					return PresentModeKHR.FifoKhr;
				else
					return PresentModeKHR.ImmediateKhr;
			else
				foreach (PresentModeKHR presentMode in availablePresentModes)
					if (presentMode == PresentModeKHR.MailboxKhr)
						return presentMode;
			if (VSync)
				return PresentModeKHR.FifoKhr;
			else
				return PresentModeKHR.ImmediateKhr;
		}

		public unsafe virtual Extent2D CHSWPCHNEXTNT(SurfaceCapabilitiesKHR surfaceCapabilities)
		{
			if (surfaceCapabilities.CurrentExtent.Width != uint.MaxValue)
			{
				return surfaceCapabilities.CurrentExtent;
			}
			else
			{
				var framebufferSize = wnd!.FramebufferSize;

				Extent2D actualExtent = new()
				{
					Width = (uint)framebufferSize.X,
					Height = (uint)framebufferSize.Y
				};

				actualExtent.Width = Math.Clamp(actualExtent.Width, surfaceCapabilities.MinImageExtent.Width, surfaceCapabilities.MaxImageExtent.Width);
				actualExtent.Height = Math.Clamp(actualExtent.Height, surfaceCapabilities.MinImageExtent.Height, surfaceCapabilities.MaxImageExtent.Height);

				return actualExtent;
			}
		} 

		public virtual unsafe SurfaceFormatKHR CHSWPSRFCFRMT(SurfaceFormatKHR[] surfaceFormats)
		{
			foreach (var availableFormat in surfaceFormats)
			{
				if (availableFormat.Format == Format.B8G8R8A8Srgb && availableFormat.ColorSpace == ColorSpaceKHR.SpaceSrgbNonlinearKhr)
				{
					return availableFormat;
				}
			}

			return surfaceFormats[0];
		}

		public virtual unsafe byte** GTEXTS(ref uint count)
		{
			string[] ext = ["VK_KHR_surface"];

			if (Platform == VKDrawPlatform.Windows)
				ext = [.. ext, "VK_KHR_win32_surface"];
			if (Platform == VKDrawPlatform.Linux)
				ext = [.. ext, "VK_KHR_xcb_surface", "VK_KHR_Wayland_surface"];
			if (Platform == VKDrawPlatform.iOS)
				ext = [.. ext, "VK_MVK_ios_surface"];
			if (Platform == VKDrawPlatform.MacOS)
				ext = [.. ext, "VK_MVK_ios_surface"];
			if (Platform == VKDrawPlatform.Android)
				ext = [.. ext, "VK_KHR_android_surface"];
			count = (uint)ext.Length;

			return (byte**)SilkMarshal.StringArrayToPtr(ext);
		}

		public virtual void INITVWRNDRNG(IView vw)
		{
			vw.Run();
		}

		private void VWRNDR(double obj)
		{
			Elapsed = obj;
			TotalElapsed += obj;
		}

		public virtual void STES(IView vw)
		{
			vw.Render += VWRNDR;
			vw.Load += LD;
		}

		public virtual IView CRTWND()
		{
			//VO = ViewOptions.DefaultVulkan;
			//VO.VSync = VSync;
			//VO.API = new(ContextAPI.Vulkan, version);
			//IView vw = Silk.NET.Windowing.Window.GetView(VO);
			

			IView vw = Silk.NET.Windowing.Window.GetView(ViewOptions.DefaultVulkan);
			vw.Initialize();

			if (vw.VkSurface == null)
			{
				// Error, Internal/External Error, Failed to get a IVkSurface. A test will be done after you close this window, to see if your environment supports Vulkan. If you see two windows appear, the test has succeeded. If it does succeed it's an issue on our end, and you can report it at GitHub.com/TheVave/Sharp.
				ErrorHandler.ThrowError(28, false);

				new HelloTriangleApplication().Run();
			}

			return vw;
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
				ApiVersion = Vk.Version10
			};

			InstanceCreateInfo createInfo = new()
			{
				SType = StructureType.InstanceCreateInfo,
				
				PApplicationInfo = &appInfo
			};

			uint c = 0;
			byte** bpp = GTEXTS(ref c);
			createInfo.EnabledExtensionCount = c;
			createInfo.PpEnabledExtensionNames = bpp;

			createInfo.EnabledLayerCount = 0;
			Silk.NET.Vulkan.Result res = vk.CreateInstance(createInfo, null, out inst);

			SilkMarshal.Free((nint)bpp);
			if (res != Silk.NET.Vulkan.Result.Success)
			{
				//if (res.ToString() == "ExtensionNotPresent")
				//	MainSVKRenderer.rndr.TSTENV();
				//Error, Internal Error, Failed to create a Vulkan instance. Sharp may be unable to run on your hardware. Exact error: #1
				ErrorHandler.ThrowError(26, [res.ToString()], true);
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

		//private void TSTENV()
		//{
		//	if (++TestEnvIdx == 1) {
		//		Platform = VKDrawPlatform.Windows;

		//	}
		//	else if (++TestEnvIdx == 2) {
		//		Platform = VKDrawPlatform.Linux;
		//	}
		//	el
		//}
	}
}