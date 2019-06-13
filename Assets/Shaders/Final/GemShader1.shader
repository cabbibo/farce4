Shader "Final/Gem" {
  Properties {

        _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

        _Color("_Color",Color)=(1,0,0,1)
        _Swap("_Swap",float)=0
        _NoiseSize("_NoiseSize",float)=0
        _NoiseSpeed("_NoiseSpeed",float)=0



       _ColorMap ("ColorMap", 2D) = "white" {}
       _NormalMap ("NormalMap", 2D) = "white" {}
       _VideoMap ("VideoTexture", 2D) = "white" {}
    
  }

    SubShader {
        // COLOR PASS

        Pass {
            Tags{ "LightMode" = "ForwardBase" }
            Cull Off


                        // Write to Stencil buffer (so that outline pass can read)
            Stencil
            {
                Ref 10
                Comp always
                Pass replace
                ZFail keep
            }

            CGPROGRAM
            #pragma target 4.5
        

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

    #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

      #include "../Chunks/Struct16.cginc"
      
      #include "../Chunks/hsv.cginc"
      #include "../Chunks/noise.cginc"
            #include "../Chunks/safeID.cginc"


      #pragma vertex vert
            #pragma fragment frag
            

            samplerCUBE _CubeMap;

            float _Swap;
            float _NoiseSize;
            float _NoiseSpeed;

            float3 _Color;
            sampler2D _ColorMap;
            sampler2D _NormalMap;

            float4x4 _UnityDisplayTransform;
            sampler2D _TextureY;
            sampler2D _TextureCbCr;


      float3 _LightPos;
            struct varyings {
                float4 pos      : SV_POSITION;
                float3 nor      : TEXCOORD0;
                float2 uv       : TEXCOORD1;
                float3 bi      : TEXCOORD3;
                float3 tan      : TEXCOORD4;
                float3 eye      : TEXCOORD5;
                float3 worldPos : TEXCOORD6;
                float3 debug    : TEXCOORD7;
                float3 closest    : TEXCOORD8;
                float4 screenPos : TEXCOORD9;
                float4 refractedScreenPos : TEXCOORD10;
                UNITY_SHADOW_COORDS(2)
            };

      int _TransferCount;

            varyings vert(uint id : SV_VertexID) {

        //id = safeID(id, _TransferCount);

                float3 fPos     = _TransferBuffer[id].pos;
                float3 fNor     = _TransferBuffer[id].nor;
                float3 fTan     = _TransferBuffer[id].tan;
                float2 fUV      = _TransferBuffer[id].uv;
                float2 debug    = _TransferBuffer[id].debug;

                varyings o;

                UNITY_INITIALIZE_OUTPUT(varyings, o);

                o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
                 o.screenPos = ComputeScreenPos(o.pos);
                o.worldPos = fPos;
                o.eye = _WorldSpaceCameraPos - fPos;
                o.nor = normalize(fNor);
                o.uv =  fUV;
                o.tan = normalize(fTan);
                o.bi = normalize(cross(normalize(fTan),normalize(fNor)));
                o.debug = float3(debug.x,debug.y,0);


             
                UNITY_TRANSFER_SHADOW(o,o.worldPos);

                return o;
            }

            float4 frag(varyings v) : COLOR {
        
        

            float3 col;

            float2 fUV = (v.uv - .5) * 2;

            float angle = atan2( fUV.y , fUV.x );
            float radius = length(fUV);
            float3 norAdd = v.tan * cos(angle) - v.bi * sin(angle);
      
            float3 fNor = normalize(cross(normalize(ddx(v.worldPos)),normalize(ddy(v.worldPos))));//;norAdd * radius * radius * 20 + v.nor;// + norAdd * radius * .2;

            //fNor = normalize(fNor);

            float3 refractedVec = refract( normalize(v.eye), fNor , .8 );
            float3 reflectedVec = reflect( normalize(v.eye), fNor  );

           //if( radius > 1 ){discard;}
            
            float3 refracted = v.worldPos - .003 * refractedVec;

            float4 refractedScreenPos = ComputeScreenPos( mul(UNITY_MATRIX_VP, float4(refracted,1)) );


            float2 uvVal = (refractedScreenPos.xy/refractedScreenPos.w);

             float2 tmp  = uvVal;
            uvVal.x = (_UnityDisplayTransform[0].x * tmp.x + _UnityDisplayTransform[1].x * (tmp.y) + _UnityDisplayTransform[2].x);
            uvVal.y = (_UnityDisplayTransform[0].y * tmp.x + _UnityDisplayTransform[1].y * (tmp.y) + (_UnityDisplayTransform[2].y));
             
   
            float y; float4 ycbcr;

            const float4x4 ycbcrToRGBTransform = float4x4(
                    float4(1.0, +0.0000, +1.4020, -0.7010),
                    float4(1.0, -0.3441, -0.7141, +0.5291),
                    float4(1.0, +1.7720, +0.0000, -0.8860),
                    float4(0.0, +0.0000, +0.0000, +1.0000)
                );






           // uvVal = float2( 1-abs(uvVal.x % 1) , 1-abs(uvVal.y  % 1));
            y = tex2D(_TextureY, uvVal).r;
            ycbcr = float4(y, tex2D(_TextureCbCr, uvVal).rg, 1.0);
            float3 screenCol = mul(ycbcrToRGBTransform, ycbcr);
            fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos );


                col = screenCol.xyz;//tex2D(_TextureY)
                //col = tCol;// normalize(n) * .5 + .5;//lerp(tex2D(_ColorMap , float2(pow( m,4) * .4 + _Swap * .3,0)) * pow(m,4) , tCol,1-m) ;// + tCol * (1-pow( m,20));// * _Color;// hsv( v.uv.x * .4 + v.debug.x * .4 + v.debug.y * 10 , .7,1);

// if( radius>.9){ col *= (radius-.8) * 3 + 1;}

 col = col * 1.8 * (reflectedVec * .5 + .5);
              //  col = fNor * .5 + .5;
                //col *= (shadow*.5 + .5);
                return float4( col , 1.);
            }

            ENDCG
        }


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

      #pragma fragmentoption ARB_precision_hint_fastest
      #include "UnityCG.cginc"


      #include "../Chunks/Struct16.cginc"

      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_shadowcaster

      struct v2f {
        V2F_SHADOW_CASTER;
      };


      v2f vert(appdata_base v, uint id : SV_VertexID)
      {
        v2f o;


        float3 wPos = _TransferBuffer[id].pos;
        float3 wNor = _TransferBuffer[id].nor;

            // Default shadow caster pass: Apply the shadow bias.
    float scos = dot(wNor, normalize(UnityWorldSpaceLightDir(wPos)));
    wPos -= wNor * unity_LightShadowBias.z * sqrt(1 - scos * scos);
    o.pos = UnityApplyLinearShadowBias(UnityWorldToClipPos(float4(wPos, 1)));


        //o.pos = mul(UNITY_MATRIX_VP, float4(_TransferBuffer[id].pos + _TransferBuffer[id].nor * -.001, 1));
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }



    }

}