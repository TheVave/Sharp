using GLFW;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC.Errors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
				txtrContents = File.ReadAllLines($@"{Environment.CurrentDirectory}\Ctnt\Txtrs\{name}.txr");
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
				ErrorHandler.ThrowError("Error, External error, Image too big for short. Image width/height can't exceed 32600.", true);
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
				texture.ImageBytes = new byte[texture.ImageWidth * texture.ImageHeight];
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
				ErrorHandler.ThrowError("Error, Internal Error, Possible missing data or incorrect image size.", true);
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
			byte[] bytes = [];
			try
			{
				bytes = new byte[strm.Length];
				strm.Read(bytes, 0, (int)strm.Length);
			}
			catch (ObjectDisposedException e)
			{
				ErrorHandler.ThrowError("Error, Internal Error, image disposed, " + name, true);
			}
			catch (InvalidCastException ice)
			{
				ErrorHandler.ThrowError("Error, External Error, Image too big. image size exceeds 4,294,967,296 (2^32), please make the image smaller.", true);
			}
			catch (System.Exception e)
			{
				ErrorHandler.ThrowError("Error, Internal Error, Unknown exception, TexturePreloader class. Exact error: " + e.Message, true);
			}
			string firstLine = bmp.Width + " " + bmp.Height + "\n";
			File.WriteAllText($"{Environment.CurrentDirectory}\\Ctnt\\Txtrs\\main.txr", firstLine);
			File.WriteAllBytes($"{Environment.CurrentDirectory}\\Ctnt\\Txtrs\\main.txr", File.ReadAllBytes($"{Environment.CurrentDirectory}\\Ctnt\\Txtrs\\main.txr").Concat(bytes).ToArray());
		}
	}
}