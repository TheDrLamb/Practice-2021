using UnityEngine;
using System.Collections;

public class CharacterState_Base
{
    protected CharacterStateMachineController stateMachine;

    public CharacterState_Base(CharacterStateMachineController _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        InputUpdate();
        LogicUpdate();
        VisualUpdate();
    }

    public virtual void FixedUpdate()
    {
        PhysicsUpdate();
    }

    protected virtual void InputUpdate()
    {

    }
    protected virtual void LogicUpdate()
    {

    }

    protected virtual void VisualUpdate()
    {

    }
    protected virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
