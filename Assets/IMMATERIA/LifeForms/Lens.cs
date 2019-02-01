using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.iOS;
using UnityEngine.XR.ARFoundation;

public class Lens : LifeForm {
  public Texture icon;

  public Material BlitMaterial;
  public Material FaceMaterial;
  public Material BackgroundMaterial;

  public GameObject background;
  public Material skybox;

  public override void Activate(){

    MeshRenderer r = GameObject.FindGameObjectWithTag("FACE").GetComponent<MeshRenderer>();
  
    r.material = FaceMaterial;
   // Camera.main.GetComponent<ARCameraBackground>().customMaterial = BackgroundMaterial;
  }


  public override void Deactivate(){

   // background.SetActive(false);
 
  }


}
