﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;


//var app = new HelloTriangleApplication();
//app.Run();

struct QueueFamilyIndices
{
	public uint? GraphicsFamily { get; set; }
	public uint? PresentFamily { get; set; }

	public bool IsComplete()
	{
		return GraphicsFamily.HasValue && PresentFamily.HasValue;
	}
}

struct SwapChainSupportDetails
{
	public SurfaceCapabilitiesKHR Capabilities;
	public SurfaceFormatKHR[] Formats;
	public PresentModeKHR[] PresentModes;
}

unsafe class HelloTriangleApplication
{
	const int WIDTH = 800;
	const int HEIGHT = 600;

	bool EnableValidationLayers = false;

	private readonly string[] validationLayers = new[]
	{
		"VK_LAYER_KHRONOS_validation"
	};

	private readonly string[] deviceExtensions = new[]
	{
		KhrSwapchain.ExtensionName
	};

	private IWindow? window;
	private Vk? vk;

	private Instance instance;

	private ExtDebugUtils? debugUtils;
	private DebugUtilsMessengerEXT debugMessenger;
	private KhrSurface? khrSurface;
	private SurfaceKHR surface;

	private PhysicalDevice physicalDevice;
	private Device device;

	private Queue graphicsQueue;
	private Queue presentQueue;

	private KhrSwapchain? khrSwapChain;
	private SwapchainKHR swapChain;
	private Image[]? swapChainImages;
	private Format swapChainImageFormat;
	private Extent2D swapChainExtent;

	public void Run()
	{
		InitWindow();
		InitVulkan();
		MainLoop();
		CleanUp();
	}

	private void InitWindow()
	{
		//Create a window.
		var options = WindowOptions.DefaultVulkan with
		{
			Size = new Vector2D<int>(WIDTH, HEIGHT),
			Title = "Vulkan",
		};

		window = Window.Create(options);
		window.Initialize();

		if (window.VkSurface is null)
		{
			throw new Exception("Windowing platform doesn't support Vulkan.");
		}
	}

	private void InitVulkan()
	{
		CreateInstance();
		SetupDebugMessenger();
		CreateSurface();
		PickPhysicalDevice();
		CreateLogicalDevice();
		CreateSwapChain();
	}

	private void MainLoop()
	{
		window!.Run();
	}

	private void CleanUp()
	{
		khrSwapChain!.DestroySwapchain(device, swapChain, null);

		vk!.DestroyDevice(device, null);

		if (EnableValidationLayers)
		{
			//DestroyDebugUtilsMessenger equivilant to method DestroyDebugUtilsMessengerEXT from original tutorial.
			debugUtils!.DestroyDebugUtilsMessenger(instance, debugMessenger, null);
		}

		khrSurface!.DestroySurface(instance, surface, null);
		vk!.DestroyInstance(instance, null);
		vk!.Dispose();

		window?.Dispose();
	}

	private void CreateInstance()
	{
		vk = Vk.GetApi();

		if (EnableValidationLayers && !CheckValidationLayerSupport())
		{
			throw new Exception("validation layers requested, but not available!");
		}

		ApplicationInfo appInfo = new()
		{
			SType = StructureType.ApplicationInfo,
			PApplicationName = (byte*)Marshal.StringToHGlobalAnsi("Hello Triangle"),
			ApplicationVersion = new Version32(1, 0, 0),
			PEngineName = (byte*)Marshal.StringToHGlobalAnsi("No Engine"),
			EngineVersion = new Version32(1, 0, 0),
			ApiVersion = Vk.Version12
		};

		InstanceCreateInfo createInfo = new()
		{
			SType = StructureType.InstanceCreateInfo,
			PApplicationInfo = &appInfo
		};

		var extensions = GetRequiredExtensions();
		createInfo.EnabledExtensionCount = (uint)extensions.Length;
		createInfo.PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(extensions); ;

		if (EnableValidationLayers)
		{
			createInfo.EnabledLayerCount = (uint)validationLayers.Length;
			createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers);

			DebugUtilsMessengerCreateInfoEXT debugCreateInfo = new();
			PopulateDebugMessengerCreateInfo(ref debugCreateInfo);
			createInfo.PNext = &debugCreateInfo;
		}
		else
		{
			createInfo.EnabledLayerCount = 0;
			createInfo.PNext = null;
		}

		if (vk.CreateInstance(createInfo, null, out instance) != Result.Success)
		{
			throw new Exception("failed to create instance!");
		}

		Marshal.FreeHGlobal((IntPtr)appInfo.PApplicationName);
		Marshal.FreeHGlobal((IntPtr)appInfo.PEngineName);
		SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

