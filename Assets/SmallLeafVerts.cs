using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLeafVerts: Form {

  public Form verts;
  public override void SetStructSize(){ structSize = 16; }
  public override void SetCount(){ count = verts.count * 4; }

}



