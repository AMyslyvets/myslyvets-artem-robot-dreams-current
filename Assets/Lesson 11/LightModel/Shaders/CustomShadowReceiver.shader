Shader "Custom/SimpleShadows"
{
    SubShader
    {

        Tags { "RenderType" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            // This multi_compile declaration is required for the Forward rendering path
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            
            // This multi_compile declaration is required for the Forward+ rendering path
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS  : POSITION;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float4 shadowCoords : TEXCOORD3;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);

                // Get the VertexPositionInputs for the vertex position  
                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS.xyz);

                // Convert the vertex position to a position on the shadow map
                float4 shadowCoordinates = GetShadowCoord(positions);

                // Pass the shadow coordinates to the fragment shader
                OUT.shadowCoords = shadowCoordinates;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Get the value from the shadow map at the shadow coordinates
                half shadowAmount = MainLightRealtimeShadow(IN.shadowCoords);

                #if defined(_ADDITIONAL_LIGHTS)

                // Additional light loop including directional lights. This block is specific to Forward+.
                #if USE_CLUSTER_LIGHT_LOOP
                UNITY_LOOP for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
                {
                    Light additionalLight = GetAdditionalLight(lightIndex, IN.positionWS, half4(1,1,1,1));
                    shadowAmount = min(shadowAmount, additionalLight.shadowAttenuation);
                }
                #endif
                
                // Additional light loop. The GetAdditionalLightsCount method always returns 0 in Forward+.
                uint pixelLightCount = GetAdditionalLightsCount();
                LIGHT_LOOP_BEGIN(pixelLightCount)
                    Light additionalLight = GetAdditionalLight(lightIndex, IN.positionWS, half4(1,1,1,1));
                    shadowAmount = min(shadowAmount, additionalLight.shadowAttenuation);
                LIGHT_LOOP_END
                
                #endif
                
                // Set the fragment color to the shadow value
                return shadowAmount;
            }
            
            ENDHLSL
        }
    }
}