using UnityEngine;
using System.Collections;

public class EnemyState_Chasing : EnemyBehaviorState
{
    public EnemyState_Chasing(EnemyStateMachineController _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();
        //Get Move Dir towards the player
        Vector3 move = stateMachine.GetComponent<EnemyNavigationController>().GetMoveDirection();
        //Pass Move Dir to Physics Controller
        stateMachine.GetComponent<EnemyPhysicsController>().SetMoveDirection(move);
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
