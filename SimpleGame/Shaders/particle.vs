#version 330

in vec3 a_Position;
in float a_Value;
in vec4 a_Color;
in float a_STime;
in vec3 a_InitVec;
in float a_LifeTime;
in float a_Mass;
in float a_Period;

out vec4 v_Color;

uniform float u_fTime;
uniform float u_fGravity;
uniform vec3 u_Force;

const float c_PI = 3.141592f;

void raining()
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

void sinParticle()
{
	vec4 centerC = vec4(1,0,0,1);
	vec4 borderC = vec4(1,1,1,0);

	float globalTime = u_fTime * 0.5f;

	float newTime = u_fTime - a_STime;
	vec4 newPosition = vec4 (a_Position,1);
	float newAlpha = 1.0f;

	vec4 newColor = a_Color;


	if(newTime > 0){
		float period = a_Period*10;

		float t = fract((newTime)/a_LifeTime) * a_LifeTime;
		float tt = t*t;

		float x = t*2.0f-1.0f;
		float y = t * sin(2*period*t*c_PI) * ((a_Value-0.5f)*2);
		//y *= sin(fract(newTime/a_LifeTime) * c_PI);

		newPosition.xy += vec2(x,y);
		newAlpha = 1.0f - t / a_LifeTime;
		
		newColor = mix(centerC, borderC, abs(y));


		//mat4 TM = mat4(cos(globalTime), 0, sin(globalTime), 0,
		//				0, 1, 0, 0,
		//				-sin(globalTime), 0, cos(globalTime), 0,
		//				0, 0, 0, 1); 

		//newPosition.xy = (newPosition*TM).xy;

		
	}
	else{
		newPosition.y = -15.57f;
	}

	gl_Position = newPosition;
	v_Color = vec4(newColor.xyz, newAlpha);
}


void circleParticle()
{
	float newTime = fract(u_fTime - a_STime);
	float lifeTime = a_LifeTime;
	vec4 newPosition = vec4 (a_Position,1);
	float newAlpha = 1.0f;

	vec4 newColor = a_Color;


	if(newTime > 0){
		float period = a_Period*10;

		float t = fract((newTime)/a_LifeTime) * a_LifeTime;
		float tt = t*t;

		float x = sin(a_Value*2*c_PI);
		float y = cos(a_Value*2*c_PI);

		vec2 GravityVec = vec2(0, u_fGravity) * 0.0625f;

		float newX = x + 0.5f*GravityVec.x*tt;
		float newY = y + 0.5f*GravityVec.y*tt;

		newPosition.xy = vec2(newX,newY);

		newAlpha = 1.0f - t / a_LifeTime;
	
		
	}
	else{
		newPosition.y = -15.57f;
	}

	gl_Position = newPosition;
	v_Color = vec4(a_Color.xyz, newAlpha);
}

void main()
{
	//raining();
	//sinParticle();
	circleParticle();
}
