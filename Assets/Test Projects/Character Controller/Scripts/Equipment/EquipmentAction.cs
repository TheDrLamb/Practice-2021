using System.Collections;
using UnityEngine;

public class EquipmentAction : MonoBehaviour
{
    //Overacthing class for all actions that equipments have.

    //Animation Variables
    //  Set the state or animation that should play for the given equipment

    //Logic
    //Physics

    public virtual void Down()
    {
        Debug.Log("Trigger Down");
    }

    public virtual void Hold()
    {
        Debug.Log("Trigger Held");
    }
}