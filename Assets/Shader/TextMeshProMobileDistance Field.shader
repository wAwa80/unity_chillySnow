Shader "TextMeshPro/Mobile/Distance Field" {
	Properties {
		_FaceColor ("Face Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
		_UnderlayColor ("Border Color", Vector) = (0,0,0,0.5)
		_UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
		_UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
		_UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
		_UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
		_WeightNormal ("Weight Normal", Float) = 0
		_WeightBold ("Weight Bold", Float) = 0.5
		_ShaderFlags ("Flags", Float) = 0
		_ScaleRatioA ("Scale RatioA", Float) = 1
		_ScaleRatioB ("Scale RatioB", Float) = 1
		_ScaleRatioC ("Scale RatioC", Float) = 1
		_MainTex ("Font Atlas", 2D) = "white" {}
		_TextureWidth ("Texture Width", Float) = 512
		_TextureHeight ("Texture Height", Float) = 512
		_GradientScale ("Gradient Scale", Float) = 5
		_ScaleX ("Scale X", Float) = 1
		_ScaleY ("Scale Y", Float) = 1
		_PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
		_ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
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
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
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
			GpuProgramID 57736
			Program "vp" {
				SubProgram "gles " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _FaceColor;
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _WeightNormal;
					uniform highp float _WeightBold;
					uniform highp float _ScaleRatioA;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					uniform highp float _GradientScale;
					uniform highp float _ScaleX;
					uniform highp float _ScaleY;
					uniform highp float _PerspectiveFilter;
					varying lowp vec4 xlv_COLOR;
					varying lowp vec4 xlv_COLOR1;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  lowp vec4 outlineColor_3;
					  lowp vec4 faceColor_4;
					  highp float opacity_5;
					  highp float scale_6;
					  highp vec2 pixelSize_7;
					  highp vec4 vert_8;
					  highp float tmpvar_9;
					  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_8.zw = _glesVertex.zw;
					  vert_8.x = (_glesVertex.x + _VertexOffsetX);
					  vert_8.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_10;
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = vert_8.xyz;
					  tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
					  highp vec2 tmpvar_12;
					  tmpvar_12.x = _ScaleX;
					  tmpvar_12.y = _ScaleY;
					  highp mat2 tmpvar_13;
					  tmpvar_13[0] = glstate_matrix_projection[0].xy;
					  tmpvar_13[1] = glstate_matrix_projection[1].xy;
					  pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
					    (tmpvar_13 * _ScreenParams.xy)
					  )));
					  scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_14;
					    tmpvar_14[0] = unity_WorldToObject[0].xyz;
					    tmpvar_14[1] = unity_WorldToObject[1].xyz;
					    tmpvar_14[2] = unity_WorldToObject[2].xyz;
					    scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
					      normalize((_glesNormal * tmpvar_14))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
					    )));
					  };
					  scale_6 = (scale_6 / (1.0 + (
					    (_OutlineSoftness * _ScaleRatioA)
					   * scale_6)));
					  highp float tmpvar_15;
					  tmpvar_15 = (((0.5 - 
					    ((((
					      mix (_WeightNormal, _WeightBold, tmpvar_9)
					     / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
					  ) * scale_6) - 0.5);
					  highp float tmpvar_16;
					  tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
					  lowp float tmpvar_17;
					  tmpvar_17 = tmpvar_1.w;
					  opacity_5 = tmpvar_17;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xyz = tmpvar_1.xyz;
					  tmpvar_18.w = opacity_5;
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (tmpvar_18 * _FaceColor);
					  faceColor_4 = tmpvar_19;
					  faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
					  outlineColor_3.xyz = _OutlineColor.xyz;
					  outlineColor_3.w = (_OutlineColor.w * opacity_5);
					  outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
					  highp vec4 tmpvar_20;
					  tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
					    (tmpvar_16 * 2.0)
					  ))));
					  outlineColor_3 = tmpvar_20;
					  highp vec4 tmpvar_21;
					  tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_22;
					  tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
					  highp vec4 tmpvar_23;
					  tmpvar_23.xy = tmpvar_2;
					  tmpvar_23.z = tmpvar_22.x;
					  tmpvar_23.w = tmpvar_22.y;
					  highp vec4 tmpvar_24;
					  tmpvar_24.x = scale_6;
					  tmpvar_24.y = (tmpvar_15 - tmpvar_16);
					  tmpvar_24.z = (tmpvar_15 + tmpvar_16);
					  tmpvar_24.w = tmpvar_15;
					  highp vec2 tmpvar_25;
					  tmpvar_25.x = _MaskSoftnessX;
					  tmpvar_25.y = _MaskSoftnessY;
					  highp vec4 tmpvar_26;
					  tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
					  tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
					  mediump vec4 tmpvar_27;
					  mediump vec4 tmpvar_28;
					  tmpvar_27 = tmpvar_24;
					  tmpvar_28 = tmpvar_26;
					  gl_Position = tmpvar_10;
					  xlv_COLOR = faceColor_4;
					  xlv_COLOR1 = outlineColor_3;
					  xlv_TEXCOORD0 = tmpvar_23;
					  xlv_TEXCOORD1 = tmpvar_27;
					  xlv_TEXCOORD2 = tmpvar_28;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = (xlv_COLOR * tmpvar_4);
					  c_2 = tmpvar_5;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _WeightNormal;
					uniform 	float _WeightBold;
					uniform 	float _ScaleRatioA;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					uniform 	float _GradientScale;
					uniform 	float _ScaleX;
					uniform 	float _ScaleY;
					uniform 	float _PerspectiveFilter;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out mediump vec4 vs_COLOR1;
					out highp vec4 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					mediump vec4 u_xlat16_3;
					vec4 u_xlat4;
					float u_xlat5;
					float u_xlat7;
					float u_xlat10;
					float u_xlat15;
					bool u_xlatb15;
					void main()
					{
					    u_xlat0.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    u_xlat3 = u_xlat2.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    gl_Position = u_xlat2;
					    u_xlat16_3 = in_COLOR0 * _FaceColor;
					    u_xlat16_3.xyz = u_xlat16_3.www * u_xlat16_3.xyz;
					    vs_COLOR0 = u_xlat16_3;
					    u_xlat10 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat1.xyz = vec3(u_xlat10) * u_xlat1.xyz;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat2.xyz = vec3(u_xlat10) * u_xlat2.xyz;
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat1.xyz);
					    u_xlat1.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat1.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat1.xy;
					    u_xlat1.xy = vec2(abs(u_xlat1.x) * float(_ScaleX), abs(u_xlat1.y) * float(_ScaleY));
					    u_xlat1.xy = u_xlat2.ww / u_xlat1.xy;
					    u_xlat15 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat1.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.xy;
					    u_xlat1.zw = vec2(0.25, 0.25) / u_xlat1.xy;
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat2.x = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat15 = u_xlat15 * u_xlat2.x;
					    u_xlat2.x = u_xlat15 * 1.5;
					    u_xlat7 = (-_PerspectiveFilter) + 1.0;
					    u_xlat7 = u_xlat7 * abs(u_xlat2.x);
					    u_xlat15 = u_xlat15 * 1.5 + (-u_xlat7);
					    u_xlat10 = abs(u_xlat10) * u_xlat15 + u_xlat7;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb15 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb15 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat10 = (u_xlatb15) ? u_xlat10 : u_xlat2.x;
					    u_xlat15 = _OutlineSoftness * _ScaleRatioA;
					    u_xlat15 = u_xlat15 * u_xlat10 + 1.0;
					    u_xlat2.x = u_xlat10 / u_xlat15;
					    u_xlat10 = _OutlineWidth * _ScaleRatioA;
					    u_xlat10 = u_xlat2.x * u_xlat10;
					    u_xlat15 = min(u_xlat10, 1.0);
					    u_xlat15 = sqrt(u_xlat15);
					    u_xlat4.x = in_COLOR0.w * _OutlineColor.w;
					    u_xlat4.xyz = _OutlineColor.xyz * u_xlat4.xxx + (-u_xlat16_3.xyz);
					    u_xlat4.w = _OutlineColor.w * in_COLOR0.w + (-u_xlat16_3.w);
					    u_xlat3 = vec4(u_xlat15) * u_xlat4 + u_xlat16_3;
					    vs_COLOR1 = u_xlat3;
					    u_xlat3 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat3 = min(u_xlat3, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat4.xy = u_xlat0.xy + (-u_xlat3.xy);
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat3.xy);
					    u_xlat1.xy = vec2((-u_xlat3.z) + u_xlat0.x, (-u_xlat3.w) + u_xlat0.y);
					    u_xlat0.xy = vec2((-u_xlat3.x) + u_xlat3.z, (-u_xlat3.y) + u_xlat3.w);
					    vs_TEXCOORD0.zw = u_xlat4.xy / u_xlat0.xy;
					    vs_TEXCOORD2 = u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb0 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat0.x = u_xlat0.x * u_xlat5 + _WeightNormal;
					    u_xlat0.x = u_xlat0.x * 0.25 + _FaceDilate;
					    u_xlat0.x = u_xlat0.x * _ScaleRatioA;
					    u_xlat0.x = (-u_xlat0.x) * 0.5 + 0.5;
					    u_xlat2.w = u_xlat0.x * u_xlat2.x + -0.5;
					    u_xlat2.y = (-u_xlat10) * 0.5 + u_xlat2.w;
					    u_xlat2.z = u_xlat10 * 0.5 + u_xlat2.w;
					    vs_TEXCOORD1 = u_xlat2;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec4 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					layout(location = 0) out mediump vec4 SV_Target0;
					lowp float u_xlat10_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 * vs_TEXCOORD1.x + (-vs_TEXCOORD1.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    SV_Target0 = vec4(u_xlat16_1) * vs_COLOR0;
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
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _FaceColor;
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _WeightNormal;
					uniform highp float _WeightBold;
					uniform highp float _ScaleRatioA;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					uniform highp float _GradientScale;
					uniform highp float _ScaleX;
					uniform highp float _ScaleY;
					uniform highp float _PerspectiveFilter;
					varying lowp vec4 xlv_COLOR;
					varying lowp vec4 xlv_COLOR1;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  lowp vec4 outlineColor_3;
					  lowp vec4 faceColor_4;
					  highp float opacity_5;
					  highp float scale_6;
					  highp vec2 pixelSize_7;
					  highp vec4 vert_8;
					  highp float tmpvar_9;
					  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_8.zw = _glesVertex.zw;
					  vert_8.x = (_glesVertex.x + _VertexOffsetX);
					  vert_8.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_10;
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = vert_8.xyz;
					  tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
					  highp vec2 tmpvar_12;
					  tmpvar_12.x = _ScaleX;
					  tmpvar_12.y = _ScaleY;
					  highp mat2 tmpvar_13;
					  tmpvar_13[0] = glstate_matrix_projection[0].xy;
					  tmpvar_13[1] = glstate_matrix_projection[1].xy;
					  pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
					    (tmpvar_13 * _ScreenParams.xy)
					  )));
					  scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_14;
					    tmpvar_14[0] = unity_WorldToObject[0].xyz;
					    tmpvar_14[1] = unity_WorldToObject[1].xyz;
					    tmpvar_14[2] = unity_WorldToObject[2].xyz;
					    scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
					      normalize((_glesNormal * tmpvar_14))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
					    )));
					  };
					  scale_6 = (scale_6 / (1.0 + (
					    (_OutlineSoftness * _ScaleRatioA)
					   * scale_6)));
					  highp float tmpvar_15;
					  tmpvar_15 = (((0.5 - 
					    ((((
					      mix (_WeightNormal, _WeightBold, tmpvar_9)
					     / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
					  ) * scale_6) - 0.5);
					  highp float tmpvar_16;
					  tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
					  lowp float tmpvar_17;
					  tmpvar_17 = tmpvar_1.w;
					  opacity_5 = tmpvar_17;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xyz = tmpvar_1.xyz;
					  tmpvar_18.w = opacity_5;
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (tmpvar_18 * _FaceColor);
					  faceColor_4 = tmpvar_19;
					  faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
					  outlineColor_3.xyz = _OutlineColor.xyz;
					  outlineColor_3.w = (_OutlineColor.w * opacity_5);
					  outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
					  highp vec4 tmpvar_20;
					  tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
					    (tmpvar_16 * 2.0)
					  ))));
					  outlineColor_3 = tmpvar_20;
					  highp vec4 tmpvar_21;
					  tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_22;
					  tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
					  highp vec4 tmpvar_23;
					  tmpvar_23.xy = tmpvar_2;
					  tmpvar_23.z = tmpvar_22.x;
					  tmpvar_23.w = tmpvar_22.y;
					  highp vec4 tmpvar_24;
					  tmpvar_24.x = scale_6;
					  tmpvar_24.y = (tmpvar_15 - tmpvar_16);
					  tmpvar_24.z = (tmpvar_15 + tmpvar_16);
					  tmpvar_24.w = tmpvar_15;
					  highp vec2 tmpvar_25;
					  tmpvar_25.x = _MaskSoftnessX;
					  tmpvar_25.y = _MaskSoftnessY;
					  highp vec4 tmpvar_26;
					  tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
					  tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
					  mediump vec4 tmpvar_27;
					  mediump vec4 tmpvar_28;
					  tmpvar_27 = tmpvar_24;
					  tmpvar_28 = tmpvar_26;
					  gl_Position = tmpvar_10;
					  xlv_COLOR = faceColor_4;
					  xlv_COLOR1 = outlineColor_3;
					  xlv_TEXCOORD0 = tmpvar_23;
					  xlv_TEXCOORD1 = tmpvar_27;
					  xlv_TEXCOORD2 = tmpvar_28;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = (xlv_COLOR * tmpvar_4);
					  c_2 = tmpvar_5;
					  mediump float x_6;
					  x_6 = (c_2.w - 0.001);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_ALPHACLIP" }
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _WeightNormal;
					uniform 	float _WeightBold;
					uniform 	float _ScaleRatioA;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					uniform 	float _GradientScale;
					uniform 	float _ScaleX;
					uniform 	float _ScaleY;
					uniform 	float _PerspectiveFilter;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out mediump vec4 vs_COLOR1;
					out highp vec4 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					mediump vec4 u_xlat16_3;
					vec4 u_xlat4;
					float u_xlat5;
					float u_xlat7;
					float u_xlat10;
					float u_xlat15;
					bool u_xlatb15;
					void main()
					{
					    u_xlat0.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    u_xlat3 = u_xlat2.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    gl_Position = u_xlat2;
					    u_xlat16_3 = in_COLOR0 * _FaceColor;
					    u_xlat16_3.xyz = u_xlat16_3.www * u_xlat16_3.xyz;
					    vs_COLOR0 = u_xlat16_3;
					    u_xlat10 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat1.xyz = vec3(u_xlat10) * u_xlat1.xyz;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat2.xyz = vec3(u_xlat10) * u_xlat2.xyz;
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat1.xyz);
					    u_xlat1.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat1.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat1.xy;
					    u_xlat1.xy = vec2(abs(u_xlat1.x) * float(_ScaleX), abs(u_xlat1.y) * float(_ScaleY));
					    u_xlat1.xy = u_xlat2.ww / u_xlat1.xy;
					    u_xlat15 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat1.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.xy;
					    u_xlat1.zw = vec2(0.25, 0.25) / u_xlat1.xy;
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat2.x = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat15 = u_xlat15 * u_xlat2.x;
					    u_xlat2.x = u_xlat15 * 1.5;
					    u_xlat7 = (-_PerspectiveFilter) + 1.0;
					    u_xlat7 = u_xlat7 * abs(u_xlat2.x);
					    u_xlat15 = u_xlat15 * 1.5 + (-u_xlat7);
					    u_xlat10 = abs(u_xlat10) * u_xlat15 + u_xlat7;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb15 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb15 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat10 = (u_xlatb15) ? u_xlat10 : u_xlat2.x;
					    u_xlat15 = _OutlineSoftness * _ScaleRatioA;
					    u_xlat15 = u_xlat15 * u_xlat10 + 1.0;
					    u_xlat2.x = u_xlat10 / u_xlat15;
					    u_xlat10 = _OutlineWidth * _ScaleRatioA;
					    u_xlat10 = u_xlat2.x * u_xlat10;
					    u_xlat15 = min(u_xlat10, 1.0);
					    u_xlat15 = sqrt(u_xlat15);
					    u_xlat4.x = in_COLOR0.w * _OutlineColor.w;
					    u_xlat4.xyz = _OutlineColor.xyz * u_xlat4.xxx + (-u_xlat16_3.xyz);
					    u_xlat4.w = _OutlineColor.w * in_COLOR0.w + (-u_xlat16_3.w);
					    u_xlat3 = vec4(u_xlat15) * u_xlat4 + u_xlat16_3;
					    vs_COLOR1 = u_xlat3;
					    u_xlat3 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat3 = min(u_xlat3, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat4.xy = u_xlat0.xy + (-u_xlat3.xy);
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat3.xy);
					    u_xlat1.xy = vec2((-u_xlat3.z) + u_xlat0.x, (-u_xlat3.w) + u_xlat0.y);
					    u_xlat0.xy = vec2((-u_xlat3.x) + u_xlat3.z, (-u_xlat3.y) + u_xlat3.w);
					    vs_TEXCOORD0.zw = u_xlat4.xy / u_xlat0.xy;
					    vs_TEXCOORD2 = u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb0 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat0.x = u_xlat0.x * u_xlat5 + _WeightNormal;
					    u_xlat0.x = u_xlat0.x * 0.25 + _FaceDilate;
					    u_xlat0.x = u_xlat0.x * _ScaleRatioA;
					    u_xlat0.x = (-u_xlat0.x) * 0.5 + 0.5;
					    u_xlat2.w = u_xlat0.x * u_xlat2.x + -0.5;
					    u_xlat2.y = (-u_xlat10) * 0.5 + u_xlat2.w;
					    u_xlat2.z = u_xlat10 * 0.5 + u_xlat2.w;
					    vs_TEXCOORD1 = u_xlat2;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec4 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp float u_xlat10_0;
					mediump float u_xlat16_1;
					bool u_xlatb2;
					mediump float u_xlat16_4;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat16_1 = u_xlat10_0 * vs_TEXCOORD1.x + (-vs_TEXCOORD1.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_4 = vs_COLOR0.w * u_xlat16_1 + -0.00100000005;
					    u_xlat16_0 = vec4(u_xlat16_1) * vs_COLOR0;
					    SV_Target0 = u_xlat16_0;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb2 = !!(u_xlat16_4<0.0);
					#else
					    u_xlatb2 = u_xlat16_4<0.0;
					#endif
					    if((int(u_xlatb2) * int(0xffffffffu))!=0){discard;}
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
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _FaceColor;
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _WeightNormal;
					uniform highp float _WeightBold;
					uniform highp float _ScaleRatioA;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					uniform highp float _GradientScale;
					uniform highp float _ScaleX;
					uniform highp float _ScaleY;
					uniform highp float _PerspectiveFilter;
					varying lowp vec4 xlv_COLOR;
					varying lowp vec4 xlv_COLOR1;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  lowp vec4 outlineColor_3;
					  lowp vec4 faceColor_4;
					  highp float opacity_5;
					  highp float scale_6;
					  highp vec2 pixelSize_7;
					  highp vec4 vert_8;
					  highp float tmpvar_9;
					  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_8.zw = _glesVertex.zw;
					  vert_8.x = (_glesVertex.x + _VertexOffsetX);
					  vert_8.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_10;
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = vert_8.xyz;
					  tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
					  highp vec2 tmpvar_12;
					  tmpvar_12.x = _ScaleX;
					  tmpvar_12.y = _ScaleY;
					  highp mat2 tmpvar_13;
					  tmpvar_13[0] = glstate_matrix_projection[0].xy;
					  tmpvar_13[1] = glstate_matrix_projection[1].xy;
					  pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
					    (tmpvar_13 * _ScreenParams.xy)
					  )));
					  scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_14;
					    tmpvar_14[0] = unity_WorldToObject[0].xyz;
					    tmpvar_14[1] = unity_WorldToObject[1].xyz;
					    tmpvar_14[2] = unity_WorldToObject[2].xyz;
					    scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
					      normalize((_glesNormal * tmpvar_14))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
					    )));
					  };
					  scale_6 = (scale_6 / (1.0 + (
					    (_OutlineSoftness * _ScaleRatioA)
					   * scale_6)));
					  highp float tmpvar_15;
					  tmpvar_15 = (((0.5 - 
					    ((((
					      mix (_WeightNormal, _WeightBold, tmpvar_9)
					     / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
					  ) * scale_6) - 0.5);
					  highp float tmpvar_16;
					  tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
					  lowp float tmpvar_17;
					  tmpvar_17 = tmpvar_1.w;
					  opacity_5 = tmpvar_17;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xyz = tmpvar_1.xyz;
					  tmpvar_18.w = opacity_5;
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (tmpvar_18 * _FaceColor);
					  faceColor_4 = tmpvar_19;
					  faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
					  outlineColor_3.xyz = _OutlineColor.xyz;
					  outlineColor_3.w = (_OutlineColor.w * opacity_5);
					  outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
					  highp vec4 tmpvar_20;
					  tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
					    (tmpvar_16 * 2.0)
					  ))));
					  outlineColor_3 = tmpvar_20;
					  highp vec4 tmpvar_21;
					  tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_22;
					  tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
					  highp vec4 tmpvar_23;
					  tmpvar_23.xy = tmpvar_2;
					  tmpvar_23.z = tmpvar_22.x;
					  tmpvar_23.w = tmpvar_22.y;
					  highp vec4 tmpvar_24;
					  tmpvar_24.x = scale_6;
					  tmpvar_24.y = (tmpvar_15 - tmpvar_16);
					  tmpvar_24.z = (tmpvar_15 + tmpvar_16);
					  tmpvar_24.w = tmpvar_15;
					  highp vec2 tmpvar_25;
					  tmpvar_25.x = _MaskSoftnessX;
					  tmpvar_25.y = _MaskSoftnessY;
					  highp vec4 tmpvar_26;
					  tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
					  tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
					  mediump vec4 tmpvar_27;
					  mediump vec4 tmpvar_28;
					  tmpvar_27 = tmpvar_24;
					  tmpvar_28 = tmpvar_26;
					  gl_Position = tmpvar_10;
					  xlv_COLOR = faceColor_4;
					  xlv_COLOR1 = outlineColor_3;
					  xlv_TEXCOORD0 = tmpvar_23;
					  xlv_TEXCOORD1 = tmpvar_27;
					  xlv_TEXCOORD2 = tmpvar_28;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _ClipRect;
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = (xlv_COLOR * tmpvar_4);
					  c_2 = tmpvar_5;
					  mediump vec2 tmpvar_6;
					  tmpvar_6 = abs(xlv_TEXCOORD2.xy);
					  mediump vec2 tmpvar_7;
					  highp vec2 tmpvar_8;
					  tmpvar_8 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_7 = tmpvar_8;
					  c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_CLIP_RECT" }
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _WeightNormal;
					uniform 	float _WeightBold;
					uniform 	float _ScaleRatioA;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					uniform 	float _GradientScale;
					uniform 	float _ScaleX;
					uniform 	float _ScaleY;
					uniform 	float _PerspectiveFilter;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out mediump vec4 vs_COLOR1;
					out highp vec4 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					mediump vec4 u_xlat16_3;
					vec4 u_xlat4;
					float u_xlat5;
					float u_xlat7;
					float u_xlat10;
					float u_xlat15;
					bool u_xlatb15;
					void main()
					{
					    u_xlat0.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    u_xlat3 = u_xlat2.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    gl_Position = u_xlat2;
					    u_xlat16_3 = in_COLOR0 * _FaceColor;
					    u_xlat16_3.xyz = u_xlat16_3.www * u_xlat16_3.xyz;
					    vs_COLOR0 = u_xlat16_3;
					    u_xlat10 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat1.xyz = vec3(u_xlat10) * u_xlat1.xyz;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat2.xyz = vec3(u_xlat10) * u_xlat2.xyz;
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat1.xyz);
					    u_xlat1.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat1.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat1.xy;
					    u_xlat1.xy = vec2(abs(u_xlat1.x) * float(_ScaleX), abs(u_xlat1.y) * float(_ScaleY));
					    u_xlat1.xy = u_xlat2.ww / u_xlat1.xy;
					    u_xlat15 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat1.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.xy;
					    u_xlat1.zw = vec2(0.25, 0.25) / u_xlat1.xy;
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat2.x = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat15 = u_xlat15 * u_xlat2.x;
					    u_xlat2.x = u_xlat15 * 1.5;
					    u_xlat7 = (-_PerspectiveFilter) + 1.0;
					    u_xlat7 = u_xlat7 * abs(u_xlat2.x);
					    u_xlat15 = u_xlat15 * 1.5 + (-u_xlat7);
					    u_xlat10 = abs(u_xlat10) * u_xlat15 + u_xlat7;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb15 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb15 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat10 = (u_xlatb15) ? u_xlat10 : u_xlat2.x;
					    u_xlat15 = _OutlineSoftness * _ScaleRatioA;
					    u_xlat15 = u_xlat15 * u_xlat10 + 1.0;
					    u_xlat2.x = u_xlat10 / u_xlat15;
					    u_xlat10 = _OutlineWidth * _ScaleRatioA;
					    u_xlat10 = u_xlat2.x * u_xlat10;
					    u_xlat15 = min(u_xlat10, 1.0);
					    u_xlat15 = sqrt(u_xlat15);
					    u_xlat4.x = in_COLOR0.w * _OutlineColor.w;
					    u_xlat4.xyz = _OutlineColor.xyz * u_xlat4.xxx + (-u_xlat16_3.xyz);
					    u_xlat4.w = _OutlineColor.w * in_COLOR0.w + (-u_xlat16_3.w);
					    u_xlat3 = vec4(u_xlat15) * u_xlat4 + u_xlat16_3;
					    vs_COLOR1 = u_xlat3;
					    u_xlat3 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat3 = min(u_xlat3, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat4.xy = u_xlat0.xy + (-u_xlat3.xy);
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat3.xy);
					    u_xlat1.xy = vec2((-u_xlat3.z) + u_xlat0.x, (-u_xlat3.w) + u_xlat0.y);
					    u_xlat0.xy = vec2((-u_xlat3.x) + u_xlat3.z, (-u_xlat3.y) + u_xlat3.w);
					    vs_TEXCOORD0.zw = u_xlat4.xy / u_xlat0.xy;
					    vs_TEXCOORD2 = u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb0 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat0.x = u_xlat0.x * u_xlat5 + _WeightNormal;
					    u_xlat0.x = u_xlat0.x * 0.25 + _FaceDilate;
					    u_xlat0.x = u_xlat0.x * _ScaleRatioA;
					    u_xlat0.x = (-u_xlat0.x) * 0.5 + 0.5;
					    u_xlat2.w = u_xlat0.x * u_xlat2.x + -0.5;
					    u_xlat2.y = (-u_xlat10) * 0.5 + u_xlat2.w;
					    u_xlat2.z = u_xlat10 * 0.5 + u_xlat2.w;
					    vs_TEXCOORD1 = u_xlat2;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec4 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					in mediump vec4 vs_TEXCOORD2;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump vec4 u_xlat16_0;
					lowp float u_xlat10_0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_3;
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
					    u_xlat16_3 = u_xlat10_0 * vs_TEXCOORD1.x + (-vs_TEXCOORD1.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_3 = min(max(u_xlat16_3, 0.0), 1.0);
					#else
					    u_xlat16_3 = clamp(u_xlat16_3, 0.0, 1.0);
					#endif
					    u_xlat16_0 = vec4(u_xlat16_3) * vs_COLOR0;
					    SV_Target0 = vec4(u_xlat16_1) * u_xlat16_0;
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
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 glstate_matrix_projection;
					uniform highp mat4 unity_MatrixVP;
					uniform lowp vec4 _FaceColor;
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _WeightNormal;
					uniform highp float _WeightBold;
					uniform highp float _ScaleRatioA;
					uniform highp float _VertexOffsetX;
					uniform highp float _VertexOffsetY;
					uniform highp vec4 _ClipRect;
					uniform highp float _MaskSoftnessX;
					uniform highp float _MaskSoftnessY;
					uniform highp float _GradientScale;
					uniform highp float _ScaleX;
					uniform highp float _ScaleY;
					uniform highp float _PerspectiveFilter;
					varying lowp vec4 xlv_COLOR;
					varying lowp vec4 xlv_COLOR1;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  lowp vec4 outlineColor_3;
					  lowp vec4 faceColor_4;
					  highp float opacity_5;
					  highp float scale_6;
					  highp vec2 pixelSize_7;
					  highp vec4 vert_8;
					  highp float tmpvar_9;
					  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_8.zw = _glesVertex.zw;
					  vert_8.x = (_glesVertex.x + _VertexOffsetX);
					  vert_8.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_10;
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = vert_8.xyz;
					  tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
					  highp vec2 tmpvar_12;
					  tmpvar_12.x = _ScaleX;
					  tmpvar_12.y = _ScaleY;
					  highp mat2 tmpvar_13;
					  tmpvar_13[0] = glstate_matrix_projection[0].xy;
					  tmpvar_13[1] = glstate_matrix_projection[1].xy;
					  pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
					    (tmpvar_13 * _ScreenParams.xy)
					  )));
					  scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_14;
					    tmpvar_14[0] = unity_WorldToObject[0].xyz;
					    tmpvar_14[1] = unity_WorldToObject[1].xyz;
					    tmpvar_14[2] = unity_WorldToObject[2].xyz;
					    scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
					      normalize((_glesNormal * tmpvar_14))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
					    )));
					  };
					  scale_6 = (scale_6 / (1.0 + (
					    (_OutlineSoftness * _ScaleRatioA)
					   * scale_6)));
					  highp float tmpvar_15;
					  tmpvar_15 = (((0.5 - 
					    ((((
					      mix (_WeightNormal, _WeightBold, tmpvar_9)
					     / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
					  ) * scale_6) - 0.5);
					  highp float tmpvar_16;
					  tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
					  lowp float tmpvar_17;
					  tmpvar_17 = tmpvar_1.w;
					  opacity_5 = tmpvar_17;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xyz = tmpvar_1.xyz;
					  tmpvar_18.w = opacity_5;
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (tmpvar_18 * _FaceColor);
					  faceColor_4 = tmpvar_19;
					  faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
					  outlineColor_3.xyz = _OutlineColor.xyz;
					  outlineColor_3.w = (_OutlineColor.w * opacity_5);
					  outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
					  highp vec4 tmpvar_20;
					  tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
					    (tmpvar_16 * 2.0)
					  ))));
					  outlineColor_3 = tmpvar_20;
					  highp vec4 tmpvar_21;
					  tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_22;
					  tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
					  highp vec4 tmpvar_23;
					  tmpvar_23.xy = tmpvar_2;
					  tmpvar_23.z = tmpvar_22.x;
					  tmpvar_23.w = tmpvar_22.y;
					  highp vec4 tmpvar_24;
					  tmpvar_24.x = scale_6;
					  tmpvar_24.y = (tmpvar_15 - tmpvar_16);
					  tmpvar_24.z = (tmpvar_15 + tmpvar_16);
					  tmpvar_24.w = tmpvar_15;
					  highp vec2 tmpvar_25;
					  tmpvar_25.x = _MaskSoftnessX;
					  tmpvar_25.y = _MaskSoftnessY;
					  highp vec4 tmpvar_26;
					  tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
					  tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
					  mediump vec4 tmpvar_27;
					  mediump vec4 tmpvar_28;
					  tmpvar_27 = tmpvar_24;
					  tmpvar_28 = tmpvar_26;
					  gl_Position = tmpvar_10;
					  xlv_COLOR = faceColor_4;
					  xlv_COLOR1 = outlineColor_3;
					  xlv_TEXCOORD0 = tmpvar_23;
					  xlv_TEXCOORD1 = tmpvar_27;
					  xlv_TEXCOORD2 = tmpvar_28;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _ClipRect;
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = (xlv_COLOR * tmpvar_4);
					  c_2 = tmpvar_5;
					  mediump vec2 tmpvar_6;
					  tmpvar_6 = abs(xlv_TEXCOORD2.xy);
					  mediump vec2 tmpvar_7;
					  highp vec2 tmpvar_8;
					  tmpvar_8 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_7 = tmpvar_8;
					  c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
					  mediump float x_9;
					  x_9 = (c_2.w - 0.001);
					  if ((x_9 < 0.0)) {
					    discard;
					  };
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 " {
					Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 _ScreenParams;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_projection[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _WeightNormal;
					uniform 	float _WeightBold;
					uniform 	float _ScaleRatioA;
					uniform 	float _VertexOffsetX;
					uniform 	float _VertexOffsetY;
					uniform 	vec4 _ClipRect;
					uniform 	float _MaskSoftnessX;
					uniform 	float _MaskSoftnessY;
					uniform 	float _GradientScale;
					uniform 	float _ScaleX;
					uniform 	float _ScaleY;
					uniform 	float _PerspectiveFilter;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out mediump vec4 vs_COLOR1;
					out highp vec4 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					vec2 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					mediump vec4 u_xlat16_3;
					vec4 u_xlat4;
					float u_xlat5;
					float u_xlat7;
					float u_xlat10;
					float u_xlat15;
					bool u_xlatb15;
					void main()
					{
					    u_xlat0.xy = vec2(in_POSITION0.x + float(_VertexOffsetX), in_POSITION0.y + float(_VertexOffsetY));
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    u_xlat3 = u_xlat2.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    u_xlat2 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    gl_Position = u_xlat2;
					    u_xlat16_3 = in_COLOR0 * _FaceColor;
					    u_xlat16_3.xyz = u_xlat16_3.www * u_xlat16_3.xyz;
					    vs_COLOR0 = u_xlat16_3;
					    u_xlat10 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat1.xyz = vec3(u_xlat10) * u_xlat1.xyz;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat10 = inversesqrt(u_xlat10);
					    u_xlat2.xyz = vec3(u_xlat10) * u_xlat2.xyz;
					    u_xlat10 = dot(u_xlat2.xyz, u_xlat1.xyz);
					    u_xlat1.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat1.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat1.xy;
					    u_xlat1.xy = vec2(abs(u_xlat1.x) * float(_ScaleX), abs(u_xlat1.y) * float(_ScaleY));
					    u_xlat1.xy = u_xlat2.ww / u_xlat1.xy;
					    u_xlat15 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat1.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat1.xy;
					    u_xlat1.zw = vec2(0.25, 0.25) / u_xlat1.xy;
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat2.x = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat15 = u_xlat15 * u_xlat2.x;
					    u_xlat2.x = u_xlat15 * 1.5;
					    u_xlat7 = (-_PerspectiveFilter) + 1.0;
					    u_xlat7 = u_xlat7 * abs(u_xlat2.x);
					    u_xlat15 = u_xlat15 * 1.5 + (-u_xlat7);
					    u_xlat10 = abs(u_xlat10) * u_xlat15 + u_xlat7;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb15 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb15 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat10 = (u_xlatb15) ? u_xlat10 : u_xlat2.x;
					    u_xlat15 = _OutlineSoftness * _ScaleRatioA;
					    u_xlat15 = u_xlat15 * u_xlat10 + 1.0;
					    u_xlat2.x = u_xlat10 / u_xlat15;
					    u_xlat10 = _OutlineWidth * _ScaleRatioA;
					    u_xlat10 = u_xlat2.x * u_xlat10;
					    u_xlat15 = min(u_xlat10, 1.0);
					    u_xlat15 = sqrt(u_xlat15);
					    u_xlat4.x = in_COLOR0.w * _OutlineColor.w;
					    u_xlat4.xyz = _OutlineColor.xyz * u_xlat4.xxx + (-u_xlat16_3.xyz);
					    u_xlat4.w = _OutlineColor.w * in_COLOR0.w + (-u_xlat16_3.w);
					    u_xlat3 = vec4(u_xlat15) * u_xlat4 + u_xlat16_3;
					    vs_COLOR1 = u_xlat3;
					    u_xlat3 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat3 = min(u_xlat3, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat4.xy = u_xlat0.xy + (-u_xlat3.xy);
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat3.xy);
					    u_xlat1.xy = vec2((-u_xlat3.z) + u_xlat0.x, (-u_xlat3.w) + u_xlat0.y);
					    u_xlat0.xy = vec2((-u_xlat3.x) + u_xlat3.z, (-u_xlat3.y) + u_xlat3.w);
					    vs_TEXCOORD0.zw = u_xlat4.xy / u_xlat0.xy;
					    vs_TEXCOORD2 = u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb0 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb0 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat0.x = u_xlat0.x * u_xlat5 + _WeightNormal;
					    u_xlat0.x = u_xlat0.x * 0.25 + _FaceDilate;
					    u_xlat0.x = u_xlat0.x * _ScaleRatioA;
					    u_xlat0.x = (-u_xlat0.x) * 0.5 + 0.5;
					    u_xlat2.w = u_xlat0.x * u_xlat2.x + -0.5;
					    u_xlat2.y = (-u_xlat10) * 0.5 + u_xlat2.w;
					    u_xlat2.z = u_xlat10 * 0.5 + u_xlat2.w;
					    vs_TEXCOORD1 = u_xlat2;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					in mediump vec4 vs_COLOR0;
					in highp vec4 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					in mediump vec4 vs_TEXCOORD2;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump vec4 u_xlat16_0;
					lowp float u_xlat10_0;
					mediump float u_xlat16_1;
					bool u_xlatb2;
					mediump float u_xlat16_4;
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
					    u_xlat16_4 = u_xlat10_0 * vs_TEXCOORD1.x + (-vs_TEXCOORD1.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_4 = min(max(u_xlat16_4, 0.0), 1.0);
					#else
					    u_xlat16_4 = clamp(u_xlat16_4, 0.0, 1.0);
					#endif
					    u_xlat16_0 = vec4(u_xlat16_4) * vs_COLOR0;
					    u_xlat16_4 = u_xlat16_0.w * u_xlat16_1 + -0.00100000005;
					    u_xlat16_0 = vec4(u_xlat16_1) * u_xlat16_0;
					    SV_Target0 = u_xlat16_0;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb2 = !!(u_xlat16_4<0.0);
					#else
					    u_xlatb2 = u_xlat16_4<0.0;
					#endif
					    if((int(u_xlatb2) * int(0xffffffffu))!=0){discard;}
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
	CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}