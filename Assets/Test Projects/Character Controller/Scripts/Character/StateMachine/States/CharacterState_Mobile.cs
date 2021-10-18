using UnityEngine;
using System.Collections;

public class CharacterState_Mobile : CharacterState_Base
{
    float forwardRaw, forward;
    float rightRaw, right;
    float inputDeadzone;
    LayerMask mapLayer;
    bool moving;

    CharacterPhysicsController physicsController;
    CharacterRunParticleController particleController;

    public CharacterState_Mobile(CharacterStateMachineController _stateMachine) : base(_stateMachine) {
        physicsController = stateMachine.GetComponent<CharacterPhysicsController>();
        particleController = stateMachine.GetComponent<CharacterRunParticleController>();
        inputDeadzone = physicsController.inputDeadzone;
        mapLayer = physicsController.mapLayer;
    }

    protected override void InputUpdate()
    {
        forwardRaw = Input.GetAxis("Vertical");
        rightRaw = Input.GetAxis("Horizontal");
    }

    protected override void LogicUpdate()
    {
        forward = right = 0;

        if (Mathf.Abs(forwardRaw) > inputDeadzone)
        {
            forward = Mathf.Clamp(forwardRaw, -1.0f, 1.0f);
        }

        if (Mathf.Abs(rightRaw) > inputDeadzone)
        {
            right = Mathf.Clamp(rightRaw, -1.0f, 1.0f);
        }

        moving = Mathf.Abs(forwardRaw) > inputDeadzone;
    }

    protected override void VisualUpdate()
    {
        particleController.SetIsMoving(moving);
    }

    protected override void PhysicsUpdate()
    {
        PassMoveDir();
        PassLookDirection();
    }

    void PassMoveDir()
    {
        if (moving)
        {
            //Pass Move Direction to the physics controller
            Vector3 dir = Vector3.zero;
            dir = stateMachine.transform.forward * forward + stateMachine.transform.right * right;
            physicsController.SetMovementDirection(dir);
        }
        else 
        {
            physicsController.ApplyBreakingForce();
        }
    }

    void PassLookDirection()
    {
        //Have the player look in the direction of the mouse cursor on the map
        Quaternion lookDir;
        
        //Default the Look Direction to forward
        Vector3 forwardPlanarDir = stateMachine.transform.forward;
        forwardPlanarDir.y = 0;
        forwardPlanarDir.Normalize();
        lookDir = Quaternion.LookRotation(forwardPlanarDir, Vector3.up);

        //Get Mouse based direction if possible
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mapLayer))
        {
            //Get X-Z Planar Direction from the player to the hit point
            Vector3 dir = new Vector3((hit.point.x - stateMachine.transform.position.x), 0, (hit.point.z - stateMachine.transform.position.z));
            //Set player target rotation to look in that direction
            lookDir = Quaternion.LookRotation(dir, Vector3.up);
        }

        physicsController.SetLookDirection(lookDir);
    }

}
