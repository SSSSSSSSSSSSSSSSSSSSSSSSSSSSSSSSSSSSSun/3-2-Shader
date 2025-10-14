#version 330

in vec3 a_Position;

uniform float u_fTime;

out vec4 v_Color;

const float c_PI = 3.141592f;

void Flag()
{
	vec4 newPosition = vec4(a_Position, 1);

	float xValue = newPosition.x+0.5f;
	newPosition.y *= 1-abs(xValue);
	newPosition.y += sin((xValue-u_fTime) * 2.0f * c_PI) * 0.2f * xValue;

	float newColor = (sin((xValue-u_fTime) * 2.0f * c_PI) + 1)/2;

	gl_Position = newPosition;
	v_Color = vec4(1,1,1,newColor);
}

void Wave()
{
	vec4 newPosition = vec4(a_Position, 1);
	float dX = 0;
	float dY = 0;

	vec2 pos = vec2(a_Position.xy);
	vec2 cen = vec2(0,0);
	float dis = distance(cen, pos);
	float v = 2*clamp(0.5 - dis, 0, 1);
	float newColor = v*sin(dis*18*c_PI - u_fTime*10);

	
	gl_Position = newPosition;

	v_Color = vec4(1,1,1,newColor);
}

void main()
{
	//Flag();
	Wave();
}
