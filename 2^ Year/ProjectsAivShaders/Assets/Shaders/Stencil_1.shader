Shader "Custom/Stencil_1"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags {"RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Geometry" "RenderType"="Opaque"}

       
       pass
       {

            Stencil
            {
                Ref 1
                Comp equal
                pass Keep
            }

            
            HLSLPROGRAM
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            UNITY_INSTANCING_BUFFER_START(Props)
            uniform half4 _Color;
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            UNITY_INSTANCING_BUFFER_END(Props)

            struct VertexInput
            {
                float4 vertex : POSITION0; //objspace
                float4 textcoord : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float4 textcoord : TEXCOORD0;
            };

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;

                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); //projection space

                o.textcoord.xy = TRANSFORM_TEX(v.textcoord, _MainTex);

                return o;
            }

            half4 frag(VertexOutput i) : COLOR
            {
                half4 _UVColor = tex2D(_MainTex, i.textcoord);

                return _Color;
            }

            ENDHLSL
       }

    }
}
