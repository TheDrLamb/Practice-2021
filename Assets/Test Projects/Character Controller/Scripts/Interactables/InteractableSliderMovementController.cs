using UnityEngine;
using System.Collections;

public class InteractableSliderMovementController : MonoBehaviour
{
    public Transform Slider;
    public Transform StartNode, EndNode;
    public float speed = 10;
    float spd;

    Vector3 StartPosition, EndPosition;
    Vector3 sliderOffset;
    float sliderPosition;
    float relativityFactor;

    private void Start()
    {
        sliderOffset = Slider.position - StartNode.position;
        StartPosition = StartNode.position + sliderOffset;
        EndPosition = EndNode.position + sliderOffset;
        sliderPosition = 0;
        relativityFactor = Vector3.Distance(StartPosition, EndPosition);
        spd = speed / (100 * relativityFactor);
    }

    void Update()
    {
        Slider.position = Vector3.Lerp(StartPosition, EndPosition, sliderPosition);
    }

    public void AdjustPosition(Vector2 move) 
    {
        sliderPosition +=(spd * move.y);
        sliderPosition = Mathf.Clamp01(sliderPosition);
    }
}
