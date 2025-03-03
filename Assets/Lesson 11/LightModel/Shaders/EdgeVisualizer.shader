Shader "Custom/Edges"
{
    Properties
    {
        _Cutoff ("Cutoff", Float) = 0.0
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Cull Off
        ZWrite On
        Pass
        {
            // The LightMode tag matches the ShaderPassName set in UniversalRenderPipeline.cs.
            // The SRPDefaultUnlit pass and passes without the LightMode tag are also rendered by URP
            Name "ForwardLit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            
            HLSLPROGRAM
                        
            #pragma vertex vert
            #pragma fragment frag

            // This multi_compile declaration is required for the Forward rendering path
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            
            // This multi_compile declaration is required for the Forward+ rendering path
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"


            TEXTURE2D(_LightMask);
            SAMPLER(sampler_LightMask);
            float4 _LightMask_ST;
            float _Cutoff;
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float3 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float3 positionWS  : TEXCOORD1;
                float3 normalWS    : TEXCOORD2;
                float4 screenPos    : TEXCOORD3;
            };
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                OUT.screenPos.xy /= OUT.screenPos.w;
                return OUT;
            }
            
            half4 frag(Varyings input) : SV_Target0
            {
                float4 surfaceColor = float4(0.25, 0.25, 0.25, 1.0);

                clip(_Cutoff - length(input.screenPos.xy));
                
                return surfaceColor;
                //return lightmap;
            }
            
            ENDHLSL
        }
    }
}