using System.Collections;
using UnityEngine;

namespace Assets.Test_Projects.Character_Controller.Scripts.Equipment
{
    public class EquipmentAction_Tool : EquipmentAction
    {
        public override void Down()
        {
            Debug.Log("Melee Attack");
        }

        public override void Hold()
        {
            Debug.Log("Melee Attack: Just Keep Swinging");
            //Charge up Melee Swing?
        }
    }
}