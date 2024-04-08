Shader "Unlit/AirWall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) =(1,1,1,1)
        _PlayerPos("PlayerPos",Vector)  = (1,1,1,0)//定義玩家世界座標
        _EdgeRange("EdgeRange",Vector)  = (2,6,0,0)//定義邊緣漸變範圍
    }
    SubShader
    {
        Tags { "RenderType"="Transparent", "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 vertexWorldPos : SV_POSITION;//定義世界空見下的頂點座標

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _PlayerPos;
            float4 _EdgeRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertexWorldPos = UnityObjectToClipPos(v.vertex)//暫時寫到這裡02:19
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
