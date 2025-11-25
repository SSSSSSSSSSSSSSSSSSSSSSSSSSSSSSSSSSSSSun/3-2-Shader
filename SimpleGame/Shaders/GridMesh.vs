#version 330
#define MAX_POINT 100
in vec3 a_Position;

uniform float u_fTime;

out vec4 v_Color;
out vec2 v_UV;

const float c_PI = 3.141592f;
uniform vec4 u_Points[MAX_POINT];

void Flag()
{
	vec4 newPosition = vec4(a_Position, 1);

	float xValue = newPosition.x+0.5f;
	newPosition.y *= 1-abs(xValue);
	newPosition.y += sin((xValue-u_fTime) * 2.0f * c_PI) * 0.2f * xValue;

	float newColor = (sin((xValue-u_fTime) * 2.0f * c_PI) + 1)/2;

	gl_Position = newPosition;
	v_Color = vec4(1,1,1,newColor);
	v_UV = vec2(a_Position.x + 0.5f, 0.5f - a_Position.y);
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

void RainDrop()
{
	vec4 newPosition = vec4(a_Position, 1);
	float dX = 0;
	float dY = 0;

	vec2 pos = vec2(a_Position.xy);
	float newColor = 0;

	for(int i=0; i<100; ++i){
		float sTime = u_Points[i].z;
		float lTime = u_Points[i].w;
		float newTime = u_fTime - sTime;
		if(newTime > 0){
			float baseTime = fract(newTime / lTime);
			float oneMinus = 1 - baseTime;
			float t = newTime;
			float range = baseTime * lTime / 5;
			vec2 cen = u_Points[i].xy;
			float dis = distance(cen, pos);
			float v = 25*clamp(range - dis, 0, 1);
			newColor += oneMinus*v*sin(dis*18*c_PI - t*10);
		}
	}
	

	newPosition += vec4(dX, dY, 0, 0);
	gl_Position = newPosition;

	v_Color = vec4(1,1,1,newColor);
}

void Pulse()
{
   vec4 newPosition = vec4(a_Position, 1);
   float t = fract(u_fTime);

   vec2 pos = vec2(a_Position.xy);
   vec2 cen = vec2(0, 0);
   float d = distance(pos, cen);

   float ringDifference = abs(d - t);
   float thickness = 20.0;

   float ring = 1.0 - clamp(ringDifference * thickness, 0, 1);
   float fade = 1.0 - t;
   float newColor = ring * fade;

   gl_Position = newPosition;
   v_Color = vec4(1,1,1,newColor);
}

void P8()
{
	vec4 newPosition = vec4(a_Position, 1);
	float valueX = newPosition.x + 0.5;
	float greyScale = sin(2*c_PI * valueX * 4);
	v_Color = vec4(greyScale);
	gl_Position = newPosition;
}

void P9()
{
	vec4 newPosition = vec4(a_Position, 1);
	float valueX = newPosition.x + 0.5;
	float valueY = newPosition.y + 0.5;
	float greyScale = sin(2*c_PI * valueX * 2);
	greyScale = greyScale + sin(2*c_PI*valueY*2);
	v_Color = vec4(greyScale);
	gl_Position = newPosition;
}

void main()
{
	//Flag();
	//Wave();
	//RainDrop();
	//Pulse();
	P9();


}
