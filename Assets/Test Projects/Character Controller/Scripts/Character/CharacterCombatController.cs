using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatController : MonoBehaviour
{
    public GameObject Gun;
    public Transform GunHoldLeft, GunHoldRight;

    CharacterAnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<CharacterAnimationController>();
    }

    public void EquipGun() {
        Gun.SetActive(true);
        animationController.SetHandTransforms(GunHoldLeft, GunHoldRight);
    }

    public void UnequipGun() {
        Gun.SetActive(false);
    }
}
enum CurrentWeapon { 
    Gun,
    Consumable,
    Tool,
    Equipment
}
