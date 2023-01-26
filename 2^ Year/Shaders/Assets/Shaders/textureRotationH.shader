// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/textureRotationH" {
    Properties {
        _Color("Main Color", Color) = (1, 1, 1, 0)
        _MainTex("Main Texture", 2D) = "white" {}
        _AlfaDegrees("Alpha Degrees", Range(0, 360)) = 0
        _RotationCenterX("Rotation Center X", Range(0, 1)) = 0
        _RotationCenterY("Rotation Center Y",  Range(0, 1)) = 0
    }

    Subshader {
        Tags {
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue"="Transparent"
        }

        Pass
        {
            ZWrite Off

            //AlphaBlending
            Blend SrcAlpha OneMinusSrcAlpha
            BlendOp Add

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc" 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            uniform half4 _Color;
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float _AlfaDegrees;
            uniform float _RotationCenterX;
            uniform float _RotationCenterY;

            struct vertexInput
            {
                float4 vertex : POSITION; //obj space
                float4 texcoord: TEXCOORD0;
            };
            struct vertexOutput
            {
                float4 pos : SV_POSITION; //proj space
                float4 texcoord: TEXCOORD0;
            };

            vertexOutput vert(vertexInput v)
            {
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP ,v.vertex);
                o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                float rad = radians(_AlfaDegrees);
                float seno = sin(rad);
                float coseno = cos(rad);
                float2x2 rotMatrix = float2x2(
                    coseno, -seno,
                    seno, coseno
                );
                o.texcoord.xy -= float2(_RotationCenterX, _RotationCenterY);
                o.texcoord.xy =  mul(rotMatrix, o.texcoord.xy);
                o.texcoord.xy += float2(_RotationCenterX, _RotationCenterY);

                return o;
            }

            half4 frag(vertexOutput i): COLOR
            {
                half4 texColor = tex2D(_MainTex, i.texcoord);
                return texColor * _Color;
            }

            ENDHLSL
        }
    }
}
