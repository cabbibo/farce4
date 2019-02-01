using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEvent : MonoBehaviour {

  public int triggerDirection;
  public float triggerValue;
  public string type;

  /*void OnEnable(){
    GameObject.FindGameObjectWithTag("FACEEVENTS").GetComponent<BlendShapeEventSystem>().AddFaceEvent( this );
  }

  void OnDisable(){

    if( GameObject.FindGameObjectWithTag("FACEEVENTS") != null ){
      GameObject.FindGameObjectWithTag("FACEEVENTS").GetComponent<BlendShapeEventSystem>().RemoveFaceEvent( this );
    }

  }*/

	public virtual void FaceFire(){

  }

}
