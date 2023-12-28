
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	internal static class MeshUtilites
	{
		internal static int CalculateDistFromPoint(Translation.Position position)
		{
			return (int)(Math.Abs(position.xPos) + Math.Abs(position.yPos) + Math.Abs(position.zPos));
		}
		internal static int CalculateMaxDistFromCenter(Mesh mesh, Translation.Position placeholderPositionObject)
		{
			int maxDist = 0;
			for (int i = 0; i < mesh.MeshPointsX.Length; i++)
			{
				placeholderPositionObject.xPos = mesh.MeshPointsX[i];
				placeholderPositionObject.yPos = mesh.MeshPointsY[i];
				placeholderPositionObject.zPos = mesh.MeshPointsZ[i];
				if (CalculateDistFromPoint(placeholderPositionObject) > maxDist) 
					maxDist = CalculateDistFromPoint(placeholderPositionObject);
			}
			return maxDist;
		}
		static internal bool _2dCheckIfPosInObject(Translation.Position pos, Mesh mesh)
		{
			throw new NotImplementedException();
		}
	}
}
