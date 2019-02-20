using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileRecording : MonoBehaviour
{

    public Renderer Face;
    public Renderer Plant;

    public float flashRate;
    public float shakeRate;

    private float lastFlashFace;
    private float lastFlashPlant;

    private Color plantCol;
    private Color faceCol;


    void Start(){
      lastFlashPlant = 0;
      lastFlashFace = flashRate * .5f;
    }

    void Update(){
     
      transform.Rotate(  Vector3.forward*Mathf.Sin(Time.time * shakeRate));
      if( Time.time - lastFlashFace > flashRate ){
        lastFlashFace = Time.time;
        faceCol =  Random.ColorHSV(0f,.999f,1f, 1f,1f, 1f);
      }

      Face.material.SetColor("_Color",Color.Lerp( Face.material.GetColor("_Color") , faceCol , .4f));


      if( Time.time - lastFlashPlant > flashRate ){
        lastFlashPlant = Time.time;
        plantCol = Random.ColorHSV(0f,.999f,1f, 1f,1f, 1f);

      }

        Plant.material.SetColor("_Color",Color.Lerp( Plant.material.GetColor("_Color") , plantCol , .4f));


    }

}
