using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour
{
    public string Name;
    public GameObject obj;
    public EquipmentType type;
    public Transform leftHold, rightHold;
    public EquipmentAction action;
}

public enum EquipmentType
{
    Gun,
    Tool,
    Consumable,
    Throwable
}