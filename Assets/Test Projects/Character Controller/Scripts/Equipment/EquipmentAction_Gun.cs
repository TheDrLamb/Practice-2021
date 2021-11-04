using System.Collections;
using UnityEngine;

namespace Assets.Test_Projects.Character_Controller.Scripts.Equipment
{
    public class EquipmentAction_Gun : EquipmentAction
    {
        public float rateOfFire = 1.0f;
        float roundsPerSecond;
        float timer = 0.0f;

        private void Start()
        {
            roundsPerSecond = 1 / rateOfFire;
        }

        public override void Down()
        {
            Fire();
        }

        public override void Hold()
        {
            timer += Time.deltaTime;
            if (timer >= roundsPerSecond) {
                Fire();
            }
        }

        private void Fire() {
            timer = 0.0f;
            Debug.Log("Bang!");
        }
    }
}