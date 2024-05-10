using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;

namespace Sharp.Simulation.ObjectHierarchy
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
