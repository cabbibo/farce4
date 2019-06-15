using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemParticles : Particles {

  public Form placedParticles;
  public int levels;
  public int particlesPerLevel;


  public struct TreeData{
    public int idOfParent;
    public int id;
    public int idInBranch;
    public int level;
  };

  public List<TreeData> treeData;


  public override void SetCount(){ 

    count = 0;



    treeData = new List<TreeData>();

    for(int i = 0; i < placedParticles.count ; i++ ){

      Recurse( i , 0, 0 );

    }


  }

  public void Recurse( int parentID , int idInBranch ,  int level  ){



    TreeData td = new TreeData();

    td.idOfParent = parentID;
    td.level = level;
    td.id = count;
    td.idInBranch = idInBranch;
    treeData.Add( td );

    int id = count;
    count ++;

    if( level < levels ){
      for( int i = 0; i < particlesPerLevel; i++ ){
        Recurse(id, i , level+1);
      }
    }
  }



  public override void Embody(){
    

    float[] values = new float[count*structSize];
    for( int i =0 ; i < count; i++ ){


      // need to change debug values! ID in branch and which level
      //uv will store ID of parent!
    
      values[ i * structSize + 6 + 0 ] = 0;
      values[ i * structSize + 6 + 1 ] = 1;
      values[ i * structSize + 6 + 2 ] = 0;

      values[ i * structSize + 12 + 0 ] = treeData[i].idOfParent;
      values[ i * structSize + 12 + 1 ] = treeData[i].id ;

      values[ i * structSize + 12 + 2 ] = treeData[i].idInBranch;
      values[ i * structSize + 12 + 3 ] = treeData[i].level;


    }

    SetData( values );


  }

}
