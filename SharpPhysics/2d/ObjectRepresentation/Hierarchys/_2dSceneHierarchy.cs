﻿
using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics._2d.ObjectRepresentation.Hierarchies
{
	public class _2dSceneHierarchy
	{
        public _2dSimulatedObject[] Objects = [];
        public byte HierarchyId = 0;
		public static void RegisterObject(byte hierarchyId)
		{
			ErrorHandler.ThrowNotImplementedExcepetion();
		}
	}
}
