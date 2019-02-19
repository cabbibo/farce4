using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordIt : MonoBehaviour
{



    public TouchToRay touch;

    public Recorder recorder;


      
    public void StartIt(){


      RaycastHit hit;
        if (GetComponent<BoxCollider>().Raycast(touch.ray, out hit, 1000000.0f))
        {
      
          recorder.StartRecord();
        }else{
          print("nahBoi");
        }
    }

    public void EndIt(){
      recorder.EndRecord();
    }
}
