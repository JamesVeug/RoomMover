
Shader "3DAutosports/Unlit Diffuse Alpha" {

Properties {
    _Color ("Color Tint", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
 
SubShader {
    Cull Off
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutoff"}
    Pass {
        Alphatest Greater [_Cutoff]
        AlphaToMask True
        ColorMask RGB
 
        SetTexture [_MainTex] {
            Combine texture, texture
        }
    }
}
 
}