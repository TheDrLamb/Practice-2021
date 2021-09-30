using UnityEngine;
using System.Collections;

public class FixedInteractable : GrabbableInteractable
{
    public InteractableSubtype subtype;

    public bool horizontalLock;
    public bool verticalLock;

    public Transform playerPosition;
}
