using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOnPlacedParticles : Particles {

  public Form placedParticles;
  public int particlesPerParticle;

  public override void SetCount(){ count = particlesPerParticle * placedParticles.count;  }


}
