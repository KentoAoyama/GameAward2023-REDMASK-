Shader "Custom/XDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "while" {}
        _XAmount ("X Amount", Range(0.0, 1.0)) = 0.5
        _XRange ("X Range", Range(0.0, 1.0)) = 0.5
        [HDR]_GlowColor ("Grow Color", Color) =(1, 1, 1, 1)
        _Scroll ("Scroll", Vector) = (0, 0, 0, 0)
        _Distortion ("Distortion", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _TextureSampleAdd;
            float4 _ClipRect;
            float _MaskSoftnessX;
            float _MaskSoftnessY;

            sampler2D _DissolveTex;
            float _DissolveRange;
            float _XAmount;
            float _XRange;
            float3 _GlowColor;
            float4 _Scroll;
            float _Distortion;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float remap(float value, float inMin, float inMax, float outMin, float outMax)
            {
                return(value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                // textureÇÃâ∫Ç0Ç∆ÇµÇƒÅA_YAmountÇ‹Ç≈ï\é¶Ç∑ÇÈ
                _XAmount = remap(_XAmount, 0, 1, -_XRange, 1 + _XRange);
                float fromX= _XAmount - _XRange;

                float alphaX = remap(uv.y, fromX, _XAmount, 0, 1);
                alphaX = saturate(alphaX);

                uv += _Scroll * _Time.x;
                half dissolveTexAlpha = tex2D(_DissolveTex, uv).r;
                half reverseAlphaX = 1 - alphaX;

                half2 uvDiff = half2(0, reverseAlphaX * dissolveTexAlpha * _Distortion);
                float4 color = i.color * (tex2D(_MainTex, i.uv + uvDiff) + _TextureSampleAdd);

                dissolveTexAlpha *= alphaX;
                _DissolveRange *= reverseAlphaX;
                if (dissolveTexAlpha < reverseAlphaX + _DissolveRange)
                {
                    color.rgb += _GlowColor;
                }

                if (dissolveTexAlpha < reverseAlphaX)
                {
                    color.a = 0;
                }

                color.rgb *= color.a;
                return color;
                
            }
            ENDHLSL
        }
    }
}
