#version 330

layout(location=0) out vec4 FragColor;
layout(location=1) out vec4 FragColor1;

in vec2 v_UV;

uniform sampler2D u_RGBTexture;
uniform sampler2D u_DigitTextureLoc;
uniform sampler2D u_NumTextureLoc;
uniform float u_fTime;

const float c_PI = 3.141592;

vec4 temp()
{
	vec4 newColor = vec4(0);
    
    float xValue = 1-pow(abs(sin(v_UV.x * 2 * 4 * c_PI)), 0.5);
    float yValue = 1-pow(abs(sin(v_UV.y * 2 * 4 * c_PI)), 0.5);
    float rdValue = 1-pow(abs(sin((v_UV.x-v_UV.y) * 2 * 4 * c_PI)), 0.5);
    float ldValue = 1-pow(abs(sin((1-v_UV.x-v_UV.y) * 2 * 4 * c_PI)), 0.5);
    newColor = vec4(max(max(max(xValue,yValue),rdValue),ldValue)); 
    return newColor;
}

vec4 texture1()
{
    vec2 newUV = v_UV;
    float dx = 0;
    float dy = 0;

    newUV += vec2(dx,dy);

    vec4 sampledColor = texture(u_RGBTexture, newUV);

    return sampledColor;
}

vec4 texture2()
{
    vec2 newUV = v_UV;
    float dx = 0.1*sin(v_UV.y * 2 * c_PI * 2+ u_fTime);
    float dy = 0.1*sin(v_UV.x * 2 * c_PI * 2+ u_fTime);

    newUV += vec2(dx,dy);

    vec4 sampledColor = texture(u_RGBTexture, newUV);
   
    return sampledColor;
}


vec4 Circles()
{
    vec2 newUV = v_UV; //0~1, left top (0,0)
    vec2 center = vec2(0.5, 0.5);
    vec4 newColor = vec4(0);
    float d = distance(newUV, center);

    float value = sin(d*4*c_PI*4 - u_fTime * 10);
    newColor = vec4(value);

    //if(d < 0.5)
    //{
    //    newColor = vec4(1);
    //}

    return newColor;
}

vec4 Flag()
{
    vec2 newUV = vec2(v_UV.x, 1-v_UV.y-0.5); //0~1, left bottom (0,0)
    vec4 newColor = vec4(0);

    float width = 0.2 * (1-newUV.x);
    float sinValue = newUV.x*0.2*sin(newUV.x*2*c_PI - u_fTime*2);

    if (newUV.y < sinValue+width && newUV.y > sinValue-width)
    {
        newColor = vec4(1);
    }
    //else
    //{
    //    discard;
    //}
    return newColor;
}

vec4 Q1()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y); //0~1, left top (0,0)
    float x = newUV.x; //0~1
    float y = 1-abs(2*(newUV.y - 0.5)); //0~1~0

    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    return newColor;
}

vec4 Q2()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y); //0~1, left top (0,0)
    float x = fract(newUV.x*3); //0~1 0~1 0~1
    float y = (2-floor(newUV.x*3))/3 + newUV.y/3; //

    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    return newColor;
}

vec4 Q3()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y); //0~1, left top (0,0)
    float x = fract(newUV.x*3); //0~1 0~1 0~1
    float y = (floor(newUV.x*3))/3 + newUV.y/3; //

    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    return newColor;
}

vec4 Brick_Horizontal()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y);
    float rCount = 5.0;
    float sAmount = 0.5;
    float x = fract(newUV.x * rCount) + floor(newUV.y*rCount - 1) * sAmount;
    float y = fract(newUV.y * rCount);

    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    return newColor;
}

vec4 Brick_Vertical()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y);
    float x = fract(newUV.x * 2);
    float y = fract(newUV.y * 2) + floor(newUV.x*2) * 0.5f;

    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    return newColor;
}

