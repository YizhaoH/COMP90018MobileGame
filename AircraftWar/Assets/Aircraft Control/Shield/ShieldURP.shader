Shader "taecg/Template/Unlit URP Shader"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture" , 2D) = "white" {}
        _Fresnel("Fresnel Intensity", Range(0,100)) = 3.0
        _FresnelWidth("Fresnel Width", Range(0,5)) = 1
        _Distort("Distort" , Range(0,100)) = 1
        _IntersectionThreshold("Intersection Threshold Highlight", Range(0,1)) = .1
        _Threshold("Threshold", Range(0,1)) = 1
        _ScrollSpeedU("U Speed", float) = 2
        _ScrollSpeedV("V Speed", float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType" = "Transparent" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "Unlit"
            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
                float4 normal           : NORMAL;
            };

            struct Varyings
            {
                float3 positionWS   : TEXCOORD4;
                float4 positionCS       : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float2 uv2          : TEXCOORD1;
                half3 rimColor      : TEXCOORD2;
                float4 screenPos    : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
            half4 _MainColor;
            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            half _Fresnel;
            half _FresnelWidth;
            half _Distort;
            half _IntersectionThreshold;
            half _Threshold;
            half _ScrollSpeedU;
            half _ScrollSpeedV;
            CBUFFER_END
            TEXTURE2D (_MainTex);SAMPLER(sampler_MainTex);
            TEXTURE2D (_MaskTex);SAMPLER(sampler_MaskTex);
            TEXTURE2D (_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);
            TEXTURE2D (_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);


            // #define smp _linear_clampU_mirrorV
            // SAMPLER(smp);

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _MaskTex);

                //scroll UV
                o.uv.x += _Time*_ScrollSpeedU;
                o.uv.y += _Time*_ScrollSpeedV;
                o.uv2.x += _Time*_ScrollSpeedU;
                o.uv2.y += _Time*_ScrollSpeedV;

                half3 viewDir = normalize(_WorldSpaceCameraPos - o.positionWS);
                half dotProduct = 1- saturate(dot(v.normal, viewDir));
                o.rimColor = smoothstep(1-_FresnelWidth, 1.0, dotProduct) * .5f;


                o.screenPos = ComputeScreenPos(v.positionOS);
                return o;
            }

            half4 frag(Varyings i, half face:VFACE) : SV_Target
            {
                half4 c;
                half3 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                //intersection
                //屏幕空间下的UV坐标
                float2 screenUV = i.positionCS.xy/_ScreenParams.xy;
                half depthTex = SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,screenUV).r;
                half intersect = saturate((abs(LinearEyeDepth(depthTex,_ZBufferParams) - i.screenPos.z)) / _IntersectionThreshold);


                //distortion
                float2  opaqueUV = screenUV ;
                half4 opaqueTex = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, opaqueUV);
                //return opaqueTex;
                 i.screenPos.xy += (main.rg*2-1)* _Distort *opaqueTex.xy;
                half3 distortColor = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, opaqueUV);
                distortColor *= _MainColor * _MainColor.a + 1;

                //return half4(distortColor,1);


                //intersect hightlight
                i.rimColor *= intersect* clamp(0,1,face);
                main *= _MainColor * pow(_Fresnel, i.rimColor);

                //                 //lerp distort color & fresnel color
                main = lerp(distortColor, main, i.rimColor.r);
                main += (1-intersect) * (face>0?0.03 : 0.3) * _MainColor * _Fresnel;


                half4 singleCol = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv2).x;
                
                half4 finalCol = singleCol>=_Threshold?half4(main,0.8):half4(main,0);

                return finalCol;
            }
            ENDHLSL
        }
    }


}
