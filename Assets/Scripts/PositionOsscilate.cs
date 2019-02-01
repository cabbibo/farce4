using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOsscilate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.position += Vector3.left * Mathf.Sin( Time.time  * 10) * .4f;
		
	}
}
