Shader "Custom/MonochromeDissolve"
{
    Properties
    {
        [ParRendererData]_MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _Amount ("Amount", Range(-1.0, 1.0)) = -1.0
        _Range ("Amount", Range(0.0, 1.0)) = 0.0
        [HDR]_MonoColor1 ("Monochrome Color 1", Color) = (0, 0, 0, 1)
        [HDR]_MonoColor2 ("Monochrome Color 2", Color) = (0, 0, 0, 1)
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

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;

            //Dissolve
            uniform sampler2D _DissolveTex;
            uniform float4 _DissolveTex_ST;
            uniform float _Amount;
            uniform float _Range;

            //Monochrome
            uniform float4 _MonoColor1;
            uniform float4 _MonoColor2;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 duv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.duv = TRANSFORM_TEX(v.uv, _DissolveTex);
                return o;
            }

            float3 Monochrome (float3 color)
            {
                return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                float alpha = tex2D(_DissolveTex, i.duv).r;

                float mono = Monochrome(col.rgb);
                //col.rgb = Monochrome(col.rgb);


                if (_Amount > 0)
                {
                    float value = clamp((_Amount) / (alpha), -1, 1);
                    col.rgb = lerp(mono + _MonoColor1, col.rgb, value);
                }
                else
                {
                    float value = clamp((_Amount + 1) / (alpha), -1, 1);
                    col.rgb = lerp(mono + _MonoColor2, mono + _MonoColor1, value);
                }

                return col;
            }
            ENDHLSL
        }
    }
}
