Shader "Sandman/Flask/GhostFlaskWithSandman"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Animation("Animation", Range(0, 1)) = 0.0
		_AlphaAddition("Alpha Addition", Range(0, 1)) = 0.0
		_FogSpeed("Fog Speed", Range(0, 1)) = 0.0

		_Multiplier("Multiplier", float) = 1
		_NormalColor("Normal Color", Color) = (1,1,1,1)
		_BlockedColor("Blocked Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite off
        
		Pass
		{
		//normal
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};


			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};
			
		    sampler2D _MainTex;
			float4 _NormalColor;
			float _Animation;
			float _AlphaAddition;
			float _Multiplier;	
			float _FogSpeed;
            float4 _MainTex_ST;


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float alpha = 1 - dot(i.normal, i.viewDir);
				float alphaFactor = tex2D(_MainTex, i.uv + _Time.gg * _FogSpeed).b + _AlphaAddition;
				_NormalColor.a = alpha * alphaFactor * _Multiplier * _Animation;
				return _NormalColor;
			}
						
			ENDCG
		}
	}
}
