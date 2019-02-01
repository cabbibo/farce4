using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindHands : MonoBehaviour {

  public Life toBind;
  public Hands hands;


	// Use this for initialization
	void Start () {
    toBind.BindAttribute( "_HandL"  , "handL" , hands );
    toBind.BindAttribute( "_HandR"  , "handR" , hands );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
