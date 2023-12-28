
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	
	[Serializable]
	public abstract class UIElement
	{
		public int XPos = 0,YPos = 0;
		public bool IsAbsoloutePositionBased;
		protected string? objectString;
		internal UIElements ElementStyle;
		public UIElement Parent;
		public abstract override string ToString();
	}
}