vec4 Brick_Horizontal_AI()
{
    float TILE_X = 5.0; // 가로 타일 개수 (그룹 개수)
    float TILE_Y = 5.0; // 세로 타일 개수

    vec2 tiledUV = v_UV * vec2(TILE_X, TILE_Y);

    // ********** 1. 벽돌 타일링 UV 생성 **********
    float row = floor(tiledUV.y); // 현재 픽셀이 속한 행 인덱스
    
    // 홀수 행에만 반 타일(0.5)만큼 오프셋 적용
    float offset = mod(row, 2.0) * 0.5; 
    
    float brickX = tiledUV.x + offset;
    // fract()를 사용하여 타일을 반복합니다.
    vec2 finalUV = vec2(fract(brickX), fract(tiledUV.y));
    
    // 텍스처에서 색상 샘플링 (벽돌 모양으로 타일링된 텍스처)
    vec4 sampledColor = texture(u_RGBTexture, finalUV);
    
    return sampledColor;
}

// 벽돌 타일링이 적용된 화면을 회전시키는 함수
vec4 RotatingBrickTiling()
{
    // ********** 1. UV 좌표 회전 변환 **********
    vec2 center = vec2(0.5, 0.5); // 회전 중심 (화면 중앙)

    // UV 좌표를 중심으로 이동 (-0.5 ~ 0.5 범위로 만듦)
    vec2 rotatedUV = v_UV - center;

    // 시간에 따른 회전 각도 계산 (u_fTime에 비례하여 회전)
    float rotationSpeed = 1.5; // 회전 속도 조절 (값이 클수록 빠르게 회전)
    float angle = u_fTime * rotationSpeed; // 라디안 각도

    // 회전 행렬 계산에 필요한 sin/cos 값
    float cosAngle = cos(angle);
    float sinAngle = sin(angle);

    // X, Y 좌표 회전 공식 적용
    // X' = X*cos(angle) - Y*sin(angle)
    // Y' = X*sin(angle) + Y*cos(angle)
    float x_rotated = rotatedUV.x * cosAngle - rotatedUV.y * sinAngle;
    float y_rotated = rotatedUV.x * sinAngle + rotatedUV.y * cosAngle;

    // 회전된 좌표를 다시 0.0 ~ 1.0 범위로 이동 (중심으로 다시 +0.5)
    rotatedUV = vec2(x_rotated, y_rotated) + center;

    // ********** 2. 벽돌 타일링 UV 생성 (회전된 UV에 적용) **********
    // 회전된 UV 좌표가 0.0 ~ 1.0 범위를 벗어날 수 있으므로,
    // fmod()나 fract()를 사용하여 반복 처리 필요.
    // 하지만 여기서는 화면 내에서만 처리하므로 fract는 최종 텍스처에만 적용.

    // Tiling 계수 설정
    float TILE_X = 3.0; // 가로 타일 개수
    float TILE_Y = 3.0; // 세로 타일 개수

    // 회전된 UV에 타일링 스케일 적용
    vec2 tiledUV = rotatedUV * vec2(TILE_X, TILE_Y);

    // 벽돌 오프셋 계산 (행 인덱스도 회전된 UV 기준)
    float row = floor(tiledUV.y); 
    float offset = mod(row, 2.0) * 0.5; // 홀수 행은 반 타일 오프셋
    
    float brickX = tiledUV.x + offset;

    // 최종 UV 좌표는 fract()를 적용하여 0.0~1.0 범위로 반복
    vec2 finalUV = vec2(fract(brickX), fract(tiledUV.y));
    
    // ********** 3. 텍스처 샘플링 **********
    vec4 sampledColor = texture(u_RGBTexture, finalUV);
    
    return sampledColor;
}

vec4 Digit()
{
    return texture(u_DigitTextureLoc, v_UV);
}

vec4 Digit_Num()
{
    int digit = int(u_fTime)%10;

    int tileIndex = (digit + 9)%10;

    float time = u_fTime - 1.0f;
    float tx = v_UV.x;
    float ty = v_UV.y; 
    tx *= 0.2f;
    ty *= 0.5f;
    tx += 0.2f * float(digit % 5) / 5;
    ty += 0.5f * floor(float(tileIndex) / 5);
    return texture(u_NumTextureLoc, vec2(tx, ty));
}

