using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Utilities
{
	public enum VKDrawPlatform
	{
		Windows,
		// supports X windowing system and Wayland
		Linux,
		MacOS,
		iOS,
		Android,
		Other,
		// Works with Windows, Linux, Mac, iOS, and Android.
		// It is prefferable to use a specific OS, as using this flag will add every listed windowing system
		Unknown
	}

}
