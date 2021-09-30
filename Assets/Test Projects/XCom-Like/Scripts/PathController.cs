using Shapes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public Color pathColor;
    Polyline line;
    Triangle triangle;
    Vector3 offset;

    private void Start()
    {
        line = GetComponentInChildren<Polyline>();
        triangle = GetComponentInChildren<Triangle>();
        offset = new Vector3(0.5f, 0, 0.5f);
    }

    public void Clear() {
        line.enabled = false;
        triangle.enabled = false;
    }
    public void SetPath(List<PathNode> _path) {
        if (_path.Count > 0)
        {
            List<Vector3> points = GetPathPoints(_path);

            line.enabled = true;
            triangle.enabled = true;

            line.SetPoints(points, GetPathPointColors(points.Count));

            Vector3 lastPoint = new Vector3(points[points.Count - 1].x, 0, points[points.Count - 1].y);
            Vector3 secondLastPoint = new Vector3(points[points.Count - 2].x, 0, points[points.Count - 2].y);
            Vector3 triangleDir = lastPoint - secondLastPoint;
            triangle.transform.localPosition = lastPoint + offset;
            triangle.transform.forward = triangleDir;
        }
    }

    List<Vector3> GetPathPoints(List<PathNode> _path)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < _path.Count; i++) {
            points.Add(_path[i].position);
        }
        return points;
    }

    List<Color> GetPathPointColors(int count) {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < count; i++)
        {
            colors.Add(pathColor);
        }
        return colors;
    }
}
