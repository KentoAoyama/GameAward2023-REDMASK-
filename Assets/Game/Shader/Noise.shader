Shader "Custom/Noise"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]_BufferTex ("Buffer", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            sampler2D _GlitchTex;
            sampler2D _BufferTex;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);

                // ノイズ
                float4 glitch = tex2D(_GlitchTex, i.uv);

                //閾値
                float thresh = 1.001 - _Intensity * 1.001;
                float w_d = step(thresh, pow(glitch.z, 2.5));
                float w_b = step(thresh, pow(glitch.w, 2.5));
                float w_c = step(thresh, pow(glitch.z, 3.5));

                float2 uv = i.uv + glitch.xy * w_d;

                // バッファのテクスチャと現在の色を線形補完している
                float3 color = lerp(col, tex2D(_MainTex, uv), w_b).rgb;

                color = lerp(color, color - col.bbg * 2 + color.grr * 2, w_c);

                return float4(color, col.a);
            }
            ENDHLSL
        }
    }
}
