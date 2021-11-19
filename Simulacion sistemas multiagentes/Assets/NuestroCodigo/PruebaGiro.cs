using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaGiro : MonoBehaviour
{
    Vector3[] points; // Geometry
    int[] tris;       // Topology
    float angle = 0;
    float x = 0;
    float z = 0;

    void TransformCar()
    {
        int n = points.Length;
        int i;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];
        for(i=0; i < n; i++)
        {
            vs[i] = points[i];
            vs[i].w = 1.0f;
        }
        /*
        Vector4 v0 = points[0];
        Vector4 v1 = points[1];
        Vector4 v2 = points[2];
        v0.w = 1.0f;
        v1.w = 1.0f;
        v2.w = 1.0f;
        */
        // 1. ¿Cómo traslado el triángulo 3.2 unidades a la derecha y 2.4 unidades abajo?

        /* Estos hacen giro al sentido contrario de las manecillas del reloj
         * Abajo a la izquierda*/
        Matrix4x4 transform2 = Transformations.RotateM(-angle, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(-5f, 0.0f, 0.0f);
        /* arriba izquierda
        Matrix4x4 transform2 = Transformations.RotateM(-angle - 90, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0f, 0.0f, -5.0f);
        * izquierda abajo
        Matrix4x4 transform2 = Transformations.RotateM(-angle - 180, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(5.0f, 0.0f, 0.0f);
        * abajo derecha
        Matrix4x4 transform2 = Transformations.RotateM(-angle - 270, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0.0f, 0.0f, 5.0f);
        
        
        /* Hacen giro en sentido de las manecillas del reloj
        * Arriba derecha
        Matrix4x4 transform2 = Transformations.RotateM(angle + 90, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0f, 0.0f, 5.0f);
        * Abajo derecha
        Matrix4x4 transform2 = Transformations.RotateM(angle + 180, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(5.0f, 0.0f, 0.0f);
        * izquierda arriba
        Matrix4x4 transform2 = Transformations.RotateM(angle + 270, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0.0f, 0.0f, -5.0f);
        * arriba
        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(-5.0f, 0.0f, 0.0f);
        
        /*
        Vector4 temp1 = new Vector4(points[0].x, points[0].y, points[0].z, 1);
        Vector4 temp2 = new Vector4(points[1].x, points[1].y, points[1].z, 1);
        Vector4 temp3 = new Vector4(points[2].x, points[2].y, points[2].z, 1);
        
        v0 = transform1 * transform2 * temp1;
        v1 = transform1 * transform2 * temp2;
        v2 = transform1 * transform2 * temp3;
        */
        //Matrix4x4 A = transform1 * transform2;
        for (i = 0; i < n; i++)
        {
            vs[i] = transform3 * transform2 * transform1 * vs[i];
            final[i] = vs[i];
        }
        /*
        v0 = A * v0;
        v1 = A * v1;
        v2 = A * v2;
        */
        //Vector3[] points2 = { v0, v1, v2 };

        GetComponent<MeshFilter>().mesh.vertices = final;

    }


    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Geometry
        points = mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        angle += 0.5f;
        if (angle > 90)
            angle = 0.0f;

        x += 0.1f;
        if (x > 9)
            x = 0.0f;

        z += 0.0f;
        if (z > 10)
            z = 0.0f;
        TransformCar();
    }
}
