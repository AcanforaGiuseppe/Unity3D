Shader "Custom/textureOutlineH"
{
	Properties{
		_Color("MainColor", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_Border("Border 1", Range(0,5)) = 0
		_BorderColor("Color 1", Color) = (1,1,1,1)
		_Border2("Border 2", Range(0,5)) = 0
		_BorderColor2("Color 2", Color) = (1,1,1,1)
	}

	Subshader
	{
		Tags
		{
			"RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" // Compatibility with BIRP
		}

		Pass
		{
			ZWrite Off
			Cull Front

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "HLSLSupport.cginc" // Compatibility with BIRP
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			uniform half4 _BorderColor;
			uniform float _Border;
			uniform float _Border2;

			// Utils
			float map(float value, float min1, float max1, float min2, float max2) {
				return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
			}

			struct Attributes
			{
				float4 vertex : POSITION; // obj space
			};

			struct Varyings
			{
				float4 pos : SV_POSITION; // proj space
			};


			Varyings vert(Attributes IN)
			{
				Varyings OUT;

				// float border = map(sin(_Border * _Time.y * 20), -1, 1, _Border2, _Border);
				float border = _Border;

				float4x4 scaleMatrix = float4x4(
				1+border, 0, 0, 0,
				0, 1+border, 0, 0,
				0, 0, 1+border, 0,
				0, 0, 0, 1
				);
				float4 scaledObjectPos = mul(scaleMatrix, IN.vertex);
				OUT.pos = mul(UNITY_MATRIX_MVP, scaledObjectPos); // Proj space
				return OUT;
			}

			half4 frag(Varyings IN): COLOR
			{
				return _BorderColor;
			}

			ENDHLSL
		}

		Pass
		{
			//ZWrite Off
			Cull Front
			// Blend SrcAlpha OneMinusSrcAlpha
			// BlendOp Add
			
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "HLSLSupport.cginc" // Compatibility with BIRP
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			uniform float _Border;
			uniform half4 _BorderColor2;
			uniform float _Border2;

			// Utils
			float map(float value, float min1, float max1, float min2, float max2) {
				return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
			}

			struct Attributes
			{
				float4 vertex : POSITION; // obj space
			};

			struct Varyings
			{
				float4 pos : SV_POSITION; // proj space
			};


			Varyings vert(Attributes IN)
			{
				Varyings OUT;

				// float border = map(sin(_Border2 * _Time.y * 20), -1, 1, 0.05, _Border2);
				float border = _Border2;

				float4x4 scaleMatrix = float4x4(
				1+border, 0, 0, 0,
				0, 1+border, 0, 0,
				0, 0, 1+border, 0,
				0, 0, 0, 1
				);
				float4 scaledObjectPos = mul(scaleMatrix, IN.vertex);
				OUT.pos = mul(UNITY_MATRIX_MVP, scaledObjectPos); // Proj space
				return OUT;
			}

			half4 frag(Varyings IN): COLOR
			{
				return _BorderColor2;
			}

			ENDHLSL
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			BlendOp Add

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "HLSLSupport.cginc" // Compatibility with BIRP
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			uniform half4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST; // auto fill by unity

			struct Attributes
			{
				float4 vertex : POSITION; // obj space
				float4 texcoord: TEXCOORD0;
			};

			struct Varyings
			{
				float4 pos : SV_POSITION; // proj space
				float4 texcoord: TEXCOORD0;
			};

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); // Proj space
				OUT.texcoord.xy = TRANSFORM_TEX(IN.texcoord, _MainTex).xy;
				return OUT;
			}

			half4 frag(Varyings IN): COLOR
			{
				half4 texColor = tex2D(_MainTex, IN.texcoord);
				return texColor * _Color;
			}

			ENDHLSL
		}		
	}
}
