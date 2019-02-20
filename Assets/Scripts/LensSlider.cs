using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensSlider : MonoBehaviour {


  public FaceSwitcher switcher;
  public GameObject lensPrefab;
  public Material iconMat;
  public List<GameObject> lenses;
  public float lockPosition;
  public float anchorPosition;

  public float velocity;
  public float pos;
  public float maxDist;

  public float buttonSize;
  public float buttonPadding;
  public float invisibility;

  public float offsetVal;



	// Use this for initialization
	void Start () {

    float index = 0;
    foreach(Lens l in switcher.lenses){

      GameObject lens = Instantiate( lensPrefab );


      lens.transform.localScale = Vector3.one * buttonSize;
      lens.transform.position = Vector3.left * index * ( buttonSize + buttonPadding );
      lens.transform.parent = transform;
      //lens.GetComponent<MeshRenderer>().material = iconMat;//SetTexture("_MainTex",l.icon);
      //lens.GetComponent<MeshRenderer>().material.SetTexture("_MainTex",l.icon);
      lens.GetComponent<MeshRenderer>().enabled = true;//material.SetTexture("_MainTex",l.icon);
      lens.GetComponent<TextMesh>().text = l.gameObject.name;

      lenses.Add(lens);

      index ++;

    }
		
	}

  public void Switch(int id){
    anchorPosition = (float)id * ( buttonSize + buttonPadding );
    //SwitchWithPos( id , 1 );
  }
	
  public void SwitchWithPos( int id  , float dif){

    anchorPosition = (float)id * ( buttonSize + buttonPadding );
    velocity = 0;
    pos = -anchorPosition;
    pos += dif * offsetVal;
  }
	// Update is called once per frame
	void FixedUpdate() {


    velocity += -anchorPosition-pos;
    pos += velocity * .02f;
    velocity *= .8f;

    if( pos > maxDist ){
      pos = maxDist;
    }else if( pos < -maxDist ){
      pos = -maxDist;
    }




    foreach(GameObject l in lenses){
      float dif = l.transform.localPosition.x - pos;
      l.transform.localScale = (1-invisibility) * Vector3.one * (( buttonSize / Mathf.Max(.5f,(Mathf.Abs(dif) * 50))) );//l.transform.localPosition.x
    }

    transform.localPosition = Vector3.left * pos; // velocity;
		
	}


}
