using UnityEngine;
using System.Collections;

public class InteractableSliderMovementController : MonoBehaviour
{
    public Transform Slider;
    public Transform StartNode, EndNode;
    public MovementType movementType = MovementType.Horizontal;
    public float speed = 10;
    float spd;

    Vector3 StartPosition, EndPosition;
    Vector3 sliderOffset;
    float sliderPosition, sliderPosition2;
    float relativityFactor;

    private void Start()
    {
        sliderOffset = Slider.position - StartNode.position;
        StartPosition = StartNode.position + sliderOffset;
        EndPosition = EndNode.position + sliderOffset;
        sliderPosition = sliderPosition2 = 0;
        relativityFactor = Vector3.Distance(StartPosition, EndPosition);
        spd = speed / (100 * relativityFactor);
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        {
            if (movementType == MovementType.Free)
            {
                float x, z;
                x = Mathf.Lerp(StartPosition.x, EndPosition.x, sliderPosition);
                z = Mathf.Lerp(StartPosition.y, EndPosition.y, sliderPosition2);
                Slider.position = new Vector3(x, Slider.position.y, z);
            }
            else
            {
                Slider.position = Vector3.Lerp(StartPosition, EndPosition, sliderPosition);
            }
        }
    }

    public void SetPosition(Vector2 move)
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                sliderPosition += (spd * move.x);
                sliderPosition = Mathf.Clamp01(sliderPosition);
                break;
            case MovementType.Vertical:
                sliderPosition += (spd * move.y);
                sliderPosition = Mathf.Clamp01(sliderPosition);
                break;
            case MovementType.Free:
                sliderPosition += (spd * move.x);
                sliderPosition = Mathf.Clamp01(sliderPosition);
                sliderPosition2 += (spd * move.y);
                sliderPosition2 = Mathf.Clamp01(sliderPosition2);
                break;
        }
    }
}
