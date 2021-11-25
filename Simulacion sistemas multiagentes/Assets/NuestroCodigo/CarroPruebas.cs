using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
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
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    private int nCubos = 0;
    private int[] indicesAux = new int[8];
    private float anchoLlantas = 0.15f;
    private float anchoCarro = 0.5f;
    private float largoCarro = 1.2f;
    private float radioLlantas = 0.3f;
    private float espacioLlantas = 0.05f;
    private int cubos = 5;
    public int n;

    // Start is called before the first frame update
    void Start()
    {   
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        n = vertices.Length;
        UpdateMesh();

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

    void CreateShape()
    {
        vertices = new Vector3[cubos * 8];
        triangles = new int[cubos * 36];
        
        CreateCubo(-(anchoCarro/2) - anchoLlantas, 0, -(largoCarro/2) + espacioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo(-(anchoCarro/2) - anchoLlantas, 0, (largoCarro/2) - espacioLlantas - radioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo((anchoCarro/2), 0, -(largoCarro/2) + espacioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo((anchoCarro/2), 0, (largoCarro/2) - espacioLlantas - radioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo(-(anchoCarro/2), 0.1f, -(largoCarro/2), anchoCarro, largoCarro, anchoCarro);
        

    }

    void CreateCubo(float origenX, float origenY, float origenZ, float ancho, float largo, float alto)
    {
        int aux = nCubos * 8;

        for(int i = 0; i < 8; i++){
            indicesAux[i] = aux + i;
        }

        vertices[indicesAux[0]] = new Vector3(origenX, origenY, origenZ);
        vertices[indicesAux[1]] = new Vector3(origenX, origenY + alto, origenZ);
        vertices[indicesAux[2]] = new Vector3(origenX + ancho, origenY + alto, origenZ);
        vertices[indicesAux[3]] = new Vector3(origenX + ancho, origenY, origenZ);
        vertices[indicesAux[4]] = new Vector3(origenX, origenY, origenZ + largo);
        vertices[indicesAux[5]] = new Vector3(origenX, origenY + alto, origenZ + largo);
        vertices[indicesAux[6]] = new Vector3(origenX + ancho, origenY + alto, origenZ + largo);
        vertices[indicesAux[7]] = new Vector3(origenX + ancho, origenY, origenZ + largo);

        aux = nCubos * 36;
        triangles[aux] = indicesAux[0];
        triangles[aux + 1] = indicesAux[1];
        triangles[aux + 2] = indicesAux[2];
        triangles[aux + 3] = indicesAux[2];
        triangles[aux + 4] = indicesAux[3];
        triangles[aux + 5] = indicesAux[0];
        triangles[aux + 6] = indicesAux[4];
        triangles[aux + 7] = indicesAux[5];
        triangles[aux + 8] = indicesAux[1];
        triangles[aux + 9] = indicesAux[1];
        triangles[aux + 10] = indicesAux[0];
        triangles[aux + 11] = indicesAux[4];
        triangles[aux + 12] = indicesAux[1];
        triangles[aux + 13] = indicesAux[5];
        triangles[aux + 14] = indicesAux[6];
        triangles[aux + 15] = indicesAux[6];
        triangles[aux + 16] = indicesAux[2];
        triangles[aux + 17] = indicesAux[1];
        triangles[aux + 18] = indicesAux[2];
        triangles[aux + 19] = indicesAux[6];
        triangles[aux + 20] = indicesAux[7];
        triangles[aux + 21] = indicesAux[7];
        triangles[aux + 22] = indicesAux[3];
        triangles[aux + 23] = indicesAux[2];
        triangles[aux + 24] = indicesAux[7];
        triangles[aux + 25] = indicesAux[6];
        triangles[aux + 26] = indicesAux[5];
        triangles[aux + 27] = indicesAux[5];
        triangles[aux + 28] = indicesAux[4];
        triangles[aux + 29] = indicesAux[7];
        triangles[aux + 30] = indicesAux[0];
        triangles[aux + 31] = indicesAux[3];
        triangles[aux + 32] = indicesAux[4];
        triangles[aux + 33] = indicesAux[4];
        triangles[aux + 34] = indicesAux[3];
        triangles[aux + 35] = indicesAux[7];

        nCubos++;

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
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
                        GirarMejorado();
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

        if (tipoGiro == 1){
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 2){
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 3){
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 4){
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 5){
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 6){
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 7){
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        else{
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }

        points = GetComponent<MeshFilter>().mesh.vertices;

    }

    /*
    void CalcularPosiciones(){
        int numeroAleatorio = UnityEngine.Random.Range(0, miCiudad.nPosibilidades[waypointDestino]);
        tipoAvance = miCiudad.tipoRecorrido[waypointDestino, nuevoNumeroAleatorio];
        waypointActual = waypointDestino;
        waypointDestino = miCiudad.posibilidades[waypointDestino, nuevoNumeroAleatorio];
        posicionInicio = posicionDestino;
        transform.position = posicionDestino;
        posicionActual = posicionDestino;
        posicionDestino = miCiudad.waypoints[waypointDestino];
        distanciaFaltante = posicionDestino - posicionInicio;
        if (tipoAvance != 0){
            angle = 0;
            mesh = GetComponent<MeshFilter>().mesh;
            points = mesh.vertices;
        }
        /*float numeroAleatorio = UnityEngine.Random.Range(0.0f, 1.0f);
        float probabilidad = 0f, probabilidadAcumulada;
        for(int i = 0; i < 48; i++) {
            probabilidad = miCiudad.probabilidadPosibilidades[waypointActual, i];
            if (probabilidad > 0){
                probabilidadAcumulada += probabilidad;
                if (probabilidadAcumulada >= numeroAleatorio){
                    int nuevoNumeroAleatorio = probabilidadAcumulada;
                    
                }
            }   
        }*/
     /*   
    }
    */

    void CalcularPosiciones(){
        
        int numeroAleatorio = CalcularAleatorio(waypointDestino);
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
            mesh = GetComponent<MeshFilter>().mesh;
            points = mesh.vertices;
        }
        
    }

    int CalcularAleatorio(int waypoint){
        int decision = 0;
        int cantidadPosible = miCiudad.nPosibilidades[waypoint];

        int aleatorio = UnityEngine.Random.Range(0, 100);
        int acumulado = 0;
        for (int i = 0; i < cantidadPosible; i++){
            acumulado += miCiudad.porcentajes[waypoint, i];
            if (aleatorio <= acumulado){
                decision = i;
                break;
            }
        }
        return decision;
    }

    void GirarMejorado(){

        int indiceM = miCiudad.indicesMatrizGiro[waypointActual];
        Vector3[] final = new Vector3[n];

        for (int i = 0; i < n; i++){
            final[i] = miCiudad.matricesDeGiros[indiceM, angle, i];
        }

        GetComponent<MeshFilter>().mesh.vertices = final;

    }

    // Update is called once per frame
    void Update()
    {
        Avanzar();
        /*
        if (parado)
        {
            Debug.Log("CochePruebaSiii>>:" + numeroSerie + "," + waypointActual + "," + 0);
        }
        else
            Debug.Log("CochePruebaSiii>>:" + numeroSerie + "," + waypointActual + "," + velocidad);
        */
    }
}
