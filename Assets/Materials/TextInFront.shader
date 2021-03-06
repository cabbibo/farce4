﻿Shader "GUI/TextInFront" { 
    Properties {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
    }
 
    SubShader {
        Tags { "Queue"="Geometry+30" "IgnoreProjector"="True" }
        Lighting Off 
        Cull Off 
        ZWrite On
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            Color [_Color]
            SetTexture [_MainTex] {
                combine primary, texture * primary
            }
        }
    }
}
