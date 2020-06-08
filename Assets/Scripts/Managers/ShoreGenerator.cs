using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoreGenerator : MonoBehaviour
{

    public int ShoreSize
    {
        get
        {
            return shoreSize;
        }
        set
        {
            Debug.Log("SEtting shore size!!!");
            GenerateMesh();
            shoreSize = value;
        }
    }

    [SerializeField]
    int shoreSize = 10;

    Mesh shoreMesh;
    MeshFilter filter;

    //Points for the curve

    // The vertices and triangles of the mesh
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    List<Vector3[]> curves = new List<Vector3[]>();

    void Start()
    {
        // Get a reference to the mesh and clear it
        filter = GetComponent<MeshFilter>();
        GenerateMesh();

    }


    void GenerateMesh()
    {
        shoreMesh = filter.mesh;
        shoreMesh.Clear();

        // Generate 4 random points for the top
        var xPos = 0f;
        for (int c = 0; c < shoreSize; c++)
        {
            var curve = new Vector3[4];
            for (int i = 0; i < curve.Length; i++)
            {
                Vector3[] prev = null;
                if (curves.Count > 0)
                {
                    prev = curves[curves.Count - 1];
                }
                if (prev != null && i == 0)
                {
                    // Start of a new curve
                    // Set to the last point of the previous
                    curve[i] = prev[curve.Length - 1];
                }
                else if (prev != null && i == 1)
                {
                    // First control point of a new curve
                    // Use the end of the previous curve to calculate
                    curve[i] = 2f *
                                prev[curve.Length - 1] -
                                prev[curve.Length - 2];
                }
                else
                {
                    // Generate random point
                    curve[i] = new Vector3(xPos, Random.Range(1f, 2f), 0f);
                }
                xPos += 0.5f;
            }
            curves.Add(curve);

        }

        // Number of points to draw, how smooth each curve is
        int curveResolution = 20;
        foreach (var curve in curves)
        {
            for (int i = 0; i < curveResolution; i++)
            {
                float t = (float)i / (float)(curveResolution - 1);
                Vector3 p = InterpolateBezierPoint(t, curve[0], curve[1], curve[2], curve[3]);
                AddMeshPoint(p);
            }
        }

        // Assign the vertices and triangles to the mesh 
        shoreMesh.vertices = vertices.ToArray();
        shoreMesh.triangles = triangles.ToArray();
        Bounds bounds = shoreMesh.bounds;

        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / bounds.size.x, vertices[i].y / bounds.size.y);
        }
        shoreMesh.uv = uvs;
        shoreMesh.RecalculateNormals();
    }

    void AddMeshPoint(Vector3 point)
    {
        // Create a corresponding point along the bottom 
        vertices.Add(new Vector3(point.x, 0f, 0f)); // Then add our top point 
        vertices.Add(point);

        if (vertices.Count >= 4)
        {
            // Completed a new quad, create 2 triangles
            int start = vertices.Count - 4;
            triangles.Add(start + 0);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start + 1);
            triangles.Add(start + 3);
            triangles.Add(start + 2);
        }
    }



    /**
     * Interpolate a point on the curve at t, a value between 0.0 and 1.0
     * which represents the start point and end point of the curve.
     * https://vimeo.com/106757336
     */
    Vector3 InterpolateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

}
