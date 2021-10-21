using UnityEngine;
using System.Collections;

public class CharacterStateMachineController : MonoBehaviour
{
    CharacterState_Base currentState;

    //Interaction States
    CharacterState_ChildInteraction childInteraction;
    CharacterState_ParentInteraction parentInteraction;

    //Combat States
    CharacterState_Gun gunState;
    CharacterState_Tool toolState;
    CharacterState_Consumable consumableState;

    public CharacterState status;

    private void Start()
    {
        parentInteraction = new CharacterState_ParentInteraction(this);
        childInteraction = new CharacterState_ChildInteraction(this);
        gunState = new CharacterState_Gun(this);
        toolState = new CharacterState_Tool(this);
        consumableState = new CharacterState_Consumable(this);

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

    public void ChangeState(CharacterState_Base newState, bool interactDown = false)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        currentState.interactDown = interactDown;
    }

    public void ChangeState(CharacterState newState, bool interactDown = false) {
        switch (newState)
        {
            case CharacterState.ChildInteraction:
                ChangeState(childInteraction, interactDown);
                break;
            case CharacterState.ParentInteraction:
                ChangeState(parentInteraction, interactDown);
                break;
            case CharacterState.Gun:
                ChangeState(gunState, interactDown);
                break;
            case CharacterState.Tool:
                ChangeState(toolState, interactDown);
                break;
            case CharacterState.Consumable:
                ChangeState(consumableState, interactDown);
                break;
        }
    }
}

public enum CharacterState { 
    ParentInteraction,
    ChildInteraction,
    Gun,
    Tool,
    Consumable,
    Throwable
}
