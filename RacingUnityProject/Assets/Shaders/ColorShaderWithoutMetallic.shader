Shader "Sandman/Color/Color Shaded without Metallic" {
	Properties{
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert fullforwardshadows noambient  

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		struct Input {
		float4 color : COLOR;
	};

	float Metallic;

	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = IN.color.rgb;
	}
	ENDCG
	}
		Fallback "Diffuse"
}