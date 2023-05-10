Shader "Custom/UIOutlineCircle"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        [HDR] _OutlineColor ("OutlineColor", Color) = (1, 1, 1, 1)
        _OutlineRange ("OutlineRange", Range(0.0, 1.0)) = 0.1
        [Toggle(NONLIGHTING)] _NonLighting ("NonLighting", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        Cull Off
        ZWrite Off
        blend One OneMinusSrcAlpha

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                #ifndef NONLIGHTING
                half2 lightingUV : TEXCOORD1;
                #endif
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
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _TextureSampleAdd;
            float4 _OutlineColor;
            float _OutlineRange;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;

                #if defined(DEBUG_DISPLAY)
                o.vertexWS = TransformObjectToWorld(v.vertex.xyz);
                #endif

                #ifndef NONLIGHTING
                o.lightingUV = half2(ComputeScreenPos(o.vertex / o.vertex.w).xy);
                #endif

                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            float4 frag (v2f i) : SV_Target
            {
                float2 correction = (i.uv * 2 - 1) * -_OutlineRange;

                float outlineAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + correction).a;
                float4 outCol = float4(_OutlineColor.rgb, outlineAlpha);

                float4 col = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) + _TextureSampleAdd) * i.color;
                col = lerp(outCol, col, col.a);

                #ifndef NONLIGHTING
                float4 mask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                SurfaceData2D surfaceData;
                InputData2D inputData;

                InitializeSurfaceData(col.rgb, col.a, mask, surfaceData);
                InitializeInputData(i.uv, i.lightingUV, inputData);

                col = CombinedShapeLightShared(surfaceData, inputData);
                #endif

                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
    }
}
