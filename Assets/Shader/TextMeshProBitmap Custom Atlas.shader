Shader "TextMeshPro/Bitmap Custom Atlas" {
	Properties {
		_MainTex ("Font Atlas", 2D) = "white" {}
		_FaceTex ("Font Texture", 2D) = "white" {}
		_FaceColor ("Text Color", Vector) = (1,1,1,1)
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
		_ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
		_Padding ("Padding", Float) = 0
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask 0 -1
			ZWrite Off
			Cull Off
			Stencil {
				ReadMask 0
				WriteMask 0
				Comp Disabled
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			Fog {
				Mode Off
			}
			GpuProgramID 38079
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _FaceTex_ST;
					uniform lowp vec4 _FaceColor;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = vert_1.xyz;
					  tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
					  highp vec4 pos_4;
					  pos_4.zw = tmpvar_2.zw;
					  highp vec2 tmpvar_5;
					  tmpvar_5 = (_ScreenParams.xy * 0.5);
					  pos_4.xy = ((floor(
					    (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
					  ) / tmpvar_5) * tmpvar_2.w);
					  highp vec2 xlat_varoutput_6;
					  xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
					  tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
					  highp vec4 tmpvar_8;
					  tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_9;
					  tmpvar_9.x = _MaskSoftnessX;
					  tmpvar_9.y = _MaskSoftnessY;
					  highp vec4 tmpvar_10;
					  tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
					  tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
					    abs(tmpvar_7)
					  )));
					  gl_Position = pos_4;
					  xlv_COLOR = (_glesColor * _FaceColor);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  xlv_TEXCOORD2 = tmpvar_10;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _FaceTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_FaceTex, xlv_TEXCOORD1)) * xlv_COLOR);
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _FaceTex_ST;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat0.xy = u_xlat0.xy + u_xlat6.xy;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat1 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    u_xlat6.xy = u_xlat1.xy / u_xlat1.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat6.xy = u_xlat6.xy * u_xlat1.xy;
					    u_xlat6.xy = roundEven(u_xlat6.xy);
					    u_xlat6.xy = u_xlat6.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat1.ww * u_xlat6.xy;
					    gl_Position.zw = u_xlat1.zw;
					    vs_COLOR0 = in_COLOR0 * _FaceColor;
					    u_xlat6.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat6.x = floor(u_xlat6.x);
					    u_xlat6.y = (-u_xlat6.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat6.xy = u_xlat6.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD1.xy = u_xlat6.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat6.x = _ScreenParams.x * hlslcc_mtx4x4glstate_matrix_projection[0].x;
					    u_xlat6.y = _ScreenParams.y * hlslcc_mtx4x4glstate_matrix_projection[1].y;
					    u_xlat0.xy = u_xlat1.ww / abs(u_xlat6.xy);
					    u_xlat0.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat0.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_FaceTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * u_xlat10_1;
					    u_xlat16_0 = u_xlat16_0 * vs_COLOR0;
					    SV_Target0 = u_xlat16_0;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles " {
					Keywords { "UNITY_UI_ALPHACLIP" }
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _FaceTex_ST;
					uniform lowp vec4 _FaceColor;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = vert_1.xyz;
					  tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
					  highp vec4 pos_4;
					  pos_4.zw = tmpvar_2.zw;
					  highp vec2 tmpvar_5;
					  tmpvar_5 = (_ScreenParams.xy * 0.5);
					  pos_4.xy = ((floor(
					    (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
					  ) / tmpvar_5) * tmpvar_2.w);
					  highp vec2 xlat_varoutput_6;
					  xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
					  tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
					  highp vec4 tmpvar_8;
					  tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_9;
					  tmpvar_9.x = _MaskSoftnessX;
					  tmpvar_9.y = _MaskSoftnessY;
					  highp vec4 tmpvar_10;
					  tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
					  tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
					    abs(tmpvar_7)
					  )));
					  gl_Position = pos_4;
					  xlv_COLOR = (_glesColor * _FaceColor);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  xlv_TEXCOORD2 = tmpvar_10;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _FaceTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_FaceTex, xlv_TEXCOORD1)) * xlv_COLOR);
					  lowp float x_2;
					  x_2 = (tmpvar_1.w - 0.001);
					  if ((x_2 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_ALPHACLIP" }
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _FaceTex_ST;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat0.xy = u_xlat0.xy + u_xlat6.xy;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat1 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    u_xlat6.xy = u_xlat1.xy / u_xlat1.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat6.xy = u_xlat6.xy * u_xlat1.xy;
					    u_xlat6.xy = roundEven(u_xlat6.xy);
					    u_xlat6.xy = u_xlat6.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat1.ww * u_xlat6.xy;
					    gl_Position.zw = u_xlat1.zw;
					    vs_COLOR0 = in_COLOR0 * _FaceColor;
					    u_xlat6.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat6.x = floor(u_xlat6.x);
					    u_xlat6.y = (-u_xlat6.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat6.xy = u_xlat6.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD1.xy = u_xlat6.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat6.x = _ScreenParams.x * hlslcc_mtx4x4glstate_matrix_projection[0].x;
					    u_xlat6.y = _ScreenParams.y * hlslcc_mtx4x4glstate_matrix_projection[1].y;
					    u_xlat0.xy = u_xlat1.ww / abs(u_xlat6.xy);
					    u_xlat0.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat0.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					bool u_xlatb0;
					lowp vec4 u_xlat10_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_FaceTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * u_xlat10_1;
					    u_xlat16_2 = u_xlat16_0.w * vs_COLOR0.w + -0.00100000005;
					    u_xlat16_0 = u_xlat16_0 * vs_COLOR0;
					    SV_Target0 = u_xlat16_0;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_2<0.0);
					#else
					    u_xlatb0 = u_xlat16_2<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    return;
					}
					
					#endif"
				}
				SubProgram "gles " {
					Keywords { "UNITY_UI_CLIP_RECT" }
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _FaceTex_ST;
					uniform lowp vec4 _FaceColor;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = vert_1.xyz;
					  tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
					  highp vec4 pos_4;
					  pos_4.zw = tmpvar_2.zw;
					  highp vec2 tmpvar_5;
					  tmpvar_5 = (_ScreenParams.xy * 0.5);
					  pos_4.xy = ((floor(
					    (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
					  ) / tmpvar_5) * tmpvar_2.w);
					  highp vec2 xlat_varoutput_6;
					  xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
					  tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
					  highp vec4 tmpvar_8;
					  tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_9;
					  tmpvar_9.x = _MaskSoftnessX;
					  tmpvar_9.y = _MaskSoftnessY;
					  highp vec4 tmpvar_10;
					  tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
					  tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
					    abs(tmpvar_7)
					  )));
					  gl_Position = pos_4;
					  xlv_COLOR = (_glesColor * _FaceColor);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  xlv_TEXCOORD2 = tmpvar_10;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _FaceTex;
					uniform highp vec4 _ClipRect;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 color_1;
					  mediump vec2 tmpvar_2;
					  highp vec2 tmpvar_3;
					  tmpvar_3 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - 
					    abs(xlv_TEXCOORD2.xy)
					  ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_2 = tmpvar_3;
					  color_1 = (((texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_FaceTex, xlv_TEXCOORD1)) * xlv_COLOR) * (tmpvar_2.x * tmpvar_2.y));
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_CLIP_RECT" }
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _FaceTex_ST;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat0.xy = u_xlat0.xy + u_xlat6.xy;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat1 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    u_xlat6.xy = u_xlat1.xy / u_xlat1.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat6.xy = u_xlat6.xy * u_xlat1.xy;
					    u_xlat6.xy = roundEven(u_xlat6.xy);
					    u_xlat6.xy = u_xlat6.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat1.ww * u_xlat6.xy;
					    gl_Position.zw = u_xlat1.zw;
					    vs_COLOR0 = in_COLOR0 * _FaceColor;
					    u_xlat6.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat6.x = floor(u_xlat6.x);
					    u_xlat6.y = (-u_xlat6.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat6.xy = u_xlat6.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD1.xy = u_xlat6.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat6.x = _ScreenParams.x * hlslcc_mtx4x4glstate_matrix_projection[0].x;
					    u_xlat6.y = _ScreenParams.y * hlslcc_mtx4x4glstate_matrix_projection[1].y;
					    u_xlat0.xy = u_xlat1.ww / abs(u_xlat6.xy);
					    u_xlat0.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat0.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					in highp vec4 vs_TEXCOORD2;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					vec2 u_xlat1;
					lowp vec4 u_xlat10_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_FaceTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * u_xlat10_1;
					    u_xlat16_0 = u_xlat16_0 * vs_COLOR0;
					    u_xlat1.xy = vec2((-_ClipRect.x) + _ClipRect.z, (-_ClipRect.y) + _ClipRect.w);
					    u_xlat1.xy = u_xlat1.xy + -abs(vs_TEXCOORD2.xy);
					    u_xlat1.xy = vec2(u_xlat1.x * vs_TEXCOORD2.z, u_xlat1.y * vs_TEXCOORD2.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
					#else
					    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
					#endif
					    u_xlat16_2 = u_xlat1.y * u_xlat1.x;
					    SV_Target0 = u_xlat16_0 * vec4(u_xlat16_2);
					    return;
					}
					
					#endif"
				}
				SubProgram "gles " {
					Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _FaceTex_ST;
					uniform lowp vec4 _FaceColor;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = vert_1.xyz;
					  tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
					  highp vec4 pos_4;
					  pos_4.zw = tmpvar_2.zw;
					  highp vec2 tmpvar_5;
					  tmpvar_5 = (_ScreenParams.xy * 0.5);
					  pos_4.xy = ((floor(
					    (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
					  ) / tmpvar_5) * tmpvar_2.w);
					  highp vec2 xlat_varoutput_6;
					  xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
					  tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
					  highp vec4 tmpvar_8;
					  tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_9;
					  tmpvar_9.x = _MaskSoftnessX;
					  tmpvar_9.y = _MaskSoftnessY;
					  highp vec4 tmpvar_10;
					  tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
					  tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
					    abs(tmpvar_7)
					  )));
					  gl_Position = pos_4;
					  xlv_COLOR = (_glesColor * _FaceColor);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  xlv_TEXCOORD2 = tmpvar_10;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _FaceTex;
					uniform highp vec4 _ClipRect;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 color_1;
					  mediump vec2 tmpvar_2;
					  highp vec2 tmpvar_3;
					  tmpvar_3 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - 
					    abs(xlv_TEXCOORD2.xy)
					  ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_2 = tmpvar_3;
					  color_1 = (((texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_FaceTex, xlv_TEXCOORD1)) * xlv_COLOR) * (tmpvar_2.x * tmpvar_2.y));
					  lowp float x_4;
					  x_4 = (color_1.w - 0.001);
					  if ((x_4 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _FaceTex_ST;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat0.xy = u_xlat0.xy + u_xlat6.xy;
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat1 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    u_xlat6.xy = u_xlat1.xy / u_xlat1.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat6.xy = u_xlat6.xy * u_xlat1.xy;
					    u_xlat6.xy = roundEven(u_xlat6.xy);
					    u_xlat6.xy = u_xlat6.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat1.ww * u_xlat6.xy;
					    gl_Position.zw = u_xlat1.zw;
					    vs_COLOR0 = in_COLOR0 * _FaceColor;
					    u_xlat6.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat6.x = floor(u_xlat6.x);
					    u_xlat6.y = (-u_xlat6.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat6.xy = u_xlat6.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD1.xy = u_xlat6.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat6.x = _ScreenParams.x * hlslcc_mtx4x4glstate_matrix_projection[0].x;
					    u_xlat6.y = _ScreenParams.y * hlslcc_mtx4x4glstate_matrix_projection[1].y;
					    u_xlat0.xy = u_xlat1.ww / abs(u_xlat6.xy);
					    u_xlat0.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat0.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					in highp vec4 vs_TEXCOORD2;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					vec2 u_xlat1;
					lowp vec4 u_xlat10_1;
					bool u_xlatb1;
					mediump float u_xlat16_2;
					mediump float u_xlat16_5;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_FaceTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * u_xlat10_1;
					    u_xlat16_0 = u_xlat16_0 * vs_COLOR0;
					    u_xlat1.xy = vec2((-_ClipRect.x) + _ClipRect.z, (-_ClipRect.y) + _ClipRect.w);
					    u_xlat1.xy = u_xlat1.xy + -abs(vs_TEXCOORD2.xy);
					    u_xlat1.xy = vec2(u_xlat1.x * vs_TEXCOORD2.z, u_xlat1.y * vs_TEXCOORD2.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
					#else
					    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
					#endif
					    u_xlat16_2 = u_xlat1.y * u_xlat1.x;
					    u_xlat16_5 = u_xlat16_0.w * u_xlat16_2 + -0.00100000005;
					    u_xlat16_0 = u_xlat16_0 * vec4(u_xlat16_2);
					    SV_Target0 = u_xlat16_0;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb1 = !!(u_xlat16_5<0.0);
					#else
					    u_xlatb1 = u_xlat16_5<0.0;
					#endif
					    if((int(u_xlatb1) * int(0xffffffffu))!=0){discard;}
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
				SubProgram "gles " {
					Keywords { "UNITY_UI_ALPHACLIP" }
					"!!!!GLES"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_ALPHACLIP" }
					"!!!!GLES3"
				}
				SubProgram "gles " {
					Keywords { "UNITY_UI_CLIP_RECT" }
					"!!!!GLES"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_CLIP_RECT" }
					"!!!!GLES3"
				}
				SubProgram "gles " {
					Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
					"!!!!GLES"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
					"!!!!GLES3"
				}
			}
		}
	}
	CustomEditor "TMPro.EditorUtilities.TMP_BitmapShaderGUI"
}