using UnityEngine;
using System.Collections;

public class PlayerStateMachine
{
    PlayerController playerController;
    PlayerState currentState;

    public PlayerStateMachine(PlayerController _playerController) {
        playerController = _playerController;
    }

    public void Initialize(PlayerState startingState) {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update() {
        currentState.Update();
    }
}
