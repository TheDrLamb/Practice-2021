using UnityEngine;
using System.Collections;

public class CharacterState_Throwable : CharacterState_Combat
{
    public CharacterState_Throwable(CharacterStateMachineController _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        stateMachine.status = CharacterState.Throwable;
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    protected override void VisualUpdate()
    {
        base.VisualUpdate();
    }

    protected override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
