using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Export : MonoBehaviour {

  public string exportName;
  public Material testMaterial;
  public Body[] bodiesToExport;

  public void Save(){
    foreach( Body body in bodiesToExport ){
      string name = "Bakes/" +exportName + "/_" + body.gameObject.name;
      body.Bake();
      ObjExporter.Save( name , body.mesh );
    }
  }

  public void TestExport(){

    foreach( Body body in bodiesToExport ){
      string name = "Bakes/" +exportName + "/_" + body.gameObject.name;
      body.Bake();

      GameObject go = new GameObject();
      go.name = body.gameObject.name;
      Renderer r = go.AddComponent<MeshRenderer>();
      MeshFilter m = go.AddComponent<MeshFilter>();

      r.material = testMaterial;
      m.mesh = body.mesh;

    }

  }

  
}
