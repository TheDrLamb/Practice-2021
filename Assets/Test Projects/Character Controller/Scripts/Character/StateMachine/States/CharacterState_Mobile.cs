using UnityEngine;
using System.Collections;

public class CharacterState_Mobile : CharacterState_Base
{
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
        base.InputUpdate();
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();

        moving = Mathf.Abs(forwardRaw) > inputDeadzone;

        if (interact > inputDeadzone)
        {
            if (!interactDown)
            {
                interactDown = true;
                InteractDown();
            }
        }
        else 
        {
            interactDown = false;
            InteractUp();
        }
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

    void InteractDown() 
    {
        interactController.InteractDown();
    }

    void InteractUp()
    {
        interactController.InteractUp();
    }
}
