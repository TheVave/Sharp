using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics._2d.Rendering;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics._2d.ObjectRepresentation.Hierarchies
{
	[Serializable]
	public class _2dSceneHierarchy : IAny
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
				SetObjects(value);
			}
		}

		internal void SetObjects(SimulatedObject2d[] value)
		{
			unsafe
			{
				List<HowChanged> changes = [.. Utils.FindChange(trueObjs, value)];
				trueObjs = value;
				List<int> addedObjects = changes
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
					SGLRenderedObject*[] ptrArray = new SGLRenderedObject*[addedObjects.Count];
					SimulatedObject2d[] objs = new SimulatedObject2d[addedObjects.Count];
					SGLRenderedObject LatestObject;
					//foreach (SimulatedObject2d obj in objs)
					for (int i = 0; i < addedObjects.Count; i++)
					{
						LatestObject = RenderingUtils.GetBlankSGLRenderedObjectFromSimulatedObject2d(trueObjs[addedObjects[i]]);
						ptrArray[i] = &LatestObject;
					}
					if (MainRenderer.IsRendering)
						MainRenderer.ARO(ptrArray, HierarchyId);
					else {
						// constructing a proper, memory safe array from all that unsafe array stuff
						SGLRenderedObject[] properArray = new SGLRenderedObject[addedObjects.Count];
						for (int i = 0; i < addedObjects.Count; i++)
							properArray[i] = (*ptrArray[i]);
						MainRendererSGL.renderer.ObjectsToRender = properArray;
					}
				}
				if (changes.Contains(HowChanged.Removed))
				{
					// dealing with removing objects
					if (MainRenderer.IsRendering)
						MainRenderer.RRO(/* removedObjects.ToArray() -> */[.. removedObjects], HierarchyId);
					else
						MainRendererSGL.renderer.ObjectsToRender = RenderingUtils.GetBlankSGLRenderedObjectArrayFromSimulatedObject2dArray(Utils.IndexArrayToTArray([.. removedObjects], trueObjs));
				}
				// we don't care about moved or unchanged objects, so do nothing.
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
