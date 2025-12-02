#version 330

layout(location=0) out vec4 FragColor;
layout(location=1) out vec4 FragColor1;

uniform sampler2D u_Texture;

in vec4 v_Color;
in vec2 v_UV;

vec4 Textured()
{
	vec4 result = texture(u_Texture, v_UV)*v_Color*2;
	//vec4 result = texture(u_Texture, v_UV);
	//result = v_Color;
	float brightness = dot(result.rgb, vec3(0.2126, 0.7152, 0.0722));
	
	if(brightness > 1.0)
		FragColor1 = result;
	else
		FragColor1 = vec4(0);

	return result;
}

void main()
{
	FragColor = Textured();
}
