// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/posColorObjectH"
{
	Properties
    {
        _LeftColor("LeftColor", Color) = (1,1,1,1)
        _RightColor("RightColor", Color) = (0,1,0,1)
	}

	Subshader
	{
        Tags
        {
        "RenderPipeline"="UniversalRenderPipeline" "Queue"="Geometry"
        }

		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 3.0

            #include "HLSLSupport.cginc" 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			
            uniform half4 _LeftColor;
            uniform half4 _RightColor;


			struct vertexInput
			{
                float4 vertex : POSITION; // obj space
			};

			struct vertexOutput
			{
                float4 pos : SV_POSITION;
				float4 xRange : DEPTH0;
			};

			vertexOutput vert(vertexInput v)
			{
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP,v.vertex); // Projection space
				o.xRange = smoothstep(-5,5,v.vertex.x);
                return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
                // i.pos - Screenspace
                // return half4(1,0,0,1);
                return _LeftColor * i.xRange + _RightColor * (1 - i.xRange);
			}

			ENDHLSL
		}
	}
}