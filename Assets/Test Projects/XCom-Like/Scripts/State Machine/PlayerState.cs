using UnityEngine;
using System.Collections;

public class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerController _player, PlayerStateMachine _statemachine)
    {
        player = _player;
        stateMachine = _statemachine;
    }
    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

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
