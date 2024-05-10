using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;

namespace Sharp.Simulation.ObjectHierarchy
{
	public class ObjectLink : IAny
	{
		public SimulatedObject2d LinkedWith = new();
		public SimulatedObject2d Linker = new();
		public LinkType Type = LinkType.Full;
	}
}
