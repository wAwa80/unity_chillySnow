Shader "Hidden/Internal-CubemapToEquirect" {
	Properties {
		_MainTex ("Texture", Cube) = "" {}
	}
	SubShader {
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 43648
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  tmpvar_1.w = 1.0;
					  tmpvar_1.xyz = _glesVertex.xyz;
					  gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_1));
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp samplerCube _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  highp vec2 tmpvar_1;
					  tmpvar_1 = ((xlv_TEXCOORD0 * vec2(6.283185, 3.141593)) + vec2(-3.141593, -1.570796));
					  highp vec2 tmpvar_2;
					  tmpvar_2 = cos(tmpvar_1);
					  highp vec2 tmpvar_3;
					  tmpvar_3 = sin(tmpvar_1);
					  highp vec3 tmpvar_4;
					  tmpvar_4.x = (tmpvar_3.x * tmpvar_2.y);
					  tmpvar_4.y = tmpvar_3.y;
					  tmpvar_4.z = (tmpvar_2.x * tmpvar_2.y);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = textureCube (_MainTex, tmpvar_4);
					  gl_FragData[0] = tmpvar_5;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp samplerCube _MainTex;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec3 u_xlat0;
					lowp vec4 u_xlat10_0;
					vec2 u_xlat1;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * vec2(6.28318548, 3.14159274) + vec2(-3.14159274, -1.57079637);
					    u_xlat1.xy = cos(u_xlat0.xy);
					    u_xlat0.xy = sin(u_xlat0.xy);
					    u_xlat0.x = u_xlat0.x * u_xlat1.y;
					    u_xlat0.z = u_xlat1.y * u_xlat1.x;
					    u_xlat10_0 = texture(_MainTex, u_xlat0.xyz);
					    SV_Target0 = u_xlat10_0;
					    return;
					}
					
					#endif"
				}
			}
			Program "fp" {
				SubProgram "gles " {
					"!!!!GLES"
				}
				SubProgram "gles3 " {
					"!!!!GLES3"
				}
			}
		}
	}
}