using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldDownFeedback : MonoBehaviour
{

    public float timeTilSelect;
    public float velCutoff;

    public UnityEvent OnTrigger;
    public UnityEvent OnUpEvent;
    public FloatEvent WhileCounting;

    public bool counting;

    public void OnDown(){
      counting = true;
    }

    public void OnUp(){
      counting = false;
      OnUpEvent.Invoke();
    }

    public void HoldDown( Vector2 vel , float time ){

//      print( vel );
 

      if( counting ){

        if( vel.magnitude > velCutoff ){
          OnUp();
        }
       // print()
        WhileCounting.Invoke( time / timeTilSelect );
        if( time > timeTilSelect ){
          OnTrigger.Invoke();
          counting = false;
        }
      }

    }
}
