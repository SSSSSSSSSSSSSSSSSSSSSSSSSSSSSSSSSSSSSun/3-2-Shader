#version 330

in vec3 a_Position;

uniform float u_fTime;

out vec4 v_Color;

const float c_PI = 3.141592f;

void main()
{
	vec4 newPosition = vec4(a_Position, 1);

	float xValue = newPosition.x+0.5f;
	newPosition.y *= 1-abs(xValue);
	newPosition.y += sin((xValue+u_fTime) * 2.0f * c_PI) * 0.2f * xValue;

	float newColor = (sin((xValue+u_fTime) * 2.0f * c_PI) + 1)/2;

	gl_Position = newPosition;
	v_Color = vec4(newColor);
}
