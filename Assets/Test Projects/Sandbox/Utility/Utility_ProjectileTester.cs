using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_ProjectileTester : MonoBehaviour
{
    public float timeInterval = 1.5f;
    public Rigidbody projectile;
    public float force = 2500;
    float timer = 0;

    private void Start()
    {
        Fire();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeInterval)
        {
            Fire();
        }
    }

    void Fire() {
        timer = 0;
        Rigidbody newProjectile = Instantiate(projectile, this.transform.position, this.transform.rotation);
        newProjectile.AddForce(force * this.transform.forward,ForceMode.Force);
    }
}
