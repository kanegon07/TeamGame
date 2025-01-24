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

			//radian�̍ő�l�Adegree�Ō�����360�x�̂���
			static const float PI2 = 3.14159 * 2;

			VS_Output vert (VS_Input input) {
				VS_Output output;
				UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
				output.uv = input.uv;
				return output;
			}
		
			//������͈͂�2�����ϐ���0~1�͈̔͂�1�����ϐ��ɕϊ�����
			float random(float2 uv) {
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
			}

			//�����ɓn���ꂽ�Ώۍ��W�̊p�x��0~1�͈̔͂Ɉ��k���ĕԂ��B
			//�����v����ɏ��X�ɒl���傫���Ȃ�B
			float2 getUvAngle(float2 uv) {
				//uv���W���A��ʒ��������_�Ƃ���(0,0)>(1,1)����(-1,-1)>(1,1)�͈̔͂ɏC������
				float2 fixUv = uv * 2 - 1;
				//�C������uv���W�̊p�x��radian�l�Ŏ擾
				float angle = atan2(fixUv.y, fixUv.x);
				//0~PI*2�̒l��0~1�͈̔͂Ɉ��k
				return angle / PI2;
			}

			//�����v����ɃO���f�[�V��������0~1�̒l���A�o���o���́A��������x�͂܂Ƃ܂����W�����ɕϊ�����
			float getRadialLine(float angle) {
				//_NoiseScale��傫������قǁA�W�������ׂ����Ȃ�B
				//�����_NoiseScale�Œl��傫������ƁAfloor�Ő؂�̂Ă���v�f�����Ȃ��Ȃ�
				//angle�ׂ̍����l�܂ŘR��Ȃ����random�֐��ɔ��f����邽�߁B
				//random�֐��́A�ǂ�ȋ���Ȓl��0~1�͈̔͂Ɏ��߂邽�߁A�^����l�̑傫���͖��ł͂Ȃ�
				//�ǂ�����ǂ��܂ł�angle�l�𓯒l�Ƃ��Ĉ��������d�v�ɂȂ�
				float2 lineSeed = floor(float2(angle, angle) * _NoiseScale);
				float lineVal = random(lineSeed); ;

				//sin�֐��͂�����l���K���I��-1~1�͈̔͂̔g�`�ɕϊ�����B
				//�����ł͏W�����̏o���p�^�[����ς�����ʂɎg���Ă���B
				//_IsAutoAnim��true�̎��́AUnity�Đ����Ɏ����ŃA�j���[�V���������s���A
				//_IsAutoAnim��false�̎��́A_PatternSeed�𑀍삷�邱�ƂŏW�����̌����ڂ��ω�����
				lineVal = (_IsAutoAnim * sin(lineVal * _Time.w * _AutoAnimSpeed)) + 
							((1 - _IsAutoAnim) * sin(lineVal * _PatternSeed));
				return lineVal;
			}

			//������uv���W���Q�Ƃ��A���S����O���Ɍ�����0����1�ɑJ�ڂ��Ă����l��Ԃ�
			float getCenterCircle(float2 uv) {
				//uv���W���A��ʒ��������_�Ƃ���(0,0)>(1,1)����(-1,-1)>(1,1)�͈̔͂ɏC������
				float2 fixUv = uv * 2 - 1;

				return length(fixUv);
			}

			half4 frag (VS_Output input) : SV_Target {
				half4 col = tex2D(_MainTex, input.uv);

				//�ȉ��̏����͑S��alpha�l���Z�o���邽�߂̏���
				float angle = getUvAngle(input.uv);
				float lineVal = getRadialLine(angle);
				float circle = getCenterCircle(input.uv);
				//���S�܂œ͂��W�����ƁA���S�ɋ߂��ق�0�ɋ߂Â��~���|���Z���āA
				//���S�t�߂͋�ɂȂ�W���������
				float resultLine = saturate(lineVal * circle);
				//smoothstep�ŔZ�����͂��Z���A�������͂�蔖������
				float smoothAlpha = smoothstep(_Edge1, _Edge2, resultLine);

				col = _Color;
				col.a = smoothAlpha;
				return col;
			}
			ENDHLSL
		}
	}
}