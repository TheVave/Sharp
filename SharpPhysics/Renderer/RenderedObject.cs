using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics.Renderer
{
    public class RenderedObject
	{

		/// <summary>
		/// Stores the compiled shaders
		/// </summary>
		public Shader ObjShader = new Shader();

		/// <summary>
		/// The simulated object to link shader info to
		/// </summary>
		public _2dSimulatedObject ObjectToRender;

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

		/// <summary>
		/// Loads necessary info to render the object
		/// </summary>
		// loads compiledVertexColorsArray.
		public void Init()
		{
			// could be improved ||
			//                   ||
			//                   \/
			compiledVertexColorsArray = new float[vertices.Length + colors.Length];
			for (int i = 0; i < compiledVertexColorsArray.Length; i++)
			{
				if (GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) > 2) compiledVertexColorsArray[i] =
						vertices[(int)GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5];
				else compiledVertexColorsArray[i] =
						colors[(int)GenericMathUtils.GetDifferenceFromNearestMultiple(i, /* the length of vertex info */ 5) + i / 5];
			}
		}

		public RenderedObject()
		{
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			ObjectToRender = new _2dSimulatedObject(sqrMesh, new(), new(0,0,0));
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			colorOverride = new Color(ColorName.White);
		}

		public RenderedObject(_2dSimulatedObject objectToRender)
		{
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			ObjectToRender = objectToRender;
		}

		public RenderedObject(_2dSimulatedObject objectToRender, Color colorOverride)
		{
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			ObjectToRender = objectToRender;
			this.colorOverride = colorOverride;
		}

		public RenderedObject(float[] vertices, Color colorOverride)
		{
			// construct object here
			this.vertices = vertices;
			this.colorOverride = colorOverride;
		}

		public RenderedObject(_2dSimulatedObject objectToRender, float[] vertices, float[] colors)
		{
			ObjectToRender = objectToRender;
			this.vertices = vertices;
			this.colors = colors;
		}

		public RenderedObject(Shader objShader, _2dSimulatedObject objectToRender, float[] colors)
		{
			Mesh sqrMesh = _2dBaseObjects.LoadSquareMesh();
			ObjShader = objShader;
			ObjectToRender = objectToRender;
			vertices = RenderingUtils.MeshToVerticies(sqrMesh);
			this.colors = colors;
		}

		public RenderedObject(Shader objShader, _2dSimulatedObject objectToRender, float[] vertices, float[] colors)
		{
			ObjShader = objShader;
			ObjectToRender = objectToRender;
			this.vertices = vertices;
			this.colors = colors;
		}
	}
}
