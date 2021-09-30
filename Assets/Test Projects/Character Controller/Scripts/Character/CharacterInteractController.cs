
using System.Collections;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    public Transform HoldLocation;
    public Transform LeftHand, RightHand;
    public LayerMask interactableLayer;
    public float interactionRange;
    public HoldState state = HoldState.Free;

    public Interactable currentHold;

    GameObject currentTarget;

    public float releaseTime = 1.5f;
    float releaseTimer;

    CharacterShootingController shooting;


    private void Start()
    {
        shooting = GetComponent<CharacterShootingController>();
    }

    private void Update()
    {
        CheckForInteractables();
        InteractUIVisualUpdate();
    }

    private void FixedUpdate()
    {
        //[NOTE] -> if not grabbing something then smooth lerp the weight of the arms rig to 0
        if (currentHold) HandVisualsUpdate();
    }

    private void CheckForInteractables() {
        //Sphere cast all from the player for all interactable in range.
        //Choose the one closest to the cursor and set it as the target

        //[NOTE] -> Update to check near cursor for the future

        Collider[] interactables = Physics.OverlapSphere(this.transform.position, interactionRange, interactableLayer);
        if (interactables.Length > 0) {
            float  d = 9999;
            foreach (Collider c in interactables) {
                float r = Vector3.Distance(this.transform.position, c.transform.position);
                if (r < d)
                {
                    currentTarget = c.gameObject;
                    d = r;
                }
            }
        }
        else 
        {
            currentTarget = null;
        }
    }

    private void HandVisualsUpdate()
    {
        LeftHand.position = currentHold.Left.position;
        LeftHand.rotation = currentHold.Left.rotation;
        RightHand.position = currentHold.Right.position;
        RightHand.rotation = currentHold.Right.rotation;
    }

    private void InteractUIVisualUpdate() {
        //Set Interaction UI
        //Debug.Log($"Press E to grab {currentTarget.name}");
    }

    public void InteractDown() {
        //Makes the determinations of what is being interacted with and calls the relevant functions
        if (currentTarget)
        {
            Interactable target = currentTarget.GetComponent<Interactable>();
            switch (target.type)
            {
                case InteractableType.Fixed:
                    //Fixed Code
                    break;
                case InteractableType.Holdable:
                    Grab(target);
                    break;
            }
        }
    }

    public void InteractHeld() {
        //Default Release condition -- If something is being interacted with already.
        if (state != HoldState.Free)
        {
            releaseTimer += Time.deltaTime;
            ReleaseVisualUpdate();
            if (releaseTimer >= releaseTime) Release();
        }
    }

    public void InteractUp()
    {
        releaseTimer = 0;
    }

    private void Grab(Interactable target){
        //Drop/Release the currently Held Item
        if(currentHold != null)
        {
            //Release currently held object
            Release();
        }
        //Check Interactable type
        currentHold = target;
        currentHold.Grab();
        if (currentTarget.GetComponent<HoldableObject>())
        {
            if (currentTarget.GetComponent<GunInteractable>())
            {
                //Set state Hold Gun
                state = HoldState.HoldingGun;
                //Trigger Gun Specific Code
                GunInteractable gun = currentTarget.GetComponent<GunInteractable>();
                shooting.gunHeld = true;
                shooting.damageAmount = gun.damageAmount;
                shooting.rateOfFire = gun.rateOfFire;
                shooting.fireMode = gun.fireMode;
                shooting.muzzleFlash = gun.muzzleFlash;
            }
            else 
            {
                //Set state Hold
                state = HoldState.Holding;
            }
            //Grab the new item
            Vector3 offset = currentHold.GetComponent<HoldableObject>().offset;
            StartCoroutine(SlerpHandTransforms(LeftHand, RightHand, currentHold.Left, currentHold.Right, 0.5f));
            StartCoroutine(SlerpTransformLocal(currentHold.transform, HoldLocation, 0.75f, offset));
        }
    }

    private void Release() {
        //Drop/Release the currently Held Item
        currentHold.Drop();

        //Add Random Force to the Dropped Item
        Vector3 dirR = this.transform.forward + (this.transform.right * Random.Range(-1.5f, 1.5f)) + (0.5f * this.transform.up);
        currentHold.GetComponent<Rigidbody>().AddForce(dirR * 250, ForceMode.Acceleration);
        currentHold.GetComponent<Rigidbody>().AddTorque(-dirR * 250, ForceMode.Acceleration);

        if (state == HoldState.HoldingGun)
        {
            shooting.gunHeld = false;
            shooting.damageAmount = 0;
            shooting.rateOfFire = 0;
            shooting.muzzleFlash = null;
        }

        state = HoldState.Free;
        currentHold = null;
        releaseTimer = 0;
    }

    private void ReleaseVisualUpdate()
    {
        //Drop/Release the currently Held Item
        //Debug.LogError($"Dropping in {releaseTime - releaseTimer}");
    }
    private IEnumerator SlerpTransformLocal(Transform holdObject, Transform holdPosition, float over_time, Vector3 offset)
    {
        holdObject.parent = holdPosition;
        Vector3 start = holdObject.localPosition;
        Vector3 goal = offset;

        Quaternion startRot = holdObject.localRotation;
        Quaternion goalRot = Quaternion.identity;

        float startTime = Time.time;

        for (float t = 0; t < over_time; t += Time.time - startTime) {
            holdObject.localPosition = Vector3.Lerp(start, goal, t);
            holdObject.localRotation = Quaternion.Lerp(startRot, goalRot, t);
            yield return null;
        }
        holdObject.localPosition = goal;
        holdObject.localRotation = goalRot;
    }

    private IEnumerator SlerpHandTransforms(Transform LeftHand, Transform RightHand, Transform LeftHold, Transform RightHold, float over_time)
    {
        Vector3 leftStart = LeftHand.position;
        Vector3 leftEnd = LeftHold.position;
        Vector3 rightStart = RightHand.position;
        Vector3 rightEnd = RightHold.position;

        float startTime = Time.time;

        for (float t = 0; t < over_time; t += Time.time - startTime)
        {
            LeftHand.position = Vector3.Lerp(leftStart, leftEnd, t);
            RightHand.position = Vector3.Lerp(rightStart, rightEnd, t);
            yield return null;
        }
        LeftHand.position = leftEnd;
        RightHand.position = rightEnd;
    }

}
public enum HoldState { 
    Holding,
    HoldingGun,
    Free
}
