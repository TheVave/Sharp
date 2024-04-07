using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics._2d.Rendering;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics._2d.ObjectRepresentation.Hierarchies
{
	[Serializable]
	public class _2dSceneHierarchy
	{
		/// <summary>
		/// The objects in the scene.
		/// If you change this it will (somewhat slowly) set up/remove the objects to render.
		/// </summary>
		// Wrapper for trueObjs
		public SimulatedObject2d[] Objects
		{
			get
			{
				return trueObjs;
			}
			set
			{
				//if (Utils.RenderingStarted)
				//{
				unsafe
				{
					List<HowChanged> changes = Utils.FindChange(trueObjs, value).ToList();
					trueObjs = value;
					List<int> addedObjs = changes
											.Select((change, index) => new { change, index })
											.Where(item => item.change == HowChanged.Added)
											.Select(item => item.index)
											.ToList();
					List<int> removedObjects = changes
											.Select((change, index) => new { change, index })
											.Where(item => item.change == HowChanged.Removed)
											.Select(item => item.index)
											.ToList();
					// if there's at least one added object then do the code for adding objects
					if (changes.Contains(HowChanged.Added))
					{
						// dealing with added objects
						// this is not great code, and I may change it to not allow SGLRenderedObjects to end up in multiple random(ish) places.
						SGLRenderedObject*[] ptrArray = new SGLRenderedObject*[addedObjs.Count];
						SimulatedObject2d[] objs = new SimulatedObject2d[addedObjs.Count];
						SGLRenderedObject LatestObject;
						//foreach (SimulatedObject2d obj in objs)
						for (int i = 0; i < addedObjs.Count; i++)
						{
							LatestObject = RenderingUtils.GetBlankSGLRenderedObjectFromSimulatedObject2d(trueObjs[addedObjs[i]]);
							ptrArray[i] = &LatestObject;
						}
						MainRenderer.ARO(ptrArray, HierarchyId);
					}
					if (changes.Contains(HowChanged.Removed))
					{
						// dealing with removing objects
						MainRenderer.RRO(/* removedObjects.ToArray() -> */[.. removedObjects], HierarchyId);
					}
					// we don't care about moved or unchanged objects, so do nothing.
				}
				//}
				//else
				//{
				//trueObjs = value;
				//}
			}
		}

		/// <summary>
		/// the objects that are rendered from the scene. </br>
		/// if the current rendering scene is not this one, it will return a blank array.
		/// </summary>
		public SGLRenderedObject[] RenderedObjects
		{
			get => (MainRendererSGL.SceneIDToRender == HierarchyId) ? MainRendererSGL.renderer.ObjectsToRender : [];
			set => MainRendererSGL.ObjectsToRender = value;
		}

		internal SimulatedObject2d[] trueObjs = [];
		public byte HierarchyId { get; private set; } = 0;

		public _2dSceneHierarchy()
		{
		}

		public _2dSceneHierarchy(SimulatedObject2d[] objects, byte hierarchyId)
		{
			Objects = objects;
			HierarchyId = hierarchyId;
		}

		public static void RegisterObject(byte hierarchyId, SimulatedObject2d obj)
		{
			_2dWorld.SceneHierarchies[hierarchyId].Objects = [.. _2dWorld.SceneHierarchies[hierarchyId].Objects, obj];
		}
	}
}
