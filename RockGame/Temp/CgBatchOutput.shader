Shader "Hidden/Blend" {
	Properties {
		_MainTex ("Screen Blended", 2D) = "" {}
		_ColorBuffer ("Color", 2D) = "" {}
	}
	
	#LINE 68
 
	
Subshader {
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode off }  
	  	
 Pass {    

      Program "vp" {
// Vertex combos: 1
//   opengl - ALU: 6 to 6
//   d3d9 - ALU: 14 to 14
SubProgram "opengl " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
# 6 ALU
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
MOV result.texcoord[1].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_ColorBuffer_TexelSize]
"vs_2_0
; 14 ALU
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}

SubProgram "gles " {
Keywords { }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;

attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  vec2 tmpvar_1[2];
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1[0] = tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_1[1] = tmpvar_3;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1[0];
  xlv_TEXCOORD0_1 = tmpvar_1[1];
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
uniform mediump float _Intensity;
uniform sampler2D _ColorBuffer;
void main ()
{
  vec2 tmpvar_1[2];
  tmpvar_1[0] = xlv_TEXCOORD0;
  tmpvar_1[1] = xlv_TEXCOORD0_1;
  gl_FragData[0] = (1.0 - ((1.0 - clamp ((texture2D (_MainTex, tmpvar_1[0]) * _Intensity), 0.0, 1.0)) * (1.0 - texture2D (_ColorBuffer, tmpvar_1[1]))));
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;

attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  vec2 tmpvar_1[2];
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1[0] = tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_1[1] = tmpvar_3;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1[0];
  xlv_TEXCOORD0_1 = tmpvar_1[1];
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
uniform mediump float _Intensity;
uniform sampler2D _ColorBuffer;
void main ()
{
  vec2 tmpvar_1[2];
  tmpvar_1[0] = xlv_TEXCOORD0;
  tmpvar_1[1] = xlv_TEXCOORD0_1;
  gl_FragData[0] = (1.0 - ((1.0 - clamp ((texture2D (_MainTex, tmpvar_1[0]) * _Intensity), 0.0, 1.0)) * (1.0 - texture2D (_ColorBuffer, tmpvar_1[1]))));
}



#endif"
}

}
Program "fp" {
// Fragment combos: 1
//   opengl - ALU: 6 to 6, TEX: 2 to 2
//   d3d9 - ALU: 5 to 5, TEX: 2 to 2
SubProgram "opengl " {
Keywords { }
Float 0 [_Intensity]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_ColorBuffer] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 6 ALU, 2 TEX
PARAM c[2] = { program.local[0],
		{ 1 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[1], 2D;
MUL_SAT R0, R0, c[0].x;
ADD R1, -R1, c[1].x;
ADD R0, -R0, c[1].x;
MAD result.color, -R0, R1, c[1].x;
END
# 6 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Float 0 [_Intensity]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_ColorBuffer] 2D
"ps_2_0
; 5 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c1, 1.00000000, 0, 0, 0
dcl t0.xy
dcl t1.xy
texld r1, t0, s0
texld r0, t1, s1
mul_sat r1, r1, c0.x
add r0, -r0, c1.x
add_pp r1, -r1, c1.x
mad r0, -r1, r0, c1.x
mov_pp oC0, r0
"
}

SubProgram "gles " {
Keywords { }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES"
}

}

#LINE 80

  }

 Pass {    

      Program "vp" {
// Vertex combos: 1
//   opengl - ALU: 6 to 6
//   d3d9 - ALU: 14 to 14
SubProgram "opengl " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
# 6 ALU
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
MOV result.texcoord[1].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_ColorBuffer_TexelSize]
"vs_2_0
; 14 ALU
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}

SubProgram "gles " {
Keywords { }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;

attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  vec2 tmpvar_1[2];
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1[0] = tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_1[1] = tmpvar_3;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1[0];
  xlv_TEXCOORD0_1 = tmpvar_1[1];
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
uniform mediump float _Intensity;
uniform sampler2D _ColorBuffer;
void main ()
{
  vec2 tmpvar_1[2];
  tmpvar_1[0] = xlv_TEXCOORD0;
  tmpvar_1[1] = xlv_TEXCOORD0_1;
  gl_FragData[0] = ((texture2D (_MainTex, tmpvar_1[0]) * _Intensity) + texture2D (_ColorBuffer, tmpvar_1[1]));
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;

attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  vec2 tmpvar_1[2];
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1[0] = tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_1[1] = tmpvar_3;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1[0];
  xlv_TEXCOORD0_1 = tmpvar_1[1];
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
uniform mediump float _Intensity;
uniform sampler2D _ColorBuffer;
void main ()
{
  vec2 tmpvar_1[2];
  tmpvar_1[0] = xlv_TEXCOORD0;
  tmpvar_1[1] = xlv_TEXCOORD0_1;
  gl_FragData[0] = ((texture2D (_MainTex, tmpvar_1[0]) * _Intensity) + texture2D (_ColorBuffer, tmpvar_1[1]));
}



#endif"
}

}
Program "fp" {
// Fragment combos: 1
//   opengl - ALU: 3 to 3, TEX: 2 to 2
//   d3d9 - ALU: 2 to 2, TEX: 2 to 2
SubProgram "opengl " {
Keywords { }
Float 0 [_Intensity]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_ColorBuffer] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 3 ALU, 2 TEX
PARAM c[1] = { program.local[0] };
TEMP R0;
TEMP R1;
TEX R1, fragment.texcoord[1], texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MAD result.color, R0, c[0].x, R1;
END
# 3 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Float 0 [_Intensity]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_ColorBuffer] 2D
"ps_2_0
; 2 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
dcl t0.xy
dcl t1.xy
texld r0, t1, s1
texld r1, t0, s0
mad r0, r1, c0.x, r0
mov_pp oC0, r0
"
}

SubProgram "gles " {
Keywords { }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES"
}

}

