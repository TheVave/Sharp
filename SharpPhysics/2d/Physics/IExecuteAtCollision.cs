﻿using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.Physics.CollisionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics._2d.Physics
{
	public interface IExecuteAtCollision
	{
		public virtual void Execute(CollisionData data) { }
		public virtual void Execute() { }
		public virtual void Execute(SimulatedObject2d obj1, SimulatedObject2d obj2) { }
	}
}
