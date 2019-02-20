using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAway : MonoBehaviour
{

  public Interface inter;

  public Recorder recorder;

  public bool down = false;

  public float left;
  public float up;

  public float upVal;
  public float leftVal;


  public float leftVel;
  public float upVel;


public float tUp;
public float tLeft;

public float leftCutoff;
public float rightCutoff;

public float swipeLeft;
public float swipeRight;

public float triggerVal;

    // Start is called before the first frame update
    void Start()
    {

      tUp = 0;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {



      if( recorder.previewing == true ){
        if( left < leftCutoff ){
          tLeft = swipeLeft;
        }

        if( left > rightCutoff ){
          tLeft = swipeRight;
        }

        if( left < rightCutoff  && left > leftCutoff ){
          tLeft = 0;
        }


        if( down == false ){
         
          up = Mathf.Lerp( up,tUp , .1f);//(up - tUp) .8f;
         
          left = Mathf.Lerp( left,tLeft , .1f);//(up - tUp) .8f;
          

        }


        upVel *= .9f;
        leftVel *= .9f;

        up += upVel;
        left += leftVel;




        transform.rotation = inter.viewpoint.rotation;
        transform.position = - inter.viewpoint.forward * .0001f + inter.bottomLeft - ( inter.bottomLeft-inter.bottomRight) * .5f + ( inter.topLeft-inter.bottomLeft ) * .5f + inter.right * left *leftVal + inter.up * up * upVal;
        transform.localScale = inter.transform.localScale * .9f;
          

        down = false;


        if( Mathf.Abs( left - swipeLeft ) < triggerVal ){
          recorder.KillShareBtnPress();
          up = 0;
          left = 0;

          upVel = 0;
          leftVel = 0;

          tLeft = 0;
        } 

        if( Mathf.Abs( left - swipeRight ) < triggerVal ){
          recorder.ShareBtnPress();
           up = 0;
          left = 0;
          upVel = 0;
          leftVel = 0;
          tLeft = 0;
        } 

      }
    }

    public void WhileDown( Vector2 vel , float time ){

      down = true;
      upVel = vel.y;
      leftVel = vel.x;

    }
}
