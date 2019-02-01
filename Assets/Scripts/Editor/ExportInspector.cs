using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Export))]
public class ExportInspector : Editor {
 
  private Export creator;

  private void OnEnable () {
    creator = target as Export ;
  }

  public override void OnInspectorGUI () {
    DrawDefaultInspector();
    if(GUILayout.Button("SAVE")){ creator.Save();  } 
    if(GUILayout.Button("TEST")){ creator.TestExport();  } 
  }

}
