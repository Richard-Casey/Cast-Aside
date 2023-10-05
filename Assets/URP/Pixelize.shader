Shader "Custom/Pixelize"
{
    Properties
    {
        _MainTex("Albedo & Alpha", 2D) = "white"
   

    }
    SubShader
    {
        

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        

        struct Attributes {
            float4 positionOS : POSITION;
          
            float2 uv : TEXCOORD0;
        };

        struct Varyings {
            float4 positionHCS : SV_POSITION;
            float depth : DEPTH;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);
        TEXTURE2D(_CameraDepthTexture);
        SAMPLER(sampler_MainTex);

        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        SamplerState sampler_point_clamp;

        uniform float2 _BlockCount;
        uniform float2 _BlockSize;
        uniform float2 _HalfBlockSize;


        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.positionHCS = TransformObjectToHClip(float3(IN.positionOS.xy,0));
            OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
            return OUT;
        }

        ENDHLSL


            
            LOD 100
            ZWrite On
            ZTest Less
            Blend SrcAlpha OneMinusSrcAlpha
           
        Pass{
            Name "Pixelation"

            HLSLPROGRAM



            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 blockPos = floor(IN.uv * _BlockCount);
                float2 blockCenter = blockPos * _BlockSize + _HalfBlockSize;
                
                float4 MainTexOut = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenter);
                
                return MainTexOut;

            }
            ENDHLSL
        }

           
    }
}