#LINE 89

  }

 Pass {    

      Program "vp" {
// Vertex combos: 1
//   opengl - ALU: 9 to 9
//   d3d9 - ALU: 12 to 12
SubProgram "opengl " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_TexelSize]
"!!ARBvp1.0
# 9 ALU
PARAM c[6] = { { 0.5, -0.5, 0 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
MOV R0.xy, c[0];
MAD result.texcoord[0].xy, R0.x, c[5], vertex.texcoord[0];
MAD result.texcoord[1].xy, R0.x, -c[5], vertex.texcoord[0];
MAD result.texcoord[2].xy, R0, -c[5], vertex.texcoord[0];
MAD result.texcoord[3].xy, R0, c[5], vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 9 instructions, 1 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 12 ALU
def c5, 0.50000000, -0.50000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT0.xy, c5.x, r0, v1
mad oT1.xy, c5.x, -r0.zwzw, v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2.xy, c5, -r0, v1
mad oT3.xy, c5, r0.zwzw, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}

SubProgram "gles " {
Keywords { }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD0_3;
varying highp vec2 xlv_TEXCOORD0_2;
varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;

uniform mediump vec4 _MainTex_TexelSize;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  vec2 tmpvar_1[4];
  mediump vec2 tmpvar_2;
  tmpvar_2 = (_glesMultiTexCoord0.xy + (_MainTex_TexelSize.xy * 0.5));
  tmpvar_1[0] = tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = (_glesMultiTexCoord0.xy - (_MainTex_TexelSize.xy * 0.5));
  tmpvar_1[1] = tmpvar_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = (_glesMultiTexCoord0.xy - (vec2(0.5, -0.5) * _MainTex_TexelSize.xy));
  tmpvar_1[2] = tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5 = (_glesMultiTexCoord0.xy + (vec2(0.5, -0.5) * _MainTex_TexelSize.xy));
  tmpvar_1[3] = tmpvar_5;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1[0];
  xlv_TEXCOORD0_1 = tmpvar_1[1];
  xlv_TEXCOORD0_2 = tmpvar_1[2];
  xlv_TEXCOORD0_3 = tmpvar_1[3];
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD0_3;
varying highp vec2 xlv_TEXCOORD0_2;
varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
void main ()
{
  vec2 tmpvar_1[4];
  tmpvar_1[0] = xlv_TEXCOORD0;
  tmpvar_1[1] = xlv_TEXCOORD0_1;
  tmpvar_1[2] = xlv_TEXCOORD0_2;
  tmpvar_1[3] = xlv_TEXCOORD0_3;
  mediump vec4 outColor;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, tmpvar_1[0]);
  outColor = tmpvar_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, tmpvar_1[1]);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, tmpvar_1[2]);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, tmpvar_1[3]);
  mediump vec4 tmpvar_6;
  tmpvar_6 = (((outColor + tmpvar_3) + tmpvar_4) + tmpvar_5);
  outColor = tmpvar_6;
  gl_FragData[0] = (tmpvar_6 * 0.25);
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD0_3;
varying highp vec2 xlv_TEXCOORD0_2;
varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;

