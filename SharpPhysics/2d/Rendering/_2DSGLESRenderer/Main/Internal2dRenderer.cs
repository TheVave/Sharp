using SharpPhysics.Renderer._2DSGLESRenderer.Shaders;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Renderer;
using SharpPhysics.UI;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using SharpPhysics.Utilities.MISC;
using Silk.NET.Core;
using Silk.NET.Input;
//using Silk.NET.OpenGL;
using SharpPhysics._2d._2DSGLRenderer;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing;
using StbImageSharp;
using System.Diagnostics;
using System.Numerics;
using SharpPhysics._2d._2DSGLRenderer.Shaders;
using SharpPhysics._2d.Renderer._2DSGLESRenderer.Main;
using SharpPhysics._2d._2DSGLRenderer.Main;
namespace SharpPhysics._2d._2DSGLESRenderer.Main
{
	/// <summary>
	/// Handled by the MainRendererSGLES class.
	/// Please do not interface directly unless you know what you're doing,
	/// though useful if you want to make custom rendering code.
	/// </summary>
	public class Internal2dRendererES
	{
		/// <summary>
		/// vbo data buffer
		/// </summary>
		public float[] vboDataBuf;
		/// <summary>
		/// Current loading ebo
		/// </summary>
		public uint[] curEbo;

		/// <summary>
		/// The time since the program started
		/// </summary>
		Stopwatch PlayTime = new();

		/// <summary>
		/// The imgui renderer
		/// </summary>
		public ImGuiRenderer guiRenderer = new();

		/// <summary>
		/// how many frames it should take to collect FPS
		/// </summary>
		public double collectFPSEveryFrames = 60;

		/// <summary>
		/// The current fps
		/// -1 is not counting FPS
		/// </summary>
		public double curFPS = -1;

		/// <summary>
		/// If the program should get the FPS 
		/// </summary>
		public bool GetFPS = false;

		/// <summary>
		/// Window ref
		/// </summary>
		public IView Wnd;

		/// <summary>
		/// The objects to render, auto-generated from SceneToRenderId
		/// </summary>
		internal SGLESRenderedObject[] ObjectsToRender = [];

		/// <summary>
		/// The scene to render
		/// </summary>
		public short SceneToRenderId
		{
			get
			{
				return InternalSceneToRenderId;
			}
			set
			{
				ObjectsToRender = new SGLESRenderedObject[_2dWorld.SceneHierarchies[value].Objects.Length];
				for (int i = 0; i < ObjectsToRender.Length; i++)
				{
					ObjectsToRender[i].objToSim = _2dWorld.SceneHierarchies[value].Objects[i];
				}
				InternalSceneToRenderId = value;
			}
		}

		/// <summary>
		/// Internal actual rendering scene id
		/// </summary>
		private short InternalSceneToRenderId = 0;

		/// <summary>
		/// wnd title
		/// </summary>
		public string title = "SharpPhysics View Port";

		/// <summary>
		/// The size of the window
		/// </summary>
		public Size wndSize;

		/// <summary>
		/// If the program should print FPS to debug console.
		/// </summary>
		public bool PrintFPS;

		/// <summary>
		/// The window options to create Wnd with
		/// </summary>
		public WindowOptions WndOptions = WindowOptions.Default;

		/// <summary>
		/// GL context
		/// </summary>
		public GL gles;

		/// <summary>
		/// The color to clear to
		/// </summary>
		public Color clearBufferBit = new(ColorName.Black);

		/// <summary>
		/// OnRender
		/// </summary>
		public Action<SimulatedObject2d>[] OR = [];

		/// <summary>
		/// OnUpdate
		/// </summary>
		public Action<SimulatedObject2d>[] OU = [];

		/// <summary>
		/// OnLoad
		/// </summary>
		public Action OL = new Action(() => { });

		/// <summary>
		/// The camera to render out of.
		/// </summary>
		public _2dESCamera Cam = new();

		/// <summary>
		/// The input context.
		/// </summary>
		public IInputContext inputContext;

		/// <summary>
		/// Initializes SGL (Silk.net openGL) and the Wnd object
		/// 
		/// </summary>
		public virtual void ISGLES()
		{
			// window init
			SWCNFG();
			INITWND();
			// wnd events
			WES();
			// calling
			CLWND();
		}

