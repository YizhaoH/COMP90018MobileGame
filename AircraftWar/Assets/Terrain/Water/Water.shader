Shader "Water"
{
    Properties
    {
        [Header(Common)]
        _Water01("Direction(XY) Speed(Z) Distort(W)",vector) = (1,1,0.1,0.05)
        _Water02("Atten(X) Lightness(Y) Caustic(Z)",vector) = (1,1,3,0)
        _Water03("Specular: Distort(X) Intensity(Y) Smoothness(Z)",vector) = (0.8,5,8,0)
        _Water04("FoamRange(X) FoamNoise(Y)",vector) = (5,2.8,0,0)

        [Header(Normal)]
        _NormalTex("NormalTex",2D) = "white"{}

        [Header(Specular)]
        _SpecularColor("Specular Color",color) = (1,1,1,1)

        [Header(Reflection)]
        _ReflectionTex("ReflectionTex",Cube) = "white"{}

        [Header(Caustic)]
        _CausticTex("CausticTex",2D) = "white"{}

        [Header(Foam)]
        _FoamTex("FoamTex", 2D) = "white" {}
        _FoamColor("FoamColor",color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" }
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
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
                float3 normalOS         : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS       : SV_POSITION;
                float4 uv               : TEXCOORD0;        //.xy=foamUV 
                float4 normalUV         : TEXCOORD1; 
                float fogCoord          : TEXCOORD2;
                float3 positionVS       : TEXCOORD3;
                float3 positionWS       : TEXCOORD4;
                float3 normalWS         : TEXCOORD5;
                float2 timeOffset       : TEXCOORD6;    //随着时间流动的速度
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _Water01,_Water02,_Water03,_Water04;
            float4 _NormalTex_ST;
            half4 _SpecularColor;
            float4 _FoamTex_ST;
            half4 _FoamColor;
            float4 _CausticTex_ST;
            CBUFFER_END
            TEXTURE2D (_NormalTex);SAMPLER(sampler_NormalTex);
            TEXTURECUBE (_ReflectionTex);SAMPLER(sampler_ReflectionTex);
            TEXTURE2D (_CausticTex);SAMPLER(sampler_CausticTex);
            TEXTURE2D (_FoamTex);SAMPLER(sampler_FoamTex);

            TEXTURE2D (_RampTexture);SAMPLER(sampler_RampTexture);  //水的颜色渐变图
            TEXTURE2D (_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);
            TEXTURE2D (_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);
            // #define smp _linear_clampU_mirrorV
            // SAMPLER(smp);

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;

                //将顶点从本地空间转换到世界空间
                o.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                //将顶点坐标，从世界空间转换到观察空间
                o.positionVS = TransformWorldToView(o.positionWS);
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);

                o.timeOffset = _Time.y * _Water01.z * _Water01.xy;
                o.uv.xy = o.positionWS.xz * _FoamTex_ST.xy + o.timeOffset;
                o.normalUV.xy = TRANSFORM_TEX(v.uv,_NormalTex) + o.timeOffset ;
                o.normalUV.zw = TRANSFORM_TEX(v.uv,_NormalTex) + o.timeOffset * float2(-1.07,1.13);

                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.fogCoord = ComputeFogFactor(o.positionCS.z);

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                //参数预定义
                float distort = _Water01.w;
                float atten = _Water02.x;
                float lightness = _Water02.y;
                float causticIntensity = _Water02.z;
                float specularDistort = _Water03.x;
                float specularIntensity = _Water03.y;
                float specularSmoothness = _Water03.z;
                float foamRange = _Water04.x;
                float foamNoise = _Water04.y;

                //屏幕空间下的UV坐标
                float2 screenUV = i.positionCS.xy/_ScreenParams.xy;

                //水的深度(平静的水面)
                half depthTex = SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,screenUV).r;
                half depthScene = LinearEyeDepth(depthTex,_ZBufferParams);
                half depthWater = depthScene + i.positionVS.z;
                depthWater *= atten;
                // return depthWater;

                //法线
                half4 normalTex01 = SAMPLE_TEXTURE2D(_NormalTex,sampler_NormalTex,i.normalUV.xy);
                half4 normalTex02 = SAMPLE_TEXTURE2D(_NormalTex,sampler_NormalTex,i.normalUV.zw);
                half4 normalTex = normalTex01*normalTex02;
                half3 normal = normalTex.xyz * distort;

                //水下的扭曲
                float2 distortUV = screenUV + normal.xy;
                half depthDistortTex = SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,distortUV).r;
                half depthDistortScene = LinearEyeDepth(depthDistortTex,_ZBufferParams);
                half depthDistortWater = depthDistortScene + i.positionVS.z;
                float2 opaqueUV = distortUV;
                if(depthDistortWater<0)opaqueUV = screenUV;
                half4 opaqueTex = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, opaqueUV);
                // return opaqueTex;

                //水的焦散
                float4 depthVS = 1;
                depthVS.xy = i.positionVS.xy * depthDistortScene / -i.positionVS.z;
                depthVS.z = depthDistortScene;
                float3 depthWS = mul(unity_CameraToWorld,depthVS).xyz;
                float2 causticUV01 = depthWS.xz * _CausticTex_ST.xy + depthWS.y * 0.2 + i.timeOffset;
                half4 causticTex01 = SAMPLE_TEXTURE2D(_CausticTex,sampler_CausticTex,causticUV01);
                float2 causticUV02 = depthWS.xz * _CausticTex_ST.xy + depthWS.y * 0.1 + i.timeOffset * float2(-1.07,1.43);
                half4 causticTex02 = SAMPLE_TEXTURE2D(_CausticTex,sampler_CausticTex,causticUV02);
                half4 caustic = min(causticTex01,causticTex02);
                caustic *= causticIntensity;

                //水的高光
                //Specular = SpecularColor * Ks * pow(max(0,dot(N,H)), Shininess)
                half3 N = lerp(normalize(i.normalWS),normalTex.xyz,specularDistort);
                Light light = GetMainLight();
                half3 L = light.direction;
                half3 V = normalize(_WorldSpaceCameraPos.xyz - i.positionWS.xyz);
                half3 H = normalize(L+V);
                half4 specular = _SpecularColor * specularIntensity * pow(saturate(dot(N,H)),specularSmoothness);
                // return specular;

                //水的反射
                half3 reflectionUV = reflect(-V,normal);
                half4 reflectionTex = SAMPLE_TEXTURECUBE(_ReflectionTex,sampler_ReflectionTex,reflectionUV);
                half fresnel = pow(1-saturate(dot(i.normalWS,V)),3);
                half4 reflection = reflectionTex * fresnel;
                // return reflection;

                //水面的泡沫
                half foamWidth = depthWater * foamRange;
                half foamTex = SAMPLE_TEXTURE2D(_FoamTex,sampler_FoamTex,i.uv.xy).r;
                foamTex = pow(abs(foamTex),foamNoise);
                half foamMask = step(foamWidth,foamTex);
                half4 foam = foamMask * _FoamColor;

                half4 c = half4(0,0,0,1);

                //水的颜色
                half4 rampTex01 = SAMPLE_TEXTURE2D(_RampTexture, sampler_RampTexture, float2(depthWater,1));
                // half4 waterColor = lerp(_WaterColor01,_WaterColor02,depthWater);
                c += rampTex01 * lightness;
                c += specular * reflection;
                c += foam;

                half4 rampTex02 = SAMPLE_TEXTURE2D(_RampTexture, sampler_RampTexture, float2(depthWater,0));
                c += (opaqueTex + caustic * lightness) * rampTex02;
                
                c.a=0.6;
                return c;
            }
            ENDHLSL
        }
    }
}
