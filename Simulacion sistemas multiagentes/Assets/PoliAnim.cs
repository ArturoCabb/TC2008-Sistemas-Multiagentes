using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliAnim : MonoBehaviour
{
    Vector3[] points;
    int[] tris;
    float angle = 0;

    void TransformTriangle()
    {
        Vector4 v0 = points[0];
        Vector4 v1 = points[1];
        Vector4 v2 = points[2];

        v0.w = 1.0f;
        v1.w = 1.0f;
        v2.w = 1.0f;

        // 1. ¿Cómo traslado el triangulo 3.2 unidades a la derecha y 2.4 unidades abajo?
        Matrix4x4 transform1 = Transformations.TranslateM(3.2f, -2.4f, 0.0f);

        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        // Usando programacion dinamica
        Matrix4x4 A = transform1 * transform2;

        // NO SE DEBE HACER
        // v0 = transform1 * transform2 * v0;
        // v0 = transform1 * transform2 * v0;
        // v0 = transform1 * transform2 * v0;

        v0 = A * v0;
        v1 = A * v1;
        v2 = A * v2;

        Vector3[] points2 = { v0, v1, v2 };
        GetComponent<MeshFilter>().mesh.vertices = points2;
    }

    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        // Geometry
        points = new Vector3[3];
        points[0] = new Vector3(0, 0, 0);
        points[1] = new Vector3(5, 0, 0);
        points[2] = new Vector3(2, 5, 0);

        tris = new int[3];
        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        mesh.vertices = points; // 2, 0, -2          2, 0,4        8,0,4       -8,0,4
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(Vector3.zero, new Vector3(50, 0, 0), Color.red);
        Debug.DrawLine(Vector3.zero, new Vector3(0, 50, 0), Color.green);
        Debug.DrawLine(Vector3.zero, new Vector3(0, 0, 50), Color.blue);
        angle += 1.0f;

        if (angle > 360)
        {
            angle = 0.0f;
        }
        TransformTriangle();
    }
}
