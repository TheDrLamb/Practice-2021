using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunParticleController : MonoBehaviour
{
    CharacterInputController inputController;
    public ParticleSystem runDustParticles;

    private void Start()
    {
        inputController = GetComponent<CharacterInputController>();
    }

    private void Update()
    {
        if (inputController.IsMoving())
        {
            runDustParticles.Play(true);
        }
        else {
            runDustParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
