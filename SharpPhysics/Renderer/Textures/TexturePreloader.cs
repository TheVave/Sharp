using GLFW;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC.Errors;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpPhysics.Renderer.Textures
{
	public static class TexturePreloader
	{
		public static Texture GetFilePreloadedInfo(string name)
		{
			string[] txtrContents = [];
			try
			{
				txtrContents = File.ReadAllLines($@"{Environment.CurrentDirectory}\Ctnt\Txtrs\{name}");
			}
			catch
			{
				ErrorHandler.ThrowError("Error, External Error, Missing texture", true);
			}
			Texture texture = new();
			try
			{
				texture.ImageWidth = (short)GenericMathUtils.ParseStrToInt32(txtrContents[0]);
				texture.ImageHeight = (short)GenericMathUtils.ParseStrToInt32(txtrContents[0][(txtrContents[0].IndexOf(' ') + 1)..]);
			}
			catch (ArgumentOutOfRangeException e)
			{
				ErrorHandler.ThrowError("Error, External Error, Invalid texture format.", true);
			}
			catch (InvalidCastException e)
			{
				ErrorHandler.ThrowError("Error, External error, Image too big for short. Image width/height can't exceed 32000.", true);
			}
			List<byte> imageContents = [];
			try
			{
				foreach (string line in txtrContents) imageContents.AddRange(Encoding.GetEncoding("UTF-8").GetBytes(line));
			}
			catch (ArgumentNullException e)
			{
				ErrorHandler.ThrowError("Error, External Error, Texture does not have enough info.", true);
			}
			catch (ArgumentException e)
			{
				ErrorHandler.ThrowError("Error, Unknown error, Exact error: " + e, true);
			}
			try
			{
				texture.ImageBytes = new byte[imageContents.Count];
			}
			catch
			{
				ErrorHandler.ThrowError("Error, External Error, Image uses too much memory.", true);
			}
			try
			{
				Array.Copy(imageContents.ToArray(), texture.ImageBytes, imageContents.Count);
			}
			catch (System.Exception e)
			{
				ErrorHandler.ThrowError("Error, Internal Error, Possible missing data.", true);
			}
			return texture;
		}
		public static void SaveTexture(string name, Bitmap bmp)
		{
			MemoryStream strm = new();
			try
			{
				bmp.Save(strm, ImageFormat.Bmp);
			}
			catch (ExternalException ex) 
			{
				Console.Error.WriteLine("Non-Windows OS, SharpPhysics is only compatible with Windows.");
			}
			catch
			{
				ErrorHandler.ThrowError("Unknown Error, TexturePreloader class. SaveTexture(string, bitmap)", true);
			}
			strm.Position = 0;
			try
			{
				byte[] bytes = new byte[strm.Length];
				strm.Read(bytes, 0, (int)strm.Length);
			}
			catch (ObjectDisposedException e)
			{
				ErrorHandler.ThrowError("Error, Internal Error, image disposed, " + name, false);
			}
			catch (InvalidCastException ice)
			{
				ErrorHandler.ThrowError("Error, External Error, Image too big. image size exceeds 4,294,967,296 (2^32), please make the image smaller.", true);
			}
			catch (System.Exception e)
			{
				ErrorHandler.ThrowError("Error, Internal Error, Unknown exception, TexturePreloader class. Exact error: " + e.Message, true);
			}
		}
	}
}
