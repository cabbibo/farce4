using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.iOS;

public class BlendShapeEventSystem : MonoBehaviour {
/*
  bool shapeEnabled = false;
  Dictionary<string, float> currentBlendShapes;
  Dictionary<string, float> oldBlendShapes;
  Dictionary<string, float> safeShapes;


  float[] oldValues;

  private List<FaceEvent> faceEvents = new List<FaceEvent>();
  private List<FaceSlider> faceSliders = new List<FaceSlider>();


  // Use this for initialization
  void Start () {

    UnityARSessionNativeInterface.ARFaceAnchorAddedEvent   += FaceAdded;
    UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
    UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;

  }

  void OnGUI()
  {

  }

  void Update(){



    if( safeShapes != null ){

    Debug.Log("safe");
    foreach(KeyValuePair<string,float> kvp in safeShapes) {
      currentBlendShapes[kvp.Key] = kvp.Value;
    }


    if( oldBlendShapes != null && currentBlendShapes != null){


      CheckFaceEvents();
      CheckFaceSliders();


    }



    if( currentBlendShapes != null ){

      foreach(KeyValuePair<string,float> kvp in currentBlendShapes) {
        oldBlendShapes[kvp.Key] = kvp.Value;
      }

    }
  }
  }

  void CheckFaceEvents(){
      for( int i = 0; i < faceEvents.Count; i++ ){
          FaceEvent faceEvent = faceEvents[i];

        if( currentBlendShapes.ContainsKey(faceEvent.type) ){
          float oldVal = oldBlendShapes[faceEvent.type];
          float newVal = currentBlendShapes[faceEvent.type];

          if( faceEvent.triggerDirection == 1 ){
            //print("y1");
            if( oldVal < faceEvent.triggerValue && newVal >= faceEvent.triggerValue ){
              faceEvent.FaceFire();
            } 
          }

          if( faceEvent.triggerDirection == -1 ){
            if( oldVal >= faceEvent.triggerValue && newVal < faceEvent.triggerValue ){
              faceEvent.FaceFire();
            } 
          }
        }
      }
  }
  void CheckFaceSliders(){
    for( int i = 0; i < faceSliders.Count; i++ ){
        FaceSlider faceSlider = faceSliders[i];

      if( currentBlendShapes.ContainsKey(faceSlider.type) ){

        float newVal = currentBlendShapes[faceSlider.type];

        float v = Mathf.Clamp( (newVal - faceSlider.min)/ (faceSlider.max-faceSlider.min),0,1);
        faceSlider.FaceSliderFire(v);
        
      }
    }
  }

  void FaceAdded (ARFaceAnchor anchorData)
  {
    shapeEnabled = true;
    safeShapes = anchorData.blendShapes;
    
    oldBlendShapes = new Dictionary<string,float>();
    currentBlendShapes = new Dictionary<string,float>();

    currentBlendShapes.Clear();
    foreach(KeyValuePair<string,float> kvp in safeShapes) {
      currentBlendShapes.Add(kvp.Key,kvp.Value);
    }

    oldBlendShapes.Clear();
    foreach(KeyValuePair<string,float> kvp in safeShapes) {
      oldBlendShapes.Add(kvp.Key,kvp.Value);
    }

  }

  void FaceUpdated (ARFaceAnchor anchorData)
  {

   // print( "faceUpdated");


    safeShapes = anchorData.blendShapes;



  }

  public void AddFaceEvent( FaceEvent faceEvent ){
    faceEvents.Add( faceEvent );
  }


  public void RemoveFaceEvent( FaceEvent faceEvent ){
    faceEvents.Remove( faceEvent );
  }


  public void AddFaceSlider( FaceSlider faceEvent ){
    faceSliders.Add( faceEvent );
  }


  public void RemoveFaceSlider( FaceSlider faceEvent ){
    faceSliders.Remove( faceEvent );
  }



  void FaceRemoved (ARFaceAnchor anchorData)
  {
    shapeEnabled = false;
  }

 */
}