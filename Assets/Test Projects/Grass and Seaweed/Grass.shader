﻿Shader "Custom/Grass"
{
    Properties
    {
        [MainTexture] _BaseMap("Grass Texture",2D) = "white" {}
        _ACutoff("Cutoff", Float) = 0.5
        _Height("Height", Float) = 1.0
        _Base("Base", Float) = 1.0
        _LightingIntensity("Light Intensity", Float) = 0.10
        _AmbientIntensity("Ambient Intensity", Float) = 0.10
        _ShadowIntensity("Shadow Intensity", Float) = 0.10
        _Tint("Tint", Color) = (0.5,0.5,0.5,1)
        _ShadowTint("Shadow Tint", Color) = (0.5,0.5,0.5,1)

        _DistortionStrength("Distortion Strength", Float) = 1
        _DistortionSpeed("Distortion Speed", Float) = 2.5
        _DistortionDir("Distortion Vector", Vector) = (0.05,0,0)
        _DistortionMap("Distortion Map", 2D) = "white" {}
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
            LOD 100

            Pass
            {
                Name "Depth Pass"
                Tags {"LightMode" = "DepthOnly"}

                ZWrite On
                ColorMask 0
                Cull Off

                HLSLPROGRAM

                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0

                #pragma multi_compile _ WRITE_NORMAL_BUFFER
                #pragma multi_compile _ WRITE_MSAA_DEPTH
                #pragma shader_feature _ALPHATEST_ON
                #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma multi_compile_instancing

                #pragma require geometry

                #pragma geometry LitPassGeom
                #pragma vertex LitPassVertex
                #pragma fragment DepthPassFragment

                #define SHADOW

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

                #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

                #include "GrassPass.hlsl"

                half4 DepthPassFragment(Varyings input) : SV_TARGET
                {
                    return 0;
                }

                ENDHLSL
            }

            Pass
            {
                Name "Geometry Pass"
                Tags {"LightMode" = "UniversalForward"}

                ZWrite On
                Cull Off

                HLSLPROGRAM

                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 4.0

                #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
                #pragma shader_feature _GLOSSYREFLECTIONS_OFF
                #pragma shader_feature _ALPHATEST_ON
                #pragma shader_feature _ALPHAPREMULTIPLY_ON
                #pragma shader_feature _SPECULAR_SETUP
                #pragma shader_feature _RECEIVE_SHADOWS_ON
            //#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS

            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            #pragma require geometry

            #pragma geometry LitPassGeom
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            #include "GrassPass.hlsl"


            ENDHLSL

            }
        }
}