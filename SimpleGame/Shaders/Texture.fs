#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_Tex;

uniform sampler2D u_TexID;
uniform float u_fTime;

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


const int u_PixelSize = 50;

vec4 PixelateEffect(sampler2D textureSampler, vec2 uv)
{
    float resol = (sin(u_fTime)+1)*u_PixelSize;
    float tx = floor(uv.x*resol)/resol;
    float ty = floor(uv.y*resol)/resol;

    return texture(textureSampler, vec2(tx, ty));
}

void main()
{
    vec2 uv = vec2(v_Tex.x, 1.0 - v_Tex.y); 
    
	FragColor = PixelateEffect(u_TexID, uv);
}
