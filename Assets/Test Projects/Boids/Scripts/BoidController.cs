using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    [Range(0, 360)]
    public float viewAngle = 135;
    public float viewRadius = 5;
    public float velocity;
    public float minVelocity;
    public float rotationSpeed;

    [Range(0.1f, 3.0f)]
    public float alignmentWeight;

    [Range(0.1f, 3.0f)]
    public float cohesionWeight;

    [Range(0.1f, 3.0f)]
    public float separationWeight;

    [Range(0.1f, 3.0f)]
    public float avoidanceWeight;

    public float increment;
    public int avoidanceIncrements = 36;

    public void Start()
    {
        increment = (viewAngle * (3.1415f / 360f)) / avoidanceIncrements;
    }
}
