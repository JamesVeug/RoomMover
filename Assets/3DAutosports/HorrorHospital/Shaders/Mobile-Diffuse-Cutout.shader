
Shader "3DAutosports/Diffuse Alpha Cutout" {

    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _LightMap ("Lightmap (RGB)", 2D) = "white" {}
    }
    SubShader {
        Cull Off
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutoff"}
        Pass {
          BindChannels {
             Bind "Vertex", vertex
             Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
             Bind "texcoord", texcoord1 // alphamap uses 1nd uv
          }
           
          Alphatest Greater [_Cutoff]
          AlphaToMask True
          ColorMask RGB
         
          SetTexture [_LightMap] { constantColor [_Color] combine texture * constant DOUBLE }
          SetTexture [_MainTex] { combine previous * texture, texture }
         
        }
    }
}
//
