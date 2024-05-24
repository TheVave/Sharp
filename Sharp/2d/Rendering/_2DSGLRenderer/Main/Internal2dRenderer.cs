using Sharp._2d._2DSGLRenderer.Shaders;
using Sharp._2d.ObjectRepresentation;
using Sharp._2d.ObjectRepresentation.Hierarchies;
using Sharp.Renderer;
using Sharp.StrangeDataTypes;
using Sharp.UI;
using Sharp.Utilities;
using Sharp.Utilities.MathUtils;
using Sharp.Utilities.MathUtils.DelaunayTriangulator;
using Sharp.Utilities.MISC;
using Silk.NET.Core;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using StbImageSharp;
using System.Diagnostics;
using System.Numerics;

namespace Sharp._2d._2DSGLRenderer.Main
{
	/// <summary>
	/// Handled by the MainRendererSGL class.
	/// Please do not interface directly unless you know what you're doing,
	/// though useful if you want to make custom rendering code.
	/// </summary>
	public class Internal2dRenderer : IAny
	{
		/// <summary>
		/// vbo data buffer
		/// </summary>
		public float[] vboDataBuf;

		/// <summary>
		/// If the program needs to suspend rendering
		/// </summary>
		// Used while objects to render are being updated
		public bool PauseRender = false;

		/// <summary>
		/// Current loading ebo
		/// </summary>
		public uint[] curEbo;

		/// <summary>
		/// if the rendering process should temporarily suspend rendering to load a object 
		/// </summary>
		public int? ExecuteLoading;

		/// <summary>
		/// What object the program should remove in the main loop.
		/// </summary>
		public int? ExecuteRemove;

		/// <summary>
		/// The time since the program started
		/// </summary>
#pragma warning disable IDE0044 // Add readonly modifier
		Stopwatch PlayTime = new();
#pragma warning restore IDE0044 // Add readonly modifier

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
		public IWindow Wnd;

		/// <summary>
		/// The objects to render, auto-generated from SceneToRenderId
		/// </summary>
		internal SGLRenderedObject[] ObjectsToRender = [];

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
				unsafe
				{
					ObjectsToRender = new SGLRenderedObject[_2dWorld.SceneHierarchies[value].Objects.Length];
					SimulatedObject2d obj;
					for (int i = 0; i < ObjectsToRender.Length; i++)
					{
						obj = _2dWorld.SceneHierarchies[value].Objects[i];
						ObjectsToRender[i].objToSim = obj;
					}
					InternalSceneToRenderId = value;
				}
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
		public GL gl;

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
		public Action OL = new(() => { });

		/// <summary>
		/// The camera to render out of.
		/// </summary>
		public _2dCamera Cam = new();

		/// <summary>
		/// The input context.
		/// </summary>
		public IInputContext inputContext;

		/// <summary>
		/// the amount of passed frames
		/// </summary>
		double frames = 0;

		/// <summary>
		/// Initializes SGL (Silk.net openGL) and the Wnd object
		/// 
		/// </summary>
		public virtual void ISGL()
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
		/// Upmost method of deleting objects in the GL instance
		/// </summary>
		/// <param name="objid"></param>
		public virtual void MNDELOBJ()
		{
			if (ExecuteRemove is not null)
			{
				DELOBJ((uint)ExecuteRemove);
			}
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
				gl.Viewport(s);
				//Cam.HandleResize(new Size(s.X, s.Y));
			};
		}

