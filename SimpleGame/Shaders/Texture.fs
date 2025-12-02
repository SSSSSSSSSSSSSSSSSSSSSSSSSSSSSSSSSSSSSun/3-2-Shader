#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_Tex;

uniform sampler2D u_TexID;
uniform sampler2D u_TexID1;
uniform int u_Method; // 0 : Normal, 1 : GaussianBlueH, 2 : GaussianBlueV, 3 : Merge
uniform float u_fTime;

vec4 Normal(sampler2D textureSampler,vec2 uv)
{
    return texture(textureSampler, uv);
}

vec4 ChromaticAberration(sampler2D textureSampler,vec2 uv)
{
    // 1. 색수차 강도 및 중심 계산
    // 이 값을 조절하여 색상 번짐의 정도를 바꿉니다.
    const float aberrationStrength = 0.25; 
    vec2 center = vec2(0.5, 0.5);
    vec2 distVec = uv - center;
    
    // 중앙으로부터의 거리에 비례하여 오프셋 강도를 결정합니다.
    float distanceFactor = length(distVec) * 1.5; // 거리를 조금 증폭
    
    // 최종 오프셋 값
    vec2 offset = distVec * distanceFactor * aberrationStrength;

    // 2. 각 채널별 오프셋 적용 및 샘플링
    
    // 빨강(R) 채널: 바깥쪽(중앙에서 멀어지는 방향)으로 더 밀기
    vec2 r_uv = uv + offset * 1.0; 
    
    // 초록(G) 채널: 기본 좌표 (또는 아주 미세한 오프셋)
    vec2 g_uv = uv + offset * 0.1; 
    
    // 파랑(B) 채널: 안쪽(중앙으로 당기는 방향)으로 당기기
    vec2 b_uv = uv - offset * 0.8; 

    // 3. 각 채널 값을 가져와 조합
    float r_channel = texture(textureSampler, r_uv).r;
    float g_channel = texture(textureSampler, g_uv).g;
    float b_channel = texture(textureSampler, b_uv).b;
    float alpha = texture(textureSampler, uv).a; // 알파는 원본 좌표에서 가져옵니다.

    return vec4(r_channel, g_channel, b_channel, alpha);
}


const float c_PixelSize = 150;
const float c_MinPixelSize = 10;


vec4 PixelateEffect(sampler2D textureSampler, vec2 uv)
{
    float resol = (sin(u_fTime)+1)*(c_PixelSize-c_MinPixelSize)+c_MinPixelSize;
    float tx = floor(uv.x*resol)/resol;
    float ty = floor(uv.y*resol)/resol;

    return texture(textureSampler, vec2(tx, ty));
}

const float weight[5] = float[] (0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216);

vec4 GaussianBlueH(sampler2D textureSampler, vec2 uv)
{             
    vec2 tex_offset = 1.0 / textureSize(textureSampler, 0); // gets size of single texel
    vec3 result = texture(textureSampler, uv).rgb * weight[0]; // current fragment's contribution

    for(int i = 1; i < 5; ++i)
    {
        result += texture(textureSampler, uv + vec2(tex_offset.x * i, 0.0)).rgb * weight[i];
        result += texture(textureSampler, uv - vec2(tex_offset.x * i, 0.0)).rgb * weight[i];
    }

    return vec4(result, 1.0);
}

vec4 GaussianBlueV(sampler2D textureSampler, vec2 uv)
{             
    vec2 tex_offset = 1.0 / textureSize(textureSampler, 0); // gets size of single texel
    vec3 result = texture(textureSampler, uv).rgb * weight[0]; // current fragment's contribution

    for(int i = 1; i < 5; ++i)
    {
        result += texture(textureSampler, uv + vec2(0.0, tex_offset.y * i)).rgb * weight[i];
        result += texture(textureSampler, uv - vec2(0.0, tex_offset.y * i)).rgb * weight[i];
    }

    return vec4(result, 1.0);
}

vec4 Merge(sampler2D textureSampler,sampler2D textureSampler1, vec2 uv)
{
    const float gamma = 2.2;
    vec3 hdrColor = texture(textureSampler, uv).rgb;      
    vec3 bloomColor = texture(textureSampler1, uv).rgb;
    hdrColor += bloomColor; 

    // tone mapping
    vec3 result = vec3(1.0) - exp(-hdrColor * 5);

    // also gamma correct while we're at it       
    result = pow(result, vec3(1.0 / gamma));


    return vec4(result, 1.0);
}

void main()
{
    vec2 uv = vec2(v_Tex.x, 1.0 - v_Tex.y); 
    
	FragColor = vec4(0);

    if(0 == u_Method) { FragColor = Normal(u_TexID, uv); }
    if(1 == u_Method) { FragColor = GaussianBlueH(u_TexID, uv); }
    if(2 == u_Method) { FragColor = GaussianBlueV(u_TexID, uv); }
    if(3 == u_Method) { FragColor = Merge(u_TexID, u_TexID1, uv); }
    
}
