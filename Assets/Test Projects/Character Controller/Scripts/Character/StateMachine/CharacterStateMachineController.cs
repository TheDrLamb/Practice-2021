using UnityEngine;
using System.Collections;

public class CharacterStateMachineController : MonoBehaviour
{
    CharacterState_Base currentState;

    //All States
    CharacterState_ChildInteraction childInteract;
    CharacterState_ParentInteraction parentInteraction;
    CharacterState_Gun gunState;

    public CharacterState status;

    private void Start()
    {
        parentInteraction = new CharacterState_ParentInteraction(this);

        childInteract = new CharacterState_ChildInteraction(this);
        gunState = new CharacterState_Gun(this);

        Initialize(gunState);
    }

    private void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void Initialize(CharacterState_Base startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(CharacterState_Base newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}

public enum CharacterState { 
    ParentInteraction,
    ChildInteraction,
    Gun
}
