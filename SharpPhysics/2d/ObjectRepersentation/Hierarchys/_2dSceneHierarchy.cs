using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public class _2dSceneHierarchy
	{
		public _2dSimulatedObject[] Objects = new _2dSimulatedObject[0];
		public byte HierarchyId = 0;
		public static void RegisterObject(byte hierarchyId)
		{
			ErrorHandler.ThrowNotImplementedExcepetion();
		}
	}
}
