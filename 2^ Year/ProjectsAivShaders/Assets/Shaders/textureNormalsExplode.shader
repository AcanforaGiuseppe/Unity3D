// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/textureNormalsExplode"
{
	Properties{
		_Color("MainColor", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		//_NormalMap("Normal Map", 2D) = "white" {}
		_Amplitude("Amplitude", Range(0,5)) = 0
		_StartFade("StartFade", Range(1,5)) = 1
		_EndFade("EndFade", Range(1,5)) = 1
	}

	Subshader
	{
		Tags
		{
			"RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" // Compatibility with BIRP
		}

		Pass
		{
			// Alpha Blending
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
			uniform sampler2D _NormalMap;
			uniform float4 _NormalMap_ST; // auto fill by unity
			uniform float _Amplitude;
			uniform float _StartFade;
			uniform float _EndFade;

			struct vertexInput
			{
				float4 vertex : POSITION; // obj space
				float4 texcoord: TEXCOORD0;
				float4 normal: NORMAL;
				float4 tangent: TANGENT;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION; // proj space
				float4 texcoord: TEXCOORD0;
				// float4 normalWorld: TEXCOORD1;
				// float4 tangentWorld: TEXCOORD2;
				// float3 binormalWorld: TEXCOORD3;
				// float4 normalTexCoord: TEXCOORD4;
			};

			float4 normalExplosion(float4 vIn, float4 vNormal)
			{
				vIn.xyz += (vNormal.xyz * _Amplitude);
				return vIn;
			}

			vertexOutput vert(vertexInput v)
			{
				v.vertex = normalExplosion(v.vertex, v.normal);

				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
				// o.normalWorld = float4(TransformObjectToWorldNormal(v.normal.xyz), v.normal.w);
				// o.normalTexCoord.xy = TRANSFORM_TEX(v.texcoord, _NormalMap).xy;
				// o.tangentWorld = float4(normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz)), v.tangent.w);
				// o.binormalWorld = float3(normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w)) * unity_WorldTransformParams.w;
				return o;
			}

			// float3 normalFromColor(float4 color)
			// {
				// 	#if defined(UNITY_NO_DXT5nm) // normal map not compressed
				// 		return color.rgb * 2 - 1;
				// 	#else // normal map compressed
				// 		float3 normalDecompressed = float3(color.a * 2 - 1, color.g * 2 - 1, 0);
				// 		normalDecompressed.z = sqrt(1-dot(normalDecompressed.xy, normalDecompressed.xy));
				// 		return normalDecompressed;
				// 	#endif
			// }

			float map(float value, float min1, float max1, float min2, float max2) {
				return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
			}

			half4 frag(vertexOutput i): COLOR
			{
				// half4 finalColor = half4(1, 1, 1, 1);
				// // half4 texColor = tex2D(_MainTex, i.texcoord);

				// half4 normalColor = tex2D(_NormalMap, i.normalTexCoord);
				// // From normalColor to tangent space normal
				// half3 TSNormal = normalFromColor(normalColor);
				// // Use normalWorld, tangentWorld and binormalWorld to build TBN Matrix
				// float3x3 TBNWorld = float3x3(
				// i.tangentWorld.x, i.binormalWorld.x, i.normalWorld.x,
				// i.tangentWorld.y, i.binormalWorld.y, i.normalWorld.y,
				// i.tangentWorld.z, i.binormalWorld.z, i.normalWorld.z
				// );
				// // From tangentSpaceNormal to worldSpaceNormalAtPixel
				// float3 worldSpaceNormalAtPixel = normalize(mul(TBNWorld, TSNormal));
				// finalColor = float4(worldSpaceNormalAtPixel.rgb, 1);

				// // Debug
				// // R: not used
				// // G: Y
				// // B: not used
				// // A: Y
				// // finalColor = float4(normalColor.aaa, 1);
				// // finalColor = float4(i.normalWorld.rgb, 1);
				// return finalColor;

				half4 finalColor = tex2D(_MainTex, i.texcoord);

				if (_Amplitude >= _StartFade)
				{
					finalColor.a = 1 - map(_Amplitude, _StartFade, _EndFade, 0, 1); 
				}

				if (_Amplitude >= _EndFade)
				{
					finalColor.a = 0;
				}

				return finalColor;
				
			}

			ENDHLSL
		}
	}
}
