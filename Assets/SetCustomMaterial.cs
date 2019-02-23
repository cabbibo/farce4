using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SetCustomMaterial : MonoBehaviour
{

  public FaceSwitcher controller;
    public ARCameraBackground bg;
    public Material mat1;
    public Material mat2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       bg.customMaterial = controller.lenses[controller.activeFace].BackgroundMaterial;
    
    }
}
