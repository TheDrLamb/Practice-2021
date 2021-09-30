using UnityEngine;
using System.Collections;

public class EnemyState_Attacking : EnemyBehaviorState
{

    public EnemyState_Attacking(EnemyStateMachineController _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        //Set Up Cooldown
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();
        //Send a 0 move direction to physics controller
        stateMachine.GetComponent<EnemyPhysicsController>().SetMoveDirection(Vector3.zero);
        //Get location of player
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    protected override void VisualUpdate()
    {
        base.VisualUpdate();
        //Play Attack Animation
    }

    protected override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Fire Projectile if applicable
    }
}
