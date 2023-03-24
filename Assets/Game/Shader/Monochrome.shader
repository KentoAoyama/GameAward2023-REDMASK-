Shader "Custom/Monochrome"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MonoBlend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float3 Monochrome(float3 color)
            {
                return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv) * i.color;
                float3 MonoCol = Monochrome(col.rgb);

                col.rgb = lerp(col.rgb, MonoCol, _MonoBlend);
                return col;
            }
            ENDHLSL
        }
    }
}
