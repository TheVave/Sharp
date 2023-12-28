﻿namespace SharpPhysics
{
	internal static class SimulationHierarchy
	{
		public static SceneHierarchy[] Hierarchies = new SceneHierarchy[] { new() };
		public static int GetObjectCount()
		{
			int returnVal = 0;
			for (int i = 0; i < Hierarchies.Length; i++)
			{
				returnVal += Hierarchies[i].Objects.Length;
			}
			return returnVal;
		}
	}
}
