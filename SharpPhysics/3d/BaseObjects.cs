using System;
using SharpPhysics;

namespace SharpPhysics
{
    public static class BaseObjects
    {
        public static SimulatedObject MakeCube(Translation translation)
        {
            SimulatedObject toAddValuesTo = new SimulatedObject();
            toAddValuesTo.ObjectMesh = new Mesh(new double[] { 2, 2, -2, -2, 2, 2, -2, -2 }, new double[] { -2, -2, -2, -2, 2, 2, 2, 2 }, new double[] { 2, 2, 2, 2, -2, -2, -2, -2 });
            toAddValuesTo.Translation = translation;
            toAddValuesTo.ObjectPhysicsParams = new PhysicsParams();
            toAddValuesTo.ObjectPhysicsParams.Mass = 1;
            toAddValuesTo.ObjectPhysicsParams.RotDirection = new float[3];
            return toAddValuesTo;
        }
        public static SimulatedObject MakeCube()
        {
            SimulatedObject toAddValuesTo = new SimulatedObject();
            toAddValuesTo.ObjectMesh = new Mesh(new double[] { 2, 2, -2, -2, 2, 2, -2, -2 }, new double[] { -2, -2, -2, -2, 2, 2, 2, 2 }, new double[] { 2, 2, 2, 2, -2, -2, -2, -2 });
            toAddValuesTo.Translation.ObjectPosition.xPos = 0;
            toAddValuesTo.Translation.ObjectPosition.yPos = 0;
            toAddValuesTo.Translation.ObjectPosition.zPos = 0;
            toAddValuesTo.Translation.ObjectRotation.xRot = 0;
            toAddValuesTo.Translation.ObjectRotation.yRot = 0;
            toAddValuesTo.Translation.ObjectRotation.zRot = 0;
            toAddValuesTo.Translation.ObjectScale.xSca = 1;
            toAddValuesTo.Translation.ObjectScale.ySca = 1;
            toAddValuesTo.Translation.ObjectScale.zSca = 1;
            toAddValuesTo.ObjectPhysicsParams = new PhysicsParams();
            return toAddValuesTo;
        }
    }
}
