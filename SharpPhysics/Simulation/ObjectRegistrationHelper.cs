
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class ObjectRegistrationHelper
	{
		public static void Register2dObject(_2dSimulatedObject obj, int sceneId)
		{
			if (!_2dWorld.ids.Contains(sceneId))
			{
				_2dWorld.ids.ToList().Add(sceneId);

			}
		}
	}
}
