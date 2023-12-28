using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	[Serializable]
	public class Comment : UIElement
	{
		public string commentText;

		public override string ToString() => $"Comment/{commentText}/{XPos}/{YPos}/{IsAbsoloutePositionBased}/{objectString}/{ElementStyle}";
	}
}
