using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics;

namespace SharpPhysics
{
    public class PhysicsSimulator
    {

        /// <summary>
        /// The object to simulate
        /// </summary>
        public SimulatedObject ObjectToSimulate;

        /// <summary>
        /// bouncy multiplier
        /// </summary>
        public int ShocksMultiplier = 0;


        public bool StopSignal = false;

        public bool TickSignal = false;

        /// <summary>
        /// WARNING: this has a maximum of 1000
        /// </summary>
        public int TickSpeed = 500;
        public MovmentRepersenter CurrentMovment { get; private set; }
        public int SpeedMultiplier = 10;
        public virtual void ExecuteAtCollision(SimulatedObject hitObject, SimulatedObject self) { }
        public int DelayAmount;
        public double TimePerSimulationTick;

        private readonly bool DoManualTicking = false;

        readonly SUVATEquations sUVATEquations = new SUVATEquations();

        double displacement;

        //calculating the position based on the moving position
        private double[] speedDirection;

        public PhysicsSimulator(SimulatedObject objectToSimulate)
        {
            ObjectToSimulate = objectToSimulate;
        }

        public PhysicsSimulator(SimulatedObject objectToSimulate, int shocksMultiplier)
        {
            ShocksMultiplier = shocksMultiplier;
            ObjectToSimulate = objectToSimulate;
        }

        public PhysicsSimulator(SimulatedObject objectToSimulate, int shocksMultiplier, bool doManualTicking)
        {
            DoManualTicking = doManualTicking;
            ObjectToSimulate = objectToSimulate;
            ShocksMultiplier = shocksMultiplier;
        }

        internal PhysicsSimulator()
        {
        }

        internal void Tick()
        {
            //TODO find if the object collides with another object

            //Starting math for moving 1d

            sUVATEquations.T = TimePerSimulationTick;
            sUVATEquations.VS = ObjectToSimulate.ObjectPhysicsParams.Speed;
            sUVATEquations.A = ObjectToSimulate.ObjectPhysicsParams.SpeedAcceleration;

            displacement = sUVATEquations.NSWVSTA();

            //calculating the position based on the moving position
            speedDirection = ObjectToSimulate.ObjectPhysicsParams.SpeedDirection;

            // do standard calculations to find the displacement in a given direction
			ObjectToSimulate.Translation.ObjectPosition.xPos += ((speedDirection[0] + 1) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[0];
            ObjectToSimulate.Translation.ObjectPosition.yPos += (((speedDirection[1] + 1) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[1]) - ObjectToSimulate.ObjectPhysicsParams.GravityMultiplier * 9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass;
            ObjectToSimulate.Translation.ObjectPosition.zPos += ((speedDirection[2] + 1) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[2];

            // add momentum
            ObjectToSimulate.ObjectPhysicsParams.Momentum[0] += ((speedDirection[0] + 1) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass);
			ObjectToSimulate.ObjectPhysicsParams.Momentum[1] += ((speedDirection[1] + 1) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass) - ObjectToSimulate.ObjectPhysicsParams.GravityMultiplier * 9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass;
			ObjectToSimulate.ObjectPhysicsParams.Momentum[2] += ((speedDirection[2] + 1) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass);

            // update CurrentMovment value
			CurrentMovment.StartPosition = CurrentMovment.EndPosition;
            CurrentMovment.EndPosition = ObjectToSimulate.Translation.ObjectPosition;

            if (CollisionManager.CheckIfCollidedWithObject(ObjectToSimulate.ObjectPhysicsParams.CollidableObjects,
                ObjectToSimulate, ObjectToSimulate.ObjectPhysicsParams.PhysicsMeshStorage)) {

            }
        }

        internal void StartPhysicsSimulator()
        {
            Thread thread = new Thread(() =>
            {
                TimePerSimulationTick = ObjectToSimulate.ObjectPhysicsParams.TimeMultiplier / ObjectToSimulate.ObjectPhysicsParams.TicksPerSecond;
                DelayAmount = (int)double.Ceiling(1000d / TickSpeed);
                while (true)
                {
                    if (StopSignal) break;
                    if (!DoManualTicking)
                    {
                        Tick();
                    }
                    else
                    {
                        if (TickSignal)
                        {
                            Tick();
                            TickSignal = false;
                        }
                    }

                    Task.Delay(DelayAmount).Wait();
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}