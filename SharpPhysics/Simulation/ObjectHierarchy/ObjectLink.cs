using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics.Simulation.ObjectHierarchy
{
	public class ObjectLink : IAny
	{
		public SimulatedObject2d LinkedWith = new();
		public SimulatedObject2d Linker = new();
		public LinkType Type = LinkType.Full;
	}
}
