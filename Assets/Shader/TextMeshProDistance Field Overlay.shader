Shader "TextMeshPro/Distance Field Overlay" {
	Properties {
		_FaceTex ("Face Texture", 2D) = "white" {}
		_FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
		_FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
		_FaceColor ("Face Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineTex ("Outline Texture", 2D) = "white" {}
		_OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
		_OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
		_Bevel ("Bevel", Range(0, 1)) = 0.5
		_BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
		_BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
		_BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
		_BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
		_LightAngle ("Light Angle", Range(0, 6.283185)) = 3.1416
		_SpecularColor ("Specular", Vector) = (1,1,1,1)
		_SpecularPower ("Specular", Range(0, 4)) = 2
		_Reflectivity ("Reflectivity", Range(5, 15)) = 10
		_Diffuse ("Diffuse", Range(0, 1)) = 0.5
		_Ambient ("Ambient", Range(1, 0)) = 0.5
		_BumpMap ("Normal map", 2D) = "bump" {}
		_BumpOutline ("Bump Outline", Range(0, 1)) = 0
		_BumpFace ("Bump Face", Range(0, 1)) = 0
		_ReflectFaceColor ("Reflection Color", Vector) = (0,0,0,1)
		_ReflectOutlineColor ("Reflection Color", Vector) = (0,0,0,1)
		_Cube ("Reflection Cubemap", Cube) = "black" {}
		_EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
		_UnderlayColor ("Border Color", Vector) = (0,0,0,0.5)
		_UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
		_UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
		_UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
		_UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
		_GlowColor ("Color", Vector) = (0,1,0,0.5)
		_GlowOffset ("Offset", Range(-1, 1)) = 0
		_GlowInner ("Inner", Range(0, 1)) = 0.05
		_GlowOuter ("Outer", Range(0, 1)) = 0.05
		_GlowPower ("Falloff", Range(1, 0)) = 0.75
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
		_MaskCoord ("Mask Coordinates", Vector) = (0,0,32767,32767)
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
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Overlay" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Overlay" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ColorMask 0 -1
			ZTest Always
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
			GpuProgramID 5929
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
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform highp float _OutlineWidth;
					uniform highp mat4 _EnvMatrix;
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
					uniform highp vec4 _FaceTex_ST;
					uniform highp vec4 _OutlineTex_ST;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD3;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  highp float weight_3;
					  highp float scale_4;
					  highp vec2 pixelSize_5;
					  highp vec4 vert_6;
					  highp float tmpvar_7;
					  tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_6.zw = _glesVertex.zw;
					  vert_6.x = (_glesVertex.x + _VertexOffsetX);
					  vert_6.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_8;
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = vert_6.xyz;
					  tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _ScaleX;
					  tmpvar_10.y = _ScaleY;
					  highp mat2 tmpvar_11;
					  tmpvar_11[0] = glstate_matrix_projection[0].xy;
					  tmpvar_11[1] = glstate_matrix_projection[1].xy;
					  pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
					    (tmpvar_11 * _ScreenParams.xy)
					  )));
					  scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_12;
					    tmpvar_12[0] = unity_WorldToObject[0].xyz;
					    tmpvar_12[1] = unity_WorldToObject[1].xyz;
					    tmpvar_12[2] = unity_WorldToObject[2].xyz;
					    scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
					      normalize((_glesNormal * tmpvar_12))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
					    )));
					  };
					  weight_3 = (((
					    (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
					   + _FaceDilate) * _ScaleRatioA) * 0.5);
					  highp vec4 tmpvar_13;
					  tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_14;
					  highp vec2 xlat_varoutput_15;
					  xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
					  tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
					  highp vec4 tmpvar_16;
					  tmpvar_16.x = (((
					    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
					   / 2.0) - (0.5 / scale_4)) - weight_3);
					  tmpvar_16.y = scale_4;
					  tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
					  tmpvar_16.w = weight_3;
					  highp vec2 tmpvar_17;
					  tmpvar_17.x = _MaskSoftnessX;
					  tmpvar_17.y = _MaskSoftnessY;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
					  tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _EnvMatrix[0].xyz;
					  tmpvar_19[1] = _EnvMatrix[1].xyz;
					  tmpvar_19[2] = _EnvMatrix[2].xyz;
					  highp vec4 tmpvar_20;
					  tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
					  gl_Position = tmpvar_8;
					  xlv_COLOR = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_16;
					  xlv_TEXCOORD2 = tmpvar_18;
					  xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
					  xlv_TEXCOORD5 = tmpvar_20;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform sampler2D _FaceTex;
					uniform highp float _FaceUVSpeedX;
					uniform highp float _FaceUVSpeedY;
					uniform lowp vec4 _FaceColor;
					uniform highp float _OutlineSoftness;
					uniform sampler2D _OutlineTex;
					uniform highp float _OutlineUVSpeedX;
					uniform highp float _OutlineUVSpeedY;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _ScaleRatioA;
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 outlineColor_2;
					  mediump vec4 faceColor_3;
					  highp float c_4;
					  lowp float tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
					  c_4 = tmpvar_5;
					  highp float x_6;
					  x_6 = (c_4 - xlv_TEXCOORD1.x);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  highp float tmpvar_7;
					  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
					  highp float tmpvar_8;
					  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  highp float tmpvar_9;
					  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  faceColor_3 = _FaceColor;
					  outlineColor_2 = _OutlineColor;
					  faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _FaceUVSpeedX;
					  tmpvar_10.y = _FaceUVSpeedY;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
					  tmpvar_11 = texture2D (_FaceTex, P_12);
					  faceColor_3 = (faceColor_3 * tmpvar_11);
					  highp vec2 tmpvar_13;
					  tmpvar_13.x = _OutlineUVSpeedX;
					  tmpvar_13.y = _OutlineUVSpeedY;
					  lowp vec4 tmpvar_14;
					  highp vec2 P_15;
					  P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
					  tmpvar_14 = texture2D (_OutlineTex, P_15);
					  outlineColor_2 = (outlineColor_2 * tmpvar_14);
					  mediump float d_16;
					  d_16 = tmpvar_7;
					  lowp vec4 faceColor_17;
					  faceColor_17 = faceColor_3;
					  lowp vec4 outlineColor_18;
					  outlineColor_18 = outlineColor_2;
					  mediump float outline_19;
					  outline_19 = tmpvar_8;
					  mediump float softness_20;
					  softness_20 = tmpvar_9;
					  mediump float tmpvar_21;
					  tmpvar_21 = (1.0 - clamp ((
					    ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
					   / 
					    (1.0 + softness_20)
					  ), 0.0, 1.0));
					  faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
					  outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
					  mediump vec4 tmpvar_22;
					  tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
					    (d_16 + (outline_19 * 0.5))
					  , 0.0, 1.0) * sqrt(
					    min (1.0, outline_19)
					  ))));
					  faceColor_17 = tmpvar_22;
					  faceColor_17 = (faceColor_17 * tmpvar_21);
					  faceColor_3 = faceColor_17;
					  tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineWidth;
					uniform 	vec4 hlslcc_mtx4x4_EnvMatrix[4];
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
					uniform 	vec4 _FaceTex_ST;
					uniform 	vec4 _OutlineTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					out highp vec3 vs_TEXCOORD3;
					out highp vec4 vs_TEXCOORD5;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat6;
					vec2 u_xlat8;
					bool u_xlatb8;
					float u_xlat12;
					bool u_xlatb12;
					float u_xlat13;
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
					    vs_COLOR0 = in_COLOR0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat2.xyz = u_xlat8.xxx * u_xlat2.xyz;
					    u_xlat8.x = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat3.xyz = u_xlat8.xxx * u_xlat1.xyz;
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat3.xyz);
					    u_xlat2.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat2.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = vec2(abs(u_xlat2.x) * float(_ScaleX), abs(u_xlat2.y) * float(_ScaleY));
					    u_xlat2.xy = u_xlat2.ww / u_xlat2.xy;
					    u_xlat12 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat2.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat2.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat2.xy;
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat13 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat12 = u_xlat12 * u_xlat13;
					    u_xlat13 = u_xlat12 * 1.5;
					    u_xlat2.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat2.x = abs(u_xlat13) * u_xlat2.x;
					    u_xlat12 = u_xlat12 * 1.5 + (-u_xlat2.x);
					    u_xlat8.x = abs(u_xlat8.x) * u_xlat12 + u_xlat2.x;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb12 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb12 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat6.x = (u_xlatb12) ? u_xlat8.x : u_xlat13;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb8 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb8 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat8.x = u_xlatb8 ? 1.0 : float(0.0);
					    u_xlat12 = (-_WeightNormal) + _WeightBold;
					    u_xlat8.x = u_xlat8.x * u_xlat12 + _WeightNormal;
					    u_xlat8.x = u_xlat8.x * 0.25 + _FaceDilate;
					    u_xlat8.x = u_xlat8.x * _ScaleRatioA;
					    u_xlat6.z = u_xlat8.x * 0.5;
					    vs_TEXCOORD1.yw = u_xlat6.xz;
					    u_xlat12 = 0.5 / u_xlat6.x;
					    u_xlat13 = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat13 = (-_OutlineSoftness) * _ScaleRatioA + u_xlat13;
					    u_xlat13 = u_xlat13 * 0.5 + (-u_xlat12);
					    vs_TEXCOORD1.x = (-u_xlat8.x) * 0.5 + u_xlat13;
					    u_xlat8.x = (-u_xlat8.x) * 0.5 + 0.5;
					    vs_TEXCOORD1.z = u_xlat12 + u_xlat8.x;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat0.xyz = u_xlat1.yyy * hlslcc_mtx4x4_EnvMatrix[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4_EnvMatrix[0].xyz * u_xlat1.xxx + u_xlat0.xyz;
					    vs_TEXCOORD3.xyz = hlslcc_mtx4x4_EnvMatrix[2].xyz * u_xlat1.zzz + u_xlat0.xyz;
					    u_xlat0.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat8.x = floor(u_xlat0.x);
					    u_xlat8.y = (-u_xlat8.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat0.xy = u_xlat8.xy * vec2(0.001953125, 0.001953125);
					    vs_TEXCOORD5.xy = u_xlat0.xy * _FaceTex_ST.xy + _FaceTex_ST.zw;
					    vs_TEXCOORD5.zw = u_xlat0.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	float _FaceUVSpeedX;
					uniform 	float _FaceUVSpeedY;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineUVSpeedX;
					uniform 	float _OutlineUVSpeedY;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _ScaleRatioA;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					uniform lowp sampler2D _OutlineTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec4 vs_TEXCOORD5;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					mediump vec4 u_xlat16_2;
					mediump vec3 u_xlat16_3;
					float u_xlat4;
					mediump float u_xlat16_4;
					lowp vec4 u_xlat10_4;
					float u_xlat5;
					bool u_xlatb5;
					mediump float u_xlat16_6;
					float u_xlat9;
					mediump float u_xlat16_11;
					void main()
					{
					    u_xlat10_0.x = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat5 = u_xlat10_0.x + (-vs_TEXCOORD1.x);
					    u_xlat0.x = (-u_xlat10_0.x) + vs_TEXCOORD1.z;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb5 = !!(u_xlat5<0.0);
					#else
					    u_xlatb5 = u_xlat5<0.0;
					#endif
					    if((int(u_xlatb5) * int(0xffffffffu))!=0){discard;}
					    u_xlat5 = _OutlineWidth * _ScaleRatioA;
					    u_xlat5 = u_xlat5 * vs_TEXCOORD1.y;
					    u_xlat16_1 = min(u_xlat5, 1.0);
					    u_xlat16_6 = u_xlat5 * 0.5;
					    u_xlat16_1 = sqrt(u_xlat16_1);
					    u_xlat16_11 = u_xlat0.x * vs_TEXCOORD1.y + u_xlat16_6;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_11 = min(max(u_xlat16_11, 0.0), 1.0);
					#else
					    u_xlat16_11 = clamp(u_xlat16_11, 0.0, 1.0);
					#endif
					    u_xlat16_6 = u_xlat0.x * vs_TEXCOORD1.y + (-u_xlat16_6);
					    u_xlat16_1 = u_xlat16_1 * u_xlat16_11;
					    u_xlat0.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD5.zw;
					    u_xlat10_0 = texture(_OutlineTex, u_xlat0.xy);
					    u_xlat16_2 = u_xlat10_0 * _OutlineColor;
					    u_xlat16_3.xyz = vs_COLOR0.xyz * _FaceColor.xyz;
					    u_xlat0.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD5.xy;
					    u_xlat10_4 = texture(_FaceTex, u_xlat0.xy);
					    u_xlat16_0.xyz = u_xlat16_3.xyz * u_xlat10_4.xyz;
					    u_xlat16_4 = u_xlat10_4.w * _FaceColor.w;
					    u_xlat16_3.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4);
					    u_xlat16_2.xyz = u_xlat16_2.xyz * u_xlat16_2.www + (-u_xlat16_3.xyz);
					    u_xlat16_2.w = _OutlineColor.w * u_xlat10_0.w + (-u_xlat16_4);
					    u_xlat16_2 = vec4(u_xlat16_1) * u_xlat16_2;
					    u_xlat16_0.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4) + u_xlat16_2.xyz;
					    u_xlat16_0.w = _FaceColor.w * u_xlat10_4.w + u_xlat16_2.w;
					    u_xlat4 = _OutlineSoftness * _ScaleRatioA;
					    u_xlat9 = u_xlat4 * vs_TEXCOORD1.y;
					    u_xlat16_1 = u_xlat4 * vs_TEXCOORD1.y + 1.0;
					    u_xlat16_6 = u_xlat9 * 0.5 + u_xlat16_6;
					    u_xlat16_1 = u_xlat16_6 / u_xlat16_1;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = (-u_xlat16_1) + 1.0;
					    u_xlat16_0 = u_xlat16_0 * vec4(u_xlat16_1);
					    SV_Target0 = u_xlat16_0 * vs_COLOR0.wwww;
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
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform highp float _OutlineWidth;
					uniform highp mat4 _EnvMatrix;
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
					uniform highp vec4 _FaceTex_ST;
					uniform highp vec4 _OutlineTex_ST;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD3;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  highp float weight_3;
					  highp float scale_4;
					  highp vec2 pixelSize_5;
					  highp vec4 vert_6;
					  highp float tmpvar_7;
					  tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_6.zw = _glesVertex.zw;
					  vert_6.x = (_glesVertex.x + _VertexOffsetX);
					  vert_6.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_8;
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = vert_6.xyz;
					  tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _ScaleX;
					  tmpvar_10.y = _ScaleY;
					  highp mat2 tmpvar_11;
					  tmpvar_11[0] = glstate_matrix_projection[0].xy;
					  tmpvar_11[1] = glstate_matrix_projection[1].xy;
					  pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
					    (tmpvar_11 * _ScreenParams.xy)
					  )));
					  scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_12;
					    tmpvar_12[0] = unity_WorldToObject[0].xyz;
					    tmpvar_12[1] = unity_WorldToObject[1].xyz;
					    tmpvar_12[2] = unity_WorldToObject[2].xyz;
					    scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
					      normalize((_glesNormal * tmpvar_12))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
					    )));
					  };
					  weight_3 = (((
					    (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
					   + _FaceDilate) * _ScaleRatioA) * 0.5);
					  highp vec4 tmpvar_13;
					  tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_14;
					  highp vec2 xlat_varoutput_15;
					  xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
					  tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
					  highp vec4 tmpvar_16;
					  tmpvar_16.x = (((
					    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
					   / 2.0) - (0.5 / scale_4)) - weight_3);
					  tmpvar_16.y = scale_4;
					  tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
					  tmpvar_16.w = weight_3;
					  highp vec2 tmpvar_17;
					  tmpvar_17.x = _MaskSoftnessX;
					  tmpvar_17.y = _MaskSoftnessY;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
					  tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _EnvMatrix[0].xyz;
					  tmpvar_19[1] = _EnvMatrix[1].xyz;
					  tmpvar_19[2] = _EnvMatrix[2].xyz;
					  highp vec4 tmpvar_20;
					  tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
					  gl_Position = tmpvar_8;
					  xlv_COLOR = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_16;
					  xlv_TEXCOORD2 = tmpvar_18;
					  xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
					  xlv_TEXCOORD5 = tmpvar_20;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform sampler2D _FaceTex;
					uniform highp float _FaceUVSpeedX;
					uniform highp float _FaceUVSpeedY;
					uniform lowp vec4 _FaceColor;
					uniform highp float _OutlineSoftness;
					uniform sampler2D _OutlineTex;
					uniform highp float _OutlineUVSpeedX;
					uniform highp float _OutlineUVSpeedY;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _ScaleRatioA;
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 outlineColor_2;
					  mediump vec4 faceColor_3;
					  highp float c_4;
					  lowp float tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
					  c_4 = tmpvar_5;
					  highp float x_6;
					  x_6 = (c_4 - xlv_TEXCOORD1.x);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  highp float tmpvar_7;
					  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
					  highp float tmpvar_8;
					  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  highp float tmpvar_9;
					  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  faceColor_3 = _FaceColor;
					  outlineColor_2 = _OutlineColor;
					  faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _FaceUVSpeedX;
					  tmpvar_10.y = _FaceUVSpeedY;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
					  tmpvar_11 = texture2D (_FaceTex, P_12);
					  faceColor_3 = (faceColor_3 * tmpvar_11);
					  highp vec2 tmpvar_13;
					  tmpvar_13.x = _OutlineUVSpeedX;
					  tmpvar_13.y = _OutlineUVSpeedY;
					  lowp vec4 tmpvar_14;
					  highp vec2 P_15;
					  P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
					  tmpvar_14 = texture2D (_OutlineTex, P_15);
					  outlineColor_2 = (outlineColor_2 * tmpvar_14);
					  mediump float d_16;
					  d_16 = tmpvar_7;
					  lowp vec4 faceColor_17;
					  faceColor_17 = faceColor_3;
					  lowp vec4 outlineColor_18;
					  outlineColor_18 = outlineColor_2;
					  mediump float outline_19;
					  outline_19 = tmpvar_8;
					  mediump float softness_20;
					  softness_20 = tmpvar_9;
					  mediump float tmpvar_21;
					  tmpvar_21 = (1.0 - clamp ((
					    ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
					   / 
					    (1.0 + softness_20)
					  ), 0.0, 1.0));
					  faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
					  outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
					  mediump vec4 tmpvar_22;
					  tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
					    (d_16 + (outline_19 * 0.5))
					  , 0.0, 1.0) * sqrt(
					    min (1.0, outline_19)
					  ))));
					  faceColor_17 = tmpvar_22;
					  faceColor_17 = (faceColor_17 * tmpvar_21);
					  faceColor_3 = faceColor_17;
					  mediump float x_23;
					  x_23 = (faceColor_3.w - 0.001);
					  if ((x_23 < 0.0)) {
					    discard;
					  };
					  tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineWidth;
					uniform 	vec4 hlslcc_mtx4x4_EnvMatrix[4];
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
					uniform 	vec4 _FaceTex_ST;
					uniform 	vec4 _OutlineTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					out highp vec3 vs_TEXCOORD3;
					out highp vec4 vs_TEXCOORD5;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat6;
					vec2 u_xlat8;
					bool u_xlatb8;
					float u_xlat12;
					bool u_xlatb12;
					float u_xlat13;
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
					    vs_COLOR0 = in_COLOR0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat2.xyz = u_xlat8.xxx * u_xlat2.xyz;
					    u_xlat8.x = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat3.xyz = u_xlat8.xxx * u_xlat1.xyz;
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat3.xyz);
					    u_xlat2.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat2.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = vec2(abs(u_xlat2.x) * float(_ScaleX), abs(u_xlat2.y) * float(_ScaleY));
					    u_xlat2.xy = u_xlat2.ww / u_xlat2.xy;
					    u_xlat12 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat2.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat2.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat2.xy;
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat13 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat12 = u_xlat12 * u_xlat13;
					    u_xlat13 = u_xlat12 * 1.5;
					    u_xlat2.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat2.x = abs(u_xlat13) * u_xlat2.x;
					    u_xlat12 = u_xlat12 * 1.5 + (-u_xlat2.x);
					    u_xlat8.x = abs(u_xlat8.x) * u_xlat12 + u_xlat2.x;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb12 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb12 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat6.x = (u_xlatb12) ? u_xlat8.x : u_xlat13;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb8 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb8 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat8.x = u_xlatb8 ? 1.0 : float(0.0);
					    u_xlat12 = (-_WeightNormal) + _WeightBold;
					    u_xlat8.x = u_xlat8.x * u_xlat12 + _WeightNormal;
					    u_xlat8.x = u_xlat8.x * 0.25 + _FaceDilate;
					    u_xlat8.x = u_xlat8.x * _ScaleRatioA;
					    u_xlat6.z = u_xlat8.x * 0.5;
					    vs_TEXCOORD1.yw = u_xlat6.xz;
					    u_xlat12 = 0.5 / u_xlat6.x;
					    u_xlat13 = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat13 = (-_OutlineSoftness) * _ScaleRatioA + u_xlat13;
					    u_xlat13 = u_xlat13 * 0.5 + (-u_xlat12);
					    vs_TEXCOORD1.x = (-u_xlat8.x) * 0.5 + u_xlat13;
					    u_xlat8.x = (-u_xlat8.x) * 0.5 + 0.5;
					    vs_TEXCOORD1.z = u_xlat12 + u_xlat8.x;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat0.xyz = u_xlat1.yyy * hlslcc_mtx4x4_EnvMatrix[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4_EnvMatrix[0].xyz * u_xlat1.xxx + u_xlat0.xyz;
					    vs_TEXCOORD3.xyz = hlslcc_mtx4x4_EnvMatrix[2].xyz * u_xlat1.zzz + u_xlat0.xyz;
					    u_xlat0.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat8.x = floor(u_xlat0.x);
					    u_xlat8.y = (-u_xlat8.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat0.xy = u_xlat8.xy * vec2(0.001953125, 0.001953125);
					    vs_TEXCOORD5.xy = u_xlat0.xy * _FaceTex_ST.xy + _FaceTex_ST.zw;
					    vs_TEXCOORD5.zw = u_xlat0.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	float _FaceUVSpeedX;
					uniform 	float _FaceUVSpeedY;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineUVSpeedX;
					uniform 	float _OutlineUVSpeedY;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _ScaleRatioA;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					uniform lowp sampler2D _OutlineTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec4 vs_TEXCOORD5;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					mediump vec4 u_xlat16_2;
					mediump vec4 u_xlat16_3;
					mediump float u_xlat16_4;
					lowp vec4 u_xlat10_4;
					bool u_xlatb4;
					float u_xlat5;
					bool u_xlatb5;
					mediump float u_xlat16_6;
					mediump float u_xlat16_11;
					void main()
					{
					    u_xlat10_0.x = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat5 = u_xlat10_0.x + (-vs_TEXCOORD1.x);
					    u_xlat0.x = (-u_xlat10_0.x) + vs_TEXCOORD1.z;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb5 = !!(u_xlat5<0.0);
					#else
					    u_xlatb5 = u_xlat5<0.0;
					#endif
					    if((int(u_xlatb5) * int(0xffffffffu))!=0){discard;}
					    u_xlat5 = _OutlineWidth * _ScaleRatioA;
					    u_xlat5 = u_xlat5 * vs_TEXCOORD1.y;
					    u_xlat16_1 = min(u_xlat5, 1.0);
					    u_xlat16_6 = u_xlat5 * 0.5;
					    u_xlat16_1 = sqrt(u_xlat16_1);
					    u_xlat16_11 = u_xlat0.x * vs_TEXCOORD1.y + u_xlat16_6;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_11 = min(max(u_xlat16_11, 0.0), 1.0);
					#else
					    u_xlat16_11 = clamp(u_xlat16_11, 0.0, 1.0);
					#endif
					    u_xlat16_6 = u_xlat0.x * vs_TEXCOORD1.y + (-u_xlat16_6);
					    u_xlat16_1 = u_xlat16_1 * u_xlat16_11;
					    u_xlat0.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD5.zw;
					    u_xlat10_0 = texture(_OutlineTex, u_xlat0.xy);
					    u_xlat16_2 = u_xlat10_0 * _OutlineColor;
					    u_xlat16_3.xyz = vs_COLOR0.xyz * _FaceColor.xyz;
					    u_xlat0.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD5.xy;
					    u_xlat10_4 = texture(_FaceTex, u_xlat0.xy);
					    u_xlat16_0.xyz = u_xlat16_3.xyz * u_xlat10_4.xyz;
					    u_xlat16_4 = u_xlat10_4.w * _FaceColor.w;
					    u_xlat16_3.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4);
					    u_xlat16_2.xyz = u_xlat16_2.xyz * u_xlat16_2.www + (-u_xlat16_3.xyz);
					    u_xlat16_2.w = _OutlineColor.w * u_xlat10_0.w + (-u_xlat16_4);
					    u_xlat16_2 = vec4(u_xlat16_1) * u_xlat16_2;
					    u_xlat16_3.w = _FaceColor.w * u_xlat10_4.w + u_xlat16_2.w;
					    u_xlat16_3.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4) + u_xlat16_2.xyz;
					    u_xlat0.x = _OutlineSoftness * _ScaleRatioA;
					    u_xlat5 = u_xlat0.x * vs_TEXCOORD1.y;
					    u_xlat16_1 = u_xlat0.x * vs_TEXCOORD1.y + 1.0;
					    u_xlat16_6 = u_xlat5 * 0.5 + u_xlat16_6;
					    u_xlat16_1 = u_xlat16_6 / u_xlat16_1;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = (-u_xlat16_1) + 1.0;
					    u_xlat16_6 = u_xlat16_3.w * u_xlat16_1 + -0.00100000005;
					    u_xlat16_0 = vec4(u_xlat16_1) * u_xlat16_3;
					    SV_Target0 = u_xlat16_0 * vs_COLOR0.wwww;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb4 = !!(u_xlat16_6<0.0);
					#else
					    u_xlatb4 = u_xlat16_6<0.0;
					#endif
					    if((int(u_xlatb4) * int(0xffffffffu))!=0){discard;}
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
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform highp float _OutlineWidth;
					uniform highp mat4 _EnvMatrix;
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
					uniform highp vec4 _FaceTex_ST;
					uniform highp vec4 _OutlineTex_ST;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD3;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  highp float weight_3;
					  highp float scale_4;
					  highp vec2 pixelSize_5;
					  highp vec4 vert_6;
					  highp float tmpvar_7;
					  tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_6.zw = _glesVertex.zw;
					  vert_6.x = (_glesVertex.x + _VertexOffsetX);
					  vert_6.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_8;
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = vert_6.xyz;
					  tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _ScaleX;
					  tmpvar_10.y = _ScaleY;
					  highp mat2 tmpvar_11;
					  tmpvar_11[0] = glstate_matrix_projection[0].xy;
					  tmpvar_11[1] = glstate_matrix_projection[1].xy;
					  pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
					    (tmpvar_11 * _ScreenParams.xy)
					  )));
					  scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_12;
					    tmpvar_12[0] = unity_WorldToObject[0].xyz;
					    tmpvar_12[1] = unity_WorldToObject[1].xyz;
					    tmpvar_12[2] = unity_WorldToObject[2].xyz;
					    scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
					      normalize((_glesNormal * tmpvar_12))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
					    )));
					  };
					  weight_3 = (((
					    (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
					   + _FaceDilate) * _ScaleRatioA) * 0.5);
					  highp vec4 tmpvar_13;
					  tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_14;
					  highp vec2 xlat_varoutput_15;
					  xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
					  tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
					  highp vec4 tmpvar_16;
					  tmpvar_16.x = (((
					    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
					   / 2.0) - (0.5 / scale_4)) - weight_3);
					  tmpvar_16.y = scale_4;
					  tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
					  tmpvar_16.w = weight_3;
					  highp vec2 tmpvar_17;
					  tmpvar_17.x = _MaskSoftnessX;
					  tmpvar_17.y = _MaskSoftnessY;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
					  tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _EnvMatrix[0].xyz;
					  tmpvar_19[1] = _EnvMatrix[1].xyz;
					  tmpvar_19[2] = _EnvMatrix[2].xyz;
					  highp vec4 tmpvar_20;
					  tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
					  gl_Position = tmpvar_8;
					  xlv_COLOR = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_16;
					  xlv_TEXCOORD2 = tmpvar_18;
					  xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
					  xlv_TEXCOORD5 = tmpvar_20;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform sampler2D _FaceTex;
					uniform highp float _FaceUVSpeedX;
					uniform highp float _FaceUVSpeedY;
					uniform lowp vec4 _FaceColor;
					uniform highp float _OutlineSoftness;
					uniform sampler2D _OutlineTex;
					uniform highp float _OutlineUVSpeedX;
					uniform highp float _OutlineUVSpeedY;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _ScaleRatioA;
					uniform highp vec4 _ClipRect;
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 outlineColor_2;
					  mediump vec4 faceColor_3;
					  highp float c_4;
					  lowp float tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
					  c_4 = tmpvar_5;
					  highp float x_6;
					  x_6 = (c_4 - xlv_TEXCOORD1.x);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  highp float tmpvar_7;
					  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
					  highp float tmpvar_8;
					  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  highp float tmpvar_9;
					  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  faceColor_3 = _FaceColor;
					  outlineColor_2 = _OutlineColor;
					  faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _FaceUVSpeedX;
					  tmpvar_10.y = _FaceUVSpeedY;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
					  tmpvar_11 = texture2D (_FaceTex, P_12);
					  faceColor_3 = (faceColor_3 * tmpvar_11);
					  highp vec2 tmpvar_13;
					  tmpvar_13.x = _OutlineUVSpeedX;
					  tmpvar_13.y = _OutlineUVSpeedY;
					  lowp vec4 tmpvar_14;
					  highp vec2 P_15;
					  P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
					  tmpvar_14 = texture2D (_OutlineTex, P_15);
					  outlineColor_2 = (outlineColor_2 * tmpvar_14);
					  mediump float d_16;
					  d_16 = tmpvar_7;
					  lowp vec4 faceColor_17;
					  faceColor_17 = faceColor_3;
					  lowp vec4 outlineColor_18;
					  outlineColor_18 = outlineColor_2;
					  mediump float outline_19;
					  outline_19 = tmpvar_8;
					  mediump float softness_20;
					  softness_20 = tmpvar_9;
					  mediump float tmpvar_21;
					  tmpvar_21 = (1.0 - clamp ((
					    ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
					   / 
					    (1.0 + softness_20)
					  ), 0.0, 1.0));
					  faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
					  outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
					  mediump vec4 tmpvar_22;
					  tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
					    (d_16 + (outline_19 * 0.5))
					  , 0.0, 1.0) * sqrt(
					    min (1.0, outline_19)
					  ))));
					  faceColor_17 = tmpvar_22;
					  faceColor_17 = (faceColor_17 * tmpvar_21);
					  faceColor_3 = faceColor_17;
					  mediump vec2 tmpvar_23;
					  highp vec2 tmpvar_24;
					  tmpvar_24 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - 
					    abs(xlv_TEXCOORD2.xy)
					  ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_23 = tmpvar_24;
					  faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
					  tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineWidth;
					uniform 	vec4 hlslcc_mtx4x4_EnvMatrix[4];
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
					uniform 	vec4 _FaceTex_ST;
					uniform 	vec4 _OutlineTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					out highp vec3 vs_TEXCOORD3;
					out highp vec4 vs_TEXCOORD5;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat6;
					vec2 u_xlat8;
					bool u_xlatb8;
					float u_xlat12;
					bool u_xlatb12;
					float u_xlat13;
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
					    vs_COLOR0 = in_COLOR0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat2.xyz = u_xlat8.xxx * u_xlat2.xyz;
					    u_xlat8.x = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat3.xyz = u_xlat8.xxx * u_xlat1.xyz;
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat3.xyz);
					    u_xlat2.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat2.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = vec2(abs(u_xlat2.x) * float(_ScaleX), abs(u_xlat2.y) * float(_ScaleY));
					    u_xlat2.xy = u_xlat2.ww / u_xlat2.xy;
					    u_xlat12 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat2.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat2.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat2.xy;
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat13 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat12 = u_xlat12 * u_xlat13;
					    u_xlat13 = u_xlat12 * 1.5;
					    u_xlat2.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat2.x = abs(u_xlat13) * u_xlat2.x;
					    u_xlat12 = u_xlat12 * 1.5 + (-u_xlat2.x);
					    u_xlat8.x = abs(u_xlat8.x) * u_xlat12 + u_xlat2.x;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb12 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb12 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat6.x = (u_xlatb12) ? u_xlat8.x : u_xlat13;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb8 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb8 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat8.x = u_xlatb8 ? 1.0 : float(0.0);
					    u_xlat12 = (-_WeightNormal) + _WeightBold;
					    u_xlat8.x = u_xlat8.x * u_xlat12 + _WeightNormal;
					    u_xlat8.x = u_xlat8.x * 0.25 + _FaceDilate;
					    u_xlat8.x = u_xlat8.x * _ScaleRatioA;
					    u_xlat6.z = u_xlat8.x * 0.5;
					    vs_TEXCOORD1.yw = u_xlat6.xz;
					    u_xlat12 = 0.5 / u_xlat6.x;
					    u_xlat13 = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat13 = (-_OutlineSoftness) * _ScaleRatioA + u_xlat13;
					    u_xlat13 = u_xlat13 * 0.5 + (-u_xlat12);
					    vs_TEXCOORD1.x = (-u_xlat8.x) * 0.5 + u_xlat13;
					    u_xlat8.x = (-u_xlat8.x) * 0.5 + 0.5;
					    vs_TEXCOORD1.z = u_xlat12 + u_xlat8.x;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat0.xyz = u_xlat1.yyy * hlslcc_mtx4x4_EnvMatrix[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4_EnvMatrix[0].xyz * u_xlat1.xxx + u_xlat0.xyz;
					    vs_TEXCOORD3.xyz = hlslcc_mtx4x4_EnvMatrix[2].xyz * u_xlat1.zzz + u_xlat0.xyz;
					    u_xlat0.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat8.x = floor(u_xlat0.x);
					    u_xlat8.y = (-u_xlat8.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat0.xy = u_xlat8.xy * vec2(0.001953125, 0.001953125);
					    vs_TEXCOORD5.xy = u_xlat0.xy * _FaceTex_ST.xy + _FaceTex_ST.zw;
					    vs_TEXCOORD5.zw = u_xlat0.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	float _FaceUVSpeedX;
					uniform 	float _FaceUVSpeedY;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineUVSpeedX;
					uniform 	float _OutlineUVSpeedY;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _ScaleRatioA;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					uniform lowp sampler2D _OutlineTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec4 vs_TEXCOORD2;
					in highp vec4 vs_TEXCOORD5;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					mediump vec4 u_xlat16_2;
					mediump vec3 u_xlat16_3;
					vec2 u_xlat4;
					mediump float u_xlat16_4;
					lowp vec4 u_xlat10_4;
					float u_xlat5;
					bool u_xlatb5;
					mediump float u_xlat16_6;
					float u_xlat9;
					mediump float u_xlat16_11;
					void main()
					{
					    u_xlat10_0.x = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat5 = u_xlat10_0.x + (-vs_TEXCOORD1.x);
					    u_xlat0.x = (-u_xlat10_0.x) + vs_TEXCOORD1.z;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb5 = !!(u_xlat5<0.0);
					#else
					    u_xlatb5 = u_xlat5<0.0;
					#endif
					    if((int(u_xlatb5) * int(0xffffffffu))!=0){discard;}
					    u_xlat5 = _OutlineWidth * _ScaleRatioA;
					    u_xlat5 = u_xlat5 * vs_TEXCOORD1.y;
					    u_xlat16_1 = min(u_xlat5, 1.0);
					    u_xlat16_6 = u_xlat5 * 0.5;
					    u_xlat16_1 = sqrt(u_xlat16_1);
					    u_xlat16_11 = u_xlat0.x * vs_TEXCOORD1.y + u_xlat16_6;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_11 = min(max(u_xlat16_11, 0.0), 1.0);
					#else
					    u_xlat16_11 = clamp(u_xlat16_11, 0.0, 1.0);
					#endif
					    u_xlat16_6 = u_xlat0.x * vs_TEXCOORD1.y + (-u_xlat16_6);
					    u_xlat16_1 = u_xlat16_1 * u_xlat16_11;
					    u_xlat0.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD5.zw;
					    u_xlat10_0 = texture(_OutlineTex, u_xlat0.xy);
					    u_xlat16_2 = u_xlat10_0 * _OutlineColor;
					    u_xlat16_3.xyz = vs_COLOR0.xyz * _FaceColor.xyz;
					    u_xlat0.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD5.xy;
					    u_xlat10_4 = texture(_FaceTex, u_xlat0.xy);
					    u_xlat16_0.xyz = u_xlat16_3.xyz * u_xlat10_4.xyz;
					    u_xlat16_4 = u_xlat10_4.w * _FaceColor.w;
					    u_xlat16_3.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4);
					    u_xlat16_2.xyz = u_xlat16_2.xyz * u_xlat16_2.www + (-u_xlat16_3.xyz);
					    u_xlat16_2.w = _OutlineColor.w * u_xlat10_0.w + (-u_xlat16_4);
					    u_xlat16_2 = vec4(u_xlat16_1) * u_xlat16_2;
					    u_xlat16_0.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4) + u_xlat16_2.xyz;
					    u_xlat16_0.w = _FaceColor.w * u_xlat10_4.w + u_xlat16_2.w;
					    u_xlat4.x = _OutlineSoftness * _ScaleRatioA;
					    u_xlat9 = u_xlat4.x * vs_TEXCOORD1.y;
					    u_xlat16_1 = u_xlat4.x * vs_TEXCOORD1.y + 1.0;
					    u_xlat16_6 = u_xlat9 * 0.5 + u_xlat16_6;
					    u_xlat16_1 = u_xlat16_6 / u_xlat16_1;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = (-u_xlat16_1) + 1.0;
					    u_xlat16_0 = u_xlat16_0 * vec4(u_xlat16_1);
					    u_xlat4.xy = vec2((-_ClipRect.x) + _ClipRect.z, (-_ClipRect.y) + _ClipRect.w);
					    u_xlat4.xy = u_xlat4.xy + -abs(vs_TEXCOORD2.xy);
					    u_xlat4.xy = vec2(u_xlat4.x * vs_TEXCOORD2.z, u_xlat4.y * vs_TEXCOORD2.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat4.xy = min(max(u_xlat4.xy, 0.0), 1.0);
					#else
					    u_xlat4.xy = clamp(u_xlat4.xy, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat4.y * u_xlat4.x;
					    u_xlat16_0 = u_xlat16_0 * vec4(u_xlat16_1);
					    SV_Target0 = u_xlat16_0 * vs_COLOR0.wwww;
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
					uniform highp float _FaceDilate;
					uniform highp float _OutlineSoftness;
					uniform highp float _OutlineWidth;
					uniform highp mat4 _EnvMatrix;
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
					uniform highp vec4 _FaceTex_ST;
					uniform highp vec4 _OutlineTex_ST;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD3;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  highp float weight_3;
					  highp float scale_4;
					  highp vec2 pixelSize_5;
					  highp vec4 vert_6;
					  highp float tmpvar_7;
					  tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
					  vert_6.zw = _glesVertex.zw;
					  vert_6.x = (_glesVertex.x + _VertexOffsetX);
					  vert_6.y = (_glesVertex.y + _VertexOffsetY);
					  highp vec4 tmpvar_8;
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = vert_6.xyz;
					  tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _ScaleX;
					  tmpvar_10.y = _ScaleY;
					  highp mat2 tmpvar_11;
					  tmpvar_11[0] = glstate_matrix_projection[0].xy;
					  tmpvar_11[1] = glstate_matrix_projection[1].xy;
					  pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
					    (tmpvar_11 * _ScreenParams.xy)
					  )));
					  scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
					    abs(_glesMultiTexCoord1.y)
					   * _GradientScale) * 1.5));
					  if ((glstate_matrix_projection[3].w == 0.0)) {
					    highp mat3 tmpvar_12;
					    tmpvar_12[0] = unity_WorldToObject[0].xyz;
					    tmpvar_12[1] = unity_WorldToObject[1].xyz;
					    tmpvar_12[2] = unity_WorldToObject[2].xyz;
					    scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
					      normalize((_glesNormal * tmpvar_12))
					    , 
					      normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
					    )));
					  };
					  weight_3 = (((
					    (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
					   + _FaceDilate) * _ScaleRatioA) * 0.5);
					  highp vec4 tmpvar_13;
					  tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
					  highp vec2 tmpvar_14;
					  highp vec2 xlat_varoutput_15;
					  xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
					  xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
					  tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
					  highp vec4 tmpvar_16;
					  tmpvar_16.x = (((
					    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
					   / 2.0) - (0.5 / scale_4)) - weight_3);
					  tmpvar_16.y = scale_4;
					  tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
					  tmpvar_16.w = weight_3;
					  highp vec2 tmpvar_17;
					  tmpvar_17.x = _MaskSoftnessX;
					  tmpvar_17.y = _MaskSoftnessY;
					  highp vec4 tmpvar_18;
					  tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
					  tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _EnvMatrix[0].xyz;
					  tmpvar_19[1] = _EnvMatrix[1].xyz;
					  tmpvar_19[2] = _EnvMatrix[2].xyz;
					  highp vec4 tmpvar_20;
					  tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
					  tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
					  gl_Position = tmpvar_8;
					  xlv_COLOR = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_16;
					  xlv_TEXCOORD2 = tmpvar_18;
					  xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
					  xlv_TEXCOORD5 = tmpvar_20;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform sampler2D _FaceTex;
					uniform highp float _FaceUVSpeedX;
					uniform highp float _FaceUVSpeedY;
					uniform lowp vec4 _FaceColor;
					uniform highp float _OutlineSoftness;
					uniform sampler2D _OutlineTex;
					uniform highp float _OutlineUVSpeedX;
					uniform highp float _OutlineUVSpeedY;
					uniform lowp vec4 _OutlineColor;
					uniform highp float _OutlineWidth;
					uniform highp float _ScaleRatioA;
					uniform highp vec4 _ClipRect;
					uniform sampler2D _MainTex;
					varying lowp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 outlineColor_2;
					  mediump vec4 faceColor_3;
					  highp float c_4;
					  lowp float tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
					  c_4 = tmpvar_5;
					  highp float x_6;
					  x_6 = (c_4 - xlv_TEXCOORD1.x);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  highp float tmpvar_7;
					  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
					  highp float tmpvar_8;
					  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  highp float tmpvar_9;
					  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
					  faceColor_3 = _FaceColor;
					  outlineColor_2 = _OutlineColor;
					  faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
					  highp vec2 tmpvar_10;
					  tmpvar_10.x = _FaceUVSpeedX;
					  tmpvar_10.y = _FaceUVSpeedY;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
					  tmpvar_11 = texture2D (_FaceTex, P_12);
					  faceColor_3 = (faceColor_3 * tmpvar_11);
					  highp vec2 tmpvar_13;
					  tmpvar_13.x = _OutlineUVSpeedX;
					  tmpvar_13.y = _OutlineUVSpeedY;
					  lowp vec4 tmpvar_14;
					  highp vec2 P_15;
					  P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
					  tmpvar_14 = texture2D (_OutlineTex, P_15);
					  outlineColor_2 = (outlineColor_2 * tmpvar_14);
					  mediump float d_16;
					  d_16 = tmpvar_7;
					  lowp vec4 faceColor_17;
					  faceColor_17 = faceColor_3;
					  lowp vec4 outlineColor_18;
					  outlineColor_18 = outlineColor_2;
					  mediump float outline_19;
					  outline_19 = tmpvar_8;
					  mediump float softness_20;
					  softness_20 = tmpvar_9;
					  mediump float tmpvar_21;
					  tmpvar_21 = (1.0 - clamp ((
					    ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
					   / 
					    (1.0 + softness_20)
					  ), 0.0, 1.0));
					  faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
					  outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
					  mediump vec4 tmpvar_22;
					  tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
					    (d_16 + (outline_19 * 0.5))
					  , 0.0, 1.0) * sqrt(
					    min (1.0, outline_19)
					  ))));
					  faceColor_17 = tmpvar_22;
					  faceColor_17 = (faceColor_17 * tmpvar_21);
					  faceColor_3 = faceColor_17;
					  mediump vec2 tmpvar_23;
					  highp vec2 tmpvar_24;
					  tmpvar_24 = clamp (((
					    (_ClipRect.zw - _ClipRect.xy)
					   - 
					    abs(xlv_TEXCOORD2.xy)
					  ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
					  tmpvar_23 = tmpvar_24;
					  faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
					  mediump float x_25;
					  x_25 = (faceColor_3.w - 0.001);
					  if ((x_25 < 0.0)) {
					    discard;
					  };
					  tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
					uniform 	float _FaceDilate;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineWidth;
					uniform 	vec4 hlslcc_mtx4x4_EnvMatrix[4];
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
					uniform 	vec4 _FaceTex_ST;
					uniform 	vec4 _OutlineTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					in highp vec2 in_TEXCOORD0;
					in highp vec2 in_TEXCOORD1;
					out mediump vec4 vs_COLOR0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec4 vs_TEXCOORD2;
					out highp vec3 vs_TEXCOORD3;
					out highp vec4 vs_TEXCOORD5;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat6;
					vec2 u_xlat8;
					bool u_xlatb8;
					float u_xlat12;
					bool u_xlatb12;
					float u_xlat13;
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
					    vs_COLOR0 = in_COLOR0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat2.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat2.xyz = u_xlat8.xxx * u_xlat2.xyz;
					    u_xlat8.x = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat8.x = inversesqrt(u_xlat8.x);
					    u_xlat3.xyz = u_xlat8.xxx * u_xlat1.xyz;
					    u_xlat8.x = dot(u_xlat2.xyz, u_xlat3.xyz);
					    u_xlat2.xy = _ScreenParams.yy * hlslcc_mtx4x4glstate_matrix_projection[1].xy;
					    u_xlat2.xy = hlslcc_mtx4x4glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = vec2(abs(u_xlat2.x) * float(_ScaleX), abs(u_xlat2.y) * float(_ScaleY));
					    u_xlat2.xy = u_xlat2.ww / u_xlat2.xy;
					    u_xlat12 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat2.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vec2(0.25, 0.25) + u_xlat2.xy;
					    vs_TEXCOORD2.zw = vec2(0.25, 0.25) / u_xlat2.xy;
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat13 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat12 = u_xlat12 * u_xlat13;
					    u_xlat13 = u_xlat12 * 1.5;
					    u_xlat2.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat2.x = abs(u_xlat13) * u_xlat2.x;
					    u_xlat12 = u_xlat12 * 1.5 + (-u_xlat2.x);
					    u_xlat8.x = abs(u_xlat8.x) * u_xlat12 + u_xlat2.x;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb12 = !!(hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0);
					#else
					    u_xlatb12 = hlslcc_mtx4x4glstate_matrix_projection[3].w==0.0;
					#endif
					    u_xlat6.x = (u_xlatb12) ? u_xlat8.x : u_xlat13;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb8 = !!(0.0>=in_TEXCOORD1.y);
					#else
					    u_xlatb8 = 0.0>=in_TEXCOORD1.y;
					#endif
					    u_xlat8.x = u_xlatb8 ? 1.0 : float(0.0);
					    u_xlat12 = (-_WeightNormal) + _WeightBold;
					    u_xlat8.x = u_xlat8.x * u_xlat12 + _WeightNormal;
					    u_xlat8.x = u_xlat8.x * 0.25 + _FaceDilate;
					    u_xlat8.x = u_xlat8.x * _ScaleRatioA;
					    u_xlat6.z = u_xlat8.x * 0.5;
					    vs_TEXCOORD1.yw = u_xlat6.xz;
					    u_xlat12 = 0.5 / u_xlat6.x;
					    u_xlat13 = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat13 = (-_OutlineSoftness) * _ScaleRatioA + u_xlat13;
					    u_xlat13 = u_xlat13 * 0.5 + (-u_xlat12);
					    vs_TEXCOORD1.x = (-u_xlat8.x) * 0.5 + u_xlat13;
					    u_xlat8.x = (-u_xlat8.x) * 0.5 + 0.5;
					    vs_TEXCOORD1.z = u_xlat12 + u_xlat8.x;
					    u_xlat2 = max(_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10));
					    u_xlat2 = min(u_xlat2, vec4(2e+10, 2e+10, 2e+10, 2e+10));
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + (-u_xlat2.xy);
					    vs_TEXCOORD2.xy = vec2((-u_xlat2.z) + u_xlat0.x, (-u_xlat2.w) + u_xlat0.y);
					    u_xlat0.xyz = u_xlat1.yyy * hlslcc_mtx4x4_EnvMatrix[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4_EnvMatrix[0].xyz * u_xlat1.xxx + u_xlat0.xyz;
					    vs_TEXCOORD3.xyz = hlslcc_mtx4x4_EnvMatrix[2].xyz * u_xlat1.zzz + u_xlat0.xyz;
					    u_xlat0.x = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat8.x = floor(u_xlat0.x);
					    u_xlat8.y = (-u_xlat8.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat0.xy = u_xlat8.xy * vec2(0.001953125, 0.001953125);
					    vs_TEXCOORD5.xy = u_xlat0.xy * _FaceTex_ST.xy + _FaceTex_ST.zw;
					    vs_TEXCOORD5.zw = u_xlat0.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	float _FaceUVSpeedX;
					uniform 	float _FaceUVSpeedY;
					uniform 	mediump vec4 _FaceColor;
					uniform 	float _OutlineSoftness;
					uniform 	float _OutlineUVSpeedX;
					uniform 	float _OutlineUVSpeedY;
					uniform 	mediump vec4 _OutlineColor;
					uniform 	float _OutlineWidth;
					uniform 	float _ScaleRatioA;
					uniform 	vec4 _ClipRect;
					uniform lowp sampler2D _MainTex;
					uniform lowp sampler2D _FaceTex;
					uniform lowp sampler2D _OutlineTex;
					in mediump vec4 vs_COLOR0;
					in highp vec2 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec4 vs_TEXCOORD2;
					in highp vec4 vs_TEXCOORD5;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec2 u_xlat0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					mediump vec4 u_xlat16_2;
					mediump vec3 u_xlat16_3;
					vec2 u_xlat4;
					mediump float u_xlat16_4;
					lowp vec4 u_xlat10_4;
					bool u_xlatb4;
					float u_xlat5;
					bool u_xlatb5;
					mediump float u_xlat16_6;
					float u_xlat9;
					mediump float u_xlat16_11;
					void main()
					{
					    u_xlat10_0.x = texture(_MainTex, vs_TEXCOORD0.xy).w;
					    u_xlat5 = u_xlat10_0.x + (-vs_TEXCOORD1.x);
					    u_xlat0.x = (-u_xlat10_0.x) + vs_TEXCOORD1.z;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb5 = !!(u_xlat5<0.0);
					#else
					    u_xlatb5 = u_xlat5<0.0;
					#endif
					    if((int(u_xlatb5) * int(0xffffffffu))!=0){discard;}
					    u_xlat5 = _OutlineWidth * _ScaleRatioA;
					    u_xlat5 = u_xlat5 * vs_TEXCOORD1.y;
					    u_xlat16_1 = min(u_xlat5, 1.0);
					    u_xlat16_6 = u_xlat5 * 0.5;
					    u_xlat16_1 = sqrt(u_xlat16_1);
					    u_xlat16_11 = u_xlat0.x * vs_TEXCOORD1.y + u_xlat16_6;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_11 = min(max(u_xlat16_11, 0.0), 1.0);
					#else
					    u_xlat16_11 = clamp(u_xlat16_11, 0.0, 1.0);
					#endif
					    u_xlat16_6 = u_xlat0.x * vs_TEXCOORD1.y + (-u_xlat16_6);
					    u_xlat16_1 = u_xlat16_1 * u_xlat16_11;
					    u_xlat0.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD5.zw;
					    u_xlat10_0 = texture(_OutlineTex, u_xlat0.xy);
					    u_xlat16_2 = u_xlat10_0 * _OutlineColor;
					    u_xlat16_3.xyz = vs_COLOR0.xyz * _FaceColor.xyz;
					    u_xlat0.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD5.xy;
					    u_xlat10_4 = texture(_FaceTex, u_xlat0.xy);
					    u_xlat16_0.xyz = u_xlat16_3.xyz * u_xlat10_4.xyz;
					    u_xlat16_4 = u_xlat10_4.w * _FaceColor.w;
					    u_xlat16_3.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4);
					    u_xlat16_2.xyz = u_xlat16_2.xyz * u_xlat16_2.www + (-u_xlat16_3.xyz);
					    u_xlat16_2.w = _OutlineColor.w * u_xlat10_0.w + (-u_xlat16_4);
					    u_xlat16_2 = vec4(u_xlat16_1) * u_xlat16_2;
					    u_xlat16_0.xyz = u_xlat16_0.xyz * vec3(u_xlat16_4) + u_xlat16_2.xyz;
					    u_xlat16_0.w = _FaceColor.w * u_xlat10_4.w + u_xlat16_2.w;
					    u_xlat4.x = _OutlineSoftness * _ScaleRatioA;
					    u_xlat9 = u_xlat4.x * vs_TEXCOORD1.y;
					    u_xlat16_1 = u_xlat4.x * vs_TEXCOORD1.y + 1.0;
					    u_xlat16_6 = u_xlat9 * 0.5 + u_xlat16_6;
					    u_xlat16_1 = u_xlat16_6 / u_xlat16_1;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = (-u_xlat16_1) + 1.0;
					    u_xlat16_0 = u_xlat16_0 * vec4(u_xlat16_1);
					    u_xlat4.xy = vec2((-_ClipRect.x) + _ClipRect.z, (-_ClipRect.y) + _ClipRect.w);
					    u_xlat4.xy = u_xlat4.xy + -abs(vs_TEXCOORD2.xy);
					    u_xlat4.xy = vec2(u_xlat4.x * vs_TEXCOORD2.z, u_xlat4.y * vs_TEXCOORD2.w);
					#ifdef UNITY_ADRENO_ES3
					    u_xlat4.xy = min(max(u_xlat4.xy, 0.0), 1.0);
					#else
					    u_xlat4.xy = clamp(u_xlat4.xy, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat4.y * u_xlat4.x;
					    u_xlat16_6 = u_xlat16_0.w * u_xlat16_1 + -0.00100000005;
					    u_xlat16_0 = u_xlat16_0 * vec4(u_xlat16_1);
					    SV_Target0 = u_xlat16_0 * vs_COLOR0.wwww;
					#ifdef UNITY_ADRENO_ES3
					    u_xlatb4 = !!(u_xlat16_6<0.0);
					#else
					    u_xlatb4 = u_xlat16_6<0.0;
					#endif
					    if((int(u_xlatb4) * int(0xffffffffu))!=0){discard;}
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
	Fallback "TextMeshPro/Mobile/Distance Field"
	CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}