using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : LifeForm
{

  public Form form;
  public Life life;
  
  // Use this for initialization
  public override void Create(){

    Cycles.Insert(0,form);
    Cycles.Insert(1,life);



  }




  


}
