Shader "Dreamers/CurvedWorld/URP_CurvedLit"
{
    Properties
    {
        _BaseMap("Texture", 2D) = "white" {}
        _BaseColor("Color", Color) = (1,1,1,1)
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _EmissionMap("Emission Map", 2D) = "black" {}
        _EmissionColor("Emission Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _Metallic;
            float _Smoothness;
        CBUFFER_END

        // Global curve uniforms set by CurvedWorldController
        float4 _CurvedWorldOrigin;
        float _CurvedWorldBendX;
        float _CurvedWorldBendY;

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD1;
            float3 normalWS : NORMAL;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);
        TEXTURE2D(_EmissionMap);
        SAMPLER(sampler_EmissionMap);

        Varyings vert(Attributes input)
        {
            Varyings output;
            float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);

            // Subway Surfers Horizon Vertex Curve Calculation
            float distZ = worldPos.z - _CurvedWorldOrigin.z;
            if (distZ > 0.0)
            {
                float distSq = distZ * distZ;
                worldPos.y -= distSq * _CurvedWorldBendY * 0.001;
                worldPos.x += distSq * _CurvedWorldBendX * 0.001;
            }

            output.positionWS = worldPos;
            output.positionCS = TransformWorldToHClip(worldPos);
            output.normalWS = TransformObjectToWorldNormal(input.normalOS);
            output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
            return output;
        }

        float4 frag(Varyings input) : SV_Target
        {
            float4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
            float4 emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, input.uv) * _EmissionColor;

            Light mainLight = GetMainLight();
            float3 lightDir = normalize(mainLight.direction);
            float NdotL = saturate(dot(normalize(input.normalWS), lightDir));
            float3 directLight = mainLight.color * NdotL;
            float3 ambient = float3(0.2, 0.25, 0.35);

            float3 finalColor = albedo.rgb * (directLight + ambient) + emission.rgb;
            return float4(finalColor, albedo.a);
        }
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
