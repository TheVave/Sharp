using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics.Renderer;

namespace SharpPhysics
{
    [Serializable]
	public class Label : UIElement
	{
		public string Content;
		public int FontSize;
		public Color BackgroundColor;
		public Color ForegroundColor;

		public override string ToString() => $"Label/{Content}/{FontSize}/{BackgroundColor}/{ForegroundColor}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
