Shader "Custom/Dissolve"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        [HDR]_Color ("Main Color", Color) = (1, 1, 1, 1)
        _DissolveTex ("Dissolve Tex", 2D) = "white" {}
        [HDR]_DissolveColor ("Dissolve Color", Color) = (1, 1, 1, 1)
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.2
        _DissolveRange ("Dissolve Range", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend One OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float4 _TextureSampleAdd;

            //Dissolveä÷åWÇÃïœêî
            uniform sampler2D _DissolveTex;
            uniform float4 _DissolveTex_ST;
            uniform float4 _DissolveColor;
            uniform float _DissolveAmount;
            uniform float _DissolveRange;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 duv : TEXCOORD1;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color * _Color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) * _MainTex_ST;
                o.duv = TRANSFORM_TEX(v.uv, _DissolveTex) *  _DissolveTex_ST;
                return o;
            }

            float remap (float value, float outMin)
            {
                return value * ((1 - outMin) / 1) + outMin;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = i.color * (tex2D(_MainTex, i.uv) + _TextureSampleAdd);

                //Dissolve
                float DissolveAlpha = tex2D(_DissolveTex, i.duv).r;
                float Amount = remap(_DissolveAmount, -_DissolveRange);

                if (DissolveAlpha < Amount + _DissolveRange)
                {
                    col.rgb += _DissolveColor.rgb;
                }

                if (DissolveAlpha < Amount)
                {
                    col.a = 0;
                }

                col.rgb *= col.a;

                return col;
            }
            ENDHLSL
        }
    }
}
