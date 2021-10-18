using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunParticleController : MonoBehaviour
{
    public ParticleSystem runDustParticles;
    bool isMoving;

    private void Update()
    {
        if (isMoving)
        {
            runDustParticles.Play(true);
        }
        else {
            runDustParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void SetIsMoving(bool _isMoving) 
    {
        isMoving = _isMoving;
    }
}
