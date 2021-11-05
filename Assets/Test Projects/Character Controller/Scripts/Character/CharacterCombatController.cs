using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class CharacterCombatController : MonoBehaviour
{
    public Equipment[] equipmentList;
    public Equipment currentEquipment;

    CharacterAnimationController animationController;
    CharacterStateMachineController StateMachine;

    private void Start()
    {
        animationController = GetComponent<CharacterAnimationController>();
        StateMachine = GetComponent<CharacterStateMachineController>();
        Equip(0); // Defaulting
    }

    public void Equip(int id = 0, bool interactDown = false) 
    {
        Unequip();
        currentEquipment = equipmentList[id];
        currentEquipment.obj.SetActive(true);

        animationController.SetHandTransforms(currentEquipment.leftHold, currentEquipment.rightHold);
        animationController.SetAnimState((int)currentEquipment.type);
    }

    public void Unequip() {
        if (currentEquipment)
        {
            currentEquipment.obj.SetActive(false);
            currentEquipment = null;
        }
    }

    public void TriggerDown() {
        currentEquipment.action.Down();
    }

    public void TriggerHeld() {
        currentEquipment.action.Hold();
    }

    public void TriggerUp() {
        currentEquipment.action.Up();
    }
}