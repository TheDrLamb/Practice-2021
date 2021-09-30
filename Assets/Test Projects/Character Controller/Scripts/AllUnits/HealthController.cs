using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public UnitTypes unitType;
    public int health = 1;
    public Behaviour[] disableOnDeath;
    public Behaviour[] enableOnDeath;

    public void TakeDamage(int _dmgAmt) {
        health -= _dmgAmt;
        if (health <= 0) {
            Die();
        }
    }

    public void TakeDamage(int _dmgAmt, Transform _attacker)
    {
        health -= _dmgAmt;
        Die();
    }

    void Die()
    {
        foreach (Behaviour n in disableOnDeath)
        {
            n.enabled = false;
        }
        foreach (Behaviour n in enableOnDeath)
        {
            n.enabled = true;
        }

        if (unitType == UnitTypes.Enemy)
        {

            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = true;

            //[NOTE] -> This works for now, but in the future just ragdoll the enemy, use the standing method, then disable the collider and rigidbody when it comes to rest.
            //          Then probably start some kind of disolve animation so the map isnt littered with corpses.
            //          It may be cool to generate a rising Goo Mesh depending on where enemies die to create feedback for the number of enemies killed, like a morbid Splatoon
            //          The goo mesh could be made via a sort of marching cubes and add to the weights to the vertices according to the enemies that die in an area.
            //          Then possibly add tesselation to the goo to show the player trudging through it when it get deep.
        }
    }
}

public enum UnitTypes { 
    Enemy,
    Player
}
