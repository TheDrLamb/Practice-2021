using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsRope3D : MonoBehaviour
{
    public Transform Root;
    public Transform[] Segments;
    public Transform End;

    public GameObject SegmentPrefab;
    public float Length;
    public int SegmentCount;
    Vector3 SegmentInterval;

    //If there are Segment nodes in the array build the rope from them, Else 
    //If there is an 'End' Transform given override Length, and generate the Segments between the root and the end at intervals Segment/(Root -> End).Distance
    //  Else - If there is no End given generate Segments from the root downward

    private void Awake()
    {
        if (Segments.Length == 0)
        {
            if (End != null)
            {
                Length = Vector3.Distance(Root.position, End.position);
                Segments = new Transform[SegmentCount + 2];
                SegmentInterval = (Root.position - End.position) / (SegmentCount + 1);
            }
            else
            {
                Segments = new Transform[SegmentCount + 1];
                SegmentInterval = new Vector3(0, Length / (SegmentCount + 1), 0);
            }
            //Generate Segments
            Rigidbody lastSegment = Root.gameObject.GetComponent<Rigidbody>();
            Segments[0] = Root;
            for (int i = 1; i <= SegmentCount; i++)
            {
                //Instantiate the segment prefab below the Root 
                GameObject newSegment = Instantiate(SegmentPrefab, this.transform);
                newSegment.transform.position = Root.position + (SegmentInterval * -i);
                newSegment.GetComponent<HingeJoint>().connectedBody = lastSegment;
                lastSegment = newSegment.GetComponent<Rigidbody>();
                newSegment.name = $"Rope Segment: {i}";
                Segments[i] = newSegment.transform;
            }
            if (End != null)
            {
                End.GetComponent<HingeJoint>().connectedBody = lastSegment;
                Segments[SegmentCount + 1] = End;
            }
        }
    }
}
