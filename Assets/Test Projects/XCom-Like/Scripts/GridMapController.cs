using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapController: MonoBehaviour
{
    public int mapSize = 16;
    public int mapHeight = 16;
    public GridMap map;

    MeshFilter meshFilter;
    Mesh mesh;
    MeshCollider meshCollider;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector3> normals;
    List<Vector2> uvs;

    void Awake()
    {
        map = new GridMap(mapSize);
        VisualizeMap();
    }

    public void VisualizeMap()
    {
        //Initialize mesh
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        mesh = new Mesh();

        vertices = new List<Vector3>();
        triangles = new List<int>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();

        //Loop over the Map Tiles array and for each cell generate a quad
        foreach (GridTile c in map.tiles) {
            AddQuad(c.X, c.Y);
        }

        //Assign mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        transform.position = transform.position + new Vector3(0, mapHeight, 0);
    }

    private void AddQuad(int x, int y) {
        Vector3 a, b, c, d;
        a = new Vector3(x , 0, y );
        b = new Vector3(x + 1, 0, y );
        c = new Vector3(x , 0, y + 1);
        d = new Vector3(x + 1, 0, y + 1);
        AddQuad(a, b, c, d);
    }

    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int i = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);

        triangles.Add(i + 0);
        triangles.Add(i + 2);
        triangles.Add(i + 1);

        triangles.Add(i + 2);
        triangles.Add(i + 3);
        triangles.Add(i + 1);

        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
    }

    public int GetTileID(Vector3 _point) {
        int X, Y;
        X = Mathf.RoundToInt(_point.x - 0.45f);
        Y = Mathf.RoundToInt(_point.z - 0.45f);


        X = X >= mapSize ? mapSize - 1 : X;
        X = X < 0 ? 0 : X;

        Y = Y >= mapSize ? mapSize - 1 : Y;
        Y = Y < 0 ? 0 : Y;

        return map.GetTileID(X, Y);
    }

    public GridTile GetTile(Vector3 _point)
    {
        int X, Y;
        X = Mathf.RoundToInt(_point.x - 0.45f);
        Y = Mathf.RoundToInt(_point.z - 0.45f);


        X = X >= mapSize ? mapSize - 1 : X;
        X = X < 0 ? 0 : X;

        Y = Y >= mapSize ? mapSize - 1 : Y;
        Y = Y < 0 ? 0 : Y;

        return map.GetTile(X, Y);
    }

    public GridTile GetTile(int X, int Y)
    {
        return map.tiles[X, Y];
    }
}