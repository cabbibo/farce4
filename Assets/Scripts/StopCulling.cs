using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StopCulling : MonoBehaviour
 {
     private Camera cam;
 
     void Start()
     {
         cam = Camera.main;// this.GetComponent<Camera>();
     }
 
     void OnPreCull()
     {

        cam = Camera.main;
        Debug.Log( cam );

         cam.cullingMatrix = Matrix4x4.Ortho(-99999, 99999, -99999, 99999, 0.001f, 99999) * 
                             Matrix4x4.Translate(Vector3.forward * -99999 / 2f) * 
                             cam.worldToCameraMatrix;
     }
 
     void OnDisable()
     {
         cam.ResetCullingMatrix();
     }
 }