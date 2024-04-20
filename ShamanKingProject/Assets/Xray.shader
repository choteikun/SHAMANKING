// XRay效果
Shader "Custom/Xray"
{

	Properties
	{
		// 主貼圖
		_MainTex("Base (RGB)", 2D) = "white" {}

	     // 主顏色
	    _XRayColor("XRay Color", Color) = (0, 1, 0, 1)

		// x-ray 強度調整參數
		_Pow("Pow Factor", float) = 1
	}

		SubShader
	{
		// 設置為透明渲染&隊列
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
		LOD 200

		Zwrite Off
		Ztest Always

		// 混合模式
		Blend SrcAlpha One


		CGPROGRAM
		//#pragma surface surf Unlit keepalpha
		#pragma vertex vert
        #pragma fragment frag
        //make fog work
        #pragma multi_compile_fog

        #include "UnityCG.cginc"
            

		// 參數聲明
		sampler2D _MainTex;
		half4 _XRayColor;
		float _Pow;

		struct Input
		{
			float3 viewDir;		//視野方向
			float2 uv_MainTex;
		};

		fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
		 {
			 fixed4 c;
			 c.rgb = s.Albedo;
			 c.a = s.Alpha;
			 return c;
		 }
		 fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);

			// 法線
			float3 worldNormal = WorldNormalVector(IN, o.Normal);

			// 設置顏色
			o.Albedo = _XRayColor.rgb;

			// 把邊沿置為1
			half alpha_XRay = 1.0 - saturate(dot(normalize(IN.viewDir), worldNormal));
			// 調整 X-Ray 的效果
			alpha_XRay = pow(alpha_XRay, _Pow);

			// 前面兩個 alpha 基本為 1，關鍵是 alpha_XRay
			o.Alpha = c.a * _XRayColor.a * alpha_XRay;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
