using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KillIt : MonoBehaviour
{



    public TouchToRay touch;

    public Recorder recorder;


      
    public void Kill(){

      RaycastHit hit;
      if (GetComponent<BoxCollider>().Raycast(touch.ray, out hit, 1000000.0f))
      {
        recorder.KillShareBtnPress();
      }else{
        print("nahBoi");
      }
    }


}