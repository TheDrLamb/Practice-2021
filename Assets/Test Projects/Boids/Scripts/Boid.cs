using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidController controller;
    List<Transform> visibleBoids;
    float viewAngle;
    float viewRadius;
    float velocity;
    float minVelocity;
    float currentVelocity;
    float rotationSpeed;

    float alignmentWeight;
    Vector3 alignment;

    float cohesionWeight;
     Vector3 cohesion;

    float separationWeight;
    Vector3 separation;

    float avoidanceWeight;

    void Start()
    {
        visibleBoids = new List<Transform>();
    }

    void Update()
    {
        Vector3 targetDir = transform.up;
        cohesion = separation = alignment = Vector3.zero;
        //Check For Obstacles
        if (ObstacleTouching())
        {
            Vector3 obstSep = GetObstacleSeparation();
            Vector3 avoidance = GetAvoidanceVector();
            targetDir = (avoidance + obstSep).normalized;
            currentVelocity = minVelocity;
            Debug.DrawRay(transform.position, avoidance, Color.yellow);
            
        }
        else if (ObstructionCheck())
        {
            //Get Obstacle Avoidance Vector, and add it to the separation vector to get the new target dir.
            Vector3 avoidance = GetAvoidanceVector();
            Debug.DrawRay(transform.position, avoidance, Color.magenta);
            targetDir = avoidance;
        }
        else
        {
            GetBoidsInSight();
            GetCohesion();
            GetAlignment();
            GetSeparation();
            targetDir = (cohesion + separation + alignment).normalized;
            currentVelocity = velocity;
        }

        //Will need to be changed to Forward for 3D implementation
        Vector3 lookDir = Vector3.RotateTowards(transform.up, targetDir, rotationSpeed * Time.deltaTime, 0.0f);

        
        Debug.DrawRay(transform.position, separation, Color.red);
        Debug.DrawRay(transform.position, cohesion, Color.green);
        Debug.DrawRay(transform.position, alignment, Color.blue);
        Debug.DrawRay(transform.position, lookDir, Color.white);

        //Up defined as towards the camera for 2D, will be just up in 3D
        transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir);
        transform.position += transform.up * currentVelocity * Time.deltaTime;
    }

    private void LateUpdate()
    {
        viewAngle = controller.viewAngle;
        viewRadius = controller.viewRadius;
        velocity = controller.velocity;
        minVelocity = controller.minVelocity;
        rotationSpeed = controller.rotationSpeed;
        separationWeight = controller.separationWeight;
        cohesionWeight = controller.cohesionWeight;
        alignmentWeight = controller.alignmentWeight;
        avoidanceWeight = controller.avoidanceWeight;
    }

    private bool ObstacleTouching() {
        LayerMask obstacles = LayerMask.GetMask("Obstacle");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, avoidanceWeight, obstacles);
        if (colliders.Length > 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private Vector3 GetObstacleSeparation() {
        LayerMask obstacles = LayerMask.GetMask("Obstacle");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, avoidanceWeight, obstacles);
        Vector3 average = new Vector3();
        foreach (Collider2D col in colliders) {
            average += transform.position - col.transform.position;
        }
        if (colliders.Length > 1) {
            average /= colliders.Length;
        }
        return average.normalized;

    }

    private bool ObstructionCheck() {
        //Cast out a ray in front of the boid that goes to its view radius
        //Return true if the ray collides with an obstacle
        //else false.
        LayerMask obstacles = LayerMask.GetMask("Obstacle");

        if (LollipopCheck(transform.up, viewRadius, obstacles))
        {
            return true;
        }
        else
        {
            float rotCorrection = Mathf.Abs(transform.eulerAngles.z) >= 180.00000f ? 2 * Mathf.PI : Mathf.PI;
            float upAngle = Mathf.Atan(transform.up.y / transform.up.x);
            for (int i = 1; i < 3; i++)
            {
                float incAngleP = upAngle + (controller.increment * i) - rotCorrection;
                float incAngleN = upAngle - (controller.increment * i) - rotCorrection;
                Vector3 incVectorP = new Vector3(Mathf.Cos(incAngleP), Mathf.Sin(incAngleP)).normalized;
                Vector3 incVectorN = new Vector3(Mathf.Cos(incAngleN), Mathf.Sin(incAngleN)).normalized;
                if (RayCheck(incVectorP, viewRadius, obstacles))
                {
                    return true;
                }
                else if (RayCheck(incVectorN, viewRadius, obstacles))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Vector3 GetAvoidanceVector() {
        //Starting from the transforms Up position cast rays out at increasing angles, changing from left to right until an obstacle is not hit anymore
        LayerMask obstacles = LayerMask.GetMask("Obstacle");
        float rotCorrection = Mathf.Abs(transform.eulerAngles.z) >= 180.00000f ? 2 * Mathf.PI : Mathf.PI;
        float upAngle = Mathf.Atan(transform.up.y / transform.up.x);
        float hitDistance = viewRadius;
        for (int i = 1; i < controller.avoidanceIncrements; i++)
        {
            float incAngleP = upAngle + (controller.increment * i) - rotCorrection;
            float incAngleN = upAngle - (controller.increment * i) - rotCorrection;
            Vector3 incVectorP = new Vector3(Mathf.Cos(incAngleP), Mathf.Sin(incAngleP)).normalized;
            Vector3 incVectorN = new Vector3(Mathf.Cos(incAngleN), Mathf.Sin(incAngleN)).normalized;

            if (i % 2 == 0)
            {
                //Test Positive First
                if (!RayCheck(incVectorP, viewRadius, obstacles, out hitDistance))
                {                  
                    float weightedAngle = (upAngle + (controller.increment * (i + avoidanceWeight))) - rotCorrection;
                    return new Vector3(Mathf.Cos(weightedAngle), Mathf.Sin(weightedAngle)).normalized; 
                }
                else if (!RayCheck(incVectorN, viewRadius, obstacles, out hitDistance)) 
                {
                    float weightedAngle = (upAngle - (controller.increment * (i + avoidanceWeight))) - rotCorrection;
                    return new Vector3(Mathf.Cos(weightedAngle), Mathf.Sin(weightedAngle)).normalized; 
                }
            }
            else 
            {
                //Test Negative First
                if (!RayCheck(incVectorN, viewRadius, obstacles, out hitDistance))
                {
                    float weightedAngle = (upAngle - (controller.increment * (i + avoidanceWeight))) - rotCorrection;
                    return new Vector3(Mathf.Cos(weightedAngle), Mathf.Sin(weightedAngle)).normalized;
                }
                else if (!RayCheck(incVectorP, viewRadius, obstacles, out hitDistance))
                {
                    float weightedAngle = (upAngle + (controller.increment * (i + avoidanceWeight))) - rotCorrection;
                    return new Vector3(Mathf.Cos(weightedAngle), Mathf.Sin(weightedAngle)).normalized; 
                }
            }
        }
        currentVelocity = Mathf.Clamp((hitDistance / viewRadius) * velocity, minVelocity, velocity);
        return transform.right;
    }

    private void GetBoidsInSight()
    {
        //Gets all colliders in in the Boid:8 layermask
        LayerMask boids = LayerMask.GetMask("Boid");
        LayerMask obstacles = LayerMask.GetMask("Obstacle");
        visibleBoids.Clear();
        Collider2D[] boidsInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, boids);
        foreach (Collider2D col in boidsInRadius)
        {
            if (col.gameObject != this.gameObject)
            {
                Transform target = col.transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                //For implementation to 3D up will need to be adjusted to Forward.
                if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
                {
                    //Check for obstructed vision
                    float dist = Vector3.Distance(transform.position, target.position);
                    if (!Physics2D.Raycast(transform.position, dirToTarget, dist, obstacles))
                    {
                        visibleBoids.Add(col.transform);
                    }
                }
            }
        }
    }

    private bool RayCheck(Vector3 _dir, float _dist, LayerMask _layer) {
        //Casts a ray from transform position _dist long in _dir, checking for collisions with _layer.
        if (Physics2D.Raycast(transform.position, _dir, _dist, _layer))
        {
            return true;
        }
        else 
        {
            return false;    
        }
    }

    private bool RayCheck(Vector3 _dir, float _raydistance, LayerMask _layer, out float _hitdistance)
    {
        //Casts a ray from transform position _dist long in _dir, checking for collisions with _layer.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _dir, _raydistance, _layer);
        if (hit.collider != null)
        {
            _hitdistance = Vector2.Distance(this.transform.position, hit.point);
            return true;
        }
        else
        {
            _hitdistance = _raydistance;
            return false;
        }
    }

    private bool LollipopCheck(Vector3 _dir, float _dist, LayerMask _layer, float _checkRange = 1.0f)
    {
        //Casts a ray from transform position _dist long in _dir, checking for collisions with _layer.
        if (Physics2D.Raycast(transform.position, _dir, _dist, _layer))
        {
            return true;
        }
        else
        {
            //If the ray didnt collide, then check a range around the terminal point for any collisions with that layer.
            Vector3 terminal = transform.position + (_dir * _dist);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _checkRange, _layer);
            if (colliders.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void GetSeparation() {
        //The average of the directions away from neighbors
        Vector3 average = new Vector3();
        foreach (Transform neighbor in visibleBoids)
        {
            average += (transform.position - neighbor.position) * (1 / Mathf.Pow(Vector3.Distance(transform.position,neighbor.position),2));
        }

        if (visibleBoids.Count > 1)
        {
            average = average / visibleBoids.Count;
        }

        separation = average * separationWeight * viewRadius;
    }

    private void GetCohesion() {
        //The average of the direction towards all neighbors
        Vector3 average = new Vector3();
        foreach (Transform neighbor in visibleBoids)
        {
            average += (neighbor.position - transform.position) * Mathf.Min(1 / Vector3.Distance(transform.position, neighbor.position), cohesionWeight);
        }

        if (visibleBoids.Count > 1)
        {
            average = average / visibleBoids.Count;
        }

        //Arrive at the average direction from this boid to all the others in it's field of view
        //Return this to the boid as its cohesion direction
        cohesion = average * cohesionWeight;
    }

    private void GetAlignment() {
        //The average of the direction that the neighbors are moving in
        Vector3 average = new Vector3();
        foreach (Transform neighbor in visibleBoids)
        {
            average += neighbor.up;
        }

        if (visibleBoids.Count > 1)
        {
            average = average / visibleBoids.Count;
        }

        alignment = average * alignmentWeight;
    }
}
