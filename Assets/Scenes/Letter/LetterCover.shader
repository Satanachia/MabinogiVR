Shader "Custom/LetterCover" {
   Properties {
      _MainTex1 ("Albedo (RGB)", 2D) = "white" {}
      _MainTex2 ("Albedo (RGB)", 2D) = "white" {}

   }
   SubShader {
      Tags { "RenderType"="Opaque" }
      cull back
	  Lighting Off

      //1st Pass
      CGPROGRAM
      // Physically based Standard lighting model, and enable shadows on all light types
      #pragma surface surf Test noshadow noambient

      sampler2D _MainTex1;

      struct Input {
         float2 uv_MainTex1;
      };


      void surf (Input IN, inout SurfaceOutput o) {

         fixed4 c = tex2D (_MainTex1, IN.uv_MainTex1) ;
         o.Albedo = c.rgb;
         o.Alpha = c.a;
      }
	  float4 LightingTest(SurfaceOutput s, float3 lightDir, float atten)
	  {

		  float4 final;
		  final.rgb =s.Albedo ;
		  final.a = s.Alpha;
		  return final;
	  }

	  
      ENDCG

      cull front
	  Lighting Off
      //2nd Pass 
      CGPROGRAM
      // Physically based Standard lighting model, and enable shadows on all light types
      #pragma surface surf Test noshadow noambient

      sampler2D _MainTex2;

      struct Input {
         float2 uv_MainTex2;
      };


      void surf (Input IN, inout SurfaceOutput o) {

         fixed4 c = tex2D (_MainTex2, float2(1-IN.uv_MainTex2.x,IN.uv_MainTex2.y)) ;
         o.Albedo = c.rgb;
         o.Alpha = c.a;
      }
	  
	  float4 LightingTest(SurfaceOutput s, float3 lightDir, float atten)
	  {
		  float4 final;
		  final.rgb = s.Albedo;
		  final.a = s.Alpha;
		  return final;
	  }
      ENDCG
   }
   FallBack "Diffuse"
}