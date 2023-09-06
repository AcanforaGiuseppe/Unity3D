Shader "Custom/rimLight"
{
     Properties {
		_RimColor("RimColor", Color) = (1,1,1,1) 
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RimValue ("Rim Value", Range(0, 1)) = 0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
 
        CGPROGRAM
        #pragma surface surf Lambert alpha
 
        sampler2D _MainTex;
        fixed _RimValue;
 
		uniform half4 _Color;
		uniform half4 _RimColor;

        struct Input {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldNormal;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _RimColor;
            float3 normal = normalize(IN.worldNormal);
            float3 dir = normalize(IN.viewDir);
            float val = 1 - (abs(dot(dir, normal)));
            float rim = val * val * _RimValue;
            o.Alpha = c.a * rim;
        }
        ENDCG
    }
    FallBack "Diffuse"
}