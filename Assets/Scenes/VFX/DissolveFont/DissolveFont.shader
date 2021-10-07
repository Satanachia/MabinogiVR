Shader "Custom/DissolveFont" {
	Properties {

		_MainTex ("FontTex", 2D) = "white" {}
		_NoiseTex("NoiseTex",2D) ="white"{}
		_Speed("Speed",Range(0,2)) = 1.2
		_UnityTime("Unity timer",float) = 0

	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha:fade


		sampler2D _MainTex;
		sampler2D _NoiseTex;
		
		float _Speed;
		float _UnityTime;


		struct Input {
			float2 uv_MainTex;
			float2 uv_NoiseTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 noise = tex2D (_NoiseTex,IN.uv_NoiseTex);
			
			o.Albedo = c.rgb;
			
			float alpha;
			
			if(noise.r >= 1-_UnityTime*_Speed)
			alpha = c.a;
			else
			alpha = 0;
			
			o.Alpha = alpha;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
