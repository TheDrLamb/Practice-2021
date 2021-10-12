using UnityEngine;
using System.Collections;

public class ParentInteractable : GrabbableInteractable
{
    public InteractableSubtype subtype;
    public MovementType movementType;
    public Transform playerPosition;

    ParentInteractableMovementController movementController;

    private void Start()
    {
        if (movementType != MovementType.Fixed)
        {
            movementController = GetComponent<ParentInteractableMovementController>();
            movementController.enabled = false;
        }
    }

    public override void Interact()
    {
        held = !held;
        if (movementType != MovementType.Fixed) movementController.enabled = held;
    }

    public virtual void UpdateInput(Vector2 move)
    {
        if (movementType != MovementType.Fixed) movementController.SetPosition(move);
    }
}
public enum MovementType { 
    Fixed,
    Horizontal,
    Vertical,
    Free
}
