using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Conduce : MonoBehaviour
{
    public List<Transform> intersecciones = new List<Transform>();
    //Vector3[] intersecciones;
    private float t = 0.0f;
    private Vector3 posicionInicio;
    private Vector3 distanciaFaltante;
    private int[] rutasDefinidas;

    public int ruta;
    public int numeroSerie;

    private bool girando = false;
    Vector3[] points; // Geometry
    int[] tris;       // Topology
    float angle = 0;
    public int[,] FSMC = new int[42, 42]; // Modelo de Markov - via la matriz Adjunta

    private Transform targetWaypoint; // Hacia el que me voy a mover
    private int targetWaypointIndex = 0;
    private int lastWaypointIndex;

    private float minDistance = 1.0f; // minimia distancia para considerar que llegamos al waypoint

    private float speed = 2.0f; // velocidad del carro

    //private float rotationSpeed = 4.0f; // la rotacion para el carro (que no se vea siempre recto)


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 42; i++)
        {
            for (int j = 0; j < 42; j++)
            {
                FSMC[i, j] = 0;
            }
        }
        FSMC[0, 2] = 1;
        FSMC[1, 2] = 1;
        FSMC[2, 3] = 1;
        FSMC[3, 4] = 1;
        FSMC[3, 5] = 1;
        FSMC[5, 6] = 1;
        FSMC[6, 8] = 1;
        FSMC[7, 8] = 1;
        FSMC[8, 9] = 1;
        FSMC[9, 11] = 1;
        FSMC[10, 11] = 1;
        FSMC[10, 20] = 1;
        FSMC[12, 1] = 1;
        FSMC[13, 12] = 1;
        FSMC[13, 14] = 1;
        FSMC[14, 15] = 1;
        FSMC[15, 16] = 1;
        FSMC[15, 18] = 1;
        FSMC[16, 7] = 1;
        FSMC[17, 16] = 1;
        FSMC[17, 18] = 1;
        FSMC[18, 19] = 1;
        FSMC[19, 21] = 1;
        FSMC[19, 22] = 1;
        FSMC[20, 21] = 1;
        FSMC[20, 22] = 1;
        FSMC[21, 27] = 1;
        FSMC[23, 18] = 1;
        FSMC[25, 26] = 1;
        FSMC[26, 28] = 1;
        FSMC[27, 28] = 1;
        FSMC[28, 37] = 1;
        FSMC[29, 13] = 1;
        FSMC[30, 29] = 1;
        FSMC[31, 29] = 1;
        FSMC[32, 31] = 1;
        FSMC[33, 24] = 1;
        FSMC[34, 32] = 1;
        FSMC[34, 33] = 1;
        FSMC[35, 32] = 1;
        FSMC[35, 33] = 1;
        FSMC[37, 36] = 1;
        FSMC[37, 40] = 1;

        rutasDefinidas = new int[3];
        rutasDefinidas[0] = 0;
        rutasDefinidas[1] = 33;
        rutasDefinidas[2] = 37;

        targetWaypoint = intersecciones[ruta];

        angle = 0;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Geometry
        points = mesh.vertices;

        posicionInicio = transform.position;
        //intersecciones = new Vector3[4];
        //intersecciones[0] = new Vector3(9.693f, 0.0f, -15.457f);
        //intersecciones[1] = new Vector3(-0.03f, 0.0f, -15.573f);
        //intersecciones[2] = new Vector3(-0.93f, 0.0f, -0.57f);
        //intersecciones[3] = new Vector3(9.06f, 0.0f, -0.71f);
        lastWaypointIndex = intersecciones.Count - 1; // cuenta desde 0 (por eso el -1)
        //targetWaypoint = intersecciones[targetWaypointIndex];
        distanciaFaltante = targetWaypoint.position - posicionInicio;
    }

    // Update is called once per frame
    void Update()
    {
        float movementStep = speed * Time.deltaTime; // mueve con rapidez constante

        float currentDistance = Vector3.Distance(transform.position, targetWaypoint.position);
        CheckDistance2Waypont(currentDistance);

        if (t >= 1)
        {
            posicionInicio = transform.position;
            distanciaFaltante = targetWaypoint.position - posicionInicio;
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
        t += 0.005f;
    }

    void CheckDistance2Waypont(float currentDistance)
    {

        if (currentDistance <= minDistance)
        {
            int[] numbers = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41 }; // Esta son las opciones de los nodos posibles
            int[] shuffled = numbers.OrderBy(n => Guid.NewGuid()).ToArray(); // Presento las opciones de manera aleatoria

            for (int i = 0; i < 42; i++) // itero las opciones del agente
            {
                if (FSMC[targetWaypointIndex, shuffled[i]] == 1)
                { // En el momento que una opcion sea valida, la tomo
                    targetWaypointIndex = shuffled[i]; // Asignamiento
                    break; // Rompo el proceso dado que ya acepte una
                }
            }

            UpdateTargetWaypoint();
        }
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
        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Z);
        //Matrix4x4 A = transform1 * transform2;
        for (i = 0; i < n; i++)
        {
            vs[i] = transform2 * vs[i];
            final[i] = vs[i];
        }

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
