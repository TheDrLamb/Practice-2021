using UnityEngine;
using System.Collections;

public class FixedInteractable : GrabbableInteractable
{
    public InteractableSubtype subtype;
    public MovementType movementType;
    public Transform playerPosition;

    public virtual void UpdateInput(Vector2 move)
    {
    }
}
public enum MovementType { 
    Fixed,
    Horizontal,
    Vertical,
    Free
}
