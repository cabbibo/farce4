using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBinder : Cycle {

  public Life toBind;

  public float _Length;
  public float _Radius;
  public GemVerts verts;



  // Use this for initialization
  public override void Bind() {
    toBind.BindAttribute( "_Radius" , "_Radius" , this );
    toBind.BindAttribute( "_Length" , "_Length" , this );
    toBind.BindAttribute( "_NumSides" , "numSides" , verts );
  }
  


}
