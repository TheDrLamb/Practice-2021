using UnityEngine;
using System.Collections;

public class SliderMovementController : ParentInteractableMovementController
{
    float spd;
    Vector3 StartPosition, EndPosition;
    Vector3 offset;
    float Position, Position2;
    float relativityFactor;

    protected override void Initialize()
    {
        offset = interactableTransform.position - StartNode.position;
        StartPosition = StartNode.position + offset;
        EndPosition = EndNode.position + offset;
        Position = Position2 = 0;
        relativityFactor = Vector3.Distance(StartPosition, EndPosition);
        spd = speed / (100 * relativityFactor);
    }

    protected override void UpdatePosition()
    {
        {
            if (movementType == MovementType.Free)
            {
                float x, z;
                x = Mathf.Lerp(StartPosition.x, EndPosition.x, Position);
                z = Mathf.Lerp(StartPosition.y, EndPosition.y, Position2);
                interactableTransform.position = new Vector3(x, interactableTransform.position.y, z);
            }
            else
            {
                interactableTransform.position = Vector3.Lerp(StartPosition, EndPosition, Position);
            }
        }
    }

    public override void SetPosition(Vector2 move)
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                Position += (spd * move.x);
                Position = Mathf.Clamp01(Position);
                break;
            case MovementType.Vertical:
                Position += (spd * move.y);
                Position = Mathf.Clamp01(Position);
                break;
            case MovementType.Free:
                Position += (spd * move.x);
                Position = Mathf.Clamp01(Position);
                Position2 += (spd * move.y);
                Position2 = Mathf.Clamp01(Position2);
                break;
        }
    }
}
