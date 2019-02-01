using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSlider : MonoBehaviour {

  public float min;
  public float max;
  public string type;

  /*void OnEnable(){
    GameObject.FindGameObjectWithTag("FACEEVENTS").GetComponent<BlendShapeEventSystem>().AddFaceSlider( this );
  }

  void OnDisable(){

    if( GameObject.FindGameObjectWithTag("FACEEVENTS") != null ){
      GameObject.FindGameObjectWithTag("FACEEVENTS").GetComponent<BlendShapeEventSystem>().RemoveFaceSlider( this );
    }

  }
*/
  public virtual void FaceSliderFire(float val){

  }

}
