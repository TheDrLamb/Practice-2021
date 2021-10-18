using UnityEngine;
using System.Collections;

public class CharacterState_ParentInteraction : CharacterState_Base
{
    public CharacterState_ParentInteraction(CharacterStateMachineController _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        stateMachine.status = CharacterState.ParentInteraction;
    }

    protected override void InputUpdate()
    {

    }

    protected override void LogicUpdate()
    {

    }

    protected override void VisualUpdate()
    {

    }

    protected override void PhysicsUpdate()
    {

    }

    public override void Exit()
    {

    }
}
