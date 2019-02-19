using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordIt : MonoBehaviour
{



    public TouchToRay touch;

    public Recorder recorder;
    public Haptico haptics;

    private float baseScale;
    private Renderer r;
    public void Start(){
      baseScale = transform.localScale.x;
      r = GetComponent<Renderer>();
    }

    public void WhileDown(float v){
      float newScale = baseScale * (1 + v*v * .4f);
      transform.localScale = new Vector3( newScale , newScale , newScale );
      r.material.SetColor("_Color", Color.Lerp( Color.white , Color.red , v * v ));

    }

    public void ResetScale(){
      transform.localScale = new Vector3( baseScale,baseScale,baseScale);
      r.material.SetColor("_Color", Color.white);
    }

    public void fullStart(){
      haptics.TriggerImpactMedium();
      r.material.SetColor("_Color", Color.green);
      recorder.StartRecord();
    }
      
    public void StartIt(){


      RaycastHit hit;
        if (GetComponent<BoxCollider>().Raycast(touch.ray, out hit, 1000000.0f))
        {
          fullStart();
        }else{
          print("nahBoi");
        }
    }

    public void EndIt(){

      recorder.EndRecord();
    }
}
