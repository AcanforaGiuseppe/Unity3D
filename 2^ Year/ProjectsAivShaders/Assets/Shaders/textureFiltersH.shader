Shader "Custom/textureFiltersH"
{
    Properties{
        _Color("MainColor", Color) = (1,1,1,1) 
        _MainTex("Main texture", 2D) = "white" {}
        //Enum apparteneva a Unity se voglio il mio enum personale uso KeywordEnum
        [KeywordEnum(0_Gradient, 1_Sin, 2_Simmetry_Y, 3_Flip_Y,4_Pixelate,5_ScanEffect)] _Fx("Fx", float) = 0
        _UValue("U Value", Range(0,1)) = 0
        _FrequencyPixelate("FrequencyPixelate", Range(0,1)) = 0
        _Frequency("FrequencySin", Range(0,100)) = 0
        _FrequencyScan("FrequencyScan", Range(0,1)) = 0
        _Phase("Phase", Range(0,100)) = 0
        _Amplitude("Amplitude", Range(0,1)) = 0

    }

    Subshader
    {
        Tags{
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Transparent"
        }
        Pass
        {
            ZWrite Off 

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

            uniform float _Fx;
            uniform float _UValue;
            uniform float _Frequency;
            uniform float _FrequencyScan;
            uniform float _FrequencyPixelate;
            uniform float _Phase;
            uniform float _Amplitude;

            struct vertexInput
            {
                float4 vertex : POSITION;

                float4 texcoord : TEXCOORD0; 
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION;

                float4 texcoord : TEXCOORD0;
            };

            vertexOutput vert(vertexInput v)
            {
                
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw; 

                return o;
            }

            half4 frag(vertexOutput i): COLOR 
            {
                if(_Fx == 0){

                  half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                  return texColor * (i.texcoord.x*_UValue);

                }else if(_Fx == 1){
                  i.texcoord.x += sin(i.texcoord.y*_Frequency+_Phase)*_Amplitude;
                  //i.texcoord.y = i.texcoord.y;
                  half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                  return texColor;

                }else if(_Fx == 2){
                    if(i.texcoord.x > _Phase){
                        i.texcoord.x = 1 - i.texcoord.x;
                        i.texcoord.y = i.texcoord.y;
                        half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                        
                        return texColor;
                    }else{
                        half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                        
                        return texColor;
                    }
                }else if(_Fx == 3){
                    if(i.texcoord.x > _Phase/2){
                        i.texcoord.x = 1 - i.texcoord.x;
                        i.texcoord.y = i.texcoord.y;
                        half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                        
                        return texColor;
                    }else{
                        half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                        
                        return texColor;
                    }
                }else if(_Fx == 4){

                        i.texcoord.xy /= _FrequencyPixelate;
                        i.texcoord.xy = round(i.texcoord.xy);
                        i.texcoord.xy *= _FrequencyPixelate;
                        half4 texColor = tex2D(_MainTex,i.texcoord.xy);
                        return texColor;

                }else if(_Fx == 5){
                        float save = i.texcoord.y;
                        save /= _FrequencyScan;
                        save = round(save);

                        int rest = fmod(save, 2);

                        float2 UV;
                        if(rest==0){
                            //Odd           
                            UV = float2(i.texcoord.x+_Amplitude, i.texcoord.y);
                        }else{
                            //Even
                            UV = float2(i.texcoord.x-_Amplitude, i.texcoord.y);
                        }
                        half4 texColor = tex2D(_MainTex, UV);
                        return texColor;
                }

                float4 Uvalue = float4(i.texcoord.xy,0,_UValue);
                return Uvalue;
            }

            ENDHLSL
        }
    }
}