vec4 Digit_Num_FourDigits_NoIf()
{
    // 1. 현재 시간(정수)을 10000으로 나눈 나머지로 제한 (0~9999)
    int totalTime = int(u_fTime) % 10000; 
    
    // 2. 각 자리 숫자 분리
    int digitD1 = totalTime / 1000;         // 천의 자리
    int digitD2 = (totalTime / 100) % 10;   // 백의 자리
    int digitD3 = (totalTime / 10) % 10;    // 십의 자리
    int digitD4 = totalTime % 10;           // 일의 자리

    // 3. 현재 픽셀이 속한 열(Column) 인덱스 계산 (0, 1, 2, 3)
    float numSegments = 4.0;
    float colIndex = floor(v_UV.x * numSegments);

    // ********** 4. 숫자 선택 (if 문 대체) **********
    float digitToSample;

    // a) mix 함수를 이용한 선택
    // mix(x, y, a)는 a가 0이면 x를, a가 1이면 y를 반환합니다.
    // 여기서는 4단계의 mix를 중첩하여 4개의 digit 중 하나를 선택합니다.
    
    // colIndex가 3.0일 때 digitD4 선택 (일의 자리)
    digitToSample = float(digitD4);
    
    // colIndex가 2.0일 때 digitD3 선택 (십의 자리)
    float mask3 = step(2.0, colIndex) * step(colIndex, 3.0);
    digitToSample = mix(float(digitD3), digitToSample, step(3.0, colIndex));

    // colIndex가 1.0일 때 digitD2 선택 (백의 자리)
    float mask2 = step(1.0, colIndex) * step(colIndex, 2.0);
    digitToSample = mix(float(digitD2), digitToSample, step(2.0, colIndex));

    // colIndex가 0.0일 때 digitD1 선택 (천의 자리)
    float mask1 = step(0.0, colIndex) * step(colIndex, 1.0);
    digitToSample = mix(float(digitD1), digitToSample, step(1.0, colIndex));
    
    // 간소화된 버전 (colIndex를 사용하여 인덱싱)
    // 이 코드가 더 간단하고 명확합니다.
    float d1 = float(digitD1);
    float d2 = float(digitD2);
    float d3 = float(digitD3);
    float d4 = float(digitD4);
    
    // step(엣지, 값)은 값이 엣지보다 크거나 같으면 1.0, 아니면 0.0을 반환합니다.
    // 0.0일 때 d1, 1.0일 때 d2, 2.0일 때 d3, 3.0일 때 d4를 선택합니다.
    digitToSample = d1;
    digitToSample = mix(digitToSample, d2, step(1.0, colIndex));
    digitToSample = mix(digitToSample, d3, step(2.0, colIndex));
    digitToSample = mix(digitToSample, d4, step(3.0, colIndex));


    // 5. UV 좌표 정규화
    numSegments = 4.0;
    float finalX = v_UV.x * numSegments - colIndex; 

    // ********** 6. 아틀라스 인덱스 보정 (if 문 대체) **********
    float atlasIndex;
    
    // step(a, x)는 x >= a 일 때 1.0, x < a 일 때 0.0
    // (digitToSample == 0.0) 이면 1.0, 아니면 0.0 인 마스크 생성
    float isZero = step(0.5, 1.0 - digitToSample); // 0.0일 때 1.0, 1.0~9.0일 때 0.0

    // mix(x, y, a): isZero가 1.0이면 9.0을, 0.0이면 digitToSample - 1.0을 선택
    atlasIndex = mix(digitToSample - 1.0, 9.0, isZero);
    
    // ********** 7. 텍스처 아틀라스 내 최종 좌표 계산 **********
    float tileWidth = 0.2f;  
    float tileHeight = 0.5f; 

    float colTile = mod(atlasIndex, 5.0); 
    float rowTile = floor(atlasIndex / 5.0);

    float tx = finalX * tileWidth;
    tx += tileWidth * colTile; 

    float ty = v_UV.y * tileHeight;
    ty += tileHeight * rowTile;
    
    return texture(u_NumTextureLoc, vec2(tx, ty));
}

void main()
{
    //Digit_Num();
    FragColor = Circles();
    FragColor1 = Flag();
}