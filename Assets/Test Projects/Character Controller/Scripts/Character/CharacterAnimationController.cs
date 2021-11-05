using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator anim;
    public float handSlerpTime = 0.5f;
    public Transform LeftHand, RightHand;
    Transform LeftHold, RightHold;

    int currentAnimState = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        //[NOTE] -> Old Animation Controller Code
        // If the character is moving backwards the velocity should be negative, and the animation should change.
        /*
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

            //anim.SetBool("Rotating", Mathf.Abs(angVelocity) > inputController.inputDeadzone);
        }
        */
    }

    private void FixedUpdate()
    {
        UpdateHandVisuals();
    }

    void GetDirectionFactors()
    {
        //dirSign = inputController.GetFowardSign();
        //anim.SetFloat("Strafe", inputController.GetStrafe());
    }

    private void UpdateHandVisuals()
    {
        LeftHand.position = LeftHold.position;
        LeftHand.rotation = LeftHold.rotation;
        RightHand.position = RightHold.position;
        RightHand.rotation = RightHold.rotation;
    }

    public void SetHandTransforms(Transform _LeftHold, Transform _RightHold)
    {
        LeftHold = _LeftHold;
        RightHold = _RightHold;
        //StartCoroutine(SlerpHandTransforms());
    }

    private IEnumerator SlerpHandTransforms()
    {
        Vector3 leftStart = LeftHand.position;
        Vector3 leftEnd = LeftHold.position;
        Vector3 rightStart = RightHand.position;
        Vector3 rightEnd = RightHold.position;

        float startTime = Time.time;

        for (float t = 0; t < handSlerpTime; t += Time.time - startTime)
        {
            LeftHand.position = Vector3.Lerp(leftStart, leftEnd, t);
            RightHand.position = Vector3.Lerp(rightStart, rightEnd, t);
            yield return null;
        }
        LeftHand.position = leftEnd;
        RightHand.position = rightEnd;
    }

    public void SetAnimState(int _state)
    {
        currentAnimState = _state;
        anim.SetInteger("Anim_State", currentAnimState);
    }

    public void SetAnimFloat(string _Property, float _value)
    {
        anim.SetFloat(_Property, _value);
    }

    public void SetAnimTrigger(string _Property)
    {
        anim.SetTrigger(_Property);
    }
}
