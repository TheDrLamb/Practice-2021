using UnityEngine;
using System.Collections;

public class GunInteractable : ChildInteractable
{
    //[NOTE] -> Will be obsolete when guns are added to the inventory system.
    //Gun Statistics
    public int damageAmount = 1;
    public FireModes fireMode;
    public float rateOfFire = 1;
    public ParticleSystem muzzleFlash;
}
