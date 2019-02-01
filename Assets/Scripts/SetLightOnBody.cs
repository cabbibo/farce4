using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightOnBody : MonoBehaviour {

  public Body body;
  public Transform light;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    body.render.material.SetVector("_LightPos" , light.position );
		
	}
}
