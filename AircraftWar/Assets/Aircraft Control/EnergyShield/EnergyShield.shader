Shader "taecg/URP/EnergyShield"
{
    Properties
    {
        [Header(Base)]
        _Speed("Speed", float) = 1
        _BaseMap("BaseMap", 2D) = "white" {}
        _FresnelColor("Fresnel Color",Color) = (1,1,1,1)
        [PowerSlider(3)]_FresnelPower("FresnelPower",Range(0,15)) = 5

        [Header(HighLight)]
        _HighLightColor("HighLight Color",Color) = (1,1,1,1)
        _HighLightFade("HighLight Fade",float) = 3

        [Header(Distort)]
        _Tiling("Distort Tiling",float) = 6
        _Distort("Distort Intensity",Range(0,1))=0.4
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            // Blend One One
            Name "Unlit"
            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
                half3 normal            : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS       : SV_POSITION;
                float4 uv           : TEXCOORD0;
                float fogCoord      : TEXCOORD1;
                float3 positionVS   : TEXCOORD2;
                half3 normalWS      : TEXCOORD3;
                half3 viewWS        : TEXCOORD4;
            };

            CBUFFER_START(UnityPerMaterial)
            float _Speed;
            half4 _HighLightColor;
            half _HighLightFade;
            half4 _FresnelColor;
            half _FresnelPower;
            half _Tiling;
            half _Distort;
            float4 _BaseMap_ST;
            CBUFFER_END
            TEXTURE2D (_BaseMap);SAMPLER(sampler_BaseMap);
            TEXTURE2D (_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);
            TEXTURE2D (_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);

            // #define smp _linear_clampU_mirrorV
            // SAMPLER(smp);

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                //将顶点从本地空间转换到世界空间
                float3 positionWS = TransformObjectToWorld(v.positionOS.xyz);
                //将顶点从世界空间转换到观察空间
                o.positionVS = TransformWorldToView(positionWS);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.viewWS = normalize(_WorldSpaceCameraPos - positionWS);
                o.positionCS = TransformWViewToHClip(o.positionVS);
                o.uv.xy = v.uv;
                o.uv.zw = TRANSFORM_TEX(v.uv, _BaseMap);
                o.fogCoord = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 c;

                float2 screenUV = i.positionCS.xy/_ScreenParams.xy;
                half4 depthMap = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV);
                //获取片断对应深度图中的像素在观察空间下的Z值
                half depth = LinearEyeDepth(depthMap.r,_ZBufferParams);
                half4 hightlight = depth + i.positionVS.z;
                hightlight *= _HighLightFade;
                hightlight = 1 - hightlight;
                hightlight *= _HighLightColor;
                hightlight = saturate(hightlight);
                c = hightlight;

                //fresnel外发光
                //pow(max(0,dot(N,V)),Intensity)
                half3 N = i.normalWS;
                half3 V = i.viewWS;
                half NdotV = 1 - saturate(dot(N,V));
                half4 fresnel = pow(abs(NdotV),_FresnelPower);
                fresnel *= _FresnelColor;

                c += fresnel;
                // half4 c;
                // half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv.zw);
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv.zw + float2(0,_Time.y*_Speed));
                c += baseMap * 0.03;

                // half4 baseMap01 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv.zw + float2(0,_Time.y));
                //当前帧的抓屏
                float2 distortUV = lerp(screenUV,baseMap.rr,_Distort);
                half4 opaqueTex = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, distortUV);
                half4 distort = half4(opaqueTex.rgb,1);
                half flowMask = frac(i.uv.y * _Tiling + _Time.y*_Speed);
                //distort *= flowMask;
                c += distort;

                return c;
            }
            ENDHLSL
        }
    }

    // SubShader
    // {
    //     Tags { "Queue"="Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
    //     LOD 100

    //     GrabPass{"_GrabTex"}

    //     Pass
    //     {
    //         Blend SrcAlpha OneMinusSrcAlpha
    //         // Blend One One
    //         Name "Unlit"
    //         CGPROGRAM
            
    //         #pragma vertex vert
    //         #pragma fragment frag
    //         #include "UnityCG.cginc"

    //         struct Attributes
    //         {
    //             float4 positionOS       : POSITION;
    //             float2 uv               : TEXCOORD0;
    //             half3 normal            : NORMAL;
    //         };

    //         struct Varyings
    //         {
    //             float4 positionCS       : SV_POSITION;
    //             float4 uv           : TEXCOORD0;
    //             float fogCoord      : TEXCOORD1;
    //             float3 positionVS   : TEXCOORD2;
    //             half3 normalWS      : TEXCOORD3;
    //             half3 viewWS        : TEXCOORD4;
    //         };

    //         half _Speed;
    //         half4 _HighLightColor;
    //         half _HighLightFade;
    //         half4 _FresnelColor;
    //         half _FresnelPower;
    //         half _Tiling;
    //         half _Distort;
            
    //         sampler2D _BaseMap;float4 _BaseMap_ST;
    //         sampler2D _CameraDepthTexture;
    //         sampler2D _GrabTex;

    //         Varyings vert(Attributes v)
    //         {
    //             Varyings o = (Varyings)0;
    //             //将顶点从本地空间转换到世界空间
    //             float3 positionWS = mul(unity_ObjectToWorld,v.positionOS);
    //             //将顶点从本地空间转换到观察空间
    //             o.positionVS = UnityObjectToViewPos(v.positionOS);
    //             o.normalWS = UnityObjectToWorldNormal(v.normal);
    //             o.viewWS = normalize(_WorldSpaceCameraPos - positionWS);
    //             o.positionCS = mul(UNITY_MATRIX_P,float4(o.positionVS,1));
    //             o.uv.xy = v.uv;
    //             o.uv.zw = TRANSFORM_TEX(v.uv, _BaseMap);
    //             // o.positionCS = UnityObjectToClipPos(v.positionOS);
    //             return o;
    //         }

    //         half4 frag(Varyings i) : SV_Target
    //         {
    //             half4 c;

    //             float2 screenUV = i.positionCS.xy/_ScreenParams.xy;
    //             half4 depthMap = tex2D(_CameraDepthTexture, screenUV);
    //             //获取片断对应深度图中的像素在观察空间下的Z值
    //             half depth = LinearEyeDepth(depthMap.r);
    //             half4 hightlight = depth + i.positionVS.z;
    //             hightlight *= _HighLightFade;
    //             hightlight = 1 - hightlight;
    //             hightlight *= _HighLightColor;
    //             hightlight = saturate(hightlight);
    //             c = hightlight;

    //             //fresnel外发光
    //             //pow(max(0,dot(N,V)),Intensity)
    //             half3 N = i.normalWS;
    //             half3 V = i.viewWS;
    //             half NdotV = 1 - saturate(dot(N,V));
    //             half4 fresnel = pow(abs(NdotV),_FresnelPower);
    //             fresnel *= _FresnelColor;

    //             c += fresnel;
    //             // half4 c;
    //             // half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv.zw);
    //             half4 baseMap = tex2D(_BaseMap, i.uv.zw + float2(0,_Time.y*_Speed));
    //             c += baseMap * 0.03;

    //             // half4 baseMap01 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv.zw + float2(0,_Time.y));
    //             //当前帧的抓屏
    //             float2 distortUV = lerp(screenUV,baseMap.rr,_Distort);
    //             half4 opaqueTex = tex2D(_GrabTex, distortUV);
    //             half4 distort = half4(opaqueTex.rgb,1);
    //             half flowMask = frac(i.uv.y * _Tiling + _Time.y*_Speed);
    //             distort *= flowMask;
    //             c += distort;

    //             return c;
    //         }
    //         ENDCG
    //     }
    // }

}
