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
    }

    public void Equip(int id = 0, bool interactDown = false) 
    {
        //[NOTE] -> Change the way equipment works to go off of the array index.
        currentEquipment = equipmentList[id];
        currentEquipment.obj.SetActive(true);
        animationController.SetHandTransforms(currentEquipment.leftHold, currentEquipment.rightHold);
        StateMachine.ChangeState((CharacterState)currentEquipment.type + 2, interactDown);
    }

    public void Unequip() {
        currentEquipment.obj.SetActive(false);
        currentEquipment = null;
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
public enum EquipmentType { 
    Gun,
    Tool,
    Consumable,
    Throwable
}

[Serializable]
public class Equipment {
    public GameObject obj;
    public EquipmentType type;
    public Transform leftHold, rightHold;
}
