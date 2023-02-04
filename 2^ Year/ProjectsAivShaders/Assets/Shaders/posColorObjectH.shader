Shader "Custom/posColorObjectH"
{
    Properties{
        //Non deve per forza chiamarsi _Color basta che sia coerente coi nostri parametri
        _LeftColor("LeftColor", Color) = (1,0,0,1) //(<descrizione>,<tipo di variabile>) = (<valore di default da cui partire)
        //<tipo di variabile> : se é un vector avro i valori di modifica in ispector X Y Z, se é un color avro il Picker del colore
        _RightColor("RightColor", Color) = (0,1,0,1)
    }

    Subshader
    {
        //Tra subshader e pass (poi lo vedremo meglio) per compatibilita con la URP
        //ci deve essere un altro blocco Tags
        Tags{
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Geometry"
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0 //(il 3.0 ci permette se vogliamo di samplare la texture nel fragment shader)


            #include "HLSLSupport.cginc" //Serve per la compatibilita con il CG
            //perche alcune cose che scriveremo come half4 derivano dal CG
            //e questo include ci toglie un po di problemi di compatibilita
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //il secondo include é obbligatorio perche la nostra URP la stiamo installando con il Package Manager
            //quindi non ci sono degli include automatici, cosa che nel CG ce scrivendo uno shader in CG per BIRP 
            //gli include non servono (almeno per le cose semplici)

            uniform half4 _LeftColor; //ora posso usare la proprieta _Color presente nello scoopo ShaderLab
            //sia in vertexshader che in fragment
            uniform half4 _RightColor;
            //vertexInput - vIn
            struct vertexInput
            {
                float4 vertex : POSITION; //ti do a disposizione un float4  che si chiama vertex
                //tu riempimelo con la POSITION in object space
                //vedremo poi perche float4 e non 3 malgrado usiamo XYZ
                
            };
            //vertexOut -v2f (vertex to frag)
            struct vertexOutput //per ogni vertice dato dalla struttura vertexInput io andro a scrivere la loro
            //pos in Project Space
            {
                //qui andra data la posizione dell'oggetto in Project Space
                //posso scrivere vertex ma non avrebbe senso perche la struttura vertexInput
                //mi viene data per ogni vertice passato

                //in vertexOutput non ce il concetto di vertice. ma ce il concetto di posizione
                //perche il vertex di vertexInput viene interpolato all'interno della struttura di
                //vertexOutput e quindi é piu coerente scrivere posizione (pos) in Project Space
                //che poi verra elaborata dal Fragment e sara una pos in Screen Space
                float4 pos : SV_POSITION; //SV_POSITION mi dice la pos in project space

                float xRange : DEPTH0; //DEPTH0 viene usato come semantica per storare un float
                                       //Questo xRange dev'essere il risultato di uno smoothstep all'
                                       //interno del vertex
            };

            vertexOutput vert(vertexInput v)
            {
                //FARA OPERAZIONI con v.vertex e riempira un vertexOutput dando un return
                vertexOutput o;
                o.pos= mul(UNITY_MATRIX_MVP, v.vertex); //moltiplicazione di una matrice per il punto, il punto che mi viene 
                //dato é il v.vertex (obj space)
                //mul(matrice,punto)
                //la matrice che ci serve é la model view projection matrix (serve
                //per fare tutti i passaggi per passare da obj space a project space)
                
                //Quindi o.pos sara in Projection Space

                o.xRange = smoothstep(-5,5,v.vertex.x);
                return o;
            }

            half4 frag(vertexOutput i): COLOR //Color significa che mandero in un ouput una sola cosa alla
            //fine di tutte le operazioni fatte in frag ovvero il colore
            {
                //fragment andando a leggere i.pos mi dara la posizione in Screen Space di tutti i pixel facenti
                //parte di questa mesh
                //i.pos - ScreenSpace

                //il frag ha come input il vertexOutput cioe o
                //return half4(1,0,0,1); //anziche fare un output solo facciamone uno costante
                //usando la proprieta _Color solo che sta nello scoop del ShaderLab e noi
                //dobbiamo passarla nel codice HLSL tramite una uniform


                return _LeftColor *i.xRange + _RightColor*(1-i.xRange);
            }

            ENDHLSL
        }
    }
}

