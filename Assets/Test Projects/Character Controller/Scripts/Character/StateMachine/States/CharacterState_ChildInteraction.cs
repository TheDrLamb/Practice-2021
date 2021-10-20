using UnityEngine;
using System.Collections;

public class CharacterState_ChildInteraction : CharacterState_Mobile
{
    public CharacterState_ChildInteraction(CharacterStateMachineController _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        stateMachine.status = CharacterState.ChildInteraction;
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
            if (interactDown)
            {
                InteractHeld();
            }
        }
    }

    protected override void VisualUpdate()
    {
        base.VisualUpdate();
    }

    protected override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void InteractHeld()
    {
        interactController.InteractHeld();
    }
}
