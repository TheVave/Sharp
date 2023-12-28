using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics.Renderer;

namespace SharpPhysics
{
    public class UIPixelOverride : UIElement
	{
		public Color PixelColor = new();
		public override string ToString() => $"UIPixelOverride/{PixelColor}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
