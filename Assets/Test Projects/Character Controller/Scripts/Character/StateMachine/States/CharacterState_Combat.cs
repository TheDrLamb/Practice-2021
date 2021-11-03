using UnityEngine;
using System.Collections;

public class CharacterState_Combat : CharacterState_Mobile
{
    public CharacterCombatController combatController;

    float trigger1;
    float trigger2;
    int equipment;

    bool trigger1_Down = false;
    bool trigger2_Down = false;

    public CharacterState_Combat(CharacterStateMachineController _stateMachine) : base(_stateMachine)
    {
        combatController = stateMachine.GetComponent<CharacterCombatController>();
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();

        trigger1 = Input.GetAxis("Fire1");
        trigger2 = Input.GetAxis("Fire2");

        equipment = -1;

        if(Input.GetAxis("Equipment1") > 0)
        {
            equipment = 0;
        }
        if (Input.GetAxis("Equipment2") > 0)
        {
            equipment = 1;
        }
        if (Input.GetAxis("Equipment3") > 0)
        {
            equipment = 2;
        }
        if (Input.GetAxis("Equipment4") > 0)
        {
            equipment = 3;
        }
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();
        //Process Inputs

        //Check for Trigger 1 Down, Held
        if (trigger1 > inputDeadzone)
        {
            if (!trigger1_Down)
            {
                trigger1_Down = true;
                TriggerDown();
            }
            else
            {
                TriggerHeld(0);
            }
        }
        else
        {
            trigger1_Down = false;
        }

        //Check for Trigger 2 Held
        if (trigger2 > inputDeadzone)
        {
            if (!trigger2_Down)
            {
                trigger2_Down = true;
            }
            else
            {
                TriggerHeld(1);
            }
        }
        else
        {
            trigger2_Down = false;
        }

        //Check for Equipment Swap Buttons
        if(equipment > -1) combatController.Equip(equipment);
    }

    protected override void VisualUpdate()
    {
        base.VisualUpdate();
    }

    protected override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        combatController.Unequip();
    }

    void TriggerDown() {
        combatController.TriggerDown();
    }

    void TriggerHeld(int id) {
        combatController.TriggerHeld(id);
    }
}