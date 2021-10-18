using UnityEngine;
using System.Collections;

public class EnemyStateMachineController : MonoBehaviour
{
    //Input Controller
    EnemyInputController inputController;

    //Current State
    EnemyBehaviorState currentState;

    //States
    EnemyState_Waiting waiting;
    EnemyState_Chasing chasing;
    EnemyState_Attacking attacking;

    public EnemyState state;

    void Start()
    {
        inputController = GetComponent<EnemyInputController>();

        waiting = new EnemyState_Waiting(this);
        chasing = new EnemyState_Chasing(this);
        attacking = new EnemyState_Attacking(this);

        Initialize(waiting);
    }

    void Update()
    {
        currentState.Update();
        switch (inputController.state) {
            case InputStatus.Waiting:
                ChangeState(waiting);
                break;
            case InputStatus.Chasing:
                ChangeState(chasing);
                break;
            case InputStatus.Attacking:
                ChangeState(attacking);
                break;
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void Initialize(EnemyBehaviorState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(EnemyBehaviorState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
public enum EnemyState { 
    Waiting,
    Chasing,
    Attacking
}
