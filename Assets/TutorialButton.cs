using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public Interface inter;
    public float leftRight;
    public float upDown;


    public float timeToActivate;
    public float timeToDeactivate;

    public float activationTime;
    public float deactivationTime;

    public float flashRate;
    public float shakeRate;

    public bool hasBeen;

    public bool active;

    public bool deactivating;
    public bool activating;


    public float baseScale;
    private float lastFlash;


    public Color targetCol;
    public float lerpRate;

    private Renderer r;
    public void Start(){

      r = GetComponent<Renderer>();
      baseScale = transform.localScale.x;

    }
    
    public void Activate(){
      
      r.enabled = true;
      activating = true;
      activationTime = Time.time;

      hasBeen = true;
      active = true;

    }

    public void Deactivate(){
      deactivating = true;
      deactivationTime= Time.time;
      active = false;
    }

    public void HardDeactivate(){
      active = false;
      deactivationTime = 0;
       r.enabled = false;

    }

    // Update is called once per frame
    void LateUpdate()
    {

      if( activating == true ){
        float v = (Time.time - activationTime);
        if( v > timeToActivate ){
          activating = false;
        }else{
          v = (v /timeToActivate);//* baseScale;
          //print(v);
          transform.localScale = new Vector3(v,v,v);
        }
      }

      if( deactivating == true ){
        float v = (Time.time - deactivationTime);
        if( v > timeToDeactivate ){
          deactivating = false;
          r.enabled = false;
        }else{
          v = (v / timeToDeactivate) * baseScale;
          v = 1-v;
          transform.localScale = new Vector3(v,v,v);
        }
      }

      transform.rotation = inter.viewpoint.transform.rotation;
      transform.position = inter.topLeft + (inter.topRight - inter.topLeft) * leftRight + (inter.bottomLeft-inter.topLeft) * upDown; 
 
      transform.Rotate(  Vector3.forward*Mathf.Sin(Time.time * shakeRate));

      if( Time.time-lastFlash > flashRate  ){
        lastFlash = Time.time;
        targetCol  = Random.ColorHSV(0f,.999f,.6f, .63f,1f, 1f);     
      }

      r.material.SetColor("_Color",Color.Lerp(r.material.GetColor("_Color") , targetCol , lerpRate));

    }
}
