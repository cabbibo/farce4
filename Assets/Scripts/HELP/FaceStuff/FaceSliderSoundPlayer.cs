﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSliderSoundPlayer : FaceSlider {


  private AudioSource audio;
	// Use this for initialization
	void Start () {

    audio = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

   public override void FaceSliderFire(float val){
    audio.pitch = val * 3;
  }

}
