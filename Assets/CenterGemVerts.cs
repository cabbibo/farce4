using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGemVerts: Form {

  public DandelionVerts dandyVerts;
  public override void SetStructSize(){ structSize = 16; }
  public override void SetCount(){ count = (dandyVerts.count / dandyVerts.vertsPerVert) * (dandyVerts.vertsPerVert+2) ; }

}



