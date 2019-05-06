// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Final/RefractTest3" {
  Properties {

        _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

        _Color("_Color",Color)=(1,0,0,1)
        _Swap("_Swap",float)=0
        _NoiseSize("_NoiseSize",float)=0
        _NoiseSpeed("_NoiseSpeed",float)=0


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

      #include "../Chunks/hsv.cginc"

    #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight


      #pragma vertex vert
            #pragma fragment frag
            

            samplerCUBE _CubeMap;

            float _Swap;
            float _NoiseSize;
            float _NoiseSpeed;

            float3 _Color;
            sampler2D _ColorMap;
            sampler2D _NormalMap;


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
                UNITY_SHADOW_COORDS(2)
            };

      int _TransferCount;

            float4x4 _UnityDisplayTransform;

            varyings vert(appdata_base v) {

        //id = safeID(id, _TransferCount);

                varyings o;

                UNITY_INITIALIZE_OUTPUT(varyings, o);

                o.pos = UnityObjectToClipPos(v.vertex);
                 o.screenPos = ComputeScreenPos(o.pos);


                o.worldPos = mul(unity_ObjectToWorld, float4( v.vertex.xyz,1));
                o.eye = _WorldSpaceCameraPos - o.worldPos;
                o.nor = v.normal;
                o.uv =  v.texcoord;


                return o;
            }

            float4 frag(varyings v) : COLOR {
        
               

            float3 col;
               
            float2 uvVal = (v.screenPos.xy/v.screenPos.w );

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
            col = mul(ycbcrToRGBTransform, ycbcr);

//tex2D(_TextureY)
                //col = tCol;// normalize(n) * .5 + .5;//lerp(tex2D(_ColorMap , float2(pow( m,4) * .4 + _Swap * .3,0)) * pow(m,4) , tCol,1-m) ;// + tCol * (1-pow( m,20));// * _Color;// hsv( v.uv.x * .4 + v.debug.x * .4 + v.debug.y * 10 , .7,1);

                //col *= shadow*.5 + .2;
                return float4( col , 1.);
            }

            ENDCG
        }
    }

}

