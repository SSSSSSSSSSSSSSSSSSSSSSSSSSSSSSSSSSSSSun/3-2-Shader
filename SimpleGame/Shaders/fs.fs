#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_UV;

uniform float u_fTime;

const float c_PI = 3.141592;

void temp()
{
	vec4 newColor = vec4(0);
    
    float xValue = 1-pow(abs(sin(v_UV.x * 2 * 4 * c_PI)), 0.5);
    float yValue = 1-pow(abs(sin(v_UV.y * 2 * 4 * c_PI)), 0.5);
    float rdValue = 1-pow(abs(sin((v_UV.x-v_UV.y) * 2 * 4 * c_PI)), 0.5);
    float ldValue = 1-pow(abs(sin((1-v_UV.x-v_UV.y) * 2 * 4 * c_PI)), 0.5);
    newColor = vec4(max(max(max(xValue,yValue),rdValue),ldValue)); 
    FragColor = newColor;
}

void main()
{
    //temp();
    temp2();
}
