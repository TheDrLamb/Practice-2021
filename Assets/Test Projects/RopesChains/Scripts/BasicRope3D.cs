using UnityEngine;
using System.Collections;


public class BasicRope3D : MonoBehaviour
{
    public Transform Root;
    public Transform End;
    public bool dynamic;
    public Vector3[] Segments;
    public Vector3[] SegmentTangents;
    Vector3 SegmentInterval;

    public int SegmentCount;
    [Range(0,5)]
    public float RopeSlack = 2.5f;

    private void Awake()
    {
        //Calculate the vertex of the possible parabola
        Segments = new Vector3[SegmentCount + 1];
        SegmentTangents = new Vector3[SegmentCount + 1];

        if (!dynamic) CalculateRope();
    }

    private void LateUpdate()
    {
        if (dynamic) CalculateRope();
    }

    private void CalculateRope() {
        SegmentInterval = (Root.position - End.position) / (SegmentCount);
        //Add Root and End
        Segments[0] = Root.position;
        Segments[SegmentCount] = End.position;

        //Calculate the "Center" of the rope
        float vertex = Segments.Length / 2;
        for (int i = 1; i < Segments.Length-1; i++)
        {
            float offset = RopeSlack - RopeSlack * Mathf.Pow(Mathf.Abs(vertex - i) / vertex,2);
            Segments[i] = Root.position - (SegmentInterval * i) - new Vector3(0, offset);

            //Calculate Tangent for the node behind
            SegmentTangents[i - 1] = (Segments[i] - Segments[i - 1]).normalized;
        }
        //Calculate Tangent for the node behind the end, and the end
        SegmentTangents[SegmentCount - 1] = (Segments[SegmentCount] - Segments[SegmentCount - 1]).normalized;
        SegmentTangents[SegmentCount] = SegmentTangents[SegmentCount - 1];
    }
}
