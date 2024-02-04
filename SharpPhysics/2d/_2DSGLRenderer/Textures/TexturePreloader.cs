using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpPhysics.Renderer.Textures
{
	public static class TexturePreloader
	{
		public static Texture GetFilePreloadedInfo(string name)
		{
			System.Drawing.Bitmap txrBmp = (Bitmap)Bitmap.FromFile($@"{Environment.CurrentDirectory}\Ctnt\Txtrs\{name}.txr");
			// PixelFormat.Format24bppRGB reserves the colors each a 8 byte (255 max) area, packed back to back.
			// Most common bitmap format
			BitmapData data = txrBmp.LockBits(new Rectangle(0, 0, txrBmp.Width, txrBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			Texture toReturn = new();
			toReturn.ContainsAlphaInfo = false;
			toReturn.ImageWidth = (short)data.Width;
			toReturn.ImageHeight = (short)data.Height;
			byte[] content = new byte[toReturn.ImageWidth * toReturn.ImageHeight];
			nint ptr = data.Scan0;
			Marshal.Copy(ptr, content, 0, data.Width * data.Height);
			toReturn.ImageBytes = content;
			return toReturn;
		}
		public static void SaveTexture(string name, Bitmap bmp)
		{
			bmp.Save($"{Environment.CurrentDirectory}\\Ctnt\\Txtrs\\{name}.txr");
		}
	}
}