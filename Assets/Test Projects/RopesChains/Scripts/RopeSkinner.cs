using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSkinner : MonoBehaviour
{
    public float RopeThickness = 0.6f;
    bool dynamic = true;
    bool physicsRope = false;


    Transform[] links;

    Vector3[] points;
    Vector3[] tangents;

    MeshFilter meshFilter;
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector3> normals;
    List<Vector2> uvs;

    //If there are Segment nodes in the array build the rope from them, Else 
    //If there is an 'End' Transform given override Length, and generate the Segments between the root and the end at intervals Segment/(Root -> End).Distance
    //  Else - If there is no End given generate Segments from the root downward

    private void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = new Mesh();

        vertices = new List<Vector3>();
        triangles = new List<int>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();

        if (gameObject.GetComponent<BasicRope3D>() != null)
        {
            physicsRope = false;
            points = gameObject.GetComponent<BasicRope3D>().Segments;
            tangents = gameObject.GetComponent<BasicRope3D>().SegmentTangents;
            dynamic = gameObject.GetComponent<BasicRope3D>().dynamic;
        }
        else if(gameObject.GetComponent<PhysicsRope3D>() != null) 
        {
            physicsRope = true;
            dynamic = true;
            links = gameObject.GetComponent<PhysicsRope3D>().Segments;

            Debug.Log(links.Length);
        }

        if(!dynamic && !physicsRope) GenerateRopeMesh();
    }

    private void Update()
    {
        if (dynamic && !physicsRope) GenerateRopeMesh();
        if (physicsRope) GeneratePhysicsRopeMesh();
    }

    private void GenerateRopeMesh() {
        //For each Element in the Segments array generate a quad from the center of the current element to the next in the array.
        vertices.Clear();
        triangles.Clear();
        normals.Clear();
        uvs.Clear();

        for (int i = 0; i < points.Length - 1; i++)
        {
            AddQuads(points[i], tangents[i], points[i + 1], tangents[i + 1]);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        meshFilter.mesh = mesh;
    }

    private void GeneratePhysicsRopeMesh() {
        //For each Element in the Links array generate a quad from the center of the current element to the next in the array.
        vertices.Clear();
        triangles.Clear();
        normals.Clear();
        uvs.Clear();

        for (int i = 0; i < links.Length - 1; i++)
        {
            AddQuads(links[i], links[i + 1]);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        meshFilter.mesh = mesh;
    }

    private void AddQuads(Vector3 a, Vector3 tanA, Vector3 b, Vector3 tanB)
    {
        Vector3 forwardA, rightA, forwardB, rightB;

        forwardA = new Vector3(tanA.y, -tanA.x, tanA.z);
        rightA = new Vector3(forwardA.x, -forwardA.z, forwardA.y);

        forwardB = new Vector3(tanB.y, -tanB.x, tanB.z);
        rightB = new Vector3(forwardB.x, -forwardB.z, forwardB.y);

        float thickness = RopeThickness / 2;

        AddQuad(a + (forwardA - rightA) * thickness,
                a + (forwardA + rightA) * thickness,
                b + (forwardB - rightB) * thickness,
                b + (forwardB + rightB) * thickness);

        AddQuad(a + (-forwardA + rightA) * thickness,
                a + (-forwardA - rightA) * thickness,
                b + (-forwardB + rightB) * thickness,
                b + (-forwardB - rightB) * thickness);

        AddQuad(a + (rightA + forwardA) * thickness,
                a + (rightA - forwardA) * thickness,
                b + (rightB + forwardB) * thickness,
                b + (rightB - forwardB) * thickness);

        AddQuad(a + (-rightA - forwardA) * thickness,
                a + (-rightA + forwardA) * thickness,
                b + (-rightB - forwardB) * thickness,
                b + (-rightB + forwardB) * thickness);
    }

    private void AddQuads(Transform a, Transform b) {
        float thickness = RopeThickness / 2;
        AddQuad(a.localPosition + (a.forward + a.right) * thickness,
                a.localPosition + (a.forward - a.right) * thickness,
                b.localPosition + (b.forward + b.right) * thickness,
                b.localPosition + (b.forward - b.right) * thickness);

        AddQuad(a.localPosition + (-a.forward - a.right) * thickness,
                a.localPosition + (-a.forward + a.right) * thickness,
                b.localPosition + (-b.forward - b.right) * thickness,
                b.localPosition + (-b.forward + b.right) * thickness);

        AddQuad(a.localPosition + (a.right - a.forward) * thickness,
                a.localPosition + (a.right + a.forward) * thickness,
                b.localPosition + (b.right - b.forward) * thickness,
                b.localPosition + (b.right + b.forward) * thickness);

        AddQuad(a.localPosition + (-a.right + a.forward) * thickness,
                a.localPosition + (-a.right - a.forward) * thickness,
                b.localPosition + (-b.right + b.forward) * thickness,
                b.localPosition + (-b.right - b.forward) * thickness);
    }

    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int i = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);

        triangles.Add(i + 1);
        triangles.Add(i + 2);
        triangles.Add(i);

        triangles.Add(i + 1);
        triangles.Add(i + 3);
        triangles.Add(i + 2);

        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
    }
}
