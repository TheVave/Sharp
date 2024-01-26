using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core;

namespace SharpPhysics.DXRendering
{
	public abstract class DXRenderer
	{
		public abstract void Draw();
		public abstract void Update();
		public abstract void LoadContent();
		public abstract void InitRendering();
	}
}
