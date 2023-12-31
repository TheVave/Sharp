using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;
using GLFW;
using SharpPhysics.Utilities.MISC;
using System.Numerics;

namespace SharpPhysics.Renderer
{
    public class Shader
    {
        public string VertexCode;
        public string FragmentCode;

        public uint ProgramID;

        public Shader(string vertexCode, string fragmentCode)
        {
            VertexCode = vertexCode;
            FragmentCode = fragmentCode;
            Load();
        }
		public Shader()
		{
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
			VertexCode = vertexShader;
			FragmentCode = fragmentShader;
			Load();
		}


        public virtual void Load()
        {
            uint vs = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vs, VertexCode);
            glCompileShader(vs);

            uint fs = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fs, FragmentCode);
            glCompileShader(fs);

            int[] vsstatus = glGetShaderiv(vs, GL_COMPILE_STATUS, 1);
            int[] fsstatus = glGetShaderiv(fs, GL_COMPILE_STATUS, 1);

            if (vsstatus[0] == 0 || fsstatus[0] == 0)
            {
                ErrorHandler.ThrowError("External/internal error, Shader compilation failed.\n" +
                    " You may have supplied incorrect data for the fragment/vertex shader code. " +
                    "\nThe shaders are programmed in HLSL.", false);
                ErrorHandler.ThrowError("Complete error (fs): " + glGetShaderInfoLog(fs), false);
                ErrorHandler.ThrowError("Complete error (vs): " + glGetShaderInfoLog(vs), true);
            }

            ProgramID = glCreateProgram();
            glAttachShader(ProgramID, vs);
            glAttachShader(ProgramID, fs);

            glLinkProgram(ProgramID);

            // detaching unneeded opengl mem
            glDetachShader(ProgramID, vs);
            glDetachShader(ProgramID, fs);
            glDeleteShader(vs);
            glDeleteShader(fs);
        }

        public void Use()
        {
            glUseProgram(ProgramID);
        }

        public virtual void SetMatrix4x4(string uniformName, Matrix4x4 val)
        {
            int Uniformlocation = glGetUniformLocation(ProgramID, uniformName);
            glUniformMatrix4fv(Uniformlocation, 1, false, GetMatrix4x4Values(val));
        }

        private float[] GetMatrix4x4Values(Matrix4x4 m)
        {
            return new float[]
            {
        m.M11, m.M12, m.M13, m.M14,
        m.M21, m.M22, m.M23, m.M24,
        m.M31, m.M32, m.M33, m.M34,
        m.M41, m.M42, m.M43, m.M44
            };
        }
    }
}
