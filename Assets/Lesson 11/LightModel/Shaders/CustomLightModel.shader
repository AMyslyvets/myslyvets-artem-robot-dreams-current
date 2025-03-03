Shader "Custom/AdditionalLights"
{
    Properties
    {
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
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float3 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
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

                /*float4 ndc = OUT.positionCS * 0.5f;
                float2 positionNDC = float2(ndc.x, ndc.y * _ProjectionParams.x) + ndc.w;
                //positionNDC *= OUT.positionCS.w;

                float2 pixelPosition;
            #if UNITY_UV_STARTS_AT_TOP
                pixelPosition = float2(OUT.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - OUT.positionCS.y) : OUT.positionCS.y);
            #else
                pixelPosition = float2(OUT.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - OUT.positionCS.y) : OUT.positionCS.y);
            #endif

                positionNDC = pixelPosition.xy / _ScaledScreenParams.xy;
                positionNDC.y = 1.0f - positionNDC.y;

                positionNDC = positionNDC / OUT.positionCS.w;*/
                
                //float2 screenUV = OUT.screenPos.xy / OUT.screenPos.w;
                //float2 screenUV = positionNDC / _ScreenParams.xy;// * OUT.positionCS.w;

                //OUT.screenPos.xy /= OUT.screenPos.w;

                float4 clipPos = mul(UNITY_MATRIX_MVP, IN.positionOS);  // Clip space
                float3 ndcPos = clipPos.xyz / clipPos.w;          // Normalize to NDC
                float2 screenUV = (ndcPos.xy * 0.5) + 0.5;
                
                /*float2 pixelPosition;
                #if UNITY_UV_STARTS_AT_TOP
                pixelPosition = float2(OUT.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - OUT.positionCS.y) : OUT.positionCS.y);
                #else
                pixelPosition = float2(OUT.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - OUT.positionCS.y) : OUT.positionCS.y);
                #endif

                
                
                float2 positionNDC = pixelPosition.xy / _ScaledScreenParams.xy;
                positionNDC.y = 1.0f - positionNDC.y;
                
                float2 screenUV = positionNDC * 2 - 1;*/
                //screenUV *= OUT.positionCS.w;
                //OUT.uv = TRANSFORM_TEX(screenUV, _LightMask);
                OUT.uv = screenUV * _LightMask_ST.xy;

                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS.xyz);
                
                return OUT;
            }
            
            float3 MyLightingFunction(float3 normalWS, Light light)
            {
                float NdotL = dot(normalWS, normalize(light.direction));
                NdotL = (NdotL + 1) * 0.5;
                return min(saturate(NdotL), light.shadowAttenuation) * light.distanceAttenuation * light.color;
                return saturate(NdotL) * light.color * light.distanceAttenuation * light.shadowAttenuation;
                return saturate(NdotL) * light.color * light.distanceAttenuation * light.shadowAttenuation;
            }

            float ChessboardPattern(float2 screenPos, float squareSize)
            {
                // Scale the screen position by the square size
                float2 gridPos = floor(screenPos / squareSize);

                // XOR between the two coordinates to create a checkerboard pattern
                float pattern = fmod(gridPos.x + gridPos.y, 2.0);
                
                // Return 1 for one color (e.g., white) and 0 for the other color (e.g., black)
                return 1 - pattern;
            }
            
            // This function loops through the lights in the scene
            float3 MyLightLoop(float3 color, InputData inputData)
            {
                float3 lighting = 0;
                
                // Get the main light
                Light mainLight = GetMainLight(inputData.shadowCoord);
                lighting += MyLightingFunction(inputData.normalWS, mainLight);// * MainLightRealtimeShadow(inputData.shadowCoord);
                
                // Get additional lights
                #if defined(_ADDITIONAL_LIGHTS)

                // Additional light loop including directional lights. This block is specific to Forward+.
                #if USE_CLUSTER_LIGHT_LOOP
                UNITY_LOOP for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
                {
                    Light additionalLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
                    lighting += MyLightingFunction(inputData.normalWS, additionalLight);
                }
                #endif
                
                // Additional light loop. The GetAdditionalLightsCount method always returns 0 in Forward+.
                uint pixelLightCount = GetAdditionalLightsCount();
                LIGHT_LOOP_BEGIN(pixelLightCount)
                    Light additionalLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
                    lighting += MyLightingFunction(inputData.normalWS, additionalLight);
                LIGHT_LOOP_END
                
                #endif
                
                return color * lighting;
            }
            half4 frag(Varyings input) : SV_Target0
            {
                // The Forward+ light loop (LIGHT_LOOP_BEGIN) requires the InputData struct to be in its scope.
                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = input.normalWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                
                float3 surfaceColor = float3(1, 1, 1);
                float3 lighting = MyLightLoop(surfaceColor, inputData);

                float lightmap = SAMPLE_TEXTURE2D(_LightMask, sampler_LightMask, (input.screenPos.xy / input.screenPos.w) * _LightMask_ST.xy).r;
                float stepFactor = length(lighting);
                float stepValue = step(lightmap, stepFactor);
                half4 finalColor = half4(stepValue * surfaceColor, 1);
                
                return finalColor;
                //return lightmap;
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
    }
}