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

namespace SharpPhysics.Renderer.Tests
{
	internal class StandardTest : Game
	{
		uint vao;
		uint vbo;

		Vector2 position = new Vector2(300,300);
		Vector2 scale = new Vector2(150, 150);
		float rotation = 0;
		Matrix4x4 trans;
		Matrix4x4 sca;
		Matrix4x4 rot;

		Shaders.Shader shader;

		Camera2D cam;

		public StandardTest(int initialWindowWidth, int initialWindowHeight, string windowTitle) : base(initialWindowWidth, initialWindowHeight, windowTitle)
		{
		}

		protected override void Draw()
		{
			glClearColor(0, 0, 0, 1);
			glClear(GL_COLOR_BUFFER_BIT);

			rotation = MathF.Sin((float)GameTime.TotalElapsedSeconds) * MathF.PI;

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

			// creating vao and vbo
			vao = glGenVertexArray();
			vbo = glGenBuffer();

			// binding vao and vbo
			glBindVertexArray(vao);
			glBindBuffer(GL_ARRAY_BUFFER, vbo);

			float[] vertices =
			{
				-0.5f, 0.5f, 1f, 0f, 0f, // top left
                0.5f, 0.5f, 0f, 1f, 0f,// top right
                -0.5f, -0.5f, 0f, 0f, 1f, // bottom left

                0.5f, 0.5f, 0f, 1f, 0f,// top right
                0.5f, -0.5f, 0f, 1f, 1f, // bottom right
                -0.5f, -0.5f, 0f, 0f, 1f, // bottom left
            };

			fixed (float* floatPtr = &vertices[0])
				glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, floatPtr, GL_STATIC_DRAW);
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
