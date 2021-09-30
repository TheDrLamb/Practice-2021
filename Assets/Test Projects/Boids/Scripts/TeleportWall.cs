using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    public bool vertical = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //On detecting a boid colliding with it, set the boids position to the opposite side of the screen
        if (collision.collider.GetComponent<Boid>() != null) {
            Vector3 pos = collision.collider.transform.position;
            if (vertical) {
                collision.collider.transform.position = new Vector3(pos.x, -pos.y, 0);
            }
            else {
                collision.collider.transform.position = new Vector3(-pos.x, pos.y, 0);
            }
        }
    }
}
