using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics.Simulation.ObjectHierarchy
{
	public class ObjectLink
	{
		public _2dSimulatedObject LinkedWith = new();
		public _2dSimulatedObject Linker = new();
		public LinkType Type = LinkType.Full;

		public void AddLink()
		{
			LinkedWith.ObjectPhysicsParams.LinkedObjects = LinkedWith.ObjectPhysicsParams.LinkedObjects.Append(Linker).ToArray();
		}
		public void RemoveSingleLink()
		{
			LinkedWith.ObjectPhysicsParams.LinkedObjects.ToList().Remove(Linker);
		}
		internal void DissolveLink()
		{
			LinkedWith.ObjectPhysicsParams.LinkedObjects.ToList().Remove(Linker);
		}
	}
}
