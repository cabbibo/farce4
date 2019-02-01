using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSoundEvent : FaceEvent {

  public AudioPlayer player;
  public AudioClip clip;
  public int step;

	public override void FaceFire(){
    player.Play(clip, step , 1);
  }
  
}
