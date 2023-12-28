using SharpPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
    internal static class _2dCollisionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hitables"></param>
        /// <param name="objectToCheck"></param>
        /// <param name="objectsPhysicsMeshStorage"></param>
        /// <returns></returns>
        internal static bool CheckIfCollidedWithObject(_2dSimulatedObject[] hitables, _2dSimulatedObject objectToCheck)
        {
            foreach (SimulatedObject objectToCheckIfCollided in hitables)
            {
                return false;
            }
        }
    }
}