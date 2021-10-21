using UnityEngine;
using System.Collections;

public class CharacterState_Consumable : CharacterState_Combat
{
    public CharacterState_Consumable(CharacterStateMachineController _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        stateMachine.status = CharacterState.Consumable;
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
