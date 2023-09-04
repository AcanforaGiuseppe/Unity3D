Shader "Custom/UVColorH"
{
	Properties{
		_Color("MainColor", Color) = (1,1,1,1)
		_MainTex("Main texture", 2D) = "white"{}
	}

	SubShader
	{
		Tags{
			"RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent"
		} 
		Pass
		{
			Zwrite Off
			
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
			

		    struct vertexInput
			{
				float4 vertex : POSITION; //Obj space
				float4  textcoord : TEXCOORD0;
			};
			struct vertexOutput
			{
				float4 pos  : SV_POSITION;
				float4 textcoord : TEXCOORD0;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); //Projection space
				o.textcoord.xy = TRANSFORM_TEX(v.textcoord,_MainTex).xy;
				return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
				half4 texColor = tex2D(_MainTex, i.textcoord);
				
				return float4(i.textcoord.xy,0,1);
			}

			ENDHLSL
		}
	}
}
