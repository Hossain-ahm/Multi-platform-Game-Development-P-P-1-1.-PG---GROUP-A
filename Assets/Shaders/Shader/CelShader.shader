Shader "Hidden/CelShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;   // added normal
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1; // pass normal to fragment
                float3 worldPos : TEXCOORD2; // pass world position to fragment
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.normalWS = normalize(TransformObjectToWorldNormal(v.normalOS));
                o.worldPos = TransformObjectToWorld(v.positionOS.xyz);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;

                // Sample scene color
                float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb;

                // === CEL SHADING ===
                float lum = dot(color, float3(0.2126, 0.7152, 0.0722));
                if (lum > 0.9)      color *= 1.0;
                else if(lum > 0.4) color *= 0.5;
                else if(lum > 0.1) color *= 0.2;
                else                color *= 0.1;
                return float4(color, 1);
            }
            ENDHLSL
        }
    }
}
