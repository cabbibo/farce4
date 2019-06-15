﻿Shader "Final/ManyColors" {
 Properties
    {
        _OutlineExtrusion("Outline Extrusion", float) = 0.01
        _Offset("_Offset", float) = 0.01
        _Size("_Size", float) = 0.01
    
       _ColorMap ("ColorMap", 2D) = "white" {}
    }

    SubShader
    {
        // Regular color & lighting pass
        Pass
        {

            // shadows it
      Tags { "LightMode" = "ForwardBase" "RenderType"="Opaque" "Queue"="Geometry+1" "ForceNoShadowCasting"="True"  }
            LOD 150
            //Blend Zero SrcColor
            ZWrite On

            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            //#pragma multi_compile_fwdbase // shadows

            #include "AutoLight.cginc"
            #include "UnityCG.cginc"

            
            #include "../Chunks/noise.cginc"
            #include "../Chunks/hsv.cginc"


            #include "../Chunks/safeID.cginc"


            sampler2D _ColorMap;
            float _Offset;
            float _Size;

            #include "../Chunks/VertInput.cginc"

            float4 frag(vertexOutput v) : COLOR
            {
                // lighting mode

                float m = dot( _WorldSpaceLightPos0 , v.normal );
                float3 col;

                float size = 1000;
                float n = 0.01*noise(v.world * 120);//sin(v.world.x * size)  + sin(v.world.y*size) + sin(v.world.z * size) ;
                //float attenuation = LIGHT_ATTENUATION(v);


                float m2 = floor((m + n * 10) * 6) / 6;
                float attenuation = UNITY_SHADOW_ATTENUATION(v,v.world  + v.normal * .004 );
                float attenuation2 = UNITY_SHADOW_ATTENUATION(v,v.world + n * v.normal * .1 );
                m = min( m , attenuation2) + n * .2;
                m = 1.5-1000*pow(abs( m - m2 ),4);

                col = attenuation;//hsv( attenuation * .3 + m2 * .6  + _Time.y * .2 + n * 10,.4,.7 + attenuation * .4);//+ .4*attenuation;// tex2D(_ColorMap,float2(m * _Size + _Offset,.5)) *  (m * .5 + .5);//lerp( float3(0,1,0) , float3(0,0,1) , 1-m);// * float3(0,1,0);

                float3 fNor = normalize(cross(normalize(ddx(v.world)),normalize(ddy(v.world))));

                m = dot( _WorldSpaceLightPos0 , fNor );
                col =hsv( -m*3+v.debug.y*.1+attenuation * .3,.4,attenuation * .5 + .5);//fNor * .4 + .7;
                //if( n < .36 ){ discard; }
                return saturate(float4(col, 1.0));
            }

            ENDCG
        }

        // Shadow pass
    Pass
    {
      Tags{ "LightMode" = "ShadowCaster" }


      Fog{ Mode Off }
      ZWrite On
      ZTest LEqual
      Cull Off
      Offset 1, 1
      CGPROGRAM

      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_shadowcaster
      #pragma fragmentoption ARB_precision_hint_fastest

            #include "UnityCG.cginc"
            #include "../Chunks/Shadow16.cginc"

      ENDCG
    }
  

    }
}