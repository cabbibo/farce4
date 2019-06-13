using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SuccTransferTris : IndexForm {

  public override void SetCount(){ count = 6*(toIndex.count/4); }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    for( int i = 0; i < toIndex.count/4; i++ ){
        values[ index ++ ] = i * 4 + 0;
        values[ index ++ ] = i * 4 + 1;
        values[ index ++ ] = i * 4 + 3;
        values[ index ++ ] = i * 4 + 0;
        values[ index ++ ] = i * 4 + 3;
        values[ index ++ ] = i * 4 + 2;
    }


    SetData(values);
  }

}

