#version 330

in vec3 a_Position;
in float a_Radius;
in vec4 a_Color;
in float a_STime;
in vec3 a_InitVec;
in float a_LifeTime;
in float a_Mass;

out vec4 v_Color;

uniform float u_fTime;
uniform float u_fGravity;
uniform vec3 u_Force;

const float c_PI = 3.141592f;

void main()
{
	vec2 GravityVec = vec2(0, u_fGravity)  * 0.0625f;
	float lifeTime = a_LifeTime;
	float newAlpha = 1.0f;
	float newTime = u_fTime - a_STime;
	vec4 newPosition = vec4(a_Position,1);

	if(newTime > 0)
	{
		float t = fract( (newTime)/lifeTime ) * lifeTime;	// t : 0 ~ lifeTime
		float tt = t*t;

		vec3 F = u_Force + vec3(GravityVec, 0.0f)*a_Mass;
		vec3 a = F / a_Mass;

		newPosition.xyz += a_InitVec * t + a * tt * 0.5f ;

		newAlpha = 1.0f - t / lifeTime;		// 1 ~ 0
	}
	else{
		newPosition.xy = vec2(-100000.0f, 0);
	}
	gl_Position = newPosition;
	
	v_Color = vec4(a_Color.xyz, newAlpha);
}
