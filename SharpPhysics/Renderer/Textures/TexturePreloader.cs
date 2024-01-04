using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Renderer.Textures
{
	public static class TexturePreloader
	{
		public static Texture GetFilePreloadedInfo(string name)
		{
			string[] txtrContents = File.ReadAllLines($@"{Environment.CurrentDirectory}\Ctnt\Txtrs\{name}");
			Texture texture = new();
			texture.ImageWidth = (short)GenericMathUtils.ParseStrToInt32(txtrContents[0]);
			texture.ImageHeight = (short)GenericMathUtils.ParseStrToInt32(txtrContents[0].Substring(txtrContents[0].IndexOf(' ') + 1));
			List<byte> imageContents = new List<byte>();
			foreach (string line in txtrContents) imageContents.AddRange(Encoding.GetEncoding("UTF-8").GetBytes(line));
			texture.ImageBytes = new byte[imageContents.Count];
			Array.Copy(imageContents.ToArray(), texture.ImageBytes, imageContents.Count);
			return texture;
		}
	}
}
