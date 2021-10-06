using UnityEngine;
using System.Collections;

public class InteractableSlider : FixedInteractable
{
    InteractableSliderMovementController movementController;

    private void Start()
    {
        movementController = GetComponent<InteractableSliderMovementController>();
        movementController.enabled = false;
    }
    public override void Interact()
    {
        held = !held;
        movementController.enabled = held;
    }

    public override void UpdateInput(Vector2 move) {
        movementController.AdjustPosition(move);
    }
}
