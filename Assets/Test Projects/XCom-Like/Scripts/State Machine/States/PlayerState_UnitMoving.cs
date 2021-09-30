using UnityEngine;
using System.Collections;

public class PlayerState_UnitMoving : PlayerState
{
    public PlayerState_UnitMoving(PlayerController _player, PlayerStateMachine _statemachine) : base(_player, _statemachine)
    {
        //Selecting Unit Constructor
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Current State: Unit Moving");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
