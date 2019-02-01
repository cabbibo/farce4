using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonHairTransfer : TransferLifeForm {
  
  public float radius;

  
  public override void Bind(){
    transfer.BindAttribute( "_Radius" , "radius" , this );
    transfer.BindAttribute( "_RibbonLength" , "length" , verts );
    transfer.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair" , skeleton );
  }

}