		/// <summary>
		/// Sets the window configuration
		/// </summary>
		public virtual void SWCNFG()
		{
			WndOptions.Title = title;
			WndOptions.Size = new(wndSize.Width, wndSize.Height);
		}

		/// <summary>
		/// Sets window events
		/// </summary>
		public virtual void WES()
		{
			Wnd.Update += UDT;
			Wnd.Render += RNDR;
			Wnd.Load += LD;
			Wnd.Closing += () =>
			{
				Environment.Exit(3);
			};
			Wnd.FramebufferResize += s =>
			{
				gles.Viewport(s);
			};
		}

		/// <summary>
		/// Compiles the shader with the specified name in shaders.resx
		/// </summary>
		/// <param name="name"></param>
		public virtual uint CMPLSHDRN(string name, Silk.NET.OpenGLES.ShaderType type, int objID)
		{
			uint ptr = gles.CreateShader(type);
			gles.ShaderSource(ptr, ShaderCollector.GetShader(name));

			gles.CompileShader(ptr);
			gles.GetShader(ptr, ShaderParameterName.CompileStatus, out int status);
			if (status != /* if it has failed */ 1)
			{
				// 3
				ErrorHandler.ThrowError(3, true);
			}
			if (type is Silk.NET.OpenGLES.ShaderType.VertexShader)
			{
				ObjectsToRender[objID].Program.Vrtx.ShaderCode = ShaderCollector.GetShader(name);
				ObjectsToRender[objID].Program.Vrtx.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Vrtx.ShaderType = type;
			}
			else if (type is Silk.NET.OpenGLES.ShaderType.FragmentShader)
			{
				ObjectsToRender[objID].Program.Frag.ShaderCode = ShaderCollector.GetShader(name);
				ObjectsToRender[objID].Program.Frag.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Frag.ShaderType = type;
			}
			return ptr;
		}
		public virtual uint CMPLSHDRC(string code, Silk.NET.OpenGLES.ShaderType type, int objID)
		{
			uint ptr = gles.CreateShader(type);
			gles.ShaderSource(ptr, code);

			gles.CompileShader(ptr);
			gles.GetShader(ptr, ShaderParameterName.CompileStatus, out int status);
			if (status != /* if it has failed */ 1)
			{
				// 3
				ErrorHandler.ThrowError(3, true);
			}
			if (type is Silk.NET.OpenGLES.ShaderType.VertexShader)
			{
				ObjectsToRender[objID].Program.Vrtx.ShaderCode = code;
				ObjectsToRender[objID].Program.Vrtx.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Vrtx.ShaderType = type;
			}
			else if (type is Silk.NET.OpenGLES.ShaderType.FragmentShader)
			{
				ObjectsToRender[objID].Program.Frag.ShaderCode = code;
				ObjectsToRender[objID].Program.Frag.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Frag.ShaderType = type;
			}
			return ptr;
		}

		/// <summary>
		/// Compiles a shader program with the specified names
		/// </summary>
		/// <param name="name"></param>
		/// <param name="name2"></param>
		/// <returns></returns>
		public virtual uint CMPLPROGN(string name, string name2, int objID)
		{
			uint shdr1 = CMPLSHDRN(name, Silk.NET.OpenGLES.ShaderType.VertexShader, objID);
			uint shdr2 = CMPLSHDRN(name2, Silk.NET.OpenGLES.ShaderType.FragmentShader, objID);

			uint prog;
			prog = gles.CreateProgram();

			gles.AttachShader(prog, shdr1);
			gles.AttachShader(prog, shdr2);

			gles.LinkProgram(prog);

			gles.GetProgram(prog, ProgramPropertyARB.LinkStatus, out int status);
			// 1s good.
			if (status is not 1)
			{
				// 4
				ErrorHandler.ThrowError(4, true);
			}

			gles.DeleteShader(shdr1);
			gles.DeleteShader(shdr2);

			gles.DetachShader(prog, shdr1);
			gles.DetachShader(prog, shdr2);

			ObjectsToRender[objID].Program.ProgramPtr = prog;

			return prog;
		}

