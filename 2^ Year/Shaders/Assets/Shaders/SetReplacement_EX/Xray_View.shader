Shader "Custom/Xray_View"
{
    Properties
    {    
        _Color("MainColor", Color) = (1,1,1,1)
    }

    Subshader
	{
		Tags{"Queue" = "Geometry"  "RenderType" = "Opaque" "CustomTag"="Floor"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			uniform half4 _MainColor;

			struct vertexInput
			{
				float4 vertex : POSITION;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
				float4 finalColor = float4(0.3,0.3,0.3,1);
				return finalColor;
			}
            ENDCG
		}
	}
    Subshader
    {
        Tags{"Queue" = "Geometry" "RenderType" = "Opaque" "CustomTag"="Friend"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag
			
			uniform half4 _MainColor;

			struct vertexInput
			{
				float4 vertex : POSITION;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
				float4 finalColor = float4(0,1,0,1);
				return finalColor;
			}
            ENDCG
        }
    }
    Subshader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
        ZWrite Off
        
        Tags{"Queue" = "Transparent" "RenderType" = "Opaque" "CustomTag"="Wall"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag
			
			uniform half4 _MainColor;

			struct vertexInput
			{
				float4 vertex : POSITION;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
				float4 finalColor = float4(0.5,0.5,0.5,0.8);
				return finalColor;
			}
            ENDCG
        }
    }
        Subshader
    {
        Tags{"Queue" = "Transparent+100" "RenderType" = "Opaque" "CustomTag"="Enemy"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag
			
			uniform half4 _MainColor;

			struct vertexInput
			{
				float4 vertex : POSITION;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(vertexOutput i): COLOR
			{
				float4 finalColor = float4(1,0,0,1);
				return finalColor;
			}

			ENDCG
        }
    }
}

