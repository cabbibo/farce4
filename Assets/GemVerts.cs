using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemVerts: Form {

  public Form particles;
  public int numSides;
  public override void SetStructSize(){ structSize = 16; }
  public override void SetCount(){ count = particles.count* (numSides * 2 +1) ; }

}


