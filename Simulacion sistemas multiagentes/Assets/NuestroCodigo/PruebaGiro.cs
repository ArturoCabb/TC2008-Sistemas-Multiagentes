using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaGiro : MonoBehaviour
{
    Vector3[] points; // Geometry
    int[] tris;       // Topology
    float angle = 0;
    //Vector3 posInicio;

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
        Vector4 auxi = posInicio;
        auxi.w = 1.0f;
        Vector3 final;
        */

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
         * Arriba derecha*/
         /*
        Matrix4x4 transform2 = Transformations.RotateM(-angle, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(2.5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(-2.5f, 0.0f, 0.0f);
        //Matrix4x4 transform4 = Transformations.TranslateM(10, 0.0f, 10);
        */
        /* arriba izquierda*/
        /*
        Matrix4x4 transform2 = Transformations.RotateM(-angle - 90, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0f, 0.0f, -5.0f);
        */
        // izquierda abajo*/
        /*
        Matrix4x4 transform2 = Transformations.RotateM(-angle - 180, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(5.0f, 0.0f, 0.0f);
        */
        //abajo derecha */
        /*
        Matrix4x4 transform2 = Transformations.RotateM(-angle - 270, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0.0f, 0.0f, 5.0f);
        */
        
        
        /* Hacen giro en sentido de las manecillas del reloj
        */
        // Abajo izquierda
        /*
        Matrix4x4 transform2 = Transformations.RotateM(angle + 90, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0f, 0.0f, 5.0f);
        */
        // Arriba izquierda
        /*
        Matrix4x4 transform2 = Transformations.RotateM(angle + 180, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(5.0f, 0.0f, 0.0f);
        */
        // derecha arriba
        /*
        Matrix4x4 transform2 = Transformations.RotateM(angle + 270, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(5f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(0.0f, 0.0f, -5.0f);
        */
        // abajo derecha
        
        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(10f, 0.0f, 0.0f);
        Matrix4x4 transform3 = Transformations.TranslateM(-10.0f, 0.0f, 0.0f);
        
        
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
        

        //auxi = transform3 * transform2 * transform1 * auxi;
        //final = auxi;
        /*
        v0 = A * v0;
        v1 = A * v1;
        v2 = A * v2;
        */
        //Vector3[] points2 = { v0, v1, v2 };

        
        GetComponent<MeshFilter>().mesh.vertices = final;
        
        //transform.position = final;

    }

    void QuitarPivote(){
        int n = points.Length;
        int i;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];
        for(i=0; i < n; i++)
        {
            vs[i] = points[i];
            vs[i].w = 1.0f;
        }
        Matrix4x4 transform1 = Transformations.TranslateM(0.0f, 0.0f, 10.0f);

        for (i = 0; i < n; i++)
        {
            vs[i] = transform1 * vs[i];
            final[i] = vs[i];
        }
        GetComponent<MeshFilter>().mesh.vertices = final;
    }


    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
        transform.position = new Vector3(10,0,10);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Geometry
        points = mesh.vertices;

        //posInicio = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        angle += 0.3f;
        if (angle > 90){
            angle = 0;
            //QuitarPivote();
        }
        TransformCar();
    }
}
