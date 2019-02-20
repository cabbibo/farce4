using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMesh : Cycle {


  public MeshRenderer[] meshes;

  // Use this for initialization
  public override void Activate() {
     for( int i =0; i < meshes.Length; i++){
      meshes[i].enabled = true;
     }
  }
   

  public override void Deactivate(){
    for( int i =0; i < meshes.Length; i++){
      meshes[i].enabled = false;
     }
  }


}