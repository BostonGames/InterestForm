// Copyright 2019 Frameplay. All Rights Reserved.
#ifndef FRAMEPLAY_IMAGE_COMPARISON_CGINC
#define FRAMEPLAY_IMAGE_COMPARISON_CGINC
#define FRAMEPLAY_ROLLING_AVERAGE
float f(fixed3 f) { return dot(f, fixed3(.2126, .7152, .0722)); }fixed f(float4 f, float4 v) { return dot(1., max(sign(f - v), 0.)); }float4 f(fixed2 x, float3 f, float3 v, float3 i, float4x4 y) { float4 z = float4(f + x.x * v + x.y * i, 1.); return mul(y, z); }float4 t(float4 f) {
	float4 v = f * .5f; v.xy = float2(v.x, v.y) + v.w; v.zw = f.zw;
#if FRAMEPLAY_os
	v.z *= .5; v.z += .5;
#endif
	return v;
}fixed D(float4 v) { float4 x = abs(v * 2. - 1.); fixed z = f(float4(x.xyz, 0.), x.w); return z; }struct v2fRQ { float4 jp:TEXCOORD0; float4 xv:SV_POSITION; float4 bl:TEXCOORD1; float4 na:TEXCOORD2; float4 miX:TEXCOORD3; float4 miY:TEXCOORD4; }; struct appdataRQ { float4 xv:POSITION; float2 uv:TEXCOORD0; }; float4 D(appdataRQ v, float4x4 x, float4x4 i) { const float4 z = x._m00_m10_m20_m30, y = x._m01_m11_m21_m31, s = x._m02_m12_m22_m32; float4 a = f(v.uv.xy, z, y, s, i); return t(a); }struct yr { float3 go; float3 tc; }; yr D(float3 f, float3 v) { yr x; x.go = f; x.tc = v; return x; }yr f(float2 f, float4x4 v, float3 x) { float3 z = mul(v, float4(f, 0.f, 1.f)).xyz; return D(x, z); }yr D(float2 f, float3 v, float4x4 x, float4 z) { return D(v + mul((float3x3)x, float3(f, 0.)), z); }float3 f(yr f, float3 v, float3 x, float t) { float z = dot(x, f.tc), y = t / z; float3 i = f.go + y * f.tc; return i; }float3 t(yr f, float3 v, float3 x) { float z = dot(x, f.tc), y = dot(v - f.go, x) / z; float3 i = f.go + y * f.tc; return i; }struct ac { float3 na; float3 bl; float pd; float4x4 nd; float3 cp; float4 miX; float4 miY; }; float4x4 _F_V, _F_N, _F_CF[4]; float4 _F_IX[4], _F_IY[4], _F_S, _F_C, _F_F; sampler2D _AdTex0, _AdTex1, _AdTex2, _AdTex3; float4 _DerivativeMultipliers; half2 t(float2 v, ac x) {
	yr z; float3 y;
#if FRAMEPLAY_co
	z = D(v, x.cp, x.nd, _F_F); y = t(z, x.bl, x.na);
#else
	z = f(v, x.nd, x.cp); y = f(z, x.bl, x.na, x.pd);
#endif
	half4 i = half4(y, 1.); half2 w = half2(dot(x.miX, i), dot(x.miY, i)); return w;
}v2fRQ vertRQ(appdataRQ v) { int z = (int)(v.xv.z * 4.); float4x4 x = _F_CF[z]; v2fRQ r; r.xv = v.xv; float3 y = x._m00_m10_m20, s = x._m01_m11_m21, w = x._m02_m12_m22; float4 i = f(v.uv.xy, y, s, w, _F_V); r.jp = t(i); r.bl.xyz = y; r.bl.w = _DerivativeMultipliers[z]; r.na = x._m03_m13_m23_m33; r.jp.xy *= _F_S.xy; r.miX = _F_IX[z]; r.miY = _F_IY[z]; return r; }fixed4 fragRQ(v2fRQ x) :SV_Target{ fixed4 v = 0; float2 z = x.jp.xy / x.jp.w; z = floor(z) + .5; z = z * _F_S.zw - 1; ac r; r.cp = _F_C; r.nd = _F_N; r.miX = x.miX; r.miY = x.miY; r.na = x.na.xyz; r.bl = x.bl.xyz; r.pd = x.na.w; float2 y = t(z,r); const float2 i = float2(_F_S.z,0.),s = float2(0.,_F_S.w); half2 w = t(z + i,r),l = t(z + s,r),e = y.xy - w,h = y.xy - l; float d = x.bl.w; e *= d; h *= d; float S = 0,m = 1;
#if SHADER_API_GLES3||SHADER_API_GLES||SHADER_API_GLCORE
S = .5; m = .5;
#endif
bool a = x.xv.z == 0 * m + S,n = x.xv.z == 1 / 4. * m + S,R = x.xv.z == 2 / 4. * m + S,X = x.xv.z == 3 / 4. * m + S;
#ifdef SHADER_API_GLES
if (a)v = tex2D(_AdTex0,y); else if (n)v = tex2D(_AdTex1,y); else if (R)v = tex2D(_AdTex2,y); else if (X)v = tex2D(_AdTex3,y);
#else
if (a)v = tex2D(_AdTex0,y,e,h); else if (n)v = tex2D(_AdTex1,y,e,h); else if (R)v = tex2D(_AdTex2,y,e,h); else if (X)v = tex2D(_AdTex3,y,e,h);
#endif
v.z = f(v.xyz); v.xy = 0; return v; }struct v2fSQ { float4 jp:TEXCOORD0; float2 uv:TEXCOORD1; float4 xv:SV_POSITION; }; v2fSQ vertSQ(appdataRQ x) { v2fSQ f; f.xv = x.xv; int v = (int)(x.xv.z * 4.); f.jp = D(x, _F_CF[v], _F_V); f.uv = x.uv; return f; }sampler2D _F_T; fixed4 fragSQ(v2fSQ x) :SV_Target{ fixed4 v = 0; float4 z = x.jp / x.jp.w; fixed i = D(z);
#if SHADER_API_GLES
v = tex2Dproj(_F_T,x.jp);
#else
z.w = 0; v = tex2Dlod(_F_T,z);
#endif
v.y = f(v.xyz) * (1 - i); v.xz = 0; return v; }struct v2f1TB { float2 uv:TEXCOORD0; float4 xv:SV_POSITION; }; v2f1TB vert1TB(appdataRQ x) { v2f1TB f; f.xv = x.xv; f.uv = x.xv.xy * fixed2(.5, -.5) + .5; return f; }sampler2D _F_A; fixed4 fragEA(v2f1TB x) :SV_Target{ const float2 v = float2(1. / 256.,1. / 64.); const fixed3 f = fixed3(1.,-1.,0.); fixed2 z = tex2D(_F_A,f.zx * v + x.uv).yz,y = tex2D(_F_A,f.xz * v + x.uv).yz,s = tex2D(_F_A,x.uv).yz; half2 i = z - s,w = y - s; half4 r = fixed4(w,i); half2 a = atan2(i,w); const float m = .31831; fixed2 S = fixed2(length(r.xz),length(r.yw)); fixed4 t = fixed4((a * m + 1.) * .5,S * 12.7); fixed2 e = t.yw,h = t.xz; return float4(e,h); }sampler2D _F_E; fixed4 fragCAS(v2f1TB x) :SV_Target{ const float2 f = float2(1. / 256.,1. / 64.); fixed3 v = fixed3(1.,-1.,0.); fixed4 z = tex2D(_F_E,f * v.xx + x.uv),y = tex2D(_F_E,f * v.xy + x.uv),r = tex2D(_F_E,f * v.yx + x.uv); fixed i = min(r.x,min(z.x,y.x)),t = max(r.x,max(z.x,y.x)),w = .333333; fixed3 a = fixed3(z.y,y.y,r.y); fixed d = dot(a,w),s = d > .2; const fixed S = .0005; float3 h = float3(r.x,z.x,y.x); h = min(frac(h),frac(-h)); i = min(h.x,min(h.y,h.z)); t = max(h.x,max(h.y,h.z)); fixed m = t - i >= S; fixed3 R = fixed3(z.w,y.w,r.w); fixed n = .01; fixed3 e = saturate((a - n) * 9999),l = saturate((R - n) * 9999);
const float N = .02; half2 u = frac(half2(x.uv.x * 4,x.uv.y)); fixed2 c = abs(u - .5) > .5 - N; c.x += c.y; s -= c.x; s *= m; s *= dot(e,w); fixed3 X = fixed3(z.z,y.z,r.z),Y = fixed3(z.x,y.x,r.x); half3 F = Y - X; F = min(frac(F),frac(-F)); fixed b;
#if SHADER_API_GLES
b = .13;
#else
b = .04;
#endif
F = F < b; fixed3 U = e * l; F *= U; fixed D = dot(F,1),A = D * s; return fixed4(s,A,s,s); }struct v2fAC { float2 uv:TEXCOORD0; float4 uv2:TEXCOORD1; float4 xv:SV_POSITION; }; sampler2D _F_PC; float4 _F_P, _F_Y, _F_R; float _F_X; sampler2D _F_O; v2fAC vertAC(appdataRQ x) {
	v2fAC f; f.xv = x.xv; int v = (int)(x.xv.z * 4.); float4 z = float4(v == 0, v == 1, v == 2, v == 3); f.uv2.xyw = 0; f.uv2.z = dot(_F_R, z); f.xv.x = _F_X; f.xv.y = _F_Y[v]; f.uv = f.xv;
#if UNITY_UV_STARTS_AT_TOP
	f.xv.y = 1 - f.xv.y;
#endif
	float2 y = x.xv.xy; f.xv.xy += y * _F_P.xy; const float4 i = float4(.125, .375, .125 + .25 * 2, .125 + .25 * 3);
#ifdef SHADER_API_GLES
	f.xv.xy += _F_P.xy * .99; f.uv2.xy = x.uv.xy;
#else
	f.uv2.xyw = float3(i[v], .5, 5); f.xv.xy += _F_P.xy * .9372;
#endif
	f.xv.xy = f.xv.xy * 2. - 1.; return f;
}fixed2 v(sampler2D v, float4 x) {
	fixed4 f = 0;
#ifdef SHADER_API_GLES
	f = tex2D(v, x.xy);
#else
	f = tex2Dlod(v, x);
#endif
	fixed z = f.x; fixed2 r = 0; r.y = 1 - z; r.x = f.y / max(.001, z); return r;
}fixed4 fragAC(v2fAC x) :SV_Target{ fixed2 f = v(_F_O,x.uv2); fixed z = f.x; fixed4 r;
#ifdef FRAMEPLAY_ROLLING_AVERAGE
float4 i = tex2D(_F_PC,x.uv); float y = x.uv2.z; float4 h = (i * (y - 1.) + z) / y; r = fixed4(h.x,h.x,f.y,1.);
#else
r = fixed4(z,z,f.y,1.);
#endif
return r; }sampler2D _MainTex; v2f1TB vertCD(appdataRQ x) { v2f1TB f; f.xv = UnityObjectToClipPos(x.xv); f.uv = x.uv; f.uv.y *= 2.; return f; }fixed4 fragCD(v2f1TB x) :SV_Target{ fixed4 f = tex2D(_MainTex,x.uv); f *= x.uv.y < 1.; return f; }v2f1TB vertBO(appdataRQ x) { v2f1TB f; f.xv = UnityObjectToClipPos(x.xv); f.uv = x.uv; return f; }fixed4 fragBO(v2f1TB x) : SV_Target{ fixed4 v = tex2D(_MainTex,x.uv); return v; }

#endif