Shader "Custom/ScrollShader" {
	Properties {
		_MainTex("Albedo", 2D) = "white" {}
		_ScrollSpeedX ("Scroll Speed X", Range(-1.0, 1.0)) = 1.0
		_ScrollSpeedY ("Scroll Speed Y", Range(-1.0, 1.0)) = 1.0
	}

	SubShader {
		Tags {
			"RenderType"="Transparent"
			"RenderPipeline"="UniversalPipeline"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass {
			Tags {
				"LightMode"="UniversalForward"
			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

			struct VS_Input {
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VS_Output {
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex;
			half _ScrollSpeedX;
			half _ScrollSpeedY;

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			CBUFFER_END

			VS_Output vert (VS_Input input) {
				VS_Output output;
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return output;
			}

			half4 frag (VS_Output input) : SV_Target {
				half x = _ScrollSpeedX * _Time.y + input.uv.x;
				half y = _ScrollSpeedY * _Time.y + input.uv.y;

				return tex2D(_MainTex, float2(x, y));
			}
			ENDHLSL
		}
	}
}