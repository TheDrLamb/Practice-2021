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
    public CharacterStateMachineController StateMachine;

    private void Start()
    {
        animationController = GetComponent<CharacterAnimationController>();
        StateMachine = GetComponent<CharacterStateMachineController>();
        Equip(0); // Defaulting
    }

    public void Equip(int id = 0, bool interactDown = false) 
    {
        //Swap State -> Unequips current Equipment
        StateMachine.ChangeState((CharacterState)equipmentList[id].type + 2, interactDown);

        currentEquipment = equipmentList[id];
        currentEquipment.obj.SetActive(true);
        animationController.SetHandTransforms(currentEquipment.leftHold, currentEquipment.rightHold);
    }

    public void Unequip() {
        if (currentEquipment)
        {
            currentEquipment.obj.SetActive(false);
            currentEquipment = null;
        }
    }

    public void TriggerDown() {
        Debug.Log("Bang!");
    }

    public void TriggerHeld(int id) {
        if (id == 0)
        {
            Debug.Log("BRRRRRRRrrrrr....");
        }
        else {
            Debug.Log("Taking Aim!");
        }
    }
}