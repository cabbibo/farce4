Shader "Final/Basic16" {
  Properties {

		_Color("_Color",Color)=(1,0,0,1)
    
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
      #include "../Chunks/Struct16.cginc"
			#include "../Chunks/safeID.cginc"


      #pragma vertex vert
			#pragma fragment frag
			
			float3 _Color;

			struct varyings {
				float4 pos 		: SV_POSITION;
				float3 nor 		: TEXCOORD0;
				float3 world 	: TEXCOORD1;
			};

      int _TransferCount;

			varyings vert(uint id : SV_VertexID) {

        //id = safeID(id, _TransferCount);

				float3 fPos 	= _TransferBuffer[id].pos;
				float3 fNor 	= _TransferBuffer[id].nor;

				varyings o;

				///UNITY_INITIALIZE_OUTPUT(varyings, o);

				o.pos = mul(UNITY_MATRIX_VP, float4(fPos - fNor * .001,1));
				o.nor = normalize(fNor);
				o.world = fPos - fNor * .001;

				return o;
			}

			float4 frag(varyings v) : COLOR {

				float3 fNor = normalize(cross( ddx( v.world ) , ddy(v.world)));
				//return float4( _Color , 1.);
				return float4( fNor * .4 + .7 , 1.);
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