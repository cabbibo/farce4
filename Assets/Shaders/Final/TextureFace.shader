Shader "Final/TextureFace"
{
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
            Blend Zero SrcColor
            ZWrite On
            Blend OneMinusDstColor One // Soft Additive

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

            // Optimised by Alan Zucconi
float3 bump3y (float3 x, float3 yoffset)
{
 float3 y = float3( 1. - x * x );
 y = clamp(y-yoffset,0.,1.);
 return y;
}
float3 spectral (float w)
{
    // w: [400, 700]
 // x: [0,   1]
 float x = w;// clamp((w - 400.0)/ 300.0,0.,1.);
 
 const float3 cs = float3(3.54541723, 2.86670055, 2.29421995);
 const float3 xs = float3(0.69548916, 0.49416934, 0.28269708);
 const float3 ys = float3(0.02320775, 0.15936245, 0.53520021);
 
 return bump3y ( cs * (x - xs), ys);
}

float3 sat(float3 rgb, float adjustment)
{
    const float3 W = float3(0.2125, 0.7154, 0.0721);
    float3 intensity = dot(rgb, W);
    return lerp(intensity, rgb, adjustment);
}


            float4 frag(vertexOutput v) : COLOR
            {
                // lighting mode

                float m = dot( _WorldSpaceLightPos0 , v.normal );
                float3 col;

                float size = 1000;
                float n = noise(v.world * 120);//sin(v.world.x * size)  + sin(v.world.y*size) + sin(v.world.z * size) ;
                //float attenuation = LIGHT_ATTENUATION(v);


                float attenuation = UNITY_SHADOW_ATTENUATION(v,v.world  + v.normal * .004 );
                //float attenuation2 = UNITY_SHADOW_ATTENUATION(v,v.world + n * v.normal * .1 );
                //m = min( m , attenuation) + n * .2;
                //float m2 = floor(m * 10) / 10;
                //m = 1.5-1000*pow(abs( m - m2 ),4);

                col = tex2D(_ColorMap , v.uv * float2(1,-.7) + float2(0,1));

                //col = hsv((abs(attenuation2 - attenuation)*.1)+m2 * .8,.2,(abs(attenuation2 - attenuation)*.1)+m2 + .8);//+ .4*attenuation;// tex2D(_ColorMap,float2(m * _Size + _Offset,.5)) *  (m * .5 + .5);//lerp( float3(0,1,0) , float3(0,0,1) , 1-m);// * float3(0,1,0);

               //col = 1;
               //float n2 = noise( float3(v.uv , _Time.x * .1) * 40 ) * .3;
               //if( sin(v.uv.x* 60) > .8 - n2 ){ col = clamp( 1 -  (sin(v.uv.x * 60)-.8- n2 ) * 4,0,1);}
                col *= attenuation * .5 + .5;

                col += float3(.3,.3,.3);
                float d = dot( normalize(float3(v.uv.x,v.uv.y,0)), float3(1,0,0));

                float startV = (d-.38-.01*noise(float3(v.uv*30 + _Time.y,1)) * 4) * 2;
                float v2 =floor(startV*10)/10;


                float v3 = ( startV * 10 +.1 ) %1;


                float oppo =  saturate((.5-abs(v2-.5)) * 30);

                col = hsv(v2*1+_Time.y*.4,1,v3) * oppo;

                col = saturate(col*1.3);
                //col += d;
                //col = lerp( 0, col, pow(length(col),2)*2);
                //if( d < .45 ){col=1; }
                //if( d > .9 ){ discard; }

                //if( length(col) < .3 ){discard;}
                //if( n < .36 ){ discard; }
                return saturate(float4(col, length(col)));
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