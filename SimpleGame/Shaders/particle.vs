#version 330

in vec3 a_Position;
in float a_Radius;
in vec4 a_Color;
in float a_STime;
in vec3 a_InitVec;

out vec4 v_Color;

uniform float u_fTime;

const float c_PI = 3.141592f;
const vec2 c_G = vec2(0, -9.8f);

void main()
{
	float newTime = u_fTime - a_STime;
	vec4 newPosition = vec4(a_Position,1);

	if(newTime > 0)
	{
		float t = fract( (u_fTime+a_STime)*0.5f)*2.0f;
		float tt = t*t;

		float rad = c_PI * t ;
		newPosition.xy += a_InitVec.xy * t + 0.5f * c_G * tt * 0.0625f ;
	}
	else{
		newPosition.xy = vec2(-100000.0f, 0);
	}
	gl_Position = newPosition;
	v_Color = a_Color;
}
