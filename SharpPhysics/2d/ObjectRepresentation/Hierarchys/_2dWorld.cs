using SharpPhysics._2d.ObjectRepresentation.Hierarchies;
using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics
{
	public static class _2dWorld
	{
		public static _2dSceneHierarchy[] SceneHierarchies = [];
		public static string[] HierarchyNames = [];
		internal static int[] ids = [];
		public static void RegisterSceneHierarchy(_2dSceneHierarchy hierarchy)
		{
			try
			{
				SceneHierarchies = [.. SceneHierarchies, hierarchy];
			}
			catch (System.Exception e)
			{
				ErrorHandler.ThrowError("Error, Unknown Error, Exact error: " + e, true);
			}
		}
	}
}