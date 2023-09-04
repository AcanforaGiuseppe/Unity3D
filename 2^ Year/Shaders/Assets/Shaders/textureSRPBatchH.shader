// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/textureSRPBatchH"
{
	Properties
    {
        _Color("MainColor", Color) = (1,1,1,1)
		// [NoScaleOffset]
		// [Normal]
		// [MainTexture]
		_MainTex("Main texture", 2D) = "white" {}
		_MipMap("MipMapIndex", Range(0,5)) = 0
	}

	Subshader
	{
        Tags
        {
        "RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent"
        }

		Pass
		{
			ZWrite Off

			// Alpha Blending
			Blend SrcAlpha OneMinusSrcAlpha
			BlendOp Add

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc" 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			
			uniform sampler2D _MainTex;
			CBUFFER_START(UnityPerMaterial)
            	uniform half4 _Color;
				uniform float4 _MainTex_ST;
				uniform float _MipMap;
			CBUFFER_END

			struct vertexInput
			{
                float4 vertex : POSITION; // obj space
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
                float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				// float4 texColor : COLOR0;
			};

			vertexOutput vert(vertexInput v)
			{
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP,v.vertex); // Projection space
				o.texcoord = v.texcoord;
				//o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				// Vertex Shader sampling
				// o.texColor = tex2Dlod(_MainTex, float4(o.texcoord.xyz, _MipMap));

                return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
				// Auto MipMap index
				half4 texColor = tex2D(_MainTex, i.texcoord);

				// Specify MipMap index
				// half4 texColor = tex2Dlod(_MainTex, float4(i.texcoord.xyz, _MipMap));
				
                 return texColor * _Color;

				// Vertex Shader sampling
                //  return i.texColor * _Color;
			}

			ENDHLSL
		}
	}
}