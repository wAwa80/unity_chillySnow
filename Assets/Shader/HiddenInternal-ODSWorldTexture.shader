Shader "Hidden/Internal-ODSWorldTexture" {
	Properties {
		_MainTex ("", 2D) = "white" {}
		_Cutoff ("", Float) = 0.5
		_Color ("", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Tags { "RenderType" = "Opaque" }
			GpuProgramID 62904
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					varying highp vec4 xlv_TEXCOORD0;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  tmpvar_1.w = 1.0;
					  tmpvar_1.xyz = _glesVertex.xyz;
					  highp vec4 tmpvar_2;
					  tmpvar_2.w = 1.0;
					  tmpvar_2.xyz = (unity_ObjectToWorld * tmpvar_1).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_2);
					  xlv_TEXCOORD0 = (unity_ObjectToWorld * _glesVertex);
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying highp vec4 xlv_TEXCOORD0;
					void main ()
					{
					  gl_FragData[0] = xlv_TEXCOORD0;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					in highp vec4 in_POSITION0;
					out highp vec4 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec3 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1.xyz = u_xlat0.xyz + hlslcc_mtx4x4unity_ObjectToWorld[3].xyz;
					    vs_TEXCOORD0 = hlslcc_mtx4x4unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat0 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = u_xlat0 + hlslcc_mtx4x4unity_MatrixVP[3];
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					in highp vec4 vs_TEXCOORD0;
					layout(location = 0) out highp vec4 SV_Target0;
					void main()
					{
					    SV_Target0 = vs_TEXCOORD0;
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
	SubShader {
		Tags { "RenderType" = "TransparentCutout" }
		Pass {
			Tags { "RenderType" = "TransparentCutout" }
			GpuProgramID 66522
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
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  tmpvar_1.w = 1.0;
					  tmpvar_1.xyz = _glesVertex.xyz;
					  highp vec4 tmpvar_2;
					  tmpvar_2.w = 1.0;
					  tmpvar_2.xyz = (unity_ObjectToWorld * tmpvar_1).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_2);
					  xlv_TEXCOORD0 = (unity_ObjectToWorld * _glesVertex);
					  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					uniform lowp vec4 _Color;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD1).w * _Color.w) - _Cutoff);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD0;
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
					in highp vec4 in_TEXCOORD0;
					out highp vec4 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					vec4 u_xlat0;
					vec3 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1.xyz = u_xlat0.xyz + hlslcc_mtx4x4unity_ObjectToWorld[3].xyz;
					    vs_TEXCOORD0 = hlslcc_mtx4x4unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat0 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = u_xlat0 + hlslcc_mtx4x4unity_MatrixVP[3];
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump float _Cutoff;
					uniform 	mediump vec4 _Color;
					uniform lowp sampler2D _MainTex;
					in highp vec4 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy).w;
					    u_xlat16_1 = u_xlat10_0 * _Color.w + (-_Cutoff);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD0;
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
	SubShader {
		Tags { "RenderType" = "TreeBark" }
		Pass {
			Tags { "RenderType" = "TreeBark" }
			GpuProgramID 157769
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec4 _Time;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _TreeInstanceScale;
					uniform highp vec4 _SquashPlaneNormal;
					uniform highp float _SquashAmount;
					uniform highp vec4 _Wind;
					varying highp vec4 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  tmpvar_2.w = _glesVertex.w;
					  tmpvar_2.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
					  highp vec4 tmpvar_3;
					  tmpvar_3.xy = tmpvar_1.xy;
					  tmpvar_3.zw = _glesMultiTexCoord1.xy;
					  highp vec4 pos_4;
					  pos_4.w = tmpvar_2.w;
					  highp vec3 bend_5;
					  highp float tmpvar_6;
					  tmpvar_6 = (dot (unity_ObjectToWorld[3].xyz, vec3(1.0, 1.0, 1.0)) + tmpvar_3.x);
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = dot (tmpvar_2.xyz, vec3((tmpvar_3.y + tmpvar_6)));
					  tmpvar_7.y = tmpvar_6;
					  highp vec4 tmpvar_8;
					  tmpvar_8 = abs(((
					    fract((((
					      fract(((_Time.yy + tmpvar_7).xxyy * vec4(1.975, 0.793, 0.375, 0.193)))
					     * 2.0) - 1.0) + 0.5))
					   * 2.0) - 1.0));
					  highp vec4 tmpvar_9;
					  tmpvar_9 = ((tmpvar_8 * tmpvar_8) * (3.0 - (2.0 * tmpvar_8)));
					  highp vec2 tmpvar_10;
					  tmpvar_10 = (tmpvar_9.xz + tmpvar_9.yw);
					  bend_5.xz = ((tmpvar_3.y * 0.1) * _glesNormal).xz;
					  bend_5.y = (_glesMultiTexCoord1.y * 0.3);
					  pos_4.xyz = (tmpvar_2.xyz + ((
					    (tmpvar_10.xyx * bend_5)
					   + 
					    ((_Wind.xyz * tmpvar_10.y) * _glesMultiTexCoord1.y)
					  ) * _Wind.w));
					  pos_4.xyz = (pos_4.xyz + (_glesMultiTexCoord1.x * _Wind.xyz));
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = mix ((pos_4.xyz - (
					    (dot (_SquashPlaneNormal.xyz, pos_4.xyz) + _SquashPlaneNormal.w)
					   * _SquashPlaneNormal.xyz)), pos_4.xyz, vec3(_SquashAmount));
					  tmpvar_2 = tmpvar_11;
					  highp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = tmpvar_11.xyz;
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = (unity_ObjectToWorld * tmpvar_12).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_13);
					  xlv_TEXCOORD0 = (unity_ObjectToWorld * tmpvar_11);
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying highp vec4 xlv_TEXCOORD0;
					void main ()
					{
					  gl_FragData[0] = xlv_TEXCOORD0;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _Time;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _TreeInstanceScale;
					uniform 	vec4 _SquashPlaneNormal;
					uniform 	float _SquashAmount;
					uniform 	vec4 _Wind;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in highp vec4 in_TEXCOORD1;
					in mediump vec4 in_COLOR0;
					out highp vec4 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					float u_xlat4;
					float u_xlat8;
					float u_xlat12;
					void main()
					{
					    u_xlat0.x = dot(hlslcc_mtx4x4unity_ObjectToWorld[3].xyz, vec3(1.0, 1.0, 1.0));
					    u_xlat0.y = u_xlat0.x + in_COLOR0.x;
					    u_xlat8 = u_xlat0.y + in_COLOR0.y;
					    u_xlat1.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
					    u_xlat0.x = dot(u_xlat1.xyz, vec3(u_xlat8));
					    u_xlat0 = u_xlat0.xxyy + _Time.yyyy;
					    u_xlat0 = u_xlat0 * vec4(1.97500002, 0.792999983, 0.375, 0.193000004);
					    u_xlat0 = fract(u_xlat0);
					    u_xlat0 = u_xlat0 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
					    u_xlat0 = fract(u_xlat0);
					    u_xlat0 = u_xlat0 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
					    u_xlat2 = abs(u_xlat0) * abs(u_xlat0);
					    u_xlat0 = -abs(u_xlat0) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
					    u_xlat0 = u_xlat0 * u_xlat2;
					    u_xlat0.xy = vec2(u_xlat0.y + u_xlat0.x, u_xlat0.w + u_xlat0.z);
					    u_xlat2.xyz = u_xlat0.yyy * _Wind.xyz;
					    u_xlat2.xyz = u_xlat2.xyz * in_TEXCOORD1.yyy;
					    u_xlat3.y = u_xlat0.y * in_TEXCOORD1.y;
					    u_xlat4 = in_COLOR0.y * 0.100000001;
					    u_xlat3.xz = vec2(u_xlat4) * in_NORMAL0.xz;
					    u_xlat0.z = 0.300000012;
					    u_xlat0.xyz = u_xlat0.xzx * u_xlat3.xyz + u_xlat2.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * _Wind.www + u_xlat1.xyz;
					    u_xlat0.xyz = in_TEXCOORD1.xxx * _Wind.xyz + u_xlat0.xyz;
					    u_xlat12 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
					    u_xlat12 = u_xlat12 + _SquashPlaneNormal.w;
					    u_xlat1.xyz = (-vec3(u_xlat12)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    vs_TEXCOORD0 = u_xlat0;
					    gl_Position = u_xlat1 + hlslcc_mtx4x4unity_MatrixVP[3];
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					in highp vec4 vs_TEXCOORD0;
					layout(location = 0) out highp vec4 SV_Target0;
					void main()
					{
					    SV_Target0 = vs_TEXCOORD0;
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
	SubShader {
		Tags { "RenderType" = "TreeLeaf" }
		Pass {
			Tags { "RenderType" = "TreeLeaf" }
			GpuProgramID 211170
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec4 _Time;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 unity_MatrixInvV;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _TreeInstanceScale;
					uniform highp vec4 _SquashPlaneNormal;
					uniform highp float _SquashAmount;
					uniform highp vec4 _Wind;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  highp mat4 m_1;
					  m_1 = (unity_WorldToObject * unity_MatrixInvV);
					  highp mat4 tmpvar_2;
					  tmpvar_2[0].x = m_1[0].x;
					  tmpvar_2[0].y = m_1[1].x;
					  tmpvar_2[0].z = m_1[2].x;
					  tmpvar_2[0].w = m_1[3].x;
					  tmpvar_2[1].x = m_1[0].y;
					  tmpvar_2[1].y = m_1[1].y;
					  tmpvar_2[1].z = m_1[2].y;
					  tmpvar_2[1].w = m_1[3].y;
					  tmpvar_2[2].x = m_1[0].z;
					  tmpvar_2[2].y = m_1[1].z;
					  tmpvar_2[2].z = m_1[2].z;
					  tmpvar_2[2].w = m_1[3].z;
					  tmpvar_2[3].x = m_1[0].w;
					  tmpvar_2[3].y = m_1[1].w;
					  tmpvar_2[3].z = m_1[2].w;
					  tmpvar_2[3].w = m_1[3].w;
					  highp vec3 tmpvar_3;
					  tmpvar_3 = _glesNormal;
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = _glesColor;
					  highp vec4 tmpvar_5;
					  highp vec4 pos_6;
					  highp float tmpvar_7;
					  tmpvar_7 = (1.0 - abs(_glesTANGENT.w));
					  highp vec4 tmpvar_8;
					  tmpvar_8.w = 0.0;
					  tmpvar_8.xyz = tmpvar_3;
					  highp vec4 tmpvar_9;
					  tmpvar_9.zw = vec2(0.0, 0.0);
					  tmpvar_9.xy = tmpvar_3.xy;
					  pos_6 = (_glesVertex + ((tmpvar_9 * tmpvar_2) * tmpvar_7));
					  tmpvar_5.w = pos_6.w;
					  tmpvar_5.xyz = (pos_6.xyz * _TreeInstanceScale.xyz);
					  highp vec4 tmpvar_10;
					  tmpvar_10.xy = tmpvar_4.xy;
					  tmpvar_10.zw = _glesMultiTexCoord1.xy;
					  highp vec4 pos_11;
					  pos_11.w = tmpvar_5.w;
					  highp vec3 bend_12;
					  highp float tmpvar_13;
					  tmpvar_13 = (dot (unity_ObjectToWorld[3].xyz, vec3(1.0, 1.0, 1.0)) + tmpvar_10.x);
					  highp vec2 tmpvar_14;
					  tmpvar_14.x = dot (tmpvar_5.xyz, vec3((tmpvar_10.y + tmpvar_13)));
					  tmpvar_14.y = tmpvar_13;
					  highp vec4 tmpvar_15;
					  tmpvar_15 = abs(((
					    fract((((
					      fract(((_Time.yy + tmpvar_14).xxyy * vec4(1.975, 0.793, 0.375, 0.193)))
					     * 2.0) - 1.0) + 0.5))
					   * 2.0) - 1.0));
					  highp vec4 tmpvar_16;
					  tmpvar_16 = ((tmpvar_15 * tmpvar_15) * (3.0 - (2.0 * tmpvar_15)));
					  highp vec2 tmpvar_17;
					  tmpvar_17 = (tmpvar_16.xz + tmpvar_16.yw);
					  bend_12.xz = ((tmpvar_10.y * 0.1) * mix (_glesNormal, normalize(
					    (tmpvar_8 * tmpvar_2)
					  ).xyz, vec3(tmpvar_7))).xz;
					  bend_12.y = (_glesMultiTexCoord1.y * 0.3);
					  pos_11.xyz = (tmpvar_5.xyz + ((
					    (tmpvar_17.xyx * bend_12)
					   + 
					    ((_Wind.xyz * tmpvar_17.y) * _glesMultiTexCoord1.y)
					  ) * _Wind.w));
					  pos_11.xyz = (pos_11.xyz + (_glesMultiTexCoord1.x * _Wind.xyz));
					  highp vec4 tmpvar_18;
					  tmpvar_18.w = 1.0;
					  tmpvar_18.xyz = mix ((pos_11.xyz - (
					    (dot (_SquashPlaneNormal.xyz, pos_11.xyz) + _SquashPlaneNormal.w)
					   * _SquashPlaneNormal.xyz)), pos_11.xyz, vec3(_SquashAmount));
					  tmpvar_5 = tmpvar_18;
					  highp vec4 tmpvar_19;
					  tmpvar_19.w = 1.0;
					  tmpvar_19.xyz = tmpvar_18.xyz;
					  highp vec4 tmpvar_20;
					  tmpvar_20.w = 1.0;
					  tmpvar_20.xyz = (unity_ObjectToWorld * tmpvar_19).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_20);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (unity_ObjectToWorld * tmpvar_18);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  mediump float alpha_1;
					  lowp float tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0).w;
					  alpha_1 = tmpvar_2;
					  mediump float x_3;
					  x_3 = (alpha_1 - _Cutoff);
					  if ((x_3 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _Time;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixInvV[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _TreeInstanceScale;
					uniform 	vec4 _SquashPlaneNormal;
					uniform 	float _SquashAmount;
					uniform 	vec4 _Wind;
					in highp vec4 in_POSITION0;
					in highp vec4 in_TANGENT0;
					in highp vec3 in_NORMAL0;
					in highp vec4 in_TEXCOORD0;
					in highp vec4 in_TEXCOORD1;
					in mediump vec4 in_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					float u_xlat12;
					void main()
					{
					    u_xlat0 = hlslcc_mtx4x4unity_WorldToObject[1] * hlslcc_mtx4x4unity_MatrixInvV[1].yyyy;
					    u_xlat0 = hlslcc_mtx4x4unity_WorldToObject[0] * hlslcc_mtx4x4unity_MatrixInvV[1].xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_WorldToObject[2] * hlslcc_mtx4x4unity_MatrixInvV[1].zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_WorldToObject[3] * hlslcc_mtx4x4unity_MatrixInvV[1].wwww + u_xlat0;
					    u_xlat0 = u_xlat0 * in_NORMAL0.yyyy;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[1] * hlslcc_mtx4x4unity_MatrixInvV[0].yyyy;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[0] * hlslcc_mtx4x4unity_MatrixInvV[0].xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[2] * hlslcc_mtx4x4unity_MatrixInvV[0].zzzz + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[3] * hlslcc_mtx4x4unity_MatrixInvV[0].wwww + u_xlat1;
					    u_xlat0 = in_NORMAL0.xxxx * u_xlat1 + u_xlat0;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[1] * hlslcc_mtx4x4unity_MatrixInvV[2].yyyy;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[0] * hlslcc_mtx4x4unity_MatrixInvV[2].xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[2] * hlslcc_mtx4x4unity_MatrixInvV[2].zzzz + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_WorldToObject[3] * hlslcc_mtx4x4unity_MatrixInvV[2].wwww + u_xlat1;
					    u_xlat1 = in_NORMAL0.zzzz * u_xlat1 + u_xlat0;
					    u_xlat12 = dot(u_xlat1, u_xlat1);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat1.xy = u_xlat1.xz * vec2(u_xlat12) + (-in_NORMAL0.xz);
					    u_xlat12 = -abs(in_TANGENT0.w) + 1.0;
					    u_xlat1.xy = vec2(u_xlat12) * u_xlat1.xy + in_NORMAL0.xz;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(u_xlat12) + in_POSITION0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * _TreeInstanceScale.xyz;
					    u_xlat12 = in_COLOR0.y * 0.100000001;
					    u_xlat1.xz = u_xlat1.xy * vec2(u_xlat12);
					    u_xlat12 = dot(hlslcc_mtx4x4unity_ObjectToWorld[3].xyz, vec3(1.0, 1.0, 1.0));
					    u_xlat2.y = u_xlat12 + in_COLOR0.x;
					    u_xlat12 = u_xlat2.y + in_COLOR0.y;
					    u_xlat2.x = dot(u_xlat0.xyz, vec3(u_xlat12));
					    u_xlat2 = u_xlat2.xxyy + _Time.yyyy;
					    u_xlat2 = u_xlat2 * vec4(1.97500002, 0.792999983, 0.375, 0.193000004);
					    u_xlat2 = fract(u_xlat2);
					    u_xlat2 = u_xlat2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
					    u_xlat2 = fract(u_xlat2);
					    u_xlat2 = u_xlat2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
					    u_xlat3 = abs(u_xlat2) * abs(u_xlat2);
					    u_xlat2 = -abs(u_xlat2) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
					    u_xlat2 = u_xlat2 * u_xlat3;
					    u_xlat2.xy = vec2(u_xlat2.y + u_xlat2.x, u_xlat2.w + u_xlat2.z);
					    u_xlat3.xyz = u_xlat2.yyy * _Wind.xyz;
					    u_xlat3.xyz = u_xlat3.xyz * in_TEXCOORD1.yyy;
					    u_xlat1.y = u_xlat2.y * in_TEXCOORD1.y;
					    u_xlat2.z = 0.300000012;
					    u_xlat1.xyz = u_xlat2.xzx * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * _Wind.www + u_xlat0.xyz;
					    u_xlat0.xyz = in_TEXCOORD1.xxx * _Wind.xyz + u_xlat0.xyz;
					    u_xlat12 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
					    u_xlat12 = u_xlat12 + _SquashPlaneNormal.w;
					    u_xlat1.xyz = (-vec3(u_xlat12)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    vs_TEXCOORD1 = u_xlat0;
					    gl_Position = u_xlat1 + hlslcc_mtx4x4unity_MatrixVP[3];
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump float _Cutoff;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 + (-_Cutoff);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD1;
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
	SubShader {
		Tags { "DisableBatching" = "true" "RenderType" = "TreeOpaque" }
		Pass {
			Tags { "DisableBatching" = "true" "RenderType" = "TreeOpaque" }
			GpuProgramID 303200
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _TreeInstanceScale;
					uniform highp mat4 _TerrainEngineBendTree;
					uniform highp vec4 _SquashPlaneNormal;
					uniform highp float _SquashAmount;
					varying highp vec4 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 pos_2;
					  pos_2.w = _glesVertex.w;
					  highp float alpha_3;
					  alpha_3 = tmpvar_1.w;
					  pos_2.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 0.0;
					  tmpvar_4.xyz = pos_2.xyz;
					  pos_2.xyz = mix (pos_2.xyz, (_TerrainEngineBendTree * tmpvar_4).xyz, vec3(alpha_3));
					  highp vec4 tmpvar_5;
					  tmpvar_5.w = 1.0;
					  tmpvar_5.xyz = mix ((pos_2.xyz - (
					    (dot (_SquashPlaneNormal.xyz, pos_2.xyz) + _SquashPlaneNormal.w)
					   * _SquashPlaneNormal.xyz)), pos_2.xyz, vec3(_SquashAmount));
					  pos_2 = tmpvar_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6.w = 1.0;
					  tmpvar_6.xyz = tmpvar_5.xyz;
					  highp vec4 tmpvar_7;
					  tmpvar_7.w = 1.0;
					  tmpvar_7.xyz = (unity_ObjectToWorld * tmpvar_6).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_7);
					  xlv_TEXCOORD0 = (unity_ObjectToWorld * tmpvar_5);
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying highp vec4 xlv_TEXCOORD0;
					void main ()
					{
					  gl_FragData[0] = xlv_TEXCOORD0;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _TreeInstanceScale;
					uniform 	vec4 hlslcc_mtx4x4_TerrainEngineBendTree[4];
					uniform 	vec4 _SquashPlaneNormal;
					uniform 	float _SquashAmount;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					out highp vec4 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
					    u_xlat1.xyz = u_xlat0.yyy * hlslcc_mtx4x4_TerrainEngineBendTree[1].xyz;
					    u_xlat1.xyz = hlslcc_mtx4x4_TerrainEngineBendTree[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    u_xlat1.xyz = hlslcc_mtx4x4_TerrainEngineBendTree[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
					    u_xlat1.xyz = (-in_POSITION0.xyz) * _TreeInstanceScale.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = in_COLOR0.www * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat6 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
					    u_xlat6 = u_xlat6 + _SquashPlaneNormal.w;
					    u_xlat1.xyz = (-vec3(u_xlat6)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    vs_TEXCOORD0 = u_xlat0;
					    gl_Position = u_xlat1 + hlslcc_mtx4x4unity_MatrixVP[3];
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					in highp vec4 vs_TEXCOORD0;
					layout(location = 0) out highp vec4 SV_Target0;
					void main()
					{
					    SV_Target0 = vs_TEXCOORD0;
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
	SubShader {
		Tags { "DisableBatching" = "true" "RenderType" = "TreeTransparentCutout" }
		Pass {
			Tags { "DisableBatching" = "true" "RenderType" = "TreeTransparentCutout" }
			GpuProgramID 330159
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _TreeInstanceScale;
					uniform highp mat4 _TerrainEngineBendTree;
					uniform highp vec4 _SquashPlaneNormal;
					uniform highp float _SquashAmount;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 pos_2;
					  pos_2.w = _glesVertex.w;
					  highp float alpha_3;
					  alpha_3 = tmpvar_1.w;
					  pos_2.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 0.0;
					  tmpvar_4.xyz = pos_2.xyz;
					  pos_2.xyz = mix (pos_2.xyz, (_TerrainEngineBendTree * tmpvar_4).xyz, vec3(alpha_3));
					  highp vec4 tmpvar_5;
					  tmpvar_5.w = 1.0;
					  tmpvar_5.xyz = mix ((pos_2.xyz - (
					    (dot (_SquashPlaneNormal.xyz, pos_2.xyz) + _SquashPlaneNormal.w)
					   * _SquashPlaneNormal.xyz)), pos_2.xyz, vec3(_SquashAmount));
					  pos_2 = tmpvar_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6.w = 1.0;
					  tmpvar_6.xyz = tmpvar_5.xyz;
					  highp vec4 tmpvar_7;
					  tmpvar_7.w = 1.0;
					  tmpvar_7.xyz = (unity_ObjectToWorld * tmpvar_6).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_7);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (unity_ObjectToWorld * tmpvar_5);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  mediump float alpha_1;
					  lowp float tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0).w;
					  alpha_1 = tmpvar_2;
					  mediump float x_3;
					  x_3 = (alpha_1 - _Cutoff);
					  if ((x_3 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _TreeInstanceScale;
					uniform 	vec4 hlslcc_mtx4x4_TerrainEngineBendTree[4];
					uniform 	vec4 _SquashPlaneNormal;
					uniform 	float _SquashAmount;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec4 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
					    u_xlat1.xyz = u_xlat0.yyy * hlslcc_mtx4x4_TerrainEngineBendTree[1].xyz;
					    u_xlat1.xyz = hlslcc_mtx4x4_TerrainEngineBendTree[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    u_xlat1.xyz = hlslcc_mtx4x4_TerrainEngineBendTree[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
					    u_xlat1.xyz = (-in_POSITION0.xyz) * _TreeInstanceScale.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = in_COLOR0.www * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat6 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
					    u_xlat6 = u_xlat6 + _SquashPlaneNormal.w;
					    u_xlat1.xyz = (-vec3(u_xlat6)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    vs_TEXCOORD1 = u_xlat0;
					    gl_Position = u_xlat1 + hlslcc_mtx4x4unity_MatrixVP[3];
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump float _Cutoff;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 + (-_Cutoff);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD1;
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
		Pass {
			Tags { "DisableBatching" = "true" "RenderType" = "TreeTransparentCutout" }
			Cull Front
			GpuProgramID 457754
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _TreeInstanceScale;
					uniform highp mat4 _TerrainEngineBendTree;
					uniform highp vec4 _SquashPlaneNormal;
					uniform highp float _SquashAmount;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 pos_2;
					  pos_2.w = _glesVertex.w;
					  highp float alpha_3;
					  alpha_3 = tmpvar_1.w;
					  pos_2.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 0.0;
					  tmpvar_4.xyz = pos_2.xyz;
					  pos_2.xyz = mix (pos_2.xyz, (_TerrainEngineBendTree * tmpvar_4).xyz, vec3(alpha_3));
					  highp vec4 tmpvar_5;
					  tmpvar_5.w = 1.0;
					  tmpvar_5.xyz = mix ((pos_2.xyz - (
					    (dot (_SquashPlaneNormal.xyz, pos_2.xyz) + _SquashPlaneNormal.w)
					   * _SquashPlaneNormal.xyz)), pos_2.xyz, vec3(_SquashAmount));
					  pos_2 = tmpvar_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6.w = 1.0;
					  tmpvar_6.xyz = tmpvar_5.xyz;
					  highp vec4 tmpvar_7;
					  tmpvar_7.w = 1.0;
					  tmpvar_7.xyz = (unity_ObjectToWorld * tmpvar_6).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_7);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (unity_ObjectToWorld * tmpvar_5);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = (texture2D (_MainTex, xlv_TEXCOORD0).w - _Cutoff);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _TreeInstanceScale;
					uniform 	vec4 hlslcc_mtx4x4_TerrainEngineBendTree[4];
					uniform 	vec4 _SquashPlaneNormal;
					uniform 	float _SquashAmount;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec4 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
					    u_xlat1.xyz = u_xlat0.yyy * hlslcc_mtx4x4_TerrainEngineBendTree[1].xyz;
					    u_xlat1.xyz = hlslcc_mtx4x4_TerrainEngineBendTree[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    u_xlat1.xyz = hlslcc_mtx4x4_TerrainEngineBendTree[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
					    u_xlat1.xyz = (-in_POSITION0.xyz) * _TreeInstanceScale.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = in_COLOR0.www * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat6 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
					    u_xlat6 = u_xlat6 + _SquashPlaneNormal.w;
					    u_xlat1.xyz = (-vec3(u_xlat6)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    vs_TEXCOORD1 = u_xlat0;
					    gl_Position = u_xlat1 + hlslcc_mtx4x4unity_MatrixVP[3];
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump float _Cutoff;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 + (-_Cutoff);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD1;
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
	SubShader {
		Tags { "RenderType" = "TreeBillboard" }
		Pass {
			Tags { "RenderType" = "TreeBillboard" }
			Cull Off
			GpuProgramID 490650
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec3 _TreeBillboardCameraRight;
					uniform highp vec4 _TreeBillboardCameraUp;
					uniform highp vec4 _TreeBillboardCameraFront;
					uniform highp vec4 _TreeBillboardCameraPos;
					uniform highp vec4 _TreeBillboardDistances;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  tmpvar_1 = _glesMultiTexCoord0;
					  highp vec2 tmpvar_2;
					  highp vec4 pos_3;
					  pos_3 = _glesVertex;
					  highp vec2 offset_4;
					  offset_4 = _glesMultiTexCoord1.xy;
					  highp float offsetz_5;
					  offsetz_5 = tmpvar_1.y;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = (_glesVertex.xyz - _TreeBillboardCameraPos.xyz);
					  highp float tmpvar_7;
					  tmpvar_7 = dot (tmpvar_6, tmpvar_6);
					  if ((tmpvar_7 > _TreeBillboardDistances.x)) {
					    offsetz_5 = 0.0;
					    offset_4 = vec2(0.0, 0.0);
					  };
					  pos_3.xyz = (_glesVertex.xyz + (_TreeBillboardCameraRight * offset_4.x));
					  pos_3.xyz = (pos_3.xyz + (_TreeBillboardCameraUp.xyz * mix (offset_4.y, offsetz_5, _TreeBillboardCameraPos.w)));
					  pos_3.xyz = (pos_3.xyz + ((_TreeBillboardCameraFront.xyz * 
					    abs(offset_4.x)
					  ) * _TreeBillboardCameraUp.w));
					  highp vec4 tmpvar_8;
					  tmpvar_8.w = 1.0;
					  tmpvar_8.xyz = pos_3.xyz;
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = (unity_ObjectToWorld * tmpvar_8).xyz;
					  tmpvar_2.x = tmpvar_1.x;
					  tmpvar_2.y = float((_glesMultiTexCoord0.y > 0.0));
					  gl_Position = (unity_MatrixVP * tmpvar_9);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = (unity_ObjectToWorld * pos_3);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = (texture2D (_MainTex, xlv_TEXCOORD0).w - 0.001);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec3 _TreeBillboardCameraRight;
					uniform 	vec4 _TreeBillboardCameraUp;
					uniform 	vec4 _TreeBillboardCameraFront;
					uniform 	vec4 _TreeBillboardCameraPos;
					uniform 	vec4 _TreeBillboardDistances;
					in highp vec4 in_POSITION0;
					in highp vec4 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					float u_xlat4;
					void main()
					{
					    u_xlat0.xyz = in_POSITION0.xyz + (-_TreeBillboardCameraPos.xyz);
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(_TreeBillboardDistances.x<u_xlat0.x);
					#else
					    u_xlatb0 = _TreeBillboardDistances.x<u_xlat0.x;
					#endif
					    u_xlat1.xy = in_TEXCOORD1.xy;
					    u_xlat1.z = in_TEXCOORD0.y;
					    u_xlat0.xyz = (bool(u_xlatb0)) ? vec3(0.0, 0.0, 0.0) : u_xlat1.xyz;
					    u_xlat4 = (-u_xlat0.y) + u_xlat0.z;
					    u_xlat2 = _TreeBillboardCameraPos.w * u_xlat4 + u_xlat0.y;
					    u_xlat1.xyz = vec3(_TreeBillboardCameraRight.x, _TreeBillboardCameraRight.y, _TreeBillboardCameraRight.z) * u_xlat0.xxx + in_POSITION0.xyz;
					    u_xlat0.xzw = abs(u_xlat0.xxx) * _TreeBillboardCameraFront.xyz;
					    u_xlat1.xyz = _TreeBillboardCameraUp.xyz * vec3(u_xlat2) + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xzw * _TreeBillboardCameraUp.www + u_xlat1.xyz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat1.xyz = u_xlat0.xyz + hlslcc_mtx4x4unity_ObjectToWorld[3].xyz;
					    vs_TEXCOORD1 = hlslcc_mtx4x4unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat0 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = u_xlat0 + hlslcc_mtx4x4unity_MatrixVP[3];
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(0.0<in_TEXCOORD0.y);
					#else
					    u_xlatb0 = 0.0<in_TEXCOORD0.y;
					#endif
					    vs_TEXCOORD0.y = u_xlatb0 ? 1.0 : float(0.0);
					    vs_TEXCOORD0.x = in_TEXCOORD0.x;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 + -0.00100000005;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD1;
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
	SubShader {
		Tags { "RenderType" = "GrassBillboard" }
		Pass {
			Tags { "RenderType" = "GrassBillboard" }
			Cull Off
			GpuProgramID 553361
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _WavingTint;
					uniform highp vec4 _WaveAndDistance;
					uniform highp vec4 _CameraPosition;
					uniform highp vec3 _CameraRight;
					uniform highp vec3 _CameraUp;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  tmpvar_1 = _glesMultiTexCoord0;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = _glesColor;
					  highp vec4 pos_3;
					  pos_3 = _glesVertex;
					  highp vec2 offset_4;
					  offset_4 = _glesTANGENT.xy;
					  highp vec3 tmpvar_5;
					  tmpvar_5 = (_glesVertex.xyz - _CameraPosition.xyz);
					  highp float tmpvar_6;
					  tmpvar_6 = dot (tmpvar_5, tmpvar_5);
					  if ((tmpvar_6 > _WaveAndDistance.w)) {
					    offset_4 = vec2(0.0, 0.0);
					  };
					  pos_3.xyz = (_glesVertex.xyz + (offset_4.x * _CameraRight));
					  pos_3.xyz = (pos_3.xyz + (offset_4.y * _CameraUp));
					  highp vec4 vertex_7;
					  vertex_7.yw = pos_3.yw;
					  lowp vec4 color_8;
					  color_8.xyz = tmpvar_2.xyz;
					  lowp vec3 waveColor_9;
					  highp vec3 waveMove_10;
					  highp vec4 s_11;
					  highp vec4 waves_12;
					  waves_12 = (pos_3.x * (vec4(0.012, 0.02, 0.06, 0.024) * _WaveAndDistance.y));
					  waves_12 = (waves_12 + (pos_3.z * (vec4(0.006, 0.02, 0.02, 0.05) * _WaveAndDistance.y)));
					  waves_12 = (waves_12 + (_WaveAndDistance.x * vec4(1.2, 2.0, 1.6, 4.8)));
					  highp vec4 tmpvar_13;
					  tmpvar_13 = fract(waves_12);
					  waves_12 = tmpvar_13;
					  highp vec4 val_14;
					  highp vec4 s_15;
					  val_14 = ((tmpvar_13 * 6.408849) - 3.141593);
					  highp vec4 tmpvar_16;
					  tmpvar_16 = (val_14 * val_14);
					  highp vec4 tmpvar_17;
					  tmpvar_17 = (tmpvar_16 * val_14);
					  highp vec4 tmpvar_18;
					  tmpvar_18 = (tmpvar_17 * tmpvar_16);
					  s_15 = (((val_14 + 
					    (tmpvar_17 * -0.1616162)
					  ) + (tmpvar_18 * 0.0083333)) + ((tmpvar_18 * tmpvar_16) * -0.00019841));
					  s_11 = (s_15 * s_15);
					  s_11 = (s_11 * s_11);
					  highp float tmpvar_19;
					  tmpvar_19 = (dot (s_11, vec4(0.6741998, 0.6741998, 0.2696799, 0.13484)) * 0.7);
					  s_11 = (s_11 * _glesTANGENT.y);
					  waveMove_10.y = 0.0;
					  waveMove_10.x = dot (s_11, vec4(0.024, 0.04, -0.12, 0.096));
					  waveMove_10.z = dot (s_11, vec4(0.006, 0.02, -0.02, 0.1));
					  vertex_7.xz = (pos_3.xz - (waveMove_10.xz * _WaveAndDistance.z));
					  highp vec3 tmpvar_20;
					  tmpvar_20 = mix (vec3(0.5, 0.5, 0.5), _WavingTint.xyz, vec3(tmpvar_19));
					  waveColor_9 = tmpvar_20;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = (vertex_7.xyz - _CameraPosition.xyz);
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (((2.0 * 
					    (_WaveAndDistance.w - dot (tmpvar_21, tmpvar_21))
					  ) * _CameraPosition.w), 0.0, 1.0);
					  color_8.w = tmpvar_22;
					  lowp vec4 tmpvar_23;
					  tmpvar_23.xyz = ((2.0 * waveColor_9) * _glesColor.xyz);
					  tmpvar_23.w = color_8.w;
					  highp vec4 tmpvar_24;
					  tmpvar_24.w = 1.0;
					  tmpvar_24.xyz = vertex_7.xyz;
					  highp vec4 tmpvar_25;
					  tmpvar_25.w = 1.0;
					  tmpvar_25.xyz = (unity_ObjectToWorld * tmpvar_24).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_25);
					  xlv_COLOR = tmpvar_23;
					  xlv_TEXCOORD0 = tmpvar_1.xy;
					  xlv_TEXCOORD1 = (unity_ObjectToWorld * vertex_7);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD0).w * xlv_COLOR.w) - _Cutoff);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _WavingTint;
					uniform 	vec4 _WaveAndDistance;
					uniform 	vec4 _CameraPosition;
					uniform 	vec3 _CameraRight;
					uniform 	vec3 _CameraUp;
					in highp vec4 in_POSITION0;
					in highp vec4 in_TANGENT0;
					in highp vec4 in_TEXCOORD0;
					in mediump vec4 in_COLOR0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					mediump vec3 u_xlat16_4;
					float u_xlat5;
					float u_xlat15;
					void main()
					{
					    u_xlat0.xyz = in_POSITION0.xyz + (-_CameraPosition.xyz);
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(_WaveAndDistance.w<u_xlat0.x);
					#else
					    u_xlatb0 = _WaveAndDistance.w<u_xlat0.x;
					#endif
					    u_xlat0.xy = (bool(u_xlatb0)) ? vec2(0.0, 0.0) : in_TANGENT0.xy;
					    u_xlat0.xzw = u_xlat0.xxx * _CameraRight.xyz + in_POSITION0.xyz;
					    u_xlat0.xyz = u_xlat0.yyy * _CameraUp.xyz + u_xlat0.xzw;
					    u_xlat1.xy = u_xlat0.xz * _WaveAndDistance.yy;
					    u_xlat2 = u_xlat1.yyyy * vec4(0.00600000005, 0.0199999996, 0.0199999996, 0.0500000007);
					    u_xlat1 = u_xlat1.xxxx * vec4(0.0120000001, 0.0199999996, 0.0599999987, 0.0240000002) + u_xlat2;
					    u_xlat1 = _WaveAndDistance.xxxx * vec4(1.20000005, 2.0, 1.60000002, 4.80000019) + u_xlat1;
					    u_xlat1 = fract(u_xlat1);
					    u_xlat1 = u_xlat1 * vec4(6.40884876, 6.40884876, 6.40884876, 6.40884876) + vec4(-3.14159274, -3.14159274, -3.14159274, -3.14159274);
					    u_xlat2 = u_xlat1 * u_xlat1;
					    u_xlat3 = u_xlat1 * u_xlat2;
					    u_xlat1 = u_xlat3 * vec4(-0.161616161, -0.161616161, -0.161616161, -0.161616161) + u_xlat1;
					    u_xlat3 = u_xlat2 * u_xlat3;
					    u_xlat2 = u_xlat2 * u_xlat3;
					    u_xlat1 = u_xlat3 * vec4(0.00833330024, 0.00833330024, 0.00833330024, 0.00833330024) + u_xlat1;
					    u_xlat1 = u_xlat2 * vec4(-0.000198409994, -0.000198409994, -0.000198409994, -0.000198409994) + u_xlat1;
					    u_xlat1 = u_xlat1 * u_xlat1;
					    u_xlat1 = u_xlat1 * u_xlat1;
					    u_xlat2 = u_xlat1 * in_TANGENT0.yyyy;
					    u_xlat15 = dot(u_xlat1, vec4(0.674199879, 0.674199879, 0.269679934, 0.134839967));
					    u_xlat15 = u_xlat15 * 0.699999988;
					    u_xlat1.x = dot(u_xlat2, vec4(0.0240000002, 0.0399999991, -0.119999997, 0.0960000008));
					    u_xlat1.z = dot(u_xlat2, vec4(0.00600000005, 0.0199999996, -0.0199999996, 0.100000001));
					    u_xlat0.xz = (-u_xlat1.xz) * _WaveAndDistance.zz + u_xlat0.xz;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat2.xyz = u_xlat0.xyz + (-_CameraPosition.xyz);
					    u_xlat5 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat5 = (-u_xlat5) + _WaveAndDistance.w;
					    u_xlat5 = dot(_CameraPosition.ww, vec2(u_xlat5));
					#ifdef UNITY_ADRENO_ES3
					    u_xlat5 = min(max(u_xlat5, 0.0), 1.0);
					#else
					    u_xlat5 = clamp(u_xlat5, 0.0, 1.0);
					#endif
					    vs_COLOR0.w = u_xlat5;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0.xyz = u_xlat1.xyz + hlslcc_mtx4x4unity_ObjectToWorld[3].xyz;
					    vs_TEXCOORD1 = hlslcc_mtx4x4unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat1;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = u_xlat1 + hlslcc_mtx4x4unity_MatrixVP[3];
					    u_xlat16_4.xyz = _WavingTint.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat16_4.xyz = vec3(u_xlat15) * u_xlat16_4.xyz + vec3(0.5, 0.5, 0.5);
					    u_xlat16_4.xyz = u_xlat16_4.xyz * in_COLOR0.xyz;
					    vs_COLOR0.xyz = u_xlat16_4.xyz + u_xlat16_4.xyz;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump float _Cutoff;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 * vs_COLOR0.w + (-_Cutoff);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD1;
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
	SubShader {
		Tags { "RenderType" = "Grass" }
		Pass {
			Tags { "RenderType" = "Grass" }
			Cull Off
			GpuProgramID 649013
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _WavingTint;
					uniform highp vec4 _WaveAndDistance;
					uniform highp vec4 _CameraPosition;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec4 vertex_1;
					  vertex_1.yw = _glesVertex.yw;
					  lowp vec4 color_2;
					  color_2.xyz = _glesColor.xyz;
					  lowp vec3 waveColor_3;
					  highp vec3 waveMove_4;
					  highp vec4 s_5;
					  highp vec4 waves_6;
					  waves_6 = (_glesVertex.x * (vec4(0.012, 0.02, 0.06, 0.024) * _WaveAndDistance.y));
					  waves_6 = (waves_6 + (_glesVertex.z * (vec4(0.006, 0.02, 0.02, 0.05) * _WaveAndDistance.y)));
					  waves_6 = (waves_6 + (_WaveAndDistance.x * vec4(1.2, 2.0, 1.6, 4.8)));
					  highp vec4 tmpvar_7;
					  tmpvar_7 = fract(waves_6);
					  waves_6 = tmpvar_7;
					  highp vec4 val_8;
					  highp vec4 s_9;
					  val_8 = ((tmpvar_7 * 6.408849) - 3.141593);
					  highp vec4 tmpvar_10;
					  tmpvar_10 = (val_8 * val_8);
					  highp vec4 tmpvar_11;
					  tmpvar_11 = (tmpvar_10 * val_8);
					  highp vec4 tmpvar_12;
					  tmpvar_12 = (tmpvar_11 * tmpvar_10);
					  s_9 = (((val_8 + 
					    (tmpvar_11 * -0.1616162)
					  ) + (tmpvar_12 * 0.0083333)) + ((tmpvar_12 * tmpvar_10) * -0.00019841));
					  s_5 = (s_9 * s_9);
					  s_5 = (s_5 * s_5);
					  highp float tmpvar_13;
					  tmpvar_13 = (dot (s_5, vec4(0.6741998, 0.6741998, 0.2696799, 0.13484)) * 0.7);
					  s_5 = (s_5 * (_glesColor.w * _WaveAndDistance.z));
					  waveMove_4.y = 0.0;
					  waveMove_4.x = dot (s_5, vec4(0.024, 0.04, -0.12, 0.096));
					  waveMove_4.z = dot (s_5, vec4(0.006, 0.02, -0.02, 0.1));
					  vertex_1.xz = (_glesVertex.xz - (waveMove_4.xz * _WaveAndDistance.z));
					  highp vec3 tmpvar_14;
					  tmpvar_14 = mix (vec3(0.5, 0.5, 0.5), _WavingTint.xyz, vec3(tmpvar_13));
					  waveColor_3 = tmpvar_14;
					  highp vec3 tmpvar_15;
					  tmpvar_15 = (vertex_1.xyz - _CameraPosition.xyz);
					  highp float tmpvar_16;
					  tmpvar_16 = clamp (((2.0 * 
					    (_WaveAndDistance.w - dot (tmpvar_15, tmpvar_15))
					  ) * _CameraPosition.w), 0.0, 1.0);
					  color_2.w = tmpvar_16;
					  lowp vec4 tmpvar_17;
					  tmpvar_17.xyz = ((2.0 * waveColor_3) * _glesColor.xyz);
					  tmpvar_17.w = color_2.w;
					  highp vec4 tmpvar_18;
					  tmpvar_18.w = 1.0;
					  tmpvar_18.xyz = vertex_1.xyz;
					  highp vec4 tmpvar_19;
					  tmpvar_19.w = 1.0;
					  tmpvar_19.xyz = (unity_ObjectToWorld * tmpvar_18).xyz;
					  gl_Position = (unity_MatrixVP * tmpvar_19);
					  xlv_COLOR = tmpvar_17;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (unity_ObjectToWorld * vertex_1);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD0).w * xlv_COLOR.w) - _Cutoff);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = xlv_TEXCOORD1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _WavingTint;
					uniform 	vec4 _WaveAndDistance;
					uniform 	vec4 _CameraPosition;
					in highp vec4 in_POSITION0;
					in highp vec4 in_TEXCOORD0;
					in mediump vec4 in_COLOR0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					mediump vec3 u_xlat16_3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0.xy = in_POSITION0.xz * _WaveAndDistance.yy;
					    u_xlat1 = u_xlat0.yyyy * vec4(0.00600000005, 0.0199999996, 0.0199999996, 0.0500000007);
					    u_xlat0 = u_xlat0.xxxx * vec4(0.0120000001, 0.0199999996, 0.0599999987, 0.0240000002) + u_xlat1;
					    u_xlat0 = _WaveAndDistance.xxxx * vec4(1.20000005, 2.0, 1.60000002, 4.80000019) + u_xlat0;
					    u_xlat0 = fract(u_xlat0);
					    u_xlat0 = u_xlat0 * vec4(6.40884876, 6.40884876, 6.40884876, 6.40884876) + vec4(-3.14159274, -3.14159274, -3.14159274, -3.14159274);
					    u_xlat1 = u_xlat0 * u_xlat0;
					    u_xlat2 = u_xlat0 * u_xlat1;
					    u_xlat0 = u_xlat2 * vec4(-0.161616161, -0.161616161, -0.161616161, -0.161616161) + u_xlat0;
					    u_xlat2 = u_xlat1 * u_xlat2;
					    u_xlat1 = u_xlat1 * u_xlat2;
					    u_xlat0 = u_xlat2 * vec4(0.00833330024, 0.00833330024, 0.00833330024, 0.00833330024) + u_xlat0;
					    u_xlat0 = u_xlat1 * vec4(-0.000198409994, -0.000198409994, -0.000198409994, -0.000198409994) + u_xlat0;
					    u_xlat0 = u_xlat0 * u_xlat0;
					    u_xlat0 = u_xlat0 * u_xlat0;
					    u_xlat1.x = in_COLOR0.w * _WaveAndDistance.z;
					    u_xlat1 = u_xlat0 * u_xlat1.xxxx;
					    u_xlat0.x = dot(u_xlat0, vec4(0.674199879, 0.674199879, 0.269679934, 0.134839967));
					    u_xlat0.x = u_xlat0.x * 0.699999988;
					    u_xlat2.x = dot(u_xlat1, vec4(0.0240000002, 0.0399999991, -0.119999997, 0.0960000008));
					    u_xlat2.z = dot(u_xlat1, vec4(0.00600000005, 0.0199999996, -0.0199999996, 0.100000001));
					    u_xlat1.xz = (-u_xlat2.xz) * _WaveAndDistance.zz + in_POSITION0.xz;
					    u_xlat2 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat2 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat4.xyz = u_xlat2.xyz + hlslcc_mtx4x4unity_ObjectToWorld[3].xyz;
					    vs_TEXCOORD1 = hlslcc_mtx4x4unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat2;
					    u_xlat2 = u_xlat4.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat4.xxxx + u_xlat2;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat4.zzzz + u_xlat2;
					    gl_Position = u_xlat2 + hlslcc_mtx4x4unity_MatrixVP[3];
					    u_xlat16_3.xyz = _WavingTint.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat16_3.xyz = u_xlat0.xxx * u_xlat16_3.xyz + vec3(0.5, 0.5, 0.5);
					    u_xlat16_3.xyz = u_xlat16_3.xyz * in_COLOR0.xyz;
					    vs_COLOR0.xyz = u_xlat16_3.xyz + u_xlat16_3.xyz;
					    u_xlat1.y = in_POSITION0.y;
					    u_xlat0.xyz = u_xlat1.xyz + (-_CameraPosition.xyz);
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat0.x = (-u_xlat0.x) + _WaveAndDistance.w;
					    u_xlat0.x = dot(_CameraPosition.ww, u_xlat0.xx);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
					#else
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					#endif
					    vs_COLOR0.w = u_xlat0.x;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump float _Cutoff;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					layout(location = 0) out highp vec4 SV_Target0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 * vs_COLOR0.w + (-_Cutoff);
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0 = vs_TEXCOORD1;
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