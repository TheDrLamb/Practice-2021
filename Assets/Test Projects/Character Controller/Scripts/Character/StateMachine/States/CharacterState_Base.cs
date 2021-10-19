using UnityEngine;
using System.Collections;

public class CharacterState_Base
{
    public float forwardRaw, forward;
    public float rightRaw, right;
    public float interact;
    public float inputDeadzone;

    public bool interactDown = false;

    public CharacterInteractController interactController;

    protected CharacterStateMachineController stateMachine;

    public CharacterState_Base(CharacterStateMachineController _stateMachine)
    {
        stateMachine = _stateMachine;
        interactController = stateMachine.GetComponent<CharacterInteractController>();
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
        forwardRaw = Input.GetAxis("Vertical");
        rightRaw = Input.GetAxis("Horizontal");
        interact = Input.GetAxis("Interact");
    }

    protected virtual void LogicUpdate()
    {
        forward = right = 0;

        if (Mathf.Abs(forwardRaw) > inputDeadzone)
        {
            forward = Mathf.Clamp(forwardRaw, -1.0f, 1.0f);
        }

        if (Mathf.Abs(rightRaw) > inputDeadzone)
        {
            right = Mathf.Clamp(rightRaw, -1.0f, 1.0f);
        }
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
