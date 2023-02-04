Shader "Custom/StencilShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Int) = 2
        _StencilRef ("Stencil Ref", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Pass Stencil Op", Float) = 0
    }

    SubShader
    {
        Tags {"RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Geometry" "RenderType"="Opaque"}

       pass
       {
           Stencil 
           {
               Ref  [_StencilRef]
               Comp [_StencilComp]
               Pass [_StencilOp]
           }
           
           
        Cull [_Culling]
           
           
            HLSLPROGRAM
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            
            uniform half4 _Color;
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            

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

                return _UVColor;
            }

            ENDHLSL
       }

    }
}
