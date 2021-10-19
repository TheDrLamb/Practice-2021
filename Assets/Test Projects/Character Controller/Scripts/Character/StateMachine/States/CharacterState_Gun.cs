using UnityEngine;
using System.Collections;

public class CharacterState_Gun : CharacterState_Mobile
{
    CharacterCombatController combatController;
    public CharacterState_Gun(CharacterStateMachineController _stateMachine) : base(_stateMachine) 
    {
        combatController = stateMachine.GetComponent<CharacterCombatController>();
    }

    public override void Enter()
    {
        stateMachine.status = CharacterState.Gun;
        //Equip the Gun
        combatController.EquipGun();
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();
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

    public override void Exit()
    {
        combatController.UnequipGun();
    }
}
