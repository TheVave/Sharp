using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics;

namespace SharpPhysics
{
    public struct _2dSimulatedObject
    {
        public Mesh ObjectMesh { get; set; }
        public _2dPhysicsParams ObjectPhysicsParams;
        public _2dTranslation Translation;

		public _2dSimulatedObject(Mesh objectMesh, _2dPhysicsParams objectPhysicsParams, _2dTranslation translation)
		{
			ObjectMesh = objectMesh;
			ObjectPhysicsParams = objectPhysicsParams;
			Translation = translation;
			SimulationHierarcy.Hierarchies[0].Objects = SimulationHierarcy.Hierarchies[0].Objects.Append(this).ToArray();
		}

		public _2dSimulatedObject()
		{
			ObjectMesh = _2dBaseObjects.LoadSquareMesh();
			ObjectPhysicsParams = new _2dPhysicsParams();
			Translation = new _2dTranslation();
			
		}

		public void RegisterToScene(int scene)
		{
			SimulationHierarcy.Hierarchies[scene].Objects = SimulationHierarcy.Hierarchies[0].Objects.Append(this).ToArray();
		}
		public void RegisterToScene()
		{
			SimulationHierarcy.Hierarchies[0].Objects = SimulationHierarcy.Hierarchies[0].Objects.Append(this).ToArray();
		}

        /// <summary>
        /// Start the physics simulation for the object based on ObjectPhysicsParams
        /// </summary>
        /// <returns></returns>
        public _2dPhysicsSimulator StartPhysicsSimulation()
        {
            _2dPhysicsSimulator physicsSimulator = new(this);
            physicsSimulator.StartPhysicsSimulator();
            return physicsSimulator;

        }
    }
}
