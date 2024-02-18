using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics.Simulation.ObjectHierarchy
{
	public class ObjectLink
	{
		public SimulatedObject2d LinkedWith = new();
		public SimulatedObject2d Linker = new();
		public LinkType Type = LinkType.Full;
	}
}
