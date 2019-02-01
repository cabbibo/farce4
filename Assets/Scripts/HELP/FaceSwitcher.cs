using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.iOS;

public class FaceSwitcher : Cycle {

  public Lens[] lenses;
  public MakeFaceMesh faceMesh;

  public MeshRenderer face;
  //public UnityARVideo bg;

  public LensSlider slider;

  public bool allOn;

  public int activeFace;
  public int activeFaceMat;
  public int activeBG;


	// Use this for initialization
	public override void Create() {

    Cycles.Insert(0,faceMesh);

    for( int i = 0; i < lenses.Length; i++ ){
      Cycles.Insert(i+1,lenses[i]);
    }

		
	}
	
  public override void OnBirthed(){
    SwitchFace();
  }



  public void Switch(float val){
    if( val > 0){
      activeFace -= 1;
      if( activeFace < 0 ){ activeFace = lenses.Length-1;}

    }else{
      activeFace += 1;
      activeFace %= lenses.Length;
    }

  
    SwitchFace();
  }

  void SwitchFace(){

  slider.Switch( activeFace );
    for( int i = 0; i < lenses.Length; i++ ){
        if( allOn ){
         lenses[i]._Deactivate();
        }  
    }


    lenses[activeFace]._Activate();

  }





}
