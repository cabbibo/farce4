using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.iOS;

public class CheckToSeeIfTrueDepth : MonoBehaviour
{


    public bool doCheck;
    public Canvas canvas;
    // Start is called before the first frame update
    void Awake()
    {

      if( doCheck ){

        if(    Device.generation == DeviceGeneration.iPadPro3Gen
            || Device.generation == DeviceGeneration.iPadPro11Inch
            || Device.generation == DeviceGeneration.iPhoneX
            || Device.generation == DeviceGeneration.iPhoneXR
            || Device.generation == DeviceGeneration.iPhoneXS
            || Device.generation == DeviceGeneration.iPhoneXSMax
            || Device.generation == DeviceGeneration.iPhoneXSMax
        ){

          Supported();

        }else{
          NotSupported();
        }

      }else{
        Supported();
      }
        
    }


    void Supported(){
      canvas.enabled = false;
    }

    void NotSupported(){
      Screen.orientation = ScreenOrientation.Portrait;
      Camera.main.GetComponent<TouchToRay>().enabled = false;
    }
  


}

