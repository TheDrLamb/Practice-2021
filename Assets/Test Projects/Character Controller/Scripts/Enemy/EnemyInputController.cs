using UnityEngine;
using System.Collections;

public class EnemyInputController : MonoBehaviour
{
    public bool alerted;
    public Transform target;

    public InputStatus state;

    EnemyAttackController attackController;

    private void Start()
    {
        attackController = GetComponent<EnemyAttackController>();
    }

    private void Update()
    {
        VerifyTarget();
        StatusUpdate();
    }

    private void VerifyTarget()
    {
        if (target == null && alerted) alerted = false;
    }

    private void StatusUpdate() {
        if (attackController.attacking)
            state = InputStatus.Attacking;
        else if (alerted)
            state = InputStatus.Chasing;
        else
            state = InputStatus.Waiting;
    }
}

public enum InputStatus { 
    Waiting,
    Chasing,
    Attacking
}
