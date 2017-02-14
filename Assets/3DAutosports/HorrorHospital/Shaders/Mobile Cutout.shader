// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color
Shader "3DAutosports/Mobile Cutout"{
 
Properties
    {
       
        _Cutoff ("Alpha Cutoff" , Range(0, 1)) = .5
       _MainTex ("Texture", 2D) = ""
        //_Color ("Color Tint", Color) = (1,1,1,1)    
        //_MainTex ("Base (RGB) Alpha (A)", 2D) = "white"
    }
 
SubShader
    {
       
        Alphatest Greater [_Cutoff]
        Cull Off
        // Tags { Queue = Geometry +500}
       
       
        BindChannels
        {
            Bind "vertex", vertex
            Bind "color", color
            Bind "texcoord1", texcoord
        }
       
        Pass
        {
            SetTexture[_MainTex] { Combine texture, texture * primary}
        }
    }
}