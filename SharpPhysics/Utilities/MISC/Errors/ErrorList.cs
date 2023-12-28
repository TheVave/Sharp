using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	[Serializable]
	public class ErrorList
	{
		public Tuple<string, int>[] Errors = {
			new("That is not implemented.", 0),
			new("Undefined error.", 1),
			new("Missing config files.", 2),
			new("Non-Windows or pre-Windows 2000 machine, the current device does not have the file User32.dll.", 3),
			new("Can't divide by zero.", 4),
			new("Can't have negative scale.", 5),
			new("NumberToLarge exeption, the number inputted is too large for the number type.", 6),
			new("MEM_ERROR",7),
			new("Mesh manifest missing.", 8),
			new("Mesh manifest points to a non-existant file.", 9),
			new("Interpret error, can't interpret some file.", 10),
			new("Interpret error, can't interpret the animation.", 11),
			new("Interpret error, can't interpret the mesh.", 12),
			new("Mesh manifest can't have non-anm or non-msh file extentions,", 13),
			new("No error info, missing .err file.", 14)
		};
	}
}
