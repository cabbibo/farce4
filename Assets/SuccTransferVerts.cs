using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccTransferVerts: Form {

  public FernVerts verts;
  public SmoothedHair hair;

  public override void SetStructSize(){ structSize = 16; }
  public override void SetCount(){ count = verts.count * 4; }

}
