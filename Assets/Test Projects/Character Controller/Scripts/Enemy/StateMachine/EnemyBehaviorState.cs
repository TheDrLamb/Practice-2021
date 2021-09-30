using UnityEngine;
using System.Collections;

public class EnemyBehaviorState
{
    protected EnemyStateMachineController stateMachine;

    public EnemyBehaviorState(EnemyStateMachineController _stateMachine)
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

