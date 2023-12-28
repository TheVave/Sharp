using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class ErrorFileCreator
	{
		public static void CreateErrorFile()
		{
			ErrorList errors = new();
			string json = JsonSerializer.Serialize(errors);
			File.WriteAllText(@$"{Environment.CurrentDirectory}\ErrorAsm", json);
		}
	}
}
