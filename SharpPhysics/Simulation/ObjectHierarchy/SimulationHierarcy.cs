using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	internal static class SimulationHierarcy
	{
		public static SceneHierarchy[] Hierarchies = new SceneHierarchy[] { new() };
		public static int GetObjectCount() 
		{
			int returnVal = 0;
			for (int i =0; i < Hierarchies.Length; i++)
			{
				returnVal += Hierarchies[i].Objects.Length;
			}
			return returnVal;
		}
	}
}
