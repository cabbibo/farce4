using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInterface : MonoBehaviour
{

  public bool down;



    public void Toggle(){
      down = !down;

      GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");

      for( int i = 0; i < objs.Length; i++ ){
        if( objs[i].GetComponent<Renderer>() != null ){
         objs[i].GetComponent<Renderer>().enabled = down;
       }
      }

    }
}
