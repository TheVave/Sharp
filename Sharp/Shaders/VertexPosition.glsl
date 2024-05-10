#version 330 core
layout (location = 0) in vec2 aPosition;
out vec4 vertexColor;

// has support for transformation
						
uniform mat4 model;
uniform mat4 projection;
void main() 
{
    gl_Position = projection * vec4(aPosition.xy, 0, 1.0);
}