		if (EnableValidationLayers)
		{
			SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
		}
	}

	private void PopulateDebugMessengerCreateInfo(ref DebugUtilsMessengerCreateInfoEXT createInfo)
	{
		createInfo.SType = StructureType.DebugUtilsMessengerCreateInfoExt;
		createInfo.MessageSeverity = DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt |
									 DebugUtilsMessageSeverityFlagsEXT.WarningBitExt |
									 DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt;
		createInfo.MessageType = DebugUtilsMessageTypeFlagsEXT.GeneralBitExt |
								 DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt |
								 DebugUtilsMessageTypeFlagsEXT.ValidationBitExt;
		createInfo.PfnUserCallback = (DebugUtilsMessengerCallbackFunctionEXT)DebugCallback;
	}

	private void SetupDebugMessenger()
	{
		if (!EnableValidationLayers) return;

		//TryGetInstanceExtension equivilant to method CreateDebugUtilsMessengerEXT from original tutorial.
		if (!vk!.TryGetInstanceExtension(instance, out debugUtils)) return;

		DebugUtilsMessengerCreateInfoEXT createInfo = new();
		PopulateDebugMessengerCreateInfo(ref createInfo);

		if (debugUtils!.CreateDebugUtilsMessenger(instance, in createInfo, null, out debugMessenger) != Result.Success)
		{
			throw new Exception("failed to set up debug messenger!");
		}
	}

	private void CreateSurface()
	{
		if (!vk!.TryGetInstanceExtension<KhrSurface>(instance, out khrSurface))
		{
			throw new NotSupportedException("KHR_surface extension not found.");
		}

		surface = window!.VkSurface!.Create<AllocationCallbacks>(instance.ToHandle(), null).ToSurface();
	}

	private void PickPhysicalDevice()
	{
		var devices = vk!.GetPhysicalDevices(instance);

		foreach (var device in devices)
		{
			if (IsDeviceSuitable(device))
			{
				physicalDevice = device;
				break;
			}
		}

		if (physicalDevice.Handle == 0)
		{
			throw new Exception("failed to find a suitable GPU!");
		}
	}

	private void CreateLogicalDevice()
	{
		var indices = FindQueueFamilies(physicalDevice);

		var uniqueQueueFamilies = new[] { indices.GraphicsFamily!.Value, indices.PresentFamily!.Value };
		uniqueQueueFamilies = uniqueQueueFamilies.Distinct().ToArray();

		using var mem = GlobalMemory.Allocate(uniqueQueueFamilies.Length * sizeof(DeviceQueueCreateInfo));
		var queueCreateInfos = (DeviceQueueCreateInfo*)Unsafe.AsPointer(ref mem.GetPinnableReference());

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

		PhysicalDeviceFeatures deviceFeatures = new();

		DeviceCreateInfo createInfo = new()
		{
			SType = StructureType.DeviceCreateInfo,
			QueueCreateInfoCount = (uint)uniqueQueueFamilies.Length,
			PQueueCreateInfos = queueCreateInfos,

			PEnabledFeatures = &deviceFeatures,

			EnabledExtensionCount = (uint)deviceExtensions.Length,
			PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(deviceExtensions)
		};

		if (EnableValidationLayers)
		{
			createInfo.EnabledLayerCount = (uint)validationLayers.Length;
			createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers);
		}
		else
		{
			createInfo.EnabledLayerCount = 0;
		}

		if (vk!.CreateDevice(physicalDevice, in createInfo, null, out device) != Result.Success)
		{
			throw new Exception("failed to create logical device!");
		}

		vk!.GetDeviceQueue(device, indices.GraphicsFamily!.Value, 0, out graphicsQueue);
		vk!.GetDeviceQueue(device, indices.PresentFamily!.Value, 0, out presentQueue);

		if (EnableValidationLayers)
		{
			SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
		}

		SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

	}

	private void CreateSwapChain()
	{
		var swapChainSupport = QuerySwapChainSupport(physicalDevice);

		var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
		var presentMode = ChoosePresentMode(swapChainSupport.PresentModes);
		var extent = ChooseSwapExtent(swapChainSupport.Capabilities);

		var imageCount = swapChainSupport.Capabilities.MinImageCount + 1;
		if (swapChainSupport.Capabilities.MaxImageCount > 0 && imageCount > swapChainSupport.Capabilities.MaxImageCount)
		{
			imageCount = swapChainSupport.Capabilities.MaxImageCount;
		}

		SwapchainCreateInfoKHR creatInfo = new()
		{
			SType = StructureType.SwapchainCreateInfoKhr,
			Surface = surface,

			MinImageCount = imageCount,
			ImageFormat = surfaceFormat.Format,
			ImageColorSpace = surfaceFormat.ColorSpace,
			ImageExtent = extent,
			ImageArrayLayers = 1,
			ImageUsage = ImageUsageFlags.ColorAttachmentBit,
		};

		var indices = FindQueueFamilies(physicalDevice);
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

		if (!vk!.TryGetDeviceExtension(instance, device, out khrSwapChain))
		{
			throw new NotSupportedException("VK_KHR_swapchain extension not found.");
		}

		if (khrSwapChain!.CreateSwapchain(device, creatInfo, null, out swapChain) != Result.Success)
		{
			throw new Exception("failed to create swap chain!");
		}

		khrSwapChain.GetSwapchainImages(device, swapChain, ref imageCount, null);
		swapChainImages = new Image[imageCount];
		fixed (Image* swapChainImagesPtr = swapChainImages)
		{
			khrSwapChain.GetSwapchainImages(device, swapChain, ref imageCount, swapChainImagesPtr);
		}

		swapChainImageFormat = surfaceFormat.Format;
		swapChainExtent = extent;
	}

	private SurfaceFormatKHR ChooseSwapSurfaceFormat(IReadOnlyList<SurfaceFormatKHR> availableFormats)
	{
		foreach (var availableFormat in availableFormats)
		{
			if (availableFormat.Format == Format.B8G8R8A8Srgb && availableFormat.ColorSpace == ColorSpaceKHR.SpaceSrgbNonlinearKhr)
			{
				return availableFormat;
			}
		}

		return availableFormats[0];
	}

	private PresentModeKHR ChoosePresentMode(IReadOnlyList<PresentModeKHR> availablePresentModes)
	{
		foreach (var availablePresentMode in availablePresentModes)
		{
			if (availablePresentMode == PresentModeKHR.MailboxKhr)
			{
				return availablePresentMode;
			}
		}

		return PresentModeKHR.FifoKhr;
	}

	private Extent2D ChooseSwapExtent(SurfaceCapabilitiesKHR capabilities)
	{
		if (capabilities.CurrentExtent.Width != uint.MaxValue)
		{
			return capabilities.CurrentExtent;
		}
		else
		{
			var framebufferSize = window!.FramebufferSize;

			Extent2D actualExtent = new()
			{
				Width = (uint)framebufferSize.X,
				Height = (uint)framebufferSize.Y
			};

			actualExtent.Width = Math.Clamp(actualExtent.Width, capabilities.MinImageExtent.Width, capabilities.MaxImageExtent.Width);
			actualExtent.Height = Math.Clamp(actualExtent.Height, capabilities.MinImageExtent.Height, capabilities.MaxImageExtent.Height);

			return actualExtent;
		}
	}

	private SwapChainSupportDetails QuerySwapChainSupport(PhysicalDevice physicalDevice)
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

	private bool IsDeviceSuitable(PhysicalDevice device)
	{
		var indices = FindQueueFamilies(device);

		bool extensionsSupported = CheckDeviceExtensionsSupport(device);

		bool swapChainAdequate = false;
		if (extensionsSupported)
		{
			var swapChainSupport = QuerySwapChainSupport(device);
			swapChainAdequate = swapChainSupport.Formats.Any() && swapChainSupport.PresentModes.Any();
		}

		return indices.IsComplete() && extensionsSupported && swapChainAdequate;
	}

	private bool CheckDeviceExtensionsSupport(PhysicalDevice device)
	{
		uint extentionsCount = 0;
		vk!.EnumerateDeviceExtensionProperties(device, (byte*)null, ref extentionsCount, null);

		var availableExtensions = new ExtensionProperties[extentionsCount];
		fixed (ExtensionProperties* availableExtensionsPtr = availableExtensions)
		{
			vk!.EnumerateDeviceExtensionProperties(device, (byte*)null, ref extentionsCount, availableExtensionsPtr);
		}

		var availableExtensionNames = availableExtensions.Select(extension => Marshal.PtrToStringAnsi((IntPtr)extension.ExtensionName)).ToHashSet();

		return deviceExtensions.All(availableExtensionNames.Contains);

	}

	private QueueFamilyIndices FindQueueFamilies(PhysicalDevice device)
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

			khrSurface!.GetPhysicalDeviceSurfaceSupport(device, i, surface, out var presentSupport);

			if (presentSupport)
			{
				indices.PresentFamily = i;
			}

			if (indices.IsComplete())
			{
				break;
			}

			i++;
		}

		return indices;
	}

	private string[] GetRequiredExtensions()
	{
		var glfwExtensions = window!.VkSurface!.GetRequiredExtensions(out var glfwExtensionCount);
		var extensions = SilkMarshal.PtrToStringArray((nint)glfwExtensions, (int)glfwExtensionCount);

		if (EnableValidationLayers)
		{
			return extensions.Append(ExtDebugUtils.ExtensionName).ToArray();
		}

		return extensions;
	}

	private bool CheckValidationLayerSupport()
	{
		uint layerCount = 0;
		vk!.EnumerateInstanceLayerProperties(ref layerCount, null);
		var availableLayers = new LayerProperties[layerCount];
		fixed (LayerProperties* availableLayersPtr = availableLayers)
		{
			vk!.EnumerateInstanceLayerProperties(ref layerCount, availableLayersPtr);
		}

		var availableLayerNames = availableLayers.Select(layer => Marshal.PtrToStringAnsi((IntPtr)layer.LayerName)).ToHashSet();

		return validationLayers.All(availableLayerNames.Contains);
	}

	private uint DebugCallback(DebugUtilsMessageSeverityFlagsEXT messageSeverity, DebugUtilsMessageTypeFlagsEXT messageTypes, DebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
	{
		Console.WriteLine($"validation layer:" + Marshal.PtrToStringAnsi((nint)pCallbackData->PMessage));

		return Vk.False;
	}
}