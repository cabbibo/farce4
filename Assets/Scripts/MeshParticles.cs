using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticles : Particles {

  public Form mesh;

  public override void SetCount(){
    count = mesh.count;
  }

}
