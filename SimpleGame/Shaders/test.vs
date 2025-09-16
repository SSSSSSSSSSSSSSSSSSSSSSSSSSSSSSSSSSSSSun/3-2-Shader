#version 330

in vec3 a_Position;
in vec4 a_Color;

out vec4 v_Color;

uniform float u_fTime;

const float PI = 3.141592f;

void main()
{
	vec4 newPosition = vec4(a_Position, 1);

	//newPosition.xy = newPosition.xy + vec2(tan(u_fTime)*0.5f,0);

	float rad = PI * u_fTime;
	float size = fract(u_fTime*0.125f) ;

	newPosition.xy = newPosition.xy + vec2(cos(rad)*size, sin(rad)*size);

	gl_Position = newPosition;

	v_Color = a_Color;
}
