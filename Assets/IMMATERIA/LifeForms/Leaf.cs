﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : LifeForm {

  public float armLength;
  public Life force;
  public Life constrain;
  public Form skeleton;
  public LeafVerts verts;
  public override void Create(){


    Cycles.Insert(0 , force );
    Cycles.Insert(1 , constrain );
    Cycles.Insert(2 , verts );

  

}
    public override void Bind(){

    force.BindPrimaryForm("_VertBuffer", verts );
    force.BindForm("_SkeletonBuffer", skeleton );

   // force.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", skeleton );
  //  force.BindAttribute( "_NumHairs" , "numHairs", skeleton );

    constrain.BindPrimaryForm("_VertBuffer", verts );
    constrain.BindForm("_SkeletonBuffer", skeleton );

    //constrain.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", skeleton );
    //constrain.BindAttribute( "_NumHairs" , "numHairs", skeleton );
    constrain.BindAttribute( "_Length" , "armLength" , this );


  }


}