uniform mediump vec4 _MainTex_TexelSize;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  vec2 tmpvar_1[4];
  mediump vec2 tmpvar_2;
  tmpvar_2 = (_glesMultiTexCoord0.xy + (_MainTex_TexelSize.xy * 0.5));
  tmpvar_1[0] = tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = (_glesMultiTexCoord0.xy - (_MainTex_TexelSize.xy * 0.5));
  tmpvar_1[1] = tmpvar_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = (_glesMultiTexCoord0.xy - (vec2(0.5, -0.5) * _MainTex_TexelSize.xy));
  tmpvar_1[2] = tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5 = (_glesMultiTexCoord0.xy + (vec2(0.5, -0.5) * _MainTex_TexelSize.xy));
  tmpvar_1[3] = tmpvar_5;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1[0];
  xlv_TEXCOORD0_1 = tmpvar_1[1];
  xlv_TEXCOORD0_2 = tmpvar_1[2];
  xlv_TEXCOORD0_3 = tmpvar_1[3];
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD0_3;
varying highp vec2 xlv_TEXCOORD0_2;
varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
void main ()
{
  vec2 tmpvar_1[4];
  tmpvar_1[0] = xlv_TEXCOORD0;
  tmpvar_1[1] = xlv_TEXCOORD0_1;
  tmpvar_1[2] = xlv_TEXCOORD0_2;
  tmpvar_1[3] = xlv_TEXCOORD0_3;
  mediump vec4 outColor;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, tmpvar_1[0]);
  outColor = tmpvar_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, tmpvar_1[1]);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, tmpvar_1[2]);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, tmpvar_1[3]);
  mediump vec4 tmpvar_6;
  tmpvar_6 = (((outColor + tmpvar_3) + tmpvar_4) + tmpvar_5);
  outColor = tmpvar_6;
  gl_FragData[0] = (tmpvar_6 * 0.25);
}



#endif"
}

}
Program "fp" {
// Fragment combos: 1
//   opengl - ALU: 8 to 8, TEX: 4 to 4
//   d3d9 - ALU: 5 to 5, TEX: 4 to 4
SubProgram "opengl " {
Keywords { }
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 4 TEX
PARAM c[1] = { { 0.25 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R3, fragment.texcoord[3], texture[0], 2D;
TEX R2, fragment.texcoord[2], texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
MUL result.color, R0, c[0].x;
END
# 8 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 5 ALU, 4 TEX
dcl_2d s0
def c0, 0.25000000, 0, 0, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
texld r0, t3, s0
texld r1, t2, s0
texld r2, t1, s0
texld r3, t0, s0
add_pp r2, r3, r2
add_pp r1, r2, r1
add_pp r0, r1, r0
mul_pp r0, r0, c0.x
mov_pp oC0, r0
"
}

SubProgram "gles " {
Keywords { }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES"
}

}

#LINE 98

  } 
}

Fallback off
	
} // shader