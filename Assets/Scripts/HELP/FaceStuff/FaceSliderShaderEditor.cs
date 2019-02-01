using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSliderShaderEditor : FaceSlider {

  public string uniformName;
  public MeshRenderer renderer;
	
	public override void FaceSliderFire(float val){
    renderer.material.SetFloat(uniformName , val);
  }
  
}
