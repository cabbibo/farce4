using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.FaceSubsystem;
using UnityEngine.XR;
using UnityEngine.Experimental.XR;




public class MakeFaceMesh : Cycle {

  public LoadFaceMesh face;
  public string title;
  public Camera cameraTransform;
  private Mesh loadedFaceMesh;

  public Mesh mesh;

  public MutatingVerts verts;
  public MutatingTris tris;
 // public ARFaceManager faceManager;
  //private UnityARSessionNativeInterface m_session;


  public Body body;
  public FaceController faceController;

  public override void Create(){
    


    Cycles.Insert(0,verts);
    mesh =  face.Load(title);

    mesh.RecalculateBounds();
    mesh.RecalculateTangents();


    ((MutatingVerts)verts).mesh = mesh;

  }

  public override void OnBirthed(){
    print("hi");
    body.render.castShadows = false;

  }







  public void Set(){

    if( ((MutatingVerts)verts).mesh != null ){
     // print("setting");
      verts._WhileLiving(1);
    }
  }

}
