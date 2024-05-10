using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC;

namespace Sharp.Simulation
{
	public static class ObjectRegistrationHelper
	{
		public static void Register2dObject(SimulatedObject2d obj, int sceneId)
		{
			if (!_2dWorld.ids.Contains(sceneId))
			{
				_2dWorld.ids.ToList().Add(sceneId);
				_2dWorld.SceneHierarchies = [.. _2dWorld.SceneHierarchies, new _2d.ObjectRepresentation.Hierarchies._2dSceneHierarchy()];
			}
			ArrayUtils.AppendArrayObject(_2dWorld.SceneHierarchies[sceneId].Objects, obj);
		}
	}
}
