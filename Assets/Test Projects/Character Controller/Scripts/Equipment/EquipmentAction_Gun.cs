using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Test_Projects.Character_Controller.Scripts.Equipment
{
    public class EquipmentAction_Gun : EquipmentAction
    {
        public float rateOfFire = 1.0f;
        public Transform firePoint;
        public Rigidbody projectile;
        public float fireSpeed = 3000;
        Task fireTask;

        public override void Down()
        {
            fireTask = Fire();
        }

        public override void Hold()
        {
            if(fireTask.IsCompleted) fireTask = Fire();
        }

        private async Task Fire() {
            Rigidbody newRigid = Instantiate(projectile,firePoint.position,firePoint.rotation);
            newRigid.AddForce(fireSpeed * firePoint.forward);
            await Task.Delay((int) (rateOfFire * 1000));
        }
    }
}