using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInteractable : GrabbableInteractable
{
    public Vector3 offset;

    Rigidbody rigid;
    protected Collider col;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    protected override void Grab() {
        base.Grab();
        col.enabled = false;
        rigid.isKinematic = true;
    }

    protected override void Drop() {
        base.Drop();
        col.enabled = true;
        rigid.isKinematic = false;
    }
}
