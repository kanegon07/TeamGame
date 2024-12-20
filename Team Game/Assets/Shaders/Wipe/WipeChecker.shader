Shader "Unlit/WipeChecker" {
    Properties {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Wipe Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _WipeSize ("Wipe Size", Range(0.0, 1.0)) = 0.0
    }

    SubShader {
        Tags {
            "RenderType"="TransparentCutout"
            "RenderPipeline"="UniversalPipeline"
            "Queue"="AlphaTest"
            "PreviewType"="Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always

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
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _Color;

            float _WipeSize;
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
                // セルのサイズ
                const float cellSizeX = 0.2;
                const float cellSizeY = 0.2;
                _WipeSize *= cellSizeX;

                // 行番号を求めて0or1にする
                float t = floor(input.uv.y / cellSizeY);
                t = fmod(t, 2.0);

                // 奇数行ならX座標をシフトさせて縦じまワイプをかける
                t = fmod(input.uv.x + cellSizeX / 2.0 * t, cellSizeX);

                // ワイプサイズを超えた場合、クリップする
                if (t - _WipeSize > 0) {
                    discard;
                }

                return _Color;
            }
            ENDHLSL
        }
    }
}
