using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics.Simulation
{
	public static class ObjectRegistrationHelper
	{
		public static void Register2dObject(SimulatedObject2d obj, int sceneId)
		{
			if (!_2dWorld.ids.Contains(sceneId))
			{
				_2dWorld.ids.ToList().Add(sceneId);

			}
		}
	}
}
