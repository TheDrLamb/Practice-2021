using UnityEngine;
using System.Collections;

public class EnemyState_Waiting : EnemyBehaviorState
{
    public EnemyState_Waiting(EnemyStateMachineController _stateMachine) : base(_stateMachine) { }

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
        //Send a 0 move direction to physics controller
        stateMachine.GetComponent<EnemyPhysicsController>().SetMoveDirection(Vector3.zero);
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();
        //Process Info From Senses
    }

    protected override void VisualUpdate()
    {
        base.VisualUpdate();
        //Play Idle Animations
    }

    protected override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Send Info to Physics Controller
    }
}
