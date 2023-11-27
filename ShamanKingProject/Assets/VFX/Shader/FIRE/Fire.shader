// Made with Amplify Shader Editor v1.9.1.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fire"
{
	Properties
	{
		[Toggle]_Float0("開啟深度測試", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float1("火焰溶解", Range( 0 , 2)) = 0
		[HDR]_Color0("描邊顏色", Color) = (0,0,0,1)
		_float2("內焰範圍", Range( 0 , 2)) = 0
		[HDR]_Color1("外焰顏色", Color) = (0,0,0,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite [_Float0]
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _Float0;
			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform float _float2;
			uniform float _Float1;
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float4 tex2DNode2 = tex2D( _TextureSample0, uv_TextureSample0 );
				float4 lerpResult10 = lerp( _Color0 , _Color1 , step( ( _float2 + _Float1 ) , tex2DNode2.r ));
				float4 appendResult7 = (float4(lerpResult10.rgb , step( _Float1 , tex2DNode2.r )));
				
				
				finalColor = appendResult7;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19109
Node;AmplifyShaderEditor.SamplerNode;2;-891.0569,9.442139;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1;-120.0569,-439.5578;Inherit;False;Property;_Float0;開啟深度測試;0;1;[Toggle];Create;False;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-724.0569,-600.5579;Inherit;False;Property;_Color0;描邊顏色;3;1;[HDR];Create;False;0;0;0;False;0;False;0,0,0,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-769.0569,-225.5579;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-722.6343,-414.2224;Inherit;False;Property;_Color1;外焰顏色;5;1;[HDR];Create;False;0;0;0;False;0;False;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1054.057,-287.5579;Inherit;False;Property;_float2;內焰範圍;4;0;Create;False;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1040.057,-159.5579;Inherit;False;Property;_Float1;火焰溶解;2;0;Create;False;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;45.9431,-194.5579;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;10;-141.0569,-254.5579;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;4;-212.0569,22.44214;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;12;-309.5048,-139.7122;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;389,-183;Float;False;True;-1;2;ASEMaterialInspector;100;5;Fire;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;1;True;_Float0;True;3;False;_Float0;True;True;0;False;;0;False;;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-520.6783,-255.3896;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-369.7809,-248.2207;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
WireConnection;8;0;9;0
WireConnection;8;1;5;0
WireConnection;7;0;10;0
WireConnection;7;3;4;0
WireConnection;10;0;6;0
WireConnection;10;1;11;0
WireConnection;10;2;12;0
WireConnection;4;0;5;0
WireConnection;4;1;2;1
WireConnection;12;0;8;0
WireConnection;12;1;2;1
WireConnection;0;0;7;0
WireConnection;14;0;13;0
ASEEND*/
//CHKSM=8F89DD561406CED4082D1941C26BF97B34C9A21A