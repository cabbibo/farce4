 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordIt : MonoBehaviour
{



    public TouchToRay touch;

    public Recorder recorder;
    public Haptico haptics;


    public float startScale;
    public float endScale;



    public LineRenderer border;
    public GameObject farcePlarnt;
    public GameObject farce;
    public GameObject plarnt;
    public LensSlider icons;



    private float baseScale;
    private Renderer r1;
    private Renderer r2;

    public void Start(){
      baseScale = icons.transform.localScale.x;
      r1 = farce.GetComponent<Renderer>();
      r2 = farce.GetComponent<Renderer>();

    }

    public void WhileDown(float v){
      farcePlarnt.transform.localScale = new Vector3( v*v*endScale,v*v*endScale,v*v*endScale );
     
      float v2 = Mathf.Lerp( baseScale , 0 , v );

      icons.invisibility = v;
      //icons.transform.localScale = new Vector3(v2,v2,v2);

      /*
      float newScale = baseScale * (1 + v*v * .4f);
      //transform.localScale = new Vector3( newScale , newScale , newScale );

      //border.SetWidth( startScale + v*v* .1f ,  startScale + v*v* .1f  );
      border.startWidth = startScale + v*v*(endScale-startScale) ;
      border.endWidth = startScale + v*v*(endScale-startScale) ;*/
      r1.material.SetColor("_Color", Color.Lerp( Color.white , Color.red , v * v ));
      r2.material.SetColor("_Color", Color.Lerp( Color.white , Color.red , v * v ));

    }

    public void ResetScale(){
     // transform.localScale = new Vector3( baseScale,baseScale,baseScale);
      //border.startWidth = startScale ;
      //border.endWidth = startScale ;

icons.invisibility = 0;
     // icons.transform.localScale = new Vector3(baseScale,baseScale,baseScale);
      farcePlarnt.transform.localScale = new Vector3(0,0,0);// v*v*endScale;
      r1.material.SetColor("_Color", Color.white);
      r2.material.SetColor("_Color", Color.white);
    }

    public void fullStart(){

      haptics.TriggerImpactMedium();
      //r.material.SetColor("_Color", Color.green);
     // border.startWidth = endScale * 1.1f ;
     // border.endWidth = endScale * 1.1f ;
     // icons.transform.localScale = new Vector3(0,0,0);// v*v*endScale;
icons.invisibility = 1;

      farcePlarnt.transform.localScale = new Vector3( 1.2f * endScale ,1.2f * endScale,1.2f * endScale);
      r1.material.SetColor("_Color",Color.green);
      r2.material.SetColor("_Color",Color.green);

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

      ResetScale();
      recorder.EndRecord();
    }
}
