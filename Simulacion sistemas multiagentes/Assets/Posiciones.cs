using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posiciones : MonoBehaviour
{
    Vector3[] intersecciones;
    private float t = 0.0f;
    private Vector3 posicionInicio;
    private Vector3 distanciaFaltante;

    private bool girando = false;
    Vector3[] points; // Geometry
    int[] tris;       // Topology
    float angle = 0;


    private Vector3 targetWaypoint; // Hacia el que me voy a mover
    private int targetWaypointIndex = 1;
    private int lastWaypointIndex;

    private float minDistance = 2.0f; // minimia distancia para considerar que llegamos al waypoint

    private float speed = 4.0f; // velocidad del carro

    //private float rotationSpeed = 4.0f; // la rotacion para el carro (que no se vea siempre recto)


    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Geometry
        points = mesh.vertices;

        posicionInicio = transform.position;
        intersecciones = new Vector3[4];
        intersecciones[0] = new Vector3(9.45f, 0.0f, 0.21f);
        intersecciones[1] = new Vector3(9.48f, 0.0f, -20.09f);
        intersecciones[2] = new Vector3(-0.07f, 0.0f, -20.19f);
        intersecciones[3] = new Vector3(0.35f, 0.0f, -0.7743893f);
        lastWaypointIndex = 3; // cuenta desde 0 (por eso el -1)
        targetWaypoint = intersecciones[targetWaypointIndex];
        distanciaFaltante = targetWaypoint - posicionInicio;
    }

    // Update is called once per frame
    void Update()
    {
        float movementStep = speed * Time.deltaTime; // mueve con rapidez constante

        float currentDistance = Vector3.Distance(transform.position, targetWaypoint);
        if (t >= 1)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
            posicionInicio = transform.position;
            distanciaFaltante = targetWaypoint - posicionInicio;
            t = 0;
        }

        // Esta funcion es la que me dice si ya llegue
        CheckDistance(currentDistance);

        if (girando)
        {
            angle -= 5.0f;
            if (angle >= -45)
                TransformCar();
            else
            {
                girando = false;
            }
        }
        // Aqui hago el movimiento del carro
        transform.position = posicionInicio + (distanciaFaltante * t);
        t += 0.008f;
    }

    void UpdateTargetWaypoint()
    {
        if (targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
        }
        targetWaypoint = intersecciones[targetWaypointIndex];
    }

    void TransformCar()
    {
        int n = points.Length;
        int i;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];
        for (i = 0; i < n; i++)
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
        //Matrix4x4 transform1 = Transformations.TranslateM(3.2f, -2.4f, 0.0f);
        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Z);
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
            vs[i] = transform2 * vs[i];
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

    void CheckDistance(float currentDistance)
    {

        if (currentDistance < minDistance)
        {
            angle = 0;
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            // Geometry
            points = mesh.vertices;
            girando = true;
        }
    }

}
