using UnityEngine;
using System.Collections;

public class CharacterState_ParentInteraction : CharacterState_Base
{
    public CharacterState_ParentInteraction(CharacterStateMachineController _stateMachine) : base(_stateMachine) 
    {
        inputDeadzone = stateMachine.GetComponent<CharacterPhysicsController>().inputDeadzone;
    }

    public override void Enter()
    {
        stateMachine.status = CharacterState.ParentInteraction;
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();

        if (interact > inputDeadzone)
        {
            if (!interactDown)
            {
                interactDown = true;
            }
            else 
            {
                InteractHeld();
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

    }

    protected override void PhysicsUpdate()
    {
        //Parent Interaction Data Pass Through
        ((ParentInteractable)interactController.currentInteract).UpdateInput(new Vector2(right,forward));
    }

    public override void Exit()
    {

    }

    void InteractHeld()
    {
        interactController.InteractHeld();
    }

    void InteractUp()
    {
        interactController.InteractUp();
    }
}
