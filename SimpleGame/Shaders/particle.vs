#version 330

in vec3 a_Position;
in float a_Radius;
in vec4 a_Color;

out vec4 v_Color;

uniform float u_fTime;

const float PI = 3.141592f;

void main()
{
	vec4 newPosition = vec4(a_Position, 1);

	float rad = PI * u_fTime * 0.25f;

	newPosition.x = cos(rad) * a_Radius + newPosition.x;
	newPosition.y = sin(rad) * a_Radius + newPosition.y;

	gl_Position = newPosition;

	v_Color = a_Color;
}
