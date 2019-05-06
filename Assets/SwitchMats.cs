using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMats : MonoBehaviour
{

    public Material[] mats;
    public Shader[] shads;
    public Renderer rend;

    public int cur = 0;
    public void Start(){
      mats = new Material[shads.Length];
      for( int i = 0; i < shads.Length; i++ ){
        mats[i] = new Material(shads[i]);
      }
    }

    public void Switch(){
      cur ++;
      cur %= mats.Length;
      rend.material = mats[cur];

    }
}
