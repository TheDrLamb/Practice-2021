
using System.Collections;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    public Transform HoldLocation;
    public LayerMask interactableLayer;
    public float interactionRange;

    public Interactable currentInteract;

    GameObject currentTarget;

    public float buttonHoldTime = 1.5f;
    float buttonHoldTimer;

    CharacterStateMachineController stateMachine;
    CharacterCombatController combatController;
    CharacterAnimationController animationController;

    private void Start()
    {
        stateMachine = GetComponent<CharacterStateMachineController>();
        combatController = GetComponent<CharacterCombatController>();
        animationController = GetComponent<CharacterAnimationController>();
    }

    private void Update()
    {
        CheckForInteractables();
        InteractUIVisualUpdate();
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
                if (stateMachine.status == CharacterState.ChildInteraction || stateMachine.status == CharacterState.ParentInteraction)
                {
                    buttonHoldTimer = 0;
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
            Release(true);
        }
        //Check Interactable type
        currentInteract = target;
        currentInteract.Interact();
        if (target.GetComponent<ChildInteractable>())
        {
            // *************
            // Send Message to the stateMachine to transition to the ChildInteraction State
            // *************
            stateMachine.ChangeState(CharacterState.ChildInteraction);

            //Grab the new item
            ChildInteractable currentHold = currentInteract.GetComponent<ChildInteractable>();
            Vector3 offset = currentInteract.GetComponent<ChildInteractable>().offset;

            //Set Hand positions
            animationController.SetHandTransforms(currentHold.Left, currentHold.Right);
            //Set holdable position
            StartCoroutine(SlerpTransformLocal(currentInteract.transform, HoldLocation, 0.75f, offset));
        }
    }

    private void FixedInteraction(Interactable target)
    {
        //Drop/Release the currently Held Item
        if (currentInteract != null)
        {
            //Release currently held object
            Release(true);
        }
        //Check Interactable type
        currentInteract = target;
        currentInteract.Interact();
        if (target.GetComponent<ParentInteractable>())
        {
            //Set state
            // *************
            // Send Message to the stateMachine to transition to the ParentInteraction State
            // *************
            stateMachine.ChangeState(CharacterState.ParentInteraction);

            //Reverse grab onto the Interactable
            ParentInteractable currentInt = currentInteract.GetComponent<ParentInteractable>();
            GetComponent<Rigidbody>().isKinematic = true;

            //Set the Hand Postions
            animationController.SetHandTransforms(currentInt.Left, currentInt.Right);
            //Set Character Postion
            StartCoroutine(SlerpTransformLocal(this.transform, currentInt.playerPosition, 0.75f, Vector3.zero));
        }
    }

    private void Release(bool swap = false) {
        //Drop/Release the currently Held Item
        currentInteract.Interact();

        //If child
        if (currentInteract.GetComponent<ChildInteractable>())
        {
            //Add Random Force to the Dropped Item
            Vector3 tempPos = currentInteract.transform.position;
            currentInteract.transform.parent = null;
            currentInteract.transform.position = tempPos;

            Vector3 dirR = currentInteract.transform.forward + (currentInteract.transform.right * Random.Range(-1.5f, 1.5f)) + (0.5f * currentInteract.transform.up);
            currentInteract.GetComponent<Rigidbody>().AddForce(dirR * 250, ForceMode.Acceleration);
            currentInteract.GetComponent<Rigidbody>().AddTorque(-dirR * 250, ForceMode.Acceleration);
        }

        //If parent
        if (currentInteract.GetComponent<ParentInteractable>())
        {
            //Unparent the player
            Vector3 tempPos = this.transform.position;
            this.transform.parent = null;
            this.transform.position = tempPos;
            GetComponent<Rigidbody>().isKinematic = false;
            //[Note] -> Add force to the player to bounce them back
        }

        if (!swap)
        {
            // *************
            // Transition to default equipment state
            // *************
            combatController.Equip();
        }

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

}
