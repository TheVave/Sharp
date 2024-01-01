//#define ENABLE_PAGE_DEBUG
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC;
using System.Numerics;

namespace SharpPhysics.Renderer
{
	public class RenderedObject
	{

		/// <summary>
		/// Stores the compiled shaders
		/// </summary>
		public Shader ObjShader;

		/// <summary>
		/// The simulated object to link shader info to
		/// </summary>
		public _2dSimulatedObject Rendered2dSimulatedObject = new();

		/// <summary>
		/// WARNING: must be 2d
		/// The 2d vertices that are needed to render the object.
		/// </summary>
		public float[] vertices;

		/// <summary>
		/// The color data for all the vertices defined in the vertices object.
		/// </summary>
		public float[] colors;

		/// <summary>
		/// The combined info from the vertices and colors arrays. Style is:
		/// vertices[i], vertices[i + 1], colors[j], colors[j + 1], colors[j + 2]
		/// for one vertex. Eventually stored in video card memory.
		/// </summary>
		internal float[] compiledVertexColorsArray;

		/// <summary>
		/// If you want you're shape to be one color, this is the spot for you.
		/// 
		/// The color for the entire shape. Currently does not work.
		/// </summary>
		public Color colorOverride;

		/// <summary>
		/// The position of the object. In pixels.
		/// </summary>
		public ScreenCoordinate RenderedPosition;

		/// <summary>
		/// All transformation matrices. 
		/// </summary>
		internal Matrix4x4 trans, sca, rot;

		/// <summary>
		/// The display camera.
		/// </summary>
		public Camera2D Camera;

#if ENABLE_PAGE_DEBUG
		private int initReturnDebugger = 0;
		private int initReturnSecondardDebugger = 0;
#endif

		/// <summary>
		/// Loads necessary info to render the object
		/// </summary>
		// loads compiledVertexColorsArray. Currently does not work.
		public void Init()
		{
#if ENABLE_PAGE_DEBUG
			if (colors is null) MessageBoxDisplay.ThrowError("Error, External error, colors was null, RenderedObjectInstance.Init(), please initialize colors before calling Init().", true);
			// could be improved ||
			//                   ||
			//                   \/
			//compiledVertexColorsArray = new float[vertices.Length + colors.Length];
			//for (int i = 0; i < compiledVertexColorsArray.Length; i++)
			//{   
			//	initReturnDebugger = (int)(GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5);
			//	initReturnSecondardDebugger = (int)(GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5);
			//	if (GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) > 2) compiledVertexColorsArray[i] =
			//			vertices[(int)(GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5)];
			//	else compiledVertexColorsArray[i] =
			//			colors[(int)GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5];
			//}
#else
			if (colors is null) MessageBoxDisplay.ThrowError("Error, External error, colors was null, RenderedObjectInstance.Init(), please initialize colors before calling Init().", true);
			// could be improved ||
			//                   ||
			//                   \/
			//compiledVertexColorsArray = new float[vertices.Length + colors.Length];
			//for (int i = 0; i < compiledVertexColorsArray.Length; i++)
			//{
			//	if (GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) > 2) compiledVertexColorsArray[i] =
			//			vertices[(int)(GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5)];
			//	else compiledVertexColorsArray[i] =
			//			colors[(int)GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5];
			//}
			compiledVertexColorsArray = [-0.5f, 0.5f, 1f, 0f, 0f,
										0.5f, 0.5f, 0f, 1f, 0f,
										-0.5f, -0.5f, 0f, 0f, 1f,

										0.5f, 0.5f, 0f, 1f, 0f,
										0.5f, -0.5f, 0f, 1f, 1f,
										-0.5f, -0.5f, 0f, 0f, 1f,];
#endif
		}

		public RenderedObject()
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			Rendered2dSimulatedObject = new _2dSimulatedObject(sqrMesh, new(), new(0,0,0));
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			colorOverride = new Color(ColorName.White);
		}

		public RenderedObject(_2dSimulatedObject objectToRender)
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			Rendered2dSimulatedObject = objectToRender;
		}

		public RenderedObject(_2dSimulatedObject objectToRender, Color colorOverride)
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			Rendered2dSimulatedObject = objectToRender;
			this.colorOverride = colorOverride;
		}

		public RenderedObject(float[] vertices, Color colorOverride)
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			// construct object here
			this.vertices = vertices;
			this.colorOverride = colorOverride;
		}

		public RenderedObject(_2dSimulatedObject objectToRender, float[] vertices, float[] colors)
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			Rendered2dSimulatedObject = objectToRender;
			this.vertices = vertices;
			this.colors = colors;
		}

		public RenderedObject(Shader objShader, _2dSimulatedObject objectToRender, float[] colors)
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			ObjShader = objShader;
			Rendered2dSimulatedObject = objectToRender;
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			this.colors = colors;
		}

		public RenderedObject(Shader objShader, _2dSimulatedObject objectToRender, float[] vertices, float[] colors)
		{
			GLFW.Glfw.Init();
			ObjShader = new Shader();
			ObjShader = objShader;
			Rendered2dSimulatedObject = objectToRender;
			this.vertices = vertices;
			this.colors = colors;
		}
	}
}
