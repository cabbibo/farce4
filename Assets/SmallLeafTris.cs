using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallLeafTris : IndexForm {


public Form verts;
  public override void SetCount(){count = verts.count * (3 * 2); }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    /*
  
       2-3
       |/| 
       0-1
      

    */
    for( int i = 0; i < verts.count; i++ ){
        
        values[ index ++ ] = i * 4 + 0;
        values[ index ++ ] = i * 4 + 1;
        values[ index ++ ] = i * 4 + 3;
        values[ index ++ ] = i * 4 + 3;
        values[ index ++ ] = i * 4 + 2;
        values[ index ++ ] = i * 4 + 0;

    }

    SetData(values);
  
  }

}
