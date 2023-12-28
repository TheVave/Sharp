using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class MainUIElementParser
	{
		public static UIElement[] elements = Array.Empty<UIElement>();
		public static void ParseXaml(string Pagepath)
		{
			string[] str = File.ReadAllLines(Pagepath);
			// Tokenizing
			
		}
	}
}
