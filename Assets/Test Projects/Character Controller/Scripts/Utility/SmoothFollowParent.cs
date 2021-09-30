using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowParent : MonoBehaviour
{

    Vector3 origPosition, lastPosition;
    Quaternion origRotation, lastRotation;

    public float positionSmoothingFactor = 25f;
    public float rotationSmoothingFactor = 5f;

    void Start()
    {
        origPosition = this.transform.localPosition;
        origRotation = this.transform.localRotation;

        lastPosition = this.transform.position;
        lastRotation = this.transform.rotation;
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = this.transform.parent.TransformPoint(origPosition);
        Quaternion targetRotation = this.transform.parent.rotation * origRotation;

        targetPosition = Vector3.Lerp(lastPosition, targetPosition, Time.fixedDeltaTime * positionSmoothingFactor);
        targetRotation = Quaternion.Lerp(lastRotation, targetRotation, Time.fixedDeltaTime * rotationSmoothingFactor);

        this.transform.position = lastPosition = targetPosition;
        this.transform.rotation = lastRotation = targetRotation;
    }
}
