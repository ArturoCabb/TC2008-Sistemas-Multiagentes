using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poli : MonoBehaviour
{
    Vector3[] points;
    int[] tris;
    float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        points = new Vector3[3];
        points[0] = new Vector3(2, 0, -2);
        points[1] = new Vector3(5, 0, 0);
        points[2] = new Vector3(2, 5, 0);

        tris = new int[3];
        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        mesh.vertices = points;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(Vector3.zero, new Vector3(50, 0, 0), Color.red);
        Debug.DrawLine(Vector3.zero, new Vector3(00, 50, 0), Color.green);
        Debug.DrawLine(Vector3.zero, new Vector3(00, 0, 50), Color.blue);
    }
}
