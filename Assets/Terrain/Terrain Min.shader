// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TerrainMin" {
	Properties{
		_MainTex("Main Texture",2D) = "white"{}
		_LightTexture("Light Texture",2D) = "white"{}
		[MaterialToggle] _InvertedLight("Inverted Light", Float) = 0
	}
		SubShader{

		Pass
	{
		//Tags { "Queue"="Overlay" }

		//Tags { "Queue" = "Transparent" }
		//ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha


		Tags{ "LightMode" = "ForwardBase" }
		//Blend One One
		//ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

//include useful shader functions
#include "UnityCG.cginc"

#pragma vertex vert
#pragma fragment frag
		//#pragma exclude_renderers flash

	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;

	uniform sampler2D _LightTexture;
	uniform float4 _LightTexture_ST;

	float _InvertedLight;

	struct vertexInput {
		float4 vertex : POSITION;
		//float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;
		float4 uv : TEXCOORD1;
	};

	struct vertexOutput {
		float4 pos : SV_POSITION;
		float2 tex : TEXCOORD0;
		float4 posWorld : TEXCOORD5;
		float4 uv : TEXCOORD1;
		float4 scrPos: TEXCOORD4;
		//float3 normalDir : TEXCOORD2;
	};

	vertexOutput vert(vertexInput v) {
		vertexOutput o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.tex = v.texcoord;
		o.posWorld = mul(unity_WorldToObject, v.vertex);
		o.uv = v.uv;
		o.scrPos = ComputeScreenPos(o.pos);
		return o;
	}

	float4 frag(vertexOutput i) : COLOR
	{
		float2 screenPosition = (i.scrPos.xy / i.scrPos.w);
		float4 light = tex2D(_LightTexture, screenPosition);

		//atten = atten * atten;
		float atten = light.x;
		if (atten > 1)
			atten = 1;
		if (_InvertedLight != 0)
			atten = 1 - atten;

		float4 texMain = tex2D(_MainTex, i.uv);

		float4 tex = /*texDark * (1 - atten) + */texMain;
		tex.a = atten;
		return tex;

	}

		ENDCG
	}

	}
		//FallBack "Diffuse"
}
