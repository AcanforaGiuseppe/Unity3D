Shader "Custom/textureStencil"
{
    Properties{
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1"}
        LOD 100
 
        ColorMask 0
 
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            struct appdata{
                float4 vertex : POSITION;
            };
 
            struct v2f{
                float4 vertex : SV_POSITION;
            };
 
            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target{
                fixed4 col = 1.0;
                return col;
            }
            ENDCG
        }
    }

	SubShader
    {
        Tags { "Queue"="Geometry-2" "RenderType" = "Opaque"}
		LOD 300

        ColorMask 0
		ZWrite On
        Pass{}
    }
}