		/// <summary>
		/// Creates a shader program with the specified code.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="code2"></param>
		/// <returns></returns>
		public virtual uint CMPLPROGC(string code, string code2, int objID)
		{
			uint shdr1 = CMPLSHDRC(code, Silk.NET.OpenGLES.ShaderType.VertexShader, objID);
			uint shdr2 = CMPLSHDRC(code2, Silk.NET.OpenGLES.ShaderType.FragmentShader, objID);

			uint prog;
			prog = gles.CreateProgram();

			gles.AttachShader(prog, shdr1);
			gles.AttachShader(prog, shdr2);

			gles.LinkProgram(prog);

			gles.GetProgram(prog, ProgramPropertyARB.LinkStatus, out int status);
			// 1s good.
			if (status is not 1)
			{
				//4
				ErrorHandler.ThrowError(4, true);
			}

			gles.DeleteShader(shdr1);
			gles.DeleteShader(shdr2);

			gles.DetachShader(prog, shdr1);
			gles.DetachShader(prog, shdr2);

			ObjectsToRender[objID].Program.ProgramPtr = prog;

			return prog;
		}

		/// <summary>
		/// Sets logo to logo.png
		/// </summary>
		public virtual void STLOGO()
		{
			//RawImage img = RenderingUtils.GetRawImageFromImageResult(ImageResult.FromMemory(File.ReadAllBytes($"{Environment.CurrentDirectory}\\logo.png"), ColorComponents.RedGreenBlueAlpha));
			//Wnd.SetWindowIcon(ref img);
		}

		/// <summary>
		/// Initializes the window
		/// </summary>
		public virtual void INITWND()
		{
			ViewOptions? options = ViewOptions.Default with { API = new GraphicsAPI(ContextAPI.OpenGLES, ContextProfile.Core, ContextFlags.Default, new APIVersion(3, 3)), VSync = false };
			Wnd = Window.GetView(options);
		}

		/// <summary>
		/// Starts the window
		/// </summary>
		public virtual void CLWND()
		{
			Wnd.Run();
		}

		/// <summary>
		/// the amount of passed frames
		/// </summary>
		double frames = 0;

		/// <summary>
		/// Called every frame to render the object(s)
		/// </summary>
		public unsafe virtual void RNDR(double deltaTime)
		{
			CLR();

			for (int i = 0; i < ObjectsToRender.Length; i++)
			{
				SELOBJ(i);
				DRWOBJ(i);
			}

			if (GetFPS)
			{
				COLFPS(deltaTime);
			}

			// imgui
			guiRenderer.ImGuiRndr(deltaTime);
		}

		/// <summary>
		/// Collects FPS
		/// </summary>
		public unsafe virtual void COLFPS(double delta)
		{
			curFPS = frames++ / (PlayTime.ElapsedMilliseconds / 1000);
			if (Math.Floor(frames / collectFPSEveryFrames) == frames / collectFPSEveryFrames)
			{
				frames = 0;
				PlayTime.Restart();
				Debug.WriteLine(curFPS);
			}
		}

		/// <summary>
		/// Invokes user draw code
		/// </summary>
		public unsafe virtual void IVKUSRRNDRS()
		{
			for (int obj = 0; obj < ObjectsToRender.Length; obj++)
			{
				if (OR is not null && OR.Length <= obj)
					OR[obj].Invoke(ObjectsToRender[obj].objToSim);
			}
		}

		/// <summary>
		/// Selects an object to draw
		/// </summary>
		/// <param name="objectID"></param>
		public unsafe virtual void SELOBJ(int objectID)
		{
			// select vao
			gles.BindVertexArray(ObjectsToRender[objectID].BoundVao);
			gles.UseProgram(ObjectsToRender[objectID].Program.ProgramPtr);

			// use texture
			gles.ActiveTexture(TextureUnit.Texture0);
			gles.BindTexture(TextureTarget.Texture2D, ObjectsToRender[objectID].TexturePtr);
		}

		/// <summary>
		/// Draws the object at objectID
		/// </summary>
		/// <param name="objectID"></param>
		public unsafe virtual void DRWOBJ(int objectID)
		{
			STTRNSFRMM4(objectID, "mod");
			gles.DrawArrays(PrimitiveType.Triangles, 0, (uint)ObjectsToRender[objectID].objToSim.ObjectMesh.MeshTriangles.Length * 3);
		}

