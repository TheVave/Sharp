using Sharp.Utilities.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp._2d.Rendering._2DSVKRenderer.Main
{
	public class SVKObject
	{
		public ulong? ObjectIndex = null;
		// Wrapper for InternalSVKRenderer.Textures through ObjectIndex
		public SVKTexture Texture 
		{ 
			get
			{
				if (ObjectIndex is null)
					ErrorHandler.ThrowError(29, ["NullReferenceException: SVKObject 'get' was called before ObjectIndex was set"], true);
				return MainSVKRenderer.rndr.Textures[(ulong)ObjectIndex];
			}
			set
			{
				if (ObjectIndex is null)
					ErrorHandler.ThrowError(29, ["NullReferenceException: SVKObject 'set' was called before ObjectIndex was set"], true);
				MainSVKRenderer.rndr.Textures[(ulong)ObjectIndex] = value;
			} 
		}
	}
}
