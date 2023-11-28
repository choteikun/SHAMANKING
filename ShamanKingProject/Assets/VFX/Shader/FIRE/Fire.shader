// Made with Amplify Shader Editor v1.9.1.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fire"
{
	Properties
	{
		[Toggle]_Float0("開啟深度測試", Float) = 0
		_Float1("火焰溶解", Range( 0 , 1)) = 0
		[HDR]_Color0("描邊顏色", Color) = (0,0,0,1)
		_Float2("內焰範圍", Range( 0 , 0.5)) = 0
		[HDR]_Color1("外焰顏色", Color) = (0,0,0,1)
		_Float3("內內火焰", Range( 0 , 0.5)) = 0
		[HDR]_Color2("最內顏色", Color) = (0,0,0,1)
		_Float4("火焰scale", Float) = 6
		_Float6("火焰scale2", Float) = 3
		_TilingSpeed("Tiling&Speed", Vector) = (2,1,0,-1)
		_TilingSpeed02("Tiling&Speed02", Vector) = (2,1,0,-1)
		_Float8("火焰範圍", Range( 0 , 1)) = 0.5

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
			#include "UnityShaderVariables.cginc"


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
			uniform float4 _Color2;
			uniform float _Float3;
			uniform float _Float2;
			uniform float _Float1;
			uniform float _Float4;
			uniform float4 _TilingSpeed;
			uniform float _Float6;
			uniform float4 _TilingSpeed02;
			uniform float _Float8;
					float2 voronoihash21( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi21( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash21( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash32( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi32( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash32( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			
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
				float temp_output_8_0 = ( _Float2 + _Float1 );
				float mulTime23 = _Time.y * 3.0;
				float time21 = ( mulTime23 * 1.0 );
				float2 voronoiSmoothId21 = 0;
				float2 appendResult31 = (float2(_TilingSpeed.z , _TilingSpeed.w));
				float2 appendResult30 = (float2(_TilingSpeed.x , _TilingSpeed.y));
				float2 texCoord27 = i.ase_texcoord1.xy * appendResult30 + float2( 0,0 );
				float2 panner28 = ( 1.0 * _Time.y * appendResult31 + texCoord27);
				float2 coords21 = panner28 * _Float4;
				float2 id21 = 0;
				float2 uv21 = 0;
				float voroi21 = voronoi21( coords21, time21, id21, uv21, 0, voronoiSmoothId21 );
				float mulTime35 = _Time.y * 3.0;
				float time32 = mulTime35;
				float2 voronoiSmoothId32 = 0;
				float2 appendResult39 = (float2(_TilingSpeed02.z , _TilingSpeed02.w));
				float2 appendResult38 = (float2(_TilingSpeed02.x , _TilingSpeed02.y));
				float2 texCoord41 = i.ase_texcoord1.xy * appendResult38 + float2( 0,0 );
				float2 panner37 = ( 1.0 * _Time.y * appendResult39 + texCoord41);
				float2 coords32 = panner37 * _Float6;
				float2 id32 = 0;
				float2 uv32 = 0;
				float voroi32 = voronoi32( coords32, time32, id32, uv32, 0, voronoiSmoothId32 );
				float blendOpSrc43 = voroi21;
				float blendOpDest43 = voroi32;
				float2 texCoord52 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_45_0 = saturate( ( ( saturate( (( blendOpDest43 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest43 ) * ( 1.0 - blendOpSrc43 ) ) : ( 2.0 * blendOpDest43 * blendOpSrc43 ) ) )) * (1.0 + (distance( float2( 0.5,0.5 ) , texCoord52 ) - 0.0) * (0.0 - 1.0) / (_Float8 - 0.0)) ) );
				float4 lerpResult18 = lerp( _Color1 , _Color2 , step( ( _Float3 + temp_output_8_0 ) , temp_output_45_0 ));
				float4 lerpResult10 = lerp( _Color0 , lerpResult18 , step( temp_output_8_0 , temp_output_45_0 ));
				float4 appendResult7 = (float4(lerpResult10.rgb , step( ( _Float1 * 0.5 ) , temp_output_45_0 )));
				
				
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
Node;AmplifyShaderEditor.VoronoiNode;21;-1120,752;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1312,784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;23;-1504,704;Inherit;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;28;-1328,512;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-1808,480;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-1808,576;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;29;-2016,512;Inherit;False;Property;_TilingSpeed;Tiling&Speed;10;0;Create;True;0;0;0;False;0;False;2,1,0,-1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1600,368;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1600.908,1088.341;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;37;-1326.885,1096.795;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;32;-1090.562,1126.396;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1337.185,1295.965;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;35;-1529.185,1215.965;Inherit;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1513.185,1327.965;Inherit;False;Constant;_Float7;Float 5;9;0;Create;True;0;0;0;False;0;False;0.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1497.185,1455.965;Inherit;False;Property;_Float6;火焰scale2;9;0;Create;False;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-1705.708,1267.103;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-1705.708,1363.103;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;40;-1923.824,1254.595;Inherit;False;Property;_TilingSpeed02;Tiling&Speed02;11;0;Create;True;0;0;0;False;0;False;2,1,0,-1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;43;-828.6041,1007.988;Inherit;True;Overlay;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;848,368;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1200,384;Float;False;True;-1;2;ASEMaterialInspector;100;5;Fire;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;1;True;_Float0;True;3;False;_Float0;True;True;0;False;;0;False;;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.ColorNode;6;128,-416;Inherit;False;Property;_Color0;描邊顏色;3;1;[HDR];Create;False;0;0;0;False;0;False;0,0,0,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;144,-224;Inherit;False;Property;_Color1;外焰顏色;5;1;[HDR];Create;False;0;0;0;False;0;False;0,0,0,1;1.498039,0.2588235,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1;912,32;Inherit;False;Property;_Float0;開啟深度測試;0;1;[Toggle];Create;False;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;144,80;Inherit;False;Property;_Color2;最內顏色;7;1;[HDR];Create;False;0;0;0;False;0;False;0,0,0,1;0.8490566,0.8490566,0.8490566,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;448,128;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;656,176;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-534,246;Inherit;False;Property;_Float3;內內火焰;6;0;Create;False;0;0;0;False;0;False;0;1;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-533,349;Inherit;False;Property;_Float2;內焰範圍;4;0;Create;False;0;0;0;False;0;False;0;0.71;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-553.6398,593.6135;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;69eeab36b534786489fab6a670d6ac8c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;4;221,648;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;17;219.6354,417.8668;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;12;226.3865,529.7335;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-596.2404,449.7089;Inherit;False;Property;_Float1;火焰溶解;2;0;Create;False;0;0;0;False;0;False;0;0.03;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-252,409;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-145.3799,251.4682;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-128.1699,564.7766;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;50;-831.1111,1379.566;Inherit;False;Constant;_Vector0;Vector 0;12;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-880.4016,1563.653;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;51;-561.4018,1390.654;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;53;-264.8123,1458.484;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-74.07013,1171.546;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;340.7724,1249.154;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-598.5246,1706.859;Inherit;False;Property;_Float8;火焰範圍;12;0;Create;False;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1488,816;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1472,944;Inherit;False;Property;_Float4;火焰scale;8;0;Create;False;0;0;0;False;0;False;6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;45;148.0148,873.8624;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;21;0;28;0
WireConnection;21;1;25;0
WireConnection;21;2;24;0
WireConnection;25;0;23;0
WireConnection;25;1;26;0
WireConnection;28;0;27;0
WireConnection;28;2;31;0
WireConnection;30;0;29;1
WireConnection;30;1;29;2
WireConnection;31;0;29;3
WireConnection;31;1;29;4
WireConnection;27;0;30;0
WireConnection;41;0;38;0
WireConnection;37;0;41;0
WireConnection;37;2;39;0
WireConnection;32;0;37;0
WireConnection;32;1;35;0
WireConnection;32;2;34;0
WireConnection;33;0;35;0
WireConnection;33;1;36;0
WireConnection;38;0;40;1
WireConnection;38;1;40;2
WireConnection;39;0;40;3
WireConnection;39;1;40;4
WireConnection;43;0;21;0
WireConnection;43;1;32;0
WireConnection;7;0;10;0
WireConnection;7;3;4;0
WireConnection;0;0;7;0
WireConnection;18;0;11;0
WireConnection;18;1;19;0
WireConnection;18;2;17;0
WireConnection;10;0;6;0
WireConnection;10;1;18;0
WireConnection;10;2;12;0
WireConnection;4;0;47;0
WireConnection;4;1;45;0
WireConnection;17;0;14;0
WireConnection;17;1;45;0
WireConnection;12;0;8;0
WireConnection;12;1;45;0
WireConnection;8;0;9;0
WireConnection;8;1;5;0
WireConnection;14;0;15;0
WireConnection;14;1;8;0
WireConnection;47;0;5;0
WireConnection;51;0;50;0
WireConnection;51;1;52;0
WireConnection;53;0;51;0
WireConnection;53;2;54;0
WireConnection;56;0;43;0
WireConnection;56;1;53;0
WireConnection;45;0;56;0
ASEEND*/
//CHKSM=407B5DD777BAA407CB2E61BC6F69A77EE66AFC83