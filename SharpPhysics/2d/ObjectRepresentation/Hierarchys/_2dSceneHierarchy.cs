namespace SharpPhysics._2d.ObjectRepresentation.Hierarchies
{
	[Serializable]
	public class _2dSceneHierarchy
	{
		public SimulatedObject2d[] Objects = [];
		public byte HierarchyId = 0;

		public _2dSceneHierarchy()
		{
		}

		public _2dSceneHierarchy(SimulatedObject2d[] objects, byte hierarchyId)
		{
			Objects = objects;
			HierarchyId = hierarchyId;
		}

		public static void RegisterObject(byte hierarchyId, SimulatedObject2d obj)
		{
			_2dWorld.SceneHierarchies[hierarchyId].Objects = [.. _2dWorld.SceneHierarchies[hierarchyId].Objects, obj];
		}
	}
}