		/// <summary>
		/// Gets a transform matrix
		/// </summary>
		/// <param name="objectID"></param>
		/// <returns></returns>
		public unsafe virtual Matrix4x4 GTTRNSFRMMTRX(int objectID)
		{
			Vector3 vctr3 = new Vector3((float)ObjectsToRender[objectID].objToSim.Translation.ObjectPosition.X, (float)ObjectsToRender[objectID].objToSim.Translation.ObjectPosition.Y, 0);
			Matrix4x4 model = Matrix4x4.CreateScale(ObjectsToRender[objectID].objToSim.Translation.ObjectScale.xSca,
																ObjectsToRender[objectID].objToSim.Translation.ObjectScale.ySca,
																0f) *
																Matrix4x4.CreateRotationZ((float)GenericMathUtils.DegreesToRadians(ObjectsToRender[objectID].objToSim.Translation.ObjectRotation.xRot)) *
																Matrix4x4.CreateTranslation(vctr3);
			return model;
		}

		/// <summary>
		/// Sets transform matrix
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="name"></param>
		public unsafe virtual void STTRNSFRMM4(int objectID, string name)
		{
			int pos = gles.GetUniformLocation(ObjectsToRender[objectID].Program.ProgramPtr, name);
			gles.UniformMatrix4(pos, false, GetMatrix4x4Values(GTTRNSFRMMTRX(objectID) * GTCMRAMTRX()));
		}

		/// <summary>
		/// Gets the camera transform matrix
		/// </summary>
		/// <returns>
		/// Camera matrix
		/// </returns>
		public unsafe virtual Matrix4x4 GTCMRAMTRX() => Cam.GetProjectionMatrix(this);


		/// <summary>
		/// Initializes some info for objects
		/// </summary>
		public unsafe virtual void INITOBJS()
		{
			int objid = 0;
			foreach (SGLESRenderedObject obj in ObjectsToRender)
			{
				ObjectsToRender[objid].objToSim.ObjectMesh.MeshTriangles = DelaunayTriangulator.DelaunayTriangulation(ObjectsToRender[0].objToSim.ObjectMesh.MeshPoints).ToArray();
				objid++;
			}
		}

		/// <summary>
		/// Update. Called before render.
		/// </summary>
		public virtual void UDT(double deltaTime)
		{
			ParallelFor.ParallelForLoop((int obj) =>
			{
				if (OU is not null && OU.Length > obj)
				{
					OU[obj].Invoke(ObjectsToRender[obj].objToSim);
				}
			}, ObjectsToRender.Length);
		}

		/// <summary>
		/// Initializes the input context
		/// </summary>
		public virtual void INPTINIT() =>
			inputContext = Wnd.CreateInput();

		/// <summary>
		/// Binds the scroll event to scrlwhl
		/// </summary>
		public virtual void BNDSCRL() =>
			inputContext.Mice[0].Scroll += SCRLWHL;

		/// <summary>
		/// Called before anything else (other than OpenGL.Init), only called once.
		/// </summary>
		public virtual void LD()
		{
			// loads some necessary info for the objects.
			INITOBJS();
			// loads input context
			INPTINIT();
			// binds scroll event
			BNDSCRL();
			// inits the OpenGL context
			INTGLCNTXT();
			// sets clear color
			STCLRCOLR(ColorName.Blue);
			// sets texture settings
			TXRHINTS();

			for (int i = 0; i < ObjectsToRender.Length; i++)
			{
				// binds vao
				BNDVAO(i);
				// creates vbo
				INITVBO(i);
				// gets vbo data buffer
				GTVBOBUF(i);
				// gets ebo
				GTEBO(i);
				// sets vbo data
				STVBO(i);
				// sets ebo data
				STEBO(i);
				// compiles shaders and shader progs
				CMPLPROGC(ObjectsToRender[i].VrtxShader, ObjectsToRender[i].FragShader, i);
				// sets the texture supporting attributes
				STSTDATTRIB();
				// sets vbo and vao
				BFRST((uint)i);
				// binds texture info
				TXINIT(i);
				// sets the texture info
				TXST(i);
				// generates mipmaps
				GNMPMPS();
				// sets the texture info to the shader
				STTXTRUNI(i);
			}
			// cleans up a little
			BUFCLNUP();
			// loads user-defined info
			OL.Invoke();
			//initializes fps counter
			FPSCNTRINIT();
			// sets logo
			STLOGO();
			// inits ImGui
			guiRenderer.LD(Wnd, gles);

			// old code:

			// loads some necessary info for the objects.
			//INITOBJS();
			//// inits the OpenGL context
			//INTGLCNTXT();
			//// binds vao
			//BNDVAO();
			//// creates vbo
			//INITVBO();
			//// sets vbo data
			//STVBO();
			//// compiles shaders and shader progs
			//CMPLPROGC(objectToRender[0].VrtxShader, objectToRender[0].FragShader, 0);
			//// sets clear color
			//STCLRCOLR(ColorName.Blue);
			//// sets the texture supporting attributes
			//STSTDATTRIB();
			//// cleans up some stuff mid-way
			//CLNUP();
			//// binds texture info
			//TXINIT();
			//// sets the texture info
			//TXST();
			//// sets texture settings
			//TXRHINTS();
			//// generates mipmaps
			//GNMPMPS();
			//// sets the texture info to the shader
			//STTXTRUNI();
			//// loads user-defined info
			//OL.Invoke();
		}

