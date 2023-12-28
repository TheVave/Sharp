using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics;

namespace SharpPhysics
{
    public sealed class SimulatedObject
    {
        public Mesh ObjectMesh { get; set; }
        public PhysicsParams ObjectPhysicsParams = new PhysicsParams();
        public Translation Translation;

		public SimulatedObject(Mesh objectMesh, PhysicsParams objectPhysicsParams, Translation translation)
		{
			ObjectMesh = objectMesh;
			ObjectPhysicsParams = objectPhysicsParams;
			Translation = translation;
		}
        internal SimulatedObject() { }

        /// <summary>
        /// Start the physics simulation for the object based on ObjectPhysicsParams
        /// </summary>
        /// <returns></returns>
        public PhysicsSimulator StartPhysicsSimulation()
        {
            PhysicsSimulator physicsSimulator = new PhysicsSimulator(this);
            physicsSimulator.StartPhysicsSimulator();
            return physicsSimulator;

        }
    }
}
