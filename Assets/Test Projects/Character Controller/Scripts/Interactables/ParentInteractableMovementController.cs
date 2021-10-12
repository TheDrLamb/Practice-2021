using UnityEngine;
using System.Collections;

public class ParentInteractableMovementController : MonoBehaviour
{
    public Transform interactableTransform;
    public Transform StartNode, EndNode;
    public MovementType movementType = MovementType.Horizontal;
    public float speed = 10;

    private void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdatePosition();
    }

    protected virtual void Initialize() {
    }

    protected virtual void UpdatePosition()
    {
    }

    public virtual void SetPosition(Vector2 move)
    {
    }
}