		public virtual void GTVBOBUF(int objid)
		{
			vboDataBuf = GVFPS(ObjectsToRender[objid].objToSim.ObjectMesh);
		}

		public virtual void BUFCLNUP()
		{
			Array.Clear(vboDataBuf);
			Array.Clear(curEbo);
		}

		public unsafe virtual void STEBO(int objid)
		{
			ObjectsToRender[objid].eboPtr = gles.GenBuffer();
			gles.BindBuffer(BufferTargetARB.ElementArrayBuffer, ObjectsToRender[objid].eboPtr);
			fixed (void* i = &curEbo)
			{
				gles.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(curEbo.Length * sizeof(uint)), i, BufferUsageARB.StaticDraw);
			}
		}

		public virtual void GTEBO(int objid)
		{
			curEbo = RenderingUtils.GetEbo(ref vboDataBuf);
		}

		/// <summary>
		/// Initializes the fps counter
		/// </summary>
		public virtual void FPSCNTRINIT()
		{
			PlayTime.Start();
		}

		/// <summary>
		/// Initializes some info for the textures
		/// </summary>
		public virtual void TXINIT(int objid)
		{
			ObjectsToRender[objid].TexturePtr = gles.GenTexture();
			gles.ActiveTexture(TextureUnit.Texture0);
			gles.BindTexture(TextureTarget.Texture2D, ObjectsToRender[objid].TexturePtr);
		}

