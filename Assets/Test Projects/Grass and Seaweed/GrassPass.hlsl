
struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 uv           : TEXCOORD0;
    float2 uvLM         : TEXCOORD1;
    float4 color : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct Varyings
{
    float3 normalOS : NORMAL;
    float2 uv                       : TEXCOORD0;
    float2 uvLM                     : TEXCOORD1;
    float4 positionWSAndFogFactor   : TEXCOORD2; // xyz: positionWS, w: vertex fog factor
    half3  normalWS                 : TEXCOORD3;
    half3 tangentWS                 : TEXCOORD4;
    float4 positionOS : TEXCOORD5;

    float4 color : COLOR;

#if _NORMALMAP
    half3 bitangentWS               : TEXCOORD5;
#endif

#ifdef _MAIN_LIGHT_SHADOWS
    float4 shadowCoord              : TEXCOORD6; // compute shadow coord per-vertex for the main light
#endif
    float4 positionCS               : SV_POSITION;
};

//Properties
float _ACutoff;
float _Height;
float _Base;
float4 _Tint;
float4 _ShadowTint;
float _LightingIntensity;
float _AmbientIntensity;
float _ShadowIntensity;
float _DistortionSpeed;
float _DistortionStrength;
float3 _DistortionDir;
sampler2D _DistortionMap;

//Functions
float4 TransformWorldToShadowCoords(float3 _pos)
{
    half cascadeIndex = ComputeCascadeIndex(_pos);
    return mul(_MainLightWorldToShadow[cascadeIndex], float4(_pos, 1.0));
}

float3x3 RotateY(float _angle)
{
    return float3x3
        (
            cos(_angle), 0, sin(_angle),
            0, 1, 0,
            -sin(_angle), 0, cos(_angle)
            );
}

float3x3 RotateX(float _angle)
{
    return float3x3
        (
            1, 0, 0,
            0, cos(_angle), -sin(_angle),
            0, sin(_angle), cos(_angle)
            );
}

float3x3 RotateZ(float _angle)
{
    return float3x3
        (
            cos(_angle), -sin(_angle), 0,
            sin(_angle), cos(_angle), 0,
            0, 0, 1
            );
}

float rand(float3 co)
{
    return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
}


//VERTEX PASS
Varyings LitPassVertex(Attributes input)
{
    Varyings output;

    output.color = input.color;

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);
    VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

    float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

    output.uvLM = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;

    output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
    output.positionCS = vertexInput.positionCS;
    output.positionOS = input.positionOS;

    output.normalWS = vertexNormalInput.normalWS;
    output.tangentWS = vertexNormalInput.tangentWS;

#ifdef _NORMALMAP
    output.bitangentWS = vertexNormalInput.bitangentWS;
#endif

#ifdef _MAIN_LIGHT_SHADOWS
    output.shadowCoord = GetShadowCoord(vertexInput);
#endif

    return output;
}

//Geometry Pass
[maxvertexcount(6)]
void LitPassGeom(triangle Varyings input[3], inout TriangleStream<Varyings> outStream)
{
    float2 uv = (input[0].positionOS.xy * _Time.xy * _DistortionDir) * _DistortionSpeed;

    float4 distortionSample = tex2Dlod(_DistortionMap, float4(uv, 0, 0)) * _DistortionStrength;

    float3 rotNormal = mul(mul(input[0].normalWS, RotateZ(distortionSample.x)), RotateX(distortionSample.y));

    float randHeight = clamp(rand(input[0].positionWSAndFogFactor.xyz * 2.234352f) * _Height, 0.5 * _Height, 1.5 * _Height);

    float3 basePos = (input[0].positionWSAndFogFactor.xyz + input[1].positionWSAndFogFactor.xyz + input[2].positionWSAndFogFactor.xyz) / 3;

    Varyings o = input[0];
    float3 tangentRot = normalize(mul(o.tangentWS, RotateY(rand(o.positionWSAndFogFactor.xyz) * 90)));
    float3 oPos = (basePos - tangentRot * _Base);
    o.positionCS = TransformWorldToHClip(oPos);

    Varyings o2 = input[0];
    float3 o2Pos = (basePos + tangentRot * _Base);
    o2.positionCS = TransformWorldToHClip(o2Pos);

    Varyings o3 = input[0];
    float3 o3Pos = (basePos + tangentRot * _Base + rotNormal * randHeight * 2);
    o3.positionCS = TransformWorldToHClip(o3Pos);

    Varyings o4 = input[0];
    float3 o4Pos = (basePos - tangentRot * _Base + rotNormal * randHeight * 2);
    o4.positionCS = TransformWorldToHClip(o4Pos);

    float3 normalRot = mul(tangentRot, RotateY(PI / 2));

    o.uv = TRANSFORM_TEX(float2(0, 0), _BaseMap);
    o2.uv = TRANSFORM_TEX(float2(1, 0), _BaseMap);
    o3.uv = TRANSFORM_TEX(float2(1, 1), _BaseMap);
    o4.uv = TRANSFORM_TEX(float2(0, 1), _BaseMap);

    o.normalWS = normalRot;
    o2.normalWS = normalRot;
    o3.normalWS = normalRot;
    o4.normalWS = normalRot;

    outStream.Append(o4);
    outStream.Append(o3);
    outStream.Append(o);

    outStream.RestartStrip();

    outStream.Append(o3);
    outStream.Append(o2);
    outStream.Append(o);

    outStream.RestartStrip();
}

//Fragment Pass
half4 LitPassFragment(Varyings input, bool vf : SV_IsFrontFace) : SV_Target
{
    half3 normalWS = input.normalWS;

    normalWS = normalize(normalWS);

    if (vf == true) 
    {
        normalWS = -normalWS;
    }

    float3 positionWS = input.positionWSAndFogFactor.xyz;

    half3 color = (0, 0, 0);

    Light mainLight;

    float4 shadowCoord = TransformWorldToShadowCoords(positionWS);

    mainLight = GetMainLight(shadowCoord);

    float3 normalLight = LightingLambert(mainLight.color, mainLight.direction, normalWS) * _LightingIntensity;
    float3 invNormalLight = LightingLambert(mainLight.color, mainLight.direction, -normalWS) * _AmbientIntensity;

    color = _Tint + normalLight + invNormalLight;

    color = lerp(color, _ShadowTint, 1 - input.uv.y);

    color = lerp(_ShadowTint, color, clamp(mainLight.shadowAttenuation + _ShadowIntensity, 0, 1));

    float fogFactor = input.positionWSAndFogFactor.w;

    color = MixFog(color, fogFactor);

    float alpha = _BaseMap.Sample(sampler_BaseMap, input.uv).a;

    clip(alpha - _ACutoff);

    return half4(color,1);
}