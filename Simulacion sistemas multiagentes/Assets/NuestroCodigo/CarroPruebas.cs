using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarroPruebas : MonoBehaviour
{   
    private CiudadPruebas miCiudad;
    private int waypointDestino;
    private Vector3 posicionInicio;
    private Vector3 posicionDestino;
    public Vector3 posicionActual;
    private Vector3 distanciaFaltante;
    private float velocidad = 0.06f;
    private float tamaPaso;
    private float t;
    private int tipoAvance;
    private int angle;
    private Vector3[] points;
    public int numeroSerie;
    public Vector3 vectorDireccionUnitario;
    public bool parado = false;
    public int waypointActual;

    // Start is called before the first frame update
    void Start()
    {
        miCiudad = FindObjectOfType<CiudadPruebas>();
        waypointDestino = 38;
        posicionInicio = transform.position;
        posicionActual = posicionInicio;
        posicionDestino = miCiudad.waypoints[waypointDestino];
        waypointActual = 36;
        distanciaFaltante = posicionDestino - posicionInicio;
        t = 0;
        CalcularTamaPaso();
        tipoAvance = 0;
        angle = 0;
        vectorDireccionUnitario = distanciaFaltante * tamaPaso;
    }

    void CalcularTamaPaso(){
        float x = (distanciaFaltante.x);
        float z = (distanciaFaltante.z);
        float distanciaDestino = (float) Math.Sqrt((x*x)+(z*z));
        float tiempo = distanciaDestino / velocidad;

        if (tipoAvance == 0){
            tamaPaso = 1 / tiempo;
            vectorDireccionUnitario = distanciaFaltante * tamaPaso;
        }
        else{
            vectorDireccionUnitario = distanciaFaltante * 0.0111f;
        }

    }

    void Avanzar(){
        
        if (parado){
            if(miCiudad.estados[waypointActual] == 0){
                parado = false;
            }
        }

        if (!parado){
            bool bandera = true;
            for(int i = 0; i < miCiudad.nCarros; i++){
                if (miCiudad.matrizEuclidiana[numeroSerie, i] == false){
                    bandera = false;
                    break;
                }
            }

            if (bandera){
                if (tipoAvance == 0){
                    if (t <= 1){
                        transform.position = posicionInicio + (distanciaFaltante * t);
                        posicionActual = transform.position;
                        t += tamaPaso;
                    }
                    else{
                        t = 0;
                        CalcularPosiciones();
                        if (miCiudad.estados[waypointActual] != 0){
                            parado = true;
                        }
                        CalcularTamaPaso();
                    }
                }
                else{
                    if (angle <= 90){
                        Girar();
                        posicionActual = posicionInicio + (vectorDireccionUnitario * angle);
                        angle += 1;
                    }
                    else{
                        angle = 0;

                        ReacomodarVertices(tipoAvance);
                        CalcularPosiciones();
                        if (miCiudad.estados[waypointActual] != 0){
                            parado = true;
                        }
                        CalcularTamaPaso();
                    }
                }
            }
        }

        
    }

    void ReacomodarVertices(int tipoGiro){

        GetComponent<MeshFilter>().mesh.vertices = points;

    }

    void CalcularPosiciones(){
        int numeroAleatorio = UnityEngine.Random.Range(0, miCiudad.nPosibilidades[waypointDestino]);
        tipoAvance = miCiudad.tipoRecorrido[waypointDestino, numeroAleatorio];
        waypointActual = waypointDestino;
        waypointDestino = miCiudad.posibilidades[waypointDestino, numeroAleatorio];
        posicionInicio = posicionDestino;
        transform.position = posicionDestino;
        posicionActual = posicionDestino;
        posicionDestino = miCiudad.waypoints[waypointDestino];
        distanciaFaltante = posicionDestino - posicionInicio;
        if (tipoAvance != 0){
            angle = 0;
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            points = mesh.vertices;
        }
    }

    void Girar(){
        int n = points.Length;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];

        for(int i = 0; i < n; i++)
        {
            vs[i] = points[i];
            vs[i].w = 1.0f;
        }

        Matrix4x4 A = miCiudad.matricesGiro[tipoAvance, angle];

        for (int i = 0; i < n; i++)
        {
            vs[i] = A * vs[i];
            final[i] = vs[i];
        }

        GetComponent<MeshFilter>().mesh.vertices = final;
    }

    // Update is called once per frame
    void Update()
    {
        Avanzar();
    }
}