		/// <summary>
		/// Compiles the shader with the specified name in shaders
		/// </summary>
		/// <param name="name"></param>
		public virtual uint CMPLSHDRN(string name, Silk.NET.OpenGL.ShaderType type, int objID)
		{
			uint ptr = gl.CreateShader(type);
			gl.ShaderSource(ptr, ShaderCollector.GetShader(name));

			gl.CompileShader(ptr);
			gl.GetShader(ptr, ShaderParameterName.CompileStatus, out int status);
			if (status != /* if it has failed */ 1)
			{
				// 3
				ErrorHandler.ThrowError(3, true);
			}
			if (type is Silk.NET.OpenGL.ShaderType.VertexShader)
			{
				ObjectsToRender[objID].Program.Vrtx.ShaderCode = ShaderCollector.GetShader(name);
				ObjectsToRender[objID].Program.Vrtx.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Vrtx.ShaderType = type;
			}
			else if (type is Silk.NET.OpenGL.ShaderType.FragmentShader)
			{
				ObjectsToRender[objID].Program.Frag.ShaderCode = ShaderCollector.GetShader(name);
				ObjectsToRender[objID].Program.Frag.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Frag.ShaderType = type;
			}
			return ptr;
		}
		public virtual uint CMPLSHDRC(string code, Silk.NET.OpenGL.ShaderType type, int objID)
		{
			uint ptr = gl.CreateShader(type);
			gl.ShaderSource(ptr, code);

			gl.CompileShader(ptr);
			gl.GetShader(ptr, ShaderParameterName.CompileStatus, out int status);
			if (status != /* if it has failed */ 1)
			{
				// 3
				ErrorHandler.ThrowError(3, true);
			}
			if (type is Silk.NET.OpenGL.ShaderType.VertexShader)
			{
				ObjectsToRender[objID].Program.Vrtx.ShaderCode = code;
				ObjectsToRender[objID].Program.Vrtx.ShaderCompilePtr = ptr;
				ObjectsToRender[objID].Program.Vrtx.ShaderType = type;
			}
			else if (type is Silk.NET.OpenGL.ShaderType.FragmentShader)
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
		// NEW CHANGE: DOESN'T COMPILE UNNECESSARY SHADERS
		public virtual uint CMPLPROGN(string name, string name2, int objID)
		{
			int? otherSames = null;
			uint prog;
			for (int i = 0; i < ShaderCollector.Pairs.Length - 1; i++)
				if (ShaderCollector.Pairs[i] == ShaderCollector.Pairs[^1])
				{
					otherSames = i;
				}

			if (otherSames == null)
			{
				uint shdr1 = CMPLSHDRN(name, Silk.NET.OpenGL.ShaderType.VertexShader, objID);
				uint shdr2 = CMPLSHDRN(name2, Silk.NET.OpenGL.ShaderType.FragmentShader, objID);

				prog = gl.CreateProgram();

				gl.AttachShader(prog, shdr1);
				gl.AttachShader(prog, shdr2);

				gl.LinkProgram(prog);

				gl.GetProgram(prog, ProgramPropertyARB.LinkStatus, out int status);
				// 1s good.
				if (status is not 1)
				{
					// 4
					ErrorHandler.ThrowError(4, true);
				}

				gl.DeleteShader(shdr1);
				gl.DeleteShader(shdr2);

				gl.DetachShader(prog, shdr1);
				gl.DetachShader(prog, shdr2);
			}
			else
			{
				prog = (uint)otherSames;
			}

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
			uint shdr1 = CMPLSHDRC(code, Silk.NET.OpenGL.ShaderType.VertexShader, objID);
			uint shdr2 = CMPLSHDRC(code2, Silk.NET.OpenGL.ShaderType.FragmentShader, objID);

			uint prog;
			prog = gl.CreateProgram();

			gl.AttachShader(prog, shdr1);
			gl.AttachShader(prog, shdr2);

			gl.LinkProgram(prog);

			gl.GetProgram(prog, ProgramPropertyARB.LinkStatus, out int status);
			// 1s good.
			if (status is not 1)
			{
				//4
				ErrorHandler.ThrowError(4, true);
			}

			gl.DeleteShader(shdr1);
			gl.DeleteShader(shdr2);

			gl.DetachShader(prog, shdr1);
			gl.DetachShader(prog, shdr2);

			ObjectsToRender[objID].Program.ProgramPtr = prog;

			return prog;
		}

		/// <summary>
		/// Sets logo to logo.png
		/// </summary>
		public virtual void STLOGO()
		{
			RawImage img = RenderingUtils.GetRawImageFromImageResult(ImageResult.FromMemory(File.ReadAllBytes($"{Environment.CurrentDirectory}\\logo.png"), ColorComponents.RedGreenBlueAlpha));
			Wnd.SetWindowIcon(ref img);
		}

		/// <summary>
		/// Initializes the window
		/// </summary>
		public virtual void INITWND()
		{
			Wnd = Silk.NET.Windowing.Window.Create(WndOptions);
		}

		public virtual void MNINITSNGLOBJ()
		{
			if (ExecuteLoading != null)
			{
				INITSNGLOBJ((int)ExecuteLoading);
			}
		}

		/// <summary>
		/// Starts the window
		/// </summary>
		public virtual void CLWND()
		{
			Wnd.Run();
		}

		/// <summary>
		/// Called every frame to render the object(s)
		/// </summary>
		public unsafe virtual void RNDR(double deltaTime)
		{
			if (!Utils.RenderingStarted)
				Utils.RenderingStarted = true;
			CLR();

			MNINITSNGLOBJ();
			MNDELOBJ();



			for (int i = 0; i < ObjectsToRender.Length; i++)
			{
				if (PauseRender)
				{
					AWTUNPAWSRNDR();
				}

				try
				{
					if (ObjectsToRender[i].ObjectInitialized)
					{
						SELOBJ(i);
						DRWOBJ(i);
					}
				}
				catch
				{
					// something weird happened.
					// Error, Internal/External Error, unknown cause.
					ErrorHandler.ThrowError(5, true);
				}
			}

			if (GetFPS)
			{
				COLFPS(deltaTime);
			}

			// imgui
			guiRenderer.ImGuiRndr(deltaTime);
		}

		/// <summary>
		/// Awaits rendering unpausing
		/// </summary>
		public virtual void AWTUNPAWSRNDR()
		{
			while (PauseRender)
			{
				// a little less than a frame
				Task.Delay(3);
				if (ExecuteLoading != null)
				{
					INITSNGLOBJ((int)ExecuteLoading);
				}
			}
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
			gl.BindVertexArray(ObjectsToRender[objectID].BoundVao);
			gl.UseProgram(ObjectsToRender[objectID].Program.ProgramPtr);

			// use texture
			gl.ActiveTexture(TextureUnit.Texture0);
			gl.BindTexture(TextureTarget.Texture2D, ObjectsToRender[objectID].TexturePtr);
		}

		/// <summary>
		/// Draws the object at objectID
		/// </summary>
		/// <param name="objectID"></param>
		public unsafe virtual void DRWOBJ(int objectID)
		{
			STTRNSFRMM4(objectID, "mod");
			gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)ObjectsToRender[objectID].objToSim.ObjectMesh.MeshTriangles.Length * 3);
		}

		/// <summary>
		/// Gets a transform matrix
		/// </summary>
		/// <param name="objectID"></param>
		/// <returns></returns>
		public unsafe virtual Matrix4x4 GTTRNSFRMMTRX(int objectID)
		{
			Vector3 vctr3 = new((float)ObjectsToRender[objectID].objToSim.Translation.ObjectPosition.X, (float)ObjectsToRender[objectID].objToSim.Translation.ObjectPosition.Y, 0);
			Matrix4x4 model = Matrix4x4.CreateScale(ObjectsToRender[objectID].objToSim.Translation.ObjectScale.XSca,
																ObjectsToRender[objectID].objToSim.Translation.ObjectScale.YSca,
																0f) *
																Matrix4x4.CreateRotationZ((float)GenericMathUtils.DegreesToRadians(ObjectsToRender[objectID].objToSim.Translation.ObjectRotation.XRot)) *
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
			int pos = gl.GetUniformLocation(ObjectsToRender[objectID].Program.ProgramPtr, name);
			gl.UniformMatrix4(pos, false, GetMatrix4x4Values(GTTRNSFRMMTRX(objectID) * GTCMRAMTRX()));
		}

		/// <summary>
		/// Gets the camera transform matrix
		/// </summary>
		/// <returns>
		/// Camera matrix
		/// </returns>
		public unsafe virtual Matrix4x4 GTCMRAMTRX() => Cam.GetProjectionMatrix(this);


		/// <summary>
		/// Initializes some (necessary) info for objects
		/// </summary>
		public unsafe virtual void INITOBJMSH()
		{
			for (int i = 0; i < ObjectsToRender.Length; i++)
			{
				ObjectsToRender[i].objToSim.ObjectMesh.MeshTriangles = [.. DelaunayTriangulator.DelaunayTriangulation(ObjectsToRender[i].objToSim.ObjectMesh.MeshPoints)];
			}
		}

		/// <summary>
		/// Actually loads the objects to render
		/// </summary>
		public unsafe virtual void STOBJSTORENDER()
		{
			int objid = 0;
			_2dSceneHierarchy hierarchy = _2dWorld.SceneHierarchies[SceneToRenderId];
			fixed (SimulatedObject2d* obj0ptr = &(hierarchy.Objects[0]))
			{
				SimulatedObject2d[]* objs = (SimulatedObject2d[]*)obj0ptr;
				ObjectsToRender = new SGLRenderedObject[objs->Length];
				for (int i = 0; i < objs->Length; i++)
				{
					ObjectsToRender[i] = new SGLRenderedObject();
					ObjectsToRender[i].objToSim = (*objs)[i];
				}
			}
		}

		public unsafe virtual void ADDOBJSTRNDR(SGLRenderedObject*[] objs)
		{
			new Thread(() =>
			{
				PauseRender = true;
				SGLRenderedObject[] rndrBfr = new SGLRenderedObject[ObjectsToRender.Length + objs.Length];
				fixed (SGLRenderedObject[]* ptr = &ObjectsToRender)
				{
					try
					{
						ObjectsToRender.CopyTo(rndrBfr, 0);
						objs.CopyTo(rndrBfr, ObjectsToRender.Length - 1);
					}
					catch
					{
						objs.CopyTo(rndrBfr, 0);
					}
					// renderbfr has all the objects

					ObjectsToRender = rndrBfr;

					// now to initalize them


					PauseRender = false;
					for (int i = ptr->Length - 1; i < ObjectsToRender.Length; i++)
					{
						SGLRenderedObject obj = *objs[i];
						ExecuteLoading = i;
						// a frame
						Task.Delay(16);
					}
					ExecuteLoading = null;
				}
			})
			{
				Name = "Graphics Helper"
			}.Start();
		}

		/// <summary>
		/// Update. Called before render.
		/// </summary>
		public unsafe virtual void UDT(double deltaTime)
		{
			Utils.ParallelForLoop((int obj) =>
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
			//INITOBJMSH();
			// /\
			// ||
			// possibly not necessary
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
			// loads camera info
			Cam.originalWindowSize = wndSize;

			for (int i = 0; i < ObjectsToRender.Length; i++)
			{
				INITSNGLOBJ(i);
				ObjectsToRender[i].HashCode = ObjectsToRender[i].GetHashCode();
				ObjectsToRender[i].ObjectInitialized = true;
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
			guiRenderer.LD(Wnd, gl);
			MainRendererSGL.IsRendering = true;
		}

		/// <summary>
		/// Initializes an individual object
		/// </summary>
		/// <param name="i"></param>
		public unsafe virtual void INITSNGLOBJ(int i)
		{
			// binds vao
			SELVAO(i);
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

			ExecuteLoading = null;
		}

		public unsafe virtual void GTVBOBUF(int objid)
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
			ObjectsToRender[objid].eboPtr = gl.GenBuffer();
			gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ObjectsToRender[objid].eboPtr);
			fixed (void* i = &curEbo)
			{
				gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(curEbo.Length * sizeof(uint)), i, BufferUsageARB.StaticDraw);
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
			ObjectsToRender[objid].TexturePtr = gl.GenTexture();
			gl.ActiveTexture(TextureUnit.Texture0);
			gl.BindTexture(TextureTarget.Texture2D, ObjectsToRender[objid].TexturePtr);
		}

		/// <summary>
		/// Sets the texture info
		/// </summary>
		public unsafe virtual void TXST(int objid)
		{
			ImageResult result = new();
			try
			{
				result = ImageResult.FromMemory(File.ReadAllBytes($"{Environment.CurrentDirectory}\\Ctnt\\Txtrs\\{ObjectsToRender[objid].ObjectTextureLocation}"), ColorComponents.RedGreenBlueAlpha);
			}
			catch
			{
				// 2
				ErrorHandler.ThrowError(2, true);
			}
			fixed (byte* ptr = result.Data)
			{
				gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)result.Width,
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
			gl.BindTexture(TextureTarget.Texture2D, ObjectsToRender[0].TexturePtr);
			gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, (int)((!MainRendererSGL.Use8BitStyleTextures) ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
		}

		/// <summary>
		/// Generates all the mipmaps for OpenGL
		/// </summary>
		public virtual void GNMPMPS()
		{
			gl.GenerateMipmap(TextureTarget.Texture2D);
		}

		/// <summary>
		/// Sets the texture info in the shader
		/// </summary>
		public virtual void STTXTRUNI(int objid)
		{
			gl.BindTexture(TextureTarget.Texture2D, (uint)objid);

			int location = gl.GetUniformLocation(ObjectsToRender[objid].Program.ProgramPtr, "uTexture");
			gl.Uniform1(location, 0);
		}

		/// <summary>
		/// Enables blending
		/// </summary>
		public virtual void ENABLBLEND()
		{
			gl.Enable(EnableCap.Blend);
			gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		}

		/// <summary>
		/// Selects buffers
		/// </summary>
		public virtual void BFRST(uint objid)
		{
			gl.BindVertexArray(objid);
			gl.BindBuffer(BufferTargetARB.ArrayBuffer, objid);
			gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, objid);
		}

		/// <summary>
		/// Gets a float[] containing the points from a mesh object
		/// </summary>
		public unsafe virtual float[] GVFPS(Mesh msh) =>
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
		public virtual void MNGVAO(int objid)
		{
			CRTVAO(objid);
			SELVAO(objid);
		}

		/// <summary>
		/// creates a vao
		/// </summary>
		/// <param name="objid"></param>
		public virtual void CRTVAO(int objid) =>
			ObjectsToRender[objid].BoundVao = gl.GenVertexArray();

		/// <summary>
		/// binds a vao
		/// </summary>
		/// <param name="objid"></param>
		public virtual void SELVAO(int objid) =>
			gl.BindVertexArray(ObjectsToRender[objid].BoundVao);

		/// <summary>
		/// Sets standard attribs for support with textures
		/// </summary>
		public unsafe virtual void STSTDATTRIB()
		{
			const uint stride = (3 * sizeof(float)) + (2 * sizeof(float));

			const uint positionLoc = 0;
			gl.EnableVertexAttribArray(positionLoc);
			gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, stride, (void*)0);

			const uint textureLoc = 1;
			gl.EnableVertexAttribArray(textureLoc);
			gl.VertexAttribPointer(textureLoc, 2, VertexAttribPointerType.Float, false, stride, (void*)(3 * sizeof(float)));
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
				gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(float) * data.Length), buf, BufferUsageARB.StaticDraw);
		}

		/// <summary>
		/// Creates & binds a vbo
		/// </summary>
		public virtual void INITVBO(int objid)
		{
			ObjectsToRender[objid].vbo = gl.GenBuffer();
			gl.BindBuffer(BufferTargetARB.ArrayBuffer, ObjectsToRender[objid].vbo);
		}

		/// <summary>
		/// Initializes the OpenGL context
		/// </summary>
		public virtual void INTGLCNTXT()
		{
			gl = Wnd.CreateOpenGL();
		}
		/// <summary>
		/// sets the clear color.
		/// </summary>
		/// <param name="name">
		/// The color to set to.
		/// </param>
		public virtual void STCLRCOLR(ColorName name)
		{
			Color clr = new(name);
			gl.ClearColor(clr.R, clr.G, clr.B, clr.A);
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
			gl.ClearColor(r, g, b, a);
			clearBufferBit = new Color(r, g, b, a);
		}

		/// <summary>
		/// Clears the screen to clearBufferBit
		/// </summary>
		public virtual void CLR()
		{
			gl.Clear(ClearBufferMask.ColorBufferBit);
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


		/// <summary>
		/// Deallocates a objects VRAM and makes it not rendered,
		/// and then have the garbage collector clean the rest up.
		/// NOT TESTED! 
		/// MAY FAIL!
		/// </summary>
		/// <param name="objid"></param>
		// DEV NOTE: I WIL CUM BAK 2 THIS
		public virtual void DELOBJ(uint objid)
		{
			// remove vbo (vertex data)
			DLVRTXARR(ObjectsToRender[objid].vbo);

			// remove ebo
			DLBFR(ObjectsToRender[objid].eboPtr);

			// remove texture
			DLTXTR(ObjectsToRender[objid].TexturePtr);

			// object is removed from GL. This class is not for handling SP, so removing the object from SP will be done later, but we need to signal it's gone.
			ExecuteRemove = null;
		}

		public virtual void DLBFR(uint bfr) =>
			gl.DeleteBuffer(bfr);
		public virtual void DLVRTXARR(uint idx) =>
			gl.DeleteVertexArray(idx);
		public virtual void DLTXTR(uint txtrIdx) =>
			gl.DeleteTexture(txtrIdx);

		/// <summary>
		/// Changes an object's texture.
		/// NOT TESTED! MAY FAIL!
		/// </summary>
		/// <param name="objid"></param>
		public virtual void CHNGTXTR(int objid, string newTexturePath)
		{
			// del texture
			DLTXTR(ObjectsToRender[objid].TexturePtr);
			// recreate (new) texture
			// create & bind texture
			TXINIT(objid);
			// set texture info
			TXST(objid);
			// send info to the shader uniform.
			STTXTRUNI(objid);
		}

		//public virtual void CHNGMSH(int objid, Mesh toChangeTo)
	}
}