using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildIncrmtr
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string str = File.ReadAllText(@"O:\users\tobya\source\repos\SharpPhysics\buildnumber...");
			int num = int.Parse(str);
			num++;
			File.WriteAllText(@"O:\users\tobya\source\repos\SharpPhysics\buildnumber", num.ToString());
		}
	}
}
