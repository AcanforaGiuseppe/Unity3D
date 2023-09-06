Shader "Custom/textureFadeH"
{
    Properties{
        _Color("MainColor", Color) = (1,1,1,1) 
        _TexA("TexA", 2D) = "white" {}
        _TexB("TexB", 2D) = "white" {}
        _Fade("Fade", Range(0,1)) = 0
    }

    Subshader
    {
        Tags{
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Transparent"
        }
        Pass
        {
            ZWrite off 
            Blend SrcAlpha OneMinusSrcAlpha
            BlendOp Add

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0 


            #include "HLSLSupport.cginc" 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            uniform half4 _Color;  
            uniform sampler2D _TexA;
            uniform sampler2D _TexB;
            uniform float _Fade;
            uniform float4 _TexA_ST;
            uniform float4 _TexB_ST;

            struct vertexInput
            {
                float4 vertex : POSITION; //obj space
                float4 texcoord : TEXCOORD0; 
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION; //proj space
                float4 texcoordA: TEXCOORD0;
                float4 texcoordB : TEXCOOR1;
            };

            vertexOutput vert(vertexInput v)
            {
                
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
                o.texcoordA.xy = TRANSFORM_TEX(v.texcoord, _TexA).xy;
                o.texcoordB.xy = TRANSFORM_TEX(v.texcoord, _TexB).xy;
                return o;
            }

            half4 frag(vertexOutput i): COLOR 
            {
                //Auto mipmap index
                half4 texColorA = tex2D(_TexA, i.texcoordA);
                half4 texColorB = tex2D(_TexB, i.texcoordB);
    
                return _Color * _Fade*texColorA + (1-_Fade)*texColorB;
            }

            ENDHLSL
        }
    }
}

