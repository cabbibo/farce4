using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccBinder:Cycle{

  public Life toBind;
  public FernVerts fern;
  public SmoothedHair hair;

  public float _LeafWidth;
  public override void Bind(){

    toBind.BindForm("_FernBuffer" , fern);
    toBind.BindAttribute("_VertsPerVert" , "vertsPerVert" , fern);
    toBind.BindAttribute("_VertsPerHair" , "numVertsPerHair" , hair);
    toBind.BindAttribute("_LeafWidth" , "_LeafWidth" , this);
    
  }



}
