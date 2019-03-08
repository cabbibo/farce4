using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGemTris : IndexForm {


  public Form verts;
  public DandelionVerts dandyVerts;

  public override void SetCount(){count = verts.count * (dandyVerts.vertsPerVert * 3 * 2); }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    /*
  
       2-3
       |/| 
       0-1
      

    */
    for( int i = 0; i < verts.count ; i++ ){
      for( int j = 0; j < dandyVerts.vertsPerVert; j++ ){
        values[ index ++ ] = i * (dandyVerts.vertsPerVert + 2) + j;
        values[ index ++ ] = i * (dandyVerts.vertsPerVert + 2) + ((j+1)%dandyVerts.vertsPerVert);
        values[ index ++ ] = i * (dandyVerts.vertsPerVert + 2) + dandyVerts.vertsPerVert;

        values[ index ++ ] = i * (dandyVerts.vertsPerVert + 2) + j;
        values[ index ++ ] = i * (dandyVerts.vertsPerVert + 2) + ((j+1)%dandyVerts.vertsPerVert);
        values[ index ++ ] = i * (dandyVerts.vertsPerVert + 2) + dandyVerts.vertsPerVert+1;
      }
    }

    SetData(values);
  
  }

}
