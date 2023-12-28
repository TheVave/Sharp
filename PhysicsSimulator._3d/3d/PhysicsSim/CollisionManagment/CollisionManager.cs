
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
    internal static class CollisionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hitables"></param>
        /// <param name="objectToCheck"></param>
        /// <param name="objectsPhysicsMeshStorage"></param>
        /// <returns></returns>
        internal static bool CheckIfCollidedWithObject(SimulatedObject[] hitables, SimulatedObject objectToCheck, PhysicsMeshStorage objectsPhysicsMeshStorage)
        {
            foreach (SimulatedObject objectToCheckIfCollided in hitables)
            {
                // starting var init
                int[] TriangleDistLength;

                // starting design for cheching if a object hit another object


                // check if a object is in a position that the object could actualy hit it (for porformance)

                if (MeshUtilites.CalculateMaxDistFromCenter(objectToCheckIfCollided.ObjectMesh, new Translation.Position()) +
                    MeshUtilites.CalculateMaxDistFromCenter(objectToCheck.ObjectMesh, new Translation.Position()) <
                    MeshUtilites.CalculateDistFromPoint(objectToCheck.Translation.ObjectPosition) -
                    MeshUtilites.CalculateDistFromPoint(objectToCheckIfCollided.Translation.ObjectPosition))
                {
                    // if the max distances that objects could hit each other is less then how far they are apart then the program does not need to compute them. 
                    return false;
                }
                else
                {
                    // used to turn the mesh into a 2d array
                    if (objectsPhysicsMeshStorage.TriEmpty == true) { 
                        //Mesh.ShiftPhysicsTriangles(objectToCheck.ObjectMesh, out objectsPhysicsMeshStorage.PhysicsTriangles);
                        objectsPhysicsMeshStorage.TriEmpty = false;
					}
					if (objectToCheckIfCollided.ObjectPhysicsParams.PhysicsMeshStorage.TriEmpty == true)
					{
						//Mesh.ShiftPhysicsTriangles(objectToCheck.ObjectMesh, out objectToCheckIfCollided.ObjectPhysicsParams.PhysicsMeshStorage.PhysicsTriangles);
						objectToCheckIfCollided.ObjectPhysicsParams.PhysicsMeshStorage.TriEmpty = false;
					}
                    // compute physics triangles
                    if (objectToCheck.ObjectPhysicsParams.StoreComplexValues)
                    {
                        // getting triangle
                        for (int i = 0; i < objectsPhysicsMeshStorage.PhysicsTriangles.GetLength(0); i++)
                        {
                            // finding the opposite adjacent hypotenuse sides
                            return false;
						}
					}
                }
            }
            return false;
        }
    }
}