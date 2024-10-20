using Silk.NET.Vulkan;

namespace Sharp._2d.Rendering._2DSVKRenderer.Main
{
	public class SVKTexture
	{
		// The texture image
		public Image Image { get; set; }
		// Image view, vulkan's image wrapper
		public ImageView ImageView { get; set; }
		// The image create info
		public ImageViewCreateInfo ImageViewCreateInfo { get; set; }
		// Defaults to no mip-mapping
		public uint[] MipMapLevelRange { get; set; } = { 1, 1 };
		// The image format.
		// Dont look at the options.
		public Format Format { get; set; }
		// The image view type of the image
		public ImageViewType ImageViewType { get; set; } = ImageViewType.Type2D /* don't use depracated ImageViewType.ImageViewType2D */;
		// The image type, swapchain or texture
		public ImageType ImageType { get; set; } = ImageType.Texture;

	}
}
