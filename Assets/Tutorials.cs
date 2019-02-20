using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{

  public TutorialButton yuk;
  public TutorialButton yum;
  public TutorialButton holdToRecord;
  public TutorialButton swipeToChange;


  public Recorder   recorder;

  public bool hasSwiped = false;
  public bool hasRecorded = false;

  public bool previewing;
  public float previewStartTime;
  public float timeToReshow;

  public float lastSwipeTime;
  public float lastRecordTime;

  public void Start(){
    lastRecordTime = Time.time;
    lastSwipeTime = Time.time;
  }

  public void PreviewShow(){
    previewing = true;
    previewStartTime = Time.time;
    holdToRecord.Deactivate();
    swipeToChange.Deactivate();
  }

  public void PreviewEnd(){
    previewing = false;
    yuk.Deactivate();
    yum.Deactivate();
  }

  public void RecordStart(){
    lastRecordTime = Time.time;
    yuk.Deactivate();
    yum.Deactivate();
    holdToRecord.HardDeactivate();
    swipeToChange.Deactivate();
    hasRecorded = true;
  }

  public void OnSwipe(float v){
    hasSwiped = true;
    lastSwipeTime = Time.time;
    lastRecordTime = Time.time;
    swipeToChange.Deactivate();
  }



  public void Update(){


    if( (Time.time - lastSwipeTime) > 3  && hasSwiped == false && swipeToChange.active == false){
      swipeToChange.Activate();

    }

    if( (Time.time - lastRecordTime) > 3 && holdToRecord.active == false && hasSwiped == true  && hasRecorded == false  && swipeToChange.active == false ){
      holdToRecord.Activate();
    }

    if(previewing){

      float dif = Time.time  - previewStartTime;
      if( dif > timeToReshow ){
        yuk.Activate();
        yum.Activate();
        timeToReshow += 6;
      }

    }

  }




}
