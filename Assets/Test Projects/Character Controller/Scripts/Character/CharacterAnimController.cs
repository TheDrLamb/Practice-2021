using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    public int GunAnimState = 1;
    public float speed = 3;
    public bool smoothing = true;
    [Range(0.1f,2.0f)]
    public float UpFrameWeight = 3.0f;

    float lastVelocity = 0;
    float lastAngVelocity = 0;
    float r = 0;

    float physicsSpeed;
    float physicsRotSpeed;

    Quaternion strafeDir_LastTarget;

    CharacterInputController inputController;
    int dirSign = 1;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();

        physicsSpeed = GetComponent<CharacterPhysicsController>().maxSpeed;
        physicsRotSpeed = GetComponent<CharacterPhysicsController>().rotationSpeed;

        inputController = GetComponent<CharacterInputController>();
    }

    void Update()
    {
        //[NOTE] -> Determine the direction of the velocity with respect to the forward direction.
        //          If the character is moving backwards the velocity should be negative, and the animation should change.
        Vector3 velocityVector = rigid.velocity;
        float velocity;
        velocity = Mathf.Sqrt((velocityVector.x * velocityVector.x) + (velocityVector.z * velocityVector.z)); //X-Z Planar velocity of the rigidbody

        Vector3 angVelocityVector = rigid.angularVelocity;
        float angVelocity = angVelocityVector.y;

        if (smoothing)
        {
            velocity = Mathf.Lerp(lastVelocity, velocity, Mathf.SmoothStep(0.0f, 1.0f, Time.unscaledTime));
            angVelocity = Mathf.Lerp(lastAngVelocity, angVelocity, Mathf.SmoothStep(0.0f, 1.0f, Time.unscaledTime));
        }

        lastVelocity = velocity;
        lastAngVelocity = angVelocity;

        velocity = Mathf.Max(velocity, 0); //Velocity no less than 0

        anim.SetInteger("Gun", GunAnimState);

        
        if (inputController.IsMoving())
        {
            anim.SetBool("Running", true);

            GetDirectionFactors();

            r += velocity * speed / 100;
            if (r > 6.28318) r = 0;

            float blendFactor = Mathf.Clamp(velocity / physicsSpeed, -1, 1);

            anim.SetFloat("Sin", Mathf.Sin(r) * blendFactor * dirSign);
            anim.SetFloat("Cos", Mathf.Cos(r) * UpFrameWeight * blendFactor);
        }
        else 
        {
            anim.SetBool("Running", false);

            //When character not moving but is rotating
            r += angVelocity * speed / 30;
            if (r > 6.28318) r = 0;

            float blendFactor = Mathf.Clamp(angVelocity, -1,1);

            anim.SetFloat("Sin", Mathf.Sin(r) * blendFactor);
            anim.SetFloat("Cos", Mathf.Cos(r) * blendFactor);

            anim.SetBool("Rotating", Mathf.Abs(angVelocity) > inputController.inputDeadzone);
        }
    }

    void GetDirectionFactors()
    {
        dirSign = inputController.GetFowardSign();
        anim.SetFloat("Strafe", inputController.GetStrafe());
    }

}
