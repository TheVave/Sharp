using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics;

namespace SharpPhysics
{
    public sealed class _2dSimulatedObject
    {
        public Mesh ObjectMesh { get; set; }
        public PhysicsParams ObjectPhysicsParams = new PhysicsParams();
        public Translation Translation;

		public _2dSimulatedObject(Mesh objectMesh, PhysicsParams objectPhysicsParams, Translation translation)
		{
			ObjectMesh = objectMesh;
			ObjectPhysicsParams = objectPhysicsParams;
			Translation = translation;
		}
        internal _2dSimulatedObject() { }

        /// <summary>
        /// Start the physics simulation for the object based on ObjectPhysicsParams
        /// </summary>
        /// <returns></returns>
        public _2dPhysicsSimulator StartPhysicsSimulation()
        {
            _2dPhysicsSimulator physicsSimulator = new _2dPhysicsSimulator(this);
            physicsSimulator.StartPhysicsSimulator();
            return physicsSimulator;

        }
    }
}
