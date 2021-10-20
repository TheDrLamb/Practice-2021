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

    private void Start()
    {
        animationController = GetComponent<CharacterAnimationController>();
    }

    public void Equip(EquipmentType type) 
    {
        currentEquipment = equipmentList.Where(e => e.type == type).FirstOrDefault();
        currentEquipment.obj.SetActive(true);
        animationController.SetHandTransforms(currentEquipment.leftHold, currentEquipment.rightHold);
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
    Consumable
}

[Serializable]
public class Equipment {
    public GameObject obj;
    public EquipmentType type;
    public Transform leftHold, rightHold;
}
