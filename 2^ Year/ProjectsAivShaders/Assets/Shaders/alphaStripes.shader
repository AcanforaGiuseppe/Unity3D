Shader "Custom/alphaStripes"
{
      Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Normal ("Normal", 2D) = "normal" {}
      _StripesCutOff ("StripesCutOff", Range(0,1)) = 0
      _Stripes ("Stripes", Range(0,5)) = 0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Off
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
          float2 uv_Normal;
          float3 worldPos;
          float3 objPos;
      };
      sampler2D _MainTex;
      sampler2D _Normal;
      float _StripesCutOff;
      float _Stripes;
      
       void vert (inout appdata_full v, out Input o) {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.objPos = v.vertex;
       }
      
      void surf (Input IN, inout SurfaceOutput o) {
      
          clip (frac((IN.worldPos.y*_Stripes) * _StripesCutOff) - 0.5);
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          o.Normal = UnpackNormal (tex2D (_Normal, IN.uv_Normal));
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }
  