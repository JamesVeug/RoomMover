// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "3DAutosports/Diffuse Alpha lightmap" {

Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Color ("Main Color", Color) = (1,1,1,1)
}
 
SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100
 
    ZWrite on
    Blend SrcAlpha OneMinusSrcAlpha
 
    Pass {
        Tags { "LightMode" = "Vertex" }
        Lighting Off
            SetTexture [_MainTex] {
                constantColor [_Color]
                combine texture*constant
            }
        }
// Lightmapped, encoded as RGBM
        Pass {
            Tags { "LightMode" = "VertexLMRGBM" }
            Lighting Off
            BindChannels {
                Bind "Vertex", vertex
                Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
                Bind "texcoord", texcoord1 // main uses 1st uv
            }
         
            SetTexture [unity_Lightmap] {
                matrix [unity_LightmapMatrix]
                combine texture * texture alpha DOUBLE
            }
         
            SetTexture [_MainTex] {
                constantColor [_Color]
                combine constant * previous
            }
         
            SetTexture [_MainTex] {
               combine texture * previous QUAD, texture * primary
            }
        }
     
        // Lightmapped, encoded as dLDR
        Pass {
            Tags { "LightMode" = "VertexLM" }
            Lighting Off
            BindChannels {
                Bind "Vertex", vertex
                Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
                Bind "texcoord", texcoord1 // main uses 1st uv
            }
            SetTexture [unity_Lightmap] {
                matrix [unity_LightmapMatrix]
                constantColor [_Color]
                combine texture * constant
            }
            SetTexture [_MainTex] {
                combine texture * previous DOUBLE, texture * primary
            }
        }
    }
}