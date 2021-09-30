using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float Smoothing = 0.5f;
    [Range(0.25f,1.0f)]
    public float ZoomFactor = 0.5f;
    [Range(0.1f, 2.0f)]
    public float ZoomSpeed = 0.75f;
    [Range(15.0f, 60.0f)]
    float baseZoomTilt = 60.0f;
    float baseZoom = 25f;
    [Range(0.0f, 360.0f)]
    public float Rotation = 0.0f;
    [Range(1.0f, 5.0f)]
    public float RotationSpeed = 2.5f;
    //[NOTE] -> Add a setting that will make the cameras rotation snap to angles of 90 degrees

    public Transform cameraTransform;

    void Start()
    {
        baseZoom = Camera.main.orthographicSize / ZoomFactor;
        baseZoomTilt = cameraTransform.rotation.eulerAngles.x / ZoomFactor;
    }
    void Update()
    {
        if (Input.GetAxis("Zoom") != 0) {
            float ZoomAmount = Input.GetAxis("Zoom") * ZoomSpeed;

            ZoomFactor += ZoomAmount;
            ZoomFactor = Mathf.Clamp(ZoomFactor, 0.25f, 1.0f);
        }

        if (Input.GetAxis("HorizontalRotation") != 0)
        {
            float RotationAmount = Input.GetAxis("HorizontalRotation") * RotationSpeed;
            if (Rotation + RotationAmount < 0) Rotation = 360;
            if (Rotation + RotationAmount > 360) Rotation = 0;
            Rotation += RotationAmount;

            Rotation = Mathf.Clamp(Rotation, 0, 360.0f);
        }
        //If the current rig Y axis rotation doesnt match the Rotation variable
        //Smoothly Modify the camera rig's current rotation to match the Rotation variable
        if (Rotation != this.transform.rotation.eulerAngles.y) {
            UpdateRotation();
        }

        //If the current camera ortho size doesnt match the intended
        //Smoothly Modify the camera's orthographic size based on the ZoomFactor
        //Smoothly Modify the camera's X axis rotation as a function of the ZoomFactor
        if (Camera.main.orthographicSize != baseZoom * ZoomFactor) {
            UpdateZoom();
            UpdateTilt();
            //Add an update to the Camera Rigs Y Height to change as a function of ZoomFactor as well
        }
    }

    void UpdateRotation() {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, Rotation, 0), Time.deltaTime * 10 * Smoothing);
    }
    void UpdateZoom() {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, baseZoom * ZoomFactor, Time.deltaTime * 10 * Smoothing);
    }
    void UpdateTilt() { 
        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, Quaternion.Euler(baseZoomTilt * ZoomFactor, -45.0f, 0), Time.deltaTime * 10 * Smoothing);
    }
}
