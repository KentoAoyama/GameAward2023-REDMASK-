Shader "Custom/Monochrome"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        [HDR] _MonoColor("MonoColor", Color) = (1, 1, 1, 1)
        [Toggle(NONLIGHTING)]  _NonLighting("NonLighting", Float) = 0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags {"LightMode" = "Universal2D"}
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile _ NONLIGHTING

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
                half2 lightingUV : TEXCOORD1;
                #if defined(DEBUG_DISPLAY)
                float3 vertexWS : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);
            half4 _MainTex_ST;
            half4 _TextureSampleAdd;
            float _MonoBlend;
            float4 _MonoColor;

            #if USE_SHAPE_LIGHT_TYPE_0
            SHAPE_LIGHT(0)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_1
            SHAPE_LIGHT(1)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_2
            SHAPE_LIGHT(2)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_3
            SHAPE_LIGHT(3)
            #endif

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                #if defined(DEBUG_DISPLAY)
                o.vertexWS = TranformObjectToWorld(v.vertex.xyz);
                #endif
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.lightingUV = half2(ComputeScreenPos(o.vertex / o.vertex.w).xy);
                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            float3 Monochrome(float3 color)
            {
                return (color.r * 0.299 + color.g * 0.587 + color.b * 0.114) * _MonoColor;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv)  + _TextureSampleAdd) * i.color;

                float4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
#ifndef NONLIGHTING
                SurfaceData2D surfaceData;
                InputData2D inpuData;

                InitializeSurfaceData(col.rgb, col.a, mask, surfaceData);
                InitializeInputData(i.uv, i.lightingUV, inpuData);

                col = CombinedShapeLightShared(surfaceData, inpuData);
#endif
                col.rgb = lerp(col.rgb, Monochrome(col.rgb), _MonoBlend);

                return col;
            }
            ENDHLSL
        }
    }
}