		/// <summary>
		/// Sets the texture info
		/// </summary>
		public unsafe virtual void TXST(int objid)
		{
			ImageResult result = new();
			try
			{
				result = ImageResult.FromMemory(File.ReadAllBytes($"{Environment.CurrentDirectory}\\Ctnt\\Txtrs\\{ObjectsToRender[objid].objTextureLoc}"), ColorComponents.RedGreenBlueAlpha);
			}
			catch
			{
				// 2
				ErrorHandler.ThrowError(2, true);
			}
			fixed (byte* ptr = result.Data)
			{
				gles.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)result.Width,
					(uint)result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
			}
		}

		/// <summary>
		/// Called on scroll wheel scroll.
		/// Zooms camera.
		/// </summary>
		/// <param name="mouse"></param>
		/// <param name="scrollwheel"></param>
		public unsafe virtual void SCRLWHL(IMouse mouse, ScrollWheel scrollwheel)
		{
			if (MainRendererSGL.UseCamZoom)
			{
				Cam.Zoom = Math.Clamp((Cam.Zoom - scrollwheel.Y) * (float)MainRendererSGL.ZoomIntensity, 0, 512);
			}
		}

		/// <summary>
		/// Sets some settings for OpenGL and textures.
		/// </summary>
		public unsafe virtual void TXRHINTS()
		{
			gles.BindTexture(TextureTarget.Texture2D, ObjectsToRender[0].TexturePtr);
			gles.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			gles.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			gles.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			gles.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, (int)((!MainRendererSGL.Use8BitStyleTextures) ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
		}

		/// <summary>
		/// Generates all the mipmaps for OpenGL
		/// </summary>
		public virtual void GNMPMPS()
		{
			gles.GenerateMipmap(TextureTarget.Texture2D);
		}

		/// <summary>
		/// Sets the texture info in the shader
		/// </summary>
		public virtual void STTXTRUNI(int objid)
		{
			gles.BindTexture(TextureTarget.Texture2D, (uint)objid);

			int location = gles.GetUniformLocation(ObjectsToRender[objid].Program.ProgramPtr, "uTexture");
			gles.Uniform1(location, 0);
		}

		/// <summary>
		/// Enables blending
		/// </summary>
		public virtual void ENABLBLEND()
		{
			gles.Enable(EnableCap.Blend);
			gles.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		}

		/// <summary>
		/// Selects buffers
		/// </summary>
		public virtual void BFRST(uint objid)
		{
			gles.BindVertexArray(objid);
			gles.BindBuffer(BufferTargetARB.ArrayBuffer, objid);
			gles.BindBuffer(BufferTargetARB.ElementArrayBuffer, objid);
		}

		/// <summary>
		/// Gets a float[] containing the points from a mesh object
		/// </summary>
		public virtual float[] GVFPS(Mesh msh) =>
			Triangle.ToFloats3D(ObjectsToRender[0].objToSim.ObjectMesh.MeshTriangles);

		/// <summary>
		/// Connects the mesh and texture cords
		/// </summary>
		/// <param name="points"></param>
		/// <param name="msh"></param>
		/// <returns></returns>
		public virtual float[] MSHTXCRDS(float[] points, Mesh msh) =>
			RenderingUtils.MashMeshTextureFloats(points, RenderingUtils.GetTXCords(msh));

		/// <summary>
		/// Binds a VAO object
		/// </summary>
		public virtual void BNDVAO(int objid)
		{
			ObjectsToRender[objid].BoundVao = gles.GenVertexArray();
			gles.BindVertexArray(ObjectsToRender[objid].BoundVao);
		}

		/// <summary>
		/// Sets standard attribs for support with textures
		/// </summary>
		public unsafe virtual void STSTDATTRIB()
		{
			const uint stride = (3 * sizeof(float)) + (2 * sizeof(float));

			const uint positionLoc = 0;
			gles.EnableVertexAttribArray(positionLoc);
			gles.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, stride, (void*)0);

			const uint textureLoc = 1;
			gles.EnableVertexAttribArray(textureLoc);
			gles.VertexAttribPointer(textureLoc, 2, VertexAttribPointerType.Float, false, stride, (void*)(3 * sizeof(float)));
		}

		/// <summary>
		/// Gets the shader with the specified name
		/// </summary>
		/// <param name="nm"></param>
		public virtual void GTSHDR(string nm) =>
			ShaderCollector.GetShader(nm);

		/// <summary>
		/// Sets the inside of the vbo to objectToRender[0].triangles
		/// </summary>
		public virtual unsafe void STVBO(int objid)
		{
			float[] data = MSHTXCRDS(vboDataBuf, ObjectsToRender[objid].objToSim.ObjectMesh);
			fixed (float* buf = data)
				gles.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(float) * data.Length), buf, BufferUsageARB.StaticDraw);
		}

		/// <summary>
		/// Creates a vbo
		/// </summary>
		public virtual void INITVBO(int objid)
		{
			ObjectsToRender[objid].vbo = gles.GenBuffer();
			gles.BindBuffer(BufferTargetARB.ArrayBuffer, ObjectsToRender[objid].vbo);
		}

		/// <summary>
		/// Initializes the OpenGL context
		/// </summary>
		public virtual void INTGLCNTXT()
		{
			gles = Wnd.CreateOpenGLES();
		}
		/// <summary>
		/// sets the clear color.
		/// </summary>
		/// <param name="name">
		/// The color to set to.
		/// </param>
		public virtual void STCLRCOLR(ColorName name)
		{
			Color clr = new Color(name);
			gles.ClearColor(clr.R, clr.G, clr.B, clr.A);
			clearBufferBit = clr;
		}

		/// <summary>
		/// Uses the rgba syntax to set the buffer bit
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public virtual void STCLRCOLR(byte r, byte g, byte b, byte a)
		{
			gles.ClearColor(r, g, b, a);
			clearBufferBit = new Color(r, g, b, a);
		}

		/// <summary>
		/// Clears the screen to clearBufferBit
		/// </summary>
		public virtual void CLR()
		{
			gles.Clear(ClearBufferMask.ColorBufferBit);
		}

		/// <summary>
		/// Gets float[] from matrix4x4
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		private float[] GetMatrix4x4Values(Matrix4x4 m) =>
			[
				m.M11,
				m.M12,
				m.M13,
				m.M14,
				m.M21,
				m.M22,
				m.M23,
				m.M24,
				m.M31,
				m.M32,
				m.M33,
				m.M34,
				m.M41,
				m.M42,
				m.M43,
				m.M44
			];

	}
}