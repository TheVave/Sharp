using Sharp._2d.ObjectRepresentation;
using Sharp._2d.Physics.CollisionManagement;

namespace Sharp._2d.Physics
{
	public interface IExecuteAtCollision
	{
		public virtual void Execute(CollisionData data) { }
		public virtual void Execute() { }
		public virtual void Execute(SimulatedObject2d obj1, SimulatedObject2d obj2) { }
	}
}
