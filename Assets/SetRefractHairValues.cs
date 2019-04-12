using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRefractHairValues : MonoBehaviour
{


    public Body body;
    public GetBackgroundInfo info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        body.material.SetTexture("_TextureY" , info.textureY );
        body.material.SetTexture("_TextureCbCr" , info.textureCbCr );
    }
}
