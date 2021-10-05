using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    CharacterPhysicsController physicsController;
    CharacterAnimController animationController;
    CharacterInteractController interactController;
    public KeyCode InteractKey = KeyCode.E;

    public float inputDeadzone = 0.1f;
    public LayerMask mapLayer;

    float forwardRaw, forward;
    float rightRaw, right;
    bool triggerDown, triggerUp;

    public MovementLockStatus movementLock = MovementLockStatus.ForwardAndStrafe;
    public bool rotationLock = false;


    private void Start()
    {
        physicsController = GetComponent<CharacterPhysicsController>();
        animationController = GetComponent<CharacterAnimController>();
        interactController = GetComponent<CharacterInteractController>();
    }
    private void FixedUpdate()
    {
        forwardRaw = Input.GetAxis("Vertical");
        rightRaw = Input.GetAxis("Horizontal");

        forward = right = 0;

        if (Mathf.Abs(forwardRaw) > inputDeadzone)
        {
            forward = Mathf.Clamp(forwardRaw, -1.0f, 1.0f);
        }

        if (Mathf.Abs(rightRaw) > inputDeadzone)
        {
            right = Mathf.Clamp(rightRaw, -1.0f, 1.0f);
        }

        triggerDown = triggerUp = false;
        if (Input.GetMouseButtonDown(0)) {
            triggerDown = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            triggerUp = true;
        }

        if (Input.GetKeyDown(InteractKey))
        {
            interactController.InteractDown();
        }

        if (Input.GetKey(InteractKey))
        {
            interactController.InteractHeld();
        }

        if (Input.GetKeyUp(InteractKey))
        {
            interactController.InteractUp();
        }
    }

    public Vector3 GetMoveDir() {
        Vector3 dir = Vector3.zero;
        switch (movementLock)
        {
            case MovementLockStatus.ForwardAndStrafe:
                dir = this.transform.forward * forward + this.transform.right * right;
                break;
            case MovementLockStatus.RightOnly:
                dir = this.transform.right * right;
                break;
            case MovementLockStatus.ForwardOnly:
                dir = this.transform.forward * forward;
                break;
        }
        return dir.normalized;
    }

    public Quaternion GetLookDirection() {
        //Have the player look in the direction of the mouse cursor on the map
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mapLayer) && !rotationLock)
        {
            //Get X-Z Planar Direction from the player to the hit point
            Vector3 dir = new Vector3((hit.point.x - this.transform.position.x), 0, (hit.point.z - this.transform.position.z));
            //Set player target rotation to look in that direction
            return Quaternion.LookRotation(dir, Vector3.up);
        }
        Vector3 forwardPlanarDir = this.transform.forward;
        forwardPlanarDir.y = 0;
        forwardPlanarDir.Normalize();
        return Quaternion.LookRotation(forwardPlanarDir, Vector3.up);
    }

    public bool IsMoving() {
        if (movementLock == MovementLockStatus.RightOnly)
        {
            return (Mathf.Abs(rightRaw) >= inputDeadzone);
        }
        else 
        {
            return (Mathf.Abs(forwardRaw) >= inputDeadzone);
        }
    }

    public int GetFowardSign() {
        if (forwardRaw >= 0) {
            return 1;
        }
        return -1;
    }

    public float GetStrafe() {
        if (forwardRaw >= 0)
        {
            return right / (Mathf.Abs(right) + Mathf.Abs(forward));
        }
        else 
        {
            return -(right / (Mathf.Abs(right) + Mathf.Abs(forward)));
        }
    }

    public bool GetTriggerDown() {
        return triggerDown;
    }

    public bool GetTriggerUp()
    {
        return triggerUp;
    }
}
public enum MovementLockStatus { 
    ForwardOnly,
    RightOnly,
    ForwardAndStrafe
}
