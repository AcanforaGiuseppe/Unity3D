// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentShaderH"
{
	Properties
    {
        _Color("MainColor", Color) = (1,1,1,1)
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend SRC", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend DST", Float) = 1
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend OP", Float) = 1
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

			// Additive
			// Blend One One
			// BlendOp Add

			// Multiply
			// Blend DstColor Zero
			// BlendOp Add

			// Alpha Blending
			// Blend SrcAlpha OneMinusSrcAlpha
			// BlendOp Add

			// Custom
			Blend [_BlendSrc] [_BlendDst]
			BlendOp [_BlendOp]

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc" 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			
            uniform half4 _Color;

			struct vertexInput
			{
                float4 vertex : POSITION; // obj space
			};

			struct vertexOutput
			{
                float4 pos : SV_POSITION;
			};

			vertexOutput vert(vertexInput v)
			{
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP,v.vertex); // Projection space
                return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
                return _Color;
			}

			ENDHLSL
		}
	}
}