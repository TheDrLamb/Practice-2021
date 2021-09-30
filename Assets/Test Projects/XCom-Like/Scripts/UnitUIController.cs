using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controls the UI of the Unit --> Attach to the Canvas Child Object of the Unit
public class UnitUIController : MonoBehaviour
{
    public StatBarController APBar;
    public StatBarController HPBar;

    Transform canvasParent;
    Transform canvas;
    Quaternion canvasOrigRot;
    
    Transform cameraTransform;
    float orthoZoom;
    float orthoZoom_orig;
    Vector3 UI_ScaleOrig;
    float UI_ScaleFactor;

    public UnitController unitController;


    //[NOTE] -> Adjust the Callout End Length based on health, up to 5 then affect thickness
    //      -> Also possibly scale up thickness as a factor of zoom

    public Transform[] CalloutStartPoints;
    public Transform[] CalloutPoints;
    public Polyline callout;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>().gameObject.transform;
        canvasOrigRot = canvas.rotation;

        canvasParent = canvas.parent;

        cameraTransform = Camera.main.transform;
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        orthoZoom = Camera.main.orthographicSize;

        //[NOTE] ---> These variables should be set by a global default settings file in the future.
        orthoZoom_orig = 8;
        UI_ScaleOrig = new Vector3(0.5f, 0.5f, 0.5f);

        Debug.Log("UI Controller Start");
        APBar.Initialize(unitController.unit.maxAP);
        HPBar.Initialize(unitController.unit.maxHP);
    }
    void Update()
    {
        Callout();
        Billboard();
        ZoomScaling();
    }

    void Callout() {
        //Set the callout start point that is closest to the camera
        CalloutPoints[0] = CalloutStartPoints[0];
        float d = Vector3.Distance(CalloutPoints[0].position, cameraTransform.position);

        foreach (Transform pt in CalloutStartPoints) {
            if (Vector3.Distance(pt.position, cameraTransform.position) < d) 
            {
                CalloutPoints[0] = pt;
                d = Vector3.Distance(CalloutPoints[0].position, cameraTransform.position);
            }
        }

        //Generate the callout.
        //[NOTE] -> Possibly set the line thickness based on the othro factor as well
        for (int i = 0; i < CalloutPoints.Length; i++)
        {
            callout.SetPointPosition(i, CalloutPoints[i].position - this.transform.parent.position);
        }
    }

    void Billboard() {

        Quaternion billboard = cameraTransform.rotation * canvasOrigRot;
        canvas.rotation = billboard;

        Vector3 lookPosition = cameraTransform.position - canvasParent.position;
        lookPosition.y = 0;
        canvasParent.rotation = Quaternion.LookRotation(lookPosition);
    }

    void ZoomScaling()
    {
        orthoZoom = Camera.main.orthographicSize;
        UI_ScaleFactor = Mathf.Clamp(orthoZoom / orthoZoom_orig, 0.5f, 1.75f);
        canvas.localScale = UI_ScaleOrig * UI_ScaleFactor;
    }

    public void UpdateStats()
    {
        Debug.Log("Update Stats");
        APBar.Set(unitController.unit.currentAP);
        HPBar.Set(unitController.unit.currentHP);
    }
}
