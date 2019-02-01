﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cycle : MonoBehaviour{

 public bool created = false;

 public bool begunGestation = false;
 public bool gestating = false;
 public bool gestated = false;

 public bool begunBirth = false;
 public bool birthing = false;
 public bool birthed = false;

 public bool begunLive = false;
 public bool living = false;
 public bool lived = false;

 public bool begunDeath = false;
 public bool dying = false;
 public bool died = false;

 public bool destroyed = true;

  public bool debug = false;
  public bool active = false;

  public List<Cycle> Cycles;

/*

  Creation

*/
  public virtual void _Create(){ DoCreate(); }
  public virtual void Create(){}

  protected void DoCreate(){

    //_Destroy();
    SetStates();

    if( created ){ DebugThis("Created Multiple Times"); }
    if( debug ){ DebugThis("DoCreate"); }
    Create();

    foreach( Cycle c in Cycles ){
      if( c == null ){
        DebugThis( "SOME CYCLE NULL");
      }

      c._Create();
    }

    destroyed = false;
    created = true;

  }

/*

  Gestation

*/

  public virtual void _OnGestate(){ DoGestate(); }
  public virtual void OnGestate(){}

  protected void DoGestate(){

    if( begunGestation ){ DebugThis("On Gestate Multiple Times"); }
    if( debug ){ DebugThis("DoGestate"); }
    begunGestation = true;

    _Bind();

    OnGestate();
    foreach( Cycle c in Cycles ){
      c._OnGestate();
    }

    gestating = true;

  }

  public virtual void _Bind(){Bind();}
  public virtual void Bind(){}
  
  public virtual void _WhileGestating(float v){ DoGestating(v); }
  public virtual void WhileGestating(float v){}

  protected void DoGestating(float v){
    WhileGestating(v);
    foreach( Cycle c in Cycles ){
      c._WhileGestating(v);
    }
  }
  


  public virtual void _OnGestated(){ DoGestated(); }
  public virtual void OnGestated(){}

  protected void DoGestated(){

    if( gestated ){ DebugThis("On Gestated Multiple Times"); }
    if( debug ){ DebugThis("DoGestated"); }
    gestating = false;
    OnGestated();
    foreach( Cycle c in Cycles ){
      c._OnGestated();
    }
    gestated = true;

  }



/*

  Birth

*/


  public virtual void _OnBirth(){ DoBirth();}
  public virtual void OnBirth(){}

  protected void DoBirth(){
    if( begunBirth ){ DebugThis("begunBirth multiple times"); }
    if( debug ){ DebugThis("DoBirth"); }
    begunBirth = true;
    OnBirth();
    foreach( Cycle c in Cycles ){
      c._OnBirth();
    }
    birthing = true;
  }

  

  public virtual void _WhileBirthing(float v){ DoBirthing(v);}
  public virtual void WhileBirthing(float v){}

  protected void DoBirthing(float v){
    WhileBirthing(v); 
    foreach( Cycle c in Cycles ){
      c._WhileBirthing(v);
    }
  }
  

  public virtual void _OnBirthed(){ DoBirthed(); }
  public virtual void OnBirthed(){}

  protected void DoBirthed(){
    if( birthed ){ DebugThis("On Birthed Multiple Times"); }
    birthing = false;
    OnBirthed();
    foreach( Cycle c in Cycles ){
      c._OnBirthed();
    }
    birthed = true;
  }


/*

  LIVE

*/

  public virtual void _OnLive(){ DoLive(); }
  public virtual void OnLive(){}

  protected void DoLive(){
    if( living ){ DebugThis("BegunLive Multiple Times"); }
    begunLive = true;
    OnLive();
    foreach( Cycle c in Cycles ){
      c._OnLive();
    }
    living = true;
  }
  
  public virtual void _WhileLiving(float v){ DoLiving(v);}
  public virtual void WhileLiving(float v){}

  protected void DoLiving(float v){
    WhileLiving(v);
    foreach( Cycle c in Cycles ){
      c._WhileLiving(v);
    }
  }
  
  public virtual void _OnLived(){ DoLived(); }
  public virtual void OnLived(){}

  void DoLived(){
    if( lived ){ DebugThis("on lived Multiple Times"); }
    living = false;
    OnLived();
    foreach( Cycle c in Cycles ){
      c._OnLived();
    }
    lived = true;
  }



/*

  DEATH

*/


  public virtual void _OnDie(){ DoDie(); }
  public virtual void OnDie(){}

  protected void DoDie(){
    if( begunDeath ){ DebugThis("On Die Multiple Times"); }
    begunDeath = true;
    OnDie();
    foreach( Cycle c in Cycles ){
      c._OnDie();
    }
    dying = true;
  }
  
  public virtual void _WhileDying(float v){ DoDying(v); }
  public virtual void WhileDying(float v){}

  protected void DoDying(float v){

    WhileDying(v);    
    foreach( Cycle c in Cycles ){
      c._WhileDying(v);
    }
  }
  
  public virtual void _OnDied(){ DoDied(); }
  public virtual void OnDied(){}

  protected void DoDied(){
    if( died ){ DebugThis("On Died Multiple Times"); }
    dying = false;
    OnDied();
    foreach( Cycle c in Cycles ){
      c._OnDied();
    }
    died = true;
  }



  /*
      Destroy
  */

  public virtual void _Destroy(){ DoDestroy(); }
  public virtual void Destroy(){}

  protected void DoDestroy(){
    SetStates();
    foreach( Cycle c in Cycles ){
      c._Destroy();
    }
    Destroy();
  }



/*

  Activate Deactivate

*/

public virtual void _Activate(){
  Activate();
  foreach( Cycle c in Cycles ){
    c._Activate();
  }
  active = true;
}
public virtual void Activate(){}

public virtual void _Deactivate(){
  Deactivate();
   foreach( Cycle c in Cycles ){
    c._Deactivate();
  }
  active = false;
}
public virtual void Deactivate(){}


void SetStates(){
      created = false;
    begunGestation = false;
    gestating = false;
    gestated = false;
    begunBirth = false;
    birthed = false;
    birthing = false;
    begunLive = false;
    living = false;
    lived = false;
    begunDeath = false;
    dying = false;
    died = false;
    destroyed = true;
}

  /*
        DEBUG
  */

  public virtual void _WhileDebug(){ DoDebug(); }
  public virtual void WhileDebug(){}

  protected void DoDebug(){
    
    if( debug ){ WhileDebug(); }

    foreach( Cycle c in Cycles ){
      c._WhileDebug();
    }
    
  }


  /*
    Helpers
  */
  public void DebugThis( string s ){
    print( "Object Name : " + this.gameObject.name +"     || Script Name : "+this.GetType()+ "     || Message: " + s );
  }

}
