using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridMapController gridMap;
    public UnitController selectedUnit;
    public RangeController rangeController;
    public PathController pathController;
    
    //State Machine
    PlayerStateMachine stateMachine;
    //States
    PlayerState_SelectingUnit State_SelectingUnit;
    PlayerState_UnitAction State_UnitAction;
    PlayerState_UnitMoving State_UnitMoving;


    private void Start()
    {
        stateMachine = new PlayerStateMachine(this);
        State_SelectingUnit = new PlayerState_SelectingUnit(this, stateMachine);
        State_UnitAction = new PlayerState_UnitAction(this, stateMachine);
        State_UnitMoving = new PlayerState_UnitMoving(this, stateMachine);

        stateMachine.Initialize(State_SelectingUnit);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void SetUnit(UnitController _newUnit)
    {
        selectedUnit = _newUnit;
        selectedUnit.UnitMoving += UnitMoving;
        selectedUnit.UnitStopped += UnitStopped;
        stateMachine.ChangeState(State_UnitAction);
    }

    void UnitMoving(object sender, EventArgs e)
    {
        Console.WriteLine("Unit Moving");
        stateMachine.ChangeState(State_UnitMoving);
    }

    void UnitStopped(object sender, EventArgs e)
    {
        Console.WriteLine("Unit Stopped");
        stateMachine.ChangeState(State_UnitAction);
    }
}

