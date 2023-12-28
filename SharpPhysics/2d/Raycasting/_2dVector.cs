using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class _2dVector
	{
		public Angle VectorAngle;
		public double Magnitude;

		public _2dVector(Angle vectorAngle)
		{
			this.VectorAngle = vectorAngle;
		}

		public _2dVector(Angle vectorAngle, double magnitude)
		{
			this.VectorAngle = vectorAngle ?? throw new ArgumentNullException(nameof(vectorAngle));
			this.Magnitude = magnitude;
		}
	}
}
