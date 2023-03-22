Shader "Custom/GuideLineDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //dissolve
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0.0, 1.0)) = 0.5
        _DissolveRange ("Dissolve Range", Range(0.0, 1.0)) = 0.15
        [HDR]_DissolveColor ("Dissolve Color", Color) = (1, 1, 1, 1)
        [Space]
        _Scroll ("Scroll", Vector) = (0, 0, 0, 0)
        _ScrollStrength ("Scroll Strength", Range(0.0, 1.0)) = 0
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TextureSampleAdd;

            //Dissolve
            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;
            float _DissolveAmount;
            float _DissolveRange;
            float4 _DissolveColor;

            //Dissolve Scroll
            float4 _Scroll;
            float _ScrollStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //Dissolve Scroll
                float2 ScrollOffset = _Scroll.xy * _ScrollStrength * _Time.x;
                o.duv = TRANSFORM_TEX(v.uv + ScrollOffset, _DissolveTex) * _DissolveTex_ST;
                return o;
            }

            float remap (float value, float inMin, float inMax, float outMin, float outMax)
            {
                return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = i.color * (tex2D(_MainTex, i.uv) + _TextureSampleAdd);

                // Dissolve
                float dissolveAlpha = tex2D(_DissolveTex, i.duv).r;
                _DissolveAmount = remap(_DissolveAmount, 0, 1, -_DissolveRange, 1);
                
                if (dissolveAlpha < _DissolveAmount + _DissolveRange)
                {
                    col.rgb += _DissolveColor;
                }

                if (dissolveAlpha < _DissolveAmount)
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
