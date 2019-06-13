using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTris : IndexForm {


  public GemVerts verts;
  public Form particles;

  public override void SetCount(){count = particles.count * 3 * verts.numSides*3; }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    /*
  
      
      Each Layer
      0 -- 1
      |    |
      3 -- 2


    */


    for( int i = 0; i < particles.count; i++ ){
      for( int j = 0; j < verts.numSides; j++ ){

        int baseID = i * (verts.numSides * 2 + 1);


        values[ index ++ ] = baseID + j;
        values[ index ++ ] = baseID + ((j+1)%verts.numSides);
        values[ index ++ ] = baseID + verts.numSides + ((j+1)%verts.numSides);


        values[ index ++ ] = baseID + j;
        values[ index ++ ] = baseID + verts.numSides + ((j+1)%verts.numSides);
        values[ index ++ ] = baseID + verts.numSides + j;

        values[ index ++ ] = baseID + verts.numSides + j;
        values[ index ++ ] = baseID + verts.numSides + ((j+1)%verts.numSides);
        values[ index ++ ] = baseID + verts.numSides * 2 ;


       
      }
    }

    int max = 0;
    for( int i = 0; i < values.Length; i++  ){
      if( values[i] > max ){ max = values[i];}

    }
    print( max );
    SetData(values);
    
  }

}
