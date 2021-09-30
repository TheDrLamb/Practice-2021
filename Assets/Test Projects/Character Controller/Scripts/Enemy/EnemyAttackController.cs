using UnityEngine;
using System.Collections;

public class EnemyAttackController : MonoBehaviour
{
    public bool attacking;
    public float attackRange;

    EnemyInputController inputController;

    private void Start()
    {
        inputController = GetComponent<EnemyInputController>();
    }

    private void Update()
    {
        attacking = false;
        if (inputController.target != null)
        {
            if (Vector3.Distance(this.transform.position, inputController.target.transform.position) <= attackRange) {
                attacking = true;
            }
        }
    }
}
