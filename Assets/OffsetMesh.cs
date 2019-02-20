using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetMesh : Cycle {

  public Transform face;
  public Transform mesh;
  public float offset;

  // Use this for initialization
  public override void Activate() {
     mesh.parent = face;
     mesh.localRotation = new Quaternion();
     mesh.localPosition = new Vector3(0,0,offset);
  }
   


}