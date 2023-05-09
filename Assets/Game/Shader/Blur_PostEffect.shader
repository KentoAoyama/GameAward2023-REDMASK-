Shader "Hidden/Blur_PostEffect"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _BlurStrength;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float offsetU = _BlurStrength * _MainTex_TexelSize.x;
                float offsetV = _BlurStrength * _MainTex_TexelSize.y;

                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(offsetU, offsetV));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(offsetU, 0.0f));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(offsetU, -offsetV));

                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(0.0f, offsetV));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(0.0f, -offsetV));

                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(-offsetU, offsetV));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(-offsetU, 0.0f));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(-offsetU, -offsetV));

                col /= 9.0f;

                return col;
            }
            ENDHLSL
        }
    }
}
