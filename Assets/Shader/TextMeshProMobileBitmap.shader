Shader "TextMeshPro/Mobile/Bitmap" {
	Properties {
		_MainTex ("Font Atlas", 2D) = "white" {}
		_Color ("Text Color", Vector) = (1,1,1,1)
		_DiffusePower ("Diffuse Power", Range(1, 4)) = 1
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
		_ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
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
			GpuProgramID 5252
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _Color;
					uniform highp float _DiffusePower;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  lowp vec4 tmpvar_2;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = vert_1.xyz;
					  tmpvar_3 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  highp vec4 pos_5;
					  pos_5.zw = tmpvar_3.zw;
					  highp vec2 tmpvar_6;
					  tmpvar_6 = (_ScreenParams.xy * 0.5);
					  pos_5.xy = ((floor(
					    (((tmpvar_3.xy / tmpvar_3.w) * tmpvar_6) + vec2(0.5, 0.5))
					  ) / tmpvar_6) * tmpvar_3.w);
					  tmpvar_2 = (_glesColor * _Color);
					  tmpvar_2.xyz = (tmpvar_2.xyz * _DiffusePower);
					  highp vec4 tmpvar_7;
					  tmpvar_7 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_8;
					  tmpvar_8.x = _MaskSoftnessX;
					  tmpvar_8.y = _MaskSoftnessY;
					  highp vec4 tmpvar_9;
					  tmpvar_9.xy = (((vert_1.xy * 2.0) - tmpvar_7.xy) - tmpvar_7.zw);
					  tmpvar_9.zw = (0.25 / ((0.25 * tmpvar_8) + tmpvar_3.ww));
					  gl_Position = pos_5;
					  xlv_COLOR = tmpvar_2;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD2 = tmpvar_9;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1.xyz = xlv_COLOR.xyz;
					  tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
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
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _Color;
					uniform 	float _DiffusePower;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					mediump vec4 u_xlat16_1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
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
					    u_xlat6.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.ww;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat6.xy;
					    u_xlat16_1 = in_COLOR0 * _Color;
					    u_xlat2.xyz = u_xlat16_1.xyz * vec3(_DiffusePower);
					    vs_COLOR0.w = u_xlat16_1.w;
					    vs_COLOR0.xyz = u_xlat2.xyz;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat1 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat1 = min(u_xlat1, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat1.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat1.z) + u_xlat0.x, (-u_xlat1.w) + u_xlat0.y);
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump float u_xlat16_0;
					lowp float u_xlat10_0;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_0 = u_xlat10_0 * vs_COLOR0.w;
					    SV_Target0.w = u_xlat16_0;
					    SV_Target0.xyz = vs_COLOR0.xyz;
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
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _Color;
					uniform highp float _DiffusePower;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  lowp vec4 tmpvar_2;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = vert_1.xyz;
					  tmpvar_3 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  highp vec4 pos_5;
					  pos_5.zw = tmpvar_3.zw;
					  highp vec2 tmpvar_6;
					  tmpvar_6 = (_ScreenParams.xy * 0.5);
					  pos_5.xy = ((floor(
					    (((tmpvar_3.xy / tmpvar_3.w) * tmpvar_6) + vec2(0.5, 0.5))
					  ) / tmpvar_6) * tmpvar_3.w);
					  tmpvar_2 = (_glesColor * _Color);
					  tmpvar_2.xyz = (tmpvar_2.xyz * _DiffusePower);
					  highp vec4 tmpvar_7;
					  tmpvar_7 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_8;
					  tmpvar_8.x = _MaskSoftnessX;
					  tmpvar_8.y = _MaskSoftnessY;
					  highp vec4 tmpvar_9;
					  tmpvar_9.xy = (((vert_1.xy * 2.0) - tmpvar_7.xy) - tmpvar_7.zw);
					  tmpvar_9.zw = (0.25 / ((0.25 * tmpvar_8) + tmpvar_3.ww));
					  gl_Position = pos_5;
					  xlv_COLOR = tmpvar_2;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD2 = tmpvar_9;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1.xyz = xlv_COLOR.xyz;
					  tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
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
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _Color;
					uniform 	float _DiffusePower;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					mediump vec4 u_xlat16_1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
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
					    u_xlat6.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.ww;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat6.xy;
					    u_xlat16_1 = in_COLOR0 * _Color;
					    u_xlat2.xyz = u_xlat16_1.xyz * vec3(_DiffusePower);
					    vs_COLOR0.w = u_xlat16_1.w;
					    vs_COLOR0.xyz = u_xlat2.xyz;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat1 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat1 = min(u_xlat1, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat1.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat1.z) + u_xlat0.x, (-u_xlat1.w) + u_xlat0.y);
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump float u_xlat16_0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = vs_COLOR0.w * u_xlat10_0 + -0.00100000005;
					    u_xlat16_0 = u_xlat10_0 * vs_COLOR0.w;
					    SV_Target0.w = u_xlat16_0;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_1<0.0);
					#else
					    u_xlatb0 = u_xlat16_1<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
					    SV_Target0.xyz = vs_COLOR0.xyz;
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
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _Color;
					uniform highp float _DiffusePower;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  lowp vec4 tmpvar_2;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = vert_1.xyz;
					  tmpvar_3 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  highp vec4 pos_5;
					  pos_5.zw = tmpvar_3.zw;
					  highp vec2 tmpvar_6;
					  tmpvar_6 = (_ScreenParams.xy * 0.5);
					  pos_5.xy = ((floor(
					    (((tmpvar_3.xy / tmpvar_3.w) * tmpvar_6) + vec2(0.5, 0.5))
					  ) / tmpvar_6) * tmpvar_3.w);
					  tmpvar_2 = (_glesColor * _Color);
					  tmpvar_2.xyz = (tmpvar_2.xyz * _DiffusePower);
					  highp vec4 tmpvar_7;
					  tmpvar_7 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_8;
					  tmpvar_8.x = _MaskSoftnessX;
					  tmpvar_8.y = _MaskSoftnessY;
					  highp vec4 tmpvar_9;
					  tmpvar_9.xy = (((vert_1.xy * 2.0) - tmpvar_7.xy) - tmpvar_7.zw);
					  tmpvar_9.zw = (0.25 / ((0.25 * tmpvar_8) + tmpvar_3.ww));
					  gl_Position = pos_5;
					  xlv_COLOR = tmpvar_2;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD2 = tmpvar_9;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp vec4 _ClipRect;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2.xyz = xlv_COLOR.xyz;
					  tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
					  mediump vec2 tmpvar_3;
					  highp vec2 tmpvar_4;
					  tmpvar_4 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - 
					    abs(xlv_TEXCOORD2.xy)
					  ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_3 = tmpvar_4;
					  color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
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
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _Color;
					uniform 	float _DiffusePower;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					mediump vec4 u_xlat16_1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
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
					    u_xlat6.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.ww;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat6.xy;
					    u_xlat16_1 = in_COLOR0 * _Color;
					    u_xlat2.xyz = u_xlat16_1.xyz * vec3(_DiffusePower);
					    vs_COLOR0.w = u_xlat16_1.w;
					    vs_COLOR0.xyz = u_xlat2.xyz;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat1 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat1 = min(u_xlat1, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat1.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat1.z) + u_xlat0.x, (-u_xlat1.w) + u_xlat0.y);
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD2;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump float u_xlat16_0;
					lowp float u_xlat10_0;
					mediump float u_xlat16_1;
					vec2 u_xlat2;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_0 = u_xlat10_0 * vs_COLOR0.w;
					    u_xlat2.xy = vec2((-_ClipRect.x) + _ClipRect.z, (-_ClipRect.y) + _ClipRect.w);
					    u_xlat2.xy = u_xlat2.xy + -abs(vs_TEXCOORD2.xy);
					    u_xlat2.xy = vec2(u_xlat2.x * vs_TEXCOORD2.z, u_xlat2.y * vs_TEXCOORD2.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat2.xy = min(max(u_xlat2.xy, 0.0), 1.0);
					#else
					    u_xlat2.xy = clamp(u_xlat2.xy, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat2.y * u_xlat2.x;
					    SV_Target0.w = u_xlat16_0 * u_xlat16_1;
					    SV_Target0.xyz = vec3(u_xlat16_1) * vs_COLOR0.xyz;
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
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _Color;
					uniform highp float _DiffusePower;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 vert_1;
					  lowp vec4 tmpvar_2;
					  vert_1.zw = _glesVertex.zw;
					  vert_1.x = (_glesVertex.x + _VertexOffsetX);
					  vert_1.y = (_glesVertex.y + _VertexOffsetY);
					  vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
					  highp vec4 tmpvar_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = vert_1.xyz;
					  tmpvar_3 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  highp vec4 pos_5;
					  pos_5.zw = tmpvar_3.zw;
					  highp vec2 tmpvar_6;
					  tmpvar_6 = (_ScreenParams.xy * 0.5);
					  pos_5.xy = ((floor(
					    (((tmpvar_3.xy / tmpvar_3.w) * tmpvar_6) + vec2(0.5, 0.5))
					  ) / tmpvar_6) * tmpvar_3.w);
					  tmpvar_2 = (_glesColor * _Color);
					  tmpvar_2.xyz = (tmpvar_2.xyz * _DiffusePower);
					  highp vec4 tmpvar_7;
					  tmpvar_7 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_8;
					  tmpvar_8.x = _MaskSoftnessX;
					  tmpvar_8.y = _MaskSoftnessY;
					  highp vec4 tmpvar_9;
					  tmpvar_9.xy = (((vert_1.xy * 2.0) - tmpvar_7.xy) - tmpvar_7.zw);
					  tmpvar_9.zw = (0.25 / ((0.25 * tmpvar_8) + tmpvar_3.ww));
					  gl_Position = pos_5;
					  xlv_COLOR = tmpvar_2;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD2 = tmpvar_9;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp vec4 _ClipRect;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2.xyz = xlv_COLOR.xyz;
					  tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
					  mediump vec2 tmpvar_3;
					  highp vec2 tmpvar_4;
					  tmpvar_4 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - 
					    abs(xlv_TEXCOORD2.xy)
					  ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_3 = tmpvar_4;
					  color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
					  lowp float x_5;
					  x_5 = (color_1.w - 0.001);
					  if ((x_5 < 0.0)) {
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
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _Color;
					uniform 	float _DiffusePower;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					in highp vec4 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					vec4 u_xlat1;
					mediump vec4 u_xlat16_1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = in_POSITION0.w * 0.5;
					    u_xlat0.xy = u_xlat0.xx / _ScreenParams.xy;
					    u_xlat6.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
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
					    u_xlat6.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.ww;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat6.xy;
					    u_xlat16_1 = in_COLOR0 * _Color;
					    u_xlat2.xyz = u_xlat16_1.xyz * vec3(_DiffusePower);
					    vs_COLOR0.w = u_xlat16_1.w;
					    vs_COLOR0.xyz = u_xlat2.xyz;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat1 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat1 = min(u_xlat1, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat1.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat1.z) + u_xlat0.x, (-u_xlat1.w) + u_xlat0.y);
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD2;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump float u_xlat16_0;
					lowp float u_xlat10_0;
					bool u_xlatb0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_3;
					mediump float u_xlat16_5;
					void main()
					{
					    u_xlat0.xy = vec2((-_ClipRect.x) + _ClipRect.z, (-_ClipRect.y) + _ClipRect.w);
					    u_xlat0.xy = u_xlat0.xy + -abs(vs_TEXCOORD2.xy);
					    u_xlat0.xy = vec2(u_xlat0.x * vs_TEXCOORD2.z, u_xlat0.y * vs_TEXCOORD2.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat0.xy = min(max(u_xlat0.xy, 0.0), 1.0);
					#else
					    u_xlat0.xy = clamp(u_xlat0.xy, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat0.y * u_xlat0.x;
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_0 = u_xlat10_0 * vs_COLOR0.w;
					    u_xlat16_3 = u_xlat16_0 * u_xlat16_1 + -0.00100000005;
					    u_xlat16_5 = u_xlat16_1 * u_xlat16_0;
					    SV_Target0.xyz = vec3(u_xlat16_1) * vs_COLOR0.xyz;
					    SV_Target0.w = u_xlat16_5;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(u_xlat16_3<0.0);
					#else
					    u_xlatb0 = u_xlat16_3<0.0;
					#endif
					    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
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
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZTest Always
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 117148
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
					uniform highp vec4 _MainTex_ST;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  tmpvar_2 = clamp (_glesColor, 0.0, 1.0);
					  tmpvar_1 = tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = _glesVertex.xyz;
					  xlv_COLOR0 = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 col_1;
					  col_1.xyz = (_Color * xlv_COLOR0).xyz;
					  col_1.w = (_Color.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
					  gl_FragData[0] = col_1;
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
					in highp vec3 in_POSITION0;
					in mediump vec4 in_COLOR0;
					in highp vec3 in_TEXCOORD0;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_COLOR0 = in_COLOR0;
					#ifdef UNITY_ADRENO_ES3
					    vs_COLOR0 = min(max(vs_COLOR0, 0.0), 1.0);
					#else
					    vs_COLOR0 = clamp(vs_COLOR0, 0.0, 1.0);
					#endif
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump vec4 _Color;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					lowp float u_xlat10_0;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    SV_Target0.w = u_xlat10_0 * _Color.w;
					    SV_Target0.xyz = vs_COLOR0.xyz * _Color.xyz;
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
	CustomEditor "TMPro.EditorUtilities.TMP_BitmapShaderGUI"
}