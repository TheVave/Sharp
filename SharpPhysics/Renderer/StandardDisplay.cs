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

		public RenderedObject[] objectsToRender;

		float rotation = 0;

		public Shader shader;

		public Camera2D cam;

		public StandardDisplay(int initialWindowWidth, int initialWindowHeight, string windowTitle) : base(initialWindowWidth, initialWindowHeight, windowTitle)
		{
		}

		protected override void Draw()
		{
			glClearColor(0, 0, 0, 1);
			glClear(GL_COLOR_BUFFER_BIT);

			for (int i = 0; i < objectsToRender.Length; i++)
			{
				objectsToRender[i].trans = Matrix4x4.CreateTranslation((float)objectsToRender[i].Rendered2dSimulatedObject.Translation.ObjectPosition.xPos, (float)objectsToRender[i].Rendered2dSimulatedObject.Translation.ObjectPosition.yPos, 0);
				objectsToRender[i].sca = Matrix4x4.CreateScale(objectsToRender[i].Rendered2dSimulatedObject.Translation.ObjectScale.xSca * 128, objectsToRender[i].Rendered2dSimulatedObject.Translation.ObjectScale.ySca * 128, 1);
				objectsToRender[i].rot = Matrix4x4.CreateRotationZ(objectsToRender[i].Rendered2dSimulatedObject.Translation.ObjectRotation.xRot);

				objectsToRender[i].ObjShader.SetMatrix4x4("model", objectsToRender[i].sca * objectsToRender[i].rot * objectsToRender[i].trans);
			}

			shader.Use();
			shader.SetMatrix4x4("projection", cam.GetProjectionMatrix());

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
			objectsToRender = [new()];
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
			shader = new Shader(vertexShader, fragmentShader);

			// creating vao and vbo
			vao = glGenVertexArray();
			vbo = glGenBuffer();

			// binding vao and vbo
			glBindVertexArray(vao);
			glBindBuffer(GL_ARRAY_BUFFER, vbo);
			/*
			float[] vertices =
			{
				-0.5f, 0.5f, 1f, 0f, 0f, // top left
                0.5f, 0.5f, 0f, 1f, 0f,// top right
                -0.5f, -0.5f, 0f, 0f, 1f, // bottom left

                0.5f, 0.5f, 0f, 1f, 0f,// top right
                0.5f, -0.5f, 0f, 1f, 1f, // bottom right
                -0.5f, -0.5f, 0f, 0f, 1f, // bottom left
            };
			*/
			objectsToRender[0].vertices = RenderingUtils.MeshToVerticies(_2dBaseObjects.LoadSquareMesh());
			objectsToRender[0].colors = [1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1];

			fixed (float* floatPtr = &objectsToRender[0].vertices[0])
				glBufferData(GL_ARRAY_BUFFER, sizeof(float) * objectsToRender[0].vertices.Length, floatPtr, GL_STATIC_DRAW);
			// vertexes
			glVertexAttribPointer(0, 2, GL_FLOAT, false, 5 * sizeof(float), (void*)0);
			glEnableVertexAttribArray(0);

			// colors
			glVertexAttribPointer(1, 3, GL_FLOAT, false, 5 * sizeof(float), (void*)(2 * sizeof(float)));
			glEnableVertexAttribArray(1);

			glBindBuffer(GL_ARRAY_BUFFER, 0);
			glBindVertexArray(0);

			cam = new Camera2D(Display.DisplayManager.WindowSize / 2, 1);
			
			objectsToRender[0].Init();
		}

		protected override void Update()
		{
		}
	}
}
