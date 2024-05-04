using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics.Simulation.ObjectHierarchy
{
	public class SceneHierarchy : IAny
	{
		/// <summary>
		/// The last updated scene ID of the scene
		/// </summary>
		public int? LastSceneID = null;

		public SimulatedObject2d[] Objects;

		internal SimulatedObject2d[] objs = [new()];
	}
}
