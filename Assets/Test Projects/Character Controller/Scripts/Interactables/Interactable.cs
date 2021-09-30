using UnityEngine;
using System.Collections;
using System;

public class Interactable : MonoBehaviour
{
    public InteractableType type;

    public virtual void Interact() {
        throw new NotImplementedException();
    }
}

public enum InteractableType
{
    Holdable,
    Fixed
}
public enum InteractableSubtype
{
    SingleAxis,
    DoubleAxis,
    UIInterface
}
