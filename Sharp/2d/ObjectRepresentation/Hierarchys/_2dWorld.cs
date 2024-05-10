using Sharp._2d.ObjectRepresentation.Hierarchies;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC;

namespace Sharp
{
	[Serializable]
	public static class _2dWorld
	{
		public static _2dSceneHierarchy[] SceneHierarchies = [new()];
		public static string[] HierarchyNames = ["Main"];
		internal static int[] ids = [0];
		public static void RegisterSceneHierarchy(_2dSceneHierarchy hierarchy)
		{
			try
			{
				SceneHierarchies = [.. SceneHierarchies, hierarchy];
			}
			catch (System.Exception e)
			{
				//5
				ErrorHandler.ThrowError(5, true);
				throw;
			}
		}
	}
}