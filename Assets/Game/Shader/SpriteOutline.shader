Shader "Unlit/SpriteOutline"
{
    Properties
    {
        [ParRendererData]_MainTex ("Texture", 2D) = "white" {}
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineRange;

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
                float alpha = saturate(
                    tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x * _OutlineRange, 0)).a +
                    tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x * -_OutlineRange, 0)).a +
                    tex2D(_MainTex, i.uv + float2(0, _MainTex_TexelSize.y * _OutlineRange)).a +
                    tex2D(_MainTex, i.uv + float2(0, _MainTex_TexelSize.y * -_OutlineRange)).a
                    );
                float4 outCol = float4(_OutlineColor.rgb, alpha);
                float4 col = tex2D(_MainTex, i.uv);
                col = col.a > 0 ? col : outCol;
                

                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
    }
}
