using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Utilities.MISC
{
    public static class RenderingUtils
    {
        public static float[] MeshToVerticies(Mesh mesh)
        {
            float[] vertices = new float[mesh.MeshPointsX.Length * 2];
            for (int i = 0; i < mesh.MeshPointsX.Length; i++)
            {
                if (GenericMathUtils.IsOdd(i)) /* this is the x position */ vertices[i] = (float)mesh.MeshPointsX[i / 2];
                else /* this is the y position */ vertices[i] = (float)mesh.MeshPointsY[i / 2];
            }
            return vertices;
        }
    }
}
