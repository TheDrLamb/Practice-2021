using System.Collections;
using UnityEngine;

namespace Assets.Test_Projects.Character_Controller.Scripts.Equipment
{
    public class EquipmentAction_Tool : EquipmentAction
    {
        [Range(0.5f, 5.0f)]
        public float chargeSpeed = 2;
        float charge = 0.0f;
        public CharacterAnimationController animationController;

        private void Start()
        {
            animationController = transform.root.GetComponent<CharacterAnimationController>();
        }

        public override void Hold()
        {
            charge += chargeSpeed * Time.deltaTime;
            animationController.SetAnimFloat("Melee_Charge", charge);
        }

        public override void Up()
        {
            charge = 0;

            animationController.SetAnimTrigger("Melee_Swing");
            animationController.SetAnimFloat("Melee_Charge", charge);
        }
    }
}