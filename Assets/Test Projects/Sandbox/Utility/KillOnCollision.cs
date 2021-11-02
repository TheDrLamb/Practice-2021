using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour
{
    public float particleSystemsLifetime = 0.5f;
    public GameObject splat;
    ParticleSystem[] particleSystems;
    TrailRenderer trail;
    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject newSplat = Instantiate(splat);
        newSplat.transform.position = this.transform.position;
        newSplat.transform.rotation = this.transform.rotation;

        Destroy(newSplat, particleSystemsLifetime);

        trail.transform.parent = null;
        Destroy(trail.gameObject, particleSystemsLifetime / 2);

        foreach (ParticleSystem p in particleSystems) {
            p.transform.parent = null;
            Destroy(p.gameObject, particleSystemsLifetime);
        }

        Destroy(this.gameObject);
    }
}
