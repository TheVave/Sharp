using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Renderer.Textures
{
	public class Texture
	{
		public byte[] ImageBytes;
		public short ImageWidth;
		public short ImageHeight;
		public bool ContainsAlphaInfo = false;
	}
}
