#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED
//------------------------------------------------------------------------------------------------------
// Additinal Light Shadows
//------------------------------------------------------------------------------------------------------

#ifndef SHADERGRAPH_PREVIEW
	#if VERSION_GREATER_EQUAL(9, 0)
		#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
		#if (SHADERPASS != SHADERPASS_FORWARD)
			#undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
		#endif
	#else
		#ifndef SHADERPASS_FORWARD
			#undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
		#endif
	#endif
#endif

#pragma multi_compile _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
void MainLightShadows_float (float3 WorldPos, out float ShadowAtten){
	#ifdef SHADERGRAPH_PREVIEW
		ShadowAtten = 1;
	#else
		float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		int additionalLightsCount = GetAdditionalLightsCount();
#if VERSION_GREATER_EQUAL(10, 1)

		//ShadowAtten = MainLightShadow(shadowCoord, WorldPos, half4(1,1,1,1), _MainLightOcclusionProbes);
		ShadowAtten = 0;
		for (int i = 0; i < additionalLightsCount; ++i) {
			Light light = GetAdditionalLight(i, WorldPos, half4(1, 1, 1, 1));
			ShadowAtten += light.shadowAttenuation;
		}

		ShadowAtten = ShadowAtten;
#else
		ShadowAtten = MainLightRealtimeShadow(shadowCoord);
#endif

	#endif
	}

	void MainLightShadows_half (half3 WorldPos, out half ShadowAtten){
	#ifdef SHADERGRAPH_PREVIEW
		ShadowAtten = 1;
	#else
		half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		int additionalLightsCount = GetAdditionalLightsCount();
		#if VERSION_GREATER_EQUAL(10, 1)
			
			//ShadowAtten = MainLightShadow(shadowCoord, WorldPos, half4(1,1,1,1), _MainLightOcclusionProbes);
			ShadowAtten = 0;
			for (int i = 0; i < additionalLightsCount; ++i) {
				Light light = GetAdditionalLight(i, WorldPos, half4(1, 1, 1, 1));
				ShadowAtten += light.shadowAttenuation;
			}

			ShadowAtten = ShadowAtten;
		#else
			ShadowAtten = MainLightRealtimeShadow(shadowCoord);
		#endif


	#endif
}
#endif
