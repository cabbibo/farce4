using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTris : Form
{

  public Form vert;
  public IndexForm tris;
 

  public override void SetStructSize(){ structSize = 16; }
 public override void WhileDebug(){
   if( active){

    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", vert._buffer);
    debugMaterial.SetBuffer("_TriBuffer", tris._buffer);
    debugMaterial.SetInt("_Count",tris._buffer.count);
    Graphics.DrawProcedural(MeshTopology.Triangles, tris._buffer.count * 3 * 2 );
  }
 }

}
