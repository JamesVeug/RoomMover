Shader "3DAutosports/³ RGB + Alpha Spec" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
 
SubShader {
    LOD 200
    Tags { "RenderType" = "Opaque" }
CGPROGRAM
#pragma surface surf BlinnPhong
struct Input {
  float2 uv_MainTex;
};
sampler2D _MainTex;
float4 _Color;
float _Shininess;
 
void surf (Input IN, inout SurfaceOutput o)
{
  half4 tex = tex2D (_MainTex, IN.uv_MainTex);
  o.Albedo = tex.rgb * _Color;
  o.Gloss = tex.a;
  o.Specular = _Shininess;
}
ENDCG
}
FallBack "Legacy Shaders/Lightmapped/VertexLit"
}