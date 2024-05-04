﻿using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
{
	public class ShaderProgram : ISizeGettable, IAny
	{
		public Shader Vrtx;
		public Shader Frag;
		public uint ProgramPtr;

		public ShaderProgram()
		{
			Vrtx = new Shader();
			Frag = new Shader();
		}

		public int GetSize() =>
			// simple objects
			sizeof(uint)
			// complex objects
			+ Vrtx.GetSize()
			+ Frag.GetSize();
	}
}
