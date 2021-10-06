using UnityEngine;
using System.Collections;

public class GrabbableInteractable : Interactable
{
    public bool held;
    public Transform Left, Right;

    public override void Interact() {
        if (!held) Grab();
        else Drop();
    }

    protected virtual void Grab()
    {
        held = true;
    }

    protected virtual void Drop()
    {
        held = false;
    }
}
