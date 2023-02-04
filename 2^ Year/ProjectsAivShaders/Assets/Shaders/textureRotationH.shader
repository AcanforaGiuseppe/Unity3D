Shader "Custom/textureRotationH"
{
    Properties{
        _Color("MainColor", Color) = (1,1,1,1) 
        _MainTex("Main texture", 2D) = "white" {}
        _AlfaDegrees("Alpha Degrees", Range(0,100))= 0
        _RotationCenterX("Rotation Center X", Range(0,1))=0
        _RotationCenterY("Rotation Center Y", Range(0,1))=0
    }

    Subshader
    {
        Tags{
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Transparent"
        }
        Pass
        {
            ZWrite Off 

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
                float4 vertex : POSITION;

                float4 texcoord : TEXCOORD0; 
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION; 

                float4 texcoord : TEXCOORD0;
            };

            vertexOutput vert(vertexInput v)
            {
                
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
                //o.texcoord = v.texcoord;
                //o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw; //questo é il calvolo che mi va a modificare la U e la V della texcoord
                                                                                 //cosi funziona il Tiling e Offset della texture dall'inspector
                o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex).xy; //oppure usando questa macro piu semplice
                
                float rad = radians(_AlfaDegrees);
                float seno = sin(rad);
                float coseno = cos(rad);
                float2x2 rotMatrix = float2x2(
                    coseno, -seno,
                    seno, coseno
                );
                o.texcoord.xy -= float2(_RotationCenterX, _RotationCenterY); //per ruotare l'immagino partendo dal centro. Primo lo traslo
                o.texcoord.xy = mul(rotMatrix, o.texcoord.xy); //poi ruoto
                o.texcoord.xy += float2(_RotationCenterX, _RotationCenterY); //e lo traslo al contrario
                
                return o;
            }

            half4 frag(vertexOutput i): COLOR 
            {
                //Auto mipmap index
                half4 texColor = tex2D(_MainTex, i.texcoord);

                //MipMapSampling
                //half4 texColor = tex2Dlod(_MainTex, float4(i.texcoord.xyz, _MipMap));
                return texColor * _Color;
                //lo spostiamo e lo facciamo nel vertex shader

                //Vertex shader sampling
                //return i.texColor * _Color;
            }

            ENDHLSL
        }
    }
}

