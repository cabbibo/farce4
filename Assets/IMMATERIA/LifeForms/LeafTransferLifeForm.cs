using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafTransferLifeForm :TransferLifeForm{

  public LeafVerts leaf;
  public float radius;

  public override void Bind(){

    transfer.BindForm("_LeafBuffer" , leaf );
    transfer.BindAttribute("_Radius" , "radius" , this);
    
  }



}
