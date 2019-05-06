using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StealCameraTexture : MonoBehaviour
{

    public GetBackgroundInfo bgInfo;
    public Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
  

        render.material.SetMatrix( "_UnityDisplayTransform", bgInfo.displayTransform);
        render.material.SetTexture("_TextureCbCr",bgInfo.textureCbCr);
        render.material.SetTexture("_TextureY",bgInfo.textureY);
    }
}
