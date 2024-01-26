using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.DXRendering
{
	public class StandardDXDisplay : DXRenderer
	{
		IWindow wnd;

		public override void Draw()
		{
			
		}

		public override void InitRendering()
		{
			wnd = Window.Create(WindowOptions.Default);
			wnd.Run();
		}

		public override void LoadContent()
		{
			
		}

		public override void Update()
		{
			
		}
	}
}
