Shader "Doctrina/Ocean Water" {	
    Properties {
      _Color ("Color", Color) = (1,1,1,1)
      _SSS ("SSS Color", Color) = (1,1,1,1)
      _Cube ("Reflection Cubemap", Cube) = "_Skybox" { }
      _MainTex ("Texture", 2D) = "white" {}
      _BumpMap ("Normalmap", 2D) = "bump" {}
      _MicroBumpMap ("Micro Normalmap", 2D) = "bump" {}

      _FPOW("FPOW", Float) = 5.0
	  _R0("R0", Float) = 0.05
	  _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    }

    SubShader {
      
      Tags { "RenderType" = "Opaque" "DisableBatching" = "True" }
      CGPROGRAM
      #pragma surface surf SSSSpecular vertex:vert 

      struct Input {          
          half2 worldUV;
          float3 viewDir;
          float Height;
          float3 worldRefl;
          INTERNAL_DATA
      };

      struct SurfaceOutputCustom {
        fixed3 Albedo;
        fixed3 Normal;
        fixed3 Emission;
        half Specular;
        fixed Gloss;
        fixed Alpha;
        fixed Height;
      };

      sampler2D _MainTex; 
      sampler2D _BumpMap;
      sampler2D _MicroBumpMap;
      samplerCUBE _Cube;

      float4 _Color; 
      float4 _SSS; 
      float4 _ReflectColor;

      float _FPOW;
	  float _R0;

	  half4 LightingSSSSpecular (SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten) {
	          
        half3 h = normalize (lightDir + viewDir);
        half diff = max (0, dot (s.Normal, lightDir));

        float nh = max (0, dot (s.Normal, h));
        float spec = pow (nh, 48.0);

        half NdotL = 1 - dot (s.Normal, lightDir );
        NdotL = pow( NdotL, 3 );

        half4 c;
        c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten + NdotL *_SSS * s.Height;
        c.a = s.Alpha;
        return c;

     }

	  inline fixed3 combineNormalMaps (fixed3 base, fixed3 detail) {
    	base += fixed3(0, 0, 1);
    	detail *= fixed3(-1, -1, 1);
    	return base * dot(base, detail) / base.z - detail;
	  }

      void vert (inout appdata_full v, out Input o) {

          UNITY_INITIALIZE_OUTPUT(Input,o);

          float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;

          float4 d = tex2Dlod( _MainTex, float4( worldPos.x*0.08 + _Time.x, worldPos.z * 0.08, 0,0 ));
          float4 d2 = tex2Dlod( _MainTex, float4( worldPos.x*.12, worldPos.z*0.12 + _Time.x, 0,0 ));

          v.vertex.z += (d + d2) * 0.5; 

          float3 nmp = UnpackNormal( tex2Dlod( _BumpMap, float4( worldPos.x*0.08 + _Time.x, worldPos.z * 0.08, 0,0 )) );
          float3 nmp2 = UnpackNormal( tex2Dlod( _BumpMap, float4( worldPos.x*.12, worldPos.z*0.12 + _Time.x, 0,0 )));

          float3 r = combineNormalMaps( nmp, nmp2 );
          v.normal = r;
         
          o.worldUV = float2( worldPos.x, worldPos.z )/2;
          o.Height = clamp( pow( v.vertex.z, 3 ) - 0.4, 0, 1 );
      }
                
      void surf (Input IN, inout SurfaceOutputCustom o) {
      	  
          float3 n1 = UnpackNormal(tex2D(_MicroBumpMap, IN.worldUV + float2( _Time.x * 2,0 )  ));
		  float3 n2 = UnpackNormal(tex2D(_MicroBumpMap, IN.worldUV * 2 + float2( 0,_Time.x * 5 ) ));

		  o.Normal = combineNormalMaps( n1, n2 );
          o.Normal.xy *= 2;

		  //Freshel 
		  half fresnel = saturate( 1.0 - dot(o.Normal, normalize(IN.viewDir)) );
		  fresnel = pow(fresnel, _FPOW);
		  fresnel =  _R0 + (1.0 - _R0) * fresnel;

		  //
		  float3 worldRefl = WorldReflectionVector (IN, o.Normal );
		  fixed4 reflcol = texCUBE (_Cube, o.Normal );

		  o.Albedo = lerp( _Color, reflcol, fresnel );

		  o.Specular = 1;
		  o.Gloss=10;

          o.Height = IN.Height; //pass heightmap to light function for SSS calculation                   
      }
      
      ENDCG
    } 

  }