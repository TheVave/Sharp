using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	[Serializable]
	public class ProgressBar : UIElement
	{
		byte percent = 0;

		public override string ToString() => $"ProgressBar/0/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
