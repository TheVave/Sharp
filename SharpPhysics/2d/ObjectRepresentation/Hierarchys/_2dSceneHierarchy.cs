
using SharpPhysics.Utilities.MISC.Errors;
using System.Runtime.CompilerServices;

namespace SharpPhysics._2d.ObjectRepresentation.Hierarchies
{
	public class _2dSceneHierarchy
	{
        public SimulatedObject2d[] Objects = [];
        public byte HierarchyId = 0;
		public static void RegisterObject(byte hierarchyId, SimulatedObject2d obj)
		{
			_2dWorld.SceneHierarchies[hierarchyId].Objects = [.. _2dWorld.SceneHierarchies[hierarchyId].Objects, obj];
		}
	}
}
