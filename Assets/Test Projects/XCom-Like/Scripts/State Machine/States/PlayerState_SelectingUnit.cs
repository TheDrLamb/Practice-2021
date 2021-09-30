using UnityEngine;
using System.Collections;

public class PlayerState_SelectingUnit : PlayerState
{
    public PlayerState_SelectingUnit(PlayerController _player, PlayerStateMachine _statemachine) : base(_player, _statemachine) 
    {
        //Selecting Unit Constructor
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Current State: Selecting Unit");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
