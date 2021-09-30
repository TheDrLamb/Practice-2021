using UnityEngine;

public class SimpleSmoothCamera : MonoBehaviour
{
    public Transform target;

    [Range(0.1f,3)]
    public float smoothSpeed = 0.125f;
    public Vector3 cameraOffset;

    private void FixedUpdate()
    {
        Vector3 goalPosition = target.position + cameraOffset;
        Vector3 interpolatedPostion = Vector3.Lerp(transform.position, goalPosition, Time.deltaTime * smoothSpeed * 10.0f);
        transform.position = interpolatedPostion;

        transform.LookAt(target);
    }
}
