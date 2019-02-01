using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMeDebug : Cycle {

  public MutatingVerts  faceVerts;
  public Material       faceVertsMaterial;


  public MutatingTris  faceTris;
  public Material       faceTrisMaterial;

  public InstancedMeshVerts meshVerts;
  public InstancedMeshTris meshTris;

  public Transform face;

	// Use this for initialization
	public override void Activate() {
		  print("WHEEEE");

      //faceVerts.debug = true;
      //faceTris.debug = true;

      meshVerts.debug = true;
      meshTris.debug = true;
      debug = true;
	}
	 

  public override void Deactivate(){

      //faceVerts.debug = false;
      //faceTris.debug = false;

      meshVerts.debug = false;
      meshTris.debug = false;

      debug = false;
  }



  public override void WhileDebug(){

    faceVertsMaterial.SetPass(0);
    faceVertsMaterial.SetBuffer("_VertBuffer", faceVerts._buffer);
    faceVertsMaterial.SetInt("_Count",faceVerts.count);
    faceVertsMaterial.SetMatrix("_Transform",face.localToWorldMatrix);

    Graphics.DrawProcedural(MeshTopology.Triangles, faceVerts.count * 3 * 2 );

    faceTrisMaterial.SetPass(0);
    faceTrisMaterial.SetMatrix("_Transform",face.localToWorldMatrix);
    faceTrisMaterial.SetBuffer("_VertBuffer", faceVerts._buffer);
    faceTrisMaterial.SetBuffer("_TriBuffer", faceTris._buffer);
    faceTrisMaterial.SetInt("_Count",faceTris.count);
    faceTrisMaterial.SetInt("_VertCount",faceVerts.count);
    Graphics.DrawProcedural(MeshTopology.Triangles, ((faceTris.count-1) * 2) * 3 * 2 );
    
  }




}
