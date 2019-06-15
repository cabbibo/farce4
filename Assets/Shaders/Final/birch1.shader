Shader "Finals/Birch1"{
 Properties
    {
        _OutlineExtrusion("Outline Extrusion", float) = 0.01
        _Offset("_Offset", float) = 0.01
        _Size("_Size", float) = 0.01
    
       _TexMap ("Tex", 2D) = "white" {}
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
            sampler2D _TexMap;
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
                float3 refl = reflect( v.eye , fNor );
                float mRefl = dot( normalize( refl) , normalize(_WorldSpaceLightPos0));
                //col =hsv( -m*3+v.debug.y*.1+attenuation * .3,.4,attenuation * .5 + .5);//fNor * .4 + .7;

                float dVal = saturate( pow((noise( v.world * 1000 ) + .3*noise(v.world * 100))/1.3,1));//pow( sin( v.uv.xy * 10  ) , 20);
                col =hsv( pow( mRefl ,3) *1 + _Time.y *.3 + sin(v.debug.x*.001)+ dVal,1,1)*dVal*pow( mRefl ,6)*3;//spectral(-v.debug.y * .1+1 + m)*m;// * tex2D(_TexMap , v.uv.xy *float2(.03 ,.3) + v.debug.x * .001 + v.debug.y*.1 ).xyz*hsv( v.debug.y  * .3 + v.uv.y * .3 + sin(v.debug.x*.0001) , .7,1)*3;
                

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