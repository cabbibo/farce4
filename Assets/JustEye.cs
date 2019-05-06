using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustEye : MonoBehaviour
{

    public Transform Eye;
    public Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

//      print( Eye.position );
//      print( render.material );
      render.material.SetVector("_Eye", Eye.position );
      //Debug.Log( "S H :"+ Display.main.systemHeight);
      //Debug.Log( "S W :"+ Display.main.systemWidth);
      //Debug.Log( "R H :"+ Display.main.renderingHeight);
      //Debug.Log( "R H :"+ Display.main.renderingWidth);
        
    }
}
