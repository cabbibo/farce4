using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaveIt : MonoBehaviour
{



    public TouchToRay touch;

    public Recorder recorder;


      
    public void Save(){

      RaycastHit hit;
      if (GetComponent<BoxCollider>().Raycast(touch.ray, out hit, 1000000.0f))
      {
        recorder.ShareBtnPress();
      }else{
        print("nahBoi");
      }
    }


}