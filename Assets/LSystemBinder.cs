using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemBinder : Cycle
{
    public Life toBind;

    public LSystemParticles particles;
    public PlacedDynamicMeshParticles anchors;

    public float branchHeight;
    public float spread;

    public Transform face;
    public Vector3 faceForward;
    public Vector3 facePosition;
    public Vector3 cameraPosition;
    public Vector3 cameraForward;

    public override void Bind(){

      toBind.BindPrimaryForm( "_VertBuffer" , particles );
      toBind.BindForm( "_AnchorBuffer" , anchors );

      toBind.BindAttribute("_ParticlesPerBranch", "particlesPerLevel",particles);
      toBind.BindAttribute("_Levels", "levels",particles);

      toBind.BindAttribute("_FaceForward", "faceForward",this);
      toBind.BindAttribute("_CameraForward", "cameraForward",this);
      toBind.BindAttribute("_CameraPosition", "cameraPosition",this);
      toBind.BindAttribute("_FacePosition", "facePosition",this);
      toBind.BindAttribute("_BranchHeight", "branchHeight",this);
      toBind.BindAttribute("_Spread", "spread",this);

    }

    public override void WhileLiving( float v ){
      faceForward    = face.forward;
      facePosition    = face.position;
      cameraForward  = Camera.main.transform.forward;
      cameraPosition = Camera.main.transform.position;
    }
}
