using SharpPhysics._2d.ObjectRepresentation.Hierarchies;

namespace SharpPhysics
{
	public static class _2dWorld
	{
		public static _2dSceneHierarchy[] SceneHierarchies = new _2dSceneHierarchy[0];
		public static string[] HierarchyNames = new string[0];
		internal static int[] ids = new int[0];
		public static void RegisterSceneHierarchy(_2dSceneHierarchy hierarchy)
		{
			SceneHierarchies = SceneHierarchies.Append(hierarchy).ToArray();
		}
	}
}