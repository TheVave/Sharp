﻿using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation.Translation
{
	public class Rotation : ISizeGettable, IAny
	{
		public override string ToString()
		{
			return $"Rotation:{{XRot:{XRot}, YRot:{YRot}}}";
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Rotation);
			}
		}

		public Rotation(float x)
		{
			XRot = x;
		}
		public Rotation()
		{
			XRot = 0;
		}
		public float XRot = 0;
		public float YRot = 0;
	}
}
