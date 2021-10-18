using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShootingController : MonoBehaviour
{
    public int damageAmount = 1;
    public FireModes fireMode;
    public LayerMask hitMasks;
    public float rateOfFire = 1;
    public bool shooting = false;
    public ParticleSystem muzzleFlash;
    public bool gunHeld = false;


    private void Fire() {
        shooting = true;
        //Play the muzzle flash particle system
        muzzleFlash.Play(true);
        //Play Gunshot noise
        //Raycast out from the characters center
        Ray ray;
        RaycastHit hit;
        //[NOTE] -> In the future the forward direction will have a random factor of the left or right added to it to simulate spread.
        ray = new Ray(this.transform.position, this.transform.forward);

        if (Physics.Raycast(ray, out hit, 250, hitMasks))
        {
            //If you hit something
            //      Create a hit particle effect
            //      Play a hit sound

            //Check if the object hit has a health component
            HealthController health = hit.transform.GetComponent<HealthController>();
            if (health != null)
            {
                //Damage the object that was hit.
                health.TakeDamage(damageAmount, this.transform);
                //Conditional Create a blood splatter graphic
            }

            //Impart physics if the object has a rigidbody
            Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
            if (rigidbody != null) {
                rigidbody.AddForce(100 * this.transform.forward,ForceMode.Impulse);
            }

            //Conditional Create a bullet hole graphic
        }
    }
}

public enum FireModes { 
    Single,
    Auto
}
