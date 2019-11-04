Shader "Animated/AnimatedTexture"
{
    Properties
    {
	// use this to change transparency
	// albedo stuff
		_Color("Tex Color",Color) = (1,1,1,1)		
		// maintex
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		// maintex scroll speed
		_Speed ("Speed", Range(0, 10)) = 1
		// maintex scroll direction,direction is setup like a circle where 1 and 0 move the texture in the same direction, values inbetween move in different directions.
		_Direction ("Direction", Range(0, 1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		// big shiny
        _Metallic ("Metallic", Range(0,1)) = 0.0
		
		// displacement map stuff
		// height map
        _ParallaxMap ("Height Map", 2D) = "black" {}
		// height map speed
		_ParallaxSpeed ("Parallax Speed", Range(0, 10)) = 1
		// height map direcition, 
		_ParallaxDirection ("Parallax Direction", Range(0, 1)) = 0
		_ParallaxStrength ("Parallax Strength", Range(0, 1)) = 1

		// emission stuff
		// emission colour
		[HDR]
		_EmissionColor("Emission Color",Color) = (0,0,0)	
		// emission map
        _EmissionMap ("Emision", 2D) = "white" {}			
		// height map speed
		_EmissionSpeed ("Emission Speed", Range(0, 10)) = 1
		// height map direcition, 
		_EmissionDirection ("Emission Direction", Range(0, 1)) = 0
		_EmissionStrength ("Emission Strength", Range(0, 1)) = 1
		//_Cutoff("CutOff",Range(0,1)) = 0
		
    }
    SubShader
    {
	// render transparency
        Tags {"RenderType"="Opaque" "PerformanceChecks"="False"}
        LOD 300

		Blend SrcAlpha OneMinusSrcAlpha
		
		
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0
		#pragma _PARRALAXMAP
		#pragma _EMISSION
        sampler2D _MainTex;

        struct Input
        {
		// input uv
            float2 uv_MainTex;
			float2 uv_ParallaxMap;
			float2 uv_EmissionMap;
        };

		// var for all properties
        half _Glossiness;
        half _Metallic;
		// albedo float
		float _Speed;
		float _Direction;
		float _Parallax;
		// sampler2D
		sampler2D _ParallaxMap;
		sampler2D _EmissionMap;
		// displacement float
		float _ParallaxSpeed;
		float _ParallaxDirection;
		float _ParallaxStrength;
		// emission float
		float _EmissionSpeed;
		float _EmissionDirection;
		float _EmissionStrength;
		// colours
		float4 _Color;
		float4 _EmissionColor;
	//	float _Cutoff
        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
		// change the direction the texture offset moves , use sin and cos to make it circular where 0 and 1 move to the right and the values inbetween determine different directions
		//				.25
		//				|
		//		  .5 --     -- 1 & 0
		//				|
		//				.75
			float2 direction = float2(cos(_Direction * UNITY_PI * 2), sin(_Direction * UNITY_PI * 2));
			float2 parallaxDirection = float2(cos(_ParallaxDirection * UNITY_PI * 2), sin(_ParallaxDirection * UNITY_PI * 2));
			float2 emissionDirection = float2(cos(_EmissionDirection * UNITY_PI * 2), sin(_EmissionDirection * UNITY_PI * 2));
			// the maintex uv += the parallax map movemet and direction 
			//scroll the parallax and set it's speed, direction and strength because unity is dumb and sets the heightscale too low'
			IN.uv_MainTex += tex2D(_ParallaxMap, IN.uv_ParallaxMap + parallaxDirection * _Time.x * _ParallaxSpeed) * _ParallaxStrength;
			// emission stuff
			IN.uv_MainTex += tex2D(_EmissionMap, IN.uv_EmissionMap + emissionDirection * _Time.x * _EmissionSpeed) * _EmissionStrength;
			IN.uv_EmissionMap += tex2D(_EmissionMap, IN.uv_EmissionMap + emissionDirection * _Time.x * _EmissionSpeed) * _EmissionStrength;
			// c = the maintexture and uv 
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex + direction * _Time.x * _Speed )* _Color;
			// albedo rgb
            o.Albedo = c.rgb;
			// apply metallic
            o.Metallic = _Metallic;
			// apply glossiness
            o.Smoothness = _Glossiness;

			//apply emission
			o.Emission = tex2D(_EmissionMap, IN.uv_MainTex).rgb * _EmissionColor.rgb;
			// apply alpha
            o.Alpha = c.a;
			
			//if (c.b > _Cutoff) c.a = 0;
			//c.rgb *= c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
