﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Renders constant color when ztest fails

Shader "Custom/LookThroughTransparentShader" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1) //The color of our object
        _Tex ("Pattern", 2D) = "white" {} //Optional texture

        _Shininess ("Shininess", Float) = 10 //Shininess
        _AmbientFactor ("Ambient", Float) = 2 //Ambient
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1) //Specular highlights color
    }
    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" } //We're not rendering any transparent objects
        LOD 200 //Level of detail

        Pass {
            Tags { "LightMode" = "ForwardBase" } //For the first light
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc" //Provides us with light data, camera information, etc

                uniform float4 _LightColor0; //From UnityCG

                sampler2D _Tex; //Used for texture
                float4 _Tex_ST; //For tiling

                uniform float4 _Color; //Use the above variables in here
                uniform float4 _SpecColor;
                uniform float _Shininess;
                uniform float _AmbientFactor;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID //Insert

                };

                struct v2f
                {
                    float4 pos : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO //Insert

                };

                v2f vert(appdata v)
                {
                    v2f o;

                    UNITY_SETUP_INSTANCE_ID(v); //Insert
                    UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                    o.posWorld = mul(unity_ObjectToWorld, v.vertex); //Calculate the world position for our point
                    o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz); //Calculate the normal
                    o.pos = UnityObjectToClipPos(v.vertex); //And the position
                    o.uv = TRANSFORM_TEX(v.uv, _Tex);

                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    float3 normalDirection = normalize(i.normal);
                    float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);

                    float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                    float oneOverDistance = 1.0 / length(vert2LightSource);
                    float attenuation = lerp(1.0, oneOverDistance, _WorldSpaceLightPos0.w); //Optimization for spot lights. This isn't needed if you're just getting started.
                    float3 lightDirection = _WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w;

                    float3 ambientLighting = _AmbientFactor * UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb; //Ambient component
                    float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection)); //Diffuse component
                    float3 specularReflection;
                    if (dot(i.normal, lightDirection) < 0.0) //Light on the wrong side - no specular
                    {
                        specularReflection = float3(0.0, 0.0, 0.0);
                	  }
                    else
                    {
                        //Specular component
                        specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
                    }

                    float3 color = (ambientLighting + diffuseReflection) * tex2D(_Tex, i.uv) + specularReflection; //Texture is not applient on specularReflection
                    return float4(color, _Color.a);
                }
            ENDCG
        }
        Pass {
            ZTest Greater
            ZWrite Off  
            Blend One One
            
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                uniform float4 _Color; //Use the above variables in here
                
                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : POSITION;
                    float3 normal : NORMAL;
                };

                v2f vert(appdata v)
                {
                    v2f o;

                    o.normal = normalize(mul(UNITY_MATRIX_IT_MV,float4(v.normal, 0.0)).xyz); //Calculate the normal
                    o.pos = UnityObjectToClipPos(v.vertex); //And the position
                    
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    float intensity = 0.04;
                    return float4(_Color.xyz * intensity, 1.0);
                }
            ENDCG
        }
        Pass {
          Tags { "LightMode" = "ForwardAdd" } //For every additional light
        	Blend One One //Additive blending
            
        	CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag

          #include "UnityCG.cginc" //Provides us with light data, camera information, etc

          uniform float4 _LightColor0; //From UnityCG

          sampler2D _Tex; //Used for texture
          float4 _Tex_ST; //For tiling

          uniform float4 _Color; //Use the above variables in here
          uniform float4 _SpecColor;
          uniform float _Shininess;
          
          struct appdata
          {
              float4 vertex : POSITION;
              float3 normal : NORMAL;
              float2 uv : TEXCOORD0;
          };

          struct v2f
          {
              float4 pos : POSITION;
              float3 normal : NORMAL;
              float2 uv : TEXCOORD0;
              float4 posWorld : TEXCOORD1;
          };

          v2f vert(appdata v)
          {
              v2f o;

              o.posWorld = mul(unity_ObjectToWorld, v.vertex); //Calculate the world position for our point
              o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz); //Calculate the normal
              o.pos = UnityObjectToClipPos(v.vertex); //And the position
              o.uv = TRANSFORM_TEX(v.uv, _Tex);

              return o;
          }

          fixed4 frag(v2f i) : COLOR
          {
              float3 normalDirection = normalize(i.normal);
              float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);

              float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
              float oneOverDistance = 1.0 / length(vert2LightSource);
              float attenuation = lerp(1.0, oneOverDistance, _WorldSpaceLightPos0.w); //Optimization for spot lights. This isn't needed if you're just getting started.
              float3 lightDirection = _WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w;

              float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection)); //Diffuse component
              float3 specularReflection;
              if (dot(i.normal, lightDirection) < 0.0) //Light on the wrong side - no specular
              {
                specularReflection = float3(0.0, 0.0, 0.0);
              }
              else
              {
                  //Specular component
                  specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
              }

              float3 color = (diffuseReflection) * tex2D(_Tex, i.uv) + specularReflection; //No ambient component this time
              return float4(color, _Color.a);
          }
      ENDCG
        }
    }
}