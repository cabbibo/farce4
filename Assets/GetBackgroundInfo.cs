using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GetBackgroundInfo : MonoBehaviour
{

  public ARCameraBackground bg;



    public Matrix4x4 displayTransform;//         float4x4 _UnityDisplayTransform;
    public Texture textureY;
    public Texture textureCbCr;//           sampler2D _textureY;
//            sampler2D _textureCbCr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        textureY    = bg.material.GetTexture("_textureY");
        textureCbCr = bg.material.GetTexture("_textureCbCr");
        
        if( bg.material.HasProperty("_UnityDisplayTransform")){
        Matrix4x4 udp = bg.material.GetMatrix("_UnityDisplayTransform");
      
        if( udp != null){
            print( udp );
           displayTransform = udp;
        }
    }

    }
}
