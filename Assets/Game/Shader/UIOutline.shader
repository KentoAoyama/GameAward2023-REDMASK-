Shader "Custom/UIOutline"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        [HDR] _OutlineColor ("OutlineColor", Color) = (1, 1, 1, 1)
        _OutlineRange ("OutlineRange", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        blend One OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _TextureSampleAdd;
            float4 _OutlineColor;
            float _OutlineRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float outlineAlpha = saturate(
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(_MainTex_TexelSize.x * _OutlineRange, 0)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(_MainTex_TexelSize.x * -_OutlineRange, 0)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(0, _MainTex_TexelSize.y * _OutlineRange)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(0, _MainTex_TexelSize.y * -_OutlineRange)).a
                );
                float4 outCol = float4(_OutlineColor.rgb, outlineAlpha);

                float4 col = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) + _TextureSampleAdd) * i.color;
                col = col.a > 0 ? col : outCol;

                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
    }
}
