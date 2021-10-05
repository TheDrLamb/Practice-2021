using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable
{
    public GameObject targetGameObject;
    public string targetFunction;

    public override void Interact()
    {
        targetGameObject.SendMessage(targetFunction);
    }
}
