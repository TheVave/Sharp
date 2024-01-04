using SharpPhysics.Renderer.GameLoop;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;
using SharpPhysics.Renderer.Cameras;
using System.Numerics;
using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics.Renderer.Tests
{
	internal class StandardDisplay : Game
	{
		uint vao;
		uint vbo;

		RenderedObject objectToRender;

		Vector2 position = new Vector2(200, 200);
		Vector2 scale = new Vector2(150, 150);
		float rotation = 0;
		Matrix4x4 trans;
		Matrix4x4 sca;
		Matrix4x4 rot;

		Shaders.Shader shader;

		Camera2D cam;

		public StandardDisplay(int initialWindowWidth, int initialWindowHeight, string windowTitle) : base(initialWindowWidth, initialWindowHeight, windowTitle)
		{
		}

		protected override void Draw()
		{
			glClearColor(0, 0, 0, 1);
			glClear(GL_COLOR_BUFFER_BIT);


			trans = Matrix4x4.CreateTranslation(position.X, position.Y, 0);
			sca = Matrix4x4.CreateScale(scale.X, scale.Y, 1);
			rot = Matrix4x4.CreateRotationZ(rotation);

			shader.SetMatrix4x4("model", sca * rot * trans);

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
			shader = new Shaders.Shader(vertexShader, fragmentShader);
			vao = glGenVertexArray();

			// creating vao and vbo
			vbo = glGenBuffer();
			glBindBuffer(GL_ARRAY_BUFFER, vbo);

			// binding vao and vbo
			glBindVertexArray(vao);

			objectToRender.vertices = RenderingUtils.MeshToVertices(_2dBaseObjects.LoadSquareMesh());
			objectToRender.colors = new float[(objectToRender.vertices.Length / 2) * 3];
			Random rand = new Random();
			for (int j = 0; j < objectToRender.colors.Length; j++) objectToRender.colors[j] = (float)rand.NextDouble();
			objectToRender.Init();

			fixed (float* floatPtr = &objectToRender.compiledVertexColorsArray[0])
				glBufferData(GL_ARRAY_BUFFER, sizeof(float) * objectToRender.compiledVertexColorsArray.Length, floatPtr, GL_STATIC_DRAW);
			// vertexes
			glVertexAttribPointer(0, 2, GL_FLOAT, false, 5 * sizeof(float), (void*)0);
			glEnableVertexAttribArray(0);

			// colors
			glVertexAttribPointer(1, 3, GL_FLOAT, false, 5 * sizeof(float), (void*)(2 * sizeof(float)));
			glEnableVertexAttribArray(1);

			glBindBuffer(GL_ARRAY_BUFFER, 0);
			glBindVertexArray(0);

			cam = new Camera2D(DisplayManager.WindowSize / 2, 1);
		}

		protected override void Update()
		{
		}
	}
}
