Shader "Custom/WindShader" {
	Properties {
		[HideInInspector] _MainTex("_", 2D) = "white" {}
		[KeywordEnum(Rough, Sharp)] _Line("Line", Int) = 0
		_Color("Color", Color) = (0.0, 0.0, 0.0, 1.0)

		[Space(10)]
		_NoiseScale("NoiseScale", Range(100, 1000)) = 500
		_PatternSeed("PatternSeed", Range(10, 100)) = 1  
		
		[Space(10)]
		_Edge1("Edge1", Range(0, 1)) = 0.5
		_Edge2("Edge2", Range(0, 1)) = 1

		[Space(10)]
		[Toggle] _IsAutoAnim("IsAutoAnim", float) = 0
		_AutoAnimSpeed("AutoAnimSpeed", Range(1, 20)) = 10
	}

	SubShader {
		Tags {
			"RenderType"="Transparent"
			"RenderPipeline"="UniversalPipeline"
			"Queue" = "Transparent"
		}

		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass {
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

			CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _Color;

			float _NoiseScale;
			float _PatternSeed;

			float _Edge1;
			float _Edge2;

			float _IsAutoAnim;
			float _AutoAnimSpeed;
            CBUFFER_END

			//radianの最大値、degreeで言うと360度のこと
			static const float PI2 = 3.14159 * 2;

			VS_Output vert (VS_Input input) {
				VS_Output output;
				UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
				output.uv = input.uv;
				return output;
			}
		
			//あらゆる範囲の2次元変数を0~1の範囲の1次元変数に変換する
			float random(float2 uv) {
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
			}

			//引数に渡された対象座標の角度を0~1の範囲に圧縮して返す。
			//反時計周りに徐々に値が大きくなる。
			float2 getUvAngle(float2 uv) {
				//uv座標を、画面中央を原点として(0,0)>(1,1)から(-1,-1)>(1,1)の範囲に修正する
				float2 fixUv = uv * 2 - 1;
				//修正したuv座標の角度をradian値で取得
				float angle = atan2(fixUv.y, fixUv.x);
				//0~PI*2の値を0~1の範囲に圧縮
				return angle / PI2;
			}

			//半時計周りにグラデーションする0~1の値を、バラバラの、かつある程度はまとまった集中線に変換する
			float getRadialLine(float angle) {
				//_NoiseScaleを大きくするほど、集中線が細かくなる。
				//これは_NoiseScaleで値を大きくすると、floorで切り捨てられる要素が少なくなり
				//angleの細かい値まで漏れなく後のrandom関数に反映されるため。
				//random関数は、どんな巨大な値も0~1の範囲に収めるため、与える値の大きさは問題ではなく
				//どこからどこまでのangle値を同値として扱うかが重要になる
				float2 lineSeed = floor(float2(angle, angle) * _NoiseScale);
				float lineVal = random(lineSeed); ;

				//sin関数はあらゆる値を規則的な-1~1の範囲の波形に変換する。
				//ここでは集中線の出現パターンを変える効果に使っている。
				//_IsAutoAnimがtrueの時は、Unity再生時に自動でアニメーションを実行し、
				//_IsAutoAnimがfalseの時は、_PatternSeedを操作することで集中線の見た目が変化する
				lineVal = (_IsAutoAnim * sin(lineVal * _Time.w * _AutoAnimSpeed)) + 
							((1 - _IsAutoAnim) * sin(lineVal * _PatternSeed));
				return lineVal;
			}

			//引数のuv座標を参照し、中心から外側に向けて0から1に遷移していく値を返す
			float getCenterCircle(float2 uv) {
				//uv座標を、画面中央を原点として(0,0)>(1,1)から(-1,-1)>(1,1)の範囲に修正する
				float2 fixUv = uv * 2 - 1;

				return length(fixUv);
			}

			half4 frag (VS_Output input) : SV_Target {
				half4 col = tex2D(_MainTex, input.uv);

				//以下の処理は全てalpha値を算出するための処理
				float angle = getUvAngle(input.uv);
				float lineVal = getRadialLine(angle);
				float circle = getCenterCircle(input.uv);
				//中心まで届く集中線と、中心に近いほど0に近づく円を掛け算して、
				//中心付近は空になる集中線を作る
				float resultLine = saturate(lineVal * circle);
				//smoothstepで濃い所はより濃く、薄い所はより薄くする
				float smoothAlpha = smoothstep(_Edge1, _Edge2, resultLine);

				col = _Color;
				col.a = smoothAlpha;
				return col;
			}
			ENDHLSL
		}
	}
}