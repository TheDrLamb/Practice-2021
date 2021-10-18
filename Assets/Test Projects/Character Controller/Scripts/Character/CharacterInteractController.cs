
using System.Collections;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    public Transform HoldLocation;
    public Transform LeftHand, RightHand;
    public LayerMask interactableLayer;
    public float interactionRange;
    public InteractionState state = InteractionState.Free;

    public Interactable currentInteract;

    GameObject currentTarget;

    public float buttonHoldTime = 1.5f;
    float buttonHoldTimer;

    CharacterCombatController shooting;
    CharacterInputController inputController;


    private void Start()
    {
        shooting = GetComponent<CharacterCombatController>();
        inputController = GetComponent<CharacterInputController>();
    }

    private void Update()
    {
        CheckForInteractables();
        InteractUIVisualUpdate();
    }

    private void FixedUpdate()
    {
        //[NOTE] -> if not grabbing something then smooth lerp the weight of the arms rig to 0
        if (currentInteract) HandVisualsUpdate();
        if (state == InteractionState.ParentInteraction) currentInteract.GetComponent<ParentInteractable>().UpdateInput(inputController.GetInputRaw());
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
        if (currentInteract.GetComponent<GrabbableInteractable>())
        {
            GrabbableInteractable currentGrab = currentInteract.GetComponent<GrabbableInteractable>();
            LeftHand.position = currentGrab.Left.position;
            LeftHand.rotation = currentGrab.Left.rotation;
            RightHand.position = currentGrab.Right.position;
            RightHand.rotation = currentGrab.Right.rotation;
        }
    }

    private void InteractUIVisualUpdate() {
        //Set Interaction UI
        //Debug.Log($"Press E to grab {currentTarget.name}");
    }

    public void InteractDown()
    {
        //Makes the determinations of what is being interacted with and calls the relevant functions
        if (currentTarget)
        {
            Interactable target = currentTarget.GetComponent<Interactable>();
            if(!target) target = currentTarget.GetComponentInParent<Interactable>();
            switch (target.type)
            {
                case InteractableType.Parent:
                    FixedInteraction(target);
                    break;
                case InteractableType.Child:
                    HoldableInteraction(target);
                    break;
                case InteractableType.Static:
                    target.Interact();
                    break;

            }
        }
    }

    public void InteractHeld() {
        //Default Release condition -- If something is being interacted with already.
        if (currentInteract != null)
        {
            buttonHoldTimer += Time.deltaTime;
            if (buttonHoldTimer >= buttonHoldTime)
            {
                if (state != InteractionState.Free)
                {
                    ReleaseVisualUpdate();
                    Release();
                }
            }
        }
    }

    public void InteractUp()
    {
        buttonHoldTimer = 0;
    }

    private void HoldableInteraction(Interactable target){
        //Drop/Release the currently Held Item
        if(currentInteract != null)
        {
            //Release currently held object
            Release();
        }
        //Check Interactable type
        currentInteract = target;
        currentInteract.Interact();
        if (target.GetComponent<ChildInteractable>())
        {
            state = InteractionState.ChildInteraction;
            //Grab the new item
            ChildInteractable currentHold = currentInteract.GetComponent<ChildInteractable>();
            Vector3 offset = currentInteract.GetComponent<ChildInteractable>().offset;

            //[NOTE] -> Commented out hand lerp for hard prototyping
            //StartCoroutine(SlerpHandTransforms(LeftHand, RightHand, currentHold.Left, currentHold.Right, 0.5f));
            
            StartCoroutine(SlerpTransformLocal(currentInteract.transform, HoldLocation, 0.75f, offset));
        }
    }

    private void FixedInteraction(Interactable target)
    {
        //Drop/Release the currently Held Item
        if (currentInteract != null)
        {
            //Release currently held object
            Release();
        }
        //Check Interactable type
        currentInteract = target;
        currentInteract.Interact();
        if (target.GetComponent<ParentInteractable>())
        {
            Debug.Log("Fixed Interaction");
            //Set state Hold
            state = InteractionState.ParentInteraction;
            //Reverse grab onto the Interactable
            ParentInteractable currentInt = currentInteract.GetComponent<ParentInteractable>();
            GetComponent<Rigidbody>().isKinematic = true;

            //[NOTE] -> Commented out hand lerp for hard prototyping
            //StartCoroutine(SlerpHandTransforms(LeftHand, RightHand, currentInt.Left, currentInt.Right, 0.5f));
            
            StartCoroutine(SlerpTransformLocal(this.transform, currentInt.playerPosition, 0.75f, Vector3.zero));
        }
    }

    private void Release() {
        //Drop/Release the currently Held Item
        currentInteract.Interact();
        //If held Item
        if (currentInteract.GetComponent<ChildInteractable>())
        {
            //Add Random Force to the Dropped Item
            Vector3 dirR = this.transform.forward + (this.transform.right * Random.Range(-1.5f, 1.5f)) + (0.5f * this.transform.up);
            currentInteract.GetComponent<Rigidbody>().AddForce(dirR * 250, ForceMode.Acceleration);
            currentInteract.GetComponent<Rigidbody>().AddTorque(-dirR * 250, ForceMode.Acceleration);
        }

        //If fixed
        if (currentInteract.GetComponent<ParentInteractable>())
        {
            //Unparent the player
            this.transform.parent = null;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        state = InteractionState.Free;
        currentInteract = null;
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
public enum InteractionState { 
    ChildInteraction,
    ParentInteraction,
    Free
}
