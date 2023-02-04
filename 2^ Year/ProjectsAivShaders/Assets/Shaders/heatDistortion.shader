Shader "Custom/heatDistortion"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Distortion ("Distortion", float) = 1
        _Speed ("Speed", float) = 1
    }

    SubShader
    {
        Tags {"RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent"}

       pass
       {

            HLSLPROGRAM
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            uniform sampler2D _Noise;
            uniform float4 _Noise_ST;
            uniform sampler2D _Mask;
            uniform float4 _Mask_ST;
            uniform sampler2D _CameraOpaqueTexture;
            uniform float _Distortion;
            uniform float _Speed;

            struct VertexInput
            {
                float4 vertex : POSITION0; //objspace
                float4 noiseCoord : TEXCOORD0;
                float4 maskCoord : TEXCOORD1;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float4 noiseCoord : TEXCOORD0;
                float4 maskCoord : TEXCOORD1;
                float4 grabCoord : TEXCOORD2;
            };

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;

                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); //projection space

                o.noiseCoord.xy = TRANSFORM_TEX(v.noiseCoord, _Noise);

                o.maskCoord.xy = TRANSFORM_TEX(v.maskCoord, _Mask);

                o.grabCoord = ComputeScreenPos(o.pos);

                float noise = tex2Dlod(_Noise, float4(v.noiseCoord.xyz, 0)).rgb;
                float mask = tex2Dlod(_Mask, float4(v.maskCoord.xyz, 0)).rgb;

                //o.grabCoord.x += cos(noise * _Time.y * _Speed) * mask * _Distortion;
                //o.grabCoord.y += sin(noise * _Time.y * _Speed) * mask * _Distortion;
                o.grabCoord.x += sin(noise * _Time.y * _Speed) * mask * _Distortion;

                //o.textcoord.xy = TRANSFORM_TEX(v.textcoord, _MainTex);

                return o;
            }

            half4 frag(VertexOutput i) : COLOR
            {
                //half4 _UVColor = tex2D(_MainTex, i.textcoord);

                half4 grabColor = tex2Dproj(_CameraOpaqueTexture, i.grabCoord);

                return grabColor;
            }

            ENDHLSL
       }

    }
}
