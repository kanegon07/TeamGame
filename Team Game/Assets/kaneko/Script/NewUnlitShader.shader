Shader "Neko/StencilShadow"
{
	Properties
	{
	}//Properties

	CGINCLUDE
	#include "UnityCG.cginc"
	ENDCG

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
		}

		ZWrite Off

		Pass
		{
			ColorMask 0
			Cull Front
			ZTest GEqual
			Stencil
			{
				Ref 1
				Comp Equal
				Pass IncrSat
				ZFail Zero
			}//Stencil

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 vert( float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos( vertex);
			}

			half4 frag() : SV_Target
			{
				return half4( 1, 0, 0, 1);
			}
			ENDCG
		}//Pass

		Pass
		{
			ColorMask 0
			Cull Back
			ZTest LEqual
			Stencil
			{
				Ref 2
				Comp Equal
				Pass IncrSat
				Fail Zero
				ZFail Zero
			}//Stencil
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 vert( float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos( vertex);
			}

			half4 frag() : SV_Target
			{
				return half4( 0, 1, 0, 1);
			}
			ENDCG
		}//Pass

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha // êÃÇ»Ç™ÇÁÇÃìßñæ 
			ZTest Off
			Cull Back
			Stencil
			{
				Ref 3
				Comp Equal
				Pass Keep
				ZFail Zero
			}//Stencil
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 vert( float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos( vertex);
			}

			half4 frag() : SV_Target
			{
				return float4( 0, 0, 0, 0.5);
			}
			ENDCG
		}//Pass

	}//SubShader
}//Shader