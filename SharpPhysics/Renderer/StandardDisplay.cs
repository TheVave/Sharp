using GLFW;
using static OpenGL.GL;
using System.Numerics;
using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics.Renderer.Tests
{
	internal class StandardDisplay : Game
	{
		uint vao;
		uint vbo;

		public RenderedObject objectToRender;

		public Camera2D cam;

		public StandardDisplay(int initialWindowWidth, int initialWindowHeight, string windowTitle) : base(initialWindowWidth, initialWindowHeight, windowTitle)
		{
		}

		protected override void Draw()
		{
			glClearColor(0, 0, 0, 1);
			glClear(GL_COLOR_BUFFER_BIT);

			objectToRender.Rendered2dSimulatedObject.Translation.ObjectRotation.xRot = MathF.Sin((float)GameTime.TotalElapsedSeconds) * MathF.PI;

			objectToRender.trans = Matrix4x4.CreateTranslation((float)objectToRender.Rendered2dSimulatedObject.Translation.ObjectPosition.xPos, (float)objectToRender.Rendered2dSimulatedObject.Translation.ObjectPosition.yPos, 0);
			objectToRender.sca = Matrix4x4.CreateScale(objectToRender.Rendered2dSimulatedObject.Translation.ObjectScale.xSca * 64, objectToRender.Rendered2dSimulatedObject.Translation.ObjectScale.ySca * 64, 1);
			objectToRender.rot = Matrix4x4.CreateRotationZ(objectToRender.Rendered2dSimulatedObject.Translation.ObjectRotation.xRot);

			objectToRender.ObjShader.SetMatrix4x4("model", objectToRender.sca * objectToRender.rot * objectToRender.trans);

			objectToRender.ObjShader.Use();
			objectToRender.ObjShader.SetMatrix4x4("projection", cam.GetProjectionMatrix());

			glBindVertexArray(vao);
			glDrawArrays(GL_TRIANGLES, 0, 6);

			Glfw.SwapBuffers(DisplayManager.Window);
		}

		protected override void Init()
		{
		}

		protected unsafe override void LoadContent()
		{
			Glfw.Init();
			objectToRender = new();
			// shaders
			string vertexShader = @"#version 330 core
                                    layout (location = 0) in vec2 aPosition;
                                    layout (location = 1) in vec3 aColor;
                                    out vec4 vertexColor;
									
									uniform mat4 model;
									uniform mat4 projection;
                                    void main() 
                                    {
                                        vertexColor = vec4(aColor.rgb, 1.0);
                                        gl_Position = projection * model * vec4(aPosition.xy, 0, 1.0);
                                    }";

			string fragmentShader = @"#version 330 core
                                    out vec4 FragColor;
                                    in vec4 vertexColor;

                                    void main() 
                                    {
                                        FragColor = vertexColor;
                                    }";
			objectToRender.ObjShader = new Shader(vertexShader, fragmentShader);


			// creating vao and vbo
			vao = glGenVertexArray();
			vbo = glGenBuffer();

			// binding vao and vbo
			glBindVertexArray(vao);
			glBindBuffer(GL_ARRAY_BUFFER, vbo);

			objectToRender.vertices = RenderingUtils.MeshToVertices(_2dBaseObjects.LoadSquareMesh());
			objectToRender.colors = new float[(objectToRender.vertices.Length / 2) * 3];
			Random rand = new Random();
			for (int i = 0; i < objectToRender.colors.Length; i++) objectToRender.colors[i] = (float)rand.NextDouble();
			objectToRender.Init();

			fixed (float* floatPtr = &objectToRender.compiledVertexColorsArray[0])
				glBufferData(GL_ARRAY_BUFFER, sizeof(float) * objectToRender.vertices.Length, floatPtr, GL_STATIC_DRAW);
			// vertexes
			glVertexAttribPointer(0, 2, GL_FLOAT, false, 5 * sizeof(float), (void*)0);
			glEnableVertexAttribArray(0);

			// colors
			glVertexAttribPointer(1, 3, GL_FLOAT, false, 5 * sizeof(float), (void*)(2 * sizeof(float)));
			glEnableVertexAttribArray(1);

			glBindBuffer(GL_ARRAY_BUFFER, 0);
			glBindVertexArray(0);

			cam = new Camera2D(Display.DisplayManager.WindowSize / 2, 1);
		}

		protected override void Update()
		{
		}
	}
}
