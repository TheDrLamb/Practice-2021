using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharacterPhysicsController : MonoBehaviour
{
    public float acceleration = 100;
    public float maxAcceleration = 200;
    public float maxSpeed = 10;
    public float rotationSpeed = 5;
    public float brakingForce = 5;

    public float rideHeight = 1.25f;
    public float rideSpringStrength = 150f;
    public float rideSpringDamping = 5f;
    public float rideDownFactor = 2f;

    public float uprightSpringStrength = 100f;
    public float uprightSpringDamping = 5f;

    public LayerMask mapLayer;
    public float inputDeadzone = 0.1f;

    Quaternion lastPlayerTargetRotation;
    Quaternion playerTargetRotation;
    Rigidbody rigid;
    Vector3 groundVelocity;
    Vector3 old_GoalVelocity;
    Vector3 old_Move;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        old_Move = Vector3.zero;
        old_GoalVelocity = Vector3.zero;
        playerTargetRotation = lastPlayerTargetRotation = rigid.transform.rotation;
    }
    private void FixedUpdate()
    {
        UpdateRideHeight();
        UpdateUprightForce();
    }

    public void SetMovementDirection(Vector3 move)
    {
        if (move.magnitude > 1f)
            move.Normalize();

        old_Move = move;

        Vector3 unitVelocity = old_GoalVelocity.normalized;

        float velDot = Vector3.Dot(old_Move, unitVelocity);

        float accel = acceleration * AccelerationFromDot(velDot);

        Vector3 goalVelocity = move * maxSpeed;

        old_GoalVelocity = Vector3.MoveTowards(old_GoalVelocity, goalVelocity + groundVelocity, accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (old_GoalVelocity - rigid.velocity) / Time.fixedDeltaTime;

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAcceleration * AccelerationFromDot(velDot));
        neededAccel.y = 0;

        rigid.AddForce((neededAccel * rigid.mass) + Physics.gravity);
        //playerTargetRotation = Quaternion.LookRotation(old_Move, Vector3.up); // Have player look in direction of movement
    }

    public void ApplyBreakingForce() {
        //Braking Force
        rigid.velocity = new Vector3(rigid.velocity.x * (1 / brakingForce), rigid.velocity.y, rigid.velocity.z * (1 / brakingForce));
    }

    public void SetLookDirection(Quaternion targetRotation)
    {
        playerTargetRotation = Quaternion.Slerp(lastPlayerTargetRotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        lastPlayerTargetRotation = playerTargetRotation;
    }

    void UpdateRideHeight() {
        //Cast a Ray downward twice the length of ride height and apply a dampened spring force to the character to maintain them at ride height above the ground
        RaycastHit hit;
        if (Physics.Raycast(rigid.transform.position, -Vector3.up, out hit, rideHeight * rideDownFactor, mapLayer))
        {
            Vector3 playerVelocity = rigid.velocity;
            Vector3 playerDownDir = -Vector3.up;
            Debug.DrawRay(rigid.transform.position, playerDownDir * 2);

            groundVelocity = Vector3.zero;
            Rigidbody hitRigidbody = hit.rigidbody;
            if (hitRigidbody != null)
            {
                groundVelocity = hitRigidbody.velocity;
            }

            float playerDirVelocity = Vector3.Dot(playerDownDir, playerVelocity);
            float hitDirVelocity = Vector3.Dot(playerDownDir, groundVelocity);

            float relativeVelocity = playerDirVelocity - hitDirVelocity;

            float d = hit.distance - rideHeight;

            //Apply squash the player if d exceeds some limit?

            float springForce = (d * rideSpringStrength) - (relativeVelocity * rideSpringDamping);

            rigid.AddForce((playerDownDir * springForce * rigid.mass));

            if (hitRigidbody != null)
            {
                hitRigidbody.AddForceAtPosition(playerDownDir * -springForce * rigid.mass, hit.point);
            }
        }
        else 
        {
            //Character is ungrounded, apply immediate force of gravity.
            rigid.AddForce(Physics.gravity * rigid.mass * 6f);
        }
    }

    void UpdateUprightForce() {
        Quaternion playerCurrent = rigid.transform.rotation;
        Quaternion playerGoal = Utility.ShortestRotation(playerTargetRotation, playerCurrent);

        Vector3 rotAxis;
        float rotDegrees;

        playerGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        rigid.AddTorque((rotAxis * (rotRadians * uprightSpringStrength)) - (rigid.angularVelocity * uprightSpringDamping));
    }

    float AccelerationFromDot(float _dotProduct) {
        return Mathf.Clamp(1 - _dotProduct,1,2);
    }

    public Vector3 GetMoveDir() {
        return old_Move;
    }
}
